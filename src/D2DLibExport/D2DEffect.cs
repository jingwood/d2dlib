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

namespace unvell.D2DLib
{
	/// <summary>
	/// Base class for Direct2D built-in effects. Requires Direct2D 1.1 (D2DDeviceContext).
	/// </summary>
	public class D2DEffect : IDisposable
	{
		internal HANDLE Handle { get; }

		internal D2DDevice Device { get; }

		internal D2DEffect(D2DDevice device, HANDLE handle)
		{
			this.Device = device;
			this.Handle = handle;
		}

		public void SetInput(uint index, D2DBitmap bitmap)
		{
			D2D.SetEffectInput(this.Handle, index, bitmap.Handle);
		}

		public void SetInput(uint index, D2DEffect inputEffect)
		{
			D2D.SetEffectInputEffect(this.Handle, index, inputEffect.Handle);
		}

		protected void SetValue(uint index, float value)
		{
			D2D.SetEffectValueFloat(this.Handle, index, value);
		}

		protected void SetValue(uint index, int value)
		{
			D2D.SetEffectValueInt(this.Handle, index, value);
		}

		protected void SetValue(uint index, bool value)
		{
			D2D.SetEffectValueBool(this.Handle, index, value);
		}

		protected void SetValue(uint index, D2DPoint value)
		{
			D2D.SetEffectValueVector2(this.Handle, index, value);
		}

		protected void SetValue(uint index, D2DVector3 value)
		{
			D2D.SetEffectValueVector3(this.Handle, index, value);
		}

		protected void SetValue(uint index, D2DColor value)
		{
			D2D.SetEffectValueColor(this.Handle, index, value);
		}

		protected float GetValueFloat(uint index)
		{
			return D2D.GetEffectValueFloat(this.Handle, index);
		}

		protected int GetValueInt(uint index)
		{
			return D2D.GetEffectValueInt(this.Handle, index);
		}

		protected D2DVector3 GetValueVector3(uint index)
		{
			return D2D.GetEffectValueVector3(this.Handle, index);
		}

		public void Dispose()
		{
			D2D.DestroyEffect(this.Handle);
		}
	}

	/// <summary>
	/// Gaussian blur effect. Applies a Gaussian blur to the input image.
	/// </summary>
	public class D2DGaussianBlurEffect : D2DEffect
	{
		private const uint PropStandardDeviation = 0;
		private const uint PropOptimization = 1;
		private const uint PropBorderMode = 2;

		internal D2DGaussianBlurEffect(D2DDevice device, HANDLE handle)
			: base(device, handle) { }

		/// <summary>
		/// Amount of blur to apply (standard deviation of Gaussian function). Default is 3.0.
		/// </summary>
		public float StandardDeviation
		{
			get => GetValueFloat(PropStandardDeviation);
			set => SetValue(PropStandardDeviation, value);
		}
	}

	/// <summary>
	/// Shadow effect. Creates a shadow of the input image.
	/// </summary>
	public class D2DShadowEffect : D2DEffect
	{
		private const uint PropBlurStandardDeviation = 0;
		private const uint PropColor = 1;
		private const uint PropOptimization = 2;

		internal D2DShadowEffect(D2DDevice device, HANDLE handle)
			: base(device, handle) { }

		/// <summary>
		/// Amount of blur for the shadow. Default is 3.0.
		/// </summary>
		public float BlurStandardDeviation
		{
			get => GetValueFloat(PropBlurStandardDeviation);
			set => SetValue(PropBlurStandardDeviation, value);
		}

		/// <summary>
		/// Color of the shadow.
		/// </summary>
		public D2DColor Color
		{
			set => SetValue(PropColor, value);
		}
	}

	/// <summary>
	/// Saturation effect. Alters the saturation of the input image.
	/// </summary>
	public class D2DSaturationEffect : D2DEffect
	{
		private const uint PropSaturation = 0;

		internal D2DSaturationEffect(D2DDevice device, HANDLE handle)
			: base(device, handle) { }

		/// <summary>
		/// Saturation level. 0.0 = fully desaturated, 1.0 = original, values above 1.0 = over-saturated.
		/// </summary>
		public float Saturation
		{
			get => GetValueFloat(PropSaturation);
			set => SetValue(PropSaturation, value);
		}
	}

	/// <summary>
	/// Brightness effect. Adjusts the brightness of the input image.
	/// </summary>
	public class D2DBrightnessEffect : D2DEffect
	{
		private const uint PropWhitePoint = 0;
		private const uint PropBlackPoint = 1;

		internal D2DBrightnessEffect(D2DDevice device, HANDLE handle)
			: base(device, handle) { }

		/// <summary>
		/// White point adjustment.
		/// </summary>
		public D2DPoint WhitePoint
		{
			set => SetValue(PropWhitePoint, value);
		}

