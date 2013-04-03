namespace WealthLab
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="RuleVariable", IsNullable=false)]
    public class RuleVariable
    {
        private string string_0;
        private string string_1;

        public RuleVariable()
        {
        }

        public RuleVariable(RuleVariable baseRV)
        {
            this.string_1 = baseRV.string_1;
            this.string_0 = baseRV.string_0;
        }

        public override string ToString()
        {
            return this.string_0;
        }

        public string AliasName
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
    }
}

