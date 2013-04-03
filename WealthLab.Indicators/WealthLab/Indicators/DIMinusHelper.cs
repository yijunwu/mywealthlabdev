namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class DIMinusHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 200) };
        private static string[] string_0 = new string[] { "Bars", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
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
                return "The DIMinus value represents downward price movement as a percentage of true range. The more each down bar's price is equal to the true range, the larger the value of the DIMinus. The DIPlus and the DIMinus are not mirror images.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DIMinus);
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
                return "http://www2.wealth-lab.com/WL5Wiki/DIMinus.ashx";
            }
        }
    }
}

