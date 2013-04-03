namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class KalmanHelper : IndicatorHelper
    {
        private static object[] object_0 = new object[] { CoreDataSeries.Close };
        private static string[] string_0 = new string[] { "Source" };

        public override Color DefaultColor
        {
            get
            {
                return Color.DarkRed;
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
                return "The Kalman can be thought of as generating an optimal (in a linear, white noise, mean-square-error sense) estimate of a future position based on the current position of a target and an estimate of its velocity and acceleration and their uncertainties.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Kalman);
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
                return "http://www2.wealth-lab.com/WL5Wiki/Kalman.ashx";
            }
        }
    }
}

