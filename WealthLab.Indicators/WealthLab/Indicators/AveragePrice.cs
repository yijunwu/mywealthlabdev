namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class AveragePrice : DataSeries
    {
        private Bars bars_1;
        private const int int_1 = 2;

        public AveragePrice(Bars bars, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            for (int i = 0; i < bars.Count; i++)
            {
                base[i] = (bars.High[i] + bars.Low[i]) / 2.0;
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.bars_1.High.PartialValue != double.NaN)
            {
                base.PartialValue = (this.bars_1.High.PartialValue + this.bars_1.Low.PartialValue) / 2.0;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static AveragePrice Series(Bars bars)
        {
            string key = "AveragePrice()";
            if (bars.Cache.ContainsKey(key))
            {
                return (AveragePrice) bars.Cache[key];
            }
            AveragePrice price = new AveragePrice(bars, key);
            bars.Cache[key] = price;
            return price;
        }
    }
}

