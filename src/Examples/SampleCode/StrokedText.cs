/*
 * MIT License
 *
 * Copyright (c) 2009-2022 Jingwood, unvell.com. All right reserved.
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

		D2DPathGeometry textPath1, textPath2;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Exception will be thrown when the specified font not found in the Windows OS
			textPath1 = this.Device.CreateTextPathGeometry("Hello World", "Arial", 36, D2DFontWeight.ExtraBold);
		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			//
			// Example 1: Create a path geometry from text and render it
			//
			// change the location to draw the text
			g.TranslateTransform(100, 100);

			g.FillPath(this.textPath1, D2DColor.Yellow);
			g.DrawPath(this.textPath1, D2DColor.Blue, 3);

			// restore the location after drawing text
			g.TranslateTransform(-100, -100);



			//
			// Example 2: Use a helper method to draw a stroked text in simplest way
			//
			g.DrawStrokedText("Stroked text rendered using D2DLib", 100, 200,
				D2DColor.DarkOliveGreen, 2, D2DColor.LightCyan, "Consolas", 24);



			// Example 3: Text in non-English language (Unicode fonts)
			//            The characters in the text must exist in the font.
			//            This case only works in a Japanese language Windows.
			//
			//var textPath2 = this.Device.CreateTextPathGeometry("こんにちは、世界", "MS Gothic", 28);

			//g.TranslateTransform(100, 300);
			//g.DrawPath(textPath2, D2DColor.DarkGreen);
			//g.TranslateTransform(-100, -300);

		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			this.textPath1?.Dispose();
			//this.textPath2?.Dispose();
		}
	}
}
