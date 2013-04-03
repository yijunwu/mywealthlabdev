namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class EMAHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundInt32(60, 2, 200), EMACalculation.Modern };
        private static string[] string_0 = new string[] { "Source", "Period", "Calculation Type" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Indigo;
            }
        }

        public override string Description
        {
            get
            {
                return "EMA returns the Exponential Moving Average.  This is a way of averaging a set of values but giving more weight to the data that occurs more recently.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(EMA);
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

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/EMA.ashx";
            }
        }
    }
}

