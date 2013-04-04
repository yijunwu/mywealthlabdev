namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class RSS : DataSeries
    {
        private SMA smooth;

        public RSS(DataSeries ds, int fastsmaperiod, int slowsmaperiod, int rsiperiod, int smoothperiod, string description) : base(ds, description)
        {
            DataSeries series = SMA.Series(ds, fastsmaperiod) - SMA.Series(ds, slowsmaperiod);
            DataSeries series2 = RSI.Series(series, rsiperiod);
            this.smooth = SMA.Series(series2, smoothperiod);
            base.FirstValidValue = (((fastsmaperiod > slowsmaperiod) ? fastsmaperiod : slowsmaperiod) + rsiperiod) + smoothperiod;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = this.smooth[i];
            }
        }

        public override void CalculatePartialValue()
        {
            this.smooth.CalculatePartialValue();
            base.PartialValue = this.smooth.PartialValue;
        }

        public static RSS Series(DataSeries ds, int fastsmaperiod, int slowsmaperiod, int rsiperiod, int smoothperiod)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "RSS(", ds.Description, ",", fastsmaperiod, ",", slowsmaperiod, ",", rsiperiod, ",", smoothperiod, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (RSS) ds.Cache[key];
            }
            ds.Cache[key] = series = new RSS(ds, fastsmaperiod, slowsmaperiod, rsiperiod, smoothperiod, key);
            return (RSS) series;
        }
    }
}

