namespace WealthLab.International.CustomersWebService
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlType(Namespace="http://www.wealth-lab.com/"), GeneratedCode("System.Xml", "4.0.30319.233"), DesignerCategory("code"), DebuggerStepThrough]
    public class AuthEducationalParams
    {
        private string activationKeyField;
        private DateTime computerDateField;
        private string computerNameField;
        private string hwidField;
        private DateTime lifetimeDateField;
        private string messageIdField;
        private int productIdField;
        private AuthEducationalResult resultField;
        private string universityField;
        private string versionField;

        public string ActivationKey
        {
            get
            {
                return this.activationKeyField;
            }
            set
            {
                this.activationKeyField = value;
            }
        }

        public DateTime ComputerDate
        {
            get
            {
                return this.computerDateField;
            }
            set
            {
                this.computerDateField = value;
            }
        }

        public string ComputerName
        {
            get
            {
                return this.computerNameField;
            }
            set
            {
                this.computerNameField = value;
            }
        }

        public string Hwid
        {
            get
            {
                return this.hwidField;
            }
            set
            {
                this.hwidField = value;
            }
        }

        public DateTime LifetimeDate
        {
            get
            {
                return this.lifetimeDateField;
            }
            set
            {
                this.lifetimeDateField = value;
            }
        }

        public string MessageId
        {
            get
            {
                return this.messageIdField;
            }
            set
            {
                this.messageIdField = value;
            }
        }

        public int ProductId
        {
            get
            {
                return this.productIdField;
            }
            set
            {
                this.productIdField = value;
            }
        }

        public AuthEducationalResult Result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }

        public string University
        {
            get
            {
                return this.universityField;
            }
            set
            {
                this.universityField = value;
            }
        }

        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }
}

