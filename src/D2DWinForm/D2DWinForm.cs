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
using System.Windows.Forms;

namespace unvell.D2DLib.WinForm
{
	public class D2DForm : Form
	{
		private D2DDevice device;
		public D2DDevice Device
		{
			get
			{
				var hwnd = this.Handle;
				if (this.device == null)
				{
					this.device = D2DDevice.FromHwnd(hwnd);
				}
				return this.device;
			}
		}

		private D2DBitmap backgroundImage = null;

		public new D2DBitmap BackgroundImage
		{
			get { return this.backgroundImage; }
			set
			{
				if (this.backgroundImage != value)
				{
					if (this.backgroundImage != null)
					{
						this.backgroundImage.Dispose();
					}
					this.backgroundImage = value;
					Invalidate();
				}
			}
		}

		private D2DGraphics graphics;

		private int currentFps = 0;
		private int lastFps = 0;
		public bool ShowFPS { get; set; }
		private DateTime lastFpsUpdate = DateTime.Now;

		private Timer timer = new Timer() { Interval = 10 };
		public bool EscapeKeyToClose { get; set; } = true;

		private bool animationDraw;
		public bool AnimationDraw
		{
			get { return this.animationDraw; }
			set
			{
				this.animationDraw = value;

				if (!this.animationDraw)
				{
					if (timer.Enabled) timer.Stop();
				}
				else
				{
					if (!timer.Enabled) timer.Start();
				}
			}
		}

		protected bool SceneChanged { get; set; }

		protected override void CreateHandle()
		{
			base.CreateHandle();

			this.DoubleBuffered = false;

			if (this.device == null)
			{
				this.device = D2DDevice.FromHwnd(this.Handle);
			}

			this.graphics = new D2DGraphics(this.device);
			this.graphics.SetDPI(96, 96);

			this.timer.Tick += (ss, ee) =>
			{
				if (AnimationDraw || SceneChanged)
				{
					OnFrame();
					Invalidate();
					SceneChanged = false;
				}
			};
		}

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
		{
			// prevent the .NET windows form to paint the original background
		}
		
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			if (this.DesignMode)
			{
				e.Graphics.Clear(System.Drawing.Color.Black);
				e.Graphics.DrawString("D2DLib windows form cannot render in design time.", this.Font, System.Drawing.Brushes.White, 10, 10);
			}
			else
			{
				if (this.backgroundImage != null)
				{
					this.graphics.BeginRender(this.backgroundImage);
				}
				else
				{
					this.graphics.BeginRender(D2DColor.FromGDIColor(this.BackColor));
				}

				OnRender(this.graphics);

				if (ShowFPS)
				{
					if (this.lastFpsUpdate.Second != DateTime.Now.Second)
					{
						this.lastFps = this.currentFps;
						this.currentFps = 0;
						this.lastFpsUpdate = DateTime.Now;
					}
					else
					{
						this.currentFps++;
					}

					string fpsInfo = string.Format("{0} fps", lastFps);
					System.Drawing.SizeF size = e.Graphics.MeasureString(fpsInfo, Font, Width);
					this.graphics.DrawText(fpsInfo, unvell.D2DLib.D2DColor.Silver, Font,
						new System.Drawing.PointF(ClientRectangle.Right - size.Width - 10, 5));
				}

				this.graphics.EndRender();

				if (this.animationDraw && !this.timer.Enabled)
				{
					this.timer.Start();
				}
			}
		}

		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			switch (m.Msg)
			{
				case (int)Win32.WMessages.WM_ERASEBKGND:
					break;

				case (int)Win32.WMessages.WM_SIZE:
					base.WndProc(ref m);
					if (this.device != null)
					{
						this.device.Resize();
						Invalidate(false);
					}
					break;

				case (int)Win32.WMessages.WM_DESTROY:
					if (this.backgroundImage != null) this.backgroundImage.Dispose();
					if (this.device != null) this.device.Dispose();
					base.WndProc(ref m);
					break;

				default:
					base.WndProc(ref m);
					break;
			}
		}

		protected virtual void OnRender(D2DGraphics g) { }

		protected virtual void OnFrame() { }

		public new void Invalidate()
		{
			base.Invalidate(false);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			switch (e.KeyCode)
			{
				case Keys.Escape:
					if (EscapeKeyToClose) Close();
					break;
			}
		}
	}
}
