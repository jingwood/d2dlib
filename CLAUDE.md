# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository overview

`d2dlib` is a .NET library that wraps Direct2D for hardware-accelerated, immediate-mode 2D rendering on Windows. It is split across a native C++ DLL and several .NET projects that consume it via P/Invoke.

The exposed programming model mirrors Windows Forms `Graphics`: subclass `D2DForm` / `D2DControl`, override `OnRender(D2DGraphics g)` (NOT `OnPaint`), and draw with `g`.

## Building

The solution is `src/D2DLib.sln`. There are only two configurations: `x86` and `x64` (no `Any CPU`) — Direct2D requires an architecture-specific target.

```shell
# Build the whole solution (run from src/)
msbuild D2DLib.sln /p:Configuration=Release /p:Platform=x64
msbuild D2DLib.sln /p:Configuration=Debug   /p:Platform=x86
```

With only Visual Studio 2026 installed, add `/p:PlatformToolset=v145` (the project names v143).

The native `d2dlib` C++ project must build first; the .NET projects depend on it and copy the resulting DLL into their output via [src/NativeLibraries.props](src/NativeLibraries.props). DLL naming the managed code expects (see [src/D2DLibExport/D2DLib.cs](src/D2DLibExport/D2DLib.cs)):

| Config        | x86              | x64              |
| ------------- | ---------------- | ---------------- |
| Debug         | `d2dlib32d.dll`  | `d2dlib64d.dll`  |
| Release       | `d2dlib32.dll`   | `d2dlib64.dll`   |

The `X86` / `X64` preprocessor symbol is set automatically from `$(Platform)` in [src/Directory.Build.props](src/Directory.Build.props) and selects which DLL name `DllImport` uses. If you add new P/Invoke entry points, follow that same `#if X86 / X64` pattern.

CI is currently disabled (`.github/workflows/CI.yaml.disabled`); there is no automated test runner configured in-tree.

## Architecture

Three layers, each in its own project:

1. **`src/d2dlib/`** — Native C++ wrapper (`D2DLib.vcxproj`, output `d2dlib*.dll`).
   Wraps Direct2D 1.1 / DirectWrite / WIC into a flat C ABI (`extern "C"`, `__cdecl`) so it can be P/Invoked from .NET. Central type is `D2DContext` in [src/d2dlib/Context.h](src/d2dlib/Context.h), which holds the `ID2D1Factory1`, the D3D11/DXGI device chain, the swap chain, the `ID2D1DeviceContext` (for the effects pipeline), the active `ID2D1RenderTarget`, the matrix stack, and VSync state. The same `D2DContext` is used for both HWND-backed contexts and off-screen `BitmapRenderTarget` contexts — the `isBitmapRenderTarget` flag and the `renderTarget` vs `deviceContext` split exist so the same drawing primitives work for both.
   Files are organized by Direct2D concept: `Context`, `Brush`, `Bitmap`, `Geometry`, `Pen`, `Text`, `Transform`, `Effect`, `Style`, `Simple` (one-off draw helpers).
   `TextLayout` holds the device-independent DirectWrite API (formats/layouts on the process-wide shared factory, usable without a context). Color-taking draw calls go through `GetSolidBrush` (one cached brush per context) — don't create a brush per call. `CreateContextEx` adds flags (blt-model `GDI_COMPATIBLE`, `OPAQUE`, `VSYNC`, `FORCE_WARP`) and device sharing; `CreateWicBitmapContext` renders off-screen with pixel read-back.

2. **`src/D2DLibExport/`** — Managed wrapper (`d2dlibexport.dll`, namespace `unvell.D2DLib`, multi-targets `net462;netstandard2.0`).
   [D2DLib.cs](src/D2DLibExport/D2DLib.cs) is the `internal static class D2D` containing every `[DllImport]` into the native DLL. Everything else in this project (`D2DDevice`, `D2DGraphics`, `D2DBitmap`, `D2DBrush`, `D2DPen`, `D2DGeometry` and subclasses, `D2DEffect`, `D2DLayer`, …) is an object-oriented facade over those P/Invokes. `D2DStructs.cs` defines the blittable structs (e.g. `D2DRect`, `D2DPoint`, `D2DColor`, `D2DEllipse`) that cross the boundary — keep them blittable and matching the C++ side. `D2DObject` is the common base for handle-owning wrappers; new wrapper types should inherit from it for consistent disposal.

3. **`src/D2DWinForm/`** — Windows Forms integration (`d2dwinform.dll`, namespace `unvell.D2DLib.WinForm`).
   Provides `D2DControl` and `D2DForm` (in [D2DControl.cs](src/D2DWinForm/D2DControl.cs) / [D2DWinForm.cs](src/D2DWinForm/D2DWinForm.cs)). They lazily create a `D2DDevice` from `this.Handle`, set `DoubleBuffered = false`, and route paints into a `D2DGraphics` passed to a virtual `OnRender`. `FpsCounter` powers the optional `ShowFPS` overlay.

`src/D2DLibExportCore/` and `src/D2DWinFormCore/` are SDK-style .NET (Core) variants of the same managed projects (they reference the same `.cs` files) — they aren't in the main `.sln` but exist for .NET Core packaging.

### Examples and tests

- [src/Examples/](src/Examples/) — WinForms demo gallery (referenced by `D2DLib.sln`). `MenuForm` / `DemoSelectionForm` launch individual demos under `src/Examples/Demos/`. This is the primary place to look for "how do I use feature X" answers and also serves as the manual smoke-test surface.
- `src/ExamplesCore/`, `src/WindowsFormsApp1/`, `src/Tests/` exist on disk but are not in `D2DLib.sln`.

## Conventions

- The `Directory.Build.props` adds many global `using`s and type aliases (`UINT`, `HWND`, `HANDLE`, `HRESULT`, `BOOL`, `FLOAT`, plus `using System.Numerics`). Don't redeclare them in individual files.
- Nullable reference types are enabled project-wide; `LangVersion` is 10.
- Assemblies are public-signed with [src/d2dlib.snk](src/d2dlib.snk) — don't disable signing.
- Package version is set centrally via `<Version>` in `Directory.Build.props`.
- Matching the existing C++/C# pairs is important: a new native function needs (a) the C++ implementation in the right `d2dlib/*.cpp`, (b) an `extern "C"` export, (c) a `[DllImport]` in `D2DLibExport/D2DLib.cs`, and (d) a method on the appropriate managed wrapper class.
- Off-screen rendering uses `Device.CreateBitmapGraphics(...)`, which produces another `D2DGraphics` whose work must be bracketed by `BeginRender()` / `EndRender()` before its bitmap can be drawn back to the screen.
- Transforms are stack-based: pair every `PushTransform()` with a `PopTransform()`.
