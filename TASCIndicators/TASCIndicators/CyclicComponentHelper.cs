namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class CyclicComponentHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(30, 5, 200) };
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
                return "For a comprehensive explanation please refer to John Ehlers\x00b4 article 'Modeling the Market = Building Trading Strategies' in the August 2006 issue of Stocks and Commodities Magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CyclicComponent);
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
                return "CyclicComponent";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/CyclicComponent.ashx";
            }
        }
    }
}

