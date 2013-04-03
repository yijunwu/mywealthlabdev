namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class StdDev : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;
        private StdDevCalculation stdDevCalculation_0;

        public StdDev(DataSeries dataSeries_2, int period, StdDevCalculation calcType, string description) : base(dataSeries_2, description)
        {
            this.dataSeries_1 = dataSeries_2;
            this.int_1 = period;
            this.stdDevCalculation_0 = calcType;
            base.FirstValidValue = period + dataSeries_2.FirstValidValue;
            for (int i = period - 1; i < dataSeries_2.Count; i++)
            {
                base[i] = Value(i, dataSeries_2, period, calcType);
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.dataSeries_1.Count < this.int_1)
            {
                base.PartialValue = double.NaN;
            }
            else if (this.dataSeries_1.PartialValue == double.NaN)
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                double num2 = 0.0;
                double num3 = 0.0;
                for (int i = 0; i < (this.int_1 - 1); i++)
                {
                    int num4 = (this.dataSeries_1.Count - i) - 1;
                    num2 += this.dataSeries_1[num4];
                    num3 += this.dataSeries_1[num4] * this.dataSeries_1[num4];
                }
                num2 += this.dataSeries_1.PartialValue;
                num3 += this.dataSeries_1.PartialValue * this.dataSeries_1.PartialValue;
                if (this.stdDevCalculation_0 == StdDevCalculation.Sample)
                {
                    base.PartialValue = Math.Sqrt((num3 - ((num2 * num2) / ((double) this.int_1))) / ((double) (this.int_1 - 1)));
                }
                else
                {
                    base.PartialValue = Math.Sqrt((num3 - ((num2 * num2) / ((double) this.int_1))) / ((double) this.int_1));
                }
            }
        }

        public static StdDev Series(DataSeries dataSeries_2, int period, StdDevCalculation calcType)
        {
            string key = string.Concat(new object[] { "StdDev(", dataSeries_2.Description, ",", period, ",", calcType.ToString(), ")" });
            if (dataSeries_2.Cache.ContainsKey(key))
            {
                return (StdDev) dataSeries_2.Cache[key];
            }
            StdDev dev = new StdDev(dataSeries_2, period, calcType, key);
            dataSeries_2.Cache[key] = dev;
            return dev;
        }

        public static double Value(int int_2, DataSeries dataSeries_2, int period, StdDevCalculation calcType)
        {
            double num3;
            if (int_2 < (period - 1))
            {
                return 0.0;
            }
            double num2 = 0.0;
            double num = 0.0;
            for (int i = 0; i < period; i++)
            {
                num2 += dataSeries_2[int_2 - i];
                num += dataSeries_2[int_2 - i] * dataSeries_2[int_2 - i];
            }
            if (calcType == StdDevCalculation.Sample)
            {
                num3 = Math.Sqrt((num - ((num2 * num2) / ((double) period))) / ((double) (period - 1)));
            }
            else
            {
                num3 = Math.Sqrt((num - ((num2 * num2) / ((double) period))) / ((double) period));
            }
            if (double.IsNaN(num3))
            {
                return 0.0;
            }
            return num3;
        }
    }
}

