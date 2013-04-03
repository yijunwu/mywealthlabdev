namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class KAMA : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public KAMA(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            if ((period < 1) || (period > (source.Count + 1)))
            {
                period = source.Count + 1;
            }
            double num8 = source[period];
            for (int i = period + 1; i < base.Count; i++)
            {
                double num3;
                double num5 = Math.Abs((double) (source[i] - source[i - period]));
                double num6 = 0.0;
                for (int j = 0; j < period; j++)
                {
                    double num4 = Math.Abs((double) (source[i - j] - source[(i - j) - 1]));
                    num6 += num4;
                }
                if (num6 != 0.0)
                {
                    num3 = num5 / num6;
                }
                else
                {
                    num3 = 0.0;
                }
                double num7 = (num3 * 0.60215053763440851) + 0.064516129032258063;
                num7 *= num7;
                num8 += num7 * (source[i] - num8);
                base[i] = num8;
            }
            base.FirstValidValue = 0;
        }

        public override void CalculatePartialValue()
        {
            if (((this.int_1 >= 1) && (this.int_1 <= (this.dataSeries_1.Count + 1))) && (this.dataSeries_1.PartialValue != double.NaN))
            {
                double num5 = this.dataSeries_1[this.int_1];
                for (int i = this.int_1 + 1; i < base.Count; i++)
                {
                    double num3;
                    double num7 = Math.Abs((double) (this.dataSeries_1[i] - this.dataSeries_1[i - this.int_1]));
                    double num2 = 0.0;
                    for (int j = 0; j < this.int_1; j++)
                    {
                        double num8 = Math.Abs((double) (this.dataSeries_1[i - j] - this.dataSeries_1[(i - j) - 1]));
                        num2 += num8;
                    }
                    if (num2 != 0.0)
                    {
                        num3 = num7 / num2;
                    }
                    else
                    {
                        num3 = 0.0;
                    }
                    double num4 = (num3 * 0.0) + 0.0;
                    num4 *= num4;
                    num5 += num4 * (this.dataSeries_1[i] - num5);
                    base.PartialValue = num5;
                }
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static KAMA Series(DataSeries source, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "KAMA(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (KAMA) source.Cache[key];
            }
            source.Cache[key] = series = new KAMA(source, period, key);
            return (KAMA) series;
        }
    }
}

