namespace Fidelity.Components
{
    using System;

    public class Bin
    {
        private double double_0;
        private double double_1;
        private int int_0;

        public int Count
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

        public double High
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

        public double Low
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

