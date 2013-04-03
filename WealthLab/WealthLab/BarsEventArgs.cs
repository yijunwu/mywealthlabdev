namespace WealthLab
{
    using System;

    public class BarsEventArgs : EventArgs
    {
        private WealthLab.Bars bars_0;

        public BarsEventArgs(WealthLab.Bars bars)
        {
            this.bars_0 = bars;
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars_0;
            }
        }
    }
}

