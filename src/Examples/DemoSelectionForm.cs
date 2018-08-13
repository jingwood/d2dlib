/*
 * MIT License
 * 
 * Copyright (c) 2009-2018 Jingwood, unvell.com. All right reserved.
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

			var types = Assembly.GetAssembly(this.GetType()).GetTypes();

			foreach (var t in types)
			{
				if (t.IsSubclassOf(typeof(System.Windows.Forms.Form))
					&& t.FullName.IndexOf(".Demos.") > 0)
				{
					string name = t.Name;
					if (name.EndsWith("Form"))
					{
						name = name.Substring(0, name.Length - 4);
					}
					listBox1.Items.Add(new DemoEntry
					{
						Type = t,
						Name = name
					});
				}
			}

			listBox1.Sorted = true;

			listBox1.SelectedIndexChanged += (s, e) =>
			{
				if (listBox1.SelectedItem != null)
				{
					var f = (Form)System.Activator.CreateInstance(((DemoEntry)listBox1.SelectedItem).Type);
					f.ShowDialog();
				}
			};

			listBox1.KeyDown += ListBox1_KeyDown;
		}

		private void ListBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}
	}
}
