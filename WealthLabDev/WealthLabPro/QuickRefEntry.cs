namespace WealthLabPro
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="QuickRefEntry", IsNullable=false)]
    public class QuickRefEntry : IComparable
    {
        private WealthLabPro.EntryType entryType_0;
        private string string_0;
        private string string_1;
        private string string_2;

        public int CompareTo(object target)
        {
            QuickRefEntry entry = (QuickRefEntry) target;
            return this.Name.CompareTo(entry.Name);
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

        public WealthLabPro.EntryType EntryType
        {
            get
            {
                return this.entryType_0;
            }
            set
            {
                this.entryType_0 = value;
            }
        }

        public string Example
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

