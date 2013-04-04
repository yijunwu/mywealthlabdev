namespace Steema.TeeChart.Styles
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class OrgItems : CollectionBase
    {
        internal OrgSeries owner;

        public OrgItems(OrgSeries Series)
        {
            this.owner = Series;
        }

        public OrgItem Add()
        {
            OrgItem i = new OrgItem(this);
            this.Add(i);
            return i;
        }

        public int Add(OrgItem i)
        {
            int num = base.List.Add(i);
            i.Index = num;
            return num;
        }

        public void AddRange(ICollection c)
        {
            object[] array = new object[c.Count - 1];
            c.CopyTo(array, 0);
            foreach (object obj2 in array)
            {
                this.Add(obj2 as OrgItem);
            }
        }

        public void Assign(OrgItems value)
        {
            base.Clear();
            this.AddRange(value);
        }

        public int IndexOf(OrgItem l)
        {
            return base.List.IndexOf(l);
        }

        public int LevelOf(OrgItem i)
        {
            int superior = i.Superior;
            if (superior == -1)
            {
                return 0;
            }
            OrgItem item = this[superior];
            int num2 = 1;
            while (item.Superior != -1)
            {
                item = this[item.Superior];
                num2++;
            }
            return num2;
        }

        public OrgItem this[int index]
        {
            get
            {
                return (OrgItem) base.List[index];
            }
            set
            {
                OrgItem item1 = (OrgItem) base.List[index];
            }
        }
    }
}

