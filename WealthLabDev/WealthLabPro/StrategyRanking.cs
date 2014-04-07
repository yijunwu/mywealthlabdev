namespace WealthLabPro
{
    using Fidelity.Components;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;

    public class StrategyRanking : Form, IWorkspace, IWealthScriptProvider
    {
        private BarDataRange barDataRange_0;
        private WealthLab.BarScale barScale_0;
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private bool bool_3;
        private ToolStripButton btnActivate;
        private ToolStripButton btnAdd;
        private ToolStripButton btnCancel;
        private ToolStripButton btnRemove;
        private ToolStripButton btnUsePV;
        private ToolStripComboBox cmbScorecard;
        private ColumnHeader columnHeader_0;
        private DataSource dataSource_0;
        private DataSourceManager dataSourceManager_0;
        private Exception exception_0;
        private FundamentalsLoader fundamentalsLoader_0;
        private IContainer components;
        private static readonly ILog ilog_0 = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ImageList imageList_0;
        private int int_0;
        private ToolStripLabel lblScorecard;
        private ToolStripLabel lblStrategy;
        private List<StrategyScorecard> list_0 = new List<StrategyScorecard>();
        private List<Bars> list_1 = new List<Bars>();
        private SortableListView lvStrategies;
        private ToolStripMenuItem mniAddStrategy;
        private ToolStripMenuItem mniCopyResults;
        private ToolStripMenuItem mniOpen;
        private ToolStripMenuItem mniPrintResults;
        private ToolStripMenuItem mniPV;
        private ToolStripMenuItem mniRemoveStrategy;
        private object object_0 = new object();
        private object object_1 = new object();
        private ContextMenuStrip popupStrategies;
        private WealthLab.PositionSize positionSize_0;
        private ToolStripSeparator sepMenu;
        private ToolStripSeparator sepMenu3;
        private ToolStripStatusLabel statusDataSet;
        private ToolStripStatusLabel statusMessage;
        private ToolStripStatusLabel statusPositionSize;
        private ToolStripStatusLabel statusRange;
        private ToolStripStatusLabel statusScale;
        private ToolStripStatusLabel statusStrategies;
        private StatusStrip statusStrip1;
        private StrategyRankingItem strategyRankingItem_0;
        private StrategyRankingSettings strategyRankingSettings_0 = new StrategyRankingSettings();
        private StrategyScorecard strategyScorecard_0;
        private string string_0;
        private string string_1 = "";
        private TabControl tabControl1;
        private TabPage tabErrors;
        private TabPage tabResults;
        private TextBox tbErrors;
        private Thread thread_0;
        private ToolStrip toolbar;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private TradingSystemExecutor tradingSystemExecutor_0;

        public StrategyRanking()
        {
            this.InitializeComponent();
            this.string_0 = MainModule.Instance.DataPath + @"\StrategyRankings.xml";
            this.dataSourceManager_0 = MainModule.Instance.DataSources;
        }

        public void Activate(bool _value)
        {
            this.bool_2 = _value;
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            this.ExecuteRankings();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.method_8())
            {
                this.method_13();
                this.method_16();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.bool_0 = true;
            if (this.IsBusy)
            {
                this.thread_0.Abort();
                this.thread_0 = null;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.method_10();
            this.method_16();
        }

        private void btnUsePV_Click(object sender, EventArgs e)
        {
            this.btnUsePV.Checked = !this.btnUsePV.Checked;
            this.mniPV.Checked = this.btnUsePV.Checked;
            foreach (ListViewItem item2 in this.lvStrategies.SelectedItems)
            {
                StrategyRankingItem tag = (StrategyRankingItem) item2.Tag;
                if (tag.UsePreferredValues && !this.btnUsePV.Checked)
                {
                    tag.WealthScript.RestoreParameterDefaults();
                }
                tag.UsePreferredValues = this.btnUsePV.Checked;
            }
            this.method_17();
        }

        private void cmbScorecard_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.method_0();
        }

        public void CopyToClipboard()
        {
            MainModule.Instance.CopyListViewToClipboard(this.lvStrategies);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void ExecuteRankings()
        {
            if (!this.bool_2)
            {
                if (this.bool_3)
                {
                    this.method_0();
                }
                if (!this.SymbolsSelected)
                {
                    MessageBox.Show("Please select a symbol or Dataset");
                }
                else if (this.method_16())
                {
                    this.method_12();
                    this.method_37(false);
                    this.method_36();
                    this.bool_0 = false;
                    List<StrategyRankingItem> parameter = new List<StrategyRankingItem>();
                    foreach (ListViewItem item in this.lvStrategies.Items)
                    {
                        StrategyRankingItem tag = (StrategyRankingItem) item.Tag;
                        parameter.Add(tag);
                    }
                    this.thread_0 = new Thread(new ParameterizedThreadStart(this.method_23));
                    this.thread_0.IsBackground = true;
                    this.thread_0.Start(parameter);
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(StrategyRanking));
            WealthLab.PositionSize size = new WealthLab.PositionSize();
            this.tabControl1 = new TabControl();
            this.tabResults = new TabPage();
            this.lvStrategies = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.popupStrategies = new ContextMenuStrip(this.components);
            this.mniAddStrategy = new ToolStripMenuItem();
            this.mniRemoveStrategy = new ToolStripMenuItem();
            this.mniPV = new ToolStripMenuItem();
            this.sepMenu = new ToolStripSeparator();
            this.mniOpen = new ToolStripMenuItem();
            this.sepMenu3 = new ToolStripSeparator();
            this.mniCopyResults = new ToolStripMenuItem();
            this.mniPrintResults = new ToolStripMenuItem();
            this.imageList_0 = new ImageList(this.components);
            this.tabErrors = new TabPage();
            this.tbErrors = new TextBox();
            this.toolbar = new ToolStrip();
            this.lblScorecard = new ToolStripLabel();
            this.cmbScorecard = new ToolStripComboBox();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.lblStrategy = new ToolStripLabel();
            this.btnAdd = new ToolStripButton();
            this.btnRemove = new ToolStripButton();
            this.btnUsePV = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.btnActivate = new ToolStripButton();
            this.btnCancel = new ToolStripButton();
            this.statusStrip1 = new StatusStrip();
            this.statusStrategies = new ToolStripStatusLabel();
            this.statusDataSet = new ToolStripStatusLabel();
            this.statusScale = new ToolStripStatusLabel();
            this.statusRange = new ToolStripStatusLabel();
            this.statusPositionSize = new ToolStripStatusLabel();
            this.statusMessage = new ToolStripStatusLabel();
            this.tradingSystemExecutor_0 = new TradingSystemExecutor(this.components);
            this.fundamentalsLoader_0 = new FundamentalsLoader(this.components);
            this.tabControl1.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.popupStrategies.SuspendLayout();
            this.tabErrors.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabResults);
            this.tabControl1.Controls.Add(this.tabErrors);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0x19);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x306, 0x196);
            this.tabControl1.TabIndex = 2;
            this.tabResults.Controls.Add(this.lvStrategies);
            this.tabResults.Location = new Point(4, 0x16);
            this.tabResults.Name = "tabResults";
            this.tabResults.Padding = new Padding(3);
            this.tabResults.Size = new Size(0x2fe, 380);
            this.tabResults.TabIndex = 0;
            this.tabResults.Text = "Results";
            this.tabResults.UseVisualStyleBackColor = true;
            this.lvStrategies.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0 });
            this.lvStrategies.ContextMenuStrip = this.popupStrategies;
            this.lvStrategies.Dock = DockStyle.Fill;
            this.lvStrategies.FullRowSelect = true;
            this.lvStrategies.HideSelection = false;
            this.lvStrategies.Location = new Point(3, 3);
            this.lvStrategies.Name = "lvStrategies";
            this.lvStrategies.Size = new Size(760, 0x176);
            this.lvStrategies.SmallImageList = this.imageList_0;
            this.lvStrategies.StateImageList = this.imageList_0;
            this.lvStrategies.TabIndex = 0;
            this.lvStrategies.UseCompatibleStateImageBehavior = false;
            this.lvStrategies.View = View.Details;
            this.lvStrategies.SelectedIndexChanged += new EventHandler(this.lvStrategies_SelectedIndexChanged);
            this.lvStrategies.DoubleClick += new EventHandler(this.mniOpen_Click);
            this.lvStrategies.Leave += new EventHandler(this.lvStrategies_Leave);
            this.lvStrategies.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.lvStrategies_ItemSelectionChanged);
            this.columnHeader_0.Text = "Strategy";
            this.columnHeader_0.Width = 120;
            this.popupStrategies.Items.AddRange(new ToolStripItem[] { this.mniAddStrategy, this.mniRemoveStrategy, this.mniPV, this.sepMenu, this.mniOpen, this.sepMenu3, this.mniCopyResults, this.mniPrintResults });
            this.popupStrategies.Name = "popupStrategies";
            this.popupStrategies.Size = new Size(0x119, 0x94);
            this.popupStrategies.Opening += new CancelEventHandler(this.popupStrategies_Opening);
            this.mniAddStrategy.Image = (Image) resources.GetObject("mniAddStrategy.Image");
            this.mniAddStrategy.ImageTransparentColor = Color.Fuchsia;
            this.mniAddStrategy.Name = "mniAddStrategy";
            this.mniAddStrategy.Size = new Size(280, 0x16);
            this.mniAddStrategy.Text = "Add a Strategy ...";
            this.mniAddStrategy.Click += new EventHandler(this.btnAdd_Click);
            this.mniRemoveStrategy.Enabled = false;
            this.mniRemoveStrategy.Image = (Image) resources.GetObject("mniRemoveStrategy.Image");
            this.mniRemoveStrategy.ImageTransparentColor = Color.Fuchsia;
            this.mniRemoveStrategy.Name = "mniRemoveStrategy";
            this.mniRemoveStrategy.Size = new Size(280, 0x16);
            this.mniRemoveStrategy.Text = "Remove Selected Strategies";
            this.mniRemoveStrategy.Click += new EventHandler(this.btnRemove_Click);
            this.mniPV.Image = (Image) resources.GetObject("mniPV.Image");
            this.mniPV.Name = "mniPV";
            this.mniPV.Size = new Size(280, 0x16);
            this.mniPV.Text = "Use Preferred Values";
            this.mniPV.Click += new EventHandler(this.btnUsePV_Click);
            this.sepMenu.Name = "sepMenu";
            this.sepMenu.Size = new Size(0x115, 6);
            this.mniOpen.Enabled = false;
            this.mniOpen.Image = (Image) resources.GetObject("mniOpen.Image");
            this.mniOpen.ImageTransparentColor = Color.Fuchsia;
            this.mniOpen.Name = "mniOpen";
            this.mniOpen.Size = new Size(280, 0x16);
            this.mniOpen.Text = "Open this Strategy in a Strategy window";
            this.mniOpen.Click += new EventHandler(this.mniOpen_Click);
            this.sepMenu3.Name = "sepMenu3";
            this.sepMenu3.Size = new Size(0x115, 6);
            this.mniCopyResults.Image = (Image) resources.GetObject("mniCopyResults.Image");
            this.mniCopyResults.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyResults.Name = "mniCopyResults";
            this.mniCopyResults.Size = new Size(280, 0x16);
            this.mniCopyResults.Text = "Copy to Clipboard";
            this.mniCopyResults.Click += new EventHandler(this.mniCopyResults_Click);
            this.mniPrintResults.Image = (Image) resources.GetObject("mniPrintResults.Image");
            this.mniPrintResults.Name = "mniPrintResults";
            this.mniPrintResults.Size = new Size(280, 0x16);
            this.mniPrintResults.Text = "Print";
            this.mniPrintResults.Click += new EventHandler(this.mniPrintResults_Click);
            this.imageList_0.ImageStream = (ImageListStreamer) resources.GetObject("imageList.ImageStream");
            this.imageList_0.TransparentColor = Color.Transparent;
            this.imageList_0.Images.SetKeyName(0, "Check.bmp");
            this.imageList_0.Images.SetKeyName(1, "Canceled.bmp");
            this.imageList_0.Images.SetKeyName(2, "PV.bmp");
            this.tabErrors.Controls.Add(this.tbErrors);
            this.tabErrors.Location = new Point(4, 0x16);
            this.tabErrors.Name = "tabErrors";
            this.tabErrors.Padding = new Padding(3);
            this.tabErrors.Size = new Size(0x2fe, 0x17a);
            this.tabErrors.TabIndex = 1;
            this.tabErrors.Text = "Errors";
            this.tabErrors.UseVisualStyleBackColor = true;
            this.tbErrors.BackColor = SystemColors.Window;
            this.tbErrors.Dock = DockStyle.Fill;
            this.tbErrors.Location = new Point(3, 3);
            this.tbErrors.Multiline = true;
            this.tbErrors.Name = "tbErrors";
            this.tbErrors.ReadOnly = true;
            this.tbErrors.Size = new Size(760, 0x174);
            this.tbErrors.TabIndex = 0;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.lblScorecard, this.cmbScorecard, this.toolStripSeparator1, this.lblStrategy, this.btnAdd, this.btnRemove, this.btnUsePV, this.toolStripSeparator2, this.btnActivate, this.btnCancel });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x306, 0x19);
            this.toolbar.TabIndex = 4;
            this.toolbar.Text = "toolStrip1";
            this.toolbar.ItemClicked += new ToolStripItemClickedEventHandler(this.toolbar_ItemClicked);
            this.lblScorecard.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.lblScorecard.Name = "lblScorecard";
            this.lblScorecard.Size = new Size(0x40, 0x16);
            this.lblScorecard.Text = "Scorecard";
            this.cmbScorecard.Name = "cmbScorecard";
            this.cmbScorecard.Size = new Size(0x79, 0x19);
            this.cmbScorecard.Text = "Select a Scorecard";
            this.cmbScorecard.SelectedIndexChanged += new EventHandler(this.cmbScorecard_SelectedIndexChanged);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.lblStrategy.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.lblStrategy.Name = "lblStrategy";
            this.lblStrategy.Size = new Size(0x39, 0x16);
            this.lblStrategy.Text = "Strategy";
            this.btnAdd.Image = (Image) resources.GetObject("btnAdd.Image");
            this.btnAdd.ImageTransparentColor = Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(0x62, 0x16);
            this.btnAdd.Text = "Add Strategies";
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.btnRemove.Enabled = false;
            this.btnRemove.Image = (Image) resources.GetObject("btnRemove.Image");
            this.btnRemove.ImageTransparentColor = Color.Magenta;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new Size(0x6f, 0x16);
            this.btnRemove.Text = "Remove Strategy";
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
            this.btnUsePV.Image = (Image) resources.GetObject("btnUsePV.Image");
            this.btnUsePV.ImageTransparentColor = Color.Magenta;
            this.btnUsePV.Name = "btnUsePV";
            this.btnUsePV.Size = new Size(0x80, 0x16);
            this.btnUsePV.Text = "Use Preferred Values";
            this.btnUsePV.Click += new EventHandler(this.btnUsePV_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.btnActivate.Enabled = false;
            this.btnActivate.Image = (Image) resources.GetObject("btnActivate.Image");
            this.btnActivate.ImageTransparentColor = Color.Magenta;
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new Size(0x35, 0x16);
            this.btnActivate.Text = "Begin";
            this.btnActivate.ToolTipText = "Run the strategies";
            this.btnActivate.Click += new EventHandler(this.btnActivate_Click);
            this.btnCancel.Image = (Image) resources.GetObject("btnCancel.Image");
            this.btnCancel.ImageTransparentColor = Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x3b, 0x16);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.ToolTipText = "Cancel execution of strategies";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.statusStrategies, this.statusDataSet, this.statusScale, this.statusRange, this.statusPositionSize, this.statusMessage });
            this.statusStrip1.Location = new Point(0, 0x1af);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x306, 0x16);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrategies.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusStrategies.Name = "statusStrategies";
            this.statusStrategies.Size = new Size(0x45, 0x11);
            this.statusStrategies.Text = "0 Strategies";
            this.statusDataSet.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusDataSet.Name = "statusDataSet";
            this.statusDataSet.Size = new Size(50, 0x11);
            this.statusDataSet.Text = "DataSet";
            this.statusScale.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusScale.Name = "statusScale";
            this.statusScale.Size = new Size(0x22, 0x11);
            this.statusScale.Text = "Daily";
            this.statusRange.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusRange.Name = "statusRange";
            this.statusRange.Size = new Size(0x2a, 0x11);
            this.statusRange.Text = "Range";
            this.statusPositionSize.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusPositionSize.Name = "statusPositionSize";
            this.statusPositionSize.Size = new Size(0x43, 0x11);
            this.statusPositionSize.Text = "PositionSize";
            this.statusMessage.Name = "statusMessage";
            this.statusMessage.Size = new Size(0, 0x11);
            this.tradingSystemExecutor_0.ApplyCommission = false;
            this.tradingSystemExecutor_0.ApplyDividends = false;
            this.tradingSystemExecutor_0.ApplyInterest = false;
            this.tradingSystemExecutor_0.BarsLoader = null;
            this.tradingSystemExecutor_0.BenchmarkBuyAndHoldON = false;
            this.tradingSystemExecutor_0.BenchmarkSymbol = null;
            this.tradingSystemExecutor_0.BuildEquityCurves = false;
            this.tradingSystemExecutor_0.CashRate = 0.0;
            this.tradingSystemExecutor_0.EnableSlippage = false;
            this.tradingSystemExecutor_0.ExceptionEvents = false;
            this.tradingSystemExecutor_0.FundamentalsLoader = null;
            this.tradingSystemExecutor_0.IsStreaming = false;
            this.tradingSystemExecutor_0.LimitDaySimulation = false;
            this.tradingSystemExecutor_0.LimitOrderSlippage = false;
            this.tradingSystemExecutor_0.MarginRate = 0.0;
            this.tradingSystemExecutor_0.NoDecimalRoundingForLimitStopPrice = false;
            this.tradingSystemExecutor_0.OverrideShareSize = 0.0;
            size.DollarSize = 5000.0;
            size.MarginFactor = 1.0;
            size.Mode = PosSizeMode.RawProfitDollar;
            size.OverrideShareSize = 0.0;
            size.PctSize = 10.0;
            size.PosSizerConfig = "";
            size.RawProfitDollarSize = 5000.0;
            size.RawProfitShareSize = 100.0;
            size.RiskSize = 3.0;
            size.ShareSize = 100.0;
            size.SimuScriptName = "";
            size.StartingCapital = 100000.0;
            this.tradingSystemExecutor_0.PosSize = size;
            this.tradingSystemExecutor_0.PricingDecimalPlaces = 0;
            this.tradingSystemExecutor_0.RedcuceQtyPct = 10.0;
            this.tradingSystemExecutor_0.ReduceQtyBasedOnVolume = false;
            this.tradingSystemExecutor_0.Renderer = null;
            this.tradingSystemExecutor_0.RoundLots = false;
            this.tradingSystemExecutor_0.RoundLots50 = false;
            this.tradingSystemExecutor_0.SlippageTicks = 1;
            this.tradingSystemExecutor_0.SlippageUnits = 1.0;
            this.tradingSystemExecutor_0.StrategyName = "";
            this.tradingSystemExecutor_0.WorstTradeSimulation = false;
            this.tradingSystemExecutor_0.ExternalSymbolFromDataSetRequested += new EventHandler<LoadSymbolFromDataSetEventArgs>(this.method_41);
            this.tradingSystemExecutor_0.ExternalSymbolRequested += new EventHandler<LoadSymbolEventArgs>(this.method_40);
            this.tradingSystemExecutor_0.LookupStrategy += new EventHandler<StrategyEventArgs>(this.method_44);
            this.tradingSystemExecutor_0.SetParameterValues += new EventHandler<StrategyParameterEventArgs>(this.method_42);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x306, 0x1c5);
            base.Controls.Add(this.tabControl1);
            base.Controls.Add(this.toolbar);
            base.Controls.Add(this.statusStrip1);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "StrategyRanking";
            this.Text = "Strategy Ranking";
            base.Load += new EventHandler(this.StrategyRanking_Load);
            base.FormClosed += new FormClosedEventHandler(this.StrategyRanking_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabResults.ResumeLayout(false);
            this.popupStrategies.ResumeLayout(false);
            this.tabErrors.ResumeLayout(false);
            this.tabErrors.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadWorkspaceItems(IList<string> items, int version)
        {
            if (version != 1)
            {
                this.bool_1 = true;
                int num3 = 0;
                this.strategyRankingSettings_0 = new StrategyRankingSettings();
                this.strategyRankingSettings_0.ScorecardName = items[0];///WYJ fix
                this.strategyRankingSettings_0.BarDataScale = WealthLab.BarDataScale.Parse(items[1]);
                this.strategyRankingSettings_0.PosSize = WealthLab.PositionSize.Parse(items[2]);
                this.strategyRankingSettings_0.DataRange = BarDataRange.Parse(items[3]);
                this.strategyRankingSettings_0.Symbol = items[4];
                this.strategyRankingSettings_0.DataSourceName = items[5];
                int num4 = int.Parse(items[6]);
                this.strategyRankingSettings_0.Strategies.Clear();
                List<StrategyRankingItem> list = new List<StrategyRankingItem>();
                for (int i = 0; i < num4; i++)
                {
                    StrategyRankingItem item = new StrategyRankingItem {
                        StrategyID = items[num3++]
                    };
                    if (version >= 3)
                    {
                        item.ParametersNeedSave = bool.Parse(items[num3++]);
                    }
                    string[] strArray = items[num3++].Split(new char[] { ',' });
                    for (int j = 0; j < strArray.Length; j++)
                    {
                        if (strArray[j] != "")
                        {
                            item.ParameterValues.Add(double.Parse(strArray[j]));
                        }
                    }
                    if (version > 3)
                    {
                        item.UsePreferredValues = bool.Parse(items[num3++]);
                    }
                    list.Add(item);
                }
                this.strategyRankingSettings_0.Strategies = list;
                this.method_21();
                this.method_39();
                this.method_43();
                foreach (ListViewItem item2 in this.lvStrategies.Items)
                {
                    StrategyRankingItem tag = (StrategyRankingItem) item2.Tag;
                    WealthLab.Strategy iD = MainModule.Instance.Strategies.LookupID(tag.StrategyID);
                    tag.Strategy = iD;
                    tag.LvItem = item2;
                    tag.WealthScript = MainModule.Instance.Strategies.GetWealthScriptObject(iD);
                }
            }
        }

        private void lvStrategies_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected)
            {
                this.method_18(e.Item);
            }
        }

        private void lvStrategies_Leave(object sender, EventArgs e)
        {
            this.method_17();
        }

        private void lvStrategies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lvStrategies.SelectedItems.Count <= 0)
            {
                this.method_11(false);
            }
            else
            {
                this.method_11(true);
                this.method_17();
            }
            this.MyMainForm.BuildParameterSliders();
            bool flag = false;
            IEnumerator enumerator = this.lvStrategies.SelectedItems.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ListViewItem current = (ListViewItem)enumerator.Current;
                        StrategyRankingItem tag = (StrategyRankingItem)current.Tag;
                        if (tag.UsePreferredValues)
                        {
                            flag = true;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            this.btnUsePV.Checked = flag;
            this.mniPV.Checked = flag;
        }

        private void method_0()
        {
            if (this.cmbScorecard.SelectedIndex != -1)
            {
                using (List<StrategyScorecard>.Enumerator enumerator = this.list_0.GetEnumerator())
                {
                    StrategyScorecard current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (current.FriendlyName == ((string) this.cmbScorecard.SelectedItem))
                        {
                            ///goto  Label_004D;  ///WYJ fix, simplify the flow
                            this.strategyScorecard_0 = current;
                            this.method_15(current);
                            this.method_16();
                            return;
                        }
                    }
                    return;
                }
            }
        }

        private void method_1()
        {
            this.BarDataScale = this.MyMainForm.BarDataScale;
            if (this.positionSize_0 != this.MyMainForm.PositionSize)
            {
                if ((this.positionSize_0 != null) && (this.positionSize_0.RawProfitMode != this.MyMainForm.PositionSize.RawProfitMode))
                {
                    this.bool_3 = true;
                }
                this.PositionSize = this.MyMainForm.PositionSize;
            }
            this.DataRange = MainModule.Instance.DataRange;
            this.dataSource_0 = this.MyMainForm.DataSource;
            this.string_1 = this.MyMainForm.Symbol;
        }

        private void method_10()
        {
            while (this.lvStrategies.SelectedItems.Count > 0)
            {
                this.lvStrategies.Items.Remove(this.lvStrategies.SelectedItems[0]);
            }
            this.method_13();
            this.method_11(false);
        }

        private void method_11(bool bool_4)
        {
            this.btnRemove.Enabled = bool_4;
            this.mniRemoveStrategy.Enabled = bool_4;
        }

        private void method_12()
        {
            this.method_13();
            this.method_14();
        }

        private void method_13()
        {
            this.statusStrategies.Text = this.lvStrategies.Items.Count + " Strategies";
        }

        private void method_14()
        {
            if ((this.string_1 == "") && (this.dataSource_0 != null))
            {
                this.statusDataSet.Text = this.dataSource_0.Name;
            }
            else
            {
                this.statusDataSet.Text = this.string_1;
            }
            this.statusPositionSize.Text = this.positionSize_0.Text;
            this.statusRange.Text = this.barDataRange_0.Text;
            this.statusScale.Text = this.barScale_0.ToString();
        }

        private void method_15(StrategyScorecard strategyScorecard_1)
        {
            strategyScorecard_1.SetupListViewColumns(this.lvStrategies, this.MyMainForm.PositionSize, 1);
            this.method_30("");
        }

        private bool method_16()
        {
            this.btnActivate.Enabled = (this.cmbScorecard.SelectedIndex != -1) && (this.lvStrategies.Items.Count > 0);
            return this.btnActivate.Enabled;
        }

        private void method_17()
        {
            for (int i = 0; i < this.lvStrategies.SelectedItems.Count; i++)
            {
                this.method_18(this.lvStrategies.SelectedItems[i]);
            }
        }

        private void method_18(ListViewItem listViewItem_0)
        {
            int num;
            string str = this.method_5(listViewItem_0) + " " + this.method_3(listViewItem_0);
            string[] strArray = listViewItem_0.Text.Split(new char[] { '-' });
            if (int.TryParse(strArray[strArray.Length - 1], out num))
            {
                str = str + "-" + num.ToString();
            }
            if (str != listViewItem_0.Text)
            {
                listViewItem_0.Text = str;
            }
            StrategyRankingItem tag = (StrategyRankingItem) listViewItem_0.Tag;
            if (tag.UsePreferredValues)
            {
                listViewItem_0.StateImageIndex = 2;
            }
            else
            {
                listViewItem_0.StateImageIndex = -1;
            }
        }

        private void method_19()
        {
            if (!this.bool_1)
            {
                this.method_36();
                SettingsManager settings = MainModule.Instance.Settings;
                settings.Set(this, "StrategyRankings");
                settings.Set("StrategyRankings.ScorecardName", this.strategyRankingSettings_0.ScorecardName);
                settings.Set("StrategyRankings.BarDataScale", this.strategyRankingSettings_0.BarDataScale.ToString());
                settings.Set("StrategyRankings.DataRange", this.strategyRankingSettings_0.DataRange.ToString());
                settings.Set("StrategyRankings.PositionSizing", this.strategyRankingSettings_0.PosSize.ToString());
                settings.Set("StrategyRankings.Symbol", this.strategyRankingSettings_0.Symbol);
                settings.Set("StrategyRankings.DataSet", this.strategyRankingSettings_0.DataSourceName);
            }
        }

        private WealthLab.WealthScript method_2(ListViewItem listViewItem_0)
        {
            StrategyRankingItem tag = (StrategyRankingItem) listViewItem_0.Tag;
            return tag.WealthScript;
        }

        private bool method_20()
        {
            this.lvStrategies.Items.Clear();
            this.method_1();
            SettingsManager settings = MainModule.Instance.Settings;
            settings.Get(this, "StrategyRankings");
            this.strategyRankingSettings_0.ScorecardName = settings.Get("StrategyRankings.ScorecardName", this.cmbScorecard.Text);
            this.strategyRankingSettings_0.BarDataScale = WealthLab.BarDataScale.Parse(settings.Get("StrategyRankings.BarDataScale", this.BarDataScale.ToString()));
            this.strategyRankingSettings_0.DataRange = BarDataRange.Parse(settings.Get("StrategyRankings.DataRange", this.DataRange.ToString()));
            this.strategyRankingSettings_0.PosSize = WealthLab.PositionSize.Parse(settings.Get("StrategyRankings.PositionSizing", this.PositionSize.ToString()));
            this.strategyRankingSettings_0.Symbol = settings.Get("StrategyRankings.Symbol", this.Symbol);
            this.strategyRankingSettings_0.DataSourceName = settings.Get("StrategyRankings.DataSet", (this.DataSet != null) ? this.DataSet.Name : "");
            return true;
        }

        private void method_21()
        {
            this.DataRange = this.strategyRankingSettings_0.DataRange;
            this.PositionSize = this.strategyRankingSettings_0.PosSize;
            this.BarDataScale = this.strategyRankingSettings_0.BarDataScale;
            this.dataSource_0 = MainModule.Instance.DataSources.FindDataSource(this.strategyRankingSettings_0.DataSourceName);
            this.string_1 = this.strategyRankingSettings_0.Symbol;
            for (int i = 0; i < this.cmbScorecard.Items.Count; i++)
            {
                if (this.strategyRankingSettings_0.ScorecardName == ((string) this.cmbScorecard.Items[i]))
                {
                    this.cmbScorecard.SelectedIndex = i;
                    break;
                }
            }
            this.method_22();
        }

        private void method_22()
        {
            if (this.strategyRankingSettings_0 != null)
            {
                this.Cursor = Cursors.WaitCursor;
                this.lvStrategies.Items.Clear();
                foreach (StrategyRankingItem item in this.strategyRankingSettings_0.Strategies)
                {
                    ListViewItem item2;
                    item.Strategy = MainModule.Instance.Strategies.LookupID(item.StrategyID);
                    this.statusMessage.Text = "Loading " + item.Strategy.Name;
                    if (item.Strategy != null)
                    {
                        item.WealthScript = MainModule.Instance.Strategies.GetWealthScriptObject(item.Strategy);
                        if ((item.WealthScript != null) && (item.WealthScript.Parameters.Count == item.ParameterValues.Count))
                        {
                            for (int i = 0; i < item.ParameterValues.Count; i++)
                            {
                                item.WealthScript.Parameters[i].Value = item.ParameterValues[i];
                            }
                        }
                    }
                    if (item.Strategy == null)
                    {
                        item2 = this.lvStrategies.Items.Add("Error: Strategy not found");
                    }
                    else if ((item.StrategyName != "") && (item.StrategyName != null))
                    {
                        item2 = this.lvStrategies.Items.Add(item.StrategyName);
                    }
                    else if (item.WealthScript == null)
                    {
                        item2 = this.lvStrategies.Items.Add(item.Strategy.Name);
                    }
                    else
                    {
                        item2 = this.lvStrategies.Items.Add(item.Strategy.Name + " " + item.WealthScript.ParameterString);
                    }
                    item2.Tag = item;
                    item2.ImageIndex = -1;
                    item.LvItem = item2;
                    Application.DoEvents();
                }
                this.statusMessage.Text = "";
                this.Cursor = Cursors.Default;
            }
        }

        private void method_23(object object_2)
        {
            try
            {
                List<StrategyRankingItem> list = (List<StrategyRankingItem>) object_2;
                this.method_29("Updating data for: " + (this.MultiSymbolMode ? this.dataSource_0.Name : this.string_1));
                BarsLoader loader = new BarsLoader();
                if (this.method_38(ref loader))
                {
                    this.tradingSystemExecutor_0.ApplySettings(MainModule.Instance.Executor);
                    this.tradingSystemExecutor_0.PosSize = this.strategyRankingSettings_0.PosSize;
                    this.tradingSystemExecutor_0.BarsLoader = loader;
                    this.tradingSystemExecutor_0.BuildEquityCurves = true;
                    this.tradingSystemExecutor_0.FundamentalsLoader = this.fundamentalsLoader_0;
                    this.fundamentalsLoader_0.DataHost = this.dataSourceManager_0;
                    this.tradingSystemExecutor_0.IsStreaming = this.strategyRankingSettings_0.DataRange.IsStreaming;
                    this.tradingSystemExecutor_0.DataSet = this.strategyRankingSettings_0.DataSet;
                    foreach (StrategyRankingItem item in list)
                    {
                        if (this.bool_0)
                        {
                            return;
                        }
                        this.tradingSystemExecutor_0.StrategyName = item.Strategy.Name;
                        this.method_26(item, -1);
                        this.method_29("Running Strategy: " + item.Strategy.Name);
                        this.method_25(item);
                        if (this.exception_0 == null)
                        {
                            this.method_26(item, 0);
                        }
                        else
                        {
                            this.method_26(item, 1);
                            this.method_31(this.exception_0);
                            this.exception_0 = null;
                        }
                        base.Invoke(new Delegate52(this.method_24), new object[] { item.LvItem, this.tradingSystemExecutor_0.Performance });
                    }
                }
            }
            catch (Exception exception)
            {
                this.exception_0 = exception;
            }
            finally
            {
                this.method_33();
            }
        }

        private void method_24(ListViewItem listViewItem_0, SystemPerformance systemPerformance_0)
        {
            for (int i = listViewItem_0.SubItems.Count - 1; i > 0; i--)
            {
                listViewItem_0.SubItems.RemoveAt(i);
            }
            this.strategyScorecard_0.PopulateScorecard(listViewItem_0, systemPerformance_0);
        }

        private void method_25(StrategyRankingItem strategyRankingItem_1)
        {
            lock (this.object_1)
            {
                this.exception_0 = null;
                try
                {
                    this.strategyRankingItem_0 = strategyRankingItem_1;
                    if (this.MultiSymbolMode)
                    {
                        this.tradingSystemExecutor_0.Execute(strategyRankingItem_1.Strategy, strategyRankingItem_1.WealthScript, null, this.list_1);
                    }
                    else
                    {
                        this.tradingSystemExecutor_0.Execute(strategyRankingItem_1.Strategy, strategyRankingItem_1.WealthScript, this.list_1[0]);
                    }
                }
                catch (Exception exception)
                {
                    this.exception_0 = exception;
                }
            }
        }

        private void method_26(StrategyRankingItem strategyRankingItem_1, int int_1)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Delegate53(this.method_27), new object[] { strategyRankingItem_1, int_1 });
            }
            else
            {
                this.method_27(strategyRankingItem_1, int_1);
            }
        }

        private void method_27(StrategyRankingItem strategyRankingItem_1, int int_1)
        {
            ListViewItem item = this.method_28(strategyRankingItem_1.LvItem);
            if (item != null)
            {
                if (int_1 == 10)
                {
                    for (int i = 1; i < strategyRankingItem_1.LvItem.SubItems.Count; i++)
                    {
                        item.SubItems.Add(strategyRankingItem_1.LvItem.SubItems[i]);
                    }
                }
                else
                {
                    item.ImageIndex = int_1;
                }
                Application.DoEvents();
            }
        }

        private ListViewItem method_28(ListViewItem listViewItem_0)
        {
            return listViewItem_0;
        }

        private void method_29(string string_2)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Delegate54(this.method_30), new object[] { string_2 });
            }
            else
            {
                this.method_30(string_2);
            }
        }

        private string method_3(ListViewItem listViewItem_0)
        {
            StrategyRankingItem tag = (StrategyRankingItem) listViewItem_0.Tag;
            if (tag.WealthScript != null)
            {
                return tag.WealthScript.ParameterString;
            }
            return "";
        }

        private void method_30(string string_2)
        {
            this.statusMessage.Text = string_2;
            Application.DoEvents();
        }

        private void method_31(Exception exception_1)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Delegate55(this.method_32), new object[] { exception_1 });
            }
            else
            {
                this.method_32(exception_1);
            }
        }

        private void method_32(Exception exception_1)
        {
            if (this.tbErrors.Text != "")
            {
                this.tbErrors.Text = this.tbErrors.Text + Environment.NewLine;
            }
            this.tbErrors.Text = this.tbErrors.Text + this.tradingSystemExecutor_0.StrategyName + ": " + exception_1.Message;
            ilog_0.Error(string.Concat(new object[] { "Strategy ", this.tradingSystemExecutor_0.StrategyName, " Error: ", exception_1 }));
            Application.DoEvents();
        }

        private void method_33()
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Delegate56(this.method_34));
            }
            else
            {
                this.method_34();
            }
        }

        private void method_34()
        {
            this.method_29(this.bool_0 ? "Strategy Ranking canceled" : "");
            this.bool_0 = false;
            this.method_35();
            this.method_17();
            this.method_37(true);
            this.method_43();
        }

        private void method_35()
        {
            this.BarDataScale = this.strategyRankingSettings_0.BarDataScale;
            this.DataRange = this.strategyRankingSettings_0.DataRange;
            this.PositionSize = this.strategyRankingSettings_0.PosSize;
            this.dataSource_0 = MainModule.Instance.DataSources.FindDataSource(this.strategyRankingSettings_0.DataSourceName);
            this.string_1 = this.strategyRankingSettings_0.Symbol;
            this.MyMainForm.ActivateMdiChild();
        }

        private void method_36()
        {
            if (this.strategyRankingSettings_0 == null)
            {
                this.strategyRankingSettings_0 = new StrategyRankingSettings();
            }
            this.strategyRankingSettings_0.BarDataScale = this.BarDataScale;
            this.strategyRankingSettings_0.DataRange = this.barDataRange_0;
            this.strategyRankingSettings_0.DataSourceName = (this.dataSource_0 != null) ? this.dataSource_0.Name : "";
            this.strategyRankingSettings_0.DataSet = this.dataSource_0;
            this.strategyRankingSettings_0.PosSize = this.positionSize_0;
            this.strategyRankingSettings_0.ScorecardName = (this.strategyScorecard_0 != null) ? this.strategyScorecard_0.FriendlyName : "";
            this.strategyRankingSettings_0.Symbol = this.string_1;
            this.strategyRankingSettings_0.Strategies.Clear();
            List<StrategyRankingItem> list = new List<StrategyRankingItem>();
            foreach (ListViewItem item2 in this.lvStrategies.Items)
            {
                StrategyRankingItem tag = (StrategyRankingItem) item2.Tag;
                if (tag.Strategy != null)
                {
                    tag.StrategyID = tag.Strategy.ID.ToString();
                }
                tag.StrategyName = item2.Text;
                tag.ParameterValues.Clear();
                if (tag.WealthScript != null)
                {
                    foreach (StrategyParameter parameter in tag.WealthScript.Parameters)
                    {
                        tag.ParameterValues.Add(parameter.Value);
                    }
                }
                list.Add(tag);
            }
            this.strategyRankingSettings_0.Strategies = list;
        }

        private void method_37(bool bool_4)
        {
            this.btnActivate.Visible = bool_4;
            this.btnCancel.Visible = !bool_4;
            this.lvStrategies.Enabled = bool_4;
            this.cmbScorecard.Enabled = bool_4;
            this.btnAdd.Enabled = bool_4;
            Application.DoEvents();
        }

        private bool method_38(ref BarsLoader barsLoader_0)
        {
            barsLoader_0.DataHost = this.dataSourceManager_0;
            barsLoader_0.BarDataScale = this.BarDataScale;
            this.barDataRange_0.ConfigureBarsLoader(barsLoader_0);
            this.list_1.Clear();
            if (this.MultiSymbolMode)
            {
                foreach (string str in this.dataSource_0.Symbols)
                {
                    Bars data = barsLoader_0.GetData(this.dataSource_0, str);
                    this.list_1.Add(data);
                }
            }
            else
            {
                bool flag;
                try
                {
                    Bars item = barsLoader_0.GetData(this.dataSource_0, this.string_1);
                    this.list_1.Add(item);
                    ///goto  Label_00C0;  ///WYJ fix, simplify the flow
                    return true;
                }
                catch
                {
                    MessageBox.Show("Please select a symbol or Dataset");
                    flag = false;
                }
                return flag;
            }
            return true;
        }

        private void method_39()
        {
            this.method_12();
            this.method_16();
        }

        private WealthLab.Strategy method_4(ListViewItem listViewItem_0)
        {
            StrategyRankingItem tag = (StrategyRankingItem) listViewItem_0.Tag;
            return tag.Strategy;
        }

        private void method_40(object sender, LoadSymbolEventArgs e)
        {
            e.SymbolData = MainModule.Instance.LoadExternalSymbol(e.Symbol, e.Scale, e.BarInterval, false);
        }

        private void method_41(object sender, LoadSymbolFromDataSetEventArgs e)
        {
            e.Bars = MainModule.Instance.LoadExternalSymbol(e.DataSetName, e.Symbol);
        }

        private void method_42(object sender, StrategyParameterEventArgs e)
        {
            if (this.strategyRankingItem_0.UsePreferredValues)
            {
                this.strategyRankingItem_0.Strategy.LoadPreferredValues(e.Symbol, e.WealthScript);
            }
        }

        private void method_43()
        {
            foreach (ListViewItem item in this.lvStrategies.Items)
            {
                item.Selected = true;
            }
            this.lvStrategies.SelectedItems.Clear();
        }

        private void method_44(object sender, StrategyEventArgs e)
        {
            e.Strategy = MainModule.Instance.Strategies.LookupID(e.StrategyID);
        }

        private string method_5(ListViewItem listViewItem_0)
        {
            StrategyRankingItem tag = (StrategyRankingItem) listViewItem_0.Tag;
            return tag.Strategy.Name;
        }

        private void method_6(ListViewItem listViewItem_0, bool bool_4)
        {
            StrategyRankingItem tag = (StrategyRankingItem) listViewItem_0.Tag;
            tag.ParametersNeedSave = bool_4;
        }

        private bool method_7(ListViewItem listViewItem_0)
        {
            StrategyRankingItem tag = (StrategyRankingItem) listViewItem_0.Tag;
            return tag.ParametersNeedSave;
        }

        private bool method_8()
        {
            StrategyExplorerForm form = new StrategyExplorerForm {
                ToolBarVisible = false,
                MultiSelect = true,
                Text = "Select Strategies to Add"
            };
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    foreach (WealthLab.Strategy strategy in form.StrategiesSelected)
                    {
                        this.statusMessage.Text = "Loading " + strategy.Name;
                        this.method_9(strategy);
                        Application.DoEvents();
                    }
                    break;

                case DialogResult.Cancel:
                    return false;
            }
            this.statusMessage.Text = "";
            return true;
        }

        private void method_9(WealthLab.Strategy strategy_0)
        {
            WealthLab.WealthScript wealthScriptObject = MainModule.Instance.Strategies.GetWealthScriptObject(strategy_0);
            if (wealthScriptObject == null)
            {
                MessageBox.Show("Unable to add strategy " + strategy_0.Name + ". Please check strategy for compilation errors and try again.");
            }
            else
            {
                StrategyRankingItem item = new StrategyRankingItem(strategy_0, wealthScriptObject);
                if (!item.Strategy.RestoreSavedParameterValues(item.WealthScript))
                {
                    item.WealthScript.RestoreParameterDefaults();
                }
                int num2 = 0;
                for (int i = 0; i < this.lvStrategies.Items.Count; i++)
                {
                    if (this.lvStrategies.Items[i].Text.Contains(strategy_0.Name))
                    {
                        num2++;
                    }
                }
                ListViewItem item2 = this.lvStrategies.Items.Add(strategy_0.Name + " " + wealthScriptObject.ParameterString + ((num2 > 0) ? ("-" + num2.ToString()) : ""));
                item2.Tag = item;
                item2.ImageIndex = -1;
                item.LvItem = item2;
            }
        }

        private void mniCopyResults_Click(object sender, EventArgs e)
        {
            this.CopyToClipboard();
        }

        private void mniOpen_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvStrategies.SelectedItems)
            {
                WealthLab.Strategy strategy = this.method_4(item);
                WealthLab.WealthScript script = this.method_2(item);
                ChartForm form = this.MyMainForm.FindStrategyFormByTag(strategy, this);
                if (form == null)
                {
                    form = this.MyMainForm.OpenStrategyWindow(strategy, false, false);
                    form.Tag = this;
                }
                form.BringToFront();
                form.WindowState = FormWindowState.Normal;
                form.DataSource = this.dataSource_0;
                form.Symbol = this.string_1;
                form.DataRange = this.barDataRange_0;
                form.PositionSize = this.positionSize_0;
                form.BarDataScale = this.BarDataScale;
                form.SetBarDataScaleForDataSource(this.dataSource_0, this.BarDataScale);
                WealthLab.WealthScript wealthScript = form.WealthScript;
                if ((wealthScript != null) && (script != null))
                {
                    for (int i = 0; i < wealthScript.Parameters.Count; i++)
                    {
                        wealthScript.Parameters[i].Value = script.Parameters[i].Value;
                    }
                }
                this.MyMainForm.ActivateMdiChild();
                this.MyMainForm.SelectTreeNode(this.dataSource_0, this.string_1);
                form.ResetStreaming();
                form.GoButtonPressed(this.string_1, true);
            }
        }

        private void mniPrintResults_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private void popupStrategies_Opening(object sender, CancelEventArgs e)
        {
            if (this.lvStrategies.SelectedItems.Count > 0)
            {
                this.mniOpen.Enabled = true;
            }
            else
            {
                this.mniOpen.Enabled = false;
            }
        }

        public void Print()
        {
            DataObject doPrint = new DataObject();
            doPrint.SetData(PrintReport.fmtTitle.Name, "Strategy Rankings");
            doPrint.SetData(PrintReport.fmtListView.Name, this.lvStrategies);
            doPrint.SetData(PrintReport.fmtBaseTitle.Name, MainModule.Instance.AuthProvider.ApplicationName);
            string data = "";
            if (this.strategyRankingSettings_0 != null)
            {
                if (this.MultiSymbolMode)
                {
                    doPrint.SetData(PrintReport.fmtSymbol.Name, this.strategyRankingSettings_0.DataSourceName);
                }
                else
                {
                    doPrint.SetData(PrintReport.fmtSymbol.Name, this.strategyRankingSettings_0.Symbol);
                }
                if (this.strategyRankingSettings_0.PosSize.RawProfitMode)
                {
                    data = data + "Raw Profit Mode | ";
                }
                else
                {
                    data = data + "Portfolio Simulation | Starting Capital: " + this.strategyRankingSettings_0.PosSize.StartingCapital.ToString("C") + " | ";
                }
                string str4 = data;
                data = str4 + "Scale: " + this.strategyRankingSettings_0.BarDataScale.ToString() + " | Data Range: " + this.strategyRankingSettings_0.DataRange.Text + " | Position Sizing: " + this.strategyRankingSettings_0.PosSize.Text;
            }
            else
            {
                if (this.MultiSymbolMode)
                {
                    doPrint.SetData(PrintReport.fmtSymbol.Name, this.dataSource_0.Name);
                }
                else
                {
                    doPrint.SetData(PrintReport.fmtSymbol.Name, this.string_1);
                }
                if (this.positionSize_0.RawProfitMode)
                {
                    data = data + "Raw Profit Mode | ";
                }
                else
                {
                    data = data + "Portfolio Simulation | Starting Capital: " + this.positionSize_0.StartingCapital.ToString("C") + " | ";
                }
                string str2 = data;
                data = str2 + "Scale: " + this.BarDataScale.ToString() + " | Data Range: " + this.barDataRange_0.Text + " | Position Sizing: " + this.positionSize_0.Text;
            }
            if (!this.MultiSymbolMode && (this.list_1.Count > 0))
            {
                Bars bars = this.list_1[0];
                data = data + " | Bars: " + bars.Count;
                int num = 0;
                string str3 = "";
                num = bars.Count - 1;
                str3 = bars.Date[num].ToShortDateString();
                if (bars.IsIntraday)
                {
                    str3 = str3 + " " + bars.Date[num].ToShortTimeString();
                }
                data = data + " | Last Bar date: " + str3;
            }
            doPrint.SetData(PrintReport.fmtDetails.Name, data);
            new PrintReport(doPrint, true) { ShowPrintPreview = !MainModule.Instance.Settings.Get("HidePrintPreview", false), ShowPrintDialog = !MainModule.Instance.Settings.Get("HidePrintDialog", false) }.PrintGraphicReport(this.MyMainForm.DefaultPageSettings);
        }

        public bool SaveStrategy()
        {
            return true;
        }

        public int SaveWorkspaceItems(IList<string> items)
        {
            this.bool_1 = true;
            this.method_36();
            items.Add(this.strategyRankingSettings_0.ScorecardName);
            items.Add(this.strategyRankingSettings_0.BarDataScale.ToString());
            items.Add(this.strategyRankingSettings_0.PosSize.ToString());
            items.Add(this.strategyRankingSettings_0.DataRange.ToString());
            items.Add(this.strategyRankingSettings_0.Symbol);
            items.Add(this.strategyRankingSettings_0.DataSourceName);
            items.Add(this.strategyRankingSettings_0.Strategies.Count.ToString());
            foreach (StrategyRankingItem item in this.strategyRankingSettings_0.Strategies)
            {
                items.Add(item.StrategyID);
                items.Add(item.ParametersNeedSave.ToString());
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < item.ParameterValues.Count; i++)
                {
                    if (i > 0)
                    {
                        builder.Append(",");
                    }
                    builder.Append(item.ParameterValues[i].ToString());
                }
                items.Add(builder.ToString());
                items.Add(item.UsePreferredValues.ToString());
            }
            return 4;
        }

        private void StrategyRanking_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.method_19();
        }

        private void StrategyRanking_Load(object sender, EventArgs e)
        {
            AssemblyLoader loader = new AssemblyLoader {
                BaseClass = "StrategyScorecard",
                Path = MainModule.Instance.AppPath
            };
            foreach (System.Type type in loader.Types)
            {
                StrategyScorecard item = (StrategyScorecard) loader.CreateInstance(type);
                this.cmbScorecard.Items.Add(item.FriendlyName);
                this.list_0.Add(item);
            }
            if (!this.method_20())
            {
                this.method_1();
            }
            else
            {
                this.method_21();
            }
            this.method_39();
        }

        private void toolbar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.method_17();
        }

        public void UpdateDataSource(DataSource dataSource_1, string string_2)
        {
            if (!this.IsBusy && !this.bool_2)
            {
                if (dataSource_1 != null)
                {
                    this.dataSource_0 = dataSource_1;
                }
                this.string_1 = string_2;
                this.BarDataScale = dataSource_1.BarDataScale;
            }
        }

        public WealthLab.BarDataScale BarDataScale
        {
            get
            {
                return new WealthLab.BarDataScale(this.BarScale, this.BarInterval);
            }
            set
            {
                if (!this.IsBusy)
                {
                    this.BarScale = value.Scale;
                    this.BarInterval = value.BarInterval;
                }
            }
        }

        public int BarInterval
        {
            get
            {
                return this.int_0;
            }
            set
            {
                if (!this.IsBusy)
                {
                    this.int_0 = value;
                }
            }
        }

        public WealthLab.BarScale BarScale
        {
            get
            {
                return this.barScale_0;
            }
            set
            {
                if (!this.IsBusy)
                {
                    this.barScale_0 = value;
                }
            }
        }

        public DataSourceManager DataHost
        {
            get
            {
                return this.dataSourceManager_0;
            }
        }

        public BarDataRange DataRange
        {
            get
            {
                return this.barDataRange_0;
            }
            set
            {
                if (!this.IsBusy)
                {
                    string str = value.ToString();
                    this.barDataRange_0 = BarDataRange.Parse(str);
                }
            }
        }

        public DataSource DataSet
        {
            get
            {
                return this.dataSource_0;
            }
            set
            {
                if (!this.IsBusy)
                {
                    this.dataSource_0 = value;
                }
            }
        }

        public bool IsBusy
        {
            get
            {
                return ((this.thread_0 != null) && this.thread_0.IsAlive);
            }
        }

        public bool MultiSymbolMode
        {
            get
            {
                if (this.strategyRankingSettings_0 != null)
                {
                    return ((this.strategyRankingSettings_0.Symbol == "") && ((this.strategyRankingSettings_0.DataSourceName != null) && (this.strategyRankingSettings_0.DataSourceName != "")));
                }
                if (this.string_1 != "")
                {
                    return false;
                }
                return (this.dataSource_0 != null);
            }
        }

        public MainForm MyMainForm
        {
            get
            {
                return (base.MdiParent as MainForm);
            }
        }

        public bool ParametersNeedSave
        {
            get
            {
                return ((this.lvStrategies.SelectedItems.Count == 1) && this.method_7(this.lvStrategies.SelectedItems[0]));
            }
            set
            {
                if (this.lvStrategies.SelectedItems.Count == 1)
                {
                    this.method_6(this.lvStrategies.SelectedItems[0], value);
                }
            }
        }

        public WealthLab.PositionSize PositionSize
        {
            get
            {
                return this.positionSize_0;
            }
            set
            {
                if (!this.IsBusy)
                {
                    if ((this.positionSize_0 != null) && (this.positionSize_0.RawProfitMode != value.RawProfitMode))
                    {
                        this.bool_3 = true;
                    }
                    string str = value.ToString();
                    this.positionSize_0 = WealthLab.PositionSize.Parse(str);
                }
            }
        }

        public WealthLab.Strategy Strategy
        {
            get
            {
                if (this.lvStrategies.SelectedItems.Count == 1)
                {
                    return this.method_4(this.lvStrategies.SelectedItems[0]);
                }
                return null;
            }
            set
            {
            }
        }

        public string Symbol
        {
            get
            {
                return this.string_1;
            }
            set
            {
                if (!this.IsBusy)
                {
                    this.string_1 = value;
                }
            }
        }

        public bool SymbolsSelected
        {
            get
            {
                return ((this.string_1 != "") || (this.dataSource_0 != null));
            }
        }

        public WealthLab.WealthScript WealthScript
        {
            get
            {
                if (this.lvStrategies.SelectedItems.Count == 1)
                {
                    return this.method_2(this.lvStrategies.SelectedItems[0]);
                }
                return null;
            }
            set
            {
            }
        }

        private delegate void Delegate52(ListViewItem listViewItem_0, SystemPerformance systemPerformance_0);

        private delegate void Delegate53(StrategyRankingItem strategyRankingItem_0, int int_0);

        private delegate void Delegate54(string string_0);

        private delegate void Delegate55(Exception exception_0);

        private delegate void Delegate56();
    }
}

