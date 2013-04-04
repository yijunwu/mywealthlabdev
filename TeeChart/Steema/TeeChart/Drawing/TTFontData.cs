namespace Steema.TeeChart.Drawing
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct TTFontData
    {
        public Rectangle FontBBox;
        public int FirstChar;
        public int LastChar;
        public int CapHeight;
        public int Ascent;
        public int Descent;
        public int MaxWidth;
        public int AvgWidth;
        public int ItalicAngle;
        public int[] Widths;
        public TTFontData(ChartFont f)
        {
            this.FontBBox = new Rectangle(0, 0, 0, 0);
            this.FirstChar = 0;
            this.LastChar = 0xff;
            this.Widths = new int[0x100];
            this.CapHeight = 0;
            this.Ascent = 0;
            this.Descent = 0;
            this.MaxWidth = 0;
            this.AvgWidth = 0;
            this.ItalicAngle = 0;
        }
    }
}

