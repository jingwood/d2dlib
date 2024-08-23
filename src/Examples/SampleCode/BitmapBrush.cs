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


using System.ComponentModel;

namespace unvell.D2DLib.Examples.SampleCode
{
	public partial class BitmapBrush : ExampleForm
	{
		public BitmapBrush()
		{
			Text = "HelloWorld - d2dlib Examples";
		}

		D2DBitmap bmp1, bmp2;
		D2DBitmapBrush bmpBrush1, bmpBrush2;
		D2DTextFormat textFormat, textFormatShadow;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			bmp1 = Device.LoadBitmap(Properties.Resources.IMGP6873);
			bmp2 = Device.CreateBitmapFromGDIBitmap(Properties.Resources.Flowers);

			bmpBrush1 = Device.CreateBitmapBrush(bmp1, D2DExtendMode.Clamp, D2DExtendMode.Clamp);
			bmpBrush2 = Device.CreateBitmapBrush(bmp2, D2DExtendMode.Wrap, D2DExtendMode.Wrap);

			textFormat = Device.CreateTextFormat("Arial", 64, D2DFontWeight.ExtraBlack, D2DFontStyle.Normal,
				D2DFontStretch.Normal, DWriteTextAlignment.Center, DWriteParagraphAlignment.Center);
		}

		protected override void OnRender(D2DGraphics g)
		{
			var bounds = this.ClientRectangle;

			D2DRect rect = new D2DRect(bounds.X + 100, bounds.Y + 200, bounds.Width - 200, bounds.Height - 400);

			g.FillEllipse(new D2DEllipse(rect), bmpBrush1);

			using (var shadowBrush = Device.CreateSolidColorBrush(new D2DColor(0.5f, D2DColor.Black)))
			{
				var rectShadow = rect;
				rectShadow.Offset(5, 5);
				g.DrawText("Draw text with bitmap brush", shadowBrush, textFormat, rectShadow);
			}

			g.DrawText("Draw text with bitmap brush", bmpBrush2, textFormat, rect);

			//g.DrawRectangle(rect, D2DColor.Red);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			this.bmpBrush1?.Dispose();
			this.bmpBrush2?.Dispose();
			this.bmp1?.Dispose();
			this.bmp2?.Dispose();
		}
	}

}
