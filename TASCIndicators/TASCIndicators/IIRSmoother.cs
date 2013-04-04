namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class IIRSmoother : DataSeries
    {
        private DataSeries ds;

        public IIRSmoother(DataSeries ds, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = ds.FirstValidValue + 4;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            base[base.FirstValidValue - 1] = ds[base.FirstValidValue - 1];
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = (0.2 * ((2.0 * ds[i]) - ds[i - 4])) + (0.8 * base[i - 1]);
            }
        }

        public override void CalculatePartialValue()
        {
            int count = this.ds.Count;
            if (count < base.FirstValidValue)
            {
                base.PartialValue = 0.0;
            }
            else
            {
                this.ds.CalculatePartialValue();
                if (double.IsNaN(this.ds.PartialValue))
                {
                    base.PartialValue = double.NaN;
                }
                else
                {
                    base.PartialValue = (0.2 * ((2.0 * this.ds.PartialValue) - this.ds[base.Count - 4])) + (0.8 * base[count - 1]);
                }
            }
        }

        public static IIRSmoother Series(DataSeries ds)
        {
            DataSeries series;
            string key = "IIRSmoother(" + ds.Description + ")";
            if (ds.Cache.ContainsKey(key))
            {
                return (IIRSmoother) ds.Cache[key];
            }
            ds.Cache[key] = series = new IIRSmoother(ds, key);
            return (IIRSmoother) series;
        }
    }
}

