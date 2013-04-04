namespace QWhale.Common
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class GdiFontInfo : FontInfo
    {
        private bool asciInitialized;
        private int[] asciTable;
        private Hashtable chartable;
        private bool useDrawText;

        public GdiFontInfo(Font font, IntPtr dc) : base(font, dc)
        {
            this.asciTable = new int[0x100];
            if (!this.IsFontMonoSpaced())
            {
                this.InitChartable();
            }
        }

        public GdiFontInfo(IntPtr hFont, IntPtr dc, string fontName) : base(hFont, dc, fontName)
        {
            this.asciTable = new int[0x100];
            if (!this.IsFontMonoSpaced())
            {
                this.InitChartable();
            }
        }

        public int CharWidth(char ch)
        {
            if (this.IsFontMonoSpaced())
            {
                return base.FontWidth;
            }
            if ((ch <= '\x00ff') && this.asciInitialized)
            {
                return this.asciTable[ch];
            }
            object charWidth = this.chartable[ch];
            if (charWidth == null)
            {
                charWidth = this.GetCharWidth(ch);
                this.chartable.Add(ch, charWidth);
            }
            return (int) charWidth;
        }

        protected int GetCharWidth(char ch)
        {
            Win32.SelectObject(base.dc, base.HFont);
            if (this.useDrawText)
            {
                Rectangle empty = Rectangle.Empty;
                Win32.DrawText(base.dc, ch.ToString(), 1, ref empty, 0x400);
                return empty.Width;
            }
            Size size = new Size(0, 0);
            Win32.GetTextExtentPoint32(base.dc, ch.ToString(), 1, ref size);
            return size.Width;
        }

        protected void InitChartable()
        {
            this.chartable = new Hashtable();
            this.asciInitialized = Win32.GetCharABCWidths(base.dc, 0, 0xff, ref this.asciTable);
        }

        protected virtual bool IsFontMonoSpaced()
        {
            return false;
        }

        protected virtual void OnUseDrawTextChanged()
        {
        }

        public bool UseDrawText
        {
            get
            {
                return this.useDrawText;
            }
            set
            {
                if (this.useDrawText != value)
                {
                    this.useDrawText = value;
                    this.OnUseDrawTextChanged();
                }
            }
        }
    }
}

