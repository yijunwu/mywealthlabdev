namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class Position
    {
        public static readonly Position AllPositions = new Position(null, WealthLab.PositionType.Long, "");
        private WealthLab.Bars bars_0;
        private bool bool_0;
        [CompilerGenerated]
        private CombinedStrategyInfo combinedStrategyInfo_0;
        private double double_0;
        private double double_1;
        private double double_10;
        private double double_11;
        private double double_12;
        private double double_13;
        private double double_14;
        private double double_15;
        private double double_16;
        private double double_17;
        private double double_2;
        private double double_3;
        private double double_4;
        private double double_5;
        private double double_6;
        private double double_7;
        private double double_8;
        private double double_9;
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        [CompilerGenerated]
        private int int_6;
        private object object_0;
        private OrderType orderType_0;
        private OrderType orderType_1;
        private WealthLab.PositionType positionType_0;
        private static Random random_0;
        private string string_0;
        private string string_1;
        [CompilerGenerated]
        private string string_2;

        public Position(WealthLab.Bars bars, WealthLab.PositionType positionType_1, string strategyID)
        {
            this.bool_0 = true;
            this.int_1 = -1;
            this.double_5 = double.NaN;
            this.double_6 = double.NaN;
            this.double_10 = 1.0;
            this.int_2 = -1;
            this.double_14 = -1.0;
            this.int_3 = -1;
            this.double_15 = -1.0;
            this.int_4 = -1;
            this.double_16 = -1.0;
            this.int_5 = -1;
            this.double_17 = -1.0;
            this.StrategyID = strategyID;
            this.bars_0 = bars;
            this.positionType_0 = positionType_1;
            if (random_0 == null)
            {
                random_0 = new Random();
            }
            this.double_11 = random_0.NextDouble();
        }

        public Position(WealthLab.Bars bars, WealthLab.PositionType positionType_1, double basisPrice, int entryBar, double entryPrice, int exitBar, double exitPrice)
        {
            this.bool_0 = true;
            this.int_1 = -1;
            this.double_5 = double.NaN;
            this.double_6 = double.NaN;
            this.double_10 = 1.0;
            this.int_2 = -1;
            this.double_14 = -1.0;
            this.int_3 = -1;
            this.double_15 = -1.0;
            this.int_4 = -1;
            this.double_16 = -1.0;
            this.int_5 = -1;
            this.double_17 = -1.0;
            this.bars_0 = bars;
            this.positionType_0 = positionType_1;
            if (random_0 == null)
            {
                random_0 = new Random();
            }
            this.double_11 = random_0.NextDouble();
            this.int_0 = entryBar;
            this.double_2 = entryPrice;
            this.int_1 = exitBar;
            this.double_3 = exitPrice;
            this.double_4 = basisPrice;
            this.bool_0 = exitBar < 0;
        }

        public Position(WealthLab.Bars bars, WealthLab.PositionType positionType_1, double basisPrice, int entryBar, double entryPrice, int exitBar, double exitPrice, string strategyID)
        {
            this.bool_0 = true;
            this.int_1 = -1;
            this.double_5 = double.NaN;
            this.double_6 = double.NaN;
            this.double_10 = 1.0;
            this.int_2 = -1;
            this.double_14 = -1.0;
            this.int_3 = -1;
            this.double_15 = -1.0;
            this.int_4 = -1;
            this.double_16 = -1.0;
            this.int_5 = -1;
            this.double_17 = -1.0;
            this.StrategyID = strategyID;
            this.bars_0 = bars;
            this.positionType_0 = positionType_1;
            if (random_0 == null)
            {
                random_0 = new Random();
            }
            this.double_11 = random_0.NextDouble();
            this.int_0 = entryBar;
            this.double_2 = entryPrice;
            this.int_1 = exitBar;
            this.double_3 = exitPrice;
            this.double_4 = basisPrice;
            this.bool_0 = exitBar < 0;
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
            return (maxValue - this.double_7);
        }

        public double MAEAsOfBarPercent(int int_7)
        {
            return ((this.MAEAsOfBar(int_7) * 100.0) / ((this.EntryPrice * this.Shares) * this.Bars.SymbolInfo.PointValue));
        }

        internal void method_0(WealthLab.Bars bars_1)
        {
            this.bars_0 = bars_1;
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
            this.int_1 = int_7;
            this.double_3 = double_18;
            this.orderType_1 = orderType_2;
            this.bool_0 = false;
            this.double_5 = this.MFEAsOfBar(int_7);
            this.double_6 = this.MAEAsOfBar(int_7);
        }

        internal void method_2()
        {
            if (this.Active)
            {
                this.double_5 = this.MFEAsOfBar(this.Bars.Count - 1);
                this.double_6 = this.MAEAsOfBar(this.Bars.Count - 1);
            }
            else
            {
                this.double_5 = this.MFEAsOfBar(this.ExitBar);
                this.double_6 = this.MAEAsOfBar(this.ExitBar);
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
            return (minValue - this.double_7);
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
            num -= this.double_7;
            if (!this.Active && (int_7 >= this.int_1))
            {
                num -= this.double_8;
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
                return this.bool_0;
            }
        }

        public double AutoProfitLevel
        {
            get
            {
                return this.double_13;
            }
            set
            {
                this.double_13 = value;
            }
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars_0;
            }
        }

        public int BarsHeld
        {
            get
            {
                int num;
                if (this.Active)
                {
                    num = this.bars_0.Count - this.EntryBar;
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
                return this.double_4;
            }
            internal set
            {
                this.double_4 = value;
            }
        }

        internal int CombinedPriority
        {
            [CompilerGenerated]
            get
            {
                return this.int_6;
            }
            [CompilerGenerated]
            set
            {
                this.int_6 = value;
            }
        }

        internal CombinedStrategyInfo CSI
        {
            [CompilerGenerated]
            get
            {
                return this.combinedStrategyInfo_0;
            }
            [CompilerGenerated]
            set
            {
                this.combinedStrategyInfo_0 = value;
            }
        }

        public int EntryBar
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

        public double EntryCommission
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
                return this.orderType_0;
            }
            internal set
            {
                this.orderType_0 = value;
            }
        }

        public double EntryPrice
        {
            get
            {
                return this.double_2;
            }
            internal set
            {
                this.double_2 = value;
            }
        }

        public string EntrySignal
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

        public int ExitBar
        {
            get
            {
                return this.int_1;
            }
        }

        public double ExitCommission
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
                return this.orderType_1;
            }
        }

        public double ExitPrice
        {
            get
            {
                return this.double_3;
            }
        }

        public string ExitSignal
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
                return this.double_6;
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
                return this.double_5;
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
                num -= this.double_7;
                if (!this.Active)
                {
                    num -= this.double_8;
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
                return this.double_0;
            }
            set
            {
                this.double_0 = value;
            }
        }

        public WealthLab.PositionType PositionType
        {
            get
            {
                return this.positionType_0;
            }
            internal set
            {
                this.positionType_0 = value;
            }
        }

        public double Priority
        {
            get
            {
                return this.double_11;
            }
            set
            {
                this.double_11 = value;
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
                    return this.bars_0.Close[this.bars_0.Count - 1];
                }
                return this.ExitPrice;
            }
        }

        public double RiskStopLevel
        {
            get
            {
                return this.double_12;
            }
            set
            {
                this.double_12 = value;
            }
        }

        public double Shares
        {
            get
            {
                return this.double_1;
            }
            internal set
            {
                this.double_1 = value;
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
                return this.double_10;
            }
            set
            {
                this.double_10 = value;
            }
        }

        public string StrategyID
        {
            [CompilerGenerated]
            get
            {
                return this.string_2;
            }
            [CompilerGenerated]
            set
            {
                this.string_2 = value;
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
                return this.object_0;
            }
            set
            {
                this.object_0 = value;
            }
        }

        public double TrailingStop
        {
            get
            {
                return this.double_9;
            }
            internal set
            {
                this.double_9 = value;
            }
        }
    }
}

