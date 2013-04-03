namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class MoneyFlowHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars };
        private static string[] string_0 = new string[] { "Bars" };

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
                return "Money flow in technical analysis is typical price multiplied by volume, a kind of approximation to the dollar value of a day's trading.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MoneyFlow);
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
                return "MoneyFlow";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/MoneyFlow.ashx";
            }
        }
    }
}

