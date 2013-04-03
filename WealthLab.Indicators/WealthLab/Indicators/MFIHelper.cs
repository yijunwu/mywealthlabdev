namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class MFIHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundInt32(20, 1, 200) };
        private static string[] string_0 = new string[] { "Bars", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.IndianRed;
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
                return "A momentum indicator that measures the strength of money in and out of a security. A divergence between the MFI and price trend can be interpreted as a possible trend reversal.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MFI);
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
                return "MFI";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/MFI.ashx";
            }
        }
    }
}

