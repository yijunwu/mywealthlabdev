namespace WealthLab.Visualizers.Extensions
{
    using log4net;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Windows.Forms.Integration;
    using WealthLab;
    using WealthLab.Visualizers.Extensions.Properties;

    public class HeatMapVisualizerContainer : UserControl, IPerformanceVisualizer
    {
        private ToolStripComboBox _cb_zoom;
        private SystemPerformance _performance;
        private IVisualizerHost _visHost;
        private ContextMenuStrip cms_options;
        private IContainer components;
        private ToolStripMenuItem copyChartToClipboardToolStripMenuItem;
        private ToolStripMenuItem copyDataToClipboardToolStripMenuItem;
        private ElementHost elementHost1;
        private HeatMap heatmap;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private PrintDialog pd;
        private PrintPreviewDialog ppd;
        private ToolStripMenuItem printAllToolStripMenuItem;
        private ToolStripMenuItem printToolStripMenuItem;
        private PageSetupDialog psd;

        public HeatMapVisualizerContainer()
        {
            try
            {
                this.InitializeComponent();
            }
            catch (Exception exception)
            {
                MessageBox.Show("There is some problem occurred while loading Heatmap. /nPlease ensure that you are having .NET runtime version 3.5+");
                logger.Error("Inside Heat Map - Exception Message is " + exception.Message);
                if (exception.InnerException != null)
                {
                    logger.Error("Inner Exception is " + exception.InnerException.Message);
                }
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.ppd.Close();
        }

        private void btn_pagedwn_Click(object sender, EventArgs e)
        {
        }

        private void btn_pageup_Click(object sender, EventArgs e)
        {
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            this.pd.ShowDialog();
            if (this.pd.ShowDialog() == DialogResult.OK)
            {
                this.pd.Document.Print();
            }
            this.pd.Dispose();
            this.ppd.Dispose();
            this.psd.Dispose();
        }

        private void btn_Setup_Click(object sender, EventArgs e)
        {
            if (this.psd.ShowDialog() == DialogResult.OK)
            {
                this.ppd.Document = this.psd.Document;
                this.ppd.Refresh();
            }
        }

        private void cb_zoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this._cb_zoom.SelectedItem.ToString())
            {
                case "Auto":
                    this.ppd.PrintPreviewControl.AutoZoom = true;
                    break;

                case "500%":
                    this.ppd.PrintPreviewControl.Zoom = 5.0;
                    break;

                case "200%":
                    this.ppd.PrintPreviewControl.Zoom = 2.0;
                    break;

                case "150%":
                    this.ppd.PrintPreviewControl.Zoom = 1.5;
                    break;

                case "100%":
                    this.ppd.PrintPreviewControl.Zoom = 1.0;
                    break;

                case "75%":
                    this.ppd.PrintPreviewControl.Zoom = 0.75;
                    break;

                case "50%":
                    this.ppd.PrintPreviewControl.Zoom = 0.5;
                    break;

                case "25%":
                    this.ppd.PrintPreviewControl.Zoom = 0.25;
                    break;

                case "10%":
                    this.ppd.PrintPreviewControl.Zoom = 0.1;
                    break;

                case "Two Pages":
                    this.ppd.PrintPreviewControl.Rows = 2;
                    break;

                case "Whole Page":
                    this.ppd.PrintPreviewControl.Rows = 1;
                    break;

                case "Page Width":
                    this.ppd.PrintPreviewControl.Zoom = 0.85;
                    break;

                case "All Pages":
                    this.ppd.PrintPreviewControl.Rows = 5;
                    this.ppd.PrintPreviewControl.Columns = 5;
                    break;
            }
            this.ppd.Refresh();
        }

        private void copyChartToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CopyToClipboard();
        }

        private void copyEquityCurveDataToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = string.Format("Date{0,8}Equity{1,5}Long{2,10}Short{3,5}Buy & Hold{4,10}Cash\r\n", new object[] { " ", " ", " ", " ", " " });
            for (int i = 0; i < this._performance.Results.EquityCurve.Count; i++)
            {
                object[] objArray2 = new object[] { this._performance.Results.EquityCurve.Date[i].ToShortDateString(), "{0,5}", this._performance.Results.EquityCurve[i], "{1,5}", this._performance.ResultsLong.EquityCurve[i], "{2,5}", this._performance.ResultsShort.EquityCurve[i], "{3,5}", this._performance.ResultsBuyHold.EquityCurve[i], "{4,10:C4}", this._performance.Results.CashCurve[i], "\r\n" };
                text = text + string.Format(string.Concat(objArray2), new object[] { " ", " ", " ", " ", " " });
            }
            try
            {
                Clipboard.SetText(text);
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public void CopyToClipboard()
        {
            Bitmap bitmap = new Bitmap(this.elementHost1.Width, this.elementHost1.Height);
            this.elementHost1.DrawToBitmap(bitmap, this.elementHost1.Bounds);
            try
            {
                Clipboard.SetImage(bitmap);
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public void CreateVisualization(SystemPerformance performance, IVisualizerHost visHost)
        {
            this._performance = performance;
            this._visHost = visHost;
            this.heatmap.InitVariables(performance, visHost);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            string s = "Neumont University Heatmap Chart";
            Size size = new Size(650, 400);
            Point location = new Point((e.PageBounds.Width / 2) - (size.Width / 2), 200);
            Bitmap bitmap = new Bitmap(this.elementHost1.Width, this.elementHost1.Height);
            this.elementHost1.DrawToBitmap(bitmap, this.elementHost1.Bounds);
            e.Graphics.DrawImage(bitmap, new Rectangle(location, size));
            DataObject printObject = new DataObject();
            this._visHost.StrategySummary(ref printObject);
            e.Graphics.DrawString(s, new Font(FontFamily.GenericSansSerif, 14f, FontStyle.Bold), Brushes.Black, (float) ((e.PageBounds.Width / 2) - (s.Length * 5)), 100f);
            e.Graphics.DrawString(DateTime.Now.ToString(), new Font(FontFamily.GenericSansSerif, 9f, FontStyle.Regular), Brushes.Black, (float) (e.MarginBounds.Width - DateTime.Now.ToString().Length), 120f);
            e.Graphics.DrawString(string.Concat(new object[] { "Strategy ", printObject.GetData("Strategy"), "\r\nSymbol: ", printObject.GetData("Symbol"), "\r\n\r\n", printObject.GetData("Details") }), new Font(FontFamily.GenericSansSerif, 9f, FontStyle.Regular), Brushes.Black, new RectangleF(new PointF((float) location.X, (float) (location.Y - 80)), (SizeF) size));
        }

        public void EnableControls(bool enable)
        {
        }

        private void HeatMapVisualizerContainer_Resize(object sender, EventArgs e)
        {
        }

        private void HeatMapVisualizerContainer_SizeChanged(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(HeatMapVisualizerContainer));
            this.psd = new PageSetupDialog();
            this.ppd = new PrintPreviewDialog();
            this.pd = new PrintDialog();
            this.cms_options = new ContextMenuStrip(this.components);
            this.copyDataToClipboardToolStripMenuItem = new ToolStripMenuItem();
            this.copyChartToClipboardToolStripMenuItem = new ToolStripMenuItem();
            this.printToolStripMenuItem = new ToolStripMenuItem();
            this.printAllToolStripMenuItem = new ToolStripMenuItem();
            this.elementHost1 = new ElementHost();
            this.heatmap = new HeatMap();
            this.cms_options.SuspendLayout();
            base.SuspendLayout();
            this.ppd.AutoScrollMargin = new Size(0, 0);
            this.ppd.AutoScrollMinSize = new Size(0, 0);
            this.ppd.ClientSize = new Size(400, 300);
            this.ppd.Enabled = true;
            this.ppd.Icon = (Icon) manager.GetObject("ppd.Icon");
            this.ppd.Name = "ppd";
            this.ppd.Visible = false;
            this.pd.UseEXDialog = true;
            this.cms_options.Items.AddRange(new ToolStripItem[] { this.copyDataToClipboardToolStripMenuItem, this.copyChartToClipboardToolStripMenuItem, this.printToolStripMenuItem, this.printAllToolStripMenuItem });
            this.cms_options.Name = "cms_options";
            this.cms_options.Size = new Size(0x107, 0x5c);
            this.copyDataToClipboardToolStripMenuItem.Name = "copyDataToClipboardToolStripMenuItem";
            this.copyDataToClipboardToolStripMenuItem.Size = new Size(0x106, 0x16);
            this.copyDataToClipboardToolStripMenuItem.Text = "Copy Equity Curve Data to Clipboard";
            this.copyDataToClipboardToolStripMenuItem.Click += new EventHandler(this.copyEquityCurveDataToClipboardToolStripMenuItem_Click);
            this.copyChartToClipboardToolStripMenuItem.Name = "copyChartToClipboardToolStripMenuItem";
            this.copyChartToClipboardToolStripMenuItem.Size = new Size(0x106, 0x16);
            this.copyChartToClipboardToolStripMenuItem.Text = "Copy Chart to Clipboard";
            this.copyChartToClipboardToolStripMenuItem.Click += new EventHandler(this.copyChartToClipboardToolStripMenuItem_Click);
            this.printToolStripMenuItem.Image = Resources.printer11;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new Size(0x106, 0x16);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new EventHandler(this.printToolStripMenuItem_Click);
            this.printAllToolStripMenuItem.Name = "printAllToolStripMenuItem";
            this.printAllToolStripMenuItem.Size = new Size(0x106, 0x16);
            this.printAllToolStripMenuItem.Text = "Print All";
            this.printAllToolStripMenuItem.Click += new EventHandler(this.printAllToolStripMenuItem_Click);
            this.elementHost1.ContextMenuStrip = this.cms_options;
            this.elementHost1.Dock = DockStyle.Fill;
            this.elementHost1.Location = new Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new Size(0x339, 0x1f7);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.heatmap;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.elementHost1);
            base.Name = "HeatMapVisualizerContainer";
            base.Size = new Size(0x339, 0x1f7);
            base.Resize += new EventHandler(this.HeatMapVisualizerContainer_Resize);
            base.SizeChanged += new EventHandler(this.HeatMapVisualizerContainer_SizeChanged);
            this.cms_options.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void Print()
        {
            PrintDocument document = new PrintDocument();
            document.PrintPage += new PrintPageEventHandler(this.document_PrintPage);
            this.ppd.Width = 750;
            this.ppd.Height = 550;
            this.ppd.Document = document;
            this.psd.Document = this.ppd.Document;
            this.pd.Document = this.psd.Document;
            this.PrintToolbar();
            this.ppd.ShowDialog();
        }

        private void printAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._visHost.PrintAll();
        }

        public void PrintToolbar()
        {
            ToolStripItem[] items = new ToolStripItem[14];
            this.ppd.Controls.RemoveAt(1);
            ToolStripButton button = new ToolStripButton {
                Text = "Print...",
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            button.Click += new EventHandler(this.btn_Print_Click);
            items[0] = button;
            ToolStripSeparator separator = new ToolStripSeparator();
            items[1] = separator;
            ToolStripButton button2 = new ToolStripButton {
                Text = "Setup",
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            button2.Click += new EventHandler(this.btn_Setup_Click);
            items[2] = button2;
            ToolStripSeparator separator2 = new ToolStripSeparator();
            items[3] = separator2;
            ToolStripLabel label = new ToolStripLabel {
                Text = "Zoom:"
            };
            items[4] = label;
            this._cb_zoom = new ToolStripComboBox();
            this._cb_zoom.Items.AddRange(new object[] { "Auto", "500%", "200%", "150%", "100%", "75%", "50%", "25%", "10%", "Two Pages", "Whole Page", "Page Width", "All Pages" });
            this._cb_zoom.SelectedIndexChanged += new EventHandler(this.cb_zoom_SelectedIndexChanged);
            this._cb_zoom.SelectedIndex = 0;
            items[5] = this._cb_zoom;
            ToolStripSeparator separator3 = new ToolStripSeparator();
            items[6] = separator3;
            ToolStripLabel label2 = new ToolStripLabel {
                Text = "Page"
            };
            items[7] = label2;
            ToolStripTextBox box = new ToolStripTextBox {
                Text = string.Format("{0}", this.ppd.PrintPreviewControl.StartPage + 1),
                Size = new Size(0x19, 0x19)
            };
            items[8] = box;
            ToolStripLabel label3 = new ToolStripLabel {
                Text = "of " + this.ppd.PrintPreviewControl.Columns.ToString()
            };
            items[9] = label3;
            ToolStripButton button3 = new ToolStripButton {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = Resources.arrowb
            };
            button3.Click += new EventHandler(this.btn_pagedwn_Click);
            items[10] = button3;
            ToolStripButton button4 = new ToolStripButton {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = Resources.arrowf
            };
            button4.Click += new EventHandler(this.btn_pageup_Click);
            items[11] = button4;
            ToolStripSeparator separator4 = new ToolStripSeparator();
            items[12] = separator4;
            ToolStripButton button5 = new ToolStripButton {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Text = "Close"
            };
            button5.Click += new EventHandler(this.btn_close_Click);
            items[13] = button5;
            ToolStrip strip = new ToolStrip(items);
            this.ppd.Controls.Add(strip);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Print();
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
                return "The Heat Map is a collection of polygons corresponding to trades (or groups of trades by symbol) whose color depth and size represent metrics such as Profit %, Profit/Loss, and Number of Trades or Trade Size. Requires .NET Framework 3.5 (minimum).";
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
                return "HeatMap";
            }
        }
    }
}

