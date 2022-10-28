﻿/*
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using unvell.D2DLib;
using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.SampleCode
{
	public partial class TextStyles : ExampleForm
	{
		public TextStyles()
		{
			Text = "Text Style Samples - d2dlib Examples";
			BackColor = Color.White;
		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			var rect = new D2DRect(100, 20, 400, 20);

			int i = 0;
			foreach (var fontStyle in Enum.GetValues(typeof(D2DFontStyle)))
			{
				var styleKeyName = Enum.GetName(typeof(D2DFontStyle), fontStyle);

				foreach (var fontStretch in Enum.GetValues(typeof(D2DFontStretch)))
				{
					var stretchKeyName = Enum.GetName(typeof(D2DFontStretch), fontStretch);

					foreach (var fontWeight in Enum.GetValues(typeof(D2DFontWeight)))
					{
						var weightKeyName = Enum.GetName(typeof(D2DFontWeight), fontWeight);

						g.DrawText((i++).ToString(), D2DColor.Black, "Arial", 10,
							new D2DRect(rect.X, rect.Y-10, 20, 20));

						g.DrawText($"{styleKeyName}, {stretchKeyName}, {weightKeyName}", D2DColor.Black, "Arial", 16,
							(D2DFontWeight)fontWeight, (D2DFontStyle)fontStyle, (D2DFontStretch)fontStretch,
							rect);

						rect.X += rect.Width + 100;

						if (rect.X > ClientSize.Width - 100)
						{
							rect.X = 100;
							rect.Y += 30;
						}
					}
				}
			}
		}
	}
}