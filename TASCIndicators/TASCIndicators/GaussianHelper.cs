namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class GaussianHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundDouble(14.0, 1.0, 200.0), new RangeBoundInt32(4, 1, 4) };
        private static string[] _paramNames = new string[] { "Source", "Period", "Poles" };

        public override Color DefaultColor
        {
            get
            {
                return Color.DarkGreen;
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
                return "This filter can be used for smoothing. It rejects high frequencies (fast movements) better than an EMA and has lower lag.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Gaussian);
            }
        }

        public override IList<object> ParameterDefaultValues
        {
            get
            {
                return _paramDefaults;
            }
        }

        public override IList<string> ParameterDescriptions
        {
            get
            {
                return _paramNames;
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
                return "http://www2.wealth-lab.com/WL5Wiki/Gaussian.ashx";
            }
        }
    }
}

