namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class TrendLinePeaksHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundDouble(3.0, 0.1, 20.0), false };
        private static string[] _paramNames = new string[] { "Source", "ReversePct", "UseLogScale" };

        public override Color DefaultColor
        {
            get
            {
                return Color.DarkGreen;
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
                return "Calculates the projection of the trend line resulting from the two most recent peaks, which are determined by the ReversePct (reversal percentage). Pass true to the boolean parameter UseLogScale to determine the projection on a semi-log scale.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TrendLinePeaks);
            }
        }

        public override IList<object> ParameterDefaultValues
        {
            get
            {
                return _paramDefaults;
            }
        }

        public override IList<string> ParameterDescriptions
        {
            get
            {
                return _paramNames;
            }
        }

        public override string TargetPane
        {
            get
            {
                return "";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/TrendLinePeaks.ashx";
            }
        }
    }
}

