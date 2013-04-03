using System;

public class EventArgs6 : EventArgs
{
    public readonly Enum6 enum6_0;
    public readonly Enum7 enum7_0;
    public readonly string string_0;
    public readonly string string_1;
    public readonly string string_2;

    public EventArgs6(Enum6 enum6_1, Enum7 enum7_1, string string_3, string string_4)
    {
        this.enum6_0 = enum6_1;
        this.enum7_0 = enum7_1;
        this.string_0 = string_3;
        this.string_1 = string_4;
    }

    public EventArgs6(Enum6 enum6_1, Enum7 enum7_1, string string_3, string string_4, string string_5) : this(enum6_1, enum7_1, string_3, string_4)
    {
        this.string_2 = string_5;
    }
}

