namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class MAMAHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundDouble(0.5, 0.0, 10.0), new RangeBoundDouble(0.05, 0.0, 1.0) };
        private static string[] string_0 = new string[] { "Source", "FastLimit", "SlowLimit" };

        public override Color DefaultColor
        {
            get
            {
                return Color.IndianRed;
            }
        }

        public override string Description
        {
            get
            {
                return "MAMA stands for MESA Adaptive Moving Average. MAMA is an adaptive exponential moving average.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MAMA);
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
                return "http://www2.wealth-lab.com/WL5Wiki/MAMA.ashx";
            }
        }
    }
}

