using System;

///WYJ fix, original name EventArgs1, candidate name: RequestResultEventArgs
internal abstract class RequestResultEventArgs : EventArgs
{
    public readonly DataRequest request;

    public RequestResultEventArgs(DataRequest r)
    {
        this.request = r;
    }
}

