namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class Volatility : DataSeries
    {
        private Bars bars_1;
        private int int_1;
        private int int_2;

        public Volatility(Bars bars, int emaPeriod, int rocPeriod, string description) : base(bars, description)
        {
            DataSeries series;
            this.bars_1 = bars;
            this.int_1 = emaPeriod;
            this.int_2 = rocPeriod;
            int num = Math.Max(emaPeriod, rocPeriod);
            if (base.Cache.ContainsKey("DiffHighLow"))
            {
                series = base.Cache["DiffHighLow"];
            }
            else
            {
                series = bars.High - bars.Low;
                base.Cache["DiffHighLow"] = series;
            }
            DataSeries series3 = ROC.Series(EMA.Series(series, emaPeriod, EMACalculation.Modern), rocPeriod);
            for (int i = 0; i < base.Count; i++)
            {
                base[i] = series3[i];
            }
            base.FirstValidValue += num + 1;
        }

        public override void CalculatePartialValue()
        {
            if ((this.bars_1.Count != 0) && (this.bars_1.Close.PartialValue != double.NaN))
            {
                DataSeries series;
                int num2 = Math.Max(this.int_1, this.int_2);
                if (base.Cache.ContainsKey("DiffHighLow"))
                {
                    series = base.Cache["DiffHighLow"];
                }
                else
                {
                    series = this.bars_1.High - this.bars_1.Low;
                    base.Cache["DiffHighLow"] = series;
                }
                DataSeries series3 = ROC.Series(EMA.Series(series, this.int_1, EMACalculation.Modern), this.int_2);
                for (int i = 0; i < (base.Count - 1); i++)
                {
                    base.PartialValue = series3[i];
                }
                base.FirstValidValue += num2 + 1;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static Volatility Series(Bars bars, int emaPeriod, int rocPeriod)
        {
            string key = string.Concat(new object[] { "Volatility(", emaPeriod, ",", rocPeriod, ")" });
            if (bars.Cache.ContainsKey(key))
            {
                return (Volatility) bars.Cache[key];
            }
            Volatility volatility = new Volatility(bars, emaPeriod, rocPeriod, key);
            bars.Cache[key] = volatility;
            return volatility;
        }
    }
}

