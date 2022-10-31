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

using unvell.D2DLib.WinForm;
using unvell.D2DLib.Examples.Properties;

namespace unvell.D2DLib.Examples.Demos
{
	public partial class Subtitle : DemoForm
	{
		private static readonly Font subtitleFont = new Font("Times New Roman", 32f, FontStyle.Italic);

		public Subtitle()
		{
			Text = "Subtitle Demo - d2dlib Examples";

			Size = new Size(1280, 768);
			BackgroundImage = Device.LoadBitmap(Resources.space_bg);
			ShowFPS = true;
			AnimationDraw = true;

			// create a memory bitmap
			bg = Device.CreateBitmapGraphics(new D2DSize(750, 1500));

			// draw subtitle on memory bitmap
			bg.BeginRender();
			bg.DrawText(Resources.Star_Subtitle_Original, new D2DColor(.5f, 0, 0, 0), subtitleFont, 13, 13);
			bg.DrawText(Resources.Star_Subtitle_Original, new D2DColor(.8f, D2DColor.DarkOrange), subtitleFont, 10, 10);
			bg.EndRender();
		}

		~Subtitle()
		{
			bg?.Dispose();
		}

		private D2DBitmapGraphics bg;
		private float y = 968;

		protected override void OnFrame()
		{
			if (y < -1000) y = ClientRectangle.Bottom + 200;
			y -= 3f;

			SceneChanged = true;
		}

		protected override void OnRender(D2DGraphics g)
		{
			// draw the subtitle memory bitmap on screen
			var rect = new D2DRect(50, y, ClientRectangle.Width - 100, 1500);
			g.DrawBitmap(bg, rect);
		}
	}
}

