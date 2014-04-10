namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="HistoricalTrade", IsNullable=false)]
    public class HistoricalTrade
    {
        private BarDataScale barDataScale;
        private DateTime timeStamp;
        private double quantity;
        private double price;
        private string accountNumber;
        private string symbol;
        private string orderID;
        private string strategyID;
        [CompilerGenerated]
        private string accountTradeType;
        private WealthLab.TradeType tradeType;

        public HistoricalTrade()
        {
        }

        public HistoricalTrade(Order order)
        {
            this.accountNumber = order.Account;
            this.tradeType = order.AlertType;
            this.symbol = order.Symbol;
            this.orderID = order.OrderID;
            this.barDataScale = order.DataScale;
            if (order.Strategy == null)
            {
                this.strategyID = "";
            }
            else
            {
                this.strategyID = order.Strategy.ID.ToString();
            }
            this.AccountTradeType = order.AccountTradeType;
        }

        public string AccountNumber
        {
            get
            {
                return this.accountNumber;
            }
            set
            {
                this.accountNumber = value;
            }
        }

        public string AccountTradeType
        {
            [CompilerGenerated]
            get
            {
                return this.accountTradeType;
            }
            [CompilerGenerated]
            set
            {
                this.accountTradeType = value;
            }
        }

        public string OrderID
        {
            get
            {
                return this.orderID;
            }
            set
            {
                this.orderID = value;
            }
        }

        public double Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
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

        public BarDataScale Scale
        {
            get
            {
                return this.barDataScale;
            }
            set
            {
                this.barDataScale = value;
            }
        }

        public string StrategyID
        {
            get
            {
                return this.strategyID;
            }
            set
            {
                this.strategyID = value;
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

        public DateTime TimeStamp
        {
            get
            {
                return this.timeStamp;
            }
            set
            {
                this.timeStamp = value;
            }
        }

        public WealthLab.TradeType TradeType
        {
            get
            {
                return this.tradeType;
            }
            set
            {
                this.tradeType = value;
            }
        }
    }
}

