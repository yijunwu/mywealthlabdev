namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class TRIXHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 200) };
        private static string[] string_0 = new string[] { "Source", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Tomato;
            }
        }

        public override string Description
        {
            get
            {
                return "TRIX displays the percentage Rate of Change (see ROC) of a triple exponentiallysmoothed\nmoving average (EMA) over the specified Period.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TRIX);
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
                return "TRIX";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/TRIX.ashx";
            }
        }
    }
}

