namespace finantic.TL
{
    using Fidelity.Components;
    using Microsoft.VisualBasic;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Media;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.Indicators;
    using WealthLab.Rules;

    public abstract class WealthScriptTL : WealthScript
    {
        private double _AS_BreakEvenStopTrigger = 0.0;
        private double _AS_ProfitTarget = 0.0;
        private double _AS_ReversalBreakEvenStopLevel = 0.0;
        private double _AS_StopLoss = 0.0;
        private int _AS_TimeBasedExit = 0;
        private double _AS_TrailingStopLevel = 0.0;
        private double _AS_TrailingStopTrigger = 0.0;
        private EMACalculation emaCalculation = EMACalculation.Modern;
        private Dictionary<int, StreamReader> fileReaders = new Dictionary<int, StreamReader>();
        private Dictionary<int, FileStream> fileStreams = new Dictionary<int, FileStream>();
        private Dictionary<int, TextWriter> fileWriters = new Dictionary<int, TextWriter>();
        private int nextFileHandle = -1;
        private Dictionary<int, ChartPane> panes = new Dictionary<int, ChartPane>();
        private PeakTroughMode peakThroughMode = PeakTroughMode.Percent;
        private Dictionary<int, DataSeries> priceSeries = new Dictionary<int, DataSeries>();
        private int priceSeriesKey = 8;
        private Random randomGen;
        private int randomSeed;
        private StdDevCalculation stddevCalculation = StdDevCalculation.Population;
        private bool synchExternalSeries = true;
        private bool warn_ChangeBar = false;
        private bool warn_DrawImage = false;
        private bool warn_DrawRoundRect = false;
        private bool warn_PlotSeriesLabel = false;
        private bool warn_PlotSymbol = false;
        private bool warn_PlotSyntheticSymbol = false;
        private Dictionary<string, bool> warning_printed = new Dictionary<string, bool>();
        public const int WL_ALL = -123;
        public const int WL_AQUA = 0x63;
        public const int WL_ASDOLLAR = 2;
        public const int WL_ASPERCENT = 0;
        public const int WL_ASPOINT = 1;
        public const int WL_BLACK = 0;
        public const int WL_BLUE = 9;
        public const int WL_BLUEBKG = 0x379;
        public const int WL_CANDLE = 1;
        public const int WL_DOTS = 6;
        public const int WL_DOTTED = 3;
        public const int WL_FUCHSIA = 0x38d;
        public const int WL_GRAY = 0x22b;
        public const int WL_GREEN = 90;
        public const int WL_GREENBKG = 0x382;
        public const int WL_HISTOGRAM = 4;
        public const int WL_LIME = 90;
        public const int WL_LINE = 2;
        public const int WL_MAROON = 500;
        public const int WL_NAVY = 5;
        public const int WL_OHLC = 0;
        public const int WL_OLIVE = 550;
        public const int WL_PURPLE = 0x1f9;
        public const int WL_RED = 900;
        public const int WL_REDBKG = 0x3dc;
        public const int WL_SILVER = 0x309;
        public const int WL_TEAL = 0xff;
        public const int WL_THICK = 2;
        public const int WL_THICKHIST = 5;
        public const int WL_THIN = 1;
        public const int WL_WHITE = 0x3e7;
        public const int WL_WINLOSS = 800;
        public const int WL_YELLOW = 990;

        public WealthScriptTL()
        {
            WSTL.C = this;
        }

        public int AbsSeries(int series)
        {
            this.check_series(series, "series");
            DataSeries source = this.priceSeries[series];
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, DataSeries.Abs(source));
            return key;
        }

        public double AccumDist(int bar)
        {
            return WealthLab.Indicators.AccumDist.Series(base.Bars)[bar];
        }

