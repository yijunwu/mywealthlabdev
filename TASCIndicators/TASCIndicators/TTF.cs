namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class TTF : DataSeries
    {
        private Bars ds;
        private DataSeries HH;
        private DataSeries LL;
        private int period;

        public TTF(Bars ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            this.HH = Highest.Series(ds.High, period);
            this.LL = Lowest.Series(ds.Low, period);
            if ((period < 1) || (period > (ds.Count + 1)))
            {
                period = ds.Count + 1;
            }
            base.FirstValidValue = (2 * period) - 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                double num2 = this.HH[i] - this.LL[i - period];
                double num3 = this.HH[i - period] - this.LL[i];
                if ((num2 + num3) != 0.0)
                {
                    base[i] = (200.0 * (num2 - num3)) / (num2 + num3);
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.period < 1) || ((2 * this.period) > (this.ds.Count + 1)))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                this.HH.CalculatePartialValue();
                this.LL.CalculatePartialValue();
                int count = this.ds.Count;
                double num2 = this.HH.PartialValue - this.LL[count - this.period];
                double num3 = this.HH[count - this.period] - this.LL.PartialValue;
                if ((num2 + num3) == 0.0)
                {
                    base.PartialValue = 0.0;
                }
                else
                {
                    base.PartialValue = (200.0 * (num2 - num3)) / (num2 + num3);
                }
            }
        }

        public static TTF Series(Bars ds, int period)
        {
            DataSeries series;
            string key = "TTF(" + period + ")";
            if (ds.Cache.ContainsKey(key))
            {
                return (TTF) ds.Cache[key];
            }
            ds.Cache[key] = series = new TTF(ds, period, key);
            return (TTF) series;
        }

        public static double Value(int bar, Bars ds, int period)
        {
            if (bar < ((2 * period) - 1))
            {
                return 0.0;
            }
            double num = Highest.Value(bar, ds.High, period) - Lowest.Value(bar - period, ds.Low, period);
            double num2 = Highest.Value(bar - period, ds.High, period) - Lowest.Value(bar, ds.Low, period);
            if ((num + num2) == 0.0)
            {
                return 0.0;
            }
            return ((200.0 * (num - num2)) / (num + num2));
        }
    }
}

