namespace WealthLabPro
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="QuickRefEntry", IsNullable=false)]
    public class QuickRefEntry : IComparable
    {
        private WealthLabPro.EntryType entryType;
        private string name;
        private string description;
        private string example;

        public int CompareTo(object target)
        {
            QuickRefEntry entry = (QuickRefEntry) target;
            return this.Name.CompareTo(entry.Name);
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public WealthLabPro.EntryType EntryType
        {
            get
            {
                return this.entryType;
            }
            set
            {
                this.entryType = value;
            }
        }

        public string Example
        {
            get
            {
                return this.example;
            }
            set
            {
                this.example = value;
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

