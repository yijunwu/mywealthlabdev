namespace WealthLab.PosSizers
{
    using Fidelity.Components;
    using System;
    using System.Windows.Forms;
    using WealthLab;

    public class AvgDownPctEquityLimit : BasicPosSizer, ICustomSettings
    {
        private AvgDownPctEquityLimitSettings avgDownPctEquityLimitSettings_0 = new AvgDownPctEquityLimitSettings();
        private double double_3 = 10.0;

        public override void ApplyConfigString(string config)
        {
            base.ApplyConfigString(config);
            string[] strArray = config.Split(new char[] { '|' });
            this.double_3 = double.Parse(strArray[4]);
        }

        public void ChangeSettings(UserControl userControl_0)
        {
            base.ChangeBasicSettings(this.avgDownPctEquityLimitSettings_0);
            this.double_3 = this.avgDownPctEquityLimitSettings_0.MaxPositionSize;
        }

        public override string GetConfigString()
        {
            return (base.GetConfigString() + this.double_3 + "|");
        }

        public UserControl GetSettingsUI()
        {
            base.InitializeSettings(this.avgDownPctEquityLimitSettings_0);
            this.avgDownPctEquityLimitSettings_0.MaxPositionSize = this.double_3;
            return this.avgDownPctEquityLimitSettings_0;
        }

        public void ReadSettings(ISettingsHost host)
        {
            base.ReadBasicSettings(host);
            this.double_3 = host.Get("AvgDownPctEquityLimit.MaxPctEquity", (double) 10.0);
        }

        public override double SizePosition(Position currentPos, Bars bars, int int_0, double basisPrice, PositionType positionType_0, double riskStopLevel, double equity, double cash)
        {
            double num = 0.0;
            string symbol = bars.Symbol;
            foreach (Position position in base.ActivePositions)
            {
                if (position.Symbol == symbol)
                {
                    double num9 = position.Shares * bars.Close[int_0];
                    num += num9;
                }
            }
            double num5 = (num * 100.0) / equity;
            if (num5 > this.double_3)
            {
                return 0.0;
            }
            double num2 = base.CalcPositionSize(base.PosSizeMode, base.GetPosSizeValue(base.PosSizeMode, base.FixedDollarSize, base.PctEquitySize, base.MaxRiskSize), bars, int_0, positionType_0, basisPrice, riskStopLevel, equity);
            double num3 = num2 * basisPrice;
            double num4 = (num3 / equity) * 100.0;
            double num6 = num5 + num4;
            if (num6 > this.double_3)
            {
                double num7 = this.double_3 - num5;
                double num8 = (num7 / 100.0) * equity;
                return (num8 / basisPrice);
            }
            return num2;
        }

        public void WriteSettings(ISettingsHost host)
        {
            base.WriteBasicSettings(host);
            host.Set("AvgDownPctEquityLimit.MaxPctEquity", this.double_3);
        }

        public override string FriendlyName
        {
            get
            {
                return "Avg Down with Pct Equity Limit";
            }
        }
    }
}

