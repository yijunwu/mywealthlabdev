namespace WealthLab.Visualizers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.Indicators;

    [ToolboxItem(false)]
    public class PVReport : UserControl, IPerformanceVisualizer
    {
        private ToolStripComboBox cmbChildStrategy;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private Dictionary<FontStyle, Font> dictionary_0 = new Dictionary<FontStyle, Font>();
        private IContainer icontainer_0;
        private int int_0;
        private int int_1;
        private IVisualizerHost ivisualizerHost_0;
        private Label lblDisclaimer;
        private ToolStripLabel lblPeriod;
        private ToolStripLabel lblRangeValue;
        private ToolStripLabel lblReport;
        private ListView lvReport;
        private ToolStripMenuItem mniCopy;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrintAll;
        private PageSettings pageSettings_0 = new PageSettings();
        private ContextMenuStrip popup;
        private static string string_0 = Resources.Descriptions;
        private SystemPerformance systemPerformance_0;
        private ToolStrip toolbar;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private TextBox txtDescription;

        public PVReport()
        {
            this.InitializeComponent();
        }

        protected void AddBlankLine(string label)
        {
            this.AddBlankLine(label, Color.Empty, Color.Empty, FontStyle.Regular);
        }

        protected void AddBlankLine(string label, Color backColor, Color labelFontColor, FontStyle labelFontStyle)
        {
            this.method_2(label, "", backColor, labelFontColor, Color.Empty, labelFontStyle, FontStyle.Regular);
        }

        protected void AddCurrencyValue(string label, double value)
        {
            this.AddCurrencyValue(label, value, Color.Empty, Color.Empty, Color.Empty, FontStyle.Regular, FontStyle.Regular);
        }

        protected void AddCurrencyValue(string label, double value, Color backColor, Color labelFontColor, Color itemFontColor, FontStyle labelFontStyle, FontStyle itemFontStyle)
        {
            this.method_2(label, value.ToString("C"), backColor, labelFontColor, itemFontColor, labelFontStyle, itemFontStyle);
        }

        protected void AddDateTimeValue(string label, DateTime value)
        {
            this.AddDateTimeValue(label, value, Color.Empty, Color.Empty, Color.Empty, FontStyle.Regular, FontStyle.Regular);
        }

        protected void AddDateTimeValue(string label, DateTime value, Color backColor, Color labelFontColor, Color itemFontColor, FontStyle labelFontStyle, FontStyle itemFontStyle)
        {
            this.method_2(label, value.ToShortDateString() + " " + value.ToShortTimeString(), backColor, labelFontColor, itemFontColor, labelFontStyle, itemFontStyle);
        }

        protected void AddDateValue(string label, DateTime value)
        {
            this.AddDateValue(label, value, Color.Empty, Color.Empty, Color.Empty, FontStyle.Regular, FontStyle.Regular);
        }

        protected void AddDateValue(string label, DateTime value, Color backColor, Color labelFontColor, Color itemFontColor, FontStyle labelFontStyle, FontStyle itemFontStyle)
        {
            this.method_2(label, value.ToShortDateString(), backColor, labelFontColor, itemFontColor, labelFontStyle, itemFontStyle);
        }

        protected void AddNumericValue(string label, double value, int decimalPlaces)
        {
            this.AddNumericValue(label, value, decimalPlaces, Color.Empty, Color.Empty, Color.Empty, FontStyle.Regular, FontStyle.Regular);
        }

        protected void AddNumericValue(string label, double value, int decimalPlaces, Color backColor, Color labelFontColor, Color itemFontColor, FontStyle labelFontStyle, FontStyle itemFontStyle)
        {
            this.method_2(label, value.ToString("N" + decimalPlaces), backColor, labelFontColor, itemFontColor, labelFontStyle, itemFontStyle);
        }

        protected void AddPercentValue(string label, double value, int decimalPlaces)
        {
            this.AddPercentValue(label, value, decimalPlaces, Color.Empty, Color.Empty, Color.Empty, FontStyle.Regular, FontStyle.Regular);
        }

        protected void AddPercentValue(string label, double value, int decimalPlaces, Color backColor, Color labelFontColor, Color itemFontColor, FontStyle labelFontStyle, FontStyle itemFontStyle)
        {
            this.method_2(label, value.ToString("N" + decimalPlaces) + "%", backColor, labelFontColor, itemFontColor, labelFontStyle, itemFontStyle);
        }

        protected void AddStringValue(string label, string value)
        {
            this.AddStringValue(label, value, Color.Empty, Color.Empty, Color.Empty, FontStyle.Regular, FontStyle.Regular);
        }

        protected void AddStringValue(string label, string value, Color backColor, Color labelFontColor, Color itemFontColor, FontStyle labelFontStyle, FontStyle itemFontStyle)
        {
            this.method_2(label, value, backColor, labelFontColor, itemFontColor, labelFontStyle, itemFontStyle);
        }

        protected void AddTimeValue(string label, DateTime value)
        {
            this.AddTimeValue(label, value, Color.Empty, Color.Empty, Color.Empty, FontStyle.Regular, FontStyle.Regular);
        }

        protected void AddTimeValue(string label, DateTime value, Color backColor, Color labelFontColor, Color itemFontColor, FontStyle labelFontStyle, FontStyle itemFontStyle)
        {
            this.method_2(label, value.ToShortTimeString(), backColor, labelFontColor, itemFontColor, labelFontStyle, itemFontStyle);
        }

        private void cmbChildStrategy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbChildStrategy.SelectedIndex == 0)
            {
                this.method_1(this.systemPerformance_0);
            }
            else
            {
                CombinedStrategyInfo selectedItem = (CombinedStrategyInfo) this.cmbChildStrategy.SelectedItem;
                this.method_1(this.systemPerformance_0.GenerateChildStrategyPerformance(selectedItem, this.ivisualizerHost_0.GetExecutor()));
            }
        }

        public void CopyToClipboard()
        {
            this.ivisualizerHost_0.CopyListViewToClipboard(this.lvReport);
        }

        public void CreateVisualization(SystemPerformance performance, IVisualizerHost visHost)
        {
            this.ivisualizerHost_0 = visHost;
            this.systemPerformance_0 = performance;
            this.systemPerformance_0.Signal += new EventHandler<EventArgs>(this.method_0);
            this.method_1(performance);
            if (performance.Strategy.StrategyType == StrategyType.CombinedStrategy)
            {
                this.cmbChildStrategy.Visible = true;
                this.cmbChildStrategy.Items.Clear();
                this.cmbChildStrategy.Items.Add("Strategies in Aggregate");
                foreach (CombinedStrategyInfo info in performance.Strategy.CombinedStrategyChildren)
                {
                    this.cmbChildStrategy.Items.Add(info);
                }
                this.cmbChildStrategy.SelectedIndex = 0;
            }
            else
            {
                this.cmbChildStrategy.Visible = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public void EnableControls(bool enable)
        {
        }

        protected virtual void GenerateReport(SystemPerformance performance, SystemResults results)
        {
            if ((results.EquityCurve != null) && (results.EquityCurve.Count != 0))
            {
                double num5;
                double num6;
                double num7;
                double num13;
                double num14;
                double num17;
                double num19;
                double num20;
                bool rawProfitMode = performance.PositionSize.RawProfitMode;
                int count = results.Positions.Count;
                double startingCapital = performance.PositionSize.StartingCapital;
                double num34 = performance.PositionSize.StartingCapital + results.NetProfit;
                double num = 0.0;
                double num2 = 0.0;
                TimeSpan span = results.EquityCurve.Date[results.EquityCurve.Count - 1] - results.EquityCurve.Date[0];
                if (!rawProfitMode)
                {
                    this.AddCurrencyValue("Starting Capital", startingCapital);
                    this.AddCurrencyValue("Ending Capital", num34);
                }
                this.AddCurrencyValue("Net Profit", results.NetProfit, Color.Empty, Color.Empty, this.GetItemColor(results.NetProfit), FontStyle.Bold, FontStyle.Regular);
                if (rawProfitMode)
                {
                    this.AddCurrencyValue("Profit per Bar", results.ProfitPerBar, Color.Empty, Color.Empty, this.GetItemColor(results.NetProfit), FontStyle.Bold, FontStyle.Regular);
                }
                else
                {
                    this.AddPercentValue("Net Profit %", (results.NetProfit * 100.0) / startingCapital, 2);
                    num = (Math.Pow(num34 / startingCapital, 365.25 / ((double) span.Days)) - 1.0) * 100.0;
                    this.AddPercentValue("Annualized Gain %", num, 2, Color.Empty, Color.Empty, Color.Empty, FontStyle.Bold, FontStyle.Regular);
                }
                if (!rawProfitMode)
                {
                    double num23 = 0.0;
                    double num22 = 0.0;
                    for (int i = 0; i < results.EquityCurve.Count; i++)
                    {
                        num23 += results.EquityCurve[i];
                        num22 += results.EquityCurve[i] - results.CashCurve[i];
                    }
                    num2 = (num22 * 100.0) / num23;
                    this.AddPercentValue("Exposure", num2, 2);
                }
                this.AddCurrencyValue("Total Commission", -results.TotalCommission, Color.Empty, Color.Empty, this.GetItemColor(-results.TotalCommission), FontStyle.Regular, FontStyle.Regular);
                if (!rawProfitMode)
                {
                    this.AddCurrencyValue("Return on Cash", results.CashReturn, Color.Empty, Color.Empty, this.GetItemColor(results.CashReturn), FontStyle.Regular, FontStyle.Regular);
                    this.AddCurrencyValue("Margin Interest Paid", results.MarginInterest, Color.Empty, Color.Empty, this.GetItemColor(results.MarginInterest), FontStyle.Regular, FontStyle.Regular);
                    this.AddCurrencyValue("Dividends Received", results.DividendsPaid, Color.Empty, Color.Empty, this.GetItemColor(results.DividendsPaid), FontStyle.Regular, FontStyle.Regular);
                }
                this.AddBlankLine("");
                this.AddNumericValue("Number of Trades", (double) count, 0, Color.Gainsboro, Color.Empty, Color.Empty, FontStyle.Bold, FontStyle.Regular);
                if (count > 0)
                {
                    num5 = results.NetProfit / ((double) count);
                    num6 = 0.0;
                    num7 = 0.0;
                    foreach (Position position in results.Positions)
                    {
                        num6 += position.NetProfitPercent;
                        num7 += position.BarsHeld;
                    }
                    num13 = num6 / ((double) count);
                    num20 = num7 / ((double) count);
                }
                else
                {
                    num5 = 0.0;
                    num13 = 0.0;
                    num20 = 0.0;
                }
                this.AddCurrencyValue("Average Profit", num5, Color.Empty, Color.Empty, this.GetItemColor(num5), FontStyle.Regular, FontStyle.Regular);
                this.AddPercentValue("Average Profit %", num13, 2, Color.Empty, Color.Empty, this.GetItemColor(num13), FontStyle.Regular, FontStyle.Regular);
                this.AddNumericValue("Average Bars Held", num20, 2);
                this.AddBlankLine("");
                int num25 = 0;
                int num28 = 0;
                int num27 = 0;
                num6 = 0.0;
                num7 = 0.0;
                double num26 = 0.0;
                foreach (Position position3 in results.Positions)
                {
                    if (position3.NetProfit > 0.0)
                    {
                        num25++;
                        num28++;
                        if (num28 > num27)
                        {
                            num27 = num28;
                        }
                        num26 += position3.NetProfit;
                        num6 += position3.NetProfitPercent;
                        num7 += position3.BarsHeld;
                    }
                    else
                    {
                        num28 = 0;
                    }
                }
                if (num25 > 0)
                {
                    num17 = (num25 * 100.0) / ((double) count);
                    num5 = num26 / ((double) num25);
                    num13 = num6 / ((double) num25);
                    num20 = num7 / ((double) num25);
                }
                else
                {
                    num17 = 0.0;
                    num5 = 0.0;
                    num13 = 0.0;
                    num20 = 0.0;
                }
                this.AddNumericValue("Winning Trades", (double) num25, 0, Color.Gainsboro, Color.Empty, Color.Empty, FontStyle.Bold, FontStyle.Regular);
                this.AddPercentValue("Win Rate", num17, 2);
                this.AddCurrencyValue("Gross Profit", num26, Color.Empty, Color.Empty, this.GetItemColor(num26), FontStyle.Regular, FontStyle.Regular);
                this.AddCurrencyValue("Average Profit", num5, Color.Empty, Color.Empty, this.GetItemColor(num5), FontStyle.Regular, FontStyle.Regular);
                this.AddPercentValue("Average Profit %", num13, 2, Color.Empty, Color.Empty, this.GetItemColor(num13), FontStyle.Regular, FontStyle.Regular);
                this.AddNumericValue("Average Bars Held", num20, 2);
                this.AddNumericValue("Max Consecutive Winners", (double) num27, 0);
                this.AddBlankLine("");
                int num16 = 0;
                num28 = 0;
                num27 = 0;
                num6 = 0.0;
                num7 = 0.0;
                double num18 = 0.0;
                foreach (Position position2 in results.Positions)
                {
                    if (position2.NetProfit <= 0.0)
                    {
                        num16++;
                        num28++;
                        if (num28 > num27)
                        {
                            num27 = num28;
                        }
                        num18 += position2.NetProfit;
                        num6 += position2.NetProfitPercent;
                        num7 += position2.BarsHeld;
                    }
                    else
                    {
                        num28 = 0;
                    }
                }
                if (num16 > 0)
                {
                    num17 = (num16 * 100.0) / ((double) count);
                    num19 = num18 / ((double) num16);
                    num14 = num6 / ((double) num16);
                    num20 = num7 / ((double) num16);
                }
                else
                {
                    num17 = 0.0;
                    num19 = 0.0;
                    num14 = 0.0;
                    num20 = 0.0;
                }
                this.AddNumericValue("Losing Trades", (double) num16, 0, Color.Gainsboro, Color.Empty, Color.Empty, FontStyle.Bold, FontStyle.Regular);
                this.AddPercentValue("Loss Rate", num17, 2);
                this.AddCurrencyValue("Gross Loss", num18, Color.Empty, Color.Empty, this.GetItemColor(num18), FontStyle.Regular, FontStyle.Regular);
                this.AddCurrencyValue("Average Loss", num19, Color.Empty, Color.Empty, this.GetItemColor(num19), FontStyle.Regular, FontStyle.Regular);
                this.AddPercentValue("Average Loss %", num14, 2, Color.Empty, Color.Empty, this.GetItemColor(num14), FontStyle.Regular, FontStyle.Regular);
                this.AddNumericValue("Average Bars Held", num20, 2);
                this.AddNumericValue("Max Consecutive Losses", (double) num27, 0);
                this.AddBlankLine("");
                double minValue = double.MinValue;
                double num39 = 0.0;
                double num12 = 0.0;
                DateTime now = DateTime.Now;
                DateTime time = DateTime.Now;
                DataSeries equityCurve = results.EquityCurve;
                if (equityCurve.Count > 0)
                {
                    now = equityCurve.Date[0];
                    for (int j = 0; j < equityCurve.Count; j++)
                    {
                        if (equityCurve[j] > minValue)
                        {
                            minValue = equityCurve[j];
                        }
                        if (equityCurve[j] < minValue)
                        {
                            double num40 = -(minValue - equityCurve[j]);
                            if (num40 < num39)
                            {
                                num39 = num40;
                                now = equityCurve.Date[j];
                            }
                            double num11 = ((equityCurve[j] - minValue) * 100.0) / minValue;
                            if (num11 < num12)
                            {
                                num12 = num11;
                                time = equityCurve.Date[j];
                            }
                        }
                    }
                    this.AddCurrencyValue("Maximum Drawdown", num39, Color.Empty, Color.Empty, this.GetItemColor(num39), FontStyle.Regular, FontStyle.Regular);
                    this.AddDateValue("Maximum Drawdown Date", now);
                    if (!rawProfitMode)
                    {
                        this.AddPercentValue("Maximum Drawdown %", num12, 2, Color.Empty, Color.Empty, this.GetItemColor(num12), FontStyle.Regular, FontStyle.Regular);
                        this.AddDateValue("Maximum Drawdown % Date", time);
                    }
                    this.AddBlankLine("");
                }
                if (!rawProfitMode)
                {
                    double num8 = 0.0;
                    if (num2 > 0.0)
                    {
                        double num3 = (num * 100.0) / num2;
                        if (num3 > 0.0)
                        {
                            num8 = num3 * (1.0 + (num12 / 100.0));
                        }
                        else
                        {
                            num8 = num3 * (1.0 + Math.Abs((double) (num12 / 100.0)));
                        }
                    }
                    this.AddNumericValue("Wealth-Lab Score", num8, 2, Color.Empty, Color.Empty, this.GetItemColor(num8), FontStyle.Bold, FontStyle.Regular);
                    double num24 = 0.0;
                    if ((span.Days > 0x1f) && (results.Positions.Count > 0))
                    {
                        int num29;
                        double num31;
                        double num32;
                        DataSeries series2 = new DataSeries("Returns");
                        double num30 = results.EquityCurve[0];
                        DateTime time2 = results.EquityCurve.Date[0];
                        for (num29 = 1; num29 < (results.EquityCurve.Count - 1); num29++)
                        {
                            DateTime time3 = results.EquityCurve.Date[num29];
                            if ((time3.Month != time2.Month) || (time3.Year != time2.Year))
                            {
                                num31 = results.EquityCurve[num29 - 1] - num30;
                                num32 = (num31 * 100.0) / num30;
                                series2.Add(num32, time2);
                                num30 = results.EquityCurve[num29 - 1];
                                time2 = time3;
                            }
                        }
                        num29 = results.EquityCurve.Count - 1;
                        num31 = results.EquityCurve[num29] - num30;
                        num32 = (num31 * 100.0) / num30;
                        series2.Add(num32, time2);
                        double num35 = SMA.Value(series2.Count - 1, series2, series2.Count) * 12.0;
                        double num36 = StdDev.Value(series2.Count - 1, series2, series2.Count, StdDevCalculation.Population) * Math.Sqrt(12.0);
                        num24 = (num35 - performance.CashReturnRate) / num36;
                    }
                    this.AddNumericValue("Sharpe Ratio", num24, 2, Color.Empty, Color.Empty, this.GetItemColor(num24), FontStyle.Regular, FontStyle.Regular);
                }
                double num37 = 0.0;
                if (count > 0)
                {
                    num37 = num26 / Math.Abs(num18);
                }
                this.AddNumericValue("Profit Factor", num37, 2);
                double num38 = 0.0;
                if (results.NetProfit > 0.0)
                {
                    num38 = Math.Abs((double) (results.NetProfit / num39));
                }
                this.AddNumericValue("Recovery Factor", num38, 2);
                double num15 = 0.0;
                if ((count > 0) && (num14 != 0.0))
                {
                    num15 = Math.Abs((double) (num13 / num14));
                }
                this.AddNumericValue("Payoff Ratio", num15, 2);
            }
        }

        protected Color GetItemColor(double value)
        {
            if (value == 0.0)
            {
                return this.ForeColor;
            }
            if (value > 0.0)
            {
                return Color.Blue;
            }
            return Color.Red;
        }

        protected virtual string GetItemDescription(string itemName)
        {
            int index = string_0.IndexOf(itemName + "=");
            string str = string_0.Substring((index + itemName.Length) + 1);
            index = str.IndexOf('\n');
            return str.Substring(0, index - 1);
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PVReport));
            this.txtDescription = new TextBox();
            this.toolbar = new ToolStrip();
            this.lblReport = new ToolStripLabel();
            this.cmbChildStrategy = new ToolStripComboBox();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.lblPeriod = new ToolStripLabel();
            this.lblRangeValue = new ToolStripLabel();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.lvReport = new ListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.popup = new ContextMenuStrip(this.icontainer_0);
            this.mniCopy = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.mniPrintAll = new ToolStripMenuItem();
            this.lblDisclaimer = new Label();
            this.toolbar.SuspendLayout();
            this.popup.SuspendLayout();
            base.SuspendLayout();
            this.txtDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.txtDescription.Location = new Point(0, 0x109);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.Size = new Size(0x28e, 0x44);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.Text = "Click an item above to read a description here.";
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.lblReport, this.cmbChildStrategy, this.toolStripSeparator1, this.lblPeriod, this.lblRangeValue, this.toolStripSeparator2 });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x28e, 0x19);
            this.toolbar.TabIndex = 2;
            this.toolbar.Text = "toolStrip1";
            this.lblReport.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new Size(0xaf, 0x16);
            this.lblReport.Text = "Backtest Performance Report";
            this.cmbChildStrategy.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbChildStrategy.Items.AddRange(new object[] { "Strategies in Aggregate" });
            this.cmbChildStrategy.Name = "cmbChildStrategy";
            this.cmbChildStrategy.Size = new Size(140, 0x19);
            this.cmbChildStrategy.Visible = false;
            this.cmbChildStrategy.SelectedIndexChanged += new EventHandler(this.cmbChildStrategy_SelectedIndexChanged);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new Size(0x2a, 0x16);
            this.lblPeriod.Text = "Range:";
            this.lblRangeValue.ForeColor = SystemColors.ActiveCaption;
            this.lblRangeValue.Name = "lblRangeValue";
            this.lblRangeValue.Size = new Size(13, 0x16);
            this.lblRangeValue.Text = "  ";
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.lvReport.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvReport.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_1, this.columnHeader_2, this.columnHeader_3, this.columnHeader_4 });
            this.lvReport.ContextMenuStrip = this.popup;
            this.lvReport.FullRowSelect = true;
            this.lvReport.HideSelection = false;
            this.lvReport.Location = new Point(0, 0x19);
            this.lvReport.MultiSelect = false;
            this.lvReport.Name = "lvReport";
            this.lvReport.Size = new Size(0x28e, 0xf2);
            this.lvReport.TabIndex = 3;
            this.lvReport.UseCompatibleStateImageBehavior = false;
            this.lvReport.View = View.Details;
            this.lvReport.SelectedIndexChanged += new EventHandler(this.lvReport_SelectedIndexChanged);
            this.columnHeader_0.Text = "";
            this.columnHeader_0.Width = 160;
            this.columnHeader_1.Text = "All Trades";
            this.columnHeader_1.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_1.Width = 100;
            this.columnHeader_2.Text = "Long Trades";
            this.columnHeader_2.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_2.Width = 100;
            this.columnHeader_3.Text = "Short Trades";
            this.columnHeader_3.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_3.Width = 100;
            this.columnHeader_4.Text = "Buy & Hold";
            this.columnHeader_4.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_4.Width = 100;
            this.popup.Items.AddRange(new ToolStripItem[] { this.mniCopy, this.mniPrint, this.mniPrintAll });
            this.popup.Name = "popup";
            this.popup.Size = new Size(0x7a, 70);
            this.mniCopy.Image = (Image) manager.GetObject("mniCopy.Image");
            this.mniCopy.ImageTransparentColor = Color.Fuchsia;
            this.mniCopy.Name = "mniCopy";
            this.mniCopy.Size = new Size(0x79, 0x16);
            this.mniCopy.Text = "Copy";
            this.mniCopy.ToolTipText = "Copy data to the clipboard";
            this.mniCopy.Click += new EventHandler(this.mniCopy_Click);
            this.mniPrint.Image = (Image) manager.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0x79, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.mniPrintAll.Name = "mniPrintAll";
            this.mniPrintAll.Size = new Size(0x79, 0x16);
            this.mniPrintAll.Text = "Print All";
            this.mniPrintAll.ToolTipText = "Print content from all tabs";
            this.mniPrintAll.Click += new EventHandler(this.mniPrintAll_Click);
            this.lblDisclaimer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.lblDisclaimer.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblDisclaimer.Location = new Point(-2, 0x150);
            this.lblDisclaimer.Name = "lblDisclaimer";
            this.lblDisclaimer.Size = new Size(0x290, 0x35);
            this.lblDisclaimer.TabIndex = 9;
            this.lblDisclaimer.Text = manager.GetString("lblDisclaimer.Text");
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Window;
            this.ContextMenuStrip = this.popup;
            base.Controls.Add(this.txtDescription);
            base.Controls.Add(this.lblDisclaimer);
            base.Controls.Add(this.lvReport);
            base.Controls.Add(this.toolbar);
            base.Name = "PVReport";
            base.Size = new Size(0x28e, 0x185);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.popup.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lvReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lvReport.SelectedItems.Count == 1)
            {
                ListViewItem item = this.lvReport.SelectedItems[0];
                string itemName = item.Text.Trim();
                if (itemName == "")
                {
                    this.txtDescription.Text = "";
                }
                else
                {
                    this.txtDescription.Text = this.GetItemDescription(itemName);
                }
            }
        }

        private void method_0(object sender, EventArgs e)
        {
            CombinedStrategyInfo info = new CombinedStrategyInfo();
            string str = (string) sender;
            using (List<CombinedStrategyInfo>.Enumerator enumerator = this.systemPerformance_0.Strategy.CombinedStrategyChildren.GetEnumerator())
            {
                CombinedStrategyInfo current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (str == current.StrategyID.ToString())
                    {
                        goto Label_0056;
                    }
                }
                goto Label_0068;
            Label_0056:
                info = current;
            }
        Label_0068:
            this.cmbChildStrategy.SelectedItem = info;
            this.ivisualizerHost_0.SelectPV(this.TabText);
        }

        private void method_1(SystemPerformance systemPerformance_1)
        {
            DataSeries equityCurve = systemPerformance_1.Results.EquityCurve;
            if (equityCurve.Count == 0)
            {
                this.lblRangeValue.Text = "  ";
            }
            else
            {
                DateTime time = equityCurve.Date[0];
                DateTime time2 = equityCurve.Date[equityCurve.Count - 1];
                this.lblRangeValue.Text = string.Concat(new object[] { time.ToShortDateString(), " to ", time2.ToShortDateString(), " (", equityCurve.Count, " Bars)" });
            }
            this.lvReport.BeginUpdate();
            this.lvReport.Items.Clear();
            this.dictionary_0.Clear();
            this.dictionary_0.Add(this.Font.Style, this.Font);
            this.int_0 = 0;
            this.int_1 = 0;
            this.GenerateReport(systemPerformance_1, systemPerformance_1.Results);
            this.int_0++;
            this.int_1 = 0;
            this.GenerateReport(systemPerformance_1, systemPerformance_1.ResultsLong);
            this.int_0++;
            this.int_1 = 0;
            this.GenerateReport(systemPerformance_1, systemPerformance_1.ResultsShort);
            this.int_0++;
            this.int_1 = 0;
            this.GenerateReport(systemPerformance_1, systemPerformance_1.ResultsBuyHold);
            this.lvReport.EndUpdate();
            if (systemPerformance_1.BenchmarkSymbolbars != null)
            {
                this.columnHeader_4.Text = "Benchmark Buy & Hold (" + systemPerformance_1.BenchmarkSymbolbars.Symbol + ")";
                this.columnHeader_4.Width = 200;
            }
            else
            {
                this.columnHeader_4.Text = "Buy & Hold";
                this.columnHeader_4.Width = 100;
            }
        }

        private void method_2(string string_1, string string_2, Color color_0, Color color_1, Color color_2, FontStyle fontStyle_0, FontStyle fontStyle_1)
        {
            ListViewItem item;
            if (this.int_0 == 0)
            {
                item = this.lvReport.Items.Add(string_1);
                item.UseItemStyleForSubItems = false;
                item.ForeColor = color_1;
                if (color_0 != Color.Empty)
                {
                    item.BackColor = color_0;
                }
                if (this.dictionary_0.ContainsKey(fontStyle_0))
                {
                    item.Font = this.dictionary_0[fontStyle_0];
                }
                else
                {
                    Font font = new Font(this.Font, fontStyle_0);
                    this.dictionary_0.Add(fontStyle_0, font);
                    item.Font = font;
                }
            }
            else
            {
                item = this.lvReport.Items[this.int_1++];
            }
            item.SubItems.Add(string_2);
            if (color_0 != Color.Empty)
            {
                item.SubItems[item.SubItems.Count - 1].BackColor = color_0;
            }
            if (color_2 != Color.Empty)
            {
                item.SubItems[item.SubItems.Count - 1].ForeColor = color_2;
            }
            if (this.dictionary_0.ContainsKey(fontStyle_1))
            {
                item.SubItems[item.SubItems.Count - 1].Font = this.dictionary_0[fontStyle_1];
            }
            else
            {
                Font font2 = new Font(this.Font, fontStyle_1);
                this.dictionary_0.Add(fontStyle_1, font2);
                item.SubItems[item.SubItems.Count - 1].Font = font2;
            }
        }

        private void method_3(ref DataObject dataObject_0)
        {
            dataObject_0.SetData(PrintReport.fmtBaseTitle.Name, this.ivisualizerHost_0.ApplicationName());
            dataObject_0.SetData(PrintReport.fmtTitle.Name, "Performance");
            DataObject printObject = new DataObject();
            this.ivisualizerHost_0.StrategySummary(ref printObject);
            if (printObject.GetDataPresent(PrintReport.fmtTitle.Name))
            {
                dataObject_0.SetData(PrintReport.fmtTitle.Name, printObject.GetData(PrintReport.fmtTitle.Name));
            }
            if (printObject.GetDataPresent(PrintReport.fmtSymbol.Name))
            {
                dataObject_0.SetData(PrintReport.fmtSymbol.Name, printObject.GetData(PrintReport.fmtSymbol.Name));
            }
            if (printObject.GetDataPresent(PrintReport.fmtStrategy.Name))
            {
                dataObject_0.SetData(PrintReport.fmtStrategy.Name, printObject.GetData(PrintReport.fmtStrategy.Name));
            }
            dataObject_0.SetData(PrintReport.fmtListView.Name, this.lvReport);
        }

        private void mniCopy_Click(object sender, EventArgs e)
        {
            this.CopyToClipboard();
        }

        private void mniPrint_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private void mniPrintAll_Click(object sender, EventArgs e)
        {
            this.ivisualizerHost_0.PrintAll();
        }

        public void Print()
        {
            DataObject obj2 = new DataObject();
            this.method_3(ref obj2);
            PrintReport report = new PrintReport(obj2, true) {
                ShowPrintPreview = this.ivisualizerHost_0.ShowPrintPreview(),
                ShowPrintDialog = this.ivisualizerHost_0.ShowPrintDialog()
            };
            this.ivisualizerHost_0.GetPageSettings(ref this.pageSettings_0);
            report.PrintGraphicReport(this.pageSettings_0);
        }

        public VisualizerAppliesTo AppliesTo
        {
            get
            {
                return VisualizerAppliesTo.All;
            }
        }

        public string Description
        {
            get
            {
                return "Displays a detailed report of important performance statistics that describe the results of the Strategy.";
            }
        }

        public bool SupportClipboardCopy
        {
            get
            {
                return true;
            }
        }

        public bool SupportsPrint
        {
            get
            {
                return true;
            }
        }

        public string TabText
        {
            get
            {
                return "Performance";
            }
        }
    }
}

