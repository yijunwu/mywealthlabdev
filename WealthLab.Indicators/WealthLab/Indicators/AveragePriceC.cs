namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class AveragePriceC : DataSeries
    {
        private Bars bars_1;
        private const int int_1 = 3;

        public AveragePriceC(Bars bars, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            for (int i = 0; i < bars.Count; i++)
            {
                base[i] = ((bars.High[i] + bars.Low[i]) + bars.Close[i]) / 3.0;
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.bars_1.High.PartialValue != double.NaN)
            {
                base.PartialValue = ((this.bars_1.High.PartialValue + this.bars_1.Low.PartialValue) + this.bars_1.Close.PartialValue) / 3.0;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static AveragePriceC Series(Bars bars)
        {
            string key = "AveragePriceC()";
            if (bars.Cache.ContainsKey(key))
            {
                return (AveragePriceC) bars.Cache[key];
            }
            AveragePriceC ec = new AveragePriceC(bars, key);
            bars.Cache[key] = ec;
            return ec;
        }
    }
}

