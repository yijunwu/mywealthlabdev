namespace WealthLab
{
    using System;

    public class LoadSymbolEventArgs : EventArgs
    {
        private Bars symbolData;
        private BarScale barScale;
        private int barInterval;
        private string symbol;

        public LoadSymbolEventArgs(string symbol, BarScale scale, int barInterval)
        {
            this.symbol = symbol;
            this.barScale = scale;
            this.barInterval = barInterval;
        }

        public int BarInterval
        {
            get
            {
                return this.barInterval;
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale;
            }
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
        }

        public Bars SymbolData
        {
            get
            {
                return this.symbolData;
            }
            set
            {
                this.symbolData = value;
            }
        }
    }
}

