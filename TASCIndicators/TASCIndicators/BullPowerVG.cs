namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class BullPowerVG : DataSeries
    {
        private Bars ds;

        public BullPowerVG(Bars ds, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                double num2 = ds.Close[i];
                double num3 = ds.Open[i];
                double num4 = ds.Close[i - 1];
                double num5 = ds.High[i];
                double num6 = ds.Low[i];
                double num7 = Math.Max((double) (num3 - num4), (double) (num5 - num6));
                double num8 = Math.Max((double) (num5 - Math.Min(num4, num3)), (double) (num2 - num6));
                if (num2 > num3)
                {
                    base[i] = num7;
                }
                else if (num2 < num3)
                {
                    base[i] = num8;
                }
                else if ((num5 - num2) < (num2 - num6))
                {
                    base[i] = num7;
                }
                else if ((num5 - num2) > (num2 - num6))
                {
                    base[i] = num8;
                }
                else if (num2 > num4)
                {
                    base[i] = num7;
                }
                else
                {
                    base[i] = num8;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            double partialValue = this.ds.Close.PartialValue;
            double num2 = this.ds.Open.PartialValue;
            double num3 = this.ds.Close[base.Count - 1];
            double num4 = this.ds.High.PartialValue;
            double num5 = this.ds.Low.PartialValue;
            double num6 = Math.Max((double) (num2 - num3), (double) (num4 - num5));
            double num7 = Math.Max((double) (num4 - Math.Min(num3, num2)), (double) (partialValue - num5));
            if (partialValue > num2)
            {
                base.PartialValue = num6;
            }
            else if (partialValue < num2)
            {
                base.PartialValue = num7;
            }
            else if ((num4 - partialValue) < (partialValue - num5))
            {
                base.PartialValue = num6;
            }
            else if ((num4 - partialValue) > (partialValue - num5))
            {
                base.PartialValue = num7;
            }
            else if (partialValue > num3)
            {
                base.PartialValue = num6;
            }
            else
            {
                base.PartialValue = num7;
            }
        }

        public static BullPowerVG Series(Bars ds)
        {
            DataSeries series;
            string key = "BullPowerVG()";
            if (ds.Cache.ContainsKey(key))
            {
                return (BullPowerVG) ds.Cache[key];
            }
            ds.Cache[key] = series = new BullPowerVG(ds, key);
            return (BullPowerVG) series;
        }

        public static double Value(int bar, Bars ds)
        {
            double num = ds.Close[bar];
            double num2 = ds.Open[bar];
            double num3 = ds.Close[bar - 1];
            double num4 = ds.High[bar];
            double num5 = ds.Low[bar];
            double num6 = Math.Max((double) (num2 - num3), (double) (num4 - num5));
            double num7 = Math.Max((double) (num4 - Math.Min(num3, num2)), (double) (num - num5));
            if (num > num2)
            {
                return num6;
            }
            if (num >= num2)
            {
                if ((num4 - num) < (num - num5))
                {
                    return num6;
                }
                if ((num4 - num) > (num - num5))
                {
                    return num7;
                }
                if (num > num3)
                {
                    return num6;
                }
            }
            return num7;
        }
    }
}

