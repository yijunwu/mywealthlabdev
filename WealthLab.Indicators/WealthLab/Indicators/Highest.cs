namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class Highest : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public Highest(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = (period - 1) + source.FirstValidValue;
            for (int i = period - 1; i < source.Count; i++)
            {
                double minValue = double.MinValue;
                for (int j = 0; j < period; j++)
                {
                    if (source[i - j] > minValue)
                    {
                        minValue = source[i - j];
                    }
                }
                base[i] = minValue;
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
                for (int i = 0; i < (this.int_1 - 1); i++)
                {
                    if (this.dataSeries_1[num - i] > partialValue)
                    {
                        partialValue = this.dataSeries_1[num - i];
                    }
                }
                base.PartialValue = partialValue;
            }
        }

        public static Highest Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "Highest(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (Highest) source.Cache[key];
            }
            Highest highest = new Highest(source, period, key);
            source.Cache[key] = highest;
            return highest;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            if (int_2 < (period - 1))
            {
                return 0.0;
            }
            double minValue = double.MinValue;
            for (int i = 0; i < period; i++)
            {
                if (source[int_2 - i] > minValue)
                {
                    minValue = source[int_2 - i];
                }
            }
            return minValue;
        }
    }
}

