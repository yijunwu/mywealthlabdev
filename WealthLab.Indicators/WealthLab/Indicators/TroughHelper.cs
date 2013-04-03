namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class TroughHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundDouble(7.0, 1.0, 20.0), PeakTroughMode.Percent };
        private static string[] string_0 = new string[] { "Source", "Reversal Amount", "Peak/Trough Mode" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 3;
            }
        }

        public override string Description
        {
            get
            {
                return "Returns the most recent trough value based on a reversal amount of either a raw value, or a percentage.  Trough returns the value as of the point in time the reversal was detected, use TroughBar to find the bar of the actual Trough.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Trough);
            }
        }

        public override IList<object> ParameterDefaultValues
        {
            get
            {
                return object_0;
            }
        }

        public override IList<string> ParameterDescriptions
        {
            get
            {
                return string_0;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Trough.ashx";
            }
        }
    }
}

