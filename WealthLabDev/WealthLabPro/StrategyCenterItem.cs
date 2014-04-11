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
        private BarDataRange barDataRange;
        private BarScale barScale;
        private bool bool_0;
        private bool activated;
        [CompilerGenerated]
        private bool logging;
        [CompilerGenerated]
        private bool usingStreamingFilters;
        private bool autoStage;
        private bool emailAlerts;
        private bool addedToExecutionList;
        private bool isPopulating;
        private bool runOnce;
        private bool wasActivated;
        [CompilerGenerated]
        private bool usePreferredValues;
        [CompilerGenerated]
        private bool hasRun;
        private DataSource dataSource_0;
        private DateTime lastRun;
        private DateTime nextRun;
        private Dictionary<string, DateTime> dictionary_0;
        private int barInterval;
        private int trades;
        private int newTradeCount;
        private int badSymbolCount;
        private int executeHours;
        private int executeMin;
        private List<double> parameterValues;
        private List<Alert> alertsList;
        private List<Bars> barsList;
        private List<string> list_ItemLog;
        private System.Windows.Forms.ListViewItem listViewItem;
        private MarketHours marketHours_0;
        [CompilerGenerated]
        private WealthLab.MarketInfo marketInfo;
        private WealthLab.PositionSize positionSize;
        private WealthLab.Strategy strategy;
        private StrategyCenterExecutionItem strategyCenterExecutionItem;
        private StrategyCenterForm strategyCenterForm_0;
        private StrategyCenterItemDataType targetDataType;
        private string symbol;
        private string strategyID;
        private string dataSourceName;
        private string accountNumber;
        private string accountTradeType;
        private Thread thread_0;
        private Thread thread_1;
        private WealthLab.WealthScript wealthScript;

        public StrategyCenterItem()
        {
            this.barDataRange = new BarDataRange();
            this.strategyID = "";
            this.dataSourceName = "";
            this.lastRun = DateTime.MinValue;
            this.nextRun = DateTime.MaxValue;
            this.parameterValues = new List<double>();
            this.alertsList = new List<Alert>();
            this.barsList = new List<Bars>();
            this.accountNumber = "";
            this.accountTradeType = "";
            this.dictionary_0 = new Dictionary<string, DateTime>();
            this.marketHours_0 = new MarketHours();
            this.list_ItemLog = new List<string>();
            this.executeHours = 0x10;
            this.executeMin = 30;
        }

        public StrategyCenterItem(System.Windows.Forms.ListViewItem listViewItem_1, StrategyCenterForm strategyCenterForm_1)
        {
            this.barDataRange = new BarDataRange();
            this.strategyID = "";
            this.dataSourceName = "";
            this.lastRun = DateTime.MinValue;
            this.nextRun = DateTime.MaxValue;
            this.parameterValues = new List<double>();
            this.alertsList = new List<Alert>();
            this.barsList = new List<Bars>();
            this.accountNumber = "";
            this.accountTradeType = "";
            this.dictionary_0 = new Dictionary<string, DateTime>();
            this.marketHours_0 = new MarketHours();
            this.list_ItemLog = new List<string>();
            this.executeHours = 0x10;
            this.executeMin = 30;
            this.strategyCenterForm_0 = strategyCenterForm_1;
            this.listViewItem = listViewItem_1;
            this.barDataRange.Range = BarRange.FixedBars;
            this.barDataRange.FixedBars = 0x3e8;
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
                lock (this.list_ItemLog)
                {
                    this.list_ItemLog.Add(DateTime.Now.ToString() + ": " + string_5);
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
            lock (this.alertsList)
            {
                for (int i = this.alertsList.Count - 1; i >= 0; i--)
                {
                    if (this.alertsList[i].Symbol == symbol)
                    {
                        this.alertsList.RemoveAt(i);
                    }
                }
                foreach (Alert alert in newAlerts)
                {
                    this.alertsList.Add(alert);
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
                        ///goto  Label_0034; ///WYJ fix, simplify the flow
                        parameter = current;
                        break;
                    }
                }
            }
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
                return this.accountNumber;
            }
            set
            {
                this.accountNumber = value;
            }
        }

        public string AccountTradeType
        {
            get
            {
                return this.accountTradeType;
            }
            set
            {
                this.accountTradeType = value;
            }
        }

        public bool Activated
        {
            get
            {
                return this.activated;
            }
            set
            {
                this.activated = value;
            }
        }

        [XmlIgnore]
        public bool AddedToExecutionList
        {
            get
            {
                return this.addedToExecutionList;
            }
            set
            {
                this.addedToExecutionList = value;
            }
        }

        public List<Alert> AlertsList
        {
            get
            {
                return this.alertsList;
            }
            set
            {
                this.alertsList = value;
            }
        }

        public bool AutoStage
        {
            get
            {
                return this.autoStage;
            }
            set
            {
                this.autoStage = value;
            }
        }

        [XmlIgnore]
        public int BadSymbolCount
        {
            get
            {
                return this.badSymbolCount;
            }
            set
            {
                this.badSymbolCount = value;
            }
        }

        public int BarInterval
        {
            get
            {
                return this.barInterval;
            }
            set
            {
                this.barInterval = value;
            }
        }

        [XmlIgnore]
        public List<Bars> BarsList
        {
            get
            {
                return this.barsList;
            }
        }

        public BarDataRange DataRange
        {
            get
            {
                return this.barDataRange;
            }
            set
            {
                string str = value.ToString();
                this.barDataRange = BarDataRange.Parse(str);
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
                return this.dataSourceName;
            }
            set
            {
                this.dataSourceName = value;
            }
        }

        public bool EmailAlerts
        {
            get
            {
                return this.emailAlerts;
            }
            set
            {
                this.emailAlerts = value;
            }
        }

        public int ExecuteHours
        {
            get
            {
                return this.executeHours;
            }
            set
            {
                this.executeHours = value;
            }
        }

        public int ExecuteMin
        {
            get
            {
                return this.executeMin;
            }
            set
            {
                this.executeMin = value;
            }
        }

        [XmlIgnore]
        public bool HasRun
        {
            [CompilerGenerated]
            get
            {
                return this.hasRun;
            }
            [CompilerGenerated]
            set
            {
                this.hasRun = value;
            }
        }

        [XmlIgnore]
        public bool IsPopulating
        {
            get
            {
                return this.isPopulating;
            }
            set
            {
                this.isPopulating = value;
            }
        }

        [XmlIgnore]
        public List<string> ItemLog
        {
            get
            {
                return this.list_ItemLog;
            }
        }

        public DateTime LastRun
        {
            get
            {
                return this.lastRun;
            }
            set
            {
                this.lastRun = value;
            }
        }

        [XmlIgnore]
        public System.Windows.Forms.ListViewItem ListViewItem
        {
            get
            {
                return this.listViewItem;
            }
            set
            {
                this.listViewItem = value;
            }
        }

        public bool Logging
        {
            [CompilerGenerated]
            get
            {
                return this.logging;
            }
            [CompilerGenerated]
            set
            {
                this.logging = value;
            }
        }

        [XmlIgnore]
        public WealthLab.MarketInfo MarketInfo
        {
            [CompilerGenerated]
            get
            {
                return this.marketInfo;
            }
            [CompilerGenerated]
            set
            {
                this.marketInfo = value;
            }
        }

        [XmlIgnore]
        public int NewTradeCount
        {
            get
            {
                return this.newTradeCount;
            }
            set
            {
                this.newTradeCount = value;
            }
        }

        public DateTime NextRun
        {
            get
            {
                return this.nextRun;
            }
            set
            {
                this.nextRun = value;
                this.addedToExecutionList = false;
            }
        }

        public List<double> ParameterValues
        {
            get
            {
                return this.parameterValues;
            }
            set
            {
                this.parameterValues = value;
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
                return this.positionSize;
            }
            set
            {
                string str = value.ToString();
                this.positionSize = WealthLab.PositionSize.Parse(str);
            }
        }

        [XmlIgnore]
        public bool RunOnce
        {
            get
            {
                return this.runOnce;
            }
            set
            {
                this.runOnce = value;
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale;
            }
            set
            {
                this.barScale = value;
            }
        }

        [XmlIgnore]
        public StrategyCenterExecutionItem SCEI
        {
            get
            {
                return this.strategyCenterExecutionItem;
            }
            set
            {
                this.strategyCenterExecutionItem = value;
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
                return this.strategy;
            }
            set
            {
                this.strategy = value;
            }
        }

        public string StrategyID
        {
            get
            {
                return this.strategyID;
            }
            set
            {
                this.strategyID = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
            set
            {
                this.symbol = value;
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
                return this.targetDataType;
            }
            set
            {
                this.targetDataType = value;
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
                return this.trades;
            }
            set
            {
                this.trades = value;
            }
        }

        public bool UsePreferredValues
        {
            [CompilerGenerated]
            get
            {
                return this.usePreferredValues;
            }
            [CompilerGenerated]
            set
            {
                this.usePreferredValues = value;
            }
        }

        [XmlIgnore]
        public bool UsingStreamingFilters
        {
            [CompilerGenerated]
            get
            {
                return this.usingStreamingFilters;
            }
            [CompilerGenerated]
            set
            {
                this.usingStreamingFilters = value;
            }
        }

        [XmlIgnore]
        public bool WasActivated
        {
            get
            {
                return this.wasActivated;
            }
            set
            {
                this.wasActivated = value;
            }
        }

        [XmlIgnore]
        public WealthLab.WealthScript WealthScript
        {
            get
            {
                return this.wealthScript;
            }
            set
            {
                this.wealthScript = value;
            }
        }
    }
}

