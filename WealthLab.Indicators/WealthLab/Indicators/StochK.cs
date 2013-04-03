namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class StochK : DataSeries
    {
        private Bars bars_1;
        private int int_1;

        public StochK(Bars source, int period, string description) : base(source, description)
        {
            this.bars_1 = source;
            this.int_1 = period;
            if ((period < 1) || (period > (source.Count + 1)))
            {
                period = source.Count + 1;
            }
            for (int i = period; i < base.Count; i++)
            {
                double num = Lowest.Value(i, source.Low, period);
                double num2 = Highest.Value(i, source.High, period);
                if ((num2 - num) != 0.0)
                {
                    base[i] = ((source.Close[i] - num) / (num2 - num)) * 100.0;
                }
                else
                {
                    base[i] = 0.0;
                }
            }
            base.FirstValidValue = period;
        }

        public override void CalculatePartialValue()
        {
            if (((this.int_1 >= 1) && (this.int_1 <= (this.bars_1.Count + 1))) && (base.PartialValue != double.NaN))
            {
                for (int i = this.int_1; i < base.Count; i++)
                {
                    double num2 = Lowest.Value(i, this.bars_1.Low, this.int_1);
                    double num3 = Highest.Value(i, this.bars_1.High, this.int_1);
                    if ((num3 - num2) != 0.0)
                    {
                        base.PartialValue = ((this.bars_1.Close[i] - num2) / (num3 - num2)) * 100.0;
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

        public static StochK Series(Bars source, int period)
        {
            DataSeries series;
            string key = "StochK(" + period + ")";
            if (source.Cache.ContainsKey(key))
            {
                return (StochK) source.Cache[key];
            }
            source.Cache[key] = series = new StochK(source, period, key);
            return (StochK) series;
        }
    }
}

