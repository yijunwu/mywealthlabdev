namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class ADXRHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 200) };
        private static string[] string_0 = new string[] { "Bars", "Period" };

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
                return 3;
            }
        }

        public override string Description
        {
            get
            {
                return "ADXR (Average Directional Movement Index Rating) attempts to measure the strength of price movement in positive and negative directions, as well as the overall strength of the trend. The ADXR component is simply a special type of moving average (WilderMA) applied to the ADX indicator.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ADXR);
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
                return "http://www2.wealth-lab.com/WL5Wiki/ADXR.ashx";
            }
        }
    }
}

