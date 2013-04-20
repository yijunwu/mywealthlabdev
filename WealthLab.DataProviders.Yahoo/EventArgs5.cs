using System;

///WYJ fix, original name: EventArgs5; candidate name: StreamingErrorEventArgs
internal class StreamingErrorEventArgs : EventArgs
{
    public readonly string errorMsg;
    public readonly string string_1;

    public StreamingErrorEventArgs(string string_2, string msg)
    {
        this.string_1 = string_2;
        this.errorMsg = msg;
    }
}

