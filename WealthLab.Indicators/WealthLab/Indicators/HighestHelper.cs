namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class HighestHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.High, new RangeBoundInt32(50, 2, 200) };
        private static string[] string_0 = new string[] { "Source", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override string Description
        {
            get
            {
                return "The Highest indicator returns the highest value in the underlying data series within the specified number of bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Highest);
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
                return "http://www2.wealth-lab.com/WL5Wiki/Highest.ashx";
            }
        }
    }
}

