namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class PCRiSlowIFTHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(4, 1, 10), new RangeBoundInt32(2, 1, 10), new RangeBoundInt32(8, 2, 20) };
        private static string[] _paramNames = new string[] { "Raw P/C Ratio", "Rainbow Period", "WMA Smooth Period", "RSI Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
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
                return "PCRiSlowIFT is the Slow Put/Call Ratio indicator Inverse Fisher Transform from the November 2011 issue of TASC Magazine.  Use the CBOE Provider to pass the raw Put/Call Ratio for Equities as the DataSeries input.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PCRiSlowIFT);
            }
        }

        public override IList<object> ParameterDefaultValues
        {
            get
            {
                return _paramDefaults;
            }
        }

        public override IList<string> ParameterDescriptions
        {
            get
            {
                return _paramNames;
            }
        }

        public override string TargetPane
        {
            get
            {
                return "PCRiSlowIFTPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/PCRiSlowIFT.ashx";
            }
        }
    }
}

