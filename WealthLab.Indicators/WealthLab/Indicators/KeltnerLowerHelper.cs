namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class KeltnerLowerHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(10, 2, 200) };
        private static string[] string_0 = new string[] { "Bars", "Period One", "Period Two" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
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
                return "The KeltnerLower has fixed bands that are plotted above and below a simple moving average of average price.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(KeltnerLower);
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
                return typeof(KeltnerUpper);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/KeltnerLower.ashx";
            }
        }
    }
}

