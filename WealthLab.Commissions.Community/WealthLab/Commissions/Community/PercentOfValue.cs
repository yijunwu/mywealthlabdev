namespace WealthLab.Commissions.Community
{
    using Fidelity.Components;
    using System;
    using System.Windows.Forms;
    using WealthLab;

    public class PercentOfValue : Commission, ICustomSettings
    {
        private double _percent = 0.1;
        private uxDollarsPer _settingsUI;
        private string _uxLabel = "Percentage commission to apply to each trade's 'dollar' value. (e.g. 0.1 = 0.1%)";

        public override double Calculate(TradeType tradeType, OrderType orderType, double orderPrice, double shares, Bars bars)
        {
            return (((this._percent / 100.0) * orderPrice) * shares);
        }

        public void ChangeSettings(UserControl ui)
        {
            this._percent = this._settingsUI.Commission;
        }

        public UserControl GetSettingsUI()
        {
            if (this._settingsUI == null)
            {
                this._settingsUI = new uxDollarsPer();
                this._settingsUI.Label = this._uxLabel;
                this._settingsUI.Commission = this._percent;
            }
            return this._settingsUI;
        }

        public void ReadSettings(ISettingsHost host)
        {
            this._percent = host.Get("uxDollarsPer.Percent", (double) 0.01);
            this._uxLabel = "Percentage commission to apply to each trade's 'dollar' value. (e.g. 0.1 = 0.1%)";
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("uxDollarsPer.Percent", this._percent);
        }

        public override string Description
        {
            get
            {
                return (this._percent.ToString("$0.00######") + " % of Trade Value");
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Percent of Trade Value";
            }
        }
    }
}

