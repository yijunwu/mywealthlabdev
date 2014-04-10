namespace WealthLab
{
    using System;

    public class RangeBoundDouble
    {
        private double doubleValue;
        private double minimumValue;
        private double maximumValue;

        public RangeBoundDouble(double value, double minValue, double maxValue)
        {
            this.doubleValue = value;
            this.minimumValue = minValue;
            this.maximumValue = maxValue;
        }

        public double MaximumValue
        {
            get
            {
                return this.maximumValue;
            }
            set
            {
                this.maximumValue = value;
            }
        }

        public double MinimumValue
        {
            get
            {
                return this.minimumValue;
            }
            set
            {
                this.minimumValue = value;
            }
        }

        public double Value
        {
            get
            {
                return this.doubleValue;
            }
            set
            {
                this.doubleValue = value;
            }
        }
    }
}

