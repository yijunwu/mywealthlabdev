namespace WealthLab
{
    using System;
    using System.Text;

    public class PositionSize
    {
        private double rawProfitDollarSize;
        private double rawProfitShareSize;
        private double startingCapital;
        private double dollarSize;
        private double shareSize;
        private double pctSize;
        private double riskSize;
        private double overrideShareSize;
        private double marginFactor;
        private PosSizeMode posSizeMode;
        private string simuScriptName;
        private string posSizerConfig;

        public PositionSize()
        {
            this.rawProfitDollarSize = 5000.0;
            this.rawProfitShareSize = 100.0;
            this.startingCapital = 100000.0;
            this.dollarSize = 5000.0;
            this.shareSize = 100.0;
            this.pctSize = 10.0;
            this.riskSize = 3.0;
            this.simuScriptName = "";
            this.marginFactor = 1.0;
            this.posSizerConfig = "";
        }

        public PositionSize(PosSizeMode mode, double amount)
        {
            this.rawProfitDollarSize = 5000.0;
            this.rawProfitShareSize = 100.0;
            this.startingCapital = 100000.0;
            this.dollarSize = 5000.0;
            this.shareSize = 100.0;
            this.pctSize = 10.0;
            this.riskSize = 3.0;
            this.simuScriptName = "";
            this.marginFactor = 1.0;
            this.posSizerConfig = "";
            this.posSizeMode = mode;
            switch (this.posSizeMode)
            {
                case PosSizeMode.RawProfitDollar:
                    this.rawProfitDollarSize = amount;
                    return;

                case PosSizeMode.RawProfitShare:
                    this.rawProfitShareSize = amount;
                    return;

                case PosSizeMode.Dollar:
                    this.dollarSize = amount;
                    return;

                case PosSizeMode.Share:
                    this.shareSize = amount;
                    return;

                case PosSizeMode.PctEquity:
                    this.pctSize = amount;
                    return;

                case PosSizeMode.MaxRisk:
                    this.riskSize = amount;
                    return;

                case PosSizeMode.SimuScript:
                    break;

                case PosSizeMode.ScriptOverride:
                    this.overrideShareSize = amount;
                    break;

                default:
                    return;
            }
        }

        public static PositionSize Parse(string string_2)
        {
            PositionSize size = new PositionSize();
            string[] strArray = string_2.Split(new char[] { ';' });
            size.posSizeMode = (PosSizeMode) Enum.Parse(typeof(PosSizeMode), strArray[0]);
            size.rawProfitDollarSize = double.Parse(strArray[1]);
            size.rawProfitShareSize = double.Parse(strArray[2]);
            size.startingCapital = double.Parse(strArray[3]);
            size.dollarSize = double.Parse(strArray[4]);
            size.shareSize = double.Parse(strArray[5]);
            size.pctSize = double.Parse(strArray[6]);
            size.riskSize = double.Parse(strArray[7]);
            size.simuScriptName = strArray[8];
            if (strArray.Length > 9)
            {
                size.marginFactor = double.Parse(strArray[9]);
            }
            if (strArray.Length > 10)
            {
                size.overrideShareSize = double.Parse(strArray[10]);
            }
            if (strArray.Length > 11)
            {
                size.posSizerConfig = strArray[11];
            }
            return size;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.posSizeMode);
            builder.Append(";");
            builder.Append(this.rawProfitDollarSize);
            builder.Append(";");
            builder.Append(this.rawProfitShareSize);
            builder.Append(";");
            builder.Append(this.startingCapital);
            builder.Append(";");
            builder.Append(this.dollarSize);
            builder.Append(";");
            builder.Append(this.shareSize);
            builder.Append(";");
            builder.Append(this.pctSize);
            builder.Append(";");
            builder.Append(this.riskSize);
            builder.Append(";");
            builder.Append(this.simuScriptName);
            builder.Append(";");
            builder.Append(this.marginFactor);
            builder.Append(";");
            builder.Append(this.overrideShareSize);
            builder.Append(";");
            builder.Append(this.posSizerConfig);
            return builder.ToString();
        }

        public double DollarSize
        {
            get
            {
                return this.dollarSize;
            }
            set
            {
                this.dollarSize = value;
            }
        }

        public double MarginFactor
        {
            get
            {
                return this.marginFactor;
            }
            set
            {
                this.marginFactor = value;
            }
        }

        public PosSizeMode Mode
        {
            get
            {
                return this.posSizeMode;
            }
            set
            {
                this.posSizeMode = value;
            }
        }

        public double OverrideShareSize
        {
            get
            {
                return this.overrideShareSize;
            }
            set
            {
                this.overrideShareSize = value;
            }
        }

        public double PctSize
        {
            get
            {
                return this.pctSize;
            }
            set
            {
                this.pctSize = value;
            }
        }

        public string PosSizerConfig
        {
            get
            {
                return this.posSizerConfig;
            }
            set
            {
                this.posSizerConfig = value;
            }
        }

        public string PosSizerThatWasConfigured
        {
            get
            {
                if (this.PosSizerConfig == "")
                {
                    return "";
                }
                if (this.PosSizerConfig[0] != '*')
                {
                    return "";
                }
                return this.PosSizerConfig.Split(new char[] { '^' })[0].Substring(1);
            }
        }

        public double RawProfitDollarSize
        {
            get
            {
                return this.rawProfitDollarSize;
            }
            set
            {
                this.rawProfitDollarSize = value;
            }
        }

        public bool RawProfitMode
        {
            get
            {
                if (this.Mode != PosSizeMode.RawProfitDollar)
                {
                    return (this.Mode == PosSizeMode.RawProfitShare);
                }
                return true;
            }
        }

        public double RawProfitShareSize
        {
            get
            {
                return this.rawProfitShareSize;
            }
            set
            {
                this.rawProfitShareSize = value;
            }
        }

        public double RiskSize
        {
            get
            {
                return this.riskSize;
            }
            set
            {
                this.riskSize = value;
            }
        }

        public double ShareSize
        {
            get
            {
                return this.shareSize;
            }
            set
            {
                this.shareSize = value;
            }
        }

        public string SimuScriptName
        {
            get
            {
                return this.simuScriptName;
            }
            set
            {
                this.simuScriptName = value;
            }
        }

        public double StartingCapital
        {
            get
            {
                return this.startingCapital;
            }
            set
            {
                this.startingCapital = value;
            }
        }

        public string Text
        {
            get
            {
                switch (this.Mode)
                {
                    case PosSizeMode.RawProfitDollar:
                        return (this.RawProfitDollarSize.ToString("C0") + " (RP)");

                    case PosSizeMode.RawProfitShare:
                        return (this.RawProfitShareSize.ToString("N0") + " shares(RP)");

                    case PosSizeMode.Dollar:
                        return this.DollarSize.ToString("C0");

                    case PosSizeMode.Share:
                        return (this.ShareSize.ToString("N0") + " shares");

                    case PosSizeMode.PctEquity:
                        return (this.PctSize.ToString() + "% equity");

                    case PosSizeMode.MaxRisk:
                        return (this.RiskSize.ToString() + "% risk");

                    case PosSizeMode.SimuScript:
                        return this.SimuScriptName;

                    case PosSizeMode.ScriptOverride:
                        return "Override in script";
                }
                return "Unknown";
            }
        }
    }
}

