namespace WealthLab.DataProviders.Yahoo
{
    using System;
    using System.Drawing;
    using WealthLab;
    using WealthLab.DataProviders.Yahoo.Properties;

    public class GStream0 : StreamingDataProvider
    {
        private bool bool_1;
        private DataFetcher class26_0;
        private IConnectionStatus iconnectionStatus_1;
        private YahooStaticProvider yahooStaticProvider_0;

        public override void ConnectStreaming(IConnectionStatus connStatus)
        {
            this.iconnectionStatus_1 = connStatus;
            this.method_2(true, this.iconnectionStatus_1);
        }

        public override void DisconnectStreaming()
        {
            this.bool_1 = false;
            base.DisconnectStreaming();
        }

        public Quote GetQuote(string symbol)
        {
            this.method_2(false, null);
            return this.class26_0.GetRealTimeQuoteForSymbol(symbol);
        }

        public override StaticDataProvider GetStaticProvider()
        {
            if (this.yahooStaticProvider_0 == null)
            {
                this.yahooStaticProvider_0 = new YahooStaticProvider();
                this.yahooStaticProvider_0.Initialize(base.DataHost);
            }
            return this.yahooStaticProvider_0;
        }

        private void updateStatus()
        {
            this.iconnectionStatus_1.StatusUpdate(ConnStatus.OK, 0, this.class26_0.GetSubscribedSymbolCount() + " Symbols Subscribed");
        }

        private void method_2(bool bool_2, IConnectionStatus iconnectionStatus_2)
        {
            if (!this.bool_1)
            {
                this.bool_1 = true;
                this.class26_0 = new DataFetcher();
                this.class26_0.AddStreamingDataHandler(new DataFetcher.Delegate3(this.method_4));
                this.class26_0.AddStreamingErrorHandler(new DataFetcher.Delegate4(this.onError));
                this.class26_0.login();
                if (bool_2)
                {
                    this.class26_0.startStreamingRequesterAndProcessor();
                }
                if (iconnectionStatus_2 != null)
                {
                    iconnectionStatus_2.Connect();
                }
            }
        }

        private void onError(object sender, EventArgs5 e)
        {
            base.ConnectionStatus.StatusUpdate(ConnStatus.Error, 0, e.string_0);
        }

        private void method_4(object sender, EventArgs4 e)
        {
            if ((((e.quote_0.Price != 0.0) && (e.double_1 != 0.0)) && (e.double_2 != 0.0)) && (e.double_0 != 0.0))
            {
                Logger.LogWithStackTrace(new object[] { e.quote_0.TimeStamp.ToString(), e.quote_0.Symbol, e.quote_0.Price, e.quote_0.Size, e.double_0, e.double_1, e.double_2 });
                base.UpdateMiniBar(e.quote_0, e.double_0, e.double_1, e.double_2);
            }
        }

        protected override void Subscribe(string symbol)
        {
            Logger.LogParameters(new object[] { symbol });
            if (this.bool_1 && (symbol != string.Empty))
            {
                this.class26_0.subscribeSymbol(symbol.ToUpper());
                this.updateStatus();
            }
        }

        protected override void UnSubscribe(string symbol)
        {
            Logger.LogParameters(new object[] { symbol });
            if (this.bool_1 && (symbol != string.Empty))
            {
                this.class26_0.unsubscribeSymbol(symbol.ToUpper());
                this.updateStatus();
            }
        }

        public override string Description
        {
            get
            {
                return "Streaming real-time stock quotes from Yahoo! Finance";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Yahoo! Finance";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Yahoo;
            }
        }

        public override bool IsConnected
        {
            get
            {
                return true;
            }
        }
    }
}

