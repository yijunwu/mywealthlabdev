namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class LowestBar2 : DataSeries
    {
        private DataSeries ds;
        private int lb;
        private int period;

        public LowestBar2(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            if ((period < 1) || (period > (ds.Count + 1)))
            {
                period = ds.Count + 1;
            }
            base.FirstValidValue = (ds.FirstValidValue + period) - 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            this.lb = 0;
            for (int i = 1; i < base.FirstValidValue; i++)
            {
                if (ds[i] < ds[this.lb])
                {
                    this.lb = i;
                }
            }
            for (int j = base.FirstValidValue; j < ds.Count; j++)
            {
                if (ds[j] < ds[this.lb])
                {
                    this.lb = j;
                }
                else if (this.lb == (j - period))
                {
                    int num3 = ++this.lb;
                    while (++num3 <= j)
                    {
                        if (ds[num3] < ds[this.lb])
                        {
                            this.lb = num3;
                        }
                    }
                }
                base[j] = this.lb;
            }
        }

        public override void CalculatePartialValue()
        {
            this.ds.CalculatePartialValue();
            if (((this.period < 1) || (this.period > (this.ds.Count + 1))) || double.IsNaN(this.ds.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                int count = this.ds.Count;
                if (this.ds.PartialValue < this.ds[this.lb])
                {
                    base.PartialValue = count;
                }
                else if (this.lb == (count - this.period))
                {
                    int num2 = ++this.lb;
                    while (++num2 < count)
                    {
                        if (this.ds[num2] < this.ds[this.lb])
                        {
                            this.lb = num2;
                        }
                    }
                    base.PartialValue = (this.ds.PartialValue < this.ds[this.lb]) ? ((double) base.Count) : ((double) this.lb);
                }
                else
                {
                    base.PartialValue = this.lb;
                }
            }
        }

        public static LowestBar2 Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "LowestBar2(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (LowestBar2) ds.Cache[key];
            }
            ds.Cache[key] = series = new LowestBar2(ds, period, key);
            return (LowestBar2) series;
        }

        public static double Value(int bar, DataSeries ds, int period)
        {
            if ((period < 1) || (period > (bar + 1)))
            {
                return 0.0;
            }
            int num = bar - period;
            int num2 = ++num;
            while (++num2 <= bar)
            {
                if (ds[num2] < ds[num])
                {
                    num = num2;
                }
            }
            return (double) num;
        }
    }
}

