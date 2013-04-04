namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class UCIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(0x19, 3, 200), new RangeBoundInt32(12, 4, 100) };
        private static string[] _paramNames = new string[] { "Source", "Period", "VolaPeriod" };

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
                return "Universal Cycle Index (UCI) from the May 2005 issue of Stocks & Commodities magazine.  by Stuart Belknap, PhD: \"The UCI is nothing more than a normalized Moving Average Converging/Diverging (MACD) indicator.\"";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(UCI);
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
                return "UCI";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/UCI.ashx";
            }
        }
    }
}

