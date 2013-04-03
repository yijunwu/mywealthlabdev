namespace WealthLab.ChartStyles.Trending
{
    using System;

    public class Kagi : Column
    {
        private bool bool_3;
        private double double_4;
        private double double_5;

        public Kagi(int int_2, int int_3, double high, double double_6, double volume, double reverseLevel, bool directionUp, bool reversed, bool plotted, bool isBullish, double yinLevel, double yangLevel) : base(int_2, int_3, high, double_6, volume, reverseLevel, directionUp, reversed, plotted)
        {
            this.bool_3 = isBullish;
            this.double_4 = yinLevel;
            this.double_5 = yangLevel;
        }

        public bool IsBullish
        {
            get
            {
                return this.bool_3;
            }
        }

        public double YangLevel
        {
            get
            {
                return this.double_5;
            }
        }

        public double YinLevel
        {
            get
            {
                return this.double_4;
            }
        }
    }
}

