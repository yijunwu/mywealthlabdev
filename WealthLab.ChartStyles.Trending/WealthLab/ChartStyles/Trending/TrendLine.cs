namespace WealthLab.ChartStyles.Trending
{
    using System;
    using WealthLab;

    public class TrendLine
    {
        public Bars _bars;
        public int Accel;
        public int Bar1 = -1;
        public int Bar2 = -1;
        public int EndBar = -1;
        public int IdBar = -1;
        public bool IsRising = true;
        public double Price1;
        public double Price2;

        public TrendLine(Bars bars, int bar1, double price1, int bar2, double price2, int idBar)
        {
            this._bars = bars;
            this.EndBar = -1;
            this.Bar1 = bar1;
            this.Bar2 = bar2;
            this.Price1 = price1;
            this.Price2 = price2;
            this.IdBar = idBar;
            this.IsRising = price2 > price1;
        }
    }
}

