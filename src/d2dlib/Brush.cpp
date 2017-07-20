
#include "stdafx.h"
#include "Brush.h"

HANDLE CreateStorkeStyle(HANDLE ctx, FLOAT* dashes, UINT dashCount)
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
