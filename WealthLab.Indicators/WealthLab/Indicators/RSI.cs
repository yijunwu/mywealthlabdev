namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class RSI : DataSeries
    {
        public RSI(DataSeries dataSeries_1, int period, string description) : base(dataSeries_1, description)
        {
            base.FirstValidValue = period + dataSeries_1.FirstValidValue;
            if (dataSeries_1.Count >= period)
            {
                double num5;
                double num = 0.0;
                double num2 = 0.0;
                for (int i = period; i > 0; i--)
                {
                    if (dataSeries_1[i] > dataSeries_1[i - 1])
                    {
                        num += dataSeries_1[i] - dataSeries_1[i - 1];
                    }
                    else
                    {
                        num2 += dataSeries_1[i - 1] - dataSeries_1[i];
                    }
                }
                num /= (double) period;
                num2 /= (double) period;
                double num3 = num;
                double num4 = num2;
                if (num2 > 0.0)
                {
                    num5 = num / num2;
                }
                else
                {
                    num5 = 1.0;
                }
                base[period] = 100.0 - (100.0 / (1.0 + num5));
                for (int j = period + 1; j < dataSeries_1.Count; j++)
                {
                    num = 0.0;
                    num2 = 0.0;
                    if (dataSeries_1[j] > dataSeries_1[j - 1])
                    {
                        num = dataSeries_1[j] - dataSeries_1[j - 1];
                    }
                    else if (dataSeries_1[j] < dataSeries_1[j - 1])
                    {
                        num2 = dataSeries_1[j - 1] - dataSeries_1[j];
                    }
                    num = ((num3 * (period - 1)) + num) / ((double) period);
                    num2 = ((num4 * (period - 1)) + num2) / ((double) period);
                    num3 = num;
                    num4 = num2;
                    if (num2 > 0.0)
                    {
                        num5 = num / num2;
                    }
                    else
                    {
                        num5 = 1.0;
                    }
                    base[j] = 100.0 - (100.0 / (1.0 + num5));
                }
            }
        }

        public static RSI Series(DataSeries dataSeries_1, int period)
        {
            string key = string.Concat(new object[] { "RSI(", dataSeries_1.Description, ",", period, ")" });
            if (dataSeries_1.Cache.ContainsKey(key))
            {
                return (RSI) dataSeries_1.Cache[key];
            }
            RSI rsi = new RSI(dataSeries_1, period, key);
            dataSeries_1.Cache[key] = rsi;
            return rsi;
        }
    }
}

