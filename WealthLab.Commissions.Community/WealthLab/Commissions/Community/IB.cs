namespace WealthLab.Commissions.Community
{
    using Fidelity.Components;
    using System;
    using System.Windows.Forms;
    using WealthLab;

    internal class IB : Commission, ICustomSettings
    {
        private double _maxCommishPct;
        private double _minCommish;
        private double _perContract;
        private double _perShare;
        private uxIBBundled _settingsUI;

        public override double Calculate(TradeType tradeType, OrderType orderType, double orderPrice, double shares, Bars bars)
        {
            double num = this._minCommish;
            if (bars.SymbolInfo.SecurityType != SecurityType.Future)
            {
                double num2 = ((this._maxCommishPct / 100.0) * shares) * orderPrice;
                num = this._perShare * shares;
                if (num < this._minCommish)
                {
                    return this._minCommish;
                }
                if (num > num2)
                {
                    num = num2;
                }
                return num;
            }
            return (this._perContract * shares);
        }

        public void ChangeSettings(UserControl ui)
        {
            this._perShare = this._settingsUI.PerShare;
            this._perContract = this._settingsUI.PerContract;
            this._minCommish = this._settingsUI.MinCommish;
            this._maxCommishPct = this._settingsUI.MaxPct;
        }

        public UserControl GetSettingsUI()
        {
            if (this._settingsUI == null)
            {
                this._settingsUI = new uxIBBundled();
                this._settingsUI.PerShare = this._perShare;
                this._settingsUI.PerContract = this._perContract;
                this._settingsUI.MinCommish = this._minCommish;
                this._settingsUI.MaxPct = this._maxCommishPct;
            }
            return this._settingsUI;
        }

        public void ReadSettings(ISettingsHost host)
        {
            this._perShare = host.Get("uxIB.PerShare", (double) 0.005);
            this._perContract = host.Get("uxIB.PerContract", (double) 2.4);
            this._minCommish = host.Get("uxIB.MinCommission", (double) 1.0);
            this._maxCommishPct = host.Get("uxIB.MaxPercentCommission", (double) 0.5);
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("uxIB.PerShare", this._perShare);
            host.Set("uxIB.PerContract", this._perContract);
            host.Set("uxIB.MinCommission", this._minCommish);
            host.Set("uxIB.MaxPercentCommission", this._maxCommishPct);
        }

        public override string Description
        {
            get
            {
                string str = "For stocks you can adjust the minimum commission per order, per share charge, and maximum charge based on a percentage of the trade value, ";
                string str2 = "whereas for futures symbols you can set the per contract commission.";
                return ("IB Bundled Commissions. " + str + str2);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "IB Bundled";
            }
        }
    }
}

