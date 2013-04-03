namespace WealthLab.Commissions.Community
{
    using Fidelity.Components;
    using System;
    using System.Windows.Forms;
    using WealthLab;

    public class DollarsPerShare : Commission, ICustomSettings
    {
        private double _dollars = 0.01;
        private uxDollarsPer _settingsUI;
        private string _uxLabel = "Amount of commission to apply per share:";

        public override double Calculate(TradeType tradeType, OrderType orderType, double orderPrice, double shares, Bars bars)
        {
            return (this._dollars * shares);
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
            this._dollars = host.Get("uxDollarsPer.Share", (double) 0.01);
            this._uxLabel = "Amount of commission to apply per share:";
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("uxDollarsPer.Share", this._dollars);
        }

        public override string Description
        {
            get
            {
                return (this._dollars.ToString("$0.00######") + " Per Share");
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Per Share";
            }
        }
    }
}

