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

namespace unvell.D2DLib.WinForm
{
	public class D2DControl : System.Windows.Forms.Control
	{
		private D2DDevice? device;

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

		private D2DGraphics? graphics;

		private int currentFps = 0;
		private int lastFps = 0;
		public bool ShowFPS { get; set; }
		private DateTime lastFpsUpdate = DateTime.UtcNow;

		protected override void CreateHandle()
		{
			base.CreateHandle();

			this.DoubleBuffered = false;
		
			if (this.device == null)
			{
				this.device = D2DDevice.FromHwnd(this.Handle);
			}

			this.graphics = new D2DGraphics(this.device);
		}

		private D2DBitmap? backgroundImage = null;

		public new D2DBitmap? BackgroundImage
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

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e) { }

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			Assumes.NotNull(this.graphics);

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
				if (this.lastFpsUpdate.Second != DateTime.UtcNow.Second)
				{
					this.lastFps = this.currentFps;
					this.currentFps = 0;
					this.lastFpsUpdate = DateTime.UtcNow;
				}
				else
				{
					this.currentFps++;
				}

				string info = string.Format("{0} fps", this.lastFps);
				System.Drawing.SizeF size = e.Graphics.MeasureString(info, Font, Width);
				e.Graphics.DrawString(info, Font, System.Drawing.Brushes.Black, ClientRectangle.Right - size.Width - 10, 5);
			}

			this.graphics.EndRender();
		}

		protected override void DestroyHandle()
		{
			base.DestroyHandle();
			this.device?.Dispose();
		}

		protected virtual void OnRender(D2DGraphics g) { }

		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			switch (m.Msg)
			{
				//case (int)unvell.Common.Win32Lib.Win32.WMessages.WM_PAINT:
				//	base.WndProc(ref m);
				//	break;

				case (int)unvell.D2DLib.WinForm.Win32.WMessages.WM_ERASEBKGND:
					break;

				case (int)unvell.D2DLib.WinForm.Win32.WMessages.WM_SIZE:
					base.WndProc(ref m);
					if (this.device != null) this.device.Resize();
					break;

				default:
					base.WndProc(ref m);
					break;
			}
		}

		public new void Invalidate()
		{
			base.Invalidate(false);
		}
	}
}
