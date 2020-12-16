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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.Demos
{
	public partial class HitTestByInverseTransform : DemoForm
	{
    float angle = 45;
		Matrix3 mat = new Matrix3();
		Matrix3 matInv = new Matrix3();
		Rect2D rect = new Rect2D(0, 0, 200, 100);
		bool isHitted = false;

		public HitTestByInverseTransform()
		{
			Text = "HitTest By Inverse Transform - d2dlib Examples";

      UpdateTransform();
		}

    void UpdateTransform()
    {
      // set the transform
      mat.LoadIdentity();
      mat.Translate(300, 300);
      mat.Rotate(this.angle);
      mat.Translate(-rect.Width * 0.5f, -rect.Height * 0.5f);

      // get the inversed matrix
      matInv.CopyFrom(mat).Inverse();
    }

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			// set the transform before draw rect
			g.SetTransform(mat.ToMatrix3x2());

      g.FillRectangle(rect, isHitted ? D2DColor.LightYellow : D2DColor.LightGray);
			g.DrawRectangle(rect, isHitted ? D2DColor.Red : D2DColor.Blue, 2);

      g.DrawText("Click to rotate", D2DColor.Black, this.Font.Name, 14, rect, 
        DWRITE_TEXT_ALIGNMENT.DWRITE_TEXT_ALIGNMENT_CENTER, DWRITE_PARAGRAPH_ALIGNMENT.DWRITE_PARAGRAPH_ALIGNMENT_CENTER);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			
			// transformed point to the rect
			var tp = matInv.TransformPoint(e.Location);

      // calculate whether the point inside the rect
      isHitted = rect.ContainsPoint(tp);

			Invalidate();
		}

    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);

      // transformed point to the rect
      var tp = matInv.TransformPoint(e.Location);

      if (rect.ContainsPoint(tp))
      {
        this.angle += 10;
        UpdateTransform();
        Invalidate();
      }
    }
  }

  #region Rect2D
  class Rect2D
  {
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public float Right { get { return this.X + this.Width; } }
    public float Bottom { get { return this.Y + this.Height; } }

    public Rect2D(float x, float y, float width, float height)
    {
      this.X = x;
      this.Y = y;
      this.Width = width;
      this.Height = height;
    }

    public bool ContainsPoint(PointF p)
    {
      return p.X >= this.X && p.Y >= this.Y
        && p.X <= this.Right && p.Y <= this.Bottom;
    }

    public static implicit operator D2DRect(Rect2D r) {
      return new D2DRect(r.X, r.Y, r.Width, r.Height);
    }
  }
  #endregion /* Rect2D */

  #region Matrix3
  /// <summary>
  /// Represents a left-handed 3x3 matrix.
  /// </summary>
  public class Matrix3
	{
		public float a1, b1, c1;
		public float a2, b2, c2;
		public float a3, b3, c3;

		public static readonly Matrix3 Identify = new Matrix3()
		{
			a1 = 1,
			b1 = 0,
			c1 = 0,
			a2 = 0,
			b2 = 1,
			c2 = 0,
			a3 = 0,
			b3 = 0,
			c3 = 1,
		};

		public Matrix3()
		{
		}

		public Matrix3(float a1, float b1, float c1,
			float a2, float b2, float c2,
			float a3, float b3, float c3)
		{
			this.a1 = a1; this.b1 = b1; this.c1 = c1;
			this.a2 = a2; this.b2 = b2; this.c2 = c2;
			this.a3 = a3; this.b3 = b3; this.c3 = c3;
		}

		public Matrix3(Matrix3 mat2)
		{
			this.CopyFrom(mat2);
		}

		public Matrix3 LoadIdentity()
		{
			return this.CopyFrom(Matrix3.Identify);
		}

		public Matrix3 CopyFrom(Matrix3 m2)
		{
			this.a1 = m2.a1; this.b1 = m2.b1; this.c1 = m2.c1;
			this.a2 = m2.a2; this.b2 = m2.b2; this.c2 = m2.c2;
			this.a3 = m2.a3; this.b3 = m2.b3; this.c3 = m2.c3;

			return this;
		}

		public Matrix3 Clone()
		{
			return new Matrix3(this);
		}

		public Matrix3 Rotate(float angle)
		{
			float radians = (float)(angle / 180f * Math.PI);
			float sin = (float)Math.Sin(radians);
			float cos = (float)Math.Cos(radians);

			float m2a1 = cos, m2b1 = sin;
			float m2a2 = -sin, m2b2 = cos;

			float a1 = this.a1 * m2a1 + this.a2 * m2b1;
			float b1 = this.b1 * m2a1 + this.b2 * m2b1;
			float a2 = this.a1 * m2a2 + this.a2 * m2b2;
			float b2 = this.b1 * m2a2 + this.b2 * m2b2;

			this.a1 = a1; this.b1 = b1;
			this.a2 = a2; this.b2 = b2;

			return this;
		}

		public void Scale(float x, float y)
		{
			this.a1 *= x; this.b1 *= x;
			this.a2 *= y; this.b2 *= y;
		}

		public Matrix3 Translate(Point p)
		{
			Translate(p.X, p.Y);
			return this;
		}

		public Matrix3 Translate(D2DPoint p)
		{
			Translate(p.x, p.y);
			return this;
		}

		public Matrix3 Translate(float x, float y)
		{
			this.a3 += this.a1 * x + this.a2 * y;
			this.b3 += this.b1 * x + this.b2 * y;

			return this;
		}

		public PointF TransformPoint(PointF p)
		{
			return TransformPoint(p.X, p.Y);
		}

		public PointF TransformPoint(float x, float y)
		{
			return new PointF(x * a1 + y * a2 + a3, x * b1 + y * b2 + b3);
		}

		public static Matrix3 operator *(Matrix3 m1, Matrix3 m2)
		{
			return new Matrix3(
				m1.a1 * m2.a1 + m1.b1 * m2.a2 + m1.c1 * m2.a3,
				m1.a1 * m2.b1 + m1.b1 * m2.b2 + m1.c1 * m2.b3,
				m1.a1 * m2.c1 + m1.b1 * m2.c2 + m1.c1 * m2.c3,

				m1.a2 * m2.a1 + m1.b2 * m2.a2 + m1.c2 * m2.a3,
				m1.a2 * m2.b1 + m1.b2 * m2.b2 + m1.c2 * m2.b3,
				m1.a2 * m2.c1 + m1.b2 * m2.c2 + m1.c2 * m2.c3,

				m1.a3 * m2.a1 + m1.b3 * m2.a2 + m1.c3 * m2.a3,
				m1.a3 * m2.b1 + m1.b3 * m2.b2 + m1.c3 * m2.b3,
				m1.a3 * m2.c1 + m1.b3 * m2.c2 + m1.c3 * m2.c3);
		}

		public Matrix3 CreateRotation(float angle)
		{
			return new Matrix3().LoadIdentity().Rotate(angle);
		}

		public Matrix3 Inverse()
		{
			float a = this.a1, b = this.b1, c = this.c1,
				d = this.a2, e = this.b2, f = this.c2,
				g = this.a3, h = this.b3, i = this.c3;

			float det = a * e * i - a * f * h + b * f * g - b * d * i + c * d * h - c * e * g;
			if (det == 0) return this;

			det = 1.0f / det;

			float m2a = det * e * i - f * h, m2b = det * c * h - b * i, m2c = det * b * f - c * e,
				m2d = det * f * g - d * i, m2e = det * a * i - c * g, m2f = det * c * d - a * f,
				m2g = det * d * h - e * g, m2h = det * b * g - a * h, m2i = det * a * e - b * d;

			this.a1 = m2a; this.b1 = m2b; this.c1 = m2c;
			this.a2 = m2d; this.b2 = m2e; this.c2 = m2f;
			this.a3 = m2g; this.b3 = m2h; this.c3 = m2i;

			return this;
		}

		public D2DMatrix3x2 ToMatrix3x2()
		{
			D2DMatrix3x2 mat;

			mat.a1 = this.a1; mat.b1 = this.b1; 
			mat.a2 = this.a2; mat.b2 = this.b2;
			mat.a3 = this.a3; mat.b3 = this.b3;

			return mat;
		}
	}
	#endregion // Matrix3
}
