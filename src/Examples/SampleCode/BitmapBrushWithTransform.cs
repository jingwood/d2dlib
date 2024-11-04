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


using System.ComponentModel;
using System.Drawing;
using unvell.D2DLib.Examples.Properties;

namespace unvell.D2DLib.Examples.SampleCode
{
	public partial class BitmapBrushWithTransform : ExampleForm
	{
		public BitmapBrushWithTransform()
		{
			Text = "BitmapBrushWithTransform - D2DLib Sample Code";
		}

		D2DBitmap bitmap;
		D2DBitmapBrush brush;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			bitmap = Device.LoadBitmap(Resources.IMGP6873);
			brush = Device.CreateBitmapBrush(bitmap, D2DExtendMode.Wrap, D2DExtendMode.Wrap);

			AnimationDraw = true;
			x = startX;
			y = startY;
		}

		private float startX = 300, startY = 300;
		private float x = 0, y = 0;
		private float width = 300, height = 200;
		private int direction = 0;
		private int speed = 5;

		protected override void OnFrame()
		{
			switch (direction)
			{
				case 0:
					x+=speed;
					if (x > bitmap.Width - width-startX)
					{
						direction++;
					}
					break;

				case 1:
					y += speed;
					if (y > bitmap.Height - height-startY)
					{
						direction++;
					}
					break;
				case 2:
					x -= speed;
					if (x < 0)
					{
						direction++;
					}
					break;
				case 3:
					y -= speed;
					if (y < 0)
					{
						direction = 0;
					}
					break;
			}
		}

		protected override void OnRender(D2DGraphics g)
		{
			
			brush.SetTransform(Matrix3x2.CreateTranslation(-x, -y));

			using (var pen = Device.CreatePen(D2DColor.Transparent))
			{
				g.DrawRoundedRectangle(new D2DRoundedRect
				{
					rect = new D2DRect(startY, startY, width, height),
					radiusX = 10,
					radiusY = 10
				}, pen, brush);
			}
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			this.bitmap?.Dispose();
			this.brush?.Dispose();
		}
	}

}
