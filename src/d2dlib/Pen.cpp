
#include "stdafx.h"
#include "Pen.h"

D2DLIB_API HANDLE CreatePenStroke(HANDLE ctx, D2D1_COLOR_F color, D2D1_DASH_STYLE dashStyle)
{
	RetrieveContext(ctx);

	ID2D1SolidColorBrush* brush;
	(context->renderTarget)->CreateSolidColorBrush(color, &brush);

	ID2D1StrokeStyle* strokeStyle = NULL;

	if (dashStyle != D2D1_DASH_STYLE_SOLID)
	{
		context->factory->CreateStrokeStyle(D2D1::StrokeStyleProperties(
			D2D1_CAP_STYLE_FLAT,
			D2D1_CAP_STYLE_FLAT,
			D2D1_CAP_STYLE_ROUND,
			D2D1_LINE_JOIN_MITER,
			10.0f,
			dashStyle,
			0.0f), NULL, 0, &strokeStyle);
	}

	D2DPen* pen = new D2DPen();
	pen->brush = brush;
	pen->strokeStyle = strokeStyle;

	return (HANDLE)pen;
}

D2DLIB_API void DestoryPenStroke(HANDLE penHandle)
{
	D2DPen* pen = (D2DPen*)penHandle;

	SafeRelease(&pen->strokeStyle);
	SafeRelease(&pen->brush);
}