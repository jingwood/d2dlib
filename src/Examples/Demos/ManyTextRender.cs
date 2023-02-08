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

using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.Demos
{
	public partial class ManyTextRender : DemoForm
	{
		private static readonly Random rand = new Random();
		List<Node> nodes = new List<Node>();

		public ManyTextRender()
		{
			Text = "ManyTextRender - d2dlib Examples";
			BackColor = Color.Black;

			AnimationDraw = true;
			ShowFPS = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			const int NODE_COUNT = 1000;

			for (int i = 0; i < NODE_COUNT; i++)
			{
				var node = new Node()
				{
					text = "Node" + i,
					rect = new D2DRect(rand.Next(ClientRectangle.Width), rand.Next(ClientRectangle.Height), 100, 20),
					color = D2DColor.Randomly(),
					fontSize = rand.Next(12, 24),
					speed = (float)(rand.NextDouble() * 2.0),
				};

				nodes.Add(node);
			}
		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			using (var font = new Font("Arial", 12))
			{
				foreach (var node in nodes)
				{
					using (var brush = this.Device.CreateSolidColorBrush(node.color))
					{
						var rect = node.rect;

						//rect.Size = g.MeasureText(node.text, this.Font.Name, node.fontSize, node.rect.Size);
						//rect.Width += 10;
						//rect.Height += 10;

						//g.FillRectangle(rect, brush);


						//using (var f = new Font(this.Font.FontFamily, fontSize))
						//{
						var textRect = rect;
						textRect.X += 5;
						textRect.Y += 5;
						g.DrawText(node.text, node.color, textRect.X, textRect.Y);
						//}
					}
				}
			}
		}

		protected override void OnFrame()
		{
			base.OnFrame();

			foreach (var node in nodes)
			{
				node.rect.X += node.speed;

				if (node.rect.X > ClientRectangle.Width)
				{
					node.rect.X = 0 - node.rect.Width - 10;
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
		}
	}

	class Node
	{
		public string text;
		public D2DRect rect;
		public D2DColor color;
		public float fontSize;
		public float speed;
	}

}
