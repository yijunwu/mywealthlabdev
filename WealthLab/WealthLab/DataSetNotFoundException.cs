namespace WealthLab
{
    using System;

    public class DataSetNotFoundException : Exception
    {
        public DataSetNotFoundException() : base("Dataset not found. It might have been deleted.")
        {
        }

        public DataSetNotFoundException(string message) : base(message)
        {
        }

        public DataSetNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

