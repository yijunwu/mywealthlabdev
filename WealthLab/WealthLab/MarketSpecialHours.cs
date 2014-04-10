namespace WealthLab
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="MarketSpecialHours", IsNullable=false)]
    public class MarketSpecialHours
    {
        private DateTime date;
        private DateTime openTimeNative;
        private DateTime closeTimeNative;

        public DateTime CloseTimeNative
        {
            get
            {
                return this.closeTimeNative;
            }
            set
            {
                this.closeTimeNative = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = value;
            }
        }

        public DateTime OpenTimeNative
        {
            get
            {
                return this.openTimeNative;
            }
            set
            {
                this.openTimeNative = value;
            }
        }
    }
}

