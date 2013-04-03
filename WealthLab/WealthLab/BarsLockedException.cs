namespace WealthLab
{
    using System;

    public class BarsLockedException : Exception
    {
        public BarsLockedException() : base("Bars object has been locked and cannot be modified")
        {
        }
    }
}

