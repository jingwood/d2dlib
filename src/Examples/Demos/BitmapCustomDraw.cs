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

using System;
using System.Drawing;
using System.Windows.Forms;
using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.Demos
{
	public partial class BitmapCustomDraw : DemoForm
	{
		public BitmapCustomDraw()
		{
			Text = "Bitmap Draw - d2dlib Examples";
			AnimationDraw = true;
			ShowFPS = true;

			// test1: create two dummy GDI bitmaps and convert them to Direct2D device bitmap

			gdiBmp1 = new Bitmap(1024, 1024);
			using (Graphics g = Graphics.FromImage(gdiBmp1))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				g.DrawString("This is GDI+ bitmap layer 1", new Font(this.Font.FontFamily, 48), Brushes.Black, 10, 10);
			}
			d2dbmp1 = this.Device.CreateBitmapFromGDIBitmap(gdiBmp1);

			gdiBmp2 = new Bitmap(1024, 1024);
			using (Graphics g = Graphics.FromImage(gdiBmp2))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

				using (var p = new Pen(Color.Blue, 3))
				{
					g.DrawRectangle(p, 200, 200, 400, 200);
				}

				using (var p = new Pen(Color.Red, 5))
				{
					g.DrawEllipse(p, 350, 250, 400, 350);
				}

				g.DrawString("This is GDI+ bitmap layer 2", new Font(this.Font.FontFamily, 24), Brushes.Green, 350, 400);
			}
			d2dbmp2 = this.Device.CreateBitmapFromGDIBitmap(gdiBmp2);


			// test2: create one Direct2D device bitmap

			var rect = new D2DRect(170, 790, 670, 80);
			bmpGraphics = this.Device.CreateBitmapGraphics(1024, 1024);
			bmpGraphics.BeginRender();
			bmpGraphics.FillRectangle(rect, new D2DColor(0.4f, D2DColor.Black));
			bmpGraphics.DrawTextCenter("This is Direct2D device bitmap layer", D2DColor.Goldenrod, this.Font.Name, 36, rect);
			bmpGraphics.EndRender();

		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (d2dbmp1 == null) Close();
		}

		private Bitmap gdiBmp1, gdiBmp2;
		private D2DBitmap d2dbmp1, d2dbmp2;
		private D2DBitmapGraphics bmpGraphics;

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			// draw random rectangles
			for (int i = 0; i < rectCount; i++)
			{
				var r = rects[i];
				if (r != null) g.FillRectangle(r.x, r.y, r.w, r.h, r.color);
			}

			// draw GDI+ bitmaps using hardware acceleration
			g.DrawBitmap(d2dbmp1, this.ClientRectangle);
			g.DrawBitmap(d2dbmp2, this.ClientRectangle);

			// draw Direct2D bitmap
			g.DrawBitmap(bmpGraphics, this.ClientRectangle);

			// direct draw using D2DGraphics
			g.DrawRoundedRectangle(new D2DRoundedRect
			{
				rect = new D2DRect(100, 200, 200, 150),
				radiusX = 10,
				radiusY = 10
			}, D2DColor.LightSeaGreen, D2DColor.GreenYellow, 5);
	 
			g.DrawRoundedRectangle(new D2DRoundedRect
			{
				rect = new D2DRect(150, 250, 100, 200),
				radiusX = 10,
				radiusY = 5
			}, D2DColor.Red, D2DColor.Transparent, 1);
			
			g.DrawRoundedRectangle(new D2DRoundedRect
			{
				rect = new D2DRect(50, 100, 100, 50),
				radiusX = 20,
				radiusY = 20
			}, D2DColor.Transparent, D2DColor.LightBlue, 2);
		}

		#region Rect animation
		class Rect
		{
			public float x, y, w, h;
			public D2DColor color;
			public float step;
			public float speed;
		}

		public const int rectCount = 100;
		private Rect[] rects = new Rect[rectCount];
		private static readonly Random rand = new Random();

		protected override void OnFrame()
		{
			for (int i = 0; i < rectCount; i++)
			{
				var r = rects[i];

				if (r == null)
				{
					rects[i] = r = new Rect();
				}

				if (r.step >= 2)	r.step = 0;

				if (r.step <= 0)
				{
					r.x = rand.Next(ClientRectangle.Width);
					r.y = rand.Next(ClientRectangle.Height);
					r.w = rand.Next(400) + 50;
					r.h = rand.Next(200) + 50;
					r.color = new D2DColor((float)(rand.NextDouble() * 0.5f + 0.5f),
						(float)(rand.NextDouble() * 0.5f + 0.5f), (float)(rand.NextDouble() * 0.5f + 0.5f));
					r.speed = (float)(rand.NextDouble() * 0.3f + 0.01f) * 0.1f;
					r.step = r.speed;
				}
				else
				{
					r.step += r.speed;
				}

				r.color.a = 1 - Math.Abs(1 - r.step);
			}

			this.SceneChanged = true;
		}

		#endregion Rect animation

	}
}
