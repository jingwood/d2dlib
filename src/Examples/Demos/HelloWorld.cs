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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.Demos
{
	public partial class HelloWorld : DemoForm
	{
		public HelloWorld()
		{
			Text = "HelloWorld - d2dlib Examples";
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			g.DrawText("Text drawed via Direct2D API (d2dlib)", D2DColor.Black, 100, 100);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
		}
	}

}
