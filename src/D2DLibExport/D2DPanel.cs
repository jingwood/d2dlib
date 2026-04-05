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

using System;
using System.Numerics;

namespace unvell.D2DLib
{
	/// <summary>
	/// A 2D panel that can be rendered with a 3D perspective transform.
	/// Content is drawn to an off-screen bitmap, then the bitmap is displayed
	/// using the Direct2D 3DPerspectiveTransform effect.
	/// </summary>
	public class D2DPanel : IDisposable
	{
		private readonly D2DDevice device;
		private D2DBitmapGraphics? bitmapGraphics;
		private D2D3DPerspectiveTransformEffect? effect;
		private D2DBitmap? panelBitmap;

		private D2DVector3 rotation;
		private D2DVector3 rotationOrigin;
		private float depth = 1000f;
		private bool effectDirty = true;

		/// <summary>
		/// Width of the panel in pixels.
		/// </summary>
		public float Width { get; }

		/// <summary>
		/// Height of the panel in pixels.
		/// </summary>
		public float Height { get; }

		/// <summary>
		/// Rotation angles in degrees (X, Y, Z). Setting Y rotates around the vertical axis,
		/// making one side appear farther away.
		/// </summary>
		public D2DVector3 Rotation
		{
			get => rotation;
			set { rotation = value; effectDirty = true; }
		}

		/// <summary>
		/// Origin for the 3D rotation, relative to the panel. Default is center.
		/// </summary>
		public D2DVector3 RotationOrigin
		{
			get => rotationOrigin;
			set { rotationOrigin = value; effectDirty = true; }
		}

		/// <summary>
		/// Perspective depth. Larger values produce less dramatic perspective.
		/// Default is 1000.
		/// </summary>
		public float Depth
		{
			get => depth;
			set { depth = value; effectDirty = true; }
		}

		public D2DPanel(D2DDevice device, float width, float height)
		{
			this.device = device;
			this.Width = width;
			this.Height = height;

			this.rotationOrigin = new D2DVector3(width * 0.5f, height * 0.5f, 0);

			this.bitmapGraphics = device.CreateBitmapGraphics(width, height);
			this.effect = device.Create3DPerspectiveTransformEffect();
		}

		/// <summary>
		/// Begin drawing content onto the panel's internal bitmap.
		/// Call this, draw your content, then call EndRender().
		/// </summary>
		public D2DBitmapGraphics BeginRender()
		{
			bitmapGraphics.BeginRender();
			bitmapGraphics.Clear(D2DColor.Transparent);
			return bitmapGraphics;
		}

		/// <summary>
		/// Finish drawing content and prepare the panel bitmap.
		/// </summary>
		public void EndRender()
		{
			bitmapGraphics.EndRender();

			// Get the rendered bitmap for the effect input
			if (panelBitmap != null)
			{
				panelBitmap.Dispose();
			}
			panelBitmap = bitmapGraphics.GetBitmap();
		}

		/// <summary>
		/// Draw the panel with 3D perspective transform applied.
		/// </summary>
		public void Draw(D2DGraphics g, float x = 0, float y = 0)
		{
			if (panelBitmap == null) return;

			if (effectDirty)
			{
				effect.Depth = depth;
				effect.Rotation = rotation;
				effect.RotationOrigin = rotationOrigin;
				effect.PerspectiveOrigin = new D2DPoint(
					rotationOrigin.X, rotationOrigin.Y);
				effectDirty = false;
			}

			effect.SetInput(0, panelBitmap);

			g.PushTransform();
			g.TranslateTransform(x, y);
			g.DrawEffect(effect);
			g.PopTransform();
		}

		/// <summary>
		/// Transform a screen-space point to the panel's local 2D coordinate system,
		/// accounting for the 3D perspective projection via ray-plane intersection.
		/// Returns null if the point does not hit the panel.
		/// </summary>
		public D2DPoint? ScreenToLocal(float screenX, float screenY, float panelX = 0, float panelY = 0)
		{
			// Adjust for panel position on screen
			float px = screenX - panelX;
			float py = screenY - panelY;

			float cx = rotationOrigin.X;
			float cy = rotationOrigin.Y;
			float cz = rotationOrigin.Z;

			// Build the rotation matrix (same as the effect uses internally)
			var rotMatrix = BuildRotationMatrix();

			// The perspective eye is at (perspOriginX, perspOriginY, depth)
			// PerspectiveOrigin is set to (cx, cy) in Draw()
			var eye = new Vector3(cx, cy, depth);

			// The screen point is at (px, py, 0) on the projection plane
			var screenPoint = new Vector3(px, py, 0);

			// Cast a ray from eye through screen point
			var rayDir = screenPoint - eye;

			// The panel is originally the z=0 plane. After rotation around (cx, cy, cz):
			//   rotated_point = R * (local_point - center) + center
			// The plane normal (originally z-axis) rotates to:
			var normalRotated = Vector3.TransformNormal(Vector3.UnitZ, rotMatrix);

			// A known point on the rotated plane: the rotation center maps to itself
			//   when local_point = (cx, cy, cz): R * (0,0,0) + (cx,cy,cz) = (cx,cy,cz)
			var planePoint = new Vector3(cx, cy, cz);

			// Ray-plane intersection: t = dot(planePoint - eye, normal) / dot(rayDir, normal)
			float denom = Vector3.Dot(rayDir, normalRotated);
			if (Math.Abs(denom) < 1e-6f) return null; // ray parallel to plane

			float t = Vector3.Dot(planePoint - eye, normalRotated) / denom;
			if (t < 0) return null; // intersection behind the eye

			// 3D intersection point on the rotated panel
			var hitPoint = eye + t * rayDir;

			// Transform back to local panel coordinates:
			//   rotated = R * (local - center) + center
			//   local = R^(-1) * (rotated - center) + center
			if (!Matrix4x4.Invert(rotMatrix, out Matrix4x4 invRot))
				return null;

			var local3d = Vector3.Transform(hitPoint - new Vector3(cx, cy, cz), invRot)
				+ new Vector3(cx, cy, cz);

			float localX = local3d.X;
			float localY = local3d.Y;

			// Check if the point is within the panel bounds
			if (localX < 0 || localX > Width || localY < 0 || localY > Height)
				return null;

			return new D2DPoint(localX, localY);
		}

		/// <summary>
		/// Build the 3D rotation matrix matching the effect's rotation parameters.
		/// </summary>
		private Matrix4x4 BuildRotationMatrix()
		{
			float rx = rotation.X * (float)(Math.PI / 180.0);
			float ry = rotation.Y * (float)(Math.PI / 180.0);
			float rz = rotation.Z * (float)(Math.PI / 180.0);

			return Matrix4x4.CreateRotationX(rx)
				* Matrix4x4.CreateRotationY(ry)
				* Matrix4x4.CreateRotationZ(rz);
		}

		public void Dispose()
		{
			panelBitmap?.Dispose();
			effect?.Dispose();
			bitmapGraphics?.Dispose();
		}
	}
}
