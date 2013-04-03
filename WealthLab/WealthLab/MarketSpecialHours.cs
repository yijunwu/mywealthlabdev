namespace WealthLab
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="MarketSpecialHours", IsNullable=false)]
    public class MarketSpecialHours
    {
        private DateTime dateTime_0;
        private DateTime dateTime_1;
        private DateTime dateTime_2;

        public DateTime CloseTimeNative
        {
            get
            {
                return this.dateTime_2;
            }
            set
            {
                this.dateTime_2 = value;
            }
        }

        public DateTime Date
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

        public DateTime OpenTimeNative
        {
            get
            {
                return this.dateTime_1;
            }
            set
            {
                this.dateTime_1 = value;
            }
        }
    }
}

