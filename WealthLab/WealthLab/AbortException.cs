namespace WealthLab
{
    using System;

    public class AbortException : Exception
    {
        public AbortException() : base("Abort statement executed")
        {
        }
    }
}

