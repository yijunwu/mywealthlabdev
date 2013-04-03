namespace WealthLab.ChartStyles.Trending
{
    using System;
    using WealthLab;

    public class PnFTrendLine : TrendLine
    {
        public PnFAngle Angle;
        public int EndColumn;
        internal int int_0;
        public int Reversals;
        public int StartColumn;

        public PnFTrendLine(Bars bars, int bar1, double price1, int bar2, double price2, int idBar, int startCol, PnFAngle pnFAngle_0, bool risingTrendLine) : base(bars, bar1, price1, bar2, price2, idBar)
        {
            this.StartColumn = -1;
            this.EndColumn = -1;
            this.Angle = PnFAngle.const_2;
            base.Bar1 = bar1;
            base.Bar2 = bar2;
            base.Price1 = price1;
            base.Price2 = price2;
            base.IdBar = idBar;
            this.StartColumn = startCol;
            this.Angle = pnFAngle_0;
            base.IsRising = risingTrendLine;
        }
    }
}

