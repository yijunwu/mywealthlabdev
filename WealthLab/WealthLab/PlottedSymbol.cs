namespace WealthLab
{
    using System;
    using System.Drawing;

    public class PlottedSymbol
    {
        private WealthLab.Bars bars_0;
        private Color color_0;
        private Color color_1;

        public PlottedSymbol(WealthLab.Bars bars, Color upColor, Color downColor)
        {
            this.bars_0 = bars;
            this.color_0 = upColor;
            this.color_1 = downColor;
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars_0;
            }
        }

        public Color DownColor
        {
            get
            {
                return this.color_1;
            }
        }

        public Color UpColor
        {
            get
            {
                return this.color_0;
            }
        }
    }
}

