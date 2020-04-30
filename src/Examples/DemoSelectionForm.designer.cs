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

namespace unvell.D2DLib.Examples
{
	partial class DemoSelectionForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lstDemos = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lstExamples = new System.Windows.Forms.ListBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.SuspendLayout();
			// 
			// lstDemos
			// 
			this.lstDemos.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstDemos.FormattingEnabled = true;
			this.lstDemos.ItemHeight = 25;
			this.lstDemos.Location = new System.Drawing.Point(2, 40);
			this.lstDemos.Margin = new System.Windows.Forms.Padding(4);
			this.lstDemos.Name = "lstDemos";
			this.lstDemos.Size = new System.Drawing.Size(477, 591);
			this.lstDemos.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(2, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(934, 38);
			this.label1.TabIndex = 1;
			this.label1.Text = "Choose an example...";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lstExamples
			// 
			this.lstExamples.Dock = System.Windows.Forms.DockStyle.Right;
			this.lstExamples.FormattingEnabled = true;
			this.lstExamples.ItemHeight = 25;
			this.lstExamples.Location = new System.Drawing.Point(484, 40);
			this.lstExamples.Name = "lstExamples";
			this.lstExamples.Size = new System.Drawing.Size(452, 591);
			this.lstExamples.TabIndex = 2;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Location = new System.Drawing.Point(479, 40);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(5, 591);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// DemoSelectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(938, 633);
			this.Controls.Add(this.lstDemos);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.lstExamples);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "DemoSelectionForm";
			this.Padding = new System.Windows.Forms.Padding(2);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FormSelectionForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstDemos;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lstExamples;
		private System.Windows.Forms.Splitter splitter1;
	}
}