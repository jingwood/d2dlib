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
}
