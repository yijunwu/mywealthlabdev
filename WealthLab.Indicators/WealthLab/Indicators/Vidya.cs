namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class Vidya : DataSeries
    {
        private DataSeries dataSeries_1;

        public Vidya(DataSeries source, int stdDevPeriod, double alpha, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            DataSeries series = StdDev.Series(source, stdDevPeriod, StdDevCalculation.Sample);
            for (int i = 1; i < base.Count; i++)
            {
                double num = series[i] * alpha;
                if (num > 1.0)
                {
                    num = 1.0;
                }
                if (num <= 0.0)
                {
                    num = 0.0001;
                }
                double num2 = source[i] * num;
                double num3 = base[i - 1] * (1.0 - num);
                base[i] = num2 + num3;
            }
            base.FirstValidValue = 2;
        }

        public Vidya(DataSeries source, DataSeries volatilityIndex, double alpha, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            for (int i = 1; i < base.Count; i++)
            {
                double num = volatilityIndex[i] * alpha;
                if (num > 1.0)
                {
                    num = 1.0;
                }
                if (num <= 0.0)
                {
                    num = 0.0001;
                }
                double num2 = source[i] * num;
                double num3 = base[i - 1] * (1.0 - num);
                base[i] = num2 + num3;
            }
            base.FirstValidValue = 2;
        }

        public override void CalculatePartialValue()
        {
            if (this.dataSeries_1.PartialValue == double.NaN)
            {
                base.PartialValue = double.NaN;
            }
        }

        public static Vidya Series(DataSeries source, int stdDevPeriod, double alpha)
        {
            string key = string.Concat(new object[] { "Vidya(", source.Description, ",", stdDevPeriod, ",", alpha, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (Vidya) source.Cache[key];
            }
            Vidya vidya = new Vidya(source, stdDevPeriod, alpha, key);
            source.Cache[key] = vidya;
            return vidya;
        }
    }
}

