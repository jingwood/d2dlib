/*
 * MIT License
 * 
 * Copyright (c) 2009-2018 Jingwood, unvell.com. All right reserved.
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
 
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using FLOAT = System.Single;
using UINT = System.UInt32;
using UINT32 = System.UInt32;
using HWND = System.IntPtr;
using HANDLE = System.IntPtr;
using HRESULT = System.Int64;
using BOOL = System.Int32;

namespace unvell.D2DLib
{
	internal static class D2D
	{
		#region Context

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE GetLastResult();

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateContext([In] HANDLE hwnd);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestoryContext([In] HANDLE context);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetContextProperties([In] HANDLE context, D2DAntialiasMode antialiasMode = D2DAntialiasMode.PerPrimitive);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE BeginRender([In] HANDLE context);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE BeginRenderWithBackgroundColor([In] HANDLE context, D2DColor backColor);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE BeginRenderWithBackgroundBitmap(HANDLE context, HANDLE bitmap);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void EndRender([In] HANDLE context);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void Flush([In] HANDLE context);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void Clear([In] HANDLE context, D2DColor color);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapRenderTarget([In] HANDLE context, D2DSize size);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawBitmapRenderTarget([In] HANDLE context, HANDLE bitmapRenderTarget, ref D2DRect rect,
			FLOAT opacity = 1, D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE GetBitmapRenderTargetBitmap(HANDLE bitmapRenderTarget);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestoryBitmapRenderTarget([In] HANDLE context);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE ResizeContext([In] HANDLE context);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void PushClip([In] HANDLE context, [In] ref D2DRect rect,
			D2DAntialiasMode antiAliasMode = D2DAntialiasMode.PerPrimitive);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void PopClip([In] HANDLE context);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void PushTransform([In] HANDLE context);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void PopTransform([In] HANDLE context);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void RotateTransform([In] HANDLE context, [In] FLOAT angle);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void RotateTransform([In] HANDLE context, [In] FLOAT angle, [In] D2DPoint center);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void TranslateTransform([In] HANDLE context, [In] FLOAT x, [In] FLOAT y);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ScaleTransform([In] HANDLE context, [In] FLOAT sx, [In] FLOAT sy, [Optional] D2DPoint center);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SkewTransform([In] HANDLE ctx, [In] FLOAT angleX, [In] FLOAT angleY, [Optional] D2DPoint center);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetTransform([In] HANDLE context, [In] FLOAT angle, [In] D2DPoint center);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ResetTransform([In] HANDLE context);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ReleaseObject([In] HANDLE objectHandle);

		#endregion

		#region Simple Sharp

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawLine(HANDLE context, D2DPoint start, D2DPoint end, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawLines(HANDLE context, D2DPoint[] points, UINT count, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawRectangle(HANDLE context, ref D2DRect rect, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillRectangle(HANDLE context, ref D2DRect rect, D2DColor color);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawEllipse(HANDLE context, ref D2DEllipse rect, D2DColor color,
			FLOAT width = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillEllipse(HANDLE context, ref D2DEllipse rect, D2DColor color);


		#endregion // Simple Sharp

		#region Text

		[DllImport("d2dlib.dll", EntryPoint = "DrawString", CharSet = CharSet.Unicode)]
		public static extern void DrawText([In] HANDLE context, [In] string text, [In] D2DColor color,
			[In] string fontName, [In] FLOAT fontSize, [In] ref D2DRect rect,
			[In] DWRITE_TEXT_ALIGNMENT halign = DWRITE_TEXT_ALIGNMENT.DWRITE_TEXT_ALIGNMENT_LEADING,
			[In] DWRITE_PARAGRAPH_ALIGNMENT valign = DWRITE_PARAGRAPH_ALIGNMENT.DWRITE_PARAGRAPH_ALIGNMENT_NEAR);

		#endregion

		#region Geometry
		
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateRectangleGeometry([In] HANDLE ctx, [In] ref D2DRect rect);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillGeometryWithBrush([In] HANDLE ctx, [In] HANDLE geoHandle, 
			[In] HANDLE brushHandle, [Optional] HANDLE opacityBrushHandle);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawPolygon(HANDLE ctx, D2DPoint[] points, UINT count,
			D2DColor strokeColor, FLOAT strokeWidth, D2DDashStyle dashStyle, D2DColor fillColor);
		#endregion // Geometry

		#region Path
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreatePathGeometry(HANDLE ctx);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DestoryPathGeometry(HANDLE ctx);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ClosePath(HANDLE ctx);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathLines(HANDLE path, D2DPoint[] points, uint count);
		public static void AddPathLines(HANDLE path, D2DPoint[] points) { AddPathLines(path, points, (uint)points.Length); }
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathBeziers(HANDLE ctx, D2DBezierSegment[] bezierSegments, uint count);
		public static void AddPathBeziers(HANDLE ctx, D2DBezierSegment[] bezierSegments)
			{ AddPathBeziers(ctx, bezierSegments, (uint)bezierSegments.Length); }
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathEllipse(HANDLE path, ref D2DEllipse ellipse);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void AddPathArc(HANDLE ctx, D2DSize size, D2DPoint endPoint, FLOAT sweepAngle,
		D2D1_SWEEP_DIRECTION sweepDirection = D2D1_SWEEP_DIRECTION.D2D1_SWEEP_DIRECTION_CLOCKWISE);
			
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawBeziers(HANDLE ctx, D2DBezierSegment[] bezierSegments, UINT count, 
															D2DColor strokeColor, FLOAT strokeWidth = 1, 
															D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawPath(HANDLE path, D2DColor strokeColor, FLOAT strokeWidth = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillPathD(HANDLE path, D2DColor fillColor);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillGeometryWithBrush(HANDLE path, HANDLE brush);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool PathFillContainsPoint(HANDLE pathCtx, D2DPoint point);
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool PathStrokeContainsPoint(HANDLE pathCtx, D2DPoint point, FLOAT strokeWidth = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid);
		#endregion

		#region Pen
		[DllImport("d2dlib.dll", EntryPoint = "CreatePenStroke")]
		public static extern HANDLE CreatePen(HANDLE ctx, D2DColor strokeColor, D2DDashStyle dashStyle = D2DDashStyle.Solid);

		[DllImport("d2dlib.dll", EntryPoint = "DestoryPenStroke")]
		public static extern void DestoryPen(HANDLE pen);
		#endregion Pen

		#region Brush

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateSolidColorBrush(HANDLE ctx, D2DColor color);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetSolidColorBrushColor(HANDLE brush, D2DColor color);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateRadialGradientBrush(HANDLE ctx, D2DPoint origin, D2DPoint offset,
																												  FLOAT radiusX, FLOAT radiusY, D2DGradientStop[] gradientStops, 
																													UINT gradientStopCount);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void FillEllipseWithBrush(HANDLE ctx, ref D2DEllipse ellipse, HANDLE brush);

		#endregion // Brush

		#region Bitmap
		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapFromHBitmap(HANDLE context, HANDLE hBitmap, bool useAlphaChannel);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapFromBytes(HANDLE context, byte[] buffer, UINT offset, UINT length);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern HANDLE CreateBitmapFromMemory(HANDLE ctx, UINT width, UINT height, UINT stride, IntPtr buffer,
			UINT offset, UINT length);

		[DllImport("d2dlib.dll", CharSet = CharSet.Unicode)]
		public static extern HANDLE CreateBitmapFromFile(HANDLE context, string filepath);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawD2DBitmap(HANDLE context, HANDLE bitmap);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawD2DBitmap(HANDLE context, HANDLE bitmap, ref D2DRect destRect);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawD2DBitmap(HANDLE context, HANDLE bitmap, ref D2DRect destRect, ref D2DRect srcRect,
			FLOAT opacity = 1, D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawGDIBitmap(HANDLE context, HANDLE bitmap, FLOAT opacity = 1,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void DrawGDIBitmapRect(HANDLE context, HANDLE bitmap,
			ref D2DRect destRect, ref D2DRect srcRect, FLOAT opacity = 1, bool alpha = false,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear);

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern D2DSize GetBitmapSize(HANDLE d2dbitmap);
		#endregion

		[DllImport("d2dlib.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void TestDraw(HANDLE ctx);
	}

	public class D2DDevice: IDisposable
	{
		internal HANDLE Handle { get; private set; }

		internal D2DDevice(HANDLE deviceHandle)
		{
			this.Handle = deviceHandle;
		}

		public static D2DDevice FromHwnd(HWND hwnd)
		{
			HANDLE contextHandle = D2D.CreateContext(hwnd);
			return new D2DDevice(contextHandle);
		}

		public void Resize()
		{
			if (Handle != HANDLE.Zero) D2D.ResizeContext(this.Handle);
		}

		public D2DPen CreatePen(D2DColor color, D2DDashStyle dashStyle)
		{
			HANDLE handle = D2D.CreatePen(this.Handle, color, dashStyle);
			return handle == HANDLE.Zero ? null : new D2DPen(handle, color, dashStyle);
		}

		public D2DSolidColorBrush CreateSolidColorBrush(D2DColor color)
		{
			HANDLE handle = D2D.CreateSolidColorBrush(this.Handle, color);
			return handle == HANDLE.Zero ? null : new D2DSolidColorBrush(handle, color);
		}

		public D2DRadialGradientBrush CreateRadialGradientBrush(D2DPoint origin, D2DPoint offset,
																														FLOAT radiusX, FLOAT radiusY,
																														D2DGradientStop[] gradientStops)
		{
			HANDLE handle = D2D.CreateRadialGradientBrush(this.Handle, origin, offset, radiusX, radiusY, 
				gradientStops, (uint)gradientStops.Length);

			return new D2DRadialGradientBrush(handle, gradientStops);
		}

		public D2DRectangleGeometry CreateRectangleGeometry(FLOAT width, FLOAT height)
		{
			var rect = new D2DRect(0, 0, width, height);
			return CreateRectangleGeometry(ref rect);
		}

		public D2DRectangleGeometry CreateRectangleGeometry(ref D2DRect rect)
		{
			HANDLE rectHandle = D2D.CreateRectangleGeometry(this.Handle, ref rect);
			return new D2DRectangleGeometry(this.Handle, rectHandle);
		}

		public D2DPathGeometry CreatePathGeometry()
		{
			HANDLE geoHandle = D2D.CreatePathGeometry(this.Handle);
			return new D2DPathGeometry(this.Handle, geoHandle);
		}

		public D2DBitmap LoadBitmap(byte[] buffer)
		{
			return this.LoadBitmap(buffer, 0, (uint)buffer.Length);
		}

		public D2DBitmap LoadBitmap(byte[] buffer, UINT offset, UINT length)
		{
			var bitmapHandle = D2D.CreateBitmapFromBytes(this.Handle, buffer, offset, length);
			return (bitmapHandle != HWND.Zero) ? new D2DBitmap(bitmapHandle) : null;
		}

		public D2DBitmap LoadBitmap(string filepath)
		{
			var bitmapHandle = D2D.CreateBitmapFromFile(this.Handle, filepath);
			return (bitmapHandle != HWND.Zero) ? new D2DBitmap(bitmapHandle) : null;
		}

		public D2DBitmap CreateBitmapFromMemory(UINT width, UINT height, UINT stride, IntPtr buffer, UINT offset, UINT length)
		{
			HANDLE d2dbmp = D2D.CreateBitmapFromMemory(this.Handle, width, height, stride, buffer, offset, length);
			return d2dbmp == HANDLE.Zero ? null : new D2DBitmap(d2dbmp);
		}

		public D2DBitmap CreateBitmapFromGDIBitmap(System.Drawing.Bitmap bmp)
		{
			bool useAlphaChannel =
				(bmp.PixelFormat & System.Drawing.Imaging.PixelFormat.Alpha) == System.Drawing.Imaging.PixelFormat.Alpha;

			return this.CreateBitmapFromGDIBitmap(bmp, useAlphaChannel);
		}

		public D2DBitmap CreateBitmapFromGDIBitmap(System.Drawing.Bitmap bmp, bool useAlphaChannel)
		{
			HANDLE d2dbmp = D2D.CreateBitmapFromHBitmap(this.Handle, bmp.GetHbitmap(), useAlphaChannel);
			return d2dbmp == HANDLE.Zero ? null : new D2DBitmap(d2dbmp);
		}

		public D2DBitmapGraphics CreateBitmapGraphics()
		{
			return CreateBitmapGraphics(D2DSize.Empty);
		}

		public D2DBitmapGraphics CreateBitmapGraphics(float width, float height)
		{
			return CreateBitmapGraphics(new D2DSize(width, height));
		}

		public D2DBitmapGraphics CreateBitmapGraphics(D2DSize size)
		{
			HANDLE bitmapRenderTargetHandle = D2D.CreateBitmapRenderTarget(this.Handle, size);
			return bitmapRenderTargetHandle == HANDLE.Zero ? null 
				: new D2DBitmapGraphics(bitmapRenderTargetHandle);
		}

		public void Dispose()
		{
			D2D.DestoryContext(this.Handle);
		}

	}

	public class D2DGraphics
	{
		internal HANDLE DeviceHandle { get; private set; }

		public D2DDevice Device { get; private set; }

		public D2DGraphics(D2DDevice context)
			: this(context.Handle)
		{
			this.Device = context;
		}

		public D2DGraphics(HANDLE device)
		{
			this.DeviceHandle = device;
		}

		public void BeginRender()
		{
			D2D.BeginRender(this.DeviceHandle);
		}

		public void BeginRender(D2DColor color)
		{
			D2D.BeginRenderWithBackgroundColor(this.DeviceHandle, color);
		}

		public void BeginRender(D2DBitmap bitmap)
		{
			D2D.BeginRenderWithBackgroundBitmap(this.DeviceHandle, bitmap.Handle);
		}

		public void EndRender()
		{
			D2D.EndRender(this.DeviceHandle);
		}

		public void Flush()
		{
			D2D.Flush(this.DeviceHandle);
		}

		private bool antialias = true;

		public bool Antialias
		{
			get { return this.antialias; }
			set
			{
				if (this.antialias != value)
				{
					D2D.SetContextProperties(this.DeviceHandle,
						value ? D2DAntialiasMode.PerPrimitive : D2DAntialiasMode.Aliased);

					this.antialias = value;
				}
			}
		}

		public void DrawLine(FLOAT x1, FLOAT y1, FLOAT x2, FLOAT y2, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			DrawLine(new D2DPoint(x1, y1), new D2DPoint(x2, y2), color, weight, dashStyle);
		}

		public void DrawLine(D2DPoint start, D2DPoint end, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2D.DrawLine(this.DeviceHandle, start, end, color, weight, dashStyle);
		}

		public void DrawLines(D2DPoint[] points, D2DColor color, FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2D.DrawLines(this.DeviceHandle, points, (uint)points.Length, color, weight, dashStyle);
		}

		public void DrawEllipse(FLOAT x, FLOAT y, FLOAT width, FLOAT height, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			var ellipse = new D2DEllipse(x, y, width / 2f, height / 2f);
			ellipse.origin.x += ellipse.radiusX;
			ellipse.origin.y += ellipse.radiusY;

			this.DrawEllipse(ellipse, color, weight, dashStyle);
		}

		public void DrawEllipse(D2DPoint origin, D2DSize radial, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			var ellipse = new D2DEllipse(origin, radial);
			this.DrawEllipse(ellipse, color, weight, dashStyle);
		}

		public void DrawEllipse(D2DPoint origin, FLOAT radialX, FLOAT radialY, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			var ellipse = new D2DEllipse(origin, radialX, radialY);
			this.DrawEllipse(ellipse, color, weight, dashStyle);
		}

		public void DrawEllipse(D2DEllipse ellipse, D2DColor color, FLOAT weight = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2D.DrawEllipse(this.DeviceHandle, ref ellipse, color, weight, dashStyle);
		}

		public void FillEllipse(D2DPoint p, FLOAT radial, D2DColor color)
		{
			this.FillEllipse(p, radial, radial, color);
		}

		public void FillEllipse(D2DPoint p, FLOAT w, FLOAT h, D2DColor color)
		{
			D2DEllipse ellipse = new D2DEllipse(p, w / 2, h / 2);
			ellipse.origin.x += ellipse.radiusX;
			ellipse.origin.y += ellipse.radiusY;

			this.FillEllipse(ellipse, color);
		}

		public void FillEllipse(FLOAT x, FLOAT y, FLOAT radial, D2DColor color)
		{
			this.FillEllipse(new D2DPoint(x, y), radial, radial, color);
		}

		public void FillEllipse(FLOAT x, FLOAT y, FLOAT w, FLOAT h, D2DColor color)
		{
			this.FillEllipse(new D2DPoint(x, y), w, h, color);
		}

		public void FillEllipse(D2DEllipse ellipse, D2DColor color)
		{
			D2D.FillEllipse(this.DeviceHandle, ref ellipse, color);
		}

		public void FillEllipse(D2DEllipse ellipse, D2DBrush brush)
		{
			D2D.FillEllipseWithBrush(this.DeviceHandle, ref ellipse, brush.Handle);
		}

		public void DrawBeziers(D2DBezierSegment[] bezierSegments,
														D2DColor strokeColor, FLOAT strokeWidth = 1,
														D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2D.DrawBeziers(DeviceHandle, bezierSegments, (uint)bezierSegments.Length, strokeColor, strokeWidth, dashStyle);
		}

		public void DrawPolygon(D2DPoint[] points, UINT count,
			D2DColor strokeColor, FLOAT strokeWidth  = 1f, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			this.DrawPolygon(points, count, D2DColor.Transparent, 0, D2DDashStyle.Solid, D2DColor.Transparent);
		}

		public void FillPolygon(D2DPoint[] points, UINT count, D2DColor fillColor)
		{
			this.DrawPolygon(points, count, D2DColor.Transparent, 0, D2DDashStyle.Solid, fillColor);
		}
		
		public void DrawPolygon(D2DPoint[] points, UINT count,
			D2DColor strokeColor, FLOAT strokeWidth, D2DDashStyle dashStyle, D2DColor fillColor)
		{
			D2D.DrawPolygon(DeviceHandle, points, count, strokeColor, strokeWidth, dashStyle, fillColor);
		}

		public void TestDraw()
		{
			D2D.TestDraw(this.DeviceHandle);
		}

		public void PushClip(D2DRect rect)
		{
			D2D.PushClip(this.DeviceHandle, ref rect);
		}

		public void PopClip()
		{
			D2D.PopClip(this.DeviceHandle);
		}

		public void PushTransform()
		{
			D2D.PushTransform(this.DeviceHandle);
		}

		public void PopTransform()
		{
			D2D.PopTransform(this.DeviceHandle);
		}

		public void ResetTransform()
		{
			D2D.ResetTransform(this.DeviceHandle);
		}

		public void RotateTransform(FLOAT angle)
		{
			D2D.RotateTransform(this.DeviceHandle, angle);
		}

		public void RotateTransform(FLOAT angle, D2DPoint center)
		{
			D2D.RotateTransform(this.DeviceHandle, angle, center);
		}

		public void TranslateTransform(FLOAT x, FLOAT y)
		{
			D2D.TranslateTransform(this.DeviceHandle, x, y);
		}

		public void ScaleTransform(FLOAT sx, FLOAT sy, [Optional] D2DPoint center)
		{
			D2D.ScaleTransform(this.DeviceHandle, sx, sy, center);
		}

		public void SkewTransform(FLOAT angleX, FLOAT angleY, [Optional] D2DPoint center)
		{
			D2D.SkewTransform(this.DeviceHandle, angleX, angleY, center);
		}

		public void DrawRectangle(FLOAT x, FLOAT y, FLOAT w, FLOAT h, D2DColor color, FLOAT strokeWidth = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2DRect rect = new D2DRect(x, y, w, h);
			D2D.DrawRectangle(this.DeviceHandle, ref rect, color, strokeWidth, dashStyle);
		}

		public void DrawRectangle(D2DRect rect, D2DColor color, FLOAT strokeWidth = 1, 
			D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2D.DrawRectangle(this.DeviceHandle, ref rect, color, strokeWidth, dashStyle);
		}

		public void DrawRectangle(D2DPoint origin, D2DSize size, D2DColor color, FLOAT strokeWidth = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			this.DrawRectangle(new D2DRect(origin, size), color, strokeWidth, dashStyle);
		}

		public void FillRectangle(float x, float y, float width, float height, D2DColor color)
		{
			var rect = new D2DRect(x, y, width, height);
			this.FillRectangle(rect, color);
		}

		public void FillRectangle(D2DPoint origin, D2DSize size, D2DColor color)
		{
			this.FillRectangle(new D2DRect(origin, size), color);
		}

		public void FillRectangle(D2DRect rect, D2DColor color)
		{
			D2D.FillRectangle(this.DeviceHandle, ref rect, color);
		}

		public void DrawBitmap(D2DBitmap bitmap, D2DRect destRect, FLOAT opacity = 1,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			var srcRect = new D2DRect(0, 0, bitmap.Width, bitmap.Height);
			this.DrawBitmap(bitmap, destRect, srcRect, opacity, interpolationMode);
		}

		public void DrawBitmap(D2DBitmap bitmap, D2DRect destRect, D2DRect srcRect, FLOAT opacity = 1,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			D2D.DrawD2DBitmap(this.DeviceHandle, bitmap.Handle, ref destRect, ref srcRect, opacity, interpolationMode);
		}

		public void DrawBitmap(D2DBitmapGraphics bg, D2DRect rect, FLOAT opacity = 1,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			D2D.DrawBitmapRenderTarget(this.DeviceHandle, bg.DeviceHandle, ref rect, opacity, interpolationMode);
		}

		public void DrawBitmap(D2DBitmapGraphics bg, FLOAT width, FLOAT height, FLOAT opacity = 1,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			this.DrawBitmap(bg, new D2DRect(0, 0, width, height), opacity, interpolationMode);
		}

		public void DrawGDIBitmap(HANDLE hbitmap, D2DRect rect, D2DRect srcRect, FLOAT opacity = 1, bool alpha = false,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			D2D.DrawGDIBitmapRect(this.DeviceHandle, hbitmap, ref rect, ref srcRect, opacity, alpha, interpolationMode);
		}

		public void DrawTextCenter(string text, D2DColor color, string fontName, float fontSize, D2DRect rect)
		{
			this.DrawText(text, color, fontName, fontSize, rect, 
				DWRITE_TEXT_ALIGNMENT.DWRITE_TEXT_ALIGNMENT_CENTER, DWRITE_PARAGRAPH_ALIGNMENT.DWRITE_PARAGRAPH_ALIGNMENT_CENTER);
		}

		public void DrawText(string text, D2DColor color, string fontName, float fontSize, D2DRect rect, 
			DWRITE_TEXT_ALIGNMENT halign = DWRITE_TEXT_ALIGNMENT.DWRITE_TEXT_ALIGNMENT_LEADING,
			DWRITE_PARAGRAPH_ALIGNMENT valign = DWRITE_PARAGRAPH_ALIGNMENT.DWRITE_PARAGRAPH_ALIGNMENT_NEAR)
		{
			D2D.DrawText(this.DeviceHandle, text, color, fontName, fontSize, ref rect, halign, valign);
		}

		public void DrawPath(D2DPathGeometry path, D2DColor strokeColor, 
			FLOAT strokeWidth = 1f, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2D.DrawPath(path.Handle, strokeColor, strokeWidth, dashStyle);
		}

		public void FillPath(D2DPathGeometry path, D2DColor fillColor)
		{
			D2D.FillPathD(path.Handle, fillColor);
		}
	}

	public class D2DBitmapGraphics : D2DGraphics, IDisposable
	{
		internal D2DBitmapGraphics(HANDLE handle)
			: base(handle)
		{
		}

		public D2DBitmap GetBitmap()
		{
			HANDLE bitmapHandle = D2D.GetBitmapRenderTargetBitmap(this.DeviceHandle);
			return bitmapHandle == HANDLE.Zero ? null : new D2DBitmap(bitmapHandle);
		}

		public void Dispose()
		{
			D2D.DestoryBitmapRenderTarget(this.DeviceHandle);
		}
	}

	public class D2DObject : IDisposable
	{
		internal HANDLE Handle { get; private set; }

		public D2DObject(HANDLE handle)
		{
			this.Handle = handle;
		}

		public virtual void Dispose()
		{
			if (this.Handle != IntPtr.Zero) D2D.ReleaseObject(this.Handle);
		}
	}

	public class D2DPen : D2DObject
	{
		private D2DColor color;

		public D2DColor Color { get { return this.color; } }

		private D2DDashStyle dashStyle;

		public D2DDashStyle DashStyle { get { return this.dashStyle; } }

		public D2DPen(HANDLE handle, D2DColor color, D2DDashStyle dashStyle)
			: base(handle)
		{
			this.color = color;
			this.dashStyle = dashStyle;
		}
	}

	public abstract class D2DBrush : D2DObject
	{
		internal D2DBrush(HANDLE handle) : base(handle) { }
	}

	public class D2DSolidColorBrush : D2DBrush
	{
		private D2DColor color;

		public D2DColor Color
		{
			get
			{
				return color;
			}
			set
			{
				D2D.SetSolidColorBrushColor(this.Handle, value);
			}
		}

		internal D2DSolidColorBrush(HANDLE handle, D2DColor color)
			: base(handle)
		{
			this.color = color;
		}
	}

	public class D2DRadialGradientBrush : D2DBrush
	{
		public D2DGradientStop[] GradientStops { get; private set; }

		internal D2DRadialGradientBrush(HANDLE handle, D2DGradientStop[] gradientStops)
			: base(handle)
		{
			this.GradientStops = gradientStops;
		}
	}

	public class D2DGeometry : D2DObject
	{
		internal HANDLE DeviceHandle { get; private set; }

		internal D2DGeometry(HANDLE deviceHandle, HANDLE geoHandle)
			: base(geoHandle)
		{
			this.DeviceHandle = deviceHandle;
		}

		public void FillGeometry(D2DBrush brush, [Optional] D2DBrush opacityBrush)
		{

		}
	}

	public class D2DRectangleGeometry : D2DGeometry
	{
		internal D2DRectangleGeometry(HANDLE deviceHandle, HANDLE geoHandle)
			: base(deviceHandle, geoHandle)
		{
		}
	}

	public class D2DPathGeometry : D2DGeometry
	{
		internal D2DPathGeometry(HANDLE deviceHandle, HANDLE pathHandle)
			: base(deviceHandle, pathHandle)
		{
		}	

		public void AddLines(D2DPoint[] points)
		{
			D2D.AddPathLines(this.Handle, points);
		}

		public void AddBeziers(D2DBezierSegment[] bezierSegments)
		{
			D2D.AddPathBeziers(this.Handle, bezierSegments);
		}

		public void AddEllipse(D2DEllipse ellipse)
		{
			D2D.AddPathEllipse(this.Handle, ref ellipse);
		}

		public void AddArc(D2DSize size, D2DPoint	endPoint, FLOAT sweepAngle, 
			D2D1_SWEEP_DIRECTION sweepDirection = D2D1_SWEEP_DIRECTION.D2D1_SWEEP_DIRECTION_CLOCKWISE)
		{
			D2D.AddPathArc(this.Handle, size, endPoint, sweepAngle);
		}

		public bool FillContainsPoint(D2DPoint point)
		{
			return D2D.PathFillContainsPoint(this.Handle, point);
		}

		public bool StrokeContainsPoint(D2DPoint point, FLOAT width = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			return D2D.PathStrokeContainsPoint(this.Handle, point, width, dashStyle);
		}

		public void ClosePath()
		{
			D2D.ClosePath(this.Handle);
		}
		
		public override void Dispose()
		{
			D2D.DestoryPathGeometry(this.Handle);
		}
	}

	public class D2DBitmap : D2DObject
	{
		public D2DSize Size { get; private set; }

		public FLOAT Height { get { return this.Size.height; } }
		public FLOAT Width { get { return this.Size.width; } }

		internal D2DBitmap(HANDLE handle)
			: base(handle)
		{
			this.Size = D2D.GetBitmapSize(handle);
		}
	}
}
