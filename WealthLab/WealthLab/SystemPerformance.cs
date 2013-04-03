namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class SystemPerformance
    {
        private WealthLab.Bars bars_0;
        private BarScale barScale_0;
        private double double_0;
        private int int_0;
        private List<WealthLab.Bars> list_0 = new List<WealthLab.Bars>();
        private List<PlottedIndicator> list_1 = new List<PlottedIndicator>();
        private List<Position> list_2;
        private WealthLab.PositionSize positionSize_0 = new WealthLab.PositionSize();
        [CompilerGenerated]
        private WealthLab.Strategy strategy_0;
        private SystemResults systemResults_0;
        private SystemResults systemResults_1;
        private SystemResults systemResults_2;
        private SystemResults systemResults_3;

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
            this.systemResults_0 = new SystemResults(this);
            this.systemResults_1 = new SystemResults(this);
            this.systemResults_2 = new SystemResults(this);
            this.systemResults_3 = new SystemResults(this);
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
            performance.Results.BuildEquityCurve(this.list_0, tradingSystemExecutor_0, false, tradingSystemExecutor_0.PosSizer);
            performance.ResultsLong.BuildEquityCurve(this.list_0, tradingSystemExecutor_0, false, tradingSystemExecutor_0.PosSizer);
            performance.ResultsShort.BuildEquityCurve(this.list_0, tradingSystemExecutor_0, false, tradingSystemExecutor_0.PosSizer);
            if (this.BenchmarkSymbolbars == null)
            {
                foreach (Position position in this.ResultsBuyHold.Positions)
                {
                    if (position.StrategyID == combinedStrategyInfo_0.StrategyID.ToString())
                    {
                        performance.ResultsBuyHold.method_4(position);
                    }
                }
                performance.ResultsBuyHold.BuildEquityCurve(this.list_0, tradingSystemExecutor_0, false, tradingSystemExecutor_0.PosSizer);
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
            this.systemResults_0.method_0();
            this.systemResults_1.method_0();
            this.systemResults_2.method_0();
            this.systemResults_3.method_0();
        }

        internal void method_1(WealthLab.Bars bars_1)
        {
            this.list_0.Add(bars_1);
        }

        internal void method_2()
        {
            this.systemResults_0.method_6();
            this.systemResults_1.method_6();
            this.systemResults_2.method_6();
            this.systemResults_3.method_6();
            this.list_0.Clear();
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
                return this.int_0;
            }
            internal set
            {
                this.int_0 = value;
            }
        }

        public List<WealthLab.Bars> Bars
        {
            get
            {
                return this.list_0;
            }
        }

        public WealthLab.Bars BenchmarkSymbolbars
        {
            get
            {
                return this.bars_0;
            }
            set
            {
                this.bars_0 = value;
            }
        }

        public double CashReturnRate
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
                return this.positionSize_0;
            }
            internal set
            {
                this.positionSize_0 = value;
            }
        }

        public List<Position> RawTrades
        {
            get
            {
                return this.list_2;
            }
            internal set
            {
                this.list_2 = value;
            }
        }

        public SystemResults Results
        {
            get
            {
                return this.systemResults_0;
            }
            internal set
            {
                this.systemResults_0 = value;
            }
        }

        public SystemResults ResultsBuyHold
        {
            get
            {
                return this.systemResults_3;
            }
            internal set
            {
                this.systemResults_3 = value;
            }
        }

        public SystemResults ResultsLong
        {
            get
            {
                return this.systemResults_1;
            }
            internal set
            {
                this.systemResults_1 = value;
            }
        }

        public SystemResults ResultsShort
        {
            get
            {
                return this.systemResults_2;
            }
            internal set
            {
                this.systemResults_2 = value;
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale_0;
            }
            internal set
            {
                this.barScale_0 = value;
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
    }
}

