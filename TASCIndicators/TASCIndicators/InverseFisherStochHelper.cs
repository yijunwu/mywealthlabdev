namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class InverseFisherStochHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(30, 5, 100), new RangeBoundInt32(5, 2, 10) };
        private static string[] _paramNames = new string[] { "DataSeries", "Stoch Period", "Smooth Period" };

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
                return "InverseFisherStoch is Sylvain Vervoort's Inverse Fisher Stochastic of a DataSeries used in the November 2011 issue of TASC Magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(InverseFisherStoch);
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
                return "InverseFisherStoch";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/InverseFisherStoch.ashx";
            }
        }
    }
}

