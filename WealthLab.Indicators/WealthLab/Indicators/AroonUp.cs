namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class AroonUp : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public AroonUp(DataSeries source, int period, string description) : base(source, description)
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
                double minValue = double.MinValue;
                for (int i = this.int_1; i >= 1; i--)
                {
                    if (this.dataSeries_1[(this.dataSeries_1.Count - 1) - i] >= minValue)
                    {
                        num3 = i;
                        minValue = this.dataSeries_1[(this.dataSeries_1.Count - 1) - i];
                    }
                }
                if (this.dataSeries_1.PartialValue >= minValue)
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

        public static AroonUp Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "AroonUp(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (AroonUp) source.Cache[key];
            }
            AroonUp up = new AroonUp(source, period, key);
            source.Cache[key] = up;
            return up;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            if (int_2 < period)
            {
                return 0.0;
            }
            double num3 = 0.0;
            double minValue = double.MinValue;
            for (int i = period; i >= 0; i--)
            {
                if (source[int_2 - i] >= minValue)
                {
                    num3 = i;
                    minValue = source[int_2 - i];
                }
            }
            return (((period - num3) / ((double) period)) * 100.0);
        }
    }
}

