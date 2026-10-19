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

#include "stdafx.h"
#include "TextLayout.h"

#include <vector>

#define RetrieveTextLayout(handle) IDWriteTextLayout* layout = reinterpret_cast<IDWriteTextLayout*>(handle); \
	if (layout == NULL) return E_INVALIDARG

HANDLE CreateTextFormatEx(LPCWSTR fontFamily, FLOAT fontSize,
	DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch,
	LPCWSTR locale)
{
	IDWriteFactory* factory = GetSharedWriteFactory();
	if (factory == NULL) return NULL;

	IDWriteTextFormat* textFormat = NULL;

	HRESULT hr = factory->CreateTextFormat(fontFamily, NULL,
		fontWeight, fontStyle, fontStretch, fontSize, locale != NULL ? locale : L"", &textFormat);

	return SUCCEEDED(hr) ? (HANDLE)textFormat : NULL;
}

HANDLE CreateTextLayoutEx(const WCHAR* text, UINT32 length, HANDLE textFormatHandle,
	FLOAT maxWidth, FLOAT maxHeight,
	DWRITE_TEXT_ALIGNMENT halign, DWRITE_PARAGRAPH_ALIGNMENT valign, UINT options)
{
	IDWriteFactory* factory = GetSharedWriteFactory();
	IDWriteTextFormat* textFormat = reinterpret_cast<IDWriteTextFormat*>(textFormatHandle);
	if (factory == NULL || textFormat == NULL) return NULL;

	IDWriteTextLayout* layout = NULL;

	HRESULT hr = factory->CreateTextLayout(text != NULL ? text : L"", text != NULL ? length : 0,
		textFormat, maxWidth, maxHeight, &layout);

	if (FAILED(hr)) return NULL;

	layout->SetTextAlignment(halign);
	layout->SetParagraphAlignment(valign);

	if (options & D2DLIB_TEXT_NO_WRAP)
	{
		layout->SetWordWrapping(DWRITE_WORD_WRAPPING_NO_WRAP);
	}

	DWRITE_TEXT_RANGE all = { 0, length };

	if (options & D2DLIB_TEXT_UNDERLINE)
	{
		layout->SetUnderline(TRUE, all);
	}

	if (options & D2DLIB_TEXT_STRIKETHROUGH)
	{
		layout->SetStrikethrough(TRUE, all);
	}

	if (options & D2DLIB_TEXT_TRIM_ELLIPSIS)
	{
		// The sign is made from the layout itself so it uses the layout's font; the layout keeps
		// its own reference.
		IDWriteInlineObject* ellipsis = NULL;

		if (SUCCEEDED(factory->CreateEllipsisTrimmingSign(layout, &ellipsis)))
		{
			DWRITE_TRIMMING trimming = { DWRITE_TRIMMING_GRANULARITY_CHARACTER, 0, 0 };
			layout->SetTrimming(&trimming, ellipsis);
			SafeRelease(&ellipsis);
		}
	}

	return (HANDLE)layout;
}

HRESULT SetTextLayoutFontFamily(HANDLE textLayout, UINT32 start, UINT32 length, LPCWSTR fontFamily)
{
	RetrieveTextLayout(textLayout);
	return layout->SetFontFamilyName(fontFamily, DWRITE_TEXT_RANGE{ start, length });
}

HRESULT SetTextLayoutFontSize(HANDLE textLayout, UINT32 start, UINT32 length, FLOAT fontSize)
{
	RetrieveTextLayout(textLayout);
	return layout->SetFontSize(fontSize, DWRITE_TEXT_RANGE{ start, length });
}

HRESULT SetTextLayoutFontWeight(HANDLE textLayout, UINT32 start, UINT32 length, DWRITE_FONT_WEIGHT fontWeight)
{
	RetrieveTextLayout(textLayout);
	return layout->SetFontWeight(fontWeight, DWRITE_TEXT_RANGE{ start, length });
}

HRESULT SetTextLayoutFontStyle(HANDLE textLayout, UINT32 start, UINT32 length, DWRITE_FONT_STYLE fontStyle)
{
	RetrieveTextLayout(textLayout);
	return layout->SetFontStyle(fontStyle, DWRITE_TEXT_RANGE{ start, length });
}

HRESULT SetTextLayoutUnderline(HANDLE textLayout, UINT32 start, UINT32 length, BOOL underline)
{
	RetrieveTextLayout(textLayout);
	return layout->SetUnderline(underline, DWRITE_TEXT_RANGE{ start, length });
}

