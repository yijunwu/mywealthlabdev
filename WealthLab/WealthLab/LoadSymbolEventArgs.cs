namespace WealthLab
{
    using System;

    public class LoadSymbolEventArgs : EventArgs
    {
        private Bars bars_0;
        private BarScale barScale_0;
        private int int_0;
        private string string_0;

        public LoadSymbolEventArgs(string symbol, BarScale scale, int barInterval)
        {
            this.string_0 = symbol;
            this.barScale_0 = scale;
            this.int_0 = barInterval;
        }

        public int BarInterval
        {
            get
            {
                return this.int_0;
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale_0;
            }
        }

        public string Symbol
        {
            get
            {
                return this.string_0;
            }
        }

        public Bars SymbolData
        {
            get
            {
                return this.bars_0;
            }
            set
            {
                this.bars_0 = value;
            }
        }
    }
}

