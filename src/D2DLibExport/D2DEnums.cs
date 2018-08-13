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
using System.Collections.Generic;
using System.Text;

namespace unvell.D2DLib
{

	enum D2D1_DEBUG_LEVEL
	{
		D2D1_DEBUG_LEVEL_NONE = 0,
		D2D1_DEBUG_LEVEL_ERROR = 1,
		D2D1_DEBUG_LEVEL_WARNING = 2,
		D2D1_DEBUG_LEVEL_INFORMATION = 3,
		D2D1_DEBUG_LEVEL_FORCE_DWORD = -1,
	}

	enum D2D1_FACTORY_TYPE 
	{
		//
		// The resulting factory and derived resources may only be invoked serially.
		// Reference counts on resources are interlocked, however, resource and render
		// target state is not protected from multi-threaded access.
		//
		D2D1_FACTORY_TYPE_SINGLE_THREADED = 0,

		//
		// The resulting factory may be invoked from multiple threads. Returned resources
		// use interlocked reference counting and their state is protected.
		//
		D2D1_FACTORY_TYPE_MULTI_THREADED = 1,

		D2D1_FACTORY_TYPE_FORCE_DWORD = -1
	}

	enum D2D1_RENDER_TARGET_TYPE
	{
		//
		// D2D is free to choose the render target type for the caller.
		//
		D2D1_RENDER_TARGET_TYPE_DEFAULT = 0,

		//
		// The render target will render using the CPU.
		//
		D2D1_RENDER_TARGET_TYPE_SOFTWARE = 1,

		//
		// The render target will render using the GPU.
		//
		D2D1_RENDER_TARGET_TYPE_HARDWARE = 2,

		D2D1_RENDER_TARGET_TYPE_FORCE_DWORD = -1
	}

	enum D2D1_RENDER_TARGET_USAGE
	{
		D2D1_RENDER_TARGET_USAGE_NONE = 0x00000000,

		//
		// Rendering will occur locally, if a terminal-services session is established, the
		// bitmap updates will be sent to the terminal services client.
		//
		D2D1_RENDER_TARGET_USAGE_FORCE_BITMAP_REMOTING = 0x00000001,

		//
		// The render target will allow a call to GetDC on the ID2D1GdiInteropRenderTarget
		// interface. Rendering will also occur locally.
		//
		D2D1_RENDER_TARGET_USAGE_GDI_COMPATIBLE = 0x00000002,

		D2D1_RENDER_TARGET_USAGE_FORCE_DWORD = -1
	}
	enum D2D1_FEATURE_LEVEL
	{
		//
		// The caller does not require a particular underlying D3D device level.
		//
		D2D1_FEATURE_LEVEL_DEFAULT = 0,

		//
		// The D3D device level is DX9 compatible.
		//
		D2D1_FEATURE_LEVEL_9 = D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_1,

		//
		// The D3D device level is DX10 compatible.
		//
		D2D1_FEATURE_LEVEL_10 = D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_0,

		D2D1_FEATURE_LEVEL_FORCE_DWORD = -1
	}

	enum D3D_FEATURE_LEVEL
	{
		D3D_FEATURE_LEVEL_9_1 = 0x9100,
		D3D_FEATURE_LEVEL_9_2 = 0x9200,
		D3D_FEATURE_LEVEL_9_3 = 0x9300,
		D3D_FEATURE_LEVEL_10_0 = 0xa000,
		D3D_FEATURE_LEVEL_10_1 = 0xa100,
		D3D_FEATURE_LEVEL_11_0 = 0xb000,
		D3D_FEATURE_LEVEL_11_1 = 0xb100
	}

