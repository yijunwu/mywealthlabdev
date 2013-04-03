namespace WealthLab.ChartStyles.Trending
{
    using System;

    public class LineBreak : Column
    {
        private int int_2;

        public LineBreak(int int_3, int int_4, double high, double double_4, double volume, double reverseLevel, bool directionUp, bool reversed, bool plotted, int linesInTrend) : base(int_3, int_4, high, double_4, volume, reverseLevel, directionUp, reversed, plotted)
        {
            this.int_2 = 1;
            this.int_2 = linesInTrend;
        }

        public int LinesInTrend
        {
            get
            {
                return this.int_2;
            }
        }
    }
}

