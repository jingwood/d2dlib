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
    public class D2DFontFormat : D2DObject
    {
        internal D2DFontFormat(HANDLE handle) : base(handle) { }
    }

    public class D2DTextLayout : D2DObject
    {
        internal D2DTextLayout(HANDLE handle) : base(handle) { }
    }
}
