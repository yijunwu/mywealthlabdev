namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class WilliamsR : DataSeries
    {
        private Bars bars_1;
        private int int_1;

        public WilliamsR(Bars bars, int period, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = period;
            for (int i = period; i < base.Count; i++)
            {
                double num;
                double num2 = Lowest.Value(i, bars.Low, period);
                double num3 = Highest.Value(i, bars.High, period);
                if ((num3 - num2) != 0.0)
                {
                    num = (100.0 * (num3 - bars.Close[i])) / (num3 - num2);
                }
                else
                {
                    num = 0.0;
                }
                base[i] = num;
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.bars_1.Count != 0) && (this.bars_1.Close.PartialValue != double.NaN))
            {
                for (int i = this.int_1; i < base.Count; i++)
                {
                    double num;
                    double num3 = Lowest.Value(i, this.bars_1.Low, this.int_1);
                    double num4 = Highest.Value(i, this.bars_1.High, this.int_1);
                    if ((num4 - num3) != 0.0)
                    {
                        num = (100.0 * (num4 - this.bars_1.Close[i])) / (num4 - num3);
                    }
                    else
                    {
                        num = 0.0;
                    }
                    base[i] = num;
                }
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static WilliamsR Series(Bars bars, int period)
        {
            string key = "WilliamsR(" + period + ")";
            if (bars.Cache.ContainsKey(key))
            {
                return (WilliamsR) bars.Cache[key];
            }
            WilliamsR sr = new WilliamsR(bars, period, key);
            bars.Cache[key] = sr;
            return sr;
        }
    }
}

