namespace WealthLab.Commissions
{
    using Fidelity.Components;
    using System;
    using System.Windows.Forms;
    using WealthLab;

    public class FidelityFlatRate : Commission, ICustomSettings
    {
        private double double_0 = 7.95;

        public override double Calculate(TradeType tradeType, OrderType orderType, double orderPrice, double shares, Bars bars)
        {
            return this.double_0;
        }

        public void ChangeSettings(UserControl userControl_0)
        {
            FidelityFlatRateSettings settings = userControl_0 as FidelityFlatRateSettings;
            this.double_0 = settings.Value;
        }

        public UserControl GetSettingsUI()
        {
            return new FidelityFlatRateSettings { Value = this.double_0 };
        }

        public void ReadSettings(ISettingsHost host)
        {
            this.double_0 = host.Get("FidelityFlatRate", (double) 7.95);
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("FidelityFlatRate", this.double_0);
        }

        public override string Description
        {
            get
            {
                return "All online stock trades for $7.95 per trade for an unlimited number of shares regardless of price per share.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Fidelity";
            }
        }
    }
}

