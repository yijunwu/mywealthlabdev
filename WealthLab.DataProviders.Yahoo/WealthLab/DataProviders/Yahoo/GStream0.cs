namespace WealthLab.DataProviders.Yahoo
{
    using System;
    using System.Drawing;
    using WealthLab;
    using WealthLab.DataProviders.Yahoo.Properties;

    public class GStream0 : StreamingDataProvider
    {
        private bool connected;
        private DataFetcher dataFetcher;
        private IConnectionStatus connStatus;
        private YahooStaticProvider yahooStaticProvider;

        public override void ConnectStreaming(IConnectionStatus connStatus)
        {
            this.connStatus = connStatus;
            this.createDataFetcher(true, this.connStatus);
        }

        public override void DisconnectStreaming()
        {
            this.connected = false;
            base.DisconnectStreaming();
        }

        public Quote GetQuote(string symbol)
        {
            this.createDataFetcher(false, null);
            return this.dataFetcher.GetRealTimeQuoteForSymbol(symbol);
        }

        public override StaticDataProvider GetStaticProvider()
        {
            if (this.yahooStaticProvider == null)
            {
                this.yahooStaticProvider = new YahooStaticProvider();
                this.yahooStaticProvider.Initialize(base.DataHost);
            }
            return this.yahooStaticProvider;
        }

        private void updateStatus()
        {
            this.connStatus.StatusUpdate(ConnStatus.OK, 0, this.dataFetcher.GetSubscribedSymbolCount() + " Symbols Subscribed");
        }

        ///WYJ fix, original name: method_2
        private void createDataFetcher(bool startStreaming, IConnectionStatus iconnectionStatus_2)
        {
            if (!this.connected)
            {
                this.connected = true;
                this.dataFetcher = new DataFetcher();
                this.dataFetcher.AddStreamingDataHandler(new DataFetcher.StreamingDataHandler(this.onData));
                this.dataFetcher.AddStreamingErrorHandler(new DataFetcher.StreamingErrorHandler(this.onError));
                this.dataFetcher.login();
                if (startStreaming)
                {
                    this.dataFetcher.startStreamingRequesterAndProcessor();
                }
                if (iconnectionStatus_2 != null)
                {
                    iconnectionStatus_2.Connect();
                }
            }
        }

        private void onError(object sender, StreamingErrorEventArgs e)
        {
            base.ConnectionStatus.StatusUpdate(ConnStatus.Error, 0, e.errorMsg);
        }

        private void onData(object sender, StreamingDataEventArgs e)
        {
            if ((((e.quote.Price != 0.0) && (e.high != 0.0)) && (e.low != 0.0)) && (e.open != 0.0))
            {
                Logger.LogWithStackTrace(new object[] { e.quote.TimeStamp.ToString(), e.quote.Symbol, e.quote.Price, e.quote.Size, e.open, e.high, e.low });
                base.UpdateMiniBar(e.quote, e.open, e.high, e.low);
            }
        }

        protected override void Subscribe(string symbol)
        {
            Logger.LogParameters(new object[] { symbol });
            if (this.connected && (symbol != string.Empty))
            {
                this.dataFetcher.subscribeSymbol(symbol.ToUpper());
                this.updateStatus();
            }
        }

        protected override void UnSubscribe(string symbol)
        {
            Logger.LogParameters(new object[] { symbol });
            if (this.connected && (symbol != string.Empty))
            {
                this.dataFetcher.unsubscribeSymbol(symbol.ToUpper());
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

