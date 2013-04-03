namespace WealthLab.Visualizers
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using WealthLab;

    public class BasicScorecard : StrategyScorecard
    {
        private string[] string_0 = new string[] { "Net Profit", "Profit per Bar", "Trades", "Winning %", "Avg Profit %", "Avg Bars Held", "Max Drawdown", "Profit Factor", "Recovery Factor", "Payoff Ratio" };
        private string[] string_1 = new string[] { "Net Profit", "APR %", "Trades", "Winning %", "Avg Profit %", "Avg Bars Held", "Max Drawdown", "Profit Factor", "Recovery Factor", "Payoff Ratio" };
        private string[] string_2 = new string[] { "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
        private string[] string_3 = new string[] { "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };

        public override string GetFormatCode(string metric)
        {
            if (metric == "Trades")
            {
                return "N0";
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
            double num12 = 0.0;
            double num18 = 0.0;
            double num22 = 0.0;
            double num20 = 0.0;
            double num16 = 0.0;
            double num9 = 0.0;
            if (count > 0)
            {
                double num13 = 0.0;
                double num19 = 0.0;
                double num11 = 0.0;
                double num15 = 0.0;
                double num21 = 0.0;
                double num17 = 0.0;
                foreach (Position position in results.Positions)
                {
                    num11 += position.NetProfitPercent;
                    num17 += position.BarsHeld;
                    if (position.NetProfit > 0.0)
                    {
                        num13++;
                        num22 += position.NetProfit;
                        num15 += position.NetProfitPercent;
                    }
                    else
                    {
                        num19++;
                        num20 += position.NetProfit;
                        num21 += position.NetProfitPercent;
                    }
                }
                num12 = num11 / ((double) count);
                if (num13 > 0.0)
                {
                    num14 = (num13 * 100.0) / ((double) count);
                    num16 = num15 / num13;
                }
                if (num19 > 0.0)
                {
                    num9 = num21 / num19;
                }
                num18 = num17 / ((double) count);
            }
            listViewItem_0.SubItems.Add(num14.ToString("N2"));
            listViewItem_0.SubItems.Add(num12.ToString("N2"));
            listViewItem_0.SubItems.Add(num18.ToString("N2"));
            double num7 = 0.0;
            DataSeries equityCurve = results.EquityCurve;
            if (equityCurve.Count > 0)
            {
                double minValue = double.MinValue;
                double num23 = 0.0;
                for (int i = 0; i < equityCurve.Count; i++)
                {
                    if (equityCurve[i] > minValue)
                    {
                        minValue = equityCurve[i];
                    }
                    else if (equityCurve[i] < minValue)
                    {
                        num23 = -(minValue - equityCurve[i]);
                        if (num23 < num7)
                        {
                            num7 = num23;
                        }
                    }
                }
            }
            listViewItem_0.SubItems.Add(num7.ToString("N2"));
            double num5 = 0.0;
            if (count > 0)
            {
                num5 = num22 / Math.Abs(num20);
            }
            listViewItem_0.SubItems.Add(num5.ToString("N2"));
            double num6 = 0.0;
            if (results.NetProfit > 0.0)
            {
                num6 = Math.Abs((double) (results.NetProfit / num7));
            }
            listViewItem_0.SubItems.Add(num6.ToString("N2"));
            double num8 = 0.0;
            if ((count > 0) && (num9 != 0.0))
            {
                num8 = Math.Abs((double) (num16 / num9));
            }
            listViewItem_0.SubItems.Add(num8.ToString("N2"));
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
                return "Basic Scorecard";
            }
        }
    }
}

