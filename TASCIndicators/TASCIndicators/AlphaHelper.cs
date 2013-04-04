namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class AlphaHelper : IndicatorHelper
    {
        private static string[] _descriptions = new string[] { "Source", "StdDevPeriod", "LinearRegPeriod" };
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(7, 1, 100), new RangeBoundInt32(3, 2, 100) };

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
                return "Based on an article by Rick Martinelli, published in the June 2006 issue of Stocks and Commodities Magazine. The Alpha indicator is a measure of how likely tomorrow's price will be away from normal distributed prices.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Alpha);
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
                return _descriptions;
            }
        }

        public override string TargetPane
        {
            get
            {
                return "Alpha";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Alpha.ashx";
            }
        }
    }
}

