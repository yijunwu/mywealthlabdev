namespace WealthLab
{
    using Fidelity.Components;
    using System;

    public class DecimalsManager
    {
        private static DecimalsManager decimalsManager_0 = new DecimalsManager();
        private int int_0 = 2;
        private int int_1 = 4;

        private DecimalsManager()
        {
        }

        public int GetPricingDecimalForSymbol(string symbol)
        {
            SymbolInfo symbolInfo = BarsLoader.GetSymbolInfo(symbol);
            if (symbolInfo == null)
            {
                return this.Pricing;
            }
            return symbolInfo.Decimals;
        }

        public void SetValues(SettingsManager settings)
        {
            if (settings != null)
            {
                this.int_0 = settings.Get(this.PricingKey, this.int_0);
                this.int_1 = settings.Get(this.IndicatorKey, this.int_1);
            }
        }

        public int Indicator
        {
            get
            {
                return this.int_1;
            }
        }

        public string IndicatorKey
        {
            get
            {
                return "IndicatorDecimalPlaces";
            }
        }

        public static DecimalsManager Instance
        {
            get
            {
                return decimalsManager_0;
            }
        }

        public int Pricing
        {
            get
            {
                return this.int_0;
            }
        }

        public string PricingKey
        {
            get
            {
                return "PricingDecimalPlaces";
            }
        }
    }
}

