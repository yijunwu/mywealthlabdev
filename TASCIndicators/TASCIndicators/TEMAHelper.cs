namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class TEMAHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(14, 1, 200), EMACalculation.Modern };
        private static string[] _paramNames = new string[] { "Source", "Period", "EMA Calc Type" };

        public override Color DefaultColor
        {
            get
            {
                return Color.DarkMagenta;
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
                return "TEMA is the Triple-smoothed Exponential Moving Average based on TECHNICAL ANALYSIS FROM A TO Z, 2nd Ed., pg. 328-330.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TEMA);
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
                return "http://www2.wealth-lab.com/WL5Wiki/TEMA.ashx";
            }
        }
    }
}

