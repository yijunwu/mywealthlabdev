namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class StdError : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public StdError(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = period;
            DataSeries series = LinearReg.Series(source, period);
            DataSeries series2 = LinearRegSlope.Series(source, period);
            for (int i = base.FirstValidValue; i < source.Count; i++)
            {
                double num2 = 0.0;
                for (int j = 0; j < period; j++)
                {
                    double num4 = series[i] - (j * series2[i]);
                    double num5 = source[i - j] - num4;
                    num2 += num5 * num5;
                }
                base[i] = Math.Sqrt(num2 / ((double) (period - 2)));
            }
        }

        public override void CalculatePartialValue()
        {
            DataSeries series = LinearReg.Series(this.dataSeries_1, this.int_1);
            DataSeries series2 = LinearRegSlope.Series(this.dataSeries_1, this.int_1);
            double num = 0.0;
            for (int i = 0; i < this.int_1; i++)
            {
                double num3 = series.PartialValue - (i * series2.PartialValue);
                double num4 = (this.dataSeries_1.PartialValue - i) - num3;
                num += num4 * num4;
            }
            base.PartialValue = Math.Sqrt(num / ((double) (this.int_1 - 2)));
        }

        public static StdError Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "StdError(", source.Description, ", ", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (StdError) source.Cache[key];
            }
            StdError error = new StdError(source, period, key);
            source.Cache[key] = error;
            return error;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            DataSeries series = LinearReg.Series(source, period);
            DataSeries series2 = LinearRegSlope.Series(source, period);
            double num = 0.0;
            for (int i = 0; i < period; i++)
            {
                double num3 = series[int_2] - (i * series2[int_2]);
                double num4 = source[int_2 - i] - num3;
                num += num4 * num4;
            }
            return Math.Sqrt(num / ((double) (period - 2)));
        }
    }
}

