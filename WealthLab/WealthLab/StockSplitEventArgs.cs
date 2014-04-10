namespace WealthLab
{
    using System;

    public class StockSplitEventArgs : EventArgs
    {
        private DateTime exDate;
        private double splitFactor;
        private StaticDataProvider staticDataProvider;
        private string symbol;

        public StockSplitEventArgs(StaticDataProvider provider, string symbol, double splitFactor, DateTime exDate)
        {
            this.symbol = symbol;
            this.splitFactor = splitFactor;
            this.exDate = exDate;
            this.staticDataProvider = provider;
        }

        public DateTime ExDate
        {
            get
            {
                return this.exDate;
            }
        }

        public StaticDataProvider Provider
        {
            get
            {
                return this.staticDataProvider;
            }
        }

        public double SplitFactor
        {
            get
            {
                return this.splitFactor;
            }
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
        }
    }
}

