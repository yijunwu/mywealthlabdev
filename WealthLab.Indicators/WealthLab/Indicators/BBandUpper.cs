namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class BBandUpper : DataSeries
    {
        private DataSeries dataSeries_1;
        private double double_1;
        private int int_1;

        public BBandUpper(DataSeries source, int period, double stdDevs, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            this.double_1 = stdDevs;
            base.FirstValidValue = period + source.FirstValidValue;
            SMA sma = SMA.Series(source, period);
            StdDev dev = StdDev.Series(source, period, StdDevCalculation.Sample);
            for (int i = period - 1; i < source.Count; i++)
            {
                base[i] = sma[i] + (stdDevs * dev[i]);
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.dataSeries_1.PartialValue == double.NaN)
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                SMA sma = SMA.Series(this.dataSeries_1, this.int_1);
                if (sma.PartialValue == double.NaN)
                {
                    base.PartialValue = double.NaN;
                }
                else
                {
                    StdDev dev = StdDev.Series(this.dataSeries_1, this.int_1, StdDevCalculation.Sample);
                    if (dev.PartialValue == double.NaN)
                    {
                        base.PartialValue = double.NaN;
                    }
                    else
                    {
                        base.PartialValue = sma.PartialValue + (this.double_1 * dev.PartialValue);
                    }
                }
            }
        }

        public static BBandUpper Series(DataSeries source, int period, double stdDevs)
        {
            string key = string.Concat(new object[] { "BBandUpper(", source.Description, ",", period, ",", stdDevs, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (BBandUpper) source.Cache[key];
            }
            BBandUpper upper = new BBandUpper(source, period, stdDevs, key);
            source.Cache[key] = upper;
            return upper;
        }

        public static double Value(int int_2, DataSeries source, int period, double stdDevs)
        {
            if (int_2 < (period - 1))
            {
                return 0.0;
            }
            return (SMA.Value(int_2, source, period) + (stdDevs * StdDev.Value(int_2, source, period, StdDevCalculation.Sample)));
        }
    }
}

