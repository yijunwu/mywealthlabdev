namespace WealthLab
{
    using System;

    public class LinearRegression
    {
        private double double_0;
        private double double_1;
        private double double_2;
        private double double_3;
        private double double_4;
        private double double_5;
        private double double_6;
        private double double_7;
        private int int_0;

        public void Add(double double_8, double double_9)
        {
            this.double_0 += double_8 * double_9;
            this.double_1 += double_8;
            this.double_2 += double_9;
            this.double_3 += double_8 * double_8;
            this.double_4 += double_9 * double_9;
            this.int_0++;
        }

        public void Complete()
        {
            try
            {
                double num3;
                this.double_6 = ((this.int_0 * this.double_0) - (this.double_1 * this.double_2)) / ((this.int_0 * this.double_3) - (this.double_1 * this.double_1));
                this.double_5 = (this.double_2 - (this.double_6 * this.double_1)) / ((double) this.int_0);
                double num = (this.int_0 * this.double_0) - (this.double_1 * this.double_2);
                double d = ((this.int_0 * this.double_3) - (this.double_1 * this.double_1)) * ((this.int_0 * this.double_4) - (this.double_2 * this.double_2));
                if (d > 0.0)
                {
                    num3 = Math.Sqrt(d);
                }
                else
                {
                    num3 = 0.0;
                }
                if (num3 != 0.0)
                {
                    this.double_7 = num / num3;
                }
                else
                {
                    this.double_7 = 0.0;
                }
            }
            catch (Exception)
            {
            }
        }

        public void Init()
        {
            this.double_0 = 0.0;
            this.double_1 = 0.0;
            this.double_2 = 0.0;
            this.double_3 = 0.0;
            this.double_4 = 0.0;
            this.double_5 = 0.0;
            this.double_6 = 0.0;
            this.double_7 = 0.0;
            this.int_0 = 0;
        }

        public double PredictY(double double_8)
        {
            return (this.double_5 + (this.double_6 * double_8));
        }

        public double Corr
        {
            get
            {
                return this.double_7;
            }
        }
    }
}

