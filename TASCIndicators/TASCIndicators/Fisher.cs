namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class Fisher : DataSeries
    {
        private DataSeries ds;
        private DataSeries Hi;
        private DataSeries Lo;
        private int period;
        private double Value;
        private double Value1;

        public Fisher(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            if ((period < 1) || (period > (ds.Count + 1)))
            {
                period = ds.Count + 1;
            }
            this.Lo = Lowest.Series(ds, period);
            this.Hi = Highest.Series(ds, period);
            this.Value1 = 0.0;
            this.Value = 0.0;
            base.FirstValidValue = (ds.FirstValidValue + period) - 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                this.Value1 = 0.67 * this.Value1;
                if (this.Lo[i] < this.Hi[i])
                {
                    this.Value1 += 0.33 * (((2.0 * (ds[i] - this.Lo[i])) / (this.Hi[i] - this.Lo[i])) - 1.0);
                }
                this.Value = 0.5 * this.Value;
                this.Value += 0.5 * Math.Log((1.0 + this.Value1) / (1.0 - this.Value1));
                base[i] = this.Value;
            }
        }

        public override void CalculatePartialValue()
        {
            this.ds.CalculatePartialValue();
            if (((this.period < 1) || (this.period > (this.ds.Count + 1))) || double.IsNaN(this.ds.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                double num = 0.67 * this.Value1;
                this.Lo.CalculatePartialValue();
                this.Hi.CalculatePartialValue();
                if (this.Lo.PartialValue < this.Hi.PartialValue)
                {
                    num += 0.33 * (((2.0 * (this.ds.PartialValue - this.Lo.PartialValue)) / (this.Hi.PartialValue - this.Lo.PartialValue)) - 1.0);
                }
                base.PartialValue = (0.5 * this.Value) + (0.5 * Math.Log((1.0 + num) / (1.0 - num)));
            }
        }

        public static Fisher Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "Fisher(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (Fisher) ds.Cache[key];
            }
            ds.Cache[key] = series = new Fisher(ds, period, key);
            return (Fisher) series;
        }
    }
}

