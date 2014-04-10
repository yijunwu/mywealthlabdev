namespace WealthLab
{
    using System;
    using System.Xml.Serialization;

    public class AccountPosition
    {
        private WealthLab.Account account;
        private double quantity;
        private double entryPrice;
        private double lastPrice;
        private WealthLab.PositionType positionType;
        private string symbol = "";
        private string acctType;

        [XmlIgnore]
        public WealthLab.Account Account
        {
            get
            {
                return this.account;
            }
            set
            {
                this.account = value;
            }
        }

        public string AcctType
        {
            get
            {
                return this.acctType;
            }
            set
            {
                this.acctType = value;
            }
        }

        public double EntryPrice
        {
            get
            {
                return this.entryPrice;
            }
            set
            {
                this.entryPrice = value;
            }
        }

        public double LastPrice
        {
            get
            {
                return this.lastPrice;
            }
            set
            {
                this.lastPrice = value;
            }
        }

        [XmlIgnore]
        public double MarketValue
        {
            get
            {
                return (this.lastPrice * this.quantity);
            }
        }

        public WealthLab.PositionType PositionType
        {
            get
            {
                return this.positionType;
            }
            set
            {
                this.positionType = value;
            }
        }

        [XmlIgnore]
        public double ProfitDollars
        {
            get
            {
                double num;
                if (this.entryPrice == 0.0)
                {
                    return 0.0;
                }
                if (this.positionType == WealthLab.PositionType.Long)
                {
                    num = this.lastPrice - this.entryPrice;
                }
                else
                {
                    num = this.entryPrice - this.lastPrice;
                }
                return (num * this.quantity);
            }
        }

        [XmlIgnore]
        public double ProfitPct
        {
            get
            {
                if (this.entryPrice == 0.0)
                {
                    return 0.0;
                }
                double num = this.entryPrice * this.quantity;
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
                return this.quantity;
            }
            set
            {
                this.quantity = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
            set
            {
                this.symbol = value;
            }
        }
    }
}

