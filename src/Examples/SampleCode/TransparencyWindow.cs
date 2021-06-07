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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.SampleCode
{
	public partial class TransparencyWindow : ExampleForm
	{
		public TransparencyWindow()
		{
			Text = "TransparencyWindow - d2dlib Examples";

			this.Opacity = 0.5;

			AnimationDraw = true;
		}

		float t = 0;

    protected override void OnFrame()
    {
			t += 0.1f;
    }

    protected override void OnRender(D2DGraphics g)
		{
			g.FillRectangle(new D2DRect(100, 100, 300, 200), new D2DColor(0.5f, D2DColor.Blue));

			g.DrawText("Hello World - Transparency Window", D2DColor.Black, "Arials", 24, (float)(150 + 100f * Math.Sin(t)), 200);
    }
  }

}
