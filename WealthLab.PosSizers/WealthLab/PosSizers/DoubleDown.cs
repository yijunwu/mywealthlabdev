namespace WealthLab.PosSizers
{
    using Fidelity.Components;
    using System;
    using System.Windows.Forms;
    using WealthLab;

    public class DoubleDown : BasicPosSizer, ICustomSettings
    {
        private DoubleDownSettings doubleDownSettings_0 = new DoubleDownSettings();

        public void ChangeSettings(UserControl userControl_0)
        {
            base.ChangeBasicSettings(this.doubleDownSettings_0);
        }

        public UserControl GetSettingsUI()
        {
            base.InitializeSettings(this.doubleDownSettings_0);
            return this.doubleDownSettings_0;
        }

        public void ReadSettings(ISettingsHost host)
        {
            base.ReadBasicSettings(host);
        }

        public override double SizePosition(Position currentPos, Bars bars, int int_0, double basisPrice, PositionType positionType_0, double riskStopLevel, double equity, double thisBarCash)
        {
            double posSizeValue = base.GetPosSizeValue(base.PosSizeMode, base.FixedDollarSize, base.PctEquitySize, base.MaxRiskSize);
            foreach (Position position in base.ActivePositions)
            {
                if (position.Bars.Symbol == bars.Symbol)
                {
                    posSizeValue *= 2.0;
                }
            }
            if ((base.PosSizeMode != PosSizeMode.Dollar) && (posSizeValue > 100.0))
            {
                posSizeValue = 100.0;
            }
            return base.CalcPositionSize(base.PosSizeMode, posSizeValue, bars, int_0, positionType_0, basisPrice, riskStopLevel, equity);
        }

        public void WriteSettings(ISettingsHost host)
        {
            base.WriteBasicSettings(host);
        }

        public override string FriendlyName
        {
            get
            {
                return "Double Down";
            }
        }
    }
}

