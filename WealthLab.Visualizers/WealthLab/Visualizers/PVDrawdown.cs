namespace WealthLab.Visualizers
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class PVDrawdown : UserControl, IPerformanceVisualizer, ISettingsProvider
    {
        private Area area_0;
        private Area area_1;
        private Area area_2;
        private TChart chartBarsSince;
        private TChart chartDrawdown;
        private ToolStripComboBox cmbChartUnit;
        private ToolStripComboBox cmbStrategies;
        private IContainer icontainer_0;
        private IVisualizerHost ivisualizerHost_0;
        private ToolStripMenuItem mniCopyBarsSince;
        private ToolStripMenuItem mniCopyCharts;
        private ToolStripMenuItem mniCopyCharts2;
        private ToolStripMenuItem mniCopyDrawdown;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrint2;
        private ToolStripMenuItem mniPrintAll;
        private ToolStripMenuItem mniPrintAll2;
        private PageSettings pageSettings_0 = new PageSettings();
        private ContextMenuStrip popup;
        private ContextMenuStrip popup_1;
        private SplitContainer splitContainer1;
        private SystemPerformance systemPerformance_0;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator1;

        public PVDrawdown()
        {
            this.InitializeComponent();
        }

        private void cmbChartUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.method_2();
        }

        private void cmbStrategies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbStrategies.SelectedIndex == 0)
            {
                this.method_0(this.systemPerformance_0);
            }
            else
            {
                CombinedStrategyInfo selectedItem = (CombinedStrategyInfo) this.cmbStrategies.SelectedItem;
                this.method_0(this.systemPerformance_0.GenerateChildStrategyPerformance(selectedItem, this.ivisualizerHost_0.GetExecutor()));
            }
        }

        public void CopyToClipboard()
        {
            Bitmap image = new Bitmap(this.chartDrawdown.Width, (this.chartDrawdown.Height + this.chartBarsSince.Height) + 3);
            Graphics graphics = Graphics.FromImage(image);
            graphics.DrawImageUnscaled(this.chartDrawdown.Bitmap, 0, 0);
            graphics.DrawImageUnscaled(this.chartBarsSince.Bitmap, 0, this.chartDrawdown.Height + 3);
            try
            {
                Clipboard.SetImage(image);
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public void CreateVisualization(SystemPerformance performance, IVisualizerHost visHost)
        {
            this.ivisualizerHost_0 = visHost;
            this.systemPerformance_0 = performance;
            if (this.chartDrawdown.Zoom.Zoomed || this.chartBarsSince.Zoom.Zoomed)
            {
                this.chartDrawdown.Zoom.Undo();
                this.chartBarsSince.Zoom.Undo();
            }
            this.method_0(performance);
            if (performance.Strategy.StrategyType == StrategyType.CombinedStrategy)
            {
                this.cmbStrategies.Visible = true;
                this.toolStripLabel1.Visible = true;
                this.cmbStrategies.Items.Clear();
                this.cmbStrategies.Items.Add("Strategies in Aggregate");
                foreach (CombinedStrategyInfo info in performance.Strategy.CombinedStrategyChildren)
                {
                    this.cmbStrategies.Items.Add(info);
                }
                this.cmbStrategies.SelectedIndex = 0;
            }
            else
            {
                this.cmbStrategies.Visible = false;
                this.toolStripLabel1.Visible = false;
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

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PVDrawdown));
            this.splitContainer1 = new SplitContainer();
            this.popup = new ContextMenuStrip(this.icontainer_0);
            this.cmbChartUnit = new ToolStripComboBox();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.mniCopyDrawdown = new ToolStripMenuItem();
            this.mniCopyCharts = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.mniPrintAll = new ToolStripMenuItem();
            this.chartBarsSince = new TChart();
            this.popup_1 = new ContextMenuStrip(this.icontainer_0);
            this.mniCopyBarsSince = new ToolStripMenuItem();
            this.mniCopyCharts2 = new ToolStripMenuItem();
            this.mniPrint2 = new ToolStripMenuItem();
            this.mniPrintAll2 = new ToolStripMenuItem();
            this.area_0 = new Area();
            this.area_2 = new Area();
            this.area_1 = new Area();
            this.chartDrawdown = new TChart();
            this.toolStrip1 = new ToolStrip();
            this.cmbStrategies = new ToolStripComboBox();
            this.toolStripLabel1 = new ToolStripLabel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.popup.SuspendLayout();
            this.popup_1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.splitContainer1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainer1.Location = new Point(0, 0x15);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            this.splitContainer1.Panel1.Controls.Add(this.chartDrawdown);
            this.splitContainer1.Panel2.Controls.Add(this.chartBarsSince);
            this.splitContainer1.Size = new Size(0x227, 0x16f);
            this.splitContainer1.SplitterDistance = 0xc2;
            this.splitContainer1.TabIndex = 0;
            this.popup.Items.AddRange(new ToolStripItem[] { this.cmbChartUnit, this.toolStripSeparator1, this.mniCopyDrawdown, this.mniCopyCharts, this.mniPrint, this.mniPrintAll });
            this.popup.Name = "popup";
            this.popup.Size = new Size(0xc3, 0x7b);
            this.cmbChartUnit.Items.AddRange(new object[] { "Dollar", "Percent" });
            this.cmbChartUnit.Name = "cmbChartUnit";
            this.cmbChartUnit.Size = new Size(0x79, 0x15);
            this.cmbChartUnit.Text = "Percent";
            this.cmbChartUnit.SelectedIndexChanged += new EventHandler(this.cmbChartUnit_SelectedIndexChanged);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0xbf, 6);
            this.mniCopyDrawdown.Image = (Image) manager.GetObject("mniCopyDrawdown.Image");
            this.mniCopyDrawdown.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyDrawdown.Name = "mniCopyDrawdown";
            this.mniCopyDrawdown.Size = new Size(0xc2, 0x16);
            this.mniCopyDrawdown.Text = "Copy Drawdown Chart";
            this.mniCopyDrawdown.ToolTipText = "Copy the Drawdown chart to the clipboard";
            this.mniCopyDrawdown.Click += new EventHandler(this.mniCopyDrawdown_Click);
            this.mniCopyCharts.Name = "mniCopyCharts";
            this.mniCopyCharts.Size = new Size(0xc2, 0x16);
            this.mniCopyCharts.Text = "Copy Both Charts";
            this.mniCopyCharts.ToolTipText = "Copy the Drawdown and Bars Since charts to the clipboard";
            this.mniCopyCharts.Click += new EventHandler(this.mniCopyCharts2_Click);
            this.mniPrint.Image = (Image) manager.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0xc2, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.ToolTipText = "Print the charts";
            this.mniPrint.Click += new EventHandler(this.mniPrint2_Click);
            this.mniPrintAll.Name = "mniPrintAll";
            this.mniPrintAll.Size = new Size(0xc2, 0x16);
            this.mniPrintAll.Text = "Print All";
            this.mniPrintAll.ToolTipText = "Print content from all tabs";
            this.mniPrintAll.Click += new EventHandler(this.mniPrintAll2_Click);
            this.chartBarsSince.Aspect.View3D = false;
            this.chartBarsSince.Aspect.ZOffset = 0.0;
            this.chartBarsSince.Axes.Bottom.Grid.Visible = false;
            this.chartBarsSince.Axes.Bottom.Title.Transparent = true;
            this.chartBarsSince.Axes.Depth.Title.Transparent = true;
            this.chartBarsSince.Axes.DepthTop.Title.Transparent = true;
            this.chartBarsSince.Axes.Left.Grid.Visible = false;
            this.chartBarsSince.Axes.Left.Title.Caption = "Number of Bars";
            this.chartBarsSince.Axes.Left.Title.Lines = new string[] { "Number of Bars" };
            this.chartBarsSince.Axes.Left.Title.Transparent = true;
            this.chartBarsSince.Axes.Right.Title.Transparent = true;
            this.chartBarsSince.Axes.Top.Title.Transparent = true;
            this.chartBarsSince.ContextMenuStrip = this.popup_1;
            this.chartBarsSince.Dock = DockStyle.Fill;
            this.chartBarsSince.Header.Font.Brush.Color = Color.FromArgb(0, 0, 0);
            this.chartBarsSince.Header.Lines = new string[] { "Number of Bars since last Equity High" };
            this.chartBarsSince.Legend.Visible = false;
            this.chartBarsSince.Location = new Point(0, 0);
            this.chartBarsSince.Name = "chartBarsSince";
            this.chartBarsSince.Panel.Brush.Color = Color.FromArgb(0xff, 0xff, 0xff);
            this.chartBarsSince.Series.Add(this.area_0);
            this.chartBarsSince.Size = new Size(0x227, 0xa9);
            this.chartBarsSince.TabIndex = 0;
            this.popup_1.Items.AddRange(new ToolStripItem[] { this.mniCopyBarsSince, this.mniCopyCharts2, this.mniPrint2, this.mniPrintAll2 });
            this.popup_1.Name = "popup";
            this.popup_1.Size = new Size(0xc1, 0x5c);
            this.mniCopyBarsSince.Image = (Image) manager.GetObject("mniCopyBarsSince.Image");
            this.mniCopyBarsSince.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyBarsSince.Name = "mniCopyBarsSince";
            this.mniCopyBarsSince.Size = new Size(0xc0, 0x16);
            this.mniCopyBarsSince.Text = "Copy Bars Since Chart";
            this.mniCopyBarsSince.ToolTipText = "Copy Bars Since chart to clipboard";
            this.mniCopyBarsSince.Click += new EventHandler(this.mniCopyBarsSince_Click);
            this.mniCopyCharts2.Name = "mniCopyCharts2";
            this.mniCopyCharts2.Size = new Size(0xc0, 0x16);
            this.mniCopyCharts2.Text = "Copy Both Charts";
            this.mniCopyCharts2.ToolTipText = "Copy the Drawdown and Bars Since charts to the clipboard";
            this.mniCopyCharts2.Click += new EventHandler(this.mniCopyCharts2_Click);
            this.mniPrint2.Image = (Image) manager.GetObject("mniPrint2.Image");
            this.mniPrint2.Name = "mniPrint2";
            this.mniPrint2.Size = new Size(0xc0, 0x16);
            this.mniPrint2.Text = "Print";
            this.mniPrint2.ToolTipText = "Print the charts";
            this.mniPrint2.Click += new EventHandler(this.mniPrint2_Click);
            this.mniPrintAll2.Name = "mniPrintAll2";
            this.mniPrintAll2.Size = new Size(0xc0, 0x16);
            this.mniPrintAll2.Text = "Print All";
            this.mniPrintAll2.ToolTipText = "Print content from all tabs";
            this.mniPrintAll2.Click += new EventHandler(this.mniPrintAll2_Click);
            this.area_0.AreaBrush.Color = Color.FromArgb(0, 0, 0xff);
            this.area_0.Gradient.StartColor = Color.FromArgb(0, 0, 0xff);
            this.area_0.AreaLines.Color = Color.FromArgb(0, 0, 0x99);
            this.area_0.AreaLines.Visible = false;
            this.area_0.Brush.Color = Color.FromArgb(0, 0, 0xff);
            this.area_0.Color = Color.FromArgb(0, 0, 0xff);
            this.area_0.ColorEach = false;
            this.area_0.LinePen.Color = Color.FromArgb(0, 0, 0x99);
            this.area_0.LinePen.Visible = false;
            this.area_0.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.area_0.Marks.Callout.ArrowHeadSize = 8;
            this.area_0.Marks.Callout.Brush.Color = Color.Black;
            this.area_0.Marks.Callout.Distance = 0;
            this.area_0.Marks.Callout.Draw3D = false;
            this.area_0.Marks.Callout.Length = 10;
            this.area_0.Marks.Callout.Style = PointerStyles.Rectangle;
            this.area_0.Marks.Callout.Visible = false;
            this.area_0.Pointer.Style = PointerStyles.Rectangle;
            this.area_0.Title = "area1";
            this.area_0.XValues.DataMember = "X";
            this.area_0.XValues.Order = ValueListOrder.Ascending;
            this.area_0.YValues.DataMember = "Y";
            this.area_2.AreaBrush.Color = Color.FromArgb(0xff, 0, 0);
            this.area_2.Gradient.StartColor = Color.FromArgb(0xff, 0, 0);
            this.area_2.AreaLines.Color = Color.FromArgb(0x99, 0, 0);
            this.area_2.AreaLines.Visible = false;
            this.area_2.Brush.Color = Color.FromArgb(0xff, 0, 0);
            this.area_2.Color = Color.FromArgb(0xff, 0, 0);
            this.area_2.ColorEach = false;
            this.area_2.LinePen.Color = Color.FromArgb(0x99, 0, 0);
            this.area_2.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.area_2.Marks.Callout.ArrowHeadSize = 8;
            this.area_2.Marks.Callout.Brush.Color = Color.Black;
            this.area_2.Marks.Callout.Distance = 0;
            this.area_2.Marks.Callout.Draw3D = false;
            this.area_2.Marks.Callout.Length = 10;
            this.area_2.Marks.Callout.Style = PointerStyles.Rectangle;
            this.area_2.Marks.Callout.Visible = false;
            this.area_2.Pointer.Style = PointerStyles.Rectangle;
            this.area_2.Title = "area2";
            this.area_2.UseOrigin = true;
            this.area_2.XValues.DataMember = "X";
            this.area_2.XValues.Order = ValueListOrder.Ascending;
            this.area_2.YValues.DataMember = "Y";
            this.area_1.AreaBrush.Color = Color.FromArgb(0xff, 0, 0);
            this.area_1.Gradient.StartColor = Color.FromArgb(0xff, 0, 0);
            this.area_1.AreaLines.Color = Color.FromArgb(0x99, 0, 0);
            this.area_1.AreaLines.Visible = false;
            this.area_1.Brush.Color = Color.FromArgb(0xff, 0, 0);
            this.area_1.Color = Color.FromArgb(0xff, 0, 0);
            this.area_1.ColorEach = false;
            this.area_1.LinePen.Color = Color.FromArgb(0x99, 0, 0);
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
            this.area_1.Title = "area1";
            this.area_1.UseOrigin = true;
            this.area_1.XValues.DataMember = "X";
            this.area_1.XValues.Order = ValueListOrder.Ascending;
            this.area_1.YValues.DataMember = "Y";
            this.chartDrawdown.Aspect.View3D = false;
            this.chartDrawdown.Aspect.ZOffset = 0.0;
            this.chartDrawdown.Axes.Bottom.Grid.Visible = false;
            this.chartDrawdown.Axes.Bottom.Title.Transparent = true;
            this.chartDrawdown.Axes.Depth.Title.Transparent = true;
            this.chartDrawdown.Axes.DepthTop.Title.Transparent = true;
            this.chartDrawdown.Axes.Left.Grid.Visible = false;
            this.chartDrawdown.Axes.Left.Title.Caption = "Drawdown $";
            this.chartDrawdown.Axes.Left.Title.Lines = new string[] { "Drawdown $" };
            this.chartDrawdown.Axes.Left.Title.Transparent = true;
            this.chartDrawdown.Axes.Right.Title.Transparent = true;
            this.chartDrawdown.Axes.Top.Title.Transparent = true;
            this.chartDrawdown.ContextMenuStrip = this.popup;
            this.chartDrawdown.Cursor = Cursors.Default;
            this.chartDrawdown.Dock = DockStyle.Fill;
            this.chartDrawdown.Header.Font.Brush.Color = Color.FromArgb(0, 0, 0);
            this.chartDrawdown.Header.Lines = new string[] { "Strategy Drawdown Curve" };
            this.chartDrawdown.Legend.Visible = false;
            this.chartDrawdown.Location = new Point(0, 0);
            this.chartDrawdown.Name = "chartDrawdown";
            this.chartDrawdown.Panel.Brush.Color = Color.FromArgb(0xff, 0xff, 0xff);
            this.chartDrawdown.Series.Add(this.area_1);
            this.chartDrawdown.Series.Add(this.area_2);
            this.chartDrawdown.Size = new Size(0x227, 0xc2);
            this.chartDrawdown.TabIndex = 0;
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.cmbStrategies, this.toolStripLabel1 });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x227, 0x19);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            this.cmbStrategies.Alignment = ToolStripItemAlignment.Right;
            this.cmbStrategies.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStrategies.DropDownWidth = 150;
            this.cmbStrategies.FlatStyle = FlatStyle.Flat;
            this.cmbStrategies.Items.AddRange(new object[] { "Strategies in Aggregate" });
            this.cmbStrategies.MaxDropDownItems = 15;
            this.cmbStrategies.Name = "cmbStrategies";
            this.cmbStrategies.Size = new Size(150, 0x15);
            this.cmbStrategies.SelectedIndexChanged += new EventHandler(this.cmbStrategies_SelectedIndexChanged);
            this.toolStripLabel1.Alignment = ToolStripItemAlignment.Right;
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(0x4e, 0x16);
            this.toolStripLabel1.Text = "View";
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.splitContainer1);
            base.Name = "PVDrawdown";
            base.Size = new Size(0x227, 0x184);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.popup.ResumeLayout(false);
            this.popup_1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void method_0(SystemPerformance systemPerformance_1)
        {
            DataSeries equityCurve = systemPerformance_1.Results.EquityCurve;
            this.area_1.BeginUpdate();
            this.area_2.BeginUpdate();
            this.area_0.BeginUpdate();
            this.area_1.Clear();
            this.area_2.Clear();
            this.area_0.Clear();
            if (equityCurve.Count != 0)
            {
                double num2 = equityCurve[0];
                int num3 = 0;
                for (int i = 0; i < equityCurve.Count; i++)
                {
                    if (equityCurve[i] > num2)
                    {
                        num2 = equityCurve[i];
                        num3 = 0;
                    }
                    else
                    {
                        num3++;
                    }
                    string text = equityCurve.Date[i].ToShortDateString();
                    this.area_1.Add((double) i, (double) (equityCurve[i] - num2), text);
                    if (num2 != 0.0)
                    {
                        this.area_2.Add((double) i, (double) ((equityCurve[i] - num2) / num2), text);
                    }
                    else
                    {
                        this.area_2.Add((double) i, (double) 0.0, text);
                    }
                    this.area_0.Add((double) i, (double) num3, text);
                }
                this.area_1.EndUpdate();
                this.area_2.EndUpdate();
                this.area_0.EndUpdate();
                this.method_2();
            }
        }

        private void method_1(ref DataObject dataObject_0)
        {
            dataObject_0.SetData(PrintReport.fmtBaseTitle.Name, this.ivisualizerHost_0.ApplicationName());
            dataObject_0.SetData(PrintReport.fmtTitle.Name, "Drawdown Graphs");
            this.ivisualizerHost_0.StrategySummary(ref dataObject_0);
            Bitmap image = new Bitmap(this.chartDrawdown.Width, (this.chartDrawdown.Height + this.chartBarsSince.Height) + 3);
            Graphics graphics = Graphics.FromImage(image);
            graphics.DrawImageUnscaled(this.chartDrawdown.Bitmap, 0, 0);
            graphics.DrawImageUnscaled(this.chartBarsSince.Bitmap, 0, this.chartDrawdown.Height + 3);
            dataObject_0.SetData(PrintReport.fmtGraphic.Name, image);
        }

        private void method_2()
        {
            this.chartDrawdown.Axes.Left.Automatic = true;
            if (this.cmbChartUnit.Text == "Percent")
            {
                this.chartDrawdown.Axes.Left.Title.Text = "Drawdown %";
                this.chartDrawdown.Axes.Left.Labels.ValueFormat = this.area_2.PercentFormat;
                this.area_1.Visible = false;
                this.area_2.Visible = true;
            }
            else
            {
                this.chartDrawdown.Axes.Left.Title.Text = "Drawdown $";
                this.chartDrawdown.Axes.Left.Labels.ValueFormat = this.area_1.ValueFormat;
                this.area_1.Visible = true;
                this.area_2.Visible = false;
            }
        }

        private void mniCopyBarsSince_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetImage(this.chartBarsSince.Bitmap);
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void mniCopyCharts2_Click(object sender, EventArgs e)
        {
            this.CopyToClipboard();
        }

        private void mniCopyDrawdown_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetImage(this.chartDrawdown.Bitmap);
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void mniPrint2_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private void mniPrintAll2_Click(object sender, EventArgs e)
        {
            this.ivisualizerHost_0.PrintAll();
        }

        public void Print()
        {
            DataObject obj2 = new DataObject();
            this.method_1(ref obj2);
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
                return "Displays a drawdown curve (underwater equity curve) that visually depicts the losses that the Strategy experienced.  Also displays a graph showing the number of bars the Strategy had to wait before making a new equity high.";
            }
        }

        public string SettingsString
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("ChartUnits=");
                builder.Append(this.cmbChartUnit.SelectedItem.ToString());
                return builder.ToString();
            }
            set
            {
                string[] strArray = value.Split(new char[] { '=' });
                if ((strArray.Length == 2) && (strArray[0] == "ChartUnits"))
                {
                    this.cmbChartUnit.Text = strArray[1];
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
                return "Drawdown";
            }
        }
    }
}

