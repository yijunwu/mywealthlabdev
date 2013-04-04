namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class RapidRSI : DataSeries
    {
        private DataSeries ds;
        private int period;
        private double SumAbsChg;
        private double SumChg;

        public RapidRSI(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            if ((period < 1) || (period > ds.Count))
            {
                period = ds.Count;
            }
            base.FirstValidValue = ds.FirstValidValue + period;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            this.SumChg = 0.0;
            this.SumAbsChg = 0.0;
            for (int i = ds.FirstValidValue + 1; i < base.FirstValidValue; i++)
            {
                double num2 = ds[i] - ds[i - 1];
                if (num2 > 0.0)
                {
                    this.SumChg += num2;
                    this.SumAbsChg += num2;
                }
                else
                {
                    this.SumAbsChg -= num2;
                }
            }
            for (int j = base.FirstValidValue; j < ds.Count; j++)
            {
                double num4 = ds[j] - ds[j - 1];
                if (num4 > 0.0)
                {
                    this.SumChg += num4;
                    this.SumAbsChg += num4;
                }
                else
                {
                    this.SumAbsChg -= num4;
                }
                base[j] = (100.0 * this.SumChg) / this.SumAbsChg;
                num4 = ds[(j - period) + 1] - ds[j - period];
                if (num4 > 0.0)
                {
                    this.SumChg -= num4;
                    this.SumAbsChg -= num4;
                }
                else
                {
                    this.SumAbsChg += num4;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.ds.Count < base.FirstValidValue)
            {
                base.PartialValue = 0.0;
            }
            else
            {
                this.ds.CalculatePartialValue();
                if ((this.period < 1) || double.IsNaN(this.ds.PartialValue))
                {
                    base.PartialValue = double.NaN;
                }
                else
                {
                    double num2 = this.ds.PartialValue - this.ds[this.ds.Count - 1];
                    if (num2 > 0.0)
                    {
                        base.PartialValue = (100.0 * (this.SumChg + num2)) / (this.SumAbsChg + num2);
                    }
                    else
                    {
                        base.PartialValue = (100.0 * this.SumChg) / (this.SumAbsChg - num2);
                    }
                }
            }
        }

        public static RapidRSI Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "RapidRSI(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (RapidRSI) ds.Cache[key];
            }
            ds.Cache[key] = series = new RapidRSI(ds, period, key);
            return (RapidRSI) series;
        }
    }
}

