namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public static class EMALegacy
    {
        public static DataSeries Series(DataSeries source, int period)
        {
            return EMA.Series(source, period, EMACalculation.Legacy);
        }
    }
}

