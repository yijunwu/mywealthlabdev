namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class BollingerPctB : DataSeries
    {
        private DataSeries _ds;
        private int _period;
        private DataSeries _sd;
        private DataSeries _sma;

        public BollingerPctB(DataSeries ds, int period, StdDevCalculation sdCalc, string description) : base(ds, description)
        {
            this._ds = ds;
            this._period = period;
            base.FirstValidValue = period;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            this._sd = StdDev.Series(ds, period, sdCalc);
            this._sma = SMA.Series(ds, period);
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = (100.0 * ((ds[i] + (2.0 * this._sd[i])) - this._sma[i])) / (4.0 * this._sd[i]);
            }
        }

        public override void CalculatePartialValue()
        {
            if (((this._ds.Count < (this._period - 1)) || double.IsNaN(this._ds.PartialValue)) || (double.IsNaN(this._sd.PartialValue) || double.IsNaN(this._sma.PartialValue)))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                base.PartialValue = (100.0 * ((this._ds.PartialValue + (2.0 * this._sd.PartialValue)) - this._sma.PartialValue)) / (4.0 * this._sd.PartialValue);
            }
        }

        public static BollingerPctB Series(DataSeries ds, int period, StdDevCalculation sdCalc)
        {
            DataSeries series;
            string key = "BBandPercentB(" + ds.Description + "," + period.ToString() + "," + sdCalc.ToString() + ")";
            if (ds.Cache.ContainsKey(key))
            {
                return (BollingerPctB) ds.Cache[key];
            }
            ds.Cache[key] = series = new BollingerPctB(ds, period, sdCalc, key);
            return (BollingerPctB) series;
        }
    }
}

