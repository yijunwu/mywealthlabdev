namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class TCFMinus : DataSeries
    {
        private DataSeries CFPlus;
        private DataSeries ChangeMinus;
        private DataSeries ds;
        private int period;
        private DataSeries SumCFPlus;
        private DataSeries SumChangeMinus;

        public TCFMinus(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            if ((period < 1) || (period > (ds.Count + 1)))
            {
                period = ds.Count + 1;
            }
            this.ChangeMinus = new DataSeries(ds, "Chg-");
            this.CFPlus = new DataSeries(ds, "CumChg+");
            this.ChangeMinus.FirstValidValue = ds.FirstValidValue + 1;
            this.CFPlus.FirstValidValue = ds.FirstValidValue + 1;
            for (int i = ds.FirstValidValue + 1; i < base.Count; i++)
            {
                double num2 = ds[i] - ds[i - 1];
                if (num2 > 0.0)
                {
                    this.CFPlus[i] = this.CFPlus[i - 1] + num2;
                }
                else
                {
                    this.ChangeMinus[i] = -num2;
                }
            }
            this.SumChangeMinus = Sum.Series(this.ChangeMinus, period);
            this.SumCFPlus = Sum.Series(this.CFPlus, period);
            base.FirstValidValue = ds.FirstValidValue + period;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int j = base.FirstValidValue; j < base.Count; j++)
            {
                base[j] = this.SumChangeMinus[j] - this.SumCFPlus[j];
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
                    this.ChangeMinus.PartialValue = 0.0;
                    this.CFPlus.PartialValue = this.CFPlus[count - 1] + num2;
                }
                else
                {
                    this.ChangeMinus.PartialValue = -num2;
                    this.CFPlus.PartialValue = 0.0;
                }
                this.SumChangeMinus.CalculatePartialValue();
                this.SumCFPlus.CalculatePartialValue();
                base.PartialValue = this.SumChangeMinus.PartialValue - this.SumCFPlus.PartialValue;
            }
        }

        public static TCFMinus Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "TCF-(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (TCFMinus) ds.Cache[key];
            }
            ds.Cache[key] = series = new TCFMinus(ds, period, key);
            return (TCFMinus) series;
        }
    }
}

