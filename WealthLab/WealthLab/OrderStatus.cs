namespace WealthLab
{
    using System;

    public enum OrderStatus
    {
        Staged,
        Submitted,
        Active,
        PartialFilled,
        Filled,
        CancelPending,
        Canceled,
        Error,
        Unknown,
        ErrorCancelReplace
    }
}

