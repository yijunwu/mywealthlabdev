namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class CombinedStrategyInfo
    {
        private BarDataScale barDataScale_0 = new BarDataScale(BarScale.Daily, 0);
        private bool bool_0 = true;
        [CompilerGenerated]
        private bool bool_1;
        [CompilerGenerated]
        private Guid guid_0;
        private int int_0 = 1;
        [CompilerGenerated]
        private List<double> list_0;
        [CompilerGenerated]
        private object object_0;
        private WealthLab.PositionSize positionSize_0 = new WealthLab.PositionSize(PosSizeMode.Dollar, 5000.0);
        private WealthLab.PositionSize positionSize_1 = new WealthLab.PositionSize(PosSizeMode.Dollar, 5000.0);
        private string string_0 = "";
        private string string_1 = "";
        private string string_2 = "";
        [CompilerGenerated]
        private string string_3;

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
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
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
                return this.barDataScale_0;
            }
            set
            {
                this.barDataScale_0 = value;
            }
        }

        public string DataSetName
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

        public string Name
        {
            [CompilerGenerated]
            get
            {
                return this.string_3;
            }
            [CompilerGenerated]
            set
            {
                this.string_3 = value;
            }
        }

        public List<double> ParameterValues
        {
            [CompilerGenerated]
            get
            {
                return this.list_0;
            }
            [CompilerGenerated]
            set
            {
                this.list_0 = value;
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
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }

        public Guid StrategyID
        {
            [CompilerGenerated]
            get
            {
                return this.guid_0;
            }
            [CompilerGenerated]
            set
            {
                this.guid_0 = value;
            }
        }

        public string Symbol
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
        public object Tag
        {
            [CompilerGenerated]
            get
            {
                return this.object_0;
            }
            [CompilerGenerated]
            set
            {
                this.object_0 = value;
            }
        }

        public bool UseDefaultDataSet
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

        public bool UsePreferredValues
        {
            [CompilerGenerated]
            get
            {
                return this.bool_1;
            }
            [CompilerGenerated]
            set
            {
                this.bool_1 = value;
            }
        }
    }
}

