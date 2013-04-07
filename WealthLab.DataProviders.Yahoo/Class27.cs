using System;

internal class Class27 : IComparable
{
    private DateTime start;
    private DateTime dateTime_1;
    private DateTime dateTime_2;
    private Enum4 dataType;
    private string symbol;

    public Class27(string symbol)
    {
        this.symbol = symbol;
    }

    public Class27(string string_1, DateTime dateTime_3, DateTime dateTime_4, DateTime dateTime_5, Enum4 enum4_1) : this(string_1)
    {
        this.start = dateTime_3;
        this.dateTime_2 = dateTime_4;
        this.dateTime_1 = dateTime_5;
        this.dataType = enum4_1;
    }

    public string getSymbol()
    {
        return this.symbol;
    }

    public Enum4 getDataType()
    {
        return this.dataType;
    }

    public void setDataType(Enum4 enum4_1)
    {
        this.dataType = enum4_1;
    }

    ///WYJ fix, original signature: public DateTime method_3()
    public DateTime method_3()
    {
        return this.start;
    }

    public void method_4(DateTime dateTime_3)
    {
        this.start = dateTime_3;
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
        return this.getSymbol().CompareTo((object_0 as Class27).getSymbol());
    }
}

