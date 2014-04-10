namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="StrategyRule", IsNullable=false)]
    public class StrategyRule : Rule
    {
        private List<Rule> conditions;
        private List<StrategyRule> listEntriesAppliedTo;
        private List<StrategyRule> listExitsAppliedTo;
        private string exitName;

        public StrategyRule()
        {
            this.conditions = new List<Rule>();
            this.listEntriesAppliedTo = new List<StrategyRule>();
            this.listExitsAppliedTo = new List<StrategyRule>();
        }

        public StrategyRule(Rule baseRule) : base(baseRule)
        {
            this.conditions = new List<Rule>();
            this.listEntriesAppliedTo = new List<StrategyRule>();
            this.listExitsAppliedTo = new List<StrategyRule>();
        }

        public StrategyRule(StrategyRule baseSR) : base(baseSR)
        {
            this.conditions = new List<Rule>();
            this.listEntriesAppliedTo = new List<StrategyRule>();
            this.listExitsAppliedTo = new List<StrategyRule>();
            foreach (Rule rule in baseSR.Conditions)
            {
                this.Conditions.Add(new Rule(rule));
            }
        }

        public List<Rule> Conditions
        {
            get
            {
                return this.conditions;
            }
            set
            {
                this.conditions = value;
            }
        }

        [XmlIgnore]
        public List<StrategyRule> EntriesAppliedTo
        {
            get
            {
                return this.listEntriesAppliedTo;
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
                return this.exitName;
            }
            set
            {
                this.exitName = value;
            }
        }

        [XmlIgnore]
        public List<StrategyRule> ExitsAppliedTo
        {
            get
            {
                return this.listExitsAppliedTo;
            }
        }
    }
}

