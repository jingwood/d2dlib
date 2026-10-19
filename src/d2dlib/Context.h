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

#pragma once

#include <Windows.h>
#include <d2d1_2.h>
#include <d2d1_2helper.h>
#include <d2d1effects.h>
#include <d3d11.h>
#include <dxgi1_2.h>
#include <dwrite.h>
#include "Wincodec.h"

#include <stack>

using namespace std;

#ifndef __D2DLIB_H__
#define __D2DLIB_H__

typedef struct D2DContext
{
	ID2D1Factory1* factory;
	IDWriteFactory* writeFactory;
	IWICImagingFactory* imageFactory;

	// D3D11/DXGI device chain (HWND contexts only)
	ID3D11Device* d3dDevice;
	IDXGIDevice1* dxgiDevice;
	ID2D1Device* d2dDevice;
	IDXGISwapChain1* swapChain;
	ID2D1Bitmap1* targetBitmap;

	// Primary drawing surface - all existing drawing code uses this pointer.
	// For HWND contexts: points to deviceContext (ID2D1DeviceContext inherits ID2D1RenderTarget)
	// For BitmapRenderTarget contexts: points to bitmapRenderTarget
	ID2D1RenderTarget* renderTarget;

	// D2D 1.1 device context for effects and new features (HWND contexts only)
	ID2D1DeviceContext* deviceContext;

	HWND hwnd;

	// Off-screen rendering
	ID2D1BitmapRenderTarget* bitmapRenderTarget;
	ID2D1Bitmap* bitmap;
	bool isBitmapRenderTarget;

	std::stack<D2D1_MATRIX_3X2_F>* matrixStack;

	bool useVSync;

	HRESULT lastErrorCode;

	// One brush recolored by every color-taking draw call (see GetSolidBrush), instead of a brush
	// created and released per primitive. Created on first use, owned by the context.
	ID2D1SolidColorBrush* solidBrush;

	// Off-screen rendering into a CPU-side WIC bitmap (CreateWicBitmapContext): renderTarget is a
	// WIC bitmap render target owned by this context, and the pixels can be read back.
	IWICBitmap* wicBitmap;
	bool isWicRenderTarget;

	// D2DLIB_CONTEXT_* flags the context was created with.
	UINT flags;

} D2DContext;

// Options for CreateContextEx / CreateWicBitmapContext.
enum D2DContextFlags {
	// Present synchronized to the vertical blank.
	D2DLIB_CONTEXT_VSYNC = 0x1,
	// Bit-block-transfer (blt) model swap chain instead of the flip model. A flip-model swap chain
	// takes over the window's presentation, so GDI drawing on it - child controls such as scroll
	// bars or text boxes - can be lost; a blt-model present is clipped like a GDI paint.
	D2DLIB_CONTEXT_GDI_COMPATIBLE = 0x2,
	// The target has no alpha channel (D2D1_ALPHA_MODE_IGNORE), which is what lets ClearType text
	// render on it. Without it the target is premultiplied, for transparent windows.
	D2DLIB_CONTEXT_OPAQUE = 0x4,
	// Use the WARP software rasterizer even when a hardware device is available.
	D2DLIB_CONTEXT_FORCE_WARP = 0x8,
};

// The context's cached solid color brush, set to color. NULL only if the brush cannot be created.
inline ID2D1SolidColorBrush* GetSolidBrush(D2DContext* context, const D2D1_COLOR_F& color)
{
	if (context->solidBrush == NULL)
	{
		if (FAILED(context->renderTarget->CreateSolidColorBrush(color, &context->solidBrush)))
		{
			context->solidBrush = NULL;
		}
	}
	else
	{
		context->solidBrush->SetColor(color);
	}

	return context->solidBrush;
}

// The process-wide shared DirectWrite factory, for device-independent text resources.
IDWriteFactory* GetSharedWriteFactory();

enum GeometryType {
	GeoType_RectangleGeometry,
	GeoType_PathGeometry,
};

typedef struct D2DGeometryContext {
	D2DContext* d2context;
	ID2D1Geometry* geometry;
} D2DGeometryContext;

enum BrushType {
	BrushType_SolidBrush,
	BrushType_LinearGradientBrush,
	BrushType_RadialGradientBrush,
	BrushType_BitmapBrush
};

struct D2DBrushContext {
	D2DContext* context;
	ID2D1Brush* brush;
	BrushType type;
	union {
		ID2D1GradientStopCollection* gradientStops = NULL;
		ID2D1Bitmap* bitmap;
	};
};

typedef struct D2DPathContext : D2DGeometryContext
{
	ID2D1PathGeometry* path;
	ID2D1GeometrySink* sink;
	bool isOpen;
	bool isClosed;
} D2DPathContext;

//typedef struct D2DBitmapRenderTargetContext
//{
//	ID2D1Factory *factory;
//	IDWriteFactory *writeFactory;
//	IWICImagingFactory* imageFactory;
//	HRESULT lastResult;
//
//} D2DBitmapRenderTargetContext;

// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the D2DLIB_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// D2DLIB_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef D2DLIB_EXPORTS
#define D2DLIB_API __declspec(dllexport)
#else
#define D2DLIB_API __declspec(dllimport)
#endif

template<class Interface>
inline void SafeRelease(Interface **ppInterfaceToRelease)
{
  if (*ppInterfaceToRelease != NULL)
  {
    (*ppInterfaceToRelease)->Release();
    (*ppInterfaceToRelease) = NULL;
  }
}

