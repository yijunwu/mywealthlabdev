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
        private bool bool_0;
        [CompilerGenerated]
        private bool bool_1;
        private List<double> list_0;
        private ListViewItem listViewItem_0;
        private WealthLab.Strategy strategy_0;
        private string string_0;
        private string string_1;
        private WealthLab.WealthScript wealthScript_0;

        public StrategyRankingItem()
        {
            this.list_0 = new List<double>();
        }

        public StrategyRankingItem(StrategyRankingItem _sri)
        {
            this.list_0 = new List<double>();
            this.strategy_0 = _sri.Strategy;
            this.wealthScript_0 = _sri.WealthScript;
            this.listViewItem_0 = _sri.LvItem;
            this.string_0 = _sri.StrategyID;
            this.string_1 = _sri.StrategyName;
            foreach (double num in _sri.ParameterValues)
            {
                this.list_0.Add(num);
            }
        }

        public StrategyRankingItem(WealthLab.Strategy strategy_1, WealthLab.WealthScript wealthScript_1)
        {
            this.list_0 = new List<double>();
            this.strategy_0 = strategy_1;
            this.wealthScript_0 = wealthScript_1;
        }

        [XmlIgnore]
        public ListViewItem LvItem
        {
            get
            {
                return this.listViewItem_0;
            }
            set
            {
                this.listViewItem_0 = value;
            }
        }

        public bool ParametersNeedSave
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

        public List<double> ParameterValues
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
            }
        }

        public string StrategyID
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

        public string StrategyName
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

        [XmlIgnore]
        public WealthLab.WealthScript WealthScript
        {
            get
            {
                return this.wealthScript_0;
            }
            set
            {
                this.wealthScript_0 = value;
            }
        }
    }
}

