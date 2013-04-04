namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class Gapo : DataSeries
    {
        private double logperiod;
        private DataSeries range;

        public Gapo(Bars ds, int period, string description) : base(ds, description)
        {
            this.range = Highest.Series(ds.High, period) - Lowest.Series(ds.Low, period);
            this.logperiod = Math.Log10((double) period);
            base.FirstValidValue = this.range.FirstValidValue + period;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = Math.Log10(this.range[i]) / this.logperiod;
            }
        }

        public override void CalculatePartialValue()
        {
            this.range.CalculatePartialValue();
            if ((this.range.PartialValue <= 0.0) || double.IsNaN(this.range.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                base.PartialValue = Math.Log10(this.range.PartialValue) / this.logperiod;
            }
        }

        public static Gapo Series(Bars ds, int period)
        {
            DataSeries series;
            string key = "Gapo(" + period + ")";
            if (ds.Cache.ContainsKey(key))
            {
                return (Gapo) ds.Cache[key];
            }
            ds.Cache[key] = series = new Gapo(ds, period, key);
            return (Gapo) series;
        }

        public static double Value(int bar, Bars ds, int period)
        {
            if (period < 2)
            {
                return double.NaN;
            }
            double d = Highest.Value(bar, ds.High, period) - Lowest.Value(bar, ds.Low, period);
            if (d <= 0.0)
            {
                return double.NaN;
            }
            return (Math.Log10(d) / Math.Log10((double) period));
        }
    }
}

