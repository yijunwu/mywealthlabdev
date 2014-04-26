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
        private Bars barsBeingProcessed;
        private WealthLab.BarsLoader barsLoader;
        private static bool bool_0 = false;
        private bool buildEquityCurves;  ///WYJ fix, original name: bool_1
        private bool isStreaming;
        private bool riskStopLevelNotSet;
        private bool limitDaySimulation;
        private bool applyInterest;
        private bool applyDividends;
        private bool reduceQtyBasedOnVolume;
        private bool bool_16;
        private static bool bool_17 = false;
        [CompilerGenerated]
        private bool noDecimalRoundingForLimitStopPrice;
        private bool exceptionEvents;
        private bool enableSlippage;
        private bool worstTradeSimulation;
        private bool benchmarkBuyAndHoldON;
        private bool limitOrderSlippage;
        private bool roundLots;
        private bool roundLots50;
        private bool applyCommission;
        private ChartRenderer chartRenderer;
        private WealthLab.Commission commission;
        private DataSource dataSource;   ///WYJ fix, original name: dataSource_0
        private Dictionary<string, Bars> dictionary_0;
        private double slippageUnits;
        private double overrideShareSize;
        private double riskStopLevel;
        private double cashRate;
        private double marginRate;
        private double cashAdjustmentFactor;
        private double marginAdjustmentFactor;
        private double redcuceQtyPct;
        private double autoProfitLevel;
        private static double double_9 = 1.0;
        private WealthLab.FundamentalsLoader fundamentalsLoader_0;
        private IContainer icontainer_0;
        private IList<Bars> ilist_0;
        private int slippageTicks;
        private static int int_1 = 0;
        [CompilerGenerated]
        private int pricingDecimalPlaces;
        private List<string> debugStrings;
        private List<Bars> barsList_Sync;   ///WYJ fix, original signature: list_1; the two lists here are lists for external symbols?
        private List<Bars> barsList;   ///WYJ fix, original signature: list_2
        private List<Position> masterPositions;
        private List<Alert> masterAlerts;
        private List<Position> currentPositions;
        private List<Alert> currentAlerts;
        private List<Bars> barsList_Raw;   ///WYJ fix, raw bars list, added to this list after retrieved, before any other processing, like synch. Original signature: list_7
        [CompilerGenerated]
        private object tag;
        private Position position_0;
        private PositionSize positionSize;
        private static PositionSize positionSize_1 = new PositionSize(PosSizeMode.RawProfitShare, 1.0);
        private WealthLab.PosSizer posSizer;
        public static List<WealthLab.PosSizer> PosSizers = null;
        [CompilerGenerated]
        private WealthLab.Strategy strategy;
        public int StrategyWindowID;
        private string benchmarkSymbol;  ///WYJ fix, original name: string_0
        private string strategyName;
        private SystemPerformance systemPerformance;
        private WealthScript wealthScriptExecuting;

        private EventHandler<StrategyEventArgs> eventHandler_0;

        private EventHandler<DataSourceLookupEventArgs> eventHandler_1;

        private EventHandler<LoadSymbolEventArgs> eventHandler_ExternalSymbolRequested;   ///WYJ fix, original name: eventHandler_2

        private EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler_ExternalSymbolFromDataSetRequested;   ///WYJ fix, original name: eventHandler_3

        private EventHandler<BarsEventArgs> eventHandler_4;

        private EventHandler<BarsEventArgs> eventHandler_ExecutionCompletedForChildStrategySymbol;   ///WYJ fix, original name: eventHandler_5

        private EventHandler<WSExceptionEventArgs> eventHandler_6;

        private EventHandler<EventArgs> eventHandler_FlushDebugWindow;   ///WYJ fix, original name: eventHandler_7

        private EventHandler<EventArgs> eventHandler_ClearDebugWindow;   ///WYJ fix, original name: eventHandler_8

        private EventHandler<DebugStringEventArgs> eventHandler_PrintToStatusBar;   ///WYJ fix, original name: eventHandler_9

        private EventHandler<ChartBitmapEventArgs> eventHandler_ChartBitmapRequested;   ///WYJ fix, original name: eventHandler_10

        private EventHandler<TrendLineEventArgs> eventHandler_TrendlineGetValue;   ///WYJ fix, original name: eventHandler_11

        private EventHandler<StrategyParameterEventArgs> eventHandler_12;

        public event EventHandler<ChartBitmapEventArgs> ChartBitmapRequested
        {
            add
            {
                EventHandler<ChartBitmapEventArgs> eventHandler;
                EventHandler<ChartBitmapEventArgs> eventHandler10 = this.eventHandler_ChartBitmapRequested;
                do
                {
                    eventHandler = eventHandler10;
                    EventHandler<ChartBitmapEventArgs> eventHandler1 = (EventHandler<ChartBitmapEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler10 = Interlocked.CompareExchange<EventHandler<ChartBitmapEventArgs>>(ref this.eventHandler_ChartBitmapRequested, eventHandler1, eventHandler);
                }
                while (eventHandler10 != eventHandler);
            }
            remove
            {
                EventHandler<ChartBitmapEventArgs> eventHandler;
                EventHandler<ChartBitmapEventArgs> eventHandler10 = this.eventHandler_ChartBitmapRequested;
                do
                {
                    eventHandler = eventHandler10;
                    EventHandler<ChartBitmapEventArgs> eventHandler1 = (EventHandler<ChartBitmapEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler10 = Interlocked.CompareExchange<EventHandler<ChartBitmapEventArgs>>(ref this.eventHandler_ChartBitmapRequested, eventHandler1, eventHandler);
                }
                while (eventHandler10 != eventHandler);
            }
        }

        public event EventHandler<EventArgs> ClearDebugWindow
        {
            add
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler8 = this.eventHandler_ClearDebugWindow;
                do
                {
                    eventHandler = eventHandler8;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler8 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_ClearDebugWindow, eventHandler1, eventHandler);
                }
                while (eventHandler8 != eventHandler);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler8 = this.eventHandler_ClearDebugWindow;
                do
                {
                    eventHandler = eventHandler8;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler8 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_ClearDebugWindow, eventHandler1, eventHandler);
                }
                while (eventHandler8 != eventHandler);
            }
        }

        public event EventHandler<BarsEventArgs> ExecutionCompletedForChildStrategySymbol
        {
            add
            {
                EventHandler<BarsEventArgs> eventHandler;
                EventHandler<BarsEventArgs> eventHandler5 = this.eventHandler_ExecutionCompletedForChildStrategySymbol;
                do
                {
                    eventHandler = eventHandler5;
                    EventHandler<BarsEventArgs> eventHandler1 = (EventHandler<BarsEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler5 = Interlocked.CompareExchange<EventHandler<BarsEventArgs>>(ref this.eventHandler_ExecutionCompletedForChildStrategySymbol, eventHandler1, eventHandler);
                }
                while (eventHandler5 != eventHandler);
            }
            remove
            {
                EventHandler<BarsEventArgs> eventHandler;
                EventHandler<BarsEventArgs> eventHandler5 = this.eventHandler_ExecutionCompletedForChildStrategySymbol;
                do
                {
                    eventHandler = eventHandler5;
                    EventHandler<BarsEventArgs> eventHandler1 = (EventHandler<BarsEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler5 = Interlocked.CompareExchange<EventHandler<BarsEventArgs>>(ref this.eventHandler_ExecutionCompletedForChildStrategySymbol, eventHandler1, eventHandler);
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
                EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler3 = this.eventHandler_ExternalSymbolFromDataSetRequested;
                do
                {
                    eventHandler = eventHandler3;
                    EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler1 = (EventHandler<LoadSymbolFromDataSetEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler3 = Interlocked.CompareExchange<EventHandler<LoadSymbolFromDataSetEventArgs>>(ref this.eventHandler_ExternalSymbolFromDataSetRequested, eventHandler1, eventHandler);
                }
                while (eventHandler3 != eventHandler);
            }
            remove
            {
                EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler;
                EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler3 = this.eventHandler_ExternalSymbolFromDataSetRequested;
                do
                {
                    eventHandler = eventHandler3;
                    EventHandler<LoadSymbolFromDataSetEventArgs> eventHandler1 = (EventHandler<LoadSymbolFromDataSetEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler3 = Interlocked.CompareExchange<EventHandler<LoadSymbolFromDataSetEventArgs>>(ref this.eventHandler_ExternalSymbolFromDataSetRequested, eventHandler1, eventHandler);
                }
                while (eventHandler3 != eventHandler);
            }
        }

        public event EventHandler<LoadSymbolEventArgs> ExternalSymbolRequested
        {
            add
            {
                EventHandler<LoadSymbolEventArgs> eventHandler;
                EventHandler<LoadSymbolEventArgs> eventHandler2 = this.eventHandler_ExternalSymbolRequested;
                do
                {
                    eventHandler = eventHandler2;
                    EventHandler<LoadSymbolEventArgs> eventHandler1 = (EventHandler<LoadSymbolEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler2 = Interlocked.CompareExchange<EventHandler<LoadSymbolEventArgs>>(ref this.eventHandler_ExternalSymbolRequested, eventHandler1, eventHandler);
                }
                while (eventHandler2 != eventHandler);
            }
            remove
            {
                EventHandler<LoadSymbolEventArgs> eventHandler;
                EventHandler<LoadSymbolEventArgs> eventHandler2 = this.eventHandler_ExternalSymbolRequested;
                do
                {
                    eventHandler = eventHandler2;
                    EventHandler<LoadSymbolEventArgs> eventHandler1 = (EventHandler<LoadSymbolEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler2 = Interlocked.CompareExchange<EventHandler<LoadSymbolEventArgs>>(ref this.eventHandler_ExternalSymbolRequested, eventHandler1, eventHandler);
                }
                while (eventHandler2 != eventHandler);
            }
        }

        public event EventHandler<EventArgs> FlushDebugWindow
        {
            add
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler7 = this.eventHandler_FlushDebugWindow;
                do
                {
                    eventHandler = eventHandler7;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler7 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_FlushDebugWindow, eventHandler1, eventHandler);
                }
                while (eventHandler7 != eventHandler);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler7 = this.eventHandler_FlushDebugWindow;
                do
                {
                    eventHandler = eventHandler7;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler7 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_FlushDebugWindow, eventHandler1, eventHandler);
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
                EventHandler<DebugStringEventArgs> eventHandler9 = this.eventHandler_PrintToStatusBar;
                do
                {
                    eventHandler = eventHandler9;
                    EventHandler<DebugStringEventArgs> eventHandler1 = (EventHandler<DebugStringEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler9 = Interlocked.CompareExchange<EventHandler<DebugStringEventArgs>>(ref this.eventHandler_PrintToStatusBar, eventHandler1, eventHandler);
                }
                while (eventHandler9 != eventHandler);
            }
            remove
            {
                EventHandler<DebugStringEventArgs> eventHandler;
                EventHandler<DebugStringEventArgs> eventHandler9 = this.eventHandler_PrintToStatusBar;
                do
                {
                    eventHandler = eventHandler9;
                    EventHandler<DebugStringEventArgs> eventHandler1 = (EventHandler<DebugStringEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler9 = Interlocked.CompareExchange<EventHandler<DebugStringEventArgs>>(ref this.eventHandler_PrintToStatusBar, eventHandler1, eventHandler);
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
                EventHandler<TrendLineEventArgs> eventHandler11 = this.eventHandler_TrendlineGetValue;
                do
                {
                    eventHandler = eventHandler11;
                    EventHandler<TrendLineEventArgs> eventHandler1 = (EventHandler<TrendLineEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler11 = Interlocked.CompareExchange<EventHandler<TrendLineEventArgs>>(ref this.eventHandler_TrendlineGetValue, eventHandler1, eventHandler);
                }
                while (eventHandler11 != eventHandler);
            }
            remove
            {
                EventHandler<TrendLineEventArgs> eventHandler;
                EventHandler<TrendLineEventArgs> eventHandler11 = this.eventHandler_TrendlineGetValue;
                do
                {
                    eventHandler = eventHandler11;
                    EventHandler<TrendLineEventArgs> eventHandler1 = (EventHandler<TrendLineEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler11 = Interlocked.CompareExchange<EventHandler<TrendLineEventArgs>>(ref this.eventHandler_TrendlineGetValue, eventHandler1, eventHandler);
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
            this.buildEquityCurves = true;
            this.slippageUnits = 1.0;
            this.slippageTicks = 1;
            this.debugStrings = new List<string>();
            this._activePositions = new List<Position>();
            this.strategyName = "";
            this.redcuceQtyPct = 10.0;
            this.bool_16 = true;
            this.barsList_Sync = new List<Bars>();
            this.barsList = new List<Bars>();
            this.positionSize = new PositionSize();
            this.masterPositions = new List<Position>();
            this.masterAlerts = new List<Alert>();
            this.currentPositions = new List<Position>();
            this.currentAlerts = new List<Alert>();
            this.dictionary_0 = new Dictionary<string, Bars>();
            this.barsList_Raw = new List<Bars>();
            this.systemPerformance = new SystemPerformance(null);
            this.initIContainer();
        }

        public TradingSystemExecutor(IContainer container)
        {
            this.buildEquityCurves = true;
            this.slippageUnits = 1.0;
            this.slippageTicks = 1;
            this.debugStrings = new List<string>();
            this._activePositions = new List<Position>();
            this.strategyName = "";
            this.redcuceQtyPct = 10.0;
            this.bool_16 = true;
            this.barsList_Sync = new List<Bars>();
            this.barsList = new List<Bars>();
            this.positionSize = new PositionSize();
            this.masterPositions = new List<Position>();
            this.masterAlerts = new List<Alert>();
            this.currentPositions = new List<Position>();
            this.currentAlerts = new List<Alert>();
            this.dictionary_0 = new Dictionary<string, Bars>();
            this.barsList_Raw = new List<Bars>();
            this.systemPerformance = new SystemPerformance(null);
            container.Add(this);
            this.initIContainer();
        }

        public void ApplyPositionSize()
        {
            TradingSystemExecutor executor = new TradingSystemExecutor();
            if (this.BenchmarkBuyAndHoldON)
            {
                if (this.DataSet != null)
                {
                    executor.ApplySettings(this);
                    Bars benchmarkData = this.DataSet.Provider.RequestData(this.DataSet, this.BenchmarkSymbol, DateTime.MinValue, DateTime.MaxValue, 0, false);
                    if (benchmarkData.Count == 0)
                    {
                        benchmarkData = this.findBarsDataInPool2(this.BenchmarkSymbol, false);
                    }
                    benchmarkData.SymbolInfo.SecurityType = SecurityType.MutualFund;
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
                    if (benchmarkData.Count != 0)
                    {
                        benchmarkData = BarScaleConverter.Synchronize(benchmarkData, this.ilist_0[num2]);
                    }
                    Bars[] barsArray = new Bars[] { benchmarkData };
                    executor.ilist_0 = barsArray;
                    foreach (Bars bars6 in executor.ilist_0)
                    {
                        if (bars6.Count > 0)
                        {
                            executor.Performance.addToBarsList(bars6);
                        }
                    }
                    this.Performance.BenchmarkSymbolbars = benchmarkData;
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
            this.systemPerformance.RawTrades = this.masterPositions;
            this.systemPerformance.PositionSize = this.PosSize;
            SystemResults results = new SystemResults(this.systemPerformance);
            if (!this.BenchmarkBuyAndHoldON)
            {
                foreach (Position position4 in this.systemPerformance.ResultsBuyHold.Positions)
                {
                    results.method_4(position4);
                }
            }
            int tradesNSF = this.Performance.Results.TradesNSF;
            int num4 = this.Performance.ResultsLong.TradesNSF;
            int num5 = this.Performance.ResultsShort.TradesNSF;
            this.systemPerformance.method_2();
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
                    this.systemPerformance.ResultsBuyHold.method_4(position2);
                }
            }
            if (this.ApplyInterest)
            {
                this.systemPerformance.CashReturnRate = this.CashRate;
            }
            else
            {
                this.systemPerformance.CashReturnRate = 0.0;
            }
            this.riskStopLevelNotSet = false;
            this.posSizer = null;
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
                            this.posSizer = (WealthLab.PosSizer)Activator.CreateInstance(current.GetType());
                            if ((this.PosSize.PosSizerConfig != "") && ((this.PosSize.SimuScriptName == this.PosSize.PosSizerThatWasConfigured) || (this.PosSize.PosSizerThatWasConfigured == "")))
                            {
                                try
                                {
                                    this.posSizer.ApplyConfigString(WealthLab.PosSizer.ParseConfigString(this.PosSize.PosSizerConfig));
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
                this.systemPerformance.addToBarsList(bars7);
            }
            this.systemPerformance.Results.BuildEquityCurve(this.ilist_0, this, true, this.posSizer);
            this.systemPerformance.Results.method_7(true);
            foreach (Position position in this.masterPositions)
            {
                if (position.Shares > 0.0)
                {
                    this.systemPerformance.Results.method_4(position);
                    if (position.PositionType == PositionType.Long)
                    {
                        this.systemPerformance.ResultsLong.method_4(position);
                    }
                    else
                    {
                        this.systemPerformance.ResultsShort.method_4(position);
                    }
                }
                else
                {
                    SystemResults results1 = this.systemPerformance.Results;
                    results1.TradesNSF++;
                    if (position.PositionType == PositionType.Long)
                    {
                        SystemResults resultsLong = this.systemPerformance.ResultsLong;
                        resultsLong.TradesNSF++;
                    }
                    else
                    {
                        SystemResults resultsShort = this.systemPerformance.ResultsShort;
                        resultsShort.TradesNSF++;
                    }
                }
            }
            foreach (Alert alert2 in this.masterAlerts)
            {
                if ((alert2.AlertType != TradeType.Buy) && (alert2.AlertType != TradeType.Short))
                {
                    if ((alert2.Position != null) && (alert2.Position.Shares > 0.0))
                    {
                        this.systemPerformance.Results.method_5(alert2);
                    }
                }
                else
                {
                    this.systemPerformance.Results.method_5(alert2);
                }
            }
            bool reduceQtyBasedOnVolume = this.ReduceQtyBasedOnVolume;
            this.ReduceQtyBasedOnVolume = false;
            int barsCount = 0;
            if (!this.BenchmarkBuyAndHoldON)
            {
                if (this.Strategy.StrategyType != StrategyType.CombinedStrategy)
                {
                    foreach (Bars bars in this.ilist_0)
                    {
                        if (bars.Count > 1)
                        {
                            barsCount++;
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
                                double capital = (this.PosSize.StartingCapital * this.PosSize.MarginFactor) / ((double) barsCount);
                                position5.Shares = this.getSharesForBars(bars5, capital);
                            }
                            if (position5.Shares > 0.0)
                            {
                                position5.EntryBar = 1;
                                position5.EntryPrice = bars5.Open[1];
                                position5.BasisPrice = bars5.Close[0];
                                this.systemPerformance.ResultsBuyHold.method_4(position5);
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
                        barsCount++;
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
                        double num7 = (this.PosSize.StartingCapital * this.PosSize.MarginFactor) / ((double) barsCount);
                        position3.Shares = this.getSharesForBars(bars4, num7);
                    }
                    if (position3.Shares > 0.0)
                    {
                        position3.EntryBar = num6 + 1;
                        position3.EntryPrice = bars4.Open[num6 + 1];
                        position3.BasisPrice = bars4.Close[num6];
                        this.systemPerformance.ResultsBuyHold.method_4(position3);
                        if ((this.Commission != null) && this.ApplyCommission)
                        {
                            position3.EntryCommission = this.Commission.Calculate(TradeType.Buy, OrderType.Market, position3.EntryPrice, position3.Shares, bars4);
                        }
                    }
                }
            }
            this.ReduceQtyBasedOnVolume = reduceQtyBasedOnVolume;
            this.systemPerformance.ResultsLong.BuildEquityCurve(this.ilist_0, this, false, this.posSizer);
            this.systemPerformance.ResultsShort.BuildEquityCurve(this.ilist_0, this, false, this.posSizer);
            this.systemPerformance.ResultsBuyHold.method_8();
            if (!this.BenchmarkBuyAndHoldON)
            {
                this.systemPerformance.ResultsBuyHold.BuildEquityCurve(this.ilist_0, this, false, null);
            }
            else
            {
                if (executor.ilist_0[0].Count == 0)
                {
                    executor.ilist_0[0] = this.ilist_0[0];
                }
                this.systemPerformance.ResultsBuyHold.BuildEquityCurve(executor.ilist_0, executor, false, null);
                this.systemPerformance.ResultsBuyHold.EquityCurve = BarScaleConverter.Synchronize(this.systemPerformance.ResultsBuyHold.EquityCurve, this.systemPerformance.ResultsLong.EquityCurve);
                this.systemPerformance.ResultsBuyHold.CashCurve = BarScaleConverter.Synchronize(this.systemPerformance.ResultsBuyHold.CashCurve, this.systemPerformance.ResultsLong.CashCurve);
            }
            if (this.posSizer != null)
            {
                this.systemPerformance.Results.method_9(this.posSizer);
            }
            foreach (Alert alert in this.systemPerformance.Results.Alerts)
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
            double currentEquity = this.systemPerformance.Results.CurrentEquity;
            double overrideShareSize = 0.0;
            if (this.PosSize.Mode == PosSizeMode.ScriptOverride)
            {
                overrideShareSize = this.PosSize.OverrideShareSize;
            }
            return this.CalcPositionSize(bars, int_3, basisPrice, positionType_0, riskStopLevel, currentEquity, overrideShareSize, 0.0);
        }

        public double CalcPositionSize(Bars bars, int int_3, double basisPrice, PositionType positionType_0, double riskStopLevel, bool comingFromWealthScript)
        {
            double currentEquity = this.systemPerformance.Results.CurrentEquity;
            double overrideShareSize = 0.0;
            if (this.PosSize.Mode == PosSizeMode.ScriptOverride)
            {
                overrideShareSize = this.PosSize.OverrideShareSize;
            }
            return this.doCalcPositionSize(bars, int_3, basisPrice, positionType_0, riskStopLevel, currentEquity, overrideShareSize, 0.0, comingFromWealthScript);
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
            return this.doCalcPositionSize(bars, int_3, basisPrice, positionType_0, riskStopLevel, equity, overrideShareSize, currentCash, false);
        }

        public double CalcPositionSize(Position position_1, Bars bars, int int_3, double basisPrice, PositionType positionType_0, double riskStopLevel, bool useOverRide, double overrideShareSize, double thisBarCash)
        {
            if ((this.Strategy != null) && (this.Strategy.StrategyType == StrategyType.CombinedStrategy))
            {
                return position_1.Shares;
            }
            this.position_0 = position_1;
            double currentEquity = this.systemPerformance.Results.CurrentEquity;
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
            this.masterPositions.Clear();
            this.masterAlerts.Clear();
            this.systemPerformance.method_2();
            this.barsList_Sync.Clear();
            this.barsList.Clear();
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
            this.systemPerformance.Strategy = strategy_1;
            this.barsList_Raw.Clear();
            GC.Collect();
            this.riskStopLevel = 0;
            this.autoProfitLevel = 0;
            this.debugStrings.Clear();
            this.riskStopLevelNotSet = false;
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
                    this.Performance.addToBarsList(bar1);
                }
                this.Performance.Scale = barsCollection[0].Scale;
                this.Performance.BarInterval = barsCollection[0].BarInterval;
                this.Performance.PositionSize = this.PosSize;
                this.wealthScriptExecuting = wealthScript_1;
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
                                chartRenderer0 = this.chartRenderer;
                            }
                            else
                            {
                                chartRenderer0 = null;
                            }
                            ChartRenderer chartRenderer = chartRenderer0;
                            this.executeNonCombinedStrategy(bar2, wealthScript_1, chartRenderer);
                        }
                    }
                    finally
                    {
                        this.PosSize = posSize;
                        this.barsList_Raw.Clear();
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
                            tradingSystemExecutor.ExecutionCompletedForSymbol += new EventHandler<BarsEventArgs>(this.executionCompletedForChildStrategySymbolEventHandler);
                            tradingSystemExecutor.ExternalSymbolRequested += this.eventHandler_ExternalSymbolRequested;
                            tradingSystemExecutor.ExternalSymbolFromDataSetRequested += this.eventHandler_ExternalSymbolFromDataSetRequested;
                            tradingSystemExecutor.ExceptionEvents = true;
                            tradingSystemExecutor.WealthScriptException += new EventHandler<WSExceptionEventArgs>(this.wealthScriptExceptionEventHandler);
                            try
                            {
                                tradingSystemExecutor.Execute(strategy, tag, null, bars1);
                            }
                            catch (Exception exception1)
                            {
                                Exception exception = exception1;
                                tradingSystemExecutor.addDebugString(string.Concat("Exception in Combination Strategy Child: ", strategy.Name));
                                tradingSystemExecutor.addDebugString(exception.Message);
                            }
                            tradingSystemExecutor.ExecutionCompletedForSymbol -= new EventHandler<BarsEventArgs>(this.executionCompletedForChildStrategySymbolEventHandler);
                            tradingSystemExecutor.ExternalSymbolRequested -= this.eventHandler_ExternalSymbolRequested;
                            tradingSystemExecutor.ExternalSymbolFromDataSetRequested -= this.eventHandler_ExternalSymbolFromDataSetRequested;
                            tradingSystemExecutor.WealthScriptException -= new EventHandler<WSExceptionEventArgs>(this.wealthScriptExceptionEventHandler);
                            strs.AddRange(tradingSystemExecutor.DebugStrings);
                            foreach (Position list3 in tradingSystemExecutor.masterPositions)
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
                            this.masterPositions.AddRange(tradingSystemExecutor.Performance.Results.Positions);
                            this.masterAlerts.AddRange(tradingSystemExecutor.Performance.Results.Alerts);
                            if (!this.BenchmarkBuyAndHoldON)
                            {
                                foreach (Position position1 in tradingSystemExecutor.systemPerformance.ResultsBuyHold.Positions)
                                {
                                    this.systemPerformance.ResultsBuyHold.method_4(position1);
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
                        this.debugStrings.Clear();
                        this.debugStrings.AddRange(strs);
                    }
                    finally
                    {
                        this.PosSize = posSize;
                        this.barsList_Raw.Clear();
                        TradingSystemExecutor.bool_0 = false;
                    }
                }
                this.masterPositions.Sort(this);
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
            this.systemPerformance.Results.CurrentCash = this.PosSize.StartingCapital;
            this.systemPerformance.Results.CurrentEquity = this.PosSize.StartingCapital;
        }

        ///WYJ fix, original signature: private void method_0(object sender, BarsEventArgs e)
        private void executionCompletedForChildStrategySymbolEventHandler(object sender, BarsEventArgs e)
        {
            if (this.eventHandler_ExecutionCompletedForChildStrategySymbol != null)
            {
                this.eventHandler_ExecutionCompletedForChildStrategySymbol(this, e);
            }
        }

        ///WYJ fix, original signature: private void method_1(object sender, WSExceptionEventArgs e)
        private void wealthScriptExceptionEventHandler(object sender, WSExceptionEventArgs e)
        {
            TradingSystemExecutor executor = sender as TradingSystemExecutor;
            executor.addDebugString("Exception in Combination Strategy Child: " + e.Strategy.Name);
            executor.addDebugString(e.Exception.Message);
        }

        ///WYJ fix, original signature: internal Bars method_10(string string_2, bool bool_19)
        ///WYJ note, find in barsList_Raw, if not found, try to fetch it
        internal Bars findBarsDataInPool2(string symbol, bool synchronize)
        {
            Bars bars = this.wealthScriptExecuting.Bars;
            Bars item = null;
            if (symbol == bars.Symbol)
            {
                return bars;
            }
            using (List<Bars>.Enumerator enumerator = this.barsList_Raw.GetEnumerator())
            {
                Bars current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Symbol == symbol)
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
                this.BarsLoader.setDataSource(this.dataSource);
                this.dataSource.Provider.IsStreamingRequest = this.IsStreaming;
                BarDataScale barDataScale = this.BarsLoader.BarDataScale;
                this.BarsLoader.Scale = bars.Scale;
                this.BarsLoader.BarInterval = bars.BarInterval;
                this.BarsLoader.AutoConvertScale = false;
                item = this.BarsLoader.GetData(symbol);   ///WYJ note: load the symbol data
                if ((item != null) && (item.Count > 0))
                {
                    this.barsList_Raw.Add(item);
                }
                this.BarsLoader.AutoConvertScale = true;
                this.BarsLoader.OverrideOnDemand = false;
                this.BarsLoader.BarDataScale = barDataScale;
            }
            if (((item == null) || (item.Count == 0)) && (this.eventHandler_ExternalSymbolRequested != null))
            {
                LoadSymbolEventArgs e = new LoadSymbolEventArgs(symbol, bars.Scale, bars.BarInterval);
                this.eventHandler_ExternalSymbolRequested(this, e);
                item = e.SymbolData;
                if ((item != null) && (item.Count > 0))
                {
                    this.barsList_Raw.Add(item);
                }
            }
            Bars bars3 = new Bars(item);
            bars3.Append(item);
            item = bars3;
            if ((synchronize && (item != null)) && (item.Count > 0))
            {
                item = BarScaleConverter.Synchronize(item, bars);
            }
            if (item != null)
            {
                item.lockBars();
                if (synchronize)
                {
                    this.barsList_Sync.Add(item);
                    return item;
                }
                this.barsList.Add(item);
            }
            return item;
        }

        ///WYJ fix, original signature: internal int method_11()
        internal int method_11()
        {
            int num = this.barsList_Sync.Count + this.barsList.Count;
            this.barsList_Sync.Clear();
            this.barsList.Clear();
            return num;
        }

        ///WYJ fix, original signature: internal int method_12(string string_2)
        internal int method_12(string symbol)   ///WYJ note, remove symbol data
        {
            int num = 0;
            for (int i = this.barsList_Sync.Count - 1; i >= 0; i--)
            {
                if (this.barsList_Sync[i].Symbol == symbol)
                {
                    this.barsList_Sync.RemoveAt(i);
                    num++;
                }
            }
            for (int j = this.barsList.Count - 1; j >= 0; j--)
            {
                if (this.barsList[j].Symbol == symbol)
                {
                    this.barsList.RemoveAt(j);
                    num++;
                }
            }
            return num;
        }

        ///WYJ fix, original signature: internal void method_13(Bars bars_1, bool bool_19)
        internal void method_13(Bars bars_1, bool synchronize)
        {
            bars_1.lockBars();
            if (synchronize)
            {
                this.barsList_Sync.Add(bars_1);
            }
            else
            {
                this.barsList.Add(bars_1);
            }
        }

        ///WYJ fix, original signature: internal double method_14(double double_10, bool bool_19, Bars bars_1)
        internal double priceSlip(double price, bool limitOrder, Bars argBars)
        {
            if (!this.EnableSlippage)
            {
                return 0.0;
            }
            if (limitOrder && !this.LimitOrderSlippage)
            {
                return 0.0;
            }
            if (argBars.SymbolInfo.SecurityType == SecurityType.Future)
            {
                return (this.SlippageTicks * argBars.SymbolInfo.Tick);
            }
            return ((0.01 * this.SlippageUnits) * price);
        }

        ///WYJ fix, original signature: internal void method_15(string string_2)
        internal void addDebugString(string argStr)
        {
            this.debugStrings.Add(argStr);
        }

        ///WYJ fix, original signature: internal void method_16()
        internal void flushDebugWindow()
        {
            if (this.eventHandler_FlushDebugWindow != null)
            {
                this.eventHandler_FlushDebugWindow(this, EventArgs.Empty);
            }
        }

        ///WYJ fix, original signature: internal void method_17()
        internal void clearDebugWindow()
        {
            this.debugStrings.Clear();
            if (this.eventHandler_ClearDebugWindow != null)
            {
                this.eventHandler_ClearDebugWindow(this, EventArgs.Empty);
            }
        }

        ///WYJ fix, original signature: internal void method_18(string string_2)
        internal void printToStatusBar(string string_2)
        {
            if (this.eventHandler_PrintToStatusBar != null)
            {
                this.eventHandler_PrintToStatusBar(this, new DebugStringEventArgs(string_2));
            }
        }

        ///WYJ fix, original signature: internal Bitmap method_19(int int_3, int int_4)
        internal Bitmap getChartBitmap(int width, int height)
        {
            Bitmap image = null;
            if (this.chartRenderer == null)
            {
                return null;
            }
            if (this.eventHandler_ChartBitmapRequested != null)
            {
                ChartBitmapEventArgs e = new ChartBitmapEventArgs(width, height);
                this.eventHandler_ChartBitmapRequested(this, e);
                image = e.Bitmap;
            }
            if (image == null)
            {
                image = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(image);
                using (graphics)
                {
                    this.chartRenderer.Executing = false;
                    this.chartRenderer.Render(this.barsBeingProcessed, graphics, width, height, this.chartRenderer.ChartStyle);
                    this.chartRenderer.Executing = true;
                }
            }
            return image;
        }

        ///WYJ fix, original signature: private void method_2(Bars bars_1, WealthScript wealthScript_1, ChartRenderer chartRenderer_1)
        private void executeNonCombinedStrategy(Bars bars, WealthScript wealthScript, ChartRenderer chartRenderer)
        {
            this.CurrentPositions.Clear();
            this.CurrentAlerts.Clear();
            this.ActivePositions.Clear();
            this.method_11();
            try
            {
                this.barsBeingProcessed = bars;
                bars.lockBars();
                if (!bool_0)
                {
                    if (this.eventHandler_12 != null)
                    {
                        this.eventHandler_12(this, new StrategyParameterEventArgs(wealthScript, bars.Symbol));
                    }
                }
                else if (this.Strategy.UsePreferredValues)
                {
                    this.Strategy.LoadPreferredValues(bars.Symbol, wealthScript);
                }
                wealthScript.prepareAndExecute(bars, chartRenderer, this, this.dataSource);
                wealthScript.RestoreScale();
                bars.unlockBars();
            }
            catch (Exception exception)
            {
                bars.unlockBars();
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
                this.eventHandler_4(this, new BarsEventArgs(bars));
            }
        }

        ///WYJ fix, original signature: internal double method_20(int int_3, string string_2)
        internal double trendlineGetValue(int int_3, string string_2)
        {
            if (this.eventHandler_TrendlineGetValue == null)
            {
                return 0.0;
            }
            TrendLineEventArgs e = new TrendLineEventArgs(string_2, int_3);
            this.eventHandler_TrendlineGetValue(this, e);
            return e.Value;
        }

        ///WYJ fix, original signature: private void method_21()
        private void initIContainer()
        {
            this.icontainer_0 = new Container();
        }

        ///WYJ fix, original signature: private int method_3(Bars bars_1, double double_10)
        private int getSharesForBars(Bars bars_1, double capital)
        {
            int shares = 0;
            if (WealthLab.BarsLoader.FuturesMode)
            {
                SymbolInfo symbolInfo = bars_1.SymbolInfo;
                if ((symbolInfo.SecurityType == SecurityType.Future) && (symbolInfo.Margin > 0.0))
                {
                    shares = (int) (capital / symbolInfo.Margin);
                }
            }
            if (shares == 0)
            {
                shares = (int) (capital / bars_1.Close[0]);
            }
            return shares;
        }

        ///WYJ fix, original signature: internal double method_4(Bars bars_1, int int_3, double double_10, PositionType positionType_0, double double_11, double double_12, double double_13, double double_14, bool bool_19)
        internal double doCalcPositionSize(Bars bars_1, int int_3, double double_10, PositionType positionType_0, double double_11, double double_12, double double_13, double double_14, bool bool_19)
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
                            this.riskStopLevelNotSet = true;
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
                    if (this.posSizer != null)
                    {
                        try
                        {
                            rawProfitShareSize = this.posSizer.SizePosition(this.position_0, bars_1, int_3 - 1, double_10, positionType_0, double_11, double_12, double_14);
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

        ///WYJ fix, original signature: internal void method_5(Position position_1)
        internal void addPosition(Position position)
        {
            this.MasterPositions.Add(position);
            this.CurrentPositions.Add(position);
            this.ActivePositions.Add(position);
        }

        ///WYJ fix, original signature: internal void method_6(Alert alert_0)
        internal void addAlert(Alert alert)
        {
            if (alert.Shares > 0.0)
            {
                alert.PosSize = this.PosSize;
                this.MasterAlerts.Add(alert);
                this.CurrentAlerts.Add(alert);
            }
        }

        ///WYJ fix, original signature: internal Bars method_7(string string_2, BarScale barScale_0, int int_3, bool bool_19)
        internal Bars findBarsData(string symbol, BarScale barScale_0, int barInterval, bool synchronize)
        {
            if (synchronize)
            {
                return this.findBarsData(symbol, barScale_0, barInterval, this.barsList_Sync);
            }
            return this.findBarsData(symbol, barScale_0, barInterval, this.barsList);
        }


        ///WYJ fix, original signature: private Bars method_8(string string_2, BarScale barScale_0, int int_3, List<Bars> list_8)
        private Bars findBarsData(string symbol, BarScale barScale_0, int barsInterval, List<Bars> list)
        {
            Bars bars;
            using (List<Bars>.Enumerator enumerator = list.GetEnumerator())
            {
                Bars current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (((current.Symbol == symbol) && (current.Scale == barScale_0)) && (current.BarInterval == barsInterval))
                    {
                        ///goto  Label_0040;  ///WYJ fix, simplify the flow
                        bars = current;
                        return bars;
                    }
                }
                return null;
            }
        }

        
        ///WYJ fix, original signature: internal Bars method_9(string string_2, string string_3, bool bool_19)
        internal Bars getExternalSymbol(string datasetName, string symbol, bool synchronize)
        {
            Bars bars = this.wealthScriptExecuting.Bars;
            Bars result = null;
            string key = datasetName + "|" + symbol;
            if (this.dictionary_0.ContainsKey(key))
            {
                result = this.dictionary_0[key];
            }
            if ((result == null) && (this.eventHandler_ExternalSymbolFromDataSetRequested != null))
            {
                LoadSymbolFromDataSetEventArgs e = new LoadSymbolFromDataSetEventArgs(datasetName, symbol);
                this.eventHandler_ExternalSymbolFromDataSetRequested(this, e);
                result = e.Bars;
                if (result != null)
                {
                    this.dictionary_0[key] = result;
                }
            }
            if ((synchronize && (result != null)) && (result.Count > 0))
            {
                result = BarScaleConverter.Synchronize(result, bars);
            }
            return result;
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
                return this.applyCommission;
            }
            set
            {
                this.applyCommission = value;
            }
        }

        public bool ApplyDividends
        {
            get
            {
                return this.applyDividends;
            }
            set
            {
                this.applyDividends = value;
            }
        }

        public bool ApplyInterest
        {
            get
            {
                return this.applyInterest;
            }
            set
            {
                this.applyInterest = value;
            }
        }

        internal double AutoProfitLevel
        {
            get
            {
                return this.autoProfitLevel;
            }
            set
            {
                this.autoProfitLevel = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Bars BarsBeingProcessed
        {
            get
            {
                return this.barsBeingProcessed;
            }
        }

        public WealthLab.BarsLoader BarsLoader
        {
            get
            {
                return this.barsLoader;
            }
            set
            {
                this.barsLoader = value;
            }
        }

        public bool BenchmarkBuyAndHoldON
        {
            get
            {
                return this.benchmarkBuyAndHoldON;
            }
            set
            {
                this.benchmarkBuyAndHoldON = value;
            }
        }

        public string BenchmarkSymbol
        {
            get
            {
                return this.benchmarkSymbol;
            }
            set
            {
                this.benchmarkSymbol = value;
            }
        }

        public bool BuildEquityCurves
        {
            get
            {
                return this.buildEquityCurves;
            }
            set
            {
                this.buildEquityCurves = value;
            }
        }

        public double CashAdjustmentFactor
        {
            get
            {
                return this.cashAdjustmentFactor;
            }
        }

        public double CashRate
        {
            get
            {
                return this.cashRate;
            }
            set
            {
                this.cashRate = value;
                this.cashAdjustmentFactor = Math.Exp(Math.Log(1.0 + (this.cashRate / 100.0)) / 365.25);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public WealthLab.Commission Commission
        {
            get
            {
                return this.commission;
            }
            set
            {
                this.commission = value;
            }
        }

        internal List<Alert> CurrentAlerts
        {
            get
            {
                return this.currentAlerts;
            }
        }

        internal List<Position> CurrentPositions
        {
            get
            {
                return this.currentPositions;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataSource DataSet
        {
            get
            {
                return this.dataSource;
            }
            set
            {
                this.dataSource = value;
            }
        }

        public IList<string> DebugStrings
        {
            get
            {
                return this.debugStrings;
            }
        }

        public bool EnableSlippage
        {
            get
            {
                return this.enableSlippage;
            }
            set
            {
                this.enableSlippage = value;
            }
        }

        public bool ExceptionEvents
        {
            get
            {
                return this.exceptionEvents;
            }
            set
            {
                this.exceptionEvents = value;
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
                return this.isStreaming;
            }
            set
            {
                this.isStreaming = value;
            }
        }

        public bool LimitDaySimulation
        {
            get
            {
                return this.limitDaySimulation;
            }
            set
            {
                this.limitDaySimulation = value;
            }
        }

        public bool LimitOrderSlippage
        {
            get
            {
                return this.limitOrderSlippage;
            }
            set
            {
                this.limitOrderSlippage = value;
            }
        }

        public double MarginAdjustmentFactor
        {
            get
            {
                return this.marginAdjustmentFactor;
            }
        }

        public double MarginRate
        {
            get
            {
                return this.marginRate;
            }
            set
            {
                this.marginRate = value;
                this.marginAdjustmentFactor = Math.Exp(Math.Log(1.0 + (this.marginRate / 100.0)) / 365.25);
            }
        }

        internal List<Alert> MasterAlerts
        {
            get
            {
                return this.masterAlerts;
            }
        }

        public List<Position> MasterPositions
        {
            get
            {
                return this.masterPositions;
            }
        }

        public bool NoDecimalRoundingForLimitStopPrice
        {
            [CompilerGenerated]
            get
            {
                return this.noDecimalRoundingForLimitStopPrice;
            }
            [CompilerGenerated]
            set
            {
                this.noDecimalRoundingForLimitStopPrice = value;
            }
        }

        public double OverrideShareSize
        {
            get
            {
                return this.overrideShareSize;
            }
            set
            {
                this.overrideShareSize = value;
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
                return this.systemPerformance;
            }
            set
            {
                this.systemPerformance = value;
            }
        }

        public PositionSize PosSize
        {
            get
            {
                return this.positionSize;
            }
            set
            {
                this.positionSize = value;
            }
        }

        internal WealthLab.PosSizer PosSizer
        {
            get
            {
                return this.posSizer;
            }
        }

        public int PricingDecimalPlaces
        {
            [CompilerGenerated]
            get
            {
                return this.pricingDecimalPlaces;
            }
            [CompilerGenerated]
            set
            {
                this.pricingDecimalPlaces = value;
            }
        }

        public double RedcuceQtyPct
        {
            get
            {
                return this.redcuceQtyPct;
            }
            set
            {
                this.redcuceQtyPct = value;
            }
        }

        public bool ReduceQtyBasedOnVolume
        {
            get
            {
                return this.reduceQtyBasedOnVolume;
            }
            set
            {
                this.reduceQtyBasedOnVolume = value;
            }
        }

        public ChartRenderer Renderer
        {
            get
            {
                return this.chartRenderer;
            }
            set
            {
                this.chartRenderer = value;
            }
        }

        internal double RiskStopLevel
        {
            get
            {
                return this.riskStopLevel;
            }
            set
            {
                this.riskStopLevel = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool RiskStopLevelNotSet
        {
            get
            {
                return this.riskStopLevelNotSet;
            }
            set
            {
                this.riskStopLevelNotSet = value;
            }
        }

        public bool RoundLots
        {
            get
            {
                return this.roundLots;
            }
            set
            {
                this.roundLots = value;
            }
        }

        public bool RoundLots50
        {
            get
            {
                return this.roundLots50;
            }
            set
            {
                this.roundLots50 = value;
            }
        }

        public int SlippageTicks
        {
            get
            {
                return this.slippageTicks;
            }
            set
            {
                this.slippageTicks = value;
            }
        }

        public double SlippageUnits
        {
            get
            {
                return this.slippageUnits;
            }
            set
            {
                this.slippageUnits = value;
            }
        }

        public WealthLab.Strategy Strategy
        {
            [CompilerGenerated]
            get
            {
                return this.strategy;
            }
            [CompilerGenerated]
            set
            {
                this.strategy = value;
            }
        }

        public string StrategyName
        {
            get
            {
                return this.strategyName;
            }
            set
            {
                this.strategyName = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Tag
        {
            [CompilerGenerated]
            get
            {
                return this.tag;
            }
            [CompilerGenerated]
            set
            {
                this.tag = value;
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
                return this.wealthScriptExecuting;
            }
        }

        public bool WorstTradeSimulation
        {
            get
            {
                return this.worstTradeSimulation;
            }
            set
            {
                this.worstTradeSimulation = value;
            }
        }
    }
}

