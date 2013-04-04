namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class CandleCodeHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { BarDataType.Bars };
        private static string[] _paramNames = new string[] { "Bars" };

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
                return "In the March, 2001 issue of Stocks & Commodities magazine, Viktor Likhovidov shares a method of coding candlesticks.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CandleCode);
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
                return "CandleCode";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/CandleCode.ashx";
            }
        }
    }
}

