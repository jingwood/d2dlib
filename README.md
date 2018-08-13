# d2dlib

Direct2D wrapper library for .NET applications. This library uses Direct2D graphics API to provide the high performance hardware accelerated instant rendering ability. 

The graphics context class is designed like normal Windows Form GDI+ graphics API, easily use and user friendly.

# About the projects

| Project | Language | Description | Output DLL | 
| --- | --- | --- | --- |
| d2dlib | VC++ | Direct2D wrapper host side library, calling Windows SDK and Direct2D API | d2dlib.dll | 
| d2dlibexport | C# | Direct2D client side interface between .NET and VC++ library, calling d2dlib.dll | d2dlibexport.dll |
| d2dlibexportwinform | C# | Provides the .NET classes like D2DWinForm and D2DControl that use Direct2D hardware-acceleration rendering | d2dlibwinform.dll |

# How to use

1. Compile every projects or use the DLLs from the binary folder directly
2. Add `d2dlibexport.dll` and `d2dlibwinform.dll` as application references
3. Put `d2dlib.dll` in the `Debug`, `Release` or folder where application runs
4. Make windows form or control inherited from `D2DForm` or `D2DControl` class
5. Override `OnRender(D2DGraphics g)` method (do not override .NET `OnPaint` method)
6. Draw anything inside `OnRender` method via the `g` context

*Notice*

The platform target of application project must be set to x86 when using default build configuration.

# Hello World

## Draw text

```csharp
g.DrawText("Hello World", D2DColor.Yellow, this.Font, 100, 200);
```

## Draw ellipse

```csharp
protected override void OnRender(D2DGraphics g)
{
  var ellipse = new D2DEllipse(0, 0, 10, 10);
  g.DrawEllipse(ref ellipse, D2DColor.Gray);
}
```

## Memory device bitmap

```csharp
var bmpGraphics = this.Device.CreateBitmapGraphics(1024, 1024);
bmpGraphics.BeginRender();
bmpGraphics.FillRectangle(170, 790, 670, 80, new D2DColor(0.4f, D2DColor.Black));
bmpGraphics.DrawText("This is Direct2D device bitmap", D2DColor.Goldenrod, this.Font, 180, 800);
bmpGraphics.EndRender();
```

# Examples

![Image Drawing Test](snapshots/imagetest.png)

![Bitmap Custom Draw](snapshots/bitmap_rendering.png)

![Star Space](snapshots/starspace.png)

![Subtitle](snapshots/subtitle.png)

![whiteboard](snapshots/whiteboard.png)
