namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class KSTHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(15, 2, 200), new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(20, 2, 200), new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(30, 2, 200), new RangeBoundInt32(15, 2, 200) };
        private static string[] string_0 = new string[] { "Source", "ROC1", "EMA1", "ROC2", "EMA2", "ROC3", "EMA3", "ROC4", "EMA4" };

        public override Color DefaultColor
        {
            get
            {
                return Color.DarkSalmon;
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
                return "Martin Pring's Know-Sure-Thing.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(KST);
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
                return "KST";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www.pring.com/movieweb/daily_kst.htm";
            }
        }
    }
}

