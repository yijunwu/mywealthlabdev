namespace WealthLab
{
    using System;

    public class DataSourceSymbolEventArgs : EventArgs
    {
        private WealthLab.DataSource dataSource_0;
        private string string_0;

        public DataSourceSymbolEventArgs(WealthLab.DataSource dataSource, string symbol)
        {
            this.dataSource_0 = dataSource;
            this.string_0 = symbol;
        }

        public WealthLab.DataSource DataSource
        {
            get
            {
                return this.dataSource_0;
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

