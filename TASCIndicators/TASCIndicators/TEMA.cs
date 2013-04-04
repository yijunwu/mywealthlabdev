namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class TEMA : DataSeries
    {
        private DataSeries ds;
        private DataSeries ema1;
        private DataSeries ema2;
        private DataSeries ema3;

        public TEMA(DataSeries ds, int period, EMACalculation calcType, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = ds.FirstValidValue + 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            this.ema1 = EMA.Series(ds, period, calcType);
            this.ema2 = EMA.Series(this.ema1, period, calcType);
            this.ema3 = EMA.Series(this.ema2, period, calcType);
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = ((3.0 * this.ema1[i]) - (3.0 * this.ema2[i])) + this.ema3[i];
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.ds.Count < base.FirstValidValue)
            {
                base.PartialValue = 0.0;
            }
            else
            {
                this.ds.CalculatePartialValue();
                if (double.IsNaN(this.ds.PartialValue))
                {
                    base.PartialValue = double.NaN;
                }
                else
                {
                    base.PartialValue = ((3.0 * this.ema1.PartialValue) - (3.0 * this.ema2.PartialValue)) + this.ema3.PartialValue;
                }
            }
        }

        public static TEMA Series(DataSeries ds, int period, EMACalculation calcType)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "TEMA(", ds.Description, ",", period, ",", calcType.ToString(), ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (TEMA) ds.Cache[key];
            }
            ds.Cache[key] = series = new TEMA(ds, period, calcType, key);
            return (TEMA) series;
        }
    }
}

