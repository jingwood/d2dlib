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
using System.Drawing;
using System.Linq;
using System.Text;
using unvell.Common.Win32Lib;

namespace unvell.D2DLib.WinForm
{
	public static class D2DWinFormExtensions
	{
		public static void DrawBitmap(this D2DGraphics g, Bitmap bitmap, float x, float y, float opacity = 1,
			bool alpha = false, D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			g.DrawBitmap(bitmap, new D2DRect(x, y, bitmap.Width, bitmap.Height), opacity, alpha, interpolationMode);
		}

		public static void DrawBitmap(this D2DGraphics g, Bitmap bitmap, D2DRect destRect, float opacity = 1,
			bool alpha = false, D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			IntPtr hbitmap = bitmap.GetHbitmap();

			if (hbitmap != IntPtr.Zero)
			{
				var srcRect = new D2DRect(0, 0, bitmap.Width, bitmap.Height);

				g.DrawGDIBitmap(hbitmap, destRect, srcRect, opacity, alpha, interpolationMode);
				Win32.DeleteObject(hbitmap);
			}
		}

		public static void DrawText(this D2DGraphics g, string text, D2DColor color, float x, float y)
		{
			g.DrawText(text, color, SystemFonts.DefaultFont, x, y);
		}

		public static void DrawText(this D2DGraphics g, string text, D2DColor color, Font font, float x, float y)
		{
			var rect = new D2DRect(x, y, 9999999, 9999999);
			g.DrawText(text, color, font.Name, font.Size * 96f / 72f, rect);
		}

		public static void DrawText(this D2DGraphics g, string text, D2DColor color, Font font, Point location)
		{
			var rect = new D2DRect(location.X, location.Y, 9999999, 9999999);
			g.DrawText(text, color, font.Name, font.Size * 96f / 72f, rect);
		}

		public static void DrawText(this D2DGraphics g, string text, D2DColor color, Font font, PointF location)
		{
			var rect = new D2DRect(location.X, location.Y, 9999999, 9999999);
			g.DrawText(text, color, font.Name, font.Size * 96f / 72f, rect);
		}
	}
}

namespace unvell.D2DLib
{
	public static class D2DStructExtensions
	{
	
	}
}
