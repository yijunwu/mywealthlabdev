namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class DSS : DataSeries
    {
        private Bars bars_1;
        private int int_1;
        private int int_2;
        private int int_3;

        public DSS(Bars bars, int period1, int period2, int stochP, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = period1;
            this.int_2 = period2;
            this.int_3 = stochP;
            base.FirstValidValue = Math.Max(period1, period2);
            Highest highest = Highest.Series(bars.High, stochP);
            Lowest lowest = Lowest.Series(bars.Low, stochP);
            DataSeries source = highest - lowest;
            DataSeries series2 = bars.Close - lowest;
            EMA ema = EMA.Series(series2, period2, EMACalculation.Modern);
            EMA ema2 = EMA.Series(source, period2, EMACalculation.Modern);
            EMA ema3 = EMA.Series(ema, period1, EMACalculation.Modern);
            EMA ema4 = EMA.Series(ema2, period1, EMACalculation.Modern);
            for (int i = base.FirstValidValue; i < bars.Count; i++)
            {
                double num2 = (ema3[i] / ema4[i]) * 100.0;
                if (num2 != 0.0)
                {
                    base[i] = num2;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            Highest highest = Highest.Series(this.bars_1.High, this.int_3);
            Lowest lowest = Lowest.Series(this.bars_1.Low, this.int_3);
            DataSeries source = highest - lowest;
            DataSeries series2 = this.bars_1.Close - lowest;
            EMA ema = new EMA(series2, this.int_2, EMACalculation.Modern, "");
            EMA ema2 = new EMA(source, this.int_2, EMACalculation.Modern, "");
            EMA ema3 = new EMA(ema, this.int_1, EMACalculation.Modern, "");
            EMA ema4 = new EMA(ema2, this.int_1, EMACalculation.Modern, "");
            double num = (ema3.PartialValue / ema4.PartialValue) * 100.0;
            if (num != 0.0)
            {
                base.PartialValue = num;
            }
        }

        public static DSS Series(Bars bars, int period1, int period2, int stochP)
        {
            string key = string.Concat(new object[] { "DSS(", period1, ", ", period2, ", ", stochP, ")" });
            if (bars.Cache.ContainsKey(key))
            {
                return (DSS) bars.Cache[key];
            }
            DSS dss = new DSS(bars, period1, period2, stochP, key);
            bars.Cache[key] = dss;
            return dss;
        }
    }
}

