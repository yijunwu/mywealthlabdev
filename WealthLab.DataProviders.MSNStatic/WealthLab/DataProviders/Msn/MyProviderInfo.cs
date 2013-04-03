namespace WealthLab.DataProviders.Msn
{
    using System;
    using WealthLab.DataProviders.MarketManagerService;

    public class MyProviderInfo : MarketManagerInfo
    {
        public override string ProviderName()
        {
            return "MSN";
        }
    }
}

