namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class RWIHigh : DataSeries
    {
        private Bars ds;
        private int maxperiod;
        private int minperiod;

        public RWIHigh(Bars ds, int minperiod, int maxperiod, string description) : base(ds, description)
        {
            this.ds = ds;
            this.minperiod = minperiod;
            this.maxperiod = maxperiod;
            if ((minperiod < 2) || (minperiod > (ds.Count + 1)))
            {
                minperiod = ds.Count + 1;
            }
            if ((maxperiod < minperiod) || (maxperiod > (ds.Count + 1)))
            {
                maxperiod = ds.Count + 1;
            }
            base.FirstValidValue = maxperiod;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                double num2 = 0.0;
                for (int j = 1; j < minperiod; j++)
                {
                    num2 += TrueRange.Value((i - j) + 1, ds);
                }
                double num4 = 0.0;
                for (int k = minperiod; k <= maxperiod; k++)
                {
                    num2 += TrueRange.Value((i - k) + 1, ds);
                    if (num2 > 0.0)
                    {
                        num4 = Math.Max(num4, ((ds.High[i] - ds.Low[(i - k) + 1]) * Math.Sqrt((double) k)) / num2);
                    }
                }
                base[i] = num4;
            }
        }

        public override void CalculatePartialValue()
        {
            int count = this.ds.Count;
            if (count <= base.FirstValidValue)
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                double num2 = Math.Max(this.ds.High.PartialValue, this.ds.Close[count - 1]);
                double num3 = Math.Min(this.ds.Low.PartialValue, this.ds.Close[count - 1]);
                double num4 = num2 - num3;
                for (int i = 2; i < this.minperiod; i++)
                {
                    num4 += TrueRange.Value((count - i) + 1, this.ds);
                }
                double num6 = 0.0;
                for (int j = this.minperiod; j <= this.maxperiod; j++)
                {
                    num4 += TrueRange.Value((count - j) + 1, this.ds);
                    if (num4 > 0.0)
                    {
                        num6 = Math.Max(num6, ((this.ds.High.PartialValue - this.ds.Low[(count - j) + 1]) * Math.Sqrt((double) j)) / num4);
                    }
                }
                base.PartialValue = num6;
            }
        }

        public static RWIHigh Series(Bars ds, int minperiod, int maxperiod)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "RWIHigh(", minperiod, ",", maxperiod, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (RWIHigh) ds.Cache[key];
            }
            ds.Cache[key] = series = new RWIHigh(ds, minperiod, maxperiod, key);
            return (RWIHigh) series;
        }

        public static double Value(int bar, Bars ds, int minperiod, int maxperiod)
        {
            if ((bar < (minperiod - 1)) || (bar < (maxperiod - 1)))
            {
                return 0.0;
            }
            double num = 0.0;
            for (int i = 1; i < minperiod; i++)
            {
                num += TrueRange.Value((bar - i) + 1, ds);
            }
            double num3 = 0.0;
            for (int j = minperiod; j <= maxperiod; j++)
            {
                num += TrueRange.Value((bar - j) + 1, ds);
                if (num > 0.0)
                {
                    num3 = Math.Max(num3, ((ds.High[bar] - ds.Low[(bar - j) + 1]) * Math.Sqrt((double) j)) / num);
                }
            }
            return num3;
        }
    }
}

