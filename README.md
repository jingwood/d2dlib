# d2dlib

A .NET library that provides the hardware-accelerated high performance immediate drawing functionality via Direct2D API.

By using the graphics context to draw anything on windows form, control or draw in memory. The interface of graphics context is designed like standard Windows Form GDI+ graphics interface, it's easily use and user friendly.

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

*Notice*: The platform target of application project must be set to x86 when using default build configuration.

# Basic rendering

## Draw rectangle

```csharp
var rect = new D2DRect(0, 0, 10, 10);
g.DrawEllipse(rect, D2DColor.Red);
```

## Draw ellipse

```csharp
var ellipse = new D2DEllipse(0, 0, 10, 10);
g.DrawEllipse(ellipse, D2DColor.Gray);
```

## Draw text

```csharp
protected override void OnRender(D2DGraphics g)
{
  g.DrawText("Hello World", D2DColor.Yellow, this.Font, 100, 200);
}
```

## Using brush object

### Solid color brush

```csharp
var brush = Device.CreateSolidColorBrush(new D2DColor(1, 0, 0.5));
g.DrawEllipse(rect, brush);
```

### Linear and radio gradient brush

```csharp
var brush = Device.CreateLinearGradientBrush(new D2DPoint(0, 0), new D2DPoint(200, 100),
  new D2DGradientStop[] {
    new D2DGradientStop(0, D2DColor.White),
    new D2DGradientStop(0.5, D2DColor.Green),
    new D2DGradientStop(1, D2DColor.Black),
  });
```

## Draw bitmap

```csharp
g.DrawBitmap(bmp, this.ClientRectangle);
```

## Convert GDI+ bitmap to Direct2D bitmap for getting high performance rendering

```csharp
// convert to Direct2D bitmap
var d2dbmp = Device.CreateBitmapFromGDIBitmap(gdiBitmap);

// draw Direct2D bitmap
g.DrawBitmap(d2dbmp, this.ClientRectangle);
```

# Drawing on memory bitmap

## Drawing on GDI+ bitmap

```csharp
// create and draw on GDI+ bitmap
var gdiBmp = new Bitmap(1024, 1024);
using (Graphics g = Graphics.FromImage(gdiBmp))
{
  g.DrawString("This is GDI+ bitmap layer", new Font(this.Font.FontFamily, 48), Brushes.Black, 10, 10);
}

// draw memory bitmap on screen
g.DrawBitmap(gdiBmp, this.ClientRectangle);
```

Learn more about [Bitmap](https://github.com/jingwood/d2dlib/wiki/Bitmap).
See [Example code](src/Examples/Demos/BitmapCustomDraw.cs)

## Drawing on Direct2D memory bitmap

```csharp
var bmpGraphics = this.Device.CreateBitmapGraphics(1024, 1024);
bmpGraphics.BeginRender();
bmpGraphics.FillRectangle(170, 790, 670, 80, new D2DColor(0.4f, D2DColor.Black));
bmpGraphics.DrawText("This is Direct2D device bitmap", D2DColor.Goldenrod, this.Font, 180, 800);
bmpGraphics.EndRender();

// draw this device bitmap on screen
g.DrawBitmap(bmpGraphics, this.ClientRectangle);
```

*Note:* When create a Direct2D Device bitmap, do not forget call `BeginRender` and `EndRender` method.

# Using transform

By calling `PushTransform` and `PopTransform` to make a transform session.

```csharp
g.PushTransform();

// rotate 45 degree
g.RotateTransform(45, centerPoint);

g.DrawBitmap(mybmp, rect);
g.PopTransform();
```

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
