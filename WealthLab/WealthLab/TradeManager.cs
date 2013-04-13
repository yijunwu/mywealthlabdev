namespace WealthLab
{
    using Fidelity.Components;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    [ToolboxBitmap(typeof(TradeManager), "TradeManager")]
    public class TradeManager : Component, IBrokerHost
    {
        private AutoTradingMode autoTradingMode_0;
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private static bool bool_6 = false;
        private WealthLab.BrokerProvider brokerProvider_0;
        private double double_0;
        private double double_1;
        private IContainer icontainer_0;
        private static readonly ILog ilog_0 = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int int_0;
        private int int_1;
        private ISettingsHost isettingsHost_0;
        private List<Order> list_0;
        private List<Alert> list_1;
        private List<Order> list_2;
        private List<HistoricalTrade> list_3;
        private List<OrderCancelSubmitGroup> list_4;
        private object object_0;
        public const string PAPER = "PaperAccount";
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;
        private string string_4;
        private string string_5;

        private EventHandler<EventArgs> eventHandler_0;

        private EventHandler<OrderEventArgs> eventHandler_1;

        private EventHandler<OrderEventArgs> eventHandler_2;

        private EventHandler<OrderEventArgs> eventHandler_3;

        private EventHandler<OrderEventArgs> eventHandler_4;

        private EventHandler<HistoricalTradeEventArgs> eventHandler_5;

        private EventHandler<HistoricalTradeEventArgs> eventHandler_6;

        private EventHandler<AccountEventArgs> eventHandler_7;

        private EventHandler<AccountEventArgs> eventHandler_8;

        private EventHandler<AccountPositionEventArgs> eventHandler_9;

        private EventHandler<AccountPositionEventArgs> eventHandler_10;

        private EventHandler<AccountPositionEventArgs> eventHandler_11;

        private EventHandler<QuoteEventArgs> eventHandler_12;

        private EventHandler<StringEventArgs> eventHandler_13;

        public event EventHandler<AccountEventArgs> AccountUpdated
        {
            add
            {
                EventHandler<AccountEventArgs> eventHandler;
                EventHandler<AccountEventArgs> eventHandler7 = this.eventHandler_7;
                do
                {
                    eventHandler = eventHandler7;
                    EventHandler<AccountEventArgs> eventHandler1 = (EventHandler<AccountEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler7 = Interlocked.CompareExchange<EventHandler<AccountEventArgs>>(ref this.eventHandler_7, eventHandler1, eventHandler);
                }
                while (eventHandler7 != eventHandler);
            }
            remove
            {
                EventHandler<AccountEventArgs> eventHandler;
                EventHandler<AccountEventArgs> eventHandler7 = this.eventHandler_7;
                do
                {
                    eventHandler = eventHandler7;
                    EventHandler<AccountEventArgs> eventHandler1 = (EventHandler<AccountEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler7 = Interlocked.CompareExchange<EventHandler<AccountEventArgs>>(ref this.eventHandler_7, eventHandler1, eventHandler);
                }
                while (eventHandler7 != eventHandler);
            }
        }

        public event EventHandler<HistoricalTradeEventArgs> HistoryItemAdded
        {
            add
            {
                EventHandler<HistoricalTradeEventArgs> eventHandler;
                EventHandler<HistoricalTradeEventArgs> eventHandler6 = this.eventHandler_6;
                do
                {
                    eventHandler = eventHandler6;
                    EventHandler<HistoricalTradeEventArgs> eventHandler1 = (EventHandler<HistoricalTradeEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler6 = Interlocked.CompareExchange<EventHandler<HistoricalTradeEventArgs>>(ref this.eventHandler_6, eventHandler1, eventHandler);
                }
                while (eventHandler6 != eventHandler);
            }
            remove
            {
                EventHandler<HistoricalTradeEventArgs> eventHandler;
                EventHandler<HistoricalTradeEventArgs> eventHandler6 = this.eventHandler_6;
                do
                {
                    eventHandler = eventHandler6;
                    EventHandler<HistoricalTradeEventArgs> eventHandler1 = (EventHandler<HistoricalTradeEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler6 = Interlocked.CompareExchange<EventHandler<HistoricalTradeEventArgs>>(ref this.eventHandler_6, eventHandler1, eventHandler);
                }
                while (eventHandler6 != eventHandler);
            }
        }

        public event EventHandler<HistoricalTradeEventArgs> HistoryItemUpdated
        {
            add
            {
                EventHandler<HistoricalTradeEventArgs> eventHandler;
                EventHandler<HistoricalTradeEventArgs> eventHandler5 = this.eventHandler_5;
                do
                {
                    eventHandler = eventHandler5;
                    EventHandler<HistoricalTradeEventArgs> eventHandler1 = (EventHandler<HistoricalTradeEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler5 = Interlocked.CompareExchange<EventHandler<HistoricalTradeEventArgs>>(ref this.eventHandler_5, eventHandler1, eventHandler);
                }
                while (eventHandler5 != eventHandler);
            }
            remove
            {
                EventHandler<HistoricalTradeEventArgs> eventHandler;
                EventHandler<HistoricalTradeEventArgs> eventHandler5 = this.eventHandler_5;
                do
                {
                    eventHandler = eventHandler5;
                    EventHandler<HistoricalTradeEventArgs> eventHandler1 = (EventHandler<HistoricalTradeEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler5 = Interlocked.CompareExchange<EventHandler<HistoricalTradeEventArgs>>(ref this.eventHandler_5, eventHandler1, eventHandler);
                }
                while (eventHandler5 != eventHandler);
            }
        }

        public event EventHandler<OrderEventArgs> OrderAdded
        {
            add
            {
                EventHandler<OrderEventArgs> eventHandler;
                EventHandler<OrderEventArgs> eventHandler2 = this.eventHandler_2;
                do
                {
                    eventHandler = eventHandler2;
                    EventHandler<OrderEventArgs> eventHandler1 = (EventHandler<OrderEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler2 = Interlocked.CompareExchange<EventHandler<OrderEventArgs>>(ref this.eventHandler_2, eventHandler1, eventHandler);
                }
                while (eventHandler2 != eventHandler);
            }
            remove
            {
                EventHandler<OrderEventArgs> eventHandler;
                EventHandler<OrderEventArgs> eventHandler2 = this.eventHandler_2;
                do
                {
                    eventHandler = eventHandler2;
                    EventHandler<OrderEventArgs> eventHandler1 = (EventHandler<OrderEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler2 = Interlocked.CompareExchange<EventHandler<OrderEventArgs>>(ref this.eventHandler_2, eventHandler1, eventHandler);
                }
                while (eventHandler2 != eventHandler);
            }
        }

        public event EventHandler<OrderEventArgs> OrderChanged
        {
            add
            {
                EventHandler<OrderEventArgs> eventHandler;
                EventHandler<OrderEventArgs> eventHandler4 = this.eventHandler_4;
                do
                {
                    eventHandler = eventHandler4;
                    EventHandler<OrderEventArgs> eventHandler1 = (EventHandler<OrderEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler4 = Interlocked.CompareExchange<EventHandler<OrderEventArgs>>(ref this.eventHandler_4, eventHandler1, eventHandler);
                }
                while (eventHandler4 != eventHandler);
            }
            remove
            {
                EventHandler<OrderEventArgs> eventHandler;
                EventHandler<OrderEventArgs> eventHandler4 = this.eventHandler_4;
                do
                {
                    eventHandler = eventHandler4;
                    EventHandler<OrderEventArgs> eventHandler1 = (EventHandler<OrderEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler4 = Interlocked.CompareExchange<EventHandler<OrderEventArgs>>(ref this.eventHandler_4, eventHandler1, eventHandler);
                }
                while (eventHandler4 != eventHandler);
            }
        }

        public event EventHandler<OrderEventArgs> OrderRemoved
        {
            add
            {
                EventHandler<OrderEventArgs> eventHandler;
                EventHandler<OrderEventArgs> eventHandler3 = this.eventHandler_3;
                do
                {
                    eventHandler = eventHandler3;
                    EventHandler<OrderEventArgs> eventHandler1 = (EventHandler<OrderEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler3 = Interlocked.CompareExchange<EventHandler<OrderEventArgs>>(ref this.eventHandler_3, eventHandler1, eventHandler);
                }
                while (eventHandler3 != eventHandler);
            }
            remove
            {
                EventHandler<OrderEventArgs> eventHandler;
                EventHandler<OrderEventArgs> eventHandler3 = this.eventHandler_3;
                do
                {
                    eventHandler = eventHandler3;
                    EventHandler<OrderEventArgs> eventHandler1 = (EventHandler<OrderEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler3 = Interlocked.CompareExchange<EventHandler<OrderEventArgs>>(ref this.eventHandler_3, eventHandler1, eventHandler);
                }
                while (eventHandler3 != eventHandler);
            }
        }

        public event EventHandler<OrderEventArgs> OrderStatusUpdated
        {
            add
            {
                EventHandler<OrderEventArgs> eventHandler;
                EventHandler<OrderEventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<OrderEventArgs> eventHandler2 = (EventHandler<OrderEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<OrderEventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
            remove
            {
                EventHandler<OrderEventArgs> eventHandler;
                EventHandler<OrderEventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<OrderEventArgs> eventHandler2 = (EventHandler<OrderEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<OrderEventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
        }

        public event EventHandler<EventArgs> OrdersUpdated
        {
            add
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }

        public event EventHandler<AccountPositionEventArgs> PositionAdded
        {
            add
            {
                EventHandler<AccountPositionEventArgs> eventHandler;
                EventHandler<AccountPositionEventArgs> eventHandler9 = this.eventHandler_9;
                do
                {
                    eventHandler = eventHandler9;
                    EventHandler<AccountPositionEventArgs> eventHandler1 = (EventHandler<AccountPositionEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler9 = Interlocked.CompareExchange<EventHandler<AccountPositionEventArgs>>(ref this.eventHandler_9, eventHandler1, eventHandler);
                }
                while (eventHandler9 != eventHandler);
            }
            remove
            {
                EventHandler<AccountPositionEventArgs> eventHandler;
                EventHandler<AccountPositionEventArgs> eventHandler9 = this.eventHandler_9;
                do
                {
                    eventHandler = eventHandler9;
                    EventHandler<AccountPositionEventArgs> eventHandler1 = (EventHandler<AccountPositionEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler9 = Interlocked.CompareExchange<EventHandler<AccountPositionEventArgs>>(ref this.eventHandler_9, eventHandler1, eventHandler);
                }
                while (eventHandler9 != eventHandler);
            }
        }

        public event EventHandler<AccountPositionEventArgs> PositionChanged
        {
            add
            {
                EventHandler<AccountPositionEventArgs> eventHandler;
                EventHandler<AccountPositionEventArgs> eventHandler11 = this.eventHandler_11;
                do
                {
                    eventHandler = eventHandler11;
                    EventHandler<AccountPositionEventArgs> eventHandler1 = (EventHandler<AccountPositionEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler11 = Interlocked.CompareExchange<EventHandler<AccountPositionEventArgs>>(ref this.eventHandler_11, eventHandler1, eventHandler);
                }
                while (eventHandler11 != eventHandler);
            }
            remove
            {
                EventHandler<AccountPositionEventArgs> eventHandler;
                EventHandler<AccountPositionEventArgs> eventHandler11 = this.eventHandler_11;
                do
                {
                    eventHandler = eventHandler11;
                    EventHandler<AccountPositionEventArgs> eventHandler1 = (EventHandler<AccountPositionEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler11 = Interlocked.CompareExchange<EventHandler<AccountPositionEventArgs>>(ref this.eventHandler_11, eventHandler1, eventHandler);
                }
                while (eventHandler11 != eventHandler);
            }
        }

        public event EventHandler<AccountPositionEventArgs> PositionRemoved
        {
            add
            {
                EventHandler<AccountPositionEventArgs> eventHandler;
                EventHandler<AccountPositionEventArgs> eventHandler10 = this.eventHandler_10;
                do
                {
                    eventHandler = eventHandler10;
                    EventHandler<AccountPositionEventArgs> eventHandler1 = (EventHandler<AccountPositionEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler10 = Interlocked.CompareExchange<EventHandler<AccountPositionEventArgs>>(ref this.eventHandler_10, eventHandler1, eventHandler);
                }
                while (eventHandler10 != eventHandler);
            }
            remove
            {
                EventHandler<AccountPositionEventArgs> eventHandler;
                EventHandler<AccountPositionEventArgs> eventHandler10 = this.eventHandler_10;
                do
                {
                    eventHandler = eventHandler10;
                    EventHandler<AccountPositionEventArgs> eventHandler1 = (EventHandler<AccountPositionEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler10 = Interlocked.CompareExchange<EventHandler<AccountPositionEventArgs>>(ref this.eventHandler_10, eventHandler1, eventHandler);
                }
                while (eventHandler10 != eventHandler);
            }
        }

        public event EventHandler<AccountEventArgs> PositionsUpdated
        {
            add
            {
                EventHandler<AccountEventArgs> eventHandler;
                EventHandler<AccountEventArgs> eventHandler8 = this.eventHandler_8;
                do
                {
                    eventHandler = eventHandler8;
                    EventHandler<AccountEventArgs> eventHandler1 = (EventHandler<AccountEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler8 = Interlocked.CompareExchange<EventHandler<AccountEventArgs>>(ref this.eventHandler_8, eventHandler1, eventHandler);
                }
                while (eventHandler8 != eventHandler);
            }
            remove
            {
                EventHandler<AccountEventArgs> eventHandler;
                EventHandler<AccountEventArgs> eventHandler8 = this.eventHandler_8;
                do
                {
                    eventHandler = eventHandler8;
                    EventHandler<AccountEventArgs> eventHandler1 = (EventHandler<AccountEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler8 = Interlocked.CompareExchange<EventHandler<AccountEventArgs>>(ref this.eventHandler_8, eventHandler1, eventHandler);
                }
                while (eventHandler8 != eventHandler);
            }
        }

        public event EventHandler<QuoteEventArgs> QuoteUpdated
        {
            add
            {
                EventHandler<QuoteEventArgs> eventHandler;
                EventHandler<QuoteEventArgs> eventHandler12 = this.eventHandler_12;
                do
                {
                    eventHandler = eventHandler12;
                    EventHandler<QuoteEventArgs> eventHandler1 = (EventHandler<QuoteEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler12 = Interlocked.CompareExchange<EventHandler<QuoteEventArgs>>(ref this.eventHandler_12, eventHandler1, eventHandler);
                }
                while (eventHandler12 != eventHandler);
            }
            remove
            {
                EventHandler<QuoteEventArgs> eventHandler;
                EventHandler<QuoteEventArgs> eventHandler12 = this.eventHandler_12;
                do
                {
                    eventHandler = eventHandler12;
                    EventHandler<QuoteEventArgs> eventHandler1 = (EventHandler<QuoteEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler12 = Interlocked.CompareExchange<EventHandler<QuoteEventArgs>>(ref this.eventHandler_12, eventHandler1, eventHandler);
                }
                while (eventHandler12 != eventHandler);
            }
        }

        public event EventHandler<StringEventArgs> StatusBarUpdated
        {
            add
            {
                EventHandler<StringEventArgs> eventHandler;
                EventHandler<StringEventArgs> eventHandler13 = this.eventHandler_13;
                do
                {
                    eventHandler = eventHandler13;
                    EventHandler<StringEventArgs> eventHandler1 = (EventHandler<StringEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler13 = Interlocked.CompareExchange<EventHandler<StringEventArgs>>(ref this.eventHandler_13, eventHandler1, eventHandler);
                }
                while (eventHandler13 != eventHandler);
            }
            remove
            {
                EventHandler<StringEventArgs> eventHandler;
                EventHandler<StringEventArgs> eventHandler13 = this.eventHandler_13;
                do
                {
                    eventHandler = eventHandler13;
                    EventHandler<StringEventArgs> eventHandler1 = (EventHandler<StringEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler13 = Interlocked.CompareExchange<EventHandler<StringEventArgs>>(ref this.eventHandler_13, eventHandler1, eventHandler);
                }
                while (eventHandler13 != eventHandler);
            }
        }

        public TradeManager()
        {
            this.list_0 = new List<Order>();
            this.string_1 = "";
            this.string_2 = "";
            this.list_1 = new List<Alert>();
            this.list_2 = new List<Order>();
            this.list_3 = new List<HistoricalTrade>();
            this.string_3 = "";
            this.list_4 = new List<OrderCancelSubmitGroup>();
            this.string_4 = "";
            this.string_5 = Application.UserAppDataPath + @"\Data\TradeManagerLog.txt";
            this.object_0 = new object();
            this.method_0();
        }

        public TradeManager(IContainer container)
        {
            this.list_0 = new List<Order>();
            this.string_1 = "";
            this.string_2 = "";
            this.list_1 = new List<Alert>();
            this.list_2 = new List<Order>();
            this.list_3 = new List<HistoricalTrade>();
            this.string_3 = "";
            this.list_4 = new List<OrderCancelSubmitGroup>();
            this.string_4 = "";
            this.string_5 = Application.UserAppDataPath + @"\Data\TradeManagerLog.txt";
            this.object_0 = new object();
            container.Add(this);
            this.method_0();
        }

        public void AccountBalanceUpdate(Account account)
        {
            if (this.eventHandler_7 != null)
            {
                this.eventHandler_7(this, new AccountEventArgs(account));
            }
        }

        public void AccountPositionsUpdate(Account account)
        {
            if (this.eventHandler_8 != null)
            {
                this.eventHandler_8(this, new AccountEventArgs(account));
            }
        }

        public void AddAlert(Alert alert, bool place, bool fromAutoTrading)
        {
            this.list_1.Clear();
            this.list_1.Add(alert);
            this.AddAlerts(this.list_1, place, fromAutoTrading);
        }

        public void AddAlerts(List<Alert> alerts, bool place, bool fromAutoTrading)
        {
            lock (this)
            {
                if (this.brokerProvider_0 == null)
                {
                    throw new InvalidOperationException("BrokerProvider property must be set in TradeManager");
                }
                for (int i = alerts.Count - 1; i >= 0; i--)
                {
                    if (alerts[i].OrderType == OrderType.AtClose)
                    {
                        alerts.RemoveAt(i);
                    }
                }
                foreach (Alert alert2 in alerts)
                {
                    if (alert2.Route == "")
                    {
                        alert2.Route = this.brokerProvider_0.RouteForStrategyOrders(alert2.DataScale);
                    }
                    if (alert2.TIF == "")
                    {
                        alert2.TIF = this.brokerProvider_0.TifForStrategyOrders(alert2.DataScale);
                    }
                }
                List<Order> orders = new List<Order>();
                if (alerts.Count != 0)
                {
                    List<Order> list5;
                    foreach (Alert alert in alerts)
                    {
                        if (this.method_5(alert) == null)
                        {
                            bool flag3;
                            Order order3 = new Order(alert) {
                                FromAutoTrading = fromAutoTrading
                            };
                            if ((order3.Account == "") || (order3.Account == null))
                            {
                                if (order3.Strategy != null)
                                {
                                    order3.Account = order3.Strategy.AccountNumber;
                                }
                                if (order3.Account == "")
                                {
                                    order3.Account = this.DefaultAccountNumber;
                                }
                                if (string.IsNullOrEmpty(order3.AccountTradeType))
                                {
                                    IList<string> list4 = this.brokerProvider_0.AccountTradeTypesAllowed(order3.Account, order3.AlertType.ToString());
                                    if (list4.Count > 0)
                                    {
                                        order3.AccountTradeType = list4[0];
                                    }
                                }
                            }
                            if ((order3.AlertType == TradeType.Sell) || (order3.AlertType == TradeType.Cover))
                            {
                                AccountPosition position = this.FindPosition(order3);
                                if ((fromAutoTrading && ((position == null) || (position.Quantity == 0.0))) && !DisablePortfolioSynch)
                                {
                                    continue;
                                }
                                if (((position != null) && (position.Quantity > 0.0)) && ((position.Quantity < order3.Shares) && !DisablePortfolioSynch))
                                {
                                    double quantity = (int) position.Quantity;
                                    OrderMessage item = new OrderMessage {
                                        Message = string.Concat(new object[] { "Order Quantity reduced from ", order3.Shares, " to ", quantity, " to match Position Quantity" }),
                                        DateTime = order3.AlertDate
                                    };
                                    order3.Messages.Add(item);
                                    order3.Shares = quantity;
                                }
                                if (((this.bool_0 && (order3.Strategy != null)) && ((position != null) && (position.Quantity > 0.0))) && (position.Quantity != order3.Shares))
                                {
                                    OrderMessage message2 = new OrderMessage {
                                        Message = string.Concat(new object[] { "Order Quantity changed from ", order3.Shares, " to ", position.Quantity, " to match Position Quantity (exit entire Position preference in effect)" })
                                    };
                                    order3.Messages.Add(message2);
                                    order3.Shares = position.Quantity;
                                }
                            }
                            if (!(flag3 = place))
                            {
                                Order order4 = this.method_9(order3);
                                if (order4 != null)
                                {
                                    order4.TimeStamp = order3.TimeStamp;
                                    order4.AlertDate = order3.AlertDate;
                                    order4.Price = order3.Price;
                                    order4.Shares = order3.Shares;
                                    if (this.eventHandler_4 != null)
                                    {
                                        this.eventHandler_4(this, new OrderEventArgs(order4));
                                    }
                                }
                                else
                                {
                                    lock ((list5 = this.list_0))
                                    {
                                        this.list_0.Add(order3);
                                    }
                                    if (this.eventHandler_2 != null)
                                    {
                                        this.eventHandler_2(this, new OrderEventArgs(order3));
                                    }
                                }
                            }
                            if (place && !this.method_8(order3))
                            {
                                flag3 = false;
                            }
                            if (flag3)
                            {
                                order3.Status = OrderStatus.Submitted;
                                orders.Add(order3);
                            }
                        }
                    }
                    if (orders.Count > 0)
                    {
                        if (!fromAutoTrading)
                        {
                            foreach (Order order16 in orders)
                            {
                                order16.Status = OrderStatus.Submitted;
                                lock (this.list_0)
                                {
                                    this.list_0.Add(order16);
                                }
                                if (this.eventHandler_2 != null)
                                {
                                    this.eventHandler_2(this, new OrderEventArgs(order16));
                                }
                            }
                            this.BrokerProvider.PlaceOrder(orders);
                        }
                        else
                        {
                            List<Order> list6 = new List<Order>();
                            foreach (Order order11 in orders)
                            {
                                bool flag6 = false;
                                using (List<Order>.Enumerator enumerator8 = list6.GetEnumerator())
                                {
                                    while (enumerator8.MoveNext())
                                    {
                                        Order current = enumerator8.Current;
                                        if ((current != order11) && this.method_1(current, order11))
                                        {
                                            ///goto  Label_0565;   ///WYJ fix, simplify the flow
                                            flag6 = true;
                                            break;
                                        }
                                    }
                                }
                                if (!flag6)
                                {
                                    list6.Add(order11);
                                }
                            }
                            foreach (Order order8 in list6)
                            {
                                foreach (Order order7 in this.list_0)
                                {
                                    if ((order7.Status != OrderStatus.Active) || !this.method_1(order7, order8))
                                    {
                                        continue;
                                    }
                                    bool flag5 = true;
                                    using (List<Order>.Enumerator enumerator6 = orders.GetEnumerator())
                                    {
                                        while (enumerator6.MoveNext())
                                        {
                                            Order order9 = enumerator6.Current;
                                            if (order9.Matches(order7, false))
                                            {
                                                ///goto  Label_061A;  ///WYJ fix, simplify the flow
                                                flag5 = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (flag5)
                                    {
                                        order7.Status = OrderStatus.CancelPending;
                                        if (this.eventHandler_1 != null)
                                        {
                                            this.eventHandler_1(this, new OrderEventArgs(order7));
                                        }
                                        this.BrokerProvider.CancelOrder(order7);
                                    }
                                }
                            }
                            while (orders.Count > 0)
                            {
                                List<Order>.Enumerator enumerator;
                                Order order2 = orders[0];
                                List<Order> list3 = new List<Order> {
                                    order2
                                };
                                orders.Remove(order2);
                                for (int j = orders.Count - 1; j >= 0; j--)
                                {
                                    Order order12 = orders[j];
                                    if (order12.Matches(order2, false))
                                    {
                                        list3.Add(order12);
                                        orders.Remove(order12);
                                    }
                                }
                                this.method_6(list3, OrderType.Stop, TradeType.Sell, 1);
                                this.method_6(list3, OrderType.Stop, TradeType.Cover, -1);
                                this.method_6(list3, OrderType.Limit, TradeType.Sell, -1);
                                this.method_6(list3, OrderType.Limit, TradeType.Cover, 1);
                                bool flag4 = false;
                                using (List<Order>.Enumerator enumerator4 = list3.GetEnumerator())
                                {
                                    while (enumerator4.MoveNext())
                                    {
                                        Order order18 = enumerator4.Current;
                                        if (((order18.AlertType == TradeType.Sell) || (order18.AlertType == TradeType.Cover)) && (order18.OrderType == OrderType.Market))
                                        {
                                            ///goto  Label_076E;  ///WYJ fix, simplify the flow
                                            flag4 = true;
                                            break;
                                        }
                                    }
                                }
                                if (flag4)
                                {
                                    for (int n = list3.Count - 1; n >= 0; n--)
                                    {
                                        if (((list3[n].AlertType == TradeType.Sell) || (list3[n].AlertType == TradeType.Cover)) && (list3[n].OrderType != OrderType.Market))
                                        {
                                            this.list_0.Remove(list3[n]);
                                            list3.RemoveAt(n);
                                        }
                                    }
                                }
                                List<Order> list = new List<Order>();
                                foreach (Order order in this.list_0)
                                {
                                    if (order.IsActive && order.Matches(order2, true))
                                    {
                                        list.Add(order);
                                    }
                                }
                                for (int k = list3.Count - 1; k >= 0; k--)
                                {
                                    Order order14 = list3[k];
                                    using (enumerator = list.GetEnumerator())
                                    {
                                        Order order13;
                                        while (enumerator.MoveNext())
                                        {
                                            order13 = enumerator.Current;
                                            if (order13.MatchesExactly(order14))
                                            {
                                                ///goto  Label_0887; ///WYJ fix, simplify the flow
                                                list3.RemoveAt(k);
                                                order13.TimeStamp = order14.TimeStamp;
                                                if (this.eventHandler_1 != null)
                                                {
                                                    this.eventHandler_1(this, new OrderEventArgs(order13));
                                                }
                                                list.Remove(order13);
                                                this.list_0.Remove(order14);

                                                break;
                                            }
                                        }
                                    }
                                }
                                for (int m = list3.Count - 1; m >= 0; m--)
                                {
                                    Order order19 = list3[m];
                                    using (enumerator = list.GetEnumerator())
                                    {
                                        Order order20;
                                        while (enumerator.MoveNext())
                                        {
                                            order20 = enumerator.Current;
                                            if ((order19.MatchesCancelReplace(order20) && this.BrokerProvider.CancelReplaceOrderTypesAllowed(order20).Contains(order19.OrderType.ToString())) && this.BrokerProvider.AllowCancelReplace(order19, ""))
                                            {
                                                ///goto  Label_096F; ///WYJ fix, simplify the flow
                                                list3.RemoveAt(m);
                                                list.Remove(order20);
                                                order19.Status = OrderStatus.Submitted;
                                                order19.IsCancelReplace = true;
                                                order20.Status = OrderStatus.CancelPending;
                                                lock ((list5 = this.list_0))
                                                {
                                                    this.list_0.Add(order19);
                                                }
                                                if (this.eventHandler_2 != null)
                                                {
                                                    this.eventHandler_2(this, new OrderEventArgs(order19));
                                                }
                                                if (this.eventHandler_1 != null)
                                                {
                                                    this.eventHandler_1(this, new OrderEventArgs(order20));
                                                }
                                                this.BrokerProvider.CancelReplace(order20, order19);

                                                break;
                                            }
                                        }
                                    }
                                }
                                foreach (Order order6 in this.list_0)
                                {
                                    if (((list3.Count > 0) && order6.Matches(list3[0], true)) && ((order6.Status == OrderStatus.CancelPending) && !list.Contains(order6)))
                                    {
                                        list.Add(order6);
                                    }
                                }
                                if (list.Count > 0)
                                {
                                    if (list3.Count > 0)
                                    {
                                        OrderCancelSubmitGroup group = new OrderCancelSubmitGroup();
                                        foreach (Order order15 in list)
                                        {
                                            order15.Status = OrderStatus.CancelPending;
                                            if (this.eventHandler_1 != null)
                                            {
                                                this.eventHandler_1(this, new OrderEventArgs(order15));
                                            }
                                            group.CancelPending.Add(order15);
                                        }
                                        foreach (Order order5 in list3)
                                        {
                                            group.SubmitPending.Add(order5);
                                            order5.IsCancelReplace = true;
                                            this.list_0.Remove(order5);
                                        }
                                        this.list_4.Add(group);
                                        this.BrokerProvider.CancelOrder(list);
                                    }
                                }
                                else if (list3.Count > 0)
                                {
                                    foreach (Order order17 in list3)
                                    {
                                        order17.Status = OrderStatus.Submitted;
                                        lock ((list5 = this.list_0))
                                        {
                                            this.list_0.Add(order17);
                                        }
                                        if (this.eventHandler_2 != null)
                                        {
                                            this.eventHandler_2(this, new OrderEventArgs(order17));
                                        }
                                    }
                                    this.BrokerProvider.PlaceOrder(list3);
                                }
                            }
                        }
                    }
                    this.bool_4 = true;
                    this.UpdateOrders();
                }
            }
        }

        public void AddOrder(Order order)
        {
            lock (this.list_0)
            {
                this.list_0.Add(order);
            }
            this.bool_4 = true;
            this.UpdateOrders();
        }

        public void CancelAll()
        {
            List<Order> orders = new List<Order>();
            lock (this.list_0)
            {
                foreach (Order order in this.list_0)
                {
                    if (order.Status == OrderStatus.Submitted)
                    {
                        order.FromAutoTrading = false;
                        order.Status = OrderStatus.Canceled;
                        if (this.eventHandler_1 != null)
                        {
                            this.eventHandler_1(this, new OrderEventArgs(order));
                        }
                    }
                    else if ((order.Status == OrderStatus.Active) || (order.Status == OrderStatus.PartialFilled))
                    {
                        order.Status = OrderStatus.CancelPending;
                        orders.Add(order);
                        if (this.eventHandler_1 != null)
                        {
                            this.eventHandler_1(this, new OrderEventArgs(order));
                        }
                    }
                }
            }
            if (orders.Count > 0)
            {
                this.BrokerProvider.CancelOrder(orders);
                this.UpdateOrders();
            }
        }

        public void CancelOrder(Order order)
        {
            List<Order> orders = new List<Order> {
                order
            };
            this.CancelOrders(orders);
        }

        public void CancelOrders(IList<Order> orders)
        {
            List<Order> list = new List<Order>();
            for (int i = orders.Count - 1; i >= 0; i--)
            {
                Order order = orders[i];
                if (order.Status == OrderStatus.ErrorCancelReplace)
                {
                    order.FromAutoTrading = false;
                    order.Status = OrderStatus.Canceled;
                    if (this.eventHandler_1 != null)
                    {
                        this.eventHandler_1(this, new OrderEventArgs(order));
                    }
                }
                else if ((order.Status == OrderStatus.Active) || (order.Status == OrderStatus.PartialFilled))
                {
                    order.FromAutoTrading = false;
                    order.Status = OrderStatus.CancelPending;
                    if (this.eventHandler_1 != null)
                    {
                        this.eventHandler_1(this, new OrderEventArgs(order));
                    }
                    list.Add(order);
                }
            }
            if (list.Count > 0)
            {
                this.BrokerProvider.CancelOrder(list);
            }
            this.bool_4 = true;
            this.UpdateOrders();
        }

        public void CancelReplaceOrder(Order order, Order newOrder)
        {
            order.Status = OrderStatus.CancelPending;
            newOrder.Status = OrderStatus.Submitted;
            newOrder.IsCancelReplace = true;
            lock (this.list_0)
            {
                this.list_0.Add(newOrder);
            }
            if (this.eventHandler_1 != null)
            {
                this.eventHandler_1(this, new OrderEventArgs(order));
            }
            if (this.eventHandler_2 != null)
            {
                this.eventHandler_2(this, new OrderEventArgs(newOrder));
            }
            this.BrokerProvider.CancelReplace(order, newOrder);
            this.bool_4 = true;
            this.UpdateOrders();
        }

        public void CancelStrategyOrders(string account, Strategy strategy, string symbol, BarDataScale dataScale)
        {
            if (account == "")
            {
                account = this.DefaultAccountNumber;
            }
            List<Order> orders = new List<Order>();
            lock (this.list_0)
            {
                foreach (Order order in this.list_0)
                {
                    if (((order.IsActiveAtBackEnd && (order.Account == account)) && ((order.Strategy == strategy) && (order.Symbol == symbol))) && (order.DataScale == dataScale))
                    {
                        order.Status = OrderStatus.CancelPending;
                        if (this.eventHandler_1 != null)
                        {
                            this.eventHandler_1(this, new OrderEventArgs(order));
                        }
                        orders.Add(order);
                    }
                }
            }
            if (orders.Count > 0)
            {
                this.BrokerProvider.CancelOrder(orders);
                this.UpdateOrders();
                this.bool_4 = true;
            }
        }

        public void DisplayStatusBarMessage(string message)
        {
            if (this.eventHandler_13 != null)
            {
                this.eventHandler_13(this, new StringEventArgs(message));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public Account FindAccount(string acctNum)
        {
            Account account2;
            if (this.brokerProvider_0 == null)
            {
                return null;
            }
            using (IEnumerator<Account> enumerator = this.brokerProvider_0.Accounts.GetEnumerator())
            {
                Account current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.AccountNumber == acctNum)
                    {
                        ///goto  Label_003D;  ///WYJ fix, simplify the flow
                        account2 = current;
                        return account2;
                    }
                }
                return null;
            }
        }

        public HistoricalTrade FindHistoricalTrade(string orderID)
        {
            lock (this.list_3)
            {
                foreach (HistoricalTrade trade in this.list_3)
                {
                    if (trade.OrderID == orderID)
                    {
                        return trade;
                    }
                }
            }
            return null;
        }

        public AccountPosition FindPosition(Order order)
        {
            foreach (Account account in this.brokerProvider_0.Accounts)
            {
                if (account.AccountNumber == order.Account)
                {
                    foreach (AccountPosition position in account.Positions)
                    {
                        if (((position.Symbol == order.Symbol) && (position.PositionType == order.PositionType)) && Alert.AccountTradeTypeMatch(this.brokerProvider_0.GetPositionAccountTradeType(position), order.AccountTradeType))
                        {
                            return position;
                        }
                    }
                }
            }
            return null;
        }

        public AccountPosition FindPosition(string account, PositionType posType, string symbol)
        {
            foreach (Account account2 in this.brokerProvider_0.Accounts)
            {
                if (account2.AccountNumber == account)
                {
                    foreach (AccountPosition position in account2.Positions)
                    {
                        if ((position.Symbol == symbol) && (position.PositionType == posType))
                        {
                            return position;
                        }
                    }
                }
            }
            return null;
        }

        public List<Order> GetOrders(Account account)
        {
            List<Order> list = new List<Order>();
            lock (this.list_0)
            {
                foreach (Order order in this.list_0)
                {
                    if (order.Account == account.AccountNumber)
                    {
                        list.Add(order);
                    }
                }
            }
            return list;
        }

        public IList<Order> GetOrdersForAccount(string acctNum)
        {
            this.list_2.Clear();
            lock (this.list_0)
            {
                foreach (Order order in this.list_0)
                {
                    if ((order.Account == acctNum) || (acctNum == ""))
                    {
                        this.list_2.Add(order);
                    }
                }
            }
            return this.list_2;
        }

        public void LoadOrdersAndHistory()
        {
            lock (this.list_0)
            {
                this.list_0.Clear();
                if (File.Exists(this.string_1))
                {
                    XmlSerializer serializer2 = new XmlSerializer(typeof(List<Order>));
                    TextReader textReader = new StreamReader(this.string_1);
                    try
                    {
                        this.list_0 = (List<Order>) serializer2.Deserialize(textReader);
                    }
                    finally
                    {
                        textReader.Close();
                    }
                }
                foreach (Order order in this.list_0)
                {
                    if (order.IsActive || (order.Status == OrderStatus.CancelPending))
                    {
                        order.Status = OrderStatus.Unknown;
                    }
                }
            }
            this.UpdateOrders();
            this.list_3.Clear();
            if (File.Exists(this.string_2))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<HistoricalTrade>));
                TextReader reader = new StreamReader(this.string_2);
                try
                {
                    this.list_3 = (List<HistoricalTrade>) serializer.Deserialize(reader);
                }
                finally
                {
                    reader.Close();
                }
            }
            new Thread(new ThreadStart(this.method_4)) { IsBackground = true }.Start();
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        private bool method_1(Order order_0, Order order_1)
        {
            return ((((order_0.Account == order_1.Account) && (order_0.Strategy == order_1.Strategy)) && (order_0.Symbol == order_1.Symbol)) && (order_0.DataScale == order_1.DataScale));
        }

        private void method_2(string string_6)
        {
            lock (this.string_4)
            {
                try
                {
                    this.string_4 = this.string_4 + string_6 + Environment.NewLine;
                    FileNameValidator.ValidateFileName(this.string_4);
                    File.WriteAllText(this.string_5, this.string_4);
                }
                catch
                {
                }
            }
        }

        private bool method_3(string string_6)
        {
            if (string_6.StartsWith("PaperAccount"))
            {
                return (this.AutoTradingEnabled == AutoTradingMode.Paper);
            }
            return (this.AutoTradingEnabled == AutoTradingMode.Live);
        }

        private void method_4()
        {
            while (true)
            {
                Thread.Sleep(0x1388);
                if (this.bool_3)
                {
                    this.bool_3 = false;
                    this.SaveTradeHistory();
                }
                if (this.bool_4)
                {
                    this.bool_4 = false;
                    this.SaveOrders();
                }
            }
        }

        private Order method_5(Alert alert_0)
        {
            if (!alert_0.IsSameBarExit)
            {
                foreach (Order order in this.Orders)
                {
                    if (!order.IsCompleted || (order.OriginalAlertDate == alert_0.AlertDate))
                    {
                        if (((order.Strategy == null) && (order.ChartDrawingObject == alert_0.ChartDrawingObject)) && (order.ChartDrawingObject != null))
                        {
                            return order;
                        }
                        if (((((order.Strategy == alert_0.Strategy) && !(order.Symbol != alert_0.Symbol)) && ((order.OrderType == alert_0.OrderType) && (order.AlertType == alert_0.AlertType))) && (((order.Shares == alert_0.Shares) && (Math.Round(order.Price, 4) == Math.Round(alert_0.Price, 4))) && (!(order.Account != alert_0.Account) && Alert.AccountTradeTypeMatch(order.AccountTradeType, alert_0.AccountTradeType)))) && (((order.BarInterval == alert_0.BarInterval) && (order.Scale == alert_0.Scale)) && (((order.PosSize != null) && (alert_0.PosSize != null)) && !(order.PosSize.ToString() != alert_0.PosSize.ToString()))))
                        {
                            return order;
                        }
                    }
                }
            }
            return null;
        }

        private void method_6(IList<Order> ilist_0, OrderType orderType_0, TradeType tradeType_0, int int_2)
        {
            for (int i = 0; i < ilist_0.Count; i++)
            {
                if ((ilist_0[i].OrderType == orderType_0) && (ilist_0[i].AlertType == tradeType_0))
                {
                    Order order = ilist_0[i];
                    for (int j = ilist_0.Count - 1; j > i; j--)
                    {
                        if (((ilist_0[j].OrderType == orderType_0) && (ilist_0[j].AlertType == tradeType_0)) && ((ilist_0[j].Price * int_2) > (order.Price * int_2)))
                        {
                            order = ilist_0[j];
                        }
                    }
                    for (int k = ilist_0.Count - 1; k >= 0; k--)
                    {
                        if (((ilist_0[k].OrderType == orderType_0) && (ilist_0[k].AlertType == tradeType_0)) && (ilist_0[k] != order))
                        {
                            this.list_0.Remove(ilist_0[k]);
                            ilist_0.RemoveAt(k);
                        }
                    }
                    return;
                }
            }
        }

        private void method_7(Alert alert_0, Order order_0)
        {
            alert_0.Account = order_0.Account;
            alert_0.AlertDate = order_0.AlertDate;
            alert_0.BarInterval = order_0.BarInterval;
            alert_0.DataRange = order_0.DataRange;
            alert_0.DataSet = order_0.DataSet;
            alert_0.PosSize = order_0.PosSize;
            alert_0.Route = order_0.Route;
            alert_0.Scale = order_0.Scale;
            alert_0.SecurityCode = order_0.SecurityCode;
            alert_0.Shares = order_0.Shares;
            if (order_0.Position != null)
            {
                alert_0.Shares = order_0.Position.Shares;
            }
            else
            {
                alert_0.Shares = order_0.Shares;
            }
            alert_0.Strategy = order_0.Strategy;
            alert_0.StrategyID = order_0.StrategyID;
            alert_0.Symbol = order_0.Symbol;
            alert_0.TIF = order_0.TIF;
            alert_0.IsSameBarExit = true;
            alert_0.AccountTradeType = order_0.AccountTradeType;
        }

        private bool method_8(Order order_0)
        {
            if (order_0.Strategy != null)
            {
                if (order_0.IsCancelReplace)
                {
                    return true;
                }
                if ((order_0.AlertType == TradeType.Sell) || (order_0.AlertType == TradeType.Cover))
                {
                    return true;
                }
                Account account = this.FindAccount(order_0.Account);
                if (account != null)
                {
                    if (this.EnableCashThreshold && (account.AvailableCash < this.CashThreshold))
                    {
                        OrderMessage item = new OrderMessage {
                            Message = "Order not placed because Available Cash of " + account.AvailableCash.ToString("C2") + " is below Threshold value of " + this.CashThreshold.ToString("C2"),
                            DateTime = DateTime.Now
                        };
                        order_0.Messages.Add(item);
                        order_0.Status = OrderStatus.Error;
                        return false;
                    }
                    if (this.EnableBuyingPowerThreshold && (account.BuyingPower < this.BuyingPowerThreshold))
                    {
                        OrderMessage message = new OrderMessage {
                            Message = "Order not placed because Buying Power of " + account.BuyingPower.ToString("C2") + " is below Threshold value of " + this.BuyingPowerThreshold.ToString("C2"),
                            DateTime = DateTime.Now
                        };
                        order_0.Messages.Add(message);
                        order_0.Status = OrderStatus.Error;
                        return false;
                    }
                }
            }
            return true;
        }

        private Order method_9(Order order_0)
        {
            Order order2;
            if (order_0.Strategy == null)
            {
                return null;
            }
            if (order_0.Status != OrderStatus.Staged)
            {
                return null;
            }
            using (List<Order>.Enumerator enumerator = this.list_0.GetEnumerator())
            {
                Order current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if ((((current.Status == OrderStatus.Staged) && (current.Strategy == order_0.Strategy)) && ((current.DataScale == order_0.DataScale) && (current.AlertType == order_0.AlertType))) && (((current.OrderType == order_0.OrderType) && (current.Symbol == order_0.Symbol)) && ((current.Account == order_0.Account) && Alert.AccountTradeTypeMatch(current.AccountTradeType, order_0.AccountTradeType))))
                    {
                        ///goto  Label_00BA; ///WYJ fix, simplify the flow
                        order2 = current;
                        return order2;
                    }
                }
                return null;
            }
        }

        public void OrderStatusUpdate(string orderID, OrderStatus status, DateTime timestamp, double fillPrice, double fillQty, int code, string message)
        {
            this.OrderStatusUpdate(orderID, status, timestamp, fillPrice, fillQty, code, message, -1);
        }

        public void OrderStatusUpdate(string orderID, OrderStatus status, DateTime timestamp, double fillPrice, double fillQty, int code, string message, int stateIndex)
        {
            List<Order> list = new List<Order>();
            this.method_2(string.Concat(new object[] { "Incoming Status = ", status, " ", timestamp.ToLongDateString(), " ", timestamp.ToLongTimeString() }));
            this.method_2(string.Concat(new object[] { "OrderID=", orderID, " FillPrice=", fillPrice, " FillQty=", fillQty, " Code=", code, " Msg=", message }));
            lock (this.object_0)
            {
                Order order2;
                OrderMessage message3;
                bool flag2 = false;
                for (int i = this.list_0.Count - 1; i >= 0; i--)
                {
                    order2 = this.list_0[i];
                    if (order2.OrderID == orderID)
                    {
                        ///goto  Label_0106;  ///WYJ fix, simplify the flow
                        //Label_0106:
                        message3 = new OrderMessage();
                        message3.DateTime = timestamp;
                        message3.Message = "Status Update: " + status;
                        order2.Messages.Add(message3);
                        this.method_2(string.Concat(new object[] { "Matched Order: Symbol=", order2.Symbol, " CurrentStatus=", order2.Status, " Type=", order2.AlertType, " Shares=", order2.Shares, " Order=", order2.OrderType, " Price=", order2.Price, " Filled=", order2.FillQty, " FillPrice=", order2.FillPrice }));
                        if (((order2.Status == status) && (order2.FillPrice == fillPrice)) && ((order2.FillQty == fillQty) && (message == "")))
                        {
                            this.method_2("Status, FillPrice, FillQty already matches, bypass processing.");
                            this.method_2("");
                            return;
                        }
                        if (status == OrderStatus.ErrorCancelReplace)
                        {
                            lock (this.list_0)
                            {
                                this.list_0.Remove(order2);
                            }
                            if (this.eventHandler_3 != null)
                            {
                                this.eventHandler_3(this, new OrderEventArgs(order2));
                            }
                            if (this.eventHandler_0 != null)
                            {
                                this.eventHandler_0(this, EventArgs.Empty);
                            }
                        }
                        if (order2.Status == OrderStatus.Filled)
                        {
                            this.method_2("Status of order already Filled, bypass processing.");
                            this.method_2("");
                            return;
                        }
                        order2.Status = status;
                        order2.TimeStamp = timestamp;
                        double num3 = 0.0;
                        if (fillQty > order2.FillQty)
                        {
                            order2.FillPrice = fillPrice;
                            num3 = fillQty - order2.FillQty;
                            order2.FillQty = fillQty;
                        }
                        if (message != "")
                        {
                            OrderMessage item = new OrderMessage {
                                Message = code + ": " + message,
                                DateTime = timestamp
                            };
                            order2.Messages.Add(item);
                        }
                        if ((status == OrderStatus.Filled) || (status == OrderStatus.PartialFilled))
                        {
                            Account account = this.FindAccount(order2.Account);
                            if (account != null)
                            {
                                AccountPosition position = this.FindPosition(order2);
                                if ((order2.AlertType != TradeType.Buy) && (order2.AlertType != TradeType.Short))
                                {
                                    if (position != null)
                                    {
                                        this.method_2("Remove shares from position");
                                        position.Quantity -= num3;
                                        if (position.Quantity < 0.0)
                                        {
                                            position.Quantity = 0.0;
                                        }
                                        if (position.Quantity == 0.0)
                                        {
                                            this.method_2("Remove Position");
                                            account.Positions.Remove(position);
                                            if (this.eventHandler_10 != null)
                                            {
                                                this.eventHandler_10(this, new AccountPositionEventArgs(position));
                                            }
                                        }
                                        else if (this.eventHandler_11 != null)
                                        {
                                            this.eventHandler_11(this, new AccountPositionEventArgs(position));
                                        }
                                        this.BrokerProvider.AccountPositionsMofidied(account);
                                    }
                                }
                                else
                                {
                                    if (position == null)
                                    {
                                        this.method_2("Adding new Position");
                                        position = new AccountPosition {
                                            Account = account,
                                            EntryPrice = fillPrice,
                                            LastPrice = fillPrice,
                                            PositionType = order2.PositionType,
                                            Quantity = fillQty,
                                            Symbol = order2.Symbol
                                        };
                                        account.Positions.Add(position);
                                        this.BrokerProvider.AccountPositionsMofidied(account);
                                        if (this.eventHandler_9 != null)
                                        {
                                            this.eventHandler_9(this, new AccountPositionEventArgs(position));
                                        }
                                    }
                                    else
                                    {
                                        this.method_2("Add Shares to existing Position");
                                        position.Quantity += num3;
                                        this.BrokerProvider.AccountPositionsMofidied(account);
                                        if (this.eventHandler_11 != null)
                                        {
                                            this.eventHandler_11(this, new AccountPositionEventArgs(position));
                                        }
                                    }
                                    if ((((status == OrderStatus.Filled) && this.SameBarExits) && (order2.FromAutoTrading && this.method_3(order2.Account))) && ((order2.RiskStopLevel > 0.0) || (order2.AutoProfitLevel > 0.0)))
                                    {
                                        List<Alert> alerts = new List<Alert>();
                                        if (order2.RiskStopLevel > 0.0)
                                        {
                                            this.method_2("Create same-bar stop order");
                                            Alert alert = new Alert();
                                            this.method_7(alert, order2);
                                            alert.AlertType = (order2.AlertType == TradeType.Buy) ? TradeType.Sell : TradeType.Cover;
                                            alert.OrderType = OrderType.Stop;
                                            alert.Price = order2.RiskStopLevel;
                                            alert.SameBarParentOrderNumber = order2.OrderID;
                                            alerts.Add(alert);
                                        }
                                        if (order2.AutoProfitLevel > 0.0)
                                        {
                                            this.method_2("Create same bar limit order");
                                            Alert alert2 = new Alert();
                                            this.method_7(alert2, order2);
                                            alert2.AlertType = (order2.AlertType == TradeType.Buy) ? TradeType.Sell : TradeType.Cover;
                                            alert2.OrderType = OrderType.Limit;
                                            alert2.Price = order2.AutoProfitLevel;
                                            alert2.SameBarParentOrderNumber = order2.OrderID;
                                            alerts.Add(alert2);
                                        }
                                        this.AddAlerts(alerts, true, true);
                                    }
                                }
                            }
                        }
                        flag2 = true;
                        if (stateIndex >= 0)
                        {
                            order2.StateIndex = stateIndex;
                        }
                        if (this.eventHandler_1 != null)
                        {
                            this.eventHandler_1(this, new OrderEventArgs(order2));
                        }
                        if ((status == OrderStatus.Filled) || (status == OrderStatus.PartialFilled))
                        {
                            this.method_2("Add local trade history item");
                            bool flag3 = false;
                            HistoricalTrade trade = this.FindHistoricalTrade(order2.OrderID);
                            if (trade == null)
                            {
                                trade = new HistoricalTrade(order2);
                                lock (this.list_3)
                                {
                                    this.list_3.Add(trade);
                                }
                                flag3 = true;
                            }
                            trade.TimeStamp = timestamp;
                            trade.Quantity = fillQty;
                            trade.Price = fillPrice;
                            if (flag3)
                            {
                                if (this.eventHandler_6 != null)
                                {
                                    this.eventHandler_6(this, new HistoricalTradeEventArgs(trade));
                                }
                            }
                            else if (this.eventHandler_5 != null)
                            {
                                this.eventHandler_5(this, new HistoricalTradeEventArgs(trade));
                            }
                            this.bool_3 = true;
                        }
                        if (status == OrderStatus.Canceled)
                        {
                            for (int j = this.list_4.Count - 1; j >= 0; j--)
                            {
                                OrderCancelSubmitGroup group = this.list_4[j];
                                if (group.CancelPending.Contains(order2))
                                {
                                    this.method_2("Process CancelSubmitGroup");
                                    group.CancelPending.Remove(order2);
                                    if (group.CancelPending.Count == 0)
                                    {
                                        this.method_2("Submit new CSG orders");
                                        this.list_4.RemoveAt(j);
                                        foreach (Order order3 in group.SubmitPending)
                                        {
                                            lock (this.list_0)
                                            {
                                                this.list_0.Add(order3);
                                            }
                                            order3.Status = OrderStatus.Submitted;
                                            if (this.eventHandler_2 != null)
                                            {
                                                this.eventHandler_2(this, new OrderEventArgs(order3));
                                            }
                                        }
                                        foreach (Order order4 in group.SubmitPending)
                                        {
                                            list.Add(order4);
                                        }
                                        flag2 = true;
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
                if (flag2)
                {
                    this.UpdateOrders();
                    this.bool_4 = true;
                }
                this.method_2("");
            }
            foreach (Order order in list)
            {
                this.BrokerProvider.PlaceOrder(order);
            }
        }

        public void PlaceOrder(List<Order> orders)
        {
            List<Order> list = new List<Order>();
            foreach (Order order in orders)
            {
                if (this.method_8(order))
                {
                    list.Add(order);
                    order.Status = OrderStatus.Submitted;
                }
                if (this.eventHandler_1 != null)
                {
                    this.eventHandler_1(this, new OrderEventArgs(order));
                }
            }
            if (list.Count > 0)
            {
                this.BrokerProvider.PlaceOrder(list);
            }
            this.bool_4 = true;
            this.UpdateOrders();
        }

        public void PlaceOrder(Order order)
        {
            order.Status = OrderStatus.Submitted;
            if (this.eventHandler_1 != null)
            {
                this.eventHandler_1(this, new OrderEventArgs(order));
            }
            this.BrokerProvider.PlaceOrder(order);
            this.bool_4 = true;
            this.UpdateOrders();
        }

        public void QuoteUpdate(Quote quote)
        {
            if (this.eventHandler_12 != null)
            {
                this.eventHandler_12(this, new QuoteEventArgs(quote));
            }
        }

        public void RemoveCompleted(string acctNum)
        {
            lock (this.list_0)
            {
                for (int i = this.list_0.Count - 1; i >= 0; i--)
                {
                    if (this.list_0[i].IsCompleted && ((acctNum == "") || (this.list_0[i].Account == acctNum)))
                    {
                        this.list_0.RemoveAt(i);
                    }
                }
            }
            this.UpdateOrders();
            this.bool_4 = true;
        }

        public void RemoveOrders(List<Order> orders)
        {
            lock (this.list_0)
            {
                foreach (Order order in orders)
                {
                    if (this.list_0.Contains(order))
                    {
                        this.list_0.Remove(order);
                    }
                }
            }
            this.UpdateOrders();
            this.bool_4 = true;
        }

        public void SaveOrders()
        {
            lock (this.list_0)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Order>));
                TextWriter textWriter = new StreamWriter(this.string_1);
                try
                {
                    serializer.Serialize(textWriter, this.list_0);
                }
                finally
                {
                    textWriter.Close();
                }
            }
        }

        public void SaveTradeHistory()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<HistoricalTrade>));
            TextWriter textWriter = new StreamWriter(this.string_2);
            try
            {
                lock (this.list_3)
                {
                    serializer.Serialize(textWriter, this.list_3);
                }
            }
            finally
            {
                textWriter.Close();
            }
        }

        public void UpdateOrders()
        {
            lock (this.list_0)
            {
                this.int_0 = this.list_0.Count;
                this.int_1 = 0;
                foreach (Order order in this.list_0)
                {
                    if (order.IsActive)
                    {
                        this.int_1++;
                    }
                }
            }
            if (this.eventHandler_0 != null)
            {
                this.eventHandler_0(this, EventArgs.Empty);
            }
        }

        public int ActiveOrderCount
        {
            get
            {
                return this.int_1;
            }
        }

        public bool AlwaysExitAllSharesInPosition
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public AutoTradingMode AutoTradingEnabled
        {
            get
            {
                return this.autoTradingMode_0;
            }
            set
            {
                this.autoTradingMode_0 = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WealthLab.BrokerProvider BrokerProvider
        {
            get
            {
                return this.brokerProvider_0;
            }
            set
            {
                this.brokerProvider_0 = value;
            }
        }

        public double BuyingPowerThreshold
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

        public double CashThreshold
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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DefaultAccountNumber
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

        public static bool DisablePortfolioSynch
        {
            get
            {
                return bool_6;
            }
            set
            {
                bool_6 = value;
            }
        }

        public static string DisablePortfolioSynchKey
        {
            get
            {
                return "DisablePortfolioSynch";
            }
        }

        public bool EnableBuyingPowerThreshold
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }

        public bool EnableCashThreshold
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int OrderCount
        {
            get
            {
                return this.int_0;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Order> Orders
        {
            get
            {
                return this.list_0;
            }
        }

        public string RootPath
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
                if (value != null)
                {
                    if ((this.string_0.Length > 0) && (this.string_0[this.string_0.Length - 1] != '\\'))
                    {
                        this.string_0 = this.string_0 + @"\";
                    }
                    this.string_1 = this.string_0 + "Orders.xml";
                    this.string_2 = this.string_0 + "TradeHistory.xml";
                }
            }
        }

        public bool SameBarExits
        {
            get
            {
                return this.bool_5;
            }
            set
            {
                this.bool_5 = value;
            }
        }

        public ISettingsHost Settings
        {
            get
            {
                return this.isettingsHost_0;
            }
        }

        public ISettingsHost SettingsHost
        {
            get
            {
                return this.isettingsHost_0;
            }
            set
            {
                this.isettingsHost_0 = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<HistoricalTrade> TradeHistory
        {
            get
            {
                return this.list_3;
            }
        }
    }
}

