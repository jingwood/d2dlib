
#include "Context.h"

extern "C"
{
	D2DLIB_API HANDLE CreateSolidColorBrush(HANDLE ctx, D2D1_COLOR_F color);
	D2DLIB_API void SetSolidColorBrushColor(HANDLE brush, D2D1_COLOR_F color);

	D2DLIB_API HANDLE CreateRadialGradientBrush(HANDLE ctx, D2D1_POINT_2F origin, D2D1_POINT_2F offset,
																						  FLOAT radiusX, FLOAT radiusY, D2D1_GRADIENT_STOP* gradientStops, 
																							UINT gradientStopCount);
}