namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class SpearmanHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 5, 300) };
        private static string[] _paramNames = new string[] { "Data Series", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override string Description
        {
            get
            {
                return "Spearman correlation indicator by Dan Valcu is a statistical tool that helps determine trend strength and turning points.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Spearman);
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return true;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return -80.0;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return 80.0;
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
                return "SpearmanPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Spearman.ashx";
            }
        }
    }
}

