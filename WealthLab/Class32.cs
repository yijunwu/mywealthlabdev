using System;
using System.Collections.Generic;
using WealthLab;

internal class Class32 : IUpdateRequestCompleted
{
    private BarScale barScale_0;
    private int int_0;
    private List<Bars> list_0 = new List<Bars>();

    public Class32(BarScale barScale_1, int int_1)
    {
        this.barScale_0 = barScale_1;
        this.int_0 = int_1;
    }

    public List<Bars> method_0()
    {
        return this.list_0;
    }

    public void ProcessingCompleted()
    {
    }

    public void UpdateCompleted(Bars bars)
    {
        this.method_0().Add(bars);
    }

    public void UpdateError(string symbol, Exception exception_0)
    {
        this.method_0().Add(new Bars(symbol, this.barScale_0, this.int_0));
    }
}

