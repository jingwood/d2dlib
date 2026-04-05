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
	/// <summary>
	/// Demonstrates effect chaining: Blur + Shadow + Composite to create
	/// a frosted glass / acrylic overlay effect.
	/// </summary>
	class CompositeEffectsDemo : DemoForm
	{
		D2DBitmap image;
		D2DGaussianBlurEffect blurEffect;
		D2DSaturationEffect saturationEffect;
		D2DCompositeEffect compositeEffect;
		D2DShadowEffect shadowEffect;

		float blurAmount = 8f;
		float saturation = 0.3f;
		int currentMode = 0;

		readonly string[] modeNames = {
			"Frosted Glass (Blur + Desaturate)",
			"Shadow + Original Composite",
			"Blend Modes Showcase",
		};

		public CompositeEffectsDemo()
		{
			Text = "Composite Effects Demo - d2dlib Examples";
			Size = new System.Drawing.Size(900, 700);
			AnimationDraw = false;
			ShowFPS = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			image = Device.LoadBitmap(Properties.Resources.IMGP6873);

			// Blur effect
			blurEffect = Device.CreateGaussianBlurEffect();
			blurEffect.SetInput(0, image);
			blurEffect.StandardDeviation = blurAmount;

			// Saturation effect chained after blur
			saturationEffect = Device.CreateSaturationEffect();
			saturationEffect.SetInput(0, blurEffect);  // Chain: image → blur → saturation
			saturationEffect.Saturation = saturation;

			// Shadow of the original
			shadowEffect = Device.CreateShadowEffect();
			shadowEffect.SetInput(0, image);
			shadowEffect.BlurStandardDeviation = 6f;
			shadowEffect.Color = new D2DColor(0.6f, 0, 0, 0);

			// Composite: shadow + original layered on top
			compositeEffect = Device.CreateCompositeEffect();
			compositeEffect.SetInputCount(2);
			compositeEffect.SetInput(0, shadowEffect);
			compositeEffect.SetInput(1, image);
			compositeEffect.Mode = D2DCompositeMode.SourceOver;
		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			g.Clear(new D2DColor(0.12f, 0.12f, 0.15f));

			float imgW = image.Width;
			float imgH = image.Height;
			float scale = Math.Min((float)ClientSize.Width / imgW, (float)(ClientSize.Height - 50) / imgH);
			float drawW = imgW * scale;
			float drawH = imgH * scale;
			float ox = (ClientSize.Width - drawW) * 0.5f;
			float oy = 50 + (ClientSize.Height - 50 - drawH) * 0.5f;

			g.PushTransform();
			g.TranslateTransform(ox, oy);
			g.ScaleTransform(scale, scale);

			switch (currentMode)
			{
				case 0:
					// Frosted glass: blur → saturation chain
					blurEffect.StandardDeviation = blurAmount;
					saturationEffect.Saturation = saturation;
					g.DrawEffect(saturationEffect);

					// Draw a "glass panel" overlay in the center
					g.PopTransform();
					DrawGlassOverlay(g, ox, oy, drawW, drawH);
					return;

				case 1:
					// Shadow + original composite
					g.DrawEffect(compositeEffect);
					break;

				case 2:
					// Just the shadow
					g.DrawEffect(shadowEffect);
					break;
			}

			g.PopTransform();

			DrawUI(g);
		}

		void DrawGlassOverlay(D2DGraphics g, float ox, float oy, float drawW, float drawH)
		{
			// Draw the original image as background (sharp)
			g.PushTransform();
			g.TranslateTransform(ox, oy);
			g.ScaleTransform(drawW / image.Width, drawH / image.Height);
			g.DrawBitmap(image, new D2DRect(0, 0, image.Width, image.Height));
			g.PopTransform();

			// Glass panel dimensions
			float panelW = drawW * 0.5f;
			float panelH = drawH * 0.6f;
			float px = ox + (drawW - panelW) * 0.5f;
			float py = oy + (drawH - panelH) * 0.5f;

			// Draw shadow behind the glass panel
			g.FillRectangle(new D2DRect(px + 6, py + 6, panelW, panelH),
				new D2DColor(0.3f, 0, 0, 0));

			// Clip and draw blurred content inside the glass panel area
			g.PushClip(new D2DRect(px, py, panelW, panelH));
			g.PushTransform();
			g.TranslateTransform(ox, oy);
			g.ScaleTransform(drawW / image.Width, drawH / image.Height);
			g.DrawEffect(saturationEffect);
			g.PopTransform();
			g.PopClip();

			// Glass panel border
			g.FillRectangle(new D2DRect(px, py, panelW, panelH),
				new D2DColor(0.15f, 1f, 1f, 1f));
			g.DrawRectangle(new D2DRect(px, py, panelW, panelH),
				new D2DColor(0.6f, 1f, 1f, 1f), 1.5f);

			g.DrawText("Frosted Glass Effect", D2DColor.White, "Segoe UI", 20,
				new D2DRect(px + 20, py + 20, panelW - 40, 40));
			g.DrawText("Blur + Desaturation chained via D2D effects",
				new D2DColor(0.9f, 0.9f, 0.9f), "Segoe UI", 13,
				new D2DRect(px + 20, py + 60, panelW - 40, 30));

			DrawUI(g);
		}

		void DrawUI(D2DGraphics g)
		{
			string label = $"Mode: {modeNames[currentMode]}  |  LEFT/RIGHT: switch  |  UP/DOWN: adjust blur ({blurAmount:F1})";
			g.DrawText(label, D2DColor.White, "Segoe UI", 13,
				new D2DRect(10, 10, ClientSize.Width - 20, 30));
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			switch (e.KeyCode)
			{
				case Keys.Left:
					currentMode = (currentMode - 1 + modeNames.Length) % modeNames.Length;
					Invalidate();
					break;
				case Keys.Right:
					currentMode = (currentMode + 1) % modeNames.Length;
					Invalidate();
					break;
				case Keys.Up:
					blurAmount = Math.Min(30, blurAmount + 1);
					Invalidate();
					break;
				case Keys.Down:
					blurAmount = Math.Max(0, blurAmount - 1);
					Invalidate();
					break;
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			compositeEffect?.Dispose();
			shadowEffect?.Dispose();
			saturationEffect?.Dispose();
			blurEffect?.Dispose();
			image?.Dispose();
		}
	}
}
