using System;
using System.Collections.Generic;
using System.Text;


namespace unvell.D2DLib
{
    public class D2DTextFormat : D2DObject
    {
        internal D2DTextFormat(HANDLE handle) : base(handle) { }

        /// <summary>Create a device-independent text format: it needs no <see cref="D2DDevice"/>, and
        /// layouts made from it can be measured without one and drawn on any.</summary>
        public static D2DTextFormat Create(string fontFamily, float fontSize,
            D2DFontWeight fontWeight = D2DFontWeight.Normal,
            D2DFontStyle fontStyle = D2DFontStyle.Normal,
            D2DFontStretch fontStretch = D2DFontStretch.Normal,
            string? locale = null)
        {
            return new D2DTextFormat(D2D.CreateTextFormatEx(fontFamily, fontSize, fontWeight, fontStyle, fontStretch, locale));
        }
    }

    public class D2DTextLayout : D2DObject
    {
        internal D2DTextLayout(HANDLE handle) : base(handle) { }

        /// <summary>Lay <paramref name="text"/> out in <paramref name="textFormat"/> inside a
        /// <paramref name="maxWidth"/>×<paramref name="maxHeight"/> box. The layout is device-independent;
        /// keep it to redraw the same text cheaply (laying out is what text costs).</summary>
        public static D2DTextLayout Create(string text, D2DTextFormat textFormat, float maxWidth, float maxHeight,
            DWriteTextAlignment halign = DWriteTextAlignment.Leading,
            DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near,
            D2DTextLayoutOptions options = D2DTextLayoutOptions.None)
        {
            return new D2DTextLayout(D2D.CreateTextLayoutEx(text, (uint)text.Length, textFormat.Handle,
                maxWidth, maxHeight, halign, valign, options));
        }

        public D2DTextLayoutMetrics Metrics
        {
            get
            {
                D2D.GetTextLayoutMetrics(this.Handle, out var metrics);
                return metrics;
            }
        }

        public void SetMaxSize(float maxWidth, float maxHeight) => D2D.SetTextLayoutMaxSize(this.Handle, maxWidth, maxHeight);

        public void SetFontFamily(int start, int length, string fontFamily)
            => D2D.SetTextLayoutFontFamily(this.Handle, (uint)start, (uint)length, fontFamily);

        public void SetFontSize(int start, int length, float fontSize)
            => D2D.SetTextLayoutFontSize(this.Handle, (uint)start, (uint)length, fontSize);

        public void SetFontWeight(int start, int length, D2DFontWeight fontWeight)
            => D2D.SetTextLayoutFontWeight(this.Handle, (uint)start, (uint)length, fontWeight);

        public void SetFontStyle(int start, int length, D2DFontStyle fontStyle)
            => D2D.SetTextLayoutFontStyle(this.Handle, (uint)start, (uint)length, fontStyle);

        public void SetUnderline(int start, int length, bool underline)
            => D2D.SetTextLayoutUnderline(this.Handle, (uint)start, (uint)length, underline ? 1 : 0);

        public void SetStrikethrough(int start, int length, bool strikethrough)
            => D2D.SetTextLayoutStrikethrough(this.Handle, (uint)start, (uint)length, strikethrough ? 1 : 0);
    }

    public static class D2DFonts
    {
        /// <summary>Whether the installed <paramref name="fontFamily"/> has a glyph for every character of
        /// <paramref name="chars"/>: null when no family of that name is installed.</summary>
        public static bool? HasCharacters(string fontFamily, string chars)
        {
            int result = D2D.FontFamilyHasCharacters(fontFamily, chars, (uint)chars.Length);
            return result < 0 ? null : result > 0;
        }
    }
}
