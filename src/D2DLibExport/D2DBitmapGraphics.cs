using System;
using System.Collections.Generic;
using System.Text;

using FLOAT = System.Single;
using UINT = System.UInt32;
using UINT32 = System.UInt32;
using HWND = System.IntPtr;
using HANDLE = System.IntPtr;
using HRESULT = System.Int64;
using BOOL = System.Int32;

namespace unvell.D2DLib
{
	public class D2DBitmapGraphics : D2DGraphics, IDisposable
	{
		internal D2DBitmapGraphics(HANDLE handle)
			: base(handle)
		{
		}

		public D2DBitmap GetBitmap()
		{
			HANDLE bitmapHandle = D2D.GetBitmapRenderTargetBitmap(this.DeviceHandle);
			return bitmapHandle == HANDLE.Zero ? null : new D2DBitmap(bitmapHandle);
		}

		public void Dispose()
		{
			D2D.DestoryBitmapRenderTarget(this.DeviceHandle);
		}
	}

}
