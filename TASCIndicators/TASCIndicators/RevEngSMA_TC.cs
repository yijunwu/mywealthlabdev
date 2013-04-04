namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class RevEngSMA_TC : DataSeries
    {
        private DataSeries ds;
        private int period1;
        private int period2;
        private DataSeries SMA1;
        private DataSeries SMA2;

        public RevEngSMA_TC(DataSeries ds, int period1, int period2, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period1 = period1;
            this.period2 = period2;
            this.SMA1 = SMA.Series(ds, period1 - 1);
            this.SMA2 = SMA.Series(ds, period2 - 1);
            if ((period1 < 1) || (period1 > (ds.Count + 1)))
            {
                period1 = ds.Count + 1;
            }
            if ((period2 < 1) || (period2 > (ds.Count + 1)))
            {
                period2 = ds.Count + 1;
            }
            base.FirstValidValue = (ds.FirstValidValue + Math.Max(period1, period2)) - 2;
            if (period2 != period1)
            {
                for (int i = base.FirstValidValue; i < ds.Count; i++)
                {
                    base[i] = (((this.SMA2[i] * period1) * (period2 - 1)) - ((this.SMA1[i] * period2) * (period1 - 1))) / ((double) (period2 - period1));
                }
            }
        }

        public override void CalculatePartialValue()
        {
            this.ds.CalculatePartialValue();
            if ((((this.period1 < 1) || (this.period1 > (this.ds.Count + 1))) || ((this.period2 < 1) || (this.period2 > (this.ds.Count + 1)))) || ((this.period1 == this.period2) || double.IsNaN(this.ds.PartialValue)))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                this.SMA1.CalculatePartialValue();
                this.SMA2.CalculatePartialValue();
                base.PartialValue = (((this.SMA2.PartialValue * this.period1) * (this.period2 - 1)) - ((this.SMA1.PartialValue * this.period2) * (this.period1 - 1))) / ((double) (this.period2 - this.period1));
            }
        }

        public static RevEngSMA_TC Series(DataSeries ds, int period1, int period2)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "RevEngSMA_TC(", ds.Description, ",", period1, ",", period2, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (RevEngSMA_TC) ds.Cache[key];
            }
            ds.Cache[key] = series = new RevEngSMA_TC(ds, period1, period2, key);
            return (RevEngSMA_TC) series;
        }

        public static double Value(int bar, DataSeries ds, int period1, int period2)
        {
            if (((period1 >= 1) && (period1 <= (bar + 1))) && ((period2 >= 1) && (period2 <= (bar + 1))))
            {
                return ((((SMA.Value(bar, ds, period2) * period1) * (period2 - 1)) - ((SMA.Value(bar, ds, period1) * period2) * (period1 - 1))) / ((double) (period2 - period1)));
            }
            return 0.0;
        }
    }
}

