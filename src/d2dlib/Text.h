#include "Context.h"

extern "C" 
{
	D2DLIB_API void DrawString(HANDLE handle, LPCWSTR text, D2D1_COLOR_F color,
														 LPCWSTR fontName, FLOAT fontSize, D2D1_RECT_F* rect,
														 DWRITE_TEXT_ALIGNMENT halign = DWRITE_TEXT_ALIGNMENT::DWRITE_TEXT_ALIGNMENT_LEADING,
														 DWRITE_PARAGRAPH_ALIGNMENT valign = DWRITE_PARAGRAPH_ALIGNMENT::DWRITE_PARAGRAPH_ALIGNMENT_NEAR);


	D2DLIB_API void DrawGlyphRun(HANDLE ctx, D2D1_POINT_2F baselineOrigin, 
			const DWRITE_GLYPH_RUN *glyphRun, D2D1_COLOR_F color,
			DWRITE_MEASURING_MODE measuringMode = DWRITE_MEASURING_MODE_NATURAL);
}
