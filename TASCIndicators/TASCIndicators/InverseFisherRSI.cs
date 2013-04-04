namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class InverseFisherRSI : DataSeries
    {
        private int _emaPer;
        private int _rsiPer;
        private DataSeries ds;

        public InverseFisherRSI(DataSeries ds, int rsiPeriod, int emaPeriod, string description) : base(ds, description)
        {
            this.ds = ds;
            this._emaPer = emaPeriod;
            this._rsiPer = rsiPeriod;
            base.FirstValidValue = Math.Max(ds.FirstValidValue, Math.Max(emaPeriod, rsiPeriod));
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            DataSeries series = WMA.Series(ds, 2);
            DataSeries series2 = (DataSeries) (5.0 * series);
            for (int i = 4; i >= -4; i--)
            {
                series = WMA.Series(series, 2);
                if (i > 1)
                {
                    series2 += (DataSeries) (i * series);
                }
                else
                {
                    series2 += series;
                }
            }
            series2 = (DataSeries) (series2 / 20.0);
            DataSeries source = (DataSeries) (0.1 * (RSI.Series(series2, rsiPeriod) - 50.0));
            DataSeries series4 = EMA.Series(source, emaPeriod, EMACalculation.Modern);
            DataSeries series5 = EMA.Series(series4, emaPeriod, EMACalculation.Modern);
            DataSeries series6 = series4 + (series4 - series5);
            for (int j = 0; j < ds.Count; j++)
            {
                base[j] = (InverseFisher.Value(j, series6) + 1.0) * 50.0;
            }
        }

        public static InverseFisherRSI Series(DataSeries ds, int rsiPeriod, int emaPeriod)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "InverseFisherRSI(", ds.Description, ",", rsiPeriod, ",", emaPeriod, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (InverseFisherRSI) ds.Cache[key];
            }
            ds.Cache[key] = series = new InverseFisherRSI(ds, rsiPeriod, emaPeriod, key);
            return (InverseFisherRSI) series;
        }
    }
}

