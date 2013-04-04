namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class CG : DataSeries
    {
        private DataSeries ds;
        private int period;
        private double Sum;
        private double WSum;

        public CG(DataSeries ds, int period, string description) : base(ds, description)
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
            this.WSum = 0.0;
            this.Sum = 0.0;
            for (int i = ds.FirstValidValue; i < base.FirstValidValue; i++)
            {
                this.WSum += ds[i] * (period - i);
                this.Sum += ds[i];
            }
            for (int j = period - 1; j < ds.Count; j++)
            {
                this.Sum += ds[j];
                this.WSum += ds[j];
                base[j] = -this.WSum / this.Sum;
                this.Sum -= ds[(j - period) + 1];
                this.WSum += this.Sum - (ds[(j - period) + 1] * period);
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
                base.PartialValue = (this.WSum + this.ds.PartialValue) / (this.Sum + this.ds.PartialValue);
            }
        }

        public static CG Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "CG(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (CG) ds.Cache[key];
            }
            ds.Cache[key] = series = new CG(ds, period, key);
            return (CG) series;
        }

        public static double Value(int bar, DataSeries ds, int period)
        {
            if ((period < 1) || (period > (bar + 1)))
            {
                return 0.0;
            }
            double num = ds[bar];
            double num2 = ds[bar];
            int num3 = 1;
            while (++num3 <= period)
            {
                num += ds[--bar] * num3;
                num2 += ds[bar];
            }
            return (-num / num2);
        }
    }
}

