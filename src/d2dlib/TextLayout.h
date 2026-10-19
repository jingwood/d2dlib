/*
 * MIT License
 *
 * Copyright (c) 2009-2026 Jingwood, unvell.com. All right reserved.
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

// Device-independent DirectWrite text. Formats and layouts belong to the process-wide shared
// DirectWrite factory, not to a context, so they can be created, measured and kept in a cache
// without any render target - and drawn on every context. Release them with ReleaseObject.
//
// Laying text out (shaping, font fallback) is what text costs; drawing a laid-out layout is cheap.
// A caller that redraws the same strings frame after frame should keep the layouts.

// Options for CreateTextLayoutEx.
enum D2DTextLayoutOptions {
	// Keep the text on one line however long it is.
	D2DLIB_TEXT_NO_WRAP = 0x1,
	// Cut text that does not fit maxWidth at a character and end it with an ellipsis.
	D2DLIB_TEXT_TRIM_ELLIPSIS = 0x2,
	// Underline the whole text.
	D2DLIB_TEXT_UNDERLINE = 0x4,
	// Strike the whole text through.
	D2DLIB_TEXT_STRIKETHROUGH = 0x8,
};

// Blittable; mirrored by the managed callers.
struct D2DTextLayoutMetrics
{
	FLOAT left;
	FLOAT top;
	FLOAT width;
	FLOAT widthIncludingTrailingWhitespace;
	FLOAT height;
	FLOAT layoutWidth;
	FLOAT layoutHeight;
	UINT32 lineCount;
	// Distance from the top of the first line to its baseline, and that line's height.
	FLOAT baseline;
	FLOAT lineHeight;
};

extern "C"
{
	D2DLIB_API HANDLE CreateTextFormatEx(LPCWSTR fontFamily, FLOAT fontSize,
		DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch,
		LPCWSTR locale);

	// Lay out length characters of text (no terminating zero needed) in the format, within a
	// maxWidth x maxHeight box aligned by halign / valign.
	D2DLIB_API HANDLE CreateTextLayoutEx(const WCHAR* text, UINT32 length, HANDLE textFormat,
		FLOAT maxWidth, FLOAT maxHeight,
		DWRITE_TEXT_ALIGNMENT halign, DWRITE_PARAGRAPH_ALIGNMENT valign, UINT options);

	D2DLIB_API HRESULT SetTextLayoutFontFamily(HANDLE textLayout, UINT32 start, UINT32 length, LPCWSTR fontFamily);
	D2DLIB_API HRESULT SetTextLayoutFontSize(HANDLE textLayout, UINT32 start, UINT32 length, FLOAT fontSize);
	D2DLIB_API HRESULT SetTextLayoutFontWeight(HANDLE textLayout, UINT32 start, UINT32 length, DWRITE_FONT_WEIGHT fontWeight);
	D2DLIB_API HRESULT SetTextLayoutFontStyle(HANDLE textLayout, UINT32 start, UINT32 length, DWRITE_FONT_STYLE fontStyle);
	D2DLIB_API HRESULT SetTextLayoutUnderline(HANDLE textLayout, UINT32 start, UINT32 length, BOOL underline);
	D2DLIB_API HRESULT SetTextLayoutStrikethrough(HANDLE textLayout, UINT32 start, UINT32 length, BOOL strikethrough);
	D2DLIB_API HRESULT SetTextLayoutMaxSize(HANDLE textLayout, FLOAT maxWidth, FLOAT maxHeight);

	D2DLIB_API HRESULT GetTextLayoutMetrics(HANDLE textLayout, D2DTextLayoutMetrics* metrics);

	// Draw a layout with its top-left at origin in a solid color (the context's cached brush).
	D2DLIB_API void DrawTextLayoutWithColor(HANDLE ctx, HANDLE textLayout, D2D1_POINT_2F origin,
		D2D1_COLOR_F color, D2D1_DRAW_TEXT_OPTIONS options);

	// 1 when the installed family has a glyph for every character of chars, 0 when it lacks one,
	// -1 when no family of that name is installed. Surrogate pairs are read as one character.
	D2DLIB_API INT FontFamilyHasCharacters(LPCWSTR fontFamily, const WCHAR* chars, UINT32 length);
}
