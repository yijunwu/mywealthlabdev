using System;

internal class EventArgs11 : EventArgs
{
    public readonly int int_0;
    public readonly int int_1;
    public readonly int int_2;
    public readonly long long_0;
    public readonly long long_1;
    public readonly string string_0;
    public readonly string string_1;

    public EventArgs11(long long_2, long long_3, int int_3, int int_4, int int_5, string string_2, string string_3)
    {
        this.long_0 = long_2;
        this.long_1 = long_3;
        this.int_0 = int_3;
        this.int_1 = int_4;
        this.int_2 = int_5;
        this.string_0 = string_2;
        this.string_1 = string_3;
    }
}

