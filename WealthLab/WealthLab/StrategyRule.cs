namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="StrategyRule", IsNullable=false)]
    public class StrategyRule : Rule
    {
        private List<Rule> list_2;
        private List<StrategyRule> list_3;
        private List<StrategyRule> list_4;
        private string string_9;

        public StrategyRule()
        {
            this.list_2 = new List<Rule>();
            this.list_3 = new List<StrategyRule>();
            this.list_4 = new List<StrategyRule>();
        }

        public StrategyRule(Rule baseRule) : base(baseRule)
        {
            this.list_2 = new List<Rule>();
            this.list_3 = new List<StrategyRule>();
            this.list_4 = new List<StrategyRule>();
        }

        public StrategyRule(StrategyRule baseSR) : base(baseSR)
        {
            this.list_2 = new List<Rule>();
            this.list_3 = new List<StrategyRule>();
            this.list_4 = new List<StrategyRule>();
            foreach (Rule rule in baseSR.Conditions)
            {
                this.Conditions.Add(new Rule(rule));
            }
        }

        public List<Rule> Conditions
        {
            get
            {
                return this.list_2;
            }
            set
            {
                this.list_2 = value;
            }
        }

        [XmlIgnore]
        public List<StrategyRule> EntriesAppliedTo
        {
            get
            {
                return this.list_3;
            }
        }

        [XmlIgnore]
        public int EntryExitSortCode
        {
            get
            {
                if (base.Body.Contains("AtLimit"))
                {
                    return 4;
                }
                if (base.Body.Contains("AtStop"))
                {
                    return 3;
                }
                if (base.Body.Contains("AtMarket"))
                {
                    return 2;
                }
                return 1;
            }
        }

        [XmlIgnore]
        public string ExitName
        {
            get
            {
                return this.string_9;
            }
            set
            {
                this.string_9 = value;
            }
        }

        [XmlIgnore]
        public List<StrategyRule> ExitsAppliedTo
        {
            get
            {
                return this.list_4;
            }
        }
    }
}

