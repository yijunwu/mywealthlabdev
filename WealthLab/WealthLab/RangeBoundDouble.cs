namespace WealthLab
{
    using System;

    public class RangeBoundDouble
    {
        private double double_0;
        private double double_1;
        private double double_2;

        public RangeBoundDouble(double value, double minValue, double maxValue)
        {
            this.double_0 = value;
            this.double_1 = minValue;
            this.double_2 = maxValue;
        }

        public double MaximumValue
        {
            get
            {
                return this.double_2;
            }
            set
            {
                this.double_2 = value;
            }
        }

        public double MinimumValue
        {
            get
            {
                return this.double_1;
            }
            set
            {
                this.double_1 = value;
            }
        }

        public double Value
        {
            get
            {
                return this.double_0;
            }
            set
            {
                this.double_0 = value;
            }
        }
    }
}

