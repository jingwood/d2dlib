#pragma once

#include "Context.h"

typedef struct D2DPen
{
	ID2D1SolidColorBrush* brush;
	ID2D1StrokeStyle* strokeStyle;
} D2DPen;

extern "C"
{
	D2DLIB_API HANDLE CreatePenStroke(HANDLE context, D2D1_COLOR_F color,
		D2D1_DASH_STYLE dashStyle = D2D1_DASH_STYLE::D2D1_DASH_STYLE_SOLID);

	D2DLIB_API void DestoryPenStroke(HANDLE pen);
}