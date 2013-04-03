namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public abstract class HistoricalProvider
    {
        protected HistoricalProvider()
        {
        }

        public virtual void CancelUpdate()
        {
        }

        public virtual void UpdateDataSource(DataSource dataSource_0, IDataUpdateMessage dataUpdateMsg)
        {
        }

        public virtual void UpdateProvider(IDataUpdateMessage dataUpdateMsg, List<DataSource> dataSources, bool updateNonDSSymbols, bool deleteNonDSSymbols)
        {
        }

        public abstract string Description { get; }

        public abstract string FriendlyName { get; }

        public abstract Bitmap Glyph { get; }

        public virtual bool SupportsDataSourceUpdate
        {
            get
            {
                return false;
            }
        }

        public virtual bool SupportsProviderUpdate
        {
            get
            {
                return false;
            }
        }

        public virtual string URL
        {
            get
            {
                return "";
            }
        }
    }
}

