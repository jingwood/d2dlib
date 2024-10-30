﻿/*
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

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE GetLastResult();

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateContext([In] HANDLE hwnd);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyContext([In] HANDLE context);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetContextProperties([In] HANDLE context, D2DAntialiasMode antialiasMode = D2DAntialiasMode.PerPrimitive);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetTextAntialiasMode([In] HANDLE context, D2DTextAntialiasMode textAntialiasMode);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE BeginRender([In] HANDLE context);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE BeginRenderWithBackgroundColor([In] HANDLE context, D2DColor backColor);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE BeginRenderWithBackgroundBitmap(HANDLE context, HANDLE bitmap);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void EndRender([In] HANDLE context);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Flush([In] HANDLE context);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Clear([In] HANDLE context, D2DColor color);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapRenderTarget([In] HANDLE context, D2DSize size);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawBitmapRenderTarget([In] HANDLE context, HANDLE bitmapRenderTarget, ref D2DRect rect,
			FLOAT opacity = 1, D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE GetBitmapRenderTargetBitmap(HANDLE bitmapRenderTarget);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyBitmapRenderTarget([In] HANDLE context);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE ResizeContext([In] HANDLE context);


		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE GetDPI([In] HANDLE context, out FLOAT dpix, out FLOAT dpiy);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE SetDPI([In] HANDLE context, FLOAT dpix, FLOAT dpiy);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void PushClip([In] HANDLE context, [In] ref D2DRect rect,
			D2DAntialiasMode antiAliasMode = D2DAntialiasMode.PerPrimitive);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void PopClip([In] HANDLE context);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateLayer(HANDLE ctx);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE PushLayer(HANDLE ctx, HANDLE layerHandle, D2DRect contentBounds,
			[In, Optional] HANDLE geometryHandle, [In, Optional] HANDLE opacityBrush, LayerOptions layerOptions = LayerOptions.InitializeForClearType);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void PopLayer(HANDLE ctx);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void PushTransform([In] HANDLE context);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void PopTransform([In] HANDLE context);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void RotateTransform([In] HANDLE context, [In] FLOAT angle);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void RotateTransform([In] HANDLE context, [In] FLOAT angle, [In] D2DPoint center);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void TranslateTransform([In] HANDLE context, [In] FLOAT x, [In] FLOAT y);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ScaleTransform([In] HANDLE context, [In] FLOAT sx, [In] FLOAT sy, [Optional] D2DPoint center);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SkewTransform([In] HANDLE ctx, [In] FLOAT angleX, [In] FLOAT angleY, [Optional] D2DPoint center);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetTransform([In] HANDLE context, [In] ref Matrix3x2 mat);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void GetTransform([In] HANDLE context, [Out] out Matrix3x2 mat);
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ResetTransform([In] HANDLE context);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ReleaseObject([In] HANDLE objectHandle);
		#endregion // Device Context

		#region Simple Sharp

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawLine(HANDLE context, D2DPoint start, D2DPoint end, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid,
			D2DCapStyle startCap = D2DCapStyle.Flat, D2DCapStyle endCap = D2DCapStyle.Flat);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawLines(HANDLE context, D2DPoint[] points, UINT count, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawUnconnectedLines(HANDLE context, D2DPoint[] points, UINT count, D2DColor color, FLOAT width);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawRectangle(HANDLE context, ref D2DRect rect, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawRectangleWithPen(HANDLE context, ref D2DRect rect, HANDLE strokePen, FLOAT weight = 1);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillRectangle(HANDLE context, ref D2DRect rect, D2DColor color);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillRectangleWithBrush(HANDLE context, ref D2DRect rect, HANDLE brush);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawFillRectangle(HANDLE context, ref D2DRect rect, HANDLE fillBrush, HANDLE strokePen, FLOAT width = 1);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawRoundedRect(HANDLE ctx, ref D2DRoundedRect roundedRect,
			D2DColor strokeColor, D2DColor fillColor,
			FLOAT strokeWidth = 1, D2DDashStyle strokeStyle = D2DDashStyle.Solid);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawRoundedRectWithBrush(HANDLE ctx, ref D2DRoundedRect roundedRect,
			HANDLE strokePen, HANDLE fillBrush, float strokeWidth = 1);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawEllipse(HANDLE context, ref D2DEllipse rect, D2DColor color,
			FLOAT width = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillEllipse(HANDLE context, ref D2DEllipse rect, D2DColor color);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillEllipseWithBrush(HANDLE ctx, ref D2DEllipse ellipse, HANDLE brush);

		#endregion // Simple Sharp

		#region Text

		[SuppressGCTransition]
		[DllImport(DLL_NAME, EntryPoint = "DrawString", CharSet = CharSet.Unicode,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawText([In] HANDLE context, [In] string text, [In] D2DColor color,
			[In] string fontName, [In] FLOAT fontSize, [In] D2DRect rect,
			[In] D2DFontWeight fontWeight = D2DFontWeight.Normal,
			[In] D2DFontStyle fontStyle = D2DFontStyle.Normal,
			[In] D2DFontStretch fontStretch = D2DFontStretch.Normal,
			[In] DWriteTextAlignment halign = DWriteTextAlignment.Leading,
			[In] DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, EntryPoint = "MeasureText", CharSet = CharSet.Unicode,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern void MeasureText([In] HANDLE ctx, [In] string text, [In] string fontName,
			[In] FLOAT fontSize, ref D2DSize size,
			[In] D2DFontWeight fontWeight = D2DFontWeight.Normal,
			[In] D2DFontStyle fontStyle = D2DFontStyle.Normal,
			[In] D2DFontStretch fontStretch = D2DFontStretch.Normal,
			[In] DWriteTextAlignment halign = DWriteTextAlignment.Leading,
			[In] DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static extern void MeasureTextWithFormat([In] HANDLE ctx, [In] string text, [In] HANDLE textFormat, ref D2DSize size);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, EntryPoint = "MeasureTextWithLayout", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern void MeasureTextWithLayout([In] HANDLE ctx, [In] HANDLE textLayout, ref D2DSize size);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateFontFace([In] HANDLE context, [In] string fontName,
		[In] D2DFontWeight fontWeight = D2DFontWeight.Normal,
			[In] D2DFontStyle fontStyle = D2DFontStyle.Normal,
			[In] D2DFontStretch fontStretch = D2DFontStretch.Normal);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyFontFace(HANDLE fontFaceHandle);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateTextPathGeometry(HANDLE ctx, [In] string text,
			HANDLE fontFaceHandle, FLOAT fontSize);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, EntryPoint = "CreateTextFormat", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateTextFormat([In] HANDLE ctx, [In] string fontName, [In] FLOAT fontSize,
			[In] D2DFontWeight fontWeight = D2DFontWeight.Normal,
			[In] D2DFontStyle fontStyle = D2DFontStyle.Normal,
			[In] D2DFontStretch fontStretch = D2DFontStretch.Normal,
			[In] DWriteTextAlignment hAlign = DWriteTextAlignment.Leading,
			[In] DWriteParagraphAlignment vAlign = DWriteParagraphAlignment.Near);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, EntryPoint = "CreateTextLayoutWithFormat", CharSet = CharSet.Unicode)]
		public static extern HANDLE CreateTextLayout([In] HANDLE ctx, [In] string text, [In] HANDLE textFormatHandler, [In] ref D2DSize size);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, EntryPoint = "CreateTextLayout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern HANDLE CreateTextLayout([In] HANDLE ctx, [In] string text, [In] HANDLE textFormatHandler, [In] ref D2DSize size,
			[In] D2DFontWeight fontWeight = D2DFontWeight.Normal,
			[In] D2DFontStyle fontStyle = D2DFontStyle.Normal,
			[In] D2DFontStretch fontStretch = D2DFontStretch.Normal);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static extern void DrawStringWithBrushAndTextFormat([In] HANDLE context, [In] string text, [In] HANDLE brush,
			[In] HANDLE textFormat, [In] ref D2DRect rect);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CharSet = CharSet.Unicode)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static extern void DrawStringWithLayout([In] HANDLE context, [In] HANDLE brush,
			[In] HANDLE textLayout, [In] D2DPoint origin);

		#endregion // Text

		#region Geometry

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateRectangleGeometry([In] HANDLE ctx, [In] ref D2DRect rect);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyGeometry(HANDLE geometryContext);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateEllipseGeometry(HANDLE ctx, ref D2DEllipse ellipse);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreatePathGeometry(HANDLE ctx);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateCombinedGeometry(HANDLE d2dCtx, HANDLE pathCtx1, HANDLE pathCtx2,
			D2D1CombineMode combineMode, FLOAT flatteningTolerance = 10f);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyPathGeometry(HANDLE ctx);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillGeometryWithBrush([In] HANDLE ctx, [In] HANDLE geoHandle,
			[In] HANDLE brushHandle, [Optional] HANDLE opacityBrushHandle);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawPolygon(HANDLE ctx, D2DPoint[] points, UINT count,
			D2DColor strokeColor, FLOAT strokeWidth, D2DDashStyle dashStyle, D2DColor fillColor);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawPolygonWithBrush(HANDLE ctx, D2DPoint[] points, UINT count,
			D2DColor strokeColor, FLOAT strokeWidth, D2DDashStyle dashStyle, HANDLE brushHandler);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetPathStartPoint(HANDLE ctx, D2DPoint startPoint);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ClosePath(HANDLE ctx);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathLines(HANDLE path, D2DPoint[] points, uint count);
		public static void AddPathLines(HANDLE path, D2DPoint[] points) { AddPathLines(path, points, (uint)points.Length); }

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathBeziers(HANDLE ctx, D2DBezierSegment[] bezierSegments, uint count);

		public static void AddPathBeziers(HANDLE ctx, D2DBezierSegment[] bezierSegments)
		{
			AddPathBeziers(ctx, bezierSegments, (uint)bezierSegments.Length);
		}

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathEllipse(HANDLE path, ref D2DEllipse ellipse);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathArc(HANDLE ctx, D2DPoint endPoint, D2DSize size, FLOAT sweepAngle,
			D2DArcSize arcSize = D2DArcSize.Small,
			D2DSweepDirection sweepDirection = D2DSweepDirection.Clockwise);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawBeziers(HANDLE ctx, D2DBezierSegment[] bezierSegments, UINT count,
															D2DColor strokeColor, FLOAT strokeWidth = 1,
															D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawPath(HANDLE path, D2DColor strokeColor, FLOAT strokeWidth = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawPathWithPen(HANDLE path, HANDLE pen, FLOAT strokeWidth = 1);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillPathD(HANDLE path, D2DColor fillColor);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillGeometryWithBrush(HANDLE path, HANDLE brush);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool PathFillContainsPoint(HANDLE pathCtx, D2DPoint point);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool PathStrokeContainsPoint(HANDLE pathCtx, D2DPoint point, FLOAT strokeWidth = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void GetGeometryBounds(HANDLE pathCtx, ref D2DRect rect);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void GetGeometryTransformedBounds(HANDLE pathCtx, ref Matrix3x2 mat3x2, ref D2DRect rect);

		#endregion // Geometry

		#region Style
		[SuppressGCTransition]
		[DllImport(DLL_NAME, EntryPoint = "CreateStrokeStyle", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateStrokeStyle(HANDLE ctx, FLOAT[]? dashes = null, UINT dashCount = 0, FLOAT dashOffset = 0.0f,
				D2DCapStyle startCap = D2DCapStyle.Flat, D2DCapStyle endCap = D2DCapStyle.Flat);
		#endregion Style

		#region Pen
		[SuppressGCTransition]
		[DllImport(DLL_NAME, EntryPoint = "CreatePenStroke", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreatePen(HANDLE ctx, D2DColor strokeColor, D2DDashStyle dashStyle = D2DDashStyle.Solid,
	FLOAT[]? dashes = null, UINT dashCount = 0, FLOAT dashOffset = 0.0f);

		[DllImport(DLL_NAME, EntryPoint = "DestroyPenStroke", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestroyPen(HANDLE pen);
		#endregion Pen

		#region Brush

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateSolidColorBrush(HANDLE ctx, D2DColor color);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetSolidColorBrushColor(HANDLE brush, D2DColor color);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateLinearGradientBrush(HANDLE ctx, D2DPoint startPoint, D2DPoint endPoint,
																											D2DGradientStop[] gradientStops, UINT gradientStopCount);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateRadialGradientBrush(HANDLE ctx, D2DPoint origin, D2DPoint offset,
																													FLOAT radiusX, FLOAT radiusY, D2DGradientStop[] gradientStops,
								      																					UINT gradientStopCount);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapBrush(HANDLE ctx, HANDLE bitmap,
																									D2DExtendMode extendModeX, D2DExtendMode extendModeY,
																									D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ReleaseBrush(HANDLE brushCtx);

		#endregion // Brush

		#region Bitmap
		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapFromHBitmap(HANDLE context, HANDLE hBitmap, bool useAlphaChannel);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapFromBytes(HANDLE context, byte[] buffer, UINT offset, UINT length);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapFromMemory(HANDLE ctx, UINT width, UINT height, UINT stride, IntPtr buffer,
			UINT offset, UINT length);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CharSet = CharSet.Unicode)]
		public static extern HANDLE CreateBitmapFromFile(HANDLE context, string filepath);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawD2DBitmap(HANDLE context, HANDLE bitmap);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawD2DBitmap(HANDLE context, HANDLE bitmap, ref D2DRect destRect);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawD2DBitmap(HANDLE context, HANDLE bitmap, ref D2DRect destRect, ref D2DRect srcRect,
			FLOAT opacity = 1, D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawGDIBitmap(HANDLE context, HANDLE bitmap, FLOAT opacity = 1,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawGDIBitmapRect(HANDLE context, HANDLE bitmap,
			ref D2DRect destRect, ref D2DRect srcRect, FLOAT opacity = 1, bool alpha = false,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern D2DSize GetBitmapSize(HANDLE d2dbitmap);
		#endregion

		[SuppressGCTransition]
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void TestDraw(HANDLE ctx);
	}
}
