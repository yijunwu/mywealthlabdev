using System;
using WealthLab;

///WYJ fix, original name EventArgs4, candidate name: StreamingDataEventArgs
internal class StreamingDataEventArgs : EventArgs
{
    public readonly double open;
    public readonly double high;
    public readonly double low;
    public readonly Quote quote;

    public StreamingDataEventArgs(Quote quote_1, double open, double high, double low)
    {
        this.quote = quote_1;
        this.open = open;
        this.high = high;
        this.low = low;
    }
}

