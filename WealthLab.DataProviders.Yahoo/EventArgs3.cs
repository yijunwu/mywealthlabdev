using System;

internal class EventArgs3 : EventArgs1
{
    public readonly Exception exception_0;

    public EventArgs3(Class27 class27_1, Exception exception_1) : base(class27_1)
    {
        this.exception_0 = exception_1;
    }
}

