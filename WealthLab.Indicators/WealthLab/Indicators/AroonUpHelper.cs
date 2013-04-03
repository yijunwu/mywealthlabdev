namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class AroonUpHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 200) };
        private static string[] string_0 = new string[] { "Source", "Period" };

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
                return "Aroon Up measures the strength of an uptrend, and returns the time (as a percentage) that it has taken for the underlying to reach the highest point over a given time period.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AroonUp);
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
                return "Aroon";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/AroonUp.ashx";
            }
        }
    }
}

