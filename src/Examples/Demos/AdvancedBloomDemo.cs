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
	/// Full bloom post-processing pipeline with a tunable luminance threshold
	/// and three-tap blur, so that mid-tones stay crisp and only the bright
	/// regions leak light.
	///
	/// Graph:
	///     scene ──► ColorMatrix (high-pass)
	///                │
	///                ├─► GaussianBlur σ = tightSigma ─┐
	///                ├─► GaussianBlur σ = midSigma  ──┤
	///                └─► GaussianBlur σ = wideSigma ──┤
	///                                                 ▼
	///     scene ────────────────────────────► Composite[Plus, 4 inputs] ──► screen
	///
	/// The ColorMatrix subtracts `threshold` from each channel and clamps to
	/// [0, 1], so anything below the threshold contributes nothing to the glow.
	/// The intensity diagonal (&gt;1) boosts the surviving brights before blur.
	/// </summary>
	class AdvancedBloomDemo : DemoForm
	{
		D2DBitmapGraphics? sceneGraphics;
		D2DBitmap? sceneBitmap;
		int sceneW, sceneH;

		D2DColorMatrixEffect? thresholdEffect;
		D2DGaussianBlurEffect? blurTight;
		D2DGaussianBlurEffect? blurMid;
		D2DGaussianBlurEffect? blurWide;
		D2DCompositeEffect? finalComposite;

		float threshold = 0.55f;
		float intensity = 1.4f;
		float tightSigma = 6f;
		float midSigma = 18f;
		float wideSigma = 48f;

		enum ViewMode { Final, SceneOnly, ThresholdOnly }
		ViewMode viewMode = ViewMode.Final;

		float time;

		public AdvancedBloomDemo()
		{
			Text = "Advanced Bloom (Threshold + Multi-Pass Blur) - d2dlib Examples";
			Size = new System.Drawing.Size(1024, 768);
			AnimationDraw = true;
			ShowFPS = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			thresholdEffect = Device.CreateColorMatrixEffect();
			thresholdEffect.ClampOutput = true;

			blurTight = Device.CreateGaussianBlurEffect();
			blurMid = Device.CreateGaussianBlurEffect();
			blurWide = Device.CreateGaussianBlurEffect();

			blurTight.SetInput(0, thresholdEffect);
			blurMid.SetInput(0, thresholdEffect);
			blurWide.SetInput(0, thresholdEffect);

			finalComposite = Device.CreateCompositeEffect();
			finalComposite.SetInputCount(4);
			finalComposite.Mode = D2DCompositeMode.Plus;
			finalComposite.SetInput(1, blurTight);
			finalComposite.SetInput(2, blurMid);
			finalComposite.SetInput(3, blurWide);

			EnsureSceneTarget();
			UpdateThresholdMatrix();
		}

		void UpdateThresholdMatrix()
		{
			if (thresholdEffect == null) return;

			// Per-channel high-pass: out = clamp(in * intensity - threshold, 0, 1)
			// Matrix diagonal = intensity, row 5 = -threshold bias for R,G,B.
			// Alpha passes through so the thresholded layer stays transparent
			// where the scene was transparent.
			float t = threshold;
			float k = intensity;
			thresholdEffect.Matrix = new D2DMatrix5x4
			{
				_11 = k, _22 = k, _33 = k, _44 = 1f,
				_51 = -t, _52 = -t, _53 = -t, _54 = 0f,
			};
		}

		void EnsureSceneTarget()
		{
			int w = Math.Max(1, ClientSize.Width);
			int h = Math.Max(1, ClientSize.Height);
			if (sceneGraphics != null && w == sceneW && h == sceneH) return;

			sceneBitmap?.Dispose();
			sceneBitmap = null;
			sceneGraphics?.Dispose();

			sceneGraphics = Device.CreateBitmapGraphics(w, h);
			sceneW = w;
			sceneH = h;
		}

		protected override void OnFrame()
		{
			base.OnFrame();
			time += 0.016f;
			SceneChanged = true;
		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			EnsureSceneTarget();
			if (sceneGraphics == null || thresholdEffect == null ||
				blurTight == null || blurMid == null || blurWide == null ||
				finalComposite == null)
			{
				g.Clear(D2DColor.Black);
				return;
			}

			// 1) Render the scene.
			sceneGraphics.BeginRender();
			sceneGraphics.Clear(D2DColor.Black);
			DrawScene(sceneGraphics);
			sceneGraphics.EndRender();

			sceneBitmap?.Dispose();
			sceneBitmap = sceneGraphics.GetBitmap();
			if (sceneBitmap == null) return;

			// 2) Wire the scene into the effect graph.
			thresholdEffect.SetInput(0, sceneBitmap);
			blurTight.StandardDeviation = tightSigma;
			blurMid.StandardDeviation = midSigma;
			blurWide.StandardDeviation = wideSigma;
			finalComposite.SetInput(0, sceneBitmap);

			g.Clear(D2DColor.Black);

			switch (viewMode)
			{
				case ViewMode.Final:
					g.DrawEffect(finalComposite);
					break;
				case ViewMode.SceneOnly:
					g.DrawBitmap(sceneBitmap, new D2DRect(0, 0, sceneW, sceneH));
					break;
				case ViewMode.ThresholdOnly:
					g.DrawEffect(thresholdEffect);
					break;
			}

			DrawHUD(g);
		}

		void DrawScene(D2DGraphics g)
		{
			// Dim blue gradient-ish backdrop (a few stacked translucent rects).
			for (int i = 0; i < 8; i++)
			{
				float t = i / 8f;
				var c = new D2DColor(1f,
					0.05f + t * 0.10f,
					0.07f + t * 0.12f,
					0.15f + t * 0.15f);
				g.FillRectangle(new D2DRect(0, sceneH * t, sceneW, sceneH / 8f + 1), c);
			}

			// Dim foreground shapes — these should NOT bloom (below threshold).
			g.FillRectangle(new D2DRect(60, sceneH - 180, sceneW - 120, 20),
				new D2DColor(1f, 0.25f, 0.28f, 0.32f));
			for (int i = 0; i < 6; i++)
			{
				float x = 100 + i * 140;
				g.FillRectangle(new D2DRect(x, sceneH - 160, 100, 120),
					new D2DColor(1f, 0.22f, 0.24f, 0.30f));
				// dim window lights — midtone, stays crisp
				g.FillRectangle(new D2DRect(x + 20, sceneH - 140, 25, 25),
					new D2DColor(1f, 0.45f, 0.40f, 0.20f));
				g.FillRectangle(new D2DRect(x + 55, sceneH - 140, 25, 25),
					new D2DColor(1f, 0.45f, 0.40f, 0.20f));
			}

			// Bright emissive elements — these bloom.
			float sunX = sceneW * 0.5f + (float)Math.Sin(time * 0.4f) * 200f;
			float sunY = sceneH * 0.35f;
			float sunR = 60f + (float)Math.Sin(time * 2f) * 4f;

			// soft sun halo seed (still helpful even with full bloom)
			var sunCenter = new D2DPoint(sunX, sunY);
			for (int k = 4; k >= 1; k--)
			{
				g.FillEllipse(sunCenter, sunR * k * 0.7f,
					new D2DColor(0.15f / k, 1f, 0.95f, 0.75f));
			}
			g.FillEllipse(sunCenter, sunR, new D2DColor(1f, 1f, 0.95f, 0.80f));
			g.FillEllipse(sunCenter, sunR * 0.6f, D2DColor.White);

			// Neon sign — bright text, strong colors.
			float pulse = 0.85f + 0.15f * (float)Math.Sin(time * 3f);
			g.DrawText("NEON", new D2DColor(1f, 1f * pulse, 0.15f, 0.6f * pulse),
				"Segoe UI", 110,
				new D2DRect(0, sceneH * 0.55f - 60, sceneW, 120),
				DWriteTextAlignment.Center, DWriteParagraphAlignment.Center);

			// Streetlights — three bright point sources.
			for (int i = 0; i < 3; i++)
			{
				float lx = sceneW * (0.2f + 0.3f * i);
				float ly = sceneH - 160;
				var col = i switch
				{
					0 => new D2DColor(1f, 1f, 0.7f, 0.3f),   // amber
					1 => new D2DColor(1f, 0.3f, 1f, 0.8f),   // magenta
					_ => new D2DColor(1f, 0.4f, 0.9f, 1f),   // cyan
				};
				var lightCenter = new D2DPoint(lx, ly);
				for (int k = 3; k >= 1; k--)
				{
					g.FillEllipse(lightCenter, 14f * k,
						new D2DColor(0.25f / k, col.r, col.g, col.b));
				}
				g.FillEllipse(lightCenter, 10f, col);
			}

			// A gray mid-tone text so the user can see the threshold cutoff.
			g.DrawText("mid-tone text (should stay crisp)",
				new D2DColor(1f, 0.45f, 0.45f, 0.48f),
				"Segoe UI", 20,
				new D2DRect(0, 70, sceneW, 30),
				DWriteTextAlignment.Center, DWriteParagraphAlignment.Center);
		}

		void DrawHUD(D2DGraphics g)
		{
			string line1 = $"View: {viewMode}   " +
			               $"[SPACE] cycle view   [↑/↓] threshold {threshold:F2}   " +
			               $"[←/→] intensity {intensity:F2}";
			string line2 = $"Blur σ: tight={tightSigma:F0}  mid={midSigma:F0}  wide={wideSigma:F0}   " +
			               $"[Q/A] tight  [W/S] mid  [E/D] wide";

			g.DrawText(line1, D2DColor.White, "Segoe UI", 13,
				new D2DRect(10, 6, ClientSize.Width - 20, 22));
			g.DrawText(line2, new D2DColor(1f, 0.85f, 0.85f, 0.85f), "Segoe UI", 12,
				new D2DRect(10, 26, ClientSize.Width - 20, 22));
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			bool matrixDirty = false;

			switch (e.KeyCode)
			{
				case Keys.Space:
					viewMode = (ViewMode)(((int)viewMode + 1) % 3);
					break;
				case Keys.Up:
					threshold = Math.Min(1.5f, threshold + 0.05f); matrixDirty = true; break;
				case Keys.Down:
					threshold = Math.Max(0f, threshold - 0.05f); matrixDirty = true; break;
				case Keys.Right:
					intensity = Math.Min(4f, intensity + 0.1f); matrixDirty = true; break;
				case Keys.Left:
					intensity = Math.Max(0.1f, intensity - 0.1f); matrixDirty = true; break;
				case Keys.Q: tightSigma = Math.Min(40f, tightSigma + 1f); break;
				case Keys.A: tightSigma = Math.Max(0f, tightSigma - 1f); break;
				case Keys.W: midSigma = Math.Min(80f, midSigma + 2f); break;
				case Keys.S: midSigma = Math.Max(0f, midSigma - 2f); break;
				case Keys.E: wideSigma = Math.Min(150f, wideSigma + 4f); break;
				case Keys.D: wideSigma = Math.Max(0f, wideSigma - 4f); break;
			}

			if (matrixDirty) UpdateThresholdMatrix();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			finalComposite?.Dispose();
			blurWide?.Dispose();
			blurMid?.Dispose();
			blurTight?.Dispose();
			thresholdEffect?.Dispose();
			sceneBitmap?.Dispose();
			sceneGraphics?.Dispose();
		}
	}
}
