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
	class EffectsDemo : DemoForm
	{
		private D2DBitmap image;
		private D2DGaussianBlurEffect blurEffect;
		private D2DShadowEffect shadowEffect;
		private D2DSaturationEffect saturationEffect;
		private D2DHueRotationEffect hueRotationEffect;

		private float blurAmount = 0f;
		private float hueAngle = 0f;
		private int currentEffect = 0;
		private readonly string[] effectNames = {
			"Gaussian Blur", "Shadow", "Saturation", "Hue Rotation"
		};

		public EffectsDemo()
		{
			Text = "Direct2D Effects Demo (D2D 1.1) - d2dlib Examples";
			Size = new System.Drawing.Size(900, 700);
			AnimationDraw = true;
			ShowFPS = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			image = Device.LoadBitmap(Properties.Resources.IMGP6873);

			blurEffect = Device.CreateGaussianBlurEffect();
			blurEffect.SetInput(0, image);

			shadowEffect = Device.CreateShadowEffect();
			shadowEffect.SetInput(0, image);
			shadowEffect.BlurStandardDeviation = 5f;
			shadowEffect.Color = new D2DColor(0.5f, 0, 0, 0);

			saturationEffect = Device.CreateSaturationEffect();
			saturationEffect.SetInput(0, image);

			hueRotationEffect = Device.CreateHueRotationEffect();
			hueRotationEffect.SetInput(0, image);
		}

		protected override void OnFrame()
		{
			base.OnFrame();

			switch (currentEffect)
			{
				case 0: // Gaussian Blur: animate blur amount
					blurAmount += 0.1f;
					if (blurAmount > 20f) blurAmount = 0f;
					blurEffect!.StandardDeviation = blurAmount;
					break;

				case 3: // Hue Rotation: animate angle
					hueAngle += 1f;
					if (hueAngle >= 360f) hueAngle = 0f;
					hueRotationEffect!.Angle = hueAngle;
					break;
			}

			SceneChanged = true;
		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			g.Clear(new D2DColor(0.2f, 0.2f, 0.2f));

			float imgWidth = image!.Width;
			float imgHeight = image.Height;

			// Scale to fit window
			float scaleX = (float)ClientSize.Width / imgWidth;
			float scaleY = (float)(ClientSize.Height - 40) / imgHeight;
			float scale = Math.Min(scaleX, scaleY);

			float drawWidth = imgWidth * scale;
			float drawHeight = imgHeight * scale;
			float offsetX = (ClientSize.Width - drawWidth) * 0.5f;
			float offsetY = 40 + (ClientSize.Height - 40 - drawHeight) * 0.5f;

			g.PushTransform();
			g.TranslateTransform(offsetX, offsetY);
			g.ScaleTransform(scale, scale);

			switch (currentEffect)
			{
				case 0:
					g.DrawEffect(blurEffect!);
					break;
				case 1:
					g.DrawEffect(shadowEffect!);
					break;
				case 2:
					g.DrawEffect(saturationEffect!);
					break;
				case 3:
					g.DrawEffect(hueRotationEffect!);
					break;
			}

			g.PopTransform();

			// Draw label
			string label = $"Effect: {effectNames[currentEffect]}  |  Press LEFT/RIGHT to switch  |  UP/DOWN to adjust";
			g.DrawText(label, D2DColor.White, "Segoe UI", 14,
				new D2DRect(10, 10, ClientSize.Width - 20, 30));
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			switch (e.KeyCode)
			{
				case Keys.Left:
					currentEffect = (currentEffect - 1 + effectNames.Length) % effectNames.Length;
					break;

				case Keys.Right:
					currentEffect = (currentEffect + 1) % effectNames.Length;
					break;

				case Keys.Up:
					AdjustCurrentEffect(1f);
					break;

				case Keys.Down:
					AdjustCurrentEffect(-1f);
					break;
			}
		}

		private void AdjustCurrentEffect(float delta)
		{
			switch (currentEffect)
			{
				case 0:
					blurAmount = Math.Max(0, blurAmount + delta);
					blurEffect!.StandardDeviation = blurAmount;
					break;
				case 1:
					var blur = shadowEffect!.BlurStandardDeviation + delta;
					shadowEffect.BlurStandardDeviation = Math.Max(0, blur);
					break;
				case 2:
					var sat = saturationEffect!.Saturation + delta * 0.1f;
					saturationEffect.Saturation = Math.Max(0, sat);
					break;
				case 3:
					hueAngle += delta * 10f;
					hueRotationEffect!.Angle = hueAngle;
					break;
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			hueRotationEffect?.Dispose();
			saturationEffect?.Dispose();
			shadowEffect?.Dispose();
			blurEffect?.Dispose();
			image?.Dispose();
		}
	}
}
