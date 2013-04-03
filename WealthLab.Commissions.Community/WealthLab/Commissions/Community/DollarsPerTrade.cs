namespace WealthLab.Commissions.Community
{
    using Fidelity.Components;
    using System;
    using System.Windows.Forms;
    using WealthLab;

    public class DollarsPerTrade : Commission, ICustomSettings
    {
        private double _dollars = 2.5;
        private uxDollarsPer _settingsUI;
        private string _uxLabel = "Amount of commission to apply per trade:";

        public override double Calculate(TradeType tradeType, OrderType orderType, double orderPrice, double shares, Bars bars)
        {
            return this._dollars;
        }

        public void ChangeSettings(UserControl ui)
        {
            this._dollars = this._settingsUI.Commission;
        }

        public UserControl GetSettingsUI()
        {
            if (this._settingsUI == null)
            {
                this._settingsUI = new uxDollarsPer();
                this._settingsUI.Label = this._uxLabel;
                this._settingsUI.Commission = this._dollars;
            }
            return this._settingsUI;
        }

        public void ReadSettings(ISettingsHost host)
        {
            this._dollars = host.Get("uxDollarsPer.Trade", (double) 8.0);
            this._uxLabel = "Amount of commission to apply per trade:";
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("uxDollarsPer.Trade", this._dollars);
        }

        public override string Description
        {
            get
            {
                return (this._dollars.ToString("$0.00######") + " Per Trade");
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Per Trade";
            }
        }
    }
}

