namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class TrueRange : DataSeries
    {
        private Bars bars_1;

        public TrueRange(Bars bars, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            for (int i = 0; i < bars.Count; i++)
            {
                base[i] = Value(i, bars);
            }
        }

        public override void CalculatePartialValue()
        {
            if ((((this.bars_1.Count != 0) && (this.bars_1.Close.PartialValue != double.NaN)) && (this.bars_1.High.PartialValue != double.NaN)) && (this.bars_1.Low.PartialValue != double.NaN))
            {
                int num = this.bars_1.Count - 1;
                double num2 = this.bars_1.High.PartialValue - this.bars_1.Low.PartialValue;
                double num3 = Math.Abs((double) (this.bars_1.Close[num] - this.bars_1.High.PartialValue));
                double num4 = Math.Abs((double) (this.bars_1.Close[num] - this.bars_1.Low.PartialValue));
                double num5 = Math.Max(num2, num3);
                double num6 = Math.Max(num4, num5);
                base.PartialValue = num6;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static TrueRange Series(Bars bars)
        {
            string key = "TrueRange()";
            if (bars.Cache.ContainsKey(key))
            {
                return (TrueRange) bars.Cache[key];
            }
            TrueRange range = new TrueRange(bars, key);
            bars.Cache[key] = range;
            return range;
        }

        public static double Value(int int_1, Bars bars)
        {
            if (int_1 == 0)
            {
                return (bars.High[0] - bars.Low[0]);
            }
            double num = bars.High[int_1] - bars.Low[int_1];
            double num2 = Math.Abs((double) (bars.Close[int_1 - 1] - bars.High[int_1]));
            double num3 = Math.Abs((double) (bars.Close[int_1 - 1] - bars.Low[int_1]));
            double num4 = Math.Max(num, num2);
            return Math.Max(num3, num4);
        }
    }
}

