namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class FRAMA : DataSeries
    {
        private DataSeries ds;
        private int halfperiod;
        private DataSeries HH;
        private double k;
        private DataSeries LL;
        private double Log2;

        public FRAMA(DataSeries ds, int period, double k, string description) : base(ds, description)
        {
            this.ds = ds;
            this.k = k;
            this.halfperiod = period / 2;
            this.HH = Highest.Series(ds, this.halfperiod);
            this.LL = Lowest.Series(ds, this.halfperiod);
            this.Log2 = Math.Log(2.0);
            base.FirstValidValue = (ds.FirstValidValue + (2 * this.halfperiod)) - 1;
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
                double num3 = this.HH[j - this.halfperiod];
                double num4 = this.LL[j - this.halfperiod];
                double num5 = (num3 - num4) / ((double) this.halfperiod);
                double num6 = this.HH[j];
                double num7 = this.LL[j];
                double num8 = (num6 - num7) / ((double) this.halfperiod);
                double num9 = Math.Max(num3, num6);
                double num10 = Math.Min(num4, num7);
                double num11 = (num9 - num10) / ((double) (2 * this.halfperiod));
                double num12 = num5 + num8;
                double num13 = 1.0;
                if (num12 > 0.0)
                {
                    num13 = Math.Log(num12 / num11) / this.Log2;
                }
                if (num13 < 1.0)
                {
                    num13 = 1.0;
                }
                double num14 = Math.Exp(-k * (num13 - 1.0));
                base[j] = (num14 * ds[j]) + ((1.0 - num14) * base[j - 1]);
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
                this.HH.CalculatePartialValue();
                this.LL.CalculatePartialValue();
                if (double.IsNaN(this.HH.PartialValue) || double.IsNaN(this.LL.PartialValue))
                {
                    base.PartialValue = double.NaN;
                }
                else
                {
                    double num2 = this.HH[count - this.halfperiod];
                    double num3 = this.LL[count - this.halfperiod];
                    double num4 = (num2 - num3) / ((double) this.halfperiod);
                    double num5 = this.HH[count];
                    double num6 = this.LL[count];
                    double num7 = (num5 - num6) / ((double) this.halfperiod);
                    double num8 = Math.Max(num2, num5);
                    double num9 = Math.Min(num3, num6);
                    double num10 = (num8 - num9) / ((double) (2 * this.halfperiod));
                    double num11 = num4 + num7;
                    double num12 = 1.0;
                    if (num11 > 0.0)
                    {
                        num12 = Math.Log(num11 / num10) / this.Log2;
                    }
                    if (num12 < 1.0)
                    {
                        num12 = 1.0;
                    }
                    double num13 = Math.Exp(-this.k * (num12 - 1.0));
                    base.PartialValue = (num13 * this.ds.PartialValue) + ((1.0 - num13) * base[count - 1]);
                }
            }
        }

        public static FRAMA Series(DataSeries ds, int period, double k)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "FRAMA(", ds.Description, ",", period, ",", k, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (FRAMA) ds.Cache[key];
            }
            ds.Cache[key] = series = new FRAMA(ds, period, k, key);
            return (FRAMA) series;
        }
    }
}

