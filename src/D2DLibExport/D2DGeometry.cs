﻿/*
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
	public class D2DGeometry : D2DObject
	{
		internal D2DDevice Device { get; private set; }

		internal D2DGeometry(D2DDevice device, HANDLE geoHandle)
		  : base(geoHandle)
		{
			this.Device = device;
		}

		// FIXME: TO be implemented
		//public void FillGeometry(D2DBrush brush, [Optional] D2DBrush opacityBrush)
		//{
		//  // TODO
		//}

		public D2DRect GetBounds()
		{
			var rect = new D2DRect();
			D2D.GetGeometryBounds(this.Handle, ref rect);
			return rect;
		}

		public override void Dispose()
		{
			if (this.Handle != IntPtr.Zero) D2D.DestroyGeometry(this.Handle);
			this.handle = IntPtr.Zero;
		}
	}
}
