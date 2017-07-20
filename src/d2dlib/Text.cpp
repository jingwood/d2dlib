
#include "stdafx.h"
#include "Text.h"

D2DLIB_API void DrawString(HANDLE ctx, LPCWSTR text, D2D1_COLOR_F color, 
													 LPCWSTR fontName, FLOAT fontSize, D2D1_RECT_F* rect,
													 DWRITE_TEXT_ALIGNMENT halign, DWRITE_PARAGRAPH_ALIGNMENT valign)
{
	RetrieveContext(ctx);

	IDWriteTextFormat* textFormat = NULL;

	// Create a DirectWrite text format object.
  context->writeFactory->CreateTextFormat(fontName, 
			NULL,
      DWRITE_FONT_WEIGHT_NORMAL, DWRITE_FONT_STYLE_NORMAL, DWRITE_FONT_STRETCH_NORMAL,
      fontSize,
      L"", //locale
      &textFormat);

	textFormat->SetTextAlignment(halign);
	textFormat->SetParagraphAlignment(valign);

	// Create a solid color brush.
	ID2D1SolidColorBrush* brush;
	(context->renderTarget)->CreateSolidColorBrush(color, &brush);

	context->renderTarget->DrawText(text, wcslen(text), textFormat, rect, brush);

	SafeRelease(&brush);
	SafeRelease(&textFormat);
}

void DrawGlyphRun(HANDLE ctx, D2D1_POINT_2F baselineOrigin, 
			const DWRITE_GLYPH_RUN *glyphRun, D2D1_COLOR_F color,
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