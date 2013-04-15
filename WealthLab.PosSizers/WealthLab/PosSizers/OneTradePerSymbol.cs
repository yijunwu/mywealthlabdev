namespace WealthLab.PosSizers
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using WealthLab;

    public class OneTradePerSymbol : BasicPosSizer, ICustomSettings
    {
        private PosSizerSettingsBase posSizerSettingsBase_0 = new PosSizerSettingsBase("This PosSizer allows only one trade per symbol at any given time.");

        public void ChangeSettings(UserControl userControl_0)
        {
            base.ChangeBasicSettings(this.posSizerSettingsBase_0);
        }

        public UserControl GetSettingsUI()
        {
            if (this.posSizerSettingsBase_0 == null)
            {
                this.posSizerSettingsBase_0 = new MaxEntriesPerDaySettings();
            }
            base.InitializeSettings(this.posSizerSettingsBase_0);
            return this.posSizerSettingsBase_0;
        }

        public void ReadSettings(ISettingsHost host)
        {
            base.ReadBasicSettings(host);
        }

        public override double SizePosition(Position currentPos, Bars bars, int int_0, double basisPrice, PositionType positionType_0, double riskStopLevel, double equity, double cash)
        {
            using (List<Position>.Enumerator enumerator = base.ActivePositions.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Position current = enumerator.Current;
                    if (current.Symbol == bars.Symbol)
                    {
                        ///goto  Label_0035; ///WYJ fix, simplify the flow
                        return 0.0;
                    }
                }
                return base.CalcBasicPositionSize(bars, int_0, positionType_0, basisPrice, riskStopLevel, equity);
            }
        }

        public void WriteSettings(ISettingsHost host)
        {
            base.WriteBasicSettings(host);
        }

        public override string FriendlyName
        {
            get
            {
                return "One Trade per Symbol";
            }
        }
    }
}

