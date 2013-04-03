namespace WealthLab.ChartStyles.Trending
{
    using System;

    public class Column
    {
        internal bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private double double_0;
        private double double_1;
        private double double_2;
        private double double_3;
        internal int int_0;
        internal int int_1;

        public Column(int int_2, int int_3, double high, double double_4, double volume, double reverseLevel, bool directionUp, bool reversed, bool plotted)
        {
            this.int_1 = int_2;
            this.int_0 = int_3;
            this.double_0 = high;
            this.double_1 = double_4;
            this.double_3 = volume;
            this.bool_1 = directionUp;
            this.bool_2 = reversed;
            this.bool_0 = plotted;
            this.double_2 = reverseLevel;
        }

        public int Bar
        {
            get
            {
                return this.int_1;
            }
        }

        public int Col
        {
            get
            {
                return this.int_0;
            }
        }

        public bool DirectionUp
        {
            get
            {
                return this.bool_1;
            }
        }

        public double High
        {
            get
            {
                return this.double_0;
            }
        }

        public double Low
        {
            get
            {
                return this.double_1;
            }
        }

        public bool Plotted
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public bool Reversed
        {
            get
            {
                return this.bool_2;
            }
        }

        public double ReverseLevel
        {
            get
            {
                return this.double_2;
            }
        }

        public double Volume
        {
            get
            {
                return this.double_3;
            }
        }
    }
}

