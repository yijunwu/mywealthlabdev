namespace WealthLab
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;

    [ToolboxBitmap(typeof(StreamingChartManager), "StreamingChartManager")]
    public class StreamingChartManager : Component, IStreamingUpdate
    {
        private WealthLab.Bars bars_0;
        private BarScale barScale_0;
        private BarsLoader barsLoader_0;
        private bool bool_0;
        private DateTime dateTime_0;
        private DateTime dateTime_1;
        private DateTime dateTime_2;
        private IConnectionStatus iconnectionStatus_0;
        private IContainer icontainer_0;
        private int int_0;
        private MarketHours marketHours_0;
        private Quote quote_0;
        private StaticDataProvider staticDataProvider_0;
        private StreamingDataProvider streamingDataProvider_0;
        private string string_0;

        private EventHandler<EventArgs> eventHandler_0;

        private EventHandler<EventArgs> eventHandler_1;


        public event EventHandler<EventArgs> BarsLocked
        {
            add
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<EventArgs> eventHandler2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<EventArgs> eventHandler2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
        }

        public event EventHandler<EventArgs> NewBar
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

        public StreamingChartManager()
        {
            this.string_0 = "";
            this.barsLoader_0 = new BarsLoader();
            this.marketHours_0 = new MarketHours();
            this.dateTime_1 = DateTime.MinValue;
            this.dateTime_2 = DateTime.MinValue;
            this.method_0();
        }

        public StreamingChartManager(IContainer container)
        {
            this.string_0 = "";
            this.barsLoader_0 = new BarsLoader();
            this.marketHours_0 = new MarketHours();
            this.dateTime_1 = DateTime.MinValue;
            this.dateTime_2 = DateTime.MinValue;
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

        public void Heartbeat(DateTime timeStamp)
        {
            this.dateTime_2 = timeStamp;
            if (((this.dateTime_1 != DateTime.MinValue) && (timeStamp >= this.dateTime_1)) && !this.bars_0.Locked)
            {
                bool flag = false;
                if (!double.IsNaN(this.bars_0.Open.PartialValue) && (this.dateTime_1.TimeOfDay <= this.marketHours_0.MarketCloseTimeNative.TimeOfDay))
                {
                    flag = false;
                    if (this.method_4(this.dateTime_1))
                    {
                        this.bars_0.Add(this.dateTime_1, this.bars_0.Open.PartialValue, this.bars_0.High.PartialValue, this.bars_0.Low.PartialValue, this.bars_0.Close.PartialValue, this.bars_0.Volume.PartialValue);
                        flag = true;
                    }
                }
                this.bars_0.Open.PartialValue = double.NaN;
                this.bars_0.High.PartialValue = double.NaN;
                this.bars_0.Low.PartialValue = double.NaN;
                this.bars_0.Close.PartialValue = double.NaN;
                this.bars_0.Volume.PartialValue = 0.0;
                this.dateTime_0 = this.dateTime_1;
                this.dateTime_1 = this.method_2(timeStamp);
                if (flag && (this.eventHandler_0 != null))
                {
                    this.eventHandler_0(this, EventArgs.Empty);
                }
            }
        }

        public void Heartbeat(DateTime timeStamp, MarketInfo marketInfo)
        {
            if ((this.Bars != null) && (this.Bars.MarketInfo == marketInfo))
            {
                this.Heartbeat(timeStamp);
            }
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        private bool method_1(DateTime dateTime_3)
        {
            if (dateTime_3.TimeOfDay > this.marketHours_0.MarketCloseTimeNative.TimeOfDay)
            {
                return false;
            }
            if (dateTime_3.TimeOfDay <= this.marketHours_0.MarketOpenTimeNative.TimeOfDay)
            {
                return false;
            }
            return true;
        }

        private DateTime method_2(DateTime dateTime_3)
        {
            DateTime time;
            int num;
            int year;
            if ((this.Scale != BarScale.Second) && (this.Scale != BarScale.Tick))
            {
                dateTime_3 = new DateTime(dateTime_3.Year, dateTime_3.Month, dateTime_3.Day, dateTime_3.Hour, dateTime_3.Minute, 0);
            }
            switch (this.Scale)
            {
                case BarScale.Daily:
                    dateTime_3 = dateTime_3.Date.AddDays(1.0);
                    while (!this.marketHours_0.IsTradingDay(dateTime_3))
                    {
                        dateTime_3 = dateTime_3.AddDays(1.0);
                    }
                    ///goto  Label_0399;  ///WYJ fix, simplify the flow
                    break;

                case BarScale.Weekly:
                    time = dateTime_3.Date.AddDays(1.0);
                    while (time.DayOfWeek != DayOfWeek.Monday)
                    {
                        time = time.AddDays(1.0);
                    }
                    while (!this.marketHours_0.IsTradingDay(time))
                    {
                        time = time.AddDays(1.0);
                    }
                    dateTime_3 = time;
                    ///goto  Label_0399;
                    break;

                case BarScale.Monthly:
                {
                    int month = dateTime_3.Month + 1;
                    year = dateTime_3.Year;
                    if (month > 12)
                    {
                        month -= 12;
                        year++;
                    }
                    time = new DateTime(year, month, 1, 0, 0, 0);
                    while (!this.marketHours_0.IsTradingDay(time))
                    {
                        time = time.AddDays(1.0);
                    }
                    dateTime_3 = time;
                    ///goto  Label_0399;
                    break;
                }
                case BarScale.Minute:
                    if ((dateTime_3.TimeOfDay < this.marketHours_0.MarketCloseTimeNative.TimeOfDay) || (dateTime_3.Date >= DateTime.Now.Date))
                    {
                        DateTime time2 = dateTime_3;
                        DateTime time4 = dateTime_3.Date + this.marketHours_0.MarketOpenTimeNative.TimeOfDay;
                        while (time4 <= dateTime_3)
                        {
                            time4 = time4.AddMinutes((double) this.BarInterval);
                        }
                        dateTime_3 = time4;
                        if ((dateTime_3.TimeOfDay > this.marketHours_0.MarketCloseTimeNative.TimeOfDay) && (time2.TimeOfDay < this.marketHours_0.MarketCloseTimeNative.TimeOfDay))
                        {
                            dateTime_3 = DateTime.Now.Date + this.marketHours_0.MarketCloseTimeNative.TimeOfDay;
                        }
                        return dateTime_3;
                    }
                    dateTime_3 = DateTime.Now.Date + this.marketHours_0.MarketOpenTimeNative.TimeOfDay;
                    dateTime_3 = dateTime_3.AddMinutes((double) this.BarInterval);
                    return dateTime_3;

                case BarScale.Second:
                    return dateTime_3.AddSeconds((double) this.BarInterval);

                case BarScale.Quarterly:
                    year = dateTime_3.Year;
                    num = 3;
                    switch (dateTime_3.Month)
                    {
                        case 1:
                        case 2:
                        case 3:
                            num = 3;
                            ///goto  Label_030C;
                            break;

                        case 4:
                        case 5:
                        case 6:
                            num = 6;
                            ///goto  Label_030C;
                            break;

                        case 7:
                        case 8:
                        case 9:
                            num = 9;
                            ///goto  Label_030C;
                            break;

                        case 10:
                        case 11:
                        case 12:
                            num = 12;
                            ///goto  Label_030C;
                            break;
                    }
                    //Label_030C:
                    time = new DateTime(year, num, 0x1c, 0, 0, 0);
                    while (time.Month == num)
                    {
                        time = time.AddDays(1.0);
                    }
                    while (!this.marketHours_0.IsTradingDay(time))
                    {
                        time = time.AddDays(1.0);
                    }
                    dateTime_3 = time;
                    ///goto  Label_0399;
                    break;

                case BarScale.Yearly:
                    time = new DateTime(dateTime_3.Year + 1, 1, 1, 0, 0, 0);
                    while (!this.marketHours_0.IsTradingDay(time))
                    {
                        time = time.AddDays(1.0);
                    }
                    dateTime_3 = time;
                    ///goto  Label_0399;
                    break;

                default:
                    ///goto  Label_0399;
                    break;
            }
            if (!this.BarDataScale.IsIntraday)
            {
                DateTime marketCloseTimeNative = this.marketHours_0.MarketCloseTimeNative;
                dateTime_3 = new DateTime(dateTime_3.Year, dateTime_3.Month, dateTime_3.Day, marketCloseTimeNative.Hour, marketCloseTimeNative.Minute, marketCloseTimeNative.Second);
            }
            return dateTime_3;
        }

        private void method_3(Quote quote_1)
        {
            if (this.bars_0 != null)
            {
                this.bars_0.Open.PartialValue = quote_1.Price;
                this.bars_0.High.PartialValue = quote_1.Price;
                this.bars_0.Low.PartialValue = quote_1.Price;
                this.bars_0.Close.PartialValue = quote_1.Price;
                this.bars_0.Volume.PartialValue = quote_1.Size;
                this.dateTime_1 = this.method_2(quote_1.TimeStamp);
            }
        }

        private bool method_4(DateTime dateTime_3)
        {
            return ((this.bars_0.Count == 0) || (this.bars_0.Date[this.bars_0.Count - 1] < dateTime_3));
        }

        public WealthLab.Bars StartStreaming(DataSource dataSource_0, string symbol, WealthLab.BarDataScale scale, BarDataRange range)
        {
            if (this.streamingDataProvider_0 == null)
            {
                return null;
            }
            this.marketHours_0.Market = this.streamingDataProvider_0.GetMarketInfo(symbol);
            this.barScale_0 = scale.Scale;
            this.int_0 = scale.BarInterval;
            this.dateTime_1 = DateTime.MinValue;
            if (symbol != this.string_0)
            {
                this.streamingDataProvider_0.UnSubscribe(this.string_0, this);
            }
            StreamingChartManager streamingUpdate = null;
            if (!this.streamingDataProvider_0.IsConnected)
            {
                streamingUpdate = new StreamingChartManager();
                this.streamingDataProvider_0.Subscribe(symbol, streamingUpdate);
            }
            this.barsLoader_0.IncludePartialBar = true;
            this.barsLoader_0.BarDataScale = scale;
            range.ConfigureBarsLoader(this.barsLoader_0);
            DataSource source = new DataSource(this.staticDataProvider_0) {
                DSString = dataSource_0.DSString
            };
            this.barsLoader_0.OverrideOnDemand = true;
            this.barsLoader_0.OverrideOnDemandValue = true;
            this.staticDataProvider_0.IsStreamingRequest = range.Range != BarRange.AllData;
            WealthLab.Bars data = this.barsLoader_0.GetData(source, symbol);
            if (data.Count == 1)
            {
                data.method_0();
            }
            bool isMarketOpenNow = false;
            if (data.Count > 0)
            {
                if (!data.IsIntraday)
                {
                    isMarketOpenNow = this.marketHours_0.IsMarketOpenNow;
                }
                else if (this.Scale == BarScale.Tick)
                {
                    isMarketOpenNow = false;
                }
                else
                {
                    isMarketOpenNow = this.marketHours_0.IsMarketOpenNow && (data.Date[data.Count - 1].Date == DateTime.Now.Date);
                }
            }
            DateTime minValue = DateTime.MinValue;
            if (isMarketOpenNow)
            {
                int num = data.Count - 1;
                data.Open.PartialValue = data.Open[num];
                data.High.PartialValue = data.High[num];
                data.Low.PartialValue = data.Low[num];
                data.Close.PartialValue = data.Close[num];
                data.Volume.PartialValue = data.Volume[num];
                minValue = data.Date[num];
                data.Delete(num);
                DateTime time = DateTime.MinValue;
                if (!data.IsIntraday && (data.Scale != BarScale.Daily))
                {
                    time = minValue;
                }
                else
                {
                    time = data.Date[data.Count - 1];
                }
                this.dateTime_1 = this.method_2(time);
            }
            else
            {
                data.Open.PartialValue = double.NaN;
                data.High.PartialValue = double.NaN;
                data.Low.PartialValue = double.NaN;
                data.Close.PartialValue = double.NaN;
                data.Volume.PartialValue = double.NaN;
                this.dateTime_1 = DateTime.MinValue;
            }
            if (symbol != this.string_0)
            {
                this.streamingDataProvider_0.Subscribe(symbol, this);
                this.string_0 = symbol;
            }
            this.bars_0 = data;
            if (streamingUpdate != null)
            {
                this.streamingDataProvider_0.UnSubscribe(symbol, streamingUpdate);
            }
            return data;
        }

        public void StopStreaming()
        {
            if (this.string_0 != "")
            {
                this.streamingDataProvider_0.UnSubscribe(this.string_0, this);
                this.string_0 = "";
            }
        }

        public void UpdateMiniBar(Quote quote_1, double open, double high, double double_0)
        {
            if (this.bars_0 != null)
            {
                this.dateTime_2 = quote_1.TimeStamp;
                if (this.dateTime_1 == DateTime.MinValue)
                {
                    this.method_3(quote_1);
                }
                else
                {
                    if (quote_1.TimeStamp >= this.dateTime_1)
                    {
                        if (this.bars_0.Locked)
                        {
                            this.eventHandler_1(this, EventArgs.Empty);
                        }
                        bool flag = false;
                        if (!double.IsNaN(this.bars_0.Open.PartialValue) && this.method_4(this.dateTime_1))
                        {
                            this.bars_0.Add(this.dateTime_1, this.bars_0.Open.PartialValue, this.bars_0.High.PartialValue, this.bars_0.Low.PartialValue, this.bars_0.Close.PartialValue, this.bars_0.Volume.PartialValue);
                            flag = true;
                        }
                        this.bars_0.Open.PartialValue = open;
                        this.bars_0.High.PartialValue = high;
                        this.bars_0.Low.PartialValue = double_0;
                        this.bars_0.Close.PartialValue = quote_1.Price;
                        this.bars_0.Volume.PartialValue = quote_1.Size;
                        this.dateTime_0 = this.dateTime_1;
                        this.dateTime_1 = this.method_2(quote_1.TimeStamp);
                        if (flag && (this.eventHandler_0 != null))
                        {
                            this.eventHandler_0(this, EventArgs.Empty);
                        }
                    }
                    else if (double.IsNaN(this.bars_0.Open.PartialValue))
                    {
                        this.bars_0.Open.PartialValue = open;
                        this.bars_0.High.PartialValue = high;
                        this.bars_0.Low.PartialValue = double_0;
                        this.bars_0.Close.PartialValue = quote_1.Price;
                        this.bars_0.Volume.PartialValue = quote_1.Size;
                    }
                    else
                    {
                        this.bars_0.Close.PartialValue = quote_1.Price;
                        if (high > this.bars_0.High.PartialValue)
                        {
                            this.bars_0.High.PartialValue = high;
                        }
                        if (double_0 < this.bars_0.Low.PartialValue)
                        {
                            this.bars_0.Low.PartialValue = double_0;
                        }
                        DataSeries volume = this.bars_0.Volume;
                        volume.PartialValue += quote_1.Size;
                    }
                    this.bool_0 = true;
                }
            }
        }

        public void UpdateQuote(Quote quote_1)
        {
            if (quote_1.TimeStamp < this.dateTime_2)
            {
                this.dateTime_1 = this.method_2(quote_1.TimeStamp);
            }
            this.dateTime_2 = quote_1.TimeStamp;
            if (this.dateTime_1 == DateTime.MinValue)
            {
                this.method_3(quote_1);
            }
            else if (quote_1.TimeStamp >= this.dateTime_1)
            {
                bool flag = false;
                if (!double.IsNaN(this.bars_0.Open.PartialValue))
                {
                    if (this.bars_0.Locked)
                    {
                        if (this.dateTime_0 < this.dateTime_1)
                        {
                            this.quote_0 = quote_1;
                            this.eventHandler_1(this, EventArgs.Empty);
                        }
                        return;
                    }
                    if ((this.dateTime_1.TimeOfDay <= this.marketHours_0.MarketCloseTimeNative.TimeOfDay) && this.method_4(this.dateTime_1))
                    {
                        flag = true;
                        this.bars_0.Add(this.dateTime_1, this.bars_0.Open.PartialValue, this.bars_0.High.PartialValue, this.bars_0.Low.PartialValue, this.bars_0.Close.PartialValue, this.bars_0.Volume.PartialValue);
                    }
                }
                this.bars_0.Open.PartialValue = quote_1.Price;
                this.bars_0.High.PartialValue = quote_1.Price;
                this.bars_0.Low.PartialValue = quote_1.Price;
                this.bars_0.Close.PartialValue = quote_1.Price;
                this.bars_0.Volume.PartialValue = quote_1.Size;
                this.dateTime_0 = this.dateTime_1;
                this.dateTime_1 = this.method_2(quote_1.TimeStamp);
                if (flag && (this.eventHandler_0 != null))
                {
                    this.eventHandler_0(this, EventArgs.Empty);
                }
            }
            else
            {
                if (this.bars_0 != null)
                {
                    if (double.IsNaN(this.bars_0.Close.PartialValue))
                    {
                        this.bars_0.Open.PartialValue = quote_1.Price;
                        this.bars_0.High.PartialValue = quote_1.Price;
                        this.bars_0.Low.PartialValue = quote_1.Price;
                        this.bars_0.Close.PartialValue = quote_1.Price;
                        this.bars_0.Volume.PartialValue = quote_1.Size;
                    }
                    else
                    {
                        this.bars_0.Close.PartialValue = quote_1.Price;
                        if (quote_1.Price > this.bars_0.High.PartialValue)
                        {
                            this.bars_0.High.PartialValue = quote_1.Price;
                        }
                        if (quote_1.Price < this.bars_0.Low.PartialValue)
                        {
                            this.bars_0.Low.PartialValue = quote_1.Price;
                        }
                        DataSeries volume = this.bars_0.Volume;
                        volume.PartialValue += quote_1.Size;
                    }
                }
                this.bool_0 = true;
            }
        }

        public void UpdateStreamingBar(string symbol, int barInterval, double open, double high, double double_0, double close, double volume, DateTime timeStamp, string debugInfo)
        {
        }

        public WealthLab.BarDataScale BarDataScale
        {
            get
            {
                return new WealthLab.BarDataScale(this.Scale, this.BarInterval);
            }
        }

        public int BarInterval
        {
            get
            {
                return this.int_0;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars_0;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool GhostBarHasUpdated
        {
            get
            {
                if (this.bool_0)
                {
                    this.bool_0 = false;
                    return true;
                }
                return false;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
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
                    this.staticDataProvider_0 = this.streamingDataProvider_0.GetStaticProvider();
                    this.staticDataProvider_0.Initialize(this.streamingDataProvider_0.DataHost);
                }
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale_0;
            }
        }

        public string Symbol
        {
            get
            {
                return this.string_0;
            }
        }
    }
}

