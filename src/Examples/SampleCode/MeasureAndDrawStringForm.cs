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
using System.Drawing;
using unvell.D2DLib.WinForm;
using unvell.D2DLib.Examples.Properties;

namespace unvell.D2DLib.Examples.SampleCode
{
	public partial class MeasureAndDrawStringForm : ExampleForm
	{
		private static readonly Font font1 = new Font("Times New Roman", 34f, FontStyle.Italic);

		public MeasureAndDrawStringForm()
		{
			Text = "Measure and draw string";

			Size = new Size(1280, 800);
		}

		protected override void OnRender(D2DGraphics g)
		{
      var text = "Hello World";

      var rect = new Rectangle(100, 100, 500, 500);

      var measuredSize = g.MeasureText(text, font1.Name, font1.Size, rect.Size);

      var measuredRect = new D2DRect(rect.X, rect.Y, measuredSize.width, measuredSize.height);

      g.DrawText(text, D2DColor.Black, font1.Name, font1.Size, rect);

      g.DrawRectangle(measuredRect, D2DColor.Blue);
		}
	}
}

