namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class TrueHigh : DataSeries
    {
        private Bars ds;

        public TrueHigh(Bars ds, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = Math.Max(ds.High[i], ds.Close[i - 1]);
            }
        }

        public override void CalculatePartialValue()
        {
            int count = this.ds.Count;
            if ((count < 1) || double.IsNaN(this.ds.High.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                base.PartialValue = Math.Max(this.ds.High.PartialValue, this.ds.Close[count - 1]);
            }
        }

        public static TrueHigh Series(Bars ds)
        {
            DataSeries series;
            string key = "TrueHigh()";
            if (ds.Cache.ContainsKey(key))
            {
                return (TrueHigh) ds.Cache[key];
            }
            ds.Cache[key] = series = new TrueHigh(ds, key);
            return (TrueHigh) series;
        }

        public static double Value(int bar, Bars ds)
        {
            return Math.Max(ds.High[bar], ds.Close[bar - 1]);
        }
    }
}

