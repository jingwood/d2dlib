/*
* MIT License
*
* Copyright (c) 2009-2021 Jingwood, unvell.com. All right reserved.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

// D2DLib.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "Context.h"

#include <stack>
using namespace std;

static thread_local HRESULT s_lastCreateError = S_OK;

HRESULT GetLastCreateError()
{
	return s_lastCreateError;
}

IDWriteFactory* GetSharedWriteFactory()
{
	// The shared factory is thread-safe and lives for the process, so it is created once and never
	// released.
	static IDWriteFactory* factory = []() -> IDWriteFactory* {
		IDWriteFactory* f = NULL;
		DWriteCreateFactory(DWRITE_FACTORY_TYPE_SHARED, __uuidof(IDWriteFactory), reinterpret_cast<IUnknown**>(&f));
		return f;
	}();

	return factory;
}

static HRESULT CreateImagingFactory(IWICImagingFactory** imageFactory)
{
	HRESULT hr = CoCreateInstance(CLSID_WICImagingFactory, NULL, CLSCTX_INPROC_SERVER,
		IID_IWICImagingFactory, (LPVOID*)imageFactory);

	if (hr == CO_E_NOTINITIALIZED)
	{
		// A thread that never touched COM (a test runner, a render worker): join the MTA.
		CoInitializeEx(NULL, COINIT_MULTITHREADED);
		hr = CoCreateInstance(CLSID_WICImagingFactory, NULL, CLSCTX_INPROC_SERVER,
			IID_IWICImagingFactory, (LPVOID*)imageFactory);
	}

	return hr;
}

// The device-independent factories every context holds.
static HRESULT CreateFactories(D2DContext* context)
{
	HRESULT hr = D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &context->factory);

	if (SUCCEEDED(hr))
	{
		hr = DWriteCreateFactory(DWRITE_FACTORY_TYPE_SHARED,
			__uuidof(context->writeFactory), reinterpret_cast<IUnknown**>(&context->writeFactory));
	}

	if (SUCCEEDED(hr))
	{
		hr = CreateImagingFactory(&context->imageFactory);
	}

	return hr;
}

// D3D11 device with BGRA support (required for D2D interop) -> DXGI device -> D2D device.
static HRESULT CreateDevices(D2DContext* context, bool forceWarp)
{
	D3D_FEATURE_LEVEL featureLevels[] = {
		D3D_FEATURE_LEVEL_11_1, D3D_FEATURE_LEVEL_11_0,
		D3D_FEATURE_LEVEL_10_1, D3D_FEATURE_LEVEL_10_0,
		D3D_FEATURE_LEVEL_9_3,  D3D_FEATURE_LEVEL_9_1
	};

	HRESULT hr = E_FAIL;

	if (!forceWarp)
	{
		hr = D3D11CreateDevice(nullptr, D3D_DRIVER_TYPE_HARDWARE, 0,
			D3D11_CREATE_DEVICE_BGRA_SUPPORT,
			featureLevels, ARRAYSIZE(featureLevels),
			D3D11_SDK_VERSION, &context->d3dDevice, nullptr, nullptr);
	}

	if (FAILED(hr))
	{
		// Fallback to WARP (software) driver
		hr = D3D11CreateDevice(nullptr, D3D_DRIVER_TYPE_WARP, 0,
			D3D11_CREATE_DEVICE_BGRA_SUPPORT,
			featureLevels, ARRAYSIZE(featureLevels),
			D3D11_SDK_VERSION, &context->d3dDevice, nullptr, nullptr);
	}

	if (SUCCEEDED(hr))
	{
		hr = context->d3dDevice->QueryInterface(&context->dxgiDevice);
	}

	if (SUCCEEDED(hr))
	{
		hr = CreateFactories(context);
	}

	if (SUCCEEDED(hr))
	{
		hr = context->factory->CreateDevice(context->dxgiDevice, &context->d2dDevice);
	}

	return hr;
}

static HRESULT CreateSwapChain(D2DContext* context)
{
	IDXGIAdapter* dxgiAdapter = NULL;
	IDXGIFactory2* dxgiFactory = NULL;

	HRESULT hr = context->dxgiDevice->GetAdapter(&dxgiAdapter);

	if (SUCCEEDED(hr))
	{
		hr = dxgiAdapter->GetParent(IID_PPV_ARGS(&dxgiFactory));
	}

	if (SUCCEEDED(hr))
	{
		DXGI_SWAP_CHAIN_DESC1 swapChainDesc = {};
		swapChainDesc.Width = 0;  // auto-size from HWND
		swapChainDesc.Height = 0;
		swapChainDesc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
		swapChainDesc.Stereo = FALSE;
		swapChainDesc.SampleDesc.Count = 1;
		swapChainDesc.SampleDesc.Quality = 0;
		swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
		swapChainDesc.Scaling = DXGI_SCALING_STRETCH;
		swapChainDesc.AlphaMode = DXGI_ALPHA_MODE_UNSPECIFIED;

		if (context->flags & D2DLIB_CONTEXT_GDI_COMPATIBLE)
		{
			swapChainDesc.BufferCount = 1;
			swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;
		}
		else
		{
			swapChainDesc.BufferCount = 2;
			swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL;
		}

		hr = dxgiFactory->CreateSwapChainForHwnd(
			context->d3dDevice, context->hwnd, &swapChainDesc, nullptr, nullptr,
			&context->swapChain);
	}

	if (SUCCEEDED(hr))
	{
		// DXGI would otherwise watch the window for Alt+Enter and switch the swap chain to full
		// screen - a keystroke a control usually wants for itself.
		dxgiFactory->MakeWindowAssociation(context->hwnd, DXGI_MWA_NO_WINDOW_CHANGES | DXGI_MWA_NO_ALT_ENTER);
	}

	SafeRelease(&dxgiFactory);
	SafeRelease(&dxgiAdapter);

	return hr;
}

// D2D bitmap target over the swap chain's back buffer.
static HRESULT CreateTargetBitmap(D2DContext* context)
{
	IDXGISurface* dxgiBackBuffer = NULL;
	HRESULT hr = context->swapChain->GetBuffer(0, IID_PPV_ARGS(&dxgiBackBuffer));

	if (SUCCEEDED(hr))
	{
		D2D1_ALPHA_MODE alphaMode = (context->flags & D2DLIB_CONTEXT_OPAQUE)
			? D2D1_ALPHA_MODE_IGNORE : D2D1_ALPHA_MODE_PREMULTIPLIED;

		D2D1_BITMAP_PROPERTIES1 bitmapProperties = D2D1::BitmapProperties1(
			D2D1_BITMAP_OPTIONS_TARGET | D2D1_BITMAP_OPTIONS_CANNOT_DRAW,
			D2D1::PixelFormat(DXGI_FORMAT_B8G8R8A8_UNORM, alphaMode));

		hr = context->deviceContext->CreateBitmapFromDxgiSurface(
			dxgiBackBuffer, &bitmapProperties, &context->targetBitmap);
	}

	SafeRelease(&dxgiBackBuffer);

	if (SUCCEEDED(hr))
	{
		context->deviceContext->SetTarget(context->targetBitmap);
	}

	return hr;
}

// Release everything an HWND or WIC context owns (not for BitmapRenderTarget contexts, which
// borrow their factories from the parent context).
static void ReleaseContextResources(D2DContext* context)
{
	delete context->matrixStack;
	context->matrixStack = NULL;

	SafeRelease(&context->solidBrush);

	// renderTarget points to deviceContext for HWND contexts - do not double-release.
	// A WIC context owns its render target separately.
	if (context->isWicRenderTarget)
	{
		SafeRelease(&context->renderTarget);
	}
	context->renderTarget = NULL;

	SafeRelease(&context->targetBitmap);
	SafeRelease(&context->deviceContext);
	SafeRelease(&context->swapChain);
	SafeRelease(&context->wicBitmap);
	SafeRelease(&context->d2dDevice);
	SafeRelease(&context->dxgiDevice);
	SafeRelease(&context->d3dDevice);
	SafeRelease(&context->imageFactory);
	SafeRelease(&context->writeFactory);
	SafeRelease(&context->factory);
}

static HANDLE FailCreate(D2DContext* context, HRESULT hr)
{
	s_lastCreateError = hr;
	ReleaseContextResources(context);
	delete context;
	return NULL;
}

HANDLE CreateContext(HWND hwnd, D2D1_PRESENT_OPTIONS presentOptions)
{
	return CreateContextEx(hwnd,
		(presentOptions & D2D1_PRESENT_OPTIONS_IMMEDIATELY) ? 0 : D2DLIB_CONTEXT_VSYNC, NULL);
}

HANDLE CreateContextEx(HWND hwnd, UINT flags, HANDLE shareDevicesWith)
{
	D2DContext* context = new D2DContext();
	ZeroMemory(context, sizeof(D2DContext));
	HRESULT hr;

	context->hwnd = hwnd;
	context->flags = flags;
	context->isBitmapRenderTarget = false;
	context->useVSync = (flags & D2DLIB_CONTEXT_VSYNC) != 0;
	context->matrixStack = new std::stack<D2D1_MATRIX_3X2_F>();

	D2DContext* share = reinterpret_cast<D2DContext*>(shareDevicesWith);

	if (share != NULL && share->d2dDevice != NULL)
	{
		context->factory = share->factory;
		context->writeFactory = share->writeFactory;
		context->imageFactory = share->imageFactory;
		context->d3dDevice = share->d3dDevice;
		context->dxgiDevice = share->dxgiDevice;
		context->d2dDevice = share->d2dDevice;

		context->factory->AddRef();
		context->writeFactory->AddRef();
		context->imageFactory->AddRef();
		context->d3dDevice->AddRef();
		context->dxgiDevice->AddRef();
		context->d2dDevice->AddRef();

		hr = S_OK;
	}
	else
	{
		hr = CreateDevices(context, (flags & D2DLIB_CONTEXT_FORCE_WARP) != 0);
	}

	if (SUCCEEDED(hr))
	{
		hr = context->d2dDevice->CreateDeviceContext(
			D2D1_DEVICE_CONTEXT_OPTIONS_NONE, &context->deviceContext);
	}

	if (SUCCEEDED(hr))
	{
		hr = CreateSwapChain(context);
	}

	if (SUCCEEDED(hr))
	{
		hr = CreateTargetBitmap(context);
	}

	if (FAILED(hr))
	{
		return FailCreate(context, hr);
	}

	// Backward-compatible renderTarget pointer
	context->renderTarget = context->deviceContext;

	context->renderTarget->SetAntialiasMode(D2D1_ANTIALIAS_MODE::D2D1_ANTIALIAS_MODE_PER_PRIMITIVE);

	return (HANDLE)context;
}

HANDLE CreateWicBitmapContext(UINT width, UINT height, FLOAT dpiX, FLOAT dpiY, UINT flags)
{
	D2DContext* context = new D2DContext();
	ZeroMemory(context, sizeof(D2DContext));

	context->flags = flags;
	context->isWicRenderTarget = true;
	context->matrixStack = new std::stack<D2D1_MATRIX_3X2_F>();

	HRESULT hr = CreateFactories(context);

	if (SUCCEEDED(hr))
	{
		hr = context->imageFactory->CreateBitmap(width, height, GUID_WICPixelFormat32bppPBGRA,
			WICBitmapCacheOnLoad, &context->wicBitmap);
	}

	if (SUCCEEDED(hr))
	{
		D2D1_RENDER_TARGET_PROPERTIES props = D2D1::RenderTargetProperties(
			D2D1_RENDER_TARGET_TYPE_DEFAULT,
			D2D1::PixelFormat(DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE_PREMULTIPLIED),
			dpiX, dpiY);

		hr = context->factory->CreateWicBitmapRenderTarget(context->wicBitmap, &props, &context->renderTarget);
	}

	if (FAILED(hr))
	{
		return FailCreate(context, hr);
	}

	// A WIC render target from a D2D 1.1 factory is also a device context, which the effects and
	// color-font text need. Optional: everything else draws through renderTarget.
	context->renderTarget->QueryInterface(&context->deviceContext);

	context->renderTarget->SetAntialiasMode(D2D1_ANTIALIAS_MODE::D2D1_ANTIALIAS_MODE_PER_PRIMITIVE);

	return (HANDLE)context;
}

HRESULT CopyContextPixels(HANDLE ctx, BYTE* buffer, UINT stride, UINT bufferSize)
{
	RetrieveContext(ctx);

	if (context->wicBitmap == NULL) return E_NOTIMPL;

	return context->wicBitmap->CopyPixels(NULL, stride, bufferSize, buffer);
}

void DestroyContext(HANDLE handle)
{
	if (handle == NULL) return;

	D2DContext* context = reinterpret_cast<D2DContext*>(handle);

	ReleaseContextResources(context);

	delete context;
}

void ResizeContext(HANDLE handle)
{
	if (handle == 0 || handle == INVALID_HANDLE_VALUE) return;

	ResizeContextEx(handle);
}

HRESULT ResizeContextEx(HANDLE handle)
{
	if (handle == 0 || handle == INVALID_HANDLE_VALUE) return E_INVALIDARG;

	D2DContext* context = reinterpret_cast<D2DContext*>(handle);

	if (context->swapChain == NULL || context->hwnd == INVALID_HANDLE_VALUE) return S_OK;

	// Must release the target bitmap before resizing the swap chain
	context->deviceContext->SetTarget(nullptr);
	SafeRelease(&context->targetBitmap);

	HRESULT hr = context->swapChain->ResizeBuffers(0, 0, 0,
		DXGI_FORMAT_UNKNOWN, 0);

	if (SUCCEEDED(hr))
	{
		// Recreate the bitmap target from the new back buffer
		hr = CreateTargetBitmap(context);
	}

	return hr;
}

void SetContextProperties(HANDLE ctx, D2D1_ANTIALIAS_MODE antialiasMode)
{
	RetrieveContext(ctx);

	context->renderTarget->SetAntialiasMode(antialiasMode);
}
void SetTextAntialiasMode(HANDLE ctx, D2D1_TEXT_ANTIALIAS_MODE antialiasMode)
{
	RetrieveContext(ctx);
	context->renderTarget->SetTextAntialiasMode(antialiasMode);
}

void BeginRender(HANDLE ctx)
{
	RetrieveContext(ctx);

	context->renderTarget->BeginDraw();
}

void BeginRenderWithBackgroundColor(HANDLE ctx, D2D1_COLOR_F backColor)
{
	RetrieveContext(ctx);

	context->renderTarget->BeginDraw();

	context->renderTarget->Clear(backColor);
}

void BeginRenderWithBackgroundBitmap(HANDLE ctx, HANDLE bitmap)
{
	RetrieveContext(ctx);
	ID2D1Bitmap* d2dbitmap = reinterpret_cast<ID2D1Bitmap*>(bitmap);

	context->renderTarget->BeginDraw();

	D2D1_SIZE_F size = context->renderTarget->GetSize();
	D2D1_RECT_F destRect = {0, 0, size.width, size.height};

	context->renderTarget->DrawBitmap(d2dbitmap, &destRect);
}

void Clear(HANDLE ctx, D2D1_COLOR_F color)
{
	RetrieveContext(ctx);

	context->renderTarget->Clear(color);
}

void EndRender(HANDLE ctx)
{
	RetrieveContext(ctx);
	context->renderTarget->EndDraw();

	// Present swap chain for HWND contexts
	if (context->swapChain != NULL)
	{
		DXGI_PRESENT_PARAMETERS params = {};
		UINT syncInterval = context->useVSync ? 1 : 0;
		context->swapChain->Present1(syncInterval, 0, &params);
	}
}

HRESULT EndRenderEx(HANDLE ctx)
{
	RetrieveContext(ctx);
	HRESULT hr = context->renderTarget->EndDraw();

	if (SUCCEEDED(hr) && context->swapChain != NULL)
	{
		DXGI_PRESENT_PARAMETERS params = {};
		UINT syncInterval = context->useVSync ? 1 : 0;
		hr = context->swapChain->Present1(syncInterval, 0, &params);

		// Nothing of the window is visible (minimized, covered) - not an error.
		if (hr == DXGI_STATUS_OCCLUDED) hr = S_OK;
	}

	return hr;
}

void Flush(HANDLE ctx)
{
	RetrieveContext(ctx);
	context->renderTarget->Flush();
}
//D2DLIB_API HANDLE DeleteStorkeStyle(const HANDLE handle)
//{
//
//	D2DContext* context = reinterpret_cast<D2DContext*>(handle);
//
//	ID2D1StrokeStyle* strokeStyle;
//
//	context->factory->CreateStrokeStyle(D2D1::StrokeStyleProperties(
//            D2D1_CAP_STYLE_FLAT,
//            D2D1_CAP_STYLE_FLAT,
//            D2D1_CAP_STYLE_ROUND,
//            D2D1_LINE_JOIN_MITER,
//            10.0f,
//            D2D1_DASH_STYLE_CUSTOM,
//            0.0f), dashes,
//        ARRAYSIZE(dashes), &strokeStyle);
//
//	return (HANDLE)strokeStyle;
//}


HANDLE CreateBitmapRenderTarget(HANDLE ctx, D2D1_SIZE_F size)
{
	RetrieveContext(ctx);

	D2DContext* bitmapContext = new D2DContext();
	ZeroMemory(bitmapContext, sizeof(D2DContext));

	bitmapContext->factory = context->factory;
	bitmapContext->imageFactory = context->imageFactory;
	bitmapContext->writeFactory = context->writeFactory;
	bitmapContext->isBitmapRenderTarget = true;
	bitmapContext->matrixStack = new std::stack<D2D1_MATRIX_3X2_F>();

	HRESULT hr;

	if (size.width <= 0 && size.height <= 0)
	{
		hr = context->renderTarget->CreateCompatibleRenderTarget(
			&bitmapContext->bitmapRenderTarget);
	}
	else
	{
		hr = context->renderTarget->CreateCompatibleRenderTarget(
			size, &bitmapContext->bitmapRenderTarget);
	}

	if (!SUCCEEDED(hr)) {
		context->lastErrorCode = hr;
		delete bitmapContext->matrixStack;
		delete bitmapContext;
		return NULL;
	}

	// Point renderTarget at the bitmapRenderTarget for drawing calls
	bitmapContext->renderTarget = bitmapContext->bitmapRenderTarget;

	return (HANDLE)bitmapContext;
}

void DrawBitmapRenderTarget(HANDLE ctx, HANDLE bitmapRenderTargetHandle, D2D1_RECT_F* rect,
														FLOAT opacity, D2D1_BITMAP_INTERPOLATION_MODE interpolationMode)
{
	RetrieveContext(ctx);
	D2DContext* bitmapRenderTargetContext = reinterpret_cast<D2DContext*>(bitmapRenderTargetHandle);

	if (bitmapRenderTargetContext->bitmap == NULL)
	{
		HRESULT hr = bitmapRenderTargetContext->bitmapRenderTarget->GetBitmap(&bitmapRenderTargetContext->bitmap);
	}

	if (bitmapRenderTargetContext->bitmap != NULL) {
		context->renderTarget->DrawBitmap(bitmapRenderTargetContext->bitmap, rect, opacity, interpolationMode);
	}
}

HANDLE GetBitmapRenderTargetBitmap(HANDLE bitmapRenderTargetHandle)
{
	RetrieveContext(bitmapRenderTargetHandle);

	ID2D1Bitmap* bitmap = NULL;
	context->bitmapRenderTarget->GetBitmap(&bitmap);

	return bitmap;
}

void DestroyBitmapRenderTarget(HANDLE ctx)
{
	if (ctx == NULL) return;

	RetrieveContext(ctx);

	// renderTarget points to bitmapRenderTarget - do not double-release
	context->renderTarget = NULL;

	delete context->matrixStack;

	SafeRelease(&context->solidBrush);

	if (context->bitmap != NULL)
	{
		SafeRelease(&context->bitmap);
	}

	if (context->bitmapRenderTarget != NULL)
	{
		SafeRelease(&context->bitmapRenderTarget);
	}

	delete context;
}

void GetDPI(HANDLE ctx, FLOAT* dpiX, FLOAT* dpiY) {
	RetrieveContext(ctx);
	context->renderTarget->GetDpi(dpiX, dpiY);
}

void SetDPI(HANDLE ctx, FLOAT dpiX, FLOAT dpiY) {
	RetrieveContext(ctx);
	context->renderTarget->SetDpi(dpiX, dpiY);
}

void PushClip(HANDLE ctx, D2D1_RECT_F* rect, D2D1_ANTIALIAS_MODE antiAliasMode)
{
	RetrieveContext(ctx);
	context->renderTarget->PushAxisAlignedClip(rect, antiAliasMode);
}

void PopClip(HANDLE ctx)
{
	RetrieveContext(ctx);
	context->renderTarget->PopAxisAlignedClip();
}

HANDLE CreateLayer(HANDLE ctx)
{
	RetrieveContext(ctx);

	ID2D1Layer* layer;
	context->renderTarget->CreateLayer(&layer);

	return (HANDLE)layer;
}

void PushLayer(HANDLE ctx, HANDLE layerHandle, D2D1_RECT_F contentBounds, __in_opt HANDLE geometryHandle,
		__in_opt HANDLE opacityBrush, D2D1_LAYER_OPTIONS layerOptions)
{
	RetrieveContext(ctx);

	ID2D1Geometry* geometry = NULL;
	if (geometryHandle != NULL) {
		D2DGeometryContext* geometryContext = reinterpret_cast<D2DGeometryContext*>(geometryHandle);
		geometry = geometryContext->geometry;
	}

	ID2D1Brush* brush = NULL;
	if (opacityBrush != NULL) {
		D2DBrushContext* brushContext = reinterpret_cast<D2DBrushContext*>(opacityBrush);
		brush = brushContext->brush;
	}

	D2D1_LAYER_PARAMETERS params = D2D1::LayerParameters(contentBounds, geometry,
		D2D1_ANTIALIAS_MODE_PER_PRIMITIVE, D2D1::IdentityMatrix(), 1, brush, layerOptions);

	ID2D1Layer* layer = reinterpret_cast<ID2D1Layer*>(layerHandle);
	context->renderTarget->PushLayer(&params, layer);
}

void PopLayer(HANDLE ctx)
{
	RetrieveContext(ctx);
	context->renderTarget->PopLayer();
}

HRESULT GetLastErrorCode(HANDLE ctx)
{
	RetrieveContext(ctx);
	return context->lastErrorCode;
}

void ReleaseObject(HANDLE handle)
{
	IUnknown* object = reinterpret_cast<IUnknown*>(handle);
	if (object != NULL)
		SafeRelease(&object);
}
