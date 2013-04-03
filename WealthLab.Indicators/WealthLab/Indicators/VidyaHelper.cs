namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class VidyaHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 200), new RangeBoundDouble(0.1, 0.01, 1.0) };
        private static string[] string_0 = new string[] { "Source", "StdDev Period", "Alpha" };

        public override Color DefaultColor
        {
            get
            {
                return Color.SkyBlue;
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
                return "Vidya returns Tushar Chande's Variable Index Dynamic Average. Vidya is similar to an Exponential Moving Average, but uses a different period for each bar of calculation.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Vidya);
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
                return "";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Vidya.ashx";
            }
        }
    }
}

