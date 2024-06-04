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

namespace unvell.D2DLib
{
	public class D2DSolidColorBrush : D2DBrush
	{
		private D2DColor color;

		public D2DColor Color
		{
			get
			{
				return color;
			}
			set
			{
				D2D.SetSolidColorBrushColor(this.Handle, this.color = value);
			}
		}

		internal D2DSolidColorBrush(HANDLE handle, D2DColor color)
		  : base(handle)
		{
			this.color = color;
		}
	}

	public class D2DSolidColorTextBrush : D2DObject
	{
		private D2DColor color;

		public D2DColor Color
		{
			get
			{
				return color;
			}
		}

		internal D2DSolidColorTextBrush(HANDLE handle, D2DColor color) : base(handle)
		{
			this.color = color;
		}
	}
}
