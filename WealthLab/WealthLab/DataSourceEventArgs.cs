namespace WealthLab
{
    using System;

    public class DataSourceEventArgs : EventArgs
    {
        private WealthLab.DataSource dataSource;

        public DataSourceEventArgs(WealthLab.DataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        public WealthLab.DataSource DataSource
        {
            get
            {
                return this.dataSource;
            }
        }
    }
}

