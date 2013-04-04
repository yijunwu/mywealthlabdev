namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;
    using WealthLab.Indicators;

    public class VMPlus : DataSeries
    {
        private Bars _bars;
        private int _period;
        private DataSeries _tr;
        private DataSeries _vmPlus;

        public VMPlus(Bars bars, int period, string description) : base(bars, description)
        {
            this._bars = bars;
            this._period = period;
            if ((period < 1) || (period > (bars.Count + 1)))
            {
                period = bars.Count + 1;
            }
            base.FirstValidValue = period;
            this._tr = Sum.Series(TrueRange.Series(bars), period);
            this._vmPlus = Sum.Series(DataSeries.Abs(bars.High - (bars.Low >> 1)), period);
            for (int i = period; i < bars.Count; i++)
            {
                base[i] = this._vmPlus[i] / this._tr[i];
            }
        }

        public override void CalculatePartialValue()
        {
            int count = this._bars.Count;
            if ((this._period < 1) || (this._period > this._bars.Count))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                double num2 = Math.Abs((double) (this._bars.High.PartialValue - this._bars.Low[count - 1]));
                double partialValue = TrueRange.Series(this._bars).PartialValue;
                for (int i = count - 1; i > (count - this._period); i--)
                {
                    num2 += this._vmPlus[i];
                    partialValue += this._tr[i];
                }
                base.PartialValue = num2 / partialValue;
            }
        }

        public static VMPlus Series(Bars bars, int period)
        {
            DataSeries series;
            string key = "VM+(" + period + ")";
            if (bars.Cache.ContainsKey(key))
            {
                return (VMPlus) bars.Cache[key];
            }
            bars.Cache[key] = series = new VMPlus(bars, period, key);
            return (VMPlus) series;
        }

        public class VMPlusHelper : IndicatorHelper
        {
            private static object[] _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 5, 100) };
            private static string[] _paramNames = new string[] { "Bars", "Period" };

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
                    return "Vortex Movement (VM+) from the January 2010 issue of Stocks & Commodities magazine.";
                }
            }

            public override Type IndicatorType
            {
                get
                {
                    return typeof(VMPlus);
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
                    return "VM";
                }
            }

            public override string URL
            {
                get
                {
                    return "http://www2.wealth-lab.com/WL5Wiki/VMPlus.ashx";
                }
            }
        }
    }
}

