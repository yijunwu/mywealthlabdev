namespace WealthLab
{
    using System;
    using System.Xml.Serialization;

    public class AccountPosition
    {
        private WealthLab.Account account_0;
        private double double_0;
        private double double_1;
        private double double_2;
        private WealthLab.PositionType positionType_0;
        private string string_0 = "";
        private string string_1;

        [XmlIgnore]
        public WealthLab.Account Account
        {
            get
            {
                return this.account_0;
            }
            set
            {
                this.account_0 = value;
            }
        }

        public string AcctType
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

        public double EntryPrice
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

        public double LastPrice
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

        [XmlIgnore]
        public double MarketValue
        {
            get
            {
                return (this.double_2 * this.double_0);
            }
        }

        public WealthLab.PositionType PositionType
        {
            get
            {
                return this.positionType_0;
            }
            set
            {
                this.positionType_0 = value;
            }
        }

        [XmlIgnore]
        public double ProfitDollars
        {
            get
            {
                double num;
                if (this.double_1 == 0.0)
                {
                    return 0.0;
                }
                if (this.positionType_0 == WealthLab.PositionType.Long)
                {
                    num = this.double_2 - this.double_1;
                }
                else
                {
                    num = this.double_1 - this.double_2;
                }
                return (num * this.double_0);
            }
        }

        [XmlIgnore]
        public double ProfitPct
        {
            get
            {
                if (this.double_1 == 0.0)
                {
                    return 0.0;
                }
                double num = this.double_1 * this.double_0;
                if (num == 0.0)
                {
                    return 0.0;
                }
                return ((this.ProfitDollars * 100.0) / num);
            }
        }

        public double Quantity
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

        public string Symbol
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
    }
}

