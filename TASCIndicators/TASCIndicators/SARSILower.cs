namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class SARSILower : DataSeries
    {
        private DataSeries ds;
        private double multiplier;
        private int period;

        public SARSILower(DataSeries ds, int period, double multiplier, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            this.multiplier = multiplier;
            RSI rsi = RSI.Series(ds, period);
            SMA sma = SMA.Series(rsi, period);
            DataSeries series = (DataSeries) (SMA.Series(DataSeries.Abs(rsi - sma), period) * multiplier);
            DataSeries series2 = (DataSeries) (series * -1.0);
            series += 50.0;
            series2 += 50.0;
            base.FirstValidValue = period - 1;
            if ((base.FirstValidValue > ds.Count) || (base.FirstValidValue < 0))
            {
                base.FirstValidValue = ds.Count;
            }
            if (ds.Count >= period)
            {
                for (int i = base.FirstValidValue; i < ds.Count; i++)
                {
                    base[i] = series2[i];
                }
            }
        }

        public static SARSILower Series(DataSeries ds, int period, double multiplier)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "SARSILower(", ds.Description, ",", period, ",", multiplier, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (SARSILower) ds.Cache[key];
            }
            ds.Cache[key] = series = new SARSILower(ds, period, multiplier, key);
            return (SARSILower) series;
        }
    }
}

