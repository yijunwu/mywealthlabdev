namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class SARSILowerHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(50, 2, 300), new RangeBoundDouble(2.0, 0.1, 20.0) };
        private static string[] _paramNames = new string[] { "DataSeries", "Lookback period", "Multiplier" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Purple;
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
                return "David Sepiashvili's Self-Adjusting RSI (from February 2006 Stocks & Commodities magazine) presents a technique to adjust the traditional RSI overbought and oversold thresholds to ensure that 70-80% of RSI values lie between the two thresholds.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SARSILower);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(SARSIUpper);
            }
        }

        public override string TargetPane
        {
            get
            {
                return "RSI";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/SelfAdjustingRSI.ashx";
            }
        }
    }
}

