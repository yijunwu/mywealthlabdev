namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class RWILowHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(8, 2, 40), new RangeBoundInt32(0x40, 2, 100) };
        private static string[] _paramNames = new string[] { "Bars", "MinPeriod", "MaxPeriod" };

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
                return "Presented in Technical Analysis Of Stocks and Commodities by Michael Poulos (see TASC, January 1992 and September 1992).";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(RWILow);
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
                return "RWILow";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/RWILow.ashx";
            }
        }
    }
}

