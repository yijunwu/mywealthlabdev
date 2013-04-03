namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class StdDevHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundInt32(14, 2, 200), StdDevCalculation.Population };
        private static string[] string_0 = new string[] { "Source", "Period", "Calculation Type" };

        public override Color DefaultColor
        {
            get
            {
                return Color.OrangeRed;
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
                return "Standard Deviation is the root mean square deviation of the values from their arithmetic mean.  It describes how closesly the data is clustered around the average (mean).";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(StdDev);
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
                return "StdDev";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/StdDev.ashx";
            }
        }
    }
}