		/// <summary>
		/// Black point adjustment.
		/// </summary>
		public D2DPoint BlackPoint
		{
			set => SetValue(PropBlackPoint, value);
		}
	}

	/// <summary>
	/// Hue rotation effect. Rotates the hue of the input image.
	/// </summary>
	public class D2DHueRotationEffect : D2DEffect
	{
		private const uint PropAngle = 0;

		internal D2DHueRotationEffect(D2DDevice device, HANDLE handle)
			: base(device, handle) { }

		/// <summary>
		/// Angle of hue rotation in degrees. Default is 0.
		/// </summary>
		public float Angle
		{
			get => GetValueFloat(PropAngle);
			set => SetValue(PropAngle, value);
		}
	}

	/// <summary>
	/// 3D Perspective Transform effect. Applies a 3D perspective projection to 2D content.
	/// </summary>
	public class D2D3DPerspectiveTransformEffect : D2DEffect
	{
		// D2D1_3DPERSPECTIVETRANSFORM_PROP
		private const uint PropInterpolationMode = 0;
		private const uint PropBorderMode = 1;
		private const uint PropDepth = 2;
		private const uint PropPerspectiveOrigin = 3;
		private const uint PropLocalOffset = 4;
		private const uint PropGlobalOffset = 5;
		private const uint PropRotationOrigin = 6;
		private const uint PropRotation = 7;

		internal D2D3DPerspectiveTransformEffect(D2DDevice device, HANDLE handle)
			: base(device, handle) { }

		/// <summary>
		/// Depth of the perspective projection. Default is 1000.
		/// </summary>
		public float Depth
		{
			get => GetValueFloat(PropDepth);
			set => SetValue(PropDepth, value);
		}

		/// <summary>
		/// Rotation angles in degrees (X, Y, Z).
		/// </summary>
		public D2DVector3 Rotation
		{
			get => GetValueVector3(PropRotation);
			set => SetValue(PropRotation, value);
		}

		/// <summary>
		/// Origin point for the rotation, relative to the input image.
		/// </summary>
		public D2DVector3 RotationOrigin
		{
			get => GetValueVector3(PropRotationOrigin);
			set => SetValue(PropRotationOrigin, value);
		}

		/// <summary>
		/// Origin of the perspective projection (vanishing point), in pixels.
		/// </summary>
		public D2DPoint PerspectiveOrigin
		{
			set => SetValue(PropPerspectiveOrigin, value);
		}

		/// <summary>
		/// Offset applied before rotation (in local space).
		/// </summary>
		public D2DVector3 LocalOffset
		{
			get => GetValueVector3(PropLocalOffset);
			set => SetValue(PropLocalOffset, value);
		}

		/// <summary>
		/// Offset applied after rotation (in global/screen space).
		/// </summary>
		public D2DVector3 GlobalOffset
		{
			get => GetValueVector3(PropGlobalOffset);
			set => SetValue(PropGlobalOffset, value);
		}
	}

	/// <summary>
	/// Composite effect. Combines multiple inputs using a composite mode.
	/// Supports variable number of inputs (2 or more).
	/// </summary>
	public class D2DCompositeEffect : D2DEffect
	{
		private const uint PropMode = 0;

		internal D2DCompositeEffect(D2DDevice device, HANDLE handle)
			: base(device, handle) { }

		/// <summary>
		/// Composite mode. Default is SourceOver.
		/// </summary>
		public D2DCompositeMode Mode
		{
			get => (D2DCompositeMode)GetValueInt(PropMode);
			set => SetValue(PropMode, (int)value);
		}

		/// <summary>
		/// Set the number of inputs for this composite effect.
		/// Must be called before setting inputs beyond index 1.
		/// </summary>
		public void SetInputCount(uint count)
		{
			D2D.SetEffectInputCount(this.Handle, count);
		}
	}

	/// <summary>
	/// Blend effect. Blends two inputs using a blend mode (Multiply, Screen, Overlay, etc).
	/// </summary>
	public class D2DBlendEffect : D2DEffect
	{
		private const uint PropMode = 0;

		internal D2DBlendEffect(D2DDevice device, HANDLE handle)
			: base(device, handle) { }

		/// <summary>
		/// Blend mode. Default is Multiply.
		/// </summary>
		public D2DBlendMode Mode
		{
			get => (D2DBlendMode)GetValueInt(PropMode);
			set => SetValue(PropMode, (int)value);
		}
	}

	/// <summary>
	/// Crop effect. Crops the input image to a specified rectangle.
	/// </summary>
	public class D2DCropEffect : D2DEffect
	{
		private const uint PropRect = 0;

		internal D2DCropEffect(D2DDevice device, HANDLE handle)
			: base(device, handle) { }

		// Rect property requires Vector4 (left, top, right, bottom)
		// Users can use SetEffectValueVector4 via the base class
	}
}
