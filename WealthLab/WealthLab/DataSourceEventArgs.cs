namespace WealthLab
{
    using System;

    public class DataSourceEventArgs : EventArgs
    {
        private WealthLab.DataSource dataSource_0;

        public DataSourceEventArgs(WealthLab.DataSource dataSource)
        {
            this.dataSource_0 = dataSource;
        }

        public WealthLab.DataSource DataSource
        {
            get
            {
                return this.dataSource_0;
            }
        }
    }
}

