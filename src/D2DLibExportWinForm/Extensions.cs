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

		public static void DrawBitmap(this D2DGraphics g, Bitmap bitmap, Rectangle rect, float opacity = 1,
			bool alpha = false, D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			g.DrawBitmap(bitmap, new D2DRect(rect), opacity, alpha, interpolationMode);
		}

		public static void DrawBitmap(this D2DGraphics g, Bitmap bitmap, D2DRect targetRect, float opacity = 1,
			bool alpha = false, D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			IntPtr hbitmap = bitmap.GetHbitmap();

			if (hbitmap != IntPtr.Zero)
			{
				var srcRect = new D2DRect(0, 0, bitmap.Width, bitmap.Height);

				g.DrawGDIBitmap(hbitmap, ref targetRect, ref srcRect, opacity, alpha, interpolationMode);
				Win32.DeleteObject(hbitmap);
			}
		}

		public static void DrawText(this D2DGraphics g, string text, D2DColor color, Font font, float x, float y)
		{
			var rect = new D2DRect(x, y, 9999999, 9999999);
			g.DrawText(text, color, font.Name, font.Size * 96f / 72f, ref rect);
		}

		public static void DrawText(this D2DGraphics g, string text, D2DColor color, Font font, Point location)
		{
			var rect = new D2DRect(location.X, location.Y, 9999999, 9999999);
			g.DrawText(text, color, font.Name, font.Size * 96f / 72f, ref rect);
		}

		public static void DrawText(this D2DGraphics g, string text, D2DColor color, Font font, PointF location)
		{
			var rect = new D2DRect(location.X, location.Y, 9999999, 9999999);
			g.DrawText(text, color, font.Name, font.Size * 96f / 72f, ref rect);
		}
	}
}
