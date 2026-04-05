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
	/// Demonstrates GPU-cached Geometry Realization vs normal geometry drawing.
	/// Draws thousands of complex geometries and compares FPS.
	/// </summary>
	class GeometryRealizationDemo : DemoForm
	{
		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		double drawTimeMs = 0;

		D2DPathGeometry starPath;
		D2DGeometryRealization filledRealization;
		D2DGeometryRealization strokedRealization;
		D2DSolidColorBrush fillBrush;
		D2DSolidColorBrush strokeBrush;

		bool useRealization = true;
		int starCount = 500;
		float angle = 0;

		static readonly Random rand = new Random(42);

		struct StarInstance
		{
			public float x, y, scale, rotation;
		}

		StarInstance[] stars;

		public GeometryRealizationDemo()
		{
			Text = "Geometry Realization Demo - d2dlib Examples";
			Size = new System.Drawing.Size(1100, 800);
			AnimationDraw = true;
			ShowFPS = true;
			VSyncDisabled = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Create a complex star path geometry
			starPath = Device.CreatePathGeometry();
			CreateStarGeometry(starPath, 0, 0, 20, 8, 5);
			starPath.ClosePath();

			// Create GPU-cached realizations
			filledRealization = Device.CreateFilledGeometryRealization(starPath, 0.5f);
			strokedRealization = Device.CreateStrokedGeometryRealization(starPath, 0.5f, 1.5f);

			fillBrush = Device.CreateSolidColorBrush(new D2DColor(0.9f, 0.7f, 0.2f));
			strokeBrush = Device.CreateSolidColorBrush(new D2DColor(0.6f, 0.4f, 0.1f));

			GenerateStars();
		}

		void GenerateStars()
		{
			stars = new StarInstance[starCount];
			for (int i = 0; i < starCount; i++)
			{
				stars[i] = new StarInstance
				{
					x = (float)(rand.NextDouble() * ClientSize.Width),
					y = (float)(rand.NextDouble() * ClientSize.Height),
					scale = 0.3f + (float)(rand.NextDouble() * 1.5f),
					rotation = (float)(rand.NextDouble() * 360),
				};
			}
		}

		void CreateStarGeometry(D2DPathGeometry path, float cx, float cy, float radius, float innerRadius, int points)
		{
			var pathPoints = new D2DPoint[points * 2];
			for (int i = 0; i < points * 2; i++)
			{
				float r = (i % 2 == 0) ? radius : innerRadius;
				double a = Math.PI * 2 * i / (points * 2) - Math.PI / 2;
				pathPoints[i] = new D2DPoint(
					cx + (float)(r * Math.Cos(a)),
					cy + (float)(r * Math.Sin(a)));
			}
			path.AddLines(pathPoints);
		}

		protected override void OnFrame()
		{
			base.OnFrame();
			angle += 0.5f;
			SceneChanged = true;
		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			g.Clear(new D2DColor(0.08f, 0.08f, 0.12f));

			sw.Restart();

			for (int i = 0; i < stars.Length; i++)
			{
				var s = stars[i];

				g.PushTransform();
				g.TranslateTransform(s.x, s.y);
				g.ScaleTransform(s.scale, s.scale);
				g.RotateTransform(s.rotation + angle);

				if (useRealization)
				{
					g.DrawGeometryRealization(filledRealization, fillBrush);
					g.DrawGeometryRealization(strokedRealization, strokeBrush);
				}
				else
				{
					g.FillPath(starPath, new D2DColor(0.9f, 0.7f, 0.2f));
					g.DrawPath(starPath, new D2DColor(0.6f, 0.4f, 0.1f), 1.5f);
				}

				g.PopTransform();
			}

			sw.Stop();
			// Smooth the display with exponential moving average
			drawTimeMs = drawTimeMs * 0.9 + sw.Elapsed.TotalMilliseconds * 0.1;

			// UI
			string mode = useRealization ? "REALIZATION (GPU cached)" : "NORMAL (re-tessellated)";
			string label = $"{mode}  |  {starCount} stars  |  Draw: {drawTimeMs:F2} ms  |  SPACE: toggle  |  UP/DOWN: count";
			g.DrawText(label, D2DColor.White, "Segoe UI", 14,
				new D2DRect(10, 10, ClientSize.Width - 20, 30));
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			switch (e.KeyCode)
			{
				case Keys.Space:
					useRealization = !useRealization;
					break;

				case Keys.Up:
					starCount = Math.Min(5000, starCount + 100);
					GenerateStars();
					break;

				case Keys.Down:
					starCount = Math.Max(100, starCount - 100);
					GenerateStars();
					break;
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			strokeBrush?.Dispose();
			fillBrush?.Dispose();
			strokedRealization?.Dispose();
			filledRealization?.Dispose();
			starPath?.Dispose();
		}
	}
}
