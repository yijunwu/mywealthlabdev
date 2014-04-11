namespace WealthLabPro
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="QuickRefCategory", IsNullable=false)]
    public class QuickRefCategory : IComparable
    {
        private WealthLabPro.CategoryType categoryType;
        private List<QuickRefEntry> entries = new List<QuickRefEntry>();
        private string name;
        private string description = "";

        public int CompareTo(object target)
        {
            QuickRefCategory category = (QuickRefCategory) target;
            return this.Name.CompareTo(category.Name);
        }

        public WealthLabPro.CategoryType CategoryType
        {
            get
            {
                return this.categoryType;
            }
            set
            {
                this.categoryType = value;
            }
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

        public List<QuickRefEntry> Entries
        {
            get
            {
                return this.entries;
            }
            set
            {
                this.entries = value;
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

