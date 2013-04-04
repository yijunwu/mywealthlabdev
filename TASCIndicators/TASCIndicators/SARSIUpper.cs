namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class SARSIUpper : DataSeries
    {
        private DataSeries ds;
        private double multiplier;
        private int period;

        public SARSIUpper(DataSeries ds, int period, double multiplier, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            this.multiplier = multiplier;
            RSI rsi = RSI.Series(ds, period);
            SMA sma = SMA.Series(rsi, period);
            DataSeries series = (DataSeries) (SMA.Series(DataSeries.Abs(rsi - sma), period) * multiplier);
            series += (DataSeries) 50.0;
            base.FirstValidValue = period - 1;
            if ((base.FirstValidValue > ds.Count) || (base.FirstValidValue < 0))
            {
                base.FirstValidValue = ds.Count;
            }
            if (ds.Count >= period)
            {
                for (int i = base.FirstValidValue; i < ds.Count; i++)
                {
                    base[i] = series[i];
                }
            }
        }

        public static SARSIUpper Series(DataSeries ds, int period, double multiplier)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "SARSIUpper(", ds.Description, ",", period, ",", multiplier, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (SARSIUpper) ds.Cache[key];
            }
            ds.Cache[key] = series = new SARSIUpper(ds, period, multiplier, key);
            return (SARSIUpper) series;
        }
    }
}

