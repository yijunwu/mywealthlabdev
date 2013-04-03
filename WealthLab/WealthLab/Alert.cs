namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="Alert", IsNullable=false)]
    public class Alert
    {
        private BarDataRange barDataRange_0;
        private WealthLab.Bars bars_0;
        private BarScale barScale_0;
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        [CompilerGenerated]
        private bool bool_3;
        private DataSource dataSource_0;
        private DateTime dateTime_0;
        private DateTime dateTime_1;
        private double double_0;
        private double double_1;
        private double double_2;
        private double double_3;
        private double double_4;
        private double double_5;
        private Guid guid_0;
        private int int_0;
        private object object_0;
        private WealthLab.OrderType orderType_0;
        private WealthLab.Position position_0;
        private PositionSize positionSize_0;
        private WealthLab.Strategy strategy_0;
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;
        private string string_4;
        private string string_5;
        private string string_6;
        private string string_7;
        [CompilerGenerated]
        private string string_8;
        private TradeType tradeType_0;

        public Alert()
        {
            this.double_5 = -1.0;
            this.string_4 = "";
            this.string_5 = "";
            this.string_6 = "";
        }

        public Alert(Alert alert)
        {
            this.double_5 = -1.0;
            this.string_4 = "";
            this.string_5 = "";
            this.string_6 = "";
            this.string_0 = alert.string_0;
            this.dateTime_0 = alert.dateTime_0;
            this.tradeType_0 = alert.tradeType_0;
            this.int_0 = alert.int_0;
            this.bars_0 = alert.bars_0;
            this.double_2 = alert.double_2;
            this.dataSource_0 = alert.dataSource_0;
            this.object_0 = alert.object_0;
            this.orderType_0 = alert.orderType_0;
            this.position_0 = alert.position_0;
            this.dateTime_1 = alert.dateTime_1;
            this.positionSize_0 = alert.positionSize_0;
            this.double_1 = alert.double_1;
            this.barDataRange_0 = alert.barDataRange_0;
            this.double_3 = alert.double_3;
            this.double_4 = alert.double_4;
            this.barScale_0 = alert.barScale_0;
            this.double_0 = alert.double_0;
            this.string_2 = alert.string_2;
            this.strategy_0 = alert.strategy_0;
            this.guid_0 = alert.guid_0;
            this.string_1 = alert.string_1;
            this.bool_0 = alert.bool_0;
            this.string_4 = alert.string_4;
            this.string_5 = alert.string_5;
            this.string_6 = alert.string_6;
            this.string_7 = alert.string_7;
            this.IsSameBarExit = alert.IsSameBarExit;
            this.SameBarParentOrderNumber = alert.SameBarParentOrderNumber;
        }

        public Alert(WealthLab.Strategy strategy_1, WealthLab.Bars bars_1, DateTime dateTime_2, TradeType alertType, WealthLab.OrderType ordType, double shares, string signalName)
        {
            this.double_5 = -1.0;
            this.string_4 = "";
            this.string_5 = "";
            this.string_6 = "";
            this.dateTime_0 = dateTime_2;
            this.tradeType_0 = alertType;
            this.orderType_0 = ordType;
            this.double_0 = shares;
            this.barScale_0 = bars_1.Scale;
            this.int_0 = bars_1.BarInterval;
            this.bars_0 = bars_1;
            this.string_2 = signalName;
            this.string_1 = bars_1.Symbol;
            this.strategy_0 = strategy_1;
            if (strategy_1 != null)
            {
                this.guid_0 = strategy_1.ID;
            }
        }

        public Alert(WealthLab.Strategy strategy_1, WealthLab.Bars bars_1, DateTime dateTime_2, TradeType alertType, WealthLab.OrderType ordType, double shares, string signalName, double basisPrice, double riskStopLevel, double autoProfitLevel)
        {
            this.double_5 = -1.0;
            this.string_4 = "";
            this.string_5 = "";
            this.string_6 = "";
            this.dateTime_0 = dateTime_2;
            this.tradeType_0 = alertType;
            this.orderType_0 = ordType;
            this.double_0 = shares;
            this.barScale_0 = bars_1.Scale;
            this.int_0 = bars_1.BarInterval;
            this.bars_0 = bars_1;
            this.double_2 = basisPrice;
            this.double_3 = riskStopLevel;
            this.double_4 = autoProfitLevel;
            this.string_2 = signalName;
            this.string_1 = bars_1.Symbol;
            this.strategy_0 = strategy_1;
            if (strategy_1 != null)
            {
                this.guid_0 = strategy_1.ID;
            }
        }

        public static bool AccountTradeTypeMatch(string _acctType1, string _acctType2)
        {
            if (!string.Equals(_acctType1, _acctType2))
            {
                return string.IsNullOrEmpty(_acctType1);
            }
            return true;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { this.AlertType, " ", this.Shares, " ", this.Symbol, " ", this.OrderType, " ", this.Price.ToString("N2") });
        }

        public string Account
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

        public string AccountTradeType
        {
            get
            {
                return this.string_7;
            }
            set
            {
                this.string_7 = value;
            }
        }

        public DateTime AlertDate
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

        public TradeType AlertType
        {
            get
            {
                return this.tradeType_0;
            }
            set
            {
                this.tradeType_0 = value;
            }
        }

        public double AutoProfitLevel
        {
            get
            {
                return this.double_4;
            }
            set
            {
                this.double_4 = value;
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

        [XmlIgnore]
        public WealthLab.Bars Bars
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

        public double BasisPrice
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

        [XmlIgnore]
        public object ChartDrawingObject
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

        public BarDataRange DataRange
        {
            get
            {
                return this.barDataRange_0;
            }
            set
            {
                this.barDataRange_0 = value;
            }
        }

        [XmlIgnore]
        public BarDataScale DataScale
        {
            get
            {
                return new BarDataScale(this.Scale, this.BarInterval);
            }
            set
            {
                this.Scale = value.Scale;
                this.BarInterval = value.BarInterval;
            }
        }

        [XmlIgnore]
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

        public bool EmailSent
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

        public string ExtendedOrderType
        {
            get
            {
                return this.string_6;
            }
            set
            {
                this.string_6 = value;
            }
        }

        [XmlIgnore]
        public double FirstTick
        {
            get
            {
                return this.double_5;
            }
            set
            {
                this.double_5 = value;
            }
        }

        public bool IsSameBarExit
        {
            [CompilerGenerated]
            get
            {
                return this.bool_3;
            }
            [CompilerGenerated]
            set
            {
                this.bool_3 = value;
            }
        }

        public WealthLab.OrderType OrderType
        {
            get
            {
                return this.orderType_0;
            }
            set
            {
                this.orderType_0 = value;
            }
        }

        [XmlIgnore]
        public WealthLab.Position Position
        {
            get
            {
                return this.position_0;
            }
            set
            {
                this.position_0 = value;
                if (value != null)
                {
                    this.dateTime_1 = value.EntryDate;
                }
            }
        }

        public DateTime PositionEntryDate
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

        public WealthLab.PositionType PositionType
        {
            get
            {
                if ((this.tradeType_0 != TradeType.Buy) && (this.tradeType_0 != TradeType.Sell))
                {
                    return WealthLab.PositionType.Short;
                }
                return WealthLab.PositionType.Long;
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

        public double Price
        {
            get
            {
                return this.double_1;
            }
            set
            {
                this.double_1 = value;
            }
        }

        [XmlIgnore]
        public bool ProcessedBySM
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

        public double RiskStopLevel
        {
            get
            {
                return this.double_3;
            }
            set
            {
                this.double_3 = value;
            }
        }

        public string Route
        {
            get
            {
                return this.string_4;
            }
            set
            {
                this.string_4 = value;
            }
        }

        [XmlIgnore]
        public string SameBarParentOrderNumber
        {
            [CompilerGenerated]
            get
            {
                return this.string_8;
            }
            [CompilerGenerated]
            set
            {
                this.string_8 = value;
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

        public string SecurityCode
        {
            get
            {
                if (this.Bars != null)
                {
                    if (this.Bars.SymbolInfo.SecurityType == SecurityType.MutualFund)
                    {
                        return "M";
                    }
                    if (this.Bars.SymbolInfo.SecurityType == SecurityType.Equity)
                    {
                        return "E";
                    }
                }
                return null;
            }
            set
            {
                this.string_3 = value;
            }
        }

        public double Shares
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

        public string SignalName
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        [XmlIgnore]
        public WealthLab.Strategy Strategy
        {
            get
            {
                return this.strategy_0;
            }
            set
            {
                this.strategy_0 = value;
                if (this.strategy_0 != null)
                {
                    this.guid_0 = this.strategy_0.ID;
                }
            }
        }

        public Guid StrategyID
        {
            get
            {
                return this.guid_0;
            }
            set
            {
                this.guid_0 = value;
            }
        }

        public string Symbol
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

        public string TIF
        {
            get
            {
                return this.string_5;
            }
            set
            {
                this.string_5 = value;
            }
        }

        public bool Triggered
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
    }
}

