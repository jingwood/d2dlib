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
	public class D2DDevice : IDisposable
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

		public D2DPen CreatePen(D2DColor color, D2DDashStyle dashStyle, float dashOffset = 0.0f)
		{
			HANDLE handle = D2D.CreatePen(this.Handle, color, dashStyle, null, 0, dashOffset);
			return handle == HANDLE.Zero ? null : new D2DPen(handle, color, dashStyle);
		}
		public D2DPen CreateCustomPen(D2DColor color, float[] dashes, float dashOffset = 0.0f)
		{
			if (dashes == null) throw new ArgumentNullException(nameof(dashes));

			HANDLE handle = D2D.CreatePen(this.Handle, color, D2DDashStyle.Custom, dashes, (uint)dashes.Length, dashOffset);
			return handle == HANDLE.Zero ? null : new D2DPen(handle, color, D2DDashStyle.Custom);
		}
		public void DestroyPen(D2DPen pen)
		{
			if (pen == null) throw new ArgumentNullException(nameof(pen));
			D2D.DestroyPen(pen.Handle);
		}

		public D2DSolidColorBrush CreateSolidColorBrush(D2DColor color)
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

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr obj);

		public D2DBitmap CreateBitmapFromGDIBitmap(System.Drawing.Bitmap bmp, bool useAlphaChannel)
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
			D2D.DestroyContext(this.Handle);
		}
	}
}
