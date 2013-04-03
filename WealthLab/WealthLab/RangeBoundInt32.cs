namespace WealthLab
{
    using System;

    public class RangeBoundInt32
    {
        private int int_0;
        private int int_1;
        private int int_2;

        public RangeBoundInt32(int value, int minValue, int maxValue)
        {
            this.int_0 = value;
            this.int_1 = minValue;
            this.int_2 = maxValue;
        }

        public int MaximumValue
        {
            get
            {
                return this.int_2;
            }
            set
            {
                this.int_2 = value;
            }
        }

        public int MinimumValue
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
            }
        }

        public int Value
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }
    }
}

