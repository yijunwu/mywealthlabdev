namespace WealthLab
{
    using System;
    using System.Text;

    public class PositionSize
    {
        private double double_0;
        private double double_1;
        private double double_2;
        private double double_3;
        private double double_4;
        private double double_5;
        private double double_6;
        private double double_7;
        private double double_8;
        private PosSizeMode posSizeMode_0;
        private string string_0;
        private string string_1;

        public PositionSize()
        {
            this.double_0 = 5000.0;
            this.double_1 = 100.0;
            this.double_2 = 100000.0;
            this.double_3 = 5000.0;
            this.double_4 = 100.0;
            this.double_5 = 10.0;
            this.double_6 = 3.0;
            this.string_0 = "";
            this.double_8 = 1.0;
            this.string_1 = "";
        }

        public PositionSize(PosSizeMode mode, double amount)
        {
            this.double_0 = 5000.0;
            this.double_1 = 100.0;
            this.double_2 = 100000.0;
            this.double_3 = 5000.0;
            this.double_4 = 100.0;
            this.double_5 = 10.0;
            this.double_6 = 3.0;
            this.string_0 = "";
            this.double_8 = 1.0;
            this.string_1 = "";
            this.posSizeMode_0 = mode;
            switch (this.posSizeMode_0)
            {
                case PosSizeMode.RawProfitDollar:
                    this.double_0 = amount;
                    return;

                case PosSizeMode.RawProfitShare:
                    this.double_1 = amount;
                    return;

                case PosSizeMode.Dollar:
                    this.double_3 = amount;
                    return;

                case PosSizeMode.Share:
                    this.double_4 = amount;
                    return;

                case PosSizeMode.PctEquity:
                    this.double_5 = amount;
                    return;

                case PosSizeMode.MaxRisk:
                    this.double_6 = amount;
                    return;

                case PosSizeMode.SimuScript:
                    break;

                case PosSizeMode.ScriptOverride:
                    this.double_7 = amount;
                    break;

                default:
                    return;
            }
        }

        public static PositionSize Parse(string string_2)
        {
            PositionSize size = new PositionSize();
            string[] strArray = string_2.Split(new char[] { ';' });
            size.posSizeMode_0 = (PosSizeMode) Enum.Parse(typeof(PosSizeMode), strArray[0]);
            size.double_0 = double.Parse(strArray[1]);
            size.double_1 = double.Parse(strArray[2]);
            size.double_2 = double.Parse(strArray[3]);
            size.double_3 = double.Parse(strArray[4]);
            size.double_4 = double.Parse(strArray[5]);
            size.double_5 = double.Parse(strArray[6]);
            size.double_6 = double.Parse(strArray[7]);
            size.string_0 = strArray[8];
            if (strArray.Length > 9)
            {
                size.double_8 = double.Parse(strArray[9]);
            }
            if (strArray.Length > 10)
            {
                size.double_7 = double.Parse(strArray[10]);
            }
            if (strArray.Length > 11)
            {
                size.string_1 = strArray[11];
            }
            return size;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.posSizeMode_0);
            builder.Append(";");
            builder.Append(this.double_0);
            builder.Append(";");
            builder.Append(this.double_1);
            builder.Append(";");
            builder.Append(this.double_2);
            builder.Append(";");
            builder.Append(this.double_3);
            builder.Append(";");
            builder.Append(this.double_4);
            builder.Append(";");
            builder.Append(this.double_5);
            builder.Append(";");
            builder.Append(this.double_6);
            builder.Append(";");
            builder.Append(this.string_0);
            builder.Append(";");
            builder.Append(this.double_8);
            builder.Append(";");
            builder.Append(this.double_7);
            builder.Append(";");
            builder.Append(this.string_1);
            return builder.ToString();
        }

        public double DollarSize
        {
            get
            {
                return this.double_3;
            }
            set
            {
                this.double_3 = value;
            }
        }

        public double MarginFactor
        {
            get
            {
                return this.double_8;
            }
            set
            {
                this.double_8 = value;
            }
        }

        public PosSizeMode Mode
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

        public double OverrideShareSize
        {
            get
            {
                return this.double_7;
            }
            set
            {
                this.double_7 = value;
            }
        }

        public double PctSize
        {
            get
            {
                return this.double_5;
            }
            set
            {
                this.double_5 = value;
            }
        }

        public string PosSizerConfig
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
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
                return this.double_0;
            }
            set
            {
                this.double_0 = value;
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
                return this.double_1;
            }
            set
            {
                this.double_1 = value;
            }
        }

        public double RiskSize
        {
            get
            {
                return this.double_6;
            }
            set
            {
                this.double_6 = value;
            }
        }

        public double ShareSize
        {
            get
            {
                return this.double_4;
            }
            set
            {
                this.double_4 = value;
            }
        }

        public string SimuScriptName
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public double StartingCapital
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

