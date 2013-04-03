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
        private bool bool_4;
        [CompilerGenerated]
        private bool bool_5;
        private DateTime dateTime_2;
        [CompilerGenerated]
        private DateTime dateTime_3;
        private double double_6;
        private double double_7;
        private int int_1;
        private List<OrderMessage> list_0;
        private System.Windows.Forms.ListViewItem listViewItem_0;
        private object object_1;
        private object object_2;
        private OrderStatus orderStatus_0;
        private string string_9;

        public Order()
        {
            this.string_9 = Guid.NewGuid().ToString();
            this.list_0 = new List<OrderMessage>();
            this.int_1 = -1;
        }

        public Order(Alert alert)
        {
            this.string_9 = Guid.NewGuid().ToString();
            this.list_0 = new List<OrderMessage>();
            this.int_1 = -1;
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
            this.string_9 = Guid.NewGuid().ToString();
            this.list_0 = new List<OrderMessage>();
            this.int_1 = -1;
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
                return this.object_2;
            }
            set
            {
                this.object_2 = value;
            }
        }

        public double FillPrice
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

        public double FillQty
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

        public bool FromAutoTrading
        {
            get
            {
                return this.bool_4;
            }
            set
            {
                this.bool_4 = value;
            }
        }

        [XmlIgnore]
        public bool IsActive
        {
            get
            {
                if ((this.orderStatus_0 != OrderStatus.Submitted) && (this.orderStatus_0 != OrderStatus.Active))
                {
                    return (this.orderStatus_0 == OrderStatus.PartialFilled);
                }
                return true;
            }
        }

        [XmlIgnore]
        public bool IsActiveAtBackEnd
        {
            get
            {
                if (this.orderStatus_0 != OrderStatus.Active)
                {
                    return (this.orderStatus_0 == OrderStatus.PartialFilled);
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
                return this.bool_5;
            }
            [CompilerGenerated]
            set
            {
                this.bool_5 = value;
            }
        }

        [XmlIgnore]
        public bool IsCompleted
        {
            get
            {
                if (((this.orderStatus_0 != OrderStatus.Canceled) && (this.orderStatus_0 != OrderStatus.Error)) && (this.orderStatus_0 != OrderStatus.Filled))
                {
                    return (this.orderStatus_0 == OrderStatus.ErrorCancelReplace);
                }
                return true;
            }
        }

        [XmlIgnore]
        public System.Windows.Forms.ListViewItem ListViewItem
        {
            get
            {
                return this.listViewItem_0;
            }
            set
            {
                this.listViewItem_0 = value;
            }
        }

        public List<OrderMessage> Messages
        {
            get
            {
                return this.list_0;
            }
            set
            {
                this.list_0 = value;
            }
        }

        public string OrderID
        {
            get
            {
                return this.string_9;
            }
            set
            {
                this.string_9 = value;
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
                return this.dateTime_3;
            }
            [CompilerGenerated]
            set
            {
                this.dateTime_3 = value;
            }
        }

        public int StateIndex
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
            }
        }

        public OrderStatus Status
        {
            get
            {
                return this.orderStatus_0;
            }
            set
            {
                this.orderStatus_0 = value;
            }
        }

        [XmlIgnore]
        public object Tag
        {
            get
            {
                return this.object_1;
            }
            set
            {
                this.object_1 = value;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                return this.dateTime_2;
            }
            set
            {
                this.dateTime_2 = value;
            }
        }
    }
}

