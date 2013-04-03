namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class LinearRegSlope : DataSeries
    {
        private DataSeries dataSeries_1;
        private double double_1;
        private double double_2;
        private double double_3;
        private double double_4;
        private int int_1;

        public LinearRegSlope(DataSeries dataSeries_2, int period, string description) : base(dataSeries_2, description)
        {
            this.dataSeries_1 = dataSeries_2;
            this.int_1 = period;
            this.double_4 = ((double) (period + 1)) / 3.0;
            this.double_3 = ((double) period) / 2.0;
            if ((period < 3) || (period > (dataSeries_2.Count + 1)))
            {
                period = dataSeries_2.Count;
            }
            base.FirstValidValue = (dataSeries_2.FirstValidValue + period) - 1;
            this.double_2 = 3.0 * dataSeries_2[0];
            this.double_1 = 2.0 * dataSeries_2[0];
            for (int i = 0; i < (period - 1); i++)
            {
                this.double_2 += ((2.0 * dataSeries_2[i]) - this.double_1) / this.double_4;
                this.double_1 += (dataSeries_2[i] - dataSeries_2[0]) / this.double_3;
            }
            this.double_2 += ((2.0 * dataSeries_2[period - 1]) - this.double_1) / this.double_4;
            this.double_1 += (dataSeries_2[period - 1] - dataSeries_2[0]) / this.double_3;
            base[period - 1] = ((2.0 * this.double_2) - (3.0 * this.double_1)) / ((double) (period - 1));
            for (int j = period; j < dataSeries_2.Count; j++)
            {
                this.double_2 += ((2.0 * dataSeries_2[j]) - this.double_1) / this.double_4;
                this.double_1 += (dataSeries_2[j] - dataSeries_2[j - period]) / this.double_3;
                base[j] = ((2.0 * this.double_2) - (3.0 * this.double_1)) / ((double) (period - 1));
            }
        }

        public override void CalculatePartialValue()
        {
            this.dataSeries_1.CalculatePartialValue();
            if (((this.int_1 >= 1) && (this.int_1 <= (this.dataSeries_1.Count + 1))) && (this.dataSeries_1.PartialValue != double.NaN))
            {
                double num = this.double_2 + (((2.0 * this.dataSeries_1.PartialValue) - this.double_1) / this.double_4);
                double num2 = this.double_1 + ((this.dataSeries_1.PartialValue - this.dataSeries_1[this.dataSeries_1.Count - this.int_1]) / this.double_3);
                base.PartialValue = ((2.0 * num) - (3.0 * num2)) / ((double) (this.int_1 - 1));
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static LinearRegSlope Series(DataSeries dataSeries_2, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "LinearRegSlope(", dataSeries_2.Description, ",", period, ")" });
            if (dataSeries_2.Cache.ContainsKey(key))
            {
                return (LinearRegSlope) dataSeries_2.Cache[key];
            }
            dataSeries_2.Cache[key] = series = new LinearRegSlope(dataSeries_2, period, key);
            return (LinearRegSlope) series;
        }

        public static double Value(int int_2, DataSeries dataSeries_2, int period)
        {
            if ((period < 1) || (period > (int_2 + 1)))
            {
                return 0.0;
            }
            int num3 = period;
            double num = dataSeries_2[int_2];
            double num2 = dataSeries_2[int_2] * num3;
            while (--num3 > 0)
            {
                num += dataSeries_2[--int_2];
                num2 += dataSeries_2[int_2] * num3;
            }
            return ((6.0 * (((2.0 * num2) / ((double) (period + 1))) - num)) / ((double) (period * (period - 1))));
        }
    }
}

