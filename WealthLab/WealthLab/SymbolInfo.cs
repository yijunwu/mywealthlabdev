namespace WealthLab
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="SymbolInfo", IsNullable=false)]
    public class SymbolInfo
    {
        private double double_0;
        private double double_1;
        private double tick;
        private int decimals;
        private WealthLab.SecurityType securityType;
        private string symbol;
        private string marketName;

        public SymbolInfo()
        {
        }

        public SymbolInfo(SymbolInfo symbolInfo_0)
        {
            this.symbol = symbolInfo_0.Symbol;
            this.securityType = symbolInfo_0.SecurityType;
            this.double_0 = symbolInfo_0.Margin;
            this.double_1 = symbolInfo_0.PointValue;
            this.tick = symbolInfo_0.Tick;
            this.decimals = symbolInfo_0.Decimals;
            this.marketName = symbolInfo_0.MarketName;
        }

        public SymbolInfo(string symbol, WealthLab.SecurityType securityType, double margin, double pointValue, double tick, int decimals)
        {
            this.symbol = symbol;
            this.securityType = securityType;
            this.double_0 = margin;
            this.double_1 = pointValue;
            this.tick = tick;
            this.decimals = decimals;
        }

        public int Decimals
        {
            get
            {
                return this.decimals;
            }
            set
            {
                this.decimals = value;
            }
        }

        public double Margin
        {
            get
            {
                return this.double_0;
            }
            set
            {
                if (value <= 0.0)
                {
                    this.double_0 = 1000.0;
                }
                else
                {
                    this.double_0 = value;
                }
            }
        }

        public string MarketName
        {
            get
            {
                return this.marketName;
            }
            set
            {
                this.marketName = value;
            }
        }

        public double PointValue
        {
            get
            {
                return this.double_1;
            }
            set
            {
                if (value <= 0.0)
                {
                    this.double_1 = 1.0;
                }
                else
                {
                    this.double_1 = value;
                }
            }
        }

        public WealthLab.SecurityType SecurityType
        {
            get
            {
                return this.securityType;
            }
            set
            {
                this.securityType = value;
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

        public double Tick
        {
            get
            {
                return this.tick;
            }
            set
            {
                this.tick = value;
            }
        }
    }
}

