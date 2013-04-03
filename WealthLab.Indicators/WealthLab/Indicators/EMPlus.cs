namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class EMPlus : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public EMPlus(DataSeries source, int period, string description) : base(source, description)
        {
            base.FirstValidValue = (period * 2) + source.FirstValidValue;
            this.dataSeries_1 = source;
            this.int_1 = period;
            for (int i = 0; i < source.Count; i++)
            {
                base[i] = Value(i, source, period);
            }
        }

        public override void CalculatePartialValue()
        {
            int num = this.dataSeries_1.Count - 1;
            if (num < ((this.int_1 * 2) - 1))
            {
                base.PartialValue = double.NaN;
            }
            else if (this.dataSeries_1.PartialValue == double.NaN)
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                double num3 = 0.0;
                if (this.dataSeries_1.PartialValue >= Highest.Value(num, this.dataSeries_1, this.int_1 - 1))
                {
                    num3 = 1.0;
                }
                for (int i = 0; i < (this.int_1 - 1); i++)
                {
                    if (this.dataSeries_1[num - i] == Highest.Value(num - i, this.dataSeries_1, this.int_1))
                    {
                        num3++;
                    }
                }
                base.PartialValue = (num3 / ((double) this.int_1)) * 100.0;
            }
        }

        public static EMPlus Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "EMPlus(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (EMPlus) source.Cache[key];
            }
            EMPlus plus = new EMPlus(source, period, key);
            source.Cache[key] = plus;
            return plus;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            if (int_2 < (period * 2))
            {
                return 0.0;
            }
            double num2 = 0.0;
            for (int i = 0; i < period; i++)
            {
                if (source[int_2 - i] == Highest.Value(int_2 - i, source, period))
                {
                    num2++;
                }
            }
            return ((num2 / ((double) period)) * 100.0);
        }
    }
}

