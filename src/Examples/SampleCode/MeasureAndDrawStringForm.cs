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
	public partial class MeasureAndDrawStringForm : ExampleForm
	{
		private static readonly Font font1 = new Font("Times New Roman", 34f, FontStyle.Italic);

		public MeasureAndDrawStringForm()
		{
			Text = "Measure and draw string";

			Size = new Size(1280, 800);
			brush = Device.CreateSolidColorTextBrush(D2DColor.BlueViolet);
			brushBack = Device.CreateSolidColorBrush(D2DColor.DarkGray);
			fontFormat = Device.CreateFontFormat(Font.Name, 34);

			szString = new D2DSize(60, 20);
			dispstrings = new List<string>(2000);
			AnimationDraw = true;
			ShowFPS = true;

			CreateStrings();
		}


		D2DSize szString;
		D2DSolidColorTextBrush brush;
		D2DSolidColorBrush brushBack;
		D2DFontFormat fontFormat;

		List<string> dispstrings;
		D2DPoint ptLeftTop = new D2DPoint(0, 0);
		D2DRect rect = new D2DRect(0, 0, 0, 0);

		private void CreateStrings()
        {
			for (int count = 0; count < 1000; ++count)
            {
				dispstrings.Add(Guid.NewGuid().ToString().Substring(20, 8));
			}
		}

        protected override void OnFrame()
        {
            base.OnFrame();
		}

        protected override void OnRender(D2DGraphics g)
		{
			g.FillRectangle(ClientRectangle, brushBack);
			var ratio = (double)(ClientRectangle.Width) / ClientRectangle.Height;
			
			for (int i = 0; i < dispstrings.Count; i++)
            {
				var str = dispstrings[i];
				var sz = g.MeasureText(str, fontFormat, new D2DSize(500, 200)); //cached textFormat
				//var sz = g.MeasureText(str, font1.Name, font1.Size, new D2DSize(999,200)); //not cached

				rect.left = (rect.left + rect.Width + sz.width) % (ClientSize.Width );
				rect.top = (rect.top + rect.Height + sz.height) % (ClientSize.Height );
				rect.Width = sz.width;
				rect.Height = sz.height;

				//g.DrawText(str, D2DColor.BlueViolet, font1.Name, font1.Size, rect); //32 fps + measure text not cached
				g.DrawText(str, brush, fontFormat, rect ); //45fps + measure text not cached, 64fp with measure text cached
			}
		}
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
			brush.Dispose();
			brushBack.Dispose();
			fontFormat.Dispose();
		}
        
    }
}

