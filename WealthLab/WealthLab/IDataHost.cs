namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;

    public interface IDataHost
    {
        void AdjustForStockSplit(StaticDataProvider staticDataProvider_0, string symbol, double splitFactor, DateTime exDate);
        void DeleteSymbolFromDataSets(string symbol, StaticDataProvider staticDataProvider_0);

        AuthenticationProvider AuthProvider { get; }

        string BaseDataFolder { get; }

        IList<DataSource> DataSources { get; }

        MarketInfo DefaultMarketInfo { get; }

        bool OnDemandUpdateEnabled { get; }

        ISettingsHost SettingsHost { get; }
    }
}

