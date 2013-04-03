namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class TroughBar : DataSeries
    {
        internal TroughBar(DataSeries dataSeries_1, string string_1) : base(dataSeries_1, string_1)
        {
        }

        public static TroughBar Series(DataSeries source, double reversalAmount, PeakTroughMode mode)
        {
            PeakTroughCalculator calculator = new PeakTroughCalculator(source, reversalAmount, mode);
            return calculator.TroughBar;
        }

        public static double Value(int int_1, DataSeries source, double reversalAmount, PeakTroughMode mode)
        {
            PeakTroughCalculator.smethod_0(int_1, source, reversalAmount, mode);
            return (double) PeakTroughCalculator.int_1;
        }
    }
}

