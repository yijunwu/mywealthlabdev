namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class SchaffTrendCycle : DataSeries
    {
        private DataSeries _ds;
        private int _ma1;
        private int _ma2;
        private int _tcLen;

        public SchaffTrendCycle(DataSeries ds, int tcLength, int avgPeriod1, int avgPeriod2, string description) : base(ds, description)
        {
            this._ds = ds;
            this._tcLen = tcLength;
            this._ma1 = avgPeriod1;
            this._ma2 = avgPeriod2;
            base.FirstValidValue = this._tcLen + Math.Max(this._ma1, this._ma2);
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            EMACalculation modern = EMACalculation.Modern;
            DataSeries source = EMA.Series(this._ds, this._ma1, modern) - EMA.Series(this._ds, this._ma2, modern);
            DataSeries series2 = Lowest.Series(source, this._tcLen);
            DataSeries series3 = Highest.Series(source, this._tcLen) - series2;
            DataSeries series4 = new DataSeries(this._ds, "Frac1(" + this._ds.Description + ")");
            DataSeries series5 = new DataSeries(this._ds, "PF(" + this._ds.Description + ")");
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                if (series3[i] > 0.0)
                {
                    series4[i] = 100.0 * ((source[i] - series2[i]) / series3[i]);
                }
                else
                {
                    series4[i] = series4[i - 1];
                }
                series5[i] = series5[i - 1] + (0.5 * (series4[i] - series5[i - 1]));
            }
            DataSeries series6 = Lowest.Series(series5, this._tcLen);
            DataSeries series7 = Highest.Series(series5, this._tcLen) - series6;
            DataSeries series8 = new DataSeries(this._ds, "Frac2(" + series5.Description + ")");
            for (int j = base.FirstValidValue; j < ds.Count; j++)
            {
                if (series7[j] > 0.0)
                {
                    series8[j] = 100.0 * ((series5[j] - series6[j]) / series7[j]);
                }
                else
                {
                    series8[j] = series8[j - 1];
                }
                base[j] = base[j - 1] + (0.5 * (series8[j] - base[j - 1]));
            }
        }

        public override void CalculatePartialValue()
        {
            base.PartialValue = double.NaN;
        }

        public static SchaffTrendCycle Series(DataSeries ds, int tcLength, int avgPeriod1, int avgPeriod2)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "SchaffTC(", ds.Description, ",", tcLength, ",", avgPeriod1, ",", avgPeriod2, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (SchaffTrendCycle) ds.Cache[key];
            }
            ds.Cache[key] = series = new SchaffTrendCycle(ds, tcLength, avgPeriod1, avgPeriod2, key);
            return (SchaffTrendCycle) series;
        }
    }
}

