namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class PZO : DataSeries
    {
        private Bars bars;
        private int period;

        public PZO(Bars bars, int period, string description) : base(bars, description)
        {
            this.bars = bars;
            this.period = period;
            base.FirstValidValue = period;
            DataSeries source = new DataSeries(bars, "R");
            DataSeries series2 = EMA.Series(bars.Close, period, EMACalculation.Modern);
            for (int i = period; i < bars.Count; i++)
            {
                source[i] = Math.Sign((double) (bars.Close[i] - bars.Close[i - 1])) * bars.Close[i];
            }
            DataSeries series3 = EMA.Series(source, period, EMACalculation.Modern);
            for (int j = period; j < bars.Count; j++)
            {
                if (series2[j] != 0.0)
                {
                    base[j] = (100.0 * series3[j]) / series2[j];
                }
            }
        }

        public static PZO Series(Bars bars, int period)
        {
            string key = string.Concat(new object[] { "PZO(", period, ")" });
            if (bars.Cache.ContainsKey(key))
            {
                return (PZO) bars.Cache[key];
            }
            PZO pzo = new PZO(bars, period, key);
            bars.Cache[key] = pzo;
            return pzo;
        }
    }
}

