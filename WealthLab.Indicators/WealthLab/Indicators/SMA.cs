namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class SMA : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public SMA(DataSeries dataSeries_2, int period, string description) : base(dataSeries_2, description)
        {
            this.dataSeries_1 = dataSeries_2;
            this.int_1 = period;
            base.FirstValidValue = (period - 1) + dataSeries_2.FirstValidValue;
            if (period <= dataSeries_2.Count)
            {
                double num = 0.0;
                for (int i = 0; i < period; i++)
                {
                    num += dataSeries_2[i];
                }
                base[period - 1] = num / ((double) period);
                for (int j = period; j < dataSeries_2.Count; j++)
                {
                    num -= dataSeries_2[j - period];
                    num += dataSeries_2[j];
                    base[j] = num / ((double) period);
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.dataSeries_1.Count >= (this.int_1 - 1)) && (this.dataSeries_1.PartialValue != double.NaN))
            {
                double num = 0.0;
                for (int i = (this.dataSeries_1.Count - this.int_1) + 1; i < this.dataSeries_1.Count; i++)
                {
                    num += this.dataSeries_1[i];
                }
                num += this.dataSeries_1.PartialValue;
                base.PartialValue = num / ((double) this.int_1);
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static SMA Series(DataSeries dataSeries_2, int period)
        {
            string key = string.Concat(new object[] { "SMA(", dataSeries_2.Description, ",", period, ")" });
            if ((dataSeries_2.Cache != null) && dataSeries_2.Cache.ContainsKey(key))
            {
                return (SMA) dataSeries_2.Cache[key];
            }
            SMA sma = new SMA(dataSeries_2, period, key);
            if (dataSeries_2.Cache != null)
            {
                dataSeries_2.Cache[key] = sma;
            }
            return sma;
        }

        public static double Value(int int_2, DataSeries dataSeries_2, int period)
        {
            if (dataSeries_2.Count < period)
            {
                return 0.0;
            }
            double num = 0.0;
            for (int i = int_2; i > (int_2 - period); i--)
            {
                num += dataSeries_2[i];
            }
            return (num / ((double) period));
        }
    }
}

