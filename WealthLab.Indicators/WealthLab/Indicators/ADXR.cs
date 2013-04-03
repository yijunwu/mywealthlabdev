namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class ADXR : DataSeries
    {
        internal ADXR(Bars bars_1, int int_1, string string_1) : base(bars_1, string_1)
        {
        }

        public static ADXR Series(Bars bars, int period)
        {
            Class49 class2 = new Class49(bars, period);
            return class2.method_0();
        }
    }
}

