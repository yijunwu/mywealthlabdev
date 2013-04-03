namespace WealthLab.ChartStyles.Trending
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class ColorFinder
    {
        private Color color_0 = Color.Red;
        private Color color_1 = Color.Green;

        public ColorFinder(ChartStyle chartStyle_0)
        {
            bool flag = false;
            bool flag2 = false;
            for (int i = 1; i < chartStyle_0.Bars.Count; i++)
            {
                if (!flag && (chartStyle_0.Bars.Close[i] > chartStyle_0.Bars.Open[i]))
                {
                    this.color_1 = chartStyle_0.GetBarColor(i);
                    flag = true;
                }
                if (!flag2 && (chartStyle_0.Bars.Close[i] < chartStyle_0.Bars.Open[i]))
                {
                    this.color_0 = chartStyle_0.GetBarColor(i);
                    flag2 = true;
                }
                if (flag & flag2)
                {
                    return;
                }
            }
        }

        public Color ColorDown
        {
            get
            {
                return this.color_0;
            }
        }

        public Color ColorUp
        {
            get
            {
                return this.color_1;
            }
        }
    }
}

