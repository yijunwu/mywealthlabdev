namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class DPO : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;

        public DPO(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            Bars bars = base.FindParentBars();
            int num = (period / 2) + 1;
            base.FirstValidValue = num;
            SMA sma = SMA.Series(source, period);
            for (int i = base.FirstValidValue; i < bars.Count; i++)
            {
                base[i] = bars.Close[i] - sma[i - num];
            }
        }

        public override void CalculatePartialValue()
        {
            Bars bars = base.FindParentBars();
            int num = (this.int_1 / 2) + 1;
            SMA sma = SMA.Series(this.dataSeries_1, this.int_1);
            base.PartialValue = bars.Close.PartialValue - sma[((int) base.PartialValue) - num];
        }

        public static DPO Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "DPO(", source.Description, period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (DPO) source.Cache[key];
            }
            DPO dpo = new DPO(source, period, key);
            source.Cache[key] = dpo;
            return dpo;
        }
    }
}

