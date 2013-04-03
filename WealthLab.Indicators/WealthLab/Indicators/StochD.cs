namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class StochD : DataSeries
    {
        private Bars bars_1;
        private int int_1;
        private int int_2;

        public StochD(Bars bars, int period, int smooth, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = smooth;
            this.int_2 = period;
            base.FirstValidValue += period + smooth;
            int num = Math.Min(bars.Count, period + smooth);
            for (int i = 0; i < num; i++)
            {
                base[i] = 50.0;
            }
            for (int j = period + smooth; j < bars.Count; j++)
            {
                double num4 = 0.0;
                double num5 = 0.0;
                for (int k = 0; k < smooth; k++)
                {
                    num4 += bars.Close[j - k] - Lowest.Value(j - k, bars.Low, period);
                    num5 += Highest.Value(j - k, bars.High, period) - Lowest.Value(j - k, bars.Low, period);
                }
                if (num5 != 0.0)
                {
                    base[j] = 100.0 * (num4 / num5);
                }
            }
        }

        public override void CalculatePartialValue()
        {
            int count = this.bars_1.Count;
            if ((count >= this.int_1) && (this.bars_1.Close.PartialValue != double.NaN))
            {
                int period = this.int_2 - 1;
                double num6 = Math.Min(this.bars_1.Low.PartialValue, Lowest.Value(count - 1, this.bars_1.Low, period));
                double num7 = Math.Max(this.bars_1.High.PartialValue, Highest.Value(count - 1, this.bars_1.High, period));
                double num4 = this.bars_1.Close.PartialValue - num6;
                double num3 = num7 - num6;
                for (int i = 1; i < this.int_1; i++)
                {
                    num4 += this.bars_1.Close[count - i] - Lowest.Value(count - i, this.bars_1.Low, this.int_2);
                    num3 += Highest.Value(count - i, this.bars_1.High, this.int_2) - Lowest.Value(count - i, this.bars_1.Low, this.int_2);
                }
                if (num3 != 0.0)
                {
                    base.PartialValue = 100.0 * (num4 / num3);
                }
                else
                {
                    base.PartialValue = 0.0;
                }
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static StochD Series(Bars bars, int period, int smooth)
        {
            string key = string.Concat(new object[] { "StochD(", period, ", ", smooth, ")" });
            if (bars.Cache.ContainsKey(key))
            {
                return (StochD) bars.Cache[key];
            }
            StochD hd = new StochD(bars, period, smooth, key);
            bars.Cache[key] = hd;
            return hd;
        }
    }
}

