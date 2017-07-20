#include "stdafx.h"

#include "Bitmap.h"

HANDLE CreateBitmapFromFile(HANDLE ctx,
  PCWSTR uri, UINT destinationWidth, UINT destinationHeight, ID2D1Bitmap **ppBitmap)
{
	D2DContext* context = reinterpret_cast<D2DContext*>(ctx);
	
	IWICBitmapDecoder *pDecoder = NULL;
	IWICBitmapFrameDecode *pSource = NULL;
	IWICStream *pStream = NULL;
	IWICFormatConverter *pConverter = NULL;
	//IWICBitmapScaler *pScaler = NULL;

	HRESULT hr = context->imageFactory->CreateDecoderFromFilename(
		uri, NULL, GENERIC_READ, WICDecodeMetadataCacheOnLoad, &pDecoder);
	
	if (SUCCEEDED(hr))
  {
    hr = pDecoder->GetFrame(0, &pSource);
  }

	if (SUCCEEDED(hr))
  {
    // Convert the image format to 32bppPBGRA
    // (DXGI_FORMAT_B8G8R8A8_UNORM + D2D1_ALPHA_MODE_PREMULTIPLIED).
    hr = context->imageFactory->CreateFormatConverter(&pConverter);
	}

	if (SUCCEEDED(hr))
  {
		hr = pConverter->Initialize(
          pSource,
          GUID_WICPixelFormat32bppPBGRA,
          WICBitmapDitherTypeNone,
          NULL,
          0.f,
          WICBitmapPaletteTypeMedianCut
          );
	}

	if (SUCCEEDED(hr))
  {
		hr = context->renderTarget->CreateBitmapFromWicBitmap(pConverter, NULL, ppBitmap);
	}

	SafeRelease(&pDecoder);
  SafeRelease(&pSource);
  SafeRelease(&pStream);
  SafeRelease(&pConverter);
  //SafeRelease(&pScaler);

	return (HANDLE)0;
}

HANDLE CreateBitmapFromHBitmap(HANDLE ctx, HBITMAP hBitmap, BOOL alpha)
{
	RetrieveContext(ctx);
	
	IWICBitmap* wicBitmap = NULL;

	context->imageFactory->CreateBitmapFromHBITMAP(hBitmap, 0, 
		alpha ? WICBitmapAlphaChannelOption::WICBitmapUseAlpha
		: WICBitmapAlphaChannelOption::WICBitmapIgnoreAlpha, &wicBitmap);

	ID2D1Bitmap* d2dBitmap = NULL;
		context->renderTarget->CreateBitmapFromWicBitmap(wicBitmap, NULL, &d2dBitmap);

	SafeRelease(&wicBitmap);
	
	return (HANDLE)d2dBitmap;
}

HANDLE CreateBitmapFromMemory(HANDLE ctx, UINT width, UINT height, UINT stride, BYTE* buffer, UINT offset, UINT length)
{
	RetrieveContext(ctx);

	IWICBitmap* wicBitmap = NULL;

	HRESULT hr = context->imageFactory->CreateBitmapFromMemory(width, height, 
		GUID_WICPixelFormat32bppPBGRA, stride, length, buffer, &wicBitmap);

	ID2D1Bitmap* d2dBitmap = NULL;
	hr = context->renderTarget->CreateBitmapFromWicBitmap(wicBitmap, NULL, &d2dBitmap);

	SafeRelease(&wicBitmap);

	return (HANDLE)d2dBitmap;
}