        public int AccumDistSeries()
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.AccumDist.Series(base.Bars));
            return key;
        }

        public void AddCommentary(string Line)
        {
            this.not_supported("AddCommentary");
        }

        public int AddSeries(int Series1, int Series2)
        {
            this.check_series(Series1, "Series1");
            this.check_series(Series2, "Series2");
            DataSeries series = this.priceSeries[Series1];
            DataSeries series2 = this.priceSeries[Series2];
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, series + series2);
            return key;
        }

        public int AddSeriesValue(int Series, double Value)
        {
            this.check_series(Series, "Series");
            DataSeries series = this.priceSeries[Series];
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, series + Value);
            return key;
        }

        public double ADX(int bar, int period)
        {
            return WealthLab.Indicators.ADX.Series(base.Bars, period)[bar];
        }

        public double ADXR(int bar, int period)
        {
            return WealthLab.Indicators.ADXR.Series(base.Bars, period)[bar];
        }

        public int ADXRSeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.ADXR.Series(base.Bars, period));
            return key;
        }

        public int ADXSeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.ADX.Series(base.Bars, period));
            return key;
        }

        public int AlertOrderType(int alert)
        {
            this.check_alert(alert);
            Alert alert2 = base.Alerts[alert];
            switch (alert2.OrderType)
            {
                case OrderType.Market:
                    return 0;

                case OrderType.Limit:
                    return 2;

                case OrderType.Stop:
                    return 1;

                case OrderType.AtClose:
                    return 3;
            }
            throw new Exception("unknown order type found in alert");
        }

        public int AlertPositionType(int alert)
        {
            this.check_alert(alert);
            Alert alert2 = base.Alerts[alert];
            switch (alert2.AlertType)
            {
                case TradeType.Buy:
                    return 0;

                case TradeType.Sell:
                    return 1;

                case TradeType.Short:
                    return 2;

                case TradeType.Cover:
                    return 3;
            }
            throw new Exception("unknown trade type found in alert");
        }

        public double AlertPrice(int alert)
        {
            this.check_alert(alert);
            Alert alert2 = base.Alerts[alert];
            return alert2.Price;
        }

        public int AlertShares(int alert)
        {
            this.check_alert(alert);
            Alert alert2 = base.Alerts[alert];
            return (int) Math.Round(alert2.Shares);
        }

        public string AlertSymbol(int alert)
        {
            this.check_alert(alert);
            Alert alert2 = base.Alerts[alert];
            return alert2.Symbol;
        }

        public void AnnotateBar(string text, int bar, bool abovePrices, int wlcolor, int fontSize)
        {
            if ((bar >= 0) && (bar < base.Bars.Count))
            {
                Font font = new Font("Arial", (float) fontSize);
                System.Drawing.Color color = this.WLColor2Color(wlcolor);
                base.AnnotateBar(text, bar, abovePrices, color, System.Drawing.Color.Empty, font);
            }
        }

        public void AnnotateChart(string text, int pane, int bar, double price, int wlcolor, int fontSize)
        {
            this.check_pane(pane);
            if ((bar >= 0) && (bar < base.Bars.Count))
            {
                ChartPane pane2 = this.panes[pane];
                System.Drawing.Color color = this.WLColor2Color(wlcolor);
                Font font = new Font("Arial", (float) fontSize);
                base.AnnotateChart(pane2, text, bar, price, color, System.Drawing.Color.Empty, font);
            }
        }

        public void ApplyAutoStops(int Bar)
        {
            for (int i = this.PositionCount - 1; i >= 0; i--)
            {
                if (base.ActivePositions.Count == 0)
                {
                    break;
                }
                this.check_position(i);
                Position position = base.Positions[i];
                if (this.PositionActive(i))
                {
                    int num6;
                    double num7;
                    bool flag2;
                    bool flag = false;
                    if ((this._AS_TimeBasedExit > 0) && (((Bar + 1) - this.PositionEntryBar(i)) >= this._AS_TimeBasedExit))
                    {
                        if (this.PositionLong(i))
                        {
                            this.SellAtMarket(Bar + 1, i, "Time-Based");
                        }
                        else
                        {
                            this.CoverAtMarket(Bar + 1, i, "Time-Based");
                        }
                        flag = true;
                    }
                    if ((this._AS_StopLoss > 0.0) && !flag)
                    {
                        double num2;
                        if (this.PositionLong(i))
                        {
                            num2 = ((100.0 - this._AS_StopLoss) / 100.0) * this.PositionEntryPrice(i);
                            if (this.SellAtStop(Bar + 1, num2, i, "Stop Loss"))
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            num2 = ((100.0 + this._AS_StopLoss) / 100.0) * this.PositionEntryPrice(i);
                            if (this.CoverAtStop(Bar + 1, num2, i, "Stop Loss"))
                            {
                                flag = true;
                            }
                        }
                    }
                    if ((this._AS_TrailingStopLevel != 0.0) && !flag)
                    {
                        double triggerPct = this._AS_TrailingStopTrigger;
                        double profitReversalPct = this._AS_TrailingStopLevel;
                        if (base.ExitAtAutoTrailingStop(Bar, position, triggerPct, profitReversalPct, "Trailing Stop"))
                        {
                            flag = true;
                        }
                    }
                    if ((this._AS_ReversalBreakEvenStopLevel != 0.0) && !flag)
                    {
                        double num5 = base.Bars.Close[this.PositionEntryBar(i)];
                        num6 = this.PositionEntryBar(i) + 1;
                        while (num6 < Bar)
                        {
                            num7 = base.Bars.Close[num6];
                            if (this.PositionLong(i))
                            {
                                if (num7 < num5)
                                {
                                    num5 = num7;
                                }
                            }
                            else if (num7 > num5)
                            {
                                num5 = num7;
                            }
                            num6++;
                        }
                        flag2 = false;
                        if (this.PositionLong(i))
                        {
                            if (num5 <= ((this.PositionEntryPrice(i) * (100.0 - this._AS_ReversalBreakEvenStopLevel)) / 100.0))
                            {
                                flag2 = true;
                            }
                        }
                        else if (num5 >= ((this.PositionEntryPrice(i) * (100.0 + this._AS_ReversalBreakEvenStopLevel)) / 100.0))
                        {
                            flag2 = true;
                        }
                        if (flag2 && base.ExitAtLimit(Bar, position, this.PositionEntryPrice(i), "Reverse BreakEven Stop"))
                        {
                            flag = true;
                        }
                    }
                    if ((this._AS_BreakEvenStopTrigger > 0.0) && !flag)
                    {
                        double num8 = base.Bars.Close[this.PositionEntryBar(i)];
                        for (num6 = this.PositionEntryBar(i) + 1; num6 < Bar; num6++)
                        {
                            num7 = base.Bars.Close[num6];
                            if (this.PositionLong(i))
                            {
                                if (num7 > num8)
                                {
                                    num8 = num7;
                                }
                            }
                            else if (num7 < num8)
                            {
                                num8 = num7;
                            }
                        }
                        flag2 = false;
                        if (this.PositionLong(i))
                        {
                            if (num8 >= ((this.PositionEntryPrice(i) * (100.0 + this._AS_BreakEvenStopTrigger)) / 100.0))
                            {
                                flag2 = true;
                            }
                        }
                        else if (num8 <= ((this.PositionEntryPrice(i) * (100.0 - this._AS_BreakEvenStopTrigger)) / 100.0))
                        {
                            flag2 = true;
                        }
                        if (flag2 && base.ExitAtStop(Bar, position, this.PositionEntryPrice(i), "Breakeven Stop"))
                        {
                            flag = true;
                        }
                    }
                    if ((this._AS_ProfitTarget > 0.0) && !flag)
                    {
                        double num9;
                        if (this.PositionLong(i))
                        {
                            num9 = ((100.0 + this._AS_ProfitTarget) / 100.0) * this.PositionEntryPrice(i);
                            if (this.SellAtLimit(Bar + 1, num9, i, "Profit Target"))
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            num9 = ((100.0 - this._AS_ProfitTarget) / 100.0) * this.PositionEntryPrice(i);
                            if (this.CoverAtLimit(Bar + 1, num9, i, "Profit Target"))
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
        }

        private void ApplyAutoStopsQL(int Bar)
        {
        }

        public double ArcSinh(double value)
        {
            return Math.Log(value + Math.Sqrt((value * value) + 1.0));
        }

        public double ArcTanh(double value)
        {
            return (0.5 * Math.Log((1.0 + value) / (1.0 - value)));
        }

        public double AroonDown(int bar, int series, int period)
        {
            return WealthLab.Indicators.AroonDown.Value(bar, this.priceSeries[series], period);
        }

        public int AroonDownSeries(int series, int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.AroonDown.Series(this.priceSeries[series], period));
            return key;
        }

        public double AroonUp(int bar, int series, int period)
        {
            return WealthLab.Indicators.AroonUp.Value(bar, this.priceSeries[series], period);
        }

        public int AroonUpSeries(int series, int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.AroonUp.Series(this.priceSeries[series], period));
            return key;
        }

        public string AsString(double d)
        {
            return d.ToString();
        }

        public string AsString(int i)
        {
            return i.ToString();
        }

        public double ATR(int Bar, int Period)
        {
            return WealthLab.Indicators.ATR.Value(Bar, base.Bars, Period);
        }

        public double ATRP(int Bar, int Period)
        {
            return WealthLab.Indicators.ATRP.Series(base.Bars, Period)[Bar];
        }

        public int ATRPSeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.ATRP.Series(base.Bars, period));
            return key;
        }

        public int ATRSeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.ATR.Series(base.Bars, period));
            return key;
        }

        public int BarNum(int bar)
        {
            int num = base.Bars.IntradayBarNumber(bar);
            if (num < 0)
            {
                num = 0;
            }
            return num;
        }

        public double BBandLower(int bar, int series, int period, double stdDev)
        {
            return WealthLab.Indicators.BBandLower.Value(bar, this.priceSeries[series], period, stdDev);
        }

        public int BBandLowerSeries(int Series, int Period, double stdDev)
        {
            this.check_series(Series, "Series");
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.BBandLower.Series(this.priceSeries[Series], Period, stdDev));
            return key;
        }

        public double BBandUpper(int bar, int series, int period, double stdDev)
        {
            return WealthLab.Indicators.BBandUpper.Value(bar, this.priceSeries[series], period, stdDev);
        }

        public int BBandUpperSeries(int Series, int Period, double stdDev)
        {
            this.check_series(Series, "Series");
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.BBandUpper.Series(this.priceSeries[Series], Period, stdDev));
            return key;
        }

        public int BoolToInt(bool b)
        {
            return (b ? -1 : 0);
        }

        public double BOP(int bar)
        {
            double num = base.High[bar] - base.Low[bar];
            if (num == 0.0)
            {
                return 0.0;
            }
            return ((base.Close[bar] - base.Open[bar]) / num);
        }

        public int BOPSeries()
        {
            int key = this.priceSeriesKey++;
            DataSeries series = (base.Close - base.Open) / (base.High - base.Low);
            this.priceSeries.Add(key, series);
            return key;
        }

        public bool BuyAtClose(int Bar, string SignalName)
        {
            return (base.BuyAtClose(Bar, SignalName) != null);
        }

        public bool BuyAtLimit(int Bar, double limitPrice, string SignalName)
        {
            return (base.BuyAtLimit(Bar, limitPrice, SignalName) != null);
        }

        public bool BuyAtMarket(int Bar, string SignalName)
        {
            return (base.BuyAtMarket(Bar, SignalName) != null);
        }

        public bool BuyAtStop(int Bar, double stop, string SignalName)
        {
            return (base.BuyAtStop(Bar, stop, SignalName) != null);
        }

        public double CADO(int bar)
        {
            return WealthLab.Indicators.CADO.Series(base.Bars)[bar];
        }

        public int CADOSeries()
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.CADO.Series(base.Bars));
            return key;
        }

        public double CCI(int bar, int period)
        {
            return WealthLab.Indicators.CCI.Series(base.Bars, period)[bar];
        }

        public int CCISeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.CCI.Series(base.Bars, period));
            return key;
        }

        public void ChangeBar(int bar, int date, int time, double open, double high, double low, double close, int volume, int OpenInterest)
        {
            if (bar < 0)
            {
                throw new ArgumentOutOfRangeException("bar", "bar must be >= 0");
            }
            if (bar >= base.Bars.Count)
            {
                throw new ArgumentOutOfRangeException("bar", "bar must be < Bars.Count");
            }
            DateTime time2 = this.WLDateToDateTime(date);
            if (time != 0)
            {
                TimeSpan span = this.WLTimeToTimeSpan(time);
                time2 += span;
            }
            if (base.Bars.Date[bar] != time2)
            {
                throw new Exception("ChangeBar: can't change date");
            }
            base.Bars.Open[bar] = open;
            base.Bars.High[bar] = high;
            base.Bars.Low[bar] = low;
            base.Bars.Close[bar] = close;
            base.Bars.Volume[bar] = volume;
            if ((OpenInterest != 0.0) && !this.warn_ChangeBar)
            {
                base.PrintDebug("ChangeBar: OpenInterest not supported");
            }
        }

        public string CharAt(string Value, int Index)
        {
            if ((Index < 1) || (Index > Value.Length))
            {
                char ch = '\0';
                return ch.ToString();
            }
            return Value.Substring(Index - 1, 1);
        }

        private void check_alert(int alert)
        {
            if (alert < 0)
            {
                throw new ArgumentOutOfRangeException("alert < 0");
            }
            if (alert >= base.Alerts.Count)
            {
                throw new ArgumentOutOfRangeException("alert >= Alerts.Count");
            }
        }

        private void check_bar(int Bar)
        {
            if (Bar < 0)
            {
                throw new ArgumentOutOfRangeException("Bar < 0");
            }
            if (Bar >= base.Bars.Count)
            {
                throw new ArgumentOutOfRangeException("Bar >= BarCount");
            }
        }

        private void check_bar1(int Bar)
        {
            if (Bar < 0)
            {
                throw new ArgumentOutOfRangeException("Bar < 0");
            }
            if (Bar > base.Bars.Count)
            {
                throw new ArgumentOutOfRangeException("Bar > BarCount");
            }
        }

        private void check_pane(int Pane)
        {
            if (Pane < 0)
            {
                throw new ArgumentOutOfRangeException("Pane < 0");
            }
            if (Pane >= this.panes.Count)
            {
                throw new ArgumentOutOfRangeException("Pane", "no such Pane");
            }
        }

        private void check_position(int Position)
        {
            if (Position < 0)
            {
                throw new ArgumentOutOfRangeException("position < 0");
            }
            if (Position >= this.PositionCount)
            {
                throw new ArgumentOutOfRangeException("position > PositionCount");
            }
        }

        private void check_positionOrAll(int Position)
        {
            if (Position != -123)
            {
                if (Position < 0)
                {
                    throw new ArgumentOutOfRangeException("position < 0");
                }
                if (Position >= this.PositionCount)
                {
                    throw new ArgumentOutOfRangeException("position > PositionCount");
                }
            }
        }

        private void check_series(int Series, string argumentName)
        {
            if (!this.priceSeries.ContainsKey(Series))
            {
                throw new ArgumentOutOfRangeException("Price series handle nor defined", argumentName);
            }
            if (this.priceSeries[Series] == null)
            {
                throw new ArgumentException("not a valid price series", argumentName);
            }
        }

        private void checkBar(int Bar)
        {
            if ((Bar < 0) || (Bar >= base.Bars.Count))
            {
                throw new ArgumentException("Bar out of Range. Need 0 <= Bar < BarCount.", "Bar");
            }
        }

        public string Chr(int code)
        {
            char ch = (char) code;
            return ch.ToString();
        }

        public void ClearExternalSeries(string symbol)
        {
            if (symbol != "")
            {
                throw new Exception("not supported");
            }
            base.ClearExternalSymbols();
        }

        public void ClearIndicators()
        {
            base.Bars.Cache.Clear();
        }

        public double CMF(int bar, int period)
        {
            return WealthLab.Indicators.CMF.Series(base.Bars, period)[bar];
        }

        public int CMFSeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.CMF.Series(base.Bars, period));
            return key;
        }

        public double CMO(int bar, int series, int period)
        {
            return WealthLab.Indicators.CMO.Value(bar, this.priceSeries[series], period);
        }

        public int CMOSeries(int series, int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.CMO.Series(this.priceSeries[series], period));
            return key;
        }

        public int CompareStr(string s1, string s2)
        {
            this.print_warning_once("CompareStr", "has possible different behavior");
            int num = string.Compare(s1, s2, false, CultureInfo.InvariantCulture);
            if (num < 0)
            {
                return -1;
            }
            if (num > 0)
            {
                return 1;
            }
            return 0;
        }

        public int CompareText(string s1, string s2)
        {
            int num = string.Compare(s1, s2, true, CultureInfo.InvariantCulture);
            if (num < 0)
            {
                return -1;
            }
            if (num > 0)
            {
                return 1;
            }
            return 0;
        }

        public string Copy(string s1, int index, int count)
        {
            if (index < 1)
            {
                throw new ArgumentException("index must be greater than 0", "index");
            }
            return s1.Substring(index - 1, count);
        }

        public double Correlation(int series1, int series2, int startBar, int EndBar)
        {
            throw new Exception("not supported");
        }

        public double Cotan(double value)
        {
            return -Math.Tan(1.5707963267948966 + this.DegToRad(value));
        }

        public void CoverAtClose(int Bar, int P, string SignalName)
        {
            if (P != -1)
            {
                this.check_positionOrAll(P);
                Position position = (P == -123) ? Position.AllPositions : base.Positions[P];
                base.CoverAtClose(Bar, position, SignalName);
            }
        }

        public bool CoverAtLimit(int Bar, double LimitPrice, int P, string SignalName)
        {
            if (P == -1)
            {
                return false;
            }
            this.check_positionOrAll(P);
            Position position = (P == -123) ? Position.AllPositions : base.Positions[P];
            return base.CoverAtLimit(Bar, position, LimitPrice, SignalName);
        }

        public void CoverAtMarket(int Bar, int P, string SignalName)
        {
            if (P != -1)
            {
                this.check_positionOrAll(P);
                Position position = (P == -123) ? Position.AllPositions : base.Positions[P];
                base.CoverAtMarket(Bar, position, SignalName);
            }
        }

        public bool CoverAtStop(int Bar, double StopPrice, int P, string SignalName)
        {
            if (P == -1)
            {
                return false;
            }
            this.check_positionOrAll(P);
            Position position = (P == -123) ? Position.AllPositions : base.Positions[P];
            return base.CoverAtStop(Bar, position, StopPrice, SignalName);
        }

        public bool CoverAtTrailingStop(int Bar, double StopPrice, int P, string SignalName)
        {
            this.check_positionOrAll(P);
            Position position = (P == -123) ? Position.AllPositions : base.Positions[P];
            return base.CoverAtTrailingStop(Bar, position, StopPrice, SignalName);
        }

        public int CreateNamedSeries(string SeriesName)
        {
            int key = this.priceSeriesKey++;
            DataSeries series = base.Bars.Close - base.Bars.Close;
            series.Description = SeriesName;
            this.priceSeries.Add(key, series);
            return key;
        }

        public int CreatePane(int height, bool abovePrices, bool showGrid)
        {
            ChartPane pane = base.CreatePane(height, abovePrices, showGrid);
            int count = this.panes.Count;
            this.panes.Add(count, pane);
            return count;
        }

        public int CreateSeries()
        {
            int priceSeriesKey = this.priceSeriesKey;
            return this.CreateNamedSeries("WLSeries" + priceSeriesKey.ToString());
        }

        public int CreateSeriesLength(int length)
        {
            int key = this.priceSeriesKey++;
            DataSeries series = new DataSeries("WLSeries" + key.ToString());
            DateTime time = base.Bars.Date[0];
            for (int i = 0; i < length; i++)
            {
                series.Add(0.0, time);
                time += TimeSpan.FromDays(1.0);
            }
            this.priceSeries.Add(key, series);
            return key;
        }

        public bool CrossOver(int bar, int series1, int series2)
        {
            DataSeries series = this.priceSeries[series1];
            DataSeries series3 = this.priceSeries[series2];
            return base.CrossOver(bar, series, series3);
        }

        public bool CrossOverValue(int bar, int series, double value)
        {
            DataSeries series2 = this.priceSeries[series];
            return base.CrossOver(bar, series2, value);
        }

        public bool CrossUnder(int bar, int series1, int series2)
        {
            DataSeries series = this.priceSeries[series1];
            DataSeries series3 = this.priceSeries[series2];
            return base.CrossUnder(bar, series, series3);
        }

        public bool CrossUnderValue(int bar, int series, double value)
        {
            DataSeries series2 = this.priceSeries[series];
            return base.CrossUnder(bar, series2, value);
        }

        public double CumDown(int bar, int series, int period)
        {
            return WealthLab.Indicators.CumDown.Value(bar, this.priceSeries[series], period);
        }

        public int CumDownSeries(int series, int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.CumDown.Series(this.priceSeries[series], period));
            return key;
        }

        public double CumUp(int bar, int series, int period)
        {
            return WealthLab.Indicators.CumUp.Value(bar, this.priceSeries[series], period);
        }

        public int CumUpSeries(int series, int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.CumUp.Series(this.priceSeries[series], period));
            return key;
        }

        public int CurrentDate()
        {
            return int.Parse(DateTime.Now.ToString("yyyyMMdd"));
        }

        public int CurrentTime()
        {
            return int.Parse(DateTime.Now.ToString("HHmm"));
        }

        public int DateTimeToBar(int date, int time)
        {
            int year = date / 0x2710;
            int month = (date / 100) % 100;
            int day = date % 100;
            int hour = time / 100;
            int minute = time % 100;
            DateTime time2 = new DateTime(year, month, day, hour, minute, 0);
            return base.Bars.ConvertDateToBar(time2, true);
        }

        public int DateToBar(int date)
        {
            int year = date / 0x2710;
            int month = (date / 100) % 100;
            int day = date % 100;
            DateTime time = new DateTime(year, month, day);
            int num4 = base.Bars.ConvertDateToBar(time, false);
            if (num4 == 0)
            {
                return -1;
            }
            return num4;
        }

        public string DateToStr(int date)
        {
            return this.WLDateToDateTime(date).ToString("d");
        }

        public int DayOfWeek(int bar)
        {
            DateTime time = base.Bars.Date[bar];
            return (((int) time.DayOfWeek) + 1);
        }

        public int DaysBetween(int Bar1, int Bar2)
        {
            TimeSpan span = base.Bars.Date[Bar2] - base.Bars.Date[Bar1];
            return (int) span.TotalDays;
        }

        public int DaysBetweenDates(int date1, int date2)
        {
            DateTime time = this.WLDateToDateTime(date1);
            TimeSpan span = (TimeSpan) (this.WLDateToDateTime(date2) - time);
            return span.Days;
        }

        public double DegAcos(double myValue)
        {
            return this.RadToDeg(Math.Acos(myValue));
        }

        public double DegAsin(double myValue)
        {
            return this.RadToDeg(Math.Asin(myValue));
        }

        public double DegAtan(double myValue)
        {
            return this.RadToDeg(Math.Atan(myValue));
        }

        public double DegCos(double myValue)
        {
            return Math.Cos(this.DegToRad(myValue));
        }

        public double DegSin(double myValue)
        {
            return Math.Sin(this.DegToRad(myValue));
        }

        public double DegTan(double myValue)
        {
            return Math.Tan(this.DegToRad(myValue));
        }

        public double DegToRad(double value)
        {
            return ((value * 3.1415926535897931) / 180.0);
        }

        public void Delete(ref string s1, int index, int count)
        {
            string str = s1.Remove(index - 1, count);
            s1 = str;
        }

        public double DIMinus(int bar, int period)
        {
            return WealthLab.Indicators.DIMinus.Series(base.Bars, period)[bar];
        }

        public int DIMinusSeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.DIMinus.Series(base.Bars, period));
            return key;
        }

        public double DIPlus(int bar, int period)
        {
            return WealthLab.Indicators.DIPlus.Series(base.Bars, period)[bar];
        }

        public int DIPlusSeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.DIPlus.Series(base.Bars, period));
            return key;
        }

        public int DivideSeries(int Series1, int Series2)
        {
            this.check_series(Series1, "Series1");
            this.check_series(Series2, "Series2");
            DataSeries series = this.priceSeries[Series1];
            DataSeries series2 = this.priceSeries[Series2];
            DataSeries series3 = series / series2;
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, series3);
            return key;
        }

        public int DivideSeriesValue(int Series1, double value)
        {
            this.check_series(Series1, "Series1");
            DataSeries series = this.priceSeries[Series1];
            DataSeries series2 = (DataSeries) (series / value);
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, series2);
            return key;
        }

        public int DivideValueSeries(double value, int Series1)
        {
            this.check_series(Series1, "Series1");
            DataSeries series = this.priceSeries[Series1];
            DataSeries series2 = (DataSeries) (value / series);
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, series2);
            return key;
        }

        public void DrawCircle(int radius, int pane, int bar, double price, int wlcolor, int style)
        {
            this.check_pane(pane);
            if ((bar >= 0) && (bar < base.Bars.Count))
            {
                ChartPane pane2 = this.panes[pane];
                System.Drawing.Color color = this.WLColor2Color(wlcolor);
                LineStyle style2 = this.WLStyle2LineStyle(style);
                int width = this.WLStyle2Width(style);
                base.DrawCircle(pane2, radius, bar, price, color, style2, width, false);
            }
        }

        public void DrawCircle2(int barCenter, double priceCenter, int barRadius, double PriceRadius, int pane, int wlcolor, int style)
        {
            this.check_pane(pane);
            if ((barCenter >= 0) && (barCenter < base.Bars.Count))
            {
                ChartPane pane2 = this.panes[pane];
                System.Drawing.Color color = this.WLColor2Color(wlcolor);
                LineStyle style2 = this.WLStyle2LineStyle(style);
                int width = this.WLStyle2Width(style);
                base.DrawCircle(pane2, barCenter, priceCenter, barRadius, PriceRadius, color, style2, width, false);
            }
        }

        public void DrawDiamond(int bar1, double price1, int bar2, double price2, int bar3, double price3, int bar4, double price4, int pane, int wlcolor, int style, int fillColor, bool behindPrices)
        {
            double[] coords = new double[] { (double) bar1, price1, (double) bar2, price2, (double) bar3, price3, (double) bar4, price4 };
            ChartPane pane2 = this.panes[pane];
            System.Drawing.Color color = this.WLColor2Color(wlcolor);
            LineStyle style2 = this.WLStyle2LineStyle(style);
            int width = this.WLStyle2Width(style);
            if (fillColor == -1)
            {
                base.DrawPolygon(pane2, color, style2, width, behindPrices, coords);
            }
            else
            {
                System.Drawing.Color color2 = this.WLColor2Color(fillColor);
                base.DrawPolygon(pane2, color, color2, style2, width, behindPrices, coords);
            }
        }

        public void DrawEllipse(int bar1, double price1, int bar2, double price2, int pane, int wlcolor, int style, int fillColor, bool behindPrices)
        {
            ChartPane pane2 = this.panes[pane];
            System.Drawing.Color color = this.WLColor2Color(wlcolor);
            LineStyle style2 = this.WLStyle2LineStyle(style);
            int width = this.WLStyle2Width(style);
            if (fillColor == -1)
            {
                base.DrawEllipse(pane2, bar1, price1, bar2, price2, color, style2, width, behindPrices);
            }
            else
            {
                System.Drawing.Color color2 = this.WLColor2Color(fillColor);
                base.DrawEllipse(pane2, bar1, price1, bar2, price2, color, color2, style2, width, behindPrices);
            }
        }

        public void DrawHorzLine(double value, int pane, int wlcolor, int style)
        {
            ChartPane pane2 = this.panes[pane];
            System.Drawing.Color color = this.WLColor2Color(wlcolor);
            LineStyle style2 = this.WLStyle2LineStyle(style);
            int width = this.WLStyle2Width(style);
            base.DrawHorzLine(pane2, value, color, style2, width);
        }

        public void DrawImage(string bitmap, int pane, int bar, double price, bool topDown)
        {
            if ((bar >= 0) && (bar < base.Bars.Count))
            {
                ChartPane pane2 = this.panes[pane];
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Wealth-Lab\Wealth-Lab Developer 3.0", true);
                if (key == null)
                {
                    throw new ApplicationException("can't find Wealth-Lab base directory.");
                }
                string str = (string) key.GetValue("Directory");
                Image image = Image.FromFile(Path.Combine(Path.Combine(str, "Bitmaps"), bitmap + ".bmp"));
                if (!this.warn_DrawImage)
                {
                    base.PrintDebug("DrawImage: topDown parameter ignored");
                    this.warn_DrawImage = true;
                }
                base.DrawImage(pane2, image, bar, price, false);
            }
        }

        public void DrawLabel(string value, int pane)
        {
            ChartPane pane2 = this.panes[pane];
            base.DrawLabel(pane2, value);
        }

        public void DrawLine(int bar1, double price1, int bar2, double price2, int pane, int wlcolor, int style)
        {
            if (((bar1 >= 0) && (bar1 < base.Bars.Count)) && ((bar2 >= 0) && (bar2 < base.Bars.Count)))
            {
                ChartPane pane2 = this.panes[pane];
                System.Drawing.Color color = this.WLColor2Color(wlcolor);
                LineStyle style2 = this.WLStyle2LineStyle(style);
                int width = this.WLStyle2Width(style);
                base.DrawLine(pane2, bar1, price1, bar2, price2, color, style2, width);
            }
        }

        public void DrawRectangle(int bar1, double price1, int bar2, double price2, int pane, int wlcolor, int style, int fillColor, bool behindPrices)
        {
            if (((bar1 >= 0) && (bar1 < base.Bars.Count)) && ((bar2 >= 0) && (bar2 < base.Bars.Count)))
            {
                double[] coords = new double[] { (double) bar1, price1, (double) bar2, price1, (double) bar2, price2, (double) bar1, price2 };
                ChartPane pane2 = this.panes[pane];
                System.Drawing.Color color = this.WLColor2Color(wlcolor);
                LineStyle style2 = this.WLStyle2LineStyle(style);
                int width = this.WLStyle2Width(style);
                if (fillColor == -1)
                {
                    base.DrawPolygon(pane2, color, style2, width, behindPrices, coords);
                }
                else
                {
                    System.Drawing.Color color2 = this.WLColor2Color(fillColor);
                    base.DrawPolygon(pane2, color, color2, style2, width, behindPrices, coords);
                }
            }
        }

        public void DrawRoundRect(int bar1, double price1, int bar2, double price2, int pane, int wlcolor, int style, int fillColor, bool behindPrices)
        {
            if (!this.warn_DrawRoundRect)
            {
                base.PrintDebug("DrawRoundRect: rounded corners are not supported");
                this.warn_DrawRoundRect = true;
            }
            if (((bar1 >= 0) && (bar1 < base.Bars.Count)) && ((bar2 >= 0) && (bar2 < base.Bars.Count)))
            {
                double[] coords = new double[] { (double) bar1, price1, (double) bar2, price1, (double) bar2, price2, (double) bar1, price2 };
                ChartPane pane2 = this.panes[pane];
                System.Drawing.Color color = this.WLColor2Color(wlcolor);
                LineStyle style2 = this.WLStyle2LineStyle(style);
                int width = this.WLStyle2Width(style);
                if (fillColor == -1)
                {
                    base.DrawPolygon(pane2, color, style2, width, behindPrices, coords);
                }
                else
                {
                    System.Drawing.Color color2 = this.WLColor2Color(fillColor);
                    base.DrawPolygon(pane2, color, color2, style2, width, behindPrices, coords);
                }
            }
        }

        public void DrawText(string value, int pane, int x, int y, int wlcolor, int size)
        {
            ChartPane pane2 = this.panes[pane];
            System.Drawing.Color color = this.WLColor2Color(wlcolor);
            Font font = new Font("Arial", (float) size);
            base.DrawText(pane2, value, x, y, color, System.Drawing.Color.Empty, font);
        }

        public void DrawTriangle(int bar1, double price1, int bar2, double price2, int bar3, double price3, int pane, int wlcolor, int style, int fillColor, bool behindPrices)
        {
            if ((((bar1 >= 0) && (bar1 < base.Bars.Count)) && ((bar2 >= 0) && (bar2 < base.Bars.Count))) && ((bar3 >= 0) && (bar3 < base.Bars.Count)))
            {
                double[] coords = new double[] { (double) bar1, price1, (double) bar2, price2, (double) bar3, price3 };
                ChartPane pane2 = this.panes[pane];
                System.Drawing.Color color = this.WLColor2Color(wlcolor);
                LineStyle style2 = this.WLStyle2LineStyle(style);
                int width = this.WLStyle2Width(style);
                if (fillColor == -1)
                {
                    base.DrawPolygon(pane2, color, style2, width, behindPrices, coords);
                }
                else
                {
                    System.Drawing.Color color2 = this.WLColor2Color(fillColor);
                    base.DrawPolygon(pane2, color, color2, style2, width, behindPrices, coords);
                }
            }
        }

        public double DSS(int bar, int period1, int period2, int stochPeriod)
        {
            return WealthLab.Indicators.DSS.Series(base.Bars, period1, period2, stochPeriod)[bar];
        }

        public int DSSSeries(int period1, int period2, int stochPeriod)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.DSS.Series(base.Bars, period1, period2, stochPeriod));
            return key;
        }

        public double DX(int bar, int period)
        {
            return WealthLab.Indicators.DX.Series(base.Bars, period)[bar];
        }

        public int DXSeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.DX.Series(base.Bars, period));
            return key;
        }

        public double EMA(int bar, int series, int period)
        {
            return WealthLab.Indicators.EMA.Series(this.priceSeries[series], period, this.emaCalculation)[bar];
        }

        public int EMASeries(int Series, int Period)
        {
            this.check_series(Series, "Series");
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.EMA.Series(this.priceSeries[Series], Period, this.emaCalculation));
            return key;
        }

        public double EMMinus(int bar, int series, int period)
        {
            return WealthLab.Indicators.EMMinus.Value(bar, this.priceSeries[series], period);
        }

        public int EMMinusSeries(int Series, int Period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.EMMinus.Series(this.priceSeries[Series], Period));
            return key;
        }

        public double EMPlus(int bar, int series, int period)
        {
            return WealthLab.Indicators.EMPlus.Value(bar, this.priceSeries[series], period);
        }

        public int EMPlusSeries(int Series, int Period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.EMPlus.Series(this.priceSeries[Series], Period));
            return key;
        }

        public void EnableNotes(bool enable)
        {
            this.not_supported("EnableNotes");
        }

        public void EnableTradeNotes(bool text, bool arrow, bool circle)
        {
            this.not_supported("EnableTradeNotes");
        }

        protected override void Execute()
        {
        }

        public double FAMA(int bar, int series, double fastLimit, double slowLimit)
        {
            return WealthLab.Indicators.FAMA.Series(this.priceSeries[series], fastLimit, slowLimit)[bar];
        }

        public int FAMASeries(int Series, double fastLimit, double slowLimit)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.FAMA.Series(this.priceSeries[Series], fastLimit, slowLimit));
            return key;
        }

        public void FileClear(int fileHandle)
        {
            if (fileHandle != -1)
            {
                if (!this.fileStreams.ContainsKey(fileHandle))
                {
                    throw new ArgumentException("No such file handle exists", "fileHandle");
                }
                FileStream stream = this.fileStreams[fileHandle];
                string name = stream.Name;
                this.FileClose(fileHandle);
                File.Delete(name);
                FileStream stream2 = File.Create(name);
                this.fileStreams[fileHandle] = stream2;
            }
        }

        public void FileClose(int fileHandle)
        {
            if (fileHandle != -1)
            {
                ObjectDisposedException exception;
                if (this.fileWriters.ContainsKey(fileHandle))
                {
                    TextWriter writer = this.fileWriters[fileHandle];
                    this.fileWriters.Remove(fileHandle);
                    writer.Flush();
                    try
                    {
                        writer.Close();
                    }
                    catch (ObjectDisposedException exception1)
                    {
                        exception = exception1;
                        this.noop(exception);
                    }
                    writer = null;
                }
                if (this.fileReaders.ContainsKey(fileHandle))
                {
                    TextReader reader = this.fileReaders[fileHandle];
                    this.fileReaders.Remove(fileHandle);
                    try
                    {
                        reader.Close();
                    }
                    catch (ObjectDisposedException exception2)
                    {
                        exception = exception2;
                        this.noop(exception);
                    }
                    reader = null;
                }
                if (this.fileStreams.ContainsKey(fileHandle))
                {
                    FileStream stream = this.fileStreams[fileHandle];
                    this.fileStreams.Remove(fileHandle);
                    try
                    {
                        stream.Flush();
                        stream.Close();
                    }
                    catch (ObjectDisposedException exception3)
                    {
                        exception = exception3;
                        this.noop(exception);
                    }
                    stream = null;
                }
            }
        }

        public int FileCreate(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return this.FileOpen(filePath);
        }

        public bool FileEOF(int fileHandle)
        {
            if (fileHandle == -1)
            {
                return false;
            }
            return this.getReader(fileHandle).EndOfStream;
        }

        public void FileFlush(int fileHandle)
        {
            if (fileHandle != -1)
            {
                TextWriter writer = this.fileWriters[fileHandle];
                if (writer != null)
                {
                    writer.Flush();
                }
                FileStream stream = this.fileStreams[fileHandle];
                if (stream != null)
                {
                    stream.Flush();
                }
            }
        }

        public int FileOpen(string filePath)
        {
            FileStream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            stream.Seek(0L, SeekOrigin.Begin);
            int num = ++this.nextFileHandle;
            this.fileStreams[num] = stream;
            return num;
        }

        public string FileRead(int fileHandle)
        {
            if (fileHandle == -1)
            {
                return "";
            }
            string str = this.getReader(fileHandle).ReadLine();
            if (str == null)
            {
                str = "";
            }
            return str;
        }

        public void FileWrite(int fileHandle, string text)
        {
            if (fileHandle != -1)
            {
                TextWriter writer;
                if (!this.fileStreams.ContainsKey(fileHandle))
                {
                    throw new Exception(string.Concat(new object[] { "Attempting to FileWrite(", fileHandle, ",", text, ") to a non-existant file handle!" }));
                }
                FileStream stream = this.fileStreams[fileHandle];
                if (!this.fileWriters.ContainsKey(fileHandle))
                {
                    writer = new StreamWriter(stream);
                    this.fileWriters[fileHandle] = writer;
                }
                else
                {
                    writer = this.fileWriters[fileHandle];
                }
                writer.WriteLine(text);
            }
        }

        public int FindNamedSeries(string Name)
        {
            foreach (int num in this.priceSeries.Keys)
            {
                DataSeries series = this.priceSeries[num];
                if (series.Description == Name)
                {
                    return num;
                }
            }
            return -1;
        }

        public double FIR(int bar, int series, string filter)
        {
            return WealthLab.Indicators.FIR.Value(bar, this.priceSeries[series], this.weightsFromFilter(filter));
        }

        public int FIRSeries(int Series, string filter)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.FIR.Series(this.priceSeries[Series], filter));
            return key;
        }

        public string FloatToStr(double f)
        {
            return f.ToString();
        }

        public string FormatFloat(string format, double value)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            for (int i = 0; i < format.Length; i++)
            {
                char ch = format[i];
                switch (ch)
                {
                    case ';':
                    case '\\':
                    case '"':
                    case '%':
                    case '\'':
                    {
                        builder2.Append(@"\" + ch.ToString());
                        continue;
                    }
                }
                builder2.Append(ch);
            }
            return value.ToString(builder2.ToString());
        }

        public double Frac(double value)
        {
            return (value - Math.Floor(value));
        }

        public int GetDate(int bar)
        {
            DateTime time = base.Bars.Date[bar];
            return int.Parse(time.ToString("yyyyMMdd"));
        }

        public int GetDay(int bar)
        {
            DateTime time = base.Bars.Date[bar];
            return time.Day;
        }

        public string GetDescription(int Series)
        {
            this.check_series(Series, "Series");
            return this.priceSeries[Series].Description;
        }

        public FileStream GetFileStream(int file)
        {
            return this.fileStreams[file];
        }

        public int GetHour(int bar)
        {
            DateTime time = base.Bars.Date[bar];
            return time.Hour;
        }

        public int GetMinute(int bar)
        {
            DateTime time = base.Bars.Date[bar];
            return time.Minute;
        }

        public int GetMonth(int bar)
        {
            DateTime time = base.Bars.Date[bar];
            return time.Month;
        }

        public double GetOptVar(int n)
        {
            return 0.0;
        }

        public double GetPositionData(int P)
        {
            this.check_position(P);
            double tag = 0.0;
            if ((base.Positions[P].Tag != null) && (base.Positions[P].Tag is double))
            {
                tag = (double) base.Positions[P].Tag;
            }
            return tag;
        }

        public double GetPositionPriority(int P)
        {
            this.check_position(P);
            return base.Positions[P].Priority;
        }

        private StreamReader getReader(int fileHandle)
        {
            if (!this.fileReaders.ContainsKey(fileHandle))
            {
                FileStream stream = this.fileStreams[fileHandle];
                if (stream == null)
                {
                    throw new Exception("Attempting to FileRead(" + fileHandle + ") from a non-existant file handle!");
                }
                long num = stream.Seek(0L, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(stream);
                this.fileReaders[fileHandle] = reader;
                return reader;
            }
            return this.fileReaders[fileHandle];
        }

        public DataSeries GetSeries(int Series)
        {
            this.check_series(Series, "Series");
            return this.priceSeries[Series];
        }

        public double GetSeriesValue(int Bar, int Series)
        {
            this.check_series(Series, "Series");
            this.check_bar(Bar);
            DataSeries series = this.priceSeries[Series];
            if (series.Count != base.Bars.Count)
            {
                throw new Exception("GetSeriesValue(" + Bar.ToString() + "): ds.Count = " + series.Count.ToString() + " <> base.Bars.Count: " + base.Bars.Count.ToString() + ", firstvalid: " + series.FirstValidValue.ToString());
            }
            return series[Bar];
        }

        public int GetTickCount()
        {
            return (int) (DateTime.Now.Ticks / 0x2710L);
        }

        public int GetTime(int bar)
        {
            DateTime time = base.Bars.Date[bar];
            return int.Parse(time.ToString("HHmm"));
        }

        public string GetToken(string s1, int tokenNumber, string delimiter)
        {
            if (tokenNumber < 0)
            {
                throw new ArgumentException("tokenNumber must be greater than 0", "tokenNumber");
            }
            string[] separator = new string[] { delimiter };
            return s1.Split(separator, tokenNumber + 1, StringSplitOptions.None)[tokenNumber];
        }

        public int GetYear(int bar)
        {
            DateTime time = base.Bars.Date[bar];
            return time.Year;
        }

        public double Highest(int bar, int series, int period)
        {
            return WealthLab.Indicators.Highest.Value(bar, this.priceSeries[series], period);
        }

        public int HighestBar(int bar, int series, int period)
        {
            return (int) Math.Round(WealthLab.Indicators.HighestBar.Value(bar, this.priceSeries[series], period));
        }

        public int HighestBarSeries(int Series, int period)
        {
            this.check_series(Series, "Series");
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.HighestBar.Series(this.priceSeries[Series], period));
            return key;
        }

        public int HighestSeries(int Series, int period)
        {
            this.check_series(Series, "Series");
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.Highest.Series(this.priceSeries[Series], period));
            return key;
        }

        public double HV(int bar, int series, int period, int span)
        {
            return WealthLab.Indicators.HV.Series(this.priceSeries[series], period, span)[bar];
        }

        public int HVSeries(int Series, int period, int span)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.HV.Series(this.priceSeries[Series], period, span));
            return this.priceSeriesKey++;
        }

        public double Hypot(double x, double y)
        {
            return Math.Sqrt((x * x) + (y * y));
        }

        protected void Init()
        {
            this.priceSeries.Clear();
            this.panes.Clear();
            this.panes.Add(0, base.PricePane);
            this.panes.Add(1, base.VolumePane);
        }

        public string Input(string Caption)
        {
            string caption = "Wealth-Lab Input";
            string label = Caption;
            return InputBox.Show(caption, label);
        }

        public void Insert(string source, ref string s, int index)
        {
            string str = s.Insert(index - 1, source);
            s = str;
        }

        public void InstallBreakEvenStop(double trigger)
        {
            this._AS_BreakEvenStopTrigger = trigger;
        }

        public void InstallProfitTarget(double TargetLevel)
        {
            this._AS_ProfitTarget = TargetLevel;
        }

        public void InstallReverseBreakEvenStop(double LossLevel)
        {
            this._AS_ReversalBreakEvenStopLevel = LossLevel;
        }

        public void InstallStopLoss(double StopLevel)
        {
            this._AS_StopLoss = StopLevel;
        }

        public void InstallTimeBasedExit(int Bars)
        {
            this._AS_TimeBasedExit = Bars;
        }

        public void InstallTrailingStop(double trigger, double stopLevel)
        {
            this._AS_TrailingStopTrigger = trigger;
            this._AS_TrailingStopLevel = stopLevel;
        }

        public string IntToStr(bool i)
        {
            return (i ? "true" : "false");
        }

        public string IntToStr(int i)
        {
            return Convert.ToString(i);
        }

        public bool IsLeapYear(int year)
        {
            return ((((year % 4) == 0) && ((year % 100) != 0)) || ((year % 400) == 0));
        }

        public double Kalman(int bar, int series)
        {
            return WealthLab.Indicators.Kalman.Series(this.priceSeries[series])[bar];
        }

        public int KalmanSeries(int Series)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.Kalman.Series(this.priceSeries[Series]));
            return this.priceSeriesKey++;
        }

        public double KAMA(int bar, int series, int period)
        {
            return WealthLab.Indicators.KAMA.Series(this.priceSeries[series], period)[bar];
        }

        public int KAMASeries(int Series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.KAMA.Series(this.priceSeries[Series], period));
            return this.priceSeriesKey++;
        }

        public double KeltnerLower(int bar, int period1, int period2)
        {
            return WealthLab.Indicators.KeltnerLower.Series(base.Bars, period1, period2)[bar];
        }

        public int KeltnerLowerSeries(int period1, int period2)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.KeltnerLower.Series(base.Bars, period1, period2));
            return this.priceSeriesKey++;
        }

        public double KeltnerUpper(int bar, int period1, int period2)
        {
            return WealthLab.Indicators.KeltnerUpper.Series(base.Bars, period1, period2)[bar];
        }

        public int KeltnerUpperSeries(int period1, int period2)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.KeltnerUpper.Series(base.Bars, period1, period2));
            return this.priceSeriesKey++;
        }

        public int Length(string str)
        {
            return str.Length;
        }

        public double LinearReg(int bar, int series, int period)
        {
            return WealthLab.Indicators.LinearReg.Value(bar, this.priceSeries[series], period);
        }

        public double LinearRegLine(int series1, int start, int end, int predict)
        {
            throw new Exception("not supported");
        }

        public int LinearRegSeries(int Series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.LinearReg.Series(this.priceSeries[Series], period));
            return this.priceSeriesKey++;
        }

        public double LinearRegSlope(int bar, int series, int period)
        {
            return WealthLab.Indicators.LinearRegSlope.Value(bar, this.priceSeries[series], period);
        }

        public int LinearRegSlopeSeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.LinearRegSlope.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public double Log2(double value)
        {
            return (Math.Log(value) * 1.442695041);
        }

        public string LowerCase(string value)
        {
            return value.ToLower(CultureInfo.InvariantCulture);
        }

        public double Lowest(int bar, int series, int period)
        {
            return WealthLab.Indicators.Lowest.Value(bar, this.priceSeries[series], period);
        }

        public int LowestBar(int bar, int series, int period)
        {
            return (int) Math.Round(WealthLab.Indicators.LowestBar.Value(bar, this.priceSeries[series], period));
        }

        public int LowestBarSeries(int Series, int period)
        {
            this.check_series(Series, "Series");
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.LowestBar.Series(this.priceSeries[Series], period));
            return key;
        }

        public int LowestSeries(int Series, int period)
        {
            this.check_series(Series, "Series");
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.Lowest.Series(this.priceSeries[Series], period));
            return key;
        }

        public double MACD(int bar, int series)
        {
            return WealthLab.Indicators.MACD.Series(this.priceSeries[series])[bar];
        }

        public int MACDSeries(int Series)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.MACD.Series(this.priceSeries[Series]));
            return this.priceSeriesKey++;
        }

        public double MAMA(int bar, int series, double fastLimit, double slowLimit)
        {
            return WealthLab.Indicators.MAMA.Series(this.priceSeries[series], fastLimit, slowLimit)[bar];
        }

        public int MAMASeries(int Series, double fastLimit, double slowLimit)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.MAMA.Series(this.priceSeries[Series], fastLimit, slowLimit));
            return key;
        }

        public double Median(int bar, int series, int period)
        {
            return WealthLab.Indicators.Median.Value(bar, this.priceSeries[series], period);
        }

        public int MedianSeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.Median.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public double MFI(int bar, int period)
        {
            return WealthLab.Indicators.MFI.Series(base.Bars, period)[bar];
        }

        public int MFISeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.MFI.Series(base.Bars, period));
            return key;
        }

        public double Momentum(int bar, int series, int period)
        {
            return WealthLab.Indicators.Momentum.Value(bar, this.priceSeries[series], period);
        }

        public double MomentumPct(int bar, int series, int period)
        {
            return WealthLab.Indicators.MomentumPct.Value(bar, this.priceSeries[series], period);
        }

        public int MomentumPctSeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.MomentumPct.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public int MomentumSeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.Momentum.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public double MoneyFlow(int bar)
        {
            return WealthLab.Indicators.MoneyFlow.Series(base.Bars)[bar];
        }

        public int MoneyFlowSeries()
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.MoneyFlow.Series(base.Bars));
            return this.priceSeriesKey++;
        }

        public int MultiplySeries(int Series1, int Series2)
        {
            this.check_series(Series1, "Series1");
            this.check_series(Series2, "Series2");
            DataSeries series = this.priceSeries[Series1];
            DataSeries series2 = this.priceSeries[Series2];
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, series * series2);
            return key;
        }

        public int MultiplySeriesValue(int Series, double Value)
        {
            this.check_series(Series, "Series");
            DataSeries series = this.priceSeries[Series];
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, (DataSeries) (series * Value));
            return key;
        }

        private void noop(Exception ex)
        {
        }

        private void not_supported(string item)
        {
            this.print_warning_once(item, "is not supported");
        }

        public double OBV(int bar)
        {
            return WealthLab.Indicators.OBV.Series(base.Bars)[bar];
        }

        public int OBVSeries()
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.OBV.Series(base.Bars));
            return this.priceSeriesKey++;
        }

        public int OffsetSeries(int Series, int shift)
        {
            this.check_series(Series, "Series");
            DataSeries series = this.priceSeries[Series];
            int key = this.priceSeriesKey++;
            if (shift < 0)
            {
                this.priceSeries.Add(key, series >> -shift);
                return key;
            }
            this.priceSeries.Add(key, series << shift);
            return key;
        }

        public double OpenInterest(int bar)
        {
            DataSeries series = base.Bars.FindNamedSeries("OpenInterest");
            if (series == null)
            {
                return 0.0;
            }
            return series[bar];
        }

        public bool OptionExpiryDate(int bar)
        {
            WealthLab.Rules.OptionExpiryDate date = new WealthLab.Rules.OptionExpiryDate(base.Bars);
            return (date.DaysSinceOptionExpiryDate(bar) == 0);
        }

        public int Ord(string Value)
        {
            if (Value.Length < 1)
            {
                throw new ArgumentException("value can't be empty", "value");
            }
            return Value[0];
        }

        public double Parabolic(int bar, double accelUp, double accelDown, double accelMax)
        {
            return WealthLab.Indicators.Parabolic.Series(base.Bars, accelUp, accelDown, accelMax)[bar];
        }

        public int ParabolicSeries(double accelUp, double accelDown, double accelMax)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.Parabolic.Series(base.Bars, accelUp, accelDown, accelMax));
            return this.priceSeriesKey++;
        }

        public double Peak(int bar, int series, double reversal)
        {
            return WealthLab.Indicators.Peak.Value(bar, this.priceSeries[series], reversal, this.peakThroughMode);
        }

        public int PeakBar(int bar, int series, double reversal)
        {
            return (int) Math.Round(WealthLab.Indicators.PeakBar.Value(bar, this.priceSeries[series], reversal, this.peakThroughMode));
        }

        public int PeakBarSeries(int series, double reversal)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.PeakBar.Series(this.priceSeries[series], reversal, this.peakThroughMode));
            return this.priceSeriesKey++;
        }

        public int PeakSeries(int series, double reversal)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.Peak.Series(this.priceSeries[series], reversal, this.peakThroughMode));
            return this.priceSeriesKey++;
        }

        public void PlaySound(string FileName)
        {
            new SoundPlayer(FileName).Play();
        }

        public void PlotSeries(int Series, int Pane, int WLColor, int Style)
        {
            this.PlotSeriesLabel(Series, Pane, WLColor, Style, "");
        }

        public void PlotSeriesLabel(int Series, int Pane, int WLColor, int Style, string Label)
        {
            this.check_series(Series, "Series");
            this.check_pane(Pane);
            DataSeries series = this.priceSeries[Series];
            ChartPane pane = this.panes[Pane];
            LineStyle style = this.WLStyle2LineStyle(Style);
            System.Drawing.Color color = this.WLColor2Color(WLColor);
            int width = this.WLStyle2Width(Style);
            base.PlotSeries(pane, series, color, style, width);
            if (Label != "")
            {
                if (!this.warn_PlotSeriesLabel)
                {
                    base.PrintDebug("PlotSeriesLabel: label text and position may be wrong");
                    this.warn_PlotSeriesLabel = true;
                }
                base.DrawLabel(pane, Label, color);
            }
        }

        public void PlotSymbol(string symbol, int pane, int wlcolor, int style)
        {
            if ((style != 0) && !this.warn_PlotSymbol)
            {
                base.PrintDebug("PlotSymbol: styles other than OHLC are not supported, using OHLC");
            }
            ChartPane pane2 = this.panes[pane];
            System.Drawing.Color upBarColor = this.WLColor2Color(wlcolor);
            Bars externalSymbol = base.GetExternalSymbol(symbol, true);
            base.PlotSymbol(pane2, externalSymbol, upBarColor, upBarColor);
        }

        public void PlotSyntheticSymbol(string symbol, int open, int high, int low, int close, int pane, int wlcolor, int style)
        {
            if ((style != 0) && !this.warn_PlotSyntheticSymbol)
            {
                base.PrintDebug("PlotSyntheticSymbol: styles other than OHLC are not supported, using OHLC");
            }
            ChartPane pane2 = this.panes[pane];
            System.Drawing.Color upBarColor = this.WLColor2Color(wlcolor);
            DataSeries series = this.priceSeries[open];
            DataSeries series2 = this.priceSeries[high];
            DataSeries series3 = this.priceSeries[low];
            DataSeries series4 = this.priceSeries[close];
            base.PlotSyntheticSymbol(pane2, symbol, series, series2, series3, series4, null, upBarColor, upBarColor);
        }

        public int Pos(string substring, string str)
        {
            if (substring.Length < 1)
            {
                throw new ArgumentException("substring must contain a character", "substring");
            }
            char ch = substring[0];
            int index = str.IndexOf(ch);
            if (index < 0)
            {
                return 0;
            }
            return (index + 1);
        }

        public bool PositionActive(int P)
        {
            this.check_position(P);
            return base.Positions[P].Active;
        }

        public int PositionBarsHeld(int Position)
        {
            this.check_position(Position);
            int barsHeld = base.Positions[Position].BarsHeld;
            if (barsHeld > 1)
            {
                barsHeld--;
            }
            return barsHeld;
        }

        public double PositionBasisPrice(int Position)
        {
            this.check_position(Position);
            return base.Positions[Position].BasisPrice;
        }

        public int PositionEntryBar(int Position)
        {
            if (Position < 0)
            {
                return -1;
            }
            this.check_position(Position);
            return base.Positions[Position].EntryBar;
        }

        public double PositionEntryPrice(int Position)
        {
            if (Position < 0)
            {
                return 0.0;
            }
            this.check_position(Position);
            return base.Positions[Position].EntryPrice;
        }

        public int PositionExitBar(int Position)
        {
            if (Position < 0)
            {
                return -1;
            }
            this.check_position(Position);
            if (base.Positions[Position].Active)
            {
                return 0;
            }
            return base.Positions[Position].ExitBar;
        }

        public double PositionExitPrice(int Position)
        {
            this.check_position(Position);
            return base.Positions[Position].ExitPrice;
        }

        public string PositionExitSignalName(int Position)
        {
            this.check_position(Position);
            string exitSignal = base.Positions[Position].ExitSignal;
            return ((exitSignal == null) ? "" : exitSignal);
        }

        public bool PositionLong(int Position)
        {
            if (Position < 0)
            {
                return false;
            }
            this.check_position(Position);
            return (base.Positions[Position].PositionType == PositionType.Long);
        }

        public double PositionMAE(int P)
        {
            this.print_warning_once("PositionMAE:", "results are based on 1 share");
            this.check_position(P);
            return base.Positions[P].MAE;
        }

        public double PositionMAEPct(int P)
        {
            this.check_position(P);
            return base.Positions[P].MAEPercent;
        }

        public double PositionMFE(int P)
        {
            this.print_warning_once("PositionMFE:", "results are based on 1 share");
            this.check_position(P);
            return base.Positions[P].MFE;
        }

        public double PositionMFEPct(int P)
        {
            this.check_position(P);
            return base.Positions[P].MFEPercent;
        }

        public double PositionOpenMAE(int bar, int P)
        {
            this.print_warning_once("PositionOpenMAE:", "results are based on 1 share");
            this.check_position(P);
            return base.Positions[P].MAEAsOfBar(bar);
        }

        public double PositionOpenMAEPct(int bar, int P)
        {
            this.check_position(P);
            return base.Positions[P].MAEAsOfBarPercent(bar);
        }

        public double PositionOpenMFE(int bar, int P)
        {
            this.print_warning_once("PositionOpenMFE:", "results are based on 1 share");
            this.check_position(P);
            return base.Positions[P].MFEAsOfBar(bar);
        }

        public double PositionOpenMFEPct(int bar, int P)
        {
            this.check_position(P);
            return base.Positions[P].MFEAsOfBarPercent(bar);
        }

        public double PositionOpenProfit(int bar, int P)
        {
            this.print_warning_once("PositionOpenProfit:", "results are based on 1 share");
            this.check_position(P);
            return base.Positions[P].NetProfitAsOfBar(bar);
        }

        public double PositionOpenProfitPct(int bar, int P)
        {
            this.check_position(P);
            return base.Positions[P].NetProfitAsOfBarPercent(bar);
        }

        public int PositionOrderType(int P)
        {
            if (P >= 0)
            {
                this.check_position(P);
                Position position = base.Positions[P];
                switch (position.EntryOrderType)
                {
                    case OrderType.Limit:
                        return 2;

                    case OrderType.Stop:
                        return 1;

                    case OrderType.AtClose:
                        return 3;
                }
            }
            return 0;
        }

        public double PositionProfit(int P)
        {
            this.print_warning_once("PositionProfit:", "results are based on 1 share");
            this.check_position(P);
            return base.Positions[P].NetProfit;
        }

        public double PositionProfitPct(int P)
        {
            this.check_position(P);
            return base.Positions[P].NetProfitPercent;
        }

        public int PositionShares(int P)
        {
            this.print_warning_once("PositionShares:", "always returns 1");
            this.check_position(P);
            return (int) Math.Round(base.Positions[P].Shares);
        }

        public bool PositionShort(int Position)
        {
            if (Position < 0)
            {
                return false;
            }
            this.check_position(Position);
            return (base.Positions[Position].PositionType == PositionType.Short);
        }

        public string PositionSignalName(int P)
        {
            this.check_position(P);
            return base.Positions[P].EntrySignal;
        }

        public string PositionSymbol(int P)
        {
            this.check_position(P);
            return base.Positions[P].Bars.Symbol;
        }

        public double PriceAverage(int Bar)
        {
            return this.GetSeriesValue(Bar, this.WL_AVERAGE);
        }

        public double PriceAverageC(int Bar)
        {
            return this.GetSeriesValue(Bar, this.WL_AVERAGEC);
        }

        public double PriceClose(int Bar)
        {
            return base.Bars.Close[Bar];
        }

        public double PriceHigh(int Bar)
        {
            return base.Bars.High[Bar];
        }

        public double PriceLow(int Bar)
        {
            return base.Bars.Low[Bar];
        }

        public double PriceOpen(int Bar)
        {
            return base.Bars.Open[Bar];
        }

        private void print_warning_once(string item, string message)
        {
            if (!this.warning_printed.ContainsKey(item))
            {
                base.PrintDebug(item + " " + message);
                this.warning_printed[item] = true;
            }
        }

        public double QStick(int bar, int period)
        {
            return WealthLab.Indicators.QStick.Series(base.Bars, period)[bar];
        }

        public int QStickSeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.QStick.Series(base.Bars, period));
            return key;
        }

        public double RadToDeg(double radians)
        {
            return ((radians * 180.0) / 3.1415926535897931);
        }

        public double RandG(double mean, double stddev)
        {
            throw new Exception("not supported");
        }

        public int RandomInt(int Limit)
        {
            if (this.randomGen == null)
            {
                this.randomSeed = new Random().Next();
                this.randomGen = new Random(this.randomSeed);
            }
            return this.randomGen.Next(Limit);
        }

        public void Randomize()
        {
            this.randomGen = new Random();
        }

        public void RestorePrimarySeries()
        {
            base.RestoreContext();
        }

        public double ROC(int bar, int series, int period)
        {
            return WealthLab.Indicators.ROC.Value(bar, this.priceSeries[series], period);
        }

        public int ROCSeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.ROC.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public int Round(double value)
        {
            return (int) Math.Round(value, MidpointRounding.ToEven);
        }

        public double RSI(int bar, int series, int period)
        {
            return WealthLab.Indicators.RSI.Series(this.priceSeries[series], period)[bar];
        }

        public int RSISeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.RSI.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public double RSquared(int bar, int series, int period)
        {
            return WealthLab.Indicators.RSquared.Value(bar, this.priceSeries[series], period);
        }

        public int RSquaredSeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.RSquared.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public void RunProgram(string ProgramName, bool wait)
        {
            Interaction.Shell(ProgramName, AppWinStyle.NormalFocus, wait, -1);
        }

        public double RVI(int bar, int period)
        {
            return WealthLab.Indicators.RVI.Series(base.Bars, period)[bar];
        }

        public int RVISeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.RVI.Series(base.Bars, period));
            return key;
        }

        public void SaveChartImage(string filename, int width, int height, string imageType)
        {
            ImageFormat gif;
            Bitmap chartBitmap = base.GetChartBitmap(width, height);
            string str = imageType;
            if (str != null)
            {
                if (!(str == "BMP"))
                {
                    if (str == "GIF")
                    {
                        gif = ImageFormat.Gif;
                        goto Label_004C;
                    }
                }
                else
                {
                    gif = ImageFormat.Bmp;
                    goto Label_004C;
                }
            }
            throw new ArgumentException("unsupported image type, expected BMP or GIF", "imageType");
        Label_004C:
            chartBitmap.Save(filename, gif);
        }

        public void SellAtClose(int Bar, int P, string SignalName)
        {
            if (P != -1)
            {
                this.check_positionOrAll(P);
                Position position = (P == -123) ? Position.AllPositions : base.Positions[P];
                base.SellAtClose(Bar, position, SignalName);
            }
        }

        public bool SellAtLimit(int Bar, double LimitPrice, int P, string SignalName)
        {
            if (P == -1)
            {
                return false;
            }
            this.check_positionOrAll(P);
            Position position = (P == -123) ? Position.AllPositions : base.Positions[P];
            return base.SellAtLimit(Bar, position, LimitPrice, SignalName);
        }

        public void SellAtMarket(int Bar, int P, string SignalName)
        {
            if (P != -1)
            {
                this.check_positionOrAll(P);
                Position position = (P == -123) ? Position.AllPositions : base.Positions[P];
                base.SellAtMarket(Bar, position, SignalName);
            }
        }

        public bool SellAtStop(int Bar, double StopPrice, int P, string SignalName)
        {
            if (P == -1)
            {
                return false;
            }
            this.check_positionOrAll(P);
            Position position = (P == -123) ? Position.AllPositions : base.Positions[P];
            return base.SellAtStop(Bar, position, StopPrice, SignalName);
        }

        public bool SellAtTrailingStop(int Bar, double StopPrice, int P, string SignalName)
        {
            this.check_positionOrAll(P);
            Position position = (P == -123) ? Position.AllPositions : base.Positions[P];
            return base.SellAtTrailingStop(Bar, position, StopPrice, SignalName);
        }

        public void SetAutoStopMode(int mode)
        {
            switch (mode)
            {
                case 1:
                    this.print_warning_once("SetAutoStopMode(AsPoint)", "is not supported");
                    break;

                case 2:
                    this.print_warning_once("SetAutoStopMode(AsDollar)", "is not supported");
                    break;
            }
        }

        public void SetBackgroundColor(int bar, int Color)
        {
            if ((bar >= 0) && (bar < base.Bars.Count))
            {
                base.SetBackgroundColor(bar, this.WLColor2Color(Color));
            }
        }

        public void SetBarColor(int bar, int wlcolor)
        {
            if ((bar >= 0) && (bar < base.Bars.Count))
            {
                System.Drawing.Color color = this.WLColor2Color(wlcolor);
                base.SetBarColor(bar, color);
            }
        }

        public void SetBarColors(int upBars, int downBars)
        {
            System.Drawing.Color colorUp = this.WLColor2Color(upBars);
            System.Drawing.Color colorDown = this.WLColor2Color(downBars);
            base.SetBarColors(colorUp, colorDown);
        }

        public void SetCommission(double commission)
        {
            this.not_supported("SetCommission");
        }

        public void SetDescription(int Series, string Description)
        {
            this.check_series(Series, "Series");
            this.priceSeries[Series].Description = Description;
        }

        public void SetLogScale(int pane, bool useLogScale)
        {
            ChartPane pane2 = this.panes[pane];
            base.SetLogScale(pane2, useLogScale);
        }

        public void SetPaneBackgroundColor(int bar, int pane, int wlcolor)
        {
            if ((bar >= 0) && (bar < base.Bars.Count))
            {
                ChartPane pane2 = this.panes[pane];
                System.Drawing.Color color = this.WLColor2Color(wlcolor);
                base.SetPaneBackgroundColor(pane2, bar, color);
            }
        }

        public void SetPaneMinMax(int pane, double min, double max)
        {
            ChartPane pane2 = this.panes[pane];
            base.SetPaneMinMax(pane2, min, max);
        }

        public void SetPeakTroughMode(int mode)
        {
            switch (mode)
            {
                case 0:
                    this.peakThroughMode = PeakTroughMode.Percent;
                    break;

                case 1:
                    this.peakThroughMode = PeakTroughMode.Value;
                    break;

                default:
                    throw new ArgumentException("unknown value for PeakTroughMode, expecting #AsPercent or #AsPoint", "mode");
            }
        }

        public void SetPositionData(int P, double value)
        {
            this.check_position(P);
            base.Positions[P].Tag = value;
        }

        public void SetPositionPriority(int P, double priority)
        {
            this.check_position(P);
            base.Positions[P].Priority = priority;
        }

        public void SetPositionSize(double commission)
        {
            this.not_supported("SetPositionSize");
        }

        public void SetPrimarySeries(string symbol)
        {
            base.SetContext(symbol, this.synchExternalSeries);
        }

        public void SetRandSeed(double seed)
        {
            seed *= 100.0;
            this.randomSeed = Convert.ToInt32(seed);
            this.randomGen = new Random(this.randomSeed);
        }

        public void SetRiskStopLevel(double riskStopPrice)
        {
            base.RiskStopLevel = riskStopPrice;
        }

        public void SetSeriesBarColor(int bar, int series, int wlcolor)
        {
            if ((bar >= 0) && (bar < base.Bars.Count))
            {
                System.Drawing.Color color = this.WLColor2Color(wlcolor);
                DataSeries series2 = this.priceSeries[series];
                base.SetSeriesBarColor(bar, series2, color);
            }
        }

        public void SetSeriesValue(int Bar, int Series, double Value)
        {
            this.check_bar(Bar);
            this.check_series(Series, "Series");
            DataSeries series = this.priceSeries[Series];
            series[Bar] = Value;
        }

        public void SetShareCap(int cap)
        {
            this.not_supported("SetShareCap");
        }

        public void SetShareFloor(int floor)
        {
            this.not_supported("SetShareFloor");
        }

        public void SetShareSize(int shares)
        {
            this.not_supported("SetShareSize");
        }

        public void SetSlippage(bool es, float sl, bool lo)
        {
            this.not_supported("SetSlippage");
        }

        public void SetStdDevCalculation(StdDevCalculation mode)
        {
            this.stddevCalculation = mode;
        }

        public bool ShortAtClose(int Bar, string SignalName)
        {
            return (base.ShortAtClose(Bar, SignalName) != null);
        }

        public bool ShortAtLimit(int Bar, double limit, string SignalName)
        {
            return (base.ShortAtLimit(Bar, limit, SignalName) != null);
        }

        public bool ShortAtMarket(int Bar, string SignalName)
        {
            return (base.ShortAtMarket(Bar, SignalName) != null);
        }

        public bool ShortAtStop(int Bar, double stop, string SignalName)
        {
            return (base.ShortAtStop(Bar, stop, SignalName) != null);
        }

        public void ShowMessage(string Message)
        {
            MessageBox.Show(Message, "Wealth-Lab Message");
        }

        public void Sleep(int Milliseconds)
        {
            Thread.Sleep(Milliseconds);
        }

        public double SMA(int bar, int series, int period)
        {
            return WealthLab.Indicators.SMA.Value(bar, this.priceSeries[series], period);
        }

        public int SMASeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.SMA.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public int SplitPosition(int P, double RetainPct)
        {
            this.check_position(P);
            Position position = base.Positions[P];
            Position item = base.SplitPosition(position, RetainPct);
            return base.Positions.IndexOf(item);
        }

        public double Sqr(double myValue)
        {
            return (myValue * myValue);
        }

        public double StdDev(int bar, int series, int period)
        {
            return WealthLab.Indicators.StdDev.Value(bar, this.priceSeries[series], period, this.stddevCalculation);
        }

        public int StdDevSeries(int Series, int Period)
        {
            this.check_series(Series, "Series");
            DataSeries series = this.priceSeries[Series];
            DataSeries series2 = WealthLab.Indicators.StdDev.Series(series, Period, this.stddevCalculation);
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, series2);
            return key;
        }

        public double StdError(int bar, int series, int period)
        {
            return WealthLab.Indicators.StdError.Value(bar, this.priceSeries[series], period);
        }

        public int StdErrorSeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.StdError.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public double StochD(int bar, int period, int smooth)
        {
            return WealthLab.Indicators.StochD.Series(base.Bars, smooth, period)[bar];
        }

        public int StochDSeries(int period, int smooth)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.StochD.Series(base.Bars, smooth, period));
            return this.priceSeriesKey++;
        }

        public double StochK(int bar, int period)
        {
            return WealthLab.Indicators.StochK.Series(base.Bars, period)[bar];
        }

        public int StochKSeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.StochK.Series(base.Bars, period));
            return key;
        }

        public double StochRSI(int bar, int series, int period)
        {
            return WealthLab.Indicators.StochRSI.Series(this.priceSeries[series], period)[bar];
        }

        public int StochRSISeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.StochRSI.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public int StrToDate(string value)
        {
            return int.Parse(DateTime.Parse(value, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal).ToString("yyyyMMdd"));
        }

        public double StrToFloatDef(string value, double def)
        {
            double num;
            if (!double.TryParse(value, out num))
            {
                num = def;
            }
            return num;
        }

        public int StrToIntDef(string value, int def)
        {
            int num;
            if (!int.TryParse(value, out num))
            {
                num = def;
            }
            return num;
        }

        public int StrToTime(string value)
        {
            return int.Parse(DateTime.Parse(value, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal).ToString("HHmm"));
        }

        public int SubtractSeries(int Series1, int Series2)
        {
            this.check_series(Series1, "Series1");
            this.check_series(Series2, "Series2");
            DataSeries series = this.priceSeries[Series1];
            DataSeries series2 = this.priceSeries[Series2];
            int key = this.priceSeriesKey++;
            DataSeries series3 = series - series2;
            this.priceSeries.Add(key, series3);
            return key;
        }

        public int SubtractSeriesValue(int Series1, double value)
        {
            this.check_series(Series1, "Series1");
            DataSeries series = this.priceSeries[Series1];
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, series - value);
            return key;
        }

        public int SubtractValueSeries(double value, int Series1)
        {
            this.check_series(Series1, "Series1");
            DataSeries series = this.priceSeries[Series1];
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, value - series);
            return key;
        }

        public double Sum(int bar, int series, int period)
        {
            return WealthLab.Indicators.Sum.Value(bar, this.priceSeries[series], period);
        }

        public int SumSeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.Sum.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public string TimeToStr(int time)
        {
            return this.WLTimeToTimeSpan(time).ToString();
        }

        public string Trim(string str)
        {
            return str.Trim(new char[] { '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', ' ' });
        }

        public string TrimLeft(string str)
        {
            return str.TrimStart(new char[0]);
        }

        public string TrimRight(string str)
        {
            return str.TrimEnd(new char[0]);
        }

        public double TRIX(int bar, int series, int period)
        {
            return WealthLab.Indicators.TRIX.Series(this.priceSeries[series], period)[bar];
        }

        public int TRIXSeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.TRIX.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public double Trough(int bar, int series, double reversal)
        {
            return WealthLab.Indicators.Trough.Value(bar, this.priceSeries[series], reversal, this.peakThroughMode);
        }

        public int TroughBar(int bar, int series, double reversal)
        {
            return (int) Math.Round(WealthLab.Indicators.TroughBar.Value(bar, this.priceSeries[series], reversal, this.peakThroughMode));
        }

        public int TroughBarSeries(int series, double reversal)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.TroughBar.Series(this.priceSeries[series], reversal, this.peakThroughMode));
            return this.priceSeriesKey++;
        }

        public int TroughSeries(int series, double reversal)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.Trough.Series(this.priceSeries[series], reversal, this.peakThroughMode));
            return this.priceSeriesKey++;
        }

        public double TrueRange(int bar)
        {
            return WealthLab.Indicators.TrueRange.Value(bar, base.Bars);
        }

        public int TrueRangeSeries()
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.TrueRange.Series(base.Bars));
            return this.priceSeriesKey++;
        }

        public bool TurnDown(int bar, int Series)
        {
            DataSeries series = this.priceSeries[Series];
            return base.TurnDown(bar, series);
        }

        public bool TurnUp(int bar, int Series)
        {
            DataSeries series = this.priceSeries[Series];
            return base.TurnUp(bar, series);
        }

        public double UltimateOsc(int bar)
        {
            return WealthLab.Indicators.UltimateOsc.Series(base.Bars)[bar];
        }

        public int UltimateOscSeries()
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.UltimateOsc.Series(base.Bars));
            return this.priceSeriesKey++;
        }

        public string UpperCase(string value)
        {
            return value.ToUpper(CultureInfo.InvariantCulture);
        }

        public void UseUpdatedEMA(bool Use)
        {
            if (Use)
            {
                this.emaCalculation = EMACalculation.Legacy;
            }
            else
            {
                this.emaCalculation = EMACalculation.Modern;
            }
        }

        public double VHF(int bar, int series, int period)
        {
            return WealthLab.Indicators.VHF.Series(this.priceSeries[series], period)[bar];
        }

        public int VHFSeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.VHF.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public double Vidya(int bar, int series, int volaIndex, double alpha)
        {
            return WealthLab.Indicators.Vidya.Series(this.priceSeries[series], volaIndex, alpha)[bar];
        }

        public int VidyaSeries(int Series, int volaIndex, double alpha)
        {
            int stdDevPeriod = 30;
            base.PrintDebug("VidyaSeries (WL4) is incompatible with Vidya.Series (WL5)");
            this.check_series(Series, "Series");
            this.check_series(volaIndex, "VolaIndex");
            DataSeries series = WealthLab.Indicators.Vidya.Series(this.priceSeries[Series], stdDevPeriod, alpha);
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, series);
            return key;
        }

        public double VMA(int bar, int series, int period)
        {
            return WealthLab.Indicators.VMA.Series(this.priceSeries[series], period)[bar];
        }

        public int VMASeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.VMA.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public double Volatility(int bar, int period1, int period2)
        {
            return WealthLab.Indicators.Volatility.Series(base.Bars, period1, period2)[bar];
        }

        public int VolatilitySeries(int period1, int period2)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.Volatility.Series(base.Bars, period1, period2));
            return this.priceSeriesKey++;
        }

        public double Volume(int Bar)
        {
            return base.Bars.Volume[Bar];
        }

        public string WatchListSymbol(int n)
        {
            return base.DataSetSymbols[n];
        }

        private double[] weightsFromFilter(string filter)
        {
            string[] strArray = filter.Split(new char[] { ',' });
            double[] numArray = new double[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                numArray[i] = double.Parse(strArray[i], CultureInfo.InvariantCulture);
            }
            return numArray;
        }

        public double WilderMA(int bar, int series, int period)
        {
            return WealthLab.Indicators.WilderMA.Series(this.priceSeries[series], period)[bar];
        }

        public int WilderMASeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.WilderMA.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public double WilliamsR(int bar, int period)
        {
            return WealthLab.Indicators.WilliamsR.Series(base.Bars, period)[bar];
        }

        public int WilliamsRSeries(int period)
        {
            int key = this.priceSeriesKey++;
            this.priceSeries.Add(key, WealthLab.Indicators.WilliamsR.Series(base.Bars, period));
            return key;
        }

        public System.Drawing.Color WLColor2Color(int WLColor)
        {
            if (WLColor == -1)
            {
                return System.Drawing.Color.Empty;
            }
            double num = 28.333333333333332;
            int red = Convert.ToInt32((double) (((WLColor % 0x3e8) / 100) * num));
            int green = Convert.ToInt32((double) (((WLColor % 100) / 10) * num));
            int blue = Convert.ToInt32((double) ((WLColor % 10) * num));
            return System.Drawing.Color.FromArgb(red, green, blue);
        }

        private DateTime WLDateToDateTime(int wldate)
        {
            int year = wldate / 0x2710;
            int month = (wldate / 100) % 100;
            return new DateTime(year, month, wldate % 100);
        }

        private LineStyle WLStyle2LineStyle(int Style)
        {
            switch (Style)
            {
                case 3:
                    return LineStyle.Dashed;

                case 4:
                case 5:
                    return LineStyle.Histogram;

                case 6:
                    return LineStyle.Dotted;
            }
            return LineStyle.Solid;
        }

        private int WLStyle2Width(int Style)
        {
            int num = 1;
            if (Style == 2)
            {
                num = 2;
            }
            if (Style == 5)
            {
                num = 5;
            }
            return num;
        }

        private TimeSpan WLTimeToTimeSpan(int time)
        {
            return new TimeSpan(time / 100, time % 100, 0);
        }

        public double WMA(int bar, int series, int period)
        {
            return WealthLab.Indicators.WMA.Value(bar, this.priceSeries[series], period);
        }

        public int WMASeries(int series, int period)
        {
            this.priceSeries.Add(this.priceSeriesKey, WealthLab.Indicators.WMA.Series(this.priceSeries[series], period));
            return this.priceSeriesKey++;
        }

        public int LastActivePosition
        {
            get
            {
                Position lastActivePosition = base.LastActivePosition;
                return base.Positions.IndexOf(lastActivePosition);
            }
        }

        public bool LastLongPositionActive
        {
            get
            {
                for (int i = base.Positions.Count - 1; i >= 0; i--)
                {
                    if (base.Positions[i].PositionType != PositionType.Short)
                    {
                        return base.Positions[i].Active;
                    }
                }
                return false;
            }
        }

        public int LastPosition
        {
            get
            {
                Position lastPosition = base.LastPosition;
                return base.Positions.IndexOf(lastPosition);
            }
        }

        public bool LastShortPositionActive
        {
            get
            {
                for (int i = base.Positions.Count - 1; i >= 0; i--)
                {
                    if ((base.Positions[i].PositionType != PositionType.Long) && base.Positions[i].Active)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public int MarketPosition
        {
            get
            {
                double marketPosition = base.MarketPosition;
                if (marketPosition < 0.0)
                {
                    return -1;
                }
                if (marketPosition > 0.0)
                {
                    return 1;
                }
                return 0;
            }
        }

        public int PositionCount
        {
            get
            {
                return base.Positions.Count;
            }
        }

        public double Randomm
        {
            get
            {
                if (this.randomGen == null)
                {
                    this.randomSeed = new Random().Next();
                    this.randomGen = new Random(this.randomSeed);
                }
                return this.randomGen.NextDouble();
            }
        }

        public double RandSeed
        {
            get
            {
                return (double) this.randomSeed;
            }
        }

        public int WL_AVERAGE
        {
            get
            {
                if (!this.priceSeries.ContainsKey(6))
                {
                    this.priceSeries.Add(6, AveragePrice.Series(base.Bars));
                }
                return 6;
            }
        }

        public int WL_AVERAGEC
        {
            get
            {
                if (!this.priceSeries.ContainsKey(7))
                {
                    this.priceSeries.Add(7, AveragePriceC.Series(base.Bars));
                }
                return 7;
            }
        }

        public int WL_CLOSE
        {
            get
            {
                if (!this.priceSeries.ContainsKey(3))
                {
                    this.priceSeries.Add(3, base.Bars.Close);
                }
                return 3;
            }
        }

        public int WL_HIGH
        {
            get
            {
                if (!this.priceSeries.ContainsKey(1))
                {
                    this.priceSeries.Add(1, base.Bars.High);
                }
                return 1;
            }
        }

        public int WL_LOW
        {
            get
            {
                if (!this.priceSeries.ContainsKey(2))
                {
                    this.priceSeries.Add(2, base.Bars.Low);
                }
                return 2;
            }
        }

        public int WL_OPEN
        {
            get
            {
                if (!this.priceSeries.ContainsKey(0))
                {
                    this.priceSeries.Add(0, base.Bars.Open);
                }
                return 0;
            }
        }

        public int WL_OPENINTEREST
        {
            get
            {
                if (!this.priceSeries.ContainsKey(5))
                {
                    DataSeries series = base.Bars.FindNamedSeries("OpenInterest");
                    if (series == null)
                    {
                        series = base.Bars.Close - base.Bars.Close;
                    }
                    this.priceSeries.Add(5, series);
                }
                return 5;
            }
        }

        public int WL_OPTVAR1
        {
            get
            {
                return Convert.ToInt32(this.GetOptVar(1));
            }
        }

        public int WL_OPTVAR10
        {
            get
            {
                return Convert.ToInt32(this.GetOptVar(10));
            }
        }

        public int WL_OPTVAR2
        {
            get
            {
                return Convert.ToInt32(this.GetOptVar(2));
            }
        }

        public int WL_OPTVAR3
        {
            get
            {
                return Convert.ToInt32(this.GetOptVar(3));
            }
        }

        public int WL_OPTVAR4
        {
            get
            {
                return Convert.ToInt32(this.GetOptVar(4));
            }
        }

        public int WL_OPTVAR5
        {
            get
            {
                return Convert.ToInt32(this.GetOptVar(5));
            }
        }

        public int WL_OPTVAR6
        {
            get
            {
                return Convert.ToInt32(this.GetOptVar(6));
            }
        }

        public int WL_OPTVAR7
        {
            get
            {
                return Convert.ToInt32(this.GetOptVar(7));
            }
        }

        public int WL_OPTVAR8
        {
            get
            {
                return Convert.ToInt32(this.GetOptVar(8));
            }
        }

        public int WL_OPTVAR9
        {
            get
            {
                return Convert.ToInt32(this.GetOptVar(9));
            }
        }

        public int WL_VOLUME
        {
            get
            {
                if (!this.priceSeries.ContainsKey(4))
                {
                    this.priceSeries.Add(4, base.Bars.Volume);
                }
                return 4;
            }
        }

        public enum wl_serie
        {
            open,
            high,
            low,
            close,
            volume,
            openInterest,
            average,
            averageC,
            free
        }
    }
}

