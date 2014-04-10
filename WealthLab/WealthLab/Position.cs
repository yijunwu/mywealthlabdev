namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class Position
    {
        public static readonly Position AllPositions = new Position(null, WealthLab.PositionType.Long, "");
        private WealthLab.Bars bars;
        private bool active;
        [CompilerGenerated]
        private CombinedStrategyInfo combinedStrategyInfo;
        private double overrideShareSize;
        private double shares;
        private double splitFactor;
        private double priority;
        private double riskStopLevel;
        private double autoProfitLevel;
        private double double_14;
        private double double_15;
        private double double_16;
        private double double_17;
        private double entryPrice;
        private double exitPrice;
        private double basisPrice;
        private double mfe;
        private double mae;
        private double entryCommission;
        private double exitCommission;
        private double trailingStop;
        private int entryBar;
        private int exitBar;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        [CompilerGenerated]
        private int combinedPriority;
        private object tag;
        private OrderType entryOrderType;
        private OrderType exitOrderType;
        private WealthLab.PositionType positionType;
        private static Random random_0;
        private string entrySignal;
        private string exitSignal;
        [CompilerGenerated]
        private string strategyID;

        public Position(WealthLab.Bars bars, WealthLab.PositionType positionType_1, string strategyID)
        {
            this.active = true;
            this.exitBar = -1;
            this.mfe = double.NaN;
            this.mae = double.NaN;
            this.splitFactor = 1.0;
            this.int_2 = -1;
            this.double_14 = -1.0;
            this.int_3 = -1;
            this.double_15 = -1.0;
            this.int_4 = -1;
            this.double_16 = -1.0;
            this.int_5 = -1;
            this.double_17 = -1.0;
            this.StrategyID = strategyID;
            this.bars = bars;
            this.positionType = positionType_1;
            if (random_0 == null)
            {
                random_0 = new Random();
            }
            this.priority = random_0.NextDouble();
        }

        public Position(WealthLab.Bars bars, WealthLab.PositionType positionType_1, double basisPrice, int entryBar, double entryPrice, int exitBar, double exitPrice)
        {
            this.active = true;
            this.exitBar = -1;
            this.mfe = double.NaN;
            this.mae = double.NaN;
            this.splitFactor = 1.0;
            this.int_2 = -1;
            this.double_14 = -1.0;
            this.int_3 = -1;
            this.double_15 = -1.0;
            this.int_4 = -1;
            this.double_16 = -1.0;
            this.int_5 = -1;
            this.double_17 = -1.0;
            this.bars = bars;
            this.positionType = positionType_1;
            if (random_0 == null)
            {
                random_0 = new Random();
            }
            this.priority = random_0.NextDouble();
            this.entryBar = entryBar;
            this.entryPrice = entryPrice;
            this.exitBar = exitBar;
            this.exitPrice = exitPrice;
            this.basisPrice = basisPrice;
            this.active = exitBar < 0;
        }

        public Position(WealthLab.Bars bars, WealthLab.PositionType positionType_1, double basisPrice, int entryBar, double entryPrice, int exitBar, double exitPrice, string strategyID)
        {
            this.active = true;
            this.exitBar = -1;
            this.mfe = double.NaN;
            this.mae = double.NaN;
            this.splitFactor = 1.0;
            this.int_2 = -1;
            this.double_14 = -1.0;
            this.int_3 = -1;
            this.double_15 = -1.0;
            this.int_4 = -1;
            this.double_16 = -1.0;
            this.int_5 = -1;
            this.double_17 = -1.0;
            this.StrategyID = strategyID;
            this.bars = bars;
            this.positionType = positionType_1;
            if (random_0 == null)
            {
                random_0 = new Random();
            }
            this.priority = random_0.NextDouble();
            this.entryBar = entryBar;
            this.entryPrice = entryPrice;
            this.exitBar = exitBar;
            this.exitPrice = exitPrice;
            this.basisPrice = basisPrice;
            this.active = exitBar < 0;
        }

        public double HighestHighAsOfBar(int int_7)
        {
            double minValue = double.MinValue;
            int entryBar = this.EntryBar;
            if (int_7 == this.int_4)
            {
                return this.double_16;
            }
            if ((this.int_4 >= 0) && (this.int_4 < int_7))
            {
                entryBar = this.int_4 + 1;
                minValue = this.double_16;
            }
            this.int_4 = int_7;
            for (int i = entryBar; i <= int_7; i++)
            {
                double num3 = this.Bars.High[i];
                if ((i == this.EntryBar) && (this.EntryOrderType == OrderType.AtClose))
                {
                    num3 = this.Bars.Close[i];
                }
                if ((i == this.ExitBar) && (this.ExitOrderType == OrderType.Market))
                {
                    num3 = this.Bars.Open[i];
                }
                if (num3 > minValue)
                {
                    minValue = num3;
                }
            }
            this.double_16 = minValue;
            return minValue;
        }

        public double LowestLowAsOfBar(int int_7)
        {
            double maxValue = double.MaxValue;
            int entryBar = this.EntryBar;
            if (int_7 == this.int_5)
            {
                return this.double_17;
            }
            if ((this.int_5 >= 0) && (this.int_5 < int_7))
            {
                entryBar = this.int_5 + 1;
                maxValue = this.double_17;
            }
            this.int_5 = int_7;
            for (int i = entryBar; i <= int_7; i++)
            {
                double num3 = this.Bars.Low[i];
                if ((i == this.EntryBar) && (this.EntryOrderType == OrderType.AtClose))
                {
                    num3 = this.Bars.Close[i];
                }
                if ((i == this.ExitBar) && (this.ExitOrderType == OrderType.Market))
                {
                    num3 = this.Bars.Open[i];
                }
                if (num3 < maxValue)
                {
                    maxValue = num3;
                }
            }
            this.double_17 = maxValue;
            return maxValue;
        }

        public double MAEAsOfBar(int int_7)
        {
            double maxValue = double.MaxValue;
            if (int_7 == this.int_3)
            {
                maxValue = this.double_15;
            }
            else
            {
                int entryBar = this.EntryBar;
                if ((this.int_3 >= 0) && (this.int_3 < int_7))
                {
                    maxValue = this.double_15;
                }
                this.int_3 = int_7;
                for (int i = this.EntryBar; i <= int_7; i++)
                {
                    double num4;
                    if (!this.Active && (i > this.ExitBar))
                    {
                        break;
                    }
                    if (i == this.EntryBar)
                    {
                        switch (this.EntryOrderType)
                        {
                            case OrderType.Stop:
                                if (this.PositionType != WealthLab.PositionType.Long)
                                {
                                    ///break; ///WYJ fix, simplify the flow
                                    //double num6 = Math.Max(this.Bars.Close[i], this.EntryPrice);
                                    num4 = this.EntryPrice - Math.Max(this.Bars.Close[i], this.EntryPrice);
                                }
                                else
                                    num4 = Math.Min(this.Bars.Close[i], this.EntryPrice) - this.EntryPrice;
                                ///goto  Label_02AC; ///WYJ fix, simplify the flow
                                break;

                            case OrderType.AtClose:
                                num4 = 0.0;
                                break;

                            default:
                                num4 = (this.PositionType == WealthLab.PositionType.Long) ? (this.Bars.Low[i] - this.EntryPrice) : (this.EntryPrice - this.Bars.High[i]);
                                break;
                        }
                        
                    }
                    else if (i == this.ExitBar)
                    {
                        switch (this.ExitOrderType)
                        {
                            case OrderType.Limit:
                                if (this.PositionType != WealthLab.PositionType.Long)
                                {
                                    ///break;  ///WYJ fix, simplify the flow
                                    //double num5 = Math.Max(this.Bars.Open[i], this.ExitPrice);
                                    num4 = this.EntryPrice - Math.Max(this.Bars.Open[i], this.ExitPrice);
                                }
                                else
                                    num4 = Math.Min(this.Bars.Open[i], this.ExitPrice) - this.EntryPrice;
                                break;

                            case OrderType.Stop:
                                num4 = (this.PositionType == WealthLab.PositionType.Long) ? (this.ExitPrice - this.EntryPrice) : (this.EntryPrice - this.ExitPrice);
                                break;

                            case OrderType.AtClose:
                                num4 = (this.PositionType == WealthLab.PositionType.Long) ? (this.Bars.Low[i] - this.EntryPrice) : (this.EntryPrice - this.Bars.High[i]);
                                break;

                            default:
                                num4 = (this.PositionType == WealthLab.PositionType.Long) ? (this.Bars.Open[i] - this.EntryPrice) : (this.EntryPrice - this.Bars.Open[i]);
                                break;
                        }
                    }
                    else
                    {
                        num4 = (this.PositionType == WealthLab.PositionType.Long) ? (this.Bars.Low[i] - this.EntryPrice) : (this.EntryPrice - this.Bars.High[i]);
                    }
                //Label_02AC:
                    if (maxValue > num4)
                    {
                        maxValue = num4;
                    }
                }
            }
            this.double_15 = maxValue;
            maxValue *= this.Shares;
            maxValue *= this.Bars.SymbolInfo.PointValue;
            return (maxValue - this.entryCommission);
        }

        public double MAEAsOfBarPercent(int int_7)
        {
            return ((this.MAEAsOfBar(int_7) * 100.0) / ((this.EntryPrice * this.Shares) * this.Bars.SymbolInfo.PointValue));
        }

        internal void method_0(WealthLab.Bars bars_1)
        {
            this.bars = bars_1;
        }

        internal void method_1(int int_7, double double_18, OrderType orderType_2)
        {
            if (double_18 < this.Bars.Low[int_7])
            {
                double_18 = this.Bars.Low[int_7];
            }
            if (double_18 > this.Bars.High[int_7])
            {
                double_18 = this.Bars.High[int_7];
            }
            this.exitBar = int_7;
            this.exitPrice = double_18;
            this.exitOrderType = orderType_2;
            this.active = false;
            this.mfe = this.MFEAsOfBar(int_7);
            this.mae = this.MAEAsOfBar(int_7);
        }

        internal void method_2()
        {
            if (this.Active)
            {
                this.mfe = this.MFEAsOfBar(this.Bars.Count - 1);
                this.mae = this.MAEAsOfBar(this.Bars.Count - 1);
            }
            else
            {
                this.mfe = this.MFEAsOfBar(this.ExitBar);
                this.mae = this.MAEAsOfBar(this.ExitBar);
            }
        }

        public double MFEAsOfBar(int int_7)
        {
            double minValue = double.MinValue;
            if (int_7 == this.int_2)
            {
                minValue = this.double_14;
            }
            else
            {
                int entryBar = this.EntryBar;
                if ((this.int_2 >= 0) && (this.int_2 < int_7))
                {
                    entryBar = this.int_2 + 1;
                    minValue = this.double_14;
                }
                this.int_2 = int_7;
                for (int i = entryBar; i <= int_7; i++)
                {
                    double num4;
                    if (!this.Active && (i > this.ExitBar))
                    {
                        break;
                    }
                    if (i == this.EntryBar)
                    {
                        switch (this.EntryOrderType)
                        {
                            case OrderType.Limit:
                                if (this.PositionType != WealthLab.PositionType.Long)
                                {
                                    ///break;  ///WYJ fix, simplify the flow
                                    num4 = this.EntryPrice - Math.Min(this.Bars.Close[i], this.EntryPrice);
                                }
                                else 
                                    num4 = Math.Max(this.Bars.Close[i], this.EntryPrice) - this.EntryPrice;
                                ///goto  Label_02B9;
                                break;

                            case OrderType.AtClose:
                                num4 = 0.0;
                                break;

                            default:
                                num4 = (this.PositionType == WealthLab.PositionType.Long) ? (this.Bars.High[i] - this.EntryPrice) : (this.EntryPrice - this.Bars.Low[i]);
                                break;
                        }
                    }
                    else if (i == this.ExitBar)
                    {
                        switch (this.ExitOrderType)
                        {
                            case OrderType.Limit:
                                num4 = (this.PositionType == WealthLab.PositionType.Long) ? (this.ExitPrice - this.EntryPrice) : (this.EntryPrice - this.ExitPrice);
                                break;

                            case OrderType.Stop:
                                if (this.PositionType != WealthLab.PositionType.Long)
                                {
                                    ///break;  ///WYJ fix, simplify the flow
                                    num4 = this.EntryPrice - Math.Min(this.Bars.Open[i], this.ExitPrice);
                                }
                                else 
                                    num4 = Math.Max(this.Bars.Open[i], this.ExitPrice) - this.EntryPrice;
                                break;

                            case OrderType.AtClose:
                                num4 = (this.PositionType == WealthLab.PositionType.Long) ? (this.Bars.High[i] - this.EntryPrice) : (this.EntryPrice - this.Bars.Low[i]);
                                break;

                            default:
                                num4 = (this.PositionType == WealthLab.PositionType.Long) ? (this.Bars.Open[i] - this.EntryPrice) : (this.EntryPrice - this.Bars.Open[i]);
                                break;
                        }
                        
                    }
                    else
                    {
                        num4 = (this.PositionType == WealthLab.PositionType.Long) ? (this.Bars.High[i] - this.EntryPrice) : (this.EntryPrice - this.Bars.Low[i]);
                    }
                //Label_02B9:
                    if (minValue < num4)
                    {
                        minValue = num4;
                    }
                }
            }
            this.double_14 = minValue;
            minValue *= this.Shares;
            minValue *= this.Bars.SymbolInfo.PointValue;
            return (minValue - this.entryCommission);
        }

        public double MFEAsOfBarPercent(int int_7)
        {
            return ((this.MFEAsOfBar(int_7) * 100.0) / ((this.EntryPrice * this.Shares) * this.Bars.SymbolInfo.PointValue));
        }

        public double NetProfitAsOfBar(int int_7)
        {
            if (!this.Active && (int_7 >= this.ExitBar))
            {
                return this.NetProfit;
            }
            double num = ((this.Bars.Close[int_7] - this.EntryPrice) * this.Shares) * this.Bars.SymbolInfo.PointValue;
            if (this.PositionType == WealthLab.PositionType.Short)
            {
                num = -num;
            }
            num -= this.entryCommission;
            if (!this.Active && (int_7 >= this.exitBar))
            {
                num -= this.exitCommission;
            }
            return num;
        }

        public double NetProfitAsOfBarPercent(int int_7)
        {
            return (((this.NetProfitAsOfBar(int_7) / this.Bars.SymbolInfo.PointValue) * 100.0) / (this.EntryPrice * this.Shares));
        }

        public bool Active
        {
            get
            {
                return this.active;
            }
        }

        public double AutoProfitLevel
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

        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars;
            }
        }

        public int BarsHeld
        {
            get
            {
                int num;
                if (this.Active)
                {
                    num = this.bars.Count - this.EntryBar;
                }
                else
                {
                    num = (this.ExitBar - this.EntryBar) + 1;
                }
                if (num < 1)
                {
                    num = 1;
                }
                return num;
            }
        }

        public double BasisPrice
        {
            get
            {
                return this.basisPrice;
            }
            internal set
            {
                this.basisPrice = value;
            }
        }

        internal int CombinedPriority
        {
            [CompilerGenerated]
            get
            {
                return this.combinedPriority;
            }
            [CompilerGenerated]
            set
            {
                this.combinedPriority = value;
            }
        }

        internal CombinedStrategyInfo CSI
        {
            [CompilerGenerated]
            get
            {
                return this.combinedStrategyInfo;
            }
            [CompilerGenerated]
            set
            {
                this.combinedStrategyInfo = value;
            }
        }

        public int EntryBar
        {
            get
            {
                return this.entryBar;
            }
            internal set
            {
                this.entryBar = value;
            }
        }

        public double EntryCommission
        {
            get
            {
                return this.entryCommission;
            }
            set
            {
                this.entryCommission = value;
            }
        }

        public DateTime EntryDate
        {
            get
            {
                return this.Bars.Date[this.EntryBar];
            }
        }

        public OrderType EntryOrderType
        {
            get
            {
                return this.entryOrderType;
            }
            internal set
            {
                this.entryOrderType = value;
            }
        }

        public double EntryPrice
        {
            get
            {
                return this.entryPrice;
            }
            internal set
            {
                this.entryPrice = value;
            }
        }

        public string EntrySignal
        {
            get
            {
                return this.entrySignal;
            }
            set
            {
                this.entrySignal = value;
            }
        }

        public int ExitBar
        {
            get
            {
                return this.exitBar;
            }
        }

        public double ExitCommission
        {
            get
            {
                return this.exitCommission;
            }
            set
            {
                this.exitCommission = value;
            }
        }

        public DateTime ExitDate
        {
            get
            {
                if (this.Active)
                {
                    return DateTime.MinValue;
                }
                return this.Bars.Date[this.ExitBar];
            }
        }

        public OrderType ExitOrderType
        {
            get
            {
                return this.exitOrderType;
            }
        }

        public double ExitPrice
        {
            get
            {
                return this.exitPrice;
            }
        }

        public string ExitSignal
        {
            get
            {
                return this.exitSignal;
            }
            set
            {
                this.exitSignal = value;
            }
        }

        internal int LastValidBar
        {
            get
            {
                if (!this.Active)
                {
                    return this.ExitBar;
                }
                return (this.Bars.Count - 1);
            }
        }

        public double MAE
        {
            get
            {
                return this.mae;
            }
        }

        public double MAEPercent
        {
            get
            {
                return ((this.MAE * 100.0) / ((this.EntryPrice * this.Shares) * this.Bars.SymbolInfo.PointValue));
            }
        }

        public double MFE
        {
            get
            {
                return this.mfe;
            }
        }

        public double MFEPercent
        {
            get
            {
                return ((this.MFE * 100.0) / ((this.EntryPrice * this.Shares) * this.Bars.SymbolInfo.PointValue));
            }
        }

        public double NetProfit
        {
            get
            {
                double num = ((this.RecentPrice - this.EntryPrice) * this.Shares) * this.Bars.SymbolInfo.PointValue;
                if (this.PositionType == WealthLab.PositionType.Short)
                {
                    num = -num;
                }
                num -= this.entryCommission;
                if (!this.Active)
                {
                    num -= this.exitCommission;
                }
                return num;
            }
        }

        public double NetProfitPercent
        {
            get
            {
                if (((this.EntryPrice * this.Shares) * this.Bars.SymbolInfo.PointValue) == 0.0)
                {
                    return 0.0;
                }
                return ((this.NetProfit * 100.0) / ((this.EntryPrice * this.Shares) * this.Bars.SymbolInfo.PointValue));
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
            }
        }

        public WealthLab.PositionType PositionType
        {
            get
            {
                return this.positionType;
            }
            internal set
            {
                this.positionType = value;
            }
        }

        public double Priority
        {
            get
            {
                return this.priority;
            }
            set
            {
                this.priority = value;
            }
        }

        public double ProfitPerBar
        {
            get
            {
                return (this.NetProfit / ((double) this.BarsHeld));
            }
        }

        internal double RecentPrice
        {
            get
            {
                if (this.Active)
                {
                    return this.bars.Close[this.bars.Count - 1];
                }
                return this.ExitPrice;
            }
        }

        public double RiskStopLevel
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

        public double Shares
        {
            get
            {
                return this.shares;
            }
            internal set
            {
                this.shares = value;
            }
        }

        public double Size
        {
            get
            {
                if (this.Bars.SymbolInfo.SecurityType == SecurityType.Future)
                {
                    return (this.Bars.SymbolInfo.Margin * this.Shares);
                }
                return (this.EntryPrice * this.Shares);
            }
        }

        internal double SplitFactor
        {
            get
            {
                return this.splitFactor;
            }
            set
            {
                this.splitFactor = value;
            }
        }

        public string StrategyID
        {
            [CompilerGenerated]
            get
            {
                return this.strategyID;
            }
            [CompilerGenerated]
            set
            {
                this.strategyID = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.Bars.Symbol;
            }
        }

        public object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }

        public double TrailingStop
        {
            get
            {
                return this.trailingStop;
            }
            internal set
            {
                this.trailingStop = value;
            }
        }
    }
}

