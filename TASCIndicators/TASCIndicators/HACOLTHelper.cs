namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class HACOLTHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(0x37, 2, 100), new RangeBoundDouble(1.1, 0.01, 5.0), new RangeBoundInt32(2, 1, 100), new RangeBoundInt32(60, 1, 200) };
        private static string[] _paramNames = new string[] { "Bars", "TEMA Period", "Candle Size factor", "Setup timeout", "Shorting LT Average" };

        public override Color DefaultColor
        {
            get
            {
                return Color.BlueViolet;
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
                return "HACOLT (Heikin-Ashi Candlestick Oscillator Long-Term) by Sylvain Vervoort from the July 2012 issue of Technical Analysis of Stocks & Commodities magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HACOLT);
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
                return "HACOLT";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/HACOLT.ashx";
            }
        }
    }
}

