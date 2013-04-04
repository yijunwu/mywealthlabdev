namespace TASCIndicators
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;
    using WealthLab.Indicators;

    public class VMACDH : DataSeries
    {
        private Bars _bars;
        private double _expG;
        private double _expL;
        private double _expS;

        public VMACDH(Bars bars, int shortPeriod, int longPeriod, int signalPeriod, string description) : base(bars, description)
        {
            this._bars = bars;
            this._expS = 2.0 / (1.0 + shortPeriod);
            this._expL = 2.0 / (1.0 + longPeriod);
            this._expG = 2.0 / (1.0 + signalPeriod);
            if (((shortPeriod < 1) || (longPeriod < 1)) || ((shortPeriod > bars.Count) || (longPeriod > bars.Count)))
            {
                longPeriod = bars.Count + 1;
            }
            base.FirstValidValue = longPeriod + signalPeriod;
            if (base.FirstValidValue > bars.Count)
            {
                base.FirstValidValue = bars.Count;
            }
            DataSeries series = bars.Close * bars.Volume;
            double num = Sum.Value(shortPeriod - 1, series, shortPeriod) / ((double) shortPeriod);
            double num2 = Sum.Value(longPeriod - 1, series, longPeriod) / ((double) longPeriod);
            double num3 = Sum.Value(shortPeriod - 1, bars.Volume, shortPeriod) / ((double) shortPeriod);
            double num4 = Sum.Value(longPeriod - 1, bars.Volume, longPeriod) / ((double) longPeriod);
            for (int i = shortPeriod; i < longPeriod; i++)
            {
                num += this._expS * (series[i] - num);
                num3 += this._expS * (bars.Volume[i] - num3);
            }
            DataSeries series2 = new DataSeries(bars, string.Concat(new object[] { "VMACD(", shortPeriod, ",", longPeriod, ")" }));
            for (int j = longPeriod; j < bars.Count; j++)
            {
                num += this._expS * (series[j] - num);
                num3 += this._expS * (bars.Volume[j] - num3);
                num2 += this._expL * (series[j] - num2);
                num4 += this._expL * (bars.Volume[j] - num4);
                series2[j] = (num / num3) - (num2 / num4);
                double num1 = num / num3;
                double num9 = num2 / num4;
            }
            new DataSeries(bars, string.Concat(new object[] { "EMA(VMACD(", shortPeriod, ",", longPeriod, "))" }));
            double num7 = Sum.Value((signalPeriod + longPeriod) - 1, series2, signalPeriod) / ((double) signalPeriod);
            for (int k = signalPeriod + longPeriod; k < bars.Count; k++)
            {
                num7 += this._expG * (series2[k] - num7);
                base[k] = series2[k] - num7;
                double num10 = series2[k];
            }
        }

        public static VMACDH Series(Bars bars, int shortPeriod, int longPeriod, int signalPeriod)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "VMACDH(", shortPeriod, ",", longPeriod, ",", signalPeriod, ")" });
            if (bars.Cache.ContainsKey(key))
            {
                return (VMACDH) bars.Cache[key];
            }
            bars.Cache[key] = series = new VMACDH(bars, shortPeriod, longPeriod, signalPeriod, key);
            return (VMACDH) series;
        }

        public class VMACDHHelper : IndicatorHelper
        {
            private static object[] _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(12, 2, 0x19), new RangeBoundInt32(0x1a, 10, 50), new RangeBoundInt32(9, 2, 0x19) };
            private static string[] _paramNames = new string[] { "Bars", "Short Period", "Long Period", "Signal Period" };

            public override Color DefaultColor
            {
                get
                {
                    return Color.Maroon;
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
                    return 1;
                }
            }

            public override string Description
            {
                get
                {
                    return "Volume-Weighted MACD Histogram from the October 2009 issue of Stocks & Commodities magazine.";
                }
            }

            public override Type IndicatorType
            {
                get
                {
                    return typeof(VMACDH);
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
                    return "VMACDH";
                }
            }

            public override string URL
            {
                get
                {
                    return "http://www2.wealth-lab.com/WL5Wiki/VMACDH.ashx";
                }
            }
        }
    }
}

