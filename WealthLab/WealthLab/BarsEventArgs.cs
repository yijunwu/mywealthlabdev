namespace WealthLab
{
    using System;

    public class BarsEventArgs : EventArgs
    {
        private WealthLab.Bars bars;

        public BarsEventArgs(WealthLab.Bars bars)
        {
            this.bars = bars;
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars;
            }
        }
    }
}

