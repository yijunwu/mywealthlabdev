namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class MidasHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(10, 0, 0x2710) };
        private static string[] _paramNames = new string[] { "Bars", "Start Bar" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 1;
            }
        }

        public override string Description
        {
            get
            {
                return "Midas (Market Interpretation Data Analysis System) from the September 2008 issue of Technical Analysis of Stocks & Commodities magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Midas);
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
                return "http://www2.wealth-lab.com/WL5Wiki/Midas.ashx";
            }
        }
    }
}

