namespace WealthLab.ChartStyles.Trending
{
    using System;
    using System.Collections.Generic;
    using WealthLab;
    using WealthLab.Indicators;

    public class TKagi
    {
        public IDictionary<int, Kagi> Columns;
        internal DataSeries dataSeries_0;
        internal double double_0;
        internal double double_1;
        internal int int_0;
        internal int[] int_1;
        internal KagiReverseType kagiReverseType_0;

        public TKagi(Bars bars, KagiReverseType kagiReverseType, double reversalAmount, int kagiATRPeriod)
        {
            this.double_0 = reversalAmount;
            this.kagiReverseType_0 = kagiReverseType;
            if (kagiATRPeriod < 1)
            {
                this.int_0 = 1;
            }
            else
            {
                this.int_0 = kagiATRPeriod;
            }
            int num = -1;
            int index = 0;
            bool directionUp = false;
            bool isBullish = false;
            bool flag3 = true;
            bool reversed = false;
            bool plotted = false;
            double reverseLevel = 0.0;
            double high = bars.Close[0];
            double num5 = high;
            double yinLevel = 0.0;
            double yangLevel = 1E+20;
            double volume = bars.Volume[0];
            this.Columns = new Dictionary<int, Kagi>();
            this.int_1 = new int[bars.Count];
            if (this.kagiReverseType_0 == KagiReverseType.Percent)
            {
                this.double_1 = this.double_0 / 100.0;
            }
            else if (this.kagiReverseType_0 == KagiReverseType.Points)
            {
                this.double_1 = this.double_0;
            }
            else
            {
                this.dataSeries_0 = ATR.Series(bars, this.int_0);
                int num9 = Math.Min(bars.Count, this.int_0);
                for (int j = 0; j < num9; j++)
                {
                    this.dataSeries_0[j] = TrueRange.Value(j, bars);
                }
                this.double_1 = this.double_0 * TrueRange.Value(0, bars);
            }
            this.Columns.Add(0, new Kagi(0, -1, high, num5, volume, reverseLevel, directionUp, reversed, plotted, isBullish, yinLevel, yangLevel));
            for (int i = 1; i < bars.Count; i++)
            {
                reversed = false;
                this.int_1[i] = 0;
                volume += bars.Volume[i];
                if (flag3)
                {
                    if (bars.Close[i] >= this.method_0(i, high, false))
                    {
                        flag3 = false;
                        directionUp = true;
                        isBullish = true;
                        index = i;
                        high = bars.Close[i];
                        reverseLevel = this.method_0(i, high, true);
                        yinLevel = num5;
                        yangLevel = high;
                        reversed = true;
                    }
                    else if (bars.Close[i] <= this.method_0(i, num5, true))
                    {
                        flag3 = false;
                        directionUp = false;
                        isBullish = false;
                        index = i;
                        num5 = bars.Close[i];
                        reverseLevel = this.method_0(i, num5, false);
                        yinLevel = num5;
                        yangLevel = high;
                        reversed = true;
                    }
                }
                else if (directionUp)
                {
                    if (bars.Close[i] > high)
                    {
                        index = i;
                        high = bars.Close[i];
                        reverseLevel = this.method_0(i, high, true);
                        if (!isBullish && (bars.Close[i] > yangLevel))
                        {
                            isBullish = true;
                        }
                    }
                    else if (bars.Close[i] <= reverseLevel)
                    {
                        reversed = true;
                        yangLevel = high;
                        num5 = bars.Close[i];
                        volume = bars.Volume[i];
                        directionUp = false;
                        reverseLevel = this.method_0(i, num5, false);
                        this.int_1[index] = 1;
                        this.Columns[index].Plotted = true;
                        index = i;
                        if (isBullish && (bars.Close[i] < yinLevel))
                        {
                            isBullish = false;
                        }
                    }
                }
                else if (bars.Close[i] < num5)
                {
                    index = i;
                    num5 = bars.Close[i];
                    reverseLevel = this.method_0(i, num5, false);
                    if (isBullish && (bars.Close[i] < yinLevel))
                    {
                        isBullish = false;
                    }
                }
                else if (bars.Close[i] >= reverseLevel)
                {
                    reversed = true;
                    yinLevel = num5;
                    high = bars.Close[i];
                    volume = bars.Volume[i];
                    directionUp = true;
                    reverseLevel = this.method_0(i, high, true);
                    this.int_1[index] = 1;
                    this.Columns[index].Plotted = true;
                    index = i;
                    if (!isBullish && (bars.Close[i] > yangLevel))
                    {
                        isBullish = true;
                    }
                }
                if (reversed)
                {
                    num++;
                }
                Kagi kagi = new Kagi(i, num, high, num5, volume, reverseLevel, directionUp, reversed, plotted, isBullish, yinLevel, yangLevel);
                this.Columns.Add(i, kagi);
            }
            this.int_1[index] = 1;
            this.Columns[index].Plotted = true;
            this.int_1[bars.Count - 1] = 1;
        }

        private double method_0(int int_2, double double_2, bool bool_0)
        {
            double num = double_2;
            double num2 = this.double_1;
            if (bool_0)
            {
                num2 = -num2;
            }
            switch (this.kagiReverseType_0)
            {
                case KagiReverseType.Points:
                    num = double_2 + num2;
                    break;

                case KagiReverseType.Percent:
                    num = double_2 * (1.0 + num2);
                    break;

                case KagiReverseType.ATR:
                    num2 = this.dataSeries_0[int_2] * this.double_0;
                    if (bool_0)
                    {
                        num2 = -num2;
                    }
                    num = double_2 + num2;
                    break;
            }
            if (num <= 0.0)
            {
                return 0.0;
            }
            return num;
        }
    }
}

