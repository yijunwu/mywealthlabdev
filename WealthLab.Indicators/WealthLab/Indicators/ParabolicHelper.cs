namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class ParabolicHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundDouble(0.02, 0.01, 1.0), new RangeBoundDouble(0.02, 0.01, 1.0), new RangeBoundDouble(0.2, 0.1, 1.0) };
        private static string[] string_0 = new string[] { "Bars", "AccelUp", "AccelDown", "AccelMax" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Sienna;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Dots;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 3;
            }
        }

        public override string Description
        {
            get
            {
                return "A technical analysis strategy that uses a trailing stop and reverse method called 'SAR', or stop-and-reversal, to determine good exit and entry points.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Parabolic);
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

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Parabolic.ashx";
            }
        }
    }
}

