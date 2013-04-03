namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class BBandUpperHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 200), new RangeBoundDouble(2.0, 0.5, 20.0) };
        private static string[] string_0 = new string[] { "Source", "Period", "Standard Deviations" };

        public override Color DefaultColor
        {
            get
            {
                return Color.LightSteelBlue;
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
                return "Bollinger Bands are price envelopes based on a number of Standard Deviations above and below a moving average of the underlying data series.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(BBandUpper);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(BBandLower);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/BBandUpper.ashx";
            }
        }
    }
}

