namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class MACDHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close };
        private static string[] string_0 = new string[] { "Source" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Maroon;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
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
                return "The Moving Average Convergence Divergence shows the relationship between two exponential moving averages of the underlying price.  The classical MACD indicator uses internal exponent values of 0.075 and 0.15 which correspond roughly to moving averages with lengths of 26 and 12.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MACD);
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
                return "MACD";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/MACD.ashx";
            }
        }
    }
}

