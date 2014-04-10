namespace WealthLab
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="RuleVariable", IsNullable=false)]
    public class RuleVariable
    {
        private string name;
        private string aliasName;

        public RuleVariable()
        {
        }

        public RuleVariable(RuleVariable baseRV)
        {
            this.aliasName = baseRV.aliasName;
            this.name = baseRV.name;
        }

        public override string ToString()
        {
            return this.name;
        }

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
    }
}

