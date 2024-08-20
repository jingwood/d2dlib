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
	public partial class MeasureAndDrawStringWithCachedFontFormat : ExampleForm
	{
		private static readonly Font font1 = new Font("Times New Roman", 34f, FontStyle.Italic);

		public MeasureAndDrawStringWithCachedFontFormat()
		{
			Text = "Measure and draw string performance comparison";

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

		SimpleCheckBox checkbox = new SimpleCheckBox();


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
				var rectSize = new D2DSize(500, 200);

				if (checkbox.IsChecked)
				{
					g.MeasureText(str, fontFormat, ref rectSize); //cached textFormat
				}
				else
				{
					var sz = g.MeasureText(str, font1.Name, font1.Size, new D2DSize(999, 200)); //not cached
				}

				rect.left = (rect.left + rect.Width + rectSize.width) % (ClientSize.Width);
				rect.top = (rect.top + rect.Height + rectSize.height) % (ClientSize.Height);
				rect.Width = rectSize.width;
				rect.Height = rectSize.height;

				if (checkbox.IsChecked)
				{
					g.DrawText(str, brush, fontFormat, ref rect); //45fps + measure text not cached, 64fp with measure text cached
				}
				else
				{
					g.DrawText(str, D2DColor.BlueViolet, font1.Name, font1.Size, rect); //32 fps + measure text not cached
				}
			}


			// UI
			//g.FillRectangle(10, 10, 600, 80, D2DColor.White);
			checkbox.OnRender(g);

			g.DrawStrokedText($"{this.FPS} FPS", 20, 60, D2DColor.White, 2, D2DColor.DarkBlue, "Arial", 24, D2DFontWeight.SemiBold);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			checkbox.OnMouseDown(e);
			Invalidate();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			brush.Dispose();
			brushBack.Dispose();
			fontFormat.Dispose();
		}
	}

	public class SimpleCheckBox
	{

		public SimpleCheckBox()
		{
			this.CheckBounds = new Rectangle(200, 30, 40, 40);
			this.Bounds = new Rectangle(this.CheckBounds.X - Padding, this.CheckBounds.Y - Padding, 400, this.CheckBounds.Height + Padding * 2);
		}

		public int Padding { get; set; } = 20;
		public Rectangle Bounds { get; set; }
		public Rectangle CheckBounds { get; set; }

		public bool IsChecked { get; set; } = true;

		public void OnRender(D2DGraphics g)
		{
			g.FillRectangle(Bounds, new D2DColor(.9f, D2DColor.Silver));
			g.FillRectangle(CheckBounds, D2DColor.White);
			g.DrawRectangle(CheckBounds, D2DColor.Black);
			g.DrawText("Using Cached Font Format", D2DColor.Black, "Arial", 24f, CheckBounds.Right + Padding, Bounds.Y + Bounds.Height / 2 - 12);

			if (IsChecked)
			{
				g.DrawLines(new Vector2[] {
					new Vector2(CheckBounds.X + 4, CheckBounds.Y + CheckBounds.Height / 2),
					new Vector2(CheckBounds.X + CheckBounds.Width / 2, CheckBounds.Bottom - 4),
					new Vector2(CheckBounds.Right - 4, CheckBounds.Y + 4),
				}, D2DColor.DarkSeaGreen, 3);
			}
		}

		public virtual void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && CheckBounds.Contains(e.Location))
			{
				IsChecked = !IsChecked;
			}
		}
	}
}

