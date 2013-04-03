namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="HistoricalTrade", IsNullable=false)]
    public class HistoricalTrade
    {
        private BarDataScale barDataScale_0;
        private DateTime dateTime_0;
        private double double_0;
        private double double_1;
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;
        [CompilerGenerated]
        private string string_4;
        private WealthLab.TradeType tradeType_0;

        public HistoricalTrade()
        {
        }

        public HistoricalTrade(Order order)
        {
            this.string_0 = order.Account;
            this.tradeType_0 = order.AlertType;
            this.string_1 = order.Symbol;
            this.string_2 = order.OrderID;
            this.barDataScale_0 = order.DataScale;
            if (order.Strategy == null)
            {
                this.string_3 = "";
            }
            else
            {
                this.string_3 = order.Strategy.ID.ToString();
            }
            this.AccountTradeType = order.AccountTradeType;
        }

        public string AccountNumber
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

        public string AccountTradeType
        {
            [CompilerGenerated]
            get
            {
                return this.string_4;
            }
            [CompilerGenerated]
            set
            {
                this.string_4 = value;
            }
        }

        public string OrderID
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        public double Price
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

        public BarDataScale Scale
        {
            get
            {
                return this.barDataScale_0;
            }
            set
            {
                this.barDataScale_0 = value;
            }
        }

        public string StrategyID
        {
            get
            {
                return this.string_3;
            }
            set
            {
                this.string_3 = value;
            }
        }

        public string Symbol
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

        public DateTime TimeStamp
        {
            get
            {
                return this.dateTime_0;
            }
            set
            {
                this.dateTime_0 = value;
            }
        }

        public WealthLab.TradeType TradeType
        {
            get
            {
                return this.tradeType_0;
            }
            set
            {
                this.tradeType_0 = value;
            }
        }
    }
}

