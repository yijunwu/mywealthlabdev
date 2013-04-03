namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class CumDownHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundInt32(4, 1, 200) };
        private static string[] string_0 = new string[] { "Source", "Lookback" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
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
                return "CumDown returns the number of consecutive bars that the underlying value was less than the value a certain number of bars ago.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CumDown);
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
                return "CumUpDown";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/CumDown.ashx";
            }
        }
    }
}

