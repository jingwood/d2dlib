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

#include "stdafx.h"

#include <initguid.h>
#include <d2d1effects.h>

#include "Effect.h"

// Generic effect API

HANDLE CreateEffect(HANDLE ctx, REFCLSID effectId)
{
	RetrieveContext(ctx);

	if (context->deviceContext == NULL) return NULL;

	ID2D1Effect* effect = NULL;
	HRESULT hr = context->deviceContext->CreateEffect(effectId, &effect);

	if (!SUCCEEDED(hr) || effect == NULL) {
		context->lastErrorCode = hr;
		return NULL;
	}

	D2DEffectContext* effectCtx = new D2DEffectContext();
	effectCtx->d2context = context;
	effectCtx->effect = effect;

	return (HANDLE)effectCtx;
}

void DestroyEffect(HANDLE effectHandle)
{
	if (effectHandle == NULL) return;

	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	SafeRelease(&effectCtx->effect);
	delete effectCtx;
}

void SetEffectInput(HANDLE effectHandle, UINT index, HANDLE bitmapHandle)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	ID2D1Bitmap* bitmap = reinterpret_cast<ID2D1Bitmap*>(bitmapHandle);
	effectCtx->effect->SetInput(index, bitmap);
}

void SetEffectInputEffect(HANDLE effectHandle, UINT index, HANDLE inputEffectHandle)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	D2DEffectContext* inputEffectCtx = reinterpret_cast<D2DEffectContext*>(inputEffectHandle);
	effectCtx->effect->SetInputEffect(index, inputEffectCtx->effect);
}

void DrawEffect(HANDLE ctx, HANDLE effectHandle,
	D2D1_POINT_2F* targetOffset, D2D1_RECT_F* imageRectangle,
	D2D1_INTERPOLATION_MODE interpolationMode)
{
	RetrieveContext(ctx);
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);

	ID2D1Image* output = NULL;
	effectCtx->effect->GetOutput(&output);

	context->deviceContext->DrawImage(
		output,
		targetOffset,
		imageRectangle,
		interpolationMode,
		D2D1_COMPOSITE_MODE_SOURCE_OVER);

	SafeRelease(&output);
}

// Effect property setters

void SetEffectValueFloat(HANDLE effectHandle, UINT index, FLOAT value)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	effectCtx->effect->SetValue(index, value);
}

void SetEffectValueInt(HANDLE effectHandle, UINT index, INT value)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	effectCtx->effect->SetValue(index, value);
}

void SetEffectValueBool(HANDLE effectHandle, UINT index, BOOL value)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	effectCtx->effect->SetValue(index, value);
}

void SetEffectValueVector2(HANDLE effectHandle, UINT index, D2D1_POINT_2F value)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	effectCtx->effect->SetValue(index, value);
}

void SetEffectValueVector3(HANDLE effectHandle, UINT index, D2D1_VECTOR_3F value)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	effectCtx->effect->SetValue(index, value);
}

void SetEffectValueVector4(HANDLE effectHandle, UINT index, D2D1_VECTOR_4F value)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	effectCtx->effect->SetValue(index, value);
}

void SetEffectValueColor(HANDLE effectHandle, UINT index, D2D1_COLOR_F value)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	effectCtx->effect->SetValue(index, value);
}

// Effect property getters

FLOAT GetEffectValueFloat(HANDLE effectHandle, UINT index)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	return effectCtx->effect->GetValue<FLOAT>(index);
}

D2D1_VECTOR_3F GetEffectValueVector3(HANDLE effectHandle, UINT index)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	return effectCtx->effect->GetValue<D2D1_VECTOR_3F>(index);
}

INT GetEffectValueInt(HANDLE effectHandle, UINT index)
{
	D2DEffectContext* effectCtx = reinterpret_cast<D2DEffectContext*>(effectHandle);
	return effectCtx->effect->GetValue<INT>(index);
}

// Convenience wrappers for common effects

HANDLE CreateGaussianBlurEffect(HANDLE ctx)
{
	return CreateEffect(ctx, CLSID_D2D1GaussianBlur);
}

HANDLE CreateShadowEffect(HANDLE ctx)
{
	return CreateEffect(ctx, CLSID_D2D1Shadow);
}

HANDLE CreateSaturationEffect(HANDLE ctx)
{
	return CreateEffect(ctx, CLSID_D2D1Saturation);
}

HANDLE Create3DPerspectiveTransformEffect(HANDLE ctx)
{
	return CreateEffect(ctx, CLSID_D2D13DPerspectiveTransform);
}

HANDLE CreateColorMatrixEffect(HANDLE ctx)
{
	return CreateEffect(ctx, CLSID_D2D1ColorMatrix);
}

HANDLE CreateBrightnessEffect(HANDLE ctx)
{
	return CreateEffect(ctx, CLSID_D2D1Brightness);
}

HANDLE CreateHueRotationEffect(HANDLE ctx)
{
	return CreateEffect(ctx, CLSID_D2D1HueRotation);
}

