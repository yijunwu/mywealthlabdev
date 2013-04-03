namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class StochDHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 200), new RangeBoundInt32(5, 2, 20) };
        private static string[] string_0 = new string[] { "Bars", "Period", "Smooth" };

        public override Color DefaultColor
        {
            get
            {
                return Color.OliveDrab;
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
                return "The stochastic oscillator is a momentum indicator used in technical analysis, introduced by George Lane in the 1950s, to compare the closing price of a commodity to its price range over a given time span.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(StochD);
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return true;
            }
        }

        public override IList<object> ParameterDefaultValues
        {
            get
            {
                return object_0;
            }
        }

        public override IList<string> ParameterDescriptions
        {
            get
            {
                return string_0;
            }
        }

        public override string TargetPane
        {
            get
            {
                return "StochK";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/StochD.ashx";
            }
        }
    }
}

