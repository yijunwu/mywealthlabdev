namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class InverseFisher : DataSeries
    {
        private DataSeries ds;

        public InverseFisher(DataSeries ds, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = ds.FirstValidValue;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                double num2 = Math.Exp(2.0 * ds[i]);
                base[i] = (num2 - 1.0) / (num2 + 1.0);
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
                    double num2 = Math.Exp(2.0 * this.ds[count]);
                    base.PartialValue = (num2 - 1.0) / (num2 + 1.0);
                }
            }
        }

        public static InverseFisher Series(DataSeries ds)
        {
            DataSeries series;
            string key = "InverseFisher(" + ds.Description + ")";
            if (ds.Cache.ContainsKey(key))
            {
                return (InverseFisher) ds.Cache[key];
            }
            ds.Cache[key] = series = new InverseFisher(ds, key);
            return (InverseFisher) series;
        }

        public static double Value(int bar, DataSeries ds)
        {
            double num = Math.Exp(2.0 * ds[bar]);
            return ((num - 1.0) / (num + 1.0));
        }
    }
}

