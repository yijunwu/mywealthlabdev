namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class WilderMA : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public WilderMA(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            double num = 0.0;
            if ((period - 1) < source.Count)
            {
                for (int i = period - 1; i >= 0; i--)
                {
                    num += source[i];
                }
                base[period - 1] = num / ((double) period);
                for (int j = period; j < base.Count; j++)
                {
                    num = (base[j - 1] * (period - 1)) + source[j];
                    base[j] = num / ((double) period);
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.dataSeries_1.Count >= this.int_1) && (this.dataSeries_1.PartialValue != double.NaN))
            {
                double num2 = 0.0;
                for (int i = this.int_1 - 1; i >= 0; i--)
                {
                    num2 += this.dataSeries_1[i];
                }
                base[this.int_1 - 1] = num2 / ((double) this.int_1);
                for (int j = this.int_1; j < base.Count; j++)
                {
                    num2 = (base[j - 1] * (this.int_1 - 1)) + this.dataSeries_1[j];
                    base.PartialValue = num2 / ((double) this.int_1);
                }
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static WilderMA Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "WilderMA(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (WilderMA) source.Cache[key];
            }
            WilderMA rma = new WilderMA(source, period, key);
            source.Cache[key] = rma;
            return rma;
        }
    }
}

