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
			brush = Device.CreateSolidColorBrush(D2DColor.BlueViolet);
			brushBack = Device.CreateSolidColorBrush(D2DColor.DarkGray);
			textFormat = Device.CreateFontFormat(Font.Name, 34);

			szString = new D2DSize(60, 20);
			dispstrings = new List<string>();
			AnimationDraw = true;
			ShowFPS = true;
		}


		D2DSize szString;
		D2DSolidColorBrush brush;
		D2DSolidColorBrush brushBack;
		D2DFontFormat textFormat;
		int ColCount;
		int RowCount;
		List<string> dispstrings;
		D2DPoint ptLeftTop = new D2DPoint(0, 0);

		private void ReSize()
        {
			ColCount = (int)Math.Floor(Size.Width / szString.width);
			RowCount = (int)Math.Floor(Size.Height / szString.height);
		}

		private void CreateStrings()
        {
			ReSize();

			dispstrings.Clear();
			for (int count = ColCount * RowCount; count >= 0; count--)
            {
				dispstrings.Add(Guid.NewGuid().ToString().Substring(20, 8));
			}
		}

        protected override void OnFrame()
        {
            base.OnFrame();
			CreateStrings();
		}

        protected override void OnRender(D2DGraphics g)
		{
			g.FillRectangle(ClientRectangle, brushBack);
			int pos = 0;
			for (int row = 0; row < RowCount; row++)
            {
				ptLeftTop.Y = szString.height * row + 10;
				for (int col = 0; col < ColCount; col++)
                {
					ptLeftTop.X = szString.width * col + 30;
					D2DRect rcText = new D2DRect(ptLeftTop, szString);
					//g.DrawText(dispstrings[pos++], D2DColor.Black, "Times New Roman", 34, rcText);
					g.DrawText(dispstrings[pos++], brush, textFormat, rcText);
					//g.DrawText(dispstrings[pos++], D2DColor.BlueViolet, Font.Name, Font.Size * 96F / 72F, rcText);
				}
			}
		}
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
			brush.Dispose();
			brushBack.Dispose();
			textFormat.Dispose();
		}
        
    }
}

