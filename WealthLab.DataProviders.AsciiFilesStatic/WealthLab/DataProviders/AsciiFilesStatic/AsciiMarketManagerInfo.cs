namespace WealthLab.DataProviders.AsciiFilesStatic
{
    using System;
    using WealthLab.DataProviders.MarketManagerService;

    public class AsciiMarketManagerInfo : MarketManagerInfo
    {
        public override string ProviderName()
        {
            return AsciiFilesStaticProvider.ProviderName;
        }
    }
}

