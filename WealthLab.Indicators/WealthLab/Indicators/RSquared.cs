namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class RSquared : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public RSquared(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = period;
            WMA wma = WMA.Series(source, period);
            SMA sma = SMA.Series(source, period);
            StdDev dev = StdDev.Series(source, period, StdDevCalculation.Population);
            DataSeries series = wma - sma;
            DataSeries series2 = series / dev;
            DataSeries series3 = series2 * series2;
            for (int i = base.FirstValidValue; i < source.Count; i++)
            {
                if (dev[i] != 0.0)
                {
                    base[i] = ((3.0 * (period + 1.0)) / (period - 1.0)) * series3[i];
                }
                else
                {
                    base[i] = 1.0;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            WMA wma = WMA.Series(this.dataSeries_1, this.int_1);
            SMA sma = SMA.Series(this.dataSeries_1, this.int_1);
            StdDev dev = StdDev.Series(this.dataSeries_1, this.int_1, StdDevCalculation.Population);
            DataSeries series = wma - sma;
            DataSeries series2 = series / dev;
            DataSeries series3 = series2 * series2;
            if (dev.PartialValue != 0.0)
            {
                base.PartialValue = ((3.0 * (this.int_1 + 1.0)) / (this.int_1 - 1.0)) * series3.PartialValue;
            }
            else
            {
                base.PartialValue = 1.0;
            }
        }

        public static RSquared Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "RSquared(", source.Description, ", ", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (RSquared) source.Cache[key];
            }
            RSquared squared = new RSquared(source, period, key);
            source.Cache[key] = squared;
            return squared;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            WMA wma = WMA.Series(source, period);
            SMA sma = SMA.Series(source, period);
            StdDev dev = StdDev.Series(source, period, StdDevCalculation.Population);
            DataSeries series = wma - sma;
            DataSeries series2 = series / dev;
            DataSeries series3 = series2 * series2;
            if (dev[int_2] != 0.0)
            {
                return (((3.0 * (period + 1.0)) / (period - 1.0)) * series3[int_2]);
            }
            return 1.0;
        }
    }
}

