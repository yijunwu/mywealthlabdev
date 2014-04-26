namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class SystemPerformance
    {
        private WealthLab.Bars benchmarkSymbolBars;
        private BarScale barScale;
        private double cashReturnRate;
        private int barInterval;
        private List<WealthLab.Bars> barsList = new List<WealthLab.Bars>();
        private List<PlottedIndicator> list_1 = new List<PlottedIndicator>();
        private List<Position> rawTrades;
        private WealthLab.PositionSize positionSize = new WealthLab.PositionSize();
        [CompilerGenerated]
        private WealthLab.Strategy strategy;
        private SystemResults systemResults;
        private SystemResults systemResultsLong;
        private SystemResults systemResultsShort;
        private SystemResults systemResultsBuyHold;

        private EventHandler<EventArgs> eventHandler_0;

        private EventHandler<EventArgs> eventHandler_1;

        public event EventHandler<EventArgs> Signal
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

        public SystemPerformance(WealthLab.Strategy strategy)
        {
            this.Strategy = strategy;
            this.systemResults = new SystemResults(this);
            this.systemResultsLong = new SystemResults(this);
            this.systemResultsShort = new SystemResults(this);
            this.systemResultsBuyHold = new SystemResults(this);
        }

        public SystemPerformance GenerateChildStrategyPerformance(CombinedStrategyInfo combinedStrategyInfo_0, TradingSystemExecutor tradingSystemExecutor_0)
        {
            new List<Position>();
            SystemPerformance performance = new SystemPerformance(this.Strategy) {
                PositionSize = combinedStrategyInfo_0.PositionSize
            };
            foreach (Position position2 in this.Results.Positions)
            {
                WealthLab.Bars item = position2.Bars;
                if (!performance.Bars.Contains(item))
                {
                    performance.Bars.Add(item);
                }
                if (position2.CSI == combinedStrategyInfo_0)
                {
                    performance.Results.method_4(position2);
                    if (position2.PositionType == PositionType.Long)
                    {
                        performance.ResultsLong.method_4(position2);
                    }
                    else
                    {
                        performance.ResultsShort.method_4(position2);
                    }
                }
            }
            performance.Results.BuildEquityCurve(this.barsList, tradingSystemExecutor_0, false, tradingSystemExecutor_0.PosSizer);
            performance.ResultsLong.BuildEquityCurve(this.barsList, tradingSystemExecutor_0, false, tradingSystemExecutor_0.PosSizer);
            performance.ResultsShort.BuildEquityCurve(this.barsList, tradingSystemExecutor_0, false, tradingSystemExecutor_0.PosSizer);
            if (this.BenchmarkSymbolbars == null)
            {
                foreach (Position position in this.ResultsBuyHold.Positions)
                {
                    if (position.StrategyID == combinedStrategyInfo_0.StrategyID.ToString())
                    {
                        performance.ResultsBuyHold.method_4(position);
                    }
                }
                performance.ResultsBuyHold.BuildEquityCurve(this.barsList, tradingSystemExecutor_0, false, tradingSystemExecutor_0.PosSizer);
            }
            else
            {
                performance.ResultsBuyHold = this.ResultsBuyHold;
            }
            performance.BenchmarkSymbolbars = this.BenchmarkSymbolbars;
            return performance;
        }

        internal void method_0()
        {
            this.systemResults.method_0();
            this.systemResultsLong.method_0();
            this.systemResultsShort.method_0();
            this.systemResultsBuyHold.method_0();
        }

        ///WYJ fix, original signature: internal void method_1(WealthLab.Bars bars_1)
        internal void addToBarsList(WealthLab.Bars bars_1)
        {
            this.barsList.Add(bars_1);
        }

        internal void method_2()
        {
            this.systemResults.method_6();
            this.systemResultsLong.method_6();
            this.systemResultsShort.method_6();
            this.systemResultsBuyHold.method_6();
            this.barsList.Clear();
        }

        public void SignalEvent(string string_0)
        {
            if (this.eventHandler_0 != null)
            {
                this.eventHandler_0(string_0, null);
            }
        }

        public int BarInterval
        {
            get
            {
                return this.barInterval;
            }
            internal set
            {
                this.barInterval = value;
            }
        }

        public List<WealthLab.Bars> Bars
        {
            get
            {
                return this.barsList;
            }
        }

        public WealthLab.Bars BenchmarkSymbolbars
        {
            get
            {
                return this.benchmarkSymbolBars;
            }
            set
            {
                this.benchmarkSymbolBars = value;
            }
        }

        public double CashReturnRate
        {
            get
            {
                return this.cashReturnRate;
            }
            set
            {
                this.cashReturnRate = value;
            }
        }

        public bool IsIntraday
        {
            get
            {
                if ((this.Scale != BarScale.Minute) && (this.Scale != BarScale.Second))
                {
                    return (this.Scale == BarScale.Tick);
                }
                return true;
            }
        }

        public WealthLab.PositionSize PositionSize
        {
            get
            {
                return this.positionSize;
            }
            internal set
            {
                this.positionSize = value;
            }
        }

        public List<Position> RawTrades
        {
            get
            {
                return this.rawTrades;
            }
            internal set
            {
                this.rawTrades = value;
            }
        }

        public SystemResults Results
        {
            get
            {
                return this.systemResults;
            }
            internal set
            {
                this.systemResults = value;
            }
        }

        public SystemResults ResultsBuyHold
        {
            get
            {
                return this.systemResultsBuyHold;
            }
            internal set
            {
                this.systemResultsBuyHold = value;
            }
        }

        public SystemResults ResultsLong
        {
            get
            {
                return this.systemResultsLong;
            }
            internal set
            {
                this.systemResultsLong = value;
            }
        }

        public SystemResults ResultsShort
        {
            get
            {
                return this.systemResultsShort;
            }
            internal set
            {
                this.systemResultsShort = value;
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale;
            }
            internal set
            {
                this.barScale = value;
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
    }
}

