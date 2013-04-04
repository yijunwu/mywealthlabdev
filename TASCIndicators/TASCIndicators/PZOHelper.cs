namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class PZOHelper : IndicatorHelper
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
                return "The complementary Price Zone Oscillator (see VZO) by W.Khalil and D.Steckler originates from S&C June 2011 Traders' Tips.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PZO);
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
                return "PZO";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/PZO.ashx";
            }
        }
    }
}

