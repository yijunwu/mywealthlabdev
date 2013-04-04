namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class RevEngSMA_TCHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(30, 2, 300) };
        private static string[] _paramNames = new string[] { "Source", "Period1", "Period2" };

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
                return "The RevEngSMA_TC indicator was derived by Tsokakis in the February 2007 issue of Stocks & Commodities magazine. He observed and demonstrated that crossovers of this indicator occurred one bar earlier than crossovers of SMA indicators using the same periods a very high percentage of the time.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(RevEngSMA_TC);
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
                return "http://www2.wealth-lab.com/WL5Wiki/RevEngSMA_TC.ashx";
            }
        }
    }
}

