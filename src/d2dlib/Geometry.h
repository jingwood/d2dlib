
#include "Context.h"

extern "C"
{
	D2DLIB_API HANDLE CreateRectangleGeometry(HANDLE ctx, D2D1_RECT_F& rect);

	D2DLIB_API HANDLE CreatePathGeometry(HANDLE ctx);
	D2DLIB_API void DestoryPathGeometry(HANDLE handle);
	D2DLIB_API void ClosePath(HANDLE handle);

	D2DLIB_API void AddPathLines(HANDLE ctx, D2D1_POINT_2F* points, UINT count);
	D2DLIB_API void AddPathBeziers(HANDLE ctx, D2D1_BEZIER_SEGMENT* bezierSegments, UINT count);
	D2DLIB_API void AddPathEllipse(HANDLE ctx, const D2D1_ELLIPSE* ellipse);
	D2DLIB_API void AddPathArc(HANDLE ctx, FLOAT w, FLOAT h, D2D1_POINT_2F endPoint, FLOAT sweepAngle,
		D2D1_SWEEP_DIRECTION sweepDirection = D2D1_SWEEP_DIRECTION::D2D1_SWEEP_DIRECTION_CLOCKWISE);

	D2DLIB_API void DrawBeziers(HANDLE ctx, D2D1_BEZIER_SEGMENT* bezierSegments, UINT count, 
															D2D1_COLOR_F strokeColor, FLOAT strokeWidth = 1, 
															D2D1_DASH_STYLE dashStyle = D2D1_DASH_STYLE::D2D1_DASH_STYLE_SOLID);

	D2DLIB_API void DrawPath(HANDLE pathCtx, D2D1_COLOR_F strokeColor, FLOAT strokeWidth, D2D1_DASH_STYLE dashStyle);
	D2DLIB_API void FillPathD(HANDLE pathCtx, D2D1_COLOR_F fillColor);

	D2DLIB_API void FillGeometryWithBrush(HANDLE ctx, HANDLE geoHandle, 
		__in HANDLE brushHandle, __in_opt HANDLE opacityBrushHandle = NULL);

	D2DLIB_API bool PathFillContainsPoint(HANDLE pathCtx, D2D1_POINT_2F point);
	D2DLIB_API bool PathStrokeContainsPoint(HANDLE pathCtx, D2D1_POINT_2F point, FLOAT strokeWidth = 1,
		D2D1_DASH_STYLE dashStyle = D2D1_DASH_STYLE::D2D1_DASH_STYLE_SOLID);

	D2DLIB_API void DrawPolygon(HANDLE ctx, D2D1_POINT_2F* points, UINT count,
		D2D1_COLOR_F strokeColor, FLOAT strokeWidth, D2D1_DASH_STYLE dashStyle, D2D1_COLOR_F fillColor);
}