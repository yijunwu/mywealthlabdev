using System;

internal class EventArgs12 : EventArgs
{
    public readonly bool bool_0;
    public readonly bool bool_1;
    public readonly string string_0;

    public EventArgs12()
    {
    }

    public EventArgs12(bool bool_2)
    {
        this.bool_0 = false;
        this.bool_1 = bool_2;
    }

    public EventArgs12(string string_1)
    {
        this.bool_0 = true;
        this.string_0 = string_1;
    }
}

