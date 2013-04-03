namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class CADOHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { BarDataType.Bars };
        private static string[] string_0 = new string[] { "Bars" };

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
                return "The CADO (Chaikin Advance Decline Oscillator) allows us to analyze accumulation and distribution in the convenient form of an oscillator. The principle behind this oscillator is the nearer the close is to the high, the more accumulation taken place. CADO allows you to compare price action to volume flow, to help determine market tops and bottoms.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CADO);
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
                return "CADO";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/CADO.ashx";
            }
        }
    }
}

