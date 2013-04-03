using System;
using WealthLab;

internal class EventArgs2 : EventArgs1
{
    public readonly Bars bars_0;
    public readonly Class28 class28_0;

    public EventArgs2(Class27 class27_1, Bars bars_1, Class28 class28_1) : base(class27_1)
    {
        this.bars_0 = bars_1;
        this.class28_0 = class28_1;
    }
}

