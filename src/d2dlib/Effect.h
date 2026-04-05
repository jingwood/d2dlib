/*
* MIT License
*
* Copyright (c) 2009-2024 Jingwood, unvell.com. All right reserved.
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

#pragma once

#include "Context.h"

typedef struct D2DEffectContext {
	D2DContext* d2context;
	ID2D1Effect* effect;
} D2DEffectContext;

extern "C"
{
	// Generic effect API
	D2DLIB_API HANDLE CreateEffect(HANDLE ctx, REFCLSID effectId);
	D2DLIB_API void DestroyEffect(HANDLE effectHandle);

	D2DLIB_API void SetEffectInput(HANDLE effectHandle, UINT index, HANDLE bitmapHandle);
	D2DLIB_API void SetEffectInputEffect(HANDLE effectHandle, UINT index, HANDLE inputEffectHandle);

	D2DLIB_API void DrawEffect(HANDLE ctx, HANDLE effectHandle,
		D2D1_POINT_2F* targetOffset,
		D2D1_RECT_F* imageRectangle,
		D2D1_INTERPOLATION_MODE interpolationMode);

	// Effect property setters (by type)
	D2DLIB_API void SetEffectValueFloat(HANDLE effectHandle, UINT index, FLOAT value);
	D2DLIB_API void SetEffectValueInt(HANDLE effectHandle, UINT index, INT value);
	D2DLIB_API void SetEffectValueBool(HANDLE effectHandle, UINT index, BOOL value);
	D2DLIB_API void SetEffectValueVector2(HANDLE effectHandle, UINT index, D2D1_POINT_2F value);
	D2DLIB_API void SetEffectValueVector3(HANDLE effectHandle, UINT index, D2D1_VECTOR_3F value);
	D2DLIB_API void SetEffectValueVector4(HANDLE effectHandle, UINT index, D2D1_VECTOR_4F value);
	D2DLIB_API void SetEffectValueColor(HANDLE effectHandle, UINT index, D2D1_COLOR_F value);

	// Effect property getters
	D2DLIB_API FLOAT GetEffectValueFloat(HANDLE effectHandle, UINT index);
	D2DLIB_API INT GetEffectValueInt(HANDLE effectHandle, UINT index);
	D2DLIB_API D2D1_VECTOR_3F GetEffectValueVector3(HANDLE effectHandle, UINT index);

	// Convenience wrappers for common effects
	D2DLIB_API HANDLE CreateGaussianBlurEffect(HANDLE ctx);
	D2DLIB_API HANDLE CreateShadowEffect(HANDLE ctx);
	D2DLIB_API HANDLE CreateSaturationEffect(HANDLE ctx);
	D2DLIB_API HANDLE Create3DPerspectiveTransformEffect(HANDLE ctx);
	D2DLIB_API HANDLE CreateColorMatrixEffect(HANDLE ctx);
	D2DLIB_API HANDLE CreateBrightnessEffect(HANDLE ctx);
	D2DLIB_API HANDLE CreateHueRotationEffect(HANDLE ctx);
	D2DLIB_API HANDLE CreateCompositeEffect(HANDLE ctx);
	D2DLIB_API HANDLE CreateBlendEffect(HANDLE ctx);
	D2DLIB_API HANDLE CreateBorderEffect(HANDLE ctx);
	D2DLIB_API HANDLE CreateCropEffect(HANDLE ctx);

	// Effect input count
	D2DLIB_API void SetEffectInputCount(HANDLE effectHandle, UINT count);
}