HANDLE CreateBitmapFromBytes(HANDLE ctx, BYTE* buffer, UINT offset, UINT length)
{
	RetrieveContext(ctx);

	IWICImagingFactory* imageFactory = context->imageFactory;

	IWICBitmapDecoder *decoder = NULL;
	IWICBitmapFrameDecode *source = NULL;
	IWICStream *stream = NULL;
	IWICFormatConverter *converter = NULL;
	IWICBitmapScaler *scaler = NULL;

	ID2D1Bitmap* bitmap = NULL;

	HRESULT hr = S_OK;

	if (SUCCEEDED(hr))
	{
		// Create a WIC stream to map onto the memory.
		hr = imageFactory->CreateStream(&stream);
	}

	if (SUCCEEDED(hr))
	{
		// Initialize the stream with the memory pointer and size.
		hr = stream->InitializeFromMemory(reinterpret_cast<BYTE*>(buffer + offset), length);
	}

	if (SUCCEEDED(hr))
	{
		// Create a decoder for the stream.
		hr = imageFactory->CreateDecoderFromStream(stream, NULL, WICDecodeMetadataCacheOnLoad, &decoder);
	}

	if (SUCCEEDED(hr))
	{
		// Create the initial frame.
		hr = decoder->GetFrame(0, &source);
	}
	
	if (SUCCEEDED(hr))
	{
		// Convert the image format to 32bppPBGRA
		// (DXGI_FORMAT_B8G8R8A8_UNORM + D2D1_ALPHA_MODE_PREMULTIPLIED).
		hr = imageFactory->CreateFormatConverter(&converter);
	}

	if (SUCCEEDED(hr))
  {
		hr = converter->Initialize(source, GUID_WICPixelFormat32bppPBGRA, 
			WICBitmapDitherTypeNone, NULL, 0.f, WICBitmapPaletteTypeMedianCut);
	}

	if (SUCCEEDED(hr))
	{
		//create a Direct2D bitmap from the WIC bitmap.
		hr = context->renderTarget->CreateBitmapFromWicBitmap(converter, NULL, &bitmap);
	}

	SafeRelease(&decoder);
	SafeRelease(&source);
	SafeRelease(&stream);
	SafeRelease(&converter);
	SafeRelease(&scaler);

	return (HANDLE)bitmap;
}

HANDLE CreateBitmapFromFile(HANDLE ctx, LPCWSTR filepath) 
{
	RetrieveContext(ctx);

	IWICImagingFactory* imageFactory = context->imageFactory;

	IWICBitmapDecoder *decoder = NULL;
	IWICBitmapFrameDecode *source = NULL;
	IWICStream *stream = NULL;
	IWICFormatConverter *converter = NULL;
	IWICBitmapScaler *scaler = NULL;

	ID2D1Bitmap* bitmap = NULL;

	HRESULT hr = S_OK;

	/*if (imageFactory == NULL) {
		MessageBox(NULL, L"null", NULL, MB_OK);
	}

	MessageBox(NULL, L"CreateStream", NULL, MB_OK);*/

	if (SUCCEEDED(hr))
	{
		// Create a WIC stream to map onto the memory.
		hr = imageFactory->CreateStream(&stream);
	}
	//else MessageBox(NULL, L"CreateStream failed", NULL, MB_OK);

	if (SUCCEEDED(hr))
	{
		// Initialize the stream with the memory pointer and size.
		hr = stream->InitializeFromFilename(filepath, GENERIC_READ);
	}
	//else MessageBox(NULL, L"InitializeFromFilename failed", NULL, MB_OK);

	if (SUCCEEDED(hr))
	{
		// Create a decoder for the stream.
		hr = imageFactory->CreateDecoderFromStream(stream, NULL, WICDecodeMetadataCacheOnLoad, &decoder);
	}
	//else MessageBox(NULL, L"CreateDecoderFromStream failed", NULL, MB_OK);

	if (SUCCEEDED(hr))
	{
		// Create the initial frame.
		hr = decoder->GetFrame(0, &source);
	}
	//else MessageBox(NULL, L"GetFrame failed", NULL, MB_OK);

	if (SUCCEEDED(hr))
	{
		// Convert the image format to 32bppPBGRA
		// (DXGI_FORMAT_B8G8R8A8_UNORM + D2D1_ALPHA_MODE_PREMULTIPLIED).
		hr = imageFactory->CreateFormatConverter(&converter);
	}
	//else MessageBox(NULL, L"CreateFormatConverter failed", NULL, MB_OK);

	if (SUCCEEDED(hr))
  {
		hr = converter->Initialize(source, GUID_WICPixelFormat32bppPBGRA, 
			WICBitmapDitherTypeNone, NULL, 0.f, WICBitmapPaletteTypeMedianCut);
	}
	//else MessageBox(NULL, L"Initialize failed", NULL, MB_OK);

	if (SUCCEEDED(hr))
	{
		//create a Direct2D bitmap from the WIC bitmap.
		hr = context->renderTarget->CreateBitmapFromWicBitmap(converter, NULL, &bitmap);
	}
	//else MessageBox(NULL, L"CreateBitmapFromWicBitmap failed", NULL, MB_OK);

	SafeRelease(&decoder);
	SafeRelease(&source);
	SafeRelease(&stream);
	SafeRelease(&converter);
	SafeRelease(&scaler);

	return (HANDLE)bitmap;
}

