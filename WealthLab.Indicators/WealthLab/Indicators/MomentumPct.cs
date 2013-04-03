namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class MomentumPct : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public MomentumPct(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = period + source.FirstValidValue;
            double num = 0.0;
            for (int i = period; i < source.Count; i++)
            {
                num = source[i - period];
                if (num != 0.0)
                {
                    base[i] = (source[i] / num) * 100.0;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.dataSeries_1.PartialValue != double.NaN) && (this.int_1 >= this.dataSeries_1.Count))
            {
                base.PartialValue = (this.dataSeries_1.PartialValue / this.dataSeries_1[this.dataSeries_1.Count - this.int_1]) * 100.0;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static MomentumPct Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "MomentumPct(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (MomentumPct) source.Cache[key];
            }
            MomentumPct pct = new MomentumPct(source, period, key);
            source.Cache[key] = pct;
            return pct;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            if (period <= source.Count)
            {
                return 0.0;
            }
            return ((source[int_2] / source[int_2 - period]) * 100.0);
        }
    }
}

