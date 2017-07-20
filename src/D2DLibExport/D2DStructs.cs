using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using FLOAT = System.Single;
using UINT32 = System.UInt32;

namespace unvell.D2DLib
{
	#region Color
	[Serializable]
	//[StructLayout(LayoutKind.Sequential)]
	public struct D2DColor
	{
		public FLOAT r;
		public FLOAT g;
		public FLOAT b;
		public FLOAT a;

		public D2DColor(FLOAT r, FLOAT g, FLOAT b)
			: this(1, r, g, b)
		{
		}

		public D2DColor(FLOAT a, FLOAT r, FLOAT g, FLOAT b)
		{
			this.a = a;
			this.r = r;
			this.g = g;
			this.b = b;
		}

		public D2DColor(FLOAT alpha, D2DColor color)
		{
			this.a = alpha;
			this.r = color.r;
			this.g = color.g;
			this.b = color.b;
		}

		public static D2DColor FromGDIColor(System.Drawing.Color gdiColor)
		{
			return new D2DColor(gdiColor.A / 255f, gdiColor.R / 255f, 
				gdiColor.G / 255f, gdiColor.B / 255f);
		}

		public static System.Drawing.Color ToGDIColor(D2DColor d2color)
		{
			int a = (int)(d2color.a * 255);
			int r = (int)(d2color.r * 255);
			int g = (int)(d2color.g * 255);
			int b = (int)(d2color.b * 255);

			if (a > 255) a = 255; else if (a < 0) a = 0;
			if (r > 255) r = 255; else if (r < 0) r = 0;
			if (g > 255) g = 255; else if (g < 0) g = 0;
			if (b > 255) b = 255; else if (b < 0) b = 0;

			return System.Drawing.Color.FromArgb(a, r, g, b);
		}

		public static readonly D2DColor Transparent = new D2DColor(0, 0, 0, 0);

		public static readonly D2DColor Black = new D2DColor(0, 0, 0);
		public static readonly D2DColor DimGray = D2DColor.FromGDIColor(System.Drawing.Color.DimGray);
		public static readonly D2DColor Gray = D2DColor.FromGDIColor(System.Drawing.Color.Gray);
		public static readonly D2DColor DarkGray = D2DColor.FromGDIColor(System.Drawing.Color.DarkGray);
		public static readonly D2DColor Silver = D2DColor.FromGDIColor(System.Drawing.Color.Silver);
		public static readonly D2DColor GhostWhite = D2DColor.FromGDIColor(System.Drawing.Color.GhostWhite);
		public static readonly D2DColor LightGray = D2DColor.FromGDIColor(System.Drawing.Color.LightGray);
		public static readonly D2DColor White = D2DColor.FromGDIColor(System.Drawing.Color.White);

		public static readonly D2DColor Red = D2DColor.FromGDIColor(System.Drawing.Color.Red);
		public static readonly D2DColor DarkRed = D2DColor.FromGDIColor(System.Drawing.Color.DarkRed);
		public static readonly D2DColor Coral = D2DColor.FromGDIColor(System.Drawing.Color.Coral);
		public static readonly D2DColor LightCoral = D2DColor.FromGDIColor(System.Drawing.Color.LightCoral);

		public static readonly D2DColor Yellow = D2DColor.FromGDIColor(System.Drawing.Color.Yellow);
		public static readonly D2DColor Gold = D2DColor.FromGDIColor(System.Drawing.Color.Gold);
		public static readonly D2DColor Goldenrod = D2DColor.FromGDIColor(System.Drawing.Color.Goldenrod);
		public static readonly D2DColor Orange = D2DColor.FromGDIColor(System.Drawing.Color.Orange);
		public static readonly D2DColor DarkOrange = D2DColor.FromGDIColor(System.Drawing.Color.DarkOrange);

		public static readonly D2DColor LawnGreen = D2DColor.FromGDIColor(System.Drawing.Color.LawnGreen);
		public static readonly D2DColor LightGreen = D2DColor.FromGDIColor(System.Drawing.Color.LightGreen);
		public static readonly D2DColor Green = D2DColor.FromGDIColor(System.Drawing.Color.Green);
		public static readonly D2DColor DarkGreen = D2DColor.FromGDIColor(System.Drawing.Color.DarkGreen);

		public static readonly D2DColor LightBlue = D2DColor.FromGDIColor(System.Drawing.Color.LightBlue);
		public static readonly D2DColor Blue = D2DColor.FromGDIColor(System.Drawing.Color.Blue);
		public static readonly D2DColor DarkBlue = D2DColor.FromGDIColor(System.Drawing.Color.DarkBlue);
		public static readonly D2DColor SkyBlue = D2DColor.FromGDIColor(System.Drawing.Color.SkyBlue);
		public static readonly D2DColor SteelBlue = D2DColor.FromGDIColor(System.Drawing.Color.SteelBlue);