HRESULT SetTextLayoutStrikethrough(HANDLE textLayout, UINT32 start, UINT32 length, BOOL strikethrough)
{
	RetrieveTextLayout(textLayout);
	return layout->SetStrikethrough(strikethrough, DWRITE_TEXT_RANGE{ start, length });
}

HRESULT SetTextLayoutMaxSize(HANDLE textLayout, FLOAT maxWidth, FLOAT maxHeight)
{
	RetrieveTextLayout(textLayout);

	HRESULT hr = layout->SetMaxWidth(maxWidth);
	if (SUCCEEDED(hr)) hr = layout->SetMaxHeight(maxHeight);
	return hr;
}

HRESULT GetTextLayoutMetrics(HANDLE textLayout, D2DTextLayoutMetrics* metrics)
{
	RetrieveTextLayout(textLayout);
	if (metrics == NULL) return E_POINTER;

	DWRITE_TEXT_METRICS tm;
	HRESULT hr = layout->GetMetrics(&tm);
	if (FAILED(hr)) return hr;

	metrics->left = tm.left;
	metrics->top = tm.top;
	metrics->width = tm.width;
	metrics->widthIncludingTrailingWhitespace = tm.widthIncludingTrailingWhitespace;
	metrics->height = tm.height;
	metrics->layoutWidth = tm.layoutWidth;
	metrics->layoutHeight = tm.layoutHeight;
	metrics->lineCount = tm.lineCount;
	metrics->baseline = 0;
	metrics->lineHeight = 0;

	if (tm.lineCount > 0)
	{
		// GetLineMetrics only fills the array when it holds every line.
		DWRITE_LINE_METRICS first;
		UINT32 actual = 0;

		if (tm.lineCount == 1)
		{
			hr = layout->GetLineMetrics(&first, 1, &actual);
		}
		else
		{
			std::vector<DWRITE_LINE_METRICS> lines(tm.lineCount);
			hr = layout->GetLineMetrics(lines.data(), tm.lineCount, &actual);
			first = lines[0];
		}

		if (SUCCEEDED(hr))
		{
			metrics->baseline = first.baseline;
			metrics->lineHeight = first.height;
		}
	}

	return hr;
}

void DrawTextLayoutWithColor(HANDLE ctx, HANDLE textLayout, D2D1_POINT_2F origin,
	D2D1_COLOR_F color, D2D1_DRAW_TEXT_OPTIONS options)
{
	RetrieveContext(ctx);
	IDWriteTextLayout* layout = reinterpret_cast<IDWriteTextLayout*>(textLayout);

	if (layout == NULL) return;

	ID2D1SolidColorBrush* brush = GetSolidBrush(context, color);

	if (brush != NULL)
	{
		context->renderTarget->DrawTextLayout(origin, layout, brush, options);
	}
}

INT FontFamilyHasCharacters(LPCWSTR fontFamily, const WCHAR* chars, UINT32 length)
{
	IDWriteFactory* factory = GetSharedWriteFactory();
	if (factory == NULL || fontFamily == NULL) return -1;

	IDWriteFontCollection* collection = NULL;
	IDWriteFontFamily* family = NULL;
	IDWriteFont* font = NULL;
	INT result = -1;

	HRESULT hr = factory->GetSystemFontCollection(&collection, FALSE);

	if (SUCCEEDED(hr))
	{
		UINT32 index = 0;
		BOOL exists = FALSE;
		hr = collection->FindFamilyName(fontFamily, &index, &exists);

		if (SUCCEEDED(hr) && exists)
		{
			hr = collection->GetFontFamily(index, &family);

			if (SUCCEEDED(hr))
			{
				hr = family->GetFirstMatchingFont(DWRITE_FONT_WEIGHT_NORMAL,
					DWRITE_FONT_STRETCH_NORMAL, DWRITE_FONT_STYLE_NORMAL, &font);
			}

			if (SUCCEEDED(hr))
			{
				result = 1;

				for (UINT32 i = 0; i < length && result == 1; i++)
				{
					UINT32 codePoint = chars[i];

					if (IS_HIGH_SURROGATE(chars[i]) && i + 1 < length && IS_LOW_SURROGATE(chars[i + 1]))
					{
						codePoint = 0x10000 + ((chars[i] - 0xD800) << 10) + (chars[i + 1] - 0xDC00);
						i++;
					}

					BOOL has = FALSE;
					if (FAILED(font->HasCharacter(codePoint, &has)) || !has)
					{
						result = 0;
					}
				}
			}
		}
	}

	SafeRelease(&font);
	SafeRelease(&family);
	SafeRelease(&collection);

	return result;
}
