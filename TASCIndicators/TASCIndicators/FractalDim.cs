namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class FractalDim : DataSeries
    {
        private int avPer;
        private DataSeries ds;
        private int N;
        private int per;
        private DataSeries ratio;
        private DataSeries smooth;

        public FractalDim(DataSeries ds, int period, int avgPeriod, string description) : base(ds, description)
        {
            if ((period % 2) > 0)
            {
                period++;
            }
            this.N = period;
            this.avPer = avgPeriod;
            this.ds = ds;
            base.FirstValidValue = ds.FirstValidValue + period;
            if (ds.Count >= base.FirstValidValue)
            {
                this.smooth = FIR.Series(ds, "1,2,2,1");
                DataSeries series = (DataSeries) ((Highest.Series(this.smooth, this.N) - Lowest.Series(this.smooth, this.N)) / ((double) this.N));
                this.per = this.N / 2;
                DataSeries series2 = (DataSeries) ((Highest.Series(this.smooth, this.per) - Lowest.Series(this.smooth, this.per)) / ((double) this.per));
                DataSeries source = this.smooth >> this.per;
                DataSeries series4 = (DataSeries) ((Highest.Series(source, this.per) - Lowest.Series(source, this.per)) / ((double) this.per));
                this.ratio = new DataSeries(ds, "Fractal Ratio");
                double num = Math.Log(2.0);
                for (int i = base.FirstValidValue; i < ds.Count; i++)
                {
                    if (((series2[i] > 0.0) && (series4[i] > 0.0)) && (series[i] > 0.0))
                    {
                        this.ratio[i] = 0.5 * (((Math.Log(series2[i] + series4[i]) - Math.Log(series[i])) / num) + base[i - 1]);
                    }
                    else
                    {
                        this.ratio[i] = this.ratio[i - 1];
                    }
                    if (i < this.avPer)
                    {
                        base[i] = this.ratio[i];
                    }
                    else
                    {
                        base[i] = SMA.Value(i, this.ratio, this.avPer);
                    }
                }
            }
        }

        public override void CalculatePartialValue()
        {
            int num = this.ds.Count - 1;
            if ((this.ds.Count < this.N) || double.IsNaN(this.ds.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                double num2 = (((this.ds[num - 2] + (this.ds[num - 1] * 2.0)) + (this.ds[num] * 2.0)) + this.ds.PartialValue) / 6.0;
                double d = (Math.Max(num2, Highest.Value(num, this.smooth, this.N - 1)) - Math.Min(num2, Lowest.Value(num, this.smooth, this.N - 1))) / ((double) this.N);
                double num4 = (Math.Max(num2, Highest.Value(num, this.smooth, this.per - 1)) - Math.Min(num2, Lowest.Value(num, this.smooth, this.per - 1))) / ((double) this.per);
                int num5 = (num - this.per) + 1;
                double num6 = (Highest.Value(num5, this.smooth, this.per) - Lowest.Value(num5, this.smooth, this.per)) / ((double) this.per);
                double num7 = 0.0;
                if (((num4 > 0.0) && (num6 > 0.0)) && (d > 0.0))
                {
                    num7 = 0.5 * (((Math.Log(num4 + num6) - Math.Log(d)) / Math.Log(2.0)) + base[num]);
                }
                else
                {
                    num7 = this.ratio[num];
                }
                if (num <= this.avPer)
                {
                    base.PartialValue = num7;
                }
                else
                {
                    base.PartialValue = (Sum.Value(num, this.ratio, this.avPer - 1) + num7) / ((double) this.avPer);
                }
            }
        }

        public static FractalDim Series(DataSeries ds, int period, int avgPeriod)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "FractalDim(", ds.Description, ",", period, ",", avgPeriod, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (FractalDim) ds.Cache[key];
            }
            ds.Cache[key] = series = new FractalDim(ds, period, avgPeriod, key);
            return (FractalDim) series;
        }
    }
}

