namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class LowestBarHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Low, new RangeBoundInt32(50, 2, 200) };
        private static string[] string_0 = new string[] { "Source", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.MediumSeaGreen;
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
                return "Lowest Bar returns the bar which has the lowest value within a given period.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(LowestBar);
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
                return "LowestBar";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/LowestBar.ashx";
            }
        }
    }
}

