namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class PTBarHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundDouble(3.3, 0.01, 25.0) };
        private static string[] _paramNames = new string[] { "Source", "ReversalPct" };

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
                return "Peaks&Troughs Indicator (PTBar) - as described in November 2006 Stocks&Commodities (Siligardos' article on \"Active Trend Lines\")";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PTBar);
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
                return "PTBar";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/PTBar.ashx";
            }
        }
    }
}

