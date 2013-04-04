namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class PTBar : DataSeries
    {
        private int Bar1;
        private int Bar2;
        private DataSeries ds;
        private double PeakReversalFactor;
        private double TroughReversalFactor;

        public PTBar(DataSeries ds, double percent, string description) : base(ds, description)
        {
            this.ds = ds;
            this.PeakReversalFactor = 100.0 / (100.0 + percent);
            this.TroughReversalFactor = (100.0 + percent) / 100.0;
            this.Bar1 = ds.FirstValidValue;
            this.Bar2 = this.Bar1 + 1;
            while (this.Bar2 < ds.Count)
            {
                if ((ds[this.Bar2] >= (ds[this.Bar1] * this.TroughReversalFactor)) || (ds[this.Bar2] <= (ds[this.Bar1] * this.PeakReversalFactor)))
                {
                    break;
                }
                this.Bar2++;
            }
            base.FirstValidValue = this.Bar2;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = this.Bar2; i < ds.Count; i++)
            {
                if (ds[this.Bar2] > ds[this.Bar1])
                {
                    if (ds[i] >= ds[this.Bar2])
                    {
                        this.Bar2 = i;
                    }
                    else if (ds[i] <= (ds[this.Bar2] * this.PeakReversalFactor))
                    {
                        this.Bar1 = this.Bar2;
                        this.Bar2 = i;
                    }
                }
                else if (ds[i] <= ds[this.Bar2])
                {
                    this.Bar2 = i;
                }
                else if (ds[i] >= (ds[this.Bar2] * this.TroughReversalFactor))
                {
                    this.Bar1 = this.Bar2;
                    this.Bar2 = i;
                }
                base[i] = this.Bar1;
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.ds.Count <= base.FirstValidValue)
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                this.ds.CalculatePartialValue();
                if (double.IsNaN(this.ds.PartialValue))
                {
                    base.PartialValue = double.NaN;
                }
                else if (this.ds[this.Bar2] > this.ds[this.Bar1])
                {
                    base.PartialValue = (this.ds.PartialValue <= (this.ds[this.Bar2] * this.PeakReversalFactor)) ? ((double) this.Bar2) : ((double) this.Bar1);
                }
                else
                {
                    base.PartialValue = (this.ds.PartialValue >= (this.ds[this.Bar2] * this.TroughReversalFactor)) ? ((double) this.Bar2) : ((double) this.Bar1);
                }
            }
        }

        public static PTBar Series(DataSeries ds, double percent)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "PTBar(", ds.Description, ",", percent, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (PTBar) ds.Cache[key];
            }
            ds.Cache[key] = series = new PTBar(ds, percent, key);
            return (PTBar) series;
        }
    }
}

