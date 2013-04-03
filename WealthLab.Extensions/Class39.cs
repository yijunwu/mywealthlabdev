using System;

internal class Class39
{
    private Enum6 enum6_0;
    private Enum7 enum7_0;
    private string string_0;
    private string string_1;
    private string string_2;

    public Class39()
    {
    }

    public Class39(string string_3, Enum6 enum6_1, Enum7 enum7_1, string string_4)
    {
        this.string_0 = string_3;
        this.enum6_0 = enum6_1;
        this.enum7_0 = enum7_1;
        this.string_2 = string_4;
    }

    public Class39(string string_3, Enum6 enum6_1, Enum7 enum7_1, string string_4, string string_5) : this(string_3, enum6_1, enum7_1, string_4)
    {
        this.string_1 = string_5;
    }

    public string method_0()
    {
        return this.string_0;
    }

    public void method_1(string string_3)
    {
        this.string_0 = string_3;
    }

    public Enum6 method_2()
    {
        return this.enum6_0;
    }

    public void method_3(Enum6 enum6_1)
    {
        this.enum6_0 = enum6_1;
    }

    public Enum7 method_4()
    {
        return this.enum7_0;
    }

    public void method_5(Enum7 enum7_1)
    {
        this.enum7_0 = enum7_1;
    }

    public string method_6()
    {
        return this.string_1;
    }

    public void method_7(string string_3)
    {
        this.string_1 = string_3;
    }

    public string method_8()
    {
        return this.string_2;
    }

    public void method_9(string string_3)
    {
        this.string_2 = string_3;
    }
}

