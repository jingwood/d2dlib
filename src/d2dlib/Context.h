
#include <Windows.h>
#include <d2d1.h>
#include <dwrite.h>
#include "Wincodec.h"

#include <map>
#include <stack>

using namespace std;

#ifndef __D2DLIB_H__
#define __D2DLIB_H__

typedef struct D2DContext
{
	ID2D1Factory* factory;
	IDWriteFactory* writeFactory;
	IWICImagingFactory* imageFactory;
	
	union
	{
		struct // HwndRenderTarget
		{
			ID2D1HwndRenderTarget* renderTarget;
			HWND hwnd;
		};

		struct // BitmapRenderTarget
		{
			ID2D1BitmapRenderTarget *bitmapRenderTarget;
			ID2D1Bitmap* bitmap;
		};
	};

	std::stack<D2D1_MATRIX_3X2_F>* matrixStack;

	HRESULT lastResult;
	
} D2DContext;

//typedef struct D2DBitmapRenderTargetContext
//{
//	ID2D1Factory *factory;
//	IDWriteFactory *writeFactory;
//	IWICImagingFactory* imageFactory;
//	HRESULT lastResult;
//
//} D2DBitmapRenderTargetContext;

// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the D2DLIB_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// D2DLIB_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef D2DLIB_EXPORTS
#define D2DLIB_API __declspec(dllexport)
#else
#define D2DLIB_API __declspec(dllimport)
#endif

// This class is exported from the D2DLib.dll
//class D2DLIB_API CD2DLib {
//public:
//	CD2DLib(void);
//	// TODO: add your methods here.
//};

template<class Interface>
inline void SafeRelease(Interface **ppInterfaceToRelease)
{
  if (*ppInterfaceToRelease != NULL)
  {
    (*ppInterfaceToRelease)->Release();
    (*ppInterfaceToRelease) = NULL;
  }
}

#define RetrieveContext(ctx) D2DContext* context = reinterpret_cast<D2DContext*>(ctx)
//#define RetrieveContext(ctx) D2DContext* context = (D2DContext*)(ctx)

//extern D2DLIB_API HRESULT LastResultCode;

extern "C" 
{
	D2DLIB_API HANDLE CreateContext(HWND hwnd);
	D2DLIB_API void DestoryContext(HANDLE context);
	D2DLIB_API void ResizeContext(HANDLE context);
	
	D2DLIB_API void SetContextProperties(HANDLE ctx,
		D2D1_ANTIALIAS_MODE antialiasMode = D2D1_ANTIALIAS_MODE::D2D1_ANTIALIAS_MODE_PER_PRIMITIVE);
	
	D2DLIB_API void BeginRender(HANDLE context);
	D2DLIB_API void BeginRenderWithBackgroundColor(HANDLE ctx, D2D1_COLOR_F color);
	D2DLIB_API void BeginRenderWithBackgroundBitmap(HANDLE ctx, HANDLE bitmap);
	D2DLIB_API void EndRender(HANDLE context);
	D2DLIB_API void Flush(HANDLE context);
	D2DLIB_API void Clear(HANDLE context, D2D1_COLOR_F color);

	D2DLIB_API HANDLE CreateBitmapRenderTarget(HANDLE context, D2D_SIZE_F size = D2D1::SizeF());
	D2DLIB_API void DrawBitmapRenderTarget(HANDLE context, HANDLE bitmapRenderTargetHandle, D2D1_RECT_F* rect = NULL,
		FLOAT opacity = 1, D2D1_BITMAP_INTERPOLATION_MODE interpolationMode = D2D1_BITMAP_INTERPOLATION_MODE_LINEAR);
	D2DLIB_API HANDLE GetBitmapRenderTargetBitmap(HANDLE bitmapRenderTargetHandle);
	D2DLIB_API void DestoryBitmapRenderTarget(HANDLE context);

	D2DLIB_API void PushClip(HANDLE context, D2D1_RECT_F* rect, 
												   D2D1_ANTIALIAS_MODE antiAliasMode = D2D1_ANTIALIAS_MODE::D2D1_ANTIALIAS_MODE_PER_PRIMITIVE);
	D2DLIB_API void PopClip(HANDLE context);

	D2DLIB_API void PushTransform(HANDLE context);
	D2DLIB_API void PopTransform(HANDLE context);
	D2DLIB_API void TranslateTransform(HANDLE context, FLOAT x, FLOAT y);
	D2DLIB_API void ScaleTransform(HANDLE context, FLOAT scaleX, FLOAT scaleY, D2D1_POINT_2F center = D2D1::Point2F());
	D2DLIB_API void RotateTransform(HANDLE context, FLOAT angle, D2D_POINT_2F point = D2D1::Point2F());
	D2DLIB_API void SkewTransform(HANDLE ctx, FLOAT angleX, FLOAT angleY, D2D1_POINT_2F center = D2D1::Point2F());
	D2DLIB_API void SetTransform(HANDLE context, FLOAT angle, D2D_POINT_2F center);
	D2DLIB_API void ResetTransform(HANDLE context);

	D2DLIB_API HANDLE CreateLayer(HANDLE context);
	D2DLIB_API void PushLayer(HANDLE context, HANDLE layerHandle, D2D1_RECT_F& contentBounds = D2D1::InfiniteRect(),
		 __in_opt ID2D1Brush *opacityBrush = NULL, D2D1_LAYER_OPTIONS layerOptions = D2D1_LAYER_OPTIONS_NONE);
	D2DLIB_API void PopLayer(HANDLE context, HANDLE layerHandle);

	D2DLIB_API HRESULT GetLastResult();
	D2DLIB_API void ReleaseObject(HANDLE context);

	D2DLIB_API void TestDraw(HANDLE context);
}

#endif /* __D2DLIB_H__ */