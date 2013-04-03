namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class WMA : DataSeries
    {
        private DataSeries dataSeries_1;
        private double double_1;
        private double double_2;
        private int int_1;
        private int int_2;

        public WMA(DataSeries dataSeries_2, int period, string description) : base(dataSeries_2, description)
        {
            this.dataSeries_1 = dataSeries_2;
            this.int_1 = period;
            if ((period < 1) || (period > (dataSeries_2.Count + 1)))
            {
                period = dataSeries_2.Count + 1;
            }
            base.FirstValidValue = (dataSeries_2.FirstValidValue + period) - 1;
            this.double_2 = 0.0;
            this.double_1 = 0.0;
            for (int i = 0; i < (period - 1); i++)
            {
                this.double_2 += dataSeries_2[i] * (i + 1);
                this.double_1 += dataSeries_2[i];
            }
            this.int_2 = (period * (period + 1)) / 2;
            for (int j = period - 1; j < dataSeries_2.Count; j++)
            {
                this.double_2 += dataSeries_2[j] * period;
                this.double_1 += dataSeries_2[j];
                base[j] = this.double_2 / ((double) this.int_2);
                this.double_2 -= this.double_1;
                this.double_1 -= dataSeries_2[(j - period) + 1];
            }
        }

        public override void CalculatePartialValue()
        {
            if (((this.int_1 >= 1) && (this.int_1 <= (this.dataSeries_1.Count + 1))) && (this.dataSeries_1.PartialValue != double.NaN))
            {
                base.PartialValue = (this.double_2 + (this.dataSeries_1.PartialValue * this.int_1)) / ((double) this.int_2);
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static WMA Series(DataSeries dataSeries_2, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "WMA(", dataSeries_2.Description, ",", period, ")" });
            if (dataSeries_2.Cache.ContainsKey(key))
            {
                return (WMA) dataSeries_2.Cache[key];
            }
            dataSeries_2.Cache[key] = series = new WMA(dataSeries_2, period, key);
            return (WMA) series;
        }

        public static double Value(int int_3, DataSeries dataSeries_2, int period)
        {
            if ((period < 1) || (period > (int_3 + 1)))
            {
                return 0.0;
            }
            int num3 = (period * (period + 1)) / 2;
            double num = dataSeries_2[int_3] * period;
            int num2 = period;
            while (--num2 > 0)
            {
                num += dataSeries_2[--int_3] * num2;
            }
            return (num / ((double) num3));
        }
    }
}

