namespace WealthLab
{
    using System;

    public class HistoricalTradeEventArgs : EventArgs
    {
        private WealthLab.HistoricalTrade historicalTrade_0;

        public HistoricalTradeEventArgs(WealthLab.HistoricalTrade historicalTrade_1)
        {
            this.historicalTrade_0 = historicalTrade_1;
        }

        public WealthLab.HistoricalTrade HistoricalTrade
        {
            get
            {
                return this.historicalTrade_0;
            }
        }
    }
}

