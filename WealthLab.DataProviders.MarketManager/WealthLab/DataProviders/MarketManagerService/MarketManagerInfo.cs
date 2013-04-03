namespace WealthLab.DataProviders.MarketManagerService
{
    using System;

    public abstract class MarketManagerInfo
    {
        protected MarketManagerInfo()
        {
        }

        public abstract string ProviderName();
    }
}

