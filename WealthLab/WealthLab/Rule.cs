namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="Rule", IsNullable=false)]
    public class Rule
    {
        private bool validOr;
        private Guid guid;
        private List<RuleParameter> parameters;
        private List<RuleVariable> variables;
        private WealthLab.RuleType ruleType;
        private string name;
        private string description;
        private string category;
        private string init;
        private string body;
        private string plotting;
        private string tempInit;
        private string tempBody;
        private string usingClause;

        public Rule()
        {
            this.name = "";
            this.description = "";
            this.category = "";
            this.init = "";
            this.body = "";
            this.plotting = "";
            this.guid = Guid.NewGuid();
            this.parameters = new List<RuleParameter>();
            this.variables = new List<RuleVariable>();
        }

        public Rule(Rule baseRule)
        {
            this.name = "";
            this.description = "";
            this.category = "";
            this.init = "";
            this.body = "";
            this.plotting = "";
            this.guid = Guid.NewGuid();
            this.parameters = new List<RuleParameter>();
            this.variables = new List<RuleVariable>();
            this.body = baseRule.body;
            this.category = baseRule.category;
            this.description = baseRule.description;
            this.guid = baseRule.guid;
            this.init = baseRule.init;
            this.name = baseRule.name;
            this.plotting = baseRule.plotting;
            this.ruleType = baseRule.ruleType;
            foreach (RuleParameter parameter in baseRule.Parameters)
            {
                this.Parameters.Add(new RuleParameter(parameter));
            }
            foreach (RuleVariable variable in baseRule.Variables)
            {
                this.Variables.Add(new RuleVariable(variable));
            }
            this.usingClause = baseRule.usingClause;
        }

        public string Body
        {
            get
            {
                return this.body;
            }
            set
            {
                this.body = value;
            }
        }

        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public Guid ID
        {
            get
            {
                return this.guid;
            }
            set
            {
                this.guid = value;
            }
        }

        public string Init
        {
            get
            {
                return this.init;
            }
            set
            {
                this.init = value;
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
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public List<RuleParameter> Parameters
        {
            get
            {
                return this.parameters;
            }
            set
            {
                this.parameters = value;
            }
        }

        public string Plotting
        {
            get
            {
                return this.plotting;
            }
            set
            {
                this.plotting = value;
            }
        }

        public WealthLab.RuleType RuleType
        {
            get
            {
                return this.ruleType;
            }
            set
            {
                this.ruleType = value;
            }
        }

        [XmlIgnore]
        public string TempBody
        {
            get
            {
                return this.tempBody;
            }
            set
            {
                this.tempBody = value;
            }
        }

        [XmlIgnore]
        public string TempInit
        {
            get
            {
                return this.tempInit;
            }
            set
            {
                this.tempInit = value;
            }
        }

        public string UsingClause
        {
            get
            {
                return this.usingClause;
            }
            set
            {
                this.usingClause = value;
            }
        }

        [XmlIgnore]
        public bool ValidOr
        {
            get
            {
                return this.validOr;
            }
            set
            {
                this.validOr = value;
            }
        }

        public List<RuleVariable> Variables
        {
            get
            {
                return this.variables;
            }
            set
            {
                this.variables = value;
            }
        }
    }
}

