namespace WealthLab.International.CustomersWebService
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml;
    using System.Xml.Serialization;

    [Serializable, XmlRoot(Namespace="http://www.wealth-lab.com/", IsNullable=false), DebuggerStepThrough, GeneratedCode("System.Xml", "4.0.30319.233"), XmlType(Namespace="http://www.wealth-lab.com/"), DesignerCategory("code")]
    public class ServiceHeader : SoapHeader
    {
        private XmlAttribute[] anyAttrField;
        private string paramField;

        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }

        public string Param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }
    }
}

