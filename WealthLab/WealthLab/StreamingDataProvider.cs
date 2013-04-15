namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public abstract class StreamingDataProvider : IStreamingUpdate
    {
        private static bool bool_0 = false;
        private static Dictionary<StreamingRequest, int> dictionary_0 = new Dictionary<StreamingRequest, int>();
        private Dictionary<string, Quote> dictionary_1 = new Dictionary<string, Quote>();
        private Dictionary<string, Quote> dictionary_2 = new Dictionary<string, Quote>();
        private static double double_0 = 0.2;
        private static IConnectionStatus iconnectionStatus_0;
        private static IDataHost idataHost_0;
        private const int int_0 = 0x1388;
        private static List<StreamingBarRequest> list_0 = new List<StreamingBarRequest>();
        private MarketHours marketHours_0 = new MarketHours();

        protected StreamingDataProvider()
        {
        }

        public void ClearRequests(IStreamingUpdate requestor)
        {
            lock (dictionary_0)
            {
                List<StreamingRequest> list = new List<StreamingRequest>();
                foreach (StreamingRequest request3 in dictionary_0.Keys)
                {
                    if (request3.Request == requestor)
                    {
                        list.Add(request3);
                    }
                }
                foreach (StreamingRequest request4 in list)
                {
                    dictionary_0.Remove(request4);
                }
                foreach (StreamingRequest request in list)
                {
                    bool flag2 = false;
                    using (Dictionary<StreamingRequest, int>.KeyCollection.Enumerator enumerator3 = dictionary_0.Keys.GetEnumerator())
                    {
                        while (enumerator3.MoveNext())
                        {
                            StreamingRequest current = enumerator3.Current;
                            if (current.Symbol == request.Symbol)
                            {
                                ///goto  Label_00EB;  ///WYJ fix, simplify the flow
                                flag2 = true;
                                break;
                            }
                        }
                    }
                    if (!flag2)
                    {
                        this.UnSubscribe(request.Symbol);
                    }
                }
            }
        }

        public virtual void ConnectStreaming(IConnectionStatus connStatus)
        {
            iconnectionStatus_0 = connStatus;
        }

        public virtual void DisconnectStreaming()
        {
        }

        public virtual void DisconnectStreaming(IConnectionStatus connStatus)
        {
            iconnectionStatus_0 = connStatus;
            lock (dictionary_0)
            {
                dictionary_0.Clear();
            }
        }

        public int GetBaseInterval(int interval)
        {
            if (this.StreamingBarIntervals.Contains(interval))
            {
                return interval;
            }
            for (int i = this.StreamingBarIntervals.Count - 1; i >= 0; i--)
            {
                if ((interval % this.StreamingBarIntervals[i]) == 0)
                {
                    return this.StreamingBarIntervals[i];
                }
            }
            throw new ArgumentException("Streaming bar interval cannot be supported: " + interval);
        }

        public virtual MarketInfo GetMarketInfo(string symbol)
        {
            return this.DataHost.DefaultMarketInfo;
        }

        public virtual BarData GetMostRecentBar(string symbol, int baseInterval, int barInterval)
        {
            return null;
        }

        public virtual StaticDataProvider GetStaticProvider()
        {
            return null;
        }

        public List<string> GetSymbolsSubscribed(IStreamingUpdate requestor)
        {
            List<string> list = new List<string>();
            lock (dictionary_0)
            {
                foreach (StreamingRequest request in dictionary_0.Keys)
                {
                    if (request.Request == requestor)
                    {
                        list.Add(request.Symbol);
                    }
                }
            }
            return list;
        }

        public void Heartbeat(DateTime timeStamp)
        {
            lock (dictionary_0)
            {
                foreach (StreamingRequest request in dictionary_0.Keys)
                {
                    request.Request.Heartbeat(timeStamp);
                }
            }
        }

        public void Heartbeat(DateTime timeStamp, MarketInfo marketInfo)
        {
            lock (dictionary_0)
            {
                foreach (StreamingRequest request in dictionary_0.Keys)
                {
                    if (request.MarketInfo == marketInfo)
                    {
                        request.Request.Heartbeat(timeStamp, marketInfo);
                    }
                }
            }
        }

        public virtual void Initialize(IDataHost dataHost)
        {
            idataHost_0 = dataHost;
        }

        public bool IsSymbolStreaming(string symbol, IStreamingUpdate requestor)
        {
            lock (dictionary_0)
            {
                foreach (StreamingRequest request in dictionary_0.Keys)
                {
                    if ((request.Symbol == symbol) && (request.Request == requestor))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void method_0(Quote quote_0)
        {
            lock (dictionary_0)
            {
                foreach (StreamingRequest request in dictionary_0.Keys)
                {
                    if (request.Symbol == quote_0.Symbol)
                    {
                        request.Request.UpdateQuote(quote_0);
                    }
                }
            }
        }

        public static void SetBadTickFilterSettings(bool badTickFilter, double threshold)
        {
            bool_0 = badTickFilter;
            double_0 = threshold / 100.0;
        }

        protected abstract void Subscribe(string symbol);
        public void Subscribe(string symbol, IStreamingUpdate streamingUpdate)
        {
            StreamingRequest key = new StreamingRequest(symbol, streamingUpdate, this.GetMarketInfo(symbol));
            bool flag = false;
            lock (dictionary_0)
            {
                using (Dictionary<StreamingRequest, int>.KeyCollection.Enumerator enumerator = dictionary_0.Keys.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        StreamingRequest current = enumerator.Current;
                        if (current.Symbol == symbol)
                        {
                            ///goto  Label_005A;  ///WYJ fix, simplify the flow
                            flag = true;
                            break;
                        }
                    }
                }
            }
            if (!flag)
            {
                this.Subscribe(symbol);
            }
            lock (dictionary_0)
            {
                if (!dictionary_0.ContainsKey(key))
                {
                    dictionary_0[key] = 1;
                }
                else
                {
                    Dictionary<StreamingRequest, int> dictionary3;
                    StreamingRequest request3;
                    (dictionary3 = dictionary_0)[request3 = key] = dictionary3[request3] + 1;
                }
            }
        }

        protected virtual void SubscribeBars(string symbol, int barInterval)
        {
        }

        public virtual void SubscribeBars(string symbol, int barInterval, IStreamingUpdate update)
        {
            int baseInterval = this.GetBaseInterval(barInterval);
            StreamingBarRequest item = new StreamingBarRequest {
                Symbol = symbol,
                BarInterval = baseInterval
            };
            if (baseInterval != barInterval)
            {
                if (this.marketHours_0.IsMarketOpenNow)
                {
                    BarData data = this.GetMostRecentBar(symbol, baseInterval, barInterval);
                    item.Generator = new BarGenerator(barInterval, data.Open, data.High, data.Low, data.Close, data.Volume, data.Timestamp);
                }
                else
                {
                    item.Generator = new BarGenerator(barInterval);
                }
            }
            bool flag3 = false;
            lock (list_0)
            {
                using (List<StreamingBarRequest>.Enumerator enumerator = list_0.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        StreamingBarRequest current = enumerator.Current;
                        if (item.Code == current.Code)
                        {
                            ///goto  Label_00C8;  ///WYJ fix, simplify the flow
                            flag3 = true;
                            break;
                        }
                    }
                }
            }
            if (!flag3)
            {
                this.SubscribeBars(symbol, baseInterval);
            }
            lock (list_0)
            {
                item.Request = update;
                list_0.Add(item);
            }
        }

        protected abstract void UnSubscribe(string symbol);
        public void UnSubscribe(string symbol, IStreamingUpdate streamingUpdate)
        {
            StreamingRequest key = new StreamingRequest(symbol, streamingUpdate, this.GetMarketInfo(symbol));
            lock (dictionary_0)
            {
                if (dictionary_0.ContainsKey(key))
                {
                    dictionary_0.Remove(key);
                }
                bool flag2 = true;
                using (Dictionary<StreamingRequest, int>.KeyCollection.Enumerator enumerator = dictionary_0.Keys.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        StreamingRequest current = enumerator.Current;
                        if (current.Symbol == symbol)
                        {
                            ///goto  Label_0072;  ///WYJ fix, simplify the flow
                            flag2 = false;
                            break;
                        }
                    }
                }
                if (flag2)
                {
                    this.UnSubscribe(symbol);
                }
            }
        }

        protected virtual void UnSubscribeBars(string symbol, int barInterval)
        {
        }

        public virtual void UnSubscribeBars(string symbol, int barInterval, IStreamingUpdate update)
        {
            bool flag3;
            lock (list_0)
            {
                int index = 0;
                while (index < list_0.Count)
                {
                    if (list_0[index].Request == update)
                    {
                        ///goto  Label_0042;  ///WYJ fix, simplify the flow
                        list_0.RemoveAt(index);
                        break;
                    }
                    index++;
                }
            }
            flag3 = false;
            lock (list_0)
            {
                using (List<StreamingBarRequest>.Enumerator enumerator = list_0.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        StreamingBarRequest current = enumerator.Current;
                        if ((current.Symbol == symbol) && (current.BarInterval == barInterval))
                        {
                            ///goto  Label_00AB;  ///WYJ fix, simplify the flow
                            flag3 = true;
                            break;
                        }
                    }
                }
            }
            if (!flag3)
            {
                this.UnSubscribeBars(symbol, barInterval);
            }
        }

        public void UpdateMiniBar(Quote quote_0, double open, double high, double double_1)
        {
            lock (dictionary_0)
            {
                foreach (StreamingRequest request in dictionary_0.Keys)
                {
                    if (request.Symbol == quote_0.Symbol)
                    {
                        request.Request.UpdateMiniBar(quote_0, open, high, double_1);
                    }
                }
            }
        }

        public void UpdateQuote(Quote quote_0)
        {
            if (quote_0.TimeStamp != DateTime.MinValue)
            {
                if (bool_0)
                {
                    if (!this.dictionary_2.ContainsKey(quote_0.Symbol))
                    {
                        this.method_0(quote_0);
                        this.dictionary_2[quote_0.Symbol] = quote_0;
                    }
                    else if (this.dictionary_1.ContainsKey(quote_0.Symbol))
                    {
                        Quote quote = this.dictionary_1[quote_0.Symbol];
                        if (quote.DistanceFrom(quote_0) < double_0)
                        {
                            this.method_0(quote);
                            this.method_0(quote_0);
                        }
                        else
                        {
                            this.method_0(quote_0);
                        }
                        this.dictionary_2[quote_0.Symbol] = quote_0;
                        this.dictionary_1.Remove(quote_0.Symbol);
                    }
                    else
                    {
                        Quote quote2 = this.dictionary_2[quote_0.Symbol];
                        if (quote_0.DistanceFrom(quote2) > double_0)
                        {
                            this.dictionary_1[quote_0.Symbol] = quote_0;
                        }
                        else
                        {
                            this.method_0(quote_0);
                            this.dictionary_2[quote_0.Symbol] = quote_0;
                        }
                    }
                }
                else
                {
                    this.method_0(quote_0);
                }
            }
        }

        public void UpdateStreamingBar(string symbol, int barInterval, double open, double high, double double_1, double close, double volume, DateTime timeStamp, string debugInfo)
        {
            lock (list_0)
            {
                foreach (StreamingBarRequest request in list_0)
                {
                    if ((request.Symbol == symbol) && (request.BarInterval == barInterval))
                    {
                        Quote quote = new Quote {
                            Open = open,
                            Price = close,
                            Size = volume,
                            Symbol = symbol,
                            TimeStamp = timeStamp
                        };
                        if (request.Generator != null)
                        {
                            if (request.Generator.AppendData(open, high, double_1, close, volume, timeStamp))
                            {
                                BarData lastBar = request.Generator.LastBar;
                                request.Request.UpdateStreamingBar(symbol, request.Generator.Interval, lastBar.Open, lastBar.High, lastBar.Low, lastBar.Close, lastBar.Volume, lastBar.Timestamp, debugInfo);
                            }
                        }
                        else
                        {
                            request.Request.UpdateStreamingBar(symbol, barInterval, open, high, double_1, close, volume, timeStamp, debugInfo);
                        }
                    }
                }
            }
        }

        public IConnectionStatus ConnectionStatus
        {
            get
            {
                return iconnectionStatus_0;
            }
        }

        public IDataHost DataHost
        {
            get
            {
                return idataHost_0;
            }
        }

        public abstract string Description { get; }

        public abstract string FriendlyName { get; }

        public abstract Bitmap Glyph { get; }

        public abstract bool IsConnected { get; }

        public virtual bool ProvidesOpen
        {
            get
            {
                return true;
            }
        }

        public virtual bool StreamingAtDisconnect
        {
            get
            {
                return false;
            }
        }

        public virtual List<int> StreamingBarIntervals
        {
            get
            {
                return null;
            }
        }

        public virtual bool SupportsStreamingBars
        {
            get
            {
                return false;
            }
        }

        public virtual string URL
        {
            get
            {
                return "";
            }
        }
    }
}