	enum DXGI_FORMAT
	{
		DXGI_FORMAT_UNKNOWN = 0,
		DXGI_FORMAT_R32G32B32A32_TYPELESS = 1,
		DXGI_FORMAT_R32G32B32A32_FLOAT = 2,
		DXGI_FORMAT_R32G32B32A32_UINT = 3,
		DXGI_FORMAT_R32G32B32A32_SINT = 4,
		DXGI_FORMAT_R32G32B32_TYPELESS = 5,
		DXGI_FORMAT_R32G32B32_FLOAT = 6,
		DXGI_FORMAT_R32G32B32_UINT = 7,
		DXGI_FORMAT_R32G32B32_SINT = 8,
		DXGI_FORMAT_R16G16B16A16_TYPELESS = 9,
		DXGI_FORMAT_R16G16B16A16_FLOAT = 10,
		DXGI_FORMAT_R16G16B16A16_UNORM = 11,
		DXGI_FORMAT_R16G16B16A16_UINT = 12,
		DXGI_FORMAT_R16G16B16A16_SNORM = 13,
		DXGI_FORMAT_R16G16B16A16_SINT = 14,
		DXGI_FORMAT_R32G32_TYPELESS = 15,
		DXGI_FORMAT_R32G32_FLOAT = 16,
		DXGI_FORMAT_R32G32_UINT = 17,
		DXGI_FORMAT_R32G32_SINT = 18,
		DXGI_FORMAT_R32G8X24_TYPELESS = 19,
		DXGI_FORMAT_D32_FLOAT_S8X24_UINT = 20,
		DXGI_FORMAT_R32_FLOAT_X8X24_TYPELESS = 21,
		DXGI_FORMAT_X32_TYPELESS_G8X24_UINT = 22,
		DXGI_FORMAT_R10G10B10A2_TYPELESS = 23,
		DXGI_FORMAT_R10G10B10A2_UNORM = 24,
		DXGI_FORMAT_R10G10B10A2_UINT = 25,
		DXGI_FORMAT_R11G11B10_FLOAT = 26,
		DXGI_FORMAT_R8G8B8A8_TYPELESS = 27,
		DXGI_FORMAT_R8G8B8A8_UNORM = 28,
		DXGI_FORMAT_R8G8B8A8_UNORM_SRGB = 29,
		DXGI_FORMAT_R8G8B8A8_UINT = 30,
		DXGI_FORMAT_R8G8B8A8_SNORM = 31,
		DXGI_FORMAT_R8G8B8A8_SINT = 32,
		DXGI_FORMAT_R16G16_TYPELESS = 33,
		DXGI_FORMAT_R16G16_FLOAT = 34,
		DXGI_FORMAT_R16G16_UNORM = 35,
		DXGI_FORMAT_R16G16_UINT = 36,
		DXGI_FORMAT_R16G16_SNORM = 37,
		DXGI_FORMAT_R16G16_SINT = 38,
		DXGI_FORMAT_R32_TYPELESS = 39,
		DXGI_FORMAT_D32_FLOAT = 40,
		DXGI_FORMAT_R32_FLOAT = 41,
		DXGI_FORMAT_R32_UINT = 42,
		DXGI_FORMAT_R32_SINT = 43,
		DXGI_FORMAT_R24G8_TYPELESS = 44,
		DXGI_FORMAT_D24_UNORM_S8_UINT = 45,
		DXGI_FORMAT_R24_UNORM_X8_TYPELESS = 46,
		DXGI_FORMAT_X24_TYPELESS_G8_UINT = 47,
		DXGI_FORMAT_R8G8_TYPELESS = 48,
		DXGI_FORMAT_R8G8_UNORM = 49,
		DXGI_FORMAT_R8G8_UINT = 50,
		DXGI_FORMAT_R8G8_SNORM = 51,
		DXGI_FORMAT_R8G8_SINT = 52,
		DXGI_FORMAT_R16_TYPELESS = 53,
		DXGI_FORMAT_R16_FLOAT = 54,
		DXGI_FORMAT_D16_UNORM = 55,
		DXGI_FORMAT_R16_UNORM = 56,
		DXGI_FORMAT_R16_UINT = 57,
		DXGI_FORMAT_R16_SNORM = 58,
		DXGI_FORMAT_R16_SINT = 59,
		DXGI_FORMAT_R8_TYPELESS = 60,
		DXGI_FORMAT_R8_UNORM = 61,
		DXGI_FORMAT_R8_UINT = 62,
		DXGI_FORMAT_R8_SNORM = 63,
		DXGI_FORMAT_R8_SINT = 64,
		DXGI_FORMAT_A8_UNORM = 65,
		DXGI_FORMAT_R1_UNORM = 66,
		DXGI_FORMAT_R9G9B9E5_SHAREDEXP = 67,
		DXGI_FORMAT_R8G8_B8G8_UNORM = 68,
		DXGI_FORMAT_G8R8_G8B8_UNORM = 69,
		DXGI_FORMAT_BC1_TYPELESS = 70,
		DXGI_FORMAT_BC1_UNORM = 71,
		DXGI_FORMAT_BC1_UNORM_SRGB = 72,
		DXGI_FORMAT_BC2_TYPELESS = 73,
		DXGI_FORMAT_BC2_UNORM = 74,
		DXGI_FORMAT_BC2_UNORM_SRGB = 75,
		DXGI_FORMAT_BC3_TYPELESS = 76,
		DXGI_FORMAT_BC3_UNORM = 77,
		DXGI_FORMAT_BC3_UNORM_SRGB = 78,
		DXGI_FORMAT_BC4_TYPELESS = 79,
		DXGI_FORMAT_BC4_UNORM = 80,
		DXGI_FORMAT_BC4_SNORM = 81,
		DXGI_FORMAT_BC5_TYPELESS = 82,
		DXGI_FORMAT_BC5_UNORM = 83,
		DXGI_FORMAT_BC5_SNORM = 84,
		DXGI_FORMAT_B5G6R5_UNORM = 85,
		DXGI_FORMAT_B5G5R5A1_UNORM = 86,
		DXGI_FORMAT_B8G8R8A8_UNORM = 87,
		DXGI_FORMAT_B8G8R8X8_UNORM = 88,
		DXGI_FORMAT_R10G10B10_XR_BIAS_A2_UNORM = 89,
		DXGI_FORMAT_B8G8R8A8_TYPELESS = 90,
		DXGI_FORMAT_B8G8R8A8_UNORM_SRGB = 91,
		DXGI_FORMAT_B8G8R8X8_TYPELESS = 92,
		DXGI_FORMAT_B8G8R8X8_UNORM_SRGB = 93,
		DXGI_FORMAT_BC6H_TYPELESS = 94,
		DXGI_FORMAT_BC6H_UF16 = 95,
		DXGI_FORMAT_BC6H_SF16 = 96,
		DXGI_FORMAT_BC7_TYPELESS = 97,
		DXGI_FORMAT_BC7_UNORM = 98,
		DXGI_FORMAT_BC7_UNORM_SRGB = 99,
		DXGI_FORMAT_AYUV = 100,
		DXGI_FORMAT_Y410 = 101,
		DXGI_FORMAT_Y416 = 102,
		DXGI_FORMAT_NV12 = 103,
		DXGI_FORMAT_P010 = 104,
		DXGI_FORMAT_P016 = 105,
		DXGI_FORMAT_420_OPAQUE = 106,
		DXGI_FORMAT_YUY2 = 107,
		DXGI_FORMAT_Y210 = 108,
		DXGI_FORMAT_Y216 = 109,
		DXGI_FORMAT_NV11 = 110,
		DXGI_FORMAT_AI44 = 111,
		DXGI_FORMAT_IA44 = 112,
		DXGI_FORMAT_P8 = 113,
		DXGI_FORMAT_A8P8 = 114,
		DXGI_FORMAT_B4G4R4A4_UNORM = 115,
		DXGI_FORMAT_FORCE_UINT = -1
	}

