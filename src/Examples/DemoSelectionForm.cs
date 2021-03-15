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
 
using System;
using System.Reflection;
using System.Windows.Forms;
using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples
{
	public partial class DemoSelectionForm : Form
	{
		class DemoEntry
		{
			public Type Type { get; set; }
			public string Name { get; set; }
			public override string ToString() { return Name; }
		}

		public DemoSelectionForm()
		{
			InitializeComponent();

			Text = "Example Menu";

			loadItems();

			lstDemos.KeyDown += ProcessKeyDown;
			lstExamples.KeyDown += ProcessKeyDown;
			this.KeyDown += ProcessKeyDown;

			lstDemos.Focus();
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			
			switch (e.KeyCode)
			{
				case Keys.Escape:
					Close();
					break;
			}
		}

		private void loadItems()
		{
			var types = Assembly.GetAssembly(this.GetType()).GetTypes();

			foreach (var t in types)
			{
				if (t.IsSubclassOf(typeof(DemoForm)) || t.IsSubclassOf(typeof(ExampleForm)))
				{
					string name = t.Name;
					if (name.EndsWith("Form"))
					{
						name = name.Substring(0, name.Length - 4);
					}

					if (t.IsSubclassOf(typeof(DemoForm)))
					{
						lstDemos.Items.Add(new DemoEntry
						{
							Type = t,
							Name = name
						});
					}
					else if (t.IsSubclassOf(typeof(ExampleForm)))
					{
						lstExamples.Items.Add(new DemoEntry
						{
							Type = t,
							Name = name
						});
					}
				}
			}

			lstDemos.Sorted = true;
			lstExamples.Sorted = true;

			lstDemos.SelectedIndexChanged += SelectItem;
			lstExamples.SelectedIndexChanged += SelectItem;
		}

		private void SelectItem(object sender, EventArgs args)
		{
			if (sender is ListBox listBox && listBox.SelectedItem != null)
			{
				var f = (Form)System.Activator.CreateInstance(((DemoEntry)listBox.SelectedItem).Type);
				f.ShowDialog();
			}
		}

		private void ProcessKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			lstExamples.Width = (int)(this.ClientRectangle.Width * 0.5);
		}
	}

	public class DemoForm : D2DForm
	{
		public DemoForm()
		{
			var screenSize = Screen.FromControl(this).WorkingArea.Size;
			Size = new System.Drawing.Size((int)(screenSize.Width * 0.8f), (int)(screenSize.Height * 0.9f));
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		}
	}

  public class ExampleForm : D2DForm
  {
    public ExampleForm()
    {
      var screenSize = Screen.FromControl(this).WorkingArea.Size;
      Size = new System.Drawing.Size((int)(screenSize.Width * 0.6f), (int)(screenSize.Height * 0.7f));
      StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
    }
  }
}
