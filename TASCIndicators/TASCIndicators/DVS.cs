namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class DVS : DataSeries
    {
        private DataSeries ds;
        private DataSeries dvs;

        public DVS(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            if (period < 4)
            {
                period = 4;
            }
            int num = period / 2;
            this.dvs = (ds >> (num / 2)) / SMA.Series(ds, num);
            this.dvs = SMA.Series(StdDev.Series(this.dvs, period, StdDevCalculation.Population), num);
            base.FirstValidValue = (((((ds.FirstValidValue + num) - 1) + period) - 1) + num) - 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = 100.0 * this.dvs[i];
            }
        }

        public override void CalculatePartialValue()
        {
            this.dvs.CalculatePartialValue();
            base.PartialValue = 100.0 * this.dvs.PartialValue;
        }

        public static DVS Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "DVS(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (DVS) ds.Cache[key];
            }
            ds.Cache[key] = series = new DVS(ds, period, key);
            return (DVS) series;
        }
    }
}

