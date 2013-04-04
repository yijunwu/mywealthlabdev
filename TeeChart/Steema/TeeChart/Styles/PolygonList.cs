namespace Steema.TeeChart.Styles
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;

    public class PolygonList : CollectionBase
    {
        private Map owner;

        public PolygonList(Map MapSeries)
        {
            this.owner = MapSeries;
        }

        public int Add(Polygon value)
        {
            int num = base.List.Add(value);
            value.Index = num;
            return num;
        }

        public void AddRange(ICollection c)
        {
            object[] array = new object[c.Count - 1];
            c.CopyTo(array, 0);
            foreach (object obj2 in array)
            {
                this.Add(obj2 as Polygon);
            }
        }

        public void Assign(PolygonList Value)
        {
            base.Clear();
            this.AddRange(Value);
        }

        private Polygon GetByName(string AName)
        {
            string str = AName.ToUpper();
            for (int i = 0; i < (base.Count - 1); i++)
            {
                if (this[i].ToString().ToUpper() == str)
                {
                    return this[i];
                }
            }
            return null;
        }

        public int IndexOf(Series s)
        {
            return base.List.IndexOf(s);
        }

        protected override void OnClear()
        {
            for (int i = 0; i < base.Count; i++)
            {
                (base.List[i] as Polygon).Dispose();
            }
        }

        public void RemoveRange(int index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                base.List.RemoveAt(index);
            }
        }

        public Polygon this[int index]
        {
            get
            {
                return (Polygon) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public Polygon this[string AName]
        {
            get
            {
                return this.GetByName(AName);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Map Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                this.owner = value;
            }
        }
    }
}

