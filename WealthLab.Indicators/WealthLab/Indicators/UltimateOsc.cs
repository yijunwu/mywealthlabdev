namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class UltimateOsc : DataSeries
    {
        private Bars bars_1;

        public UltimateOsc(Bars bars, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            for (int i = 0x1c; i < base.Count; i++)
            {
                double num7;
                double num8;
                double num9;
                double num = 0.0;
                double num4 = 0.0;
                for (int j = 0; j <= 6; j++)
                {
                    num += buyingUnits(i - j, bars);
                    num4 += totalActivity(i - j, bars);
                }
                double num2 = 0.0;
                double num5 = 0.0;
                for (int k = 0; k <= 13; k++)
                {
                    num2 += buyingUnits(i - k, bars);
                    num5 += totalActivity(i - k, bars);
                }
                double num3 = 0.0;
                double num6 = 0.0;
                for (int m = 0; m <= 0x1b; m++)
                {
                    num3 += buyingUnits(i - m, bars);
                    num6 += totalActivity(i - m, bars);
                }
                if (num != 0.0)
                {
                    num7 = (num / num4) * 4.0;
                }
                else
                {
                    num7 = 0.0;
                }
                if (num2 != 0.0)
                {
                    num8 = (num2 / num5) * 2.0;
                }
                else
                {
                    num8 = 0.0;
                }
                if (num3 != 0.0)
                {
                    num9 = num3 / num6;
                }
                else
                {
                    num9 = 0.0;
                }
                double num10 = (num7 + num8) + num9;
                if (num10 != 0.0)
                {
                    base[i] = (num10 / 7.0) * 100.0;
                }
                else
                {
                    base[i] = 0.0;
                }
                base.FirstValidValue = 0x1c;
            }
        }

        public static double buyingUnits(int int_1, Bars source)
        {
            return (source.Close[int_1] - trueLow(int_1, source));
        }

        public override void CalculatePartialValue()
        {
            if ((this.bars_1.Count != 0) && (this.bars_1.Close.PartialValue != double.NaN))
            {
                base.PartialValue = base[base.Count - 1] + ((((this.bars_1.Close.PartialValue - this.bars_1.Low.PartialValue) - (this.bars_1.High.PartialValue - this.bars_1.Close.PartialValue)) / (this.bars_1.High.PartialValue - this.bars_1.Low.PartialValue)) * this.bars_1.Volume.PartialValue);
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static UltimateOsc Series(Bars bars)
        {
            string key = "UltimateOsc()";
            if (bars.Cache.ContainsKey(key))
            {
                return (UltimateOsc) bars.Cache[key];
            }
            UltimateOsc osc = new UltimateOsc(bars, key);
            bars.Cache[key] = osc;
            return osc;
        }

        public static double totalActivity(int int_1, Bars source)
        {
            return (trueHigh(int_1, source) - trueLow(int_1, source));
        }

        public static double trueHigh(int int_1, Bars source)
        {
            return Math.Max(source.High[int_1], source.Close[int_1 - 1]);
        }

        public static double trueLow(int int_1, Bars source)
        {
            return Math.Min(source.Low[int_1], source.Close[int_1 - 1]);
        }
    }
}