D2D1_SIZE_F GetBitmapSize(HANDLE d2dbitmap)
{
	ID2D1Bitmap* bitmap = reinterpret_cast<ID2D1Bitmap*>(d2dbitmap);
	return bitmap->GetSize();
}

void DrawGDIBitmap(HANDLE hContext, HBITMAP hBitmap, FLOAT opacity, BOOL alpha,
									 D2D1_BITMAP_INTERPOLATION_MODE interpolationMode)
{
	DrawGDIBitmapRect(hContext, hBitmap, NULL, NULL, opacity, alpha, interpolationMode);
}

void DrawGDIBitmapRect(HANDLE hContext, HBITMAP hBitmap, D2D1_RECT_F* rect, 
											 D2D1_RECT_F* sourceRectangle, FLOAT opacity, BOOL alpha,
											 D2D1_BITMAP_INTERPOLATION_MODE interpolationMode)
{
	D2DContext* context = reinterpret_cast<D2DContext*>(hContext);
	//IWICBitmap* bitmap = reinterpret_cast<IWICBitmap*>(hBitmap);

	//IWICBitmapDecoder* decoder;
	//context->imageFactory->CreateDecoderFromFilename(
	//	TEXT("D:\\dotnet-projects\\D2DLib\\D2DLibCSharpTest\\Resources\\08.gif"),
	//	NULL, GENERIC_READ, WICDecodeOptions::WICDecodeMetadataCacheOnLoad, &decoder);

	//IWICBitmapFrameDecode *source = NULL;
	//decoder->GetFrame(0, &source);

	//IWICFormatConverter *converter = NULL;
	//context->imageFactory->CreateFormatConverter(&converter);

	//converter->Initialize(source, GUID_WICPixelFormat32bppPBGRA,
	//											WICBitmapDitherTypeNone, NULL, 0.f, WICBitmapPaletteTypeMedianCut);

	IWICBitmap* bitmap = NULL;

	context->imageFactory->CreateBitmapFromHBITMAP(hBitmap, 0, 
		alpha ? WICBitmapAlphaChannelOption::WICBitmapUseAlpha
		: WICBitmapAlphaChannelOption::WICBitmapIgnoreAlpha, &bitmap);

	ID2D1Bitmap* d2dBitmap = NULL;

	HRESULT hr = context->renderTarget->CreateBitmapFromWicBitmap(bitmap, NULL, &d2dBitmap);
	
	_ASSERT(d2dBitmap != NULL);

	context->renderTarget->DrawBitmap(d2dBitmap, rect, opacity, interpolationMode, sourceRectangle);

	SafeRelease(&d2dBitmap);
	SafeRelease(&bitmap);
	//SafeRelease(&converter);
}


void DrawD2DBitmap(HANDLE ctx, HANDLE d2dbitmap, D2D1_RECT_F* rect, FLOAT opacity,
		D2D1_BITMAP_INTERPOLATION_MODE interpolationMode)
{
	RetrieveContext(ctx);
	ID2D1Bitmap* bitmap = reinterpret_cast<ID2D1Bitmap*>(d2dbitmap);

	context->renderTarget->DrawBitmap(bitmap, rect, opacity, interpolationMode);
}