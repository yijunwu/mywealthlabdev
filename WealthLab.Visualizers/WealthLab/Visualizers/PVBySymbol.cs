namespace WealthLab.Visualizers
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class PVBySymbol : UserControl, IPerformanceVisualizer
    {
        private static Color color_0 = Color.FromArgb(0xff, 230, 230);
        private static Color color_1 = Color.FromArgb(230, 0xff, 230);
        private static Color color_2 = Color.FromArgb(230, 230, 230);
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private ColumnHeader columnHeader_5;
        private ColumnHeader columnHeader_6;
        private ColumnHeader columnHeader_7;
        private static Font font_0 = null;
        private IContainer icontainer_0;
        private IVisualizerHost ivisualizerHost_0;
        private ListViewItem listViewItem_0;
        private SortableListView lvSymbols;
        private ToolStripMenuItem mniCopy;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrintAll;
        private PageSettings pageSettings_0 = new PageSettings();
        private ContextMenuStrip popup;

        public PVBySymbol()
        {
            this.InitializeComponent();
        }

        public void CopyToClipboard()
        {
            this.ivisualizerHost_0.CopyListViewToClipboard(this.lvSymbols);
        }

        public void CreateVisualization(SystemPerformance performance, IVisualizerHost visHost)
        {
            this.ivisualizerHost_0 = visHost;
            this.lvSymbols.BeginUpdate();
            this.lvSymbols.Items.Clear();
            while (this.lvSymbols.Columns.Count > 8)
            {
                this.lvSymbols.Columns.RemoveAt(8);
            }
            List<string> list = new List<string>();
            if (performance.Bars.Count > 0)
            {
                foreach (Bars bars3 in performance.Bars)
                {
                    foreach (DataSeries series2 in bars3.Cache.Values)
                    {
                        if (!list.Contains(series2.Description))
                        {
                            list.Add(series2.Description);
                        }
                    }
                }
                foreach (Bars bars in performance.Bars)
                {
                    using (List<string>.Enumerator enumerator3 = list.GetEnumerator())
                    {
                        string current;
                        while (enumerator3.MoveNext())
                        {
                            current = enumerator3.Current;
                            if (!bars.Cache.ContainsKey(current) && (bars.Count > 0))
                            {
                                ///goto  Label_0133;  ///WYJ fix, simplify the flow
                                list.Remove(current);
                                break;
                            }
                        }
                        continue;
                    }
                }
                foreach (string str2 in list)
                {
                    ColumnHeader header = this.lvSymbols.Columns.Add(str2);
                    header.Width = 60;
                    header.TextAlign = HorizontalAlignment.Right;
                    header.Tag = "N";
                }
            }
            try
            {
                foreach (Bars bars2 in performance.Bars)
                {
                    double num = 0.0;
                    int num2 = 0;
                    int num3 = 0;
                    int num4 = 0;
                    foreach (Position position in performance.Results.Positions)
                    {
                        if (position.Bars == bars2)
                        {
                            num += position.NetProfit;
                            num2 += position.BarsHeld;
                            num3++;
                            if (position.NetProfit > 0.0)
                            {
                                num4++;
                            }
                        }
                    }
                    double num6 = 0.0;
                    int num7 = 0;
                    int num8 = 0;
                    foreach (Position position2 in performance.ResultsBuyHold.Positions)
                    {
                        if (position2.Bars == bars2)
                        {
                            num6 += position2.NetProfit;
                            num7 += position2.BarsHeld;
                            num8++;
                        }
                    }
                    this.listViewItem_0 = this.lvSymbols.Items.Add(bars2.Symbol);
                    this.listViewItem_0.UseItemStyleForSubItems = false;
                    if (num == 0.0)
                    {
                        this.listViewItem_0.BackColor = color_2;
                    }
                    else if (num > 0.0)
                    {
                        this.listViewItem_0.BackColor = color_1;
                    }
                    else
                    {
                        this.listViewItem_0.BackColor = color_0;
                    }
                    if (font_0 == null)
                    {
                        font_0 = new Font(this.lvSymbols.Font, FontStyle.Bold);
                    }
                    this.listViewItem_0.Font = font_0;
                    this.listViewItem_0.SubItems.Add(num.ToString("C"));
                    this.method_0(num);
                    this.listViewItem_0.SubItems.Add(num6.ToString("C"));
                    this.method_0(num6);
                    double num9 = 0.0;
                    if (num2 > 0)
                    {
                        num9 = num / ((double) num2);
                    }
                    this.listViewItem_0.SubItems.Add(num9.ToString("C"));
                    this.method_0(num9);
                    num9 = 0.0;
                    if (num7 > 0)
                    {
                        num9 = num6 / ((double) num7);
                    }
                    this.listViewItem_0.SubItems.Add(num9.ToString("C"));
                    this.method_0(num9);
                    this.listViewItem_0.SubItems.Add(num3.ToString("N0"));
                    this.method_1();
                    double num10 = 0.0;
                    double num11 = 0.0;
                    if (num3 > 0)
                    {
                        num10 = (num4 * 100.0) / ((double) num3);
                        num11 = ((double) num2) / ((double) num3);
                    }
                    this.listViewItem_0.SubItems.Add(num10.ToString("N2"));
                    this.method_1();
                    this.listViewItem_0.SubItems.Add(num11.ToString("N2"));
                    this.method_1();
                    foreach (string str3 in list)
                    {
                        double num5 = 0.0;
                        if (bars2.Cache.ContainsKey(str3))
                        {
                            DataSeries series = bars2.Cache[str3];
                            if (series.Count > 0)
                            {
                                num5 = series[series.Count - 1];
                            }
                        }
                        this.listViewItem_0.SubItems.Add(num5.ToString("N4"));
                        this.method_1();
                    }
                }
            }
            finally
            {
                this.lvSymbols.EndUpdate();
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
            this.lvSymbols.Enabled = enable;
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PVBySymbol));
            this.lvSymbols = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.columnHeader_5 = new ColumnHeader();
            this.columnHeader_7 = new ColumnHeader();
            this.columnHeader_6 = new ColumnHeader();
            this.popup = new ContextMenuStrip(this.icontainer_0);
            this.mniCopy = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.mniPrintAll = new ToolStripMenuItem();
            this.popup.SuspendLayout();
            base.SuspendLayout();
            this.lvSymbols.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_1, this.columnHeader_2, this.columnHeader_3, this.columnHeader_4, this.columnHeader_5, this.columnHeader_7, this.columnHeader_6 });
            this.lvSymbols.ContextMenuStrip = this.popup;
            this.lvSymbols.Dock = DockStyle.Fill;
            this.lvSymbols.FullRowSelect = true;
            this.lvSymbols.HideSelection = false;
            this.lvSymbols.Location = new Point(0, 0);
            this.lvSymbols.MultiSelect = false;
            this.lvSymbols.Name = "lvSymbols";
            this.lvSymbols.Size = new Size(0x291, 0x19f);
            this.lvSymbols.TabIndex = 0;
            this.lvSymbols.UseCompatibleStateImageBehavior = false;
            this.lvSymbols.View = View.Details;
            this.lvSymbols.DoubleClick += new EventHandler(this.lvSymbols_DoubleClick);
            this.columnHeader_0.Tag = "S";
            this.columnHeader_0.Text = "Symbol";
            this.columnHeader_1.Tag = "C";
            this.columnHeader_1.Text = "Profit";
            this.columnHeader_1.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_1.Width = 100;
            this.columnHeader_2.Tag = "C";
            this.columnHeader_2.Text = "Buy & Hold Profit";
            this.columnHeader_2.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_2.Width = 100;
            this.columnHeader_3.Tag = "C";
            this.columnHeader_3.Text = "Profit per Bar";
            this.columnHeader_3.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_3.Width = 80;
            this.columnHeader_4.Tag = "C";
            this.columnHeader_4.Text = "B&H per Bar";
            this.columnHeader_4.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_4.Width = 80;
            this.columnHeader_5.Tag = "N";
            this.columnHeader_5.Text = "Trades";
            this.columnHeader_5.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_7.Tag = "N";
            this.columnHeader_7.Text = "Pct Winners";
            this.columnHeader_7.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_7.Width = 70;
            this.columnHeader_6.Tag = "N";
            this.columnHeader_6.Text = "Avg Bars Held";
            this.columnHeader_6.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_6.Width = 80;
            this.popup.Items.AddRange(new ToolStripItem[] { this.mniCopy, this.mniPrint, this.mniPrintAll });
            this.popup.Name = "popup";
            this.popup.Size = new Size(0x99, 0x5c);
            this.mniCopy.Image = (Image) manager.GetObject("mniCopy.Image");
            this.mniCopy.ImageTransparentColor = Color.Fuchsia;
            this.mniCopy.Name = "mniCopy";
            this.mniCopy.Size = new Size(0x98, 0x16);
            this.mniCopy.Text = "Copy";
            this.mniCopy.Click += new EventHandler(this.mniCopy_Click);
            this.mniPrint.Image = (Image) manager.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0x98, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.mniPrintAll.Name = "mniPrintAll";
            this.mniPrintAll.Size = new Size(0x98, 0x16);
            this.mniPrintAll.Text = "Print All";
            this.mniPrintAll.ToolTipText = "Print content from all tabs";
            this.mniPrintAll.Click += new EventHandler(this.mniPrintAll_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.lvSymbols);
            base.Name = "PVBySymbol";
            base.Size = new Size(0x291, 0x19f);
            this.popup.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void lvSymbols_DoubleClick(object sender, EventArgs e)
        {
            if (this.lvSymbols.SelectedItems.Count != 0)
            {
                ListViewItem item = this.lvSymbols.SelectedItems[0];
                this.ivisualizerHost_0.SelectSymbol(item.Text);
            }
        }

        private void method_0(double double_0)
        {
            this.method_1();
            if (double_0 > 0.0)
            {
                this.listViewItem_0.SubItems[this.listViewItem_0.SubItems.Count - 1].ForeColor = Color.Blue;
            }
            else
            {
                this.listViewItem_0.SubItems[this.listViewItem_0.SubItems.Count - 1].ForeColor = Color.Red;
            }
        }

        private void method_1()
        {
            this.listViewItem_0.SubItems[this.listViewItem_0.SubItems.Count - 1].BackColor = this.listViewItem_0.BackColor;
        }

        private void method_2(ref DataObject dataObject_0)
        {
            dataObject_0.SetData(PrintReport.fmtBaseTitle.Name, this.ivisualizerHost_0.ApplicationName());
            dataObject_0.SetData(PrintReport.fmtTitle.Name, "By Symbol");
            this.ivisualizerHost_0.StrategySummary(ref dataObject_0);
            dataObject_0.SetData(PrintReport.fmtListView.Name, this.lvSymbols);
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
            this.method_2(ref obj2);
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
                return (VisualizerAppliesTo.PortfolioSim | VisualizerAppliesTo.RawProfit | VisualizerAppliesTo.MultiSymbol);
            }
        }

        public string Description
        {
            get
            {
                return "Displays performance statistics for each Symbol that was backtested, compared to Buy && Hold.";
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
                return "By Symbol";
            }
        }
    }
}

