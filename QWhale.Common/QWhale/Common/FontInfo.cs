namespace QWhale.Common
{
    using System;
    using System.Drawing;

    public class FontInfo
    {
        protected IntPtr dc;
        private System.Drawing.Font font;
        private Win32.TextMetrics fontMetrics;
        private string fontName;
        private IntPtr hFont;
        private bool isMonoSpaced;

        public FontInfo(System.Drawing.Font font, IntPtr dc) : this(font.ToHfont(), dc, font.Name)
        {
            this.font = font;
        }

        public FontInfo(IntPtr hFont, IntPtr dc, string fontName)
        {
            this.hFont = hFont;
            this.dc = dc;
            this.fontName = fontName;
            Win32.SelectObject(dc, hFont);
            Win32.GetTextMetrics(dc, ref this.fontMetrics);
            this.isMonoSpaced = (this.fontMetrics.PitchAndFamily & 1) == 0;
        }

        ~FontInfo()
        {
            if (this.hFont != IntPtr.Zero)
            {
                Win32.DeleteObject(this.hFont);
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this.font;
            }
        }

        public int FontHeight
        {
            get
            {
                return this.fontMetrics.Height;
            }
        }

        public string FontName
        {
            get
            {
                return this.fontName;
            }
        }

        public int FontWidth
        {
            get
            {
                return this.fontMetrics.AveCharWidth;
            }
        }

        public IntPtr HFont
        {
            get
            {
                return this.hFont;
            }
        }

        public bool IsMonoSpaced
        {
            get
            {
                return this.isMonoSpaced;
            }
        }
    }
}

