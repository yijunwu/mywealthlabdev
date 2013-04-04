namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class PCIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(30, 2, 200) };
        private static string[] _paramNames = new string[] { "Source", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.DarkGreen;
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
                return "Phase Change Index (by M.H.Pee) from the May 2004 issue of Stocks & Commodities magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PCI);
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
                return "PCI";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/PCI.ashx";
            }
        }
    }
}

