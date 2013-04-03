namespace WealthLab.ChartStyles.Trending
{
    using System;

    public class Renko : Column
    {
        private double double_4;
        private int int_2;

        public Renko(int int_3, int int_4, double high, double double_5, double volume, double reverseLevel, bool directionUp, bool reversed, bool plotted, int bricksInTrend, double nextBrick) : base(int_3, int_4, high, double_5, volume, reverseLevel, directionUp, reversed, plotted)
        {
            this.int_2 = 1;
            this.int_2 = bricksInTrend;
        }

        public int BricksInTrend
        {
            get
            {
                return this.int_2;
            }
        }

        public double NextBrick
        {
            get
            {
                return this.double_4;
            }
        }
    }
}

