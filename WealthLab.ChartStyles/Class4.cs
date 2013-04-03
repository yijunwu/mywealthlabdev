using System;
using WealthLab;

internal static class Class4
{
    internal static void smethod_0(ChartStyle chartStyle_0)
    {
        int barSpacing = chartStyle_0.BarSpacing;
        if (barSpacing < 3)
        {
            barSpacing = 3;
        }
        double minValue = chartStyle_0.Bars.Volume.MinValue;
        double num4 = chartStyle_0.Bars.Volume.MaxValue - minValue;
        for (int i = 0; i < chartStyle_0.Bars.Count; i++)
        {
            if (num4 == 0.0)
            {
                chartStyle_0.SetBarWidth(i, barSpacing);
            }
            else
            {
                double num5 = (chartStyle_0.Bars.Volume[i] - minValue) / num4;
                chartStyle_0.SetBarWidth(i, ((int) (100.0 * num5)) + barSpacing);
            }
        }
    }
}

