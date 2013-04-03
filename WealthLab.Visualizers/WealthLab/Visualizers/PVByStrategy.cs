namespace WealthLab.Visualizers
{
    using Fidelity.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class PVByStrategy : UserControl, IPerformanceVisualizer
    {
        private static Color color_0 = Color.FromArgb(0xff, 230, 230);
        private static Color color_1 = Color.FromArgb(230, 0xff, 230);
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private ColumnHeader columnHeader_5;
        private ColumnHeader columnHeader_6;
        private ColumnHeader columnHeader_7;
        private IContainer icontainer_0;
        private ToolStripStatusLabel lblNSF;
        private ToolStripStatusLabel lblTrades;
        private ListViewItem listViewItem_0;
        private SortableListView lvStrategies;
        private StatusStrip status;
        private ToolStripStatusLabel statusNSF;
        private ToolStripStatusLabel statusTrades;
        private SystemPerformance systemPerformance_0;

        public PVByStrategy()
        {
            this.InitializeComponent();
        }

        public void CopyToClipboard()
        {
        }

        public void CreateVisualization(SystemPerformance performance, IVisualizerHost visHost)
        {
            this.systemPerformance_0 = performance;
            this.lvStrategies.Items.Clear();
            foreach (CombinedStrategyInfo info in performance.Strategy.CombinedStrategyChildren)
            {
                SystemPerformance performance2 = performance.GenerateChildStrategyPerformance(info, visHost.GetExecutor());
                this.method_0(performance2, info);
            }
            this.statusTrades.Text = performance.Results.Positions.Count.ToString();
            if (performance.PositionSize.Mode == PosSizeMode.SimuScript)
            {
                this.lblNSF.Text = "Trades not included due to insufficient simulated capital or selected PosSizer:";
            }
            else
            {
                this.lblNSF.Text = "Trades not included due to insufficient simulated capital:";
            }
            this.statusNSF.Text = performance.Results.TradesNSF.ToString();
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
            this.lvStrategies = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_6 = new ColumnHeader();
            this.columnHeader_7 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.columnHeader_5 = new ColumnHeader();
            this.status = new StatusStrip();
            this.lblTrades = new ToolStripStatusLabel();
            this.statusTrades = new ToolStripStatusLabel();
            this.lblNSF = new ToolStripStatusLabel();
            this.statusNSF = new ToolStripStatusLabel();
            this.status.SuspendLayout();
            base.SuspendLayout();
            this.lvStrategies.BackColor = SystemColors.Window;
            this.lvStrategies.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_6, this.columnHeader_7, this.columnHeader_3, this.columnHeader_1, this.columnHeader_2, this.columnHeader_4, this.columnHeader_5 });
            this.lvStrategies.Dock = DockStyle.Fill;
            this.lvStrategies.FullRowSelect = true;
            this.lvStrategies.HideSelection = false;
            this.lvStrategies.Location = new Point(0, 0);
            this.lvStrategies.MultiSelect = false;
            this.lvStrategies.Name = "lvStrategies";
            this.lvStrategies.Size = new Size(0x2c8, 0x1d5);
            this.lvStrategies.TabIndex = 4;
            this.lvStrategies.UseCompatibleStateImageBehavior = false;
            this.lvStrategies.View = View.Details;
            this.lvStrategies.DoubleClick += new EventHandler(this.lvStrategies_DoubleClick);
            this.columnHeader_0.Text = "Strategy Name";
            this.columnHeader_0.Width = 100;
            this.columnHeader_6.Text = "Profit";
            this.columnHeader_6.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_6.Width = 0x4b;
            this.columnHeader_7.Text = "Buy & Hold Profit";
            this.columnHeader_7.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_7.Width = 0x4b;
            this.columnHeader_3.Tag = "";
            this.columnHeader_3.Text = "Profit Per Bar";
            this.columnHeader_3.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_3.Width = 0x4b;
            this.columnHeader_1.Tag = "";
            this.columnHeader_1.Text = "B & H Per Bar";
            this.columnHeader_1.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_1.Width = 0x4b;
            this.columnHeader_2.Tag = "";
            this.columnHeader_2.Text = "Trades";
            this.columnHeader_2.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_2.Width = 0x4b;
            this.columnHeader_4.Text = "% Winners";
            this.columnHeader_4.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_4.Width = 0x4b;
            this.columnHeader_5.Text = "Avg Bars Held";
            this.columnHeader_5.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_5.Width = 90;
            this.status.Items.AddRange(new ToolStripItem[] { this.lblTrades, this.statusTrades, this.lblNSF, this.statusNSF });
            this.status.Location = new Point(0, 0x1d5);
            this.status.Name = "status";
            this.status.Size = new Size(0x2c8, 0x16);
            this.status.TabIndex = 3;
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
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.lvStrategies);
            base.Controls.Add(this.status);
            base.Name = "PVByStrategy";
            base.Size = new Size(0x2c8, 0x1eb);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lvStrategies_DoubleClick(object sender, EventArgs e)
        {
            if (this.lvStrategies.SelectedItems.Count != 0)
            {
                CombinedStrategyInfo tag = (CombinedStrategyInfo) this.lvStrategies.SelectedItems[0].Tag;
                this.systemPerformance_0.SignalEvent(tag.StrategyID.ToString());
            }
        }

        private void method_0(SystemPerformance systemPerformance_1, CombinedStrategyInfo combinedStrategyInfo_0)
        {
            this.lvStrategies.DisableSort();
            this.lvStrategies.BeginUpdate();
            this.listViewItem_0 = this.lvStrategies.Items.Add(combinedStrategyInfo_0.Name);
            this.listViewItem_0.Tag = combinedStrategyInfo_0;
            this.listViewItem_0.UseItemStyleForSubItems = false;
            if (systemPerformance_1.Results.NetProfit > 0.0)
            {
                this.listViewItem_0.BackColor = color_1;
            }
            else
            {
                this.listViewItem_0.BackColor = color_0;
            }
            this.listViewItem_0.SubItems.Add(systemPerformance_1.Results.NetProfit.ToString("C"));
            this.method_2(systemPerformance_1.Results.NetProfit);
            this.listViewItem_0.SubItems.Add(systemPerformance_1.ResultsBuyHold.NetProfit.ToString("C"));
            this.method_2(systemPerformance_1.ResultsBuyHold.NetProfit);
            this.listViewItem_0.SubItems.Add(systemPerformance_1.Results.ProfitPerBar.ToString("C"));
            this.method_2(systemPerformance_1.Results.ProfitPerBar);
            this.listViewItem_0.SubItems.Add(systemPerformance_1.ResultsBuyHold.ProfitPerBar.ToString("C"));
            this.method_2(systemPerformance_1.ResultsBuyHold.ProfitPerBar);
            this.listViewItem_0.SubItems.Add(systemPerformance_1.Results.Positions.Count.ToString());
            this.method_1();
            bool rawProfitMode = systemPerformance_1.PositionSize.RawProfitMode;
            int count = systemPerformance_1.Results.Positions.Count;
            int num3 = 0;
            double num = 0.0;
            double num6 = 0.0;
            double num7 = 0.0;
            double num5 = 0.0;
            double num2 = 0.0;
            foreach (Position position in systemPerformance_1.Results.Positions)
            {
                if (position.NetProfit > 0.0)
                {
                    num3++;
                    num6 += position.NetProfit;
                    num7 += position.NetProfitPercent;
                    num5 += position.BarsHeld;
                }
                if (num3 > 0)
                {
                    num = (num3 * 100.0) / ((double) count);
                    num2 = num5 / ((double) num3);
                }
                else
                {
                    num = 0.0;
                    num2 = 0.0;
                }
            }
            this.listViewItem_0.SubItems.Add(num.ToString("N2"));
            this.method_1();
            this.listViewItem_0.SubItems.Add(num2.ToString("N2"));
            this.method_1();
            this.lvStrategies.EndUpdate();
        }

        private void method_1()
        {
            this.listViewItem_0.SubItems[this.listViewItem_0.SubItems.Count - 1].BackColor = this.listViewItem_0.BackColor;
        }

        private void method_2(double double_0)
        {
            this.method_1();
            Color color = (double_0 > 0.0) ? Color.Blue : Color.Red;
            this.listViewItem_0.SubItems[this.listViewItem_0.SubItems.Count - 1].ForeColor = color;
        }

        public void Print()
        {
        }

        public VisualizerAppliesTo AppliesTo
        {
            get
            {
                return (VisualizerAppliesTo.CombinationStrategy | VisualizerAppliesTo.All);
            }
        }

        public string Description
        {
            get
            {
                return "This visualizer shows results of each strategy of Combination strategy ";
            }
        }

        public bool SupportClipboardCopy
        {
            get
            {
                return false;
            }
        }

        public bool SupportsPrint
        {
            get
            {
                return false;
            }
        }

        public string TabText
        {
            get
            {
                return "By Strategy";
            }
        }
    }
}

