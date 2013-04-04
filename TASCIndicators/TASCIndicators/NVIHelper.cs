namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class NVIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { BarDataType.Bars };
        private static string[] _paramNames = new string[] { "Bars" };

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
                return "NVI (Negative Volume Index) from the April 2003 issue of Technical Analysis of Stocks & Commodities magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(NVI);
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
                return "NVI";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/NVI.ashx";
            }
        }
    }
}

