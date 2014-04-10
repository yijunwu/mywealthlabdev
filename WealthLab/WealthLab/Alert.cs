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
        private BarScale barScale;
        private bool triggered;
        private bool processedBySM;
        private bool emailSent;
        [CompilerGenerated]
        private bool isSameBarExit;
        private DataSource dataSource_0;
        private DateTime alertDate;
        private DateTime positionEntryDate;
        private double shares;
        private double price;
        private double basisPrice;
        private double riskStopLevel;
        private double autoProfitLevel;
        private double firstTick;
        private Guid strategyID;
        private int barInterval;
        private object chartDrawingObject;
        private WealthLab.OrderType orderType_0;
        private WealthLab.Position position_0;
        private PositionSize positionSize;
        private WealthLab.Strategy strategy;
        private string account;
        private string symbol;
        private string signalName;
        private string string_3;
        private string route;
        private string tif;
        private string extendedOrderType;
        private string accountTradeType;
        [CompilerGenerated]
        private string sameBarParentOrderNumber;
        private TradeType alertType;

        public Alert()
        {
            this.firstTick = -1.0;
            this.route = "";
            this.tif = "";
            this.extendedOrderType = "";
        }

        public Alert(Alert alert)
        {
            this.firstTick = -1.0;
            this.route = "";
            this.tif = "";
            this.extendedOrderType = "";
            this.account = alert.account;
            this.alertDate = alert.alertDate;
            this.alertType = alert.alertType;
            this.barInterval = alert.barInterval;
            this.bars_0 = alert.bars_0;
            this.basisPrice = alert.basisPrice;
            this.dataSource_0 = alert.dataSource_0;
            this.chartDrawingObject = alert.chartDrawingObject;
            this.orderType_0 = alert.orderType_0;
            this.position_0 = alert.position_0;
            this.positionEntryDate = alert.positionEntryDate;
            this.positionSize = alert.positionSize;
            this.price = alert.price;
            this.barDataRange_0 = alert.barDataRange_0;
            this.riskStopLevel = alert.riskStopLevel;
            this.autoProfitLevel = alert.autoProfitLevel;
            this.barScale = alert.barScale;
            this.shares = alert.shares;
            this.signalName = alert.signalName;
            this.strategy = alert.strategy;
            this.strategyID = alert.strategyID;
            this.symbol = alert.symbol;
            this.triggered = alert.triggered;
            this.route = alert.route;
            this.tif = alert.tif;
            this.extendedOrderType = alert.extendedOrderType;
            this.accountTradeType = alert.accountTradeType;
            this.IsSameBarExit = alert.IsSameBarExit;
            this.SameBarParentOrderNumber = alert.SameBarParentOrderNumber;
        }

        public Alert(WealthLab.Strategy strategy_1, WealthLab.Bars bars_1, DateTime dateTime_2, TradeType alertType, WealthLab.OrderType ordType, double shares, string signalName)
        {
            this.firstTick = -1.0;
            this.route = "";
            this.tif = "";
            this.extendedOrderType = "";
            this.alertDate = dateTime_2;
            this.alertType = alertType;
            this.orderType_0 = ordType;
            this.shares = shares;
            this.barScale = bars_1.Scale;
            this.barInterval = bars_1.BarInterval;
            this.bars_0 = bars_1;
            this.signalName = signalName;
            this.symbol = bars_1.Symbol;
            this.strategy = strategy_1;
            if (strategy_1 != null)
            {
                this.strategyID = strategy_1.ID;
            }
        }

        public Alert(WealthLab.Strategy strategy_1, WealthLab.Bars bars_1, DateTime dateTime_2, TradeType alertType, WealthLab.OrderType ordType, double shares, string signalName, double basisPrice, double riskStopLevel, double autoProfitLevel)
        {
            this.firstTick = -1.0;
            this.route = "";
            this.tif = "";
            this.extendedOrderType = "";
            this.alertDate = dateTime_2;
            this.alertType = alertType;
            this.orderType_0 = ordType;
            this.shares = shares;
            this.barScale = bars_1.Scale;
            this.barInterval = bars_1.BarInterval;
            this.bars_0 = bars_1;
            this.basisPrice = basisPrice;
            this.riskStopLevel = riskStopLevel;
            this.autoProfitLevel = autoProfitLevel;
            this.signalName = signalName;
            this.symbol = bars_1.Symbol;
            this.strategy = strategy_1;
            if (strategy_1 != null)
            {
                this.strategyID = strategy_1.ID;
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
                return this.account;
            }
            set
            {
                this.account = value;
            }
        }

        public string AccountTradeType
        {
            get
            {
                return this.accountTradeType;
            }
            set
            {
                this.accountTradeType = value;
            }
        }

        public DateTime AlertDate
        {
            get
            {
                return this.alertDate;
            }
            set
            {
                this.alertDate = value;
            }
        }

        public TradeType AlertType
        {
            get
            {
                return this.alertType;
            }
            set
            {
                this.alertType = value;
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
                return this.basisPrice;
            }
            set
            {
                this.basisPrice = value;
            }
        }

        [XmlIgnore]
        public object ChartDrawingObject
        {
            get
            {
                return this.chartDrawingObject;
            }
            set
            {
                this.chartDrawingObject = value;
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
                return this.emailSent;
            }
            set
            {
                this.emailSent = value;
            }
        }

        public string ExtendedOrderType
        {
            get
            {
                return this.extendedOrderType;
            }
            set
            {
                this.extendedOrderType = value;
            }
        }

        [XmlIgnore]
        public double FirstTick
        {
            get
            {
                return this.firstTick;
            }
            set
            {
                this.firstTick = value;
            }
        }

        public bool IsSameBarExit
        {
            [CompilerGenerated]
            get
            {
                return this.isSameBarExit;
            }
            [CompilerGenerated]
            set
            {
                this.isSameBarExit = value;
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
                    this.positionEntryDate = value.EntryDate;
                }
            }
        }

        public DateTime PositionEntryDate
        {
            get
            {
                return this.positionEntryDate;
            }
            set
            {
                this.positionEntryDate = value;
            }
        }

        public WealthLab.PositionType PositionType
        {
            get
            {
                if ((this.alertType != TradeType.Buy) && (this.alertType != TradeType.Sell))
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
                return this.positionSize;
            }
            set
            {
                this.positionSize = value;
            }
        }

        public double Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }

        [XmlIgnore]
        public bool ProcessedBySM
        {
            get
            {
                return this.processedBySM;
            }
            set
            {
                this.processedBySM = value;
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

        public string Route
        {
            get
            {
                return this.route;
            }
            set
            {
                this.route = value;
            }
        }

        [XmlIgnore]
        public string SameBarParentOrderNumber
        {
            [CompilerGenerated]
            get
            {
                return this.sameBarParentOrderNumber;
            }
            [CompilerGenerated]
            set
            {
                this.sameBarParentOrderNumber = value;
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
                return this.shares;
            }
            set
            {
                this.shares = value;
            }
        }

        public string SignalName
        {
            get
            {
                return this.signalName;
            }
            set
            {
                this.signalName = value;
            }
        }

        [XmlIgnore]
        public WealthLab.Strategy Strategy
        {
            get
            {
                return this.strategy;
            }
            set
            {
                this.strategy = value;
                if (this.strategy != null)
                {
                    this.strategyID = this.strategy.ID;
                }
            }
        }

        public Guid StrategyID
        {
            get
            {
                return this.strategyID;
            }
            set
            {
                this.strategyID = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
            set
            {
                this.symbol = value;
            }
        }

        public string TIF
        {
            get
            {
                return this.tif;
            }
            set
            {
                this.tif = value;
            }
        }

        public bool Triggered
        {
            get
            {
                return this.triggered;
            }
            set
            {
                this.triggered = value;
            }
        }
    }
}

