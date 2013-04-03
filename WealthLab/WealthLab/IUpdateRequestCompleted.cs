namespace WealthLab
{
    using System;

    public interface IUpdateRequestCompleted
    {
        void ProcessingCompleted();
        void UpdateCompleted(Bars bars);
        void UpdateError(string symbol, Exception exception_0);
    }
}

