namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class RSIBandHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(14, 2, 200), new RangeBoundDouble(30.0, 0.0, 100.0) };
        private static string[] _paramNames = new string[] { "Source", "Period", "RSI Target Level" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override string Description
        {
            get
            {
                return "Based on the indicator by Fran\x00e7ois Bertrand published in the April 2008 issue of Stocks and Commodities Magazine. RSI bands are calculated by answering the question, \"What price should the stock have to reach today to be considered overbought/oversold, given yesterday’s RSI?\"";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(RSIBand);
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
                return "P";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/RSIBand.ashx";
            }
        }
    }
}

