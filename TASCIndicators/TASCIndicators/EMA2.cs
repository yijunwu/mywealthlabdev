namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class EMA2 : DataSeries
    {
        private DataSeries ds;
        private double expnt;

        public EMA2(DataSeries ds, double period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.expnt = 2.0 / (1.0 + period);
            base.FirstValidValue = ds.FirstValidValue + 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            double num = ds[base.FirstValidValue - 1];
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                num += this.expnt * (ds[i] - num);
                base[i] = num;
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
                    base.PartialValue = base[count - 1] + (this.expnt * (this.ds.PartialValue - base[count - 1]));
                }
            }
        }

        public static EMA2 Series(DataSeries ds, double period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "EMA2(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (EMA2) ds.Cache[key];
            }
            ds.Cache[key] = series = new EMA2(ds, period, key);
            return (EMA2) series;
        }
    }
}

