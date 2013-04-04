namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class WilsonRSIChannel : DataSeries
    {
        private EMA smooth;

        public WilsonRSIChannel(DataSeries ds, int rsiperiod, int smoothperiod, double cord, string description) : base(ds, description)
        {
            DataSeries series = (DataSeries) ((RSI.Series(ds, rsiperiod) - cord) / 100.0);
            this.smooth = EMA.Series(ds - (ds * series), smoothperiod, EMACalculation.Modern);
            base.FirstValidValue = rsiperiod + smoothperiod;
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

        public static WilsonRSIChannel Series(DataSeries ds, int rsiperiod, int smoothperiod, double cord)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "WilsonRSIChannel(", ds.Description, ",", rsiperiod, ",", smoothperiod, ",", cord, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (WilsonRSIChannel) ds.Cache[key];
            }
            ds.Cache[key] = series = new WilsonRSIChannel(ds, rsiperiod, smoothperiod, cord, key);
            return (WilsonRSIChannel) series;
        }
    }
}

