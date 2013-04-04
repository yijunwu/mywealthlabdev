namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class PCRiSlowHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(5, 1, 10), new RangeBoundInt32(1, 1, 10) };
        private static string[] _paramNames = new string[] { "Raw P/C Ratio", "Rainbow Period", "WMA Smooth Period" };

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
                return "PCRiSlow is the Slow Put/Call Ratio indicator for Equities from the November 2011 issue of TASC Magazine.  Use the CBOE Provider to pass the raw Put/Call Ratio for Equities as the DataSeries input.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PCRiSlow);
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
                return "PCRiSlowPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/PCRiSlow.ashx";
            }
        }
    }
}

