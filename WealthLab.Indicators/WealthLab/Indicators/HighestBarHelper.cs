namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class HighestBarHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.High, new RangeBoundInt32(50, 2, 200) };
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
                return "Highest Bar returns the bar which has the highest value within a given period.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HighestBar);
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
                return "HighestBar";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/HighestBar.ashx";
            }
        }
    }
}

