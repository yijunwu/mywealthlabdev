namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class ADXHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 200) };
        private static string[] string_0 = new string[] { "Bars", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Purple;
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
                return "The ADX attempts to measure the strength of price movement in positive and negative direction using the DIPlus and DIMinus indicators along with the ADX.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ADX);
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
                return "ADX";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ADX.ashx";
            }
        }
    }
}

