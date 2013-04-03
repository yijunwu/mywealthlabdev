namespace WealthLab
{
    using System;

    public class DataSetScaleMismatchException : Exception
    {
        public DataSetScaleMismatchException() : base("The source dataset's scale does not match the custom index's.")
        {
        }

        public DataSetScaleMismatchException(string message) : base(message)
        {
        }

        public DataSetScaleMismatchException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

