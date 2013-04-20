using System;

///WYJ fix, original name EventArgs3, candidate name: StaticErrorEventArgs
internal class StaticErrorEventArgs : RequestResultEventArgs
{
    public readonly Exception exception;

    public StaticErrorEventArgs(DataRequest request, Exception exception_1) : base(request)
    {
        this.exception = exception_1;
    }
}

