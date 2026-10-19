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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace unvell.D2DLib
{
	internal static class D2D
	{

#if DEBUG

#if X86
		const string DLL_NAME = "d2dlib32d.dll";
#elif X64
		const string DLL_NAME = "d2dlib64d.dll";
#endif

#else // Release

#if X86
		const string DLL_NAME = "d2dlib32.dll";
#elif X64
		const string DLL_NAME = "d2dlib64.dll";
#endif

#endif

		#region Device Context

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE GetLastResult();

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateContext([In] HANDLE hwnd, D2D1PresentOptions presentOptions);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyContext([In] HANDLE context);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetContextProperties([In] HANDLE context, D2DAntialiasMode antialiasMode = D2DAntialiasMode.PerPrimitive);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetTextAntialiasMode([In] HANDLE context, D2DTextAntialiasMode textAntialiasMode);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE BeginRender([In] HANDLE context);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE BeginRenderWithBackgroundColor([In] HANDLE context, D2DColor backColor);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE BeginRenderWithBackgroundBitmap(HANDLE context, HANDLE bitmap);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void EndRender([In] HANDLE context);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Flush([In] HANDLE context);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Clear([In] HANDLE context, D2DColor color);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapRenderTarget([In] HANDLE context, D2DSize size);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawBitmapRenderTarget([In] HANDLE context, HANDLE bitmapRenderTarget, ref D2DRect rect,
			FLOAT opacity = 1, D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE GetBitmapRenderTargetBitmap(HANDLE bitmapRenderTarget);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyBitmapRenderTarget([In] HANDLE context);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE ResizeContext([In] HANDLE context);


		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE GetDPI([In] HANDLE context, out FLOAT dpix, out FLOAT dpiy);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE SetDPI([In] HANDLE context, FLOAT dpix, FLOAT dpiy);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void PushClip([In] HANDLE context, [In] ref D2DRect rect,
			D2DAntialiasMode antiAliasMode = D2DAntialiasMode.PerPrimitive);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void PopClip([In] HANDLE context);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateLayer(HANDLE ctx);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE PushLayer(HANDLE ctx, HANDLE layerHandle, D2DRect contentBounds,
			[In, Optional] HANDLE geometryHandle, [In, Optional] HANDLE opacityBrush, LayerOptions layerOptions = LayerOptions.InitializeForClearType);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void PopLayer(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void PushTransform([In] HANDLE context);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void PopTransform([In] HANDLE context);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void RotateTransform([In] HANDLE context, [In] FLOAT angle);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void RotateTransform([In] HANDLE context, [In] FLOAT angle, [In] D2DPoint center);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void TranslateTransform([In] HANDLE context, [In] FLOAT x, [In] FLOAT y);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ScaleTransform([In] HANDLE context, [In] FLOAT sx, [In] FLOAT sy, [Optional] D2DPoint center);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SkewTransform([In] HANDLE ctx, [In] FLOAT angleX, [In] FLOAT angleY, [Optional] D2DPoint center);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetTransform([In] HANDLE context, [In] ref Matrix3x2 mat);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void GetTransform([In] HANDLE context, [Out] out Matrix3x2 mat);
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ResetTransform([In] HANDLE context);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ReleaseObject([In] HANDLE objectHandle);
		#endregion // Device Context

		#region Simple Sharp

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawLine(HANDLE context, D2DPoint start, D2DPoint end, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid,
			D2DCapStyle startCap = D2DCapStyle.Flat, D2DCapStyle endCap = D2DCapStyle.Flat);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawLines(HANDLE context, D2DPoint[] points, UINT count, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawUnconnectedLines(HANDLE context, D2DPoint[] points, UINT count, D2DColor color, FLOAT width);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawRectangle(HANDLE context, ref D2DRect rect, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawRectangleWithPen(HANDLE context, ref D2DRect rect, HANDLE strokePen, FLOAT weight = 1);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillRectangle(HANDLE context, ref D2DRect rect, D2DColor color);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillRectangleWithBrush(HANDLE context, ref D2DRect rect, HANDLE brush);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawFillRectangle(HANDLE context, ref D2DRect rect, HANDLE fillBrush, HANDLE strokePen, FLOAT width = 1);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawRoundedRect(HANDLE ctx, ref D2DRoundedRect roundedRect,
			D2DColor strokeColor, D2DColor fillColor,
			FLOAT strokeWidth = 1, D2DDashStyle strokeStyle = D2DDashStyle.Solid);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawRoundedRectWithBrush(HANDLE ctx, ref D2DRoundedRect roundedRect,
			HANDLE strokePen, HANDLE fillBrush, float strokeWidth = 1);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawEllipse(HANDLE context, ref D2DEllipse rect, D2DColor color,
			FLOAT width = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillEllipse(HANDLE context, ref D2DEllipse rect, D2DColor color);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillEllipseWithBrush(HANDLE ctx, ref D2DEllipse ellipse, HANDLE brush);

		#endregion // Simple Sharp

		#region Text

		[DllImport(DLL_NAME, EntryPoint = "DrawString", CharSet = CharSet.Unicode,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawText([In] HANDLE context, [In] string text, [In] D2DColor color,
			[In] string fontName, [In] FLOAT fontSize, [In] D2DRect rect,
			[In] D2DFontWeight fontWeight = D2DFontWeight.Normal,
			[In] D2DFontStyle fontStyle = D2DFontStyle.Normal,
			[In] D2DFontStretch fontStretch = D2DFontStretch.Normal,
			[In] DWriteTextAlignment halign = DWriteTextAlignment.Leading,
			[In] DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near);

		[DllImport(DLL_NAME, EntryPoint = "MeasureText", CharSet = CharSet.Unicode,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern void MeasureText([In] HANDLE ctx, [In] string text, [In] string fontName,
			[In] FLOAT fontSize, ref D2DSize size,
			[In] D2DFontWeight fontWeight = D2DFontWeight.Normal,
			[In] D2DFontStyle fontStyle = D2DFontStyle.Normal,
			[In] D2DFontStretch fontStretch = D2DFontStretch.Normal,
			[In] DWriteTextAlignment halign = DWriteTextAlignment.Leading,
			[In] DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near);

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static extern void MeasureTextWithFormat([In] HANDLE ctx, [In] string text, [In] HANDLE textFormat, ref D2DSize size);

		[DllImport(DLL_NAME, EntryPoint = "MeasureTextWithLayout", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern void MeasureTextWithLayout([In] HANDLE ctx, [In] HANDLE textLayout, ref D2DSize size);

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateFontFace([In] HANDLE context, [In] string fontName,
		[In] D2DFontWeight fontWeight = D2DFontWeight.Normal,
			[In] D2DFontStyle fontStyle = D2DFontStyle.Normal,
			[In] D2DFontStretch fontStretch = D2DFontStretch.Normal);

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyFontFace(HANDLE fontFaceHandle);

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateTextPathGeometry(HANDLE ctx, [In] string text,
			HANDLE fontFaceHandle, FLOAT fontSize);

		[DllImport(DLL_NAME, EntryPoint = "CreateTextFormat", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateTextFormat([In] HANDLE ctx, [In] string fontName, [In] FLOAT fontSize, 
			[In] D2DFontWeight fontWeight = D2DFontWeight.Normal, 
			[In] D2DFontStyle fontStyle = D2DFontStyle.Normal, 
			[In] D2DFontStretch fontStretch = D2DFontStretch.Normal,
			[In] DWriteTextAlignment hAlign = DWriteTextAlignment.Leading, 
			[In] DWriteParagraphAlignment vAlign = DWriteParagraphAlignment.Near);

		[DllImport(DLL_NAME, EntryPoint = "CreateTextLayoutWithFormat", CharSet = CharSet.Unicode)]
		public static extern HANDLE CreateTextLayout([In] HANDLE ctx, [In] string text, [In] HANDLE textFormatHandler, [In] ref D2DSize size);

		[DllImport(DLL_NAME, EntryPoint = "CreateTextLayout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern HANDLE CreateTextLayout([In] HANDLE ctx, [In] string text, [In] HANDLE textFormatHandler, [In] ref D2DSize size,
			[In] D2DFontWeight fontWeight = D2DFontWeight.Normal,
			[In] D2DFontStyle fontStyle = D2DFontStyle.Normal,
			[In] D2DFontStretch fontStretch = D2DFontStretch.Normal);

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static extern void DrawStringWithBrushAndTextFormat([In] HANDLE context, [In] string text, [In] HANDLE brush,
			[In] HANDLE textFormat, [In] ref D2DRect rect);

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static extern void DrawStringWithLayout([In] HANDLE context, [In] HANDLE brush,
			[In] HANDLE textLayout, [In] D2DPoint origin);

		#endregion // Text

		#region Context (extended)

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateContextEx([In] HANDLE hwnd, D2DContextOptions options, HANDLE shareDevicesWith);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateWicBitmapContext(UINT width, UINT height, FLOAT dpiX, FLOAT dpiY, D2DContextOptions options);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int CopyContextPixels(HANDLE context, [Out] byte[] buffer, UINT stride, UINT bufferSize);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int GetLastCreateError();

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int ResizeContextEx([In] HANDLE context);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int EndRenderEx([In] HANDLE context);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawLineWithStrokeStyle(HANDLE context, D2DPoint start, D2DPoint end, D2DColor color,
			FLOAT width, HANDLE strokeStyle);

		#endregion // Context (extended)

		#region Text layout

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateTextFormatEx([In] string fontFamily, FLOAT fontSize,
			D2DFontWeight fontWeight, D2DFontStyle fontStyle, D2DFontStretch fontStretch, [In] string? locale);

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateTextLayoutEx([In] string text, UINT length, HANDLE textFormat,
			FLOAT maxWidth, FLOAT maxHeight, DWriteTextAlignment halign, DWriteParagraphAlignment valign,
			D2DTextLayoutOptions options);

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SetTextLayoutFontFamily(HANDLE textLayout, UINT start, UINT length, [In] string fontFamily);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SetTextLayoutFontSize(HANDLE textLayout, UINT start, UINT length, FLOAT fontSize);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SetTextLayoutFontWeight(HANDLE textLayout, UINT start, UINT length, D2DFontWeight fontWeight);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SetTextLayoutFontStyle(HANDLE textLayout, UINT start, UINT length, D2DFontStyle fontStyle);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SetTextLayoutUnderline(HANDLE textLayout, UINT start, UINT length, BOOL underline);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SetTextLayoutStrikethrough(HANDLE textLayout, UINT start, UINT length, BOOL strikethrough);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SetTextLayoutMaxSize(HANDLE textLayout, FLOAT maxWidth, FLOAT maxHeight);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int GetTextLayoutMetrics(HANDLE textLayout, out D2DTextLayoutMetrics metrics);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawTextLayoutWithColor(HANDLE context, HANDLE textLayout, D2DPoint origin,
			D2DColor color, D2DDrawTextOptions options);

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern int FontFamilyHasCharacters([In] string fontFamily, [In] string chars, UINT length);

		#endregion // Text layout

		#region Geometry

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateRectangleGeometry([In] HANDLE ctx, [In] ref D2DRect rect);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyGeometry(HANDLE geometryContext);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateEllipseGeometry(HANDLE ctx, ref D2DEllipse ellipse);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreatePathGeometry(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateCombinedGeometry(HANDLE d2dCtx, HANDLE pathCtx1, HANDLE pathCtx2,
			D2D1CombineMode combineMode, FLOAT flatteningTolerance = 10f);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyPathGeometry(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillGeometryWithBrush([In] HANDLE ctx, [In] HANDLE geoHandle,
			[In] HANDLE brushHandle, [Optional] HANDLE opacityBrushHandle);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawPolygon(HANDLE ctx, D2DPoint[] points, UINT count,
			D2DColor strokeColor, FLOAT strokeWidth, D2DDashStyle dashStyle, D2DColor fillColor);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawPolygonWithBrush(HANDLE ctx, D2DPoint[] points, UINT count,
			D2DColor strokeColor, FLOAT strokeWidth, D2DDashStyle dashStyle, HANDLE brushHandler);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetPathStartPoint(HANDLE ctx, D2DPoint startPoint);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ClosePath(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathLines(HANDLE path, D2DPoint[] points, uint count);
		public static void AddPathLines(HANDLE path, D2DPoint[] points) { AddPathLines(path, points, (uint)points.Length); }

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathBeziers(HANDLE ctx, D2DBezierSegment[] bezierSegments, uint count);

		public static void AddPathBeziers(HANDLE ctx, D2DBezierSegment[] bezierSegments)
		{
			AddPathBeziers(ctx, bezierSegments, (uint)bezierSegments.Length);
		}

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathEllipse(HANDLE path, ref D2DEllipse ellipse);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathArc(HANDLE ctx, D2DPoint endPoint, D2DSize size, FLOAT sweepAngle,
			D2DArcSize arcSize = D2DArcSize.Small,
			D2DSweepDirection sweepDirection = D2DSweepDirection.Clockwise);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawBeziers(HANDLE ctx, D2DBezierSegment[] bezierSegments, UINT count,
															D2DColor strokeColor, FLOAT strokeWidth = 1,
															D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawPath(HANDLE path, D2DColor strokeColor, FLOAT strokeWidth = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawPathWithPen(HANDLE path, HANDLE pen, FLOAT strokeWidth = 1);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillPathD(HANDLE path, D2DColor fillColor);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillGeometryWithBrush(HANDLE path, HANDLE brush);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool PathFillContainsPoint(HANDLE pathCtx, D2DPoint point);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool PathStrokeContainsPoint(HANDLE pathCtx, D2DPoint point, FLOAT strokeWidth = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void GetGeometryBounds(HANDLE pathCtx, ref D2DRect rect);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void GetGeometryTransformedBounds(HANDLE pathCtx, ref Matrix3x2 mat3x2, ref D2DRect rect);

		// Geometry Realization (D2D 1.2)
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateFilledGeometryRealization(HANDLE ctx, HANDLE geoHandle, float flatteningTolerance);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateStrokedGeometryRealization(HANDLE ctx, HANDLE geoHandle, float flatteningTolerance,
			float strokeWidth, HANDLE strokeStyle);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawGeometryRealization(HANDLE ctx, HANDLE realizationHandle, HANDLE brushHandle);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyGeometryRealization(HANDLE realizationHandle);

		#endregion // Geometry

		#region Style
		[DllImport(DLL_NAME, EntryPoint = "CreateStrokeStyle", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateStrokeStyle(HANDLE ctx, FLOAT[]? dashes = null, UINT dashCount = 0, FLOAT dashOffset = 0.0f,
				D2DCapStyle startCap = D2DCapStyle.Flat, D2DCapStyle endCap = D2DCapStyle.Flat);
		#endregion Style

		#region Pen
		[DllImport(DLL_NAME, EntryPoint = "CreatePenStroke", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreatePen(HANDLE ctx, D2DColor strokeColor, D2DDashStyle dashStyle = D2DDashStyle.Solid,
	FLOAT[]? dashes = null, UINT dashCount = 0, FLOAT dashOffset = 0.0f);

		[DllImport(DLL_NAME, EntryPoint = "DestroyPenStroke", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyPen(HANDLE pen);
		#endregion Pen

		#region Brush

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateSolidColorBrush(HANDLE ctx, D2DColor color);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetSolidColorBrushColor(HANDLE brush, D2DColor color);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateLinearGradientBrush(HANDLE ctx, D2DPoint startPoint, D2DPoint endPoint,
																											D2DGradientStop[] gradientStops, UINT gradientStopCount);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateRadialGradientBrush(HANDLE ctx, D2DPoint origin, D2DPoint offset,
																													FLOAT radiusX, FLOAT radiusY, D2DGradientStop[] gradientStops,
								      																					UINT gradientStopCount);
				
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapBrush(HANDLE ctx, HANDLE bitmap,
																									D2DExtendMode extendModeX, D2DExtendMode extendModeY,
																									D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ReleaseBrush(HANDLE brushCtx);

		#endregion // Brush

		#region Bitmap
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapFromHBitmap(HANDLE context, HANDLE hBitmap, bool useAlphaChannel);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapFromBytes(HANDLE context, byte[] buffer, UINT offset, UINT length);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapFromMemory(HANDLE ctx, UINT width, UINT height, UINT stride, IntPtr buffer,
			UINT offset, UINT length);

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode)]
		public static extern HANDLE CreateBitmapFromFile(HANDLE context, string filepath);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawD2DBitmap(HANDLE context, HANDLE bitmap);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawD2DBitmap(HANDLE context, HANDLE bitmap, ref D2DRect destRect);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawD2DBitmap(HANDLE context, HANDLE bitmap, ref D2DRect destRect, ref D2DRect srcRect,
			FLOAT opacity = 1, D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawGDIBitmap(HANDLE context, HANDLE bitmap, FLOAT opacity = 1,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawGDIBitmapRect(HANDLE context, HANDLE bitmap,
			ref D2DRect destRect, ref D2DRect srcRect, FLOAT opacity = 1, bool alpha = false,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern D2DSize GetBitmapSize(HANDLE d2dbitmap);
		#endregion

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void TestDraw(HANDLE ctx);

		#region Effects

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateEffect(HANDLE ctx, [In] ref Guid effectId);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyEffect(HANDLE effectHandle);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetEffectInput(HANDLE effectHandle, uint index, HANDLE bitmapHandle);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetEffectInputEffect(HANDLE effectHandle, uint index, HANDLE inputEffectHandle);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawEffect(HANDLE ctx, HANDLE effectHandle,
			ref D2DPoint targetOffset, ref D2DRect imageRectangle,
			D2DInterpolationMode interpolationMode = D2DInterpolationMode.Linear);

		[DllImport(DLL_NAME, EntryPoint = "DrawEffect", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawEffectSimple(HANDLE ctx, HANDLE effectHandle,
			IntPtr targetOffset, IntPtr imageRectangle,
			D2DInterpolationMode interpolationMode = D2DInterpolationMode.Linear);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetEffectValueFloat(HANDLE effectHandle, uint index, float value);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetEffectValueInt(HANDLE effectHandle, uint index, int value);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetEffectValueBool(HANDLE effectHandle, uint index, [MarshalAs(UnmanagedType.Bool)] bool value);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetEffectValueVector2(HANDLE effectHandle, uint index, D2DPoint value);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetEffectValueVector3(HANDLE effectHandle, uint index, D2DVector3 value);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetEffectValueColor(HANDLE effectHandle, uint index, D2DColor value);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetEffectValueMatrix5x4(HANDLE effectHandle, uint index, D2DMatrix5x4 value);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern float GetEffectValueFloat(HANDLE effectHandle, uint index);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int GetEffectValueInt(HANDLE effectHandle, uint index);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern D2DVector3 GetEffectValueVector3(HANDLE effectHandle, uint index);

		// Convenience wrappers
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateGaussianBlurEffect(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateShadowEffect(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateSaturationEffect(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE Create3DPerspectiveTransformEffect(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateColorMatrixEffect(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBrightnessEffect(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateHueRotationEffect(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateCompositeEffect(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBlendEffect(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBorderEffect(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateCropEffect(HANDLE ctx);

		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetEffectInputCount(HANDLE effectHandle, uint count);

		#endregion // Effects
	}
}
