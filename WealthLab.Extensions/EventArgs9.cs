using System;

internal class EventArgs9 : EventArgs7
{
    private bool bool_0;
    private string string_0;

    public EventArgs9(Enum9 enum9_1) : base(enum9_1)
    {
    }

    public EventArgs9(Enum9 enum9_1, bool bool_1, string string_1) : base(enum9_1)
    {
        this.bool_0 = bool_1;
        this.string_0 = string_1;
    }

    public bool method_1()
    {
        return this.bool_0;
    }

    public string method_2()
    {
        return this.string_0;
    }
}

