namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class AroonDown : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public AroonDown(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = period + source.FirstValidValue;
            for (int i = period + 1; i < source.Count; i++)
            {
                base[i] = Value(i, source, period);
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.dataSeries_1.PartialValue != double.NaN) && (this.dataSeries_1.Count >= this.int_1))
            {
                double num3 = 0.0;
                double maxValue = double.MaxValue;
                for (int i = this.int_1; i >= 1; i--)
                {
                    if (this.dataSeries_1[(this.dataSeries_1.Count - 1) - i] <= maxValue)
                    {
                        num3 = i;
                        maxValue = this.dataSeries_1[(this.dataSeries_1.Count - 1) - i];
                    }
                }
                if (this.dataSeries_1.PartialValue <= maxValue)
                {
                    num3 = 0.0;
                }
                base.PartialValue = ((this.int_1 - num3) / ((double) this.int_1)) * 100.0;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static AroonDown Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "AroonDown(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (AroonDown) source.Cache[key];
            }
            AroonDown down = new AroonDown(source, period, key);
            source.Cache[key] = down;
            return down;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            if (int_2 < period)
            {
                return 0.0;
            }
            double num3 = 0.0;
            double maxValue = double.MaxValue;
            for (int i = period; i >= 0; i--)
            {
                if (source[int_2 - i] <= maxValue)
                {
                    num3 = i;
                    maxValue = source[int_2 - i];
                }
            }
            return (((period - num3) / ((double) period)) * 100.0);
        }
    }
}

