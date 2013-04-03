using System;

internal class EventArgs8 : EventArgs7
{
    private int int_0;

    public EventArgs8(Enum9 enum9_1, int int_1) : base(enum9_1)
    {
        this.int_0 = int_1;
    }

    public int method_1()
    {
        return this.int_0;
    }
}

