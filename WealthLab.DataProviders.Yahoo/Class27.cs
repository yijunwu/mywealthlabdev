using System;

internal class Class27 : IComparable
{
    private DateTime dateTime_0;
    private DateTime dateTime_1;
    private DateTime dateTime_2;
    private Enum4 enum4_0;
    private string string_0;

    public Class27(string string_1)
    {
        this.string_0 = string_1;
    }

    public Class27(string string_1, DateTime dateTime_3, DateTime dateTime_4, DateTime dateTime_5, Enum4 enum4_1) : this(string_1)
    {
        this.dateTime_0 = dateTime_3;
        this.dateTime_2 = dateTime_4;
        this.dateTime_1 = dateTime_5;
        this.enum4_0 = enum4_1;
    }

    public string method_0()
    {
        return this.string_0;
    }

    public Enum4 method_1()
    {
        return this.enum4_0;
    }

    public void method_2(Enum4 enum4_1)
    {
        this.enum4_0 = enum4_1;
    }

    public DateTime method_3()
    {
        return this.dateTime_0;
    }

    public void method_4(DateTime dateTime_3)
    {
        this.dateTime_0 = dateTime_3;
    }

    public DateTime method_5()
    {
        return this.dateTime_1;
    }

    public void method_6(DateTime dateTime_3)
    {
        this.dateTime_1 = dateTime_3;
    }

    public DateTime method_7()
    {
        return this.dateTime_2;
    }

    public void method_8(DateTime dateTime_3)
    {
        this.dateTime_2 = dateTime_3;
    }

    int IComparable.CompareTo(object object_0)
    {
        return this.method_0().CompareTo((object_0 as Class27).method_0());
    }
}

