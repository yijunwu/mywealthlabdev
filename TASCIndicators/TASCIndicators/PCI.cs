namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class PCI : DataSeries
    {
        private DataSeries ds;
        private int period;

        public PCI(DataSeries ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            if ((period < 1) || (period > ds.Count))
            {
                period = ds.Count;
            }
            base.FirstValidValue = (ds.FirstValidValue + period) - 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = ds[i - (period - 1)];
                double num5 = (ds[i] - num4) / ((double) (period - 1));
                for (int j = 1; j < (period - 1); j++)
                {
                    double num7 = num4 + (j * num5);
                    double num8 = ds[(i - (period - 1)) + j] - num7;
                    if (num8 > 0.0)
                    {
                        num2 += num8;
                    }
                    else
                    {
                        num3 -= num8;
                    }
                }
                if ((num2 + num3) > 0.0)
                {
                    base[i] = (100.0 * num2) / (num2 + num3);
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
                    double num2 = 0.0;
                    double num3 = 0.0;
                    double num4 = this.ds[this.ds.Count - (this.period - 1)];
                    double num5 = (this.ds.PartialValue - num4) / ((double) (this.period - 1));
                    for (int i = 1; i < (this.period - 1); i++)
                    {
                        double num7 = num4 + (i * num5);
                        double num8 = this.ds[(this.ds.Count - (this.period - 1)) + i] - num7;
                        if (num8 > 0.0)
                        {
                            num2 += num8;
                        }
                        else
                        {
                            num3 -= num8;
                        }
                    }
                    if ((num2 + num3) <= 0.0)
                    {
                        base.PartialValue = 0.0;
                    }
                    else
                    {
                        base.PartialValue = (100.0 * num2) / (num2 + num3);
                    }
                }
            }
        }

        public static PCI Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "PCI(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (PCI) ds.Cache[key];
            }
            ds.Cache[key] = series = new PCI(ds, period, key);
            return (PCI) series;
        }

        public static double Value(int bar, DataSeries ds, int period)
        {
            if ((period < 1) || (period > ds.Count))
            {
                return 0.0;
            }
            double num = 0.0;
            double num2 = 0.0;
            double num3 = ds[bar - (period - 1)];
            double num4 = (ds[bar] - num3) / ((double) (period - 1));
            for (int i = 1; i < (period - 1); i++)
            {
                double num6 = num3 + (i * num4);
                double num7 = ds[(bar - (period - 1)) + i] - num6;
                if (num7 > 0.0)
                {
                    num += num7;
                }
                else
                {
                    num2 -= num7;
                }
            }
            if ((num + num2) <= 0.0)
            {
                return 0.0;
            }
            return ((100.0 * num) / (num + num2));
        }
    }
}