#define RetrieveContext(ctx) D2DContext* context = reinterpret_cast<D2DContext*>(ctx)
#define RetrieveD2DBitmap(ctx) ID2D1Bitmap* d2dbitmap = reinterpret_cast<ID2D1Bitmap*>(d2dbitmapHandle)
//#define RetrieveContext(ctx) D2DContext* context = (D2DContext*)(ctx)

//extern D2DLIB_API HRESULT LastResultCode;

extern "C" 
{
	D2DLIB_API HANDLE CreateContext(HWND hwnd, D2D1_PRESENT_OPTIONS presentOptions);

	// Create an HWND context with D2DLIB_CONTEXT_* flags. When shareDevicesWith is another HWND
	// context, the new one reuses its factory and D3D/D2D devices (so device resources such as
	// bitmaps are usable on both) and only adds its own device context and swap chain.
	// Returns NULL on failure; GetLastCreateError tells why.
	D2DLIB_API HANDLE CreateContextEx(HWND hwnd, UINT flags, HANDLE shareDevicesWith);

	// Create an off-screen context drawing into a CPU-side 32bpp premultiplied BGRA bitmap of
	// width x height pixels at the given DPI. Read the result with CopyContextPixels after EndRender.
	D2DLIB_API HANDLE CreateWicBitmapContext(UINT width, UINT height, FLOAT dpiX, FLOAT dpiY, UINT flags);
	D2DLIB_API HRESULT CopyContextPixels(HANDLE ctx, BYTE* buffer, UINT stride, UINT bufferSize);

	// The HRESULT of the last failed CreateContextEx / CreateWicBitmapContext on this thread.
	D2DLIB_API HRESULT GetLastCreateError();

	D2DLIB_API void DestroyContext(HANDLE context);
	D2DLIB_API void ResizeContext(HANDLE context);
	// ResizeContext that reports failure (e.g. a removed device).
	D2DLIB_API HRESULT ResizeContextEx(HANDLE context);
	
	D2DLIB_API void SetContextProperties(HANDLE ctx,
		D2D1_ANTIALIAS_MODE antialiasMode = D2D1_ANTIALIAS_MODE::D2D1_ANTIALIAS_MODE_PER_PRIMITIVE);

	D2DLIB_API void SetTextAntialiasMode(HANDLE ctx, D2D1_TEXT_ANTIALIAS_MODE antialiasMode);
	
	D2DLIB_API void BeginRender(HANDLE context);
	D2DLIB_API void BeginRenderWithBackgroundColor(HANDLE ctx, D2D1_COLOR_F color);
	D2DLIB_API void BeginRenderWithBackgroundBitmap(HANDLE ctx, HANDLE bitmap);
	D2DLIB_API void EndRender(HANDLE context);
	// EndRender that returns the result of EndDraw / Present. D2DERR_RECREATE_TARGET,
	// DXGI_ERROR_DEVICE_REMOVED and DXGI_ERROR_DEVICE_RESET mean the context must be recreated.
	D2DLIB_API HRESULT EndRenderEx(HANDLE context);
	D2DLIB_API void Flush(HANDLE context);
	D2DLIB_API void Clear(HANDLE context, D2D1_COLOR_F color);

	D2DLIB_API HANDLE CreateBitmapRenderTarget(HANDLE context, D2D1_SIZE_F size = D2D1::SizeF());
	D2DLIB_API void DrawBitmapRenderTarget(HANDLE context, HANDLE bitmapRenderTargetHandle, D2D1_RECT_F* rect = NULL,
		FLOAT opacity = 1, D2D1_BITMAP_INTERPOLATION_MODE interpolationMode = D2D1_BITMAP_INTERPOLATION_MODE_LINEAR);
	D2DLIB_API HANDLE GetBitmapRenderTargetBitmap(HANDLE bitmapRenderTargetHandle);
	D2DLIB_API void DestroyBitmapRenderTarget(HANDLE context);

	D2DLIB_API void GetDPI(HANDLE ctx, FLOAT* dpiX, FLOAT* dpiY);
	D2DLIB_API void SetDPI(HANDLE ctx, FLOAT dpiX, FLOAT dpiY);

	D2DLIB_API void PushClip(HANDLE context, D2D1_RECT_F* rect, 
												   D2D1_ANTIALIAS_MODE antiAliasMode = D2D1_ANTIALIAS_MODE::D2D1_ANTIALIAS_MODE_PER_PRIMITIVE);
	D2DLIB_API void PopClip(HANDLE context);

	D2DLIB_API HANDLE CreateLayer(HANDLE context);
	D2DLIB_API void PushLayer(HANDLE ctx, HANDLE layerHandle, D2D1_RECT_F contentBounds = D2D1::InfiniteRect(),
		__in_opt HANDLE geometryHandle = NULL, __in_opt HANDLE opacityBrush = NULL, 
		D2D1_LAYER_OPTIONS layerOptions = D2D1_LAYER_OPTIONS_NONE);
	D2DLIB_API void PopLayer(HANDLE ctx);

	D2DLIB_API HRESULT GetLastErrorCode(HANDLE ctx);

	D2DLIB_API void ReleaseObject(HANDLE handle);

	D2DLIB_API void TestDraw(HANDLE context);
}

#endif /* __D2DLIB_H__ */
