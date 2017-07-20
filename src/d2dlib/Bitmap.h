
#include "Context.h"

extern "C" 
{
	D2DLIB_API HANDLE CreateBitmapFromHBitmap(HANDLE ctx, HBITMAP hBitmap, BOOL useAlpha = FALSE);
	D2DLIB_API HANDLE CreateBitmapFromBytes(HANDLE ctx, BYTE* buffer, UINT offset, UINT length);
	D2DLIB_API HANDLE CreateBitmapFromFile(HANDLE ctx, LPCWSTR filepath);
	D2DLIB_API HANDLE CreateBitmapFromMemory(HANDLE ctx, UINT width, UINT height, UINT stride, BYTE* buffer, UINT offset, UINT length);

	D2DLIB_API void DrawD2DBitmap(HANDLE context, HANDLE d2dbitmap, D2D1_RECT_F* rect, FLOAT opacity = 1,
		D2D1_BITMAP_INTERPOLATION_MODE interpolationMode = D2D1_BITMAP_INTERPOLATION_MODE_LINEAR);

	D2DLIB_API void DrawGDIBitmap(HANDLE context, HBITMAP bitmap, FLOAT opacity = 1, BOOL alpha = FALSE,
		D2D1_BITMAP_INTERPOLATION_MODE = D2D1_BITMAP_INTERPOLATION_MODE_LINEAR);

	D2DLIB_API void DrawGDIBitmapRect(HANDLE context, HBITMAP bitmap,
		D2D1_RECT_F* rect = NULL, D2D1_RECT_F* sourceRectangle = NULL, FLOAT opacity = 1, 
		BOOL alpha = FALSE, D2D1_BITMAP_INTERPOLATION_MODE = D2D1_BITMAP_INTERPOLATION_MODE_LINEAR);

	D2DLIB_API D2D1_SIZE_F GetBitmapSize(HANDLE d2dbitmap);
}