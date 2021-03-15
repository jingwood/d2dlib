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
using System.Windows.Forms;
using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.Demos
{
	class ImageTest : DemoForm
	{
		public ImageTest()
		{
			Text = "Direct2D Image Test - d2dlib Examples";

			// put this form center on screen
			var screenSize = Screen.FromControl(this).WorkingArea.Size;
			Size = new System.Drawing.Size((int)(screenSize.Width * 0.8f), (int)(screenSize.Height * 0.9f));
			Location = new System.Drawing.Point((int)(screenSize.Width * 0.1f), (int)(screenSize.Height * 0.05f));
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;

			BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
			AnimationDraw = true;
			ShowFPS = true;
		}

		private D2DBitmap fullImage;
		private const int gridCountX = 12, gridCountY = 9;
		private float gridImageWidth, gridImageHeight;
		private float screenImageWidth, screenImageHeight;
		private int playClipNumber = 0;
		private const float animationSpeed = 0.07f;
		private static readonly Random rand = new Random();

		class Sprite
		{
			public D2DBitmapGraphics bmp;
			public float x, y;
			public float originX, originY;
			public float width, height;
			public float angle;
		}

		private Sprite[,] sprites;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			fullImage = Device.LoadBitmap(Properties.Resources.IMGP6873);

			gridImageWidth = fullImage.Width / gridCountX;
			gridImageHeight = fullImage.Height / gridCountY;

			screenImageWidth = (float)ClientRectangle.Width / gridCountX;
			screenImageHeight = (float)ClientRectangle.Height / gridCountY;

			sprites = new Sprite[gridCountX, gridCountY];

			for (int y = 0; y < gridCountY; y++)
			{
				for (int x = 0; x < gridCountX; x++)
				{
					var bmp = Device.CreateBitmapGraphics(screenImageWidth, screenImageHeight);

					// copy a part of original image
					bmp.BeginRender();
					bmp.DrawBitmap(fullImage, new D2DRect(0, 0, screenImageWidth, screenImageHeight),
						new D2DRect(x * gridImageWidth, y * gridImageHeight, gridImageWidth, gridImageHeight));
					bmp.EndRender();

					var s = new Sprite
					{
						bmp = bmp,
						originX = x * screenImageWidth,
						originY = y * screenImageHeight,
						width = screenImageWidth,
						height = screenImageHeight,
					};

					sprites[x, y] = s;
				}
			}

			BeginClip();
		}

		protected override void OnFrame()
		{
			base.OnFrame();

			bool playFinished = true;

			for (int y = 0; y < gridCountY; y++)
			{
				for (int x = 0; x < gridCountX; x++)
				{
					var p = this.sprites[x, y];

					var diffX = (p.originX - p.x) * animationSpeed;
					var diffY = (p.originY - p.y) * animationSpeed;
					var diffWidth = (screenImageWidth - p.width) * animationSpeed;
					var diffHeight = (screenImageHeight - p.height) * animationSpeed;

					p.x += diffX;
					p.y += diffY;
					p.width += diffWidth;
					p.height += diffHeight;
					p.angle -= p.angle * animationSpeed;

					if (playFinished)
					{
						playFinished = Math.Abs(diffX) < 0.01 && Math.Abs(diffY) < 0.01
							&& Math.Abs(diffWidth) < 0.01 && Math.Abs(diffHeight) < 0.01
							&& Math.Abs(p.angle) < 0.01;
					}
				}
			}

			if (playFinished)
			{
				playClipNumber++;
				BeginClip();
			}

			SceneChanged = true;
		}

		void BeginClip()
		{
			for (int y = 0; y < gridCountY; y++)
			{
				for (int x = 0; x < gridCountX; x++)
				{
					var p = this.sprites[x, y];

					switch (playClipNumber)
					{
						case 0:
							p.x = rand.Next(gridCountX) * screenImageWidth;
							p.y = rand.Next(gridCountY) * screenImageHeight;
							p.width = rand.Next(1000);
							p.height = rand.Next(1000);
							break;

						case 1:
							p.x = -gridImageWidth;
							p.y = -gridImageHeight;
							break;

						case 2:
							p.x = ClientRectangle.Width + rand.Next(1000);
							p.y = 4 * screenImageWidth;
							break;

						case 3:
							p.x = x * screenImageWidth;
							p.y = y * screenImageHeight;
							p.width = 0;
							p.height = 0;
							break;

						case 4:
							p.x = x * screenImageWidth;
							p.y = y * screenImageHeight;
							p.width = rand.Next(1000);
							p.height = rand.Next(1000);
							p.angle = rand.Next(720) - 360;
							break;

						case 5:
							p.x = ClientRectangle.Width / 2;
							p.y = ClientRectangle.Height / 2;
							p.width = 0;
							p.height = 0;
							p.angle = rand.Next(720) - 360;
							break;

						case 6:
							playClipNumber = 0;
							BeginClip();
							break;
					}
				}
			}
		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			for (int y = 0; y < gridCountY; y++)
			{
				for (int x = 0; x < gridCountX; x++)
				{
					var s = sprites[x, y];

					// if angle is almost zero, draw the partial image simply
					// this will get better performance
					if (Math.Abs(s.angle) < 0.01)
					{
						// add 0.5 for width and height to avoid edge line between image sprites
						g.DrawBitmap(s.bmp, new D2DRect(s.x, s.y, s.width + 0.5f, s.height + 0.5f));
					}
					else
					{
						// else when angle is specified, push a rotate matrix
						g.PushTransform();
						g.RotateTransform(s.angle, new D2DPoint(s.x + s.width * 0.5f, s.y + s.height * 0.5f));
						g.DrawBitmap(s.bmp, new D2DRect(s.x, s.y, s.width + 0.5f, s.height + 0.5f));
						g.PopTransform();
					}
				}
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			for (int y = 0; y < gridCountY; y++)
			{
				for (int x = 0; x < gridCountX; x++)
				{
					sprites[x, y]?.bmp?.Dispose();
				}
			}

			fullImage?.Dispose();
		}
	}
}
