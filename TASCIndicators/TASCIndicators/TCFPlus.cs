namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class TCFPlus : DataSeries
    {
        private DataSeries CFMinus;
        private DataSeries ChangePlus;
        private DataSeries ds;
        private int period;
        private DataSeries SumCFMinus;
        private DataSeries SumChangePlus;

        public TCFPlus(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            if ((period < 1) || (period > (ds.Count + 1)))
            {
                period = ds.Count + 1;
            }
            this.ChangePlus = new DataSeries(ds, "Chg+");
            this.CFMinus = new DataSeries(ds, "CumChg-");
            this.ChangePlus.FirstValidValue = ds.FirstValidValue + 1;
            this.CFMinus.FirstValidValue = ds.FirstValidValue + 1;
            for (int i = ds.FirstValidValue + 1; i < base.Count; i++)
            {
                double num2 = ds[i] - ds[i - 1];
                if (num2 > 0.0)
                {
                    this.ChangePlus[i] = num2;
                }
                else
                {
                    this.CFMinus[i] = this.CFMinus[i - 1] - num2;
                }
            }
            this.SumChangePlus = Sum.Series(this.ChangePlus, period);
            this.SumCFMinus = Sum.Series(this.CFMinus, period);
            base.FirstValidValue = ds.FirstValidValue + period;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int j = base.FirstValidValue; j < base.Count; j++)
            {
                base[j] = this.SumChangePlus[j] - this.SumCFMinus[j];
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.period < 1) || (this.period > (this.ds.Count + 1)))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                int count = this.ds.Count;
                double num2 = this.ds[count] - this.ds[count - 1];
                if (num2 > 0.0)
                {
                    this.ChangePlus.PartialValue = num2;
                    this.CFMinus.PartialValue = 0.0;
                }
                else
                {
                    this.ChangePlus.PartialValue = 0.0;
                    this.CFMinus.PartialValue = this.CFMinus[count - 1] - num2;
                }
                this.SumChangePlus.CalculatePartialValue();
                this.SumCFMinus.CalculatePartialValue();
                base.PartialValue = this.SumChangePlus.PartialValue - this.SumCFMinus.PartialValue;
            }
        }

        public static TCFPlus Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "TCF+(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (TCFPlus) ds.Cache[key];
            }
            ds.Cache[key] = series = new TCFPlus(ds, period, key);
            return (TCFPlus) series;
        }
    }
}

