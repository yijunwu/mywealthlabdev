namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class MidasLower : DataSeries
    {
        private DataSeries _midas;
        private double _swingLowPct;
        private Bars ds;

        public MidasLower(Bars ds, int startBar, int barsToSwingLow, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = startBar;
            if ((base.FirstValidValue > ds.Count) || (base.FirstValidValue < 0))
            {
                base.FirstValidValue = ds.Count;
            }
            else
            {
                this._midas = Midas.Series(ds, startBar);
                int num = startBar + barsToSwingLow;
                if (num >= (ds.Count - 1))
                {
                    this._swingLowPct = 0.0;
                }
                else
                {
                    this._swingLowPct = ds.Low[num] / this._midas[num];
                }
                for (int i = base.FirstValidValue; i < ds.Count; i++)
                {
                    base[i] = this._swingLowPct * this._midas[i];
                }
            }
        }

        public override void CalculatePartialValue()
        {
            base.PartialValue = this._midas.PartialValue * this._swingLowPct;
        }

        public static MidasLower Series(Bars ds, int startBar, int barsToSwingLow)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "MidasLower(", startBar, ",", barsToSwingLow, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (MidasLower) ds.Cache[key];
            }
            ds.Cache[key] = series = new MidasLower(ds, startBar, barsToSwingLow, key);
            return (MidasLower) series;
        }
    }
}

