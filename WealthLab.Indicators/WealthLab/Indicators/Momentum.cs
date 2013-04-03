namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class Momentum : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public Momentum(DataSeries dataSeries_2, int period, string description) : base(dataSeries_2, description)
        {
            this.dataSeries_1 = dataSeries_2;
            this.int_1 = period;
            if ((period < 1) || (period > dataSeries_2.Count))
            {
                period = dataSeries_2.Count;
            }
            base.FirstValidValue = dataSeries_2.FirstValidValue + period;
            for (int i = period; i < dataSeries_2.Count; i++)
            {
                base[i] = dataSeries_2[i] - dataSeries_2[i - period];
            }
        }

        public override void CalculatePartialValue()
        {
            if (((this.int_1 >= 1) && (this.int_1 <= this.dataSeries_1.Count)) && (this.dataSeries_1.PartialValue != double.NaN))
            {
                base.PartialValue = this.dataSeries_1.PartialValue - this.dataSeries_1[this.dataSeries_1.Count - this.int_1];
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static Momentum Series(DataSeries dataSeries_2, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "Momentum(", dataSeries_2.Description, ",", period, ")" });
            if (dataSeries_2.Cache.ContainsKey(key))
            {
                return (Momentum) dataSeries_2.Cache[key];
            }
            dataSeries_2.Cache[key] = series = new Momentum(dataSeries_2, period, key);
            return (Momentum) series;
        }

        public static double Value(int int_2, DataSeries dataSeries_2, int period)
        {
            if ((period >= 1) && (period <= int_2))
            {
                return (dataSeries_2[int_2] - dataSeries_2[int_2 - period]);
            }
            return 0.0;
        }
    }
}

