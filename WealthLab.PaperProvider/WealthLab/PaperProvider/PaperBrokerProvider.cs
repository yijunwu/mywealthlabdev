namespace WealthLab.PaperProvider
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using WealthLab;

    public class PaperBrokerProvider : BrokerProvider, ICustomSettings
    {
        private List<Account> _accounts = new List<Account>();
        private string _dataPath = "";
        private DateTime _delayedOpenTime = DateTime.MinValue;
        private List<string> _empty = new List<string>();
        private bool _firstPass;
        private MarketHours _hours = new MarketHours();
        private DateTime _lastProcessed = DateTime.MinValue;
        private List<Order> _orders = new List<Order>();
        private Dictionary<string, Quote> _quotes = new Dictionary<string, Quote>();
        private List<string> _routes = new List<string>();
        private List<Order> _tempOrders = new List<Order>();
        private List<string> _tifs = new List<string>();

        public override void AccountPositionsMofidied(Account account)
        {
            this.SaveAccounts();
            base.BrokerHost.AccountPositionsUpdate(account);
        }

        public override List<string> AccountTradeTypesAllowed(string acctNumber, string action)
        {
            return new List<string> { "" };
        }

        private void ActivateOrders()
        {
            lock (this._orders)
            {
                foreach (Order order in this._orders)
                {
                    if ((order.Status == OrderStatus.Submitted) || (order.Status == OrderStatus.Unknown))
                    {
                        base.BrokerHost.OrderStatusUpdate(order.OrderID, OrderStatus.Active, DateTime.Now, 0.0, 0.0, 0, "");
                    }
                }
            }
        }

        private void ActivateUnknownOrders()
        {
            foreach (Account account in this._accounts)
            {
                List<Order> orders = base.BrokerHost.GetOrders(account);
                if (orders.Count > 0)
                {
                    List<Order> list2 = new List<Order>();
                    foreach (Order order in orders)
                    {
                        if (order.Status == OrderStatus.Unknown)
                        {
                            list2.Add(order);
                        }
                    }
                    if (list2.Count > 0)
                    {
                        this.RequestOrderStatusUpdatesForOrders(list2);
                    }
                }
            }
        }

        public override bool AllowCancelReplace(Order order, string bracketConditional)
        {
            return false;
        }

        private void AttemptToFillOrder(Order order, double price, DateTime fillDate)
        {
            Account account = base.FindAccount(order.Account);
            if (account != null)
            {
                if (order.Strategy == null)
                {
                    order.AlertDate = fillDate;
                }
                if ((order.AlertType == TradeType.Buy) || (order.AlertType == TradeType.Short))
                {
                    double num = order.Shares * price;
                    if (account.BuyingPower < num)
                    {
                        base.BrokerHost.OrderStatusUpdate(order.OrderID, OrderStatus.Error, fillDate, 0.0, 0.0, 2, "Insufficient Buying Power to complete Trade");
                        return;
                    }
                }
                else
                {
                    AccountPosition position = this.FindPosition(order.Account, order.Symbol, (order.AlertType == TradeType.Sell) ? PositionType.Long : PositionType.Short);
                    double num2 = (position == null) ? 0.0 : position.Quantity;
                    if (order.Shares > num2)
                    {
                        base.BrokerHost.OrderStatusUpdate(order.OrderID, OrderStatus.Error, fillDate, 0.0, 0.0, 3, "Attempting to Sell/Cover more Shares than held in Account");
                        return;
                    }
                }
                this.OrderFilled(order, price);
                base.BrokerHost.OrderStatusUpdate(order.OrderID, OrderStatus.Filled, fillDate, price, order.Shares, 0, "");
                this.SaveAccounts();
                base.BrokerHost.AccountBalanceUpdate(account);
                base.BrokerHost.AccountPositionsUpdate(account);
            }
        }

        private void CancelOldOrders()
        {
            lock (this._orders)
            {
                DateTime lastTradingSessionEndedNative = this._hours.LastTradingSessionEndedNative;
                foreach (Order order in this._orders)
                {
                    if ((order.Status == OrderStatus.Active) && (order.AlertDate < lastTradingSessionEndedNative))
                    {
                        bool flag = true;
                        if (((order.AlertDate.DayOfWeek == DayOfWeek.Saturday) || (order.AlertDate.DayOfWeek == DayOfWeek.Sunday)) || (order.AlertDate.TimeOfDay > this._hours.MarketCloseTimeLocal.TimeOfDay))
                        {
                            DateTime time2 = order.AlertDate.Date.AddDays(1.0);
                            while (!this._hours.IsTradingDay(time2))
                            {
                                time2 = time2.AddDays(1.0);
                            }
                            if (DateTime.Now.Date > time2)
                            {
                                Bars bars = this.RequestHistoricalData(order.Symbol, time2, time2.AddDays(1.0));
                                if ((bars != null) && (bars.Count > 0))
                                {
                                    DateTime time10 = bars.Date[0];
                                    if (time10.Date == time2)
                                    {
                                        if (order.OrderType == OrderType.Market)
                                        {
                                            flag = false;
                                            this.AttemptToFillOrder(order, bars.Open[0], time2.Date + this._hours.MarketOpenTimeLocal.TimeOfDay);
                                        }
                                        else
                                        {
                                            bool flag2 = false;
                                            if (order.OrderType == OrderType.Limit)
                                            {
                                                flag2 = (order.AlertType == TradeType.Sell) || (order.AlertType == TradeType.Short);
                                            }
                                            else
                                            {
                                                flag2 = (order.AlertType == TradeType.Buy) || (order.AlertType == TradeType.Cover);
                                            }
                                            bool flag3 = false;
                                            if (flag2)
                                            {
                                                flag3 = bars.Open[0] >= order.Price;
                                            }
                                            else
                                            {
                                                flag3 = bars.Open[0] <= order.Price;
                                            }
                                            if (flag3)
                                            {
                                                flag = false;
                                                this.AttemptToFillOrder(order, bars.Open[0], time2.Date + this._hours.MarketOpenTimeLocal.TimeOfDay);
                                            }
                                            else
                                            {
                                                bool flag4 = false;
                                                if (flag2)
                                                {
                                                    flag4 = bars.High[0] >= order.Price;
                                                }
                                                else
                                                {
                                                    flag4 = bars.Low[0] <= order.Price;
                                                }
                                                if (flag4)
                                                {
                                                    flag = false;
                                                    this.AttemptToFillOrder(order, order.Price, time2.Date + this._hours.MarketOpenTimeLocal.TimeOfDay);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (flag)
                        {
                            base.BrokerHost.OrderStatusUpdate(order.OrderID, OrderStatus.Canceled, DateTime.Now, 0.0, 0.0, 0, "Market session ended, order canceled");
                        }
                    }
                }
            }
        }

        public override void CancelOrder(IList<Order> orders)
        {
            foreach (Order order in orders)
            {
                this.CancelOrder(order);
            }
        }

        public override void CancelOrder(Order order)
        {
            base.BrokerHost.OrderStatusUpdate(order.OrderID, OrderStatus.Canceled, DateTime.Now, 0.0, 0.0, 0, "");
        }

        public override void CancelReplace(Order order, Order newOrder)
        {
        }

        public override IList<string> CancelReplaceOrderTypesAllowed(Order order)
        {
            return this._empty;
        }

        public void ChangeSettings(UserControl ui)
        {
            PaperBrokerProviderSettings settings = ui as PaperBrokerProviderSettings;
            List<Account> accounts = settings.Accounts;
            this.Accounts.Clear();
            foreach (Account account in accounts)
            {
                this.Accounts.Add(account);
            }
        }

        public override List<string> ExtendedOrderTypesAllowed(string acctNumber, string route, string action)
        {
            return this._empty;
        }

        private void FillMarketOrders()
        {
            this._tempOrders.Clear();
            lock (this._orders)
            {
                foreach (Order order in this._orders)
                {
                    this._tempOrders.Add(order);
                }
            }
            foreach (Order order2 in this._tempOrders)
            {
                if ((order2.Status == OrderStatus.Active) && (order2.OrderType == OrderType.Market))
                {
                    Quote quote = this.GetQuote(order2.Symbol);
                    if ((quote == null) || (quote.Price == 0.0))
                    {
                        base.BrokerHost.OrderStatusUpdate(order2.OrderID, OrderStatus.Error, DateTime.Now, 0.0, 0.0, 1, "Could not get Quote for symbol: " + order2.Symbol);
                    }
                    else
                    {
                        double price = quote.Price;
                        DateTime timeStamp = quote.TimeStamp;
                        if (this._firstPass)
                        {
                            Bars bars = this.RequestHistoricalData(order2.Symbol, order2.AlertDate.Date.AddDays(1.0), DateTime.Now.Date.AddDays(1.0));
                            if ((bars != null) && (bars.Count > 0))
                            {
                                timeStamp = bars.Date[0] + this._hours.MarketOpenTimeLocal.TimeOfDay;
                                price = bars.Open[0];
                            }
                        }
                        this.AttemptToFillOrder(order2, price, timeStamp);
                    }
                }
            }
        }

        private Account FindPaperAccount(string acctNumber)
        {
            foreach (Account account in this._accounts)
            {
                if (account.AccountNumber == acctNumber)
                {
                    return account;
                }
            }
            return null;
        }

        private AccountPosition FindPosition(string acctNum, string symbol, PositionType pt)
        {
            foreach (Account account in this._accounts)
            {
                if (account.AccountNumber == acctNum)
                {
                    foreach (AccountPosition position in account.Positions)
                    {
                        if ((position.Symbol == symbol) && (position.PositionType == pt))
                        {
                            return position;
                        }
                    }
                }
            }
            return null;
        }

        protected override List<Account> GetAccounts()
        {
            this._accounts.Clear();
            string path = this._dataPath + @"\Accounts.xml";
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Account>));
                TextReader textReader = new StreamReader(path);
                try
                {
                    this._accounts = (List<Account>) serializer.Deserialize(textReader);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                textReader.Close();
                foreach (Account account in this._accounts)
                {
                    account.AccountValueTimeStamp = DateTime.Now;
                    foreach (AccountPosition position in account.Positions)
                    {
                        position.Account = account;
                    }
                }
            }
            if (!base.BrokerHost.Settings.Get("PaperAccts", false))
            {
                base.BrokerHost.Settings.Set("PaperAccts", true);
                string str2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\PaperAccounts.txt";
                if (File.Exists(str2))
                {
                    foreach (string str3 in File.ReadAllLines(str2))
                    {
                        string[] strArray2 = str3.Split(new char[] { ',' });
                        Account item = new Account {
                            AccountValueTimeStamp = DateTime.Now,
                            AccountNumber = strArray2[0]
                        };
                        if (this.FindPaperAccount(item.AccountNumber) == null)
                        {
                            item.BuyingPower = double.Parse(strArray2[1]);
                            item.AvailableCash = double.Parse(strArray2[2]);
                            item.IsPaperAccount = true;
                            this._accounts.Add(item);
                        }
                    }
                    this.SaveAccounts();
                }
            }
            return this._accounts;
        }

        public override Quote GetQuote(string symbol)
        {
            return null;
        }

        public UserControl GetSettingsUI()
        {
            PaperBrokerProviderSettings settings = new PaperBrokerProviderSettings();
            List<Account> list = new List<Account>();
            foreach (Account account in this.Accounts)
            {
                list.Add(account);
            }
            settings.Accounts = list;
            return settings;
        }

        public override void Initialize(IBrokerHost brokerHost, AuthenticationProvider authProvider)
        {
            base.Initialize(brokerHost, authProvider);
            this._dataPath = authProvider.DataHost.BaseDataFolder;
            this._routes.Add("Auto");
            this._tifs.Add("Day");
            new Thread(new ThreadStart(this.ProcessingThreadExecute)) { IsBackground = true, Name = "Order Processing" }.Start();
        }

        private void OrderFilled(Order order, double fillPrice)
        {
            foreach (Account account in this._accounts)
            {
                if (account.AccountNumber == order.Account)
                {
                    account.AccountValueTimeStamp = DateTime.Now;
                    double num = order.Shares * fillPrice;
                    if ((order.AlertType == TradeType.Buy) || (order.AlertType == TradeType.Short))
                    {
                        account.AvailableCash -= num;
                        account.BuyingPower -= num;
                        this.SynchParentAcct(account);
                    }
                    else
                    {
                        AccountPosition position = this.FindPosition(order.Account, order.Symbol, (order.AlertType == TradeType.Sell) ? PositionType.Long : PositionType.Short);
                        if (order.AlertType == TradeType.Sell)
                        {
                            account.AvailableCash += num;
                            account.BuyingPower += num;
                            this.SynchParentAcct(account);
                            double num2 = fillPrice - position.EntryPrice;
                            num2 *= order.Shares;
                            account.AccountValue += num2;
                        }
                        else
                        {
                            double num3 = position.EntryPrice * order.Shares;
                            account.AvailableCash += num3;
                            account.BuyingPower += num3;
                            this.SynchParentAcct(account);
                            double num4 = position.EntryPrice - fillPrice;
                            num4 *= order.Shares;
                            account.AvailableCash += num4;
                            account.BuyingPower += num4;
                            account.AccountValue += num4;
                            this.SynchParentAcct(account);
                        }
                    }
                    break;
                }
            }
        }

        public override void PlaceOrder(IList<Order> orders)
        {
            foreach (Order order in orders)
            {
                this.PlaceOrder(order);
            }
        }

        public override void PlaceOrder(Order order)
        {
            if (order.Strategy != null)
            {
                order.AlertDate = DateTime.Now;
            }
            order.OrderID = Guid.NewGuid().ToString();
            lock (this._orders)
            {
                this._orders.Add(order);
            }
        }

        private void ProcessingThreadExecute()
        {
        ///Label_0000: ///WYJ fix, simplify the flow
            while (true)
            {
                Thread.Sleep(500);
                this.ActivateUnknownOrders();
                if (this._orders.Count > 0)
                {
                    this.ActivateOrders();
                    this.CancelOldOrders();
                    if (!this.IsMarketOpenNow)
                    {
                        continue;
                    }
                    this.FillMarketOrders();
                    TimeSpan span = (TimeSpan)(DateTime.Now - this._lastProcessed);
                    if (span.TotalSeconds > this.PollingInterval)
                    {
                        this.ProcessStopLimitOrders();
                    }
                }
                this._firstPass = false;
            }
        }

        private void ProcessStopLimitOrders()
        {
            this._lastProcessed = DateTime.Now;
            foreach (Order order in this._tempOrders)
            {
                if ((order.Status != OrderStatus.Active) || ((order.OrderType != OrderType.Limit) && (order.OrderType != OrderType.Stop)))
                {
                    continue;
                }
                bool flag = true;
                if (order.OrderType == OrderType.Limit)
                {
                    switch (order.AlertType)
                    {
                        case TradeType.Buy:
                        case TradeType.Cover:
                            flag = false;
                            break;

                        case TradeType.Sell:
                        case TradeType.Short:
                            flag = true;
                            break;
                    }
                }
                else
                {
                    switch (order.AlertType)
                    {
                        case TradeType.Buy:
                        case TradeType.Cover:
                            flag = true;
                            break;

                        case TradeType.Sell:
                        case TradeType.Short:
                            flag = false;
                            break;
                    }
                }
                Quote quote = this.GetQuote(order.Symbol);
                if ((quote != null) && (quote.Price != 0.0))
                {
                    bool flag2 = flag ? (quote.Price >= order.Price) : (quote.Price <= order.Price);
                    double price = quote.Price;
                    DateTime timeStamp = quote.TimeStamp;
                    if (this._firstPass)
                    {
                        Bars bars = this.RequestHistoricalData(order.Symbol, order.AlertDate.Date.AddDays(1.0), DateTime.Now.Date.AddDays(1.0));
                        if ((bars != null) && (bars.Count > 0))
                        {
                            if (flag ? (bars.Open[0] >= order.Price) : (bars.Open[0] <= order.Price))
                            {
                                flag2 = true;
                                price = bars.Open[0];
                                timeStamp = bars.Date[0] + this._hours.MarketOpenTimeLocal.TimeOfDay;
                                break;
                            }
                            if (flag ? (bars.High[0] >= order.FillPrice) : (bars.Low[0] <= order.FillPrice))
                            {
                                flag2 = true;
                                price = order.Price;
                                timeStamp = bars.Date[0] + this._hours.MarketOpenTimeLocal.TimeOfDay;
                                break;
                            }
                        }
                    }
                    if (flag2)
                    {
                        this.AttemptToFillOrder(order, price, timeStamp);
                    }
                    this._quotes[order.Symbol] = quote;
                }
            }
        }

        public void ReadSettings(ISettingsHost host)
        {
        }

        public virtual Bars RequestHistoricalData(string symbol, DateTime startDate, DateTime endDate)
        {
            return null;
        }

        public override void RequestOrderStatusUpdates(Account acct)
        {
        }

        public override void RequestOrderStatusUpdatesForOrders(IList<Order> orders)
        {
            lock (this._orders)
            {
                foreach (Order order in orders)
                {
                    if (!this._orders.Contains(order))
                    {
                        this._firstPass = true;
                        this._orders.Add(order);
                    }
                }
            }
        }

        public override string RouteForStrategyOrders(BarDataScale scale)
        {
            return "Auto";
        }

        private void SaveAccounts()
        {
            foreach (Account account in this._accounts)
            {
                double num = 0.0;
                foreach (AccountPosition position in account.Positions)
                {
                    double num2 = position.EntryPrice * position.Quantity;
                    double num3 = position.LastPrice - position.EntryPrice;
                    if (position.PositionType == PositionType.Short)
                    {
                        num3 = -num3;
                    }
                    num3 *= position.Quantity;
                    num += num2;
                    num += num3;
                }
                account.AccountValue = account.BuyingPower + num;
            }
            string path = this._dataPath + @"\Accounts.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<Account>));
            TextWriter textWriter = new StreamWriter(path);
            try
            {
                serializer.Serialize(textWriter, this._accounts);
            }
            finally
            {
                textWriter.Close();
            }
        }

        private void SynchParentAcct(Account acct)
        {
            if (this.ParentProvider != null)
            {
                foreach (Account account in this.ParentProvider.Accounts)
                {
                    if (account.AccountNumber == acct.AccountNumber)
                    {
                        account.AvailableCash = acct.AvailableCash;
                        account.BuyingPower = acct.BuyingPower;
                    }
                }
            }
        }

        public override string TifForStrategyOrders(BarDataScale scale)
        {
            return "Day";
        }

        public override List<string> TifsAllowed(string acctNumber, string route, string orderType)
        {
            return this._tifs;
        }

        public override void UpdateAccounts()
        {
            foreach (Account account in this._accounts)
            {
                foreach (AccountPosition position in account.Positions)
                {
                    try
                    {
                        Quote quote = this.GetQuote(position.Symbol);
                        if (quote != null)
                        {
                            position.LastPrice = quote.Price;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            this.SaveAccounts();
            foreach (Account account2 in this._accounts)
            {
                base.BrokerHost.AccountBalanceUpdate(account2);
                base.BrokerHost.AccountPositionsUpdate(account2);
            }
        }

        public void WriteSettings(ISettingsHost host)
        {
            this.SaveAccounts();
        }

        private bool IsMarketOpenNow
        {
            get
            {
                if ((base.AuthProvider != null) && base.AuthProvider.LoggedIn)
                {
                    return this._hours.IsMarketOpenNow;
                }
                if (this._delayedOpenTime == DateTime.MinValue)
                {
                    this._delayedOpenTime = this._hours.MarketOpenTimeLocal.AddMinutes(20.0);
                }
                return (this._hours.IsMarketOpenNow && (DateTime.Now > this._delayedOpenTime));
            }
        }

        public BrokerProvider ParentProvider { get; set; }

        public virtual int PollingInterval
        {
            get
            {
                return 10;
            }
        }

        public override IList<string> Routes
        {
            get
            {
                return this._routes;
            }
        }
    }
}

