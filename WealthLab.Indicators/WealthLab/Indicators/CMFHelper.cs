namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class CMFHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars, new RangeBoundInt32(0x15, 1, 200) };
        private static string[] string_0 = new string[] { "Bars", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
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
                return "Developed by Marc Chaikin, the Chaikin Money Flow oscillator is calculated from the daily readings of the Accumulation/Distribution Line. The basic premise behind the Accumulation Distribution Line is that the degree of buying or selling pressure can be determined by the location of the Close relative to the High and Low for the corresponding period (Closing Location Value).";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CMF);
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
                return "CMF";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/CMF.ashx";
            }
        }
    }
}

