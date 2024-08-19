using System.Drawing;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using unvell.D2DLib.Examples.Demos;
using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.SampleCode
{
	internal class GeometriesCombine : ExampleForm
	{
		private const FLOAT left = 250;
		private const FLOAT offset = 50f;
		private const FLOAT size = 100f;
		
		private const string fontName = "Arials";
		private const FLOAT fontSize = 20;

		public GeometriesCombine()
		{
			Text = "Demonstrate combining two geometries - d2dlib Examples";
		}

		protected override void OnRender(D2DGraphics g)
		{
			/// prepare 
			D2DColor borderColor = D2DColor.DarkGray;
			D2DColor fillColor = D2DColor.CornflowerBlue;
			fillColor.a = 0.25f;

			D2D1CombineMode[] modes = {
				D2D1CombineMode.Union,
				D2D1CombineMode.Intersect,
				D2D1CombineMode.XOR,
				D2D1CombineMode.Exclude  
			};

			string[] modeNames = { "Union", "Intersect", "XOR", "Exclude" };

			/// create test geometries
			D2DPathGeometry geoSquare = CreateGeometry(false, new D2DRect(left + offset, offset, size, size));
			D2DPathGeometry geoDiamond = CreateGeometry(true, new D2DRect(left + offset + size / 2, offset, size, size));

			/// start drawing
			g.Clear(D2DColor.White);

			// draw test geometries without combining them
			D2DPoint pt = new D2DPoint(0, 0);
			DrawGeometries(geoSquare, geoDiamond, g, pt, fillColor, borderColor, 2);

			pt.Y += (size + offset / 2);

			// draw test geometries by combining them
			for (int i = 0; i < modes.Length; i++)
			{
				DrawCombinedGeometries(geoSquare, geoDiamond, modes[i], modeNames[i],
					g, pt, fillColor, borderColor, 2);

				pt.Y += (size + offset / 2);
			}

		}

		/// <summary>
		/// Draw the geometry combining G1 and G2 geometries using combineMode 
		/// </summary>
		protected void DrawCombinedGeometries(D2DPathGeometry G1, D2DPathGeometry G2, 
			D2D1CombineMode combineMode, string modeName, D2DGraphics g, D2DPoint topLeft,
			D2DColor fillColor, D2DColor borderColor, float borderWidth)
		{
			using (var path = Device.CreateCombinedGeometry(G1, G2, combineMode))
			{
				g.PushTransform();
				g.TranslateTransform(topLeft.X, topLeft.Y);

				g.DrawText(modeName, D2DColor.DimGray, fontName, fontSize, offset, offset + size / 2 - fontSize / 2);

				g.FillPath(path, fillColor);
				g.DrawPath(path, borderColor, borderWidth);

				g.PopTransform();
			}
		}

		/// <summary>
		/// Draw G1 and G2 geometries without combining them
		/// </summary>
		protected void DrawGeometries(D2DPathGeometry G1, D2DPathGeometry G2,
			D2DGraphics g, D2DPoint topLeft, D2DColor fillColor, D2DColor borderColor, float borderWidth)
		{
			g.PushTransform();
			g.TranslateTransform(topLeft.X, topLeft.Y);

			g.DrawText("Before combining", D2DColor.DimGray, fontName, fontSize, offset, offset + size / 2 - fontSize / 2);

			g.FillPath(G1, fillColor);
			g.DrawPath(G1, borderColor, borderWidth);

			g.FillPath(G2, fillColor);
			g.DrawPath(G2, borderColor, borderWidth);

			g.PopTransform();
		}

		/// <summary>
		/// Create and return a geometry representing a square or a diamond shape 
		/// inside the given rect
		/// </summary>
		protected D2DPathGeometry CreateGeometry(bool diamond, D2DRect rect)
		{
			Vector2 ptStart;
			Vector2[] points = new Vector2[3];

			if (diamond)
			{
				ptStart = new Vector2(rect.left, rect.top + rect.Height / 2);
				points[0] = new Vector2(rect.left + rect.Width / 2, rect.top);
				points[1] = new Vector2(rect.right, rect.top + rect.Height / 2);
				points[2] = new Vector2(rect.left + rect.Width / 2, rect.bottom);
			}
			else
			{
				ptStart = new Vector2(rect.left, rect.top);
				points[0] = new Vector2(rect.right, rect.top);
				points[1] = new Vector2(rect.right, rect.bottom);
				points[2] = new Vector2(rect.left, rect.bottom);
			}

			D2DPathGeometry geo = Device.CreatePathGeometry();
			geo.SetStartPoint(ptStart);
			geo.AddLines(points);
			geo.ClosePath();

			return geo;
		}


	}
}
