namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public abstract class PosSizer
    {
        private DataSeries dataSeries_EquityCurve;
        private DataSeries dataSeries_CashCurve;
        private DataSeries dataSeries_DrawDownCurve;
        private DataSeries dataSeries_DrawDownPctCurve;
        private List<Position> activePositions;
        private List<Position> positions;
        private List<Position> closedPositions;
        private List<Position> candidates;
        private TradingSystemExecutor tradingSystemExecutor;

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
                    PositionSize posSize = this.tradingSystemExecutor.PosSize;
                    this.tradingSystemExecutor.PosSize = new PositionSize(mode, posSizeValue);
                    double num = this.tradingSystemExecutor.CalcPositionSize(bars, int_0 + 1, costBasis, positionType_0, riskStopLevel, equity);
                    this.tradingSystemExecutor.PosSize = posSize;
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
            this.activePositions = list_4;
            this.positions = list_5;
            this.closedPositions = list_6;
            this.tradingSystemExecutor = tradingSystemExecutor_1;
            this.dataSeries_EquityCurve = dataSeries_4;
            this.dataSeries_CashCurve = dataSeries_5;
            this.dataSeries_DrawDownCurve = dataSeries_6;
            this.dataSeries_DrawDownPctCurve = dataSeries_7;
        }

        public static string ParseConfigString(string configStr)
        {
            if (configStr.Length < 1)
            {
                return configStr;
            }
            if (configStr[0] != '*')
            {
                return configStr;
            }
            return configStr.Split(new char[] { '^' })[1];
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
                return this.activePositions;
            }
            internal set
            {
                this.activePositions = value;
            }
        }

        public List<Position> Candidates
        {
            get
            {
                return this.candidates;
            }
            internal set
            {
                this.candidates = value;
            }
        }

        public DataSeries CashCurve
        {
            get
            {
                return this.dataSeries_CashCurve;
            }
            internal set
            {
                this.dataSeries_CashCurve = value;
            }
        }

        public List<Position> ClosedPositions
        {
            get
            {
                return this.closedPositions;
            }
            internal set
            {
                this.closedPositions = value;
            }
        }

        public DataSeries DrawDownCurve
        {
            get
            {
                return this.dataSeries_DrawDownCurve;
            }
            internal set
            {
                this.dataSeries_DrawDownCurve = value;
            }
        }

        public DataSeries DrawDownPctCurve
        {
            get
            {
                return this.dataSeries_DrawDownPctCurve;
            }
            internal set
            {
                this.dataSeries_DrawDownPctCurve = value;
            }
        }

        public DataSeries EquityCurve
        {
            get
            {
                return this.dataSeries_EquityCurve;
            }
            internal set
            {
                this.dataSeries_EquityCurve = value;
            }
        }

        public abstract string FriendlyName { get; }

        public List<Position> Positions
        {
            get
            {
                return this.positions;
            }
            internal set
            {
                this.positions = value;
            }
        }
    }
}

