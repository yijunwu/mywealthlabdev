namespace WealthLab.ChartStyles.Trending
{
    using System;
    using System.Collections.Generic;
    using WealthLab;

    public class TRenko
    {
        public IDictionary<int, Renko> Columns;
        internal int[] int_0;

        public TRenko(Bars bars, double rkoAmount)
        {
            if (bars.Count >= 1)
            {
                this.Columns = new Dictionary<int, Renko>();
                bool reversed = false;
                bool directionUp = true;
                bool flag3 = true;
                bool plotted = false;
                int num = -1;
                int bricksInTrend = 0;
                double volume = bars.Volume[0];
                double num4 = Math.Truncate((double) (bars.Close[0] / rkoAmount)) * rkoAmount;
                double nextBrick = num4;
                DoubleComparer comparer = num4;
                DoubleComparer comparer2 = rkoAmount;
                DoubleComparer comparer3 = comparer + comparer2;
                DoubleComparer comparer4 = comparer - comparer2;
                DoubleComparer comparer5 = comparer;
                DoubleComparer comparer6 = bars.Close[0];
                this.int_0 = new int[bars.Count];
                double high = (double) comparer;
                double num7 = (double) comparer;
                Renko renko = new Renko(0, -1, high, num7, volume, (double) comparer5, true, false, false, 0, nextBrick);
                this.Columns.Add(0, renko);
                for (int i = 1; i < bars.Count; i++)
                {
                    reversed = false;
                    plotted = false;
                    this.int_0[i] = 0;
                    volume += bars.Volume[i];
                    comparer6 = bars.Close[i];
                    if (comparer6 >= comparer3)
                    {
                        plotted = true;
                        if (flag3)
                        {
                            flag3 = false;
                        }
                        else if (!directionUp)
                        {
                            num7 = (double) (comparer5 - comparer2);
                            directionUp = true;
                            reversed = true;
                            bricksInTrend = 0;
                            volume = bars.Volume[i];
                        }
                        do
                        {
                            this.int_0[i]++;
                            comparer3 += comparer2;
                            bricksInTrend++;
                        }
                        while (comparer6 >= comparer3);
                        nextBrick = (double) comparer3;
                        comparer4 = comparer3 - comparer2;
                        high = (double) comparer4;
                        comparer4 -= comparer2 + comparer2;
                        comparer5 = comparer4;
                        num++;
                    }
                    else if (comparer6 <= comparer4)
                    {
                        plotted = true;
                        if (flag3)
                        {
                            flag3 = false;
                            directionUp = false;
                            reversed = true;
                            volume = bars.Volume[i];
                        }
                        else if (directionUp)
                        {
                            high = (double) (comparer5 + comparer2);
                            directionUp = false;
                            reversed = true;
                            bricksInTrend = 0;
                            volume = bars.Volume[i];
                        }
                        do
                        {
                            this.int_0[i]++;
                            comparer4 -= comparer2;
                            bricksInTrend++;
                        }
                        while (comparer6 <= comparer4);
                        nextBrick = (double) comparer4;
                        comparer3 = comparer4 + comparer2;
                        num7 = (double) comparer3;
                        comparer3 += comparer2 + comparer2;
                        comparer5 = comparer3;
                        num++;
                    }
                    renko = new Renko(i, num, high, num7, volume, (double) comparer5, directionUp, reversed, plotted, bricksInTrend, nextBrick);
                    this.Columns.Add(i, renko);
                    if (directionUp)
                    {
                        num7 = high;
                    }
                    else
                    {
                        high = num7;
                    }
                }
            }
        }
    }
}

