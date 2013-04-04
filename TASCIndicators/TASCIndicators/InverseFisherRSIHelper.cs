namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class InverseFisherRSIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(4, 2, 50), new RangeBoundInt32(4, 2, 50) };
        private static string[] _paramNames = new string[] { "Source", "RSI Period", "EMA Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Navy;
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
                return "InverseFisherRSI by Sylvain Vervoot from the October 2010 issue of Stocks & Commodities magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(InverseFisherRSI);
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
                return "InverseFisherRSI";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/InverseFisherRSI.ashx";
            }
        }
    }
}

