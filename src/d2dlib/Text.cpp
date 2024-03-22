/*
 * MIT License
 *
 * Copyright (c) 2009-2021 Jingwood, unvell.com. All right reserved.
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
#include "Text.h"

D2DLIB_API void DrawString(HANDLE ctx, LPCWSTR text, D2D1_COLOR_F color,
	LPCWSTR fontName, FLOAT fontSize, D2D1_RECT_F rect,
	DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle,
	DWRITE_FONT_STRETCH fontStretch,
	DWRITE_TEXT_ALIGNMENT halign, DWRITE_PARAGRAPH_ALIGNMENT valign)
{
	RetrieveContext(ctx);

	ID2D1SolidColorBrush* brush = NULL;
	IDWriteTextFormat* textFormat = NULL;

	HRESULT hr = context->writeFactory->CreateTextFormat(fontName, NULL,
		fontWeight, fontStyle, fontStretch, fontSize, L"", &textFormat);

	if (SUCCEEDED(hr) && textFormat != NULL)
	{
		textFormat->SetTextAlignment(halign);
		textFormat->SetParagraphAlignment(valign);

		// Create a solid color brush.
		hr = (context->renderTarget)->CreateSolidColorBrush(color, &brush);

		if (SUCCEEDED(hr) && brush != NULL) {
			context->renderTarget->DrawText(text, (UINT32)wcslen(text), textFormat, rect, brush);
		}
	}

	SafeRelease(&brush);
	SafeRelease(&textFormat);
}

D2DLIB_API HANDLE CreateTextLayout(HANDLE ctx, LPCWSTR text, LPCWSTR fontName, FLOAT fontSize, D2D1_SIZE_F* size,
	DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch) {
	RetrieveContext(ctx);

	IDWriteTextFormat* textFormat = NULL;

	HRESULT hr = context->writeFactory->CreateTextFormat(fontName,
		NULL,
		DWRITE_FONT_WEIGHT_NORMAL, DWRITE_FONT_STYLE_NORMAL, DWRITE_FONT_STRETCH_NORMAL,
		fontSize,
		L"", //locale
		&textFormat);

	if (SUCCEEDED(hr) && textFormat != NULL)
	{
		IDWriteTextLayout* textLayout;

		hr = context->writeFactory->CreateTextLayout(
			text,      // The string to be laid out and formatted.
			(UINT32)wcslen(text),  // The length of the string.
			textFormat,  // The text format to apply to the string (contains font information, etc).
			size->width,         // The width of the layout box.
			size->height,        // The height of the layout box.
			&textLayout  // The IDWriteTextLayout interface pointer.
		);

		if (SUCCEEDED(hr) && textLayout != NULL) {
			SafeRelease(&textFormat);
			return (HANDLE)textLayout;
		}
	}

	SafeRelease(&textFormat);

	return NULL;
}

D2DLIB_API HANDLE CreateFontFace(HANDLE ctx, LPCWSTR fontName,
	DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch) {

	RetrieveContext(ctx);
	HRESULT hr = NULL;
	D2DFontFace* d2dFontFace = NULL;

	IDWriteFontCollection* coll;
	hr = context->writeFactory->GetSystemFontCollection(&coll);

	if (SUCCEEDED(hr))
	{
		UINT32 fontIndex;
		BOOL fontIndexFound;
		coll->FindFamilyName(fontName, &fontIndex, &fontIndexFound);

		if (fontIndexFound) {

			IDWriteFontFamily* fontFamily;
			hr = coll->GetFontFamily(fontIndex, &fontFamily);

			if (SUCCEEDED(hr))
			{
				IDWriteFont* font;
				hr = fontFamily->GetFirstMatchingFont(fontWeight, fontStretch, fontStyle, &font);

				if (SUCCEEDED(hr)) {
					IDWriteFontFace* fontFace;
					hr = font->CreateFontFace(&fontFace);

					if (SUCCEEDED(hr)) {
						d2dFontFace = new D2DFontFace();
						d2dFontFace->font = font;
						d2dFontFace->fontFace = fontFace;
					}
				}

				SafeRelease(&fontFamily);
			}
		}
	}

	SafeRelease(&coll);

	return d2dFontFace;
}

void DestroyFontFace(HANDLE fontFaceHandle) {
	D2DFontFace* fontFace = reinterpret_cast<D2DFontFace*>(fontFaceHandle);

	if (fontFace != NULL) {
		SafeRelease(&fontFace->font);
		SafeRelease(&fontFace->fontFace);
		delete fontFace;
	}
}

HANDLE CreateTextPathGeometry(HANDLE ctx, LPCWSTR text, HANDLE fontFaceHandle, FLOAT fontSize) {

	RetrieveContext(ctx);
	D2DFontFace* fontFaceWrap = reinterpret_cast<D2DFontFace*>(fontFaceHandle);

	if (fontFaceWrap == NULL) {
		return NULL;
	}

	HRESULT hr = NULL;
	D2DPathContext* pathContext = NULL;
	IDWriteFontFace* fontFace = fontFaceWrap->fontFace;

	size_t textLength = wcslen(text);

	UINT* codePoints = new UINT[textLength];
	UINT16* glyphIndices = new UINT16[textLength];
	ZeroMemory(codePoints, sizeof(UINT) * textLength);
	ZeroMemory(glyphIndices, sizeof(UINT16) * textLength);

	for (size_t i = 0; i < textLength; i++)
	{
		codePoints[i] = text[i];
	}

	hr = fontFace->GetGlyphIndicesW(codePoints, (UINT32)textLength, glyphIndices);

	if (SUCCEEDED(hr)) {

		ID2D1PathGeometry* path = NULL;
		ID2D1GeometrySink* sink = NULL;

		hr = context->factory->CreatePathGeometry(&path);
		if (SUCCEEDED(hr)) {

			hr = path->Open(&sink);
			if (SUCCEEDED(hr)) {

				hr = fontFace->GetGlyphRunOutline(fontSize * 96.0f / 72.0f, glyphIndices,
					NULL, NULL, (UINT32)textLength, FALSE, FALSE, sink);

				//sink->SetFillMode(D2D1_FILL_MODE_WINDING);

				sink->Close();
				SafeRelease(&sink);

				if (SUCCEEDED(hr)) {
					pathContext = new D2DPathContext();

					pathContext->path = path;
					pathContext->geometry = pathContext->path;
					pathContext->d2context = context;
				}
			}

			if (pathContext == NULL) {
				SafeRelease(&path);
			}
		}
	}

	delete[] codePoints;
	codePoints = NULL;
	delete[] glyphIndices;
	glyphIndices = NULL;

	return (HANDLE)pathContext;
}

D2DLIB_API void MeasureText(HANDLE ctx, LPCWSTR text, LPCWSTR fontName, FLOAT fontSize, D2D1_SIZE_F* size,
	DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch) {
	RetrieveContext(ctx);

	IDWriteTextLayout* textLayout = (IDWriteTextLayout*)CreateTextLayout(ctx, text, fontName, fontSize, size);

	if (textLayout != NULL) {
		DWRITE_TEXT_METRICS tm;
		textLayout->GetMetrics(&tm);

		size->width = tm.width;
		size->height = tm.height;
	}

	SafeRelease(&textLayout);
}

void DrawGlyphRun(HANDLE ctx, D2D1_POINT_2F baselineOrigin,
	const DWRITE_GLYPH_RUN* glyphRun, D2D1_COLOR_F color,
	DWRITE_MEASURING_MODE measuringMode)
{
	D2DContext* context = reinterpret_cast<D2DContext*>(ctx);

}

//void DrawTextLayout(HANDLE ctx, D2D1_POINT_2F origin,
//  [in]  IDWriteTextLayout *textLayout,
//  [in]  ID2D1Brush *defaultForegroundBrush,
//  D2D1_DRAW_TEXT_OPTIONS options = D2D1_DRAW_TEXT_OPTIONS_NONE
//)
//{
//}
