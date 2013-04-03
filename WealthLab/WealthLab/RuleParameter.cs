namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="RuleParameter", IsNullable=false)]
    public class RuleParameter
    {
        private bool bool_0;
        private double double_0;
        private double double_1;
        private double double_2;
        private List<string> list_0;
        private RuleParamType ruleParamType_0;
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;
        private string string_4;
        private string string_5;
        [CompilerGenerated]
        private string string_6;

        public RuleParameter()
        {
            this.string_3 = "";
            this.string_4 = "";
            this.string_5 = "";
            this.list_0 = new List<string>();
        }

        public RuleParameter(RuleParameter baseRP)
        {
            this.string_3 = "";
            this.string_4 = "";
            this.string_5 = "";
            this.list_0 = new List<string>();
            this.Name = baseRP.string_0;
            this.double_0 = baseRP.double_0;
            this.double_2 = baseRP.double_2;
            this.double_1 = baseRP.double_1;
            this.ruleParamType_0 = baseRP.ruleParamType_0;
            this.string_2 = baseRP.string_2;
            this.string_1 = baseRP.string_1;
            this.string_3 = baseRP.string_3;
            this.bool_0 = baseRP.bool_0;
            this.DisplayName = baseRP.DisplayName;
            this.IndicatorParameterDisplayNames = baseRP.IndicatorParameterDisplayNames;
        }

        public override string ToString()
        {
            return this.string_0;
        }

        [XmlIgnore]
        public string AliasName
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

        public string Decoration
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

        public string DefaultValue
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

        public string DisplayName
        {
            [CompilerGenerated]
            get
            {
                return this.string_6;
            }
            [CompilerGenerated]
            set
            {
                this.string_6 = value;
            }
        }

        public bool ExposeAsSlider
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

        public List<string> IndicatorParameterDisplayNames
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

        public string Name
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
                this.DisplayName = value;
            }
        }

        public RuleParamType ParamType
        {
            get
            {
                return this.ruleParamType_0;
            }
            set
            {
                this.ruleParamType_0 = value;
            }
        }

        [XmlIgnore]
        public string ReplaceValue
        {
            get
            {
                if (this.string_5 == "")
                {
                    return this.Value;
                }
                return this.string_5;
            }
            set
            {
                this.string_5 = value;
            }
        }

        public double Start
        {
            get
            {
                return this.double_0;
            }
            set
            {
                this.double_0 = value;
            }
        }

        public double Step
        {
            get
            {
                return this.double_2;
            }
            set
            {
                this.double_2 = value;
            }
        }

        public double Stop
        {
            get
            {
                return this.double_1;
            }
            set
            {
                this.double_1 = value;
            }
        }

        public string Value
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
    }
}

