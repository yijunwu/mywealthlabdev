namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class TrueRangeHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars };
        private static string[] string_0 = new string[] { "Bars" };

        public override Color DefaultColor
        {
            get
            {
                return Color.OliveDrab;
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
                return "True Range is the greatest of the following: (1) distance from current high to current low (2) distance from current high to previous close (3) distance from current low to the previous close.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TrueRange);
            }
        }

        public override IList<object> ParameterDefaultValues
        {
            get
            {
                return object_0;
            }
        }

        public override IList<string> ParameterDescriptions
        {
            get
            {
                return string_0;
            }
        }

        public override string TargetPane
        {
            get
            {
                return "TrueRange";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/TrueRange.ashx";
            }
        }
    }
}

