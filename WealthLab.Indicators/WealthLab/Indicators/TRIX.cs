namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class TRIX : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public TRIX(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            if ((period < 1) || (period > (source.Count + 1)))
            {
                period = source.Count + 1;
            }
            DataSeries series3 = EMA.Series(EMA.Series(EMA.Series(source, period, EMACalculation.Modern), period, EMACalculation.Modern), period, EMACalculation.Modern);
            for (int i = period * 3; i < base.Count; i++)
            {
                if (series3[i] != 0.0)
                {
                    double num2 = (series3[i] - series3[i - 1]) / series3[i];
                    base[i] = num2 * 100.0;
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
                DataSeries series = EMA.Series(EMA.Series(EMA.Series(this.dataSeries_1, this.int_1, EMACalculation.Modern), this.int_1, EMACalculation.Modern), this.int_1, EMACalculation.Modern);
                for (int i = this.int_1 * 3; i < base.Count; i++)
                {
                    if (series[i] != 0.0)
                    {
                        double num2 = (series[i] - series[i - 1]) / series[i];
                        base.PartialValue = num2 * 100.0;
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

        public static TRIX Series(DataSeries source, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "TRIX(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (TRIX) source.Cache[key];
            }
            source.Cache[key] = series = new TRIX(source, period, key);
            return (TRIX) series;
        }
    }
}

