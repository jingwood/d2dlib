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

using System.Runtime.InteropServices;

namespace unvell.D2DLib
{
	public class D2DDevice : IDisposable
	{
		internal HANDLE Handle { get; private set; }

		internal D2DDevice(HANDLE deviceHandle)
		{
			this.Handle = deviceHandle;
		}

		public static D2DDevice FromHwnd(HWND hwnd, D2D1PresentOptions presentOptions = D2D1PresentOptions.None)
		{
			HANDLE contextHandle = D2D.CreateContext(hwnd, presentOptions);
			return new D2DDevice(contextHandle);
		}

		public void Resize()
		{
			if (Handle != HANDLE.Zero) D2D.ResizeContext(this.Handle);
		}

		public D2DStrokeStyle? CreateStrokeStyle(float[]? dashes = null, float dashOffset = 0.0f,
			D2DCapStyle startCap = D2DCapStyle.Flat, D2DCapStyle endCap = D2DCapStyle.Flat)
		{
			HANDLE handle = D2D.CreateStrokeStyle(this.Handle, dashes, dashes != null ? (uint)dashes.Length : 0, dashOffset, startCap, endCap);

			return handle == HANDLE.Zero ? null : new D2DStrokeStyle(this, handle, dashes, dashOffset, startCap, endCap);
		}

		public D2DPen? CreatePen(D2DColor color, D2DDashStyle dashStyle = D2DDashStyle.Solid,
			float[]? customDashes = null, float dashOffset = 0.0f)
		{
			HANDLE handle = D2D.CreatePen(this.Handle, color, dashStyle,
			customDashes, customDashes != null ? (uint)customDashes.Length : 0, dashOffset);

			return handle == HANDLE.Zero ? null : new D2DPen(this, handle, color, dashStyle, customDashes, dashOffset);
		}

		internal void DestroyPen(D2DPen pen)
		{
			if (pen == null) throw new ArgumentNullException(nameof(pen));
			D2D.DestroyPen(pen.Handle);
		}

		public D2DSolidColorBrush? CreateSolidColorBrush(D2DColor color)
		{
			HANDLE handle = D2D.CreateSolidColorBrush(this.Handle, color);
			return handle == HANDLE.Zero ? null : new D2DSolidColorBrush(handle, color);
		}

		public D2DLinearGradientBrush CreateLinearGradientBrush(D2DPoint startPoint, D2DPoint endPoint,
			D2DGradientStop[] gradientStops)
		{
			HANDLE handle = D2D.CreateLinearGradientBrush(this.Handle, startPoint, endPoint, gradientStops, (uint)gradientStops.Length);
			return new D2DLinearGradientBrush(handle, gradientStops);
		}

		public D2DRadialGradientBrush CreateRadialGradientBrush(D2DPoint origin, D2DPoint offset,
			FLOAT radiusX, FLOAT radiusY,
			D2DGradientStop[] gradientStops)
		{
			HANDLE handle = D2D.CreateRadialGradientBrush(this.Handle, origin, offset, radiusX, radiusY,
				gradientStops, (uint)gradientStops.Length);

			return new D2DRadialGradientBrush(handle, gradientStops);
		}
  
		public D2DBitmapBrush CreateBitmapBrush(D2DBitmap bitmap,
												D2DExtendMode extendModeX, D2DExtendMode extendModeY,
												D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			HANDLE handle = D2D.CreateBitmapBrush(this.Handle, bitmap.Handle, extendModeX, extendModeY, interpolationMode);
			return new D2DBitmapBrush(handle, bitmap);
		}
  
		public D2DRectangleGeometry CreateRectangleGeometry(FLOAT width, FLOAT height)
		{
			var rect = new D2DRect(0, 0, width, height);
			return CreateRectangleGeometry(rect);
		}

		public D2DRectangleGeometry CreateRectangleGeometry(D2DRect rect)
		{
			HANDLE rectGeometryHandle = D2D.CreateRectangleGeometry(this.Handle, rect);
			return new D2DRectangleGeometry(this, rectGeometryHandle);
		}

