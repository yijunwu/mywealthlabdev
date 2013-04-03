namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class HV : DataSeries
    {
        public HV(DataSeries source, int period, int span, string description) : base(source, description)
        {
            double num;
            DataSeries series = new DataSeries(source, "HVTempLogs");
            for (int i = 1; i < source.Count; i++)
            {
                if (source[i - 1] != 0.0)
                {
                    num = source[i] / source[i - 1];
                    series[i] = Math.Log(num);
                }
            }
            for (int j = period; j < source.Count; j++)
            {
                double num2 = 0.0;
                for (int k = 0; k < period; k++)
                {
                    num2 += series[j - k];
                }
                double num3 = num2 / ((double) period);
                double num4 = 0.0;
                for (int m = 0; m < period; m++)
                {
                    num = series[j - m] - num3;
                    num *= num;
                    num4 += num;
                }
                base[j] = (Math.Sqrt(num4 / ((double) (period - 1))) * 100.0) * Math.Sqrt((double) span);
            }
        }

        public static HV Series(DataSeries source, int period, int span)
        {
            string key = string.Concat(new object[] { "HV(", source.Description, ",", period, ",", span, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (HV) source.Cache[key];
            }
            HV hv = new HV(source, period, span, key);
            source.Cache[key] = hv;
            return hv;
        }
    }
}

