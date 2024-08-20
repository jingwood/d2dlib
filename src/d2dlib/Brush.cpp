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

#include "stdafx.h"
#include "Brush.h"
//
//HANDLE CreateStrokeStyle(HANDLE ctx, FLOAT* dashes, UINT dashCount)
//{
//	RetrieveContext(ctx);
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
//            0.0f), dashes, dashCount, &strokeStyle);
//
//	return (HANDLE)strokeStyle;
//}

HANDLE CreateSolidColorBrushContext(HANDLE ctx, D2D1_COLOR_F color)
{
	RetrieveContext(ctx);

	ID2D1SolidColorBrush* brush;
	context->renderTarget->CreateSolidColorBrush(color, &brush);

	D2DBrushContext* brushContext = new D2DBrushContext();
	brushContext->context = context;
	brushContext->type = BrushType::BrushType_SolidBrush;
	brushContext->brush = brush;

	return (HANDLE)brushContext;
}

HANDLE CreateSolidColorBrush(HANDLE ctx, D2D1_COLOR_F color)
{
	RetrieveContext(ctx);

	ID2D1SolidColorBrush* brush;
	HRESULT hr = (context->renderTarget)->CreateSolidColorBrush(color, &brush);
	if (SUCCEEDED(hr) && brush != NULL) {
		return (HANDLE)brush;
	}
	return NULL;
}

void SetSolidColorBrushColor(HANDLE brushHandle, D2D1_COLOR_F color)
{
	D2DBrushContext* brushContext = reinterpret_cast<D2DBrushContext*>(brushHandle);
	ID2D1SolidColorBrush* brush = reinterpret_cast<ID2D1SolidColorBrush*>(brushContext->brush);
	brush->SetColor(color);
}

HANDLE CreateLinearGradientBrush(HANDLE ctx, D2D1_POINT_2F startPoint, D2D1_POINT_2F endPoint,
	D2D1_GRADIENT_STOP* gradientStops, UINT gradientStopCount)
{
	RetrieveContext(ctx);
	ID2D1RenderTarget* renderTarget = context->renderTarget;
	HRESULT hr;

	ID2D1GradientStopCollection* gradientStopCollection = NULL;

	hr = renderTarget->CreateGradientStopCollection(gradientStops, gradientStopCount, &gradientStopCollection);

	ID2D1LinearGradientBrush* brush = NULL;
	D2DBrushContext* brushContext = NULL;

	if (SUCCEEDED(hr))
	{
		hr = renderTarget->CreateLinearGradientBrush(
			D2D1::LinearGradientBrushProperties(startPoint, endPoint), gradientStopCollection, &brush);

		if (SUCCEEDED(hr)) {
			brushContext = new D2DBrushContext();
			brushContext->context = context;
			brushContext->type = BrushType::BrushType_LinearGradientBrush;
			brushContext->brush = brush;
			brushContext->gradientStops = gradientStopCollection;
		}
	}

	return (HANDLE)brushContext;
}

HANDLE CreateBitmapBrush(HANDLE ctx, ID2D1Bitmap* bitmap,
	D2D1_EXTEND_MODE extendModeX, D2D1_EXTEND_MODE extendModeY,
	D2D1_BITMAP_INTERPOLATION_MODE interpolationMode)
{
	RetrieveContext(ctx);
	ID2D1RenderTarget* renderTarget = context->renderTarget;
	HRESULT hr;

	ID2D1BitmapBrush* brush = NULL;
	D2DBrushContext* brushContext = NULL;
	
	hr = renderTarget->CreateBitmapBrush(bitmap, 
		D2D1::BitmapBrushProperties(extendModeX, extendModeY, interpolationMode), &brush);
		
	if (SUCCEEDED(hr)) {
		brushContext = new D2DBrushContext();
		brushContext->context = context;
		brushContext->type = BrushType::BrushType_BitmapBrush;
		brushContext->brush = brush;
		brushContext->bitmap = bitmap;
	}

	return (HANDLE)brushContext;
}

HANDLE CreateRadialGradientBrush(HANDLE ctx, D2D1_POINT_2F origin, D2D1_POINT_2F offset,
																 FLOAT radiusX, FLOAT radiusY, D2D1_GRADIENT_STOP* gradientStops, 
																 UINT gradientStopCount)
{
	RetrieveContext(ctx);
	ID2D1RenderTarget* renderTarget = context->renderTarget;
	HRESULT hr;

	ID2D1GradientStopCollection *gradientStopCollection = NULL;

  hr = renderTarget->CreateGradientStopCollection(
		gradientStops, gradientStopCount, &gradientStopCollection);
	
	ID2D1RadialGradientBrush* brush = NULL;
	D2DBrushContext* brushContext = NULL;

	if (SUCCEEDED(hr)) 
	{
		hr = renderTarget->CreateRadialGradientBrush(D2D1::RadialGradientBrushProperties(
			origin, offset, radiusX, radiusY), gradientStopCollection, &brush);

		if (SUCCEEDED(hr)) {
			brushContext = new D2DBrushContext();
			brushContext->context = context;
			brushContext->type = BrushType::BrushType_RadialGradientBrush;
			brushContext->brush = brush;
			brushContext->gradientStops = gradientStopCollection;
		}
	}

	return (HANDLE)brushContext;
}

void ReleaseBrush(HANDLE brushHandle)
{
	D2DBrushContext* brushContext = reinterpret_cast<D2DBrushContext*>(brushHandle);

	switch (brushContext->type)
	{
	case BrushType::BrushType_LinearGradientBrush:
	case BrushType::BrushType_RadialGradientBrush:
		SafeRelease(&brushContext->gradientStops);
		break;
	case BrushType::BrushType_BitmapBrush:
		SafeRelease(&brushContext->bitmap);
		break;
	}

	SafeRelease(&brushContext->brush);
	
	delete brushContext;
}
