namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Xml.Serialization;

    [ToolboxBitmap(typeof(BarsLoader), "BarsLoader")]
    public class BarsLoader : Component, IDataHost
    {
        private BarScale barScale_0;
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private bool bool_3;
        private static bool bool_4 = true;
        private bool bool_5;
        private DataSource dataSource_0;
        private DateTime dateTime_0;
        private DateTime dateTime_1;
        private Dictionary<Type, StaticDataProvider> dictionary_0;
        private IContainer icontainer_0;
        private IDataHost idataHost_0;
        private int int_0;
        private int int_1;
        private static List<WealthLab.SymbolInfo> list_0 = new List<WealthLab.SymbolInfo>();
        private static object object_0 = new object();
        private object object_1;
        private StaticDataProvider staticDataProvider_0;
        private static string string_0 = "";

        public event EventHandler<BarsEventArgs> ASynchDataCompleted;

        public event EventHandler<UnhandledExceptionEventArgs> ASynchDataException;

        public BarsLoader()
        {
            this.dateTime_0 = DateTime.MinValue;
            this.dateTime_1 = DateTime.MaxValue;
            this.object_1 = new object();
            this.dictionary_0 = new Dictionary<Type, StaticDataProvider>();
            this.bool_3 = true;
            this.bool_5 = true;
            this.method_0();
        }

        public BarsLoader(IContainer container)
        {
            this.dateTime_0 = DateTime.MinValue;
            this.dateTime_1 = DateTime.MaxValue;
            this.object_1 = new object();
            this.dictionary_0 = new Dictionary<Type, StaticDataProvider>();
            this.bool_3 = true;
            this.bool_5 = true;
            container.Add(this);
            this.method_0();
        }

        public void AdjustForStockSplit(StaticDataProvider staticDataProvider_1, string symbol, double splitFactor, DateTime exDate)
        {
            this.idataHost_0.AdjustForStockSplit(staticDataProvider_1, symbol, splitFactor, exDate);
        }

        public void BeginGetData(DataSource dataSource_1, string symbol)
        {
            Delegate5 delegate2 = new Delegate5(this.GetData);
            delegate2.BeginInvoke(dataSource_1, symbol, new AsyncCallback(this.method_1), delegate2);
        }

        public bool DeleteSymbolDataFile(DataSource dataSource_1, string symbol)
        {
            bool flag = false;
            StaticDataProvider provider = this.method_3(dataSource_1);
            if (((provider != null) && provider.CanDeleteSymbolDataFile) && (symbol.Length > 0))
            {
                flag = true;
                provider.DeleteSymbolDataFile(dataSource_1, symbol);
            }
            return flag;
        }

        public void DeleteSymbolFromDataSets(string symbol, StaticDataProvider staticDataProvider_1)
        {
            this.idataHost_0.DeleteSymbolFromDataSets(symbol, staticDataProvider_1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public Bars GetData(DataSource dataSource_1, string symbol)
        {
            Bars bars4;
            if ((list_0 == null) && (string_0 != ""))
            {
                LoadSymbolInfo();
            }
            StaticDataProvider provider = this.method_3(dataSource_1);
            lock (this.object_1)
            {
                Bars bars;
                int maxBars = this.MaxBars;
                if ((this.MaxBars > 0) && ((dataSource_1.Scale != this.Scale) || (dataSource_1.IsIntraday && (dataSource_1.BarInterval != this.BarInterval))))
                {
                    switch (dataSource_1.Scale)
                    {
                        case BarScale.Daily:
                            if (this.Scale != BarScale.Weekly)
                            {
                                goto Label_00B5;
                            }
                            maxBars = this.MaxBars * 8;
                            break;

                        case BarScale.Weekly:
                            if (this.Scale != BarScale.Monthly)
                            {
                                goto Label_011A;
                            }
                            maxBars = this.MaxBars * 6;
                            break;

                        case BarScale.Monthly:
                            if (this.Scale != BarScale.Quarterly)
                            {
                                goto Label_0164;
                            }
                            maxBars = this.MaxBars * 3;
                            break;

                        case BarScale.Minute:
                            if (this.Scale != BarScale.Minute)
                            {
                                goto Label_01A8;
                            }
                            maxBars = (int) (1.2 * ((this.MaxBars * this.BarInterval) / dataSource_1.BarInterval));
                            break;

                        case BarScale.Second:
                            if (this.Scale != BarScale.Second)
                            {
                                goto Label_01D8;
                            }
                            maxBars = (int) (1.2 * ((this.MaxBars * this.BarInterval) / dataSource_1.BarInterval));
                            break;

                        case BarScale.Tick:
                            maxBars = 0;
                            break;

                        case BarScale.Quarterly:
                            if (this.Scale == BarScale.Yearly)
                            {
                                maxBars = this.MaxBars * 4;
                            }
                            break;
                    }
                }
                goto Label_01F2;
            Label_00B5:
                if (this.Scale == BarScale.Monthly)
                {
                    maxBars = this.MaxBars * 40;
                }
                else if (this.Scale == BarScale.Quarterly)
                {
                    maxBars = this.MaxBars * 120;
                }
                else if (this.Scale == BarScale.Yearly)
                {
                    maxBars = this.MaxBars * 400;
                }
                goto Label_01F2;
            Label_011A:
                if (this.Scale == BarScale.Quarterly)
                {
                    maxBars = this.MaxBars * 0x12;
                }
                else if (this.Scale == BarScale.Yearly)
                {
                    maxBars = this.MaxBars * 60;
                }
                goto Label_01F2;
            Label_0164:
                if (this.Scale == BarScale.Yearly)
                {
                    maxBars = this.MaxBars * 12;
                }
                goto Label_01F2;
            Label_01A8:
                maxBars = 0;
                goto Label_01F2;
            Label_01D8:
                maxBars = 0;
            Label_01F2:
                if (!dataSource_1.BarDataScale.CanConvertTo(this.BarDataScale) && provider.SupportsDynamicUpdate(this.Scale))
                {
                    DateTime startDate = this.StartDate;
                    if ((this.StartDate == DateTime.MinValue) && (maxBars > 0))
                    {
                        switch (this.BarDataScale.Scale)
                        {
                            case BarScale.Daily:
                                startDate = DateTime.Now.Date.AddDays((-maxBars * 1.4) * 1.2);
                                break;

                            case BarScale.Weekly:
                                startDate = DateTime.Now.Date.AddDays((-maxBars * 7) * 1.2);
                                break;

                            case BarScale.Monthly:
                                startDate = DateTime.Now.Date.AddDays((-maxBars * 30) * 1.2);
                                break;

                            case BarScale.Minute:
                            {
                                int num4 = ((maxBars / 390) + 1) * 3;
                                startDate = DateTime.Now.Date.AddDays((double) (-num4 * this.BarDataScale.BarInterval));
                                break;
                            }
                            case BarScale.Quarterly:
                                startDate = DateTime.Now.Date.AddDays((-maxBars * 90) * 1.2);
                                break;

                            case BarScale.Yearly:
                                startDate = DateTime.Now.Date.AddDays((-maxBars * 0x16d) * 1.2);
                                break;
                        }
                        if (startDate != DateTime.MinValue)
                        {
                            maxBars = 0;
                        }
                    }
                    DataSource source = new DataSource {
                        Scale = this.Scale,
                        BarInterval = this.BarInterval,
                        DSString = dataSource_1.DSString
                    };
                    bars = provider.RequestData(source, symbol, startDate, this.EndDate, maxBars, this.IncludePartialBar);
                }
                else
                {
                    bars = provider.RequestData(dataSource_1, symbol, this.StartDate, this.EndDate, maxBars, this.IncludePartialBar);
                }
                bars.MarketInfo = provider.GetMarketInfo(bars.Symbol);
                if (this.AutoConvertScale && ((this.Scale != bars.Scale) || (this.BarInterval != bars.BarInterval)))
                {
                    switch (this.Scale)
                    {
                        case BarScale.Daily:
                            bars = BarScaleConverter.ToDaily(bars);
                            break;

                        case BarScale.Weekly:
                            bars = BarScaleConverter.ToWeekly(bars);
                            break;

                        case BarScale.Monthly:
                            bars = BarScaleConverter.ToMonthly(bars);
                            break;

                        case BarScale.Minute:
                            bars = BarScaleConverter.ToIntradayCompressed(bars, BarScale.Minute, this.BarInterval);
                            break;

                        case BarScale.Second:
                            bars = BarScaleConverter.ToIntradayCompressed(bars, BarScale.Second, this.BarInterval);
                            break;

                        case BarScale.Tick:
                            bars = BarScaleConverter.ToIntradayCompressed(bars, BarScale.Tick, this.BarInterval);
                            break;

                        case BarScale.Quarterly:
                            bars = BarScaleConverter.ToQuarterly(bars);
                            break;

                        case BarScale.Yearly:
                            bars = BarScaleConverter.ToYearly(bars);
                            break;
                    }
                }
                bool flag = false;
                if (((this.StartDate != DateTime.MinValue) && (bars.Count > 0)) && (bars.Date[0] <= this.StartDate))
                {
                    flag = true;
                }
                if (((this.EndDate != DateTime.MaxValue) && (bars.Count > 0)) && (bars.Date[bars.Count - 1] >= this.EndDate))
                {
                    flag = true;
                }
                if (flag)
                {
                    Bars bars3 = new Bars(bars);
                    for (int i = 0; i < bars.Count; i++)
                    {
                        DateTime time6 = bars.Date[i];
                        if (time6.Date > this.EndDate)
                        {
                            break;
                        }
                        DateTime time7 = bars.Date[i];
                        if (time7.Date >= this.StartDate)
                        {
                            bars3.Add(bars.Date[i], bars.Open[i], bars.High[i], bars.Low[i], bars.Close[i], bars.Volume[i]);
                            foreach (DataSeries series in bars.NamedSeries)
                            {
                                DataSeries series2 = bars3.FindNamedSeries(series.Description);
                                series2[series2.Count - 1] = series[i];
                            }
                        }
                    }
                    bars = bars3;
                }
                if ((this.MaxBars > 0) && (bars.Count > this.MaxBars))
                {
                    Bars bars2 = new Bars(bars);
                    int num5 = bars.Count - this.MaxBars;
                    for (int j = num5; j < bars.Count; j++)
                    {
                        bars2.Add(bars.Date[j], bars.Open[j], bars.High[j], bars.Low[j], bars.Close[j], bars.Volume[j]);
                        foreach (DataSeries series3 in bars.NamedSeries)
                        {
                            DataSeries series4 = bars2.FindNamedSeries(series3.Description);
                            series4[series4.Count - 1] = series3[j];
                        }
                    }
                    bars = bars2;
                }
                if (bool_4)
                {
                    using (List<WealthLab.SymbolInfo>.Enumerator enumerator4 = list_0.GetEnumerator())
                    {
                        WealthLab.SymbolInfo current;
                        while (enumerator4.MoveNext())
                        {
                            current = enumerator4.Current;
                            if (current.Symbol.ToUpper() == symbol.ToUpper())
                            {
                                goto Label_07D4;
                            }
                            if (Regex.IsMatch(symbol, "^" + current.Symbol + "$"))
                            {
                                goto Label_07DE;
                            }
                        }
                        goto Label_0876;
                    Label_07D4:
                        bars.SymbolInfo = current;
                        goto Label_0876;
                    Label_07DE:
                        bars.SymbolInfo = current;
                        goto Label_0876;
                    }
                }
                using (List<WealthLab.SymbolInfo>.Enumerator enumerator2 = list_0.GetEnumerator())
                {
                    WealthLab.SymbolInfo info2;
                    while (enumerator2.MoveNext())
                    {
                        info2 = enumerator2.Current;
                        if (Regex.IsMatch(symbol, "^" + info2.Symbol + "$"))
                        {
                            goto Label_083A;
                        }
                    }
                    goto Label_0876;
                Label_083A:
                    if (bars.SymbolInfo != null)
                    {
                        bars.SymbolInfo.Tick = info2.Tick;
                        bars.SymbolInfo.Decimals = info2.Decimals;
                    }
                }
            Label_0876:
                bars4 = bars;
            }
            return bars4;
        }

        public static WealthLab.SymbolInfo GetSymbolInfo(string symbol)
        {
            WealthLab.SymbolInfo info2;
            if (string.IsNullOrEmpty(symbol))
            {
                return null;
            }
            using (IEnumerator<WealthLab.SymbolInfo> enumerator = SymbolInfo.GetEnumerator())
            {
                WealthLab.SymbolInfo current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Symbol.ToUpper() == symbol.ToUpper())
                    {
                        return current;
                    }
                    if (Regex.IsMatch(symbol, "^" + current.Symbol + "$"))
                    {
                        goto Label_0062;
                    }
                }
                return null;
            Label_0062:
                info2 = current;
            }
            return info2;
        }

        public static void LoadSymbolInfo()
        {
            if ((string_0 != "") && File.Exists(string_0 + @"\SymbolInfo.xml"))
            {
                lock (object_0)
                {
                    string path = string_0 + @"\SymbolInfo.xml";
                    XmlSerializer serializer = new XmlSerializer(typeof(List<WealthLab.SymbolInfo>));
                    TextReader textReader = new StreamReader(path);
                    try
                    {
                        list_0 = (List<WealthLab.SymbolInfo>) serializer.Deserialize(textReader);
                        foreach (WealthLab.SymbolInfo info in list_0)
                        {
                            info.Symbol = info.Symbol;
                        }
                    }
                    finally
                    {
                        textReader.Close();
                    }
                }
            }
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        private void method_1(IAsyncResult iasyncResult_0)
        {
            try
            {
                Bars bars = ((Delegate5) iasyncResult_0.AsyncState).EndInvoke(iasyncResult_0);
                if (this.eventHandler_0 != null)
                {
                    this.eventHandler_0(this, new BarsEventArgs(bars));
                }
            }
            catch (Exception exception)
            {
                if (this.eventHandler_1 != null)
                {
                    this.eventHandler_1(this, new UnhandledExceptionEventArgs(exception, false));
                }
            }
        }

        internal void method_2(DataSource dataSource_1)
        {
            this.dataSource_0 = dataSource_1;
        }

        private StaticDataProvider method_3(DataSource dataSource_1)
        {
            if (!this.AutoCreateProvider)
            {
                return dataSource_1.Provider;
            }
            StaticDataProvider provider = null;
            if (dataSource_1 != null)
            {
                Type key = dataSource_1.Provider.GetType();
                if (this.dictionary_0.ContainsKey(key))
                {
                    provider = this.dictionary_0[key];
                }
                else
                {
                    provider = (StaticDataProvider) Activator.CreateInstance(key);
                    this.idataHost_0 = dataSource_1.Provider.DataHost;
                    provider.Initialize(this);
                    this.dictionary_0.Add(key, provider);
                }
            }
            if ((provider != null) && (dataSource_1 != null))
            {
                this.staticDataProvider_0 = provider;
                this.dataSource_0 = dataSource_1;
            }
            provider.IsStreamingRequest = dataSource_1.Provider.IsStreamingRequest;
            return provider;
        }

        internal Bars method_4(string string_1)
        {
            return this.GetData(this.dataSource_0, string_1);
        }

        public static void SaveSymbolInfo()
        {
            if ((string_0 != "") && (list_0 != null))
            {
                lock (object_0)
                {
                    string path = string_0 + @"\SymbolInfo.xml";
                    XmlSerializer serializer = new XmlSerializer(typeof(List<WealthLab.SymbolInfo>));
                    TextWriter textWriter = new StreamWriter(path);
                    try
                    {
                        serializer.Serialize(textWriter, list_0);
                    }
                    finally
                    {
                        textWriter.Close();
                    }
                }
            }
        }

        public AuthenticationProvider AuthProvider
        {
            get
            {
                return this.idataHost_0.AuthProvider;
            }
        }

        public bool AutoConvertScale
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

        public bool AutoCreateProvider
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public WealthLab.BarDataScale BarDataScale
        {
            get
            {
                return new WealthLab.BarDataScale(this.Scale, this.BarInterval);
            }
            set
            {
                this.Scale = value.Scale;
                this.BarInterval = value.BarInterval;
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

        public string BaseDataFolder
        {
            get
            {
                return this.idataHost_0.BaseDataFolder;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDataHost DataHost
        {
            get
            {
                return this.idataHost_0;
            }
            set
            {
                this.idataHost_0 = value;
            }
        }

        public IList<DataSource> DataSources
        {
            get
            {
                return this.idataHost_0.DataSources;
            }
        }

        public MarketInfo DefaultMarketInfo
        {
            get
            {
                return this.idataHost_0.DefaultMarketInfo;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.dateTime_1;
            }
            set
            {
                this.dateTime_1 = value;
            }
        }

        public static bool FuturesMode
        {
            get
            {
                return bool_4;
            }
            set
            {
                bool_4 = value;
            }
        }

        public bool IncludePartialBar
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

        public int MaxBars
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

        public bool OnDemandUpdateEnabled
        {
            get
            {
                if (this.bool_1)
                {
                    return this.bool_2;
                }
                return this.idataHost_0.OnDemandUpdateEnabled;
            }
        }

        public bool OverrideOnDemand
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

        public bool OverrideOnDemandValue
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

        public static string RootPath
        {
            get
            {
                return string_0;
            }
            set
            {
                string_0 = value;
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

        public ISettingsHost SettingsHost
        {
            get
            {
                return this.idataHost_0.SettingsHost;
            }
        }

        public DateTime StartDate
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

        public static IList<WealthLab.SymbolInfo> SymbolInfo
        {
            get
            {
                return list_0;
            }
        }

        internal delegate Bars Delegate5(DataSource dataSource_0, string string_0);
    }
}

