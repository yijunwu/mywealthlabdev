namespace WealthLabPro
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="QuickRefCategory", IsNullable=false)]
    public class QuickRefCategory : IComparable
    {
        private WealthLabPro.CategoryType categoryType_0;
        private List<QuickRefEntry> list_0 = new List<QuickRefEntry>();
        private string string_0;
        private string string_1 = "";

        public int CompareTo(object target)
        {
            QuickRefCategory category = (QuickRefCategory) target;
            return this.Name.CompareTo(category.Name);
        }

        public WealthLabPro.CategoryType CategoryType
        {
            get
            {
                return this.categoryType_0;
            }
            set
            {
                this.categoryType_0 = value;
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

        public List<QuickRefEntry> Entries
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
    }
}

