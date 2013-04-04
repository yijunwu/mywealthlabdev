namespace QWhale.Common
{
    using System;
    using System.Drawing;

    public class GdiFontInfos : FontInfos
    {
        public GdiFontInfos(Font font, IntPtr dc) : base(font, dc)
        {
        }

        public GdiFontInfos(IntPtr hFont, IntPtr dc) : base(hFont, dc)
        {
        }

        protected override FontInfo CreateFontInfo(Font font)
        {
            return new GdiFontInfo(font, base.dc);
        }

        protected override FontInfo CreateFontInfo(IntPtr hFont, string fontName)
        {
            return new GdiFontInfo(hFont, base.dc, fontName);
        }

        protected override void SelectFontInfo(FontInfo fontInfo)
        {
            Win32.SelectObject(base.dc, fontInfo.HFont);
        }
    }
}

