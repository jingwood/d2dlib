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

using System;

namespace unvell.D2DLib
{
	/// <summary>
	/// GPU-cached geometry realization for extremely fast repeated drawing.
	/// Created from a D2DGeometry via D2DDevice, the geometry is tessellated
	/// once and cached on the GPU. Drawing a realization is much faster than
	/// drawing the original geometry each frame.
	/// </summary>
	public class D2DGeometryRealization : IDisposable
	{
		internal HANDLE Handle { get; }

		internal D2DGeometryRealization(HANDLE handle)
		{
			this.Handle = handle;
		}

		public void Dispose()
		{
			D2D.DestroyGeometryRealization(this.Handle);
		}
	}
}