		public D2DPathGeometry CreatePathGeometry()
		{
			HANDLE geoHandle = D2D.CreatePathGeometry(this.Handle);
			return new D2DPathGeometry(this, geoHandle);
		}

		public D2DPathGeometry CreateCombinedGeometry(D2DPathGeometry path1, D2DPathGeometry path2,
			D2D1CombineMode combineMode, FLOAT flatteningTolerance = 10f)
		{
			HANDLE geoHandle = D2D.CreateCombinedGeometry(this.Handle, path1.Handle, path2.Handle, combineMode, flatteningTolerance);
			return new D2DPathGeometry(this, geoHandle);
		}

		public D2DPathGeometry CreateCombinedGeometry(D2DGeometry path1, D2DGeometry path2,
			D2D1CombineMode combineMode, FLOAT flatteningTolerance = 10f)
		{
			HANDLE geoHandle = D2D.CreateCombinedGeometry(this.Handle, path1.Handle, path2.Handle, combineMode, flatteningTolerance);
			return new D2DPathGeometry(this, geoHandle);
		}

		public D2DGeometry CreateEllipseGeometry(D2DPoint origin, D2DSize size)
		{
			var ellipse = new D2DEllipse(origin, size);
			return new D2DGeometry(this, D2D.CreateEllipseGeometry(this.Handle, ref ellipse));
		}

		public D2DGeometry CreatePieGeometry(D2DPoint origin, D2DSize size, float startAngle, float endAngle)
		{
			var path = this.CreatePathGeometry();

			var halfSize = new D2DSize(size.width * 0.5f, size.height * 0.5f);

			var sangle = startAngle * Math.PI / 180f;
			var eangle = endAngle * Math.PI / 180f;
			var angleDiff = endAngle - startAngle;

			var startPoint = new D2DPoint((float)(origin.X + halfSize.width * Math.Cos(sangle)),
				(float)(origin.Y + halfSize.height * Math.Sin(sangle)));

			var endPoint = new D2DPoint((float)(origin.X + halfSize.width * Math.Cos(eangle)),
				(float)(origin.Y + halfSize.height * Math.Sin(eangle)));

			path.AddLines(new D2DPoint[] { origin, startPoint });

			path.AddArc(endPoint, halfSize, angleDiff,
				angleDiff > 180 ? D2DArcSize.Large : D2DArcSize.Small,
				D2DSweepDirection.Clockwise);

			path.ClosePath();

			return path;
		}

		public D2DPathGeometry CreateTextPathGeometry(string text, string fontName, float fontSize,
			D2DFontWeight fontWeight = D2DFontWeight.Normal,
			D2DFontStyle fontStyle = D2DFontStyle.Normal,
			D2DFontStretch fontStretch = D2DFontStretch.Normal)
		{
			var fontFace = D2D.CreateFontFace(this.Handle, fontName, fontWeight, fontStyle, fontStretch);
			if (fontFace == IntPtr.Zero)
			{
				throw new CreateFontFaceFailedException(fontName);
			}

			var pathHandler = D2D.CreateTextPathGeometry(this.Handle, text, fontFace, fontSize);

			D2D.DestroyFontFace(fontFace);

			if (pathHandler == IntPtr.Zero)
			{
				throw new CreatePathGeometryFailedException();
			}

			return new D2DPathGeometry(this, pathHandler);
		}

		public D2DTextFormat CreateTextFormat(string fontName, float fontSize,
				D2DFontWeight fontWeight = D2DFontWeight.Normal,
				D2DFontStyle fontStyle = D2DFontStyle.Normal,
				D2DFontStretch fontStretch = D2DFontStretch.Normal,
				DWriteTextAlignment halign = DWriteTextAlignment.Leading,
				DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near)
		{
			HANDLE fmtHandle = D2D.CreateTextFormat(this.Handle, fontName, fontSize, fontWeight, fontStyle, fontStretch, halign, valign);
			return new D2DTextFormat(fmtHandle);
		}

