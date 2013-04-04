namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class CyberCycle : DataSeries
    {
        private DataSeries ds;
        private double OneLessAlpha;
        private double OneLessAlphaSq;
        private double OneLessHalfAlphaSq;

        public CyberCycle(DataSeries ds, double alpha, string description) : base(ds, description)
        {
            this.ds = ds;
            this.OneLessAlpha = 1.0 - alpha;
            this.OneLessAlphaSq = this.OneLessAlpha * this.OneLessAlpha;
            this.OneLessHalfAlphaSq = (1.0 - (alpha / 2.0)) * (1.0 - (alpha / 2.0));
            base.FirstValidValue = ds.FirstValidValue + 5;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            double num = 0.0;
            for (int i = 0; i < base.FirstValidValue; i++)
            {
                if (i >= 2)
                {
                    num = ((ds[i] - (2.0 * ds[i - 1])) + ds[i - 2]) / 4.0;
                }
                base[i] = num;
            }
            for (int j = base.FirstValidValue; j < ds.Count; j++)
            {
                num = (((this.OneLessHalfAlphaSq * (((ds[j - 5] - ds[j - 3]) - ds[j - 2]) + ds[j])) / 6.0) + ((2.0 * this.OneLessAlpha) * base[j - 1])) - (this.OneLessAlphaSq * base[j - 2]);
                base[j] = num;
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
                    base.PartialValue = (((this.OneLessHalfAlphaSq * (((this.ds[count - 5] - this.ds[count - 3]) - this.ds[count - 2]) + this.ds.PartialValue)) / 6.0) + ((2.0 * this.OneLessAlpha) * base[count - 1])) - (this.OneLessAlphaSq * base[count - 2]);
                }
            }
        }

        public static CyberCycle Series(DataSeries ds, double alpha)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "CyberCycle(", ds.Description, ",", alpha, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (CyberCycle) ds.Cache[key];
            }
            ds.Cache[key] = series = new CyberCycle(ds, alpha, key);
            return (CyberCycle) series;
        }
    }
}

