namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class VHFHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundInt32(0x18, 2, 200) };
        private static string[] string_0 = new string[] { "Source", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.MidnightBlue;
            }
        }

        public override string Description
        {
            get
            {
                return "The Vertical Horizontal Filter is used to determine if prices are trending or are in a\ncongestion stage. It is calculated by dividing the difference in the sums of highest and\nlowest values by the sum of the absolute values of daily price differences. Typically a\nperiod of 28 is used for VHF.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(VHF);
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
                return "VHF";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/VHF.ashx";
            }
        }
    }
}