		public static readonly D2DColor Pink = D2DColor.FromGDIColor(System.Drawing.Color.Pink);
	}
	#endregion

	#region Rect
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct D2DRect
	{
		public FLOAT left;
		public FLOAT top;
		public FLOAT right;
		public FLOAT bottom;

		public D2DRect(float left, float top, float width, float height)
		{
			this.left = left;
			this.top = top;
			this.right = left + width;
			this.bottom = top + height;
		}

		public D2DRect(System.Drawing.Rectangle rect)
		{
			this.left = rect.Left;
			this.top = rect.Top;
			this.right = rect.Right;
			this.bottom = rect.Bottom;
		}

		public D2DPoint Location
		{
			get { return new D2DPoint(left, top); }
			set
			{
				FLOAT width = this.right - this.left;
				FLOAT height = this.bottom - this.top;
				this.left = value.x;
				this.right = value.x + width;
				this.top = value.y;
				this.bottom = value.y + height;
			}
		}

		public FLOAT Width
		{
			get { return this.right - this.left; }
			set { this.right = this.left + value; }
		}

		public FLOAT Height
		{
			get { return this.bottom - this.top; }
			set { this.bottom = this.top + value; }
		}

		public void Offset(FLOAT x, FLOAT y)
		{
			this.left += x;
			this.right += x;
			this.top += y;
			this.bottom += y;
		}

		public FLOAT X
		{
			get { return this.left; }
			set
			{
				FLOAT width = this.right - this.left;
				this.left = value;
				this.right = value + width;
			}
		}

		public FLOAT Y
		{
			get { return this.top; }
			set
			{
				FLOAT height = this.bottom - this.top;
				this.top = value;
				this.bottom = value + height;
			}
		}
	}
	#endregion

	#region Point
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct D2DPoint
	{
		public FLOAT x;
		public FLOAT y;

		public D2DPoint(FLOAT x, FLOAT y)
		{
			this.x = x;
			this.y = y;
		}

		public void Offset(FLOAT offx, FLOAT offy)
		{
			this.x += offx;
			this.y += offy;
		}

		public static readonly D2DPoint Zero = new D2DPoint(0, 0);

		public static implicit operator D2DPoint(System.Drawing.Point wp)
		{
			return new D2DPoint(wp.X, wp.Y);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is D2DPoint)) return false;

			var p2 = (D2DPoint)obj;

			return x == p2.x && y == p2.y;
		}

		public static bool operator==(D2DPoint p1, D2DPoint p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(D2DPoint p1, D2DPoint p2)
		{
			return !p1.Equals(p2);
		}

		public override int GetHashCode()
		{
			return (int)((this.x * 0xff) + this.y);
		}
	}
	#endregion

	#region Size
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct D2DSize
	{
		public FLOAT width;
		public FLOAT height;

		public D2DSize(FLOAT width, FLOAT height)
		{
			this.width = width;
			this.height = height;
		}

		public static readonly D2DSize Empty = new D2DSize(-1, -1);
	}
	#endregion // Size

	#region Ellipse
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct D2DEllipse
	{
		public D2DPoint point;
		public FLOAT radiusX;
		public FLOAT radiusY;

		public D2DEllipse(D2DPoint center, FLOAT rx, FLOAT ry)
		{
			this.point = center;
			this.radiusX = rx;
			this.radiusY = ry;
		}

		public D2DEllipse(FLOAT x, FLOAT y, FLOAT rx, FLOAT ry)
			: this(new D2DPoint(x, y), rx, ry)
		{
		}
	}
	#endregion

	#region BezierSegment
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct D2DBezierSegment
	{
		public D2DPoint point1;
		public D2DPoint point2;
		public D2DPoint point3;

		public D2DBezierSegment(D2DPoint point1, D2DPoint point2, D2DPoint point3)
		{
			this.point1 = point1;
			this.point2 = point2;
			this.point3 = point3;
		}

		public D2DBezierSegment(FLOAT x1, FLOAT y1, FLOAT x2, FLOAT y2, FLOAT x3, FLOAT y3)
		{
			this.point1 = new D2DPoint(x1, y1);
			this.point2 = new D2DPoint(x2, y2);
			this.point3 = new D2DPoint(x3, y3);
		}
	}
	#endregion

	#region Gradient
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct D2DGradientStop
	{
		public FLOAT position;
		public D2DColor color;

		public D2DGradientStop(FLOAT position, D2DColor color)
		{
			this.position = position;
			this.color = color;
		}
	} 
	#endregion // Gradient
}