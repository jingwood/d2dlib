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
	/// Bloom / glow effect demo.
	///
	/// Pipeline:
	///   1. Render bright emissive shapes into an offscreen bitmap (scene).
	///   2. Blur the scene with a Gaussian blur (halo).
	///   3. Additively composite the original scene on top of the blur (Plus mode).
	///
	/// Because the background is black and only the shapes emit light, the additive
	/// step acts as a threshold-free bloom: dark pixels contribute nothing, bright
	/// pixels leak outward through the blur.
	/// </summary>
	class BloomGlowDemo : DemoForm
	{
		struct Orb
		{
			public D2DPoint Pos;
			public D2DPoint Vel;
			public float Radius;
			public D2DColor Core;
		}

		D2DBitmapGraphics? sceneGraphics;
		D2DBitmap? sceneBitmap;
		int sceneW, sceneH;

		D2DGaussianBlurEffect? blurEffect;
		D2DCompositeEffect? bloomEffect;

		readonly Orb[] orbs = new Orb[14];
		readonly Random rng = new Random(0);

		float bloomAmount = 18f;
		bool bloomEnabled = true;
		float pulse;

		public BloomGlowDemo()
		{
			Text = "Bloom / Glow Demo - d2dlib Examples";
			Size = new System.Drawing.Size(960, 720);
			AnimationDraw = true;
			ShowFPS = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			blurEffect = Device.CreateGaussianBlurEffect();
			bloomEffect = Device.CreateCompositeEffect();
			bloomEffect.SetInputCount(2);
			bloomEffect.Mode = D2DCompositeMode.Plus;

			for (int i = 0; i < orbs.Length; i++)
			{
				orbs[i] = new Orb
				{
					Pos = new D2DPoint(rng.Next(100, 800), rng.Next(100, 600)),
					Vel = new D2DPoint((float)(rng.NextDouble() - 0.5) * 4f,
					                   (float)(rng.NextDouble() - 0.5) * 4f),
					Radius = 18f + (float)rng.NextDouble() * 22f,
					Core = RandomVividColor(),
				};
			}

			EnsureSceneTarget();
		}

		D2DColor RandomVividColor()
		{
			// Pick a bright, saturated hue — these drive the glow.
			float[][] palette = {
				new[] { 1.0f, 0.25f, 0.45f }, // pink
				new[] { 0.25f, 0.75f, 1.0f }, // cyan
				new[] { 1.0f, 0.85f, 0.25f }, // gold
				new[] { 0.55f, 1.0f, 0.45f }, // lime
				new[] { 0.85f, 0.4f, 1.0f  }, // violet
				new[] { 1.0f, 0.55f, 0.2f  }, // orange
			};
			var c = palette[rng.Next(palette.Length)];
			return new D2DColor(1f, c[0], c[1], c[2]);
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

			pulse += 0.04f;

			for (int i = 0; i < orbs.Length; i++)
			{
				var o = orbs[i];
				o.Pos = new D2DPoint(o.Pos.X + o.Vel.X, o.Pos.Y + o.Vel.Y);

				if (o.Pos.X < o.Radius || o.Pos.X > sceneW - o.Radius)
					o.Vel = new D2DPoint(-o.Vel.X, o.Vel.Y);
				if (o.Pos.Y < o.Radius || o.Pos.Y > sceneH - o.Radius)
					o.Vel = new D2DPoint(o.Vel.X, -o.Vel.Y);

				orbs[i] = o;
			}

			SceneChanged = true;
		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			EnsureSceneTarget();
			if (sceneGraphics == null || blurEffect == null || bloomEffect == null)
			{
				g.Clear(D2DColor.Black);
				return;
			}

			// 1) Render the emissive scene into the offscreen bitmap.
			sceneGraphics.BeginRender();
			sceneGraphics.Clear(D2DColor.Black);
			DrawScene(sceneGraphics);
			sceneGraphics.EndRender();

			// Refresh the bitmap handle after each EndRender — the underlying
			// ID2D1Bitmap is stable for the lifetime of the render target, but
			// we rebuild the effect inputs to be explicit.
			sceneBitmap?.Dispose();
			sceneBitmap = sceneGraphics.GetBitmap();
			if (sceneBitmap == null) return;

			g.Clear(D2DColor.Black);

			if (bloomEnabled)
			{
				blurEffect.SetInput(0, sceneBitmap);
				blurEffect.StandardDeviation = bloomAmount;

				bloomEffect.SetInput(0, blurEffect);   // halo (underneath)
				bloomEffect.SetInput(1, sceneBitmap);  // crisp scene (on top, added)

				g.DrawEffect(bloomEffect);
			}
			else
			{
				g.DrawBitmap(sceneBitmap, new D2DRect(0, 0, sceneW, sceneH));
			}

			DrawHUD(g);
		}

		void DrawScene(D2DGraphics g)
		{
			// Moving glowing orbs: bright core with a soft falloff drawn as a few
			// concentric translucent ellipses. The real glow comes from the blur
			// pass — this is just a seed emissive shape.
			foreach (var o in orbs)
			{
				for (int k = 5; k >= 1; k--)
				{
					float r = o.Radius * k * 0.6f;
					float a = 0.12f / k;
					g.FillEllipse(o.Pos, r, new D2DColor(a, o.Core.r, o.Core.g, o.Core.b));
				}
				g.FillEllipse(o.Pos, o.Radius * 0.55f, o.Core);
				g.FillEllipse(o.Pos, o.Radius * 0.25f, D2DColor.White);
			}

			// A pulsing neon text in the middle for a second glow source.
			float s = 0.6f + 0.4f * (float)Math.Sin(pulse);
			var textColor = new D2DColor(1f, 0.9f * s + 0.1f, 1f * s, 0.7f * s + 0.3f);
			g.DrawText("BLOOM", textColor, "Segoe UI", 96,
				new D2DRect(0, sceneH * 0.5f - 70, sceneW, 140),
				DWriteTextAlignment.Center, DWriteParagraphAlignment.Center);
		}

		void DrawHUD(D2DGraphics g)
		{
			string line = $"Bloom: {(bloomEnabled ? "ON" : "OFF")}   " +
			              $"Blur σ: {bloomAmount:F1}   " +
			              $"[SPACE] toggle   [↑/↓] intensity";
			g.DrawText(line, D2DColor.White, "Segoe UI", 13,
				new D2DRect(10, 8, ClientSize.Width - 20, 24));
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			switch (e.KeyCode)
			{
				case Keys.Space:
					bloomEnabled = !bloomEnabled;
					break;
				case Keys.Up:
					bloomAmount = Math.Min(60f, bloomAmount + 1.5f);
					break;
				case Keys.Down:
					bloomAmount = Math.Max(0f, bloomAmount - 1.5f);
					break;
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			bloomEffect?.Dispose();
			blurEffect?.Dispose();
			sceneBitmap?.Dispose();
			sceneGraphics?.Dispose();
		}
	}
}
