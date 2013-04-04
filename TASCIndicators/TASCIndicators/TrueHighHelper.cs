namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class TrueHighHelper : IndicatorHelper
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
                return "";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TrueHigh);
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
                return "P";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/TrueHigh.ashx";
            }
        }
    }
}

