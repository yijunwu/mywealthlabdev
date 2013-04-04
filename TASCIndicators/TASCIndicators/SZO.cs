namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class SZO : DataSeries
    {
        private DataSeries ds;
        private int period;

        public SZO(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            base.FirstValidValue = period * 3;
            if ((ds != null) && (ds.Count != 0))
            {
                DataSeries series = new DataSeries(ds, "R");
                DataSeries series2 = new DataSeries(ds, "SP");
                for (int i = period; i < ds.Count; i++)
                {
                    series[i] = (ds[i] > ds[i - 1]) ? ((double) 1) : ((double) (-1));
                }
                for (int j = period; j < ds.Count; j++)
                {
                    series2[j] = TEMA.Series(series, period, 0)[j];
                }
                for (int k = base.FirstValidValue; k < ds.Count; k++)
                {
                    base[k] = 100.0 * (series2[k] / ((double) period));
                }
            }
        }

        public static SZO Series(DataSeries ds, int period)
        {
            string key = string.Concat(new object[] { "SZO(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (SZO) ds.Cache[key];
            }
            SZO szo = new SZO(ds, period, key);
            ds.Cache[key] = szo;
            return szo;
        }
    }
}

