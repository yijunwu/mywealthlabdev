namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Forms;
    using WealthLab.WSDrawingObjects;

    public abstract class WealthScript
    {
        private WealthLab.Bars bars_0;
        private WealthLab.Bars bars_1;
        private ChartPane chartPane_0;
        private ChartRenderer chartRenderer;
        private DataSource dataSource_0;
        private static Dictionary<string, object> dictionary_0 = new Dictionary<string, object>();
        [CompilerGenerated]
        private int strategyWindowID;
        private List<StrategyParameter> parameters = new List<StrategyParameter>();
        private TradingSystemExecutor tradingSystemExecutor;

        protected WealthScript()
        {
        }

        public void Abort()
        {
            throw new AbortException();
        }

        public int AddCalendarDays(bool interpolate)
        {
            int num = this.bars_0.method_7(interpolate);
            if (this.chartRenderer != null)
            {
                this.chartRenderer.method_10();
                this.chartRenderer.AdjustBarPositions();
            }
            return num;
        }

        public void AnnotateBar(string text, int int_1, bool aboveBar, Color color)
        {
            this.AnnotateBar(text, int_1, aboveBar, color, Color.Empty, null);
        }

        public void AnnotateBar(string text, int int_1, bool aboveBar, Color color, Color backgroundColor)
        {
            this.AnnotateBar(text, int_1, aboveBar, color, backgroundColor, null);
        }

        public void AnnotateBar(string text, int int_1, bool aboveBar, Color color, Color backgroundColor, Font font)
        {
            if (this.chartRenderer != null)
            {
                WSDBarAnnotation annotation = new WSDBarAnnotation(text, int_1, aboveBar, color, backgroundColor, font);
                this.chartRenderer.PlotWealthScriptObject(this.chartRenderer.PricePane, annotation, false, false);
            }
        }

        public void AnnotateChart(ChartPane pane, string text, int int_1, double value, Color color)
        {
            this.AnnotateChart(pane, text, int_1, value, color, Color.Empty, null, HorizontalAlignment.Center);
        }

        public void AnnotateChart(ChartPane pane, string text, int int_1, double value, Color color, Color backgroundColor)
        {
            this.AnnotateChart(pane, text, int_1, value, color, backgroundColor, null, HorizontalAlignment.Center);
        }

        public void AnnotateChart(ChartPane pane, string text, int int_1, double value, Color color, Color backgroundColor, Font font)
        {
            this.AnnotateChart(pane, text, int_1, value, color, backgroundColor, font, HorizontalAlignment.Center);
        }

        public void AnnotateChart(ChartPane pane, string text, int int_1, double value, Color color, Color backgroundColor, Font font, HorizontalAlignment alignment)
        {
            if (this.chartRenderer != null)
            {
                WSDChartAnnotation annotation = new WSDChartAnnotation(text, int_1, value, color, backgroundColor, font, alignment);
                this.chartRenderer.PlotWealthScriptObject(pane, annotation, false, false);
            }
        }

        public Position BuyAtClose(int int_1)
        {
            return this.BuyAtClose(int_1, "");
        }

        public Position BuyAtClose(int int_1, string signalName)
        {
            if (!this.method_7(int_1))
            {
                return null;
            }
            this.method_0(int_1, 0, this.bars_0.Count);
            double num = this.bars_0.Close[int_1 - 1];
            this.method_1(num);
            double shares = this.tradingSystemExecutor.CalcPositionSize(this.bars_0, int_1, num, PositionType.Long, this.RiskStopLevel, true);
            if (int_1 == this.bars_0.Count)
            {
                Alert alert = new Alert(this.Strategy, this.bars_0, this.bars_0.Date[int_1 - 1], TradeType.Buy, OrderType.AtClose, shares, signalName, num, this.RiskStopLevel, this.AutoProfitLevel);
                this.tradingSystemExecutor.method_6(alert);
                return null;
            }
            Position position = new Position(this.bars_0, PositionType.Long, this.tradingSystemExecutor.Strategy.ID.ToString()) {
                BasisPrice = num,
                RiskStopLevel = this.RiskStopLevel,
                AutoProfitLevel = this.AutoProfitLevel,
                EntryPrice = this.bars_0.Close[int_1]
            };
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                position.EntryPrice += this.tradingSystemExecutor.method_14(this.bars_0.Close[int_1], false, this.bars_0);
                position.EntryPrice = this.roundPriceToTickMultiple(position.Bars, position.EntryPrice, true, PositionType.Long, OrderType.AtClose);
                if (position.EntryPrice > this.bars_0.High[int_1])
                {
                    position.EntryPrice = this.bars_0.High[int_1];
                }
            }
            position.EntryBar = int_1;
            position.EntryOrderType = OrderType.AtClose;
            position.EntrySignal = signalName;
            position.Shares = shares;
            position.OverrideShareSize = this.OverrideShareSize;
            this.tradingSystemExecutor.method_5(position);
            return position;
        }

        public Position BuyAtLimit(int int_1, double limitPrice)
        {
            return this.BuyAtLimit(int_1, limitPrice, "");
        }

        public Position BuyAtLimit(int int_1, double limitPrice, string signalName)
        {
            if (!this.method_7(int_1))
            {
                return null;
            }
            this.method_1(limitPrice);
            this.method_0(int_1, 0, this.bars_0.Count);
            limitPrice = this.roundPriceToTickMultiple(this.bars_0, limitPrice, true, PositionType.Long, OrderType.Limit);
            double shares = this.tradingSystemExecutor.CalcPositionSize(this.bars_0, int_1, limitPrice, PositionType.Long, this.RiskStopLevel, true);
            if (int_1 == this.bars_0.Count)
            {
                Alert alert = new Alert(this.Strategy, this.bars_0, this.bars_0.Date[int_1 - 1], TradeType.Buy, OrderType.Limit, shares, signalName, limitPrice, this.RiskStopLevel, this.AutoProfitLevel) {
                    Price = limitPrice
                };
                this.tradingSystemExecutor.method_6(alert);
                return null;
            }
            if (((this.chartRenderer != null) && this.chartRenderer.PlotStops) && (this.bars_0 == this.bars_1))
            {
                this.chartRenderer.method_13(int_1, limitPrice, TradeType.Buy);
            }
            if ((limitPrice - this.tradingSystemExecutor.method_14(limitPrice, true, this.bars_0)) < this.bars_0.Low[int_1])
            {
                return null;
            }
            Position position = new Position(this.bars_0, PositionType.Long, this.tradingSystemExecutor.Strategy.ID.ToString()) {
                BasisPrice = limitPrice,
                RiskStopLevel = this.RiskStopLevel,
                AutoProfitLevel = this.AutoProfitLevel,
                EntryBar = int_1,
                EntryOrderType = OrderType.Limit,
                EntrySignal = signalName,
                Shares = shares
            };
            if (this.bars_0.Open[int_1] < limitPrice)
            {
                limitPrice = this.bars_0.Open[int_1];
            }
            position.EntryPrice = limitPrice;
            position.OverrideShareSize = this.OverrideShareSize;
            this.tradingSystemExecutor.method_5(position);
            return position;
        }

        public Position BuyAtMarket(int int_1)
        {
            return this.BuyAtMarket(int_1, "");
        }

        public Position BuyAtMarket(int int_1, string signalName)
        {
            if (!this.method_7(int_1))
            {
                return null;
            }
            this.method_0(int_1, 1, this.bars_0.Count);
            double num = this.bars_0.Close[int_1 - 1];
            this.method_1(num);
            double shares = this.tradingSystemExecutor.CalcPositionSize(this.bars_0, int_1, num, PositionType.Long, this.RiskStopLevel, true);
            if (int_1 == this.bars_0.Count)
            {
                Alert alert = new Alert(this.Strategy, this.bars_0, this.bars_0.Date[int_1 - 1], TradeType.Buy, OrderType.Market, shares, signalName, num, this.RiskStopLevel, this.AutoProfitLevel);
                this.tradingSystemExecutor.method_6(alert);
                return null;
            }
            Position position = new Position(this.bars_0, PositionType.Long, this.tradingSystemExecutor.Strategy.ID.ToString()) {
                BasisPrice = num,
                RiskStopLevel = this.RiskStopLevel,
                AutoProfitLevel = this.AutoProfitLevel,
                EntryPrice = this.bars_0.Open[int_1]
            };
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                position.EntryPrice += this.tradingSystemExecutor.method_14(this.bars_0.Open[int_1], false, this.bars_0);
                position.EntryPrice = this.roundPriceToTickMultiple(position.Bars, position.EntryPrice, true, PositionType.Long, OrderType.Market);
                if (position.EntryPrice > this.bars_0.High[int_1])
                {
                    position.EntryPrice = this.bars_0.High[int_1];
                }
            }
            position.EntryBar = int_1;
            position.EntrySignal = signalName;
            position.Shares = shares;
            position.OverrideShareSize = this.OverrideShareSize;
            this.tradingSystemExecutor.method_5(position);
            return position;
        }

        public Position BuyAtStop(int int_1, double stopPrice)
        {
            return this.BuyAtStop(int_1, stopPrice, "");
        }

        public Position BuyAtStop(int int_1, double stopPrice, string signalName)
        {
            if (!this.method_7(int_1))
            {
                return null;
            }
            this.method_1(stopPrice);
            this.method_0(int_1, 0, this.bars_0.Count);
            stopPrice = this.roundPriceToTickMultiple(this.bars_0, stopPrice, true, PositionType.Long, OrderType.Stop);
            double shares = this.tradingSystemExecutor.CalcPositionSize(this.bars_0, int_1, stopPrice, PositionType.Long, this.RiskStopLevel, true);
            if (int_1 == this.bars_0.Count)
            {
                Alert alert = new Alert(this.Strategy, this.bars_0, this.bars_0.Date[int_1 - 1], TradeType.Buy, OrderType.Stop, shares, signalName, stopPrice, this.RiskStopLevel, this.AutoProfitLevel) {
                    Price = stopPrice
                };
                this.tradingSystemExecutor.method_6(alert);
                return null;
            }
            if (((this.chartRenderer != null) && this.chartRenderer.PlotStops) && (this.bars_0 == this.bars_1))
            {
                this.chartRenderer.method_13(int_1, stopPrice, TradeType.Buy);
            }
            if (stopPrice > this.bars_0.High[int_1])
            {
                return null;
            }
            Position position = new Position(this.bars_0, PositionType.Long, this.tradingSystemExecutor.Strategy.ID.ToString()) {
                BasisPrice = stopPrice,
                RiskStopLevel = this.RiskStopLevel,
                AutoProfitLevel = this.AutoProfitLevel,
                EntryBar = int_1,
                EntryOrderType = OrderType.Stop,
                EntrySignal = signalName,
                Shares = shares
            };
            if (this.bars_0.Open[int_1] > stopPrice)
            {
                stopPrice = this.bars_0.Open[int_1];
            }
            position.EntryPrice = stopPrice;
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                position.EntryPrice += this.tradingSystemExecutor.method_14(stopPrice, false, this.bars_0);
                position.EntryPrice = this.roundPriceToTickMultiple(position.Bars, position.EntryPrice, true, PositionType.Long, OrderType.Stop);
                if (position.EntryPrice > this.bars_0.High[int_1])
                {
                    position.EntryPrice = this.bars_0.High[int_1];
                }
            }
            position.OverrideShareSize = this.OverrideShareSize;
            this.tradingSystemExecutor.method_5(position);
            return position;
        }

        public void ClearDebug()
        {
            this.tradingSystemExecutor.method_17();
        }

        public int ClearExternalSeries(string symbol)
        {
            return this.tradingSystemExecutor.method_12(symbol);
        }

        public int ClearExternalSymbols()
        {
            return this.tradingSystemExecutor.method_11();
        }

        public void ClearGlobals()
        {
            lock (dictionary_0)
            {
                dictionary_0.Clear();
            }
        }

        public void ClearPositions()
        {
            this.tradingSystemExecutor.MasterPositions.Clear();
            this.tradingSystemExecutor.CurrentPositions.Clear();
            this.tradingSystemExecutor.MasterAlerts.Clear();
            this.tradingSystemExecutor.CurrentAlerts.Clear();
        }

        public bool CoverAtAutoTrailingStop(int int_1, Position position_0, double triggerPct, double profitReversalPct)
        {
            return this.CoverAtAutoTrailingStop(int_1, position_0, triggerPct, profitReversalPct, "AutoTrailing");
        }

        public bool CoverAtAutoTrailingStop(int int_1, Position position_0, double triggerPct, double profitReversalPct, string signalName)
        {
            if (((position_0 != null) && position_0.Active) && (position_0.MFEAsOfBarPercent(int_1 - 1) >= triggerPct))
            {
                double num = position_0.LowestLowAsOfBar(int_1 - 1);
                double num2 = position_0.EntryPrice - num;
                double num3 = (profitReversalPct / 100.0) * num2;
                return this.CoverAtTrailingStop(int_1, position_0, num + num3, signalName);
            }
            return false;
        }

        public bool CoverAtClose(int int_1, Position position_0)
        {
            return this.CoverAtClose(int_1, position_0, "");
        }

        public bool CoverAtClose(int int_1, Position position_0, string signalName)
        {
            if ((position_0 == null) || !position_0.Active)
            {
                return false;
            }
            if (position_0 == null)
            {
                return false;
            }
            if (position_0 == Position.AllPositions)
            {
                bool flag = false;
                int count = this.tradingSystemExecutor.ActivePositions.Count;
                for (int i = this.Positions.Count - 1; i >= 0; i--)
                {
                    if (count == 0)
                    {
                        return flag;
                    }
                    if (this.Positions[i].Active && this.CoverAtClose(int_1, this.Positions[i], signalName))
                    {
                        flag = true;
                        count--;
                    }
                }
                return flag;
            }
            if (!this.method_6(position_0.Bars, int_1))
            {
                return false;
            }
            this.method_0(int_1, position_0.EntryBar, this.bars_0.Count);
            if (position_0.PositionType != PositionType.Short)
            {
                return false;
            }
            if (int_1 == position_0.Bars.Count)
            {
                Alert alert = new Alert(this.Strategy, position_0.Bars, position_0.Bars.Date[int_1 - 1], TradeType.Cover, OrderType.AtClose, position_0.Shares, signalName) {
                    Position = position_0
                };
                this.tradingSystemExecutor.method_6(alert);
                return false;
            }
            double num3 = position_0.Bars.Close[int_1];
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                num3 += this.tradingSystemExecutor.method_14(num3, false, this.bars_0);
                num3 = this.roundPriceToTickMultiple(position_0.Bars, num3, false, PositionType.Short, OrderType.AtClose);
            }
            position_0.method_1(int_1, num3, OrderType.AtClose);
            this.tradingSystemExecutor.ActivePositions.Remove(position_0);
            position_0.ExitSignal = signalName;
            return true;
        }

        public bool CoverAtLimit(int int_1, Position position_0, double limitPrice)
        {
            return this.CoverAtLimit(int_1, position_0, limitPrice, "");
        }

        public bool CoverAtLimit(int int_1, Position position_0, double limitPrice, string signalName)
        {
            if ((position_0 == null) || !position_0.Active)
            {
                return false;
            }
            if (position_0 == null)
            {
                return false;
            }
            limitPrice = this.roundPriceToTickMultiple(position_0.Bars, limitPrice, false, PositionType.Short, OrderType.Limit);
            if (position_0 == Position.AllPositions)
            {
                bool flag = false;
                int count = this.tradingSystemExecutor.ActivePositions.Count;
                for (int i = this.Positions.Count - 1; i >= 0; i--)
                {
                    if (count == 0)
                    {
                        return flag;
                    }
                    if (this.Positions[i].Active && this.CoverAtLimit(int_1, this.Positions[i], limitPrice, signalName))
                    {
                        flag = true;
                        count--;
                    }
                }
                return flag;
            }
            if (!this.method_6(position_0.Bars, int_1))
            {
                return false;
            }
            this.method_0(int_1, position_0.EntryBar, position_0.Bars.Count);
            if (position_0.PositionType != PositionType.Short)
            {
                return false;
            }
            if (int_1 == position_0.Bars.Count)
            {
                Alert alert = new Alert(this.Strategy, position_0.Bars, position_0.Bars.Date[int_1 - 1], TradeType.Cover, OrderType.Limit, position_0.Shares, signalName) {
                    Position = position_0,
                    Price = limitPrice
                };
                this.tradingSystemExecutor.method_6(alert);
                return false;
            }
            if (((this.chartRenderer != null) && this.chartRenderer.PlotStops) && (position_0.Bars == this.bars_1))
            {
                this.chartRenderer.method_13(int_1, limitPrice, TradeType.Cover);
            }
            if ((limitPrice - this.tradingSystemExecutor.method_14(limitPrice, true, position_0.Bars)) < position_0.Bars.Low[int_1])
            {
                return false;
            }
            if (position_0.Bars.Open[int_1] < limitPrice)
            {
                limitPrice = position_0.Bars.Open[int_1];
            }
            position_0.method_1(int_1, limitPrice, OrderType.Limit);
            this.tradingSystemExecutor.ActivePositions.Remove(position_0);
            position_0.ExitSignal = signalName;
            return true;
        }

        public bool CoverAtMarket(int int_1, Position position_0)
        {
            return this.CoverAtMarket(int_1, position_0, "");
        }

        public bool CoverAtMarket(int int_1, Position position_0, string signalName)
        {
            if ((position_0 == null) || !position_0.Active)
            {
                return false;
            }
            if (position_0 == null)
            {
                return false;
            }
            if (position_0 == Position.AllPositions)
            {
                bool flag = false;
                int count = this.tradingSystemExecutor.ActivePositions.Count;
                for (int i = this.Positions.Count - 1; i >= 0; i--)
                {
                    if (count == 0)
                    {
                        return flag;
                    }
                    if (this.Positions[i].Active && this.CoverAtMarket(int_1, this.Positions[i], signalName))
                    {
                        flag = true;
                        count--;
                    }
                }
                return flag;
            }
            if (!this.method_6(position_0.Bars, int_1))
            {
                return false;
            }
            this.method_0(int_1, position_0.EntryBar, this.bars_0.Count);
            if (position_0.PositionType != PositionType.Short)
            {
                return false;
            }
            if (int_1 == this.bars_0.Count)
            {
                Alert alert = new Alert(this.Strategy, position_0.Bars, position_0.Bars.Date[int_1 - 1], TradeType.Cover, OrderType.Market, position_0.Shares, signalName) {
                    Position = position_0
                };
                this.tradingSystemExecutor.method_6(alert);
                return false;
            }
            double num3 = position_0.Bars.Open[int_1];
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                num3 += this.tradingSystemExecutor.method_14(num3, false, position_0.Bars);
                num3 = this.roundPriceToTickMultiple(position_0.Bars, num3, false, PositionType.Short, OrderType.Market);
            }
            position_0.method_1(int_1, num3, OrderType.Market);
            this.tradingSystemExecutor.ActivePositions.Remove(position_0);
            position_0.ExitSignal = signalName;
            return true;
        }

        public bool CoverAtStop(int int_1, Position position_0, double stopPrice)
        {
            return this.CoverAtStop(int_1, position_0, stopPrice, "");
        }

        public bool CoverAtStop(int int_1, Position position_0, double stopPrice, string signalName)
        {
            if ((position_0 == null) || !position_0.Active)
            {
                return false;
            }
            if (position_0 == null)
            {
                return false;
            }
            stopPrice = this.roundPriceToTickMultiple(position_0.Bars, stopPrice, false, PositionType.Short, OrderType.Stop);
            if (position_0 == Position.AllPositions)
            {
                bool flag = false;
                int count = this.tradingSystemExecutor.ActivePositions.Count;
                for (int i = this.Positions.Count - 1; i >= 0; i--)
                {
                    if (count == 0)
                    {
                        return flag;
                    }
                    if (this.Positions[i].Active && this.CoverAtStop(int_1, this.Positions[i], stopPrice, signalName))
                    {
                        flag = true;
                        count--;
                    }
                }
                return flag;
            }
            if (!this.method_6(position_0.Bars, int_1))
            {
                return false;
            }
            this.method_0(int_1, position_0.EntryBar, position_0.Bars.Count);
            if (position_0.PositionType != PositionType.Short)
            {
                return false;
            }
            if (int_1 == position_0.Bars.Count)
            {
                Alert alert = new Alert(this.Strategy, position_0.Bars, position_0.Bars.Date[int_1 - 1], TradeType.Cover, OrderType.Stop, position_0.Shares, signalName) {
                    Position = position_0,
                    Price = stopPrice
                };
                this.tradingSystemExecutor.method_6(alert);
                return false;
            }
            if (((this.chartRenderer != null) && this.chartRenderer.PlotStops) && (position_0.Bars == this.bars_1))
            {
                this.chartRenderer.method_13(int_1, stopPrice, TradeType.Cover);
            }
            if (stopPrice > position_0.Bars.High[int_1])
            {
                return false;
            }
            if (position_0.Bars.Open[int_1] > stopPrice)
            {
                stopPrice = position_0.Bars.Open[int_1];
            }
            double num3 = stopPrice;
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                num3 += this.tradingSystemExecutor.method_14(num3, false, position_0.Bars);
                num3 = this.roundPriceToTickMultiple(position_0.Bars, num3, false, PositionType.Short, OrderType.Stop);
            }
            position_0.method_1(int_1, num3, OrderType.Stop);
            this.tradingSystemExecutor.ActivePositions.Remove(position_0);
            position_0.ExitSignal = signalName;
            return true;
        }

        public bool CoverAtTrailingStop(int int_1, Position position_0, double stopPrice)
        {
            return this.CoverAtTrailingStop(int_1, position_0, stopPrice, "");
        }

        public bool CoverAtTrailingStop(int int_1, Position position_0, double stopPrice, string signalName)
        {
            if ((position_0 == null) || !position_0.Active)
            {
                return false;
            }
            if (position_0 == null)
            {
                return false;
            }
            if (position_0 == Position.AllPositions)
            {
                bool flag = false;
                int count = this.tradingSystemExecutor.ActivePositions.Count;
                for (int i = this.Positions.Count - 1; i >= 0; i--)
                {
                    if (count == 0)
                    {
                        return flag;
                    }
                    if (this.Positions[i].Active && this.CoverAtTrailingStop(int_1, this.Positions[i], stopPrice, signalName))
                    {
                        flag = true;
                        count--;
                    }
                }
                return flag;
            }
            if (!this.method_6(position_0.Bars, int_1))
            {
                return false;
            }
            stopPrice = this.roundPriceToTickMultiple(position_0.Bars, stopPrice, false, PositionType.Short, OrderType.Stop);
            if ((stopPrice < position_0.TrailingStop) || (position_0.TrailingStop == 0.0))
            {
                position_0.TrailingStop = stopPrice;
            }
            stopPrice = position_0.TrailingStop;
            this.method_0(int_1, position_0.EntryBar, position_0.Bars.Count);
            if (position_0.PositionType != PositionType.Short)
            {
                return false;
            }
            if (int_1 == position_0.Bars.Count)
            {
                Alert alert = new Alert(this.Strategy, position_0.Bars, position_0.Bars.Date[int_1 - 1], TradeType.Cover, OrderType.Stop, position_0.Shares, signalName) {
                    Position = position_0,
                    Price = stopPrice
                };
                this.tradingSystemExecutor.method_6(alert);
                return false;
            }
            if (((this.chartRenderer != null) && this.chartRenderer.PlotStops) && (position_0.Bars == this.bars_1))
            {
                this.chartRenderer.method_13(int_1, stopPrice, TradeType.Cover);
            }
            if (stopPrice > position_0.Bars.High[int_1])
            {
                return false;
            }
            if (position_0.Bars.Open[int_1] > stopPrice)
            {
                stopPrice = position_0.Bars.Open[int_1];
            }
            double num3 = stopPrice;
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                num3 += this.tradingSystemExecutor.method_14(stopPrice, false, position_0.Bars);
                num3 = this.roundPriceToTickMultiple(position_0.Bars, num3, false, PositionType.Short, OrderType.Stop);
            }
            position_0.method_1(int_1, num3, OrderType.Stop);
            this.tradingSystemExecutor.ActivePositions.Remove(position_0);
            position_0.ExitSignal = signalName;
            return true;
        }

        public ChartPane CreatePane(int height, bool abovePricePane, bool displayGrid)
        {
            if (this.chartRenderer != null)
            {
                return new ChartPane(this.chartRenderer, abovePricePane, height, true) { RawHeight = height, Visible = true, DisplayGrid = displayGrid, Decimals = 2 };
            }
            return new ChartPane();
        }

        protected StrategyParameter CreateParameter(string name, double value, double start, double stop, double step)
        {
            StrategyParameter item = new StrategyParameter(name, value, start, stop, step);
            this.parameters.Add(item);
            return item;
        }

        public bool CrossOver(int int_1, DataSeries dataSeries_0, double value)
        {
            this.method_0(int_1, 1, this.bars_0.Count - 1);
            return ((dataSeries_0[int_1] > value) && (dataSeries_0[int_1 - 1] <= value));
        }

        public bool CrossOver(int int_1, DataSeries dataSeries_0, DataSeries dataSeries_1)
        {
            this.method_0(int_1, 1, this.bars_0.Count - 1);
            return ((dataSeries_0[int_1] > dataSeries_1[int_1]) && (dataSeries_0[int_1 - 1] <= dataSeries_1[int_1 - 1]));
        }

        public bool CrossUnder(int int_1, DataSeries dataSeries_0, double value)
        {
            this.method_0(int_1, 1, this.bars_0.Count - 1);
            return ((dataSeries_0[int_1] < value) && (dataSeries_0[int_1 - 1] >= value));
        }

        public bool CrossUnder(int int_1, DataSeries dataSeries_0, DataSeries dataSeries_1)
        {
            this.method_0(int_1, 1, this.bars_0.Count - 1);
            return ((dataSeries_0[int_1] < dataSeries_1[int_1]) && (dataSeries_0[int_1 - 1] >= dataSeries_1[int_1 - 1]));
        }

        public int DateTimeToBar(DateTime date, bool exactMatch)
        {
            return this.bars_0.ConvertDateToBar(date, exactMatch);
        }

        public void DrawCircle(ChartPane pane, int radius, int int_1, double value, Color color, LineStyle style, int width, bool behindBars)
        {
            this.DrawCircle(pane, radius, int_1, value, color, Color.Empty, style, width, behindBars);
        }

        public void DrawCircle(ChartPane pane, int bar1, double value1, int bar2, double value2, Color color, LineStyle style, int width, bool behindBars)
        {
            this.DrawCircle(pane, bar1, value1, bar2, value2, color, Color.Empty, style, width, behindBars);
        }

        public void DrawCircle(ChartPane pane, int radius, int int_1, double value, Color color, Color fillColor, LineStyle style, int width, bool behindBars)
        {
            if (this.chartRenderer != null)
            {
                this.method_10(int_1);
                WSDCircle circle = new WSDCircle(radius, int_1, value, color, fillColor, style, width);
                this.chartRenderer.PlotWealthScriptObject(pane, circle, false, behindBars);
            }
        }

        public void DrawCircle(ChartPane pane, int bar1, double value1, int bar2, double value2, Color color, Color fillColor, LineStyle style, int width, bool behindBars)
        {
            if (this.chartRenderer != null)
            {
                this.method_10(bar1);
                this.method_10(bar2);
                WSDCircle2 circle = new WSDCircle2(bar1, value1, bar2, value2, color, fillColor, style, width);
                this.chartRenderer.PlotWealthScriptObject(pane, circle, false, behindBars);
            }
        }

        public void DrawEllipse(ChartPane pane, int bar1, double value1, int bar2, double value2, Color color, LineStyle style, int width, bool behindBars)
        {
            this.DrawEllipse(pane, bar1, value1, bar2, value2, color, Color.Empty, style, width, behindBars);
        }

        public void DrawEllipse(ChartPane pane, int bar1, double value1, int bar2, double value2, Color color, Color fillColor, LineStyle style, int width, bool behindBars)
        {
            if (this.chartRenderer != null)
            {
                this.method_10(bar1);
                this.method_10(bar2);
                WSDEllipse ellipse = new WSDEllipse(bar1, value1, bar2, value2, color, fillColor, style, width);
                this.chartRenderer.PlotWealthScriptObject(pane, ellipse, false, behindBars);
            }
        }

        public void DrawHorzLine(ChartPane pane, double value, Color color, LineStyle style, int width)
        {
            if (this.chartRenderer != null)
            {
                WSDHorzLine line = new WSDHorzLine(value, color, style, width);
                this.chartRenderer.PlotWealthScriptObject(pane, line, false, false);
            }
        }

        public void DrawImage(ChartPane pane, Image image, int int_1, double value, bool behindBars)
        {
            if (this.chartRenderer != null)
            {
                this.method_10(int_1);
                WSDImage image2 = new WSDImage(image, int_1, value);
                this.chartRenderer.PlotWealthScriptObject(pane, image2, false, behindBars);
            }
        }

        public void DrawLabel(ChartPane pane, string label)
        {
            this.DrawLabel(pane, label, Color.Black);
        }

        public void DrawLabel(ChartPane pane, string label, Color color)
        {
            if (this.chartRenderer != null)
            {
                WSDLabel label2 = new WSDLabel(label, color);
                this.chartRenderer.PlotWealthScriptObject(pane, label2, false, false);
            }
        }

        public void DrawLine(ChartPane pane, int bar1, double value1, int bar2, double value2, Color color, LineStyle style, int width)
        {
            if (this.chartRenderer != null)
            {
                this.method_10(bar1);
                this.method_10(bar2);
                WSDLine line = new WSDLine(bar1, value1, bar2, value2, color, style, width);
                this.chartRenderer.PlotWealthScriptObject(pane, line, false, false);
            }
        }

        public void DrawPolygon(ChartPane pane, Color color, LineStyle style, int width, bool behindBars, params double[] coords)
        {
            this.DrawPolygon(pane, color, Color.Empty, style, width, behindBars, coords);
        }

        public void DrawPolygon(ChartPane pane, Color color, Color fillColor, LineStyle style, int width, bool behindBars, params double[] coords)
        {
            if ((coords.Length % 2) == 1)
            {
                throw new ArgumentException("DrawPolygon must have an even number of values in coords parameter");
            }
            if (this.chartRenderer != null)
            {
                for (int i = 0; i < coords.Length; i += 2)
                {
                    this.method_10((int) coords[i]);
                }
                WSDPolygon polygon = new WSDPolygon(color, fillColor, style, width, coords);
                this.chartRenderer.PlotWealthScriptObject(pane, polygon, false, behindBars);
            }
        }

        public void DrawText(ChartPane pane, string text, int int_1, int int_2, Color color)
        {
            this.DrawText(pane, text, int_1, int_2, color, Color.Empty, null);
        }

        public void DrawText(ChartPane pane, string text, int int_1, int int_2, Color color, Color backgroundColor)
        {
            this.DrawText(pane, text, int_1, int_2, color, backgroundColor, null);
        }

        public void DrawText(ChartPane pane, string text, int int_1, int int_2, Color color, Color backgroundColor, Font font)
        {
            if (this.chartRenderer != null)
            {
                WSDText text2 = new WSDText(text, int_1, int_2, color, backgroundColor, font);
                this.chartRenderer.PlotWealthScriptObject(pane, text2, false, false);
            }
        }

        public void EnableTradeNotes(bool Text, bool Arrow, bool Circle)
        {
            if (this.chartRenderer != null)
            {
                this.chartRenderer.TradeAnnotationsVisible = Text;
                this.chartRenderer.TradeArrowsVisible = Arrow;
                this.chartRenderer.TradeCirclesVisible = Circle;
            }
        }

        protected abstract void Execute();
        public bool ExitAtAutoTrailingStop(int int_1, Position position_0, double triggerPct, double profitReversalPct)
        {
            return this.ExitAtAutoTrailingStop(int_1, position_0, triggerPct, profitReversalPct, "");
        }

        public bool ExitAtAutoTrailingStop(int int_1, Position position_0, double triggerPct, double profitReversalPct, string signalName)
        {
            if (position_0 == Position.AllPositions)
            {
                bool flag = this.SellAtAutoTrailingStop(int_1, position_0, triggerPct, profitReversalPct, signalName);
                bool flag2 = this.CoverAtAutoTrailingStop(int_1, position_0, triggerPct, profitReversalPct, signalName);
                if (!flag)
                {
                    return flag2;
                }
                return true;
            }
            if (position_0.PositionType == PositionType.Long)
            {
                return this.SellAtAutoTrailingStop(int_1, position_0, triggerPct, profitReversalPct);
            }
            return this.CoverAtAutoTrailingStop(int_1, position_0, triggerPct, profitReversalPct);
        }

        public bool ExitAtClose(int int_1, Position position_0)
        {
            return this.ExitAtClose(int_1, position_0, "");
        }

        public bool ExitAtClose(int int_1, Position position_0, string signalName)
        {
            if (position_0 == Position.AllPositions)
            {
                bool flag = this.SellAtClose(int_1, position_0, signalName);
                bool flag2 = this.CoverAtClose(int_1, position_0, signalName);
                if (!flag)
                {
                    return flag2;
                }
                return true;
            }
            if (position_0.PositionType == PositionType.Long)
            {
                return this.SellAtClose(int_1, position_0, signalName);
            }
            return this.CoverAtClose(int_1, position_0, signalName);
        }

        public bool ExitAtLimit(int int_1, Position position_0, double price)
        {
            return this.ExitAtLimit(int_1, position_0, price, "");
        }

        public bool ExitAtLimit(int int_1, Position position_0, double price, string signalName)
        {
            if (position_0 == Position.AllPositions)
            {
                bool flag = this.SellAtLimit(int_1, position_0, price, signalName);
                bool flag2 = this.CoverAtLimit(int_1, position_0, price, signalName);
                if (!flag)
                {
                    return flag2;
                }
                return true;
            }
            if (position_0.PositionType == PositionType.Long)
            {
                return this.SellAtLimit(int_1, position_0, price, signalName);
            }
            return this.CoverAtLimit(int_1, position_0, price, signalName);
        }

        public bool ExitAtMarket(int int_1, Position position_0)
        {
            return this.ExitAtMarket(int_1, position_0, "");
        }

        public bool ExitAtMarket(int int_1, Position position_0, string signalName)
        {
            if (position_0 == Position.AllPositions)
            {
                bool flag = this.SellAtMarket(int_1, position_0, signalName);
                bool flag2 = this.CoverAtMarket(int_1, position_0, signalName);
                if (!flag)
                {
                    return flag2;
                }
                return true;
            }
            if (position_0.PositionType == PositionType.Long)
            {
                return this.SellAtMarket(int_1, position_0, signalName);
            }
            return this.CoverAtMarket(int_1, position_0, signalName);
        }

        public bool ExitAtStop(int int_1, Position position_0, double price)
        {
            return this.ExitAtStop(int_1, position_0, price, "");
        }

        public bool ExitAtStop(int int_1, Position position_0, double price, string signalName)
        {
            if (position_0 == Position.AllPositions)
            {
                bool flag = this.SellAtStop(int_1, position_0, price, signalName);
                bool flag2 = this.CoverAtStop(int_1, position_0, price, signalName);
                if (!flag)
                {
                    return flag2;
                }
                return true;
            }
            if (position_0.PositionType == PositionType.Long)
            {
                return this.SellAtStop(int_1, position_0, price, signalName);
            }
            return this.CoverAtStop(int_1, position_0, price, signalName);
        }

        public bool ExitAtTrailingStop(int int_1, Position position_0, double stopPrice)
        {
            return this.ExitAtTrailingStop(int_1, position_0, stopPrice, "");
        }

        public bool ExitAtTrailingStop(int int_1, Position position_0, double stopPrice, string signalName)
        {
            if (position_0 == Position.AllPositions)
            {
                bool flag = this.SellAtTrailingStop(int_1, position_0, stopPrice, signalName);
                bool flag2 = this.CoverAtTrailingStop(int_1, position_0, stopPrice, signalName);
                if (!flag)
                {
                    return flag2;
                }
                return true;
            }
            if (position_0.PositionType == PositionType.Long)
            {
                return this.SellAtTrailingStop(int_1, position_0, stopPrice, signalName);
            }
            return this.CoverAtTrailingStop(int_1, position_0, stopPrice, signalName);
        }

        public StrategyParameter FindParameter(string name)
        {
            StrategyParameter parameter2;
            using (IEnumerator<StrategyParameter> enumerator = this.Parameters.GetEnumerator())
            {
                StrategyParameter current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Name.ToUpper() == name.ToUpper())
                    {
                        ///goto  Label_0038; ///WYJ fix, simplify the flow
                        parameter2 = current;
                        return parameter2;
                    }
                }
                return null;
            }
        }

        public void FlushDebug()
        {
            this.tradingSystemExecutor.method_16();
        }

        public IList<FundamentalItem> FundamentalDataItems(string itemName)
        {
            if (this.tradingSystemExecutor.FundamentalsLoader == null)
            {
                return null;
            }
            return this.tradingSystemExecutor.FundamentalsLoader.RequestNonSymbolItems(this.bars_0, itemName);
        }

        public IList<FundamentalItem> FundamentalDataItems(string symbol, string itemName)
        {
            if (this.tradingSystemExecutor.FundamentalsLoader == null)
            {
                return null;
            }
            return this.tradingSystemExecutor.FundamentalsLoader.RequestSymbolItems(this.bars_0, symbol, itemName);
        }

        public DataSeries FundamentalDataSeries(string itemName)
        {
            if (this.tradingSystemExecutor == null)
            {
                return null;
            }
            return this.tradingSystemExecutor.FundamentalsLoader.RequestNonSymbolDataSeries(this.bars_0, itemName);
        }

        public DataSeries FundamentalDataSeries(string itemName, int offset)
        {
            if (this.tradingSystemExecutor == null)
            {
                return null;
            }
            return this.tradingSystemExecutor.FundamentalsLoader.RequestDataSeries(this.bars_0, itemName, offset, 0, false);
        }

        public DataSeries FundamentalDataSeries(string symbol, string itemName)
        {
            if (this.tradingSystemExecutor == null)
            {
                return null;
            }
            return this.tradingSystemExecutor.FundamentalsLoader.RequestSymbolDataSeries(this.bars_0, symbol, itemName);
        }

        public DataSeries FundamentalDataSeries(string itemName, int aggregate, int offset)
        {
            if (this.tradingSystemExecutor == null)
            {
                return null;
            }
            return this.tradingSystemExecutor.FundamentalsLoader.RequestDataSeries(this.bars_0, itemName, offset, aggregate, false);
        }

        public DataSeries FundamentalDataSeries(string itemName, int aggregate, bool average, int offset)
        {
            if (this.tradingSystemExecutor == null)
            {
                return null;
            }
            return this.tradingSystemExecutor.FundamentalsLoader.RequestDataSeries(this.bars_0, itemName, offset, aggregate, average);
        }

        public DataSeries FundamentalDataSeries(string symbol, string itemName, int aggregate, bool average, int offset)
        {
            if (this.tradingSystemExecutor == null)
            {
                return null;
            }
            return this.tradingSystemExecutor.FundamentalsLoader.RequestDataSeries(this.bars_0, itemName, offset, aggregate, average);
        }

        public DataSeries FundamentalDataSeriesAnnual(string itemName, int offset)
        {
            if (this.tradingSystemExecutor == null)
            {
                return null;
            }
            return this.tradingSystemExecutor.FundamentalsLoader.RequestDataSeriesAnnual(this.bars_0, itemName, offset);
        }

        public DataSeries FundamentalDataSeriesAnnual(string symbol, string itemName, int offset)
        {
            if (this.tradingSystemExecutor == null)
            {
                return null;
            }
            return this.tradingSystemExecutor.FundamentalsLoader.RequestDataSeriesAnnual(this.bars_0, itemName, offset);
        }

        public Bitmap GetChartBitmap(int width, int height)
        {
            return this.tradingSystemExecutor.method_19(width, height);
        }

        public DataSeries GetExternalSeries(string symbol, DataSeries dataSeries_0)
        {
            WealthLab.Bars externalSymbol = this.GetExternalSymbol(symbol, false);
            if (dataSeries_0.Description != "Close")
            {
                if (dataSeries_0.Description == "Open")
                {
                    return externalSymbol.Open;
                }
                if (dataSeries_0.Description == "High")
                {
                    return externalSymbol.High;
                }
                if (dataSeries_0.Description == "Low")
                {
                    return externalSymbol.Low;
                }
                if (dataSeries_0.Description == "Volume")
                {
                    return externalSymbol.Low;
                }
            }
            return externalSymbol.Close;
        }

        public WealthLab.Bars GetExternalSymbol(string symbol, bool synchronize)
        {
            WealthLab.Bars bars = this.bars_0;
            this.SetContext(symbol, synchronize);
            WealthLab.Bars bars2 = this.bars_0;
            this.bars_0 = bars;
            return bars2;
        }

        public WealthLab.Bars GetExternalSymbol(string dataSetName, string symbol, bool synchronize)
        {
            return this.tradingSystemExecutor.method_9(dataSetName, symbol, synchronize);
        }

        public FundamentalItem GetFundamentalItem(int int_1, string symbol, string itemName)
        {
            if (this.tradingSystemExecutor.FundamentalsLoader != null)
            {
                IList<FundamentalItem> list = this.tradingSystemExecutor.FundamentalsLoader.RequestSymbolItems(this.bars_0, symbol, itemName);
                int num2 = -1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (this.bars_0.Date[int_1] < list[i].Date)
                    {
                        break;
                    }
                    num2 = i;
                }
                if (num2 >= 0)
                {
                    return list[num2];
                }
            }
            return null;
        }

        public object GetGlobal(string string_0)
        {
            lock (dictionary_0)
            {
                if (dictionary_0.ContainsKey(string_0))
                {
                    return dictionary_0[string_0];
                }
                return null;
            }
        }

        public FundamentalItem GetNextFundamentalItem(int int_1, string symbol, string itemName)
        {
            if (this.tradingSystemExecutor.FundamentalsLoader != null)
            {
                IList<FundamentalItem> list = this.tradingSystemExecutor.FundamentalsLoader.RequestSymbolItems(this.bars_0, symbol, itemName);
                int num2 = -1;
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (this.bars_0.Date[int_1] >= list[i].Date)
                    {
                        break;
                    }
                    num2 = i;
                }
                if (num2 >= 0)
                {
                    return list[num2];
                }
            }
            return null;
        }

        public double GetSessionOpen(string symbol)
        {
            return this.dataSource_0.Provider.GetSessionOpen(symbol, this.dataSource_0);
        }

        public int GetTradingLoopStartBar(int startBar)
        {
            int valueInt = -2147483648;
            foreach (StrategyParameter parameter in this.Parameters)
            {
                if (parameter.Name.ToUpper().Contains("PERIOD") && (parameter.ValueInt > valueInt))
                {
                    valueInt = parameter.ValueInt;
                }
            }
            if (valueInt <= startBar)
            {
                return startBar;
            }
            return valueInt;
        }

        public void HidePaneLines()
        {
            if (this.chartRenderer != null)
            {
                this.chartRenderer.PaneSeparatorVisible = false;
            }
        }

        public void HideVolume()
        {
            if (this.VolumePane != null)
            {
                this.VolumePane.Visible = false;
            }
        }

        public bool IsOptionExpiryDate(int int_1)
        {
            DateTime time = this.bars_0.Date[int_1];
            DateTime date = time.Date;
            DateTime time3 = new DateTime(date.Year, date.Month, 1);
            int year = date.Year;
            int month = date.Month;
            int num3 = 0;
            for (int i = 1; i <= 0x1f; i++)
            {
                time3 = new DateTime(year, month, i);
                if (time3.DayOfWeek == DayOfWeek.Friday)
                {
                    num3++;
                    if (num3 == 3)
                    {
                        break;
                    }
                }
            }
            if (date.Day == time3.Day)
            {
                return true;
            }
            if ((date < time3) && (int_1 < (this.bars_0.Count - 1)))
            {
                DateTime time4 = this.bars_0.Date[int_1 + 1];
                if (time4.Date > time3)
                {
                    return true;
                }
            }
            return false;
        }

        public double LinearRegLine(DataSeries series, int bar1, int bar2, double predict)
        {
            try
            {
                LinearRegression regression = new LinearRegression();
                regression.Init();
                for (int i = bar1; i <= bar2; i++)
                {
                    regression.Add((double) i, series[i]);
                }
                regression.Complete();
                return regression.PredictY(predict);
            }
            catch (Exception)
            {
                return 0.0;
            }
        }

        public double LineExtendX(double double_0, double double_1, double double_2, double double_3, double double_4)
        {
            if (double_0 != double_2)
            {
                double num = (double_3 - double_1) / (double_2 - double_0);
                double num2 = ((num * double_0) - double_1) * -1.0;
                return ((double_4 - num2) / num);
            }
            return double_0;
        }

        public double LineExtendY(double double_0, double double_1, double double_2, double double_3, double double_4)
        {
            double num;
            if (double_0 != double_2)
            {
                num = (double_3 - double_1) / (double_2 - double_0);
            }
            else
            {
                num = 0.0;
            }
            double num2 = ((num * double_0) - double_1) * -1.0;
            return ((num * double_4) + num2);
        }

        public double LineExtendYLog(double double_0, double double_1, double double_2, double double_3, double double_4)
        {
            double num;
            if (double_0 != double_2)
            {
                num = Math.Log10(double_1 / double_3) / (double_0 - double_2);
            }
            else
            {
                num = 0.0;
            }
            return (double_3 * Math.Pow(10.0, num * (double_4 - double_2)));
        }

        private void method_0(int int_1, int int_2, int int_3)
        {
            if (int_1 < int_2)
            {
                throw new ArgumentException("Bar number must be " + int_2 + " or greater");
            }
            if (int_1 > int_3)
            {
                throw new ArgumentException("Bar number must be " + int_3 + " or less");
            }
        }

        private void method_1(double double_0)
        {
            if (double_0 == 0.0)
            {
                throw new ArgumentException("Basis price for Position entry cannot be zero");
            }
        }

        private void method_10(int int_1)
        {
            if ((int_1 < 0) || (int_1 >= this.Bars.Count))
            {
                throw new ArgumentException("Bar number outside bounds of the chart: " + int_1);
            }
        }

        private void method_2(DataSeries dataSeries_0)
        {
            if (dataSeries_0.Count < this.bars_1.Count)
            {
                throw new ArgumentException("Plotted DataSeries has fewer bars than the Symbol being charted");
            }
            if (dataSeries_0.ContainsInfinity)
            {
                throw new ArgumentException("Cannot plot a DataSeries that contains Infinity");
            }
        }

        ///WYJ fix, original name method_3
        private double roundPriceToTickMultiple(WealthLab.Bars bars_2, double double_0, bool bool_0, PositionType positionType_0, OrderType orderType_0)
        {
            Enum5 enum2;
            if (this.tradingSystemExecutor.NoDecimalRoundingForLimitStopPrice)
            {
                return double_0;
            }
            if (bars_2 == null)
            {
                bars_2 = this.bars_0;
            }
            if (bars_2.SymbolInfo.Tick == 0.0)
            {
                string str = "1";
                if (this.tradingSystemExecutor.PricingDecimalPlaces > 0)
                {
                    str = str.PadRight(this.tradingSystemExecutor.PricingDecimalPlaces + 1, '0');
                    bars_2.SymbolInfo.Tick = 1.0 / ((double) Convert.ToInt32(str));
                }
            }
            if (bars_2.SymbolInfo.Tick == 0.0)
            {
                return double_0;
            }
            if (double_0 == 0.0)
            {
                return double_0;
            }
            double d = double_0 / bars_2.SymbolInfo.Tick;
            string str2 = d.ToString("N6");
            string str3 = Math.Truncate(d).ToString("N6");
            if (str2 == str3)
            {
                return double_0;
            }
            switch (orderType_0)
            {
                case OrderType.Limit:
                    if (positionType_0 != PositionType.Long)
                    {
                        enum2 = bool_0 ? Enum5.const_2 : Enum5.const_1;
                        break;
                    }
                    enum2 = bool_0 ? Enum5.const_1 : Enum5.const_2;
                    break;

                case OrderType.Stop:
                    if (positionType_0 != PositionType.Long)
                    {
                        enum2 = bool_0 ? Enum5.const_1 : Enum5.const_2;
                        break;
                    }
                    enum2 = bool_0 ? Enum5.const_2 : Enum5.const_1;
                    break;

                default:
                    enum2 = Enum5.const_0;
                    break;
            }
            double num = Math.Truncate((double) (double_0 / bars_2.SymbolInfo.Tick)) * bars_2.SymbolInfo.Tick;
            double num2 = num + bars_2.SymbolInfo.Tick;
            switch (enum2)
            {
                case Enum5.const_1:
                    if (num < num2)
                    {
                        return num;
                    }
                    return num2;

                case Enum5.const_2:
                    if (num > num2)
                    {
                        return num;
                    }
                    return num2;
            }
            if (Math.Abs((double) (double_0 - num)) >= Math.Abs((double) (double_0 - num2)))
            {
                return num2;
            }
            return num;
        }

        ///WYJ fix, original signature: internal void method_4(WealthLab.Bars bars_2, ChartRenderer chartRenderer_1, TradingSystemExecutor tradingSystemExecutor_1, DataSource dataSource_1)
        internal void prepareAndExecute(WealthLab.Bars bars_2, ChartRenderer chartRenderer_1, TradingSystemExecutor tradingSystemExecutor_1, DataSource dataSource_1)
        {
            this.bars_0 = bars_2;
            this.chartRenderer = chartRenderer_1;
            this.tradingSystemExecutor = tradingSystemExecutor_1;
            this.dataSource_0 = dataSource_1;
            this.bars_1 = bars_2;
            if (chartRenderer_1 != null)
            {
                chartRenderer_1.PlotStops = false;
                chartRenderer_1.CreateDefaultPanes();
                chartRenderer_1.RightPaddingBars = 0;
                chartRenderer_1.WealthScript = this;
            }
            bool executing = false;
            if (chartRenderer_1 != null)
            {
                executing = chartRenderer_1.Executing;
                chartRenderer_1.Executing = true;
            }
            try
            {
                this.Execute();
            }
            finally
            {
                if (chartRenderer_1 != null)
                {
                    chartRenderer_1.Executing = executing;
                }
            }
        }

        private void method_5(WealthLab.Bars bars_2)
        {
            string str = bars_2.DataScale.ToString();
            bars_2.Close.Description = str + "(Close)";
            bars_2.Open.Description = str + "(Open)";
            bars_2.High.Description = str + "(High)";
            bars_2.Low.Description = str + "(Low)";
            bars_2.Volume.Description = str + "(Volume)";
        }

        private bool method_6(WealthLab.Bars bars_2, int int_1)
        {
            if (this.tradingSystemExecutor.LimitDaySimulation && (int_1 < bars_2.Count))
            {
                return !bars_2.IsLimitUpDay(int_1);
            }
            return true;
        }

        private bool method_7(int int_1)
        {
            return this.method_6(this.bars_0, int_1);
        }

        private bool method_8(WealthLab.Bars bars_2, int int_1)
        {
            if (this.tradingSystemExecutor.LimitDaySimulation && (int_1 < bars_2.Count))
            {
                return !bars_2.IsLimitDownDay(int_1);
            }
            return true;
        }

        private bool method_9(int int_1)
        {
            return this.method_8(this.bars_0, int_1);
        }

        public DataSeries MultiplySeriesValue(DataSeries smaSeries, double percentage, bool bool_0)
        {
            DataSeries series = new DataSeries(smaSeries.Description);
            for (int i = 0; i < smaSeries.Count; i++)
            {
                double num2;
                if (bool_0)
                {
                    num2 = smaSeries[i] * (1.0 + (percentage / 100.0));
                }
                else
                {
                    num2 = smaSeries[i] * (1.0 - (percentage / 100.0));
                }
                series.Add(num2);
            }
            return series;
        }

        public void PadBars(int numberOfBars)
        {
            if (this.chartRenderer != null)
            {
                this.chartRenderer.RightPaddingBars = numberOfBars;
            }
        }

        public virtual void PaintHook(WealthLab.Bars bars, Graphics graphics_0, WealthLab.ChartStyle chartStyle, PaintHookStage stage)
        {
        }

        public void PlotFundamentalItems(ChartPane pane, string itemName, Color color, LineStyle style, int width)
        {
            this.PlotFundamentalItems(pane, "", itemName, color, style, width);
        }

        public void PlotFundamentalItems(ChartPane pane, string symbol, string itemName, Color color, LineStyle style, int width)
        {
            if (this.chartRenderer != null)
            {
                DataSeries series;
                if (symbol == "")
                {
                    series = this.FundamentalDataSeries(itemName);
                }
                else
                {
                    series = this.FundamentalDataSeries(symbol, itemName);
                }
                if (series != null)
                {
                    PlottedIndicator item = new PlottedIndicator(this.chartRenderer, series) {
                        Color = color,
                        Width = 1,
                        Style = LineStyle.Invisible
                    };
                    pane.PlottedIndicators.Add(item);
                    if (symbol == "")
                    {
                        this.chartRenderer.PlotFundamentalItem(pane, itemName, color, style, width, false);
                    }
                    else
                    {
                        this.chartRenderer.PlotFundamentalItem(pane, symbol, itemName, color, style, width, false);
                    }
                }
            }
        }

        public void PlotSeries(ChartPane pane, DataSeries series, Color color, LineStyle style, int width)
        {
            if (this.chartRenderer != null)
            {
                this.method_2(series);
                PlottedIndicator item = new PlottedIndicator(this.chartRenderer, series) {
                    Color = color,
                    Width = width,
                    Style = style
                };
                pane.PlottedIndicators.Add(item);
            }
        }

        public void PlotSeries(ChartPane pane, DataSeries series, Color color, LineStyle style, int width, string label)
        {
            series.Description = label;
            if (this.chartRenderer != null)
            {
                this.method_2(series);
                PlottedIndicator item = new PlottedIndicator(this.chartRenderer, series) {
                    Color = color,
                    Width = width,
                    Style = style
                };
                pane.PlottedIndicators.Add(item);
            }
        }

        public void PlotSeriesDualFillBand(ChartPane pane, DataSeries series1, DataSeries series2, Brush brush1, Brush brush2, Color color, LineStyle style, int width)
        {
            if (this.chartRenderer != null)
            {
                this.method_2(series1);
                this.method_2(series2);
                PlottedIndicator item = new PlottedIndicator(this.chartRenderer, series1) {
                    Color = color,
                    Width = width,
                    Style = style
                };
                pane.PlottedIndicators.Add(item);
                PlottedIndicator indicator2 = new PlottedIndicator(this.chartRenderer, series2) {
                    Color = color,
                    Width = width,
                    Style = style
                };
                pane.PlottedIndicators.Add(indicator2);
                WSDDualColorFilledBand band = new WSDDualColorFilledBand(series1, series2, brush1, brush2);
                this.chartRenderer.PlotWealthScriptObject(pane, band, false, true);
            }
        }

        public void PlotSeriesDualFillBand(ChartPane pane, DataSeries series1, DataSeries series2, Color fillColor1, Color fillColor2, Color color, LineStyle style, int width)
        {
            if (this.chartRenderer != null)
            {
                this.PlotSeriesDualFillBand(pane, series1, series2, new SolidBrush(fillColor1), new SolidBrush(fillColor2), color, style, width);
            }
        }

        public void PlotSeriesFillBand(ChartPane pane, DataSeries upper, DataSeries lower, Color color, Brush fillBrush, LineStyle style, int width)
        {
            if (this.chartRenderer != null)
            {
                this.method_2(upper);
                this.method_2(lower);
                PlottedIndicator item = new PlottedIndicator(this.chartRenderer, upper) {
                    Color = color,
                    Width = width,
                    Style = style
                };
                pane.PlottedIndicators.Add(item);
                PlottedIndicator indicator2 = new PlottedIndicator(this.chartRenderer, lower) {
                    Color = color,
                    Width = width,
                    Style = style
                };
                pane.PlottedIndicators.Add(indicator2);
                WSDFilledBand band = new WSDFilledBand(upper, lower, fillBrush);
                this.chartRenderer.PlotWealthScriptObject(pane, band, false, true);
            }
        }

        public void PlotSeriesFillBand(ChartPane pane, DataSeries upper, DataSeries lower, Color color, Color fillColor, LineStyle style, int width)
        {
            if (this.chartRenderer != null)
            {
                this.PlotSeriesFillBand(pane, upper, lower, color, new SolidBrush(fillColor), style, width);
            }
        }

        public void PlotSeriesOscillator(ChartPane pane, DataSeries source, double overbought, double oversold, Brush overboughtBrush, Brush oversoldBrush, Color color, LineStyle style, int width)
        {
            if (this.chartRenderer != null)
            {
                this.method_2(source);
                PlottedIndicator item = new PlottedIndicator(this.chartRenderer, source) {
                    Color = color,
                    Width = width,
                    Style = style
                };
                pane.PlottedIndicators.Add(item);
                WSDFilledOscillator oscillator = new WSDFilledOscillator(source, overbought, oversold, overboughtBrush, oversoldBrush);
                this.chartRenderer.PlotWealthScriptObject(pane, oscillator, false, true);
            }
        }

        public void PlotSeriesOscillator(ChartPane pane, DataSeries source, double overbought, double oversold, Color overboughtColor, Color oversoldColor, Color color, LineStyle style, int width)
        {
            if (this.chartRenderer != null)
            {
                this.PlotSeriesOscillator(pane, source, overbought, oversold, new SolidBrush(overboughtColor), new SolidBrush(oversoldColor), color, style, width);
            }
        }

        public void PlotStops()
        {
            if (this.chartRenderer != null)
            {
                this.chartRenderer.PlotStops = true;
            }
        }

        public void PlotSymbol(ChartPane pane, WealthLab.Bars bars, Color upBarColor, Color downBarColor)
        {
            if (this.chartRenderer != null)
            {
                this.method_2(bars.Open);
                this.chartRenderer.PlotSymbol(bars, pane, upBarColor, downBarColor);
            }
        }

        public void PlotSyntheticSymbol(ChartPane pane, string symbol, DataSeries open, DataSeries high, DataSeries dataSeries_0, DataSeries close, DataSeries volume, Color upBarColor, Color downBarColor)
        {
            if (this.chartRenderer != null)
            {
                this.method_2(open);
                WealthLab.Bars bars = new WealthLab.Bars(symbol, this.bars_0.Scale, this.bars_0.BarInterval, this.bars_0.DateList, open, high, dataSeries_0, close, volume);
                this.chartRenderer.PlotSymbol(bars, pane, upBarColor, downBarColor);
            }
        }

        public void PrintDebug(object message)
        {
            this.tradingSystemExecutor.method_15(message.ToString());
        }

        public void PrintDebug(string message)
        {
            this.tradingSystemExecutor.method_15(message);
        }

        public void PrintDebug(params object[] messages)
        {
            foreach (object obj2 in messages)
            {
                this.tradingSystemExecutor.method_15(obj2.ToString());
            }
        }

        public void PrintStatusBar(string message)
        {
            this.tradingSystemExecutor.method_18(message);
        }

        public void RemoveGlobal(object value)
        {
            lock (dictionary_0)
            {
                using (Dictionary<string, object>.Enumerator enumerator = dictionary_0.GetEnumerator())
                {
                    KeyValuePair<string, object> current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (current.Value == value)
                        {
                            ///goto  Label_003D; ///WYJ fix, simplify the flow
                            dictionary_0.Remove(current.Key);
                            return ;
                        }
                    }
                    return;
                }
            }
        }

        public void RemoveGlobal(string string_0)
        {
            lock (dictionary_0)
            {
                if (dictionary_0.ContainsKey(string_0))
                {
                    dictionary_0.Remove(string_0);
                }
            }
        }

        public void RestoreContext()
        {
            if (this.bars_0 != this.bars_1)
            {
                BarScale scale = this.bars_0.Scale;
                int barInterval = this.bars_0.BarInterval;
                this.bars_0 = this.bars_1;
                if ((this.bars_0.Scale != scale) || (this.bars_0.BarInterval != barInterval))
                {
                    WealthLab.Bars bars = this.tradingSystemExecutor.findBarsData(this.bars_0.Symbol, scale, barInterval, true);
                    if (bars != null)
                    {
                        this.bars_0 = bars;
                    }
                    else
                    {
                        this.bars_0 = BarScaleConverter.ReScale(this.bars_0, scale, barInterval);
                        this.tradingSystemExecutor.method_13(this.bars_0, true);
                    }
                }
            }
        }

        public void RestoreParameterDefaults()
        {
            foreach (StrategyParameter parameter in this.Parameters)
            {
                parameter.Value = parameter.DefaultValue;
            }
        }

        public void RestoreScale()
        {
            this.bars_0 = this.tradingSystemExecutor.findBarsData(this.bars_0.Symbol, this.bars_1.Scale, this.bars_1.BarInterval, true);
            if (this.bars_0 == null)
            {
                this.bars_0 = this.bars_1;
            }
        }

        public bool SellAtAutoTrailingStop(int int_1, Position position_0, double triggerPct, double profitReversalPct)
        {
            return this.SellAtAutoTrailingStop(int_1, position_0, triggerPct, profitReversalPct, "AutoTrailing");
        }

        public bool SellAtAutoTrailingStop(int int_1, Position position_0, double triggerPct, double profitReversalPct, string signalName)
        {
            if (((position_0 != null) && position_0.Active) && (position_0.MFEAsOfBarPercent(int_1 - 1) >= triggerPct))
            {
                double num = position_0.HighestHighAsOfBar(int_1 - 1);
                double num2 = num - position_0.EntryPrice;
                double num3 = (profitReversalPct / 100.0) * num2;
                return this.SellAtTrailingStop(int_1, position_0, num - num3, signalName);
            }
            return false;
        }

        public bool SellAtClose(int int_1, Position position_0)
        {
            return this.SellAtClose(int_1, position_0, "");
        }

        public bool SellAtClose(int int_1, Position position_0, string signalName)
        {
            if ((position_0 == null) || !position_0.Active)
            {
                return false;
            }
            if (position_0 == null)
            {
                return false;
            }
            if (position_0 == Position.AllPositions)
            {
                bool flag = false;
                int count = this.tradingSystemExecutor.ActivePositions.Count;
                for (int i = this.Positions.Count - 1; i >= 0; i--)
                {
                    if (count == 0)
                    {
                        return flag;
                    }
                    if (this.Positions[i].Active && this.SellAtClose(int_1, this.Positions[i], signalName))
                    {
                        flag = true;
                        count--;
                    }
                }
                return flag;
            }
            if (!this.method_8(position_0.Bars, int_1))
            {
                return false;
            }
            this.method_0(int_1, position_0.EntryBar, this.bars_0.Count);
            if (position_0.PositionType != PositionType.Long)
            {
                return false;
            }
            if (int_1 == position_0.Bars.Count)
            {
                Alert alert = new Alert(this.Strategy, position_0.Bars, position_0.Bars.Date[int_1 - 1], TradeType.Sell, OrderType.AtClose, position_0.Shares, signalName) {
                    Position = position_0
                };
                this.tradingSystemExecutor.method_6(alert);
                return false;
            }
            double num3 = position_0.Bars.Close[int_1];
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                num3 -= this.tradingSystemExecutor.method_14(num3, false, position_0.Bars);
                num3 = this.roundPriceToTickMultiple(position_0.Bars, num3, false, PositionType.Long, OrderType.AtClose);
            }
            position_0.method_1(int_1, num3, OrderType.AtClose);
            this.tradingSystemExecutor.ActivePositions.Remove(position_0);
            position_0.ExitSignal = signalName;
            return true;
        }

        public bool SellAtLimit(int int_1, Position position_0, double limitPrice)
        {
            return this.SellAtLimit(int_1, position_0, limitPrice, "");
        }

        public bool SellAtLimit(int int_1, Position position_0, double limitPrice, string signalName)
        {
            if ((position_0 == null) || !position_0.Active)
            {
                return false;
            }
            if (position_0 == null)
            {
                return false;
            }
            limitPrice = this.roundPriceToTickMultiple(position_0.Bars, limitPrice, false, PositionType.Long, OrderType.Limit);
            if (position_0 == Position.AllPositions)
            {
                bool flag = false;
                int count = this.tradingSystemExecutor.ActivePositions.Count;
                for (int i = this.Positions.Count - 1; i >= 0; i--)
                {
                    if (count == 0)
                    {
                        return flag;
                    }
                    if (this.Positions[i].Active && this.SellAtLimit(int_1, this.Positions[i], limitPrice, signalName))
                    {
                        flag = true;
                        count--;
                    }
                }
                return flag;
            }
            if (!this.method_8(position_0.Bars, int_1))
            {
                return false;
            }
            this.method_0(int_1, position_0.EntryBar, position_0.Bars.Count);
            if (position_0.PositionType != PositionType.Long)
            {
                return false;
            }
            if (int_1 == position_0.Bars.Count)
            {
                Alert alert = new Alert(this.Strategy, position_0.Bars, position_0.Bars.Date[int_1 - 1], TradeType.Sell, OrderType.Limit, position_0.Shares, signalName) {
                    Position = position_0,
                    Price = limitPrice
                };
                this.tradingSystemExecutor.method_6(alert);
                return false;
            }
            if (((this.chartRenderer != null) && this.chartRenderer.PlotStops) && (position_0.Bars == this.bars_1))
            {
                this.chartRenderer.method_13(int_1, limitPrice, TradeType.Sell);
            }
            if ((limitPrice + this.tradingSystemExecutor.method_14(limitPrice, true, position_0.Bars)) > position_0.Bars.High[int_1])
            {
                return false;
            }
            if (position_0.Bars.Open[int_1] > limitPrice)
            {
                limitPrice = position_0.Bars.Open[int_1];
            }
            position_0.method_1(int_1, limitPrice, OrderType.Limit);
            this.tradingSystemExecutor.ActivePositions.Remove(position_0);
            position_0.ExitSignal = signalName;
            return true;
        }

        public bool SellAtMarket(int int_1, Position position_0)
        {
            return this.SellAtMarket(int_1, position_0, "");
        }

        public bool SellAtMarket(int int_1, Position position_0, string signalName)
        {
            if ((position_0 == null) || !position_0.Active)
            {
                return false;
            }
            if (position_0 == null)
            {
                return false;
            }
            if (position_0 == Position.AllPositions)
            {
                bool flag = false;
                int count = this.tradingSystemExecutor.ActivePositions.Count;
                for (int i = this.Positions.Count - 1; i >= 0; i--)
                {
                    if (count == 0)
                    {
                        return flag;
                    }
                    if (this.Positions[i].Active && this.SellAtMarket(int_1, this.Positions[i], signalName))
                    {
                        flag = true;
                        count--;
                    }
                }
                return flag;
            }
            if (!this.method_8(position_0.Bars, int_1))
            {
                return false;
            }
            this.method_0(int_1, position_0.EntryBar, this.bars_0.Count);
            if (position_0.PositionType != PositionType.Long)
            {
                return false;
            }
            if (int_1 == this.bars_0.Count)
            {
                Alert alert = new Alert(this.Strategy, position_0.Bars, position_0.Bars.Date[int_1 - 1], TradeType.Sell, OrderType.Market, position_0.Shares, signalName) {
                    Position = position_0
                };
                this.tradingSystemExecutor.method_6(alert);
                return false;
            }
            double num3 = position_0.Bars.Open[int_1];
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                num3 -= this.tradingSystemExecutor.method_14(num3, false, position_0.Bars);
                num3 = this.roundPriceToTickMultiple(position_0.Bars, num3, false, PositionType.Long, OrderType.Market);
            }
            position_0.method_1(int_1, num3, OrderType.Market);
            this.tradingSystemExecutor.ActivePositions.Remove(position_0);
            position_0.ExitSignal = signalName;
            return true;
        }

        public bool SellAtStop(int int_1, Position position_0, double stopPrice)
        {
            return this.SellAtStop(int_1, position_0, stopPrice, "");
        }

        public bool SellAtStop(int int_1, Position position_0, double stopPrice, string signalName)
        {
            if ((position_0 == null) || !position_0.Active)
            {
                return false;
            }
            if (position_0 == null)
            {
                return false;
            }
            stopPrice = this.roundPriceToTickMultiple(position_0.Bars, stopPrice, false, PositionType.Long, OrderType.Stop);
            if (position_0 == Position.AllPositions)
            {
                bool flag = false;
                int count = this.tradingSystemExecutor.ActivePositions.Count;
                for (int i = this.Positions.Count - 1; i >= 0; i--)
                {
                    if (count == 0)
                    {
                        return flag;
                    }
                    if (this.Positions[i].Active && this.SellAtStop(int_1, this.Positions[i], stopPrice, signalName))
                    {
                        flag = true;
                        count--;
                    }
                }
                return flag;
            }
            if (!this.method_8(position_0.Bars, int_1))
            {
                return false;
            }
            this.method_0(int_1, position_0.EntryBar, position_0.Bars.Count);
            if (position_0.PositionType != PositionType.Long)
            {
                return false;
            }
            if (int_1 == position_0.Bars.Count)
            {
                Alert alert = new Alert(this.Strategy, position_0.Bars, position_0.Bars.Date[int_1 - 1], TradeType.Sell, OrderType.Stop, position_0.Shares, signalName) {
                    Position = position_0,
                    Price = stopPrice
                };
                this.tradingSystemExecutor.method_6(alert);
                return false;
            }
            if (((this.chartRenderer != null) && this.chartRenderer.PlotStops) && (position_0.Bars == this.bars_1))
            {
                this.chartRenderer.method_13(int_1, stopPrice, TradeType.Sell);
            }
            if (stopPrice < position_0.Bars.Low[int_1])
            {
                return false;
            }
            if (position_0.Bars.Open[int_1] < stopPrice)
            {
                stopPrice = position_0.Bars.Open[int_1];
            }
            double num3 = stopPrice;
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                num3 -= this.tradingSystemExecutor.method_14(stopPrice, false, position_0.Bars);
                num3 = this.roundPriceToTickMultiple(position_0.Bars, num3, false, PositionType.Long, OrderType.Stop);
            }
            position_0.method_1(int_1, num3, OrderType.Stop);
            this.tradingSystemExecutor.ActivePositions.Remove(position_0);
            position_0.ExitSignal = signalName;
            return true;
        }

        public bool SellAtTrailingStop(int int_1, Position position_0, double stopPrice)
        {
            return this.SellAtTrailingStop(int_1, position_0, stopPrice, "");
        }

        public bool SellAtTrailingStop(int int_1, Position position_0, double stopPrice, string signalName)
        {
            if ((position_0 == null) || !position_0.Active)
            {
                return false;
            }
            if (position_0 == null)
            {
                return false;
            }
            if (position_0 == Position.AllPositions)
            {
                bool flag = false;
                int count = this.tradingSystemExecutor.ActivePositions.Count;
                for (int i = this.Positions.Count - 1; i >= 0; i--)
                {
                    if (count == 0)
                    {
                        return flag;
                    }
                    if (this.Positions[i].Active && this.SellAtTrailingStop(int_1, this.Positions[i], stopPrice, signalName))
                    {
                        flag = true;
                        count--;
                    }
                }
                return flag;
            }
            if (!this.method_8(position_0.Bars, int_1))
            {
                return false;
            }
            stopPrice = this.roundPriceToTickMultiple(position_0.Bars, stopPrice, false, PositionType.Long, OrderType.Stop);
            if (stopPrice > position_0.TrailingStop)
            {
                position_0.TrailingStop = stopPrice;
            }
            stopPrice = position_0.TrailingStop;
            this.method_0(int_1, position_0.EntryBar, position_0.Bars.Count);
            if (position_0.PositionType != PositionType.Long)
            {
                return false;
            }
            if (int_1 == position_0.Bars.Count)
            {
                Alert alert = new Alert(this.Strategy, position_0.Bars, position_0.Bars.Date[int_1 - 1], TradeType.Sell, OrderType.Stop, position_0.Shares, signalName) {
                    Position = position_0,
                    Price = stopPrice
                };
                this.tradingSystemExecutor.method_6(alert);
                return false;
            }
            if (((this.chartRenderer != null) && this.chartRenderer.PlotStops) && (position_0.Bars == this.bars_1))
            {
                this.chartRenderer.method_13(int_1, stopPrice, TradeType.Sell);
            }
            if (stopPrice < position_0.Bars.Low[int_1])
            {
                return false;
            }
            if (position_0.Bars.Open[int_1] < stopPrice)
            {
                stopPrice = position_0.Bars.Open[int_1];
            }
            double num3 = stopPrice;
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                num3 -= this.tradingSystemExecutor.method_14(stopPrice, false, position_0.Bars);
                num3 = this.roundPriceToTickMultiple(position_0.Bars, num3, false, PositionType.Long, OrderType.Stop);
            }
            position_0.method_1(int_1, num3, OrderType.Stop);
            this.tradingSystemExecutor.ActivePositions.Remove(position_0);
            position_0.ExitSignal = signalName;
            return true;
        }

        public void SetBackgroundColor(int int_1, Color color)
        {
            if (this.chartRenderer != null)
            {
                this.chartRenderer.SetBackgroundColor(int_1, color);
            }
        }

        public void SetBarColor(int int_1, Color color)
        {
            if (this.chartRenderer != null)
            {
                this.chartRenderer.SetBarColor(int_1, color);
            }
        }

        public void SetBarColors(Color colorUp, Color colorDown)
        {
            if (this.chartRenderer != null)
            {
                this.chartRenderer.UpBarColor = colorUp;
                this.chartRenderer.DownBarColor = colorDown;
            }
        }

        public void SetContext(string symbol, bool synchronize)
        {
            if (symbol != this.bars_0.Symbol)
            {
                WealthLab.Bars bars2 = this.tradingSystemExecutor.findBarsData(symbol, this.bars_0.Scale, this.bars_0.BarInterval, synchronize);
                if (bars2 == null)
                {
                    bars2 = this.tradingSystemExecutor.findBarsData(symbol, this.bars_1.Scale, this.bars_1.BarInterval, synchronize);
                    if (bars2 == null)
                    {
                        bars2 = this.tradingSystemExecutor.method_10(symbol, synchronize);
                    }
                    if (bars2 == null)
                    {
                        throw new InvalidOperationException("Could not load data for symbol: " + symbol);
                    }
                }
                if (synchronize && ((bars2.Scale != this.bars_0.Scale) || (bars2.BarInterval != this.bars_0.BarInterval)))
                {
                    WealthLab.Bars bars = null;
                    try
                    {
                        bars = BarScaleConverter.ReScale(bars2, this.bars_0.Scale, this.bars_0.BarInterval);
                    }
                    catch
                    {
                        bars = BarScaleConverter.Synchronize(bars2, this.bars_0);
                    }
                    bars2 = bars;
                    this.tradingSystemExecutor.method_13(bars2, synchronize);
                }
                this.bars_0 = bars2;
                this.bars_0.Open.Description = "Open(" + symbol + ")";
                this.bars_0.High.Description = "High(" + symbol + ")";
                this.bars_0.Low.Description = "Low(" + symbol + ")";
                this.bars_0.Close.Description = "Close(" + symbol + ")";
                this.bars_0.Volume.Description = "Volume(" + symbol + ")";
            }
        }

        public void SetGlobal(string string_0, object value)
        {
            lock (dictionary_0)
            {
                dictionary_0[string_0] = value;
            }
        }

        public void SetLogScale(ChartPane pane, bool logScale)
        {
            if (this.chartRenderer != null)
            {
                pane.LogScale = logScale;
            }
        }

        public void SetPaneBackgroundColor(ChartPane pane, int int_1, Color color)
        {
            if (this.chartRenderer != null)
            {
                pane.SetBackgroundColor(int_1, color);
            }
        }

        public void SetPaneMinMax(ChartPane pane, double double_0, double double_1)
        {
            if (this.chartRenderer != null)
            {
                pane.MinValue = double_0;
                pane.MaxValue = double_1;
            }
        }

        public void SetScaleCompressed(int barInterval)
        {
            this.RestoreScale();
            WealthLab.Bars bars = this.tradingSystemExecutor.findBarsData(this.bars_0.Symbol, this.bars_0.Scale, barInterval, false);
            if (bars == null)
            {
                bars = BarScaleConverter.ToIntradayCompressed(this.bars_0, this.bars_0.Scale, barInterval);
                this.method_5(bars);
                this.tradingSystemExecutor.method_13(bars, false);
            }
            this.bars_0 = bars;
        }

        public void SetScaleDaily()
        {
            this.RestoreScale();
            WealthLab.Bars bars = this.tradingSystemExecutor.findBarsData(this.bars_0.Symbol, BarScale.Daily, 0, false);
            if (bars == null)
            {
                bars = BarScaleConverter.ToDaily(this.bars_0);
                this.method_5(bars);
                this.tradingSystemExecutor.method_13(bars, false);
            }
            this.bars_0 = bars;
        }

        public void SetScaleMonthly()
        {
            this.RestoreScale();
            WealthLab.Bars bars = this.tradingSystemExecutor.findBarsData(this.bars_0.Symbol, BarScale.Monthly, 0, false);
            if (bars == null)
            {
                bars = BarScaleConverter.ToMonthly(this.bars_0);
                this.method_5(bars);
                this.tradingSystemExecutor.method_13(bars, false);
            }
            this.bars_0 = bars;
        }

        public void SetScaleWeekly()
        {
            this.RestoreScale();
            WealthLab.Bars bars = this.tradingSystemExecutor.findBarsData(this.bars_0.Symbol, BarScale.Weekly, 0, false);
            if (bars == null)
            {
                bars = BarScaleConverter.ToWeekly(this.bars_0);
                this.method_5(bars);
                this.tradingSystemExecutor.method_13(bars, false);
            }
            this.bars_0 = bars;
        }

        public void SetSeriesBarColor(int int_1, DataSeries dataSeries_0, Color color)
        {
            PlottedIndicator indicator = null;
            if (this.chartRenderer != null)
            {
                indicator = this.chartRenderer.FindPlottedIndicator(dataSeries_0);
            }
            if (indicator != null)
            {
                indicator.SetBarColor(int_1, color);
            }
        }

        public void SetShareSize(double shares)
        {
            this.OverrideShareSize = shares;
        }

        public Position ShortAtClose(int int_1)
        {
            return this.ShortAtClose(int_1, "");
        }

        public Position ShortAtClose(int int_1, string signalName)
        {
            if (!this.method_9(int_1))
            {
                return null;
            }
            this.method_0(int_1, 0, this.bars_0.Count);
            double num = this.bars_0.Close[int_1 - 1];
            this.method_1(num);
            double shares = this.tradingSystemExecutor.CalcPositionSize(this.bars_0, int_1, num, PositionType.Short, this.RiskStopLevel, true);
            if (int_1 == this.bars_0.Count)
            {
                Alert alert = new Alert(this.Strategy, this.bars_0, this.bars_0.Date[int_1 - 1], TradeType.Short, OrderType.AtClose, shares, signalName, num, this.RiskStopLevel, this.AutoProfitLevel);
                this.tradingSystemExecutor.method_6(alert);
                return null;
            }
            Position position = new Position(this.bars_0, PositionType.Short, this.tradingSystemExecutor.Strategy.ID.ToString()) {
                BasisPrice = num,
                RiskStopLevel = this.RiskStopLevel,
                AutoProfitLevel = this.AutoProfitLevel,
                EntryPrice = this.bars_0.Close[int_1]
            };
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                position.EntryPrice -= this.tradingSystemExecutor.method_14(this.bars_0.Close[int_1], false, this.bars_0);
                position.EntryPrice = this.roundPriceToTickMultiple(position.Bars, position.EntryPrice, true, PositionType.Short, OrderType.AtClose);
                if (position.EntryPrice < this.bars_0.Low[int_1])
                {
                    position.EntryPrice = this.bars_0.Low[int_1];
                }
            }
            position.EntryBar = int_1;
            position.EntryOrderType = OrderType.AtClose;
            position.EntrySignal = signalName;
            position.Shares = shares;
            position.OverrideShareSize = this.OverrideShareSize;
            this.tradingSystemExecutor.method_5(position);
            return position;
        }

        public Position ShortAtLimit(int int_1, double limitPrice)
        {
            return this.ShortAtLimit(int_1, limitPrice, "");
        }

        public Position ShortAtLimit(int int_1, double limitPrice, string signalName)
        {
            if (!this.method_9(int_1))
            {
                return null;
            }
            this.method_1(limitPrice);
            this.method_0(int_1, 0, this.bars_0.Count);
            limitPrice = this.roundPriceToTickMultiple(this.bars_0, limitPrice, true, PositionType.Short, OrderType.Limit);
            double shares = this.tradingSystemExecutor.CalcPositionSize(this.bars_0, int_1, limitPrice, PositionType.Short, this.RiskStopLevel, true);
            if (int_1 == this.bars_0.Count)
            {
                Alert alert = new Alert(this.Strategy, this.bars_0, this.bars_0.Date[int_1 - 1], TradeType.Short, OrderType.Limit, shares, signalName, limitPrice, this.RiskStopLevel, this.AutoProfitLevel) {
                    Price = limitPrice
                };
                this.tradingSystemExecutor.method_6(alert);
                return null;
            }
            if (((this.chartRenderer != null) && this.chartRenderer.PlotStops) && (this.bars_0 == this.bars_1))
            {
                this.chartRenderer.method_13(int_1, limitPrice, TradeType.Short);
            }
            if ((limitPrice + this.tradingSystemExecutor.method_14(limitPrice, true, this.bars_0)) > this.bars_0.High[int_1])
            {
                return null;
            }
            Position position = new Position(this.bars_0, PositionType.Short, this.tradingSystemExecutor.Strategy.ID.ToString()) {
                BasisPrice = limitPrice,
                RiskStopLevel = this.RiskStopLevel,
                AutoProfitLevel = this.AutoProfitLevel,
                EntryBar = int_1,
                EntryOrderType = OrderType.Limit,
                EntrySignal = signalName,
                Shares = shares
            };
            if (this.bars_0.Open[int_1] > limitPrice)
            {
                limitPrice = this.bars_0.Open[int_1];
            }
            position.EntryPrice = limitPrice;
            position.OverrideShareSize = this.OverrideShareSize;
            this.tradingSystemExecutor.method_5(position);
            return position;
        }

        public Position ShortAtMarket(int int_1)
        {
            return this.ShortAtMarket(int_1, "");
        }

        public Position ShortAtMarket(int int_1, string signalName)
        {
            if (!this.method_9(int_1))
            {
                return null;
            }
            this.method_0(int_1, 1, this.bars_0.Count);
            double num = this.bars_0.Close[int_1 - 1];
            this.method_1(num);
            double shares = this.tradingSystemExecutor.CalcPositionSize(this.bars_0, int_1, num, PositionType.Short, this.RiskStopLevel, true);
            if (int_1 == this.bars_0.Count)
            {
                Alert alert = new Alert(this.Strategy, this.bars_0, this.bars_0.Date[int_1 - 1], TradeType.Short, OrderType.Market, shares, signalName, num, this.RiskStopLevel, this.AutoProfitLevel);
                this.tradingSystemExecutor.method_6(alert);
                return null;
            }
            Position position = new Position(this.bars_0, PositionType.Short, this.tradingSystemExecutor.Strategy.ID.ToString()) {
                BasisPrice = num,
                RiskStopLevel = this.RiskStopLevel,
                AutoProfitLevel = this.AutoProfitLevel,
                EntryPrice = this.bars_0.Open[int_1]
            };
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                position.EntryPrice -= this.tradingSystemExecutor.method_14(this.bars_0.Open[int_1], false, this.bars_0);
                position.EntryPrice = this.roundPriceToTickMultiple(position.Bars, position.EntryPrice, true, PositionType.Short, OrderType.Market);
                if (position.EntryPrice < this.bars_0.Low[int_1])
                {
                    position.EntryPrice = this.bars_0.Low[int_1];
                }
            }
            position.EntryBar = int_1;
            position.EntrySignal = signalName;
            position.Shares = shares;
            position.OverrideShareSize = this.OverrideShareSize;
            this.tradingSystemExecutor.method_5(position);
            return position;
        }

        public Position ShortAtStop(int int_1, double stopPrice)
        {
            return this.ShortAtStop(int_1, stopPrice, "");
        }

        public Position ShortAtStop(int int_1, double stopPrice, string signalName)
        {
            if (!this.method_9(int_1))
            {
                return null;
            }
            this.method_1(stopPrice);
            this.method_0(int_1, 0, this.bars_0.Count);
            stopPrice = this.roundPriceToTickMultiple(this.bars_0, stopPrice, true, PositionType.Short, OrderType.Stop);
            double shares = this.tradingSystemExecutor.CalcPositionSize(this.bars_0, int_1, stopPrice, PositionType.Short, this.RiskStopLevel, true);
            if (int_1 == this.bars_0.Count)
            {
                Alert alert = new Alert(this.Strategy, this.bars_0, this.bars_0.Date[int_1 - 1], TradeType.Short, OrderType.Stop, shares, signalName, stopPrice, this.RiskStopLevel, this.AutoProfitLevel) {
                    Price = stopPrice
                };
                this.tradingSystemExecutor.method_6(alert);
                return null;
            }
            if (((this.chartRenderer != null) && this.chartRenderer.PlotStops) && (this.bars_0 == this.bars_1))
            {
                this.chartRenderer.method_13(int_1, stopPrice, TradeType.Short);
            }
            if (stopPrice < this.bars_0.Low[int_1])
            {
                return null;
            }
            Position position = new Position(this.bars_0, PositionType.Short, this.tradingSystemExecutor.Strategy.ID.ToString()) {
                BasisPrice = stopPrice,
                RiskStopLevel = this.RiskStopLevel,
                AutoProfitLevel = this.AutoProfitLevel,
                EntryBar = int_1,
                EntryOrderType = OrderType.Stop,
                EntrySignal = signalName,
                Shares = shares
            };
            if (this.bars_0.Open[int_1] < stopPrice)
            {
                stopPrice = this.bars_0.Open[int_1];
            }
            position.EntryPrice = stopPrice;
            if (this.tradingSystemExecutor.EnableSlippage)
            {
                position.EntryPrice -= this.tradingSystemExecutor.method_14(stopPrice, false, this.bars_0);
                position.EntryPrice = this.roundPriceToTickMultiple(position.Bars, position.EntryPrice, true, PositionType.Short, OrderType.Stop);
                if (position.EntryPrice < this.bars_0.Low[int_1])
                {
                    position.EntryPrice = this.bars_0.Low[int_1];
                }
            }
            position.OverrideShareSize = this.OverrideShareSize;
            this.tradingSystemExecutor.method_5(position);
            return position;
        }

        public Position SplitPosition(Position position, double percentToRetain)
        {
            if (percentToRetain <= 0.0)
            {
                throw new ArgumentException("Percentage of shares to retain in SplitPosition must be greater than zero");
            }
            if (!position.Active)
            {
                throw new ArgumentException("Cannot split a Position that is already closed");
            }
            double num = position.Shares * (percentToRetain / 100.0);
            if (position.Bars.SymbolInfo.SecurityType != SecurityType.MutualFund)
            {
                num = (int) num;
            }
            Position position2 = new Position(position.Bars, position.PositionType, this.tradingSystemExecutor.Strategy.ID.ToString()) {
                EntryBar = position.EntryBar,
                EntryOrderType = position.EntryOrderType,
                EntryPrice = position.EntryPrice,
                EntrySignal = position.EntrySignal,
                BasisPrice = position.BasisPrice,
                RiskStopLevel = position.RiskStopLevel,
                AutoProfitLevel = position.AutoProfitLevel,
                TrailingStop = position.TrailingStop
            };
            double shares = position.Shares;
            position.Shares = num;
            position2.Shares = shares - num;
            double splitFactor = position.SplitFactor;
            position.SplitFactor *= percentToRetain / 100.0;
            position2.SplitFactor = splitFactor * (1.0 - (percentToRetain / 100.0));
            position2.OverrideShareSize = position.OverrideShareSize;
            this.tradingSystemExecutor.method_5(position2);
            return position2;
        }

        public WealthLab.Bars Synchronize(WealthLab.Bars source)
        {
            return BarScaleConverter.Synchronize(source, this.bars_0);
        }

        public DataSeries Synchronize(DataSeries source)
        {
            DataSeries series = BarScaleConverter.Synchronize(source, this.bars_0);
            if (source.FirstValidValue > 0)
            {
                DateTime time = source.Date[source.FirstValidValue];
                for (int i = 0; i < series.Count; i++)
                {
                    if (series.Date[i] >= time)
                    {
                        series.FirstValidValue = i;
                        return series;
                    }
                }
            }
            return series;
        }

        public double TrendlineValue(int int_1, string trendLineName)
        {
            return this.tradingSystemExecutor.method_20(int_1, trendLineName);
        }

        public bool TurnDown(int int_1, DataSeries series)
        {
            if (int_1 > 0)
            {
                if (series.Count < 2)
                {
                    return false;
                }
                if (series[int_1] >= series[int_1 - 1])
                {
                    return false;
                }
                for (int i = int_1 - 1; i >= 1; i--)
                {
                    if (series[i] > series[i - 1])
                    {
                        return true;
                    }
                    if (series[i] < series[i - 1])
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public bool TurnUp(int int_1, DataSeries series)
        {
            if (int_1 > 0)
            {
                if (series.Count < 2)
                {
                    return false;
                }
                if (series[int_1] <= series[int_1 - 1])
                {
                    return false;
                }
                for (int i = int_1 - 1; i >= 1; i--)
                {
                    if (series[i] < series[i - 1])
                    {
                        return true;
                    }
                    if (series[i] > series[i - 1])
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public IList<Position> ActivePositions
        {
            get
            {
                return this.tradingSystemExecutor.ActivePositions;
            }
        }

        public IList<Alert> Alerts
        {
            get
            {
                return this.tradingSystemExecutor.CurrentAlerts;
            }
        }

        public double AutoProfitLevel
        {
            get
            {
                return this.tradingSystemExecutor.AutoProfitLevel;
            }
            set
            {
                this.tradingSystemExecutor.AutoProfitLevel = value;
            }
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars_0;
            }
        }

        public WealthLab.ChartStyle ChartStyle
        {
            get
            {
                if (this.chartRenderer != null)
                {
                    return this.chartRenderer.ChartStyle;
                }
                return null;
            }
        }

        public DataSeries Close
        {
            get
            {
                return this.bars_0.Close;
            }
        }

        public IList<string> DataSetSymbols
        {
            get
            {
                if (this.dataSource_0 != null)
                {
                    return this.dataSource_0.Symbols;
                }
                return new List<string> { this.bars_0.Symbol };
            }
        }

        public IList<DateTime> Date
        {
            get
            {
                return this.bars_0.Date;
            }
        }

        public DataSeries High
        {
            get
            {
                return this.bars_0.High;
            }
        }

        public bool IsLastPositionActive
        {
            get
            {
                return ((this.Positions.Count > 0) && this.Positions[this.Positions.Count - 1].Active);
            }
        }

        public bool IsStreaming
        {
            get
            {
                return this.tradingSystemExecutor.IsStreaming;
            }
        }

        public Position LastActivePosition
        {
            get
            {
                for (int i = this.Positions.Count - 1; i >= 0; i--)
                {
                    if (this.Positions[i].Active)
                    {
                        return this.Positions[i];
                    }
                }
                return null;
            }
        }

        public Position LastPosition
        {
            get
            {
                if (this.Positions.Count == 0)
                {
                    return null;
                }
                return this.Positions[this.Positions.Count - 1];
            }
        }

        public DataSeries Low
        {
            get
            {
                return this.bars_0.Low;
            }
        }

        public double MarketPosition
        {
            get
            {
                double num = 0.0;
                foreach (Position position in this.Positions)
                {
                    if (position.Active)
                    {
                        if (position.PositionType == PositionType.Long)
                        {
                            num += position.Shares;
                        }
                        else
                        {
                            num -= position.Shares;
                        }
                    }
                }
                return num;
            }
        }

        public DataSeries Open
        {
            get
            {
                return this.bars_0.Open;
            }
        }

        public double OverrideShareSize
        {
            get
            {
                return this.tradingSystemExecutor.OverrideShareSize;
            }
            set
            {
                this.tradingSystemExecutor.OverrideShareSize = value;
            }
        }

        public IList<StrategyParameter> Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        public string ParameterString
        {
            get
            {
                if (this.Parameters.Count == 0)
                {
                    return "(None)";
                }
                StringBuilder builder = new StringBuilder();
                builder.Append("(");
                for (int i = 0; i < this.Parameters.Count; i++)
                {
                    builder.Append(this.Parameters[i].Value);
                    if (i != (this.Parameters.Count - 1))
                    {
                        builder.Append(",");
                    }
                }
                builder.Append(")");
                return builder.ToString();
            }
        }

        public IList<Position> Positions
        {
            get
            {
                return this.tradingSystemExecutor.CurrentPositions;
            }
        }

        public ChartPane PricePane
        {
            get
            {
                if (this.chartRenderer != null)
                {
                    return this.chartRenderer.PricePane;
                }
                if (this.chartPane_0 == null)
                {
                    this.chartPane_0 = new ChartPane();
                }
                return this.chartPane_0;
            }
        }

        public ChartRenderer Renderer
        {
            get
            {
                return this.chartRenderer;
            }
            set
            {
                this.chartRenderer = value;
            }
        }

        public double RiskStopLevel
        {
            get
            {
                return this.tradingSystemExecutor.RiskStopLevel;
            }
            set
            {
                this.tradingSystemExecutor.RiskStopLevel = value;
            }
        }

        public WealthLab.Strategy Strategy
        {
            get
            {
                return this.tradingSystemExecutor.Strategy;
            }
        }

        public string StrategyName
        {
            get
            {
                return this.tradingSystemExecutor.StrategyName;
            }
        }

        public int StrategyWindowID
        {
            [CompilerGenerated]
            get
            {
                return this.strategyWindowID;
            }
            [CompilerGenerated]
            set
            {
                this.strategyWindowID = value;
            }
        }

        public DataSeries Volume
        {
            get
            {
                return this.bars_0.Volume;
            }
        }

        public ChartPane VolumePane
        {
            get
            {
                if (this.chartRenderer != null)
                {
                    return this.chartRenderer.VolumePane;
                }
                if (this.chartPane_0 == null)
                {
                    this.chartPane_0 = new ChartPane();
                }
                return this.chartPane_0;
            }
        }
    }
}

