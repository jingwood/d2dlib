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
	public partial class StrokedText : ExampleForm
	{
		public StrokedText()
		{
			Text = "Stroked Text Drawing - d2dlib Examples";
			BackColor = Color.White;
		}

		D2DPathGeometry path;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			path = this.Device.CreateTextPathGeometry("Hello World", "Arial", 36, D2DFontWeight.ExtraBold);

		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			var rect = new D2DRect(100, 20, 400, 20);

			g.TranslateTransform(100, 100);

			g.FillPath(this.path, D2DColor.Yellow);
			g.DrawPath(this.path, D2DColor.Blue, 3);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			this.path?.Dispose();
		}
	}
}
