namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class CMO : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public CMO(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = period + source.FirstValidValue;
            for (int i = 0; i < source.Count; i++)
            {
                base[i] = Value(i, source, period);
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.dataSeries_1.Count <= (this.int_1 - 1))
            {
                base.PartialValue = double.NaN;
            }
            else if (this.dataSeries_1.PartialValue == double.NaN)
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                int num2 = this.dataSeries_1.Count - 1;
                double num = 0.0;
                double num4 = 0.0;
                for (int i = 0; i < (this.int_1 - 1); i++)
                {
                    if (this.dataSeries_1[num2 - i] > this.dataSeries_1[(num2 - i) - 1])
                    {
                        num += this.dataSeries_1[num2 - i] - this.dataSeries_1[(num2 - i) - 1];
                    }
                    else
                    {
                        num4 += this.dataSeries_1[(num2 - i) - 1] - this.dataSeries_1[num2 - i];
                    }
                }
                if (base.PartialValue > this.dataSeries_1[num2])
                {
                    num += base.PartialValue - this.dataSeries_1[num2];
                }
                else
                {
                    num4 += this.dataSeries_1[num2] - base.PartialValue;
                }
                if ((num + num4) == 0.0)
                {
                    base.PartialValue = 0.0;
                }
                else
                {
                    base.PartialValue = (100.0 * (num - num4)) / (num + num4);
                }
            }
        }

        public static CMO Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "CMO(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (CMO) source.Cache[key];
            }
            CMO cmo = new CMO(source, period, key);
            source.Cache[key] = cmo;
            return cmo;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            if (int_2 <= period)
            {
                return 0.0;
            }
            double num2 = 0.0;
            double num3 = 0.0;
            for (int i = 0; i < period; i++)
            {
                if (source[int_2 - i] > source[(int_2 - i) - 1])
                {
                    num2 += source[int_2 - i] - source[(int_2 - i) - 1];
                }
                else
                {
                    num3 += source[(int_2 - i) - 1] - source[int_2 - i];
                }
            }
            if ((num2 + num3) == 0.0)
            {
                return 0.0;
            }
            return ((100.0 * (num2 - num3)) / (num2 + num3));
        }
    }
}

