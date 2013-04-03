namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class FAMA : DataSeries
    {
        internal FAMA(DataSeries dataSeries_1, double double_1, double double_2, string string_1) : base(dataSeries_1, string_1)
        {
        }

        public static FAMA Series(DataSeries source, double fastLimit, double slowLimit)
        {
            string str = string.Concat(new object[] { "FAMA(", source.Description, ", ", fastLimit, ", ", slowLimit, ")" });
            MAMA.Series(source, fastLimit, slowLimit);
            return (FAMA) source.Cache[str];
        }
    }
}