	enum D2D1_ALPHA_MODE
	{
		//
		// Alpha mode should be determined implicitly. Some target surfaces do not supply
		// or imply this information in which case alpha must be specified.
		//
		D2D1_ALPHA_MODE_UNKNOWN = 0,

		//
		// Treat the alpha as premultipled.
		//
		D2D1_ALPHA_MODE_PREMULTIPLIED = 1,

		//
		// Opacity is in the 'A' component only.
		//
		D2D1_ALPHA_MODE_STRAIGHT = 2,

		//
		// Ignore any alpha channel information.
		//
		D2D1_ALPHA_MODE_IGNORE = 3,

		D2D1_ALPHA_MODE_FORCE_DWORD = -1
	}

	enum D2D1_PRESENT_OPTIONS
	{
		D2D1_PRESENT_OPTIONS_NONE = 0x00000000,

		//
		// Keep the target contents intact through present.
		//
		D2D1_PRESENT_OPTIONS_RETAIN_CONTENTS = 0x00000001,

		//
		// Do not wait for display refresh to commit changes to display.
		//
		D2D1_PRESENT_OPTIONS_IMMEDIATELY = 0x00000002,

		D2D1_PRESENT_OPTIONS_FORCE_DWORD = -1,
	}

	public enum D2DDashStyle
	{
		Solid = 0,
		Dash = 1,
		Dot = 2,
		DashDot = 3,
		DashDotDot = 4,
		Custom = 5,
	}

