namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="Order", IsNullable=false)]
    public class Order : Alert
    {
        private bool fromAutoTrading;
        [CompilerGenerated]
        private bool isCancelReplace;
        private DateTime timeStamp;
        [CompilerGenerated]
        private DateTime originalAlertDate;
        private double fillPrice;
        private double fillQty;
        private int stateIndex;
        private List<OrderMessage> messages;
        private System.Windows.Forms.ListViewItem listViewItem;
        private object tag;
        private object brokerTag;
        private OrderStatus orderStatus;
        private string orderID;

        public Order()
        {
            this.orderID = Guid.NewGuid().ToString();
            this.messages = new List<OrderMessage>();
            this.stateIndex = -1;
        }

        public Order(Alert alert)
        {
            this.orderID = Guid.NewGuid().ToString();
            this.messages = new List<OrderMessage>();
            this.stateIndex = -1;
            base.OrderType = alert.OrderType;
            base.Shares = alert.Shares;
            base.AlertDate = alert.AlertDate;
            this.OriginalAlertDate = alert.AlertDate;
            base.AlertType = alert.AlertType;
            base.Position = null;
            base.PositionEntryDate = alert.PositionEntryDate;
            base.Price = alert.Price;
            base.Account = alert.Account;
            base.Symbol = alert.Symbol;
            base.Strategy = alert.Strategy;
            base.SignalName = alert.SignalName;
            base.Scale = alert.Scale;
            base.BarInterval = alert.BarInterval;
            base.Triggered = alert.Triggered;
            base.DataSet = alert.DataSet;
            base.DataRange = alert.DataRange;
            base.PosSize = alert.PosSize;
            base.Bars = null;
            base.BasisPrice = alert.BasisPrice;
            base.RiskStopLevel = alert.RiskStopLevel;
            base.AutoProfitLevel = alert.AutoProfitLevel;
            base.ChartDrawingObject = alert.ChartDrawingObject;
            base.Route = alert.Route;
            base.TIF = alert.TIF;
            base.ExtendedOrderType = alert.ExtendedOrderType;
            base.AccountTradeType = alert.AccountTradeType;
            base.SameBarParentOrderNumber = alert.SameBarParentOrderNumber;
        }

        public Order(Order order)
        {
            this.orderID = Guid.NewGuid().ToString();
            this.messages = new List<OrderMessage>();
            this.stateIndex = -1;
            base.OrderType = order.OrderType;
            base.Shares = order.Shares;
            base.AlertDate = order.AlertDate;
            this.OriginalAlertDate = order.OriginalAlertDate;
            base.AlertType = order.AlertType;
            base.Position = null;
            base.PositionEntryDate = order.PositionEntryDate;
            base.Price = order.Price;
            base.Account = order.Account;
            base.Symbol = order.Symbol;
            base.Strategy = order.Strategy;
            base.SignalName = order.SignalName;
            base.Scale = order.Scale;
            base.BarInterval = order.BarInterval;
            base.Triggered = order.Triggered;
            base.DataSet = order.DataSet;
            base.DataRange = order.DataRange;
            base.PosSize = order.PosSize;
            base.Bars = null;
            base.BasisPrice = order.BasisPrice;
            base.RiskStopLevel = order.RiskStopLevel;
            base.AutoProfitLevel = order.AutoProfitLevel;
            base.ChartDrawingObject = order.ChartDrawingObject;
            base.Route = order.Route;
            base.TIF = order.TIF;
            base.ExtendedOrderType = order.ExtendedOrderType;
            this.Status = order.Status;
            this.FillPrice = order.FillPrice;
            this.TimeStamp = order.TimeStamp;
            this.FillQty = order.FillQty;
            this.BrokerTag = order.BrokerTag;
            base.AccountTradeType = order.AccountTradeType;
            base.SameBarParentOrderNumber = order.SameBarParentOrderNumber;
        }

        public bool Matches(Order basisOrder, bool forSameBarExits = false)
        {
            if (basisOrder.Account != base.Account)
            {
                return false;
            }
            if (basisOrder.Strategy != base.Strategy)
            {
                return false;
            }
            if (basisOrder.DataScale != base.DataScale)
            {
                return false;
            }
            if (basisOrder.Symbol != base.Symbol)
            {
                return false;
            }
            if ((basisOrder.PositionEntryDate != base.PositionEntryDate) && ((base.SameBarParentOrderNumber == null) || !forSameBarExits))
            {
                return false;
            }
            if (!Alert.AccountTradeTypeMatch(basisOrder.AccountTradeType, base.AccountTradeType))
            {
                return false;
            }
            return true;
        }

        public bool MatchesCancelReplace(Order order)
        {
            return ((this.Matches(order, false) && (order.AlertType == base.AlertType)) && (order.OrderType == base.OrderType));
        }

        public bool MatchesExactly(Order order)
        {
            return (((this.Matches(order, false) && (order.OrderType == base.OrderType)) && (order.AlertType == base.AlertType)) && (order.Price == base.Price));
        }

        public object BrokerTag
        {
            get
            {
                return this.brokerTag;
            }
            set
            {
                this.brokerTag = value;
            }
        }

        public double FillPrice
        {
            get
            {
                return this.fillPrice;
            }
            set
            {
                this.fillPrice = value;
            }
        }

        public double FillQty
        {
            get
            {
                return this.fillQty;
            }
            set
            {
                this.fillQty = value;
            }
        }

        public bool FromAutoTrading
        {
            get
            {
                return this.fromAutoTrading;
            }
            set
            {
                this.fromAutoTrading = value;
            }
        }

        [XmlIgnore]
        public bool IsActive
        {
            get
            {
                if ((this.orderStatus != OrderStatus.Submitted) && (this.orderStatus != OrderStatus.Active))
                {
                    return (this.orderStatus == OrderStatus.PartialFilled);
                }
                return true;
            }
        }

        [XmlIgnore]
        public bool IsActiveAtBackEnd
        {
            get
            {
                if (this.orderStatus != OrderStatus.Active)
                {
                    return (this.orderStatus == OrderStatus.PartialFilled);
                }
                return true;
            }
        }

        [XmlIgnore]
        public bool IsCancelReplace
        {
            [CompilerGenerated]
            get
            {
                return this.isCancelReplace;
            }
            [CompilerGenerated]
            set
            {
                this.isCancelReplace = value;
            }
        }

        [XmlIgnore]
        public bool IsCompleted
        {
            get
            {
                if (((this.orderStatus != OrderStatus.Canceled) && (this.orderStatus != OrderStatus.Error)) && (this.orderStatus != OrderStatus.Filled))
                {
                    return (this.orderStatus == OrderStatus.ErrorCancelReplace);
                }
                return true;
            }
        }

        [XmlIgnore]
        public System.Windows.Forms.ListViewItem ListViewItem
        {
            get
            {
                return this.listViewItem;
            }
            set
            {
                this.listViewItem = value;
            }
        }

        public List<OrderMessage> Messages
        {
            get
            {
                return this.messages;
            }
            set
            {
                this.messages = value;
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

        [XmlIgnore]
        public string OrderTypeString
        {
            get
            {
                if (base.ExtendedOrderType != "")
                {
                    return base.ExtendedOrderType;
                }
                return base.OrderType.ToString();
            }
        }

        public DateTime OriginalAlertDate
        {
            [CompilerGenerated]
            get
            {
                return this.originalAlertDate;
            }
            [CompilerGenerated]
            set
            {
                this.originalAlertDate = value;
            }
        }

        public int StateIndex
        {
            get
            {
                return this.stateIndex;
            }
            set
            {
                this.stateIndex = value;
            }
        }

        public OrderStatus Status
        {
            get
            {
                return this.orderStatus;
            }
            set
            {
                this.orderStatus = value;
            }
        }

        [XmlIgnore]
        public object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
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
    }
}

