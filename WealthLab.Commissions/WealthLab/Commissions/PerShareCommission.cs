namespace WealthLab.Commissions
{
    using Fidelity.Components;
    using System;
    using System.Windows.Forms;
    using WealthLab;

    public class PerShareCommission : Commission, ICustomSettings
    {
        private double double_0 = 0.02;

        public override double Calculate(TradeType tradeType, OrderType orderType, double orderPrice, double shares, Bars bars)
        {
            return (this.double_0 * shares);
        }

        public void ChangeSettings(UserControl userControl_0)
        {
            PerShareCommissionSettings settings = userControl_0 as PerShareCommissionSettings;
            this.double_0 = settings.Value;
        }

        public UserControl GetSettingsUI()
        {
            return new PerShareCommissionSettings { Value = this.double_0 };
        }

        public void ReadSettings(ISettingsHost host)
        {
            this.double_0 = host.Get("PerShareCommission", (double) 0.02);
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("PerShareCommission", this.double_0);
        }

        public override string Description
        {
            get
            {
                return "Allows you to specify an amount of commission to apply per share traded.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Per Share Commission";
            }
        }
    }
}

