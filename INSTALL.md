# Installation Notice

## For application uses .NET Core, .NET framework 4.5 and later

Make sure the "Prefer 32-bit" option is checked. You can find this option from 'Build' page of project setting.

## For application uses .NET framework 4 and early 

Change project's build configuration from 'Any CPU' to 'x86' architecture.

# Getting Started

- Make your windows form or control inherited from `D2DForm` or `D2DControl` class.
- Override `OnRender` method to draw anything on the form or control (Do not override `OnPaint` method)

Learn more at [GitHub](https://github.com/jingwood/d2dlib)