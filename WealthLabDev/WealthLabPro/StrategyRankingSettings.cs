namespace WealthLabPro
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using WealthLab;

    [XmlRoot(ElementName="StrategyRankingSettings", IsNullable=false)]
    public class StrategyRankingSettings
    {
        private BarDataRange barDataRange;
        private BarScale barScale;
        private DataSource dataSource_0;
        private int barInterval;
        private List<StrategyRankingItem> strategies;
        private PositionSize positionSize;
        private string scorecardName;
        private string symbol;
        private string dataSourceName;

        public StrategyRankingSettings()
        {
            this.scorecardName = "";
            this.symbol = "";
            this.strategies = new List<StrategyRankingItem>();
        }

        public StrategyRankingSettings(StrategyRankingSettings _srs)
        {
            this.scorecardName = "";
            this.symbol = "";
            this.strategies = new List<StrategyRankingItem>();
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
                this.barInterval = value.BarInterval;
                this.barScale = value.Scale;
            }
        }

        [XmlIgnore]
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

        public BarDataRange DataRange
        {
            get
            {
                return this.barDataRange;
            }
            set
            {
                string str = value.ToString();
                this.barDataRange = BarDataRange.Parse(str);
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
                return this.dataSourceName;
            }
            set
            {
                this.dataSourceName = value;
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
                string str = value.ToString();
                this.positionSize = PositionSize.Parse(str);
            }
        }

        [XmlIgnore]
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

        public string ScorecardName
        {
            get
            {
                return this.scorecardName;
            }
            set
            {
                this.scorecardName = value;
            }
        }

        [XmlIgnore]
        public List<StrategyRankingItem> Strategies
        {
            get
            {
                return this.strategies;
            }
            set
            {
                this.strategies = value;
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
    }
}

