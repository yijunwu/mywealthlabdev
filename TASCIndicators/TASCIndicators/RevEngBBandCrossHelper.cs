namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class RevEngBBandCrossHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 200), new RangeBoundDouble(2.0, 0.5, 4.0), true };
        private static string[] _paramNames = new string[] { "Source", "Period", "Deviations", "Upper" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 1;
            }
        }

        public override string Description
        {
            get
            {
                return "Reverse-engineered Bollinger Band cross based on the WL4 indicators by gbeltrame. Shows the DataSeries price required to cross the specified BBand on the next bar.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(RevEngBBandCross);
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
                return "http://www2.wealth-lab.com/WL5Wiki/RevEngBBandCross.ashx";
            }
        }
    }
}

