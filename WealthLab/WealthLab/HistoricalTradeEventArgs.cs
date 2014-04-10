namespace WealthLab
{
    using System;

    public class HistoricalTradeEventArgs : EventArgs
    {
        private WealthLab.HistoricalTrade historicalTrade;

        public HistoricalTradeEventArgs(WealthLab.HistoricalTrade historicalTrade_1)
        {
            this.historicalTrade = historicalTrade_1;
        }

        public WealthLab.HistoricalTrade HistoricalTrade
        {
            get
            {
                return this.historicalTrade;
            }
        }
    }
}

