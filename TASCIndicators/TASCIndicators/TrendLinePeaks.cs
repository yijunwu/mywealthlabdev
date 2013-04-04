namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class TrendLinePeaks : DataSeries
    {
        private DataSeries ds;
        private int lpdbar;
        private DataSeries pbser;
        private bool uselogscale;

        public TrendLinePeaks(DataSeries ds, double reversepct, bool uselogscale, string description) : base(ds, description)
        {
            this.ds = ds;
            this.uselogscale = uselogscale;
            this.pbser = PeakBar.Series(ds, reversepct, PeakTroughMode.Percent);
            this.lpdbar = ds.Count;
            for (int i = ds.FirstValidValue + 1; i < ds.Count; i++)
            {
                if (this.pbser[i] > this.pbser[i - 1])
                {
                    this.lpdbar = i;
                    break;
                }
            }
            for (int j = this.lpdbar + 1; j < ds.Count; j++)
            {
                if (this.pbser[j] > this.pbser[j - 1])
                {
                    this.lpdbar = j;
                    break;
                }
            }
            base.FirstValidValue = this.lpdbar;
            for (int k = base.FirstValidValue; k < ds.Count; k++)
            {
                if (this.pbser[k] > this.pbser[k - 1])
                {
                    this.lpdbar = k;
                }
                int num4 = (int) this.pbser[this.lpdbar];
                int num5 = (int) this.pbser[this.lpdbar - 1];
                double num6 = ds[num4];
                double num7 = ds[num5];
                if (uselogscale)
                {
                    base[k] = num7 * Math.Exp((Math.Log(num6 / num7) * (k - num5)) / ((double) (num4 - num5)));
                }
                else
                {
                    base[k] = num7 + (((num6 - num7) * (k - num5)) / ((double) (num4 - num5)));
                }
            }
        }

        public override void CalculatePartialValue()
        {
            int partialValue;
            this.pbser.CalculatePartialValue();
            if (this.pbser.PartialValue > this.pbser[this.ds.Count - 1])
            {
                partialValue = (int) this.pbser.PartialValue;
            }
            else
            {
                partialValue = (int) this.pbser[this.lpdbar];
            }
            int num2 = (int) this.pbser[this.lpdbar - 1];
            if (num2 < 0)
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                double num3 = this.ds[partialValue];
                double num4 = this.ds[num2];
                if (this.uselogscale)
                {
                    base.PartialValue = num4 * Math.Exp((Math.Log(num3 / num4) * (this.ds.Count - num2)) / ((double) (partialValue - num2)));
                }
                else
                {
                    base.PartialValue = num4 + (((num3 - num4) * (this.ds.Count - num2)) / ((double) (partialValue - num2)));
                }
            }
        }

        public static TrendLinePeaks Series(DataSeries ds, double reversepct, bool uselogscale)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "TrendLinePeaks(", ds.Description, ",", reversepct, ",", uselogscale, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (TrendLinePeaks) ds.Cache[key];
            }
            ds.Cache[key] = series = new TrendLinePeaks(ds, reversepct, uselogscale, key);
            return (TrendLinePeaks) series;
        }
    }
}

