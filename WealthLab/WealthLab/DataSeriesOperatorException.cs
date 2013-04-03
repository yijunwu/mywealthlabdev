namespace WealthLab
{
    using System;

    public class DataSeriesOperatorException : InvalidOperationException
    {
        public DataSeriesOperatorException() : base("DataSeries do not have equal number of values for mathematical operation")
        {
        }
    }
}

