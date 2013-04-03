namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class ATRP : DataSeries
    {
        private Bars bars_1;
        private int int_1;

        public ATRP(Bars bars, int period, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = period;
            ATR atr = ATR.Series(bars, period);
            if (period < bars.Count)
            {
                for (int i = period; i < bars.Count; i++)
                {
                    base[i] = (atr[i] * 100.0) / bars.Close[i];
                }
                base.FirstValidValue = period;
            }
        }

        public override void CalculatePartialValue()
        {
            if (((this.bars_1.Count != 0) && (this.bars_1.Close.PartialValue != double.NaN)) && ((this.bars_1.High.PartialValue != double.NaN) && (this.bars_1.Low.PartialValue != double.NaN)))
            {
                ATR atr = ATR.Series(this.bars_1, this.int_1);
                base.PartialValue = (atr.PartialValue * 100.0) / this.bars_1.Close.PartialValue;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static ATRP Series(Bars bars, int period)
        {
            string key = "ATRP(" + period + ")";
            if (bars.Cache.ContainsKey(key))
            {
                return (ATRP) bars.Cache[key];
            }
            ATRP atrp = new ATRP(bars, period, key);
            bars.Cache[key] = atrp;
            return atrp;
        }
    }
}

