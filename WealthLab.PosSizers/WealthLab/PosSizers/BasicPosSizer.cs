namespace WealthLab.PosSizers
{
    using Fidelity.Components;
    using System;
    using WealthLab;

    public abstract class BasicPosSizer : PosSizer
    {
        private double double_0 = 5000.0;
        private double double_1 = 5.0;
        private double double_2 = 2.0;
        private WealthLab.PosSizeMode posSizeMode_0 = WealthLab.PosSizeMode.Dollar;

        protected BasicPosSizer()
        {
        }

        public override void ApplyConfigString(string config)
        {
            string[] strArray = config.Split(new char[] { '|' });
            this.posSizeMode_0 = (WealthLab.PosSizeMode) Enum.Parse(typeof(WealthLab.PosSizeMode), strArray[0]);
            this.double_0 = double.Parse(strArray[1]);
            this.double_1 = double.Parse(strArray[2]);
            this.double_2 = double.Parse(strArray[3]);
        }

        public double CalcBasicPositionSize(Bars bars, int int_0, PositionType positionType_0, double costBasis, double riskStop, double equity)
        {
            return base.CalcPositionSize(this.PosSizeMode, base.GetPosSizeValue(this.PosSizeMode, this.FixedDollarSize, this.PctEquitySize, this.MaxRiskSize), bars, int_0, positionType_0, costBasis, riskStop, equity);
        }

        public void ChangeBasicSettings(PosSizerSettingsBase posSizerSettingsBase_0)
        {
            this.PosSizeMode = posSizerSettingsBase_0.PosSizeMode;
            if (posSizerSettingsBase_0.FixedDollarSize > 0.0)
            {
                this.FixedDollarSize = posSizerSettingsBase_0.FixedDollarSize;
            }
            if (posSizerSettingsBase_0.PctEquitySize > 0.0)
            {
                this.PctEquitySize = posSizerSettingsBase_0.PctEquitySize;
            }
            if (posSizerSettingsBase_0.MaxRiskSize > 0.0)
            {
                this.MaxRiskSize = posSizerSettingsBase_0.MaxRiskSize;
            }
        }

        public override string GetConfigString()
        {
            return string.Concat(new object[] { this.posSizeMode_0.ToString(), "|", this.double_0, "|", this.double_1, "|", this.double_2, "|" });
        }

        public void InitializeSettings(PosSizerSettingsBase posSizerSettingsBase_0)
        {
            posSizerSettingsBase_0.PosSizeMode = this.PosSizeMode;
            posSizerSettingsBase_0.FixedDollarSize = this.FixedDollarSize;
            posSizerSettingsBase_0.PctEquitySize = this.PctEquitySize;
            posSizerSettingsBase_0.MaxRiskSize = this.MaxRiskSize;
        }

        public void ReadBasicSettings(ISettingsHost host)
        {
            string str = base.GetType().Name + ".";
            this.PosSizeMode = (WealthLab.PosSizeMode) Enum.Parse(typeof(WealthLab.PosSizeMode), host.Get(str + "PosSizeMode", "Dollar"));
            this.FixedDollarSize = host.Get(str + "FixedDollarSize", (double) 5000.0);
            this.PctEquitySize = host.Get(str + "PctEquitySize", (double) 5.0);
            this.MaxRiskSize = host.Get(str + "MaxRiskSize", (double) 2.0);
        }

        public void WriteBasicSettings(ISettingsHost host)
        {
            string str = base.GetType().Name + ".";
            host.Set(str + "PosSizeMode", this.posSizeMode_0.ToString());
            host.Set(str + "FixedDollarSize", this.double_0);
            host.Set(str + "PctEquitySize", this.double_1);
            host.Set(str + "MaxRiskSize", this.double_2);
        }

        public double FixedDollarSize
        {
            get
            {
                return this.double_0;
            }
            set
            {
                this.double_0 = value;
            }
        }

        public double MaxRiskSize
        {
            get
            {
                return this.double_2;
            }
            set
            {
                this.double_2 = value;
            }
        }

        public double PctEquitySize
        {
            get
            {
                return this.double_1;
            }
            set
            {
                this.double_1 = value;
            }
        }

        public WealthLab.PosSizeMode PosSizeMode
        {
            get
            {
                return this.posSizeMode_0;
            }
            set
            {
                this.posSizeMode_0 = value;
            }
        }
    }
}

