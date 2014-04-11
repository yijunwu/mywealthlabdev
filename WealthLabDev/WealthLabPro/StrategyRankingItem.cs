namespace WealthLabPro
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using WealthLab;

    public class StrategyRankingItem
    {
        private bool parametersNeedSave;
        [CompilerGenerated]
        private bool usePreferredValues;
        private List<double> parameterValues;
        private ListViewItem listViewItem;
        private WealthLab.Strategy strategy;
        private string strategyID;
        private string strategyName;
        private WealthLab.WealthScript wealthScript;

        public StrategyRankingItem()
        {
            this.parameterValues = new List<double>();
        }

        public StrategyRankingItem(StrategyRankingItem _sri)
        {
            this.parameterValues = new List<double>();
            this.strategy = _sri.Strategy;
            this.wealthScript = _sri.WealthScript;
            this.listViewItem = _sri.LvItem;
            this.strategyID = _sri.StrategyID;
            this.strategyName = _sri.StrategyName;
            foreach (double num in _sri.ParameterValues)
            {
                this.parameterValues.Add(num);
            }
        }

        public StrategyRankingItem(WealthLab.Strategy strategy_1, WealthLab.WealthScript wealthScript_1)
        {
            this.parameterValues = new List<double>();
            this.strategy = strategy_1;
            this.wealthScript = wealthScript_1;
        }

        [XmlIgnore]
        public ListViewItem LvItem
        {
            get
            {
                return this.listViewItem;
            }
            set
            {
                this.listViewItem = value;
            }
        }

        public bool ParametersNeedSave
        {
            get
            {
                return this.parametersNeedSave;
            }
            set
            {
                this.parametersNeedSave = value;
            }
        }

        public List<double> ParameterValues
        {
            get
            {
                return this.parameterValues;
            }
            set
            {
                this.parameterValues = value;
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
            }
        }

        public string StrategyID
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

        public string StrategyName
        {
            get
            {
                return this.strategyName;
            }
            set
            {
                this.strategyName = value;
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

        [XmlIgnore]
        public WealthLab.WealthScript WealthScript
        {
            get
            {
                return this.wealthScript;
            }
            set
            {
                this.wealthScript = value;
            }
        }
    }
}

