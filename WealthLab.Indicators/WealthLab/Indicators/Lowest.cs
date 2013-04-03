namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class Lowest : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public Lowest(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = (period - 1) + source.FirstValidValue;
            for (int i = period - 1; i < source.Count; i++)
            {
                double maxValue = double.MaxValue;
                for (int j = 0; j < period; j++)
                {
                    if (source[i - j] < maxValue)
                    {
                        maxValue = source[i - j];
                    }
                }
                base[i] = maxValue;
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
                    if (this.dataSeries_1[num - i] < partialValue)
                    {
                        partialValue = this.dataSeries_1[num - i];
                    }
                }
                base.PartialValue = partialValue;
            }
        }

        public static Lowest Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "Lowest(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (Lowest) source.Cache[key];
            }
            Lowest lowest = new Lowest(source, period, key);
            source.Cache[key] = lowest;
            return lowest;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            if (int_2 < (period - 1))
            {
                return 0.0;
            }
            double maxValue = double.MaxValue;
            for (int i = 0; i < period; i++)
            {
                if (source[int_2 - i] < maxValue)
                {
                    maxValue = source[int_2 - i];
                }
            }
            return maxValue;
        }
    }
}

