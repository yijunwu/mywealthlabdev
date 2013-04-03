namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class LinearReg : DataSeries
    {
        private DataSeries dataSeries_1;
        private double double_1;
        private double double_2;
        private double double_3;
        private double double_4;
        private int int_1;

        public LinearReg(DataSeries dataSeries_2, int period, string description) : base(dataSeries_2, description)
        {
            this.dataSeries_1 = dataSeries_2;
            this.int_1 = period;
            this.double_3 = ((double) period) / 2.0;
            this.double_4 = ((double) (period + 1)) / 3.0;
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
            base[period - 1] = this.double_2 - this.double_1;
            for (int j = period; j < dataSeries_2.Count; j++)
            {
                this.double_2 += ((2.0 * dataSeries_2[j]) - this.double_1) / this.double_4;
                this.double_1 += (dataSeries_2[j] - dataSeries_2[j - period]) / this.double_3;
                base[j] = this.double_2 - this.double_1;
            }
        }

        public override void CalculatePartialValue()
        {
            if (((this.int_1 >= 1) && (this.int_1 <= (this.dataSeries_1.Count + 1))) && (this.dataSeries_1.PartialValue != double.NaN))
            {
                base.PartialValue = ((this.double_2 - this.double_1) + (((2.0 * this.dataSeries_1.PartialValue) - this.double_1) / this.double_4)) - ((this.dataSeries_1.PartialValue - this.dataSeries_1[this.dataSeries_1.Count - this.int_1]) / this.double_3);
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static LinearReg Series(DataSeries dataSeries_2, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "LinearReg(", dataSeries_2.Description, ",", period, ")" });
            if (dataSeries_2.Cache.ContainsKey(key))
            {
                return (LinearReg) dataSeries_2.Cache[key];
            }
            dataSeries_2.Cache[key] = series = new LinearReg(dataSeries_2, period, key);
            return (LinearReg) series;
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
            return ((2.0 * (((3.0 * num2) / ((double) (period + 1))) - num)) / ((double) period));
        }
    }
}

