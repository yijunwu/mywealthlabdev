namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class ATRModifiedHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(5, 2, 100) };
        private static string[] _paramNames = new string[] { "Bars", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.LimeGreen;
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
                return "ATRModified (Modified ATR) from the June 2009 issue of Technical Analysis of Stocks & Commodities magazine sets limits on extreme moves in True Range.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ATRModified);
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
                return "ATR";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ATRModified.ashx";
            }
        }
    }
}

