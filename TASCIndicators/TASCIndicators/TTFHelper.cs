namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class TTFHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(15, 1, 100) };
        private static string[] _paramNames = new string[] { "Bars", "Period" };

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
                return "Trend Trigger Factor from the December 2004 issue of Stocks & Commodities magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TTF);
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
                return "TTF";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/TTF.ashx";
            }
        }
    }
}

