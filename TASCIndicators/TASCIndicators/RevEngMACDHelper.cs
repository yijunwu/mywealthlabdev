namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class RevEngMACDHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(12, 2, 300), new RangeBoundInt32(0x1a, 2, 300) };
        private static string[] _paramNames = new string[] { "Data Series", "MACD Period 1", "MACD Period 2" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
            }
        }

        public override string Description
        {
            get
            {
                return "From S&C January 2012 article Reversing MACD by Johnny Dough. Returns the price value required for the MACD to move to a value on the following bar.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(RevEngMACD);
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return false;
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

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5WIKI/RevEngMACD.ashx";
            }
        }
    }
}

