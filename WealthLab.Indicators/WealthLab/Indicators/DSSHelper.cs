namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class DSSHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(20, 2, 200), new RangeBoundInt32(5, 2, 200) };
        private static string[] string_0 = new string[] { "Bars", "Period 1", "Period 2", "Stochastic Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.DodgerBlue;
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
                return "This oscillator is based on the current close in relation to the highest and lowest prices in a specified time interval (Figure 1).By definition, price increases as the close approaches the highest price of the interval and, conversely, decreases approaching the lowest price in the interval. A maximum is defined when price touches the highest price and then recedes.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DSS);
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
                return "DSS";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/DSS.ashx";
            }
        }
    }
}

