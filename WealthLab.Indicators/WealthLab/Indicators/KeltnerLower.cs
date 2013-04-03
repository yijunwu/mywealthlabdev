namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class KeltnerLower : DataSeries
    {
        private Bars bars_1;
        private int int_1;
        private int int_2;

        public KeltnerLower(Bars bars, int periodOne, int periodTwo, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = periodOne;
            this.int_2 = periodTwo;
            int num = Math.Max(periodOne, periodTwo);
            base.FirstValidValue = num;
            DataSeries series = bars.High - bars.Low;
            AveragePriceC ec = AveragePriceC.Series(bars);
            SMA sma = SMA.Series(series, periodOne);
            DataSeries series2 = SMA.Series(ec, periodTwo) - sma;
            if (base.FirstValidValue != -1)
            {
                for (int i = base.FirstValidValue; i < bars.Count; i++)
                {
                    base[i] = series2[i];
                }
            }
        }

        public override void CalculatePartialValue()
        {
            Math.Max(this.int_1, this.int_2);
            DataSeries series = this.bars_1.High - this.bars_1.Low;
            AveragePriceC ec = AveragePriceC.Series(this.bars_1);
            SMA sma = SMA.Series(series, this.int_1);
            DataSeries series2 = SMA.Series(ec, this.int_2) - sma;
            base.PartialValue = series2.PartialValue;
        }

        public static KeltnerLower Series(Bars bars, int periodOne, int periodTwo)
        {
            string key = string.Concat(new object[] { "KeltnerLower(", periodOne, ",", periodTwo, ")" });
            if (bars.Cache.ContainsKey(key))
            {
                return (KeltnerLower) bars.Cache[key];
            }
            KeltnerLower lower = new KeltnerLower(bars, periodOne, periodTwo, key);
            bars.Cache[key] = lower;
            return lower;
        }
    }
}