		public D2DTextLayout CreateTextLayout(string text, D2DTextFormat textFormat, D2DSize size)
		{
			HANDLE fmtHandle = D2D.CreateTextLayout(this.Handle, text, textFormat.Handle, ref size);
			return new D2DTextLayout(fmtHandle);
		}

		public D2DBitmap? LoadBitmap(byte[] buffer)
		{
			return this.LoadBitmap(buffer, 0, (uint)buffer.Length);
		}

		public D2DBitmap? LoadBitmap(byte[] buffer, UINT offset, UINT length)
		{
			var bitmapHandle = D2D.CreateBitmapFromBytes(this.Handle, buffer, offset, length);
			return (bitmapHandle != HWND.Zero) ? new D2DBitmap(bitmapHandle) : null;
		}

		public D2DBitmap? LoadBitmap(string filepath)
		{
			return CreateBitmapFromFile(filepath);
		}

		public D2DBitmap? CreateBitmapFromFile(string filepath)
		{
			var bitmapHandle = D2D.CreateBitmapFromFile(this.Handle, filepath);
			return (bitmapHandle != HWND.Zero) ? new D2DBitmap(bitmapHandle) : null;
		}

		public D2DBitmap? CreateBitmapFromMemory(UINT width, UINT height, UINT stride, IntPtr buffer, UINT offset, UINT length)
		{
			HANDLE d2dbmp = D2D.CreateBitmapFromMemory(this.Handle, width, height, stride, buffer, offset, length);
			return d2dbmp == HANDLE.Zero ? null : new D2DBitmap(d2dbmp);
		}

		public D2DLayer CreateLayer()
		{
			return new D2DLayer(D2D.CreateLayer(this.Handle));
		}

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr obj);

		public D2DBitmap? CreateBitmapFromHBitmap(HWND hbmp, bool useAlphaChannel)
		{
			HANDLE d2dbmp = D2D.CreateBitmapFromHBitmap(this.Handle, hbmp, useAlphaChannel);
			return d2dbmp == HANDLE.Zero ? null : new D2DBitmap(d2dbmp);
		}

		public D2DBitmap? CreateBitmapFromGDIBitmap(System.Drawing.Bitmap bmp)
		{
			bool useAlphaChannel =
				(bmp.PixelFormat & System.Drawing.Imaging.PixelFormat.Alpha) == System.Drawing.Imaging.PixelFormat.Alpha;

			return this.CreateBitmapFromGDIBitmap(bmp, useAlphaChannel);
		}

		public D2DBitmap? CreateBitmapFromGDIBitmap(System.Drawing.Bitmap bmp, bool useAlphaChannel)
		{
			HANDLE d2dbmp = HANDLE.Zero;
			HANDLE hbitmap = bmp.GetHbitmap();

			if (hbitmap != HANDLE.Zero)
			{
				d2dbmp = D2D.CreateBitmapFromHBitmap(this.Handle, hbitmap, useAlphaChannel);
				DeleteObject(hbitmap);
			}

			return d2dbmp == HANDLE.Zero ? null : new D2DBitmap(d2dbmp);
		}

		public D2DBitmapGraphics? CreateBitmapGraphics()
		{
			return CreateBitmapGraphics(D2DSize.Empty);
		}

		public D2DBitmapGraphics? CreateBitmapGraphics(float width, float height)
		{
			return CreateBitmapGraphics(new D2DSize(width, height));
		}

		public D2DBitmapGraphics? CreateBitmapGraphics(D2DSize size)
		{
			HANDLE bitmapRenderTargetHandle = D2D.CreateBitmapRenderTarget(this.Handle, size);
			return bitmapRenderTargetHandle == HANDLE.Zero ? null
				: new D2DBitmapGraphics(bitmapRenderTargetHandle);
		}

		public void Dispose()
		{
			D2D.DestroyContext(this.Handle);
		}
	}
}
