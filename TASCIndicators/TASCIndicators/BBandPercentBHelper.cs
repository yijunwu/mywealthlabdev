namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class BBandPercentBHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 100), StdDevCalculation.Population };
        private static string[] _paramNames = new string[] { "Source", "Period", "SD Calc Type" };

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
                return 2;
            }
        }

        public override string Description
        {
            get
            {
                return "Bollinger %b referenced in the May 2010 issue of Technical Analysis of Stocks & Commodities magazine.  See also BBandPercentBSmooothed.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(BollingerPctB);
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
                return "BBandPercentB";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/BBandPercentB.ashx";
            }
        }
    }
}

