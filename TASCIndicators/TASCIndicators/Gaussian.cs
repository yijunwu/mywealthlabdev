namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class Gaussian : DataSeries
    {
        private double c;
        private double c1;
        private double c2;
        private double c3;
        private double c4;
        private DataSeries ds;

        public Gaussian(DataSeries ds, double period, int poles, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = ds.FirstValidValue + 4;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            double d = 6.2831853071795862 / period;
            double num2 = (1.0 - Math.Cos(d)) / (Math.Pow(2.0, 1.0 / ((double) poles)) - 1.0);
            double num3 = -num2 + Math.Sqrt((num2 * num2) + (2.0 * num2));
            double num4 = 1.0 - num3;
            this.c4 = this.c3 = this.c2 = this.c1 = this.c = 0.0;
            switch (poles)
            {
                case 1:
                    this.c1 = num4;
                    this.c = num3;
                    break;

                case 2:
                    this.c2 = -num4 * num4;
                    this.c1 = 2.0 * num4;
                    this.c = num3 * num3;
                    break;

                case 3:
                    this.c3 = (num4 * num4) * num4;
                    this.c2 = (-3.0 * num4) * num4;
                    this.c1 = 3.0 * num4;
                    this.c = (num3 * num3) * num3;
                    break;

                case 4:
                    this.c4 = ((-num4 * num4) * num4) * num4;
                    this.c3 = ((4.0 * num4) * num4) * num4;
                    this.c2 = (-6.0 * num4) * num4;
                    this.c1 = 4.0 * num4;
                    this.c = ((num3 * num3) * num3) * num3;
                    break;
            }
            for (int i = 0; i < base.FirstValidValue; i++)
            {
                base[i] = ds[i];
            }
            for (int j = base.FirstValidValue; j < ds.Count; j++)
            {
                base[j] = ((((this.c * ds[j]) + (this.c1 * base[j - 1])) + (this.c2 * base[j - 2])) + (this.c3 * base[j - 3])) + (this.c4 * base[j - 4]);
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
                if (double.IsNaN(this.ds.PartialValue))
                {
                    base.PartialValue = double.NaN;
                }
                else
                {
                    base.PartialValue = ((((this.c * this.ds.PartialValue) + (this.c1 * base[count - 1])) + (this.c2 * base[count - 2])) + (this.c3 * base[count - 3])) + (this.c4 * base[count - 4]);
                }
            }
        }

        public static Gaussian Series(DataSeries ds, double period, int poles)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "Gaussian(", ds.Description, ",", period, ",", poles, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (Gaussian) ds.Cache[key];
            }
            ds.Cache[key] = series = new Gaussian(ds, period, poles, key);
            return (Gaussian) series;
        }
    }
}

