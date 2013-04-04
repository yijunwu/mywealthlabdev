namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class CyclicComponent : DataSeries
    {
        private double alpha;
        private DataSeries ds;
        private double HP;
        private double HP1;
        private double HP2;
        private double HP3;
        private double w;

        public CyclicComponent(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            if ((period < 1) || (period > ds.Count))
            {
                period = ds.Count;
            }
            if (period < 5)
            {
                period = 5;
            }
            this.alpha = (1.0 - Math.Sin(6.2831853071795862 / ((double) period))) / Math.Cos(6.2831853071795862 / ((double) period));
            this.w = (1.0 + this.alpha) / 2.0;
            base.FirstValidValue = (ds.FirstValidValue + period) - 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            this.HP2 = this.HP1 = this.HP = 0.0;
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                this.HP3 = this.HP2;
                this.HP2 = this.HP1;
                this.HP1 = this.HP;
                this.HP = (this.alpha * this.HP) + (this.w * (ds[i] - ds[i - 1]));
                base[i] = ((this.HP + (2.0 * (this.HP1 + this.HP2))) + this.HP3) / 6.0;
            }
        }

        public override void CalculatePartialValue()
        {
            int count = this.ds.Count;
            if (count < base.FirstValidValue)
            {
                base.PartialValue = 0.0;
            }
            else
            {
                this.ds.CalculatePartialValue();
                if (double.IsNaN(this.ds.PartialValue))
                {
                    base.PartialValue = double.NaN;
                }
                else
                {
                    base.PartialValue = (((this.alpha * this.HP) + (this.w * (this.ds.PartialValue - this.ds[count - 1]))) + (2.0 * (this.HP + this.HP1))) + this.HP2;
                }
            }
        }

        public static CyclicComponent Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "CyclicComponent(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (CyclicComponent) ds.Cache[key];
            }
            ds.Cache[key] = series = new CyclicComponent(ds, period, key);
            return (CyclicComponent) series;
        }
    }
}

