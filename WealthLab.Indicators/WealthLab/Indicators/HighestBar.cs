namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class HighestBar : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public HighestBar(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = period + source.FirstValidValue;
            int num2 = 0;
            for (int i = period - 1; i < source.Count; i++)
            {
                double minValue = double.MinValue;
                for (int j = 0; j < period; j++)
                {
                    if (source[i - j] > minValue)
                    {
                        minValue = source[i - j];
                        num2 = i - j;
                    }
                }
                base[i] = num2;
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.dataSeries_1.PartialValue == double.NaN)
            {
                base.PartialValue = double.NaN;
            }
            else if (this.dataSeries_1.Count < (this.int_1 - 1))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                int num = this.dataSeries_1.Count - 1;
                double partialValue = this.dataSeries_1.PartialValue;
                int num4 = 0;
                for (int i = 0; i < (this.int_1 - 1); i++)
                {
                    if (this.dataSeries_1[num - i] > partialValue)
                    {
                        partialValue = this.dataSeries_1[num - i];
                        num4 = num - i;
                    }
                }
                base.PartialValue = num4;
            }
        }

        public static HighestBar Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "HighestBar(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (HighestBar) source.Cache[key];
            }
            HighestBar bar = new HighestBar(source, period, key);
            source.Cache[key] = bar;
            return bar;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            if (int_2 < (period - 1))
            {
                return 0.0;
            }
            double minValue = double.MinValue;
            int num3 = 0;
            for (int i = 0; i < period; i++)
            {
                if (source[int_2 - i] > minValue)
                {
                    minValue = source[int_2 - i];
                    num3 = int_2 - i;
                }
            }
            return (double) num3;
        }
    }
}

