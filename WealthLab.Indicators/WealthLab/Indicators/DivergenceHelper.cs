namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class DivergenceHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close, MAType.EMAModern, new RangeBoundInt32(9, 2, 200) };
        private static string[] string_0 = new string[] { "Source", "MAType", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Histogram;
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
                return "Divergence returns the divergence of a DataSeries from a selected Moving Average.  This is usualy used in conjunction with the MACD indicator to show the divergence between the MACD line and the MACD Signal line (EMA with a period of 9).";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Divergence);
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
                return "?Divergence";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Divergence.ashx";
            }
        }
    }
}

