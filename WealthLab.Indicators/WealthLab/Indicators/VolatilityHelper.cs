namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class VolatilityHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundInt32(0x18, 2, 200), new RangeBoundInt32(12, 2, 200) };
        private static string[] string_0 = new string[] { "Bars", "EMA Period", "ROC Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Yellow;
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
                return "The Chaikin's Volatility function determines the volatility of a financial data series using the percent change in a moving average of the high versus low price over a given time.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Volatility);
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
                return "Volatility";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Volatility.ashx";
            }
        }
    }
}

