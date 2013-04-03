namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class WilliamsRHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundInt32(30, 2, 200) };
        private static string[] string_0 = new string[] { "Bars", "Period" };

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
                return "Momentum indicator measuring overbought and oversold levels, similar to a stochastic oscillator. Compares a stock's close to the high-low range over a certain period of time.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(WilliamsR);
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
                return "Williams %R";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/WilliamsR.ashx";
            }
        }
    }
}

