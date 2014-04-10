namespace WealthLab
{
    using System;

    public class RangeBoundInt32
    {
        private int intValue;
        private int minimumValue;
        private int maximumValue;

        public RangeBoundInt32(int value, int minValue, int maxValue)
        {
            this.intValue = value;
            this.minimumValue = minValue;
            this.maximumValue = maxValue;
        }

        public int MaximumValue
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

        public int MinimumValue
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

        public int Value
        {
            get
            {
                return this.intValue;
            }
            set
            {
                this.intValue = value;
            }
        }
    }
}

