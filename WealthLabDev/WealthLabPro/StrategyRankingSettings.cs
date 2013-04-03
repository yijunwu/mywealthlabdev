namespace WealthLabPro
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using WealthLab;

    [XmlRoot(ElementName="StrategyRankingSettings", IsNullable=false)]
    public class StrategyRankingSettings
    {
        private BarDataRange barDataRange_0;
        private BarScale barScale_0;
        private DataSource dataSource_0;
        private int int_0;
        private List<StrategyRankingItem> list_0;
        private PositionSize positionSize_0;
        private string string_0;
        private string string_1;
        private string string_2;

        public StrategyRankingSettings()
        {
            this.string_0 = "";
            this.string_1 = "";
            this.list_0 = new List<StrategyRankingItem>();
        }

        public StrategyRankingSettings(StrategyRankingSettings _srs)
        {
            this.string_0 = "";
            this.string_1 = "";
            this.list_0 = new List<StrategyRankingItem>();
            if (_srs != null)
            {
                this.BarDataScale = _srs.BarDataScale;
                this.DataRange = _srs.DataRange;
                this.DataSet = _srs.DataSet;
                this.DataSourceName = _srs.DataSourceName;
                this.PosSize = _srs.PosSize;
                this.ScorecardName = _srs.ScorecardName;
                this.Symbol = _srs.Symbol;
                this.Strategies.Clear();
                List<StrategyRankingItem> list = new List<StrategyRankingItem>();
                foreach (StrategyRankingItem item in _srs.Strategies)
                {
                    StrategyRankingItem item2 = new StrategyRankingItem(item);
                    list.Add(item2);
                }
                this.Strategies = list;
            }
        }

        public WealthLab.BarDataScale BarDataScale
        {
            get
            {
                return new WealthLab.BarDataScale(this.Scale, this.BarInterval);
            }
            set
            {
                this.int_0 = value.BarInterval;
                this.barScale_0 = value.Scale;
            }
        }

        [XmlIgnore]
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

        public BarDataRange DataRange
        {
            get
            {
                return this.barDataRange_0;
            }
            set
            {
                string str = value.ToString();
                this.barDataRange_0 = BarDataRange.Parse(str);
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

        public string DataSourceName
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

        public PositionSize PosSize
        {
            get
            {
                return this.positionSize_0;
            }
            set
            {
                string str = value.ToString();
                this.positionSize_0 = PositionSize.Parse(str);
            }
        }

        [XmlIgnore]
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

        public string ScorecardName
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

        [XmlIgnore]
        public List<StrategyRankingItem> Strategies
        {
            get
            {
                return this.list_0;
            }
            set
            {
                this.list_0 = value;
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
    }
}

