namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class CumUp : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public CumUp(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = period + source.FirstValidValue;
            int num = 0;
            for (int i = 0; i < source.Count; i++)
            {
                if (i >= period)
                {
                    if (source[i] > source[i - period])
                    {
                        num++;
                    }
                    else
                    {
                        num = 0;
                    }
                }
                base[i] = num;
            }
        }

        public override void CalculatePartialValue()
        {
            int num = this.dataSeries_1.Count - 1;
            if ((num < (this.int_1 - 1)) || (this.dataSeries_1.PartialValue == double.NaN))
            {
                base.PartialValue = double.NaN;
            }
            else if (this.dataSeries_1.PartialValue <= this.dataSeries_1[(num - this.int_1) + 1])
            {
                base.PartialValue = 0.0;
            }
            else
            {
                int num3 = 1;
                for (int i = num; i > this.int_1; i--)
                {
                    if (this.dataSeries_1[i] <= this.dataSeries_1[i - this.int_1])
                    {
                        base.PartialValue = num3;
                        return;
                    }
                    num3++;
                }
                base.PartialValue = num3;
            }
        }

        public static CumUp Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "CumUp(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (CumUp) source.Cache[key];
            }
            CumUp up = new CumUp(source, period, key);
            source.Cache[key] = up;
            return up;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            if (int_2 < period)
            {
                return 0.0;
            }
            int num = 0;
            for (int i = int_2; i > period; i--)
            {
                if (source[i] <= source[i - period])
                {
                    return (double) num;
                }
                num++;
            }
            return (double) num;
        }
    }
}

