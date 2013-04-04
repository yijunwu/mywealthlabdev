namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class WilsonRSIChannelHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(0x15, 2, 100), new RangeBoundInt32(1, 1, 20), new RangeBoundDouble(30.0, 0.0, 100.0) };
        private static string[] _paramNames = new string[] { "Source", "RSI Period", "Smoothing Period", "Cord" };

        public override Color DefaultColor
        {
            get
            {
                return Color.BurlyWood;
            }
        }

        public override string Description
        {
            get
            {
                return "Relative Price Channel, from the July 2006 issue of Stocks & Commodities magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(WilsonRSIChannel);
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
                return "http://www2.wealth-lab.com/WL5Wiki/WilsonRSIChannel.ashx";
            }
        }
    }
}

