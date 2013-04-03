namespace WealthLab.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using WealthLab;

    public static class DataSeriesOp
    {
        public static void SplitReverseFactor(WealthScript wealthScript_0, string splitItem, out DataSeries dataSeries_0)
        {
            IList<FundamentalItem> list;
            dataSeries_0 = wealthScript_0.Bars.Close / wealthScript_0.Bars.Close;
            dataSeries_0.Description = wealthScript_0.Bars.Symbol + "(Reverse Adjustment Factor)";
            try
            {
                list = wealthScript_0.FundamentalDataItems(splitItem);
                if (list.Count == 0)
                {
                    return;
                }
            }
            catch
            {
                return;
            }
            int num4 = wealthScript_0.Bars.Count - 1;
            int num5 = num4;
            int bar = 0;
            double num2 = 1.0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                FundamentalItem item = list[i];
                if (item.Date > wealthScript_0.Date[num4])
                {
                    if (item.Date <= DateTime.Today)
                    {
                        num2 *= item.Value;
                    }
                }
                else
                {
                    if (item.Date <= wealthScript_0.Date[0])
                    {
                        bar = 0;
                    }
                    else if (item.Bar < 0)
                    {
                        bar = 0;
                    }
                    else
                    {
                        bar = item.Bar;
                    }
                    for (int j = num5; j >= bar; j--)
                    {
                        dataSeries_0[j] = num2;
                    }
                    if (bar == 0)
                    {
                        return;
                    }
                    num5 = bar - 1;
                    num2 *= item.Value;
                    if (i == 0)
                    {
                        for (int k = num5; k >= 0; k--)
                        {
                            dataSeries_0[k] = num2;
                        }
                    }
                }
            }
        }
    }
}

