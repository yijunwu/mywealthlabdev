namespace WealthLab.Visualizers
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class PVEquityCurve : UserControl, IPerformanceVisualizer, ISettingsProvider
    {
        private Area area_0;
        private Area area_1;
        private Area area_2;
        private ToolStripComboBox cbChildStrategies;
        private TChart chart;
        private IContainer icontainer_0;
        private IVisualizerHost ivisualizerHost_0;
        private Line line_0;
        private Line line_1;
        private Line line_2;
        private Line[] line_3;
        private MarksTip marksTip_0;
        private ToolStripMenuItem mniCopyChart;
        private ToolStripMenuItem mnicopyToClipboard;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrintAll;
        private ToolStripMenuItem mniShowBuyAndHold;
        private ToolStripMenuItem mniShowLongAndShort;
        private ToolStripMenuItem mniShowOpenPositions;
        private PageSettings pageSettings_0 = new PageSettings();
        private ContextMenuStrip popup;
        private StringBuilder stringBuilder_0 = new StringBuilder();
        private SystemPerformance systemPerformance_0;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator1;

        public PVEquityCurve()
        {
            this.InitializeComponent();
        }

        private void cbChildStrategies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbChildStrategies.SelectedIndex == 0)
            {
                this.method_0(this.systemPerformance_0, null);
            }
            else
            {
                CombinedStrategyInfo selectedItem = (CombinedStrategyInfo) this.cbChildStrategies.SelectedItem;
                this.method_0(this.systemPerformance_0.GenerateChildStrategyPerformance(selectedItem, this.ivisualizerHost_0.GetExecutor()), selectedItem);
            }
        }

        private void chart_Click(object sender, EventArgs e)
        {
        }

        private void chart_Resize(object sender, EventArgs e)
        {
            this.chart.Legend.Visible = (this.chart.Width >= 500) && (this.chart.Height >= 300);
        }

        public void CopyToClipboard()
        {
            try
            {
                Clipboard.SetImage(this.chart.Bitmap);
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public void CreateVisualization(SystemPerformance performance, IVisualizerHost visHost)
        {
            this.ivisualizerHost_0 = visHost;
            if (this.chart.Zoom.Zoomed)
            {
                this.chart.Zoom.Undo();
            }
            this.systemPerformance_0 = performance;
            if (performance.Strategy.StrategyType == StrategyType.CombinedStrategy)
            {
                this.toolStripLabel1.Visible = true;
                this.cbChildStrategies.Visible = true;
                this.cbChildStrategies.Items.Clear();
                this.cbChildStrategies.Items.Add("Strategies in Aggregate");
                foreach (CombinedStrategyInfo info in performance.Strategy.CombinedStrategyChildren)
                {
                    this.cbChildStrategies.Items.Add(info);
                }
                this.cbChildStrategies.SelectedIndex = 0;
            }
            else
            {
                this.toolStripLabel1.Visible = false;
                this.cbChildStrategies.Visible = false;
            }
            this.method_0(performance, null);
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

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PVEquityCurve));
            this.chart = new TChart();
            this.popup = new ContextMenuStrip(this.icontainer_0);
            this.mniShowBuyAndHold = new ToolStripMenuItem();
            this.mniShowLongAndShort = new ToolStripMenuItem();
            this.mniShowOpenPositions = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.mnicopyToClipboard = new ToolStripMenuItem();
            this.mniCopyChart = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.mniPrintAll = new ToolStripMenuItem();
            this.area_0 = new Area();
            this.area_1 = new Area();
            this.line_0 = new Line();
            this.line_1 = new Line();
            this.line_2 = new Line();
            this.area_2 = new Area();
            this.toolStrip1 = new ToolStrip();
            this.cbChildStrategies = new ToolStripComboBox();
            this.toolStripLabel1 = new ToolStripLabel();
            this.popup.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.chart.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.chart.Aspect.View3D = false;
            this.chart.Aspect.ZOffset = 0.0;
            this.chart.Axes.Bottom.Grid.Visible = false;
            this.chart.Axes.Bottom.Title.Transparent = true;
            this.chart.Axes.Depth.Title.Transparent = true;
            this.chart.Axes.DepthTop.Title.Transparent = true;
            this.chart.Axes.Left.Grid.Visible = false;
            this.chart.Axes.Left.Title.Caption = "Equity $";
            this.chart.Axes.Left.Title.Lines = new string[] { "Equity $" };
            this.chart.Axes.Left.Title.Transparent = true;
            this.chart.Axes.Right.Grid.Visible = false;
            this.chart.Axes.Right.Title.Transparent = true;
            this.chart.Axes.Right.Visible = false;
            this.chart.Axes.Top.Title.Transparent = true;
            this.chart.BackColor = SystemColors.Window;
            this.chart.ContextMenuStrip = this.popup;
            this.chart.Cursor = Cursors.Default;
            this.chart.Header.Font.Brush.Color = Color.FromArgb(0, 0, 0);
            this.chart.Header.Lines = new string[] { "Strategy Equity Curve (with Buy and Hold comparison)" };
            this.chart.Legend.Alignment = LegendAlignments.Bottom;
            this.chart.Location = new Point(0, 0x16);
            this.chart.Name = "chart";
            this.chart.Panel.Brush.Color = SystemColors.Window;
            this.chart.Series.Add(this.area_0);
            this.chart.Series.Add(this.area_1);
            this.chart.Series.Add(this.line_0);
            this.chart.Series.Add(this.line_1);
            this.chart.Series.Add(this.line_2);
            this.chart.Series.Add(this.area_2);
            this.chart.Size = new Size(0x20a, 0x16d);
            this.chart.TabIndex = 0;
            this.chart.Resize += new EventHandler(this.chart_Resize);
            this.chart.Click += new EventHandler(this.chart_Click);
            this.popup.Items.AddRange(new ToolStripItem[] { this.mniShowBuyAndHold, this.mniShowLongAndShort, this.mniShowOpenPositions, this.toolStripSeparator1, this.mnicopyToClipboard, this.mniCopyChart, this.mniPrint, this.mniPrintAll });
            this.popup.Name = "popup";
            this.popup.Size = new Size(0x107, 0xa4);
            this.mniShowBuyAndHold.Checked = true;
            this.mniShowBuyAndHold.CheckState = CheckState.Checked;
            this.mniShowBuyAndHold.Name = "mniShowBuyAndHold";
            this.mniShowBuyAndHold.Size = new Size(0x106, 0x16);
            this.mniShowBuyAndHold.Text = "Show Buy and Hold";
            this.mniShowBuyAndHold.Click += new EventHandler(this.mniShowBuyAndHold_Click);
            this.mniShowLongAndShort.Checked = true;
            this.mniShowLongAndShort.CheckState = CheckState.Checked;
            this.mniShowLongAndShort.Name = "mniShowLongAndShort";
            this.mniShowLongAndShort.Size = new Size(0x106, 0x16);
            this.mniShowLongAndShort.Text = "Show Long and Short";
            this.mniShowLongAndShort.Click += new EventHandler(this.mniShowLongAndShort_Click);
            this.mniShowOpenPositions.Name = "mniShowOpenPositions";
            this.mniShowOpenPositions.Size = new Size(0x106, 0x16);
            this.mniShowOpenPositions.Text = "Show Open Positions";
            this.mniShowOpenPositions.Click += new EventHandler(this.mniShowOpenPositions_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0x103, 6);
            this.mnicopyToClipboard.Image = (Image) manager.GetObject("mnicopyToClipboard.Image");
            this.mnicopyToClipboard.ImageTransparentColor = Color.Fuchsia;
            this.mnicopyToClipboard.Name = "mnicopyToClipboard";
            this.mnicopyToClipboard.Size = new Size(0x106, 0x16);
            this.mnicopyToClipboard.Text = "Copy Equity Curve Data to Clipboard";
            this.mnicopyToClipboard.Click += new EventHandler(this.mnicopyToClipboard_Click);
            this.mniCopyChart.Name = "mniCopyChart";
            this.mniCopyChart.Size = new Size(0x106, 0x16);
            this.mniCopyChart.Text = "Copy Chart to Clipboard";
            this.mniCopyChart.Click += new EventHandler(this.mniCopyChart_Click);
            this.mniPrint.Image = (Image) manager.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0x106, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.mniPrintAll.Name = "mniPrintAll";
            this.mniPrintAll.Size = new Size(0x106, 0x16);
            this.mniPrintAll.Text = "Print All";
            this.mniPrintAll.ToolTipText = "Print content from all tabs";
            this.mniPrintAll.Click += new EventHandler(this.mniPrintAll_Click);
            this.area_0.AreaBrush.Color = Color.FromArgb(0, 0xc0, 0);
            this.area_0.Gradient.StartColor = Color.FromArgb(0, 0xc0, 0);
            this.area_0.AreaLines.Color = Color.FromArgb(0, 0x73, 0);
            this.area_0.AreaLines.Visible = false;
            this.area_0.Brush.Color = Color.FromArgb(0, 0xc0, 0);
            this.area_0.Color = Color.FromArgb(0, 0xc0, 0);
            this.area_0.ColorEach = false;
            this.area_0.LinePen.Color = Color.FromArgb(0, 0x73, 0);
            this.area_0.LinePen.Visible = false;
            this.area_0.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.area_0.Marks.Callout.ArrowHeadSize = 8;
            this.area_0.Marks.Callout.Brush.Color = Color.Black;
            this.area_0.Marks.Callout.Distance = 0;
            this.area_0.Marks.Callout.Draw3D = false;
            this.area_0.Marks.Callout.Length = 10;
            this.area_0.Marks.Callout.Style = PointerStyles.Rectangle;
            this.area_0.Marks.Callout.Visible = false;
            this.area_0.Pointer.Brush.Color = Color.FromArgb(0, 0x80, 0);
            this.area_0.Pointer.Style = PointerStyles.Rectangle;
            this.area_0.Title = "Equity";
            this.area_0.UseOrigin = true;
            this.area_0.ValueFormat = "#,##0.00 Equity";
            this.area_0.XValues.DataMember = "X";
            this.area_0.XValues.Order = ValueListOrder.Ascending;
            this.area_0.YValues.DataMember = "Y";
            this.area_1.AreaBrush.Color = Color.FromArgb(0, 0x80, 0);
            this.area_1.Gradient.StartColor = Color.FromArgb(0, 0x80, 0);
            this.area_1.AreaLines.Color = Color.FromArgb(0, 0x4d, 0);
            this.area_1.AreaLines.Visible = false;
            this.area_1.Brush.Color = Color.FromArgb(0, 0x80, 0);
            this.area_1.Color = Color.FromArgb(0, 0x80, 0);
            this.area_1.ColorEach = false;
            this.area_1.LinePen.Color = Color.FromArgb(0, 0x4d, 0);
            this.area_1.LinePen.Visible = false;
            this.area_1.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.area_1.Marks.Callout.ArrowHeadSize = 8;
            this.area_1.Marks.Callout.Brush.Color = Color.Black;
            this.area_1.Marks.Callout.Distance = 0;
            this.area_1.Marks.Callout.Draw3D = false;
            this.area_1.Marks.Callout.Length = 10;
            this.area_1.Marks.Callout.Style = PointerStyles.Rectangle;
            this.area_1.Marks.Callout.Visible = false;
            this.area_1.Pointer.Style = PointerStyles.Rectangle;
            this.area_1.Title = "Cash";
            this.area_1.UseOrigin = true;
            this.area_1.ValueFormat = "#,##0.00 Cash";
            this.area_1.XValues.DataMember = "X";
            this.area_1.XValues.Order = ValueListOrder.Ascending;
            this.area_1.YValues.DataMember = "Y";
            this.line_0.Brush.Color = Color.FromArgb(0, 0, 0xff);
            this.line_0.Color = Color.FromArgb(0, 0, 0xff);
            this.line_0.ColorEach = false;
            this.line_0.LinePen.Color = Color.FromArgb(0, 0, 0x80);
            this.line_0.LinePen.Width = 2;
            this.line_0.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.line_0.Marks.Callout.ArrowHeadSize = 8;
            this.line_0.Marks.Callout.Brush.Color = Color.Black;
            this.line_0.Marks.Callout.Distance = 0;
            this.line_0.Marks.Callout.Draw3D = false;
            this.line_0.Marks.Callout.Length = 10;
            this.line_0.Marks.Callout.Style = PointerStyles.Rectangle;
            this.line_0.Marks.Callout.Visible = false;
            this.line_0.Pointer.Brush.Color = Color.Red;
            this.line_0.Pointer.Style = PointerStyles.Rectangle;
            this.line_0.Title = "Buy and Hold";
            this.line_0.ValueFormat = "#,##0.00 B&H";
            this.line_0.XValues.DataMember = "X";
            this.line_0.XValues.Order = ValueListOrder.Ascending;
            this.line_0.YValues.DataMember = "Y";
            this.line_1.Brush.Color = Color.FromArgb(0x44, 0x66, 0xa3);
            this.line_1.Color = Color.FromArgb(0, 0, 0);
            this.line_1.ColorEach = false;
            this.line_1.LinePen.Color = Color.FromArgb(0x29, 0x3d, 0x62);
            this.line_1.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.line_1.Marks.Callout.ArrowHeadSize = 8;
            this.line_1.Marks.Callout.Distance = 0;
            this.line_1.Marks.Callout.Draw3D = false;
            this.line_1.Marks.Callout.Length = 10;
            this.line_1.Marks.Callout.Style = PointerStyles.Rectangle;
            this.line_1.Marks.Callout.Visible = false;
            this.line_1.Pointer.Brush.Color = Color.Red;
            this.line_1.Pointer.Style = PointerStyles.Rectangle;
            this.line_1.Title = "Long";
            this.line_1.ValueFormat = "#,##0.00 Long";
            this.line_1.XValues.DataMember = "X";
            this.line_1.XValues.Order = ValueListOrder.Ascending;
            this.line_1.YValues.DataMember = "Y";
            this.line_2.Brush.Color = Color.FromArgb(0xc0, 0, 0);
            this.line_2.Color = Color.FromArgb(0xc0, 0, 0);
            this.line_2.ColorEach = false;
            this.line_2.LinePen.Color = Color.FromArgb(0x60, 0, 0);
            this.line_2.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.line_2.Marks.Callout.ArrowHeadSize = 8;
            this.line_2.Marks.Callout.Brush.Color = Color.Black;
            this.line_2.Marks.Callout.Distance = 0;
            this.line_2.Marks.Callout.Draw3D = false;
            this.line_2.Marks.Callout.Length = 10;
            this.line_2.Marks.Callout.Style = PointerStyles.Rectangle;
            this.line_2.Marks.Callout.Visible = false;
            this.line_2.Pointer.Brush.Color = Color.Red;
            this.line_2.Pointer.Style = PointerStyles.Rectangle;
            this.line_2.Title = "Short";
            this.line_2.ValueFormat = "#,##0.00 Short";
            this.line_2.XValues.DataMember = "X";
            this.line_2.XValues.Order = ValueListOrder.Ascending;
            this.line_2.YValues.DataMember = "Y";
            this.area_2.AreaBrush.Color = Color.FromArgb(0x44, 0x66, 0xa3);
            this.area_2.Gradient.StartColor = Color.FromArgb(0x44, 0x66, 0xa3);
            this.area_2.AreaLines.Color = Color.FromArgb(0x29, 0x3d, 0x62);
            this.area_2.AreaLines.Visible = false;
            this.area_2.Brush.Color = Color.FromArgb(0x44, 0x66, 0xa3);
            this.area_2.Color = Color.FromArgb(0x44, 0x66, 0xa3);
            this.area_2.ColorEach = false;
            this.area_2.LinePen.Color = Color.FromArgb(0x29, 0x3d, 0x62);
            this.area_2.LinePen.Visible = false;
            this.area_2.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.area_2.Marks.Callout.ArrowHeadSize = 8;
            this.area_2.Marks.Callout.Brush.Color = Color.Black;
            this.area_2.Marks.Callout.Distance = 0;
            this.area_2.Marks.Callout.Draw3D = false;
            this.area_2.Marks.Callout.Length = 10;
            this.area_2.Marks.Callout.Style = PointerStyles.Rectangle;
            this.area_2.Marks.Callout.Visible = false;
            this.area_2.Pointer.Style = PointerStyles.Rectangle;
            this.area_2.Title = "Open Positions";
            this.area_2.VertAxis = VerticalAxis.Right;
            this.area_2.Visible = false;
            this.area_2.XValues.DataMember = "X";
            this.area_2.XValues.Order = ValueListOrder.Ascending;
            this.area_2.YValues.DataMember = "Y";
            this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.cbChildStrategies, this.toolStripLabel1 });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x20a, 0x19);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.cbChildStrategies.Alignment = ToolStripItemAlignment.Right;
            this.cbChildStrategies.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbChildStrategies.DropDownWidth = 150;
            this.cbChildStrategies.Items.AddRange(new object[] { "Strategies in Aggregate" });
            this.cbChildStrategies.Name = "cbChildStrategies";
            this.cbChildStrategies.Size = new Size(160, 0x19);
            this.cbChildStrategies.Visible = false;
            this.cbChildStrategies.SelectedIndexChanged += new EventHandler(this.cbChildStrategies_SelectedIndexChanged);
            this.toolStripLabel1.Alignment = ToolStripItemAlignment.Right;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(0x21, 0x16);
            this.toolStripLabel1.Text = "View:";
            this.toolStripLabel1.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.chart);
            base.Name = "PVEquityCurve";
            base.Size = new Size(0x20a, 0x16e);
            this.popup.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(SystemPerformance systemPerformance_1, CombinedStrategyInfo combinedStrategyInfo_0)
        {
            DataSeries equityCurve = systemPerformance_1.Results.EquityCurve;
            DataSeries cashCurve = systemPerformance_1.Results.CashCurve;
            DataSeries series3 = systemPerformance_1.ResultsBuyHold.EquityCurve;
            DataSeries series4 = systemPerformance_1.ResultsLong.EquityCurve;
            DataSeries series5 = systemPerformance_1.ResultsShort.EquityCurve;
            DataSeries openPositionCount = systemPerformance_1.Results.OpenPositionCount;
            this.area_0.BeginUpdate();
            this.area_1.BeginUpdate();
            this.line_0.BeginUpdate();
            this.area_0.Clear();
            this.area_1.Clear();
            this.line_0.Clear();
            this.line_1.Clear();
            this.line_2.Clear();
            this.area_2.Clear();
            if (this.stringBuilder_0.Length != 0)
            {
                this.stringBuilder_0.Length = 0;
            }
            if (systemPerformance_1.IsIntraday)
            {
                this.stringBuilder_0.AppendLine("Date\t\t\tEquity\tLong\tShort\tBuy & Hold\tCash");
            }
            else
            {
                this.stringBuilder_0.AppendLine("Date\t\tEquity\tLong\tShort\tBuy & Hold\tCash");
            }
            for (int i = 0; i < equityCurve.Count; i++)
            {
                string text = equityCurve.Date[i].ToShortDateString();
                this.area_0.Add((double) i, equityCurve[i], text);
                this.line_1.Add((double) i, series4[i], text);
                this.line_2.Add((double) i, series5[i], text);
                if (!systemPerformance_1.PositionSize.RawProfitMode)
                {
                    this.area_1.Add((double) i, cashCurve[i], text);
                }
                this.line_0.Add((double) i, series3[i], text);
                this.area_2.Add((double) i, openPositionCount[i], text);
                if (systemPerformance_1.IsIntraday)
                {
                    object[] objArray = new object[] { equityCurve.Date[i].ToString(), "\t", equityCurve[i], "\t", series4[i], "\t", series5[i], "\t", series3[i], "\t", cashCurve[i] };
                    this.stringBuilder_0.AppendLine(string.Concat(objArray));
                }
                else
                {
                    object[] objArray2 = new object[] { equityCurve.Date[i].ToShortDateString(), "\t", equityCurve[i], "\t", series4[i], "\t", series5[i], "\t", series3[i], "\t", cashCurve[i] };
                    this.stringBuilder_0.AppendLine(string.Concat(objArray2));
                }
            }
            this.area_0.EndUpdate();
            this.area_1.EndUpdate();
            this.line_0.EndUpdate();
            if (systemPerformance_1.Strategy.StrategyType == StrategyType.CombinedStrategy)
            {
                int index = 0;
                if (this.line_3 != null)
                {
                    foreach (Line line in this.line_3)
                    {
                        if ((line != null) && (this.chart.Series.IndexOf(line) >= 0))
                        {
                            this.chart.Series.Remove(line);
                        }
                    }
                }
                if (combinedStrategyInfo_0 == null)
                {
                    this.line_3 = new Line[systemPerformance_1.Strategy.CombinedStrategyChildren.Count];
                    foreach (CombinedStrategyInfo info in systemPerformance_1.Strategy.CombinedStrategyChildren)
                    {
                        this.line_3[index] = new Line();
                        DataSeries series8 = systemPerformance_1.GenerateChildStrategyPerformance(info, this.ivisualizerHost_0.GetExecutor()).Results.EquityCurve;
                        this.line_3[index].BeginUpdate();
                        this.line_3[index].Clear();
                        this.line_3[index].Title = info.Name;
                        for (int j = 0; j < series8.Count; j++)
                        {
                            string str3 = series8.Date[j].ToShortDateString();
                            this.line_3[index].Add((double) j, series8[j], str3);
                        }
                        this.chart.Series.Add(this.line_3[index]);
                        this.line_3[index].EndUpdate();
                        index++;
                    }
                }
                else
                {
                    this.line_3 = new Line[] { new Line() };
                    DataSeries series7 = systemPerformance_1.Results.EquityCurve;
                    this.line_3[index].BeginUpdate();
                    this.line_3[index].Clear();
                    this.line_3[index].Title = combinedStrategyInfo_0.Name;
                    for (int k = 0; k < series7.Count; k++)
                    {
                        string str2 = series7.Date[k].ToShortDateString();
                        this.line_3[index].Add((double) k, series7[k], str2);
                    }
                    this.chart.Series.Add(this.line_3[index]);
                    this.line_3[index].EndUpdate();
                    index++;
                }
            }
            bool toolsContainMarksTip = false;
            for(int i = 0; i < this.chart.Tools.Count; i ++)
            {
                if (this.chart.Tools[i].GetType().Equals(this.marksTip_0.GetType())) {
                    toolsContainMarksTip = true;
                }
            }
            if (!toolsContainMarksTip) 
            {
                this.marksTip_0 = new MarksTip(this.chart.Chart);
                this.marksTip_0.HideDelay = 0x1388;
                this.marksTip_0.MouseDelay = 100;
                this.marksTip_0.Style = MarksStyles.LabelValue;
                this.chart.Tools.Add(this.marksTip_0);                
            }
            
            if (systemPerformance_1.BenchmarkSymbolbars != null)
            {
                this.chart.Header.Lines = new string[] { "Strategy Equity Curve (With Benchmark Buy & Hold (" + systemPerformance_1.BenchmarkSymbolbars.Symbol + ") comparison)" };
            }
            else
            {
                this.chart.Header.Lines = new string[] { "Strategy Equity Curve (with Buy and Hold comparison)" };
            }
        }

        private void method_1(ref DataObject dataObject_0)
        {
            dataObject_0.SetData(PrintReport.fmtBaseTitle.Name, this.ivisualizerHost_0.ApplicationName());
            dataObject_0.SetData(PrintReport.fmtTitle.Name, "Equity Curve");
            this.ivisualizerHost_0.StrategySummary(ref dataObject_0);
            dataObject_0.SetData(PrintReport.fmtGraphic.Name, this.chart.Bitmap);
        }

        private void mniCopyChart_Click(object sender, EventArgs e)
        {
            this.CopyToClipboard();
        }

        private void mnicopyToClipboard_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Clipboard.SetDataObject(this.stringBuilder_0.ToString(), true, 2, 0x3e8);
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            this.Cursor = Cursors.Default;
        }

        private void mniPrint_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private void mniPrintAll_Click(object sender, EventArgs e)
        {
            this.ivisualizerHost_0.PrintAll();
        }

        private void mniShowBuyAndHold_Click(object sender, EventArgs e)
        {
            this.mniShowBuyAndHold.Checked = !this.mniShowBuyAndHold.Checked;
            this.line_0.Visible = this.mniShowBuyAndHold.Checked;
        }

        private void mniShowLongAndShort_Click(object sender, EventArgs e)
        {
            this.mniShowLongAndShort.Checked = !this.mniShowLongAndShort.Checked;
            this.line_1.Visible = this.mniShowLongAndShort.Checked;
            this.line_2.Visible = this.mniShowLongAndShort.Checked;
        }

        private void mniShowOpenPositions_Click(object sender, EventArgs e)
        {
            this.mniShowOpenPositions.Checked = !this.mniShowOpenPositions.Checked;
            this.area_2.Visible = this.mniShowOpenPositions.Checked;
            this.chart.Axes.Right.Visible = this.mniShowOpenPositions.Checked;
        }

        public void Print()
        {
            Cursor cursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            DataObject obj2 = new DataObject();
            this.method_1(ref obj2);
            PrintReport report = new PrintReport(obj2, true) {
                ShowPrintPreview = this.ivisualizerHost_0.ShowPrintPreview(),
                ShowPrintDialog = this.ivisualizerHost_0.ShowPrintDialog()
            };
            this.ivisualizerHost_0.GetPageSettings(ref this.pageSettings_0);
            report.PrintGraphicReport(this.pageSettings_0);
            this.Cursor = cursor;
        }

        public VisualizerAppliesTo AppliesTo
        {
            get
            {
                return VisualizerAppliesTo.All;
            }
        }

        public Bitmap ChartBitmap
        {
            get
            {
                return this.chart.Bitmap;
            }
        }

        public string Description
        {
            get
            {
                return "Displays an equity curve that portrays the Strategy's performance over time, compared to Buy and Hold.  For portfolio simulations, also displays the amount of exposure taken by the Strategy.";
            }
        }

        public string SettingsString
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("ShowBuyHold=");
                builder.Append(this.mniShowBuyAndHold.Checked.ToString());
                builder.Append(";ShowLongShort=");
                builder.Append(this.mniShowLongAndShort.Checked.ToString());
                return builder.ToString();
            }
            set
            {
                string[] strArray = value.Split(new char[] { ';' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    string[] strArray2 = strArray[i].Split(new char[] { '=' });
                    if (strArray2.Length >= 2)
                    {
                        if (strArray2[0] == "ShowBuyHold")
                        {
                            if (!bool.Parse(strArray2[1]))
                            {
                                this.mniShowBuyAndHold_Click(this, new EventArgs());
                            }
                        }
                        else if ((strArray2[0] == "ShowLongShort") && !bool.Parse(strArray2[1]))
                        {
                            this.mniShowLongAndShort_Click(this, new EventArgs());
                        }
                    }
                }
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
                return "Equity Curve";
            }
        }
    }
}

