namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class InverseFisherStoch : DataSeries
    {
        private DataSeries ds;

        public InverseFisherStoch(DataSeries ds, int stochPeriod, int smoothingPeriod, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = Math.Max(stochPeriod, smoothingPeriod);
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            DataSeries series = WMA.Series(ds, 2);
            int num = 5;
            DataSeries series2 = (DataSeries) (num * series);
            for (int i = 4; i > -5; i--)
            {
                series = WMA.Series(series, 2);
                num = (i > 0) ? i : 1;
                DataSeries series3 = (DataSeries) (num * series);
                series2 += series3;
            }
            series2 = (DataSeries) (series2 / 20.0);
            series2.Description = "StochRainbow";
            DataSeries series4 = this.StochD(series2, stochPeriod, smoothingPeriod);
            DataSeries series5 = (DataSeries) (0.1 * (series4 - 50.0));
            for (int j = base.FirstValidValue; j < ds.Count; j++)
            {
                double num4 = Math.Exp(2.0 * series5[j]);
                base[j] = 50.0 * (((num4 - 1.0) / (num4 + 1.0)) + 1.0);
            }
        }

        public override void CalculatePartialValue()
        {
        }

        public static InverseFisherStoch Series(DataSeries ds, int stochPeriod, int smoothingPeriod)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "InverseFisherStoch(", ds.Description, ",", stochPeriod, ",", smoothingPeriod, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (InverseFisherStoch) ds.Cache[key];
            }
            ds.Cache[key] = series = new InverseFisherStoch(ds, stochPeriod, smoothingPeriod, key);
            return (InverseFisherStoch) series;
        }

        private DataSeries StochD(DataSeries ds, int stoPer, int smooth)
        {
            DataSeries series = new DataSeries(ds, string.Concat(new object[] { ds.Description, "-StochD(", stoPer, ",", smooth, ")" }));
            int count = stoPer + smooth;
            if (count >= ds.Count)
            {
                count = ds.Count;
            }
            for (int i = 0; i < count; i++)
            {
                series[i] = 50.0;
            }
            for (int j = count; j < ds.Count; j++)
            {
                double num4 = 0.0;
                double num5 = 0.0;
                for (int k = 0; k < smooth; k++)
                {
                    double num7 = Lowest.Series(ds, stoPer)[j - k];
                    num4 = (num4 + ds[j - k]) - num7;
                    num5 = (num5 + Highest.Series(ds, stoPer)[j - k]) - num7;
                }
                series[j] = 100.0 * (num4 / (num5 + 0.0001));
            }
            return series;
        }
    }
}

