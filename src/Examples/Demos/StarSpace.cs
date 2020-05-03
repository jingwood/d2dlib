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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.Demos
{
	public partial class StarSpace : DemoForm
	{
		public StarSpace()
		{
			BackColor = Color.Black;
			WindowState = FormWindowState.Maximized;
			FormBorderStyle = FormBorderStyle.None;

			ShowFPS = true;
			AnimationDraw = true;

			// create a device brush in advance rather than pass color 
			// during render to get better performance
			brush = Device.CreateSolidColorBrush(D2DColor.Silver);

			for (int i = 0; i < StarCount; i++)
			{
				stars[i] = new Star();
			}
		}

		private const int StarCount = 3000;
		private Star[] stars = new Star[StarCount];
		private static readonly Random rand = new Random();

		private D2DBitmap bitmap = null;
		private D2DSolidColorBrush brush = null;

		protected override void OnFrame()
		{
			var range = ClientRectangle;
			int hw = range.Width / 2;
			int hh = range.Height / 2;

			for (int i = 0; i < stars.Length; i++)
			{
				Star s = stars[i];

				if ((s.x < -50 || s.y < -50 ||
					s.x > range.Width || s.y > range.Height) ||
					(s.x == 0 && s.y == 0))
				{
					s.x = rand.Next(range.Width);
					s.y = rand.Next(range.Height);

					s.size = (float)rand.NextDouble() * 3.0f + 0.2f;
					s.speed = (float)rand.NextDouble() * 0.03f + 0.002f;

					float gray = (float)rand.NextDouble() * 0.7f + 0.3f;
					s.color = new D2DColor(gray, gray, gray);
				}

				s.x += (s.x - hw) * s.speed;
				s.y += (s.y - hh) * s.speed;
				//s.size += 0.1f / (float)(Math.Pow(s.x - hw, 2) + Math.Pow(s.y - hh, 2));
			}

			SceneChanged = true;
		}

		protected override void OnRender(D2DGraphics g)
		{
			D2DEllipse ellipse = new D2DEllipse();

			for (int i = 0; i < stars.Length; i++)
			{
				var s = stars[i];

				if (s.x != 0 && s.y != 0)
				{
					ellipse.X = s.x;
					ellipse.Y = s.y;
					ellipse.radiusX = ellipse.radiusY = s.size;
					brush.Color = s.color;

					g.FillEllipse(ellipse, brush);
				}
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (this.bitmap != null) this.bitmap.Dispose();
			if (this.brush != null) this.brush.Dispose();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			Close();
		}
	}

	class Star
	{
		public float x, y;
		public float size;
		public D2DColor color;
		public float speed;
	}
}
