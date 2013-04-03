namespace WealthLab.Visualizers
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.Indicators;

    public class ExtendedScorecard : StrategyScorecard
    {
        private string[] string_0 = new string[] { "Net Profit", "Profit per Bar", "Trades", "Winning %", "Avg Profit %", "Avg Bars Held", "Max Drawdown", "Profit Factor", "Recovery Factor", "Payoff Ratio", "Ulcer Index", "Luck Coefficient", "Pessimistic RR" };
        private string[] string_1 = new string[] { "Net Profit", "APR %", "Trades", "Winning %", "Avg Profit %", "Avg Bars Held", "Max Drawdown", "Profit Factor", "Recovery Factor", "Payoff Ratio", "Ulcer Index", "Luck Coefficient", "Pessimistic RR", "Sharpe Ratio" };
        private string[] string_2 = new string[] { "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
        private string[] string_3 = new string[] { "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };

        public override string GetFormatCode(string metric)
        {
            if (metric == "Trades")
            {
                return "N";
            }
            return "N2";
        }

        public override void PopulateScorecard(ListViewItem listViewItem_0, SystemPerformance performance)
        {
            SystemResults results = performance.Results;
            int count = results.Positions.Count;
            listViewItem_0.SubItems.Add(results.NetProfit.ToString("N2"));
            if (performance.PositionSize.RawProfitMode)
            {
                listViewItem_0.SubItems.Add(results.ProfitPerBar.ToString("N2"));
            }
            else
            {
                listViewItem_0.SubItems.Add(results.APR.ToString("N2"));
            }
            listViewItem_0.SubItems.Add(count.ToString("N0"));
            double num14 = 0.0;
            double num13 = 0.0;
            double num11 = 0.0;
            double num38 = 0.0;
            double num39 = 0.0;
            double num7 = 0.0;
            double num8 = 0.0;
            double d = 0.0;
            double num33 = 0.0;
            if (count > 0)
            {
                double num12 = 0.0;
                double num15 = 0.0;
                double num40 = 0.0;
                double num10 = 0.0;
                foreach (Position position2 in results.Positions)
                {
                    num12 += position2.NetProfitPercent;
                    num10 += position2.BarsHeld;
                    if (position2.NetProfit > 0.0)
                    {
                        d++;
                        num38 += position2.NetProfit;
                        num15 += position2.NetProfitPercent;
                    }
                    else
                    {
                        num33++;
                        num39 += position2.NetProfit;
                        num40 += position2.NetProfitPercent;
                    }
                }
                num13 = num12 / ((double) count);
                if (d > 0.0)
                {
                    num14 = (d * 100.0) / ((double) count);
                    num7 = num15 / d;
                }
                if (num33 > 0.0)
                {
                    num8 = num40 / num33;
                }
                num11 = num10 / ((double) count);
            }
            listViewItem_0.SubItems.Add(num14.ToString("N2"));
            listViewItem_0.SubItems.Add(num13.ToString("N2"));
            listViewItem_0.SubItems.Add(num11.ToString("N2"));
            double num26 = 0.0;
            double num23 = 0.0;
            DataSeries equityCurve = results.EquityCurve;
            if (equityCurve.Count > 0)
            {
                double minValue = double.MinValue;
                double num27 = 0.0;
                double num22 = 0.0;
                for (int i = 0; i < equityCurve.Count; i++)
                {
                    if (equityCurve[i] > minValue)
                    {
                        minValue = equityCurve[i];
                    }
                    else
                    {
                        if (minValue != 0.0)
                        {
                            num22 += Math.Pow(100.0 * ((equityCurve[i] / minValue) - 1.0), 2.0);
                        }
                        if (equityCurve[i] < minValue)
                        {
                            num27 = -(minValue - equityCurve[i]);
                            if (num27 < num26)
                            {
                                num26 = num27;
                            }
                        }
                    }
                }
                num23 = Math.Sqrt(num22 / ((double) equityCurve.Count));
            }
            listViewItem_0.SubItems.Add(num26.ToString("N2"));
            double num37 = 0.0;
            if (count > 0)
            {
                num37 = num38 / Math.Abs(num39);
            }
            listViewItem_0.SubItems.Add(num37.ToString("N2"));
            double num28 = 0.0;
            if (results.NetProfit > 0.0)
            {
                num28 = Math.Abs((double) (results.NetProfit / num26));
            }
            listViewItem_0.SubItems.Add(num28.ToString("N2"));
            double num9 = 0.0;
            if ((count > 0) && (num8 != 0.0))
            {
                num9 = Math.Abs((double) (num7 / num8));
            }
            listViewItem_0.SubItems.Add(num9.ToString("N2"));
            listViewItem_0.SubItems.Add(num23.ToString("N2"));
            double num4 = 0.0;
            if (num7 > 0.0)
            {
                double num16 = double.MinValue;
                foreach (Position position in results.Positions)
                {
                    double netProfitPercent = position.NetProfitPercent;
                    if (netProfitPercent > num16)
                    {
                        num16 = netProfitPercent;
                    }
                }
                num4 = num16 / num7;
            }
            listViewItem_0.SubItems.Add(num4.ToString("N2"));
            double num5 = 0.0;
            if (((count > 0) && (d > 0.0)) && (num33 > 0.0))
            {
                try
                {
                    double num30 = d - Math.Sqrt(d);
                    double num31 = num30 / ((double) count);
                    double num32 = num31 * num7;
                    double num34 = num33 + Math.Sqrt(num33);
                    double num35 = num34 / ((double) count);
                    double num36 = num35 * Math.Abs(num8);
                    if (num36 != 0.0)
                    {
                        num5 = num32 / num36;
                    }
                }
                catch
                {
                    num5 = 0.0;
                }
            }
            listViewItem_0.SubItems.Add(num5.ToString("N2"));
            if (!performance.PositionSize.RawProfitMode)
            {
                double num43 = 0.0;
                TimeSpan span = results.EquityCurve.Date[results.EquityCurve.Count - 1] - results.EquityCurve.Date[0];
                if ((span.Days > 0x1f) && (results.Positions.Count > 0))
                {
                    int num17;
                    double num19;
                    double num20;
                    DataSeries series = new DataSeries("Returns");
                    double num18 = results.EquityCurve[0];
                    DateTime time = results.EquityCurve.Date[0];
                    for (num17 = 1; num17 < (results.EquityCurve.Count - 1); num17++)
                    {
                        DateTime time2 = results.EquityCurve.Date[num17];
                        if ((time2.Month != time.Month) || (time2.Year != time.Year))
                        {
                            num19 = results.EquityCurve[num17 - 1] - num18;
                            num20 = (num19 * 100.0) / num18;
                            series.Add(num20, time);
                            num18 = results.EquityCurve[num17 - 1];
                            time = time2;
                        }
                    }
                    num17 = results.EquityCurve.Count - 1;
                    num19 = results.EquityCurve[num17] - num18;
                    num20 = (num19 * 100.0) / num18;
                    series.Add(num20, time);
                    double num41 = SMA.Value(series.Count - 1, series, series.Count) * 12.0;
                    double num42 = StdDev.Value(series.Count - 1, series, series.Count, StdDevCalculation.Population) * Math.Sqrt(12.0);
                    num43 = (num41 - performance.CashReturnRate) / num42;
                }
                listViewItem_0.SubItems.Add(num43.ToString("N2"));
            }
        }

        public override IList<string> ColumnHeadersPortfolioSim
        {
            get
            {
                return this.string_1;
            }
        }

        public override IList<string> ColumnHeadersRawProfit
        {
            get
            {
                return this.string_0;
            }
        }

        public override IList<string> ColumnTypesPortfolioSim
        {
            get
            {
                return this.string_3;
            }
        }

        public override IList<string> ColumnTypesRawProfit
        {
            get
            {
                return this.string_2;
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Extended Scorecard";
            }
        }
    }
}

