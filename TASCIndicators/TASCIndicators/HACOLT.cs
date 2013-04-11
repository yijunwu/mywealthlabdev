namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class HACOLT : DataSeries
    {
        private int avg;
        private Bars bars;
        private double CandleSize;
        private int setuptimeout;
        private int ShLTAvg;

        public HACOLT(Bars bars, int avg, double CandleSize, int setuptimeout, int ShLTAvg, string description) : base(bars, description)
        {
            this.bars = bars;
            this.avg = avg;
            this.CandleSize = CandleSize;
            this.setuptimeout = setuptimeout;
            this.ShLTAvg = ShLTAvg;
            base.FirstValidValue = Math.Max(ShLTAvg, avg);
            int num = -1;
            int num2 = -1;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            int num3 = -1;
            bool flag6 = false;
            bool flag7 = false;
            bool flag8 = false;
            bool flag9 = false;
            bool flag10 = false;
            int num4 = -1;
            int num5 = -1;
            int num6 = -1;
            bool flag11 = false;
            int num7 = -1;
            int num8 = -1;
            bool flag12 = false;
            bool flag13 = true;
            bool flag14 = true;
            int num9 = -1;
            int num10 = 0;
            int num11 = 0;
            int num12 = 0;
            EMACalculation modern = EMACalculation.Modern;
            EMA ema = EMA.Series(bars.Close, ShLTAvg, modern);
            DataSeries series = bars.Open + 0.0;  ///WYJ fix, original: DataSeries series = bars.Open + ((DataSeries) 0.0);
            DataSeries series2 = bars.High + 0.0;
            DataSeries series3 = bars.Low + 0.0;
            DataSeries series4 = (DataSeries) ((((bars.Open + bars.High) + bars.Low) + bars.Close) / 4.0);
            DataSeries ds = series4 + 0.0;
            ds.Description = "Heikin-Ashi Close";
            for (int i = 1; i < bars.Count; i++)
            {
                double num14 = series[i - 1];
                double num15 = series4[i - 1];
                series[i] = (num14 + num15) / 2.0;
                series2[i] = Math.Max(series[i], bars.High[i]);
                series3[i] = Math.Min(series[i], bars.Low[i]);
                ds[i] = (((series4[i] + series[i]) + series2[i]) + series3[i]) / 4.0;
            }
            DataSeries series6 = TEMA.Series(ds, avg, modern);
            DataSeries series7 = TEMA.Series(series6, avg, modern);
            DataSeries series8 = series6 - series7;
            DataSeries series9 = series6 + series8;
            series6 = TEMA.Series(AveragePrice.Series(bars), avg, modern);
            series7 = TEMA.Series(series6, avg, modern);
            series8 = series6 - series7;
            DataSeries series10 = series6 + series8;
            DataSeries series11 = series10 - series9;
            series11.Description = "Crossover formula (" + avg + ")";
            for (int j = base.FirstValidValue; j < bars.Count; j++)
            {
                if (!flag)
                {
                    bool flag15 = (bars.Close[j] > ds[j]) || ((bars.High[j] > bars.High[j - 1]) || (bars.Low[j] > bars.Low[j - 1]));
                    if ((ds[j] >= series[j]) || flag15)
                    {
                        flag = true;
                        num = j;
                    }
                }
                if (flag)
                {
                    flag = ((j + 1) - num) < setuptimeout;
                }
                flag2 = series11[j] >= 0.0;
                flag4 = flag || flag2;
                if (flag4)
                {
                    num6 = j;
                }
                else
                {
                    num6 = 0;
                }
                flag5 = flag4 || ((((num6 > 0) & (j == (num6 + 1))) & (bars.Close[j] >= bars.Open[j])) | (bars.Close[j] >= bars.Close[j - 1]));
                if (flag5)
                {
                    num3 = j;
                }
                else
                {
                    num3 = 0;
                }
                flag3 = (DataSeries.Abs(bars.Close - bars.Open)[j] < ((bars.High[j] - bars.Low[j]) * CandleSize)) & (bars.High[j] >= bars.Low[j - 1]);
                flag11 = flag5 || (((num3 > 0) & (j == (num3 + 1))) & flag3);
                if (flag11)
                {
                    num7 = j;
                }
                else
                {
                    num7 = 0;
                }
                if (!flag6 && (ds[j] < series[j]))
                {
                    flag6 = true;
                    num2 = j;
                }
                if (flag6)
                {
                    flag6 = ((j + 1) - num2) < setuptimeout;
                }
                flag7 = series11[j] < 0.0;
                flag8 = (DataSeries.Abs(bars.Close - bars.Open)[j] < ((bars.High[j] - bars.Low[j]) * CandleSize)) & (bars.Low[j] <= bars.High[j - 1]);
                flag9 = flag6 || flag7;
                if (flag9)
                {
                    num5 = j;
                }
                else
                {
                    num5 = 0;
                }
                flag10 = flag9 || ((((num5 > 0) & (j == (num5 + 1))) & (bars.Close[j] < bars.Open[j])) | (bars.Close[j] < bars.Close[j - 1]));
                if (flag10)
                {
                    num4 = j;
                }
                else
                {
                    num4 = 0;
                }
                flag12 = flag10 || ((num4 > 0) & ((j == (num4 + 1)) & flag8));
                if (flag12)
                {
                    num8 = j;
                }
                else
                {
                    num8 = 0;
                }
                flag13 = !flag12 || (((num8 > 0) & (j == (num8 + 1))) & flag11);
                flag14 = !flag11 || (((num7 > 0) & (j == (num7 + 1))) & flag12);
                if (flag13)
                {
                    num10 = 1;
                    num9 = 1;
                }
                else if (flag14)
                {
                    num10 = 0;
                    num9 = 0;
                }
                else if (num9 > -1)
                {
                    num10 = num9;
                }
                bool flag16 = bars.Close[j] < ema[j];
                num11 = (num10 == 1) ? 1 : (((num10 == 0) && flag16) ? 0 : num11);
                num12 = (num10 == 1) ? 100 : (((num10 == 0) && (num11 == 1)) ? 50 : (((num10 == 0) && (num11 == 0)) ? 0 : num12));
                base[j] = num12;
            }
        }

        public static HACOLT Series(Bars bars, int avg, double CandleSize, int timeoutperiod, int ShLTAvg)
        {
            string key = string.Concat(new object[] { "HACOLT(", avg, ",", CandleSize, ",", timeoutperiod, ",", ShLTAvg, ")" });
            if (bars.Cache.ContainsKey(key))
            {
                return (HACOLT) bars.Cache[key];
            }
            HACOLT hacolt = new HACOLT(bars, avg, CandleSize, timeoutperiod, ShLTAvg, key);
            bars.Cache[key] = hacolt;
            return hacolt;
        }
    }
}

