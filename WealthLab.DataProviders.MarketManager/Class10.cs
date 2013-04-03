using System;
using System.Collections.Generic;
using System.Diagnostics;
using WealthLab;

internal class Class10
{
    private Bars bars_0;
    private BarScale barScale_0;
    private int int_0;
    private List<Class9> list_0;
    private string string_0;
    private string string_1;

    public Class10(Bars bars_1)
    {
        this.bars_0 = bars_1;
        this.string_0 = bars_1.Symbol;
        this.barScale_0 = bars_1.Scale;
        this.int_0 = bars_1.BarInterval;
        this.string_1 = bars_1.SecurityName;
    }

    public string method_0()
    {
        return this.string_0;
    }

    public BarScale method_1()
    {
        return this.barScale_0;
    }

    public int method_2()
    {
        return this.int_0;
    }

    public List<Class9> method_3()
    {
        return this.list_0;
    }

    public void method_4()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        this.list_0 = new List<Class9>(this.bars_0.Count);
        for (int i = 0; i < this.bars_0.Count; i++)
        {
            Class9 item = new Class9 {
                dateTime_0 = this.bars_0.Date[i],
                double_0 = this.bars_0.Open[i],
                double_1 = this.bars_0.High[i],
                double_2 = this.bars_0.Low[i],
                double_3 = this.bars_0.Close[i],
                double_4 = this.bars_0.Volume[i]
            };
            this.list_0.Add(item);
        }
        stopwatch.Stop();
        Trace.WriteLine("ConvertFromBars " + stopwatch.ElapsedMilliseconds);
    }

    public Bars method_5()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Bars bars = new Bars(this.bars_0.Symbol, this.bars_0.Scale, this.bars_0.BarInterval) {
            SecurityName = this.string_1
        };
        List<DataSeries> list = new List<DataSeries>();
        foreach (DataSeries series2 in this.bars_0.NamedSeries)
        {
            list.Add(bars.RegisterNamedSeries(series2.Description, false));
        }
        int num = 0;
        for (int i = 0; i < this.list_0.Count; i++)
        {
            if (!this.list_0[i].bool_0)
            {
                Class9 class2 = this.list_0[i];
                bars.Add(class2.dateTime_0, class2.double_0, class2.double_1, class2.double_2, class2.double_3, class2.double_4);
                foreach (DataSeries series in list)
                {
                    series[num] = this.bars_0.FindNamedSeries(series.Description)[i];
                }
                num++;
            }
        }
        stopwatch.Stop();
        Trace.WriteLine("ConvertToBars " + stopwatch.ElapsedMilliseconds);
        return bars;
    }
}

