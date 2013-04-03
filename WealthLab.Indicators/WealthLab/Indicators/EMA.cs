namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class EMA : DataSeries
    {
        private DataSeries dataSeries_1;
        private double double_1;
        private int int_1;

        public EMA(DataSeries source, int period, EMACalculation calcType, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = (period - 1) + source.FirstValidValue;
            if (period < base.Count)
            {
                double num = SMA.Value(period - 1, source, period);
                base[period - 1] = num;
                if (calcType == EMACalculation.Modern)
                {
                    this.double_1 = 2.0 / (1.0 + period);
                }
                else
                {
                    this.double_1 = (1.0 / ((double) period)) * 2.0;
                }
                for (int i = period; i < source.Count; i++)
                {
                    double num2 = source[i] - num;
                    num2 *= this.double_1;
                    num += num2;
                    base[i] = num;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.dataSeries_1.Count >= this.int_1) && (this.dataSeries_1.PartialValue != double.NaN))
            {
                double num = this.dataSeries_1.PartialValue - base[base.Count - 1];
                num *= this.double_1;
                base.PartialValue = base[base.Count - 1] + num;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static EMA Series(DataSeries source, int period, EMACalculation calcType)
        {
            string key = string.Concat(new object[] { "EMA(", source.Description, ",", period, ",", calcType, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (EMA) source.Cache[key];
            }
            EMA ema = new EMA(source, period, calcType, key);
            source.Cache[key] = ema;
            return ema;
        }
    }
}

