namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class CADO : DataSeries
    {
        private Bars bars_1;

        public CADO(Bars bars, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            base.FirstValidValue = 20;
            AccumDist source = AccumDist.Series(bars);
            EMA ema = EMA.Series(source, 3, EMACalculation.Modern);
            EMA ema2 = EMA.Series(source, 10, EMACalculation.Modern);
            DataSeries series = ema - ema2;
            for (int i = base.FirstValidValue; i < bars.Count; i++)
            {
                base[i] = series[i];
            }
        }

        public override void CalculatePartialValue()
        {
            AccumDist source = AccumDist.Series(this.bars_1);
            EMA ema = EMA.Series(source, 3, EMACalculation.Modern);
            EMA ema2 = EMA.Series(source, 10, EMACalculation.Modern);
            DataSeries series = ema - ema2;
            base.PartialValue = series.PartialValue;
        }

        public static CADO Series(Bars bars)
        {
            string key = "CADO()";
            if (bars.Cache.ContainsKey(key))
            {
                return (CADO) bars.Cache[key];
            }
            CADO cado = new CADO(bars, key);
            bars.Cache[key] = cado;
            return cado;
        }
    }
}

