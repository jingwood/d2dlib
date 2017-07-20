
#include "Context.h"

extern "C"
{
	D2DLIB_API void DrawLine(HANDLE ctx, D2D1_POINT_2F start, D2D1_POINT_2F end, D2D1_COLOR_F color,
		FLOAT width = 1, D2D1_DASH_STYLE dashStyle = D2D1_DASH_STYLE::D2D1_DASH_STYLE_SOLID);

	D2DLIB_API void DrawLineWithPen(HANDLE ctx, D2D1_POINT_2F start, D2D1_POINT_2F end, HANDLE pen, FLOAT width = 1);

	D2DLIB_API void DrawLines(HANDLE handle, D2D1_POINT_2F* points, UINT count, D2D1_COLOR_F color,
		FLOAT width = 1, D2D1_DASH_STYLE dashStyle = D2D1_DASH_STYLE::D2D1_DASH_STYLE_SOLID);

	D2DLIB_API void DrawRectangle(HANDLE handle, D2D1_RECT_F* rect, D2D1_COLOR_F color,
		FLOAT width = 1, D2D1_DASH_STYLE dashStyle = D2D1_DASH_STYLE::D2D1_DASH_STYLE_SOLID);

	D2DLIB_API void FillRectangle(HANDLE handle, D2D1_RECT_F* rect, D2D1_COLOR_F color);

	D2DLIB_API void DrawEllipse(HANDLE handle, D2D1_ELLIPSE* ellipse, D2D1_COLOR_F color,
		FLOAT width = 1, D2D1_DASH_STYLE dashStyle = D2D1_DASH_STYLE::D2D1_DASH_STYLE_SOLID);

	D2DLIB_API void FillEllipse(HANDLE handle, D2D1_ELLIPSE* ellipse, D2D1_COLOR_F color);

	D2DLIB_API void FillEllipseWithBrush(HANDLE ctx, D2D1_ELLIPSE* ellipse, HANDLE brush);

}