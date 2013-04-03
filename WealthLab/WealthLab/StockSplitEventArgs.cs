namespace WealthLab
{
    using System;

    public class StockSplitEventArgs : EventArgs
    {
        private DateTime dateTime_0;
        private double double_0;
        private StaticDataProvider staticDataProvider_0;
        private string string_0;

        public StockSplitEventArgs(StaticDataProvider provider, string symbol, double splitFactor, DateTime exDate)
        {
            this.string_0 = symbol;
            this.double_0 = splitFactor;
            this.dateTime_0 = exDate;
            this.staticDataProvider_0 = provider;
        }

        public DateTime ExDate
        {
            get
            {
                return this.dateTime_0;
            }
        }

        public StaticDataProvider Provider
        {
            get
            {
                return this.staticDataProvider_0;
            }
        }

        public double SplitFactor
        {
            get
            {
                return this.double_0;
            }
        }

        public string Symbol
        {
            get
            {
                return this.string_0;
            }
        }
    }
}

