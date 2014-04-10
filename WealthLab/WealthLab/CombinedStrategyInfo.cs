namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class CombinedStrategyInfo
    {
        private BarDataScale barDataScale = new BarDataScale(BarScale.Daily, 0);
        private bool useDefaultDataSet = true;
        [CompilerGenerated]
        private bool usePreferredValues;
        [CompilerGenerated]
        private Guid strategyID;
        private int priority = 1;
        [CompilerGenerated]
        private List<double> parameterValues;
        [CompilerGenerated]
        private object tag;
        private WealthLab.PositionSize positionSize_0 = new WealthLab.PositionSize(PosSizeMode.Dollar, 5000.0);
        private WealthLab.PositionSize positionSize_1 = new WealthLab.PositionSize(PosSizeMode.Dollar, 5000.0);
        private string accountNumber = "";
        private string dataSetName = "";
        private string symbol = "";
        [CompilerGenerated]
        private string name;

        public Strategy GetStrategy(StrategyManager strategyManager_0)
        {
            return strategyManager_0.LookupID(this.StrategyID.ToString());
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string AccountNumber
        {
            get
            {
                return this.accountNumber;
            }
            set
            {
                this.accountNumber = value;
            }
        }

        public WealthLab.PositionSize Allocation
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

        public BarDataScale DataScale
        {
            get
            {
                return this.barDataScale;
            }
            set
            {
                this.barDataScale = value;
            }
        }

        public string DataSetName
        {
            get
            {
                return this.dataSetName;
            }
            set
            {
                this.dataSetName = value;
            }
        }

        public string Name
        {
            [CompilerGenerated]
            get
            {
                return this.name;
            }
            [CompilerGenerated]
            set
            {
                this.name = value;
            }
        }

        public List<double> ParameterValues
        {
            [CompilerGenerated]
            get
            {
                return this.parameterValues;
            }
            [CompilerGenerated]
            set
            {
                this.parameterValues = value;
            }
        }

        public WealthLab.PositionSize PositionSize
        {
            get
            {
                return this.positionSize_1;
            }
            set
            {
                this.positionSize_1 = value;
            }
        }

        public int Priority
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

        public Guid StrategyID
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
                return this.symbol;
            }
            set
            {
                this.symbol = value;
            }
        }

        [XmlIgnore]
        public object Tag
        {
            [CompilerGenerated]
            get
            {
                return this.tag;
            }
            [CompilerGenerated]
            set
            {
                this.tag = value;
            }
        }

        public bool UseDefaultDataSet
        {
            get
            {
                return this.useDefaultDataSet;
            }
            set
            {
                this.useDefaultDataSet = value;
            }
        }

        public bool UsePreferredValues
        {
            [CompilerGenerated]
            get
            {
                return this.usePreferredValues;
            }
            [CompilerGenerated]
            set
            {
                this.usePreferredValues = value;
            }
        }
    }
}

