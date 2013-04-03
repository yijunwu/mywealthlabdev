namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class OBVHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars };
        private static string[] string_0 = new string[] { "Bars" };

        public override Color DefaultColor
        {
            get
            {
                return Color.GreenYellow;
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
                return "On-balance Volume is a momentum indicator that measures positive and negative volume flow.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(OBV);
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
                return "OBV";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/OBV.ashx";
            }
        }
    }
}

