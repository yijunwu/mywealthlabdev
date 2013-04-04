namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class MidasUpper : DataSeries
    {
        private DataSeries _midas;
        private double _swingHighPct;
        private Bars ds;

        public MidasUpper(Bars ds, int startBar, int barsToSwingHigh, string description) : base(ds, description)
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
                int num = startBar + barsToSwingHigh;
                if (num >= (ds.Count - 1))
                {
                    this._swingHighPct = 0.0;
                }
                else
                {
                    this._swingHighPct = ds.High[num] / this._midas[num];
                }
                for (int i = base.FirstValidValue; i < ds.Count; i++)
                {
                    base[i] = this._swingHighPct * this._midas[i];
                }
            }
        }

        public override void CalculatePartialValue()
        {
            base.PartialValue = this._midas.PartialValue * this._swingHighPct;
        }

        public static MidasUpper Series(Bars ds, int startBar, int barsToSwingHigh)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "MidasUpper(", startBar, ",", barsToSwingHigh, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (MidasUpper) ds.Cache[key];
            }
            ds.Cache[key] = series = new MidasUpper(ds, startBar, barsToSwingHigh, key);
            return (MidasUpper) series;
        }
    }
}

