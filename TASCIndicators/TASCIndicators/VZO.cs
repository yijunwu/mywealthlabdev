namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class VZO : DataSeries
    {
        private Bars bars;
        private int period;

        public VZO(Bars bars, int period, string description) : base(bars, description)
        {
            this.bars = bars;
            this.period = period;
            base.FirstValidValue = period;
            DataSeries source = new DataSeries(bars, "R");
            DataSeries series2 = EMA.Series(bars.Volume, period, EMACalculation.Modern);
            for (int i = period; i < bars.Count; i++)
            {
                source[i] = Math.Sign((double) (bars.Close[i] - bars.Close[i - 1])) * bars.Volume[i];
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

        public static VZO Series(Bars bars, int period)
        {
            string key = string.Concat(new object[] { "VZO(", period, ")" });
            if (bars.Cache.ContainsKey(key))
            {
                return (VZO) bars.Cache[key];
            }
            VZO vzo = new VZO(bars, period, key);
            bars.Cache[key] = vzo;
            return vzo;
        }
    }
}

