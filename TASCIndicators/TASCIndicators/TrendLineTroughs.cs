namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class TrendLineTroughs : DataSeries
    {
        private DataSeries ds;
        private int ltdbar;
        private DataSeries tbser;
        private bool uselogscale;

        public TrendLineTroughs(DataSeries ds, double reversepct, bool uselogscale, string description) : base(ds, description)
        {
            this.ds = ds;
            this.uselogscale = uselogscale;
            this.tbser = TroughBar.Series(ds, reversepct, PeakTroughMode.Percent);
            this.ltdbar = ds.Count;
            for (int i = ds.FirstValidValue + 1; i < ds.Count; i++)
            {
                if (this.tbser[i] > this.tbser[i - 1])
                {
                    this.ltdbar = i;
                    break;
                }
            }
            for (int j = this.ltdbar + 1; j < ds.Count; j++)
            {
                if (this.tbser[j] > this.tbser[j - 1])
                {
                    this.ltdbar = j;
                    break;
                }
            }
            base.FirstValidValue = this.ltdbar;
            for (int k = base.FirstValidValue; k < ds.Count; k++)
            {
                if (this.tbser[k] > this.tbser[k - 1])
                {
                    this.ltdbar = k;
                }
                int num4 = (int) this.tbser[this.ltdbar];
                int num5 = (int) this.tbser[this.ltdbar - 1];
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
            this.tbser.CalculatePartialValue();
            if (this.tbser.PartialValue > this.tbser[this.ds.Count - 1])
            {
                partialValue = (int) this.tbser.PartialValue;
            }
            else
            {
                partialValue = (int) this.tbser[this.ltdbar];
            }
            int num2 = (int) this.tbser[this.ltdbar - 1];
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

        public static TrendLineTroughs Series(DataSeries ds, double reversepct, bool uselogscale)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "TrendLineTroughs(", ds.Description, ",", reversepct, ",", uselogscale, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (TrendLineTroughs) ds.Cache[key];
            }
            ds.Cache[key] = series = new TrendLineTroughs(ds, reversepct, uselogscale, key);
            return (TrendLineTroughs) series;
        }
    }
}

