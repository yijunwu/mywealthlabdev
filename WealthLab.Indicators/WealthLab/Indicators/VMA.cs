namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class VMA : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public VMA(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = period - 1;
            Bars bars = base.FindParentBars();
            for (int i = base.FirstValidValue; i < source.Count; i++)
            {
                double num2 = 0.0;
                double num3 = 0.0;
                for (int j = i; j > (i - period); j--)
                {
                    num2 += source[j] * bars.Volume[j];
                    num3 += bars.Volume[j];
                }
                if (num3 > 0.0)
                {
                    base[i] = num2 / num3;
                }
            }
        }

        public static VMA Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "VMA(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (VMA) source.Cache[key];
            }
            VMA vma = new VMA(source, period, key);
            source.Cache[key] = vma;
            return vma;
        }
    }
}

