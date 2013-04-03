namespace WealthLab.ChartStyles.Trending
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using WealthLab;

    public class TLineBreak
    {
        public IDictionary<int, LineBreak> Columns;
        internal int[] int_0;
        private int int_1 = 3;

        public TLineBreak(Bars bars, int lines)
        {
            if (bars.Count < 1)
            {
                return;
            }
            this.int_1 = lines;
            this.Columns = new Dictionary<int, LineBreak>();
            ArrayList list = new ArrayList(this.int_1 + 1);
            this.int_0 = new int[bars.Count];
            bool reversed = false;
            bool directionUp = true;
            bool plotted = false;
            int num = -1;
            int linesInTrend = 0;
            int count = 0;
            double volume = 0.0;
            double reverseLevel = bars.Close[0];
            double num6 = reverseLevel;
            double num7 = reverseLevel;
            int index = 0;
        Label_0171:
            this.int_0[index] = 0;
            if (bars.Close[index] <= num6)
            {
                if (bars.Close[index] < num7)
                {
                    num7 = bars.Close[index];
                    list.Add(num6);
                    list.Add(num7);
                    reverseLevel = num6;
                    directionUp = false;
                    this.int_0[index] = 1;
                    plotted = true;
                    linesInTrend = 1;
                    num++;
                }
            }
            else
            {
                num6 = bars.Close[index];
                list.Add(num7);
                list.Add(num6);
                reverseLevel = num7;
                directionUp = true;
                this.int_0[index] = 1;
                plotted = true;
                linesInTrend = 1;
                num++;
            }
            volume += bars.Volume[index];
            LineBreak @break = new LineBreak(index, num, num6, num7, volume, reverseLevel, directionUp, reversed, plotted, linesInTrend);
            this.Columns.Add(index, @break);
            if (num != 0)
            {
                index++;
                if (index < bars.Count)
                {
                    goto Label_0171;
                }
            }
            if (index < bars.Count)
            {
                for (int i = index + 1; i < bars.Count; i++)
                {
                    double num10 = bars.Close[i];
                    this.int_0[i] = 0;
                    plotted = false;
                    reversed = false;
                    volume += bars.Volume[i];
                    count = list.Count;
                    if (directionUp)
                    {
                        if (num10 > num6)
                        {
                            this.method_0(ref list, num10);
                            num7 = num6;
                            num6 = num10;
                            reverseLevel = (double) list[0];
                            linesInTrend++;
                            this.int_0[i] = 1;
                            plotted = true;
                            num++;
                        }
                        else if (num10 < reverseLevel)
                        {
                            reverseLevel = num6;
                            num6 = (double) list[count - 2];
                            num7 = num10;
                            this.method_1(ref list, num6, num7);
                            this.int_0[i] = 1;
                            reversed = true;
                            linesInTrend = 1;
                            plotted = true;
                            num++;
                        }
                    }
                    else if (num10 < num7)
                    {
                        this.method_0(ref list, num10);
                        num6 = num7;
                        num7 = num10;
                        linesInTrend++;
                        reverseLevel = (double) list[0];
                        this.int_0[i] = 1;
                        plotted = true;
                        num++;
                    }
                    else if (num10 > reverseLevel)
                    {
                        reverseLevel = num7;
                        num7 = (double) list[count - 2];
                        num6 = num10;
                        this.method_1(ref list, num7, num6);
                        reversed = true;
                        linesInTrend = 1;
                        this.int_0[i] = 1;
                        plotted = true;
                        num++;
                    }
                    if (reversed)
                    {
                        directionUp = !directionUp;
                        volume = bars.Volume[i];
                    }
                    LineBreak break2 = new LineBreak(i, num, num6, num7, volume, reverseLevel, directionUp, reversed, plotted, linesInTrend);
                    this.Columns.Add(i, break2);
                }
            }
        }

        private void method_0(ref ArrayList arrayList_0, double double_0)
        {
            if (arrayList_0.Count <= this.int_1)
            {
                arrayList_0.Add(double_0);
            }
            else
            {
                arrayList_0.RemoveAt(0);
                arrayList_0.Add(double_0);
            }
        }

        private void method_1(ref ArrayList arrayList_0, double double_0, double double_1)
        {
            arrayList_0.Clear();
            arrayList_0.Add(double_0);
            arrayList_0.Add(double_1);
        }
    }
}

