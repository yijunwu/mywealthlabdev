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
    public class PVProfitDist : UserControl, IPerformanceVisualizer, ISettingsProvider
    {
        private Bar bar_0;
        private TChart chartProfitDist;
        private ToolStripComboBox cmbChartUnit;
        private Distribution distribution_0;
        private IContainer icontainer_0;
        private IVisualizerHost ivisualizerHost_0;
        private ToolStripMenuItem mniCopy;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrintAll;
        private PageSettings pageSettings_0 = new PageSettings();
        private ContextMenuStrip popup;
        private SystemPerformance systemPerformance_0;
        private ToolStripSeparator toolStripSeparator1;

        public PVProfitDist()
        {
            this.InitializeComponent();
            this.chartProfitDist.Zoom.Allow = false;
        }

        private void cmbChartUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.method_0();
        }

        public void CopyToClipboard()
        {
            try
            {
                Clipboard.SetImage(this.chartProfitDist.Bitmap);
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public void CreateVisualization(SystemPerformance performance, IVisualizerHost visHost)
        {
            this.systemPerformance_0 = performance;
            this.ivisualizerHost_0 = visHost;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PVProfitDist));
            this.chartProfitDist = new TChart();
            this.bar_0 = new Bar();
            this.distribution_0 = new Distribution(this.icontainer_0);
            this.popup = new ContextMenuStrip(this.icontainer_0);
            this.cmbChartUnit = new ToolStripComboBox();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.mniCopy = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.mniPrintAll = new ToolStripMenuItem();
            this.popup.SuspendLayout();
            base.SuspendLayout();
            this.chartProfitDist.Aspect.ZOffset = 0.0;
            this.chartProfitDist.Axes.Bottom.Grid.Visible = false;
            this.chartProfitDist.Axes.Bottom.Labels.ValueFormat = "#0.00%";
            this.chartProfitDist.Axes.Bottom.MaximumOffset = 50;
            this.chartProfitDist.Axes.Bottom.MinimumOffset = 50;
            this.chartProfitDist.Axes.Bottom.StartPosition = 1.0;
            this.chartProfitDist.Axes.Bottom.Title.Caption = "% Profit";
            this.chartProfitDist.Axes.Bottom.Title.Lines = new string[] { "% Profit" };
            this.chartProfitDist.Axes.Left.Grid.Visible = false;
            this.chartProfitDist.Axes.Left.MaximumOffset = 0x25;
            this.chartProfitDist.Axes.Left.Title.Caption = "Number of Trades";
            this.chartProfitDist.Axes.Left.Title.Lines = new string[] { "Number of Trades" };
            this.chartProfitDist.Dock = DockStyle.Fill;
            this.chartProfitDist.Header.Font.Brush.Color = Color.FromArgb(0, 0, 0);
            this.chartProfitDist.Header.Lines = new string[] { "% Profit Distribution of Strategy Trades" };
            this.chartProfitDist.Legend.Visible = false;
            this.chartProfitDist.Location = new Point(0, 0);
            this.chartProfitDist.Name = "chartProfitDist";
            this.chartProfitDist.Panel.Brush.Color = Color.FromArgb(0xff, 0xff, 0xff);
            this.chartProfitDist.Series.Add(this.bar_0);
            this.chartProfitDist.Size = new Size(0x251, 0x19c);
            this.chartProfitDist.TabIndex = 0;
            this.bar_0.Brush.Color = Color.FromArgb(0x44, 0x66, 0xa3);
            this.bar_0.Color = Color.FromArgb(0x44, 0x66, 0xa3);
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
            this.bar_0.Pen.Color = Color.FromArgb(0x29, 0x3d, 0x62);
            this.bar_0.Pen.Visible = false;
            this.bar_0.Title = "bar1";
            this.bar_0.XValues.DataMember = "X";
            this.bar_0.XValues.Order = ValueListOrder.Ascending;
            this.bar_0.YValues.DataMember = "Bar";
            this.distribution_0.BinsDesired = 0x19;
            this.popup.Items.AddRange(new ToolStripItem[] { this.cmbChartUnit, this.toolStripSeparator1, this.mniCopy, this.mniPrint, this.mniPrintAll });
            this.popup.Name = "popup";
            this.popup.Size = new Size(0xb6, 0x65);
            this.cmbChartUnit.Items.AddRange(new object[] { "Dollar", "Percent" });
            this.cmbChartUnit.Name = "cmbChartUnit";
            this.cmbChartUnit.Size = new Size(0x79, 0x15);
            this.cmbChartUnit.Text = "Percent";
            this.cmbChartUnit.SelectedIndexChanged += new EventHandler(this.cmbChartUnit_SelectedIndexChanged);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0xb2, 6);
            this.mniCopy.Image = (Image) manager.GetObject("mniCopy.Image");
            this.mniCopy.ImageTransparentColor = Color.Fuchsia;
            this.mniCopy.Name = "mniCopy";
            this.mniCopy.Size = new Size(0xb5, 0x16);
            this.mniCopy.Text = "Copy chart";
            this.mniCopy.ToolTipText = "Copy chart to the clipboard";
            this.mniCopy.Click += new EventHandler(this.mniCopy_Click);
            this.mniPrint.Image = (Image) manager.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0xb5, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.ToolTipText = "Print the chart";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.mniPrintAll.Name = "mniPrintAll";
            this.mniPrintAll.Size = new Size(0xb5, 0x16);
            this.mniPrintAll.Text = "Print All";
            this.mniPrintAll.ToolTipText = "Print content from all tabs";
            this.mniPrintAll.Click += new EventHandler(this.mniPrintAll_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.ContextMenuStrip = this.popup;
            base.Controls.Add(this.chartProfitDist);
            base.Name = "PVProfitDist";
            base.Size = new Size(0x251, 0x19c);
            this.popup.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void method_0()
        {
            this.chartProfitDist.Axes.Bottom.Automatic = true;
            if (this.cmbChartUnit.Text == "Percent")
            {
                this.chartProfitDist.Header.Text = "% Profit Distribution of Strategy Trades";
                this.chartProfitDist.Axes.Bottom.Title.Text = "% Profit";
                this.chartProfitDist.Axes.Bottom.Labels.ValueFormat = this.bar_0.PercentFormat;
                foreach (WealthLab.Position position in this.systemPerformance_0.Results.Positions)
                {
                    this.distribution_0.AddValue(position.NetProfitPercent);
                }
                this.distribution_0.MakeBins();
                this.bar_0.Clear();
                foreach (Bin bin2 in this.distribution_0.Bins)
                {
                    Color red = Color.Red;
                    if (bin2.Low >= 0.0)
                    {
                        red = Color.Blue;
                    }
                    this.bar_0.Add((double) (bin2.High / 100.0), (double) bin2.Count, red);
                }
            }
            else
            {
                this.chartProfitDist.Header.Text = "$ Profit Distribution of Strategy Trades";
                this.chartProfitDist.Axes.Bottom.Title.Text = "$ Profit";
                this.chartProfitDist.Axes.Bottom.Labels.ValueFormat = this.bar_0.ValueFormat;
                foreach (WealthLab.Position position2 in this.systemPerformance_0.Results.Positions)
                {
                    this.distribution_0.AddValue(position2.NetProfit);
                }
                this.distribution_0.MakeBins();
                this.bar_0.Clear();
                foreach (Bin bin in this.distribution_0.Bins)
                {
                    Color blue = Color.Red;
                    if (bin.Low >= 0.0)
                    {
                        blue = Color.Blue;
                    }
                    this.bar_0.Add(bin.High, (double) bin.Count, blue);
                }
            }
        }

        private void method_1(ref DataObject dataObject_0)
        {
            dataObject_0.SetData(PrintReport.fmtBaseTitle.Name, this.ivisualizerHost_0.ApplicationName());
            dataObject_0.SetData(PrintReport.fmtTitle.Name, "Profit Distribution");
            this.ivisualizerHost_0.StrategySummary(ref dataObject_0);
            dataObject_0.SetData(PrintReport.fmtGraphic.Name, this.chartProfitDist.Bitmap);
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
                return "Displays a chart showing the profit distribution for all trades generated by the Strategy.";
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
                return "Profit Distribution";
            }
        }
    }
}

