namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public static class EMAModern
    {
        public static DataSeries Series(DataSeries source, int period)
        {
            return EMA.Series(source, period, EMACalculation.Modern);
        }
    }
}

