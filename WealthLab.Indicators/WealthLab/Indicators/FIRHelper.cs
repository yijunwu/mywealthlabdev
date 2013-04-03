namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class FIRHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, "1,2,2,1" };
        private static string[] string_0 = new string[] { "Source", "Filter" };

        public override Color DefaultColor
        {
            get
            {
                return Color.DarkSlateBlue;
            }
        }

        public override string Description
        {
            get
            {
                return "FIR returns a Finite Impulse Response filter, which applies the weight values that you specifiy as a series of numbers separated by commas to the underlying data in computing an average.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(FIR);
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

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/FIR.ashx";
            }
        }
    }
}

