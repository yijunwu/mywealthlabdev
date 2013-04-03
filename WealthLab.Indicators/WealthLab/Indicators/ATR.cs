namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class ATR : DataSeries
    {
        private Bars bars_1;
        private int int_1;

        public ATR(Bars bars, int period, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = period;
            WilderMA rma = WilderMA.Series(TrueRange.Series(bars), period);
            for (int i = period; i < bars.Count; i++)
            {
                base[i] = rma[i];
            }
            base.FirstValidValue = period;
        }

        public override void CalculatePartialValue()
        {
            if (((this.bars_1.Count != 0) && (this.bars_1.Close.PartialValue != double.NaN)) && ((this.bars_1.High.PartialValue != double.NaN) && (this.bars_1.Low.PartialValue != double.NaN)))
            {
                WilderMA rma = WilderMA.Series(TrueRange.Series(this.bars_1), this.int_1);
                base.PartialValue = rma.PartialValue;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static ATR Series(Bars bars, int period)
        {
            string key = "ATR(" + period + ")";
            if (bars.Cache.ContainsKey(key))
            {
                return (ATR) bars.Cache[key];
            }
            ATR atr = new ATR(bars, period, key);
            bars.Cache[key] = atr;
            return atr;
        }

        public static double Value(int int_2, Bars bars, int period)
        {
            return Series(bars, period)[int_2];
        }
    }
}

