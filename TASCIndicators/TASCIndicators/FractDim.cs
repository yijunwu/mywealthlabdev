namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class FractDim : DataSeries
    {
        private DataSeries ds;
        private DataSeries hh;
        private DataSeries ll;
        private double ln2p;
        private int period;

        public FractDim(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            if ((period < 2) || (period > (ds.Count + 1)))
            {
                period = ds.Count + 1;
            }
            this.hh = Highest.Series(ds, period);
            this.ll = Lowest.Series(ds, period);
            this.ln2p = Math.Log((double) (2 * (period - 1)));
            base.FirstValidValue = (ds.FirstValidValue + period) - 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                double num2 = this.hh[i] - this.ll[i];
                if (num2 <= 0.0)
                {
                    base[i] = 1.0;
                }
                else
                {
                    double d = 0.0;
                    for (int j = (i - period) + 2; j <= i; j++)
                    {
                        double num5 = ds[j] - ds[j - 1];
                        d += hypot(num5 / num2, 1.0 / ((double) (period - 1)));
                    }
                    base[i] = 1.0 + (Math.Log(d) / this.ln2p);
                }
            }
        }

        public override void CalculatePartialValue()
        {
            this.ds.CalculatePartialValue();
            if (((this.period < 2) || (this.period > (this.ds.Count + 1))) || double.IsNaN(this.ds.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                this.hh.CalculatePartialValue();
                this.ll.CalculatePartialValue();
                double num = this.hh.PartialValue - this.ll.PartialValue;
                if (num <= 0.0)
                {
                    base.PartialValue = 0.0;
                }
                else
                {
                    int count = this.ds.Count;
                    double num3 = this.ds.PartialValue - this.ds[count - 1];
                    double d = hypot(num3 / num, (double) (1 / (this.period - 1)));
                    for (int i = (count - this.period) + 2; i < count; i++)
                    {
                        num3 = this.ds[i] - this.ds[i - 1];
                        d += hypot(num3 / num, (double) (1 / (this.period - 1)));
                    }
                    base.PartialValue = 1.0 + (Math.Log(d) / this.ln2p);
                }
            }
        }

        private static double hypot(double a, double b)
        {
            return Math.Sqrt((a * a) + (b * b));
        }

        public static FractDim Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "FractDim(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (FractDim) ds.Cache[key];
            }
            ds.Cache[key] = series = new FractDim(ds, period, key);
            return (FractDim) series;
        }

        public static double Value(int bar, DataSeries ds, int period)
        {
            if ((period < 2) || (period > (bar + 1)))
            {
                return 0.0;
            }
            double num = Highest.Value(bar, ds, period) - Lowest.Value(bar, ds, period);
            if (num <= 0.0)
            {
                return 0.0;
            }
            double d = 0.0;
            for (int i = (bar - period) + 2; i <= bar; i++)
            {
                double num4 = ds[i] - ds[i - 1];
                d += hypot(num4 / num, (double) (1 / (period - 1)));
            }
            return (1.0 + (Math.Log(d) / Math.Log((double) (2 * (period - 1)))));
        }
    }
}

