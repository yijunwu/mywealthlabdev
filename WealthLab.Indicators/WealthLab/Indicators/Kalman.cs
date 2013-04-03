namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class Kalman : DataSeries
    {
        private DataSeries dataSeries_1;

        public Kalman(DataSeries source, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            for (int i = 40; i < source.Count; i++)
            {
                base[i] = (0.33 * (source[i] + (0.5 * (source[i] - source[i - 3])))) + (0.67 * base[i - 1]);
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.dataSeries_1.PartialValue == double.NaN)
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                base.PartialValue = (0.33 * (this.dataSeries_1.PartialValue + (0.5 * (this.dataSeries_1.PartialValue - this.dataSeries_1[this.dataSeries_1.Count - 3])))) + (0.67 * this.dataSeries_1[this.dataSeries_1.Count - 1]);
            }
        }

        public static Kalman Series(DataSeries source)
        {
            string key = "Kalman(" + source.Description + ")";
            if (source.Cache.ContainsKey(key))
            {
                return (Kalman) source.Cache[key];
            }
            Kalman kalman = new Kalman(source, key);
            source.Cache[key] = kalman;
            return kalman;
        }
    }
}

