namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="RuleParameter", IsNullable=false)]
    public class RuleParameter
    {
        private bool exposeAsSlider;
        private double start;
        private double stop;
        private double step;
        private List<string> indicatorParameterDisplayNames;
        private RuleParamType ruleParamType;
        private string name;
        private string defaultValue;
        private string valueStr;
        private string decoration;
        private string aliasName;
        private string replaceValueStr;
        [CompilerGenerated]
        private string displayName;

        public RuleParameter()
        {
            this.decoration = "";
            this.aliasName = "";
            this.replaceValueStr = "";
            this.indicatorParameterDisplayNames = new List<string>();
        }

        public RuleParameter(RuleParameter baseRP)
        {
            this.decoration = "";
            this.aliasName = "";
            this.replaceValueStr = "";
            this.indicatorParameterDisplayNames = new List<string>();
            this.Name = baseRP.name;
            this.start = baseRP.start;
            this.step = baseRP.step;
            this.stop = baseRP.stop;
            this.ruleParamType = baseRP.ruleParamType;
            this.valueStr = baseRP.valueStr;
            this.defaultValue = baseRP.defaultValue;
            this.decoration = baseRP.decoration;
            this.exposeAsSlider = baseRP.exposeAsSlider;
            this.DisplayName = baseRP.DisplayName;
            this.IndicatorParameterDisplayNames = baseRP.IndicatorParameterDisplayNames;
        }

        public override string ToString()
        {
            return this.name;
        }

        [XmlIgnore]
        public string AliasName
        {
            get
            {
                return this.aliasName;
            }
            set
            {
                this.aliasName = value;
            }
        }

        public string Decoration
        {
            get
            {
                return this.decoration;
            }
            set
            {
                this.decoration = value;
            }
        }

        public string DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
            set
            {
                this.defaultValue = value;
            }
        }

        public string DisplayName
        {
            [CompilerGenerated]
            get
            {
                return this.displayName;
            }
            [CompilerGenerated]
            set
            {
                this.displayName = value;
            }
        }

        public bool ExposeAsSlider
        {
            get
            {
                return this.exposeAsSlider;
            }
            set
            {
                this.exposeAsSlider = value;
            }
        }

        public List<string> IndicatorParameterDisplayNames
        {
            get
            {
                return this.indicatorParameterDisplayNames;
            }
            set
            {
                this.indicatorParameterDisplayNames = value;
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
                this.DisplayName = value;
            }
        }

        public RuleParamType ParamType
        {
            get
            {
                return this.ruleParamType;
            }
            set
            {
                this.ruleParamType = value;
            }
        }

        [XmlIgnore]
        public string ReplaceValue
        {
            get
            {
                if (this.replaceValueStr == "")
                {
                    return this.Value;
                }
                return this.replaceValueStr;
            }
            set
            {
                this.replaceValueStr = value;
            }
        }

        public double Start
        {
            get
            {
                return this.start;
            }
            set
            {
                this.start = value;
            }
        }

        public double Step
        {
            get
            {
                return this.step;
            }
            set
            {
                this.step = value;
            }
        }

        public double Stop
        {
            get
            {
                return this.stop;
            }
            set
            {
                this.stop = value;
            }
        }

        public string Value
        {
            get
            {
                return this.valueStr;
            }
            set
            {
                this.valueStr = value;
            }
        }
    }
}

