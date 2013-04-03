namespace WealthLab.Rules.Candlesticks
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using WealthLab;
    using WealthLab.Indicators;

    public static class CandlePattern
    {
        private static DataSeries _atr;
        private static Bars _bars;
        private static int _barsCountMinus1;
        private static int _barsHashCode;
        private static DataSeries _CHL;
        private static DataSeries _CmL;
        private static DataSeries _HmL;
        private static DataSeries _OHL;
        private static DataSeries _OmL;
        private static Color highlightColor = Color.FromArgb(50, Color.Fuchsia);
        private const int StartBar = 10;
        private const int StartBarNoAtr = 2;

        private static void AnnotateText(WealthScript w, int bar, string s, bool above)
        {
            Font font = new Font("Arial", 7f, FontStyle.Regular);
            string[] strArray = s.Split(new char[] { ' ' });
            if (above)
            {
                for (int i = strArray.GetUpperBound(0); i >= 0; i--)
                {
                    w.AnnotateBar(strArray[i], bar, true, Color.Navy, Color.Empty, font);
                }
            }
            else
            {
                for (int j = 0; j <= strArray.GetUpperBound(0); j++)
                {
                    w.AnnotateBar(strArray[j], bar, false, Color.Navy, Color.Empty, font);
                }
            }
        }

        private static double BarRange(int bar)
        {
            return _HmL[bar];
        }

        public static void BearishBeltHold(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (BlackLineOpenEqualHigh(i) && LongLine(i)) && CloseAbovePreviousHigh(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
            }
        }

        public static void BearishDarkCloudCover(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            DataSeries series = (DataSeries) ((_bars.High + _bars.Low) / 2.0);
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((LongWhiteLine(bar) && LongBlackLine(i)) && (OpenAbovePreviousHigh(i) && (_bars.Close[i] < series[bar]))) && ((CloseAbovePreviousOpen(i) && CloseBelowPreviousClose(i)) && LongLine(i))) && LongLine(bar);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BearishDojiStar(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((WhiteLine(bar) && LongLine(bar)) && (OpenAbovePreviousClose(i) && Doji(i))) && ShortLine(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BearishEngulfingLines(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((WhiteLine(bar) && BlackLine(i)) && (OpenAbovePreviousClose(i) && CloseBelowPreviousOpen(i))) && LongLine(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BearishEveningStar(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((LongWhiteLine(num2) && (Math.Min(_HmL[i], _HmL[num2]) > _atr[num2])) && (LowAbovePreviousClose(bar) && (BarRange(bar) < (BarRange(num2) * 0.5)))) && (OpenBelowPreviousClose(i) && LongBlackLine(i))) && ShortLine(bar);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        public static void BearishHangingMan(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            DataSeries series = SMA.Series(_bars.Close, 5);
            int num = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((SmallUpperBody(i) && LittleOrNoUpperShadow(i)) && (!Doji(i) && LongLine(i))) && (series[i] > series[num]);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor);
                    }
                }
                num = i;
            }
        }

        public static void BearishHarami(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((WhiteLine(bar) && BlackLine(i)) && (!Doji(i) && LongLine(bar))) && (ShortLine(i) && OpenBelowPreviousClose(i))) && CloseAbovePreviousOpen(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BearishHaramiCross(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((WhiteLine(bar) && LongLine(bar)) && (HighBelowPreviousClose(i) && LowAbovePreviousOpen(i))) && Doji(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BearishKicking(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((WhiteMarubozo(bar) && BlackMarubozo(i)) && HighBelowPreviousOpen(i)) && (Math.Min(_HmL[i], _HmL[i - 1]) > _atr[num2]);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        public static void BearishLongBlackLine(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (OpenNearHigh(i, 0.7) && CloseNearLow(i)) && LongLine(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor);
                    }
                }
            }
        }

        public static void BearishSeparatingLines(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 1;
            for (int i = 2; i < _bars.Count; i++)
            {
                arr[i] = ((LongWhiteLine(bar) && BlackLine(i)) && OpenEqualsPreviousOpen(i, 0.05)) && LittleOrNoUpperShadow(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BearishShootingStar(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((WhiteLine(bar) && LowAbovePreviousClose(i)) && (SmallRealBody(i) && SmallLowerShadow(i))) && !ShortLine(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BearishSideBySideWhiteLines(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((BlackLine(num2) && WhiteLine(bar)) && (WhiteLine(i) && OpenBelowPreviousClose(bar))) && ((CloseBelowPreviousHigh(i) && HighAbovePreviousClose(i)) && OpenAbovePreviousLow(i))) && LowBelowPreviousOpen(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        public static void BearishThreeBlackCrows(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((((BlackLine(num2) && BlackLine(bar)) && (BlackLine(i) && OpenAbovePreviousClose(bar))) && ((OpenBelowPreviousOpen(bar) && OpenAbovePreviousClose(i)) && (OpenBelowPreviousOpen(i) && CloseBelowPreviousClose(bar)))) && CloseBelowPreviousClose(i)) && (Math.Min(_HmL[i], Math.Min(_HmL[bar], _HmL[num2])) > _atr[num2]);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        public static void BearishThreeInsideDown(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((WhiteLine(num2) && BlackLine(bar)) && (OpenBelowPreviousClose(bar) && CloseAbovePreviousOpen(bar))) && ((LongLine(num2) && ShortLine(bar)) && (!Doji(bar) && BlackLine(i)))) && CloseBelowPreviousClose(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        public static void BearishThreeOutsideDown(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((WhiteLine(num2) && BlackLine(bar)) && (OpenAbovePreviousClose(bar) && CloseBelowPreviousOpen(bar))) && (LongLine(bar) && BlackLine(i))) && CloseBelowPreviousClose(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        public static void BearishTriStar(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                double num4 = _atr[num2] * 2.0;
                arr[i] = ((((Doji(i) && OpenCloseInMiddle(i)) && ((_HmL[i] < num4) && Doji(bar))) && ((OpenCloseInMiddle(bar) && (_HmL[bar] < num4)) && (Doji(num2) && OpenCloseInMiddle(num2)))) && ((_HmL[num2] < num4) && OpenAbovePreviousOpen(bar))) && OpenBelowPreviousOpen(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, true);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        private static bool BlackLine(int bar)
        {
            return (_CmL[bar] < _OmL[bar]);
        }

        private static bool BlackLineOpenEqualHigh(int bar)
        {
            return ((_OHL[bar] > 0.95) && (_CHL[bar] < 0.3));
        }

        private static bool BlackMarubozo(int bar)
        {
            return ((_OHL[bar] > 0.95) && (_CHL[bar] < 0.05));
        }

        public static void BullishBeltHold(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 1;
            for (int i = 2; i < _bars.Count; i++)
            {
                arr[i] = ((DayWhite(i) && OpenEqualsLow(i)) && LongBlackLine(bar)) && (_bars.Low[bar] > _bars.Open[i]);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BullishDojiStar(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((BlackLine(bar) && LongLine(i)) && ((_bars.Open[i] < _bars.Close[bar]) && Doji(i))) && ShortLine(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BullishEngulfingLines(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((BlackLine(bar) && WhiteLine(i)) && (CloseAbovePreviousOpen(i) && OpenBelowPreviousClose(i))) && LongLine(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BullishHammer(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            DataSeries series = SMA.Series(_bars.Close, 5);
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((OpenAndCloseNearHigh(i) && LittleOrNoUpperShadow(i)) && (!Doji(i) && LongLine(i))) && (series[i] < series[i - 1]);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor);
                    }
                }
            }
        }

        public static void BullishHarami(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((BlackLine(bar) && WhiteLine(i)) && (NotDoji(i) && ShortLine(i))) && (LongLine(i) && CloseBelowPreviousOpen(i))) && OpenAbovePreviousClose(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BullishHaramiCross(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((BlackLine(bar) && LongLine(bar)) && (HighBelowPreviousOpen(i) && LowAbovePreviousClose(i))) && Doji(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BullishInvertedHammer(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((BlackLine(bar) && OpenBelowPreviousClose(i)) && (SmallRealBody(i) && SmallLowerShadow(i))) && !Doji(i)) && !ShortLine(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BullishKicking(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((BlackMarubozo(bar) && WhiteMarubozo(i)) && LowAbovePreviousHigh(i)) && (Math.Min(_HmL[i], _HmL[i - 1]) > _atr[num2]);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BullishLongWhiteLine(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (OpenNearLow(i, 0.3) && DayWhite(i)) && LongLine(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor);
                    }
                }
            }
        }

        public static void BullishMorningStar(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((LongBlackLine(num2) && (Math.Min(_HmL[i], _HmL[num2]) > _atr[num2])) && (ShortLine(bar) && OpenBelowPreviousClose(bar))) && ((_HmL[bar] < (_HmL[num2] * 0.5)) && OpenAbovePreviousClose(i))) && LongWhiteLine(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        public static void BullishPiercingLine(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            DataSeries series = (DataSeries) ((_bars.High + _bars.Low) / 2.0);
            int bar = 9;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((LongBlackLine(bar) && LongWhiteLine(i)) && (OpenBelowPreviousLow(i) && CloseBelowPreviousOpen(i))) && (_bars.Close[i] > series[bar])) && (Math.Min(_HmL[i], _HmL[i - 1]) > _atr[i]);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BullishSeparatingLines(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 1;
            for (int i = 2; i < _bars.Count; i++)
            {
                arr[i] = ((LongBlackLine(bar) && WhiteLine(i)) && OpenEqualsPreviousOpen(i, 0.05)) && OpenNearLow(i, 0.1);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 2);
                    }
                }
                bar = i;
            }
        }

        public static void BullishSideBySideWhiteLines(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 1;
            int num2 = bar - 1;
            for (int i = 2; i < _bars.Count; i++)
            {
                arr[i] = (((WhiteLine(num2) && WhiteLine(bar)) && (WhiteLine(i) && OpenAbovePreviousClose(bar))) && ((CloseBelowPreviousHigh(i) && HighAbovePreviousClose(i)) && OpenAbovePreviousLow(i))) && LowBelowPreviousOpen(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        public static void BullishThreeInsideUp(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((BlackLine(num2) && WhiteLine(bar)) && (!Doji(bar) && ShortLine(bar))) && ((LongLine(num2) && CloseBelowPreviousOpen(bar)) && (OpenAbovePreviousClose(bar) && WhiteLine(i)))) && CloseAbovePreviousClose(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        public static void BullishThreeOutsideUp(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((BlackLine(num2) && WhiteLine(bar)) && (CloseAbovePreviousOpen(bar) && OpenBelowPreviousClose(bar))) && (LongLine(bar) && WhiteLine(i))) && CloseAbovePreviousClose(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        public static void BullishThreeWhiteSoldiers(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = ((((WhiteLine(num2) && WhiteLine(bar)) && (WhiteLine(i) && OpenAbovePreviousOpen(bar))) && ((OpenBelowPreviousClose(bar) && OpenAbovePreviousOpen(i)) && (OpenBelowPreviousClose(i) && CloseAbovePreviousClose(bar)))) && CloseAbovePreviousClose(i)) && (Math.Min(_HmL[i], Math.Min(_HmL[bar], _HmL[num2])) > _atr[num2]);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        public static void BullishTriStar(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            int bar = 9;
            int num2 = bar - 1;
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (((Doji(i) && Doji(bar)) && (Doji(num2) && OpenCloseInMiddle(i))) && ((OpenCloseInMiddle(bar) && OpenCloseInMiddle(num2)) && ((Math.Max(_HmL[num2], Math.Max(_HmL[bar], _HmL[i])) <= (_atr[num2] * 2.0)) && OpenBelowPreviousOpen(bar)))) && OpenAbovePreviousOpen(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor, 3);
                    }
                }
                num2 = bar;
                bar = i;
            }
        }

        private static bool CloseAbovePreviousClose(int bar)
        {
            return (_bars.Close[bar] > _bars.Close[bar - 1]);
        }

        private static bool CloseAbovePreviousHigh(int bar)
        {
            return (_bars.Close[bar] > _bars.High[bar - 1]);
        }

        private static bool CloseAbovePreviousOpen(int bar)
        {
            return (_bars.Close[bar] > _bars.Open[bar - 1]);
        }

        private static bool CloseBelowPreviousClose(int bar)
        {
            return (_bars.Close[bar] < _bars.Close[bar - 1]);
        }

        private static bool CloseBelowPreviousHigh(int bar)
        {
            return (_bars.Close[bar] < _bars.High[bar - 1]);
        }

        private static bool CloseBelowPreviousOpen(int bar)
        {
            return (_bars.Close[bar] < _bars.Open[bar - 1]);
        }

        private static bool CloseNearLow(int bar)
        {
            return (_CHL[bar] < 0.3);
        }

        private static bool DayWhite(int bar)
        {
            return (_CHL[bar] > 0.7);
        }

        private static bool Doji(int bar)
        {
            return (Math.Abs((double) (_OHL[bar] - _CHL[bar])) < 0.1);
        }

        private static bool HighAbovePreviousClose(int bar)
        {
            return (_bars.High[bar] > _bars.Close[bar - 1]);
        }

        private static bool HighBelowPreviousClose(int bar)
        {
            return (_bars.High[bar] < _bars.Close[bar - 1]);
        }

        private static bool HighBelowPreviousOpen(int bar)
        {
            return (_bars.High[bar] < _bars.Open[bar - 1]);
        }

        private static void HighlightCandle(WealthScript w, int bar, Color color)
        {
            double num = _bars.High[bar];
            double num2 = _bars.Low[bar];
            int num3 = bar - 1;
            int num4 = Math.Min(bar + 1, _barsCountMinus1);
            w.DrawPolygon(w.PricePane, color, color, LineStyle.Invisible, 1, true, new double[] { (double) num3, num, (double) num4, num, (double) num4, num2, (double) num3, num2 });
        }

        private static void HighlightCandle(WealthScript w, int bar, Color color, int patternBars)
        {
            double num = _bars.High[bar];
            double num2 = _bars.Low[bar];
            if (patternBars > 1)
            {
                int num3 = patternBars - 1;
                for (int i = bar - 1; i >= (bar - num3); i--)
                {
                    num = (_bars.High[i] > num) ? _bars.High[i] : num;
                    num2 = (_bars.Low[i] < num2) ? _bars.Low[i] : num2;
                }
            }
            int num5 = bar - patternBars;
            int num6 = Math.Min(bar + 1, _barsCountMinus1);
            w.DrawPolygon(w.PricePane, color, color, LineStyle.Invisible, 1, true, new double[] { (double) num5, num, (double) num6, num, (double) num6, num2, (double) num5, num2 });
        }

        private static bool LittleOrNoUpperShadow(int bar)
        {
            return (Math.Max(_OHL[bar], _CHL[bar]) > 0.9);
        }

        private static bool LongBlackLine(int bar)
        {
            return ((_OHL[bar] > 0.7) && (_CHL[bar] < 0.3));
        }

        private static bool LongLine(int bar)
        {
            return (_HmL[bar] > (2.0 * _atr[bar - 1]));
        }

        private static bool LongWhiteLine(int bar)
        {
            return ((_OHL[bar] < 0.3) && (_CHL[bar] > 0.7));
        }

        private static bool LowAbovePreviousClose(int bar)
        {
            return (_bars.Low[bar] > _bars.Close[bar - 1]);
        }

        private static bool LowAbovePreviousHigh(int bar)
        {
            return (_bars.Low[bar] > _bars.High[bar - 1]);
        }

        private static bool LowAbovePreviousOpen(int bar)
        {
            return (_bars.Low[bar] > _bars.Open[bar - 1]);
        }

        private static bool LowBelowPreviousOpen(int bar)
        {
            return (_bars.Low[bar] < _bars.Open[bar - 1]);
        }

        public static void NeutralDoji(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (Doji(i) && OpenCloseInMiddle(i)) && (_HmL[i] < (_atr[i] * 2.0));
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor);
                    }
                }
            }
        }

        public static void NeutralMarubozu(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (Math.Abs((double) (_OHL[i] - _CHL[i])) > 0.96) && (_HmL[i] > _atr[i]);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor);
                    }
                }
            }
        }

        public static void NeutralSpinningTop(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            for (int i = 1; i < _bars.Count; i++)
            {
                arr[i] = (((Math.Min(_OHL[i], _CHL[i]) > 0.2) && (Math.Min(_OHL[i], _CHL[i]) < 0.5)) && ((Math.Max(_OHL[i], _CHL[i]) > 0.5) && (Math.Max(_OHL[i], _CHL[i]) < 0.8))) && !Doji(i);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor);
                    }
                }
            }
        }

        private static bool NotDoji(int bar)
        {
            return (_CHL[bar] > _OHL[bar]);
        }

        private static bool OpenAbovePreviousClose(int bar)
        {
            return (_bars.Open[bar] > _bars.Close[bar - 1]);
        }

        private static bool OpenAbovePreviousHigh(int bar)
        {
            return (_bars.Open[bar] > _bars.High[bar - 1]);
        }

        private static bool OpenAbovePreviousLow(int bar)
        {
            return (_bars.Open[bar] > _bars.Low[bar - 1]);
        }

        private static bool OpenAbovePreviousOpen(int bar)
        {
            return (_bars.Open[bar] > _bars.Open[bar - 1]);
        }

        private static bool OpenAndCloseNearHigh(int bar)
        {
            return (Math.Min(_OHL[bar], _CHL[bar]) > 0.7);
        }

        private static bool OpenBelowPreviousClose(int bar)
        {
            return (_bars.Open[bar] < _bars.Close[bar - 1]);
        }

        private static bool OpenBelowPreviousLow(int bar)
        {
            return (_bars.Open[bar] < _bars.Low[bar - 1]);
        }

        private static bool OpenBelowPreviousOpen(int bar)
        {
            return (_bars.Open[bar] < _bars.Open[bar - 1]);
        }

        private static bool OpenCloseInMiddle(int bar)
        {
            return ((((_OHL[bar] < 0.7) && (_OHL[bar] > 0.3)) && (_CHL[bar] < 0.7)) && (_CHL[bar] > 0.3));
        }

        private static bool OpenEqualsLow(int bar)
        {
            return (_OHL[bar] < 0.05);
        }

        private static bool OpenEqualsPreviousClose(int bar, double delta)
        {
            return (Math.Abs((double) ((_bars.Open[bar] - _bars.Close[bar - 1]) / BarRange(bar))) < delta);
        }

        private static bool OpenEqualsPreviousOpen(int bar, double delta)
        {
            return ((Math.Abs((double) (_bars.Open[bar] - _bars.Open[bar - 1])) / BarRange(bar)) < delta);
        }

        private static bool OpenNearHigh(int bar, double value)
        {
            return (_OHL[bar] > value);
        }

        private static bool OpenNearLow(int bar, double value)
        {
            return (_OHL[bar] < value);
        }

        public static void ReversalDragonflyDoji(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            for (int i = 1; i < _bars.Count; i++)
            {
                arr[i] = Doji(i) && OpenNearHigh(i, 0.8);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor);
                    }
                }
            }
        }

        public static void ReversalGravestoneDoji(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            for (int i = 1; i < _bars.Count; i++)
            {
                arr[i] = Doji(i) && OpenNearLow(i, 0.2);
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor);
                    }
                }
            }
        }

        public static void ReversalLongLeggedDoji(WealthScript w, string annotateText, bool highlight, out bool[] arr)
        {
            SetupInit(w.Bars);
            arr = new bool[_bars.Count];
            for (int i = 10; i < _bars.Count; i++)
            {
                arr[i] = (Doji(i) && OpenCloseInMiddle(i)) && (_HmL[i] > (_atr[i] * 2.0));
                if (arr[i])
                {
                    string s = annotateText.Trim();
                    if (s != "")
                    {
                        AnnotateText(w, i, s, false);
                    }
                    if (highlight)
                    {
                        HighlightCandle(w, i, highlightColor);
                    }
                }
            }
        }

        private static void SetupInit(Bars bars)
        {
            int num = bars.Count - 1;
            DateTime time = bars.Date[num];
            DateTime time2 = bars.Date[num];
            DateTime time3 = bars.Date[num];
            DateTime time4 = bars.Date[num];
            DateTime time5 = bars.Date[num];
            DateTime time6 = bars.Date[num];
            int num2 = bars.GetHashCode() - (((((((time.Year * 0x2710) + (100 * time2.Month)) + time3.Day) + (time4.Hour * 100)) + time5.Minute) + time6.Second) + bars.Count);
            if (_barsHashCode != num2)
            {
                _barsHashCode = num2;
                _bars = bars;
                _barsCountMinus1 = _bars.Count - 1;
                _OmL = _bars.Open - _bars.Low;
                _HmL = _bars.High - _bars.Low;
                _CmL = _bars.Close - _bars.Low;
                _OHL = _OmL / _HmL;
                _CHL = _CmL / _HmL;
                _atr = (DataSeries) (0.5 * ATR.Series(_bars, 10));
            }
        }

        private static bool ShortLine(int bar)
        {
            return (_HmL[bar] < _atr[bar - 1]);
        }

        private static bool SmallLowerShadow(int bar)
        {
            return (Math.Min(_CHL[bar], _OHL[bar]) < 0.1);
        }

        private static bool SmallRealBody(int bar)
        {
            return (Math.Max(_CHL[bar], _OHL[bar]) < 0.3);
        }

        private static bool SmallUpperBody(int bar)
        {
            return (Math.Min(_OHL[bar], _CHL[bar]) > 0.7);
        }

        private static bool TwoLongLines(int bar)
        {
            return (Math.Min(_HmL[bar], _HmL[bar - 1]) > (2.0 * _atr[bar - 2]));
        }

        private static bool WhiteLine(int bar)
        {
            return (_CmL[bar] > _OmL[bar]);
        }

        private static bool WhiteMarubozo(int bar)
        {
            return ((_OHL[bar] < 0.05) && (_CHL[bar] > 0.95));
        }
    }
}

