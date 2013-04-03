namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class KAMAHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 200) };
        private static string[] string_0 = new string[] { "Source", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Thistle;
            }
        }

        public override string Description
        {
            get
            {
                return "Returns Kaufman's Adaptive Moving Average for the Price Series specified in the Series\nparameter. KAMA is an adaptive moving average, and uses the noise level of the market\nto determine the length of the trend required to calculate the average.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(KAMA);
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
                return "";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/KAMA.ashx";
            }
        }
    }
}

