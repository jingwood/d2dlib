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

using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.Demos
{
	public partial class Whiteboard : DemoForm
	{
		public Whiteboard()
		{
			Text = "Whiteboard - d2dlib Examples";
			BackColor = Color.White;

			StartPosition = FormStartPosition.Manual;
			WindowState = FormWindowState.Normal;
			FormBorderStyle = FormBorderStyle.None;
			Location = new Point(0, 0);
			DesktopLocation = new Point(0, 0);
			Size = Screen.GetBounds(this).Size;

			ShowFPS = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.recreateMemoryGraphics();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			switch (e.KeyCode)
			{
				case Keys.Oemtilde:
				case Keys.E:
					this.lastPenColor = D2DColor.White;
					this.penColor = D2DColor.White; Invalidate(); break;

				case Keys.D1:
					this.lastPenColor = D2DColor.Blue;
					this.penColor = D2DColor.Blue; Invalidate(); break;
				case Keys.D2:
					this.lastPenColor = D2DColor.Red;
					this.penColor = D2DColor.Red; Invalidate(); break;
				case Keys.D3:
					this.lastPenColor = D2DColor.Green;
					this.penColor = D2DColor.Green; Invalidate(); break;
				case Keys.D4:
					this.lastPenColor = D2DColor.Yellow;
					this.penColor = D2DColor.Yellow; Invalidate(); break;
				case Keys.D5:
					this.lastPenColor = D2DColor.Pink;
					this.penColor = D2DColor.Pink; Invalidate(); break;

				case Keys.R:
					this.memg.BeginRender();
					this.memg.DrawRectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height, D2DColor.White);
					this.memg.EndRender();
					break;
			}

			// when we're in the eraser, we want to invalidate since the eraser is animated
			this.AnimationDraw = (this.penColor == D2DColor.White);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.recreateMemoryGraphics();
		}

		private void recreateMemoryGraphics()
		{
			this.memg?.Dispose();
			this.memg = this.Device.CreateBitmapGraphics(new D2DSize(this.ClientSize.Width, this.ClientSize.Height));
		}

		private D2DBitmapGraphics memg;
		private Vector2 lastPoint, cursorPoint;
		private bool isDrawing;
		private Size penSize = new Size(5, 5);
		private D2DColor penColor = D2DColor.Blue;  // use white as eraser, other else as normal pen
		private D2DColor lastPenColor = D2DColor.Blue;  // backup pen color before switch to eraser
		private bool showGettingStart = true;

		protected override void OnRender(D2DGraphics g)
		{
			// draw the memory bitmap
			g.DrawBitmap(memg, this.ClientSize.Width, this.ClientSize.Height);

			// draw tips
			g.DrawText("Tips:\n   Press 1 to 5 to switch color, E or Right to erase\n   Scroll to change pen size\n    R to clear whiteboard", D2DColor.Black, this.Font, 10, 10);

			if (showGettingStart)
			{
				g.DrawTextCenter("Draw something...", D2DColor.Goldenrod, SystemFonts.DefaultFont.Name, 36, ClientRectangle);
			}

			// draw cursor
			this.drawCursor(g, this.cursorPoint);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.penColor = this.lastPenColor;
			}
			else if (e.Button == MouseButtons.Right)
			{
				// switch to eraser
				// backup last pen color
				if (this.penColor != D2DColor.White)
				{
					this.lastPenColor = this.penColor;
				}
				this.penColor = D2DColor.White;
			}

			this.isDrawing = true;
			this.lastPoint = new Vector2(e.X, e.Y);
			this.cursorPoint = new Vector2(e.X, e.Y);
			drawPen(new Vector2(e.Location.X, e.Location.Y));
			this.showGettingStart = false;

			// when we're in the eraser, we want to invalidate since the eraser is animated
			this.AnimationDraw = (this.penColor == D2DColor.White);

			this.Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			this.cursorPoint = new Vector2(e.X, e.Y);
			if (this.isDrawing)
			{
				drawPen(new Vector2(e.Location.X, e.Location.Y));
			}
			else
			{
				cursorPoint = new Vector2(e.X, e.Y);
			}
			this.Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.isDrawing = false;
			this.cursorPoint = new Vector2(e.X, e.Y);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			Cursor.Hide();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			Cursor.Show();
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);

			int diff = (int)((float)e.Delta / 60.0f);
			this.penSize.Width += diff;
			this.penSize.Height += diff;

			if (this.penSize.Width > 100) this.penSize.Width = 100;
			if (this.penSize.Height > 100) this.penSize.Height = 100;
			if (this.penSize.Width < 1) this.penSize.Width = 1;
			if (this.penSize.Height < 1) this.penSize.Height = 1;

			this.Invalidate();
		}

		private void drawPen(Vector2 currentPoint)
		{
			var diff = new Vector2(currentPoint.X - this.lastPoint.X, currentPoint.Y - this.lastPoint.Y);

			memg.BeginRender();
			D2DEllipse ellipse = new D2DEllipse(D2DPoint.Zero, this.penSize);
			D2DRect rect = new D2DRect(0, 0, this.penSize.Width * 2, this.penSize.Height * 2);

			float len = (float)Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);
			float seg = 1.0f / len;

			// draw a continuous line 
			for (float t = 0; t < 1; t += seg)
			{
				float x = this.lastPoint.X + diff.X * t, y = this.lastPoint.Y + diff.Y * t;

				if (this.penColor == D2DColor.White)
				{
					rect.X = x - this.penSize.Width;
					rect.Y = y - this.penSize.Height;
					memg.FillRectangle(rect, this.penColor);
				}
				else
				{
					ellipse.origin = new D2DPoint(x, y);
					memg.FillEllipse(ellipse, this.penColor);
				}
			}

			memg.EndRender();

			this.lastPoint = currentPoint;
		}

		private readonly float[] eraserPenDashes = new[] { 2f, 2f };

		private float eraserDashOffset = 0.0f;

		private void drawCursor(D2DGraphics g, Vector2 p)
		{
			if (this.penColor == D2DColor.White)
			{
				// when current color is white, draw an eraser
				var eraserRectangle = new D2DRect(p.X - this.penSize.Width, p.Y - this.penSize.Height, this.penSize.Width * 2, this.penSize.Height * 2);

				using (var pen = Device.CreatePen(D2DColor.Black, D2DDashStyle.Custom, eraserPenDashes, eraserDashOffset))
				{
					g.DrawRectangle(eraserRectangle, pen, 2);
				}

				eraserDashOffset += 0.1f;
			}
			else
			{
				// else draw pen
				g.DrawEllipse(p, this.penSize, this.penColor, 2.0f);
			}
		}
	}
}
