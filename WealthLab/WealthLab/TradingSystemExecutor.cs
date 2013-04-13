namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;

    [ToolboxBitmap(typeof(TradingSystemExecutor), "TradingSystemExecutor")]
    public class TradingSystemExecutor : Component, IComparer<Position>, INotifier
    {
        public List<Position> _activePositions;
        private Bars bars_0;
        private WealthLab.BarsLoader barsLoader_0;
        private static bool bool_0 = false;
        private bool bool_1;
        private bool bool_10;
        private bool bool_11;
        private bool bool_12;
        private bool bool_13;
        private bool bool_14;
        private bool bool_15;
        private bool bool_16;
        private static bool bool_17 = false;
        [CompilerGenerated]
        private bool bool_18;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private bool bool_6;
        private bool bool_7;
        private bool bool_8;
        private bool bool_9;
        private ChartRenderer chartRenderer_0;
        private WealthLab.Commission commission_0;
        private DataSource dataSource_0;
        private Dictionary<string, Bars> dictionary_0;
        private double double_0;
        private double double_1;
        private double double_2;
        private double double_3;
        private double double_4;
        private double double_5;
        private double double_6;
        private double double_7;
        private double double_8;
        private static double double_9 = 1.0;
        private WealthLab.FundamentalsLoader fundamentalsLoader_0;
        private IContainer icontainer_0;
        private IList<Bars> ilist_0;
        private int int_0;
        private static int int_1 = 0;
        [CompilerGenerated]
        private int int_2;
        private List<string> list_0;
        private List<Bars> list_1;
        private List<Bars> list_2;
        private List<Position> list_3;
        private List<Alert> list_4;
        private List<Position> list_5;
        private List<Alert> list_6;
        private List<Bars> list_7;
        [CompilerGenerated]
        private object object_0;
        private Position position_0;
        private PositionSize positionSize_0;
        private static PositionSize positionSize_1 = new PositionSize(PosSizeMode.RawProfitShare, 1.0);
        private WealthLab.PosSizer posSizer_0;
        public static List<WealthLab.PosSizer> PosSizers = null;
        [CompilerGenerated]
        private WealthLab.Strategy strategy_0;
        public int StrategyWindowID;
        private string string_0;
        private string string_1;
        private SystemPerformance systemPerformance_0;
        private WealthScript wealthScript_0;

        private EventHandler<StrategyEventArgs> eventHandler_0;

        private EventHandler<DataSourceLookupEventArgs> eventHandler_1;

        private EventHandler<LoadSymbolEventArgs> eventHandler_2;

        private EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler_3;

        private EventHandler<BarsEventArgs> eventHandler_4;

        private EventHandler<BarsEventArgs> eventHandler_5;

        private EventHandler<WSExceptionEventArgs> eventHandler_6;

        private EventHandler<EventArgs> eventHandler_7;

        private EventHandler<EventArgs> eventHandler_8;

        private EventHandler<DebugStringEventArgs> eventHandler_9;

        private EventHandler<ChartBitmapEventArgs> eventHandler_10;

        private EventHandler<TrendLineEventArgs> eventHandler_11;

        private EventHandler<StrategyParameterEventArgs> eventHandler_12;

        public event EventHandler<ChartBitmapEventArgs> ChartBitmapRequested
        {
            add
            {
                EventHandler<ChartBitmapEventArgs> eventHandler;
                EventHandler<ChartBitmapEventArgs> eventHandler10 = this.eventHandler_10;
                do
                {
                    eventHandler = eventHandler10;
                    EventHandler<ChartBitmapEventArgs> eventHandler1 = (EventHandler<ChartBitmapEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler10 = Interlocked.CompareExchange<EventHandler<ChartBitmapEventArgs>>(ref this.eventHandler_10, eventHandler1, eventHandler);
                }
                while (eventHandler10 != eventHandler);
            }
            remove
            {
                EventHandler<ChartBitmapEventArgs> eventHandler;
                EventHandler<ChartBitmapEventArgs> eventHandler10 = this.eventHandler_10;
                do
                {
                    eventHandler = eventHandler10;
                    EventHandler<ChartBitmapEventArgs> eventHandler1 = (EventHandler<ChartBitmapEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler10 = Interlocked.CompareExchange<EventHandler<ChartBitmapEventArgs>>(ref this.eventHandler_10, eventHandler1, eventHandler);
                }
                while (eventHandler10 != eventHandler);
            }
        }

        public event EventHandler<EventArgs> ClearDebugWindow
        {
            add
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler8 = this.eventHandler_8;
                do
                {
                    eventHandler = eventHandler8;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler8 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_8, eventHandler1, eventHandler);
                }
                while (eventHandler8 != eventHandler);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler8 = this.eventHandler_8;
                do
                {
                    eventHandler = eventHandler8;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler8 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_8, eventHandler1, eventHandler);
                }
                while (eventHandler8 != eventHandler);
            }
        }

        public event EventHandler<BarsEventArgs> ExecutionCompletedForChildStrategySymbol
        {
            add
            {
                EventHandler<BarsEventArgs> eventHandler;
                EventHandler<BarsEventArgs> eventHandler5 = this.eventHandler_5;
                do
                {
                    eventHandler = eventHandler5;
                    EventHandler<BarsEventArgs> eventHandler1 = (EventHandler<BarsEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler5 = Interlocked.CompareExchange<EventHandler<BarsEventArgs>>(ref this.eventHandler_5, eventHandler1, eventHandler);
                }
                while (eventHandler5 != eventHandler);
            }
            remove
            {
                EventHandler<BarsEventArgs> eventHandler;
                EventHandler<BarsEventArgs> eventHandler5 = this.eventHandler_5;
                do
                {
                    eventHandler = eventHandler5;
                    EventHandler<BarsEventArgs> eventHandler1 = (EventHandler<BarsEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler5 = Interlocked.CompareExchange<EventHandler<BarsEventArgs>>(ref this.eventHandler_5, eventHandler1, eventHandler);
                }
                while (eventHandler5 != eventHandler);
            }
        }

        public event EventHandler<BarsEventArgs> ExecutionCompletedForSymbol
        {
            add
            {
                EventHandler<BarsEventArgs> eventHandler;
                EventHandler<BarsEventArgs> eventHandler4 = this.eventHandler_4;
                do
                {
                    eventHandler = eventHandler4;
                    EventHandler<BarsEventArgs> eventHandler1 = (EventHandler<BarsEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler4 = Interlocked.CompareExchange<EventHandler<BarsEventArgs>>(ref this.eventHandler_4, eventHandler1, eventHandler);
                }
                while (eventHandler4 != eventHandler);
            }
            remove
            {
                EventHandler<BarsEventArgs> eventHandler;
                EventHandler<BarsEventArgs> eventHandler4 = this.eventHandler_4;
                do
                {
                    eventHandler = eventHandler4;
                    EventHandler<BarsEventArgs> eventHandler1 = (EventHandler<BarsEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler4 = Interlocked.CompareExchange<EventHandler<BarsEventArgs>>(ref this.eventHandler_4, eventHandler1, eventHandler);
                }
                while (eventHandler4 != eventHandler);
            }
        }

        public event EventHandler<LoadSymbolFromDataSetEventArgs> ExternalSymbolFromDataSetRequested
        {
            add
            {
                EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler;
                EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler3 = this.eventHandler_3;
                do
                {
                    eventHandler = eventHandler3;
                    EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler1 = (EventHandler<LoadSymbolFromDataSetEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler3 = Interlocked.CompareExchange<EventHandler<LoadSymbolFromDataSetEventArgs>>(ref this.eventHandler_3, eventHandler1, eventHandler);
                }
                while (eventHandler3 != eventHandler);
            }
            remove
            {
                EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler;
                EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler3 = this.eventHandler_3;
                do
                {
                    eventHandler = eventHandler3;
                    EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler1 = (EventHandler<LoadSymbolFromDataSetEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler3 = Interlocked.CompareExchange<EventHandler<LoadSymbolFromDataSetEventArgs>>(ref this.eventHandler_3, eventHandler1, eventHandler);
                }
                while (eventHandler3 != eventHandler);
            }
        }

        public event EventHandler<LoadSymbolEventArgs> ExternalSymbolRequested
        {
            add
            {
                EventHandler<LoadSymbolEventArgs> eventHandler;
                EventHandler<LoadSymbolEventArgs> eventHandler2 = this.eventHandler_2;
                do
                {
                    eventHandler = eventHandler2;
                    EventHandler<LoadSymbolEventArgs> eventHandler1 = (EventHandler<LoadSymbolEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler2 = Interlocked.CompareExchange<EventHandler<LoadSymbolEventArgs>>(ref this.eventHandler_2, eventHandler1, eventHandler);
                }
                while (eventHandler2 != eventHandler);
            }
            remove
            {
                EventHandler<LoadSymbolEventArgs> eventHandler;
                EventHandler<LoadSymbolEventArgs> eventHandler2 = this.eventHandler_2;
                do
                {
                    eventHandler = eventHandler2;
                    EventHandler<LoadSymbolEventArgs> eventHandler1 = (EventHandler<LoadSymbolEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler2 = Interlocked.CompareExchange<EventHandler<LoadSymbolEventArgs>>(ref this.eventHandler_2, eventHandler1, eventHandler);
                }
                while (eventHandler2 != eventHandler);
            }
        }

        public event EventHandler<EventArgs> FlushDebugWindow
        {
            add
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler7 = this.eventHandler_7;
                do
                {
                    eventHandler = eventHandler7;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler7 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_7, eventHandler1, eventHandler);
                }
                while (eventHandler7 != eventHandler);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler7 = this.eventHandler_7;
                do
                {
                    eventHandler = eventHandler7;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler7 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_7, eventHandler1, eventHandler);
                }
                while (eventHandler7 != eventHandler);
            }
        }

        public event EventHandler<DataSourceLookupEventArgs> LookupDataSource
        {
            add
            {
                EventHandler<DataSourceLookupEventArgs> eventHandler;
                EventHandler<DataSourceLookupEventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<DataSourceLookupEventArgs> eventHandler2 = (EventHandler<DataSourceLookupEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<DataSourceLookupEventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
            remove
            {
                EventHandler<DataSourceLookupEventArgs> eventHandler;
                EventHandler<DataSourceLookupEventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<DataSourceLookupEventArgs> eventHandler2 = (EventHandler<DataSourceLookupEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<DataSourceLookupEventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
        }

        public event EventHandler<StrategyEventArgs> LookupStrategy
        {
            add
            {
                EventHandler<StrategyEventArgs> eventHandler;
                EventHandler<StrategyEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<StrategyEventArgs> eventHandler1 = (EventHandler<StrategyEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<StrategyEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<StrategyEventArgs> eventHandler;
                EventHandler<StrategyEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<StrategyEventArgs> eventHandler1 = (EventHandler<StrategyEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<StrategyEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }

        public event EventHandler<DebugStringEventArgs> PrintToStatusBar
        {
            add
            {
                EventHandler<DebugStringEventArgs> eventHandler;
                EventHandler<DebugStringEventArgs> eventHandler9 = this.eventHandler_9;
                do
                {
                    eventHandler = eventHandler9;
                    EventHandler<DebugStringEventArgs> eventHandler1 = (EventHandler<DebugStringEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler9 = Interlocked.CompareExchange<EventHandler<DebugStringEventArgs>>(ref this.eventHandler_9, eventHandler1, eventHandler);
                }
                while (eventHandler9 != eventHandler);
            }
            remove
            {
                EventHandler<DebugStringEventArgs> eventHandler;
                EventHandler<DebugStringEventArgs> eventHandler9 = this.eventHandler_9;
                do
                {
                    eventHandler = eventHandler9;
                    EventHandler<DebugStringEventArgs> eventHandler1 = (EventHandler<DebugStringEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler9 = Interlocked.CompareExchange<EventHandler<DebugStringEventArgs>>(ref this.eventHandler_9, eventHandler1, eventHandler);
                }
                while (eventHandler9 != eventHandler);
            }
        }

        public event EventHandler<StrategyParameterEventArgs> SetParameterValues
        {
            add
            {
                EventHandler<StrategyParameterEventArgs> eventHandler;
                EventHandler<StrategyParameterEventArgs> eventHandler12 = this.eventHandler_12;
                do
                {
                    eventHandler = eventHandler12;
                    EventHandler<StrategyParameterEventArgs> eventHandler1 = (EventHandler<StrategyParameterEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler12 = Interlocked.CompareExchange<EventHandler<StrategyParameterEventArgs>>(ref this.eventHandler_12, eventHandler1, eventHandler);
                }
                while (eventHandler12 != eventHandler);
            }
            remove
            {
                EventHandler<StrategyParameterEventArgs> eventHandler;
                EventHandler<StrategyParameterEventArgs> eventHandler12 = this.eventHandler_12;
                do
                {
                    eventHandler = eventHandler12;
                    EventHandler<StrategyParameterEventArgs> eventHandler1 = (EventHandler<StrategyParameterEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler12 = Interlocked.CompareExchange<EventHandler<StrategyParameterEventArgs>>(ref this.eventHandler_12, eventHandler1, eventHandler);
                }
                while (eventHandler12 != eventHandler);
            }
        }

        public event EventHandler<TrendLineEventArgs> TrendlineGetValue
        {
            add
            {
                EventHandler<TrendLineEventArgs> eventHandler;
                EventHandler<TrendLineEventArgs> eventHandler11 = this.eventHandler_11;
                do
                {
                    eventHandler = eventHandler11;
                    EventHandler<TrendLineEventArgs> eventHandler1 = (EventHandler<TrendLineEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler11 = Interlocked.CompareExchange<EventHandler<TrendLineEventArgs>>(ref this.eventHandler_11, eventHandler1, eventHandler);
                }
                while (eventHandler11 != eventHandler);
            }
            remove
            {
                EventHandler<TrendLineEventArgs> eventHandler;
                EventHandler<TrendLineEventArgs> eventHandler11 = this.eventHandler_11;
                do
                {
                    eventHandler = eventHandler11;
                    EventHandler<TrendLineEventArgs> eventHandler1 = (EventHandler<TrendLineEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler11 = Interlocked.CompareExchange<EventHandler<TrendLineEventArgs>>(ref this.eventHandler_11, eventHandler1, eventHandler);
                }
                while (eventHandler11 != eventHandler);
            }
        }

        public event EventHandler<WSExceptionEventArgs> WealthScriptException
        {
            add
            {
                EventHandler<WSExceptionEventArgs> eventHandler;
                EventHandler<WSExceptionEventArgs> eventHandler6 = this.eventHandler_6;
                do
                {
                    eventHandler = eventHandler6;
                    EventHandler<WSExceptionEventArgs> eventHandler1 = (EventHandler<WSExceptionEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler6 = Interlocked.CompareExchange<EventHandler<WSExceptionEventArgs>>(ref this.eventHandler_6, eventHandler1, eventHandler);
                }
                while (eventHandler6 != eventHandler);
            }
            remove
            {
                EventHandler<WSExceptionEventArgs> eventHandler;
                EventHandler<WSExceptionEventArgs> eventHandler6 = this.eventHandler_6;
                do
                {
                    eventHandler = eventHandler6;
                    EventHandler<WSExceptionEventArgs> eventHandler1 = (EventHandler<WSExceptionEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler6 = Interlocked.CompareExchange<EventHandler<WSExceptionEventArgs>>(ref this.eventHandler_6, eventHandler1, eventHandler);
                }
                while (eventHandler6 != eventHandler);
            }
        }

        public TradingSystemExecutor()
        {
            this.bool_1 = true;
            this.double_0 = 1.0;
            this.int_0 = 1;
            this.list_0 = new List<string>();
            this._activePositions = new List<Position>();
            this.string_1 = "";
            this.double_7 = 10.0;
            this.bool_16 = true;
            this.list_1 = new List<Bars>();
            this.list_2 = new List<Bars>();
            this.positionSize_0 = new PositionSize();
            this.list_3 = new List<Position>();
            this.list_4 = new List<Alert>();
            this.list_5 = new List<Position>();
            this.list_6 = new List<Alert>();
            this.dictionary_0 = new Dictionary<string, Bars>();
            this.list_7 = new List<Bars>();
            this.systemPerformance_0 = new SystemPerformance(null);
            this.method_21();
        }

        public TradingSystemExecutor(IContainer container)
        {
            this.bool_1 = true;
            this.double_0 = 1.0;
            this.int_0 = 1;
            this.list_0 = new List<string>();
            this._activePositions = new List<Position>();
            this.string_1 = "";
            this.double_7 = 10.0;
            this.bool_16 = true;
            this.list_1 = new List<Bars>();
            this.list_2 = new List<Bars>();
            this.positionSize_0 = new PositionSize();
            this.list_3 = new List<Position>();
            this.list_4 = new List<Alert>();
            this.list_5 = new List<Position>();
            this.list_6 = new List<Alert>();
            this.dictionary_0 = new Dictionary<string, Bars>();
            this.list_7 = new List<Bars>();
            this.systemPerformance_0 = new SystemPerformance(null);
            container.Add(this);
            this.method_21();
        }

        public void ApplyPositionSize()
        {
            TradingSystemExecutor executor = new TradingSystemExecutor();
            if (this.BenchmarkBuyAndHoldON)
            {
                if (this.DataSet != null)
                {
                    executor.ApplySettings(this);
                    Bars source = this.DataSet.Provider.RequestData(this.DataSet, this.BenchmarkSymbol, DateTime.MinValue, DateTime.MaxValue, 0, false);
                    if (source.Count == 0)
                    {
                        source = this.method_10(this.BenchmarkSymbol, false);
                    }
                    source.SymbolInfo.SecurityType = SecurityType.MutualFund;
                    int count = this.ilist_0.Count;
                    DateTime minValue = DateTime.MinValue;
                    minValue = this.ilist_0[0].Date[0];
                    int num2 = 0;
                    while (count-- > 0)
                    {
                        if ((this.ilist_0[count].Count > 0) && (this.ilist_0[count].Date[0] < minValue))
                        {
                            num2 = count;
                        }
                    }
                    if (source.Count != 0)
                    {
                        source = BarScaleConverter.Synchronize(source, this.ilist_0[num2]);
                    }
                    Bars[] barsArray = new Bars[] { source };
                    executor.ilist_0 = barsArray;
                    foreach (Bars bars6 in executor.ilist_0)
                    {
                        if (bars6.Count > 0)
                        {
                            executor.Performance.method_1(bars6);
                        }
                    }
                    this.Performance.BenchmarkSymbolbars = source;
                }
            }
            else
            {
                this.Performance.BenchmarkSymbolbars = null;
            }
            if (this.ilist_0 == null)
            {
                return;
            }
            this.systemPerformance_0.RawTrades = this.list_3;
            this.systemPerformance_0.PositionSize = this.PosSize;
            SystemResults results = new SystemResults(this.systemPerformance_0);
            if (!this.BenchmarkBuyAndHoldON)
            {
                foreach (Position position4 in this.systemPerformance_0.ResultsBuyHold.Positions)
                {
                    results.method_4(position4);
                }
            }
            int tradesNSF = this.Performance.Results.TradesNSF;
            int num4 = this.Performance.ResultsLong.TradesNSF;
            int num5 = this.Performance.ResultsShort.TradesNSF;
            this.systemPerformance_0.method_2();
            if (this.Strategy.StrategyType == StrategyType.CombinedStrategy)
            {
                this.Performance.Results.TradesNSF = tradesNSF;
                this.Performance.ResultsLong.TradesNSF = num4;
                this.Performance.ResultsShort.TradesNSF = num5;
            }
            if ((this.Strategy.StrategyType == StrategyType.CombinedStrategy) && !this.BenchmarkBuyAndHoldON)
            {
                foreach (Position position2 in results.Positions)
                {
                    this.systemPerformance_0.ResultsBuyHold.method_4(position2);
                }
            }
            if (this.ApplyInterest)
            {
                this.systemPerformance_0.CashReturnRate = this.CashRate;
            }
            else
            {
                this.systemPerformance_0.CashReturnRate = 0.0;
            }
            this.bool_11 = false;
            this.posSizer_0 = null;
            if (this.PosSize.Mode == PosSizeMode.SimuScript)
            {
                using (List<WealthLab.PosSizer>.Enumerator enumerator10 = PosSizers.GetEnumerator())
                {
                    WealthLab.PosSizer current;
                    while (enumerator10.MoveNext())
                    {
                        current = enumerator10.Current;
                        if (current.FriendlyName == this.PosSize.SimuScriptName)
                        {
                            ///goto  Label_036D;  WYJ fix, simplify the flow
                            this.posSizer_0 = (WealthLab.PosSizer)Activator.CreateInstance(current.GetType());
                            if ((this.PosSize.PosSizerConfig != "") && ((this.PosSize.SimuScriptName == this.PosSize.PosSizerThatWasConfigured) || (this.PosSize.PosSizerThatWasConfigured == "")))
                            {
                                try
                                {
                                    this.posSizer_0.ApplyConfigString(WealthLab.PosSizer.ParseConfigString(this.PosSize.PosSizerConfig));
                                }
                                catch
                                {
                                }
                            }
                            break;
                        }
                    }
                    ///goto  Label_0400; ///WYJ fix, simplify the code
                }
            }
        ///Label_0400: ///WYJ fix, simplify the code
            foreach (Bars bars7 in this.ilist_0)
            {
                this.systemPerformance_0.method_1(bars7);
            }
            this.systemPerformance_0.Results.BuildEquityCurve(this.ilist_0, this, true, this.posSizer_0);
            this.systemPerformance_0.Results.method_7(true);
            foreach (Position position in this.list_3)
            {
                if (position.Shares > 0.0)
                {
                    this.systemPerformance_0.Results.method_4(position);
                    if (position.PositionType == PositionType.Long)
                    {
                        this.systemPerformance_0.ResultsLong.method_4(position);
                    }
                    else
                    {
                        this.systemPerformance_0.ResultsShort.method_4(position);
                    }
                }
                else
                {
                    SystemResults results1 = this.systemPerformance_0.Results;
                    results1.TradesNSF++;
                    if (position.PositionType == PositionType.Long)
                    {
                        SystemResults resultsLong = this.systemPerformance_0.ResultsLong;
                        resultsLong.TradesNSF++;
                    }
                    else
                    {
                        SystemResults resultsShort = this.systemPerformance_0.ResultsShort;
                        resultsShort.TradesNSF++;
                    }
                }
            }
            foreach (Alert alert2 in this.list_4)
            {
                if ((alert2.AlertType != TradeType.Buy) && (alert2.AlertType != TradeType.Short))
                {
                    if ((alert2.Position != null) && (alert2.Position.Shares > 0.0))
                    {
                        this.systemPerformance_0.Results.method_5(alert2);
                    }
                }
                else
                {
                    this.systemPerformance_0.Results.method_5(alert2);
                }
            }
            bool reduceQtyBasedOnVolume = this.ReduceQtyBasedOnVolume;
            this.ReduceQtyBasedOnVolume = false;
            int num = 0;
            if (!this.BenchmarkBuyAndHoldON)
            {
                if (this.Strategy.StrategyType != StrategyType.CombinedStrategy)
                {
                    foreach (Bars bars in this.ilist_0)
                    {
                        if (bars.Count > 1)
                        {
                            num++;
                        }
                    }
                    foreach (Bars bars5 in this.ilist_0)
                    {
                        if (bars5.Count > 1)
                        {
                            Position position5 = new Position(bars5, PositionType.Long, this.Strategy.ID.ToString());
                            if (this.PosSize.RawProfitMode)
                            {
                                position5.Shares = this.CalcPositionSize(bars5, 0, bars5.Close[0], PositionType.Long, 0.0, (double) 0.0);
                            }
                            else
                            {
                                double num9 = (this.PosSize.StartingCapital * this.PosSize.MarginFactor) / ((double) num);
                                position5.Shares = this.method_3(bars5, num9);
                            }
                            if (position5.Shares > 0.0)
                            {
                                position5.EntryBar = 1;
                                position5.EntryPrice = bars5.Open[1];
                                position5.BasisPrice = bars5.Close[0];
                                this.systemPerformance_0.ResultsBuyHold.method_4(position5);
                                if ((this.Commission != null) && this.ApplyCommission)
                                {
                                    position5.EntryCommission = this.Commission.Calculate(TradeType.Buy, OrderType.Market, position5.EntryPrice, position5.Shares, bars5);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Bars bars3 in executor.ilist_0)
                {
                    if (bars3.Count > 1)
                    {
                        num++;
                    }
                }
                foreach (Bars bars4 in executor.ilist_0)
                {
                    if (bars4.Count <= 1)
                    {
                        continue;
                    }
                    Position position3 = new Position(bars4, PositionType.Long, this.Strategy.ID.ToString());
                    int num6 = 0;
                    while (num6 < bars4.Count)
                    {
                        if (bars4.Close[num6] != 0.0)
                        {
                            break;
                        }
                        num6++;
                    }
                    if (this.PosSize.RawProfitMode)
                    {
                        position3.Shares = this.CalcPositionSize(bars4, num6 + 1, bars4.Close[num6], PositionType.Long, 0.0, (double) 0.0);
                    }
                    else
                    {
                        double num7 = (this.PosSize.StartingCapital * this.PosSize.MarginFactor) / ((double) num);
                        position3.Shares = this.method_3(bars4, num7);
                    }
                    if (position3.Shares > 0.0)
                    {
                        position3.EntryBar = num6 + 1;
                        position3.EntryPrice = bars4.Open[num6 + 1];
                        position3.BasisPrice = bars4.Close[num6];
                        this.systemPerformance_0.ResultsBuyHold.method_4(position3);
                        if ((this.Commission != null) && this.ApplyCommission)
                        {
                            position3.EntryCommission = this.Commission.Calculate(TradeType.Buy, OrderType.Market, position3.EntryPrice, position3.Shares, bars4);
                        }
                    }
                }
            }
            this.ReduceQtyBasedOnVolume = reduceQtyBasedOnVolume;
            this.systemPerformance_0.ResultsLong.BuildEquityCurve(this.ilist_0, this, false, this.posSizer_0);
            this.systemPerformance_0.ResultsShort.BuildEquityCurve(this.ilist_0, this, false, this.posSizer_0);
            this.systemPerformance_0.ResultsBuyHold.method_8();
            if (!this.BenchmarkBuyAndHoldON)
            {
                this.systemPerformance_0.ResultsBuyHold.BuildEquityCurve(this.ilist_0, this, false, null);
            }
            else
            {
                if (executor.ilist_0[0].Count == 0)
                {
                    executor.ilist_0[0] = this.ilist_0[0];
                }
                this.systemPerformance_0.ResultsBuyHold.BuildEquityCurve(executor.ilist_0, executor, false, null);
                this.systemPerformance_0.ResultsBuyHold.EquityCurve = BarScaleConverter.Synchronize(this.systemPerformance_0.ResultsBuyHold.EquityCurve, this.systemPerformance_0.ResultsLong.EquityCurve);
                this.systemPerformance_0.ResultsBuyHold.CashCurve = BarScaleConverter.Synchronize(this.systemPerformance_0.ResultsBuyHold.CashCurve, this.systemPerformance_0.ResultsLong.CashCurve);
            }
            if (this.posSizer_0 != null)
            {
                this.systemPerformance_0.Results.method_9(this.posSizer_0);
            }
            foreach (Alert alert in this.systemPerformance_0.Results.Alerts)
            {
                if ((alert.AlertType != TradeType.Buy) && (alert.AlertType != TradeType.Short))
                {
                    if (alert.Position != null)
                    {
                        alert.Shares = alert.Position.Shares;
                    }
                }
                else if (this.PosSize.Mode != PosSizeMode.ScriptOverride)
                {
                    alert.Shares = this.CalcPositionSize(alert.Bars, alert.Bars.Count, alert.BasisPrice, alert.PositionType, alert.RiskStopLevel);
                }
            }
            this.Performance.method_0();
        }

        public void ApplySettings(TradingSystemExecutor executor)
        {
            this.ApplyCommission = executor.ApplyCommission;
            this.Commission = executor.Commission;
            this.PosSize = executor.PosSize;
            this.EnableSlippage = executor.EnableSlippage;
            this.LimitOrderSlippage = executor.LimitOrderSlippage;
            this.SlippageUnits = executor.SlippageUnits;
            this.SlippageTicks = executor.SlippageTicks;
            this.RoundLots = executor.RoundLots;
            this.RoundLots50 = executor.RoundLots50;
            this.LimitOrderSlippage = executor.LimitOrderSlippage;
            this.ApplyInterest = executor.ApplyInterest;
            this.CashRate = executor.CashRate;
            this.MarginRate = executor.MarginRate;
            this.ApplyDividends = executor.ApplyDividends;
            this.ReduceQtyBasedOnVolume = executor.ReduceQtyBasedOnVolume;
            this.RedcuceQtyPct = executor.RedcuceQtyPct;
            this.LimitDaySimulation = executor.LimitDaySimulation;
            this.WorstTradeSimulation = executor.WorstTradeSimulation;
            this.BenchmarkBuyAndHoldON = executor.BenchmarkBuyAndHoldON;
            this.BenchmarkSymbol = executor.BenchmarkSymbol;
            this.OverrideShareSize = executor.OverrideShareSize;
            if (this.FundamentalsLoader == null)
            {
                this.FundamentalsLoader = executor.FundamentalsLoader;
            }
            this.NoDecimalRoundingForLimitStopPrice = executor.NoDecimalRoundingForLimitStopPrice;
            this.PricingDecimalPlaces = executor.PricingDecimalPlaces;
        }

        public double CalcPositionSize(Bars bars, int int_3, double basisPrice, PositionType positionType_0, double riskStopLevel)
        {
            double currentEquity = this.systemPerformance_0.Results.CurrentEquity;
            double overrideShareSize = 0.0;
            if (this.PosSize.Mode == PosSizeMode.ScriptOverride)
            {
                overrideShareSize = this.PosSize.OverrideShareSize;
            }
            return this.CalcPositionSize(bars, int_3, basisPrice, positionType_0, riskStopLevel, currentEquity, overrideShareSize, 0.0);
        }

        public double CalcPositionSize(Bars bars, int int_3, double basisPrice, PositionType positionType_0, double riskStopLevel, bool comingFromWealthScript)
        {
            double currentEquity = this.systemPerformance_0.Results.CurrentEquity;
            double overrideShareSize = 0.0;
            if (this.PosSize.Mode == PosSizeMode.ScriptOverride)
            {
                overrideShareSize = this.PosSize.OverrideShareSize;
            }
            return this.method_4(bars, int_3, basisPrice, positionType_0, riskStopLevel, currentEquity, overrideShareSize, 0.0, comingFromWealthScript);
        }

        public double CalcPositionSize(Bars bars, int int_3, double basisPrice, PositionType positionType_0, double riskStopLevel, double equity)
        {
            double overrideShareSize = 0.0;
            if (this.PosSize.Mode == PosSizeMode.ScriptOverride)
            {
                overrideShareSize = this.PosSize.OverrideShareSize;
            }
            return this.CalcPositionSize(bars, int_3, basisPrice, positionType_0, riskStopLevel, equity, overrideShareSize, 0.0);
        }

        public double CalcPositionSize(Bars bars, int int_3, double basisPrice, PositionType positionType_0, double riskStopLevel, double equity, double overrideShareSize, double currentCash)
        {
            return this.method_4(bars, int_3, basisPrice, positionType_0, riskStopLevel, equity, overrideShareSize, currentCash, false);
        }

        public double CalcPositionSize(Position position_1, Bars bars, int int_3, double basisPrice, PositionType positionType_0, double riskStopLevel, bool useOverRide, double overrideShareSize, double thisBarCash)
        {
            if ((this.Strategy != null) && (this.Strategy.StrategyType == StrategyType.CombinedStrategy))
            {
                return position_1.Shares;
            }
            this.position_0 = position_1;
            double currentEquity = this.systemPerformance_0.Results.CurrentEquity;
            double num2 = this.CalcPositionSize(bars, int_3, basisPrice, positionType_0, riskStopLevel, currentEquity, overrideShareSize, thisBarCash);
            this.position_0 = null;
            if (bool_17)
            {
                num2 *= this.TNPAdjustment;
            }
            return num2;
        }

        public void Clear()
        {
            this.list_3.Clear();
            this.list_4.Clear();
            this.systemPerformance_0.method_2();
            this.list_1.Clear();
            this.list_2.Clear();
        }

        public int Compare(Position position_1, Position position_2)
        {
            if (position_1.EntryDate == position_2.EntryDate)
            {
                if (position_1.CombinedPriority != position_2.CombinedPriority)
                {
                    return position_1.CombinedPriority.CompareTo(position_2.CombinedPriority);
                }
                if (!this.WorstTradeSimulation)
                {
                    return -position_1.Priority.CompareTo(position_2.Priority);
                }
                return position_1.NetProfit.CompareTo(position_2.NetProfit);
            }
            DateTime entryDate = position_1.EntryDate;
            DateTime time2 = position_2.EntryDate;
            return position_1.EntryDate.CompareTo(position_2.EntryDate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public void Execute(WealthScript wealthScript_1, Bars bars)
        {
            this.Execute(new WealthLab.Strategy(), wealthScript_1, bars);
        }

        public void Execute(WealthLab.Strategy strategy_1, WealthScript wealthScript_1, Bars bars)
        {
            List<Bars> barsCollection = new List<Bars> {
                bars
            };
            this.Execute(strategy_1, wealthScript_1, bars, barsCollection);
        }

        ///WYJ fix, code from Reflector
        /*
        public void Execute(WealthLab.Strategy strategy_1, WealthScript wealthScript_1, Bars barsCharted, List<Bars> barsCollection)
        {
            if (wealthScript_1 != null)
            {
                wealthScript_1.StrategyWindowID = this.StrategyWindowID;
            }
            List<Bars> collection = new List<Bars>();
            foreach (Bars bars2 in barsCollection)
            {
                collection.Add(bars2);
            }
            this.Strategy = strategy_1;
            this.systemPerformance_0.Strategy = strategy_1;
            this.list_7.Clear();
            GC.Collect();
            this.double_2 = 0.0;
            this.double_8 = 0.0;
            this.list_0.Clear();
            this.bool_11 = false;
            if ((barsCollection != null) && (barsCollection.Count != 0))
            {
                this.Clear();
                this.ilist_0 = barsCollection;
                foreach (Bars bars9 in barsCollection)
                {
                    this.Performance.method_1(bars9);
                }
                this.Performance.Scale = barsCollection[0].Scale;
                this.Performance.BarInterval = barsCollection[0].BarInterval;
                this.Performance.PositionSize = this.PosSize;
                this.wealthScript_0 = wealthScript_1;
                PositionSize posSize = this.PosSize;
                this.bool_16 = this.PosSize.RawProfitMode;
                if (!this.PosSize.RawProfitMode && (this.PosSize.Mode != PosSizeMode.ScriptOverride))
                {
                    this.PosSize = positionSize_1;
                }
                if (this.Strategy.StrategyType == StrategyType.CombinedStrategy)
                {
                    bool_0 = true;
                    try
                    {
                        this.Performance.PositionSize = this.PosSize;
                        List<string> list2 = new List<string>();
                        using (List<CombinedStrategyInfo>.Enumerator enumerator3 = this.Strategy.CombinedStrategyChildren.GetEnumerator())
                        {
                            List<Position>.Enumerator enumerator;
                        Label_01C4:
                            if (!enumerator3.MoveNext())
                            {
                                goto  Label_0917;
                            }
                            CombinedStrategyInfo current = enumerator3.Current;
                            WealthLab.Strategy strategy = null;
                            if (this.eventHandler_0 != null)
                            {
                                StrategyEventArgs e = new StrategyEventArgs(current.StrategyID.ToString());
                                this.eventHandler_0(this, e);
                                strategy = e.Strategy;
                            }
                            if (strategy == null)
                            {
                                goto  Label_01C4;
                            }
                            TradingSystemExecutor executor = new TradingSystemExecutor {
                                BarsLoader = this.BarsLoader,
                                StrategyName = strategy.Name,
                                FundamentalsLoader = this.FundamentalsLoader
                            };
                            List<Bars> list3 = new List<Bars>();
                            if (strategy.StrategyType == StrategyType.CombinedStrategy)
                            {
                                foreach (Bars bars7 in collection)
                                {
                                    list3.Add(bars7);
                                }
                            }
                            else if (current.UseDefaultDataSet)
                            {
                                list3.AddRange(collection);
                                executor.DataSet = this.DataSet;
                            }
                            else if (this.eventHandler_1 != null)
                            {
                                DataSourceLookupEventArgs args = new DataSourceLookupEventArgs(current.DataSetName);
                                this.eventHandler_1(this, args);
                                if (args.DataSource != null)
                                {
                                    WealthLab.BarsLoader loader = new WealthLab.BarsLoader {
                                        DataHost = this.BarsLoader.DataHost,
                                        BarDataScale = current.DataScale,
                                        StartDate = this.BarsLoader.StartDate,
                                        EndDate = this.BarsLoader.EndDate,
                                        MaxBars = this.BarsLoader.MaxBars,
                                        AutoCreateProvider = true
                                    };
                                    if (current.Symbol != "")
                                    {
                                        Bars data = loader.GetData(args.DataSource, current.Symbol);
                                        if (data != null)
                                        {
                                            list3.Add(data);
                                        }
                                    }
                                    else
                                    {
                                        foreach (string str2 in args.DataSource.Symbols)
                                        {
                                            Bars item = loader.GetData(args.DataSource, str2);
                                            if (item != null)
                                            {
                                                list3.Add(item);
                                            }
                                        }
                                    }
                                    executor.DataSet = args.DataSource;
                                }
                            }
                            foreach (Bars bars6 in list3)
                            {
                                string str = bars6.ToString();
                                bool flag = false;
                                using (IEnumerator<Bars> enumerator9 = this.ilist_0.GetEnumerator())
                                {
                                    while (enumerator9.MoveNext())
                                    {
                                        Bars bars5 = enumerator9.Current;
                                        if ((bars5.ToString() == str) && (bars5.DataScale == bars6.DataScale))
                                        {
                                            goto  Label_045D;
                                        }
                                    }
                                    goto  Label_046E;
                                Label_045D:
                                    flag = true;
                                }
                            Label_046E:
                                if (!flag)
                                {
                                    this.ilist_0.Add(bars6);
                                }
                            }
                            WealthScript tag = (WealthScript) strategy.Tag;
                            executor.ApplySettings(this);
                            executor.LookupDataSource += this.eventHandler_1;
                            executor.LookupStrategy += this.eventHandler_0;
                            executor.PosSize = current.PositionSize;
                            double startingCapital = posSize.StartingCapital;
                            if (current.Allocation.Mode == PosSizeMode.PctEquity)
                            {
                                startingCapital *= current.Allocation.PctSize / 100.0;
                            }
                            else
                            {
                                startingCapital = current.Allocation.DollarSize;
                            }
                            executor.PosSize.StartingCapital = startingCapital;
                            if (executor.PosSize.Mode == PosSizeMode.RawProfitDollar)
                            {
                                executor.PosSize.Mode = PosSizeMode.Dollar;
                                executor.PosSize.DollarSize = executor.PosSize.RawProfitDollarSize;
                            }
                            if (executor.PosSize.Mode == PosSizeMode.RawProfitShare)
                            {
                                executor.PosSize.Mode = PosSizeMode.Share;
                                executor.PosSize.ShareSize = executor.PosSize.RawProfitShareSize;
                            }
                            if (tag != null)
                            {
                                for (int i = 0; i < current.ParameterValues.Count; i++)
                                {
                                    tag.Parameters[i].Value = current.ParameterValues[i];
                                }
                                strategy.UsePreferredValues = current.UsePreferredValues;
                            }
                            executor.ExecutionCompletedForSymbol += new EventHandler<BarsEventArgs>(this.method_0);
                            executor.ExternalSymbolRequested += this.eventHandler_2;
                            executor.ExternalSymbolFromDataSetRequested += this.eventHandler_3;
                            executor.ExceptionEvents = true;
                            executor.WealthScriptException += new EventHandler<WSExceptionEventArgs>(this.method_1);
                            try
                            {
                                executor.Execute(strategy, tag, null, list3);
                            }
                            catch (Exception exception)
                            {
                                executor.method_15("Exception in Combination Strategy Child: " + strategy.Name);
                                executor.method_15(exception.Message);
                            }
                            goto  Label_08A6;
                        Label_0666:
                            try
                            {
                                while (enumerator.MoveNext())
                                {
                                    Position position2 = enumerator.Current;
                                    position2.CombinedPriority = current.Priority;
                                    position2.CSI = current;
                                }
                            }
                            finally
                            {
                                enumerator.Dispose();
                            }
                            foreach (Alert alert in executor.Performance.Results.Alerts)
                            {
                                alert.Account = current.AccountNumber;
                            }
                            executor.ApplyPositionSize();
                            foreach (Position position in executor.Performance.Results.Positions)
                            {
                                foreach (Bars bars3 in this.ilist_0)
                                {
                                    if (bars3.ToString() == position.Bars.ToString())
                                    {
                                        position.method_0(bars3);
                                    }
                                }
                            }
                            this.list_3.AddRange(executor.Performance.Results.Positions);
                            this.list_4.AddRange(executor.Performance.Results.Alerts);
                            if (!this.BenchmarkBuyAndHoldON)
                            {
                                foreach (Position position3 in executor.systemPerformance_0.ResultsBuyHold.Positions)
                                {
                                    this.systemPerformance_0.ResultsBuyHold.method_4(position3);
                                }
                            }
                            SystemResults results = this.Performance.Results;
                            results.TradesNSF += executor.Performance.Results.TradesNSF;
                            SystemResults resultsLong = this.Performance.ResultsLong;
                            resultsLong.TradesNSF += executor.Performance.ResultsLong.TradesNSF;
                            SystemResults resultsShort = this.Performance.ResultsShort;
                            resultsShort.TradesNSF += executor.Performance.ResultsShort.TradesNSF;
                            executor.LookupDataSource -= this.eventHandler_1;
                            executor.LookupStrategy -= this.eventHandler_0;
                            goto  Label_01C4;
                        Label_08A6:
                            executor.ExecutionCompletedForSymbol -= new EventHandler<BarsEventArgs>(this.method_0);
                            executor.ExternalSymbolRequested -= this.eventHandler_2;
                            executor.ExternalSymbolFromDataSetRequested -= this.eventHandler_3;
                            executor.WealthScriptException -= new EventHandler<WSExceptionEventArgs>(this.method_1);
                            list2.AddRange(executor.DebugStrings);
                            enumerator = executor.list_3.GetEnumerator();
                            goto  Label_0666;
                        }
                    Label_0917:
                        this.list_0.Clear();
                        this.list_0.AddRange(list2);
                    }
                    finally
                    {
                        this.PosSize = posSize;
                        this.list_7.Clear();
                        bool_0 = false;
                    }
                }
                else
                {
                    try
                    {
                        foreach (Bars bars in barsCollection)
                        {
                            ChartRenderer renderer = (barsCharted == bars) ? this.chartRenderer_0 : null;
                            this.method_2(bars, wealthScript_1, renderer);
                        }
                    }
                    finally
                    {
                        this.PosSize = posSize;
                        this.list_7.Clear();
                    }
                }
                this.list_3.Sort(this);
                if (this.BuildEquityCurves)
                {
                    this.ApplyPositionSize();
                }
            }
        } */

        ///WYJ fix, code from JustDecompile
        public void Execute(Strategy strategy_1, WealthScript wealthScript_1, Bars barsCharted, List<Bars> barsCollection)
        {
            ChartRenderer chartRenderer0;
            if (wealthScript_1 != null)
            {
                wealthScript_1.StrategyWindowID = this.StrategyWindowID;
            }
            List<Bars> bars = new List<Bars>();
            foreach (Bars bar in barsCollection)
            {
                bars.Add(bar);
            }
            this.Strategy = strategy_1;
            this.systemPerformance_0.Strategy = strategy_1;
            this.list_7.Clear();
            GC.Collect();
            this.double_2 = 0;
            this.double_8 = 0;
            this.list_0.Clear();
            this.bool_11 = false;
            if (barsCollection == null || barsCollection.Count == 0)
            {
                return;
            }
            else
            {
                this.Clear();
                this.ilist_0 = barsCollection;
                foreach (Bars bar1 in barsCollection)
                {
                    this.Performance.method_1(bar1);
                }
                this.Performance.Scale = barsCollection[0].Scale;
                this.Performance.BarInterval = barsCollection[0].BarInterval;
                this.Performance.PositionSize = this.PosSize;
                this.wealthScript_0 = wealthScript_1;
                PositionSize posSize = this.PosSize;
                this.bool_16 = this.PosSize.RawProfitMode;
                if (!this.PosSize.RawProfitMode && this.PosSize.Mode != PosSizeMode.ScriptOverride)
                {
                    this.PosSize = TradingSystemExecutor.positionSize_1;
                }
                if (this.Strategy.StrategyType != StrategyType.CombinedStrategy)
                {
                    try
                    {
                        foreach (Bars bar2 in barsCollection)
                        {
                            if (barsCharted == bar2)
                            {
                                chartRenderer0 = this.chartRenderer_0;
                            }
                            else
                            {
                                chartRenderer0 = null;
                            }
                            ChartRenderer chartRenderer = chartRenderer0;
                            this.method_2(bar2, wealthScript_1, chartRenderer);
                        }
                    }
                    finally
                    {
                        this.PosSize = posSize;
                        this.list_7.Clear();
                    }
                }
                else
                {
                    TradingSystemExecutor.bool_0 = true;
                    try
                    {
                        this.Performance.PositionSize = this.PosSize;
                        List<string> strs = new List<string>();
                        foreach (CombinedStrategyInfo combinedStrategyChild in this.Strategy.CombinedStrategyChildren)
                        {
                            Strategy strategy = null;
                            if (this.eventHandler_0 != null)
                            {
                                Guid strategyID = combinedStrategyChild.StrategyID;
                                StrategyEventArgs strategyEventArg = new StrategyEventArgs(strategyID.ToString());
                                this.eventHandler_0(this, strategyEventArg);
                                strategy = strategyEventArg.Strategy;
                            }
                            if (strategy == null)
                            {
                                continue;
                            }
                            TradingSystemExecutor tradingSystemExecutor = new TradingSystemExecutor();
                            tradingSystemExecutor.BarsLoader = this.BarsLoader;
                            tradingSystemExecutor.StrategyName = strategy.Name;
                            tradingSystemExecutor.FundamentalsLoader = this.FundamentalsLoader;
                            List<Bars> bars1 = new List<Bars>();
                            if (strategy.StrategyType != StrategyType.CombinedStrategy)
                            {
                                if (!combinedStrategyChild.UseDefaultDataSet)
                                {
                                    if (this.eventHandler_1 != null)
                                    {
                                        DataSourceLookupEventArgs dataSourceLookupEventArg = new DataSourceLookupEventArgs(combinedStrategyChild.DataSetName);
                                        this.eventHandler_1(this, dataSourceLookupEventArg);
                                        if (dataSourceLookupEventArg.DataSource != null)
                                        {
                                            BarsLoader barsLoader = new BarsLoader();
                                            barsLoader.DataHost = this.BarsLoader.DataHost;
                                            barsLoader.BarDataScale = combinedStrategyChild.DataScale;
                                            barsLoader.StartDate = this.BarsLoader.StartDate;
                                            barsLoader.EndDate = this.BarsLoader.EndDate;
                                            barsLoader.MaxBars = this.BarsLoader.MaxBars;
                                            barsLoader.AutoCreateProvider = true;
                                            if (combinedStrategyChild.Symbol == "")
                                            {
                                                foreach (string symbol in dataSourceLookupEventArg.DataSource.Symbols)
                                                {
                                                    Bars data = barsLoader.GetData(dataSourceLookupEventArg.DataSource, symbol);
                                                    if (data == null)
                                                    {
                                                        continue;
                                                    }
                                                    bars1.Add(data);
                                                }
                                            }
                                            else
                                            {
                                                Bars data1 = barsLoader.GetData(dataSourceLookupEventArg.DataSource, combinedStrategyChild.Symbol);
                                                if (data1 != null)
                                                {
                                                    bars1.Add(data1);
                                                }
                                            }
                                            tradingSystemExecutor.DataSet = dataSourceLookupEventArg.DataSource;
                                        }
                                    }
                                }
                                else
                                {
                                    bars1.AddRange(bars);
                                    tradingSystemExecutor.DataSet = this.DataSet;
                                }
                            }
                            else
                            {
                                foreach (Bars bar3 in bars)
                                {
                                    bars1.Add(bar3);
                                }
                            }
                            foreach (Bars bar4 in bars1)
                            {
                                string str = bar4.ToString();
                                bool flag = false;
                                IEnumerator<Bars> enumerator = this.ilist_0.GetEnumerator();
                                using (enumerator)
                                {
                                    while (true)
                                    {
                                        if (enumerator.MoveNext())
                                        {
                                            Bars current = enumerator.Current;
                                            if (current.ToString() == str && current.DataScale == bar4.DataScale)
                                            {
                                                flag = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                if (flag)
                                {
                                    continue;
                                }
                                this.ilist_0.Add(bar4);
                            }
                            WealthScript tag = (WealthScript)strategy.Tag;
                            tradingSystemExecutor.ApplySettings(this);
                            tradingSystemExecutor.LookupDataSource += this.eventHandler_1;
                            tradingSystemExecutor.LookupStrategy += this.eventHandler_0;
                            tradingSystemExecutor.PosSize = combinedStrategyChild.PositionSize;
                            double startingCapital = posSize.StartingCapital;
                            startingCapital = (combinedStrategyChild.Allocation.Mode != PosSizeMode.PctEquity ? combinedStrategyChild.Allocation.DollarSize : startingCapital * combinedStrategyChild.Allocation.PctSize / 100);
                            tradingSystemExecutor.PosSize.StartingCapital = startingCapital;
                            if (tradingSystemExecutor.PosSize.Mode == PosSizeMode.RawProfitDollar)
                            {
                                tradingSystemExecutor.PosSize.Mode = PosSizeMode.Dollar;
                                tradingSystemExecutor.PosSize.DollarSize = tradingSystemExecutor.PosSize.RawProfitDollarSize;
                            }
                            if (tradingSystemExecutor.PosSize.Mode == PosSizeMode.RawProfitShare)
                            {
                                tradingSystemExecutor.PosSize.Mode = PosSizeMode.Share;
                                tradingSystemExecutor.PosSize.ShareSize = tradingSystemExecutor.PosSize.RawProfitShareSize;
                            }
                            if (tag != null)
                            {
                                for (int i = 0; i < combinedStrategyChild.ParameterValues.Count; i++)
                                {
                                    tag.Parameters[i].Value = combinedStrategyChild.ParameterValues[i];
                                }
                                strategy.UsePreferredValues = combinedStrategyChild.UsePreferredValues;
                            }
                            tradingSystemExecutor.ExecutionCompletedForSymbol += new EventHandler<BarsEventArgs>(this.method_0);
                            tradingSystemExecutor.ExternalSymbolRequested += this.eventHandler_2;
                            tradingSystemExecutor.ExternalSymbolFromDataSetRequested += this.eventHandler_3;
                            tradingSystemExecutor.ExceptionEvents = true;
                            tradingSystemExecutor.WealthScriptException += new EventHandler<WSExceptionEventArgs>(this.method_1);
                            try
                            {
                                tradingSystemExecutor.Execute(strategy, tag, null, bars1);
                            }
                            catch (Exception exception1)
                            {
                                Exception exception = exception1;
                                tradingSystemExecutor.method_15(string.Concat("Exception in Combination Strategy Child: ", strategy.Name));
                                tradingSystemExecutor.method_15(exception.Message);
                            }
                            tradingSystemExecutor.ExecutionCompletedForSymbol -= new EventHandler<BarsEventArgs>(this.method_0);
                            tradingSystemExecutor.ExternalSymbolRequested -= this.eventHandler_2;
                            tradingSystemExecutor.ExternalSymbolFromDataSetRequested -= this.eventHandler_3;
                            tradingSystemExecutor.WealthScriptException -= new EventHandler<WSExceptionEventArgs>(this.method_1);
                            strs.AddRange(tradingSystemExecutor.DebugStrings);
                            foreach (Position list3 in tradingSystemExecutor.list_3)
                            {
                                list3.CombinedPriority = combinedStrategyChild.Priority;
                                list3.CSI = combinedStrategyChild;
                            }
                            foreach (Alert alert in tradingSystemExecutor.Performance.Results.Alerts)
                            {
                                alert.Account = combinedStrategyChild.AccountNumber;
                            }
                            tradingSystemExecutor.ApplyPositionSize();
                            foreach (Position position in tradingSystemExecutor.Performance.Results.Positions)
                            {
                                foreach (Bars ilist0 in this.ilist_0)
                                {
                                    if (ilist0.ToString() != position.Bars.ToString())
                                    {
                                        continue;
                                    }
                                    position.method_0(ilist0);
                                }
                            }
                            this.list_3.AddRange(tradingSystemExecutor.Performance.Results.Positions);
                            this.list_4.AddRange(tradingSystemExecutor.Performance.Results.Alerts);
                            if (!this.BenchmarkBuyAndHoldON)
                            {
                                foreach (Position position1 in tradingSystemExecutor.systemPerformance_0.ResultsBuyHold.Positions)
                                {
                                    this.systemPerformance_0.ResultsBuyHold.method_4(position1);
                                }
                            }
                            SystemResults results = this.Performance.Results;
                            results.TradesNSF = results.TradesNSF + tradingSystemExecutor.Performance.Results.TradesNSF;
                            SystemResults resultsLong = this.Performance.ResultsLong;
                            resultsLong.TradesNSF = resultsLong.TradesNSF + tradingSystemExecutor.Performance.ResultsLong.TradesNSF;
                            SystemResults resultsShort = this.Performance.ResultsShort;
                            resultsShort.TradesNSF = resultsShort.TradesNSF + tradingSystemExecutor.Performance.ResultsShort.TradesNSF;
                            tradingSystemExecutor.LookupDataSource -= this.eventHandler_1;
                            tradingSystemExecutor.LookupStrategy -= this.eventHandler_0;
                        }
                        this.list_0.Clear();
                        this.list_0.AddRange(strs);
                    }
                    finally
                    {
                        this.PosSize = posSize;
                        this.list_7.Clear();
                        TradingSystemExecutor.bool_0 = false;
                    }
                }
                this.list_3.Sort(this);
                if (this.BuildEquityCurves)
                {
                    this.ApplyPositionSize();
                }
                return;
            }
        }

        public void Initialize()
        {
            this.Performance.PositionSize = this.PosSize;
            this.systemPerformance_0.Results.CurrentCash = this.PosSize.StartingCapital;
            this.systemPerformance_0.Results.CurrentEquity = this.PosSize.StartingCapital;
        }

        private void method_0(object sender, BarsEventArgs e)
        {
            if (this.eventHandler_5 != null)
            {
                this.eventHandler_5(this, e);
            }
        }

        private void method_1(object sender, WSExceptionEventArgs e)
        {
            TradingSystemExecutor executor = sender as TradingSystemExecutor;
            executor.method_15("Exception in Combination Strategy Child: " + e.Strategy.Name);
            executor.method_15(e.Exception.Message);
        }

        internal Bars method_10(string string_2, bool bool_19)
        {
            Bars bars = this.wealthScript_0.Bars;
            Bars item = null;
            if (string_2 == bars.Symbol)
            {
                return bars;
            }
            using (List<Bars>.Enumerator enumerator = this.list_7.GetEnumerator())
            {
                Bars current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Symbol == string_2)
                    {
                        ///goto  Label_0051; ///WYJ fix, simplify the flow
                        item = current;
                        break;
                    }
                }
            }
            if ((item == null) && (this.BarsLoader != null))
            {
                if (this.IsStreaming)
                {
                    this.BarsLoader.OverrideOnDemand = true;
                    this.BarsLoader.OverrideOnDemandValue = true;
                }
                this.BarsLoader.method_2(this.dataSource_0);
                this.dataSource_0.Provider.IsStreamingRequest = this.IsStreaming;
                BarDataScale barDataScale = this.BarsLoader.BarDataScale;
                this.BarsLoader.Scale = bars.Scale;
                this.BarsLoader.BarInterval = bars.BarInterval;
                this.BarsLoader.AutoConvertScale = false;
                item = this.BarsLoader.method_4(string_2);
                if ((item != null) && (item.Count > 0))
                {
                    this.list_7.Add(item);
                }
                this.BarsLoader.AutoConvertScale = true;
                this.BarsLoader.OverrideOnDemand = false;
                this.BarsLoader.BarDataScale = barDataScale;
            }
            if (((item == null) || (item.Count == 0)) && (this.eventHandler_2 != null))
            {
                LoadSymbolEventArgs e = new LoadSymbolEventArgs(string_2, bars.Scale, bars.BarInterval);
                this.eventHandler_2(this, e);
                item = e.SymbolData;
                if ((item != null) && (item.Count > 0))
                {
                    this.list_7.Add(item);
                }
            }
            Bars bars3 = new Bars(item);
            bars3.Append(item);
            item = bars3;
            if ((bool_19 && (item != null)) && (item.Count > 0))
            {
                item = BarScaleConverter.Synchronize(item, bars);
            }
            if (item != null)
            {
                item.method_1();
                if (bool_19)
                {
                    this.list_1.Add(item);
                    return item;
                }
                this.list_2.Add(item);
            }
            return item;
        }

        internal int method_11()
        {
            int num = this.list_1.Count + this.list_2.Count;
            this.list_1.Clear();
            this.list_2.Clear();
            return num;
        }

        internal int method_12(string string_2)
        {
            int num = 0;
            for (int i = this.list_1.Count - 1; i >= 0; i--)
            {
                if (this.list_1[i].Symbol == string_2)
                {
                    this.list_1.RemoveAt(i);
                    num++;
                }
            }
            for (int j = this.list_2.Count - 1; j >= 0; j--)
            {
                if (this.list_2[j].Symbol == string_2)
                {
                    this.list_2.RemoveAt(j);
                    num++;
                }
            }
            return num;
        }

        internal void method_13(Bars bars_1, bool bool_19)
        {
            bars_1.method_1();
            if (bool_19)
            {
                this.list_1.Add(bars_1);
            }
            else
            {
                this.list_2.Add(bars_1);
            }
        }

        internal double method_14(double double_10, bool bool_19, Bars bars_1)
        {
            if (!this.EnableSlippage)
            {
                return 0.0;
            }
            if (bool_19 && !this.LimitOrderSlippage)
            {
                return 0.0;
            }
            if (bars_1.SymbolInfo.SecurityType == SecurityType.Future)
            {
                return (this.SlippageTicks * bars_1.SymbolInfo.Tick);
            }
            return ((0.01 * this.SlippageUnits) * double_10);
        }

        internal void method_15(string string_2)
        {
            this.list_0.Add(string_2);
        }

        internal void method_16()
        {
            if (this.eventHandler_7 != null)
            {
                this.eventHandler_7(this, EventArgs.Empty);
            }
        }

        internal void method_17()
        {
            this.list_0.Clear();
            if (this.eventHandler_8 != null)
            {
                this.eventHandler_8(this, EventArgs.Empty);
            }
        }

        internal void method_18(string string_2)
        {
            if (this.eventHandler_9 != null)
            {
                this.eventHandler_9(this, new DebugStringEventArgs(string_2));
            }
        }

        internal Bitmap method_19(int int_3, int int_4)
        {
            Bitmap image = null;
            if (this.chartRenderer_0 == null)
            {
                return null;
            }
            if (this.eventHandler_10 != null)
            {
                ChartBitmapEventArgs e = new ChartBitmapEventArgs(int_3, int_4);
                this.eventHandler_10(this, e);
                image = e.Bitmap;
            }
            if (image == null)
            {
                image = new Bitmap(int_3, int_4);
                Graphics graphics = Graphics.FromImage(image);
                using (graphics)
                {
                    this.chartRenderer_0.Executing = false;
                    this.chartRenderer_0.Render(this.bars_0, graphics, int_3, int_4, this.chartRenderer_0.ChartStyle);
                    this.chartRenderer_0.Executing = true;
                }
            }
            return image;
        }

        private void method_2(Bars bars_1, WealthScript wealthScript_1, ChartRenderer chartRenderer_1)
        {
            this.CurrentPositions.Clear();
            this.CurrentAlerts.Clear();
            this.ActivePositions.Clear();
            this.method_11();
            try
            {
                this.bars_0 = bars_1;
                bars_1.method_1();
                if (!bool_0)
                {
                    if (this.eventHandler_12 != null)
                    {
                        this.eventHandler_12(this, new StrategyParameterEventArgs(wealthScript_1, bars_1.Symbol));
                    }
                }
                else if (this.Strategy.UsePreferredValues)
                {
                    this.Strategy.LoadPreferredValues(bars_1.Symbol, wealthScript_1);
                }
                wealthScript_1.method_4(bars_1, chartRenderer_1, this, this.dataSource_0);
                wealthScript_1.RestoreScale();
                bars_1.method_2();
            }
            catch (Exception exception)
            {
                bars_1.method_2();
                if (!this.ExceptionEvents)
                {
                    throw exception;
                }
                if (this.eventHandler_6 != null)
                {
                    WSExceptionEventArgs e = new WSExceptionEventArgs(exception) {
                        Strategy = this.Strategy
                    };
                    this.eventHandler_6(this, e);
                }
            }
            if (this.eventHandler_4 != null)
            {
                this.eventHandler_4(this, new BarsEventArgs(bars_1));
            }
        }

        internal double method_20(int int_3, string string_2)
        {
            if (this.eventHandler_11 == null)
            {
                return 0.0;
            }
            TrendLineEventArgs e = new TrendLineEventArgs(string_2, int_3);
            this.eventHandler_11(this, e);
            return e.Value;
        }

        private void method_21()
        {
            this.icontainer_0 = new Container();
        }

        private int method_3(Bars bars_1, double double_10)
        {
            int num = 0;
            if (WealthLab.BarsLoader.FuturesMode)
            {
                SymbolInfo symbolInfo = bars_1.SymbolInfo;
                if ((symbolInfo.SecurityType == SecurityType.Future) && (symbolInfo.Margin > 0.0))
                {
                    num = (int) (double_10 / symbolInfo.Margin);
                }
            }
            if (num == 0)
            {
                num = (int) (double_10 / bars_1.Close[0]);
            }
            return num;
        }

        internal double method_4(Bars bars_1, int int_3, double double_10, PositionType positionType_0, double double_11, double double_12, double double_13, double double_14, bool bool_19)
        {
            double rawProfitShareSize = 0.0;
            if ((bars_1.SymbolInfo.SecurityType == SecurityType.Future) && (bars_1.SymbolInfo.Margin <= 0.0))
            {
                throw new ArgumentException("Margin must be greater than zero");
            }
            switch (this.PosSize.Mode)
            {
                case PosSizeMode.RawProfitDollar:
                    if (bars_1.SymbolInfo.SecurityType != SecurityType.Future)
                    {
                        rawProfitShareSize = this.PosSize.RawProfitDollarSize / double_10;
                        break;
                    }
                    rawProfitShareSize = this.PosSize.RawProfitDollarSize / bars_1.SymbolInfo.Margin;
                    break;

                case PosSizeMode.RawProfitShare:
                    rawProfitShareSize = this.PosSize.RawProfitShareSize;
                    break;

                case PosSizeMode.Dollar:
                    if (bars_1.SymbolInfo.SecurityType != SecurityType.Future)
                    {
                        rawProfitShareSize = this.PosSize.DollarSize / double_10;
                        break;
                    }
                    rawProfitShareSize = this.PosSize.DollarSize / bars_1.SymbolInfo.Margin;
                    break;

                case PosSizeMode.Share:
                    rawProfitShareSize = this.PosSize.ShareSize;
                    break;

                case PosSizeMode.PctEquity:
                {
                    double num8 = (this.PosSize.PctSize / 100.0) * double_12;
                    if (bars_1.SymbolInfo.SecurityType != SecurityType.Future)
                    {
                        rawProfitShareSize = num8 / double_10;
                        break;
                    }
                    rawProfitShareSize = num8 / bars_1.SymbolInfo.Margin;
                    break;
                }
                case PosSizeMode.MaxRisk:
                    if (double_10 != 0.0)
                    {
                        double num7;
                        if (this.RiskStopLevel <= 0.0)
                        {
                            this.bool_11 = true;
                            return 0.0;
                        }
                        double num3 = this.PosSize.RiskSize / 100.0;
                        num3 *= double_12;
                        double num6 = (positionType_0 == PositionType.Long) ? (double_10 - double_11) : (double_11 - double_10);
                        try
                        {
                            rawProfitShareSize = num3 / (num6 * bars_1.SymbolInfo.PointValue);
                            if ((bars_1.SymbolInfo.SecurityType == SecurityType.Future) && (bars_1.SymbolInfo.Margin > 0.0))
                            {
                                if (rawProfitShareSize > (double_12 / bars_1.SymbolInfo.Margin))
                                {
                                    rawProfitShareSize = double_12 / bars_1.SymbolInfo.Margin;
                                }
                            }
                            else if (rawProfitShareSize > (double_12 / double_10))
                            {
                                rawProfitShareSize = double_12 / double_10;
                            }
                            break;
                        }
                        catch
                        {
                            num7 = 0.0;
                        }
                        return num7;
                    }
                    return 0.0;

                case PosSizeMode.SimuScript:
                    if (this.posSizer_0 != null)
                    {
                        try
                        {
                            rawProfitShareSize = this.posSizer_0.SizePosition(this.position_0, bars_1, int_3 - 1, double_10, positionType_0, double_11, double_12, double_14);
                            break;
                        }
                        catch
                        {
                            rawProfitShareSize = 0.0;
                            break;
                        }
                    }
                    rawProfitShareSize = 0.0;
                    break;

                case PosSizeMode.ScriptOverride:
                    rawProfitShareSize = double_13;
                    this.PosSize.OverrideShareSize = rawProfitShareSize;
                    break;
            }
            if (this.ReduceQtyBasedOnVolume && (int_3 < bars_1.Count))
            {
                double num9 = this.RedcuceQtyPct / 100.0;
                double num4 = bars_1.Volume[int_3] * num9;
                if (rawProfitShareSize > num4)
                {
                    rawProfitShareSize = num4;
                }
            }
            if (bars_1.SymbolInfo.SecurityType != SecurityType.MutualFund)
            {
                rawProfitShareSize = (int) rawProfitShareSize;
            }
            else
            {
                int num5 = (int) (rawProfitShareSize * 1000.0);
                rawProfitShareSize = ((double) num5) / 1000.0;
            }
            if ((!bool_19 || !this.bool_16) && bool_19)
            {
                return rawProfitShareSize;
            }
            if ((!this.RoundLots || (bars_1.SymbolInfo.SecurityType != SecurityType.Equity)) || (rawProfitShareSize <= 0.0))
            {
                return rawProfitShareSize;
            }
            double a = rawProfitShareSize / 100.0;
            a = Math.Round(a) * 100.0;
            if ((rawProfitShareSize < 100.0) && this.RoundLots50)
            {
                a = 100.0;
            }
            return a;
        }

        internal void method_5(Position position_1)
        {
            this.MasterPositions.Add(position_1);
            this.CurrentPositions.Add(position_1);
            this.ActivePositions.Add(position_1);
        }

        internal void method_6(Alert alert_0)
        {
            if (alert_0.Shares > 0.0)
            {
                alert_0.PosSize = this.PosSize;
                this.MasterAlerts.Add(alert_0);
                this.CurrentAlerts.Add(alert_0);
            }
        }

        internal Bars method_7(string string_2, BarScale barScale_0, int int_3, bool bool_19)
        {
            if (bool_19)
            {
                return this.method_8(string_2, barScale_0, int_3, this.list_1);
            }
            return this.method_8(string_2, barScale_0, int_3, this.list_2);
        }

        private Bars method_8(string string_2, BarScale barScale_0, int int_3, List<Bars> list_8)
        {
            Bars bars2;
            using (List<Bars>.Enumerator enumerator = list_8.GetEnumerator())
            {
                Bars current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (((current.Symbol == string_2) && (current.Scale == barScale_0)) && (current.BarInterval == int_3))
                    {
                        ///goto  Label_0040;  ///WYJ fix, simplify the flow
                        bars2 = current;
                        return bars2;
                    }
                }
                return null;
            }
        }

        internal Bars method_9(string string_2, string string_3, bool bool_19)
        {
            Bars bars = this.wealthScript_0.Bars;
            Bars source = null;
            string key = string_2 + "|" + string_3;
            if (this.dictionary_0.ContainsKey(key))
            {
                source = this.dictionary_0[key];
            }
            if ((source == null) && (this.eventHandler_3 != null))
            {
                LoadSymbolFromDataSetEventArgs e = new LoadSymbolFromDataSetEventArgs(string_2, string_3);
                this.eventHandler_3(this, e);
                source = e.Bars;
                if (source != null)
                {
                    this.dictionary_0[key] = source;
                }
            }
            if ((bool_19 && (source != null)) && (source.Count > 0))
            {
                source = BarScaleConverter.Synchronize(source, bars);
            }
            return source;
        }

        private static double _secureCode
        {
            get
            {
                return DateTime.Now.Add(new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)).ToOADate();
            }
        }

        internal List<Position> ActivePositions
        {
            get
            {
                return this._activePositions;
            }
        }

        public bool ApplyCommission
        {
            get
            {
                return this.bool_9;
            }
            set
            {
                this.bool_9 = value;
            }
        }

        public bool ApplyDividends
        {
            get
            {
                return this.bool_14;
            }
            set
            {
                this.bool_14 = value;
            }
        }

        public bool ApplyInterest
        {
            get
            {
                return this.bool_13;
            }
            set
            {
                this.bool_13 = value;
            }
        }

        internal double AutoProfitLevel
        {
            get
            {
                return this.double_8;
            }
            set
            {
                this.double_8 = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Bars BarsBeingProcessed
        {
            get
            {
                return this.bars_0;
            }
        }

        public WealthLab.BarsLoader BarsLoader
        {
            get
            {
                return this.barsLoader_0;
            }
            set
            {
                this.barsLoader_0 = value;
            }
        }

        public bool BenchmarkBuyAndHoldON
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

        public string BenchmarkSymbol
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

        public bool BuildEquityCurves
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

        public double CashAdjustmentFactor
        {
            get
            {
                return this.double_5;
            }
        }

        public double CashRate
        {
            get
            {
                return this.double_3;
            }
            set
            {
                this.double_3 = value;
                this.double_5 = Math.Exp(Math.Log(1.0 + (this.double_3 / 100.0)) / 365.25);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public WealthLab.Commission Commission
        {
            get
            {
                return this.commission_0;
            }
            set
            {
                this.commission_0 = value;
            }
        }

        internal List<Alert> CurrentAlerts
        {
            get
            {
                return this.list_6;
            }
        }

        internal List<Position> CurrentPositions
        {
            get
            {
                return this.list_5;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        public IList<string> DebugStrings
        {
            get
            {
                return this.list_0;
            }
        }

        public bool EnableSlippage
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

        public bool ExceptionEvents
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

        public WealthLab.FundamentalsLoader FundamentalsLoader
        {
            get
            {
                return this.fundamentalsLoader_0;
            }
            set
            {
                this.fundamentalsLoader_0 = value;
            }
        }

        public bool IsStreaming
        {
            get
            {
                return this.bool_10;
            }
            set
            {
                this.bool_10 = value;
            }
        }

        public bool LimitDaySimulation
        {
            get
            {
                return this.bool_12;
            }
            set
            {
                this.bool_12 = value;
            }
        }

        public bool LimitOrderSlippage
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

        public double MarginAdjustmentFactor
        {
            get
            {
                return this.double_6;
            }
        }

        public double MarginRate
        {
            get
            {
                return this.double_4;
            }
            set
            {
                this.double_4 = value;
                this.double_6 = Math.Exp(Math.Log(1.0 + (this.double_4 / 100.0)) / 365.25);
            }
        }

        internal List<Alert> MasterAlerts
        {
            get
            {
                return this.list_4;
            }
        }

        public List<Position> MasterPositions
        {
            get
            {
                return this.list_3;
            }
        }

        public bool NoDecimalRoundingForLimitStopPrice
        {
            [CompilerGenerated]
            get
            {
                return this.bool_18;
            }
            [CompilerGenerated]
            set
            {
                this.bool_18 = value;
            }
        }

        public double OverrideShareSize
        {
            get
            {
                return this.double_1;
            }
            set
            {
                this.double_1 = value;
                if (this.PosSize.Mode == PosSizeMode.ScriptOverride)
                {
                    this.PosSize.OverrideShareSize = value;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public SystemPerformance Performance
        {
            get
            {
                return this.systemPerformance_0;
            }
            set
            {
                this.systemPerformance_0 = value;
            }
        }

        public PositionSize PosSize
        {
            get
            {
                return this.positionSize_0;
            }
            set
            {
                this.positionSize_0 = value;
            }
        }

        internal WealthLab.PosSizer PosSizer
        {
            get
            {
                return this.posSizer_0;
            }
        }

        public int PricingDecimalPlaces
        {
            [CompilerGenerated]
            get
            {
                return this.int_2;
            }
            [CompilerGenerated]
            set
            {
                this.int_2 = value;
            }
        }

        public double RedcuceQtyPct
        {
            get
            {
                return this.double_7;
            }
            set
            {
                this.double_7 = value;
            }
        }

        public bool ReduceQtyBasedOnVolume
        {
            get
            {
                return this.bool_15;
            }
            set
            {
                this.bool_15 = value;
            }
        }

        public ChartRenderer Renderer
        {
            get
            {
                return this.chartRenderer_0;
            }
            set
            {
                this.chartRenderer_0 = value;
            }
        }

        internal double RiskStopLevel
        {
            get
            {
                return this.double_2;
            }
            set
            {
                this.double_2 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool RiskStopLevelNotSet
        {
            get
            {
                return this.bool_11;
            }
            set
            {
                this.bool_11 = value;
            }
        }

        public bool RoundLots
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

        public bool RoundLots50
        {
            get
            {
                return this.bool_8;
            }
            set
            {
                this.bool_8 = value;
            }
        }

        public int SlippageTicks
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

        public double SlippageUnits
        {
            get
            {
                return this.double_0;
            }
            set
            {
                this.double_0 = value;
            }
        }

        public WealthLab.Strategy Strategy
        {
            [CompilerGenerated]
            get
            {
                return this.strategy_0;
            }
            [CompilerGenerated]
            set
            {
                this.strategy_0 = value;
            }
        }

        public string StrategyName
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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Tag
        {
            [CompilerGenerated]
            get
            {
                return this.object_0;
            }
            [CompilerGenerated]
            set
            {
                this.object_0 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public double TNP
        {
            get
            {
                if (bool_17)
                {
                    return DateTime.Now.ToOADate();
                }
                return _secureCode;
            }
            set
            {
                if (value < DateTime.FromOADate(_secureCode).Subtract(new TimeSpan(0, 0, 0, 1)).ToOADate())
                {
                    bool_17 = true;
                }
            }
        }

        internal double TNPAdjustment
        {
            get
            {
                if (bool_17)
                {
                    Random random = new Random();
                    if (--int_1 <= 0)
                    {
                        int_1 = random.Next(10, 100);
                        double_9 = random.NextDouble();
                        if (double_9 <= 0.25)
                        {
                            double_9++;
                        }
                        else if (double_9 <= 0.5)
                        {
                            double_9 = 1.0;
                        }
                        else if (double_9 <= 0.75)
                        {
                            double_9 += double_9;
                        }
                    }
                }
                return double_9;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WealthScript WealthScriptExecuting
        {
            get
            {
                return this.wealthScript_0;
            }
        }

        public bool WorstTradeSimulation
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
    }
}

