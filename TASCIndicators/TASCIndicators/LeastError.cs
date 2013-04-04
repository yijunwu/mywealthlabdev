namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class LeastError : DataSeries
    {
        private DataSeries ds;

        public LeastError(DataSeries ds, int Length, int GainLimit, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = Length;
            double num = 2.0 / (Length + 1.0);
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = 0.0;
            double num5 = 0.0;
            DataSeries series = EMA.Series(ds, Length, EMACalculation.Modern);
            DataSeries series2 = new DataSeries(ds, "ec");
            DataSeries series3 = new DataSeries(ds, "LeastErrorSeries");
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                series[i] = (num * ds[i]) + ((1.0 - num) * series[i - 1]);
                num2 = 1000000.0;
                for (int j = -GainLimit; j <= GainLimit; j++)
                {
                    num3 = j / 10;
                    series2[i] = (num * (series[i] + (num3 * (ds[i] - series2[i - 1])))) + ((1.0 - num) * series2[i - 1]);
                    num4 = ds[i] - series2[i];
                    if (Math.Abs(num4) < num2)
                    {
                        num2 = Math.Abs(num4);
                        num5 = num3;
                    }
                }
                series3[i] = (100.0 * num2) / ds[i];
                series2[i] = (num * (series[i] + (num5 * (ds[i] - series2[i - 1])))) + ((1.0 - num) * series2[i - 1]);
                base[i] = series3[i];
            }
        }

        public static LeastError Series(DataSeries ds, int Length, int Gain)
        {
            string key = string.Concat(new object[] { "LeastError(", ds.Description, ",", Length, ",", Gain, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (LeastError) ds.Cache[key];
            }
            LeastError error = new LeastError(ds, Length, Gain, key);
            ds.Cache[key] = error;
            return error;
        }
    }
}

