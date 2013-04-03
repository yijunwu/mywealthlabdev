namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(StreamingQuoteManager), "StreamingQuoteManager")]
    public class StreamingQuoteManager : Component, IStreamingUpdate
    {
        private bool bool_0;
        private Dictionary<string, Quote> dictionary_0;
        private IConnectionStatus iconnectionStatus_0;
        private IContainer icontainer_0;
        private StreamingDataProvider streamingDataProvider_0;

        public StreamingQuoteManager()
        {
            this.dictionary_0 = new Dictionary<string, Quote>();
            this.method_0();
        }

        public StreamingQuoteManager(IContainer container)
        {
            this.dictionary_0 = new Dictionary<string, Quote>();
            container.Add(this);
            this.method_0();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public Quote GetLastQuote(string symbol)
        {
            if (!this.dictionary_0.ContainsKey(symbol))
            {
                return null;
            }
            return this.dictionary_0[symbol];
        }

        public void Heartbeat(DateTime timeStamp)
        {
        }

        public void Heartbeat(DateTime timeStamp, MarketInfo marketInfo)
        {
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        public void Subscribe(string symbol)
        {
            if (this.streamingDataProvider_0 != null)
            {
                this.streamingDataProvider_0.Subscribe(symbol, this);
            }
        }

        public void Unsubscribe(string symbol)
        {
            if (this.streamingDataProvider_0 != null)
            {
                this.streamingDataProvider_0.UnSubscribe(symbol, this);
            }
        }

        public void UpdateMiniBar(Quote quote_0, double open, double high, double double_0)
        {
            this.UpdateQuote(quote_0);
        }

        public void UpdateQuote(Quote quote_0)
        {
            this.bool_0 = true;
            if (!this.dictionary_0.ContainsKey(quote_0.Symbol))
            {
                this.dictionary_0.Add(quote_0.Symbol, quote_0);
            }
            else
            {
                this.dictionary_0[quote_0.Symbol] = quote_0;
            }
        }

        public void UpdateStreamingBar(string symbol, int barInterval, double open, double high, double double_0, double close, double volume, DateTime timeStamp, string debugInfo)
        {
            Quote quote = new Quote {
                Symbol = symbol,
                Open = open,
                Price = close,
                Size = volume,
                TimeStamp = timeStamp
            };
            this.UpdateQuote(quote);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IConnectionStatus ConnectionStatus
        {
            get
            {
                return this.iconnectionStatus_0;
            }
            set
            {
                this.iconnectionStatus_0 = value;
            }
        }

        public bool FreshQuoteReady
        {
            get
            {
                bool flag = this.bool_0;
                this.bool_0 = false;
                return flag;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StreamingDataProvider Provider
        {
            get
            {
                return this.streamingDataProvider_0;
            }
            set
            {
                if (this.ConnectionStatus == null)
                {
                    throw new InvalidOperationException("ConnectionStatus must be set before assigning Provider");
                }
                this.streamingDataProvider_0 = value;
                if (this.streamingDataProvider_0 != null)
                {
                    this.streamingDataProvider_0.ConnectStreaming(this.ConnectionStatus);
                }
            }
        }
    }
}

