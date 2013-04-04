namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class SeriesGroups : CollectionBase
    {
        internal Chart chart;

        public SeriesGroups(Chart c)
        {
            this.chart = c;
        }

        public SeriesGroup Add(string name)
        {
            SeriesGroup group = new SeriesGroup(this.chart) {
                Name = name
            };
            base.List.Add(group);
            return group;
        }

        public int Contains(Series s)
        {
            for (int i = 0; i < base.List.Count; i++)
            {
                if (this[i].Series.IndexOf(s) >= 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public SeriesGroup FindByName(string Name, bool CaseSensitive)
        {
            SeriesGroup group = null;
            for (int i = 0; i < base.Count; i++)
            {
                group = base.List[i] as SeriesGroup;
                if (this.StringsEqual(group.Name, Name, CaseSensitive))
                {
                    return group;
                }
            }
            return group;
        }

        private bool StringsEqual(string A, string B, bool CaseSensitive)
        {
            return ((CaseSensitive && (A == B)) || (!CaseSensitive && (A.ToUpper() == B.ToUpper())));
        }

        public SeriesGroup this[int index]
        {
            get
            {
                return (SeriesGroup) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

