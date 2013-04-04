namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class FRAMAHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 200), new RangeBoundDouble(4.6, 0.0, 20.0) };
        private static string[] _paramNames = new string[] { "Source", "Period", "k" };

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
                return "Fractal Adaptive Moving Average presented by John Ehlers in October issue of the Stocks and Commodities Magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(FRAMA);
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
                return "http://www2.wealth-lab.com/WL5Wiki/FRAMA.ashx";
            }
        }
    }
}

