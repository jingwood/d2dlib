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
using System.Windows.Forms;
using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.Demos
{
	class TransparentMemoryBitmapDraw : DemoForm
	{
		public TransparentMemoryBitmapDraw()
		{
			Text = "Transparenct Images Draw - d2dlib Examples";
			WindowState = FormWindowState.Maximized;

			AnimationDraw = true;
			ShowFPS = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			D2DBitmap[] bmps = new D2DBitmap[] {
				this.Device.CreateBitmapFromGDIBitmap(Properties.Resources.icons8_car_production_96),
				this.Device.CreateBitmapFromGDIBitmap(Properties.Resources.icons8_himeji_castle_96),
				this.Device.CreateBitmapFromGDIBitmap(Properties.Resources.icons8_home_page_96),
				this.Device.CreateBitmapFromGDIBitmap(Properties.Resources.icons8_music_96),
				this.Device.CreateBitmapFromGDIBitmap(Properties.Resources.icons8_uwu_emoji_96),
			};

			// Try this!
			//
			// High performance drawing by convert bitmap to D2DBitmap
			//
			for (int i = 0; i < 1000; i++)
			{
				icons.Add(new IconInfo(bmps[rand.Next(bmps.Length)], this.ClientRectangle));
			}

			// Try this!
			//
			// Low performance drawing using original GDI+ bitmap
			//
			//for (int i = 0; i < 1000; i++)
			//{
			//	icons.Add(new IconInfo(bmps[rand.Next(bmps.Length)], this.ClientRectangle));
			//}
		}

		List<IconInfo> icons = new List<IconInfo>();

		float bgPos = 0.5f, bgR = 0.8f, bgG = 0.8f, bgB = 0.8f;

		protected override void OnRender(D2DGraphics g)
		{
			var size = this.ClientRectangle.Size;

			using (var brush = this.Device.CreateLinearGradientBrush(new D2DPoint(0, 0), new D2DPoint(size.Width, -200),
				new D2DGradientStop[] {
					new D2DGradientStop(0, MathFunctions.Clamp(new D2DColor(1.0f-bgR, bgG, bgB))),
					new D2DGradientStop(bgPos, MathFunctions.Clamp(new D2DColor(bgR, bgG, bgB))),
					new D2DGradientStop(1, MathFunctions.Clamp(new D2DColor(bgR, 1.0f-bgG, bgB))),
				}))
			{
				g.FillRectangle(new D2DRect(0, 0, size.Width, size.Height), brush);
			}

			foreach (var icon in icons)
			{
				g.DrawBitmap(icon.Bitmap, new D2DRect((float)icon.X, (float)icon.Y, icon.Bitmap.Width, icon.Bitmap.Height), (float)icon.Opacity);
			}
		}

		protected override void OnFrame()
		{
			foreach (var icon in icons)
			{
				icon.Y += icon.Speed;

				if (icon.Y > ClientRectangle.Height) {
					icon.Reset(this.ClientRectangle);
				}
			}

			bgPos += (float)(rand.NextDouble() - 0.5) * 0.01f;
			bgR += (float)(rand.NextDouble() - 0.5) * 0.01f;
			bgG += (float)(rand.NextDouble() - 0.5) * 0.01f;
			bgB += (float)(rand.NextDouble() - 0.5) * 0.01f;
		}

		private static readonly Random rand = new Random();

		class IconInfo
		{
			public D2DBitmap Bitmap { get; set; }
			public double X { get; set; }
			public double Y { get; set; }
			public double Opacity { get; set; }
			public double Speed { get; set; }

			public IconInfo(D2DBitmap bitmap, Rectangle screenRect)
			{
				this.Bitmap = bitmap;
				this.Reset(screenRect);
			}

			public void Reset(Rectangle screenRect)
			{
				this.X = rand.Next(screenRect.Width + 100) - 100;
				this.Y = -100 - rand.Next(300);
				this.Opacity = rand.NextDouble() * 0.5 + 0.5;
				this.Speed = rand.NextDouble() * 10;
			}
		}
	}
}
