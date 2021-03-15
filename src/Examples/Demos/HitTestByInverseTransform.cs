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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using unvell.D2DLib.WinForm;
using System.Numerics;

namespace unvell.D2DLib.Examples.Demos
{
	public partial class HitTestByInverseTransform : DemoForm
	{
    float angle = -10;
    float scale = 1.0f;
    Vector2 pos = new Vector2(300, 300);

    Matrix3x2 mat = new Matrix3x2();
    Matrix3x2 matInv;

    Rect2D rect = new Rect2D(0, 0, 400, 100);
		bool isHitted = false;
    bool isMoved = false;

		public HitTestByInverseTransform()
		{
			Text = "HitTest By Inverse Transform - d2dlib Examples";

      UpdateTransform();
		}

    void UpdateTransform()
    {
      // set the transform
      mat = Matrix3x2.Identity;
      mat *= Matrix3x2.CreateTranslation(-rect.Width * 0.5f, -rect.Height * 0.5f);
      mat *= Matrix3x2.CreateScale(this.scale, this.scale);
      mat *= Matrix3x2.CreateRotation((float)(Math.PI * this.angle / 180f));
      mat *= Matrix3x2.CreateTranslation(pos.X, pos.Y);

      // get the inversed matrix
      Matrix3x2.Invert(mat, out matInv);
    }

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

      // set the transform before draw rect
      g.SetTransform(mat.toD2DMatrix3x2());

      g.FillRectangle(rect, isHitted ? D2DColor.LightYellow : D2DColor.LightGray);
			g.DrawRectangle(rect, isHitted ? D2DColor.Red : D2DColor.Blue, 2);

      g.DrawText("Drag to move / Click to rotate / Scroll to scale", D2DColor.Black, this.Font.Name, 14, rect, 
        DWriteTextAlignment.Center, DWriteParagraphAlignment.Center);

      g.ResetTransform();

    }

    private Point lastPoint;

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

      if (e.Button == MouseButtons.None)
      {
        // transformed point to the rect
        var tp = Vector2.Transform(new Vector2(e.X, e.Y), matInv);

        // calculate whether the point inside the rect
        isHitted = rect.ContainsPoint(new PointF(tp.X, tp.Y));
      }
      else if (isHitted)
      {
        pos.X += e.X - lastPoint.X;
        pos.Y += e.Y - lastPoint.Y;

        lastPoint = e.Location;
        isMoved = true;

        UpdateTransform();
      }

			Invalidate();
		}

    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);

      lastPoint = e.Location;
      isMoved = false;
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);

      if (isHitted && !isMoved)
      {
        this.angle += 10f;
        UpdateTransform();
        Invalidate();
      }
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
      base.OnMouseWheel(e);

      scale += (float)e.Delta / 1000f;

      UpdateTransform();
      Invalidate();
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

  public static class NumericExtension
  {
    public static D2DMatrix3x2 toD2DMatrix3x2(this Matrix3x2 mat)
    {
      D2DMatrix3x2 d2dmat;

      d2dmat.a1 = mat.M11;
      d2dmat.b1 = mat.M12;
      d2dmat.a2 = mat.M21;
      d2dmat.b2 = mat.M22;
      d2dmat.a3 = mat.M31;
      d2dmat.b3 = mat.M32;

      return d2dmat;
    }
  }
}
