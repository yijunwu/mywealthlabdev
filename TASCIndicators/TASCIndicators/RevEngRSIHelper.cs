namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class RevEngRSIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 200), new RangeBoundDouble(50.0, 0.0, 100.0) };
        private static string[] _paramNames = new string[] { "Source", "Period", "RSIVal" };

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
                return "From the article Reverse Engineering the RSI in the June 2003 issue of Stocks & Commodities magazine. The RevEngRSI indicator returns the price value required for the RSI to move to the specified value on the following bar.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(RevEngRSI);
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
                return "http://www2.wealth-lab.com/WL5Wiki/RevEngRSI.ashx";
            }
        }
    }
}

