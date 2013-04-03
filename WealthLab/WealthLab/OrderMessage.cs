namespace WealthLab
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="OrderMessage", IsNullable=false)]
    public class OrderMessage
    {
        private System.DateTime dateTime_0;
        private string string_0;

        public System.DateTime DateTime
        {
            get
            {
                return this.dateTime_0;
            }
            set
            {
                this.dateTime_0 = value;
            }
        }

        public string Message
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

