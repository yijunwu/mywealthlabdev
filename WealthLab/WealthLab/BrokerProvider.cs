namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public abstract class BrokerProvider
    {
        private AuthenticationProvider authenticationProvider_0;
        private IBrokerHost ibrokerHost_0;
        private List<Account> list_0 = new List<Account>();
        private List<string> list_1 = new List<string>();

        protected BrokerProvider()
        {
        }

        public virtual void AccountPositionsMofidied(Account account)
        {
        }

        public abstract List<string> AccountTradeTypesAllowed(string acctNumber, string action);
        public virtual bool AllowCancelReplace(Order order, string bracketConditional)
        {
            return false;
        }

        public virtual bool AllowDecimalsForExtendedOrderType(string orderType)
        {
            return true;
        }

        public virtual bool AllowOrderTypeForRoute(string route, OrderType orderType)
        {
            return true;
        }

        public abstract void CancelOrder(IList<Order> orders);
        public abstract void CancelOrder(Order order);
        public abstract void CancelReplace(Order order, Order newOrder);
        public virtual IList<string> CancelReplaceOrderTypesAllowed(Order order)
        {
            if (this.list_1.Count == 0)
            {
                this.list_1.Add("Market");
                this.list_1.Add("Limit");
                this.list_1.Add("Stop");
            }
            return this.list_1;
        }

        public virtual DateTime ConvertToMarketTimeZone(string symbol, DateTime dateTime_0)
        {
            return dateTime_0;
        }

        public abstract List<string> ExtendedOrderTypesAllowed(string acctNum, string route, string action);
        public Account FindAccount(string acctNum)
        {
            Account account2;
            using (IEnumerator<Account> enumerator = this.Accounts.GetEnumerator())
            {
                Account current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.AccountNumber == acctNum)
                    {
                        goto Label_002E;
                    }
                }
                return null;
            Label_002E:
                account2 = current;
            }
            return account2;
        }

        protected abstract List<Account> GetAccounts();
        public virtual string GetPositionAccountTradeType(AccountPosition acctPos)
        {
            return acctPos.AcctType;
        }

        public virtual Quote GetQuote(string symbol)
        {
            return null;
        }

        public virtual void GetStateImages(ImageList imageList_0)
        {
        }

        public virtual void Initialize(IBrokerHost brokerHost, AuthenticationProvider authProvider)
        {
            this.ibrokerHost_0 = brokerHost;
            this.authenticationProvider_0 = authProvider;
        }

        public abstract void PlaceOrder(IList<Order> orders);
        public abstract void PlaceOrder(Order order);
        public abstract void RequestOrderStatusUpdates(Account acct);
        public abstract void RequestOrderStatusUpdatesForOrders(IList<Order> orders);
        public void RequestUpdates()
        {
            this.list_0 = this.GetAccounts();
        }

        public abstract string RouteForStrategyOrders(BarDataScale scale);
        public virtual IList<string> RoutesForAccountNumber(string acctNumber)
        {
            return this.Routes;
        }

        public abstract string TifForStrategyOrders(BarDataScale scale);
        public abstract List<string> TifsAllowed(string acctNumber, string route, string orderType);
        public abstract void UpdateAccounts();

        public virtual IList<Account> Accounts
        {
            get
            {
                return this.list_0;
            }
        }

        public AuthenticationProvider AuthProvider
        {
            get
            {
                return this.authenticationProvider_0;
            }
        }

        public IBrokerHost BrokerHost
        {
            get
            {
                return this.ibrokerHost_0;
            }
        }

        public abstract IList<string> Routes { get; }
    }
}

