namespace WealthLab.PosSizers
{
    using Fidelity.Components;
    using System;
    using System.Windows.Forms;
    using WealthLab;

    public class PctWinnersPosSizing : PosSizer, ICustomSettings
    {
        private double double_0 = 5000.0;
        private double double_1 = 20000.0;
        private double double_2 = 75.0;
        private PctWinnersPosSizingSettings pctWinnersPosSizingSettings_0 = new PctWinnersPosSizingSettings();

        public override void ApplyConfigString(string config)
        {
            string[] strArray = config.Split(new char[] { '|' });
            this.double_0 = double.Parse(strArray[0]);
            this.double_1 = double.Parse(strArray[1]);
            this.double_2 = double.Parse(strArray[2]);
        }

        public void ChangeSettings(UserControl userControl_0)
        {
            if (((this.double_0 > 0.0) && (this.double_1 > 0.0)) && ((this.double_1 >= this.double_0) && (this.double_2 <= 100.0)))
            {
                this.double_0 = this.pctWinnersPosSizingSettings_0.MinSize;
                this.double_1 = this.pctWinnersPosSizingSettings_0.MaxSize;
                this.double_2 = this.pctWinnersPosSizingSettings_0.MaxTrigger;
            }
        }

        public override string GetConfigString()
        {
            return string.Concat(new object[] { this.double_0.ToString(), "|", this.double_1, "|", this.double_2, "|" });
        }

        public UserControl GetSettingsUI()
        {
            this.pctWinnersPosSizingSettings_0.MinSize = this.double_0;
            this.pctWinnersPosSizingSettings_0.MaxSize = this.double_1;
            this.pctWinnersPosSizingSettings_0.MaxTrigger = this.double_2;
            return this.pctWinnersPosSizingSettings_0;
        }

        public void ReadSettings(ISettingsHost host)
        {
            string str = "PctWinnersPosSize.";
            this.double_0 = host.Get(str + "MinSize", (double) 5000.0);
            this.double_1 = host.Get(str + "MaxSize", (double) 20000.0);
            this.double_2 = host.Get(str + "MaxTrigger", (double) 75.0);
        }

        public override double SizePosition(Position currentPos, Bars bars, int int_0, double basisPrice, PositionType positionType_0, double riskStopLevel, double equity, double cash)
        {
            if (base.ClosedPositions.Count <= 0)
            {
                return (this.double_0 / basisPrice);
            }
            int num = 0;
            foreach (Position position in base.ClosedPositions)
            {
                if (position.NetProfit > 0.0)
                {
                    num++;
                }
            }
            double num2 = (num * 100.0) / ((double) base.ClosedPositions.Count);
            if (num2 >= this.double_2)
            {
                return (this.double_1 / basisPrice);
            }
            double num3 = this.double_1 * (num2 / 100.0);
            if (num3 < this.double_0)
            {
                num3 = this.double_0;
            }
            return (num3 / basisPrice);
        }

        public void WriteSettings(ISettingsHost host)
        {
            string str = "PctWinnersPosSize.";
            host.Set(str + "MinSize", this.double_0);
            host.Set(str + "MaxSize", this.double_1);
            host.Set(str + "MaxTrigger", this.double_2);
        }

        public override string FriendlyName
        {
            get
            {
                return "Pct Winners Position Sizing";
            }
        }
    }
}

