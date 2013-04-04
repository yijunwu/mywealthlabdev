namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class TrueLow : DataSeries
    {
        private Bars ds;

        public TrueLow(Bars ds, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = Math.Min(ds.Low[i], ds.Close[i - 1]);
            }
        }

        public override void CalculatePartialValue()
        {
            int count = this.ds.Count;
            if ((count < 1) || double.IsNaN(this.ds.Low.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                base.PartialValue = Math.Min(this.ds.Low.PartialValue, this.ds.Close[count - 1]);
            }
        }

        public static TrueLow Series(Bars ds)
        {
            DataSeries series;
            string key = "TrueLow()";
            if (ds.Cache.ContainsKey(key))
            {
                return (TrueLow) ds.Cache[key];
            }
            ds.Cache[key] = series = new TrueLow(ds, key);
            return (TrueLow) series;
        }

        public static double Value(int bar, Bars ds)
        {
            return Math.Min(ds.Low[bar], ds.Close[bar - 1]);
        }
    }
}

