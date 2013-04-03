namespace WealthLab.Visualizers
{
    using Fidelity.Components;
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
    public class PVMaeMfe : UserControl, IPerformanceVisualizer, ISettingsProvider
    {
        private Bar bar_0;
        private Bar bar_1;
        private Bar bar_2;
        private Bar bar_3;
        private bool bool_0;
        private TChart chartMae;
        private TChart chartMfe;
        private ToolStripComboBox cmbMAEUnit;
        private ToolStripComboBox cmbMFEUnit;
        private Distribution distribution_0;
        private IContainer icontainer_0;
        private IVisualizerHost ivisualizerHost_0;
        private ToolStripMenuItem mniCopy;
        private ToolStripMenuItem mniCopy2;
        private ToolStripMenuItem mniMAECopy;
        private ToolStripMenuItem mniMFECopy;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrint2;
        private ToolStripMenuItem mniPrintAll;
        private ToolStripMenuItem mniPrintAll2;
        private PageSettings pageSettings_0 = new PageSettings();
        private ContextMenuStrip popup;
        private ContextMenuStrip popup_1;
        private SplitContainer splitMaeMfe;
        private SystemPerformance systemPerformance_0;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;

        public PVMaeMfe()
        {
            this.InitializeComponent();
            this.chartMae.Zoom.Allow = false;
            this.chartMfe.Zoom.Allow = false;
        }

        private void cmbMAEUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.bool_0)
            {
                this.bool_0 = true;
                this.cmbMFEUnit.Text = this.cmbMAEUnit.Text;
                this.method_0();
                this.bool_0 = false;
            }
        }

        private void cmbMFEUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.bool_0)
            {
                this.bool_0 = true;
                this.cmbMAEUnit.Text = this.cmbMFEUnit.Text;
                this.method_0();
                this.bool_0 = false;
            }
        }

        public void CopyToClipboard()
        {
            Bitmap image = new Bitmap(this.chartMae.Width, (this.chartMae.Height + this.chartMfe.Height) + 3);
            Graphics graphics = Graphics.FromImage(image);
            graphics.DrawImageUnscaled(this.chartMae.Bitmap, 0, 0);
            graphics.DrawImageUnscaled(this.chartMfe.Bitmap, 0, this.chartMae.Bitmap.Height + 3);
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PVMaeMfe));
            this.splitMaeMfe = new SplitContainer();
            this.chartMae = new TChart();
            this.popup = new ContextMenuStrip(this.icontainer_0);
            this.cmbMAEUnit = new ToolStripComboBox();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.mniMAECopy = new ToolStripMenuItem();
            this.mniCopy = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.mniPrintAll2 = new ToolStripMenuItem();
            this.bar_2 = new Bar();
            this.bar_3 = new Bar();
            this.chartMfe = new TChart();
            this.popup_1 = new ContextMenuStrip(this.icontainer_0);
            this.cmbMFEUnit = new ToolStripComboBox();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.mniMFECopy = new ToolStripMenuItem();
            this.mniCopy2 = new ToolStripMenuItem();
            this.mniPrint2 = new ToolStripMenuItem();
            this.mniPrintAll = new ToolStripMenuItem();
            this.bar_0 = new Bar();
            this.bar_1 = new Bar();
            this.distribution_0 = new Distribution(this.icontainer_0);
            this.splitMaeMfe.Panel1.SuspendLayout();
            this.splitMaeMfe.Panel2.SuspendLayout();
            this.splitMaeMfe.SuspendLayout();
            this.popup.SuspendLayout();
            this.popup_1.SuspendLayout();
            base.SuspendLayout();
            this.splitMaeMfe.Dock = DockStyle.Fill;
            this.splitMaeMfe.Location = new Point(0, 0);
            this.splitMaeMfe.Name = "splitMaeMfe";
            this.splitMaeMfe.Orientation = Orientation.Horizontal;
            this.splitMaeMfe.Panel1.Controls.Add(this.chartMae);
            this.splitMaeMfe.Panel2.Controls.Add(this.chartMfe);
            this.splitMaeMfe.Size = new Size(700, 0x1db);
            this.splitMaeMfe.SplitterDistance = 0xd4;
            this.splitMaeMfe.TabIndex = 0;
            this.chartMae.Aspect.ZOffset = 0.0;
            this.chartMae.Axes.Bottom.Grid.Visible = false;
            this.chartMae.Axes.Bottom.Labels.ValueFormat = "#0.00%";
            this.chartMae.Axes.Bottom.MaximumOffset = 0x2c;
            this.chartMae.Axes.Bottom.MinimumOffset = 0x2c;
            this.chartMae.Axes.Bottom.StartPosition = 1.0;
            this.chartMae.Axes.Left.Grid.Visible = false;
            this.chartMae.Axes.Left.MaximumOffset = 0x25;
            this.chartMae.ContextMenuStrip = this.popup;
            this.chartMae.Dock = DockStyle.Fill;
            this.chartMae.Header.Font.Brush.Color = Color.FromArgb(0, 0, 0);
            this.chartMae.Header.Lines = new string[] { "MAE (Max Adverse Excursion)" };
            this.chartMae.Location = new Point(0, 0);
            this.chartMae.Name = "chartMae";
            this.chartMae.Panel.Brush.Color = Color.FromArgb(0xff, 0xff, 0xff);
            this.chartMae.Series.Add(this.bar_2);
            this.chartMae.Series.Add(this.bar_3);
            this.chartMae.Size = new Size(700, 0xd4);
            this.chartMae.TabIndex = 0;
            this.popup.Items.AddRange(new ToolStripItem[] { this.cmbMAEUnit, this.toolStripSeparator1, this.mniMAECopy, this.mniCopy, this.mniPrint, this.mniPrintAll2 });
            this.popup.Name = "popup";
            this.popup.Size = new Size(0xb6, 0x7b);
            this.cmbMAEUnit.Items.AddRange(new object[] { "Dollar", "Percent" });
            this.cmbMAEUnit.Name = "cmbMAEUnit";
            this.cmbMAEUnit.Size = new Size(0x79, 0x15);
            this.cmbMAEUnit.Text = "Percent";
            this.cmbMAEUnit.SelectedIndexChanged += new EventHandler(this.cmbMAEUnit_SelectedIndexChanged);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0xb2, 6);
            this.mniMAECopy.Image = (Image) manager.GetObject("mniMAECopy.Image");
            this.mniMAECopy.ImageTransparentColor = Color.Fuchsia;
            this.mniMAECopy.Name = "mniMAECopy";
            this.mniMAECopy.Size = new Size(0xb5, 0x16);
            this.mniMAECopy.Text = "Copy MAE chart";
            this.mniMAECopy.ToolTipText = "Copy the MAE chart to the clipboard";
            this.mniMAECopy.Click += new EventHandler(this.mniMAECopy_Click);
            this.mniCopy.Name = "mniCopy";
            this.mniCopy.Size = new Size(0xb5, 0x16);
            this.mniCopy.Text = "Copy Charts";
            this.mniCopy.ToolTipText = "Copy both charts to the clipboard";
            this.mniCopy.Click += new EventHandler(this.mniCopy2_Click);
            this.mniPrint.Image = (Image) manager.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0xb5, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.ToolTipText = "Print the charts";
            this.mniPrint.Click += new EventHandler(this.mniPrint2_Click);
            this.mniPrintAll2.Name = "mniPrintAll2";
            this.mniPrintAll2.Size = new Size(0xb5, 0x16);
            this.mniPrintAll2.Text = "Print All";
            this.mniPrintAll2.ToolTipText = "Print content from all tabs";
            this.mniPrintAll2.Click += new EventHandler(this.mniPrintAll_Click);
            this.bar_2.Brush.Color = Color.FromArgb(0x80, 0, 0);
            this.bar_2.Color = Color.FromArgb(0x80, 0, 0);
            this.bar_2.ColorEach = false;
            this.bar_2.Marks.Arrow.Color = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.bar_2.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.bar_2.Marks.Callout.ArrowHeadSize = 8;
            this.bar_2.Marks.Callout.Brush.Color = Color.Black;
            this.bar_2.Marks.Callout.Distance = 0;
            this.bar_2.Marks.Callout.Draw3D = false;
            this.bar_2.Marks.Callout.Length = 20;
            this.bar_2.Marks.Callout.Style = PointerStyles.Rectangle;
            this.bar_2.Marks.Callout.Visible = false;
            this.bar_2.MultiBar = MultiBars.None;
            this.bar_2.Pen.Color = Color.FromArgb(0x4d, 0, 0);
            this.bar_2.Title = "All Trades";
            this.bar_2.XValues.DataMember = "X";
            this.bar_2.XValues.Order = ValueListOrder.Ascending;
            this.bar_2.YValues.DataMember = "Bar";
            this.bar_3.Brush.Color = Color.FromArgb(0xff, 0, 0);
            this.bar_3.Color = Color.FromArgb(0xff, 0, 0);
            this.bar_3.ColorEach = false;
            this.bar_3.Marks.Arrow.Color = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.bar_3.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.bar_3.Marks.Callout.ArrowHeadSize = 8;
            this.bar_3.Marks.Callout.Brush.Color = Color.Black;
            this.bar_3.Marks.Callout.Distance = 0;
            this.bar_3.Marks.Callout.Draw3D = false;
            this.bar_3.Marks.Callout.Length = 20;
            this.bar_3.Marks.Callout.Style = PointerStyles.Rectangle;
            this.bar_3.Marks.Callout.Visible = false;
            this.bar_3.MultiBar = MultiBars.None;
            this.bar_3.Pen.Color = Color.FromArgb(0x99, 0, 0);
            this.bar_3.Title = "Winning Trades";
            this.bar_3.UseOrigin = false;
            this.bar_3.XValues.DataMember = "X";
            this.bar_3.XValues.Order = ValueListOrder.Ascending;
            this.bar_3.YValues.DataMember = "Bar";
            this.chartMfe.Aspect.ZOffset = 0.0;
            this.chartMfe.Axes.Bottom.Grid.Visible = false;
            this.chartMfe.Axes.Bottom.Labels.ValueFormat = "#0.00%";
            this.chartMfe.Axes.Bottom.MaximumOffset = 0x2e;
            this.chartMfe.Axes.Bottom.MinimumOffset = 0x2e;
            this.chartMfe.Axes.Bottom.StartPosition = 1.0;
            this.chartMfe.Axes.Left.Grid.Visible = false;
            this.chartMfe.Axes.Left.MaximumOffset = 0x25;
            this.chartMfe.ContextMenuStrip = this.popup_1;
            this.chartMfe.Dock = DockStyle.Fill;
            this.chartMfe.Header.Font.Brush.Color = Color.FromArgb(0, 0, 0);
            this.chartMfe.Header.Lines = new string[] { "MFE (Max Favorable Excursion)" };
            this.chartMfe.Location = new Point(0, 0);
            this.chartMfe.Name = "chartMfe";
            this.chartMfe.Panel.Brush.Color = Color.FromArgb(0xff, 0xff, 0xff);
            this.chartMfe.Series.Add(this.bar_0);
            this.chartMfe.Series.Add(this.bar_1);
            this.chartMfe.Size = new Size(700, 0x103);
            this.chartMfe.TabIndex = 1;
            this.popup_1.Items.AddRange(new ToolStripItem[] { this.cmbMFEUnit, this.toolStripSeparator2, this.mniMFECopy, this.mniCopy2, this.mniPrint2, this.mniPrintAll });
            this.popup_1.Name = "popup";
            this.popup_1.Size = new Size(0xb6, 0x7b);
            this.cmbMFEUnit.Items.AddRange(new object[] { "Dollar", "Percent" });
            this.cmbMFEUnit.Name = "cmbMFEUnit";
            this.cmbMFEUnit.Size = new Size(0x79, 0x15);
            this.cmbMFEUnit.Text = "Percent";
            this.cmbMFEUnit.SelectedIndexChanged += new EventHandler(this.cmbMFEUnit_SelectedIndexChanged);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(0xb2, 6);
            this.mniMFECopy.Image = (Image) manager.GetObject("mniMFECopy.Image");
            this.mniMFECopy.ImageTransparentColor = Color.Fuchsia;
            this.mniMFECopy.Name = "mniMFECopy";
            this.mniMFECopy.Size = new Size(0xb5, 0x16);
            this.mniMFECopy.Text = "Copy MFE chart";
            this.mniMFECopy.ToolTipText = "Copy the MFE chart to the clipboard";
            this.mniMFECopy.Click += new EventHandler(this.mniMFECopy_Click);
            this.mniCopy2.Name = "mniCopy2";
            this.mniCopy2.Size = new Size(0xb5, 0x16);
            this.mniCopy2.Text = "Copy Charts";
            this.mniCopy2.ToolTipText = "Copy both charts to the clipboard";
            this.mniCopy2.Click += new EventHandler(this.mniCopy2_Click);
            this.mniPrint2.Image = (Image) manager.GetObject("mniPrint2.Image");
            this.mniPrint2.Name = "mniPrint2";
            this.mniPrint2.Size = new Size(0xb5, 0x16);
            this.mniPrint2.Text = "Print";
            this.mniPrint2.ToolTipText = "Print the charts";
            this.mniPrint2.Click += new EventHandler(this.mniPrint2_Click);
            this.mniPrintAll.Name = "mniPrintAll";
            this.mniPrintAll.Size = new Size(0xb5, 0x16);
            this.mniPrintAll.Text = "Print All";
            this.mniPrintAll.ToolTipText = "Print content from all tabs";
            this.mniPrintAll.Click += new EventHandler(this.mniPrintAll_Click);
            this.bar_0.Brush.Color = Color.FromArgb(0, 0, 0x80);
            this.bar_0.Color = Color.FromArgb(0, 0, 0x80);
            this.bar_0.ColorEach = false;
            this.bar_0.Marks.Arrow.Color = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.bar_0.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.bar_0.Marks.Callout.ArrowHeadSize = 8;
            this.bar_0.Marks.Callout.Brush.Color = Color.Black;
            this.bar_0.Marks.Callout.Distance = 0;
            this.bar_0.Marks.Callout.Draw3D = false;
            this.bar_0.Marks.Callout.Length = 20;
            this.bar_0.Marks.Callout.Style = PointerStyles.Rectangle;
            this.bar_0.Marks.Callout.Visible = false;
            this.bar_0.MultiBar = MultiBars.None;
            this.bar_0.Pen.Color = Color.FromArgb(0, 0, 0x4d);
            this.bar_0.Title = "All Trades";
            this.bar_0.XValues.DataMember = "X";
            this.bar_0.XValues.Order = ValueListOrder.Ascending;
            this.bar_0.YValues.DataMember = "Bar";
            this.bar_1.Brush.Color = Color.FromArgb(0, 0, 0xff);
            this.bar_1.Color = Color.FromArgb(0, 0, 0xff);
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
            this.bar_1.MultiBar = MultiBars.None;
            this.bar_1.Pen.Color = Color.FromArgb(0, 0, 0x99);
            this.bar_1.Title = "Losing Trades";
            this.bar_1.XValues.DataMember = "X";
            this.bar_1.XValues.Order = ValueListOrder.Ascending;
            this.bar_1.YValues.DataMember = "Bar";
            this.distribution_0.BinsDesired = 20;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.splitMaeMfe);
            base.Name = "PVMaeMfe";
            base.Size = new Size(700, 0x1db);
            this.splitMaeMfe.Panel1.ResumeLayout(false);
            this.splitMaeMfe.Panel2.ResumeLayout(false);
            this.splitMaeMfe.ResumeLayout(false);
            this.popup.ResumeLayout(false);
            this.popup_1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void method_0()
        {
            this.chartMae.Axes.Bottom.Automatic = true;
            if (this.cmbMAEUnit.Text == "Percent")
            {
                this.chartMae.Axes.Bottom.Labels.ValueFormat = this.bar_2.PercentFormat;
                foreach (WealthLab.Position position6 in this.systemPerformance_0.Results.Positions)
                {
                    this.distribution_0.AddValue(position6.MAEPercent);
                }
                this.distribution_0.MakeBins();
                this.bar_2.Clear();
                foreach (Bin bin5 in this.distribution_0.Bins)
                {
                    this.bar_2.Add((double) (bin5.High / 100.0), (double) bin5.Count);
                }
                foreach (WealthLab.Position position2 in this.systemPerformance_0.Results.Positions)
                {
                    if (position2.NetProfit > 0.0)
                    {
                        this.distribution_0.AddValue(position2.MAEPercent);
                    }
                }
                this.distribution_0.FillBins();
                this.bar_3.Clear();
                foreach (Bin bin in this.distribution_0.Bins)
                {
                    this.bar_3.Add((double) (bin.High / 100.0), (double) bin.Count);
                }
            }
            else
            {
                this.chartMae.Axes.Bottom.Labels.ValueFormat = this.bar_2.ValueFormat;
                foreach (WealthLab.Position position3 in this.systemPerformance_0.Results.Positions)
                {
                    this.distribution_0.AddValue(position3.MAE);
                }
                this.distribution_0.MakeBins();
                this.bar_2.Clear();
                foreach (Bin bin3 in this.distribution_0.Bins)
                {
                    this.bar_2.Add(bin3.High, (double) bin3.Count);
                }
                foreach (WealthLab.Position position4 in this.systemPerformance_0.Results.Positions)
                {
                    if (position4.NetProfit > 0.0)
                    {
                        this.distribution_0.AddValue(position4.MAE);
                    }
                }
                this.distribution_0.FillBins();
                this.bar_3.Clear();
                foreach (Bin bin4 in this.distribution_0.Bins)
                {
                    this.bar_3.Add(bin4.High, (double) bin4.Count);
                }
            }
            this.chartMfe.Axes.Bottom.Automatic = true;
            if (this.cmbMFEUnit.Text == "Percent")
            {
                this.chartMfe.Axes.Bottom.Labels.ValueFormat = this.bar_0.PercentFormat;
                foreach (WealthLab.Position position5 in this.systemPerformance_0.Results.Positions)
                {
                    this.distribution_0.AddValue(position5.MFEPercent);
                }
                this.distribution_0.MakeBins();
                this.bar_0.Clear();
                foreach (Bin bin7 in this.distribution_0.Bins)
                {
                    this.bar_0.Add((double) (bin7.High / 100.0), (double) bin7.Count);
                }
                foreach (WealthLab.Position position8 in this.systemPerformance_0.Results.Positions)
                {
                    if (position8.NetProfit <= 0.0)
                    {
                        this.distribution_0.AddValue(position8.MFEPercent);
                    }
                }
                this.distribution_0.FillBins();
                this.bar_1.Clear();
                foreach (Bin bin6 in this.distribution_0.Bins)
                {
                    this.bar_1.Add((double) (bin6.High / 100.0), (double) bin6.Count);
                }
            }
            else
            {
                this.chartMfe.Axes.Bottom.Labels.ValueFormat = this.bar_0.ValueFormat;
                foreach (WealthLab.Position position7 in this.systemPerformance_0.Results.Positions)
                {
                    this.distribution_0.AddValue(position7.MFE);
                }
                this.distribution_0.MakeBins();
                this.bar_0.Clear();
                foreach (Bin bin8 in this.distribution_0.Bins)
                {
                    this.bar_0.Add(bin8.High, (double) bin8.Count);
                }
                foreach (WealthLab.Position position in this.systemPerformance_0.Results.Positions)
                {
                    if (position.NetProfit <= 0.0)
                    {
                        this.distribution_0.AddValue(position.MFE);
                    }
                }
                this.distribution_0.FillBins();
                this.bar_1.Clear();
                foreach (Bin bin2 in this.distribution_0.Bins)
                {
                    this.bar_1.Add(bin2.High, (double) bin2.Count);
                }
            }
        }

        private void method_1(ref DataObject dataObject_0)
        {
            dataObject_0.SetData(PrintReport.fmtBaseTitle.Name, this.ivisualizerHost_0.ApplicationName());
            dataObject_0.SetData(PrintReport.fmtTitle.Name, "MAE/MFE Graphs");
            this.ivisualizerHost_0.StrategySummary(ref dataObject_0);
            Bitmap image = new Bitmap(this.chartMae.Width, (this.chartMae.Height + this.chartMfe.Height) + 3);
            Graphics graphics = Graphics.FromImage(image);
            graphics.DrawImageUnscaled(this.chartMae.Bitmap, 0, 0);
            graphics.DrawImageUnscaled(this.chartMfe.Bitmap, 0, this.chartMae.Bitmap.Height + 3);
            dataObject_0.SetData(PrintReport.fmtGraphic.Name, image);
        }

        private void mniCopy2_Click(object sender, EventArgs e)
        {
            this.CopyToClipboard();
        }

        private void mniMAECopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetImage(this.chartMae.Bitmap);
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void mniMFECopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetImage(this.chartMfe.Bitmap);
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

        private void mniPrintAll_Click(object sender, EventArgs e)
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
                return "Displays graphs showing the distributions for Maximum Adverse Excursion (MAE) and Maximum Favorable Excursion (MFE) for all trades generated by the Strategy.";
            }
        }

        public string SettingsString
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("ChartUnits=");
                builder.Append(this.cmbMAEUnit.SelectedItem.ToString());
                return builder.ToString();
            }
            set
            {
                string[] strArray = value.Split(new char[] { '=' });
                if ((strArray.Length == 2) && (strArray[0] == "ChartUnits"))
                {
                    this.cmbMAEUnit.Text = strArray[1];
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
                return "MAE/MFE";
            }
        }
    }
}

