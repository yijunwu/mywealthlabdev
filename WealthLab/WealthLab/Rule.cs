namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="Rule", IsNullable=false)]
    public class Rule
    {
        private bool bool_0;
        private Guid guid_0;
        private List<RuleParameter> list_0;
        private List<RuleVariable> list_1;
        private WealthLab.RuleType ruleType_0;
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;
        private string string_4;
        private string string_5;
        private string string_6;
        private string string_7;
        private string string_8;

        public Rule()
        {
            this.string_0 = "";
            this.string_1 = "";
            this.string_2 = "";
            this.string_3 = "";
            this.string_4 = "";
            this.string_5 = "";
            this.guid_0 = Guid.NewGuid();
            this.list_0 = new List<RuleParameter>();
            this.list_1 = new List<RuleVariable>();
        }

        public Rule(Rule baseRule)
        {
            this.string_0 = "";
            this.string_1 = "";
            this.string_2 = "";
            this.string_3 = "";
            this.string_4 = "";
            this.string_5 = "";
            this.guid_0 = Guid.NewGuid();
            this.list_0 = new List<RuleParameter>();
            this.list_1 = new List<RuleVariable>();
            this.string_4 = baseRule.string_4;
            this.string_2 = baseRule.string_2;
            this.string_1 = baseRule.string_1;
            this.guid_0 = baseRule.guid_0;
            this.string_3 = baseRule.string_3;
            this.string_0 = baseRule.string_0;
            this.string_5 = baseRule.string_5;
            this.ruleType_0 = baseRule.ruleType_0;
            foreach (RuleParameter parameter in baseRule.Parameters)
            {
                this.Parameters.Add(new RuleParameter(parameter));
            }
            foreach (RuleVariable variable in baseRule.Variables)
            {
                this.Variables.Add(new RuleVariable(variable));
            }
            this.string_8 = baseRule.string_8;
        }

        public string Body
        {
            get
            {
                return this.string_4;
            }
            set
            {
                this.string_4 = value;
            }
        }

        public string Category
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

        public string Description
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

        public Guid ID
        {
            get
            {
                return this.guid_0;
            }
            set
            {
                this.guid_0 = value;
            }
        }

        public string Init
        {
            get
            {
                return this.string_3;
            }
            set
            {
                this.string_3 = value;
            }
        }

        [XmlIgnore]
        public bool IsEntry
        {
            get
            {
                if (this.RuleType != WealthLab.RuleType.LongEntry)
                {
                    return (this.RuleType == WealthLab.RuleType.ShortEntry);
                }
                return true;
            }
        }

        [XmlIgnore]
        public bool IsEntryExit
        {
            get
            {
                if (((this.RuleType != WealthLab.RuleType.LongEntry) && (this.RuleType != WealthLab.RuleType.LongExit)) && (this.RuleType != WealthLab.RuleType.ShortEntry))
                {
                    return (this.RuleType == WealthLab.RuleType.ShortExit);
                }
                return true;
            }
        }

        [XmlIgnore]
        public bool IsExit
        {
            get
            {
                if (this.RuleType != WealthLab.RuleType.LongExit)
                {
                    return (this.RuleType == WealthLab.RuleType.ShortExit);
                }
                return true;
            }
        }

        public string Name
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

        public List<RuleParameter> Parameters
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

        public string Plotting
        {
            get
            {
                return this.string_5;
            }
            set
            {
                this.string_5 = value;
            }
        }

        public WealthLab.RuleType RuleType
        {
            get
            {
                return this.ruleType_0;
            }
            set
            {
                this.ruleType_0 = value;
            }
        }

        [XmlIgnore]
        public string TempBody
        {
            get
            {
                return this.string_7;
            }
            set
            {
                this.string_7 = value;
            }
        }

        [XmlIgnore]
        public string TempInit
        {
            get
            {
                return this.string_6;
            }
            set
            {
                this.string_6 = value;
            }
        }

        public string UsingClause
        {
            get
            {
                return this.string_8;
            }
            set
            {
                this.string_8 = value;
            }
        }

        [XmlIgnore]
        public bool ValidOr
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

        public List<RuleVariable> Variables
        {
            get
            {
                return this.list_1;
            }
            set
            {
                this.list_1 = value;
            }
        }
    }
}

