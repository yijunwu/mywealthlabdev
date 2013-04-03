namespace WealthLab
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="SymbolInfo", IsNullable=false)]
    public class SymbolInfo
    {
        private double double_0;
        private double double_1;
        private double double_2;
        private int int_0;
        private WealthLab.SecurityType securityType_0;
        private string string_0;
        private string string_1;

        public SymbolInfo()
        {
        }

        public SymbolInfo(SymbolInfo symbolInfo_0)
        {
            this.string_0 = symbolInfo_0.Symbol;
            this.securityType_0 = symbolInfo_0.SecurityType;
            this.double_0 = symbolInfo_0.Margin;
            this.double_1 = symbolInfo_0.PointValue;
            this.double_2 = symbolInfo_0.Tick;
            this.int_0 = symbolInfo_0.Decimals;
            this.string_1 = symbolInfo_0.MarketName;
        }

        public SymbolInfo(string symbol, WealthLab.SecurityType securityType, double margin, double pointValue, double tick, int decimals)
        {
            this.string_0 = symbol;
            this.securityType_0 = securityType;
            this.double_0 = margin;
            this.double_1 = pointValue;
            this.double_2 = tick;
            this.int_0 = decimals;
        }

        public int Decimals
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
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
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
                return this.securityType_0;
            }
            set
            {
                this.securityType_0 = value;
            }
        }

        public string Symbol
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

        public double Tick
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
    }
}

