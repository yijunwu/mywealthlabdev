namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class RegEMA : DataSeries
    {
        private DataSeries ds;
        private double regularization;
        private double smoothing;

        public RegEMA(DataSeries ds, double smoothing, double regularization, string description) : base(ds, description)
        {
            this.ds = ds;
            this.smoothing = smoothing;
            this.regularization = regularization;
            base.FirstValidValue = ds.FirstValidValue + 2;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = 0; i < base.FirstValidValue; i++)
            {
                base[i] = ds[i];
            }
            for (int j = base.FirstValidValue; j < ds.Count; j++)
            {
                double num3 = (1.0 + (2.0 * regularization)) * base[j - 1];
                double num4 = smoothing * (ds[j] - base[j - 1]);
                double num5 = regularization * base[j - 2];
                base[j] = ((num3 + num4) - num5) / (1.0 + regularization);
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
                    double num2 = base[count - 1] * (1.0 + (2.0 * this.regularization));
                    double num3 = this.smoothing * (this.ds.PartialValue - base[count - 1]);
                    double num4 = this.regularization * base[count - 2];
                    base.PartialValue = ((num2 + num3) - num4) / (1.0 + this.regularization);
                }
            }
        }

        public static RegEMA Series(DataSeries ds, double smoothing, double regularization)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "RegEMA(", ds.Description, ",", smoothing, ",", regularization, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (RegEMA) ds.Cache[key];
            }
            ds.Cache[key] = series = new RegEMA(ds, smoothing, regularization, key);
            return (RegEMA) series;
        }
    }
}

