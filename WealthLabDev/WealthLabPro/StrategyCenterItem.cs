namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using WealthLab;

    [XmlRoot(ElementName="StrategyCenterItem", IsNullable=false)]
    public class StrategyCenterItem : IDataUpdateMessage, IUpdateRequestCompleted, IStreamingUpdate
    {
        private BarDataRange barDataRange_0;
        private BarScale barScale_0;
        private bool bool_0;
        private bool bool_1;
        [CompilerGenerated]
        private bool bool_10;
        [CompilerGenerated]
        private bool bool_11;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private bool bool_6;
        private bool bool_7;
        [CompilerGenerated]
        private bool bool_8;
        [CompilerGenerated]
        private bool bool_9;
        private DataSource dataSource_0;
        private DateTime dateTime_0;
        private DateTime dateTime_1;
        private Dictionary<string, DateTime> dictionary_0;
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private List<double> list_0;
        private List<Alert> list_1;
        private List<Bars> list_2;
        private List<string> list_3;
        private System.Windows.Forms.ListViewItem listViewItem_0;
        private MarketHours marketHours_0;
        [CompilerGenerated]
        private WealthLab.MarketInfo marketInfo_0;
        private WealthLab.PositionSize positionSize_0;
        private WealthLab.Strategy strategy_0;
        private StrategyCenterExecutionItem strategyCenterExecutionItem_0;
        private StrategyCenterForm strategyCenterForm_0;
        private StrategyCenterItemDataType strategyCenterItemDataType_0;
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;
        private string string_4;
        private Thread thread_0;
        private Thread thread_1;
        private WealthLab.WealthScript wealthScript_0;

        public StrategyCenterItem()
        {
            this.barDataRange_0 = new BarDataRange();
            this.string_1 = "";
            this.string_2 = "";
            this.dateTime_0 = DateTime.MinValue;
            this.dateTime_1 = DateTime.MaxValue;
            this.list_0 = new List<double>();
            this.list_1 = new List<Alert>();
            this.list_2 = new List<Bars>();
            this.string_3 = "";
            this.string_4 = "";
            this.dictionary_0 = new Dictionary<string, DateTime>();
            this.marketHours_0 = new MarketHours();
            this.list_3 = new List<string>();
            this.int_4 = 0x10;
            this.int_5 = 30;
        }

        public StrategyCenterItem(System.Windows.Forms.ListViewItem listViewItem_1, StrategyCenterForm strategyCenterForm_1)
        {
            this.barDataRange_0 = new BarDataRange();
            this.string_1 = "";
            this.string_2 = "";
            this.dateTime_0 = DateTime.MinValue;
            this.dateTime_1 = DateTime.MaxValue;
            this.list_0 = new List<double>();
            this.list_1 = new List<Alert>();
            this.list_2 = new List<Bars>();
            this.string_3 = "";
            this.string_4 = "";
            this.dictionary_0 = new Dictionary<string, DateTime>();
            this.marketHours_0 = new MarketHours();
            this.list_3 = new List<string>();
            this.int_4 = 0x10;
            this.int_5 = 30;
            this.strategyCenterForm_0 = strategyCenterForm_1;
            this.listViewItem_0 = listViewItem_1;
            this.barDataRange_0.Range = BarRange.FixedBars;
            this.barDataRange_0.FixedBars = 0x3e8;
        }

        public void CalculateNextRun(bool forceToFuture)
        {
            this.Log("Calculating NextRun...");
            this.marketHours_0.Market = this.MarketInfo;
            DateTime time = this.marketHours_0.ConvertLocalTimeToNative(DateTime.Now);
            if ((this.LastRun == DateTime.MinValue) && !this.DataScale.IsIntraday)
            {
                this.NextRun = this.marketHours_0.GetNextTimeStamp(time.AddDays(-1.0), this.DataScale);
                this.NextRun = new DateTime(this.NextRun.Year, this.NextRun.Month, this.NextRun.Day, this.ExecuteHours, this.ExecuteMin, 0);
            }
            else
            {
                this.NextRun = this.marketHours_0.GetNextTimeStamp(this.LastRun, this.DataScale);
                if (!this.DataScale.IsIntraday)
                {
                    this.NextRun = new DateTime(this.NextRun.Year, this.NextRun.Month, this.NextRun.Day, this.ExecuteHours, this.ExecuteMin, 0);
                }
            }
            if ((this.DataScale.IsIntraday && (this.NextRun.Date > this.LastRun.Date)) && ((this.LastRun.Date == DateTime.Now.Date) && (this.LastRun.TimeOfDay < this.marketHours_0.MarketCloseTimeNative.TimeOfDay)))
            {
                this.NextRun = this.LastRun.Date + this.marketHours_0.MarketCloseTimeNative.TimeOfDay;
            }
            if (this.DataScale.IsIntraday && (this.NextRun.TimeOfDay == this.marketHours_0.MarketOpenTimeNative.TimeOfDay))
            {
                this.NextRun = this.NextRun.AddMinutes((double) this.DataScale.BarInterval);
            }
            if ((forceToFuture && !this.DataScale.IsIntraday) && (time > this.NextRun))
            {
                this.method_1();
                DateTime time10 = this.marketHours_0.AdvanceToNextMarketOpen(this.NextRun).AddMinutes(-30.0);
                if (time < time10)
                {
                    forceToFuture = false;
                }
            }
            if ((this.Scale == BarScale.Daily) && (this.marketHours_0.AfterMarketCloseNow || this.marketHours_0.IsMarketOpenNow))
            {
                DateTime date = DateTime.Now.Date;
                while (!this.marketHours_0.IsTradingDay(date))
                {
                    date = date.AddDays(-1.0);
                }
                date = new DateTime(date.Year, date.Month, date.Day, this.marketHours_0.MarketCloseTimeNative.Hour, this.marketHours_0.MarketCloseTimeNative.Minute, 0);
                if (this.marketHours_0.MarketCloseTimeNative.Hour == 0x17)
                {
                    if (this.marketHours_0.MarketCloseTimeNative.Minute < 0x2d)
                    {
                        date = date.AddMinutes(15.0);
                    }
                    else if (this.marketHours_0.MarketCloseTimeNative.Minute < 0x37)
                    {
                        date = date.AddMinutes(5.0);
                    }
                }
                else
                {
                    date = date.AddMinutes(30.0);
                }
                if (this.LastRun.Date < date.Date)
                {
                    forceToFuture = false;
                    this.NextRun = date;
                }
            }
            if (forceToFuture && (time > this.NextRun))
            {
                while (time > this.NextRun)
                {
                    this.NextRun = this.marketHours_0.GetNextTimeStamp(this.NextRun, this.DataScale);
                    this.method_1();
                }
            }
            this.method_1();
            this.Log("NextRun set to " + this.NextRun.ToString());
        }

        public void DisplayUpdateMessage(string message)
        {
            this.strategyCenterForm_0.method_6(this, message);
            this.Log(message);
        }

        public void Heartbeat(DateTime timeStamp)
        {
        }

        public void Heartbeat(DateTime timeStamp, WealthLab.MarketInfo marketInfo)
        {
        }

        public void Log(string string_5)
        {
            if (this.Logging)
            {
                lock (this.list_3)
                {
                    this.list_3.Add(DateTime.Now.ToString() + ": " + string_5);
                }
            }
        }

        internal void method_0(MarketHours marketHours_1)
        {
            string symbol = this.Symbol;
            if (((symbol == null) || (symbol == "")) && ((this.DataSet != null) && (this.DataSet.Symbols.Count > 0)))
            {
                symbol = this.DataSet.Symbols[0];
            }
            if (((symbol != null) && (symbol != "")) && (this.DataSet != null))
            {
                this.MarketInfo = this.DataSet.Provider.GetMarketInfo(symbol);
            }
            else
            {
                this.MarketInfo = marketHours_1.Market;
            }
        }

        private void method_1()
        {
            this.marketHours_0.Market = this.MarketInfo;
            if (!this.DataScale.IsIntraday)
            {
                this.NextRun = new DateTime(this.NextRun.Year, this.NextRun.Month, this.NextRun.Day, this.ExecuteHours, this.ExecuteMin, 0);
                if ((this.RunOnce && !this.marketHours_0.AfterMarketCloseNow) && this.marketHours_0.IsTodayTradingDay)
                {
                    this.NextRun = this.NextRun.AddDays(-1.0);
                }
                if (this.LastRun > this.NextRun)
                {
                    this.NextRun = new DateTime(this.LastRun.Year, this.LastRun.Month, this.LastRun.Day, this.ExecuteHours, this.ExecuteMin, 0);
                }
            }
        }

        public void ProcessingCompleted()
        {
            this.strategyCenterForm_0.method_12(this);
        }

        public void ProcessStreamingBarUpdate(object data)
        {
            Bars bars = data as Bars;
            this.strategyCenterForm_0.method_13(this, bars);
        }

        public void Refresh()
        {
            this.ListViewItem.SubItems[1].Text = this.AccountNumber;
            if (this.LastRun == DateTime.MinValue)
            {
                this.ListViewItem.SubItems[2].Text = "-None-";
            }
            else if (this.ShowLocalTimeinLastNextRun)
            {
                DateTime time6 = TimeZoneInformation.ToLocalTime(this.MarketInfo.TimeZoneName, this.LastRun, TimeZoneInformation.CurrentTimeZone.Name);
                this.ListViewItem.SubItems[2].Text = time6.ToShortDateString() + " " + time6.ToShortTimeString();
            }
            else
            {
                this.ListViewItem.SubItems[2].Text = this.LastRun.ToShortDateString() + " " + this.LastRun.ToShortTimeString();
            }
            if (this.NextRun == DateTime.MaxValue)
            {
                this.ListViewItem.SubItems[3].Text = "-None-";
            }
            else if (this.ShowLocalTimeinLastNextRun)
            {
                DateTime time = TimeZoneInformation.ToLocalTime(this.MarketInfo.TimeZoneName, this.NextRun, TimeZoneInformation.CurrentTimeZone.Name);
                this.ListViewItem.SubItems[3].Text = time.ToShortDateString() + " " + time.ToShortTimeString();
            }
            else
            {
                this.ListViewItem.SubItems[3].Text = this.NextRun.ToShortDateString() + " " + this.NextRun.ToShortTimeString();
            }
            this.ListViewItem.SubItems[4].Text = this.Trades.ToString();
            this.ListViewItem.SubItems[5].Text = this.AlertsList.Count.ToString();
            if (this.TargetDataType == StrategyCenterItemDataType.None)
            {
                this.ListViewItem.SubItems[6].Text = "-None-";
            }
            if (this.TargetDataType == StrategyCenterItemDataType.Symbol)
            {
                this.ListViewItem.SubItems[6].Text = this.Symbol;
            }
            else if (this.dataSource_0 != null)
            {
                this.ListViewItem.SubItems[6].Text = "{" + this.dataSource_0.Name + "}";
            }
            else
            {
                this.ListViewItem.SubItems[6].Text = "-None-";
            }
            this.ListViewItem.SubItems[7].Text = this.DataRange.Text;
            this.ListViewItem.SubItems[8].Text = new BarDataScale(this.Scale, this.BarInterval).ToString();
            this.ListViewItem.SubItems[9].Text = this.PositionSize.Text;
            if (this.WealthScript == null)
            {
                this.ListViewItem.SubItems[10].Text = "";
            }
            else if (this.WealthScript.Parameters.Count == 0)
            {
                this.ListViewItem.SubItems[10].Text = "";
            }
            else if (this.UsePreferredValues && (this.TargetDataType == StrategyCenterItemDataType.DataSet))
            {
                this.ListViewItem.SubItems[10].Text = "(Preferred Values)";
            }
            else
            {
                if (this.UsePreferredValues)
                {
                    this.Strategy.LoadPreferredValues(this.Symbol, this.WealthScript);
                }
                StringBuilder builder = new StringBuilder();
                builder.Append("(");
                for (int i = 0; i < this.WealthScript.Parameters.Count; i++)
                {
                    StrategyParameter parameter = this.WealthScript.Parameters[i];
                    builder.Append(parameter.Value.ToString());
                    if (i < (this.WealthScript.Parameters.Count - 1))
                    {
                        builder.Append(",");
                    }
                }
                builder.Append(")");
                this.ListViewItem.SubItems[10].Text = builder.ToString();
            }
            this.ListViewItem.SubItems[11].Text = this.AccountTradeType;
        }

        public void ReplaceAlerts(string symbol, List<Alert> newAlerts)
        {
            lock (this.list_1)
            {
                for (int i = this.list_1.Count - 1; i >= 0; i--)
                {
                    if (this.list_1[i].Symbol == symbol)
                    {
                        this.list_1.RemoveAt(i);
                    }
                }
                foreach (Alert alert in newAlerts)
                {
                    this.list_1.Add(alert);
                }
            }
        }

        public void ReportUpdateProgress(int progressPercent)
        {
        }

        public bool ShouldExecute(Bars bars_0)
        {
            if (!this.RunOnce)
            {
                if (bars_0.Count == 0)
                {
                    return true;
                }
                DateTime time = bars_0.Date[bars_0.Count - 1];
                if (!this.dictionary_0.ContainsKey(bars_0.Symbol))
                {
                    this.dictionary_0[bars_0.Symbol] = time;
                    return true;
                }
                if (this.dictionary_0[bars_0.Symbol] >= time)
                {
                    return false;
                }
                this.dictionary_0[bars_0.Symbol] = time;
            }
            return true;
        }

        public void SubscribeToStreamingBars()
        {
            if (!this.bool_0 && !this.RunOnce)
            {
                this.bool_0 = true;
                StreamingDataProvider streamingProvider = MainModule.Instance.StreamingProvider;
                foreach (string str in this.SymbolsToProcess)
                {
                    this.Log(string.Concat(new object[] { "Subscribing to ", str, ", BarInterval ", this.BarInterval }));
                    streamingProvider.SubscribeBars(str, this.BarInterval, this);
                }
            }
        }

        public void UnSubscribeToStreamingBars()
        {
            if (this.bool_0)
            {
                this.bool_0 = false;
                StreamingDataProvider streamingProvider = MainModule.Instance.StreamingProvider;
                foreach (string str in this.SymbolsToProcess)
                {
                    this.Log(string.Concat(new object[] { "Unsubscribing to ", str, ", BarInterval ", this.BarInterval }));
                    streamingProvider.UnSubscribeBars(str, this.BarInterval, this);
                }
            }
        }

        public void UpdateCompleted(Bars bars)
        {
            this.strategyCenterForm_0.method_13(this, bars);
        }

        public void UpdateError(string symbol, Exception exception_0)
        {
            this.strategyCenterForm_0.method_15(this, symbol, exception_0);
        }

        public void UpdateMiniBar(Quote quote_0, double open, double high, double double_0)
        {
        }

        public void UpdateQuote(Quote quote_0)
        {
        }

        public void UpdateStreamingBar(string symbol, int barInterval, double open, double high, double double_0, double close, double volume, DateTime timeStamp, string debugInfo)
        {
            Bars parameter = null;
            using (List<Bars>.Enumerator enumerator = this.BarsList.GetEnumerator())
            {
                Bars current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Symbol == symbol)
                    {
                        goto Label_0034;
                    }
                }
                goto Label_0047;
            Label_0034:
                parameter = current;
            }
        Label_0047:
            if (parameter != null)
            {
                int num = parameter.Count - 1;
                if (num >= 0)
                {
                    if (open == 0.0)
                    {
                        open = parameter.Open[num];
                    }
                    if (high == 0.0)
                    {
                        high = parameter.High[num];
                    }
                    if (double_0 == 0.0)
                    {
                        double_0 = parameter.Low[num];
                    }
                    if (close == 0.0)
                    {
                        close = parameter.Close[num];
                    }
                }
                parameter.Add(timeStamp, open, high, double_0, close, volume);
                parameter.Tag = debugInfo;
                new Thread(new ParameterizedThreadStart(this.ProcessStreamingBarUpdate)) { IsBackground = true }.Start(parameter);
            }
        }

        public string AccountNumber
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

        public string AccountTradeType
        {
            get
            {
                return this.string_4;
            }
            set
            {
                this.string_4 = value;
            }
        }

        public bool Activated
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

        [XmlIgnore]
        public bool AddedToExecutionList
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

        public List<Alert> AlertsList
        {
            get
            {
                return this.list_1;
            }
            set
            {
                this.list_1 = value;
            }
        }

        public bool AutoStage
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

        [XmlIgnore]
        public int BadSymbolCount
        {
            get
            {
                return this.int_3;
            }
            set
            {
                this.int_3 = value;
            }
        }

        public int BarInterval
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }

        [XmlIgnore]
        public List<Bars> BarsList
        {
            get
            {
                return this.list_2;
            }
        }

        public BarDataRange DataRange
        {
            get
            {
                return this.barDataRange_0;
            }
            set
            {
                string str = value.ToString();
                this.barDataRange_0 = BarDataRange.Parse(str);
            }
        }

        public BarDataScale DataScale
        {
            get
            {
                return new BarDataScale(this.Scale, this.BarInterval);
            }
        }

        [XmlIgnore]
        public DataSource DataSet
        {
            get
            {
                return this.dataSource_0;
            }
            set
            {
                this.dataSource_0 = value;
            }
        }

        public string DataSourceName
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        public bool EmailAlerts
        {
            get
            {
                return this.bool_3;
            }
            set
            {
                this.bool_3 = value;
            }
        }

        public int ExecuteHours
        {
            get
            {
                return this.int_4;
            }
            set
            {
                this.int_4 = value;
            }
        }

        public int ExecuteMin
        {
            get
            {
                return this.int_5;
            }
            set
            {
                this.int_5 = value;
            }
        }

        [XmlIgnore]
        public bool HasRun
        {
            [CompilerGenerated]
            get
            {
                return this.bool_9;
            }
            [CompilerGenerated]
            set
            {
                this.bool_9 = value;
            }
        }

        [XmlIgnore]
        public bool IsPopulating
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

        [XmlIgnore]
        public List<string> ItemLog
        {
            get
            {
                return this.list_3;
            }
        }

        public DateTime LastRun
        {
            get
            {
                return this.dateTime_0;
            }
            set
            {
                this.dateTime_0 = value;
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

        public bool Logging
        {
            [CompilerGenerated]
            get
            {
                return this.bool_10;
            }
            [CompilerGenerated]
            set
            {
                this.bool_10 = value;
            }
        }

        [XmlIgnore]
        public WealthLab.MarketInfo MarketInfo
        {
            [CompilerGenerated]
            get
            {
                return this.marketInfo_0;
            }
            [CompilerGenerated]
            set
            {
                this.marketInfo_0 = value;
            }
        }

        [XmlIgnore]
        public int NewTradeCount
        {
            get
            {
                return this.int_2;
            }
            set
            {
                this.int_2 = value;
            }
        }

        public DateTime NextRun
        {
            get
            {
                return this.dateTime_1;
            }
            set
            {
                this.dateTime_1 = value;
                this.bool_4 = false;
            }
        }

        public List<double> ParameterValues
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

        [XmlIgnore]
        public StrategyCenterForm Parent
        {
            get
            {
                return this.strategyCenterForm_0;
            }
            set
            {
                this.strategyCenterForm_0 = value;
            }
        }

        public WealthLab.PositionSize PositionSize
        {
            get
            {
                return this.positionSize_0;
            }
            set
            {
                string str = value.ToString();
                this.positionSize_0 = WealthLab.PositionSize.Parse(str);
            }
        }

        [XmlIgnore]
        public bool RunOnce
        {
            get
            {
                return this.bool_6;
            }
            set
            {
                this.bool_6 = value;
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale_0;
            }
            set
            {
                this.barScale_0 = value;
            }
        }

        [XmlIgnore]
        public StrategyCenterExecutionItem SCEI
        {
            get
            {
                return this.strategyCenterExecutionItem_0;
            }
            set
            {
                this.strategyCenterExecutionItem_0 = value;
            }
        }

        public bool ShowLocalTimeinLastNextRun
        {
            get
            {
                return MainModule.Instance.Settings.Get("ShowLocalTime", false);
            }
        }

        [XmlIgnore]
        public WealthLab.Strategy Strategy
        {
            get
            {
                return this.strategy_0;
            }
            set
            {
                this.strategy_0 = value;
            }
        }

        public string StrategyID
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        [XmlIgnore]
        public List<string> SymbolsToProcess
        {
            get
            {
                List<string> list = new List<string>();
                if (this.Symbol != "")
                {
                    list.Add(this.Symbol);
                    return list;
                }
                foreach (string str in this.DataSet.Symbols)
                {
                    list.Add(str);
                }
                return list;
            }
        }

        public StrategyCenterItemDataType TargetDataType
        {
            get
            {
                return this.strategyCenterItemDataType_0;
            }
            set
            {
                this.strategyCenterItemDataType_0 = value;
            }
        }

        [XmlIgnore]
        public Thread ThreadPopulate
        {
            get
            {
                return this.thread_0;
            }
            set
            {
                this.thread_0 = value;
            }
        }

        [XmlIgnore]
        public Thread ThreadUpdate
        {
            get
            {
                return this.thread_1;
            }
            set
            {
                this.thread_1 = value;
            }
        }

        public int Trades
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

        public bool UsePreferredValues
        {
            [CompilerGenerated]
            get
            {
                return this.bool_8;
            }
            [CompilerGenerated]
            set
            {
                this.bool_8 = value;
            }
        }

        [XmlIgnore]
        public bool UsingStreamingFilters
        {
            [CompilerGenerated]
            get
            {
                return this.bool_11;
            }
            [CompilerGenerated]
            set
            {
                this.bool_11 = value;
            }
        }

        [XmlIgnore]
        public bool WasActivated
        {
            get
            {
                return this.bool_7;
            }
            set
            {
                this.bool_7 = value;
            }
        }

        [XmlIgnore]
        public WealthLab.WealthScript WealthScript
        {
            get
            {
                return this.wealthScript_0;
            }
            set
            {
                this.wealthScript_0 = value;
            }
        }
    }
}

