namespace WealthLab
{
    using System;

    public class DataSourceSymbolEventArgs : EventArgs
    {
        private WealthLab.DataSource dataSource;
        private string symbol;

        public DataSourceSymbolEventArgs(WealthLab.DataSource dataSource, string symbol)
        {
            this.dataSource = dataSource;
            this.symbol = symbol;
        }

        public WealthLab.DataSource DataSource
        {
            get
            {
                return this.dataSource;
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

