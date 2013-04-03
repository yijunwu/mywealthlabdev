namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class ROC : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public ROC(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = period + source.FirstValidValue;
            for (int i = period; i < source.Count; i++)
            {
                double num = source[i - period];
                if (num != 0.0)
                {
                    base[i] = ((source[i] / num) * 100.0) - 100.0;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.dataSeries_1.PartialValue == double.NaN)
            {
                base.PartialValue = double.NaN;
            }
            else if (this.int_1 < this.dataSeries_1.Count)
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                base.PartialValue = ((this.dataSeries_1.PartialValue / this.dataSeries_1[this.dataSeries_1.Count - this.int_1]) * 100.0) - 100.0;
            }
        }

        public static double GetSeriesValue(int int_2, DataSeries series)
        {
            if (int_2 < 0)
            {
                return series[0];
            }
            return series[int_2];
        }

        public static ROC Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "ROC(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (ROC) source.Cache[key];
            }
            ROC roc = new ROC(source, period, key);
            source.Cache[key] = roc;
            return roc;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            if (period > int_2)
            {
                return 0.0;
            }
            return (((source[int_2] / source[int_2 - period]) * 100.0) - 100.0);
        }
    }
}

