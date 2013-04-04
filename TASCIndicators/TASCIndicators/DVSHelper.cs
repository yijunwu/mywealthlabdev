namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class DVSHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(50, 4, 300) };
        private static string[] _paramNames = new string[] { "Source", "Period" };

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
                return "Dynamic Volatility Sigma from the May 2005 issue of Stocks & Commodities magazine. DVS is used in the calculation of the Universal Cycle Indicator and is the standard deviation of minor plus sub-minor price oscillations with respect to a minor term centered moving average. The centered averages introduce a half-cycle minor term lag in the standard deviation parameter.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DVS);
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
                return "DVS";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/DVS.ashx";
            }
        }
    }
}

