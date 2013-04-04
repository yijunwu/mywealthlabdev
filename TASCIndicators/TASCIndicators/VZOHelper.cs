namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class VZOHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) };
        private static string[] _paramNames = new string[] { "Bars", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.DarkBlue;
            }
        }

        public override string Description
        {
            get
            {
                return "The Volume Zone Oscillator by W.Khalil and D.Steckler takes into account both time and volume fluctuations from bearish to bullish and is designed to work in trending and non-trending conditions.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(VZO);
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
                return "VZO";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/VZO.ashx";
            }
        }
    }
}

