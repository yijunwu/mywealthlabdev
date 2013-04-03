namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class AveragePriceHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars };
        private static string[] string_0 = new string[] { "Bars" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Navy;
            }
        }

        public override string Description
        {
            get
            {
                return "Average Price returns (High + Low) / 2 for each bar.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AveragePrice);
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
                return "P";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/AveragePrice.ashx";
            }
        }
    }
}

