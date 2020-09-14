/*
 * MIT License
 * 
 * Copyright (c) 2009-2018 Jingwood, unvell.com. All right reserved.
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
using System.Drawing;
using System.Windows.Forms;
using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples
{
	/// <summary>
	/// Windows Form supports both GDI+ rendering and Direct2D hardware accelerated rendering.
	/// </summary>
	/// <remarks>
	/// Usage:
	/// 1. Override the method <code>void OnDraw(IAccelerationGraphics ag)</code> to draw anything on windows form.
	/// 2. Toggle property <code>HardwardAcceleration</code> to switch between GDI+ and Direct2D rendering.
	/// </remarks>
	class D2DHybridForm : Form
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

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (string.IsNullOrEmpty(this.Text))
			{
				var str = this.GetType().Name;
				if (str.EndsWith("Form")) str = str.Substring(0, str.Length - 4);
				this.Text = str;
			}
		}

		private D2DGraphics graphics;

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

		private Direct2DGraphics d2dGraphics;

		private GDIGraphics gdiGraphics;

		private bool hardwardAcceleration = true;

		public bool HardwardAcceleration
		{
			get { return this.hardwardAcceleration; }
			set
			{
				this.hardwardAcceleration = value;
				this.DoubleBuffered = !this.hardwardAcceleration;
			}
		}

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
		{
			if (!this.hardwardAcceleration)
			{
				base.OnPaintBackground(e);
			}
		}

		public D2DHybridForm()
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (HardwardAcceleration)
			{
				this.DoubleBuffered = false;

				if (this.d2dGraphics == null)
				{
					this.d2dGraphics = new Direct2DGraphics(this.graphics);
				}
				else
				{
					this.d2dGraphics.g = this.graphics;
				}

				this.graphics.BeginRender(D2DColor.FromGDIColor(this.BackColor));

				this.OnDraw(this.d2dGraphics);

				this.graphics.EndRender();
			}
			else
			{
				this.DoubleBuffered = true;

				if (this.gdiGraphics == null)
				{
					this.gdiGraphics = new GDIGraphics(e.Graphics);
				}
				else
				{
					this.gdiGraphics.g = e.Graphics;
				}

				this.OnDraw(this.gdiGraphics);
			}
		}

		/// <summary>
		/// User drawing method. Override this method to draw anything on your form.
		/// </summary>
		/// <param name="ag">Graphics context supports both GDI+ and Direct2D rendering.</param>
		protected virtual void OnDraw(IHybridGraphics ag)
		{
			ag.DrawString("Hello World", Font, Color.Black, 
				this.ClientRectangle.Width / 2 - 30, this.ClientRectangle.Height / 2 - 20);
		}

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case 0x0014 /* WM_ERASEBKGND */:
					if (!this.hardwardAcceleration)
					{
						base.WndProc(ref m);
					}
					break;

				case 0x0005 /* WM_SIZE */:
					base.WndProc(ref m);
					if (this.device != null)
					{
						this.device.Resize();
						Invalidate(false);
					}
					break;

				case 0x0002 /* WM_DESTROY */:
					if (this.device != null) this.device.Dispose();
					base.WndProc(ref m);
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

	unsafe class MemoryBitmap : IDisposable
	{
		private uint* buffer;
		public uint* Buffer { get { return this.buffer; } }
		public IntPtr BufferPtr { get { return (IntPtr)this.buffer; } }

		public int BufferSize  { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int PixelCount { get; private set; }

		const int stride = sizeof(uint);

		public MemoryBitmap(int width, int height)
		{
			this.Width = width;
			this.Height = height;
			this.BufferSize = stride * width * height;
			this.PixelCount = this.Width * this.Height;
			this.buffer = (uint*)System.Runtime.InteropServices.Marshal.AllocHGlobal(this.BufferSize);
		}

		~MemoryBitmap()
		{
			this.Dispose();
		}

		public static void CopyBuffer(MemoryBitmap from, MemoryBitmap to)
		{
			CopyBuffer(from.BufferPtr, from.Width, from.Height, to.BufferPtr, to.Width, to.Height, 0, 0,
				Math.Min(from.Width, to.Width), Math.Min(from.Height, to.Height));
		}

		public static void CopyBuffer(IntPtr source, int sourceWidth, int sourceHeight,
			IntPtr destination, int destWidth, int destHeight, int copyLeft, int copyTop, int copyWidth, int copyHeight)
		{
			uint* fromBuffer = (uint*)source;
			uint* toBuffer = (uint*)destination;

			int right = copyLeft + copyWidth, bottom = copyTop + copyHeight;

			uint* fromLine = fromBuffer + copyTop * sourceWidth;
			uint* toLine = toBuffer + copyTop * destWidth;

			for (int y = copyTop; y < bottom; y++)
			{
				uint* from = fromLine;
				uint* to = toLine;

				for (int x = copyLeft; x < right; x++)
				{
					*to = *from;
					to++;
					from++;
				}

				fromLine += sourceWidth;
				toLine += destWidth;
			}
		}

		public void FillRectangle(int x, int y, int w, int h, Color c)
		{
			this.FillRectangle(x, y, w, h, (uint)c.ToArgb());
		}

		public void FillRectangle(int x, int y, int w, int h, uint c)
		{
			uint* line = this.buffer + (y * this.Width);

			for (int i = 0; i < h; i++)
			{
				uint* p = line + x;

				for (int j = 0; j < w; j++)
				{
					*p = c;
					p++;
				}

				line += this.Width;
			}
		}

		public void SetPixel(Point v, Color c)
		{
			this.SetPixel(v, c.ToArgb());
		}

		public void SetPixel(PointF p, int c)
		{
			this.SetPixel((int)(p.X), (int)(p.Y), (uint)c);
		}

		public void SetPixel(int x, int y, Color c)
		{
			this.SetPixel(x, y, (uint)c.ToArgb());
		}

		public void SetPixel(int x, int y, uint c)
		{
			*(this.buffer + (y * this.Width) + x) = c;
		}

		public uint GetPixel(PointF v)
		{
			return this.GetPixel((int)v.X, (int)v.Y);
		}

		public uint GetPixel(int x, int y)
		{
			return *(this.buffer + (y * this.Width) + x);
		}

		public Color GetColor4i(int x, int y)
		{
			return Color.FromArgb((int)this.GetPixel(x, y));
		}

		public Color GetColor4f(int x, int y)
		{
			return Color.FromArgb((int)this.GetPixel(x, y));
		}

		internal void Clear(Color c)
		{
			this.Clear((uint)c.ToArgb());
		}

		internal void Clear(uint c = 0xffffffff)
		{
			uint* p = this.buffer;

			for (int i = 0; i < this.PixelCount; i++)
			{
				*p = c;
				p++;
			}
		}

		public void Dispose()
		{
			if (this.buffer != null)
			{
				try
				{
					System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)this.buffer);
					this.buffer = null;
				}
				catch { }
			}
		}
	}

	class Direct2DGraphics : IHybridGraphics
	{
		internal D2DGraphics g;

		public Direct2DGraphics(D2DGraphics g)
		{
			this.g = g;
		}

		public bool SmoothingMode
		{
			get { return this.g.Antialias; }
			set { this.g.Antialias = value; }
		}

		public void DrawLine(Point p1, Point p2, Color c, float weight = 1)
		{
			this.DrawLine(p1.X, p1.Y, p2.X, p2.Y, c, weight);
		}

		public void DrawLine(PointF p1, PointF p2, Color c, float weight = 1)
		{
			this.DrawLine(p1.X, p1.Y, p2.X, p2.Y, c, weight);
		}

		public void DrawLine(float x1, float y1, float x2, float y2, Color c, float weight = 1f)
		{
			this.g.DrawLine(x1, y1, x2, y2, D2DColor.FromGDIColor(c), weight);
		}

		public void DrawRectangle(float x, float y, float w, float h, Color c, float weight = 1f)
		{
			this.g.DrawRectangle(x, y, w, h, D2DColor.FromGDIColor(c), weight);
		}

		public void FillRectangle(float x, float y, float w, float h, Color c)
		{
			this.g.FillRectangle(new D2DRect(x, y, w, h), D2DColor.FromGDIColor(c));
		}

		public void DrawEllipse(float x, float y, float w, float h, Color c, float weight = 1f)
		{
			this.g.DrawEllipse(x, y, w, h, D2DColor.FromGDIColor(c), weight);
		}

		public void FillEllipse(float x, float y, float w, float h, Color c)
		{
			this.g.FillEllipse(x, y, w, h, D2DColor.FromGDIColor(c));
		}

		public void DrawPolygon(PointF[] ps, Color strokeColor, float weight, Color fillColor)
		{
			D2DPoint[] pt = new D2DPoint[ps.Length];

			for (int i = 0; i < ps.Length; i++)
			{
				pt[i] = new D2DPoint(ps[i].X, ps[i].Y);
			}

			this.g.DrawPolygon(pt, D2DColor.FromGDIColor(strokeColor), weight, D2DDashStyle.Solid, D2DColor.FromGDIColor(fillColor));
		}

		public void DrawString(string text, Font font, Color c, float x, float y)
		{
			this.g.DrawText(text, D2DColor.FromGDIColor(c), font, x, y);
		}

		public void DrawImage(Bitmap bmp, float x, float y, bool alpha = false)
		{
			g.DrawBitmap(bmp, x, y, 1f, alpha);
		}

		public void DrawMemoryBitmap(MemoryBitmap bmp, int x, int y)
		{
			this.DrawMemoryBitmap(x, y, bmp.Width, bmp.Height, bmp.Width * sizeof(uint), bmp.BufferPtr, 0, bmp.BufferSize);
		}

		public void DrawMemoryBitmap(int x, int y, int w, int h, int stride, IntPtr buf, int offset, int length)
		{
			using (var bmp = g.Device.CreateBitmapFromMemory((uint)w, (uint)h, (uint)stride, buf, (uint)offset, (uint)length))
			{
				g.DrawBitmap(bmp, new D2DRect(x, y, w, h));
			}
		}

		public void Flush()
		{
			this.g.Flush();
		}

	}

	class GDIGraphics : IHybridGraphics
	{
		internal Graphics g;

		public GDIGraphics(Graphics g)
		{
			this.g = g;
		}

		public bool SmoothingMode
		{
			get { return this.g.SmoothingMode == System.Drawing.Drawing2D.SmoothingMode.AntiAlias; }
			set { this.g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; }
		}

		public void DrawLine(Point p1, Point p2, Color c, float weight = 1f)
		{
			this.DrawLine(p1.X, p1.Y, p2.X, p2.Y, c, weight);
		}

		public void DrawLine(PointF p1, PointF p2, Color c, float weight = 1)
		{
			this.DrawLine(p1.X, p1.Y, p2.X, p2.Y, c, weight);
		}

		public void DrawLine(float x1, float y1, float x2, float y2, Color c, float weight = 1)
		{
			using (var p = new Pen(c, weight))
			{
				this.g.DrawLine(p, x1, y1, x2, y2);
			}
		}

		public void DrawRectangle(float x, float y, float w, float h, Color c, float weight = 1f)
		{
			using (var p = new Pen(c, weight))
			{
				this.g.DrawRectangle(p, x, y, w, h);
			}
		}

		public void FillRectangle(float x, float y, float w, float h, Color c)
		{
			using (var b = new SolidBrush(c))
			{
				this.g.FillRectangle(b, x, y, w, h);
			}
		}

		public void DrawEllipse(float x, float y, float w, float h, Color c, float weight = 1f)
		{
			using (var p = new Pen(c, weight))
			{
				this.g.DrawEllipse(p, x, y, w, h);
			}
		}

		public void FillEllipse(float x, float y, float w, float h, Color c)
		{
			using (var b = new SolidBrush(c))
			{
				this.g.FillEllipse(b, x, y, w, h);
			}
		}

		public void DrawPolygon(PointF[] ps, Color strokeColor, float weight, Color fillColor)
		{
			using (var path = new System.Drawing.Drawing2D.GraphicsPath())
			{
				PointF[] pt = new PointF[ps.Length];

				for (int i = 0; i < ps.Length; i++)
				{
					pt[i] = ps[i];
				}

				path.AddLines(pt);

				if (fillColor.A > 0)
				{
					using (var b = new SolidBrush(fillColor))
					{
						g.FillPath(b, path);
					}
				}

				if (strokeColor.A > 0 && weight > 0)
				{
					using (var p = new Pen(strokeColor, weight))
					{
						g.DrawPath(p, path);
					}
				}
			}
		}

		public void DrawString(string text, Font font, Color c, float x, float y)
		{
			using (var b = new SolidBrush(c))
			{
				this.g.DrawString(text, font, b, x, y);
			}
		}

		public void DrawImage(Bitmap bmp, float x, float y, bool alpha = false)
		{
			g.DrawImage(bmp, x, y);
		}

		public void DrawMemoryBitmap(MemoryBitmap mb, int x, int y)
		{
			this.DrawMemoryBitmap(x, y, mb.Width, mb.Height, mb.Width * sizeof(uint), mb.BufferPtr, 0, mb.BufferSize);
		}

		public void DrawMemoryBitmap(int x, int y, int w, int h, int stride, IntPtr buf, int offset, int length)
		{
			using (var bmp = new Bitmap(w, h, stride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, buf))
			{
				g.DrawImage(bmp, 0, 0);
			}
		}

		public void Flush()
		{
		}

	
	}

	interface IHybridGraphics
	{
		bool SmoothingMode { get; set; }
		void DrawLine(Point p1, Point p2, Color c, float weight = 1f);
		void DrawLine(float x1, float y1, float x2, float y2, Color c, float weight = 1f);
		void DrawLine(PointF p1, PointF p2, Color c, float weight = 1f);
		void DrawRectangle(float x, float y, float w, float h, Color c, float weight = 1f);
		void FillRectangle(float x, float y, float w, float h, Color c);
		void DrawEllipse(float x, float y, float w, float h, Color c, float weight = 1f);
		void FillEllipse(float x, float y, float w, float h, Color c);
		void DrawString(string text, Font font, Color c, float x, float y);
		void DrawImage(Bitmap bmp, float x, float y, bool alpha = false);
		void DrawMemoryBitmap(int x, int y, int w, int h, int stride, IntPtr buf, int offset, int length);
		void DrawMemoryBitmap(MemoryBitmap mb, int x, int y);
		void DrawPolygon(PointF[] ps, Color strokeColor, float weight, Color fillColor);
		void Flush();
	}

}
