namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class CMF : DataSeries
    {
        private Bars bars_1;
        private int int_1;

        public CMF(Bars bars, int period, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = period;
            base.FirstValidValue = period;
            for (int i = base.FirstValidValue; i < bars.Count; i++)
            {
                double num2 = 0.0;
                double num3 = 0.0;
                for (int j = i; j > (i - period); j--)
                {
                    if (bars.High[j] != bars.Low[j])
                    {
                        num2 += (((bars.Close[j] - bars.Low[j]) - (bars.High[j] - bars.Close[j])) / (bars.High[j] - bars.Low[j])) * bars.Volume[j];
                    }
                    num3 += bars.Volume[j];
                }
                if (num3 != 0.0)
                {
                    base[i] = num2 / num3;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if ((((this.bars_1.Count != 0) && (this.bars_1.Close.PartialValue != double.NaN)) && ((this.bars_1.Low.PartialValue != double.NaN) && (this.bars_1.High.PartialValue != double.NaN))) && (this.bars_1.Volume.PartialValue != double.NaN))
            {
                double num2 = 0.0;
                double num = 0.0;
                if (this.bars_1.High.PartialValue != this.bars_1.Low.PartialValue)
                {
                    num2 += (((this.bars_1.Close.PartialValue - this.bars_1.Low.PartialValue) - (this.bars_1.High.PartialValue - this.bars_1.Close.PartialValue)) / (this.bars_1.High.PartialValue - this.bars_1.Low.PartialValue)) * this.bars_1.Volume.PartialValue;
                }
                num += this.bars_1.Volume.PartialValue;
                if (num != 0.0)
                {
                    base.PartialValue = num2 / num;
                }
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static CMF Series(Bars bars, int period)
        {
            string key = "CMF(" + period + ")";
            if (bars.Cache.ContainsKey(key))
            {
                return (CMF) bars.Cache[key];
            }
            CMF cmf = new CMF(bars, period, key);
            bars.Cache[key] = cmf;
            return cmf;
        }
    }
}

