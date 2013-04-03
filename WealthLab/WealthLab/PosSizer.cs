namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public abstract class PosSizer
    {
        private DataSeries dataSeries_0;
        private DataSeries dataSeries_1;
        private DataSeries dataSeries_2;
        private DataSeries dataSeries_3;
        private List<Position> list_0;
        private List<Position> list_1;
        private List<Position> list_2;
        private List<Position> list_3;
        private TradingSystemExecutor tradingSystemExecutor_0;

        protected PosSizer()
        {
        }

        public virtual void ApplyConfigString(string config)
        {
        }

        protected double CalcPositionSize(PosSizeMode mode, double posSizeValue, Bars bars, int int_0, PositionType positionType_0, double costBasis, double riskStopLevel, double equity)
        {
            switch (mode)
            {
                case PosSizeMode.Dollar:
                case PosSizeMode.PctEquity:
                case PosSizeMode.MaxRisk:
                {
                    PositionSize posSize = this.tradingSystemExecutor_0.PosSize;
                    this.tradingSystemExecutor_0.PosSize = new PositionSize(mode, posSizeValue);
                    double num = this.tradingSystemExecutor_0.CalcPositionSize(bars, int_0 + 1, costBasis, positionType_0, riskStopLevel, equity);
                    this.tradingSystemExecutor_0.PosSize = posSize;
                    return num;
                }
            }
            throw new ArgumentException("Invalid PosSize Mode: " + mode);
        }

        public virtual string GetConfigString()
        {
            return "";
        }

        protected double GetPosSizeValue(PosSizeMode mode, double fixedDollarSize, double pctEquitySize, double maxRiskSize)
        {
            switch (mode)
            {
                case PosSizeMode.Dollar:
                    return fixedDollarSize;

                case PosSizeMode.PctEquity:
                    return pctEquitySize;

                case PosSizeMode.MaxRisk:
                    return maxRiskSize;
            }
            throw new ArgumentException("Invalid PosSizeMode: " + mode);
        }

        public virtual void Initialize()
        {
        }

        internal void method_0(TradingSystemExecutor tradingSystemExecutor_1, List<Position> list_4, List<Position> list_5, List<Position> list_6, DataSeries dataSeries_4, DataSeries dataSeries_5, DataSeries dataSeries_6, DataSeries dataSeries_7)
        {
            this.list_0 = list_4;
            this.list_1 = list_5;
            this.list_2 = list_6;
            this.tradingSystemExecutor_0 = tradingSystemExecutor_1;
            this.dataSeries_0 = dataSeries_4;
            this.dataSeries_1 = dataSeries_5;
            this.dataSeries_2 = dataSeries_6;
            this.dataSeries_3 = dataSeries_7;
        }

        public static string ParseConfigString(string string_0)
        {
            if (string_0.Length < 1)
            {
                return string_0;
            }
            if (string_0[0] != '*')
            {
                return string_0;
            }
            return string_0.Split(new char[] { '^' })[1];
        }

        public abstract double SizePosition(Position currentPos, Bars bars, int int_0, double basisPrice, PositionType positionType_0, double riskStopLevel, double equity, double cash);
        public override string ToString()
        {
            return this.FriendlyName;
        }

        public List<Position> ActivePositions
        {
            get
            {
                return this.list_0;
            }
            internal set
            {
                this.list_0 = value;
            }
        }

        public List<Position> Candidates
        {
            get
            {
                return this.list_3;
            }
            internal set
            {
                this.list_3 = value;
            }
        }

        public DataSeries CashCurve
        {
            get
            {
                return this.dataSeries_1;
            }
            internal set
            {
                this.dataSeries_1 = value;
            }
        }

        public List<Position> ClosedPositions
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

        public DataSeries DrawDownCurve
        {
            get
            {
                return this.dataSeries_2;
            }
            internal set
            {
                this.dataSeries_2 = value;
            }
        }

        public DataSeries DrawDownPctCurve
        {
            get
            {
                return this.dataSeries_3;
            }
            internal set
            {
                this.dataSeries_3 = value;
            }
        }

        public DataSeries EquityCurve
        {
            get
            {
                return this.dataSeries_0;
            }
            internal set
            {
                this.dataSeries_0 = value;
            }
        }

        public abstract string FriendlyName { get; }

        public List<Position> Positions
        {
            get
            {
                return this.list_1;
            }
            internal set
            {
                this.list_1 = value;
            }
        }
    }
}

