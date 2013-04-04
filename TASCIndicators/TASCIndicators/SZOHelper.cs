namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class SZOHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(14, 2, 300) };
        private static string[] _paramNames = new string[] { "DataSeries", "Period" };

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
                return "The Sentiment Zone Oscillator by W.Khalil measures extreme bearishness and bullishness to help identify a change in sentiment.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SZO);
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return true;
            }
        }

        public override Color OscillatorOverboughtColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 7.0;
            }
        }

        public override Color OscillatorOversoldColor
        {
            get
            {
                return Color.Green;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return -7.0;
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
                return "SZO";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/SZO.ashx";
            }
        }
    }
}

