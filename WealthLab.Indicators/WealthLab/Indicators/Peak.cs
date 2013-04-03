namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class Peak : DataSeries
    {
        internal Peak(DataSeries dataSeries_1, string string_1) : base(dataSeries_1, string_1)
        {
        }

        public static Peak Series(DataSeries source, double reversalAmount, PeakTroughMode mode)
        {
            PeakTroughCalculator calculator = new PeakTroughCalculator(source, reversalAmount, mode);
            return calculator.Peak;
        }

        public static double Value(int int_1, DataSeries source, double reversalAmount, PeakTroughMode mode)
        {
            PeakTroughCalculator.smethod_0(int_1, source, reversalAmount, mode);
            return PeakTroughCalculator.double_3;
        }
    }
}

