namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class StochRSI : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public StochRSI(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            DataSeries series = RSI.Series(source, period);
            if ((period < 1) || (period > (source.Count + 1)))
            {
                period = source.Count + 1;
            }
            for (int i = period; i < base.Count; i++)
            {
                double num = Lowest.Value(i, series, period);
                double num2 = Highest.Value(i, series, period);
                if (num2 != num)
                {
                    base[i] = ((series[i] - num) / (num2 - num)) * 100.0;
                }
                else
                {
                    base[i] = 0.0;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if (((this.int_1 >= 1) && (this.int_1 <= (this.dataSeries_1.Count + 1))) && (this.dataSeries_1.PartialValue != double.NaN))
            {
                RSI rsi = RSI.Series(this.dataSeries_1, this.int_1);
                for (int i = this.int_1; i < base.Count; i++)
                {
                    double num2 = Lowest.Value(i, this.dataSeries_1, this.int_1);
                    double num3 = Highest.Value(i, this.dataSeries_1, this.int_1);
                    if (num3 != num2)
                    {
                        base.PartialValue = ((rsi[i] - num2) / (num3 - num2)) * 100.0;
                    }
                    else
                    {
                        base.PartialValue = 0.0;
                    }
                }
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static StochRSI Series(DataSeries source, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "StochRSI(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (StochRSI) source.Cache[key];
            }
            source.Cache[key] = series = new StochRSI(source, period, key);
            return (StochRSI) series;
        }
    }
}

