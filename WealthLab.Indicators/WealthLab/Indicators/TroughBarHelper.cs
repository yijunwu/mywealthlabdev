namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class TroughBarHelper : IndicatorHelper
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
                return 2;
            }
        }

        public override string Description
        {
            get
            {
                return "Returns the bar number that the most recent Trough occurred on, based on a reversal amount of either a raw value, or a percentage.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TroughBar);
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

        public override string TargetPane
        {
            get
            {
                return "PeakTroughBar";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/TroughBar.ashx";
            }
        }
    }
}

