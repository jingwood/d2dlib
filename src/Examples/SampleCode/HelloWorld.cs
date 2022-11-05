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
	public partial class HelloWorld : ExampleForm
	{
		public HelloWorld()
		{
			Text = "HelloWorld - d2dlib Examples";
		}

		protected override void OnRender(D2DGraphics g)
		{
			g.DrawPolygon(new D2DPoint[] { new D2DPoint(100, 100), new D2DPoint(150, 150),
				new D2DPoint(100, 150) }, D2DColor.Black, 0, D2DDashStyle.Solid, D2DColor.Red);

			g.DrawText("Text drawed using Direct2D API (d2dlib)", D2DColor.Black, "Arial", 24, 140, 110);
		}
	}

}
