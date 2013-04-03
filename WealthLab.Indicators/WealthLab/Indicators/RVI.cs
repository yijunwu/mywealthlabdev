namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class RVI : DataSeries
    {
        private Bars bars_1;
        private int int_1;

        public RVI(Bars bars, int period, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = period;
            base.FirstValidValue = period;
            DataSeries source = bars.Close - bars.Open;
            DataSeries series2 = bars.High - bars.Low;
            DataSeries series3 = FIR.Series(source, "1, 2, 2, 1");
            DataSeries series4 = FIR.Series(series2, "1, 2, 2, 1");
            for (int i = period; i < bars.Count; i++)
            {
                double num2 = 0.0;
                double num3 = 0.0;
                for (int j = 0; j < period; j++)
                {
                    num2 += series3[i - j];
                    num3 += series4[i - j];
                }
                if (num3 == 0.0)
                {
                    base[i] = 0.0;
                }
                else
                {
                    base[i] = num2 / num3;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.bars_1.Count == 0)
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                DataSeries source = this.bars_1.Close - this.bars_1.Open;
                DataSeries series2 = this.bars_1.High - this.bars_1.Low;
                DataSeries series3 = FIR.Series(source, "1, 2, 2, 1");
                DataSeries series4 = FIR.Series(series2, "1, 2, 2, 1");
                double num = 0.0;
                double num2 = 0.0;
                num += series3[this.bars_1.Count - this.int_1];
                num2 += series4[this.bars_1.Count - this.int_1];
                if (num2 == 0.0)
                {
                    base.PartialValue = 0.0;
                }
                else
                {
                    base.PartialValue = num / num2;
                }
            }
        }

        public static RVI Series(Bars bars, int period)
        {
            string key = "RVI(" + period + ")";
            if (bars.Cache.ContainsKey(key))
            {
                return (RVI) bars.Cache[key];
            }
            RVI rvi = new RVI(bars, period, key);
            bars.Cache[key] = rvi;
            return rvi;
        }
    }
}

