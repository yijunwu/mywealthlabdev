namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class Parabolic2Helper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundDouble(0.02, 0.01, 0.2), new RangeBoundDouble(0.02, 0.01, 0.2), new RangeBoundDouble(0.2, 0.1, 1.0) };
        private static string[] string_0 = new string[] { "Bars", "AccelUp", "AccelDown", "AccelMax" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Brown;
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
                return "Alternate formulation for Wealth-Lab's Standard Parabolic indicator that more closely matches AT Pro and other Technical Analysis charting sites.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Parabolic2);
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
                return "http://en.wikipedia.org/wiki/Parabolic_SAR";
            }
        }
    }
}

