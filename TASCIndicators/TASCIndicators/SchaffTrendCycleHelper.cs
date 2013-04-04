namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class SchaffTrendCycleHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 20), new RangeBoundInt32(0x17, 5, 40), new RangeBoundInt32(50, 30, 100) };
        private static string[] _paramNames = new string[] { "Source", "TC Length", "MACD Period1", "MACD Period2" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Gold;
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
                return "Schaff Trend Cycle from the April 2010 issue of Technical Analysis of Stocks & Commodities magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SchaffTrendCycle);
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
                return "SchaffTrendCycle";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/SchaffTrendCycle.ashx";
            }
        }
    }
}

