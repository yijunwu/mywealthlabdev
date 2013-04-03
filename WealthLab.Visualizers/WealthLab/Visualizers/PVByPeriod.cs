namespace WealthLab.Visualizers
{
    using Fidelity.Components;
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.Indicators;

    [ToolboxItem(false)]
    public class PVByPeriod : UserControl, IPerformanceVisualizer, ISettingsProvider
    {
        private Bar bar_0;
        private Bar bar_1;
        private BarScale barScale_0;
        private bool bool_0 = true;
        private TChart chartDistReturns;
        private TChart chartReturns;
        private ToolStripComboBox cmbChartUnits;
        private ComboBox cmbPeriod;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private ColumnHeader columnHeader_5;
        private ColumnHeader columnHeader_6;
        private DataSeries dataSeries_0;
        private DataSeries dataSeries_1;
        private DataSeries dataSeries_2;
        private DataSeries dataSeries_3;
        private Distribution distribution_0;
        private IContainer icontainer_0;
        private IList<WealthLab.Position> ilist_0;
        private int int_0;
        private int int_1;
        private int int_2;
        private IVisualizerHost ivisualizerHost_0;
        private Label lblAvgReturn;
        private Label lblAvgReturnValue;
        private Label lblBest;
        private Label lblBestValue;
        private Label lblConsecProfitable;
        private Label lblConsecProfitableValue;
        private Label lblConsecUnprofitable;
        private Label lblConsecUnprofitableValue;
        private Label lblPctProfitable;
        private Label lblPctProfitableValue;
        private Label lblPeriod;
        private Label lblPeriodsValue;
        private Label lblProfitable;
        private Label lblProfitableValue;
        private Label lblSelectPeriod;
        private Label lblSharpe;
        private Label lblSharpeValue;
        private Label lblStdDev;
        private Label lblStdDevValue;
        private Label lblWorst;
        private Label lblWorstValue;
        private SortableListView lvReturns;
        private ToolStripMenuItem mniCopyChart;
        private ToolStripMenuItem mniCopyData;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrintAll;
        private ToolStripMenuItem mniPrintAll2;
        private ToolStripMenuItem mniPrintData;
        private TabPage pageDistribution;
        private TabPage pageReturns;
        private PageSettings pageSettings_0 = new PageSettings();
        private System.Windows.Forms.Panel pnlByPeriodStats;
        private ContextMenuStrip popup;
        private ContextMenuStrip popup_1;
        private PrintPreview printPreview_0;
        private PrintReport printReport_0 = new PrintReport();
        private SplitContainer splitByPeriod;
        private string string_0;
        private static string string_1 = "Percent";
        private string string_2 = string_1;
        private string string_3 = string_1;
        private SystemPerformance systemPerformance_0;
        private TabControl tabByPeriod;
        private ToolStripSeparator toolStripSeparator1;

        public PVByPeriod()
        {
            this.InitializeComponent();
            this.chartDistReturns.Zoom.Allow = false;
            this.chartReturns.Axes.Left.Automatic = true;
            this.chartDistReturns.Axes.Bottom.Automatic = true;
            this.chartReturns.Axes.Left.Labels.ValueFormat = this.bar_0.PercentFormat;
            this.chartDistReturns.Axes.Bottom.Labels.ValueFormat = this.bar_1.PercentFormat;
        }

        public void byPeriodReport_BeginPrint(object sender, PrintEventArgs e)
        {
            if ((this.printPreview_0 != null) && this.printPreview_0.PrintSomePages)
            {
                this.printReport_0.startPageCount = this.int_0 = this.printPreview_0.FromPage;
                this.printReport_0.endPageCount = this.int_1 = this.printPreview_0.ToPage;
            }
            this.printReport_0.BasePrintTitle = this.ivisualizerHost_0.ApplicationName();
            this.printReport_0.printTitle = "Performance By Period";
            this.printReport_0.UseDefaultDisclosure();
            this.printReport_0.intPageCounter = 1;
        }

        public void byPeriodReport_EndPrint(object sender, PrintEventArgs e)
        {
            if (this.printPreview_0 != null)
            {
                this.printPreview_0.FromPage = 1;
                this.printPreview_0.ToPage = this.int_1 = --this.printReport_0.intPageCounter;
            }
        }

        public void byPeriodReport_PrintPage(object sender, PrintPageEventArgs e)
        {
            bool printPage = true;
            Rectangle destRect = new Rectangle(e.MarginBounds.X, e.MarginBounds.Y, e.MarginBounds.Width, e.MarginBounds.Height);
            this.int_2 = ((int) e.Graphics.MeasureString("Test", this.printReport_0.printFontBody).Height) + 1;
            if (this.printReport_0.printSomePages && (this.printReport_0.intPageCounter < this.int_0))
            {
                printPage = false;
            }
            else
            {
                this.printReport_0.PrintTitle(e, ref destRect);
                DataObject printObject = new DataObject();
                this.ivisualizerHost_0.StrategySummary(ref printObject);
                this.printReport_0.printStrategy = printObject.GetData(PrintReport.fmtStrategy.Name).ToString();
                this.printReport_0.printSymbol = printObject.GetData(PrintReport.fmtSymbol.Name).ToString();
                this.printReport_0.PrintHeader(e, ref destRect);
            }
            if (this.printReport_0.intPageCounter == 1)
            {
                ListView view = new ListView();
                view.Columns.Add("", 0x91, HorizontalAlignment.Left);
                view.Columns.Add("", 60, HorizontalAlignment.Left);
                ListViewItem item = view.Items.Add("Period:");
                if (this.cmbPeriod.Text != null)
                {
                    item.SubItems.Add(this.cmbPeriod.Text);
                }
                view.Items.Add(this.lblAvgReturn.Text).SubItems.Add(this.lblAvgReturnValue.Text);
                view.Items.Add(this.lblStdDev.Text).SubItems.Add(this.lblStdDevValue.Text);
                view.Items.Add(this.lblSharpe.Text).SubItems.Add(this.lblSharpeValue.Text);
                view.Items.Add(this.lblBest.Text).SubItems.Add(this.lblBestValue.Text);
                view.Items.Add(this.lblWorst.Text).SubItems.Add(this.lblWorstValue.Text);
                view.Items.Add(this.lblPeriod.Text).SubItems.Add(this.lblPeriodsValue.Text);
                view.Items.Add(this.lblProfitable.Text).SubItems.Add(this.lblProfitableValue.Text);
                view.Items.Add(this.lblPctProfitable.Text).SubItems.Add(this.lblPctProfitableValue.Text);
                view.Items.Add(this.lblConsecProfitable.Text).SubItems.Add(this.lblConsecProfitableValue.Text);
                view.Items.Add(this.lblConsecUnprofitable.Text).SubItems.Add(this.lblConsecUnprofitableValue.Text);
                this.printReport_0.printListView = view;
                Rectangle rectangle6 = new Rectangle(destRect.X, destRect.Y, 0xcd, destRect.Height);
                this.printReport_0.PrintListView(e, ref rectangle6, printPage);
                if (this.tabByPeriod.SelectedTab.Name == "pageDistribution")
                {
                    if (this.chartDistReturns.Bitmap != null)
                    {
                        this.printReport_0.printGraphic = this.chartDistReturns.Bitmap;
                    }
                }
                else if (this.chartReturns.Bitmap != null)
                {
                    this.printReport_0.printGraphic = this.chartReturns.Bitmap;
                }
                Rectangle rectangle7 = new Rectangle(destRect.X + 210, destRect.Y, destRect.Width - 0xcd, rectangle6.Y - destRect.Y);
                this.printReport_0.PrintGraphic(e, ref rectangle7, printPage);
                destRect.Y = rectangle6.Y;
                destRect.Height = rectangle6.Height;
                destRect.Y += this.int_2;
                destRect.Height -= this.int_2;
                this.printReport_0.printListView = this.lvReturns;
            }
            SizeF disclosureRect = this.printReport_0.GetDisclosureRect(e, ref destRect);
            this.printReport_0.PrintListView(e, ref destRect, printPage);
            this.printReport_0.PrintFooter(e, printPage, ref destRect, disclosureRect);
            e.HasMorePages = this.printReport_0.fPrintListView;
            if ((++this.printReport_0.intPageCounter > this.int_1) && this.printReport_0.printSomePages)
            {
                e.HasMorePages = false;
            }
        }

        private void cmbChartUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.bool_0)
            {
                this.method_4();
                if (this.tabByPeriod.SelectedTab.Name == "pageDistribution")
                {
                    this.method_1();
                    this.string_3 = this.cmbChartUnits.Text;
                }
                else
                {
                    this.method_2();
                    this.string_2 = this.cmbChartUnits.Text;
                }
            }
        }

        private void cmbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.method_0();
        }

        public void CopyToClipboard()
        {
            this.ivisualizerHost_0.CopyListViewToClipboard(this.lvReturns);
        }

        public void CreateVisualization(SystemPerformance performance, IVisualizerHost visHost)
        {
            this.ivisualizerHost_0 = visHost;
            this.systemPerformance_0 = performance;
            if (this.chartReturns.Zoom.Zoomed)
            {
                this.chartReturns.Zoom.Undo();
            }
            this.dataSeries_0 = performance.Results.EquityCurve;
            this.dataSeries_1 = performance.Results.CashCurve;
            this.ilist_0 = performance.Results.Positions;
            this.barScale_0 = BarScale.Daily;
            if (performance.Results.Positions.Count > 0)
            {
                this.barScale_0 = performance.Results.Positions[0].Bars.Scale;
            }
            this.method_0();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PVByPeriod));
            this.splitByPeriod = new SplitContainer();
            this.tabByPeriod = new TabControl();
            this.popup = new ContextMenuStrip(this.icontainer_0);
            this.cmbChartUnits = new ToolStripComboBox();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.mniCopyChart = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.mniPrintAll = new ToolStripMenuItem();
            this.pageReturns = new TabPage();
            this.chartReturns = new TChart();
            this.bar_0 = new Bar();
            this.pageDistribution = new TabPage();
            this.chartDistReturns = new TChart();
            this.bar_1 = new Bar();
            this.pnlByPeriodStats = new System.Windows.Forms.Panel();
            this.lblConsecUnprofitableValue = new Label();
            this.lblConsecUnprofitable = new Label();
            this.lblConsecProfitableValue = new Label();
            this.lblWorst = new Label();
            this.lblPctProfitableValue = new Label();
            this.lblPctProfitable = new Label();
            this.lblProfitableValue = new Label();
            this.lblProfitable = new Label();
            this.lblPeriodsValue = new Label();
            this.lblPeriod = new Label();
            this.lblWorstValue = new Label();
            this.lblConsecProfitable = new Label();
            this.lblBestValue = new Label();
            this.lblBest = new Label();
            this.lblSharpeValue = new Label();
            this.lblSharpe = new Label();
            this.lblStdDevValue = new Label();
            this.lblStdDev = new Label();
            this.lblAvgReturnValue = new Label();
            this.lblAvgReturn = new Label();
            this.cmbPeriod = new ComboBox();
            this.lblSelectPeriod = new Label();
            this.lvReturns = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.columnHeader_5 = new ColumnHeader();
            this.columnHeader_6 = new ColumnHeader();
            this.popup_1 = new ContextMenuStrip(this.icontainer_0);
            this.mniCopyData = new ToolStripMenuItem();
            this.mniPrintData = new ToolStripMenuItem();
            this.mniPrintAll2 = new ToolStripMenuItem();
            this.distribution_0 = new Distribution(this.icontainer_0);
            this.splitByPeriod.Panel1.SuspendLayout();
            this.splitByPeriod.Panel2.SuspendLayout();
            this.splitByPeriod.SuspendLayout();
            this.tabByPeriod.SuspendLayout();
            this.popup.SuspendLayout();
            this.pageReturns.SuspendLayout();
            this.pageDistribution.SuspendLayout();
            this.pnlByPeriodStats.SuspendLayout();
            this.popup_1.SuspendLayout();
            base.SuspendLayout();
            this.splitByPeriod.Dock = DockStyle.Fill;
            this.splitByPeriod.Location = new Point(0, 0);
            this.splitByPeriod.Name = "splitByPeriod";
            this.splitByPeriod.Orientation = Orientation.Horizontal;
            this.splitByPeriod.Panel1.Controls.Add(this.tabByPeriod);
            this.splitByPeriod.Panel1.Controls.Add(this.pnlByPeriodStats);
            this.splitByPeriod.Panel2.Controls.Add(this.lvReturns);
            this.splitByPeriod.Size = new Size(0x271, 0x1a2);
            this.splitByPeriod.SplitterDistance = 0xd0;
            this.splitByPeriod.TabIndex = 0;
            this.tabByPeriod.ContextMenuStrip = this.popup;
            this.tabByPeriod.Controls.Add(this.pageReturns);
            this.tabByPeriod.Controls.Add(this.pageDistribution);
            this.tabByPeriod.Dock = DockStyle.Fill;
            this.tabByPeriod.Location = new Point(0xd4, 0);
            this.tabByPeriod.Name = "tabByPeriod";
            this.tabByPeriod.SelectedIndex = 0;
            this.tabByPeriod.Size = new Size(0x19d, 0xd0);
            this.tabByPeriod.TabIndex = 1;
            this.tabByPeriod.SelectedIndexChanged += new EventHandler(this.tabByPeriod_SelectedIndexChanged);
            this.popup.Items.AddRange(new ToolStripItem[] { this.cmbChartUnits, this.toolStripSeparator1, this.mniCopyChart, this.mniPrint, this.mniPrintAll });
            this.popup.Name = "popup";
            this.popup.Size = new Size(0xb6, 0x65);
            this.cmbChartUnits.Items.AddRange(new object[] { "Percent", "Dollar" });
            this.cmbChartUnits.Name = "cmbChartUnits";
            this.cmbChartUnits.Size = new Size(0x79, 0x15);
            this.cmbChartUnits.Text = "Percent";
            this.cmbChartUnits.SelectedIndexChanged += new EventHandler(this.cmbChartUnits_SelectedIndexChanged);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0xb2, 6);
            this.mniCopyChart.Image = (Image) manager.GetObject("mniCopyChart.Image");
            this.mniCopyChart.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyChart.Name = "mniCopyChart";
            this.mniCopyChart.Size = new Size(0xb5, 0x16);
            this.mniCopyChart.Text = "Copy Chart";
            this.mniCopyChart.ToolTipText = "Copy the chart to the clipboard";
            this.mniCopyChart.Click += new EventHandler(this.mniCopyChart_Click);
            this.mniPrint.Image = (Image) manager.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0xb5, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.ToolTipText = "Print By Period results";
            this.mniPrint.Click += new EventHandler(this.mniPrintData_Click);
            this.mniPrintAll.Name = "mniPrintAll";
            this.mniPrintAll.Size = new Size(0xb5, 0x16);
            this.mniPrintAll.Text = "Print All";
            this.mniPrintAll.ToolTipText = "Print content from all tabs";
            this.mniPrintAll.Click += new EventHandler(this.mniPrintAll2_Click);
            this.pageReturns.Controls.Add(this.chartReturns);
            this.pageReturns.Location = new Point(4, 0x16);
            this.pageReturns.Name = "pageReturns";
            this.pageReturns.Padding = new Padding(3);
            this.pageReturns.Size = new Size(0x195, 0xb6);
            this.pageReturns.TabIndex = 0;
            this.pageReturns.Text = "Raw Returns";
            this.pageReturns.UseVisualStyleBackColor = true;
            this.chartReturns.Aspect.View3D = false;
            this.chartReturns.Aspect.ZOffset = 0.0;
            this.chartReturns.Axes.Bottom.Grid.Visible = false;
            this.chartReturns.Axes.Bottom.MaximumOffset = 0x22;
            this.chartReturns.Axes.Bottom.MinimumOffset = 0x22;
            this.chartReturns.Axes.Bottom.StartPosition = 1.0;
            this.chartReturns.BackColor = Color.Transparent;
            this.chartReturns.Cursor = Cursors.Default;
            this.chartReturns.Dock = DockStyle.Fill;
            this.chartReturns.Header.Visible = false;
            this.chartReturns.Legend.Visible = false;
            this.chartReturns.Location = new Point(3, 3);
            this.chartReturns.Name = "chartReturns";
            this.chartReturns.Panel.Brush.Color = Color.FromArgb(0xff, 0xff, 0xff);
            this.chartReturns.Series.Add(this.bar_0);
            this.chartReturns.Size = new Size(0x18f, 0xb0);
            this.chartReturns.TabIndex = 0;
            this.bar_0.Brush.Color = Color.FromArgb(0x44, 0x66, 0xa3);
            this.bar_0.Color = Color.FromArgb(0x44, 0x66, 0xa3);
            this.bar_0.ColorEach = false;
            this.bar_0.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.bar_0.Marks.Callout.ArrowHeadSize = 8;
            this.bar_0.Marks.Callout.Brush.Color = Color.Black;
            this.bar_0.Marks.Callout.Distance = 0;
            this.bar_0.Marks.Callout.Draw3D = false;
            this.bar_0.Marks.Callout.Length = 20;
            this.bar_0.Marks.Callout.Style = PointerStyles.Rectangle;
            this.bar_0.Marks.Callout.Visible = false;
            this.bar_0.Marks.Visible = false;
            this.bar_0.Pen.Color = Color.FromArgb(0x29, 0x3d, 0x62);
            this.bar_0.Pen.Visible = false;
            this.bar_0.Title = "bar1";
            this.bar_0.XValues.DataMember = "X";
            this.bar_0.XValues.Order = ValueListOrder.Ascending;
            this.bar_0.YValues.DataMember = "Bar";
            this.pageDistribution.Controls.Add(this.chartDistReturns);
            this.pageDistribution.Location = new Point(4, 0x16);
            this.pageDistribution.Name = "pageDistribution";
            this.pageDistribution.Padding = new Padding(3);
            this.pageDistribution.Size = new Size(0x195, 0xb6);
            this.pageDistribution.TabIndex = 1;
            this.pageDistribution.Text = "Distribution of Returns";
            this.pageDistribution.UseVisualStyleBackColor = true;
            this.chartDistReturns.Aspect.ZOffset = 0.0;
            this.chartDistReturns.Axes.Bottom.Grid.Visible = false;
            this.chartDistReturns.Axes.Bottom.Labels.ValueFormat = "#0.00%";
            this.chartDistReturns.Axes.Left.Grid.Visible = false;
            this.chartDistReturns.BackColor = Color.Transparent;
            this.chartDistReturns.Dock = DockStyle.Fill;
            this.chartDistReturns.Footer.Font.Brush.Color = Color.Black;
            this.chartDistReturns.Footer.Lines = new string[] { "Periods with return of 0% not included in distribution" };
            this.chartDistReturns.Header.Visible = false;
            this.chartDistReturns.Legend.Visible = false;
            this.chartDistReturns.Location = new Point(3, 3);
            this.chartDistReturns.Name = "chartDistReturns";
            this.chartDistReturns.Panel.Brush.Color = Color.FromArgb(0xff, 0xff, 0xff);
            this.chartDistReturns.Series.Add(this.bar_1);
            this.chartDistReturns.Size = new Size(0x18f, 0xb0);
            this.chartDistReturns.TabIndex = 0;
            this.bar_1.Brush.Color = Color.FromArgb(0x44, 0x66, 0xa3);
            this.bar_1.Color = Color.FromArgb(0x44, 0x66, 0xa3);
            this.bar_1.ColorEach = false;
            this.bar_1.Marks.Arrow.Color = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.bar_1.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.bar_1.Marks.Callout.ArrowHeadSize = 8;
            this.bar_1.Marks.Callout.Brush.Color = Color.Black;
            this.bar_1.Marks.Callout.Distance = 0;
            this.bar_1.Marks.Callout.Draw3D = false;
            this.bar_1.Marks.Callout.Length = 20;
            this.bar_1.Marks.Callout.Style = PointerStyles.Rectangle;
            this.bar_1.Marks.Callout.Visible = false;
            this.bar_1.Pen.Color = Color.FromArgb(0x29, 0x3d, 0x62);
            this.bar_1.Title = "bar1";
            this.bar_1.XValues.DataMember = "X";
            this.bar_1.XValues.Order = ValueListOrder.Ascending;
            this.bar_1.YValues.DataMember = "Bar";
            this.pnlByPeriodStats.Controls.Add(this.lblConsecUnprofitableValue);
            this.pnlByPeriodStats.Controls.Add(this.lblConsecUnprofitable);
            this.pnlByPeriodStats.Controls.Add(this.lblConsecProfitableValue);
            this.pnlByPeriodStats.Controls.Add(this.lblWorst);
            this.pnlByPeriodStats.Controls.Add(this.lblPctProfitableValue);
            this.pnlByPeriodStats.Controls.Add(this.lblPctProfitable);
            this.pnlByPeriodStats.Controls.Add(this.lblProfitableValue);
            this.pnlByPeriodStats.Controls.Add(this.lblProfitable);
            this.pnlByPeriodStats.Controls.Add(this.lblPeriodsValue);
            this.pnlByPeriodStats.Controls.Add(this.lblPeriod);
            this.pnlByPeriodStats.Controls.Add(this.lblWorstValue);
            this.pnlByPeriodStats.Controls.Add(this.lblConsecProfitable);
            this.pnlByPeriodStats.Controls.Add(this.lblBestValue);
            this.pnlByPeriodStats.Controls.Add(this.lblBest);
            this.pnlByPeriodStats.Controls.Add(this.lblSharpeValue);
            this.pnlByPeriodStats.Controls.Add(this.lblSharpe);
            this.pnlByPeriodStats.Controls.Add(this.lblStdDevValue);
            this.pnlByPeriodStats.Controls.Add(this.lblStdDev);
            this.pnlByPeriodStats.Controls.Add(this.lblAvgReturnValue);
            this.pnlByPeriodStats.Controls.Add(this.lblAvgReturn);
            this.pnlByPeriodStats.Controls.Add(this.cmbPeriod);
            this.pnlByPeriodStats.Controls.Add(this.lblSelectPeriod);
            this.pnlByPeriodStats.Dock = DockStyle.Left;
            this.pnlByPeriodStats.Location = new Point(0, 0);
            this.pnlByPeriodStats.Name = "pnlByPeriodStats";
            this.pnlByPeriodStats.Size = new Size(0xd4, 0xd0);
            this.pnlByPeriodStats.TabIndex = 0;
            this.lblConsecUnprofitableValue.Location = new Point(0x91, 0xb8);
            this.lblConsecUnprofitableValue.Name = "lblConsecUnprofitableValue";
            this.lblConsecUnprofitableValue.Size = new Size(0x38, 13);
            this.lblConsecUnprofitableValue.TabIndex = 0x15;
            this.lblConsecUnprofitableValue.Text = "0";
            this.lblConsecUnprofitableValue.TextAlign = ContentAlignment.TopRight;
            this.lblConsecUnprofitable.AutoSize = true;
            this.lblConsecUnprofitable.Location = new Point(7, 0xb8);
            this.lblConsecUnprofitable.Name = "lblConsecUnprofitable";
            this.lblConsecUnprofitable.Size = new Size(0x81, 13);
            this.lblConsecUnprofitable.TabIndex = 20;
            this.lblConsecUnprofitable.Text = "Max Consec Unprofitable:";
            this.lblConsecProfitableValue.Location = new Point(0x91, 0xab);
            this.lblConsecProfitableValue.Name = "lblConsecProfitableValue";
            this.lblConsecProfitableValue.Size = new Size(0x38, 13);
            this.lblConsecProfitableValue.TabIndex = 0x13;
            this.lblConsecProfitableValue.Text = "0";
            this.lblConsecProfitableValue.TextAlign = ContentAlignment.TopRight;
            this.lblWorst.AutoSize = true;
            this.lblWorst.Location = new Point(7, 100);
            this.lblWorst.Name = "lblWorst";
            this.lblWorst.Size = new Size(0x49, 13);
            this.lblWorst.TabIndex = 0x12;
            this.lblWorst.Text = "Worst Return:";
            this.lblPctProfitableValue.Location = new Point(0x91, 0x94);
            this.lblPctProfitableValue.Name = "lblPctProfitableValue";
            this.lblPctProfitableValue.Size = new Size(0x38, 13);
            this.lblPctProfitableValue.TabIndex = 0x11;
            this.lblPctProfitableValue.Text = "0.00%";
            this.lblPctProfitableValue.TextAlign = ContentAlignment.TopRight;
            this.lblPctProfitable.AutoSize = true;
            this.lblPctProfitable.Location = new Point(7, 0x94);
            this.lblPctProfitable.Name = "lblPctProfitable";
            this.lblPctProfitable.Size = new Size(0x67, 13);
            this.lblPctProfitable.TabIndex = 0x10;
            this.lblPctProfitable.Text = "% Profitable Periods:";
            this.lblProfitableValue.Location = new Point(0x91, 0x87);
            this.lblProfitableValue.Name = "lblProfitableValue";
            this.lblProfitableValue.Size = new Size(0x38, 13);
            this.lblProfitableValue.TabIndex = 15;
            this.lblProfitableValue.Text = "0";
            this.lblProfitableValue.TextAlign = ContentAlignment.TopRight;
            this.lblProfitable.AutoSize = true;
            this.lblProfitable.Location = new Point(7, 0x87);
            this.lblProfitable.Name = "lblProfitable";
            this.lblProfitable.Size = new Size(0x5c, 13);
            this.lblProfitable.TabIndex = 14;
            this.lblProfitable.Text = "Profitable Periods:";
            this.lblPeriodsValue.Location = new Point(0x91, 0x7a);
            this.lblPeriodsValue.Name = "lblPeriodsValue";
            this.lblPeriodsValue.Size = new Size(0x38, 13);
            this.lblPeriodsValue.TabIndex = 13;
            this.lblPeriodsValue.Text = "0";
            this.lblPeriodsValue.TextAlign = ContentAlignment.TopRight;
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new Point(7, 0x7a);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new Size(0x61, 13);
            this.lblPeriod.TabIndex = 12;
            this.lblPeriod.Text = "Number of Periods:";
            this.lblWorstValue.Location = new Point(0x91, 100);
            this.lblWorstValue.Name = "lblWorstValue";
            this.lblWorstValue.Size = new Size(0x38, 13);
            this.lblWorstValue.TabIndex = 11;
            this.lblWorstValue.Text = "0.00%";
            this.lblWorstValue.TextAlign = ContentAlignment.TopRight;
            this.lblConsecProfitable.AutoSize = true;
            this.lblConsecProfitable.Location = new Point(7, 0xab);
            this.lblConsecProfitable.Name = "lblConsecProfitable";
            this.lblConsecProfitable.Size = new Size(0x74, 13);
            this.lblConsecProfitable.TabIndex = 10;
            this.lblConsecProfitable.Text = "Max Consec Profitable:";
            this.lblBestValue.Location = new Point(0x91, 0x57);
            this.lblBestValue.Name = "lblBestValue";
            this.lblBestValue.Size = new Size(0x38, 13);
            this.lblBestValue.TabIndex = 9;
            this.lblBestValue.Text = "0.00%";
            this.lblBestValue.TextAlign = ContentAlignment.TopRight;
            this.lblBest.AutoSize = true;
            this.lblBest.Location = new Point(7, 0x57);
            this.lblBest.Name = "lblBest";
            this.lblBest.Size = new Size(0x42, 13);
            this.lblBest.TabIndex = 8;
            this.lblBest.Text = "Best Return:";
            this.lblSharpeValue.Location = new Point(0x91, 0x41);
            this.lblSharpeValue.Name = "lblSharpeValue";
            this.lblSharpeValue.Size = new Size(0x38, 13);
            this.lblSharpeValue.TabIndex = 7;
            this.lblSharpeValue.Text = "0.00";
            this.lblSharpeValue.TextAlign = ContentAlignment.TopRight;
            this.lblSharpe.AutoSize = true;
            this.lblSharpe.Location = new Point(7, 0x41);
            this.lblSharpe.Name = "lblSharpe";
            this.lblSharpe.Size = new Size(0x48, 13);
            this.lblSharpe.TabIndex = 6;
            this.lblSharpe.Text = "Sharpe Ratio:";
            this.lblStdDevValue.Location = new Point(0x8e, 0x34);
            this.lblStdDevValue.Name = "lblStdDevValue";
            this.lblStdDevValue.Size = new Size(0x3b, 13);
            this.lblStdDevValue.TabIndex = 5;
            this.lblStdDevValue.Text = "0.00";
            this.lblStdDevValue.TextAlign = ContentAlignment.TopRight;
            this.lblStdDev.AutoSize = true;
            this.lblStdDev.Location = new Point(7, 0x34);
            this.lblStdDev.Name = "lblStdDev";
            this.lblStdDev.Size = new Size(0x81, 13);
            this.lblStdDev.TabIndex = 4;
            this.lblStdDev.Text = "Std. Deviation of Returns:";
            this.lblAvgReturnValue.Location = new Point(0x8e, 0x27);
            this.lblAvgReturnValue.Name = "lblAvgReturnValue";
            this.lblAvgReturnValue.Size = new Size(0x3b, 13);
            this.lblAvgReturnValue.TabIndex = 3;
            this.lblAvgReturnValue.Text = "0.00%";
            this.lblAvgReturnValue.TextAlign = ContentAlignment.TopRight;
            this.lblAvgReturn.AutoSize = true;
            this.lblAvgReturn.Location = new Point(7, 0x27);
            this.lblAvgReturn.Name = "lblAvgReturn";
            this.lblAvgReturn.Size = new Size(0x55, 13);
            this.lblAvgReturn.TabIndex = 2;
            this.lblAvgReturn.Text = "Average Return:";
            this.cmbPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.Items.AddRange(new object[] { "Daily", "Weekly", "Monthly", "Quarterly", "Annually" });
            this.cmbPeriod.Location = new Point(0x62, 4);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new Size(0x67, 0x15);
            this.cmbPeriod.TabIndex = 1;
            this.cmbPeriod.SelectedIndexChanged += new EventHandler(this.cmbPeriod_SelectedIndexChanged);
            this.lblSelectPeriod.AutoSize = true;
            this.lblSelectPeriod.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblSelectPeriod.Location = new Point(4, 4);
            this.lblSelectPeriod.Name = "lblSelectPeriod";
            this.lblSelectPeriod.Size = new Size(0x57, 13);
            this.lblSelectPeriod.TabIndex = 0;
            this.lblSelectPeriod.Text = "Select Period:";
            this.lvReturns.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_1, this.columnHeader_2, this.columnHeader_3, this.columnHeader_4, this.columnHeader_5, this.columnHeader_6 });
            this.lvReturns.ContextMenuStrip = this.popup_1;
            this.lvReturns.Dock = DockStyle.Fill;
            this.lvReturns.Location = new Point(0, 0);
            this.lvReturns.Name = "lvReturns";
            this.lvReturns.Size = new Size(0x271, 0xce);
            this.lvReturns.TabIndex = 0;
            this.lvReturns.UseCompatibleStateImageBehavior = false;
            this.lvReturns.View = View.Details;
            this.columnHeader_0.Tag = "D";
            this.columnHeader_0.Text = "Period Starting";
            this.columnHeader_0.Width = 90;
            this.columnHeader_1.Tag = "C";
            this.columnHeader_1.Text = "Return";
            this.columnHeader_1.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_1.Width = 80;
            this.columnHeader_2.Tag = "N";
            this.columnHeader_2.Text = "Return %";
            this.columnHeader_2.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_2.Width = 80;
            this.columnHeader_3.Tag = "N";
            this.columnHeader_3.Text = "Max DD %";
            this.columnHeader_3.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_3.Width = 80;
            this.columnHeader_4.Tag = "N";
            this.columnHeader_4.Text = "Exposure %";
            this.columnHeader_4.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_4.Width = 80;
            this.columnHeader_5.Tag = "N";
            this.columnHeader_5.Text = "Entries";
            this.columnHeader_5.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_5.Width = 80;
            this.columnHeader_6.Tag = "N";
            this.columnHeader_6.Text = "Exits";
            this.columnHeader_6.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_6.Width = 80;
            this.popup_1.Items.AddRange(new ToolStripItem[] { this.mniCopyData, this.mniPrintData, this.mniPrintAll2 });
            this.popup_1.Name = "popup";
            this.popup_1.Size = new Size(0x89, 70);
            this.mniCopyData.Image = (Image) manager.GetObject("mniCopyData.Image");
            this.mniCopyData.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyData.Name = "mniCopyData";
            this.mniCopyData.Size = new Size(0x88, 0x16);
            this.mniCopyData.Text = "Copy Data";
            this.mniCopyData.ToolTipText = "Copy the report to the clipboard";
            this.mniCopyData.Click += new EventHandler(this.mniCopyData_Click);
            this.mniPrintData.Image = (Image) manager.GetObject("mniPrintData.Image");
            this.mniPrintData.Name = "mniPrintData";
            this.mniPrintData.Size = new Size(0x88, 0x16);
            this.mniPrintData.Text = "Print";
            this.mniPrintData.ToolTipText = "Print By Period results";
            this.mniPrintData.Click += new EventHandler(this.mniPrintData_Click);
            this.mniPrintAll2.Name = "mniPrintAll2";
            this.mniPrintAll2.Size = new Size(0x88, 0x16);
            this.mniPrintAll2.Text = "Print All";
            this.mniPrintAll2.ToolTipText = "Print content from all tabs";
            this.mniPrintAll2.Click += new EventHandler(this.mniPrintAll2_Click);
            this.distribution_0.BinsDesired = 30;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.splitByPeriod);
            base.Name = "PVByPeriod";
            base.Size = new Size(0x271, 0x1a2);
            this.splitByPeriod.Panel1.ResumeLayout(false);
            this.splitByPeriod.Panel2.ResumeLayout(false);
            this.splitByPeriod.ResumeLayout(false);
            this.tabByPeriod.ResumeLayout(false);
            this.popup.ResumeLayout(false);
            this.pageReturns.ResumeLayout(false);
            this.pageDistribution.ResumeLayout(false);
            this.pnlByPeriodStats.ResumeLayout(false);
            this.pnlByPeriodStats.PerformLayout();
            this.popup_1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void method_0()
        {
            if ((this.cmbPeriod.SelectedIndex != -1) && (this.dataSeries_0.Count >= 2))
            {
                this.lvReturns.BeginUpdate();
                this.lvReturns.Items.Clear();
                this.string_0 = this.cmbPeriod.Text;
                int num10 = 1;
                DateTime now = DateTime.Now;
                this.dataSeries_2 = new DataSeries("PvByPeriod Returns");
                this.dataSeries_3 = new DataSeries("PvByPeriod Returns Dollar");
                DateTime time = this.dataSeries_0.Date[0];
                int num = 0;
                for (int i = 1; i < this.dataSeries_0.Count; i++)
                {
                    DateTime time2 = this.dataSeries_0.Date[i];
                    bool flag = false;
                    string str = this.string_0;
                    if (str != null)
                    {
                        if (str == "Daily")
                        {
                            flag = time.Day != time2.Day;
                            num10 = 0x16d;
                        }
                        else if (str == "Weekly")
                        {
                            DateTime time3 = this.dataSeries_0.Date[i - 1];
                            flag = (((time2.DayOfWeek < time3.DayOfWeek) || (this.barScale_0 == BarScale.Weekly)) || ((this.barScale_0 == BarScale.Monthly) || (this.barScale_0 == BarScale.Quarterly))) || (this.barScale_0 == BarScale.Yearly);
                            num10 = 0x34;
                        }
                        else if (str == "Monthly")
                        {
                            flag = (((time2.Month != time.Month) || (this.barScale_0 == BarScale.Monthly)) || (this.barScale_0 == BarScale.Quarterly)) || (this.barScale_0 == BarScale.Yearly);
                            num10 = 12;
                        }
                        else if (!(str == "Quarterly"))
                        {
                            if (str == "Annually")
                            {
                                flag = (time2.Year != time.Year) || (this.barScale_0 == BarScale.Yearly);
                            }
                        }
                        else
                        {
                            flag = (((time2.Month != time.Month) && (((time2.Month - 1) % 3) == 0)) || (this.barScale_0 == BarScale.Quarterly)) || (this.barScale_0 == BarScale.Yearly);
                            num10 = 4;
                        }
                    }
                    if (flag)
                    {
                        this.method_3(num, i - 1);
                        num = i - 1;
                        time = this.dataSeries_0.Date[i];
                    }
                }
                this.method_3(num, this.dataSeries_0.Count - 1);
                this.lvReturns.EndUpdate();
                int num7 = this.dataSeries_2.Count - 1;
                double num8 = SMA.Value(num7, this.dataSeries_2, this.dataSeries_2.Count);
                this.lblAvgReturnValue.Text = num8.ToString("N2") + "%";
                double num9 = StdDev.Value(num7, this.dataSeries_2, this.dataSeries_2.Count, StdDevCalculation.Population);
                this.lblStdDevValue.Text = num9.ToString("N2");
                num8 *= num10;
                num9 *= Math.Sqrt((double) num10);
                this.lblSharpeValue.Text = ((num8 - this.systemPerformance_0.CashReturnRate) / num9).ToString("N2");
                this.lblBestValue.Text = Highest.Value(num7, this.dataSeries_2, this.dataSeries_2.Count).ToString("N2") + "%";
                this.lblWorstValue.Text = Lowest.Value(num7, this.dataSeries_2, this.dataSeries_2.Count).ToString("N2") + "%";
                this.lblPeriodsValue.Text = this.dataSeries_2.Count.ToString();
                int num15 = 0;
                int num5 = 0;
                int num4 = 0;
                int num16 = 0;
                int num6 = 0;
                for (int j = 0; j < this.dataSeries_2.Count; j++)
                {
                    if (this.dataSeries_2[j] > 0.0)
                    {
                        num15++;
                        num5++;
                        num4 = 0;
                        if (num5 > num16)
                        {
                            num16 = num5;
                        }
                    }
                    else if (this.dataSeries_2[j] < 0.0)
                    {
                        num4++;
                        num5 = 0;
                        if (num4 > num6)
                        {
                            num6 = num4;
                        }
                    }
                    else
                    {
                        num5 = 0;
                        num4 = 0;
                    }
                }
                this.lblProfitableValue.Text = num15.ToString();
                this.lblPctProfitableValue.Text = (((num15 * 100.0) / ((double) this.dataSeries_2.Count))).ToString("N2") + "%";
                this.lblConsecProfitableValue.Text = num16.ToString();
                this.lblConsecUnprofitableValue.Text = num6.ToString();
                this.method_4();
                this.method_2();
                this.method_1();
            }
        }

        private void method_1()
        {
            if ((this.dataSeries_2 != null) && (this.dataSeries_3 != null))
            {
                Color red;
                this.bar_1.Clear();
                this.bar_1.Pen.Visible = false;
                if (this.cmbChartUnits.Text == string_1)
                {
                    for (int i = 0; i < this.dataSeries_2.Count; i++)
                    {
                        if (this.dataSeries_2[i] != 0.0)
                        {
                            this.distribution_0.AddValue(this.dataSeries_2[i]);
                        }
                    }
                    this.distribution_0.MakeBins();
                    foreach (Bin bin2 in this.distribution_0.Bins)
                    {
                        red = Color.Red;
                        if (bin2.Low >= 0.0)
                        {
                            red = Color.Blue;
                        }
                        this.bar_1.Add((double) (bin2.High / 100.0), (double) bin2.Count, red);
                    }
                }
                else
                {
                    for (int j = 0; j < this.dataSeries_3.Count; j++)
                    {
                        if (this.dataSeries_3[j] != 0.0)
                        {
                            this.distribution_0.AddValue(this.dataSeries_3[j]);
                        }
                    }
                    this.distribution_0.MakeBins();
                    foreach (Bin bin in this.distribution_0.Bins)
                    {
                        red = Color.Red;
                        if (bin.Low >= 0.0)
                        {
                            red = Color.Blue;
                        }
                        this.bar_1.Add(bin.High, (double) bin.Count, red);
                    }
                }
            }
        }

        private void method_2()
        {
            if ((this.dataSeries_2 != null) && (this.dataSeries_3 != null))
            {
                Color blue;
                this.bar_0.Clear();
                if (this.cmbChartUnits.Text == string_1)
                {
                    for (int i = 0; i < this.dataSeries_2.Count; i++)
                    {
                        if (this.dataSeries_2[i] > 0.0)
                        {
                            blue = Color.Blue;
                        }
                        else
                        {
                            blue = Color.Red;
                        }
                        DateTime time2 = this.dataSeries_2.Date[i];
                        string text = time2.ToShortDateString();
                        string str4 = this.string_0;
                        if (str4 != null)
                        {
                            if (!(str4 == "Monthly") && !(str4 == "Quarterly"))
                            {
                                if (str4 == "Annually")
                                {
                                    text = time2.Year.ToString();
                                }
                            }
                            else
                            {
                                text = time2.Month.ToString("00") + "/" + time2.Year.ToString();
                            }
                        }
                        this.bar_0.Add((double) (this.dataSeries_2[i] / 100.0), text, blue);
                    }
                }
                else
                {
                    for (int j = 0; j < this.dataSeries_3.Count; j++)
                    {
                        if (this.dataSeries_3[j] > 0.0)
                        {
                            blue = Color.Blue;
                        }
                        else
                        {
                            blue = Color.Red;
                        }
                        DateTime time = this.dataSeries_3.Date[j];
                        string str = time.ToShortDateString();
                        string str2 = this.string_0;
                        if (str2 != null)
                        {
                            if (!(str2 == "Monthly") && !(str2 == "Quarterly"))
                            {
                                if (str2 == "Annually")
                                {
                                    str = time.Year.ToString();
                                }
                            }
                            else
                            {
                                str = time.Month.ToString("00") + "/" + time.Year.ToString();
                            }
                        }
                        this.bar_0.Add(this.dataSeries_3[j], str, blue);
                    }
                }
            }
        }

        private void method_3(int int_3, int int_4)
        {
            double num = this.dataSeries_0[int_4] - this.dataSeries_0[int_3];
            double num2 = (num * 100.0) / this.dataSeries_0[int_3];
            DateTime time = this.dataSeries_0.Date[int_4];
            string str = this.string_0;
            if (str != null)
            {
                if (!(str == "Monthly") && !(str == "Quarterly"))
                {
                    if (str == "Annually")
                    {
                        time = new DateTime(time.Year, 1, 1);
                    }
                }
                else
                {
                    time = new DateTime(time.Year, time.Month, 1);
                }
            }
            this.dataSeries_2.Add(num2, time);
            this.dataSeries_3.Add(num, time);
            DateTime time4 = this.dataSeries_0.Date[int_3 + 1];
            ListViewItem item = this.lvReturns.Items.Add(time4.ToShortDateString());
            item.SubItems.Add(num.ToString("C"));
            item.SubItems.Add(num2.ToString("N2"));
            double num6 = this.dataSeries_0[int_3];
            double num9 = 0.0;
            double num12 = 0.0;
            double num4 = 0.0;
            double num5 = 0.0;
            for (int i = int_3 + 1; i <= int_4; i++)
            {
                num4 += this.dataSeries_1[i];
                num5 += this.dataSeries_0[i];
                if (this.dataSeries_0[i] > num6)
                {
                    num6 = this.dataSeries_0[i];
                }
                num12 = ((this.dataSeries_0[i] - num6) * 100.0) / num6;
                if (num12 < num9)
                {
                    num9 = num12;
                }
            }
            item.SubItems.Add(num9.ToString("N2"));
            double num10 = ((num5 - num4) * 100.0) / num5;
            item.SubItems.Add(num10.ToString("N2"));
            int num8 = 0;
            int num7 = 0;
            DateTime time3 = this.dataSeries_0.Date[int_3 + 1];
            DateTime time2 = this.dataSeries_0.Date[int_4];
            foreach (WealthLab.Position position in this.ilist_0)
            {
                if (position.EntryDate > time2)
                {
                    break;
                }
                if ((position.EntryDate >= time3) && (position.EntryDate <= time2))
                {
                    num8++;
                }
                if ((!position.Active && (position.ExitDate >= time3)) && (position.ExitDate <= time2))
                {
                    num7++;
                }
            }
            item.SubItems.Add(num8.ToString());
            item.SubItems.Add(num7.ToString());
        }

        private void method_4()
        {
            if (this.tabByPeriod.SelectedTab.Name == "pageDistribution")
            {
                this.chartDistReturns.Axes.Bottom.Automatic = true;
                if (this.cmbChartUnits.Text == string_1)
                {
                    this.chartDistReturns.Axes.Bottom.Labels.ValueFormat = this.bar_1.PercentFormat;
                }
                else
                {
                    this.chartDistReturns.Axes.Bottom.Labels.ValueFormat = this.bar_1.ValueFormat;
                }
            }
            else
            {
                this.chartReturns.Axes.Left.Automatic = true;
                if (this.cmbChartUnits.Text == string_1)
                {
                    this.chartReturns.Axes.Left.Labels.ValueFormat = this.bar_0.PercentFormat;
                }
                else
                {
                    this.chartReturns.Axes.Left.Labels.ValueFormat = this.bar_0.ValueFormat;
                }
            }
        }

        private void mniCopyChart_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tabByPeriod.SelectedTab.Name == "pageDistribution")
                {
                    if (this.chartDistReturns.Bitmap != null)
                    {
                        Clipboard.SetImage(this.chartDistReturns.Bitmap);
                    }
                }
                else if (this.chartReturns.Bitmap != null)
                {
                    Clipboard.SetImage(this.chartReturns.Bitmap);
                }
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void mniCopyData_Click(object sender, EventArgs e)
        {
            this.CopyToClipboard();
        }

        private void mniPrintAll2_Click(object sender, EventArgs e)
        {
            this.ivisualizerHost_0.PrintAll();
        }

        private void mniPrintData_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        public void Print()
        {
            PrintDocument prtdoc = new PrintDocument();
            prtdoc.BeginPrint += new PrintEventHandler(this.byPeriodReport_BeginPrint);
            prtdoc.PrintPage += new PrintPageEventHandler(this.byPeriodReport_PrintPage);
            prtdoc.EndPrint += new PrintEventHandler(this.byPeriodReport_EndPrint);
            this.ivisualizerHost_0.GetPageSettings(ref this.pageSettings_0);
            prtdoc.DefaultPageSettings = this.pageSettings_0;
            if (this.ivisualizerHost_0.ShowPrintPreview())
            {
                this.printPreview_0 = new PrintPreview(prtdoc);
                this.printPreview_0.ShowPrintDialog = this.ivisualizerHost_0.ShowPrintDialog();
                this.printPreview_0.ShowDialog();
            }
            else
            {
                PrintDialog dialog = new PrintDialog {
                    Document = prtdoc
                };
                if (this.ivisualizerHost_0.ShowPrintDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        dialog.Document.Print();
                    }
                }
                else
                {
                    dialog.Document.Print();
                }
                dialog.Dispose();
            }
            prtdoc.Dispose();
        }

        private void tabByPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.bool_0 = false;
            if (this.tabByPeriod.SelectedTab.Name == "pageDistribution")
            {
                this.cmbChartUnits.Text = this.string_3;
            }
            else
            {
                this.cmbChartUnits.Text = this.string_2;
            }
            this.bool_0 = true;
        }

        public VisualizerAppliesTo AppliesTo
        {
            get
            {
                return (VisualizerAppliesTo.PortfolioSim | VisualizerAppliesTo.MultiSymbol | VisualizerAppliesTo.SingleSymbol);
            }
        }

        public string Description
        {
            get
            {
                return "Displays the Strategy performance broken down by day, week, month, quarter, or year.";
            }
        }

        public string SettingsString
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                if (this.cmbPeriod.SelectedItem != null)
                {
                    builder.Append("Perod=");
                    builder.Append(this.cmbPeriod.SelectedItem.ToString());
                }
                builder.Append(";Tab=");
                builder.Append(this.tabByPeriod.SelectedIndex.ToString());
                builder.Append(";ReturnsUnits=");
                builder.Append(this.string_2);
                builder.Append(";DistReturnsUnits=");
                builder.Append(this.string_3);
                return builder.ToString();
            }
            set
            {
                string[] strArray = value.Split(new char[] { ';' });
                int result = 0;
                string str2 = string_1;
                string str3 = string_1;
                for (int i = 0; i < strArray.Length; i++)
                {
                    string[] strArray2 = strArray[i].Split(new char[] { '=' });
                    if (strArray2.Length >= 2)
                    {
                        if (strArray2[0] == "Perod")
                        {
                            this.cmbPeriod.Text = strArray2[1];
                        }
                        else if (strArray2[0] == "Tab")
                        {
                            int.TryParse(strArray2[1], out result);
                        }
                        else if (strArray2[0] == "ReturnsUnits")
                        {
                            str2 = strArray2[1];
                        }
                        else if (strArray2[0] == "DistReturnsUnits")
                        {
                            str3 = strArray2[1];
                        }
                    }
                }
                this.cmbChartUnits.Text = str2;
                this.tabByPeriod.SelectedIndex = 1;
                this.cmbChartUnits.Text = str3;
                this.tabByPeriod.SelectedIndex = result;
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
                return "By Period";
            }
        }
    }
}

