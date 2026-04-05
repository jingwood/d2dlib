/*
 * MIT License
 *
 * Copyright (c) 2009-2024 Jingwood, unvell.com. All right reserved.
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

namespace unvell.D2DLib.Examples.Demos
{
	class PerspectivePanelDemo : DemoForm
	{
		private D2DPanel panel;
		private D2DBitmap image;

		private float angleY = 0f;
		private float angleX = 0f;
		private bool isDragging = false;
		private System.Drawing.Point lastMouse;

		// Simulated "buttons" on the panel
		private readonly D2DRect[] buttons = new D2DRect[]
		{
			new D2DRect(30, 30, 140, 40),
			new D2DRect(30, 90, 140, 40),
			new D2DRect(30, 150, 140, 40),
		};
		private readonly string[] buttonLabels = { "Button 1", "Button 2", "Button 3" };
		private int hoveredButton = -1;

		public PerspectivePanelDemo()
		{
			Text = "3D Perspective Panel Demo - d2dlib Examples";
			Size = new System.Drawing.Size(1000, 700);
			AnimationDraw = false;
			ShowFPS = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			image = Device.LoadBitmap(Properties.Resources.IMGP6873);
			panel = new D2DPanel(Device, 400, 300);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Right)
			{
				isDragging = true;
				lastMouse = e.Location;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (e.Button == MouseButtons.Right)
			{
				isDragging = false;
			}

			// Click test
			if (e.Button == MouseButtons.Left && hoveredButton >= 0)
			{
				MessageBox.Show($"Clicked: {buttonLabels[hoveredButton]}",
					"3D Hit Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (isDragging)
			{
				float dx = e.X - lastMouse.X;
				float dy = e.Y - lastMouse.Y;
				angleY += dx * 0.5f;
				angleX -= dy * 0.5f;

				// Clamp
				angleX = Math.Max(-80, Math.Min(80, angleX));
				angleY = Math.Max(-80, Math.Min(80, angleY));

				lastMouse = e.Location;
				Invalidate();
			}

			// Hit test through the 3D transform
			float panelX = (ClientSize.Width - panel.Width) * 0.5f;
			float panelY = (ClientSize.Height - panel.Height) * 0.5f;
			var local = panel.ScreenToLocal(e.X, e.Y, panelX, panelY);

			int newHovered = -1;
			if (local.HasValue)
			{
				for (int i = 0; i < buttons.Length; i++)
				{
					if (local.Value.X >= buttons[i].left && local.Value.X <= buttons[i].right &&
						local.Value.Y >= buttons[i].top && local.Value.Y <= buttons[i].bottom)
					{
						newHovered = i;
						break;
					}
				}
			}

			if (newHovered != hoveredButton)
			{
				hoveredButton = newHovered;
				Invalidate();
			}
		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			g.Clear(new D2DColor(0.15f, 0.15f, 0.2f));

			// Update panel rotation
			panel.Rotation = new D2DVector3(angleX, angleY, 0);

			// Draw content onto the panel
			var pg = panel.BeginRender();

			// Background
			pg.FillRectangle(new D2DRect(0, 0, panel.Width, panel.Height),
				new D2DColor(0.9f, 0.92f, 0.96f));

			// Border
			pg.DrawRectangle(new D2DRect(0, 0, panel.Width, panel.Height),
				new D2DColor(0.5f, 0.5f, 0.6f), 2f);

			// Title bar
			pg.FillRectangle(new D2DRect(0, 0, panel.Width, 24),
				new D2DColor(0.3f, 0.4f, 0.7f));
			pg.DrawText("3D Panel", D2DColor.White, "Segoe UI", 12,
				new D2DRect(8, 4, 200, 20));

			// Image thumbnail
			if (image != null)
			{
				pg.DrawBitmap(image, new D2DRect(200, 30, 170, 120));
			}

			// Buttons
			for (int i = 0; i < buttons.Length; i++)
			{
				var rect = buttons[i];
				var isHovered = (i == hoveredButton);

				pg.FillRectangle(rect, isHovered
					? new D2DColor(0.3f, 0.5f, 0.9f)
					: new D2DColor(0.4f, 0.55f, 0.8f));
				pg.DrawRectangle(rect, new D2DColor(0.2f, 0.3f, 0.6f), 1f);
				pg.DrawText(buttonLabels[i], D2DColor.White, "Segoe UI", 13,
					new D2DRect(rect.left + 10, rect.top + 10, rect.Width, rect.Height));
			}

			// Info text
			pg.DrawText("Image + Buttons inside 3D panel",
				new D2DColor(0.3f, 0.3f, 0.3f), "Segoe UI", 11,
				new D2DRect(200, 160, 200, 20));

			// Footer
			pg.DrawText($"Rotation: X={angleX:F1}  Y={angleY:F1}",
				new D2DColor(0.4f, 0.4f, 0.4f), "Segoe UI", 10,
				new D2DRect(10, 270, 300, 20));

			panel.EndRender();

			// Draw the panel with 3D perspective at center of window
			float panelX = (ClientSize.Width - panel.Width) * 0.5f;
			float panelY = (ClientSize.Height - panel.Height) * 0.5f;
			panel.Draw(g, panelX, panelY);

			// Draw instructions
			g.DrawText("Right-drag to rotate  |  Left-click buttons (with 3D hit test)",
				D2DColor.White, "Segoe UI", 14,
				new D2DRect(10, 10, ClientSize.Width - 20, 30));

			string hitInfo = hoveredButton >= 0
				? $"Hovering: {buttonLabels[hoveredButton]}"
				: "No button hovered";
			g.DrawText(hitInfo, new D2DColor(0.7f, 0.8f, 1f), "Segoe UI", 13,
				new D2DRect(10, ClientSize.Height - 40, 400, 30));
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			panel?.Dispose();
			image?.Dispose();
		}
	}
}
