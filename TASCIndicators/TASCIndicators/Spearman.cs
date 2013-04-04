namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class Spearman : DataSeries
    {
        private DataSeries ds;
        private int period;

        public Spearman(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            base.FirstValidValue = period;
            int num = period;
            DataSeries series = new DataSeries(ds, "coeffcorr");
            DataSeries series2 = new DataSeries(ds, "sc");
            DataSeries series3 = new DataSeries(ds, "r1");
            DataSeries series4 = new DataSeries(ds, "r11");
            new DataSeries(ds, "r2");
            DataSeries series5 = new DataSeries(ds, "r21");
            DataSeries series6 = new DataSeries(ds, "r22");
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                for (int j = num; j >= 1; j--)
                {
                    series3[j] = j;
                    series6[j] = j;
                    series4[j] = ds[(i - num) + j];
                    series5[j] = ds[(i - num) + j];
                }
                int num4 = 1;
                while (num4 > 0)
                {
                    num4 = 0;
                    double num5 = 0.0;
                    for (int n = 1; n <= (num - 1); n++)
                    {
                        if (series5[n + 1] < series5[n])
                        {
                            num5 = series5[n];
                            series5[n] = series5[n + 1];
                            series5[n + 1] = num5;
                            num4 = 1;
                        }
                    }
                }
                for (int k = 1; k <= num; k++)
                {
                    int num8 = 0;
                    while (num8 < 1)
                    {
                        for (int num9 = 1; num9 <= num; num9++)
                        {
                            if (series5[num9] == series4[k])
                            {
                                series6[k] = num9;
                                num8 = 1;
                            }
                        }
                    }
                }
                double num10 = 0.0;
                double x = 0.0;
                double num12 = 0.0;
                for (int m = 1; m <= num; m++)
                {
                    x = series3[m] - series6[m];
                    num12 = Math.Pow(x, 2.0);
                    num10 += num12;
                }
                series[i] = 1.0 - ((6.0 * num10) / ((double) (num * ((num * num) - 1))));
                series2[i] = 100.0 * series[i];
                base[i] = series2[i];
            }
        }

        public static Spearman Series(DataSeries ds, int period)
        {
            string key = string.Concat(new object[] { "Spearman(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (Spearman) ds.Cache[key];
            }
            Spearman spearman = new Spearman(ds, period, key);
            ds.Cache[key] = spearman;
            return spearman;
        }
    }
}

