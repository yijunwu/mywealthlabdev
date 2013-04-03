namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class VHF : DataSeries
    {
        private DataSeries dataSeries_1;
        private double double_1;
        private double double_2;
        private double double_3;
        private double double_4;
        private int int_1;

        public VHF(DataSeries source, int period, string description) : base(source, description)
        {
            this.double_1 = double.MinValue;
            this.double_2 = double.MaxValue;
            this.dataSeries_1 = source;
            this.int_1 = period;
            if ((period < 1) || (period > (source.Count + 1)))
            {
                period = source.Count + 1;
            }
            for (int i = period + 2; i < base.Count; i++)
            {
                this.double_3 = 0.0;
                this.double_2 = double.MaxValue;
                this.double_1 = double.MinValue;
                for (int j = i; j >= ((i - period) + 1); j--)
                {
                    if (source[j] < this.double_2)
                    {
                        this.double_2 = source[j];
                    }
                    if (source[j] > this.double_1)
                    {
                        this.double_1 = source[j];
                    }
                    this.double_3 += Math.Abs((double) (source[j] - source[j - 1]));
                }
                this.double_4 = this.double_1 - this.double_2;
                if (this.double_3 != 0.0)
                {
                    base[i] = this.double_4 / this.double_3;
                }
                else
                {
                    base[i] = 0.0;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if (((this.int_1 >= 1) && (this.int_1 <= (this.dataSeries_1.Count + 1))) && (this.dataSeries_1.PartialValue != double.NaN))
            {
                base.PartialValue = this.double_4 / this.double_3;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static VHF Series(DataSeries source, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "VHF(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (VHF) source.Cache[key];
            }
            source.Cache[key] = series = new VHF(source, period, key);
            return (VHF) series;
        }
    }
}

