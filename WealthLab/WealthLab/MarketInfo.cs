namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="MarketInfo", IsNullable=false)]
    public class MarketInfo
    {
        private DateTime dateTime_0;
        private DateTime dateTime_1;
        private List<DateTime> list_0 = new List<DateTime>();
        private List<MarketSpecialHours> list_1 = new List<MarketSpecialHours>();
        private string string_0;
        private string string_1;
        private string string_2;

        public DateTime CloseTimeNative
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

        public string Description
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

        public List<DateTime> Holidays
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
            }
        }

        public DateTime OpenTimeNative
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

        public List<MarketSpecialHours> SpecialHours
        {
            get
            {
                return this.list_1;
            }
            set
            {
                this.list_1 = value;
            }
        }

        public string TimeZoneName
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

