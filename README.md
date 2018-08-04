# d2dlib

Direct2D wrapper library for .NET applications.

# About the projects

| Project | Language | Description | Output DLL | 
| --- | --- | --- | --- |
| d2dlib | VC++ | Direct2D wrapper host side library, calling Windows SDK and Direct2D API | d2dlib.dll | 
| d2dlibexport | C# | Direct2D client side interface between .NET and VC++ library, calling d2dlib.dll | d2dlibexport.dll |
| d2dlibexportwinform | C# | Provides the .NET classes like D2DWinForm and D2DControl that use Direct2D hardware-acceleration rendering | d2dlibwinform.dll |


# How to use

1. Compile every projects or use the DLLs from the binary folder directly
2. Add `d2dlibexport.dll` and `d2dlibwinform.dll` as your project references
3. Put `d2dlib.dll` in the `Debug`, `Release` or folder where your application runs
4. Make your windows form or control inherited from `D2DForm` or `D2DControl` class
5. Override `OnRender(D2DGraphics g)` method (do not override .NET `OnPaint` method)
6. Draw anything inside `OnRender` method via the `g` context

# Sample

## Draw ellipse

```csharp
protected override void OnRender(D2DGraphics g)
{
  var ellipse = new D2DEllipse(0, 0, 10, 10);
  g.DrawEllipse(ref ellipse, D2DColor.Gray);
}
```

## Draw text

```csharp
g.DrawText("Hello World", D2DColor.Yellow, this.Font, 100, 200);
```

# Snapshots

![Used for software 3D Engine rendering](http://necotech.org/wp-content/uploads/2017/07/d2dlib_s1.png)
