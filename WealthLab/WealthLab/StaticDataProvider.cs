namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public abstract class StaticDataProvider : HistoricalProvider, IWizard
    {
        private bool bool_0;
        private IDataHost idataHost_0;
        private static List<DataBehaviorUserControl> list_0 = new List<DataBehaviorUserControl>();

        protected StaticDataProvider()
        {
        }

        public virtual void CheckConnectionWithServer()
        {
        }

        public abstract DataSource CreateDataSource();
        public virtual void DeleteSymbolDataFile(DataSource dataSource_0, string symbol)
        {
        }

        public virtual int GetChunkCount(DataSource dataSource_0, string symbol, DateTime startDate, DateTime endDate)
        {
            return 1;
        }

        public virtual MarketInfo GetMarketInfo(string symbol)
        {
            return this.DataHost.DefaultMarketInfo;
        }

        public virtual SecurityType GetSecurityType(string symbol)
        {
            return SecurityType.Equity;
        }

        public virtual double GetSessionOpen(string symbol, DataSource dataSource_0)
        {
            return 0.0;
        }

        public virtual void Initialize(IDataHost dataHost)
        {
            this.idataHost_0 = dataHost;
        }

        public virtual string ModifySymbols(DataSource dataSource_0, List<string> symbols)
        {
            return "";
        }

        public abstract void PopulateSymbols(DataSource dataSource_0, List<string> symbols);
        public abstract Bars RequestData(DataSource dataSource_0, string symbol, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar);
        public virtual void RequestUpdates(List<string> symbols, DateTime startDate, DateTime endDate, BarScale scale, int barInterval, IUpdateRequestCompleted requestCompleted)
        {
        }

        public virtual void SaveEditedSymbolDataFile(DataSource dataSource_0, Bars bars)
        {
        }

        public virtual void SaveSymbolSecurityType()
        {
        }

        public abstract bool SupportsDynamicUpdate(BarScale scale);
        public override string ToString()
        {
            return this.FriendlyName;
        }

        public virtual bool WarnUserAboutDelay(string symbol, BarScale scale, int barInterval)
        {
            return false;
        }

        public abstract UserControl WizardFirstPage();
        public abstract UserControl WizardNextPage(UserControl currentPage);
        public abstract UserControl WizardPreviousPage(UserControl currentPage);

        public virtual bool CanDeleteSymbolDataFile
        {
            get
            {
                return false;
            }
        }

        public virtual bool CanEditSymbolDataFile
        {
            get
            {
                return false;
            }
        }

        public virtual bool CanModifySymbols
        {
            get
            {
                return false;
            }
        }

        public virtual bool CanRequestUpdates
        {
            get
            {
                return false;
            }
        }

        public IDataHost DataHost
        {
            get
            {
                return this.idataHost_0;
            }
        }

        public virtual bool DataSourceNameReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual BarDataStore DataStore
        {
            get
            {
                return null;
            }
        }

        public virtual IList<DataBehaviorUserControl> ExtendedBehaviors
        {
            get
            {
                return list_0;
            }
        }

        public virtual bool InternalUseOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsStreamingRequest
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public virtual string SuggestedDataSourceName
        {
            get
            {
                return "";
            }
        }
    }
}

