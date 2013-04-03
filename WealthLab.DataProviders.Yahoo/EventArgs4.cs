using System;
using WealthLab;

internal class EventArgs4 : EventArgs
{
    public readonly double double_0;
    public readonly double double_1;
    public readonly double double_2;
    public readonly Quote quote_0;

    public EventArgs4(Quote quote_1, double double_3, double double_4, double double_5)
    {
        this.quote_0 = quote_1;
        this.double_0 = double_3;
        this.double_1 = double_4;
        this.double_2 = double_5;
    }
}

