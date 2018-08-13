# d2dlib

A .NET library that provides the hardware-accelerated high performance immediate drawing functionality via Direct2D API.

By using the graphics context to draw anything on windows form, control or even in memory. The interface of graphics context is designed like standard Windows Form GDI+ graphics interface, it's easily use and user friendly.

| Project | Language | Description | Output DLL | 
| --- | --- | --- | --- |
| d2dlib | VC++ | Wrapper host side library, calling Windows SDK and Direct2D API | d2dlib.dll | 
| d2dlibexport | C# | Wrapper client side library, export the interface provided from d2dlib | d2dlibexport.dll |
| d2dlibexportwinform | C# | Provides the .NET classes used in windows form development, like D2DWinForm and D2DControl that use Direct2D hardware-acceleration rendering | d2dlibwinform.dll |

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
protected override void OnRender(D2DGraphics g)
{
  g.DrawText("Hello World", D2DColor.Yellow, this.Font, 100, 200);
}
```

## Draw ellipse

```csharp
var ellipse = new D2DEllipse(0, 0, 10, 10);
g.DrawEllipse(ref ellipse, D2DColor.Gray);
```

## Memory device bitmap

```csharp
var bmpGraphics = this.Device.CreateBitmapGraphics(1024, 1024);
bmpGraphics.BeginRender();
bmpGraphics.FillRectangle(170, 790, 670, 80, new D2DColor(0.4f, D2DColor.Black));
bmpGraphics.DrawText("This is Direct2D device bitmap", D2DColor.Goldenrod, this.Font, 180, 800);
bmpGraphics.EndRender();

// draw this device bitmap on screen
g.DrawBitmap(bmpGraphics, this.ClientRectangle);
```

Learn more about [Bitmap](https://github.com/jingwood/d2dlib/wiki/Bitmap).

# Examples

Fast images rendering
![Image Drawing Test](snapshots/imagetest.png)
See [source code](src/Examples/Demos/ImageTest.cs)

Custom draw on memory bitmap
![Bitmap Custom Draw](snapshots/bitmap_rendering.png)
See [source code](src/Examples/Demos/BitmapCustomDraw.cs)

Star space simulation
![Star Space](snapshots/starspace.png)
See [source code](src/Examples/Demos/StarSpace.cs)

Subtitle rendering
![Subtitle](snapshots/subtitle.png)
See [source code](src/Examples/Demos/Subtitle.cs)

Whiteboard App
![whiteboard](snapshots/whiteboard.png)\
See [source code](src/Examples/Demos/Whiteboard.cs)
