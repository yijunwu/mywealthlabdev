namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class NVI : DataSeries
    {
        private Bars ds;

        public NVI(Bars ds, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            double num = 0.0;
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                if (ds.Volume[i] <= ds.Volume[i - 1])
                {
                    num += ((100.0 * ds.Close[i]) / ds.Close[i - 1]) - 100.0;
                }
                base[i] = num;
            }
        }

        public override void CalculatePartialValue()
        {
            int count = this.ds.Count;
            if (this.ds.Volume.PartialValue <= this.ds.Volume[count - 1])
            {
                base.PartialValue = (base[count - 1] + ((100.0 * this.ds.Close.PartialValue) / this.ds.Close[count - 1])) - 100.0;
            }
            else
            {
                base.PartialValue = base[count - 1];
            }
        }

        public static NVI Series(Bars ds)
        {
            DataSeries series;
            string key = "NVI()";
            if (ds.Cache.ContainsKey(key))
            {
                return (NVI) ds.Cache[key];
            }
            ds.Cache[key] = series = new NVI(ds, key);
            return (NVI) series;
        }
    }
}

