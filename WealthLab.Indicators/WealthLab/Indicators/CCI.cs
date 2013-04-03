namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class CCI : DataSeries
    {
        private Bars bars_1;
        private int int_1;

        public CCI(Bars bars, int period, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = period;
            base.FirstValidValue = period;
            for (int i = period; i < bars.Count; i++)
            {
                double num2 = ((bars.High[i] + bars.Low[i]) + bars.Close[i]) / 3.0;
                double num3 = 0.0;
                for (int j = i; j > (i - period); j--)
                {
                    num3 += ((bars.High[j] + bars.Low[j]) + bars.Close[j]) / 3.0;
                }
                num3 /= (double) period;
                double num5 = 0.0;
                for (int k = i; k > (i - period); k--)
                {
                    num5 += Math.Abs((double) ((((bars.High[k] + bars.Low[k]) + bars.Close[k]) / 3.0) - num3));
                }
                num5 /= (double) period;
                if (num5 == 0.0)
                {
                    base[i] = 0.0;
                }
                else
                {
                    base[i] = (num2 - num3) / (0.015 * num5);
                }
            }
        }

        public override void CalculatePartialValue()
        {
            double num = ((this.bars_1.High.PartialValue + this.bars_1.Low.PartialValue) + this.bars_1.Close.PartialValue) / 3.0;
            double num2 = 0.0;
            for (int i = (int) base.PartialValue; i < (i - this.int_1); i--)
            {
                num2 += ((this.bars_1.High[i] + this.bars_1.Low[i]) + this.bars_1.Close[i]) / 3.0;
            }
            num2 /= (double) this.int_1;
            double num5 = 0.0;
            for (int j = (int) base.PartialValue; j < (j - this.int_1); j--)
            {
                num5 += Math.Abs((double) ((((this.bars_1.High[j] + this.bars_1.Low[j]) + this.bars_1.Close[j]) / 3.0) - num2));
            }
            num5 /= (double) this.int_1;
            if (num5 == 0.0)
            {
                base.PartialValue = 0.0;
            }
            else
            {
                base.PartialValue = (num - num2) / (0.015 * num5);
            }
        }

        public static CCI Series(Bars bars, int period)
        {
            string key = "CCI(" + period + ")";
            if (bars.Cache.ContainsKey(key))
            {
                return (CCI) bars.Cache[key];
            }
            CCI cci = new CCI(bars, period, key);
            bars.Cache[key] = cci;
            return cci;
        }
    }
}