	public enum D2D1_ARC_SIZE
	{
		D2D1_ARC_SIZE_SMALL = 0,
		D2D1_ARC_SIZE_LARGE = 1,
	}

	public enum D2D1_SWEEP_DIRECTION
	{
		D2D1_SWEEP_DIRECTION_COUNTER_CLOCKWISE = 0,
		D2D1_SWEEP_DIRECTION_CLOCKWISE = 1,
	}

	public enum D2DAntialiasMode
	{
		//
		// The edges of each primitive are antialiased sequentially.
		//
		PerPrimitive = 0,

		//
		// Each pixel is rendered if its pixel center is contained by the geometry.
		//
		Aliased = 1,
	}

	public enum D2DBitmapInterpolationMode
	{
		//
		// Nearest Neighbor filtering. Also known as nearest pixel or nearest point
		// sampling.
		//
		NearestNeighbor = 0,

		//
		// Linear filtering.
		//
		Linear = 1,
	}

	enum DWRITE_MEASURING_MODE
	{
		/// <summary>
		/// Text is measured using glyph ideal metrics whose values are independent to the current display resolution.
		/// </summary>
		DWRITE_MEASURING_MODE_NATURAL = 0,

		/// <summary>
		/// Text is measured using glyph display compatible metrics whose values tuned for the current display resolution.
		/// </summary>
		DWRITE_MEASURING_MODE_GDI_CLASSIC = 1,

		/// <summary>
		/// Text is measured using the same glyph display metrics as text measured by GDI using a font
		/// created with CLEARTYPE_NATURAL_QUALITY.
		/// </summary>
		DWRITE_MEASURING_MODE_GDI_NATURAL = 2

	}

	/// <summary>
	/// Alignment of paragraph text along the reading direction axis relative to 
	/// the leading and trailing edge of the layout box.
	/// </summary>
	public enum DWRITE_TEXT_ALIGNMENT
	{
		/// <summary>
		/// The leading edge of the paragraph text is aligned to the layout box's leading edge.
		/// </summary>
		DWRITE_TEXT_ALIGNMENT_LEADING,

		/// <summary>
		/// The trailing edge of the paragraph text is aligned to the layout box's trailing edge.
		/// </summary>
		DWRITE_TEXT_ALIGNMENT_TRAILING,

		/// <summary>
		/// The center of the paragraph text is aligned to the center of the layout box.
		/// </summary>
		DWRITE_TEXT_ALIGNMENT_CENTER
	};

	/// <summary>
	/// Alignment of paragraph text along the flow direction axis relative to the
	/// flow's beginning and ending edge of the layout box.
	/// </summary>
	public enum DWRITE_PARAGRAPH_ALIGNMENT
	{
		/// <summary>
		/// The first line of paragraph is aligned to the flow's beginning edge of the layout box.
		/// </summary>
		DWRITE_PARAGRAPH_ALIGNMENT_NEAR,

		/// <summary>
		/// The last line of paragraph is aligned to the flow's ending edge of the layout box.
		/// </summary>
		DWRITE_PARAGRAPH_ALIGNMENT_FAR,

		/// <summary>
		/// The center of the paragraph is aligned to the center of the flow of the layout box.
		/// </summary>
		DWRITE_PARAGRAPH_ALIGNMENT_CENTER
	};

	public enum LayerOptions
	{
		None = 0x00000000,

		/// <summary>
		/// The layer will render correctly for ClearType text. If the render target was set
		/// to ClearType previously, the layer will continue to render ClearType. If the
		/// render target was set to ClearType and this option is not specified, the render
		/// target will be set to render gray-scale until the layer is popped. The caller
		/// can override this default by calling SetTextAntialiasMode while within the
		/// layer. This flag is slightly slower than the default.
		/// </summary>
		InitializeForClearType = 0x00000001,
	}
}

