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

namespace unvell.D2DLib.Examples.SampleCode
{
	public partial class ClipMaskDraw : ExampleForm
	{
		public ClipMaskDraw()
		{
			Text = "ClipMaskDraw - d2dlib Examples";

			AnimationDraw = true;
		}

		double offsetX = 0;
		double waveWidth = 100;
		double t = 0;

		protected override void OnRender(D2DGraphics g)
		{
			using (var path = this.Device.CreateEllipseGeometry(
			  new D2DPoint((float)(340 + waveWidth * offsetX), 200), new D2DSize(130, 130)))
			{
				using (var layer = g.PushLayer(path))
				{
					g.FillRectangle(ClientRectangle, new D2DColor(.7f, .7f, .7f));

					g.DrawText("Text drawed via Direct2D API (d2dlib)", D2DColor.Blue, "Arial", 24, 140, 180);

					g.PopLayer();
				}
			}
		}

		protected override void OnFrame()
		{
			offsetX = Math.Sin(t);
			t += .03;

			SceneChanged = true;
		}
	}

}
