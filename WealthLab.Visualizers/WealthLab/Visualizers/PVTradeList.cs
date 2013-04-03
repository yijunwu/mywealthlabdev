namespace WealthLab.Visualizers
{
    using Fidelity.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class PVTradeList : UserControl, IPerformanceVisualizer
    {
        private static Color color_0 = Color.FromArgb(0xff, 230, 230);
        private static Color color_1 = Color.FromArgb(230, 0xff, 230);
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_10;
        private ColumnHeader columnHeader_11;
        private ColumnHeader columnHeader_12;
        private ColumnHeader columnHeader_13;
        private ColumnHeader columnHeader_14;
        private ColumnHeader columnHeader_15;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private ColumnHeader columnHeader_5;
        private ColumnHeader columnHeader_6;
        private ColumnHeader columnHeader_7;
        private ColumnHeader columnHeader_8;
        private ColumnHeader columnHeader_9;
        private static Font font_0 = null;
        private IContainer icontainer_0;
        private ImageList imageList_0;
        private IVisualizerHost ivisualizerHost_0;
        private ToolStripStatusLabel lblNSF;
        private ToolStripStatusLabel lblTrades;
        private ListViewItem listViewItem_0;
        private SortableListView lvTrades;
        private ToolStripMenuItem mniCopy;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrintAll;
        private PageSettings pageSettings_0 = new PageSettings();
        private ContextMenuStrip popup;
        private StatusStrip status;
        private ToolStripStatusLabel statusNSF;
        private ToolStripStatusLabel statusTrades;

        public PVTradeList()
        {
            this.InitializeComponent();
        }

        public void CopyToClipboard()
        {
            this.ivisualizerHost_0.CopyListViewToClipboard(this.lvTrades);
        }

        public void CreateVisualization(SystemPerformance performance, IVisualizerHost visHost)
        {
            if (performance.Strategy.StrategyType != StrategyType.CombinedStrategy)
            {
                this.lvTrades.Columns.Remove(this.columnHeader_15);
            }
            this.ivisualizerHost_0 = visHost;
            this.method_0(performance);
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
            this.lvTrades.Enabled = enable;
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PVTradeList));
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.status = new StatusStrip();
            this.lblTrades = new ToolStripStatusLabel();
            this.statusTrades = new ToolStripStatusLabel();
            this.lblNSF = new ToolStripStatusLabel();
            this.statusNSF = new ToolStripStatusLabel();
            this.lvTrades = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.columnHeader_5 = new ColumnHeader();
            this.columnHeader_6 = new ColumnHeader();
            this.columnHeader_7 = new ColumnHeader();
            this.columnHeader_8 = new ColumnHeader();
            this.columnHeader_9 = new ColumnHeader();
            this.columnHeader_10 = new ColumnHeader();
            this.columnHeader_11 = new ColumnHeader();
            this.columnHeader_12 = new ColumnHeader();
            this.columnHeader_13 = new ColumnHeader();
            this.columnHeader_14 = new ColumnHeader();
            this.popup = new ContextMenuStrip(this.icontainer_0);
            this.mniCopy = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.mniPrintAll = new ToolStripMenuItem();
            this.columnHeader_15 = new ColumnHeader();
            this.status.SuspendLayout();
            this.popup.SuspendLayout();
            base.SuspendLayout();
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("imagesArrows.ImageStream");
            this.imageList_0.TransparentColor = Color.White;
            this.imageList_0.Images.SetKeyName(0, "LongOpen.bmp");
            this.imageList_0.Images.SetKeyName(1, "LongClosed.bmp");
            this.imageList_0.Images.SetKeyName(2, "ShortOpen.bmp");
            this.imageList_0.Images.SetKeyName(3, "ShortClosed.bmp");
            this.status.Items.AddRange(new ToolStripItem[] { this.lblTrades, this.statusTrades, this.lblNSF, this.statusNSF });
            this.status.Location = new Point(0, 0x1a3);
            this.status.Name = "status";
            this.status.Size = new Size(0x34e, 0x16);
            this.status.TabIndex = 1;
            this.status.Text = "statusStrip1";
            this.lblTrades.Name = "lblTrades";
            this.lblTrades.Size = new Size(0xb0, 0x11);
            this.lblTrades.Text = "Trades included in backtest results:";
            this.statusTrades.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusTrades.Name = "statusTrades";
            this.statusTrades.Size = new Size(0x11, 0x11);
            this.statusTrades.Text = "0";
            this.lblNSF.Name = "lblNSF";
            this.lblNSF.Size = new Size(0x114, 0x11);
            this.lblNSF.Text = "Trades not included due to insufficient simulated capital:";
            this.statusNSF.Name = "statusNSF";
            this.statusNSF.Size = new Size(13, 0x11);
            this.statusNSF.Text = "0";
            this.lvTrades.BackColor = SystemColors.Window;
            this.lvTrades.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_15, this.columnHeader_1, this.columnHeader_2, this.columnHeader_3, this.columnHeader_4, this.columnHeader_5, this.columnHeader_6, this.columnHeader_7, this.columnHeader_8, this.columnHeader_9, this.columnHeader_10, this.columnHeader_11, this.columnHeader_12, this.columnHeader_13, this.columnHeader_14 });
            this.lvTrades.Dock = DockStyle.Fill;
            this.lvTrades.FullRowSelect = true;
            this.lvTrades.HideSelection = false;
            this.lvTrades.Location = new Point(0, 0);
            this.lvTrades.MultiSelect = false;
            this.lvTrades.Name = "lvTrades";
            this.lvTrades.Size = new Size(0x34e, 0x1a3);
            this.lvTrades.SmallImageList = this.imageList_0;
            this.lvTrades.TabIndex = 2;
            this.lvTrades.UseCompatibleStateImageBehavior = false;
            this.lvTrades.View = View.Details;
            this.lvTrades.DoubleClick += new EventHandler(this.lvTrades_DoubleClick);
            this.columnHeader_0.Text = "Position";
            this.columnHeader_0.Width = 0x4b;
            this.columnHeader_1.DisplayIndex = 2;
            this.columnHeader_1.Text = "Symbol";
            this.columnHeader_1.Width = 0x4b;
            this.columnHeader_2.DisplayIndex = 3;
            this.columnHeader_2.Tag = "N";
            this.columnHeader_2.Text = "Quantity";
            this.columnHeader_2.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_2.Width = 0x4b;
            this.columnHeader_3.DisplayIndex = 4;
            this.columnHeader_3.Tag = "D";
            this.columnHeader_3.Text = "Entry Date";
            this.columnHeader_3.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_3.Width = 0x4b;
            this.columnHeader_4.DisplayIndex = 5;
            this.columnHeader_4.Tag = "C";
            this.columnHeader_4.Text = "Entry Price";
            this.columnHeader_4.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_4.Width = 0x4b;
            this.columnHeader_5.DisplayIndex = 6;
            this.columnHeader_5.Tag = "D";
            this.columnHeader_5.Text = "Exit Date";
            this.columnHeader_5.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_5.Width = 0x4b;
            this.columnHeader_6.DisplayIndex = 7;
            this.columnHeader_6.Tag = "C";
            this.columnHeader_6.Text = "Exit Price";
            this.columnHeader_6.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_6.Width = 0x4b;
            this.columnHeader_7.DisplayIndex = 8;
            this.columnHeader_7.Tag = "N";
            this.columnHeader_7.Text = "Profit %";
            this.columnHeader_7.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_7.Width = 0x4b;
            this.columnHeader_8.DisplayIndex = 9;
            this.columnHeader_8.Tag = "C";
            this.columnHeader_8.Text = "Profit $";
            this.columnHeader_8.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_8.Width = 0x4b;
            this.columnHeader_9.DisplayIndex = 10;
            this.columnHeader_9.Tag = "N";
            this.columnHeader_9.Text = "Bars Held";
            this.columnHeader_9.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_9.Width = 0x4b;
            this.columnHeader_10.DisplayIndex = 11;
            this.columnHeader_10.Tag = "C";
            this.columnHeader_10.Text = "Profit per Bar";
            this.columnHeader_10.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_10.Width = 0x4b;
            this.columnHeader_11.DisplayIndex = 12;
            this.columnHeader_11.Text = "Entry Name";
            this.columnHeader_11.Width = 0x4b;
            this.columnHeader_12.DisplayIndex = 13;
            this.columnHeader_12.Text = "Exit Name";
            this.columnHeader_12.Width = 0x4b;
            this.columnHeader_13.DisplayIndex = 14;
            this.columnHeader_13.Tag = "N";
            this.columnHeader_13.Text = "MAE %";
            this.columnHeader_13.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_13.Width = 0x4b;
            this.columnHeader_14.DisplayIndex = 15;
            this.columnHeader_14.Tag = "N";
            this.columnHeader_14.Text = "MFE %";
            this.columnHeader_14.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_14.Width = 0x4b;
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
            this.mniPrint.ToolTipText = "Print the data";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.mniPrintAll.Name = "mniPrintAll";
            this.mniPrintAll.Size = new Size(0x79, 0x16);
            this.mniPrintAll.Text = "Print All";
            this.mniPrintAll.ToolTipText = "Print content from all tabs";
            this.mniPrintAll.Click += new EventHandler(this.mniPrintAll_Click);
            this.columnHeader_15.Text = "Strategy";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.ContextMenuStrip = this.popup;
            base.Controls.Add(this.lvTrades);
            base.Controls.Add(this.status);
            base.Name = "PVTradeList";
            base.Size = new Size(0x34e, 0x1b9);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.popup.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lvTrades_DoubleClick(object sender, EventArgs e)
        {
            if (this.lvTrades.SelectedItems.Count > 0)
            {
                ListViewItem item = this.lvTrades.SelectedItems[0];
                Position tag = (Position) item.Tag;
                this.ivisualizerHost_0.SelectPosition(tag);
            }
        }

        private void method_0(SystemPerformance systemPerformance_0)
        {
            bool flag2 = systemPerformance_0.Strategy.StrategyType == StrategyType.CombinedStrategy;
            SystemResults results = systemPerformance_0.Results;
            this.statusTrades.Text = systemPerformance_0.Results.Positions.Count.ToString();
            if (systemPerformance_0.PositionSize.Mode == PosSizeMode.SimuScript)
            {
                this.lblNSF.Text = "Trades not included due to insufficient simulated capital or selected PosSizer:";
            }
            else
            {
                this.lblNSF.Text = "Trades not included due to insufficient simulated capital:";
            }
            this.statusNSF.Text = systemPerformance_0.Results.TradesNSF.ToString();
            this.lvTrades.DisableSort();
            this.lvTrades.BeginUpdate();
            this.lvTrades.Items.Clear();
            bool isIntraday = systemPerformance_0.IsIntraday;
            foreach (Bars bars in systemPerformance_0.Bars)
            {
                if (bars.IsIntraday)
                {
                    isIntraday = true;
                }
            }
            if (isIntraday)
            {
                this.columnHeader_3.Width = 150;
                this.columnHeader_5.Width = 150;
                this.columnHeader_3.Tag = "DT";
                this.columnHeader_5.Tag = "DT";
            }
            else
            {
                this.columnHeader_3.Width = 0x4b;
                this.columnHeader_5.Width = 0x4b;
                this.columnHeader_3.Tag = "D";
                this.columnHeader_5.Tag = "D";
            }
            foreach (Position position in results.Positions)
            {
                if (font_0 == null)
                {
                    font_0 = new Font(this.lvTrades.Font, FontStyle.Bold);
                }
                string format = "N" + position.Bars.SymbolInfo.Decimals;
                this.listViewItem_0 = this.lvTrades.Items.Add(position.PositionType.ToString());
                this.listViewItem_0.Tag = position;
                this.listViewItem_0.UseItemStyleForSubItems = false;
                if (position.NetProfit > 0.0)
                {
                    this.listViewItem_0.BackColor = color_1;
                }
                else
                {
                    this.listViewItem_0.BackColor = color_0;
                }
                if (position.PositionType == PositionType.Long)
                {
                    if (position.Active)
                    {
                        this.listViewItem_0.ImageIndex = 0;
                    }
                    else
                    {
                        this.listViewItem_0.ImageIndex = 1;
                    }
                }
                else if (position.Active)
                {
                    this.listViewItem_0.ImageIndex = 2;
                }
                else
                {
                    this.listViewItem_0.ImageIndex = 3;
                }
                if (flag2)
                {
                    this.listViewItem_0.SubItems.Add(this.ivisualizerHost_0.LookupStrategyName(position.StrategyID));
                    this.method_2();
                }
                this.listViewItem_0.SubItems.Add(position.Bars.Symbol);
                this.listViewItem_0.SubItems[this.listViewItem_0.SubItems.Count - 1].Font = font_0;
                this.method_2();
                this.listViewItem_0.SubItems.Add(position.Shares.ToString("N0"));
                this.method_2();
                if (isIntraday)
                {
                    this.listViewItem_0.SubItems.Add(position.EntryDate.ToShortDateString() + " " + position.EntryDate.ToShortTimeString());
                }
                else
                {
                    this.listViewItem_0.SubItems.Add(position.EntryDate.ToShortDateString());
                }
                this.method_2();
                this.listViewItem_0.SubItems.Add(position.EntryPrice.ToString(format));
                this.method_2();
                if (position.Active)
                {
                    this.listViewItem_0.SubItems.Add("Open");
                    this.method_2();
                    this.listViewItem_0.SubItems.Add("Open");
                    this.method_2();
                }
                else
                {
                    if (isIntraday)
                    {
                        this.listViewItem_0.SubItems.Add(position.ExitDate.ToShortDateString() + " " + position.ExitDate.ToShortTimeString());
                    }
                    else
                    {
                        this.listViewItem_0.SubItems.Add(position.ExitDate.ToShortDateString());
                    }
                    this.method_2();
                    this.listViewItem_0.SubItems.Add(position.ExitPrice.ToString(format));
                    this.method_2();
                }
                this.listViewItem_0.SubItems.Add(position.NetProfitPercent.ToString("F2"));
                this.method_3(position.NetProfitPercent);
                this.listViewItem_0.SubItems.Add(position.NetProfit.ToString("C"));
                this.method_3(position.NetProfit);
                this.listViewItem_0.SubItems.Add(position.BarsHeld.ToString("N0"));
                this.method_2();
                this.listViewItem_0.SubItems.Add(position.ProfitPerBar.ToString("C"));
                this.method_3(position.ProfitPerBar);
                this.listViewItem_0.SubItems.Add(position.EntrySignal);
                this.method_2();
                this.listViewItem_0.SubItems.Add(position.ExitSignal);
                this.method_2();
                this.listViewItem_0.SubItems.Add(position.MAEPercent.ToString("F2"));
                this.method_3(position.MAEPercent);
                this.listViewItem_0.SubItems.Add(position.MFEPercent.ToString("F2"));
                this.method_3(position.MFEPercent);
            }
            this.lvTrades.EndUpdate();
        }

        private void method_1(ref DataObject dataObject_0)
        {
            dataObject_0.SetData(PrintReport.fmtBaseTitle.Name, this.ivisualizerHost_0.ApplicationName());
            dataObject_0.SetData(PrintReport.fmtTitle.Name, "Trade List");
            this.ivisualizerHost_0.StrategySummary(ref dataObject_0);
            dataObject_0.SetData(PrintReport.fmtListView.Name, this.lvTrades);
            dataObject_0.SetData(PrintReport.fmtDisclosure.Name, "1. " + PrintReport.DefaultDisclosure + "\n2. Profit and Profit % represent profit and % profit of a trade, less commissons and slippage (based on customer preference settings).");
        }

        private void method_2()
        {
            this.listViewItem_0.SubItems[this.listViewItem_0.SubItems.Count - 1].BackColor = this.listViewItem_0.BackColor;
        }

        private void method_3(double double_0)
        {
            this.method_2();
            Color color = (double_0 > 0.0) ? Color.Blue : Color.Red;
            this.listViewItem_0.SubItems[this.listViewItem_0.SubItems.Count - 1].ForeColor = color;
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
            PrintReport report = new PrintReport(obj2) {
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
                return "Displays a list of all of the trades (Positions) generated by the Strategy.  You can sort by any column, then double click to zero in on a particular trade on the chart.";
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
                return "Trades";
            }
        }
    }
}

