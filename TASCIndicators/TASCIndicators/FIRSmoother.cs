namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class FIRSmoother : DataSeries
    {
        private DataSeries ds;

        public FIRSmoother(DataSeries ds, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = ds.FirstValidValue + 6;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = (((((((2.0 * ds[i]) + (7.0 * ds[i - 1])) + (9.0 * ds[i - 2])) + (6.0 * ds[i - 3])) + (1.0 * ds[i - 4])) - (1.0 * ds[i - 5])) - (3.0 * ds[i - 6])) / 21.0;
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
                    base.PartialValue = (((((((2.0 * this.ds.PartialValue) + (7.0 * this.ds[count - 1])) + (9.0 * this.ds[count - 2])) + (6.0 * this.ds[count - 3])) + (1.0 * this.ds[count - 4])) - (1.0 * this.ds[count - 5])) - (3.0 * this.ds[count - 6])) / 21.0;
                }
            }
        }

        public static FIRSmoother Series(DataSeries ds)
        {
            DataSeries series;
            string key = "FIRSmoother(" + ds.Description + ")";
            if (ds.Cache.ContainsKey(key))
            {
                return (FIRSmoother) ds.Cache[key];
            }
            ds.Cache[key] = series = new FIRSmoother(ds, key);
            return (FIRSmoother) series;
        }

        public static double Value(int bar, DataSeries ds)
        {
            if (bar < 6)
            {
                return 0.0;
            }
            return ((((((((2.0 * ds[bar]) + (7.0 * ds[bar - 1])) + (9.0 * ds[bar - 2])) + (6.0 * ds[bar - 3])) + (1.0 * ds[bar - 4])) - (1.0 * ds[bar - 5])) - (3.0 * ds[bar - 6])) / 21.0);
        }
    }
}

