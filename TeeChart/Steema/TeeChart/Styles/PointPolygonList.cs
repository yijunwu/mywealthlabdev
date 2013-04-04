namespace Steema.TeeChart.Styles
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class PointPolygonList : CollectionBase
    {
        private Steema.TeeChart.Styles.Series series;

        public PointPolygonList(Steema.TeeChart.Styles.Series s)
        {
            this.series = s;
        }

        public int Add(PointPolygon value)
        {
            return base.List.Add(value);
        }

        public void AddRange(ICollection c)
        {
            object[] array = new object[c.Count - 1];
            c.CopyTo(array, 0);
            foreach (object obj2 in array)
            {
                this.Add(obj2 as PointPolygon);
            }
        }

        public void Assign(PointPolygonList Value)
        {
            base.Clear();
            this.AddRange(Value);
        }

        private PointPolygon GetByName(string AName)
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

        public int IndexOf(Steema.TeeChart.Styles.Series s)
        {
            return base.List.IndexOf(s);
        }

        public void RemoveRange(int index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                base.List.RemoveAt(index);
            }
        }

        public PointPolygon this[int index]
        {
            get
            {
                return (PointPolygon) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public PointPolygon this[string AName]
        {
            get
            {
                return this.GetByName(AName);
            }
        }

        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.series;
            }
            set
            {
                this.series = value;
            }
        }
    }
}

