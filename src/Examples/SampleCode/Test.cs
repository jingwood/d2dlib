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
	public partial class TestForm : ExampleForm
	{
		public TestForm()
		{
			Text = "HelloWorld - d2dlib Examples";
		}

		protected override void OnRender(D2DGraphics g)

		{

			g.Clear(D2DColor.FromGDIColor(this.BackColor));



			using (var rect = this.Device.CreateRectangleGeometry(new D2DRect(10, 10, 20, 20)))

			{

				g.DrawPath(rect, D2DColor.BlueViolet, 1, D2DDashStyle.Dash);

			}



			using (var circle = this.Device.CreateEllipseGeometry(new D2DPoint(100, 100), new D2DSize(50, 50)))

			{

				using (var pen = this.Device.CreatePen(D2DColor.BlueViolet, D2DDashStyle.Dash))

				{

					if (pen != null)

					{

						g.DrawPath(circle, pen, 2);

						g.FillPath(circle, D2DColor.DarkBlue);

					}

				}

			}

		}

	}

}
