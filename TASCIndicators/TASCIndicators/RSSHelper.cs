namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class RSSHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(40, 2, 300), new RangeBoundInt32(5, 2, 40), new RangeBoundInt32(5, 2, 40) };
        private static string[] _paramNames = new string[] { "Source", "Fast SMA Period", "Slow SMA Period", "RSI Period", "Smoothing Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.BlueViolet;
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
                return "Relative Spread Strength indicator, from Ian Copsey's article in the October 2006 issue of Stocks & Commodities magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(RSS);
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
                return "RSS";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/RSS.ashx";
            }
        }
    }
}

