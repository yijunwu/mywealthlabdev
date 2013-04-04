namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class BollingerPctBSmoothed : DataSeries
    {
        private DataSeries _avg;
        private Bars _ds;
        private EMACalculation _ecType;
        private DataSeries _haC;
        private DataSeries _haOpen;
        private int _period;
        private int _periodSmooth;
        private DataSeries _sd;
        private DataSeries _wma;

        public BollingerPctBSmoothed(Bars ds, int pctbPeriod, int smoothPeriod, StdDevCalculation sdCalc, string description) : base(ds, description)
        {
            this._ds = ds;
            this._period = pctbPeriod;
            this._periodSmooth = smoothPeriod;
            base.FirstValidValue = Math.Max(smoothPeriod, pctbPeriod);
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            if (ds.Count >= 2)
            {
                this._avg = (DataSeries) ((((ds.Open + ds.High) + ds.Low) + ds.Close) / 4.0);
                this._haOpen = new DataSeries(ds, "haOpen");
                this._haC = new DataSeries(ds, "haC");
                this._haOpen[0] = this._avg[0];
                this._haC[0] = this._avg[0];
                for (int i = 1; i < ds.Count; i++)
                {
                    this._haOpen[i] = (this._avg[i - 1] + this._haOpen[i - 1]) / 2.0;
                    this._haC[i] = (((this._avg[i] + this._haOpen[i]) + Math.Max(ds.High[i], this._haOpen[i])) + Math.Min(ds.Low[i], this._haOpen[i])) / 4.0;
                }
                DataSeries series = TEMA.Series(this._haC, this._periodSmooth, this._ecType);
                DataSeries series2 = TEMA.Series(series, this._periodSmooth, this._ecType);
                DataSeries series3 = series - series2;
                DataSeries series4 = series + series3;
                DataSeries series5 = TEMA.Series(series4, this._periodSmooth, this._ecType);
                this._sd = StdDev.Series(series5, this._period, StdDevCalculation.Population);
                this._wma = WMA.Series(series5, this._period);
                for (int j = base.FirstValidValue; j < ds.Count; j++)
                {
                    base[j] = (100.0 * ((series5[j] + (2.0 * this._sd[j])) - this._wma[j])) / (4.0 * this._sd[j]);
                }
            }
        }

        public static BollingerPctBSmoothed Series(Bars ds, int pctbPeriod, int smoothPeriod, StdDevCalculation sdCalc)
        {
            DataSeries series;
            string key = "BollingerPctBSmoothed(" + pctbPeriod.ToString() + "," + sdCalc.ToString() + ")";
            if (ds.Cache.ContainsKey(key))
            {
                return (BollingerPctBSmoothed) ds.Cache[key];
            }
            ds.Cache[key] = series = new BollingerPctBSmoothed(ds, pctbPeriod, smoothPeriod, sdCalc, key);
            return (BollingerPctBSmoothed) series;
        }
    }
}

