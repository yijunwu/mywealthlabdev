using System;

///WYJ fix, original name: Class27
internal class DataRequest : IComparable
{
    private DateTime start;
    private DateTime snDStartDate;  ///WYJ note, start date for SnD data
    private DateTime end;
    private DataTypeEnum dataType;
    private string symbol;

    public DataRequest(string symbol)
    {
        this.symbol = symbol;
    }

    public DataRequest(string symbol, DateTime startDateTime, DateTime endDateTime, DateTime sndStartDateTime, DataTypeEnum dataType) : this(symbol)
    {
        this.start = startDateTime;
        this.end = endDateTime;
        this.snDStartDate = sndStartDateTime;
        this.dataType = dataType;
    }

    public string getSymbol()
    {
        return this.symbol;
    }

    public DataTypeEnum getDataType()
    {
        return this.dataType;
    }

    public void setDataType(DataTypeEnum enum4_1)
    {
        this.dataType = enum4_1;
    }

    ///WYJ fix, original signature: public DateTime method_3()
    public DateTime getStartDate()
    {
        return this.start;
    }

    public void setStartDate(DateTime dateTime)
    {
        this.start = dateTime;
    }

    public DateTime getSnDStartDate()
    {
        return this.snDStartDate;
    }

    public void setSnDStartDate(DateTime dateTime)
    {
        this.snDStartDate = dateTime;
    }

    public DateTime getEndDate()
    {
        return this.end;
    }

    public void setEndDate(DateTime endDateTime)
    {
        this.end = endDateTime;
    }

    int IComparable.CompareTo(object object_0)
    {
        return this.getSymbol().CompareTo((object_0 as DataRequest).getSymbol());
    }
}

