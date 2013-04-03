namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class MFI : DataSeries
    {
        private Bars bars_1;

        public MFI(Bars bars, int period, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            base.FirstValidValue += period;
            for (int i = period; i < bars.Count; i++)
            {
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                double num5 = 0.0;
                double num6 = 0.0;
                for (int j = i; j > (i - period); j--)
                {
                    num2 = ((bars.High[j] + bars.Low[j]) + bars.Close[j]) / 3.0;
                    num3 = ((bars.High[j - 1] + bars.Low[j - 1]) + bars.Close[j - 1]) / 3.0;
                    num4 = num2 * bars.Volume[j];
                    if (num2 > num3)
                    {
                        num5 += num4;
                    }
                    else if (num2 < num3)
                    {
                        num6 += num4;
                    }
                }
                base[i] = 100.0 - (100.0 / (1.0 + (num5 / num6)));
            }
        }

        public override void CalculatePartialValue()
        {
            if ((((this.bars_1.Count != 0) && (this.bars_1.Close.PartialValue != double.NaN)) && ((this.bars_1.Low.PartialValue != double.NaN) && (this.bars_1.High.PartialValue != double.NaN))) && (this.bars_1.Volume.PartialValue != double.NaN))
            {
                double num = 0.0;
                double num2 = 0.0;
                double num4 = 0.0;
                double num5 = 0.0;
                double num3 = 0.0;
                num = ((this.bars_1.High.PartialValue + this.bars_1.Low.PartialValue) + this.bars_1.Close.PartialValue) / 3.0;
                num2 = (((((this.bars_1.High.PartialValue - 1.0) + this.bars_1.Low.PartialValue) - 1.0) + this.bars_1.Close.PartialValue) - 1.0) / 3.0;
                num4 = num * this.bars_1.Volume.PartialValue;
                if (num > num2)
                {
                    num5 += num4;
                }
                else if (num < num2)
                {
                    num3 += num4;
                }
                base.PartialValue = 100.0 - (100.0 / (1.0 + (num5 / num3)));
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static MFI Series(Bars bars, int period)
        {
            string key = "MFI(" + period + ")";
            if (bars.Cache.ContainsKey(key))
            {
                return (MFI) bars.Cache[key];
            }
            MFI mfi = new MFI(bars, period, key);
            bars.Cache[key] = mfi;
            return mfi;
        }
    }
}

