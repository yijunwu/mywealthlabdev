namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class KeltnerUpper : DataSeries
    {
        private Bars bars_1;
        private int int_1;
        private int int_2;

        public KeltnerUpper(Bars bars, int periodOne, int periodTwo, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = periodOne;
            this.int_2 = periodTwo;
            int num = Math.Max(periodOne, periodTwo);
            base.FirstValidValue = num;
            DataSeries series = bars.High - bars.Low;
            AveragePriceC ec = AveragePriceC.Series(bars);
            SMA sma = SMA.Series(series, periodOne);
            DataSeries series2 = SMA.Series(ec, periodTwo) + sma;
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
            DataSeries series2 = SMA.Series(ec, this.int_2) + sma;
            base.PartialValue = series2.PartialValue;
        }

        public static KeltnerUpper Series(Bars bars, int periodOne, int periodTwo)
        {
            string key = string.Concat(new object[] { "KeltnerUpper(", periodOne, ",", periodTwo, ")" });
            if (bars.Cache.ContainsKey(key))
            {
                return (KeltnerUpper) bars.Cache[key];
            }
            KeltnerUpper upper = new KeltnerUpper(bars, periodOne, periodTwo, key);
            bars.Cache[key] = upper;
            return upper;
        }
    }
}

