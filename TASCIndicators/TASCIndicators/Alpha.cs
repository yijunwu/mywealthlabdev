namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class Alpha : DataSeries
    {
        private DataSeries ds;
        private double k1;
        private double k2;
        private int lrper;
        private double s1;
        private double s2;
        private DataSeries sd;

        public Alpha(DataSeries ds, int sdper, int lrper, string description) : base(ds, description)
        {
            this.ds = ds;
            this.lrper = lrper;
            this.k2 = ((double) (lrper + 1)) / 3.0;
            this.k1 = ((double) lrper) / 2.0;
            this.sd = StdDev.Series(Momentum.Series(ds, 1), sdper, StdDevCalculation.Population);
            if ((lrper < 3) || (lrper > (ds.Count + 1)))
            {
                lrper = ds.Count + 1;
            }
            base.FirstValidValue = (ds.FirstValidValue + Math.Max(lrper, sdper)) - 1;
            this.s2 = 3.0 * ds[0];
            this.s1 = 2.0 * ds[0];
            for (int i = 0; i < (lrper - 1); i++)
            {
                this.s2 += ((2.0 * ds[i]) - this.s1) / this.k2;
                this.s1 += (ds[i] - ds[0]) / this.k1;
            }
            this.s2 += ((2.0 * ds[lrper - 1]) - this.s1) / this.k2;
            this.s1 += (ds[lrper - 1] - ds[0]) / this.k1;
            double num2 = (((lrper + 1) * this.s2) - ((lrper + 2) * this.s1)) / ((double) (lrper - 1));
            if (this.sd[lrper - 1] > 0.0)
            {
                base[lrper - 1] = (num2 - ds[lrper - 1]) / this.sd[lrper - 1];
            }
            for (int j = lrper; j < ds.Count; j++)
            {
                this.s2 += ((2.0 * ds[j]) - this.s1) / this.k2;
                this.s1 += (ds[j] - ds[j - lrper]) / this.k1;
                num2 = (((lrper + 1) * this.s2) - ((lrper + 2) * this.s1)) / ((double) (lrper - 1));
                if (this.sd[j] > 0.0)
                {
                    base[j] = (num2 - ds[j]) / this.sd[j];
                }
            }
        }

        public override void CalculatePartialValue()
        {
            this.ds.CalculatePartialValue();
            if (((this.lrper < 1) || (this.lrper > (this.ds.Count + 1))) || double.IsNaN(this.ds.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                double num = this.s2 + (((2.0 * this.ds.PartialValue) - this.s1) / this.k2);
                double num2 = this.s1 + ((this.ds.PartialValue - this.ds[this.ds.Count - this.lrper]) / this.k1);
                this.sd.CalculatePartialValue();
                if (this.sd.PartialValue > 0.0)
                {
                    base.PartialValue = (((((this.lrper + 1) * num) - ((this.lrper + 2) * num2)) / ((double) (this.lrper - 1))) - this.ds.PartialValue) / this.sd.PartialValue;
                }
            }
        }

        public static Alpha Series(DataSeries ds, int sdper, int lrper)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "Alpha(", ds.Description, ",", sdper, ",", lrper, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (Alpha) ds.Cache[key];
            }
            ds.Cache[key] = series = new Alpha(ds, sdper, lrper, key);
            return (Alpha) series;
        }

        public static double Value(int bar, DataSeries ds, int sdper, int lrper)
        {
            if ((lrper < 1) || (lrper > (bar + 1)))
            {
                return 0.0;
            }
            double num = StdDev.Value(bar, ds, sdper, StdDevCalculation.Population);
            if (num == 0.0)
            {
                return 0.0;
            }
            int num2 = lrper;
            double num3 = ds[bar];
            double num4 = ds[bar] * num2;
            while (--num2 > 0)
            {
                num3 += ds[bar];
                num4 += ds[--bar] * num2;
            }
            return (((((6.0 * num4) - ((2 * (lrper + 2)) * num3)) / ((double) (lrper * (lrper - 1)))) - ds[bar + lrper]) / num);
        }
    }
}

