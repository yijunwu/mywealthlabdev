namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class HVHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 200), 0xfc };
        private static string[] string_0 = new string[] { "Source", "Period", "Bars per Year Span" };

        public override Color DefaultColor
        {
            get
            {
                return Color.DarkMagenta;
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
                return "Historical Volatility computes a standard deviation of the logarithm of changes in the underlying value.  Increases in HV reflect sharp moves in the underlying data.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HV);
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
                return "HV";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/HV.ashx";
            }
        }
    }
}

