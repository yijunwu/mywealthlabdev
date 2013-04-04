namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class InstantaneousTrendLine : DataSeries
    {
        private DataSeries ds;
        private int period;
        private DataSeries sma;

        public InstantaneousTrendLine(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            if ((period < 1) || (period > ds.Count))
            {
                period = ds.Count;
            }
            this.sma = SMA.Series(ds, period);
            base.FirstValidValue = (ds.FirstValidValue + period) + 2;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                double num2 = (((ds[i] - ds[(i - period) + 1]) + ds[i - 3]) - ds[((i - period) + 1) - 3]) + (2.0 * (((ds[i - 1] - ds[((i - period) + 1) - 1]) + ds[i - 2]) - ds[((i - period) + 1) - 2]));
                base[i] = this.sma[i] + (num2 / 12.0);
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
                if ((this.period < 1) || double.IsNaN(this.ds.PartialValue))
                {
                    base.PartialValue = double.NaN;
                }
                else
                {
                    double num2 = (((this.ds.PartialValue - this.ds[(count - this.period) + 1]) + this.ds[count - 3]) - this.ds[((count - this.period) + 1) - 3]) + (2.0 * (((this.ds[count - 1] - this.ds[((count - this.period) + 1) - 1]) + this.ds[count - 2]) - this.ds[((count - this.period) + 1) - 2]));
                    this.sma.CalculatePartialValue();
                    base.PartialValue = this.sma.PartialValue + (num2 / 12.0);
                }
            }
        }

        public static InstantaneousTrendLine Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "InstantaneousTrendLine(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (InstantaneousTrendLine) ds.Cache[key];
            }
            ds.Cache[key] = series = new InstantaneousTrendLine(ds, period, key);
            return (InstantaneousTrendLine) series;
        }

        public static double Value(int bar, DataSeries ds, int period)
        {
            if ((bar < (period + 2)) || (period > ds.Count))
            {
                return 0.0;
            }
            double num = (((ds[bar] - ds[(bar - period) + 1]) + ds[bar - 3]) - ds[((bar - period) + 1) - 3]) + (2.0 * (((ds[bar - 1] - ds[((bar - period) + 1) - 1]) + ds[bar - 2]) - ds[((bar - period) + 1) - 2]));
            return (SMA.Value(bar, ds, period) + (num / 12.0));
        }
    }
}

