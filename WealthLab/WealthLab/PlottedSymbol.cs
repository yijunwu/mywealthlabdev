namespace WealthLab
{
    using System;
    using System.Drawing;

    public class PlottedSymbol
    {
        private WealthLab.Bars bars;
        private Color upColor;
        private Color downColor;

        public PlottedSymbol(WealthLab.Bars bars, Color upColor, Color downColor)
        {
            this.bars = bars;
            this.upColor = upColor;
            this.downColor = downColor;
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars;
            }
        }

        public Color DownColor
        {
            get
            {
                return this.downColor;
            }
        }

        public Color UpColor
        {
            get
            {
                return this.upColor;
            }
        }
    }
}

