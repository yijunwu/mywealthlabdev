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
        private BarScale barScale;
        private bool includePartialBar;
        private bool overrideOnDemand;
        private bool overrideOnDemandValue;
        private bool autoCreateProvider;
        private static bool futuresMode = true;
        private bool autoConvertScale;
        private DataSource dataSource_0;
        private DateTime startDate;
        private DateTime endDate;
        private Dictionary<Type, StaticDataProvider> dictionary_0;
        private IContainer icontainer_0;
        private IDataHost idataHost_0;
        private int barInterval;
        private int maxBars;
        private static List<WealthLab.SymbolInfo> symbolInfoList = new List<WealthLab.SymbolInfo>();
        private static object object_0 = new object();
        private object object_1;
        private StaticDataProvider staticDataProvider_0;
        private static string rootPath = "";

        private EventHandler<BarsEventArgs> eventHandler_0;
        private EventHandler<UnhandledExceptionEventArgs> eventHandler_1;

        public event EventHandler<BarsEventArgs> ASynchDataCompleted
        {
            add
            {
                EventHandler<BarsEventArgs> eventHandler;
                EventHandler<BarsEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<BarsEventArgs> eventHandler1 = (EventHandler<BarsEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<BarsEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<BarsEventArgs> eventHandler;
                EventHandler<BarsEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<BarsEventArgs> eventHandler1 = (EventHandler<BarsEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<BarsEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }

        public event EventHandler<UnhandledExceptionEventArgs> ASynchDataException
        {
            add
            {
                EventHandler<UnhandledExceptionEventArgs> eventHandler;
                EventHandler<UnhandledExceptionEventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<UnhandledExceptionEventArgs> eventHandler2 = (EventHandler<UnhandledExceptionEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<UnhandledExceptionEventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
            remove
            {
                EventHandler<UnhandledExceptionEventArgs> eventHandler;
                EventHandler<UnhandledExceptionEventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<UnhandledExceptionEventArgs> eventHandler2 = (EventHandler<UnhandledExceptionEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<UnhandledExceptionEventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
        }

        public BarsLoader()
        {
            this.startDate = DateTime.MinValue;
            this.endDate = DateTime.MaxValue;
            this.object_1 = new object();
            this.dictionary_0 = new Dictionary<Type, StaticDataProvider>();
            this.autoCreateProvider = true;
            this.autoConvertScale = true;
            this.method_0();
        }

        public BarsLoader(IContainer container)
        {
            this.startDate = DateTime.MinValue;
            this.endDate = DateTime.MaxValue;
            this.object_1 = new object();
            this.dictionary_0 = new Dictionary<Type, StaticDataProvider>();
            this.autoCreateProvider = true;
            this.autoConvertScale = true;
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

        ///WYJ fix, code from Reflector
        /*
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
        } */

        ///WYJ fix, code from JustDecompile
        /*
        public Bars GetData(DataSource dataSource_1, string symbol)
        {
            Bars marketInfo;
            StaticDataProvider staticDataProvider = null;
            Bars bar;
            lock ()
            {
                int maxBars = this.MaxBars;
                if (this.MaxBars > 0 && (dataSource_1.Scale != this.Scale || dataSource_1.IsIntraday && dataSource_1.BarInterval != this.BarInterval))
                {
                    BarScale scale = dataSource_1.Scale;
                    switch (scale)
                    {
                        case BarScale.Daily:
                        {
                            if (this.Scale != BarScale.Weekly)
                            {
                                if (this.Scale != BarScale.Monthly)
                                {
                                    if (this.Scale != BarScale.Quarterly)
                                    {
                                        if (this.Scale != BarScale.Yearly)
                                        {
                                            break;
                                        }
                                        maxBars = this.MaxBars * 400;
                                        break;
                                    }
                                    else
                                    {
                                        maxBars = this.MaxBars * 120;
                                        break;
                                    }
                                }
                                else
                                {
                                    maxBars = this.MaxBars * 40;
                                    break;
                                }
                            }
                            else
                            {
                                maxBars = this.MaxBars * 8;
                                break;
                            }
                        }
                        case BarScale.Weekly:
                        {
                            if (this.Scale != BarScale.Monthly)
                            {
                                if (this.Scale != BarScale.Quarterly)
                                {
                                    if (this.Scale != BarScale.Yearly)
                                    {
                                        break;
                                    }
                                    maxBars = this.MaxBars * 60;
                                    break;
                                }
                                else
                                {
                                    maxBars = this.MaxBars * 18;
                                    break;
                                }
                            }
                            else
                            {
                                maxBars = this.MaxBars * 6;
                                break;
                            }
                        }
                        case BarScale.Monthly:
                        {
                            if (this.Scale != BarScale.Quarterly)
                            {
                                if (this.Scale != BarScale.Yearly)
                                {
                                    break;
                                }
                                maxBars = this.MaxBars * 12;
                                break;
                            }
                            else
                            {
                                maxBars = this.MaxBars * 3;
                                break;
                            }
                        }
                        case BarScale.Minute:
                        {
                            if (this.Scale != BarScale.Minute)
                            {
                                maxBars = 0;
                                break;
                            }
                            else
                            {
                                maxBars = (int)(1.2 * (double)(this.MaxBars * this.BarInterval / dataSource_1.BarInterval));
                                break;
                            }
                        }
                        case BarScale.Second:
                        {
                            if (this.Scale != BarScale.Second)
                            {
                                maxBars = 0;
                                break;
                            }
                            else
                            {
                                maxBars = (int)(1.2 * (double)(this.MaxBars * this.BarInterval / dataSource_1.BarInterval));
                                break;
                            }
                        }
                        case BarScale.Tick:
                        {
                            maxBars = 0;
                            break;
                        }
                        case BarScale.Quarterly:
                        {
                            if (this.Scale != BarScale.Yearly)
                            {
                                break;
                            }
                            maxBars = this.MaxBars * 4;
                            break;
                        }
                    }
                }
                BarDataScale barDataScale = dataSource_1.BarDataScale;
                if (barDataScale.CanConvertTo(this.BarDataScale) || !staticDataProvider.SupportsDynamicUpdate(this.Scale))
                {
                    marketInfo = staticDataProvider.RequestData(dataSource_1, symbol, this.StartDate, this.EndDate, maxBars, this.IncludePartialBar);
                }
                else
                {
                    DateTime startDate = this.StartDate;
                    if (this.StartDate == DateTime.MinValue && maxBars > 0)
                    {
                        BarDataScale barDataScale1 = this.BarDataScale;
                        BarScale barScale = barDataScale1.Scale;
                        switch (barScale)
                        {
                            case BarScale.Daily:
                            {
                                DateTime now = DateTime.Now;
                                DateTime date = now.Date;
                                startDate = date.AddDays((double)(-maxBars) * 1.4 * 1.2);
                                goto case BarScale.Tick;
                            }
                            case BarScale.Weekly:
                            {
                                DateTime dateTime = DateTime.Now;
                                DateTime date1 = dateTime.Date;
                                startDate = date1.AddDays((double)(-maxBars * 7) * 1.2);
                                goto case BarScale.Tick;
                            }
                            case BarScale.Monthly:
                            {
                                DateTime now1 = DateTime.Now;
                                DateTime dateTime1 = now1.Date;
                                startDate = dateTime1.AddDays((double)(-maxBars * 30) * 1.2);
                                goto case BarScale.Tick;
                            }
                            case BarScale.Minute:
                            {
                                int num = (maxBars / 390 + 1) * 3;
                                DateTime now2 = DateTime.Now;
                                DateTime date2 = now2.Date;
                                BarDataScale barDataScale2 = this.BarDataScale;
                                startDate = date2.AddDays((double)(-num * barDataScale2.BarInterval));
                                goto case BarScale.Tick;
                            }
                            case BarScale.Second:
                            case BarScale.Tick:
                            {
                                if (startDate == DateTime.MinValue)
                                {
                                    break;
                                }
                                maxBars = 0;
                                break;
                            }
                            case BarScale.Quarterly:
                            {
                                DateTime dateTime2 = DateTime.Now;
                                DateTime date3 = dateTime2.Date;
                                startDate = date3.AddDays((double)(-maxBars * 90) * 1.2);
                                goto case BarScale.Tick;
                            }
                            case BarScale.Yearly:
                            {
                                DateTime now3 = DateTime.Now;
                                DateTime dateTime3 = now3.Date;
                                startDate = dateTime3.AddDays((double)(-maxBars * 365) * 1.2);
                                goto case BarScale.Tick;
                            }
                            default:
                            {
                                goto case BarScale.Tick;
                            }
                        }
                    }
                    DataSource dataSource = new DataSource();
                    dataSource.Scale = this.Scale;
                    dataSource.BarInterval = this.BarInterval;
                    dataSource.DSString = dataSource_1.DSString;
                    marketInfo = staticDataProvider.RequestData(dataSource, symbol, startDate, this.EndDate, maxBars, this.IncludePartialBar);
                }
                marketInfo.MarketInfo = staticDataProvider.GetMarketInfo(marketInfo.Symbol);
                if (this.AutoConvertScale && (this.Scale != marketInfo.Scale || this.BarInterval != marketInfo.BarInterval))
                {
                    BarScale scale1 = this.Scale;
                    switch (scale1)
                    {
                        case BarScale.Daily:
                        {
                            marketInfo = BarScaleConverter.ToDaily(marketInfo);
                            break;
                        }
                        case BarScale.Weekly:
                        {
                            marketInfo = BarScaleConverter.ToWeekly(marketInfo);
                            break;
                        }
                        case BarScale.Monthly:
                        {
                            marketInfo = BarScaleConverter.ToMonthly(marketInfo);
                            break;
                        }
                        case BarScale.Minute:
                        {
                            marketInfo = BarScaleConverter.ToIntradayCompressed(marketInfo, BarScale.Minute, this.BarInterval);
                            break;
                        }
                        case BarScale.Second:
                        {
                            marketInfo = BarScaleConverter.ToIntradayCompressed(marketInfo, BarScale.Second, this.BarInterval);
                            break;
                        }
                        case BarScale.Tick:
                        {
                            marketInfo = BarScaleConverter.ToIntradayCompressed(marketInfo, BarScale.Tick, this.BarInterval);
                            break;
                        }
                        case BarScale.Quarterly:
                        {
                            marketInfo = BarScaleConverter.ToQuarterly(marketInfo);
                            break;
                        }
                        case BarScale.Yearly:
                        {
                            marketInfo = BarScaleConverter.ToYearly(marketInfo);
                            break;
                        }
                    }
                }
                bool flag = false;
                if (this.StartDate != DateTime.MinValue && marketInfo.Count > 0 && marketInfo.Date[0] <= this.StartDate)
                {
                    flag = true;
                }
                if (this.EndDate != DateTime.MaxValue && marketInfo.Count > 0 && marketInfo.Date[marketInfo.Count - 1] >= this.EndDate)
                {
                    flag = true;
                }
                if (flag)
                {
                    Bars bar1 = new Bars(marketInfo);
                    for (int i = 0; i < marketInfo.Count; i++)
                    {
                        DateTime item = marketInfo.Date[i];
                        if (item.Date > this.EndDate)
                        {
                            break;
                        }
                        DateTime item1 = marketInfo.Date[i];
                        if (item1.Date >= this.StartDate)
                        {
                            bar1.Add(marketInfo.Date[i], marketInfo.Open[i], marketInfo.High[i], marketInfo.Low[i], marketInfo.Close[i], marketInfo.Volume[i]);
                            foreach (DataSeries namedSeries in marketInfo.NamedSeries)
                            {
                                DataSeries dataSeries = bar1.FindNamedSeries(namedSeries.Description);
                                dataSeries[dataSeries.Count - 1] = namedSeries[i];
                            }
                        }
                    }
                    marketInfo = bar1;
                }
                if (this.MaxBars > 0 && marketInfo.Count > this.MaxBars)
                {
                    Bars bar2 = new Bars(marketInfo);
                    int count = marketInfo.Count - this.MaxBars;
                    for (int j = count; j < marketInfo.Count; j++)
                    {
                        bar2.Add(marketInfo.Date[j], marketInfo.Open[j], marketInfo.High[j], marketInfo.Low[j], marketInfo.Close[j], marketInfo.Volume[j]);
                        foreach (DataSeries namedSeries1 in marketInfo.NamedSeries)
                        {
                            DataSeries dataSeries1 = bar2.FindNamedSeries(namedSeries1.Description);
                            dataSeries1[dataSeries1.Count - 1] = namedSeries1[j];
                        }
                    }
                    marketInfo = bar2;
                }
                if (!BarsLoader.bool_4)
                {
                    List<SymbolInfo>.Enumerator enumerator = BarsLoader.list_0.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            if (enumerator.MoveNext())
                            {
                                SymbolInfo current = enumerator.Current;
                                if (Regex.IsMatch(symbol, string.Concat("^", current.Symbol, "$")))
                                {
                                    if (marketInfo.SymbolInfo == null)
                                    {
                                        break;
                                    }
                                    marketInfo.SymbolInfo.Tick = current.Tick;
                                    marketInfo.SymbolInfo.Decimals = current.Decimals;
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator).Dispose();
                    }
                }
                else
                {
                    List<SymbolInfo>.Enumerator enumerator1 = BarsLoader.list_0.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            if (enumerator1.MoveNext())
                            {
                                SymbolInfo symbolInfo = enumerator1.Current;
                                if (symbolInfo.Symbol.ToUpper() == symbol.ToUpper())
                                {
                                    marketInfo.SymbolInfo = symbolInfo;
                                    break;
                                }
                                else
                                {
                                    if (Regex.IsMatch(symbol, string.Concat("^", symbolInfo.Symbol, "$")))
                                    {
                                        marketInfo.SymbolInfo = symbolInfo;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator1).Dispose();
                    }
                }
                bar = marketInfo;
            }
            return bar;
        }  */

        ///WYJ fix, code from ILSpy
        // WealthLab.BarsLoader
        public Bars GetData(DataSource dataSource_1, string symbol)
        {
            if (BarsLoader.symbolInfoList == null && BarsLoader.rootPath != "")
            {
                BarsLoader.LoadSymbolInfo();
            }
            StaticDataProvider staticDataProvider = this.method_3(dataSource_1);
            Monitor.Enter(this.object_1);
            Bars result;
            try
            {
                int num = this.MaxBars;
                if (this.MaxBars > 0 && (dataSource_1.Scale != this.Scale || (dataSource_1.IsIntraday && dataSource_1.BarInterval != this.BarInterval)))
                {
                    switch (dataSource_1.Scale)
                    {
                        case BarScale.Daily:
                            if (this.Scale == BarScale.Weekly)
                            {
                                num = this.MaxBars * 8;
                            }
                            else
                            {
                                if (this.Scale == BarScale.Monthly)
                                {
                                    num = this.MaxBars * 40;
                                }
                                else
                                {
                                    if (this.Scale == BarScale.Quarterly)
                                    {
                                        num = this.MaxBars * 120;
                                    }
                                    else
                                    {
                                        if (this.Scale == BarScale.Yearly)
                                        {
                                            num = this.MaxBars * 400;
                                        }
                                    }
                                }
                            }
                            break;
                        case BarScale.Weekly:
                            if (this.Scale == BarScale.Monthly)
                            {
                                num = this.MaxBars * 6;
                            }
                            else
                            {
                                if (this.Scale == BarScale.Quarterly)
                                {
                                    num = this.MaxBars * 18;
                                }
                                else
                                {
                                    if (this.Scale == BarScale.Yearly)
                                    {
                                        num = this.MaxBars * 60;
                                    }
                                }
                            }
                            break;
                        case BarScale.Monthly:
                            if (this.Scale == BarScale.Quarterly)
                            {
                                num = this.MaxBars * 3;
                            }
                            else
                            {
                                if (this.Scale == BarScale.Yearly)
                                {
                                    num = this.MaxBars * 12;
                                }
                            }
                            break;
                        case BarScale.Minute:
                            if (this.Scale == BarScale.Minute)
                            {
                                num = (int)(1.2 * (double)(this.MaxBars * this.BarInterval / dataSource_1.BarInterval));
                            }
                            else
                            {
                                num = 0;
                            }
                            break;
                        case BarScale.Second:
                            if (this.Scale == BarScale.Second)
                            {
                                num = (int)(1.2 * (double)(this.MaxBars * this.BarInterval / dataSource_1.BarInterval));
                            }
                            else
                            {
                                num = 0;
                            }
                            break;
                        case BarScale.Tick:
                            num = 0;
                            break;
                        case BarScale.Quarterly:
                            if (this.Scale == BarScale.Yearly)
                            {
                                num = this.MaxBars * 4;
                            }
                            break;
                    }
                }
                Bars bars;
                if (!dataSource_1.BarDataScale.CanConvertTo(this.BarDataScale) && staticDataProvider.SupportsDynamicUpdate(this.Scale))
                {
                    DateTime dateTime = this.StartDate;
                    if (this.StartDate == DateTime.MinValue && num > 0)
                    {
                        switch (this.BarDataScale.Scale)
                        {
                            case BarScale.Daily:
                                dateTime = DateTime.Now.Date.AddDays((double)(-(double)num) * 1.4 * 1.2);
                                break;
                            case BarScale.Weekly:
                                dateTime = DateTime.Now.Date.AddDays((double)(-(double)num * 7) * 1.2);
                                break;
                            case BarScale.Monthly:
                                dateTime = DateTime.Now.Date.AddDays((double)(-(double)num * 30) * 1.2);
                                break;
                            case BarScale.Minute:
                                {
                                    int num2 = (num / 390 + 1) * 3;
                                    dateTime = DateTime.Now.Date.AddDays((double)(-(double)num2 * this.BarDataScale.BarInterval));
                                    break;
                                }
                            case BarScale.Quarterly:
                                dateTime = DateTime.Now.Date.AddDays((double)(-(double)num * 90) * 1.2);
                                break;
                            case BarScale.Yearly:
                                dateTime = DateTime.Now.Date.AddDays((double)(-(double)num * 365) * 1.2);
                                break;
                        }
                        if (dateTime != DateTime.MinValue)
                        {
                            num = 0;
                        }
                    }
                    bars = staticDataProvider.RequestData(new DataSource
                    {
                        Scale = this.Scale,
                        BarInterval = this.BarInterval,
                        DSString = dataSource_1.DSString
                    }, symbol, dateTime, this.EndDate, num, this.IncludePartialBar);
                }
                else
                {
                    bars = staticDataProvider.RequestData(dataSource_1, symbol, this.StartDate, this.EndDate, num, this.IncludePartialBar);
                }
                bars.MarketInfo = staticDataProvider.GetMarketInfo(bars.Symbol);
                if (this.AutoConvertScale && (this.Scale != bars.Scale || this.BarInterval != bars.BarInterval))
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
                if (this.StartDate != DateTime.MinValue && bars.Count > 0 && bars.Date[0] <= this.StartDate)
                {
                    flag = true;
                }
                if (this.EndDate != DateTime.MaxValue && bars.Count > 0 && bars.Date[bars.Count - 1] >= this.EndDate)
                {
                    flag = true;
                }
                if (flag)
                {
                    Bars bars2 = new Bars(bars);
                    int num3 = 0;
                    while (num3 < bars.Count && !(bars.Date[num3].Date > this.EndDate))
                    {
                        if (bars.Date[num3].Date >= this.StartDate)
                        {
                            bars2.Add(bars.Date[num3], bars.Open[num3], bars.High[num3], bars.Low[num3], bars.Close[num3], bars.Volume[num3]);
                            foreach (DataSeries current in bars.NamedSeries)
                            {
                                DataSeries dataSeries = bars2.FindNamedSeries(current.Description);
                                dataSeries[dataSeries.Count - 1] = current[num3];
                            }
                        }
                        num3++;
                    }
                    bars = bars2;
                }
                if (this.MaxBars > 0 && bars.Count > this.MaxBars)
                {
                    Bars bars3 = new Bars(bars);
                    int num4 = bars.Count - this.MaxBars;
                    for (int i = num4; i < bars.Count; i++)
                    {
                        bars3.Add(bars.Date[i], bars.Open[i], bars.High[i], bars.Low[i], bars.Close[i], bars.Volume[i]);
                        foreach (DataSeries current2 in bars.NamedSeries)
                        {
                            DataSeries dataSeries2 = bars3.FindNamedSeries(current2.Description);
                            dataSeries2[dataSeries2.Count - 1] = current2[i];
                        }
                    }
                    bars = bars3;
                }
                if (BarsLoader.futuresMode)
                {
                    using (List<SymbolInfo>.Enumerator enumerator3 = BarsLoader.symbolInfoList.GetEnumerator())
                    {
                        while (enumerator3.MoveNext())
                        {
                            SymbolInfo current3 = enumerator3.Current;
                            if (!(current3.Symbol.ToUpper() == symbol.ToUpper()))
                            {
                                if (!Regex.IsMatch(symbol, "^" + current3.Symbol + "$"))
                                {
                                    continue;
                                }
                                bars.SymbolInfo = current3;
                            }
                            else
                            {
                                bars.SymbolInfo = current3;
                            }
                            break;
                        }
                    }
                    result = bars;   ///WYJ fix, replace goto with more friendly statements
                    return result;
                }
                foreach (SymbolInfo current4 in BarsLoader.symbolInfoList)
                {
                    if (Regex.IsMatch(symbol, "^" + current4.Symbol + "$"))
                    {
                        if (bars.SymbolInfo != null)
                        {
                            bars.SymbolInfo.Tick = current4.Tick;
                            bars.SymbolInfo.Decimals = current4.Decimals;
                        }
                        break;
                    }
                }
                result = bars;
            }
            finally
            {
                Monitor.Exit(this.object_1);
            }
            return result;
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
                        ///goto  Label_0062;  ///WYJ fix, simplify the flow
                        info2 = current;
                        return info2;
                    }
                }
                return null;
            }
        }

        public static void LoadSymbolInfo()
        {
            if ((rootPath != "") && File.Exists(rootPath + @"\SymbolInfo.xml"))
            {
                lock (object_0)
                {
                    string path = rootPath + @"\SymbolInfo.xml";
                    XmlSerializer serializer = new XmlSerializer(typeof(List<WealthLab.SymbolInfo>));
                    TextReader textReader = new StreamReader(path);
                    try
                    {
                        symbolInfoList = (List<WealthLab.SymbolInfo>) serializer.Deserialize(textReader);
                        foreach (WealthLab.SymbolInfo info in symbolInfoList)
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

        ///WYJ fix, original signature: internal Bars method_4(string string_1)
        internal Bars GetData(string symbol)
        {
            return this.GetData(this.dataSource_0, symbol);
        }

        public static void SaveSymbolInfo()
        {
            if ((rootPath != "") && (symbolInfoList != null))
            {
                lock (object_0)
                {
                    string path = rootPath + @"\SymbolInfo.xml";
                    XmlSerializer serializer = new XmlSerializer(typeof(List<WealthLab.SymbolInfo>));
                    TextWriter textWriter = new StreamWriter(path);
                    try
                    {
                        serializer.Serialize(textWriter, symbolInfoList);
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
                return this.autoConvertScale;
            }
            set
            {
                this.autoConvertScale = value;
            }
        }

        public bool AutoCreateProvider
        {
            get
            {
                return this.autoCreateProvider;
            }
            set
            {
                this.autoCreateProvider = value;
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
                return this.barInterval;
            }
            set
            {
                this.barInterval = value;
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
                return this.endDate;
            }
            set
            {
                this.endDate = value;
            }
        }

        public static bool FuturesMode
        {
            get
            {
                return futuresMode;
            }
            set
            {
                futuresMode = value;
            }
        }

        public bool IncludePartialBar
        {
            get
            {
                return this.includePartialBar;
            }
            set
            {
                this.includePartialBar = value;
            }
        }

        public int MaxBars
        {
            get
            {
                return this.maxBars;
            }
            set
            {
                this.maxBars = value;
            }
        }

        public bool OnDemandUpdateEnabled
        {
            get
            {
                if (this.overrideOnDemand)
                {
                    return this.overrideOnDemandValue;
                }
                return this.idataHost_0.OnDemandUpdateEnabled;
            }
        }

        public bool OverrideOnDemand
        {
            get
            {
                return this.overrideOnDemand;
            }
            set
            {
                this.overrideOnDemand = value;
            }
        }

        public bool OverrideOnDemandValue
        {
            get
            {
                return this.overrideOnDemandValue;
            }
            set
            {
                this.overrideOnDemandValue = value;
            }
        }

        public static string RootPath
        {
            get
            {
                return rootPath;
            }
            set
            {
                rootPath = value;
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
                return this.startDate;
            }
            set
            {
                this.startDate = value;
            }
        }

        public static IList<WealthLab.SymbolInfo> SymbolInfo
        {
            get
            {
                return symbolInfoList;
            }
        }

        internal delegate Bars Delegate5(DataSource dataSource_0, string string_0);
    }
}

