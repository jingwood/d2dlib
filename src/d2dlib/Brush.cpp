/*
* MIT License
*
* Copyright (c) 2009-2018 Jingwood, unvell.com. All right reserved.
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

HANDLE CreateStrokeStyle(HANDLE ctx, FLOAT* dashes, UINT dashCount)
{
	RetrieveContext(ctx);

	ID2D1StrokeStyle* strokeStyle;

	context->factory->CreateStrokeStyle(D2D1::StrokeStyleProperties(
            D2D1_CAP_STYLE_FLAT,
            D2D1_CAP_STYLE_FLAT,
            D2D1_CAP_STYLE_ROUND,
            D2D1_LINE_JOIN_MITER,
            10.0f,
            D2D1_DASH_STYLE_CUSTOM,
            0.0f), dashes, dashCount, &strokeStyle);

	return (HANDLE)strokeStyle;
}

HANDLE CreateSolidColorBrush(HANDLE ctx, D2D1_COLOR_F color)
{
	RetrieveContext(ctx);

	ID2D1SolidColorBrush* brush;
	context->renderTarget->CreateSolidColorBrush(color, &brush);
	
	return (HANDLE)brush;
}

void SetSolidColorBrushColor(HANDLE brushHandle, D2D1_COLOR_F color)
{
	ID2D1SolidColorBrush* brush = reinterpret_cast<ID2D1SolidColorBrush*>(brushHandle);
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

	if (SUCCEEDED(hr))
	{
		hr = renderTarget->CreateLinearGradientBrush(
			D2D1::LinearGradientBrushProperties(startPoint, endPoint), gradientStopCollection, &brush);
	}

	return (HANDLE)brush;
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
	
	ID2D1RadialGradientBrush* radialGradientBrush = NULL;

	if (SUCCEEDED(hr)) 
	{
		hr = renderTarget->CreateRadialGradientBrush(D2D1::RadialGradientBrushProperties(
			origin, offset, radiusX, radiusY), gradientStopCollection, &radialGradientBrush);
	}

	return (HANDLE)radialGradientBrush;
}
