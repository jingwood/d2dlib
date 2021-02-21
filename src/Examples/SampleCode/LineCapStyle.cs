/*
* MIT License
*
* Copyright (c) 2017-2021 Jingwood, unvell.com. All right reserved.
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
	public partial class LineCapStyle : ExampleForm
	{
		public LineCapStyle()
		{
			Text = "LineCapStyle Tests - d2dlib Examples";
		}

		protected override void OnRender(D2DGraphics g)
		{
			g.DrawText("Flat", D2DColor.DimGray, "Arials", 20, 110, 83);
			g.DrawLine(300, 100, 600, 100, D2DColor.Black, 20, D2DDashStyle.Solid, D2DCapStyle.Flat, D2DCapStyle.Flat);
  
			g.DrawText("Round", D2DColor.DimGray, "Arials", 20, 110, 183);
			g.DrawLine(300, 200, 600, 200, D2DColor.Black, 20, D2DDashStyle.Solid, D2DCapStyle.Round, D2DCapStyle.Round);

			g.DrawText("Square", D2DColor.DimGray, "Arials", 20, 110, 283);
			g.DrawLine(300, 300, 600, 300, D2DColor.Black, 20, D2DDashStyle.Solid, D2DCapStyle.Square, D2DCapStyle.Square);

			g.DrawText("Triangle", D2DColor.DimGray, "Arials", 20, 110, 383);
			g.DrawLine(300, 400, 600, 400, D2DColor.Black, 20, D2DDashStyle.Solid, D2DCapStyle.Triangle, D2DCapStyle.Triangle);

			g.DrawText("Round, Triangle", D2DColor.DimGray, "Arials", 20, 110, 483);
			g.DrawLine(300, 500, 600, 500, D2DColor.Black, 20, D2DDashStyle.Solid, D2DCapStyle.Round, D2DCapStyle.Triangle);
		}
	}

}
