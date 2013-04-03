namespace WealthLab.DataProviders.Msn
{
    using System;

    internal class ParsingException : Exception
    {
        public ParsingException(string message) : base(message)
        {
        }
    }
}

