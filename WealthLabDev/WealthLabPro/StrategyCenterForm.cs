namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using WealthLab;

    public class StrategyCenterForm : Form, IComparer<Bars>, IWorkspace
    {
        private BarsLoader barsLoader_0;
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private ToolStripButton btnActivate;
        private ToolStripButton btnAdd;
        private ToolStripButton btnAutoStage;
        private ToolStripButton btnEmailAlert;
        private ToolStripButton btnHelp;
        private ToolStripButton btnHelpAlerts;
        private ToolStripButton btnPlace;
        private ToolStripButton btnRemove;
        private ToolStripButton btnSelectAll;
        private ToolStripButton btnSendToQuote;
        private ToolStripButton btnSettings;
        private ToolStripButton btnShowAllAlerts;
        private ToolStripButton btnShowLocalTime;
        private ToolStripButton btnStage;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_10;
        private ColumnHeader columnHeader_11;
        private ColumnHeader columnHeader_12;
        private ColumnHeader columnHeader_13;
        private ColumnHeader columnHeader_14;
        private ColumnHeader columnHeader_15;
        private ColumnHeader columnHeader_16;
        private ColumnHeader columnHeader_17;
        private ColumnHeader columnHeader_18;
        private ColumnHeader columnHeader_19;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_20;
        private ColumnHeader columnHeader_21;
        private ColumnHeader columnHeader_22;
        private ColumnHeader columnHeader_23;
        private ColumnHeader columnHeader_24;
        private ColumnHeader columnHeader_25;
        private ColumnHeader columnHeader_26;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private ColumnHeader columnHeader_5;
        private ColumnHeader columnHeader_6;
        private ColumnHeader columnHeader_7;
        private ColumnHeader columnHeader_8;
        private ColumnHeader columnHeader_9;
        private FundamentalsLoader fundamentalsLoader_0;
        private IContainer components;
        private ImageList imageList_0;
        private ImageList imageList_1;
        private ImageList imageList_2;
        public static StrategyCenterForm Instance;
        private const int int_0 = 0;
        private const int int_1 = 1;
        private const int int_2 = 0;
        private int int_3 = 13;
        private ToolStripLabel lblAlerts;
        private ToolStripLabel lblStrategies;
        private List<StrategyCenterExecutionItem> list_0 = new List<StrategyCenterExecutionItem>();
        private ListView listView_0;
        private SortableListView lvAlerts;
        private SortableListView lvStrategies;
        private MarketHours marketHours_0;
        private ToolStripMenuItem mniActivate;
        private ToolStripMenuItem mniAddStrategy;
        private ToolStripMenuItem mniChangeSettings;
        private ToolStripMenuItem mniCopyAlerts;
        private ToolStripMenuItem mniCopyStrategies;
        private ToolStripMenuItem mniDisableLogging;
        private ToolStripMenuItem mniEditAlert;
        private ToolStripMenuItem mniEmailAlerts;
        private ToolStripMenuItem mniGenerateOrders;
        private ToolStripMenuItem mniItemLogging;
        private ToolStripMenuItem mniOpen;
        private ToolStripMenuItem mniPlace;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrintAlerts;
        private ToolStripMenuItem mniRemoveStrategy;
        private ToolStripMenuItem mniRunNow;
        private ToolStripMenuItem mniSelectAll;
        private ToolStripMenuItem mniSendToQuote;
        private ToolStripMenuItem mniShowAllAlerts;
        private ToolStripMenuItem mniShowLocalTime;
        private ToolStripMenuItem mniStage;
        private ToolStripMenuItem mniViewItemLog;
        private bool? nullable_0 = null;
        private object object_0 = new object();
        private ContextMenuStrip popupAlerts;
        private ContextMenuStrip popupStrategies;
        private PrintPreview printPreview_0;
        private PrintReport printReport_0;
        private ToolStripSeparator sepHelp;
        private ToolStripSeparator sepMenu;
        private ToolStripSeparator sepMenu2;
        private ToolStripSeparator sepMenu3;
        private ToolStripSeparator sepPrint;
        private ToolStripSeparator sepQuote;
        private ToolStripSeparator sepQuotes;
        private ToolStripSeparator sepSelectAll;
        private ToolStripSeparator sepStage;
        private SplitContainer splitMain;
        private StatusStrip status;
        private ToolStripStatusLabel statusActive;
        private ToolStripStatusLabel statusAlerts;
        private ToolStripStatusLabel statusStrategies;
        private ToolStripStatusLabel statusSymbols;
        private string string_0;
        private System.Windows.Forms.Timer timer_0;
        private ToolStrip toolbar;
        private ToolStrip toolbarAlerts;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private TradingSystemExecutor tradingSystemExecutor_0;

        public StrategyCenterForm()
        {
            this.InitializeComponent();
            this.string_0 = MainModule.Instance.DataPath + @"\StrategyMonitorConfiguration.xml";
            this.method_22();
        }

        public StrategyCenterItem AddStrategy(Strategy strategy_0)
        {
            ListViewItem item = this.lvStrategies.Items.Add(strategy_0.Name);
            item.ImageIndex = -1;
            item.SubItems.Add(MainModule.Instance.DefaultAccountNumber);
            for (int i = 2; i < this.lvStrategies.Columns.Count; i++)
            {
                item.SubItems.Add("");
            }
            WealthScript wealthScriptObject = MainModule.Instance.Strategies.GetWealthScriptObject(strategy_0);
            StrategyCenterItem item2 = new StrategyCenterItem(item, this) {
                Strategy = strategy_0,
                WealthScript = wealthScriptObject,
                DataRange = new BarDataRange()
            };
            item2.DataRange.Range = BarRange.FixedBars;
            item2.DataRange.FixedBars = 0x3e8;
            if (strategy_0.AccountNumber == "")
            {
                item2.AccountNumber = MainModule.Instance.DefaultAccountNumber;
            }
            else
            {
                item2.AccountNumber = strategy_0.AccountNumber;
            }
            item2.AccountTradeType = MainModule.Instance.DefaultAccountTradeType(item2.AccountNumber);
            item.Tag = item2;
            this.method_0();
            item.Selected = true;
            return item2;
        }

        public void AddStrategyToStrategyCenter(Strategy strategy_0)
        {
            StrategyCenterItem item = this.AddStrategy(strategy_0);
            if (this.MyMainForm.Symbol != "")
            {
                item.TargetDataType = StrategyCenterItemDataType.Symbol;
                item.DataSet = this.MyMainForm.DataSource;
                item.DataSourceName = this.MyMainForm.DataSource.Name;
                item.Symbol = this.MyMainForm.Symbol;
            }
            else if (this.MyMainForm.DataSource != null)
            {
                item.TargetDataType = StrategyCenterItemDataType.DataSet;
                item.DataSet = this.MyMainForm.DataSource;
                item.DataSourceName = this.MyMainForm.DataSource.Name;
                item.Symbol = "";
            }
            else
            {
                item.TargetDataType = StrategyCenterItemDataType.None;
            }
            item.Scale = this.MyMainForm.BarDataScale.Scale;
            item.BarInterval = this.MyMainForm.BarDataScale.BarInterval;
            item.PositionSize = this.MyMainForm.PositionSize;
            if ((MainModule.Instance.DataRange.Range != BarRange.AllData) || !this.MyMainForm.BarDataScale.IsIntraday)
            {
                item.DataRange = MainModule.Instance.DataRange;
            }
            if (!item.PositionSize.RawProfitMode)
            {
                ShowPosSizeWarning();
            }
            item.method_0(this.marketHours_0);
            if (item.DataSet != null)
            {
                this.marketHours_0.Market = MainModule.Instance.DataSources.GetProviderInstance(item.DataSet.Provider.GetType()).GetMarketInfo(item.Symbol);
            }
            item.Refresh();
        }

        private void btnActivate_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.btnActivate.Checked)
            {
                this.btnActivate.Text = "De-activate Strategy";
                this.btnActivate.ToolTipText = "De-activate the selected Strategy";
            }
            else
            {
                this.btnActivate.Text = "Activate Strategy";
                this.btnActivate.ToolTipText = "Activate the selected Strategy";
            }
            this.mniActivate.Text = this.btnActivate.Text;
            this.mniActivate.ToolTipText = this.mniActivate.ToolTipText;
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            StrategyCenterItem itemSelected = this.ItemSelected;
            if (itemSelected != null)
            {
                this.method_3(itemSelected, !this.btnActivate.Checked);
                this.method_21();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.method_1())
            {
                this.lvStrategies.Items[this.lvStrategies.Items.Count - 1].Selected = true;
                this.btnSettings.PerformClick();
                if (!this.bool_0)
                {
                    this.RemoveSelectedStrategy();
                }
                this.method_21();
            }
        }

        private void btnAutoStage_Click(object sender, EventArgs e)
        {
            StrategyCenterItem itemSelected = this.ItemSelected;
            if (itemSelected != null)
            {
                this.btnAutoStage.Checked = !this.btnAutoStage.Checked;
                itemSelected.AutoStage = this.btnAutoStage.Checked;
                this.method_5(itemSelected);
                this.method_21();
            }
        }

        private void btnEmailAlert_Click(object sender, EventArgs e)
        {
            StrategyCenterItem itemSelected = this.ItemSelected;
            if (itemSelected != null)
            {
                this.btnEmailAlert.Checked = !this.btnEmailAlert.Checked;
                itemSelected.EmailAlerts = this.btnEmailAlert.Checked;
                this.method_5(itemSelected);
                this.method_21();
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MainModule.Instance.ContextSensitiveHelp("strategy_pane.htm");
        }

        private void btnHelpAlerts_Click(object sender, EventArgs e)
        {
            MainModule.Instance.ContextSensitiveHelp("alerts_pane.htm");
        }

        private void btnPlace_Click(object sender, EventArgs e)
        {
            this.method_26(true);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.RemoveSelectedStrategy();
            this.method_21();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvAlerts.Items)
            {
                item.Selected = true;
            }
        }

        private void btnSendToQuote_Click(object sender, EventArgs e)
        {
            if (MainModule.Instance.Authenticate())
            {
                foreach (ListViewItem item in this.lvAlerts.Items)
                {
                    if (item.Selected)
                    {
                        Alert tag = (Alert) item.Tag;
                        Account account = MainModule.Instance.TradeManager.FindAccount(tag.Account);
                        if (account == null)
                        {
                            account = MainModule.Instance.TradeManager.FindAccount(tag.Account);
                            if (account == null)
                            {
                                continue;
                            }
                        }
                        QuotesForm quoteForm = this.MyMainForm.GetQuoteForm(account.AutoTradingMode);
                        quoteForm.BringToFront();
                        quoteForm.WindowState = FormWindowState.Normal;
                        quoteForm.AddAlert(tag);
                    }
                }
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            StrategyCenterItem itemSelected = this.ItemSelected;
            if (itemSelected != null)
            {
                StrategySettingsForm form = new StrategySettingsForm {
                    Item = itemSelected
                };
                this.bool_0 = false;
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    itemSelected.ExecuteHours = form.ExecuteHours;
                    itemSelected.ExecuteMin = form.ExecuteMin;
                    if ((itemSelected.NextRun != DateTime.MaxValue) && !form.DataScale.IsIntraday)
                    {
                        itemSelected.NextRun = new DateTime(itemSelected.NextRun.Year, itemSelected.NextRun.Month, itemSelected.NextRun.Day, itemSelected.ExecuteHours, itemSelected.ExecuteMin, 0);
                    }
                    this.method_3(itemSelected, false);
                    this.ChangeData(form.DataSet, form.Symbol);
                    this.ChangeDataRange(form.DataRange);
                    this.ChangePositionSize(form.PositionSize);
                    this.ChangeScale(form.DataScale.Scale, form.DataScale.BarInterval);
                    itemSelected.AutoStage = form.AutoStage;
                    itemSelected.EmailAlerts = form.EmailAlerts;
                    this.btnAutoStage.Checked = itemSelected.AutoStage;
                    if (MainModule.Instance.EmailSettingsAvailable)
                    {
                        this.btnEmailAlert.Checked = itemSelected.EmailAlerts;
                    }
                    itemSelected.AccountNumber = form.AccountNumber;
                    itemSelected.AccountTradeType = form.AccountTradeType;
                    this.bool_0 = true;
                    itemSelected.BarsList.Clear();
                    itemSelected.AlertsList.Clear();
                    itemSelected.UsePreferredValues = form.UsePreferredValues;
                    this.ShowAutoTradingState(MainModule.Instance.AutoTradingEnabled);
                    itemSelected.method_0(this.marketHours_0);
                    itemSelected.Refresh();
                    this.method_21();
                }
            }
        }

        private void btnShowAllAlerts_Click(object sender, EventArgs e)
        {
            this.btnShowAllAlerts.Checked = !this.btnShowAllAlerts.Checked;
            this.mniShowAllAlerts.Checked = this.btnShowAllAlerts.Checked;
            MainModule.Instance.Settings.Set("StrategyMonitor_ShowAllAlerts", this.btnShowAllAlerts.Checked);
            this.method_25();
        }

        private void btnShowLocalTime_Click(object sender, EventArgs e)
        {
            this.btnShowLocalTime.Checked = !this.btnShowLocalTime.Checked;
            this.mniShowLocalTime.Checked = this.btnShowLocalTime.Checked;
            MainModule.Instance.Settings.Set("ShowLocalTime", this.btnShowLocalTime.Checked);
            this.method_32(this.btnShowLocalTime.Checked);
        }

        private void btnStage_Click(object sender, EventArgs e)
        {
            this.method_26(false);
        }

        public void ChangeData(DataSource dataSource, string symbol)
        {
            StrategyCenterItem itemSelected = this.ItemSelected;
            if (itemSelected != null)
            {
                itemSelected.DataSet = dataSource;
                itemSelected.Symbol = symbol;
                if (symbol == "")
                {
                    itemSelected.TargetDataType = StrategyCenterItemDataType.DataSet;
                }
                else
                {
                    itemSelected.TargetDataType = StrategyCenterItemDataType.Symbol;
                }
                this.btnActivate.Enabled = true;
                this.mniActivate.Enabled = true;
                this.btnAutoStage.Enabled = true;
                this.mniGenerateOrders.Enabled = true;
                if (MainModule.Instance.EmailSettingsAvailable)
                {
                    this.btnEmailAlert.Enabled = true;
                    this.mniEmailAlerts.Enabled = true;
                }
                else
                {
                    this.btnEmailAlert.ToolTipText = "Please add Email Settings in Preferences to enable this feature.";
                }
                itemSelected.Refresh();
            }
        }

        public void ChangeDataRange(BarDataRange dataRange)
        {
            StrategyCenterItem itemSelected = this.ItemSelected;
            if (itemSelected != null)
            {
                itemSelected.DataRange = dataRange;
                itemSelected.Refresh();
            }
        }

        public void ChangePositionSize(PositionSize posSize)
        {
            StrategyCenterItem itemSelected = this.ItemSelected;
            if (itemSelected != null)
            {
                itemSelected.PositionSize = posSize;
                itemSelected.Refresh();
            }
        }

        public void ChangeScale(BarScale scale, int barInterval)
        {
            StrategyCenterItem itemSelected = this.ItemSelected;
            if (itemSelected != null)
            {
                itemSelected.Scale = scale;
                itemSelected.BarInterval = barInterval;
                new BarDataScale(scale, barInterval);
                itemSelected.Refresh();
            }
        }

        public int Compare(Bars bars_0, Bars bars_1)
        {
            return bars_0.Symbol.CompareTo(bars_1.Symbol);
        }

        public void CopyToClipboard()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Strategies");
            builder.Append(Environment.NewLine);
            foreach (ColumnHeader header2 in this.lvStrategies.Columns)
            {
                builder.Append(header2.Text);
                builder.Append('\t');
            }
            builder.Append(Environment.NewLine);
            foreach (ListViewItem item2 in this.lvStrategies.Items)
            {
                builder.Append(item2.Text);
                for (int i = 1; i < item2.SubItems.Count; i++)
                {
                    builder.Append('\t');
                    builder.Append(item2.SubItems[i].Text);
                }
                builder.Append(Environment.NewLine);
            }
            builder.Append(Environment.NewLine);
            builder.Append("Alerts");
            builder.Append(Environment.NewLine);
            foreach (ColumnHeader header in this.lvAlerts.Columns)
            {
                builder.Append(header.Text);
                builder.Append('\t');
            }
            builder.Append(Environment.NewLine);
            foreach (ListViewItem item in this.lvAlerts.Items)
            {
                builder.Append(item.Text);
                for (int j = 1; j < item.SubItems.Count; j++)
                {
                    builder.Append('\t');
                    builder.Append(item.SubItems[j].Text);
                }
                builder.Append(Environment.NewLine);
            }
            try
            {
                Clipboard.SetText(builder.ToString());
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(StrategyCenterForm));
            PositionSize size = new PositionSize();
            this.splitMain = new SplitContainer();
            this.lvStrategies = new SortableListView();
            this.columnHeader_12 = new ColumnHeader();
            this.columnHeader_13 = new ColumnHeader();
            this.columnHeader_14 = new ColumnHeader();
            this.columnHeader_15 = new ColumnHeader();
            this.columnHeader_22 = new ColumnHeader();
            this.columnHeader_16 = new ColumnHeader();
            this.columnHeader_17 = new ColumnHeader();
            this.columnHeader_18 = new ColumnHeader();
            this.columnHeader_19 = new ColumnHeader();
            this.columnHeader_20 = new ColumnHeader();
            this.columnHeader_21 = new ColumnHeader();
            this.columnHeader_26 = new ColumnHeader();
            this.columnHeader_24 = new ColumnHeader();
            this.popupStrategies = new ContextMenuStrip(this.components);
            this.mniAddStrategy = new ToolStripMenuItem();
            this.mniRemoveStrategy = new ToolStripMenuItem();
            this.mniChangeSettings = new ToolStripMenuItem();
            this.mniShowLocalTime = new ToolStripMenuItem();
            this.sepMenu = new ToolStripSeparator();
            this.mniActivate = new ToolStripMenuItem();
            this.mniGenerateOrders = new ToolStripMenuItem();
            this.sepMenu2 = new ToolStripSeparator();
            this.mniEmailAlerts = new ToolStripMenuItem();
            this.mniOpen = new ToolStripMenuItem();
            this.mniRunNow = new ToolStripMenuItem();
            this.sepMenu3 = new ToolStripSeparator();
            this.mniCopyStrategies = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.toolStripSeparator7 = new ToolStripSeparator();
            this.mniItemLogging = new ToolStripMenuItem();
            this.mniDisableLogging = new ToolStripMenuItem();
            this.mniViewItemLog = new ToolStripMenuItem();
            this.imageList_0 = new ImageList(this.components);
            this.imageList_2 = new ImageList(this.components);
            this.toolbar = new ToolStrip();
            this.lblStrategies = new ToolStripLabel();
            this.btnAdd = new ToolStripButton();
            this.btnRemove = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.btnShowLocalTime = new ToolStripButton();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.btnSettings = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.btnActivate = new ToolStripButton();
            this.btnAutoStage = new ToolStripButton();
            this.btnEmailAlert = new ToolStripButton();
            this.toolStripSeparator6 = new ToolStripSeparator();
            this.btnHelp = new ToolStripButton();
            this.lvAlerts = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.columnHeader_5 = new ColumnHeader();
            this.columnHeader_6 = new ColumnHeader();
            this.columnHeader_7 = new ColumnHeader();
            this.columnHeader_8 = new ColumnHeader();
            this.columnHeader_9 = new ColumnHeader();
            this.columnHeader_10 = new ColumnHeader();
            this.columnHeader_11 = new ColumnHeader();
            this.columnHeader_23 = new ColumnHeader();
            this.columnHeader_25 = new ColumnHeader();
            this.popupAlerts = new ContextMenuStrip(this.components);
            this.mniSelectAll = new ToolStripMenuItem();
            this.mniEditAlert = new ToolStripMenuItem();
            this.sepSelectAll = new ToolStripSeparator();
            this.mniPlace = new ToolStripMenuItem();
            this.mniStage = new ToolStripMenuItem();
            this.sepStage = new ToolStripSeparator();
            this.mniSendToQuote = new ToolStripMenuItem();
            this.sepQuote = new ToolStripSeparator();
            this.mniShowAllAlerts = new ToolStripMenuItem();
            this.sepPrint = new ToolStripSeparator();
            this.mniCopyAlerts = new ToolStripMenuItem();
            this.mniPrintAlerts = new ToolStripMenuItem();
            this.imageList_1 = new ImageList(this.components);
            this.toolbarAlerts = new ToolStrip();
            this.lblAlerts = new ToolStripLabel();
            this.btnSelectAll = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.btnPlace = new ToolStripButton();
            this.btnStage = new ToolStripButton();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.btnSendToQuote = new ToolStripButton();
            this.sepQuotes = new ToolStripSeparator();
            this.btnShowAllAlerts = new ToolStripButton();
            this.sepHelp = new ToolStripSeparator();
            this.btnHelpAlerts = new ToolStripButton();
            this.status = new StatusStrip();
            this.statusStrategies = new ToolStripStatusLabel();
            this.statusActive = new ToolStripStatusLabel();
            this.statusAlerts = new ToolStripStatusLabel();
            this.statusSymbols = new ToolStripStatusLabel();
            this.timer_0 = new System.Windows.Forms.Timer(this.components);
            this.tradingSystemExecutor_0 = new TradingSystemExecutor(this.components);
            this.barsLoader_0 = new BarsLoader(this.components);
            this.fundamentalsLoader_0 = new FundamentalsLoader(this.components);
            this.marketHours_0 = new MarketHours(this.components);
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.popupStrategies.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.popupAlerts.SuspendLayout();
            this.toolbarAlerts.SuspendLayout();
            this.status.SuspendLayout();
            base.SuspendLayout();
            this.splitMain.BackColor = SystemColors.ControlDark;
            this.splitMain.Dock = DockStyle.Fill;
            this.splitMain.Location = new Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = Orientation.Horizontal;
            this.splitMain.Panel1.BackColor = SystemColors.Control;
            this.splitMain.Panel1.Controls.Add(this.lvStrategies);
            this.splitMain.Panel1.Controls.Add(this.toolbar);
            this.splitMain.Panel1.Click += new EventHandler(this.btnRemove_Click);
            this.splitMain.Panel2.BackColor = SystemColors.Control;
            this.splitMain.Panel2.Controls.Add(this.lvAlerts);
            this.splitMain.Panel2.Controls.Add(this.toolbarAlerts);
            this.splitMain.Panel2.Controls.Add(this.status);
            this.splitMain.Size = new Size(0x33f, 0x1d9);
            this.splitMain.SplitterDistance = 0x109;
            this.splitMain.TabIndex = 0;
            this.lvStrategies.Columns.AddRange(new ColumnHeader[] { this.columnHeader_12, this.columnHeader_13, this.columnHeader_14, this.columnHeader_15, this.columnHeader_22, this.columnHeader_16, this.columnHeader_17, this.columnHeader_18, this.columnHeader_19, this.columnHeader_20, this.columnHeader_21, this.columnHeader_26, this.columnHeader_24 });
            this.lvStrategies.ContextMenuStrip = this.popupStrategies;
            this.lvStrategies.Dock = DockStyle.Fill;
            this.lvStrategies.FullRowSelect = true;
            this.lvStrategies.HideSelection = false;
            this.lvStrategies.Location = new Point(0, 0x19);
            this.lvStrategies.MultiSelect = false;
            this.lvStrategies.Name = "lvStrategies";
            this.lvStrategies.Size = new Size(0x33f, 240);
            this.lvStrategies.SmallImageList = this.imageList_0;
            this.lvStrategies.StateImageList = this.imageList_2;
            this.lvStrategies.TabIndex = 13;
            this.lvStrategies.UseCompatibleStateImageBehavior = false;
            this.lvStrategies.View = View.Details;
            this.lvStrategies.SelectedIndexChanged += new EventHandler(this.lvStrategies_SelectedIndexChanged);
            this.lvStrategies.DoubleClick += new EventHandler(this.btnSettings_Click);
            this.columnHeader_12.Text = "Strategy";
            this.columnHeader_12.Width = 0xa2;
            this.columnHeader_13.Text = "Account";
            this.columnHeader_14.Text = "Last Run";
            this.columnHeader_14.Width = 120;
            this.columnHeader_15.Text = "Next Run";
            this.columnHeader_15.Width = 120;
            this.columnHeader_22.Text = "Trades";
            this.columnHeader_16.Text = "Alerts";
            this.columnHeader_17.Text = "Data";
            this.columnHeader_17.Width = 80;
            this.columnHeader_18.Text = "Range";
            this.columnHeader_18.Width = 80;
            this.columnHeader_19.Text = "Scale";
            this.columnHeader_19.Width = 80;
            this.columnHeader_20.Text = "Position Size";
            this.columnHeader_20.Width = 80;
            this.columnHeader_21.Text = "Parameters";
            this.columnHeader_21.Width = 80;
            this.columnHeader_26.Text = "Trade Type";
            this.columnHeader_24.Text = "Action";
            this.columnHeader_24.Width = 80;
            this.popupStrategies.Items.AddRange(new ToolStripItem[] { 
                this.mniAddStrategy, this.mniRemoveStrategy, this.mniChangeSettings, this.mniShowLocalTime, this.sepMenu, this.mniActivate, this.mniGenerateOrders, this.sepMenu2, this.mniEmailAlerts, this.mniOpen, this.mniRunNow, this.sepMenu3, this.mniCopyStrategies, this.mniPrint, this.toolStripSeparator7, this.mniItemLogging, 
                this.mniDisableLogging, this.mniViewItemLog
             });
            this.popupStrategies.Name = "popupStrategies";
            this.popupStrategies.Size = new Size(0x11a, 0x166);
            this.mniAddStrategy.Image = (Image) resources.GetObject("mniAddStrategy.Image");
            this.mniAddStrategy.ImageTransparentColor = Color.Fuchsia;
            this.mniAddStrategy.Name = "mniAddStrategy";
            this.mniAddStrategy.Size = new Size(0x119, 0x16);
            this.mniAddStrategy.Text = "Add a Strategy ...";
            this.mniAddStrategy.Click += new EventHandler(this.btnAdd_Click);
            this.mniRemoveStrategy.Enabled = false;
            this.mniRemoveStrategy.Image = (Image) resources.GetObject("mniRemoveStrategy.Image");
            this.mniRemoveStrategy.ImageTransparentColor = Color.Fuchsia;
            this.mniRemoveStrategy.Name = "mniRemoveStrategy";
            this.mniRemoveStrategy.Size = new Size(0x119, 0x16);
            this.mniRemoveStrategy.Text = "Remove Selected Strategy";
            this.mniRemoveStrategy.Click += new EventHandler(this.btnRemove_Click);
            this.mniChangeSettings.Enabled = false;
            this.mniChangeSettings.Image = (Image) resources.GetObject("mniChangeSettings.Image");
            this.mniChangeSettings.ImageTransparentColor = Color.Fuchsia;
            this.mniChangeSettings.Name = "mniChangeSettings";
            this.mniChangeSettings.Size = new Size(0x119, 0x16);
            this.mniChangeSettings.Text = "Change Settings ...";
            this.mniChangeSettings.Click += new EventHandler(this.btnSettings_Click);
            this.mniShowLocalTime.Name = "mniShowLocalTime";
            this.mniShowLocalTime.Size = new Size(0x119, 0x16);
            this.mniShowLocalTime.Text = "Show Local Times";
            this.mniShowLocalTime.Click += new EventHandler(this.btnShowLocalTime_Click);
            this.sepMenu.Name = "sepMenu";
            this.sepMenu.Size = new Size(0x116, 6);
            this.mniActivate.Enabled = false;
            this.mniActivate.Image = (Image) resources.GetObject("mniActivate.Image");
            this.mniActivate.ImageTransparentColor = Color.Fuchsia;
            this.mniActivate.Name = "mniActivate";
            this.mniActivate.Size = new Size(0x119, 0x16);
            this.mniActivate.Text = "Activate Strategy";
            this.mniActivate.Click += new EventHandler(this.btnActivate_Click);
            this.mniGenerateOrders.Enabled = false;
            this.mniGenerateOrders.Image = (Image) resources.GetObject("mniGenerateOrders.Image");
            this.mniGenerateOrders.ImageTransparentColor = Color.Fuchsia;
            this.mniGenerateOrders.Name = "mniGenerateOrders";
            this.mniGenerateOrders.Size = new Size(0x119, 0x16);
            this.mniGenerateOrders.Text = "Auto-Stage";
            this.mniGenerateOrders.Click += new EventHandler(this.btnAutoStage_Click);
            this.sepMenu2.Name = "sepMenu2";
            this.sepMenu2.Size = new Size(0x116, 6);
            this.mniEmailAlerts.Enabled = false;
            this.mniEmailAlerts.Image = (Image) resources.GetObject("mniEmailAlerts.Image");
            this.mniEmailAlerts.ImageScaling = ToolStripItemImageScaling.None;
            this.mniEmailAlerts.Name = "mniEmailAlerts";
            this.mniEmailAlerts.Size = new Size(0x119, 0x16);
            this.mniEmailAlerts.Text = "Auto-Email";
            this.mniEmailAlerts.Click += new EventHandler(this.btnEmailAlert_Click);
            this.mniOpen.Enabled = false;
            this.mniOpen.Image = (Image) resources.GetObject("mniOpen.Image");
            this.mniOpen.ImageTransparentColor = Color.Fuchsia;
            this.mniOpen.Name = "mniOpen";
            this.mniOpen.Size = new Size(0x119, 0x16);
            this.mniOpen.Text = "Open this Strategy in a Strategy window";
            this.mniOpen.Click += new EventHandler(this.mniOpen_Click);
            this.mniRunNow.Enabled = false;
            this.mniRunNow.Image = (Image) resources.GetObject("mniRunNow.Image");
            this.mniRunNow.ImageTransparentColor = Color.Fuchsia;
            this.mniRunNow.Name = "mniRunNow";
            this.mniRunNow.Size = new Size(0x119, 0x16);
            this.mniRunNow.Text = "Run this Strategy now";
            this.mniRunNow.Click += new EventHandler(this.mniRunNow_Click);
            this.sepMenu3.Name = "sepMenu3";
            this.sepMenu3.Size = new Size(0x116, 6);
            this.mniCopyStrategies.Image = (Image) resources.GetObject("mniCopyStrategies.Image");
            this.mniCopyStrategies.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyStrategies.Name = "mniCopyStrategies";
            this.mniCopyStrategies.Size = new Size(0x119, 0x16);
            this.mniCopyStrategies.Text = "Copy Strategies to Clipboard";
            this.mniCopyStrategies.Click += new EventHandler(this.mniCopyStrategies_Click);
            this.mniPrint.Image = (Image) resources.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0x119, 0x16);
            this.mniPrint.Text = "Print Strategies";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new Size(0x116, 6);
            this.mniItemLogging.Name = "mniItemLogging";
            this.mniItemLogging.Size = new Size(0x119, 0x16);
            this.mniItemLogging.Text = "Enable Logging for Selected Item(s)";
            this.mniItemLogging.Click += new EventHandler(this.mniItemLogging_Click);
            this.mniDisableLogging.Name = "mniDisableLogging";
            this.mniDisableLogging.Size = new Size(0x119, 0x16);
            this.mniDisableLogging.Text = "Disable Logging for Selected Item(s)";
            this.mniDisableLogging.Click += new EventHandler(this.mniDisableLogging_Click);
            this.mniViewItemLog.Name = "mniViewItemLog";
            this.mniViewItemLog.Size = new Size(0x119, 0x16);
            this.mniViewItemLog.Text = "View the Item Log";
            this.mniViewItemLog.Click += new EventHandler(this.mniViewItemLog_Click);
            this.imageList_0.ImageStream = (ImageListStreamer) resources.GetObject("images.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "Execute.bmp");
            this.imageList_0.Images.SetKeyName(1, "SendToOrderMgr.bmp");
            this.imageList_0.Images.SetKeyName(2, "Execute.bmp");
            this.imageList_2.ImageStream = (ImageListStreamer) resources.GetObject("imagesState.ImageStream");
            this.imageList_2.TransparentColor = Color.Silver;
            this.imageList_2.Images.SetKeyName(0, "alertsent.bmp");
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.lblStrategies, this.btnAdd, this.btnRemove, this.toolStripSeparator2, this.btnShowLocalTime, this.toolStripSeparator5, this.btnSettings, this.toolStripSeparator1, this.btnActivate, this.btnAutoStage, this.btnEmailAlert, this.toolStripSeparator6, this.btnHelp });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x33f, 0x19);
            this.toolbar.TabIndex = 12;
            this.toolbar.Text = "toolStrip1";
            this.lblStrategies.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.lblStrategies.Name = "lblStrategies";
            this.lblStrategies.Size = new Size(0x42, 0x16);
            this.lblStrategies.Text = "Strategies";
            this.btnAdd.Image = (Image) resources.GetObject("btnAdd.Image");
            this.btnAdd.ImageTransparentColor = Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(0x5b, 0x16);
            this.btnAdd.Text = "Add Strategy";
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.btnRemove.Enabled = false;
            this.btnRemove.Image = (Image) resources.GetObject("btnRemove.Image");
            this.btnRemove.ImageTransparentColor = Color.Magenta;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new Size(0x6f, 0x16);
            this.btnRemove.Text = "Remove Strategy";
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.btnShowLocalTime.ImageTransparentColor = Color.Magenta;
            this.btnShowLocalTime.Name = "btnShowLocalTime";
            this.btnShowLocalTime.Size = new Size(0x5e, 0x16);
            this.btnShowLocalTime.Text = "Show Local Times";
            this.btnShowLocalTime.Click += new EventHandler(this.btnShowLocalTime_Click);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(6, 0x19);
            this.btnSettings.Enabled = false;
            this.btnSettings.Image = (Image) resources.GetObject("btnSettings.Image");
            this.btnSettings.ImageTransparentColor = Color.Magenta;
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new Size(0x6a, 0x16);
            this.btnSettings.Text = "Change Settings";
            this.btnSettings.ToolTipText = "Change the Settings for the Selected Strategy";
            this.btnSettings.Click += new EventHandler(this.btnSettings_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.btnActivate.Enabled = false;
            this.btnActivate.Image = (Image) resources.GetObject("btnActivate.Image");
            this.btnActivate.ImageTransparentColor = Color.Magenta;
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new Size(0x70, 0x16);
            this.btnActivate.Text = "Activate Strategy";
            this.btnActivate.ToolTipText = "Activate the Selected Strategy to automatically produce Alerts";
            this.btnActivate.CheckStateChanged += new EventHandler(this.btnActivate_CheckStateChanged);
            this.btnActivate.Click += new EventHandler(this.btnActivate_Click);
            this.btnAutoStage.Enabled = false;
            this.btnAutoStage.Image = (Image) resources.GetObject("btnAutoStage.Image");
            this.btnAutoStage.ImageTransparentColor = Color.Magenta;
            this.btnAutoStage.Name = "btnAutoStage";
            this.btnAutoStage.Size = new Size(0x52, 0x16);
            this.btnAutoStage.Text = "Auto-Stage";
            this.btnAutoStage.ToolTipText = "Automatically Stage Orders from Strategy Alerts";
            this.btnAutoStage.Click += new EventHandler(this.btnAutoStage_Click);
            this.btnEmailAlert.Enabled = false;
            this.btnEmailAlert.Image = (Image) resources.GetObject("btnEmailAlert.Image");
            this.btnEmailAlert.ImageScaling = ToolStripItemImageScaling.None;
            this.btnEmailAlert.ImageTransparentColor = Color.Magenta;
            this.btnEmailAlert.Name = "btnEmailAlert";
            this.btnEmailAlert.Size = new Size(0x4f, 0x16);
            this.btnEmailAlert.Text = "Auto-Email";
            this.btnEmailAlert.ToolTipText = "Automatically Send Emails for Strategy Trade Alerts ";
            this.btnEmailAlert.Click += new EventHandler(this.btnEmailAlert_Click);
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new Size(6, 0x19);
            this.btnHelp.Image = (Image) resources.GetObject("btnHelp.Image");
            this.btnHelp.ImageTransparentColor = Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x30, 0x16);
            this.btnHelp.Text = "Help";
            this.btnHelp.ToolTipText = "Help on Strategy Monitor";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.lvAlerts.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_2, this.columnHeader_1, this.columnHeader_3, this.columnHeader_4, this.columnHeader_5, this.columnHeader_6, this.columnHeader_7, this.columnHeader_8, this.columnHeader_9, this.columnHeader_10, this.columnHeader_11, this.columnHeader_23, this.columnHeader_25 });
            this.lvAlerts.ContextMenuStrip = this.popupAlerts;
            this.lvAlerts.Dock = DockStyle.Fill;
            this.lvAlerts.FullRowSelect = true;
            this.lvAlerts.HideSelection = false;
            this.lvAlerts.Location = new Point(0, 0x19);
            this.lvAlerts.Name = "lvAlerts";
            this.lvAlerts.Size = new Size(0x33f, 0x9d);
            this.lvAlerts.SmallImageList = this.imageList_1;
            this.lvAlerts.TabIndex = 13;
            this.lvAlerts.UseCompatibleStateImageBehavior = false;
            this.lvAlerts.View = View.Details;
            this.lvAlerts.SelectedIndexChanged += new EventHandler(this.lvAlerts_SelectedIndexChanged);
            this.lvAlerts.DoubleClick += new EventHandler(this.lvAlerts_DoubleClick);
            this.lvAlerts.ItemDrag += new ItemDragEventHandler(this.lvAlerts_ItemDrag);
            this.columnHeader_0.Tag = "DT";
            this.columnHeader_0.Text = "Alert Time";
            this.columnHeader_0.Width = 120;
            this.columnHeader_2.Tag = "S";
            this.columnHeader_2.Text = "Symbol";
            this.columnHeader_1.Tag = "S";
            this.columnHeader_1.Text = "Account";
            this.columnHeader_3.Tag = "S";
            this.columnHeader_3.Text = "Action";
            this.columnHeader_4.Tag = "N";
            this.columnHeader_4.Text = "Qty";
            this.columnHeader_5.Tag = "S";
            this.columnHeader_5.Text = "Order Type";
            this.columnHeader_5.Width = 70;
            this.columnHeader_6.Tag = "N";
            this.columnHeader_6.Text = "Price";
            this.columnHeader_6.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_7.Tag = "S";
            this.columnHeader_7.Text = "Strategy";
            this.columnHeader_7.Width = 120;
            this.columnHeader_8.Tag = "S";
            this.columnHeader_8.Text = "Data";
            this.columnHeader_9.Tag = "S";
            this.columnHeader_9.Text = "Range";
            this.columnHeader_10.Tag = "S";
            this.columnHeader_10.Text = "Scale";
            this.columnHeader_11.Text = "Position Size";
            this.columnHeader_23.Text = "Signal Name";
            this.columnHeader_23.Width = 80;
            this.columnHeader_25.Text = "Trade Type";
            this.popupAlerts.Items.AddRange(new ToolStripItem[] { this.mniSelectAll, this.mniEditAlert, this.sepSelectAll, this.mniPlace, this.mniStage, this.sepStage, this.mniSendToQuote, this.sepQuote, this.mniShowAllAlerts, this.sepPrint, this.mniCopyAlerts, this.mniPrintAlerts });
            this.popupAlerts.Name = "popupAlerts";
            this.popupAlerts.Size = new Size(0x11a, 0xcc);
            this.mniSelectAll.Image = (Image) resources.GetObject("mniSelectAll.Image");
            this.mniSelectAll.ImageTransparentColor = Color.Fuchsia;
            this.mniSelectAll.Name = "mniSelectAll";
            this.mniSelectAll.Size = new Size(0x119, 0x16);
            this.mniSelectAll.Text = "Select All";
            this.mniSelectAll.Click += new EventHandler(this.btnSelectAll_Click);
            this.mniEditAlert.Enabled = false;
            this.mniEditAlert.Image = (Image) resources.GetObject("mniEditAlert.Image");
            this.mniEditAlert.ImageTransparentColor = Color.Fuchsia;
            this.mniEditAlert.Name = "mniEditAlert";
            this.mniEditAlert.Size = new Size(0x119, 0x16);
            this.mniEditAlert.Text = "Edit Alert ...";
            this.mniEditAlert.Click += new EventHandler(this.mniEditAlert_Click);
            this.sepSelectAll.Name = "sepSelectAll";
            this.sepSelectAll.Size = new Size(0x116, 6);
            this.mniPlace.Enabled = false;
            this.mniPlace.Image = (Image) resources.GetObject("mniPlace.Image");
            this.mniPlace.ImageTransparentColor = Color.Fuchsia;
            this.mniPlace.Name = "mniPlace";
            this.mniPlace.Size = new Size(0x119, 0x16);
            this.mniPlace.Text = "Place Selected Orders";
            this.mniPlace.Click += new EventHandler(this.btnPlace_Click);
            this.mniStage.Enabled = false;
            this.mniStage.Image = (Image) resources.GetObject("mniStage.Image");
            this.mniStage.ImageTransparentColor = Color.Fuchsia;
            this.mniStage.Name = "mniStage";
            this.mniStage.Size = new Size(0x119, 0x16);
            this.mniStage.Text = "Stage Selected Orders";
            this.mniStage.Click += new EventHandler(this.btnStage_Click);
            this.sepStage.Name = "sepStage";
            this.sepStage.Size = new Size(0x116, 6);
            this.mniSendToQuote.Enabled = false;
            this.mniSendToQuote.Image = (Image) resources.GetObject("mniSendToQuote.Image");
            this.mniSendToQuote.ImageTransparentColor = Color.Fuchsia;
            this.mniSendToQuote.Name = "mniSendToQuote";
            this.mniSendToQuote.Size = new Size(0x119, 0x16);
            this.mniSendToQuote.Text = "Monitor Selected Alerts in Quote Window";
            this.mniSendToQuote.Click += new EventHandler(this.btnSendToQuote_Click);
            this.sepQuote.Name = "sepQuote";
            this.sepQuote.Size = new Size(0x116, 6);
            this.mniShowAllAlerts.Name = "mniShowAllAlerts";
            this.mniShowAllAlerts.Size = new Size(0x119, 0x16);
            this.mniShowAllAlerts.Text = "Show Alerts for all Strategies";
            this.mniShowAllAlerts.Click += new EventHandler(this.btnShowAllAlerts_Click);
            this.sepPrint.Name = "sepPrint";
            this.sepPrint.Size = new Size(0x116, 6);
            this.mniCopyAlerts.Image = (Image) resources.GetObject("mniCopyAlerts.Image");
            this.mniCopyAlerts.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyAlerts.Name = "mniCopyAlerts";
            this.mniCopyAlerts.Size = new Size(0x119, 0x16);
            this.mniCopyAlerts.Text = "Copy Alerts to Clipboard";
            this.mniCopyAlerts.Click += new EventHandler(this.mniCopyAlerts_Click);
            this.mniPrintAlerts.Image = (Image) resources.GetObject("mniPrintAlerts.Image");
            this.mniPrintAlerts.Name = "mniPrintAlerts";
            this.mniPrintAlerts.Size = new Size(0x119, 0x16);
            this.mniPrintAlerts.Text = "Print Alerts";
            this.mniPrintAlerts.Click += new EventHandler(this.mniPrintAlerts_Click);
            this.imageList_1.ImageStream = (ImageListStreamer) resources.GetObject("imgOrders.ImageStream");
            this.imageList_1.TransparentColor = Color.Silver;
            this.imageList_1.Images.SetKeyName(0, "buy.bmp");
            this.imageList_1.Images.SetKeyName(1, "sell.bmp");
            this.imageList_1.Images.SetKeyName(2, "short.bmp");
            this.imageList_1.Images.SetKeyName(3, "cover.bmp");
            this.toolbarAlerts.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbarAlerts.Items.AddRange(new ToolStripItem[] { this.lblAlerts, this.btnSelectAll, this.toolStripSeparator3, this.btnPlace, this.btnStage, this.toolStripSeparator4, this.btnSendToQuote, this.sepQuotes, this.btnShowAllAlerts, this.sepHelp, this.btnHelpAlerts });
            this.toolbarAlerts.Location = new Point(0, 0);
            this.toolbarAlerts.Name = "toolbarAlerts";
            this.toolbarAlerts.Size = new Size(0x33f, 0x19);
            this.toolbarAlerts.TabIndex = 12;
            this.toolbarAlerts.Text = "toolStrip1";
            this.lblAlerts.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.lblAlerts.Image = (Image) resources.GetObject("lblAlerts.Image");
            this.lblAlerts.ImageTransparentColor = Color.Silver;
            this.lblAlerts.Name = "lblAlerts";
            this.lblAlerts.Size = new Size(0x39, 0x16);
            this.lblAlerts.Text = "Alerts";
            this.btnSelectAll.Image = (Image) resources.GetObject("btnSelectAll.Image");
            this.btnSelectAll.ImageTransparentColor = Color.Magenta;
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new Size(70, 0x16);
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.Click += new EventHandler(this.btnSelectAll_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 0x19);
            this.btnPlace.Enabled = false;
            this.btnPlace.Image = (Image) resources.GetObject("btnPlace.Image");
            this.btnPlace.ImageTransparentColor = Color.Magenta;
            this.btnPlace.Name = "btnPlace";
            this.btnPlace.Size = new Size(0x58, 0x16);
            this.btnPlace.Text = "Place Orders";
            this.btnPlace.ToolTipText = "Place Orders for selected Alerts";
            this.btnPlace.Click += new EventHandler(this.btnPlace_Click);
            this.btnStage.Enabled = false;
            this.btnStage.Image = (Image) resources.GetObject("btnStage.Image");
            this.btnStage.ImageTransparentColor = Color.Magenta;
            this.btnStage.Name = "btnStage";
            this.btnStage.Size = new Size(0x5b, 0x16);
            this.btnStage.Text = "Stage Orders";
            this.btnStage.ToolTipText = "Stage Orders for selected Alerts";
            this.btnStage.Click += new EventHandler(this.btnStage_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(6, 0x19);
            this.btnSendToQuote.Enabled = false;
            this.btnSendToQuote.Image = (Image) resources.GetObject("btnSendToQuote.Image");
            this.btnSendToQuote.ImageTransparentColor = Color.Magenta;
            this.btnSendToQuote.Name = "btnSendToQuote";
            this.btnSendToQuote.Size = new Size(0x99, 0x16);
            this.btnSendToQuote.Text = "Monitor in Quotes Window";
            this.btnSendToQuote.ToolTipText = "Monitor selected Alerts in Quotes Window";
            this.btnSendToQuote.Click += new EventHandler(this.btnSendToQuote_Click);
            this.sepQuotes.Name = "sepQuotes";
            this.sepQuotes.Size = new Size(6, 0x19);
            this.btnShowAllAlerts.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnShowAllAlerts.Image = (Image) resources.GetObject("btnShowAllAlerts.Image");
            this.btnShowAllAlerts.ImageTransparentColor = Color.Magenta;
            this.btnShowAllAlerts.Name = "btnShowAllAlerts";
            this.btnShowAllAlerts.Size = new Size(150, 0x16);
            this.btnShowAllAlerts.Text = "Show Alerts for all Strategies";
            this.btnShowAllAlerts.Click += new EventHandler(this.btnShowAllAlerts_Click);
            this.sepHelp.Name = "sepHelp";
            this.sepHelp.Size = new Size(6, 0x19);
            this.btnHelpAlerts.Image = (Image) resources.GetObject("btnHelpAlerts.Image");
            this.btnHelpAlerts.ImageTransparentColor = Color.Magenta;
            this.btnHelpAlerts.Name = "btnHelpAlerts";
            this.btnHelpAlerts.Size = new Size(0x30, 0x16);
            this.btnHelpAlerts.Text = "Help";
            this.btnHelpAlerts.ToolTipText = "Help on Strategy Monitor";
            this.btnHelpAlerts.Click += new EventHandler(this.btnHelpAlerts_Click);
            this.status.Items.AddRange(new ToolStripItem[] { this.statusStrategies, this.statusActive, this.statusAlerts, this.statusSymbols });
            this.status.Location = new Point(0, 0xb6);
            this.status.Name = "status";
            this.status.Size = new Size(0x33f, 0x16);
            this.status.TabIndex = 2;
            this.status.Text = "statusStrip1";
            this.statusStrategies.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusStrategies.Name = "statusStrategies";
            this.statusStrategies.Size = new Size(0x45, 0x11);
            this.statusStrategies.Text = "0 Strategies";
            this.statusActive.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusActive.Name = "statusActive";
            this.statusActive.Size = new Size(50, 0x11);
            this.statusActive.Text = "0 Active";
            this.statusAlerts.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusAlerts.Name = "statusAlerts";
            this.statusAlerts.Size = new Size(0x30, 0x11);
            this.statusAlerts.Text = "0 Alerts";
            this.statusSymbols.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusSymbols.Name = "statusSymbols";
            this.statusSymbols.Size = new Size(0x92, 0x11);
            this.statusSymbols.Text = "Symbols pending updates: 0";
            this.timer_0.Enabled = true;
            this.timer_0.Tick += new EventHandler(this.timer_0_Tick);
            this.tradingSystemExecutor_0.ApplyCommission = false;
            this.tradingSystemExecutor_0.ApplyDividends = false;
            this.tradingSystemExecutor_0.ApplyInterest = false;
            this.tradingSystemExecutor_0.BarsLoader = this.barsLoader_0;
            this.tradingSystemExecutor_0.BenchmarkBuyAndHoldON = false;
            this.tradingSystemExecutor_0.BenchmarkSymbol = null;
            this.tradingSystemExecutor_0.BuildEquityCurves = true;
            this.tradingSystemExecutor_0.CashRate = 0.0;
            this.tradingSystemExecutor_0.EnableSlippage = false;
            this.tradingSystemExecutor_0.ExceptionEvents = false;
            this.tradingSystemExecutor_0.FundamentalsLoader = this.fundamentalsLoader_0;
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
            size.StartingCapital = 50000.0;
            this.tradingSystemExecutor_0.PosSize = size;
            this.tradingSystemExecutor_0.PricingDecimalPlaces = 0;
            this.tradingSystemExecutor_0.RedcuceQtyPct = 10.0;
            this.tradingSystemExecutor_0.ReduceQtyBasedOnVolume = false;
            this.tradingSystemExecutor_0.Renderer = null;
            this.tradingSystemExecutor_0.RoundLots = false;
            this.tradingSystemExecutor_0.RoundLots50 = false;
            this.tradingSystemExecutor_0.SlippageTicks = 1;
            this.tradingSystemExecutor_0.SlippageUnits = 1.0;
            this.tradingSystemExecutor_0.Strategy = null;
            this.tradingSystemExecutor_0.StrategyName = "";
            this.tradingSystemExecutor_0.WorstTradeSimulation = false;
            this.tradingSystemExecutor_0.LookupStrategy += new EventHandler<StrategyEventArgs>(this.method_33);
            this.barsLoader_0.AutoConvertScale = true;
            this.barsLoader_0.AutoCreateProvider = true;
            this.barsLoader_0.BarInterval = 0;
            this.barsLoader_0.EndDate = new DateTime(0x270f, 12, 0x1f, 0x17, 0x3b, 0x3b, 0x3e7);
            this.barsLoader_0.IncludePartialBar = false;
            this.barsLoader_0.MaxBars = 0;
            this.barsLoader_0.OverrideOnDemand = false;
            this.barsLoader_0.OverrideOnDemandValue = false;
            this.barsLoader_0.Scale = BarScale.Daily;
            this.barsLoader_0.StartDate = new DateTime(0L);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x33f, 0x1d9);
            base.Controls.Add(this.splitMain);
            this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "StrategyCenterForm";
            base.StartPosition = FormStartPosition.WindowsDefaultBounds;
            this.Text = "Strategy Monitor";
            base.Load += new EventHandler(this.StrategyCenterForm_Load);
            base.FormClosed += new FormClosedEventHandler(this.StrategyCenterForm_FormClosed);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.Panel2.PerformLayout();
            this.splitMain.ResumeLayout(false);
            this.popupStrategies.ResumeLayout(false);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.popupAlerts.ResumeLayout(false);
            this.toolbarAlerts.ResumeLayout(false);
            this.toolbarAlerts.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            base.ResumeLayout(false);
        }

        public void LoadWorkspaceItems(IList<string> items, int version)
        {
            this.btnShowAllAlerts.Checked = bool.Parse(items[0]);
            this.mniShowAllAlerts.Checked = this.btnShowAllAlerts.Checked;
        }

        private void lvAlerts_DoubleClick(object sender, EventArgs e)
        {
            if (this.lvAlerts.SelectedItems.Count == 1)
            {
                StrategyCenterItem tag = (StrategyCenterItem) this.lvAlerts.SelectedItems[0].SubItems[1].Tag;
                this.method_20(tag);
            }
        }

        private void lvAlerts_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) || (e.Button == MouseButtons.Left))
            {
                base.DoDragDrop(e.Item, DragDropEffects.Copy);
            }
        }

        private void lvAlerts_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag;
            bool flag1;
            bool count = this.lvAlerts.SelectedItems.Count > 0;
            flag = (!MainModule.Instance.AuthProvider.LoggedIn ? false : MainModule.Instance.BrokerProvider != null);
            bool flag2 = flag;
            bool flag3 = flag;
            if (!flag2)
            {
                IEnumerator enumerator = this.lvAlerts.SelectedItems.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            ListViewItem current = (ListViewItem)enumerator.Current;
                            Alert tag = current.Tag as Alert;
                            if (tag.Account.StartsWith("Paper"))
                            {
                                flag3 = true;
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
            }
            ToolStripButton toolStripButton = this.btnPlace;
            flag1 = (!count ? false : flag3);
            toolStripButton.Enabled = flag1;
            this.btnStage.Enabled = count;
            this.btnSendToQuote.Enabled = count;
            this.mniPlace.Enabled = this.btnPlace.Enabled;
            this.mniStage.Enabled = this.btnStage.Enabled;
            this.mniSendToQuote.Enabled = this.btnSendToQuote.Enabled;
            this.mniEditAlert.Enabled = this.lvAlerts.SelectedItems.Count == 1;
            this.btnSendToQuote.Visible = MainModule.Instance.AuthProvider.AllowStreaming;
            this.mniSendToQuote.Visible = this.btnSendToQuote.Visible;
        }

        private void lvStrategies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lvStrategies.SelectedItems.Count == 0)
            {
                this.btnActivate.Checked = false;
                this.mniActivate.Checked = false;
                this.btnActivate.Enabled = false;
                this.mniActivate.Enabled = false;
                this.btnAutoStage.Enabled = false;
                this.mniGenerateOrders.Enabled = false;
                this.btnEmailAlert.Enabled = false;
                this.mniEmailAlerts.Enabled = false;
                this.btnSettings.Enabled = false;
                this.mniChangeSettings.Enabled = false;
                this.mniOpen.Enabled = false;
                this.mniRunNow.Enabled = false;
                this.mniRemoveStrategy.Enabled = false;
                this.btnRemove.Enabled = false;
                this.lvAlerts.Items.Clear();
            }
            else
            {
                StrategyCenterItem itemSelected = this.ItemSelected;
                this.btnSettings.Enabled = true;
                this.mniChangeSettings.Enabled = true;
                this.btnRemove.Enabled = true;
                this.ShowAutoTradingState(MainModule.Instance.AutoTradingEnabled);
                ListViewItem item2 = this.lvStrategies.SelectedItems[0];
                this.btnActivate.Enabled = true;
                this.mniActivate.Enabled = true;
                this.btnActivate.Checked = item2.ImageIndex >= 0;
                this.mniActivate.Checked = this.btnActivate.Checked;
                this.btnAutoStage.Enabled = this.btnActivate.Enabled;
                this.mniGenerateOrders.Enabled = this.btnAutoStage.Enabled;
                this.btnAutoStage.Checked = itemSelected.AutoStage;
                this.mniGenerateOrders.Checked = itemSelected.AutoStage;
                if (MainModule.Instance.EmailSettingsAvailable)
                {
                    this.btnEmailAlert.Enabled = true;
                    this.mniEmailAlerts.Enabled = true;
                }
                else
                {
                    this.btnEmailAlert.ToolTipText = "Please add Email Settings in Preferences to enable this feature.";
                }
                if (MainModule.Instance.EmailSettingsAvailable)
                {
                    this.btnEmailAlert.Checked = itemSelected.EmailAlerts;
                    this.mniEmailAlerts.Checked = itemSelected.EmailAlerts;
                }
                this.mniOpen.Enabled = true;
                this.mniRunNow.Enabled = true;
                this.mniRemoveStrategy.Enabled = true;
                this.mniItemLogging.Enabled = this.lvStrategies.SelectedItems.Count > 0;
                this.mniDisableLogging.Enabled = this.mniItemLogging.Enabled;
                this.mniDisableLogging.Enabled = this.lvStrategies.SelectedItems.Count == 1;
                this.method_25();
            }
        }

        private void method_0()
        {
            this.statusStrategies.Text = this.lvStrategies.Items.Count + " Strategies";
            int num = 0;
            int num2 = 0;
            foreach (ListViewItem item2 in this.lvStrategies.Items)
            {
                if (item2.ImageIndex >= 0)
                {
                    num++;
                }
                StrategyCenterItem tag = (StrategyCenterItem) item2.Tag;
                num2 += tag.AlertsList.Count;
            }
            this.statusActive.Text = num + " Active";
            int num3 = 0;
            lock (this.list_0)
            {
                foreach (StrategyCenterExecutionItem item in this.list_0)
                {
                    num3 += item.SymbolsProcessing.Count;
                }
            }
            this.statusSymbols.Text = "Symbols pending updates: " + num3;
            this.statusAlerts.Text = num2 + " Alerts";
        }

        private bool method_1()
        {
            StrategyExplorerForm form = new StrategyExplorerForm {
                ToolBarVisible = false,
                MultiSelect = false,
                Text = "Select a Strategy to Add"
            };
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                {
                    using (IEnumerator<Strategy> enumerator = form.StrategiesSelected.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Strategy current = enumerator.Current;
                            if (current.StrategyType == StrategyType.CombinedStrategy)
                            {
                                ///goto  Label_006E;  ///WYJ fix, simplify the flow
                                MessageBox.Show("Combination Strategies can not be added to the Strategy Monitor", Application.ProductName);
                                return false;
                            }
                            this.AddStrategyToStrategyCenter(current);
                        }
                        break;
                    }
                }
                case DialogResult.Cancel:
                    return false;
            }
            return true;
        }

        private void method_10(object object_1)
        {
            StrategyCenterExecutionItem item = (StrategyCenterExecutionItem) object_1;
            StrategyCenterItem item2 = item.Item;
            this.method_6(item2, "Entering Intraday Thread");
            try
            {
                item2.SCEI = item;
                int num = 1;
                while (item2.IsPopulating)
                {
                    this.method_6(item2, "Waiting for Populate to complete... (" + num + ")");
                    num++;
                    Thread.Sleep(0x1388);
                }
                Thread.Sleep(0x1388);
                DateTime maxValue = DateTime.MaxValue;
                foreach (Bars bars in item2.BarsList)
                {
                    if (bars.Date[bars.Count - 1] < maxValue)
                    {
                        maxValue = bars.Date[bars.Count - 1];
                    }
                }
                maxValue = maxValue.AddMinutes((double) (-item2.BarInterval * 2));
                item2.Log("Earliest End Date is " + maxValue.ToString());
                item.SymbolsProcessing.Clear();
                foreach (Bars bars2 in item2.BarsList)
                {
                    if (bars2.Symbol == null)
                    {
                        this.method_6(item2, "Null Symbol");
                    }
                    item.SymbolsProcessing.Add(bars2.Symbol);
                }
                item2.NewTradeCount = 0;
                try
                {
                    this.method_6(item2, "Requesting Updates for " + item.SymbolsNeedingProcessing);
                    item2.DataSet.Provider.RequestUpdates(item.SymbolsProcessingCopied, maxValue, item2.NextRun, item2.Scale, item2.BarInterval, item2);
                }
                catch (Exception exception)
                {
                    this.method_6(item2, "Error(6): " + exception.Message);
                }
            }
            catch (Exception exception2)
            {
                this.method_6(item2, "Error(7): " + exception2.Message);
            }
            item2.ThreadUpdate = null;
            item2.Log("Leaving Intraday Thread");
        }

        private void method_11(object object_1)
        {
            StrategyCenterExecutionItem item = (StrategyCenterExecutionItem) object_1;
            StrategyCenterItem item2 = item.Item;
            item2.Log("Entering 2nd chance Intraday Thread");
            try
            {
                string str = "Update2: ";
                foreach (Bars bars in item2.BarsList)
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, bars.Symbol, "=", bars.Count, ", " });
                }
                this.method_6(item2, str);
                item2.SCEI = item;
                DateTime maxValue = DateTime.MaxValue;
                foreach (Bars bars2 in item2.BarsList)
                {
                    if (bars2.Date[bars2.Count - 1] < maxValue)
                    {
                        maxValue = bars2.Date[bars2.Count - 1];
                    }
                }
                maxValue = maxValue.AddMinutes((double) -item2.BarInterval);
                item2.Log("Earliest end date = " + maxValue.ToString());
                try
                {
                    this.method_6(item2, "Requesting Updates(2) for " + item.SymbolsNeedingProcessing);
                    item2.DataSet.Provider.RequestUpdates(item.SymbolsProcessingCopied, maxValue, item2.NextRun, item2.Scale, item2.BarInterval, item2);
                }
                catch (Exception exception)
                {
                    this.method_6(item2, "Error(8): " + exception.Message);
                }
            }
            catch (Exception exception2)
            {
                this.method_6(item2, "Error(9): " + exception2.Message);
            }
            item2.ThreadUpdate = null;
            item2.Log("Leaving 2nd chance Intraday Thread");
        }

        internal void method_12(StrategyCenterItem strategyCenterItem_0)
        {
            strategyCenterItem_0.Log("Processing Completed");
            if (strategyCenterItem_0.Activated)
            {
                this.method_16(strategyCenterItem_0);
            }
        }

        internal void method_13(StrategyCenterItem strategyCenterItem_0, Bars bars_0)
        {
            if (strategyCenterItem_0.Activated)
            {
                try
                {
                    string str = "N/A";
                    if (bars_0.Count > 0)
                    {
                        int num = bars_0.Count - 1;
                        str = bars_0.Date[num].ToString();
                        string str2 = "";
                        if ((bars_0.Tag != null) && (bars_0.Tag is string))
                        {
                            str2 = ", " + bars_0.Tag;
                        }
                        this.method_6(strategyCenterItem_0, string.Concat(new object[] { 
                            "Update Completed: ", bars_0.Symbol, ", LastDate=", str, ", O=", bars_0.Open[num], " H=", bars_0.High[num], " L=", bars_0.Low[num], " C=", bars_0.Close[num], " V=", bars_0.Volume[num], " Count=", bars_0.Count, 
                            str2
                         }));
                    }
                    else
                    {
                        this.method_6(strategyCenterItem_0, "Zero Count Update: " + bars_0.Symbol);
                    }
                    if (!strategyCenterItem_0.UsingStreamingFilters)
                    {
                        int num2 = 0;
                        while (bars_0.Count > 0)
                        {
                            if (bars_0.Date[bars_0.Count - 1] <= strategyCenterItem_0.NextRun)
                            {
                                break;
                            }
                            num2++;
                            bars_0.Delete(bars_0.Count - 1);
                        }
                        if (num2 > 0)
                        {
                            this.method_6(strategyCenterItem_0, string.Concat(new object[] { "Deleted ", num2, ": ", bars_0.Symbol }));
                        }
                    }
                    if ((bars_0.Count > 0) && bars_0.IsLastCompressedBarPartial)
                    {
                        strategyCenterItem_0.Log("Removing Partial Bar: " + strategyCenterItem_0.Symbol);
                        bars_0.Delete(bars_0.Count - 1);
                    }
                    bool flag2 = (bars_0.Count == 0) || (bars_0.Date[bars_0.Count - 1] < strategyCenterItem_0.NextRun);
                    if (strategyCenterItem_0.RunOnce)
                    {
                        flag2 = false;
                    }
                    if (flag2)
                    {
                        this.method_6(strategyCenterItem_0, string.Concat(new object[] { "Skip Update (not current): ", bars_0.Symbol, " (", bars_0.Count, ")" }));
                        strategyCenterItem_0.Log("Bar count = " + bars_0.Count);
                        if (bars_0.Count > 0)
                        {
                            strategyCenterItem_0.Log("Last Date = " + bars_0.Date[bars_0.Count - 1].ToString() + " Next Run = " + strategyCenterItem_0.NextRun.ToString());
                        }
                        lock (strategyCenterItem_0.SCEI)
                        {
                            StrategyCenterExecutionItem sCEI = strategyCenterItem_0.SCEI;
                            sCEI.UpdateCount--;
                        }
                        return;
                    }
                    this.method_6(strategyCenterItem_0, "Applying Update");
                    Bars bars = null;
                    if (strategyCenterItem_0.UsingStreamingFilters)
                    {
                        ///goto  Label_03FA;  ///WYJ fix, simplify the flow
                        bars = bars_0;
                        //goto  Label_03FD;
                    }
                    else
                    {
                        using (List<Bars>.Enumerator enumerator = strategyCenterItem_0.BarsList.GetEnumerator())
                        {
                            Bars current;
                            while (enumerator.MoveNext())
                            {
                                current = enumerator.Current;
                                if (current.Symbol == bars_0.Symbol)
                                {
                                    ///goto  Label_03BA;  ///WYJ fix, simplify the flow
                                    bars = current;
                                    break;
                                }
                            }
                        }
                        if (bars == null)
                        {
                            return;
                        }
                        this.method_6(strategyCenterItem_0, "Append: " + bars.Symbol);
                        bars.Append(bars_0);
                    }
                Label_03FD:
                    if (!strategyCenterItem_0.IsPopulating)
                    {
                        this.method_6(strategyCenterItem_0, "Clear Indicators");
                        bars.Cache.Clear();
                        strategyCenterItem_0.Log("Creating Executor");
                        TradingSystemExecutor executor = new TradingSystemExecutor();
                        executor.LookupStrategy += new EventHandler<StrategyEventArgs>(this.method_33);
                        BarsLoader loader = new BarsLoader {
                            DataHost = MainModule.Instance.DataSources,
                            OverrideOnDemand = true,
                            OverrideOnDemandValue = true,
                            BarDataScale = strategyCenterItem_0.DataScale
                        };
                        strategyCenterItem_0.DataRange.ConfigureBarsLoader(loader);
                        executor.ApplySettings(MainModule.Instance.Executor);
                        executor.PosSize = strategyCenterItem_0.PositionSize;
                        executor.BarsLoader = loader;
                        executor.BuildEquityCurves = true;
                        executor.FundamentalsLoader = this.fundamentalsLoader_0;
                        executor.ExternalSymbolRequested += new EventHandler<LoadSymbolEventArgs>(this.method_30);
                        executor.ExternalSymbolFromDataSetRequested += new EventHandler<LoadSymbolFromDataSetEventArgs>(this.method_31);
                        this.fundamentalsLoader_0.DataHost = MainModule.Instance.DataSources;
                        executor.IsStreaming = true;
                        executor.Tag = strategyCenterItem_0;
                        executor.SetParameterValues += new EventHandler<StrategyParameterEventArgs>(this.method_14);
                        executor.DataSet = strategyCenterItem_0.DataSet;
                        executor.StrategyName = strategyCenterItem_0.Strategy.Name;
                        try
                        {
                            if (strategyCenterItem_0.ShouldExecute(bars))
                            {
                                this.method_6(strategyCenterItem_0, "Execute: " + bars.Symbol + " " + strategyCenterItem_0.Strategy.Name);
                                WealthScript wealthScriptObject = MainModule.Instance.Strategies.GetWealthScriptObject(strategyCenterItem_0.Strategy);
                                executor.Execute(strategyCenterItem_0.Strategy, wealthScriptObject, bars);
                                strategyCenterItem_0.HasRun = true;
                            }
                            else
                            {
                                this.method_6(strategyCenterItem_0, "Don't execute: " + bars.Symbol);
                            }
                        }
                        catch (Exception exception2)
                        {
                            this.method_6(strategyCenterItem_0, "Execute Error " + bars.Symbol + ": " + exception2.Message);
                        }
                        executor.LookupStrategy -= new EventHandler<StrategyEventArgs>(this.method_33);
                        lock (strategyCenterItem_0)
                        {
                            this.method_6(strategyCenterItem_0, string.Concat(new object[] { "Build Results: ", bars.Symbol, " Alerts: ", executor.Performance.Results.Alerts.Count }));
                            foreach (Alert alert in executor.Performance.Results.Alerts)
                            {
                                strategyCenterItem_0.Log(alert.ToString());
                            }
                            strategyCenterItem_0.NewTradeCount += executor.Performance.Results.Positions.Count;
                            this.method_29(executor.Performance.Results.Alerts, strategyCenterItem_0);
                            try
                            {
                                if (((executor.Performance.Results.Alerts.Count == 0) && strategyCenterItem_0.AutoStage) && MainModule.Instance.ShouldOrderBePlaced(strategyCenterItem_0.AccountNumber))
                                {
                                    MainModule.Instance.TradeManager.CancelStrategyOrders(strategyCenterItem_0.AccountNumber, strategyCenterItem_0.Strategy, bars.Symbol, strategyCenterItem_0.DataScale);
                                }
                            }
                            catch (Exception exception)
                            {
                                this.method_6(strategyCenterItem_0, "Error(T2): " + exception.Message);
                            }
                            strategyCenterItem_0.ReplaceAlerts(bars.Symbol, executor.Performance.Results.Alerts);
                            this.method_6(strategyCenterItem_0, string.Concat(new object[] { "Replace Alerts: ", bars.Symbol, " Alerts: ", strategyCenterItem_0.AlertsList.Count }));
                        }
                        this.method_27(executor);
                        StrategyCenterExecutionItem item = strategyCenterItem_0.SCEI;
                        lock (item)
                        {
                            item.UpdateCount--;
                            item.SymbolsProcessing.Remove(bars_0.Symbol);
                        }
                        int num3 = item.UpdateCount - strategyCenterItem_0.BadSymbolCount;
                        this.method_6(strategyCenterItem_0, string.Concat(new object[] { "Update Completed(2): ", bars_0.Symbol, " (", num3, " symbols left)" }));
                    }
                    else
                    {
                        strategyCenterItem_0.Log("Still populating, avoid execution: " + strategyCenterItem_0.Symbol);
                    }
                }
                catch (Exception exception3)
                {
                    this.method_6(strategyCenterItem_0, "Error(5): " + exception3.Message);
                }
                strategyCenterItem_0.Log("Leaving Update Completed");
            }
        }

        private void method_14(object sender, StrategyParameterEventArgs e)
        {
            TradingSystemExecutor executor = sender as TradingSystemExecutor;
            StrategyCenterItem tag = executor.Tag as StrategyCenterItem;
            if (tag.UsePreferredValues)
            {
                tag.Strategy.LoadPreferredValues(e.Symbol, e.WealthScript);
            }
        }

        internal void method_15(StrategyCenterItem strategyCenterItem_0, string string_1, Exception exception_0)
        {
            strategyCenterItem_0.Log("Update Error: " + string_1 + " " + exception_0.Message);
            StrategyCenterExecutionItem sCEI = strategyCenterItem_0.SCEI;
            lock (sCEI)
            {
                sCEI.UpdateCount--;
                sCEI.SymbolsProcessing.Remove(string_1);
            }
            if (sCEI.UpdateCount == 0)
            {
                this.method_16(strategyCenterItem_0);
            }
        }

        private void method_16(StrategyCenterItem strategyCenterItem_0)
        {
            this.method_6(strategyCenterItem_0, "All Updates Returned");
            StrategyCenterExecutionItem sCEI = strategyCenterItem_0.SCEI;
            if ((sCEI.SymbolsProcessing.Count > 0) && !strategyCenterItem_0.UsingStreamingFilters)
            {
                strategyCenterItem_0.Log("Still need to update " + sCEI.SymbolsNeedingProcessing);
                DateTime time = this.marketHours_0.ConvertLocalTimeToNative(DateTime.Now);
                DateTime time4 = strategyCenterItem_0.NextRun.AddMinutes((double) strategyCenterItem_0.BarInterval).AddSeconds(-20.0);
                if (time < time4)
                {
                    sCEI.UpdateCount = sCEI.SymbolsProcessing.Count;
                    string str = "Update2: ";
                    foreach (Bars bars in strategyCenterItem_0.BarsList)
                    {
                        object obj2 = str;
                        str = string.Concat(new object[] { obj2, bars.Symbol, "=", bars.Count, ", " });
                    }
                    this.method_6(strategyCenterItem_0, str);
                    Thread.Sleep(0x2710);
                    Thread thread = new Thread(new ParameterizedThreadStart(this.method_11));
                    strategyCenterItem_0.ThreadUpdate = thread;
                    thread.Name = "SM SecondChance";
                    thread.IsBackground = true;
                    thread.Start(sCEI);
                    return;
                }
                this.method_6(strategyCenterItem_0, "Time's up!");
            }
            base.Invoke(new Delegate31(this.method_17), new object[] { strategyCenterItem_0 });
        }

        private void method_17(StrategyCenterItem strategyCenterItem_0)
        {
            strategyCenterItem_0.Log("Item Processing Completed");
            StrategyCenterExecutionItem sCEI = strategyCenterItem_0.SCEI;
            if (!this.list_0.Contains(sCEI))
            {
                strategyCenterItem_0.Log("Avoid processing twice, returning");
                strategyCenterItem_0.LastRun = sCEI.NextRun;
                strategyCenterItem_0.CalculateNextRun(false);
                sCEI.SymbolsProcessing.Clear();
                strategyCenterItem_0.ListViewItem.ForeColor = Color.Blue;
                strategyCenterItem_0.Refresh();
            }
            strategyCenterItem_0.Trades = strategyCenterItem_0.NewTradeCount;
            lock (this.list_0)
            {
                strategyCenterItem_0.LastRun = sCEI.NextRun;
                strategyCenterItem_0.CalculateNextRun(false);
                this.list_0.Remove(sCEI);
                sCEI.BarsList.Clear();
            }
            this.method_21();
            strategyCenterItem_0.Log("Refreshing Item in GUI");
            if (Instance != null)
            {
                base.Invoke(new Delegate32(this.method_23), new object[] { strategyCenterItem_0 });
            }
            if (sCEI.SymbolsProcessing.Count > 0)
            {
                sCEI.SymbolsProcessing.Clear();
                strategyCenterItem_0.ListViewItem.ForeColor = Color.Blue;
                strategyCenterItem_0.CalculateNextRun(false);
                strategyCenterItem_0.Refresh();
            }
        }

        private void method_18(object object_1)
        {
            DateTime native;
            DateTime dateTime;
            Bars data;
            int num;
            StrategyCenterExecutionItem object1 = (StrategyCenterExecutionItem)object_1;
            StrategyCenterItem item = object1.Item;
            try
            {
                item.Log("Thread Execute");
                Thread.Sleep(5000);
                BarsLoader barsLoader = new BarsLoader();
                barsLoader.DataHost = MainModule.Instance.DataSources;
                barsLoader.BarDataScale = item.DataScale;
                barsLoader.OverrideOnDemand = true;
                barsLoader.OverrideOnDemandValue = true;
                item.DataRange.ConfigureBarsLoader(barsLoader);
                bool flag = true;
                BarDataScale dataScale = item.DataScale;
                if (!dataScale.IsIntraday)
                {
                    DateTime nextRun = object1.NextRun;
                    dateTime = nextRun.AddMinutes(5);
                }
                else
                {
                    if (item.BarInterval != 1)
                    {
                        num = (item.BarInterval > 5 ? 90 : 60);
                    }
                    else
                    {
                        num = 30;
                    }
                    DateTime nextRun1 = object1.NextRun;
                    dateTime = nextRun1.AddSeconds((double)num);
                }
                while (true)
                {
                    if (!flag)
                    {
                        Thread.Sleep(10000);
                    }
                    else
                    {
                        flag = false;
                    }
                    native = this.marketHours_0.ConvertLocalTimeToNative(DateTime.Now);
                    DataSource dataSource = null;
                    StaticDataProvider providerInstance = MainModule.Instance.DataSources.GetProviderInstance(item.DataSet.Provider.GetType());
                    if (item.DataSet.Provider.SupportsDataSourceUpdate)
                    {
                        this.method_6(item, string.Concat("Performing batch update for ", object1.SymbolsNeedingProcessing));
                        dataSource = new DataSource(providerInstance);
                        dataSource.BarDataScale = item.DataScale;
                        if (item.Symbol == "")
                        {
                            foreach (string symbolsProcessingCopied in object1.SymbolsProcessingCopied)
                            {
                                dataSource.Symbols.Add(symbolsProcessingCopied);
                            }
                        }
                        else
                        {
                            dataSource.Symbols.Add(item.Symbol);
                        }
                        dataSource.DSString = item.DataSet.DSString;
                        barsLoader.OverrideOnDemandValue = false;
                        try
                        {
                            providerInstance.Initialize(barsLoader);
                            item.Log("Calling UpdateDataSource");
                            providerInstance.UpdateDataSource(dataSource, item);
                        }
                        catch (Exception exception1)
                        {
                            Exception exception = exception1;
                            item.Log(string.Concat("Error: ", exception.Message));
                        }
                    }
                    if (dataSource != null)
                    {
                        dataSource.Provider = item.DataSet.Provider;
                    }
                    else
                    {
                        dataSource = item.DataSet;
                    }
                    for (int i = object1.SymbolsProcessing.Count - 1; i >= 0; i--)
                    {
                        string str = object1.SymbolsProcessing[i];
                        try
                        {
                            data = barsLoader.GetData(dataSource, str);
                        }
                        catch
                        {
                            data = new Bars(str, item.Scale, item.BarInterval);
                        }
                        while (data.Count > 0)
                        {
                            if (data.Date[data.Count - 1] <= object1.NextRun)
                            {
                                break;
                            }
                            data.Delete(data.Count - 1);
                        }
                        DateTime minValue = DateTime.MinValue;
                        if (data.Count > 0)
                        {
                            minValue = data.Date[data.Count - 1];
                        }
                        DateTime date = object1.NextRun;
                        BarDataScale barDataScale = item.DataScale;
                        if (!barDataScale.IsIntraday)
                        {
                            date = date.Date;
                        }
                        BarDataScale dataScale1 = item.DataScale;
                        if (!dataScale1.IsIntraday && !this.marketHours_0.AfterMarketCloseNow)
                        {
                            date = date.AddDays(-1);
                            while (!this.marketHours_0.IsTradingDay(date))
                            {
                                date = date.AddDays(-1);
                            }
                        }
                        if (data.Count > 0 && minValue >= date || item.RunOnce)
                        {
                            object1.BarsList.Add(data);
                            object1.SymbolsProcessing.Remove(str);
                            item.Log(string.Concat(str, " is current"));
                        }
                    }
                    if (object1.SymbolsProcessing.Count <= 0)
                    {
                        break;
                    }
                    if (native >= dateTime)
                    {
                        break;
                    }
                }
                if (object1.SymbolsProcessingCopied.Count > 0)
                {
                    item.Log(string.Concat("Symbols not updated in time: ", object1.SymbolsNeedingProcessing));
                }
                if (object1.SymbolsProcessing.Count == 0 || native > dateTime)
                {
                    this.method_19(item);
                    lock (this.list_0)
                    {
                        if (item.LastRun < object1.NextRun)
                        {
                            item.LastRun = object1.NextRun;
                        }
                        item.CalculateNextRun(false);
                        this.list_0.Remove(object1);
                        object1.BarsList.Clear();
                    }
                    if (StrategyCenterForm.Instance != null)
                    {
                        object[] objArray = new object[] { item };
                        base.Invoke(new StrategyCenterForm.Delegate32(this.method_23), objArray);
                    }
                }
                this.method_21();
            }
            catch (Exception exception3)
            {
                Exception exception2 = exception3;
                this.method_6(item, string.Concat("Error(99): ", exception2.Message));
            }
            item.Log("Leaving Thread Execute");
            this.method_6(item, "");
        }

        private void method_19(StrategyCenterItem strategyCenterItem_0)
        {
            StrategyCenterExecutionItem sCEI = strategyCenterItem_0.SCEI;
            strategyCenterItem_0.Log("Creating Executor");
            TradingSystemExecutor executor = new TradingSystemExecutor();
            executor.LookupStrategy += new EventHandler<StrategyEventArgs>(this.method_33);
            executor.ApplySettings(MainModule.Instance.Executor);
            executor.PosSize = strategyCenterItem_0.PositionSize;
            executor.BarsLoader = this.barsLoader_0;
            executor.BuildEquityCurves = true;
            executor.FundamentalsLoader = this.fundamentalsLoader_0;
            this.fundamentalsLoader_0.DataHost = MainModule.Instance.DataSources;
            executor.IsStreaming = true;
            executor.Tag = strategyCenterItem_0;
            executor.SetParameterValues += new EventHandler<StrategyParameterEventArgs>(this.method_14);
            strategyCenterItem_0.AlertsList.Clear();
            strategyCenterItem_0.Trades = 0;
            executor.DataSet = strategyCenterItem_0.DataSet;
            executor.StrategyName = strategyCenterItem_0.Strategy.Name;
            sCEI.BarsList.Sort(this);
            if (strategyCenterItem_0.RunOnce && (sCEI.BarsList.Count == 0))
            {
                foreach (Bars bars2 in strategyCenterItem_0.BarsList)
                {
                    sCEI.BarsList.Add(bars2);
                }
            }
            using (List<Bars>.Enumerator enumerator = sCEI.BarsList.GetEnumerator())
            {
            //Label_0135:
                while (enumerator.MoveNext())
                {
                    Bars current = enumerator.Current;
                    try
                    {
                        if (strategyCenterItem_0.ShouldExecute(current))
                        {
                            this.method_6(strategyCenterItem_0, "Executing: " + current.Symbol);
                            executor.Execute(strategyCenterItem_0.Strategy, strategyCenterItem_0.WealthScript, current);
                            strategyCenterItem_0.HasRun = true;
                        }
                    }
                    catch (Exception exception2)
                    {
                        this.method_6(strategyCenterItem_0, "Error(4): " + exception2.Message);
                    }
                    ///goto  Label_022E; ///WYJ fix, simplify the flow
                    executor.LookupStrategy -= new EventHandler<StrategyEventArgs>(this.method_33);
                    strategyCenterItem_0.Trades += executor.Performance.Results.Positions.Count;
                    this.method_29(executor.Performance.Results.Alerts, strategyCenterItem_0);
                    //goto  Label_01AB;
                    //Label_01AB:
                    try
                    {
                        if (((executor.Performance.Results.Alerts.Count == 0) && strategyCenterItem_0.AutoStage) && MainModule.Instance.ShouldOrderBePlaced(strategyCenterItem_0.AccountNumber))
                        {
                            MainModule.Instance.TradeManager.CancelStrategyOrders(strategyCenterItem_0.AccountNumber, strategyCenterItem_0.Strategy, current.Symbol, strategyCenterItem_0.DataScale);
                        }
                    }
                    catch (Exception exception)
                    {
                        this.method_6(strategyCenterItem_0, "Error(T3): " + exception.Message);
                    }
                    this.method_27(executor);
                }  //goto  Label_0135;
            }
        }

        private void method_2(object sender, EventArgs e)
        {
            this.method_1();
        }

        private void method_20(StrategyCenterItem strategyCenterItem_0)
        {
            if (strategyCenterItem_0 != null)
            {
                ChartForm dataSet = this.MyMainForm.FindStrategyFormByTag(strategyCenterItem_0.Strategy, this);
                if (dataSet == null)
                {
                    dataSet = this.MyMainForm.OpenStrategyWindow(strategyCenterItem_0.Strategy, false, false);
                    dataSet.Tag = this;
                }
                dataSet.BringToFront();
                dataSet.WindowState = FormWindowState.Normal;
                dataSet.DataSource = strategyCenterItem_0.DataSet;
                dataSet.Symbol = strategyCenterItem_0.Symbol;
                dataSet.DataRange = strategyCenterItem_0.DataRange;
                dataSet.PositionSize = strategyCenterItem_0.PositionSize;
                dataSet.BarDataScale = strategyCenterItem_0.DataScale;
                dataSet.SetBarDataScaleForDataSource(strategyCenterItem_0.DataSet, strategyCenterItem_0.DataScale);
                WealthScript wealthScript = dataSet.WealthScript;
                if (wealthScript != null && strategyCenterItem_0.WealthScript != null)
                {
                    for (int i = 0; i < wealthScript.Parameters.Count; i++)
                    {
                        wealthScript.Parameters[i].Value = strategyCenterItem_0.WealthScript.Parameters[i].Value;
                    }
                }
                this.MyMainForm.ActivateMdiChild();
                string symbol = strategyCenterItem_0.Symbol;
                if (symbol == "")
                {
                    IEnumerator enumerator = this.lvAlerts.SelectedItems.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            if (enumerator.MoveNext())
                            {
                                ListViewItem current = (ListViewItem)enumerator.Current;
                                Alert tag = (Alert)current.Tag;
                                if (strategyCenterItem_0.DataSet.Symbols.Contains(tag.Symbol))
                                {
                                    symbol = tag.Symbol;
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
                }
                this.MyMainForm.SelectTreeNode(strategyCenterItem_0.DataSet, symbol);
                return;
            }
            else
            {
                return;
            }
        }

        private void method_21()
        {
            lock (this.object_0)
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                List<StrategyCenterItem> o = new List<StrategyCenterItem>();
                foreach (ListViewItem item2 in this.lvStrategies.Items)
                {
                    StrategyCenterItem tag = (StrategyCenterItem) item2.Tag;
                    if (tag.Strategy != null)
                    {
                        tag.StrategyID = tag.Strategy.ID.ToString();
                    }
                    if (tag.DataSet != null)
                    {
                        tag.DataSourceName = tag.DataSet.Name;
                    }
                    tag.ParameterValues.Clear();
                    if (tag.WealthScript != null)
                    {
                        foreach (StrategyParameter parameter in tag.WealthScript.Parameters)
                        {
                            tag.ParameterValues.Add(parameter.Value);
                        }
                    }
                    o.Add(tag);
                }
                XmlSerializer serializer = new XmlSerializer(typeof(List<StrategyCenterItem>));
                TextWriter textWriter = new StreamWriter(this.string_0);
                serializer.Serialize(textWriter, o);
                textWriter.Close();
                Control.CheckForIllegalCrossThreadCalls = true;
            }
        }

        private void method_22()
        {
            this.lvStrategies.Items.Clear();
            this.lvAlerts.Items.Clear();
            if (File.Exists(this.string_0))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<StrategyCenterItem>));
                TextReader textReader = new StreamReader(this.string_0);
                List<StrategyCenterItem> list = (List<StrategyCenterItem>) serializer.Deserialize(textReader);
                textReader.Close();
                foreach (StrategyCenterItem item in list)
                {
                    ListViewItem item2;
                    item.Parent = this;
                    item.DataSet = MainModule.Instance.DataSources.FindDataSource(item.DataSourceName);
                    if (item.DataSet == null)
                    {
                        item.AlertsList.Clear();
                    }
                    item.method_0(this.marketHours_0);
                    item.Strategy = MainModule.Instance.Strategies.LookupID(item.StrategyID);
                    if ((item.AccountNumber == "") || (item.AccountNumber == null))
                    {
                        if (item.Strategy != null)
                        {
                            item.AccountNumber = item.Strategy.AccountNumber;
                        }
                        if (item.AccountNumber == "")
                        {
                            item.AccountNumber = MainModule.Instance.DefaultAccountNumber;
                        }
                    }
                    foreach (Alert alert in item.AlertsList)
                    {
                        alert.Strategy = item.Strategy;
                        alert.PosSize = item.PositionSize;
                        alert.DataRange = item.DataRange;
                        alert.Scale = item.Scale;
                        alert.BarInterval = item.BarInterval;
                        alert.DataSet = item.DataSet;
                        if ((alert.Account == "") || (alert.Account == null))
                        {
                            alert.Account = item.AccountNumber;
                        }
                    }
                    if (item.Strategy != null)
                    {
                        item.WealthScript = MainModule.Instance.Strategies.GetWealthScriptObject(item.Strategy);
                        if ((item.WealthScript != null) && (item.WealthScript.Parameters.Count == item.ParameterValues.Count))
                        {
                            for (int j = 0; j < item.ParameterValues.Count; j++)
                            {
                                item.WealthScript.Parameters[j].Value = item.ParameterValues[j];
                            }
                        }
                    }
                    if (item.Strategy == null)
                    {
                        item2 = this.lvStrategies.Items.Add("Error: Strategy not found");
                    }
                    else
                    {
                        item2 = this.lvStrategies.Items.Add(item.Strategy.Name);
                    }
                    item2.Tag = item;
                    item2.ImageIndex = -1;
                    item2.SubItems.Add(item.AccountNumber);
                    for (int i = 2; i < this.lvStrategies.Columns.Count; i++)
                    {
                        item2.SubItems.Add("");
                    }
                    item.CalculateNextRun(true);
                    item.ListViewItem = item2;
                    if (item.DataSet == null)
                    {
                        item.Activated = false;
                    }
                    if (item.Activated)
                    {
                        if (item.DataScale.IsIntraday && this.bool_1)
                        {
                            item.Activated = false;
                        }
                        else
                        {
                            this.method_3(item, true);
                        }
                    }
                    this.method_23(item);
                }
                this.method_0();
            }
        }

        private void method_23(StrategyCenterItem strategyCenterItem_0)
        {
            strategyCenterItem_0.Refresh();
            strategyCenterItem_0.ListViewItem.ForeColor = this.ForeColor;
            if (strategyCenterItem_0.AlertsList.Count > 0)
            {
                strategyCenterItem_0.ListViewItem.StateImageIndex = 0;
            }
            else
            {
                strategyCenterItem_0.ListViewItem.StateImageIndex = -1;
            }
            if (strategyCenterItem_0.ListViewItem.Selected)
            {
                this.lvStrategies_SelectedIndexChanged(this.lvStrategies, EventArgs.Empty);
            }
            else if (this.btnShowAllAlerts.Checked)
            {
                this.method_25();
            }
            if (strategyCenterItem_0.RunOnce)
            {
                strategyCenterItem_0.RunOnce = false;
                if (!strategyCenterItem_0.WasActivated)
                {
                    this.method_3(strategyCenterItem_0, false);
                }
            }
        }

        private void method_24(StrategyCenterItem strategyCenterItem_0)
        {
            strategyCenterItem_0.Refresh();
        }

        private void method_25()
        {
            this.lvAlerts.BeginUpdate();
            this.lvAlerts.Items.Clear();
            foreach (ListViewItem item in this.lvStrategies.Items)
            {
                if (item.Selected || this.btnShowAllAlerts.Checked)
                {
                    StrategyCenterItem tag = (StrategyCenterItem) item.Tag;
                    foreach (Alert alert in tag.AlertsList)
                    {
                        string text = alert.AlertDate.ToShortDateString();
                        if (alert.DataScale.IsIntraday)
                        {
                            text = text + " " + alert.AlertDate.ToShortTimeString();
                        }
                        ListViewItem item2 = this.lvAlerts.Items.Add(text);
                        item2.Tag = alert;
                        item2.ImageIndex = (int) alert.AlertType;
                        item2.SubItems.Add(alert.Symbol);
                        item2.SubItems[1].Tag = tag;
                        item2.SubItems.Add(alert.Account);
                        item2.SubItems.Add(alert.AlertType.ToString());
                        item2.SubItems.Add(alert.Shares.ToString());
                        item2.SubItems.Add(alert.OrderType.ToString());
                        if ((alert.OrderType != OrderType.Market) && (alert.OrderType != OrderType.AtClose))
                        {
                            item2.SubItems.Add(alert.Price.ToString("N" + DecimalsManager.Instance.GetPricingDecimalForSymbol(alert.Symbol)));
                        }
                        else
                        {
                            item2.SubItems.Add("");
                        }
                        item2.SubItems.Add(tag.Strategy.Name);
                        if (tag.TargetDataType == StrategyCenterItemDataType.Symbol)
                        {
                            item2.SubItems.Add(tag.Symbol);
                        }
                        else
                        {
                            item2.SubItems.Add("{" + tag.DataSet.Name + "}");
                        }
                        item2.SubItems.Add(tag.DataRange.Text);
                        BarDataScale scale2 = new BarDataScale(tag.Scale, tag.BarInterval);
                        item2.SubItems.Add(scale2.ToString());
                        item2.SubItems.Add(tag.PositionSize.Text);
                        item2.SubItems.Add(alert.SignalName);
                        if (string.IsNullOrEmpty(alert.AccountTradeType))
                        {
                            item2.SubItems.Add("");
                        }
                        else
                        {
                            item2.SubItems.Add(alert.AccountTradeType);
                        }
                    }
                }
            }
            this.lvAlerts.EndUpdate();
        }

        private void method_26(bool bool_6)
        {
            List<Alert> alerts = new List<Alert>();
            foreach (ListViewItem item in this.lvAlerts.SelectedItems)
            {
                bool flag;
                Alert tag = (Alert) item.Tag;
                if (!(flag = !bool_6 || MainModule.Instance.AuthProvider.LoggedIn) && tag.Account.StartsWith("Paper"))
                {
                    flag = true;
                }
                if (flag)
                {
                    alerts.Add(tag);
                }
            }
            MainModule.Instance.TradeManager.AddAlerts(alerts, bool_6, false);
        }

        private void method_27(TradingSystemExecutor tradingSystemExecutor_1)
        {
            base.Invoke(new Delegate33(this.method_28), new object[] { tradingSystemExecutor_1 });
        }

        private void method_28(TradingSystemExecutor tradingSystemExecutor_1)
        {
            if ((tradingSystemExecutor_1.Performance.Results.Alerts.Count > 0) && MainModule.Instance.Settings.Get("SoundsStrategyMonitor", true))
            {
                string defaultValue = MainModule.Instance.AppPath + @"\Data\Sounds\alert2.wav";
                string path = MainModule.Instance.Settings.Get("SoundsStrategyMonitor_File", defaultValue);
                if (!File.Exists(path))
                {
                    path = defaultValue;
                    MainModule.Instance.Settings.Set("SoundsStrategyMonitor_File", path);
                }
                MainModule.Instance.PlaySound(path);
            }
        }

        private void method_29(IList<Alert> ilist_0, StrategyCenterItem strategyCenterItem_0)
        {
            foreach (Alert alert2 in ilist_0)
            {
                alert2.Account = strategyCenterItem_0.AccountNumber;
                if (MainModule.Instance.AccountTradeTypes(strategyCenterItem_0.AccountNumber, alert2.AlertType.ToString()).Contains(strategyCenterItem_0.AccountTradeType))
                {
                    alert2.AccountTradeType = strategyCenterItem_0.AccountTradeType;
                }
                else
                {
                    alert2.AccountTradeType = MainModule.Instance.DefaultAccountTradeType(strategyCenterItem_0.AccountNumber, alert2.AlertType.ToString());
                }
                alert2.Strategy = strategyCenterItem_0.Strategy;
                alert2.PosSize = strategyCenterItem_0.PositionSize;
                alert2.DataRange = strategyCenterItem_0.DataRange;
                alert2.Scale = strategyCenterItem_0.Scale;
                alert2.BarInterval = strategyCenterItem_0.BarInterval;
                alert2.DataSet = strategyCenterItem_0.DataSet;
                strategyCenterItem_0.AlertsList.Add(alert2);
            }
            if (strategyCenterItem_0.AutoStage)
            {
                try
                {
                    List<Alert> alerts = new List<Alert>();
                    foreach (Alert alert4 in ilist_0)
                    {
                        if (!alert4.ProcessedBySM)
                        {
                            alert4.ProcessedBySM = true;
                            alerts.Add(alert4);
                        }
                        else
                        {
                            this.method_6(strategyCenterItem_0, "Bypassing Alert: " + alert4.Symbol);
                        }
                    }
                    if (alerts.Count > 0)
                    {
                        string str = "Adding " + alerts.Count + " Alerts: ";
                        foreach (Alert alert3 in alerts)
                        {
                            str = str + alert3.Symbol + ", ";
                        }
                        this.method_6(strategyCenterItem_0, str);
                        MainModule.Instance.TradeManager.AddAlerts(alerts, MainModule.Instance.ShouldOrderBePlaced(alerts[0]), true);
                    }
                }
                catch (Exception exception)
                {
                    this.method_6(strategyCenterItem_0, "Error(T1): " + exception.Message);
                }
            }
            if (strategyCenterItem_0.EmailAlerts)
            {
                List<Alert> list = new List<Alert>();
                foreach (Alert alert in ilist_0)
                {
                    if (!alert.EmailSent)
                    {
                        alert.EmailSent = true;
                        list.Add(alert);
                    }
                }
                if (list.Count > 0)
                {
                    MainModule.Instance.emailAlerts(list);
                }
            }
        }

        private bool method_3(StrategyCenterItem strategyCenterItem_0, bool bool_6)
        {
            if (bool_6 && (strategyCenterItem_0.DataSet == null))
            {
                this.btnSettings.PerformClick();
                if (strategyCenterItem_0.DataSet == null)
                {
                    return false;
                }
            }
            if ((bool_6 && strategyCenterItem_0.DataScale.IsIntraday) && (!MainModule.Instance.AuthProvider.LoggedIn && !MainModule.Instance.Authenticate()))
            {
                this.bool_1 = true;
                return false;
            }
            strategyCenterItem_0.Activated = bool_6;
            this.method_5(strategyCenterItem_0);
            if (!bool_6)
            {
                strategyCenterItem_0.ListViewItem.ForeColor = Color.Black;
                if (strategyCenterItem_0.ThreadPopulate != null)
                {
                    strategyCenterItem_0.Log("Aborting Populate Thread");
                    strategyCenterItem_0.ThreadPopulate.Abort();
                    strategyCenterItem_0.ThreadPopulate = null;
                    strategyCenterItem_0.BarsList.Clear();
                }
                if (strategyCenterItem_0.ThreadUpdate != null)
                {
                    strategyCenterItem_0.Log("Aborting Update Thread");
                    strategyCenterItem_0.ThreadUpdate.Abort();
                    strategyCenterItem_0.ThreadUpdate = null;
                    strategyCenterItem_0.BarsList.Clear();
                }
                lock (this.list_0)
                {
                    this.list_0.Remove(strategyCenterItem_0.SCEI);
                }
                if (strategyCenterItem_0.UsingStreamingFilters)
                {
                    strategyCenterItem_0.UnSubscribeToStreamingBars();
                }
            }
            else
            {
                if (!strategyCenterItem_0.RunOnce)
                {
                    strategyCenterItem_0.CalculateNextRun(true);
                    strategyCenterItem_0.Refresh();
                }
                bool flag2 = strategyCenterItem_0.DataScale.IsIntraday && ((strategyCenterItem_0.BarInterval < 0x3e8) || this.HaveStreamingFilters);
                if (strategyCenterItem_0.RunOnce)
                {
                    flag2 = true;
                }
                if (flag2)
                {
                    if (strategyCenterItem_0.BarsList.Count == 0)
                    {
                        strategyCenterItem_0.Log("Preparing for initial Data Population");
                        strategyCenterItem_0.IsPopulating = true;
                        StrategyCenterExecutionItem item = new StrategyCenterExecutionItem(strategyCenterItem_0, strategyCenterItem_0.NextRun);
                        strategyCenterItem_0.SCEI = item;
                        strategyCenterItem_0.AddedToExecutionList = true;
                        lock (this.list_0)
                        {
                            if (!this.method_4(strategyCenterItem_0))
                            {
                                this.list_0.Add(item);
                            }
                        }
                        Thread thread = new Thread(new ParameterizedThreadStart(this.method_8));
                        strategyCenterItem_0.ThreadPopulate = thread;
                        thread.Name = "SM Populate";
                        thread.IsBackground = true;
                        strategyCenterItem_0.Log("Starting Populate Thread");
                        thread.Start(item);
                    }
                    if (!strategyCenterItem_0.RunOnce)
                    {
                        StreamingDataProvider streamingProvider = MainModule.Instance.StreamingProvider;
                        if ((strategyCenterItem_0.Scale == BarScale.Minute) && streamingProvider.SupportsStreamingBars)
                        {
                            if (!streamingProvider.IsConnected)
                            {
                                streamingProvider.ConnectStreaming(MainModule.Instance);
                            }
                            DateTime time = DateTime.Now.AddSeconds(30.0);
                            while (DateTime.Now < time)
                            {
                                if (streamingProvider.IsConnected)
                                {
                                    break;
                                }
                                Application.DoEvents();
                            }
                            strategyCenterItem_0.SubscribeToStreamingBars();
                        }
                    }
                }
                if (!strategyCenterItem_0.RunOnce)
                {
                    strategyCenterItem_0.Refresh();
                }
            }
            this.method_0();
            return true;
        }

        private void method_30(object sender, LoadSymbolEventArgs e)
        {
            e.SymbolData = MainModule.Instance.LoadExternalSymbol(e.Symbol, e.Scale, e.BarInterval, false);
        }

        private void method_31(object sender, LoadSymbolFromDataSetEventArgs e)
        {
            e.Bars = MainModule.Instance.LoadExternalSymbol(e.DataSetName, e.Symbol);
        }

        private void method_32(bool bool_6)
        {
            foreach (ListViewItem item in this.lvStrategies.Items)
            {
                ((StrategyCenterItem) item.Tag).Refresh();
            }
        }

        private void method_33(object sender, StrategyEventArgs e)
        {
            e.Strategy = MainModule.Instance.Strategies.LookupID(e.StrategyID);
        }

        private bool method_4(StrategyCenterItem strategyCenterItem_0)
        {
            using (List<StrategyCenterExecutionItem>.Enumerator enumerator = this.list_0.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    StrategyCenterExecutionItem current = enumerator.Current;
                    if (current.Item == strategyCenterItem_0)
                    {
                        ///goto  Label_002D;  ///WYJ fix, simplify the flow
                        return true;
                    }
                }
                return false;
            }
        }

        private void method_5(StrategyCenterItem strategyCenterItem_0)
        {
            if (!strategyCenterItem_0.Activated)
            {
                strategyCenterItem_0.ListViewItem.ImageIndex = -1;
                this.lvStrategies.Refresh();
            }
            else if (strategyCenterItem_0.AutoStage)
            {
                strategyCenterItem_0.ListViewItem.ImageIndex = 1;
            }
            else
            {
                strategyCenterItem_0.ListViewItem.ImageIndex = 0;
            }
            if (strategyCenterItem_0.ListViewItem.Selected)
            {
                this.btnActivate.Checked = strategyCenterItem_0.Activated;
                this.mniActivate.Checked = this.btnActivate.Checked;
                this.btnAutoStage.Checked = strategyCenterItem_0.AutoStage;
                this.mniGenerateOrders.Checked = this.btnAutoStage.Checked;
                if (MainModule.Instance.EmailSettingsAvailable)
                {
                    this.btnEmailAlert.Checked = strategyCenterItem_0.EmailAlerts;
                    this.mniEmailAlerts.Checked = this.btnEmailAlert.Checked;
                }
            }
        }

        internal void method_6(StrategyCenterItem strategyCenterItem_0, string string_1)
        {
            strategyCenterItem_0.Log(string_1);
            try
            {
                base.Invoke(new Delegate30(this.method_7), new object[] { strategyCenterItem_0, string_1 });
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void method_7(StrategyCenterItem strategyCenterItem_0, string string_1)
        {
            strategyCenterItem_0.ListViewItem.SubItems[12].Text = string_1;
        }

        private void method_8(object object_1)
        {
            StrategyCenterExecutionItem item = (StrategyCenterExecutionItem) object_1;
            StrategyCenterItem item2 = item.Item;
            item2.Log("In Populate Thread");
            try
            {
                bool flag2;
                Bars data;
                List<StrategyCenterExecutionItem> list = null; ///WYJ fix
                this.method_6(item2, "Populating: " + item2.DataSet.Name);
                bool flag = true;
                StaticDataProvider providerInstance = MainModule.Instance.DataSources.GetProviderInstance(item2.DataSet.Provider.GetType());
                providerInstance.Initialize(MainModule.Instance.DataSources);
                if (((item2.DataRange.Range != BarRange.AllData) && item2.DataScale.IsIntraday) && (item2.BarInterval <= 0x3e8))
                {
                    item2.DataSet.Provider.IsStreamingRequest = true;
                }
                providerInstance.IsStreamingRequest = item2.DataSet.Provider.IsStreamingRequest;
                if (providerInstance.SupportsDataSourceUpdate)
                {
                    flag = false;
                    DataSource source = new DataSource(providerInstance) {
                        BarDataScale = item2.DataScale
                    };
                    if (item2.Symbol != "")
                    {
                        source.Symbols.Add(item2.Symbol);
                    }
                    else
                    {
                        foreach (string str in item2.DataSet.Symbols)
                        {
                            source.Symbols.Add(str);
                        }
                    }
                    if (item2.DataSet != null)
                    {
                        this.method_6(item2, "Updating: " + item2.DataSet.Name);
                    }
                    else
                    {
                        this.method_6(item2, "Updating: " + item2.Symbol);
                    }
                    providerInstance.UpdateDataSource(source, item2);
                }
                else
                {
                    item2.Log("Provider does not support Updates");
                }
                BarsLoader loader = new BarsLoader {
                    DataHost = MainModule.Instance.DataSources,
                    BarDataScale = item2.DataScale,
                    OverrideOnDemand = true,
                    OverrideOnDemandValue = flag
                };
                item2.DataRange.ConfigureBarsLoader(loader);
                if (item2.Symbol != "")
                {
                    try
                    {
                        item2.Log("Loading Data for " + item2.Symbol);
                        data = loader.GetData(item2.DataSet, item2.Symbol);
                        item2.Log(data.Count + " bars returned");
                    }
                    catch (Exception exception2)
                    {
                        this.method_6(item2, "Error(1): " + exception2.Message);
                        data = new Bars(item2.Symbol, item2.Scale, item2.BarInterval);
                    }
                    if (data.Count > 0)
                    {
                        item2.BarsList.Add(data);
                    }
                    else
                    {
                        item2.BadSymbolCount++;
                    }
                    lock (item)
                    {
                        item.SymbolsProcessing.Remove(item2.Symbol);
                        goto Label_03C9;
                    }
                }
                foreach (string str2 in item2.DataSet.Symbols)
                {
                    try
                    {
                        this.method_6(item2, "Loading Data for: " + str2);
                        data = loader.GetData(item2.DataSet, str2);
                        this.method_6(item2, data.Count + " bars returned");
                    }
                    catch (Exception exception)
                    {
                        this.method_6(item2, "Error(2): " + exception.Message);
                        data = new Bars(str2, item2.Scale, item2.BarInterval);
                    }
                    if (data.Count > 0)
                    {
                        item2.BarsList.Add(data);
                    }
                    else
                    {
                        item2.BadSymbolCount++;
                    }
                    lock (item)
                    {
                        item.SymbolsProcessing.Remove(str2);
                    }
                }
            Label_03C9:
                flag2 = false;
                try
                {
                    Monitor.Enter(list = this.list_0, ref flag2);
                    this.list_0.Remove(item);
                }
                finally
                {
                    if (flag2)
                    {
                        Monitor.Exit(list);
                    }
                }
                item2.IsPopulating = false;
                item2.ThreadPopulate = null;
                if (item2.RunOnce)
                {
                    item2.Activated = false;
                    base.Invoke(new ActivateItemCallback(this.method_3), new object[] { item2, false });
                    this.method_19(item2);
                    item2.Log("Run Once Processing Completed");
                    this.method_6(item2, "Run Now Ended");
                    item2.RunOnce = false;
                    item2.AddedToExecutionList = false;
                    item2.LastRun = item2.NextRun;
                    item2.BarsList.Clear();
                    if (Instance != null)
                    {
                        base.Invoke(new Delegate32(this.method_23), new object[] { item2 });
                    }
                }
                else
                {
                    item2.CalculateNextRun(true);
                }
                item2.Log("Leaving Populate Thread");
                base.Invoke(new Delegate32(this.method_24), new object[] { item2 });
            }
            catch (Exception exception3)
            {
                this.method_6(item2, "Error(3): " + exception3.Message);
                item2.ThreadPopulate = null;
                item2.IsPopulating = false;
                item2.Log("Leaving Update Thread from catch");
            }
        }

        private void method_9(object object_1)
        {
            StrategyCenterExecutionItem item = object_1 as StrategyCenterExecutionItem;
            StrategyCenterItem item2 = item.Item;
            this.method_6(item2, "Entering Monitor Thread");
            while (item2.IsPopulating)
            {
                this.method_6(item2, "Wait for Populate to complete... ");
                Thread.Sleep(0x3e8);
            }
            Thread.Sleep(0x1388);
            DateTime time2 = item2.NextRun.AddSeconds(50.0);
            while ((item.UpdateCount - item2.BadSymbolCount) > 0)
            {
                if (TimeZoneInformation.ToLocalTime(DateTime.Now.ToUniversalTime(), item2.MarketInfo.TimeZoneName) >= time2)
                {
                    break;
                }
                Thread.Sleep(0x3e8);
            }
            this.method_16(item2);
            this.method_6(item2, "Leaving Streaming Filter Monitor Thread");
        }

        private void mniCopyAlerts_Click(object sender, EventArgs e)
        {
            MainModule.Instance.CopyListViewToClipboard(this.lvAlerts);
        }

        private void mniCopyStrategies_Click(object sender, EventArgs e)
        {
            MainModule.Instance.CopyListViewToClipboard(this.lvStrategies);
        }

        private void mniDisableLogging_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvStrategies.SelectedItems)
            {
                StrategyCenterItem tag = item.Tag as StrategyCenterItem;
                tag.Logging = false;
            }
        }

        private void mniEditAlert_Click(object sender, EventArgs e)
        {
            if (this.lvAlerts.SelectedItems.Count == 1)
            {
                ListViewItem item = this.lvAlerts.SelectedItems[0];
                Alert tag = (Alert) item.Tag;
                EditAlertForm form = new EditAlertForm {
                    Alert = tag
                };
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    item.SubItems[1].Text = tag.Symbol;
                    item.SubItems[2].Text = tag.Account;
                    item.SubItems[3].Text = tag.AlertType.ToString();
                    item.SubItems[4].Text = tag.Shares.ToString();
                    item.SubItems[5].Text = tag.OrderType.ToString();
                    if ((tag.OrderType != OrderType.Limit) && (tag.OrderType != OrderType.Stop))
                    {
                        item.SubItems[6].Text = "";
                    }
                    else
                    {
                        item.SubItems[6].Text = tag.Price.ToString("N" + DecimalsManager.Instance.GetPricingDecimalForSymbol(tag.Symbol));
                    }
                    while (item.SubItems.Count < this.int_3)
                    {
                        item.SubItems.Add("");
                    }
                    item.SubItems[this.int_3].Text = tag.AccountTradeType;
                }
                this.lvAlerts_SelectedIndexChanged(item, EventArgs.Empty);
            }
        }

        private void mniItemLogging_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvStrategies.SelectedItems)
            {
                StrategyCenterItem tag = item.Tag as StrategyCenterItem;
                tag.Logging = true;
            }
        }

        private void mniOpen_Click(object sender, EventArgs e)
        {
            StrategyCenterItem itemSelected = this.ItemSelected;
            if (itemSelected != null)
            {
                this.method_20(itemSelected);
            }
        }

        private void mniPrint_Click(object sender, EventArgs e)
        {
            this.listView_0 = this.lvStrategies;
            this.Print();
            this.listView_0 = null;
        }

        private void mniPrintAlerts_Click(object sender, EventArgs e)
        {
            this.listView_0 = this.lvAlerts;
            this.Print();
            this.listView_0 = null;
        }

        private void mniRunNow_Click(object sender, EventArgs e)
        {
            StrategyCenterItem itemSelected = this.ItemSelected;
            if (itemSelected.Activated)
            {
                this.method_3(itemSelected, false);
                itemSelected.Activated = false;
            }
            if (((itemSelected != null) && !itemSelected.AddedToExecutionList) && !itemSelected.IsPopulating)
            {
                this.method_6(itemSelected, "Run Once beginning ...");
                itemSelected.Log("RunNow Processing Beginning");
                itemSelected.RunOnce = true;
                itemSelected.WasActivated = itemSelected.Activated;
                if (!itemSelected.Activated)
                {
                    this.method_3(itemSelected, true);
                }
                if (itemSelected.DataSet != null)
                {
                    this.marketHours_0.Market = itemSelected.DataSet.Provider.GetMarketInfo(itemSelected.Symbol);
                }
                itemSelected.NextRun = this.marketHours_0.ConvertLocalTimeToNative(DateTime.Now);
                itemSelected.Refresh();
            }
        }

        private void mniViewItemLog_Click(object sender, EventArgs e)
        {
            StrategyCenterItem tag = this.lvStrategies.SelectedItems[0].Tag as StrategyCenterItem;
            string path = MainModule.Instance.DataPath + @"\SCLog.txt";
            TextWriter writer = new StreamWriter(path);
            foreach (string str2 in tag.ItemLog)
            {
                writer.WriteLine(str2);
            }
            writer.Close();
            Process.Start(path);
        }

        public void Print()
        {
            PrintDocument prtdoc = new PrintDocument();
            prtdoc.BeginPrint += new PrintEventHandler(this.StrategyCenterReport_BeginPrint);
            prtdoc.PrintPage += new PrintPageEventHandler(this.StrategyCenterReport_PrintPage);
            prtdoc.EndPrint += new PrintEventHandler(this.StrategyCenterReport_EndPrint);
            prtdoc.DefaultPageSettings = this.MyMainForm.DefaultPageSettings;
            this.bool_5 = !MainModule.Instance.Settings.Get("HidePrintPreview", false);
            this.bool_4 = !MainModule.Instance.Settings.Get("HidePrintDialog", false);
            if (this.bool_5)
            {
                this.printPreview_0 = new PrintPreview(prtdoc);
                this.printPreview_0.ShowPrintDialog = this.bool_4;
                this.printPreview_0.ShowDialog();
            }
            else
            {
                PrintDialog dialog = new PrintDialog {
                    Document = prtdoc
                };
                if (this.bool_4)
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

        public void RemoveSelectedStrategy()
        {
            if (this.lvStrategies.SelectedItems.Count != 0)
            {
                this.lvStrategies.Items.Remove(this.lvStrategies.SelectedItems[0]);
                this.method_0();
            }
        }

        public int SaveWorkspaceItems(IList<string> items)
        {
            items.Add(this.btnShowAllAlerts.Checked.ToString());
            return 1;
        }

        public void ShowAutoTradingState(AutoTradingMode mode)
        {
            string str;
            AutoTradingMode live = AutoTradingMode.Live;
            if (this.lvStrategies.SelectedItems.Count == 1)
            {
                StrategyCenterItem tag = (StrategyCenterItem) this.lvStrategies.SelectedItems[0].Tag;
                if (tag.AccountNumber.StartsWith("PaperAccount"))
                {
                    live = AutoTradingMode.Paper;
                }
            }
            if (live == mode)
            {
                str = "Place";
            }
            else
            {
                str = "Stage";
            }
            this.btnAutoStage.Text = "Auto-" + str;
            this.mniGenerateOrders.Text = this.btnAutoStage.Text;
            this.btnAutoStage.ToolTipText = "Automatically " + str + " Orders from Strategy Alerts";
        }

        public static void ShowPosSizeWarning()
        {
            if (!MainModule.Instance.Settings.Get("DontShowStrategyCenterPosSizeWarning", false))
            {
                DontShowAgainForm form = new DontShowAgainForm("Position Size Setting", "You have selected a Portfolio Simulation position sizing mode.  This might lead to inconsistent trade alerts.  Raw Profit position size mode is recommended in the Strategy Monitor.");
                form.ShowDialog();
                MainModule.Instance.Settings.Set("DontShowStrategyCenterPosSizeWarning", form.DontShowAgain);
            }
        }

        private void StrategyCenterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.method_21();
            Instance = null;
        }

        private void StrategyCenterForm_Load(object sender, EventArgs e)
        {
            Instance = this;
            if (MainModule.Instance.Settings.Get("StrategyMonitor_ShowAllAlerts", false))
            {
                this.btnShowAllAlerts_Click(sender, e);
            }
            this.ShowAutoTradingState(MainModule.Instance.AutoTradingEnabled);
            if (!MainModule.Instance.AuthProvider.AllowStreaming)
            {
                this.btnSendToQuote.Visible = false;
                this.mniSendToQuote.Visible = false;
                this.sepQuote.Visible = false;
                this.sepQuotes.Visible = false;
            }
            this.bool_2 = MainModule.Instance.Settings.Get("ShowLocalTime", false);
            this.mniShowLocalTime.Checked = this.bool_2;
            this.btnShowLocalTime.Checked = this.bool_2;
        }

        public void StrategyCenterReport_BeginPrint(object sender, PrintEventArgs e)
        {
            this.printReport_0 = new PrintReport();
            this.printReport_0.printTitle = "Strategy Center";
            this.printReport_0.BasePrintTitle = MainModule.Instance.AuthProvider.ApplicationName;
            if (this.listView_0 == null)
            {
                this.printReport_0.printListView = this.lvStrategies;
                this.bool_3 = false;
            }
            else
            {
                this.printReport_0.printListView = this.listView_0;
                this.bool_3 = true;
            }
            this.printReport_0.lvRow = 0;
            if ((this.printPreview_0 != null) && this.printPreview_0.PrintSomePages)
            {
                this.printReport_0.printSomePages = this.printPreview_0.PrintSomePages;
                this.printReport_0.startPageCount = this.printPreview_0.FromPage;
                this.printReport_0.endPageCount = this.printPreview_0.ToPage;
            }
            this.printReport_0.intPageCounter = 1;
        }

        public void StrategyCenterReport_EndPrint(object sender, PrintEventArgs e)
        {
            this.printReport_0.intPageCounter--;
            this.printReport_0.endPageCount = this.printReport_0.intPageCounter;
            if (this.printPreview_0 != null)
            {
                this.printPreview_0.FromPage = 1;
                this.printPreview_0.ToPage = this.printReport_0.intPageCounter;
            }
        }

        public void StrategyCenterReport_PrintPage(object sender, PrintPageEventArgs e)
        {
            bool printPage = true;
            Rectangle destRect = new Rectangle(e.MarginBounds.X, e.MarginBounds.Y, e.MarginBounds.Width, e.MarginBounds.Height);
            float num = e.Graphics.MeasureString("Test", this.printReport_0.printFontBody).Height + 1f;
            if (this.printReport_0.printSomePages && (this.printReport_0.intPageCounter < this.printReport_0.startPageCount))
            {
                printPage = false;
            }
            else
            {
                this.printReport_0.PrintTitle(e, ref destRect);
                destRect.Y += ((int) num) * 2;
                destRect.Height -= ((int) num) * 2;
            }
            SizeF disclosureRect = this.printReport_0.GetDisclosureRect(e, ref destRect);
            if (this.printReport_0.fPrintListView)
            {
                this.printReport_0.PrintListView(e, ref destRect, printPage);
                if (!this.printReport_0.fPrintListView && !this.bool_3)
                {
                    this.printReport_0.printListView = this.lvAlerts;
                    this.printReport_0.lvRow = 0;
                    this.bool_3 = true;
                    destRect.Y += ((int) num) * 2;
                    destRect.Height -= ((int) num) * 2;
                    if (destRect.Height > (num * 2f))
                    {
                        this.printReport_0.PrintListView(e, ref destRect, printPage);
                    }
                }
            }
            this.printReport_0.PrintFooter(e, printPage, ref destRect, disclosureRect);
            e.HasMorePages = this.printReport_0.fPrintListView;
            this.printReport_0.intPageCounter++;
            if ((this.printReport_0.intPageCounter > this.printReport_0.endPageCount) && this.printReport_0.printSomePages)
            {
                e.HasMorePages = false;
            }
        }

        private void timer_0_Tick(object sender, EventArgs e)
        {
            foreach (ListViewItem item2 in this.lvStrategies.Items)
            {
                if (item2.ImageIndex >= 0)
                {
                    StrategyCenterItem tag = (StrategyCenterItem) item2.Tag;
                    if ((!tag.AddedToExecutionList && (tag.NextRun != DateTime.MaxValue)) && (tag.NextRun != DateTime.MinValue))
                    {
                        if ((tag.ListViewItem.ForeColor == Color.Orange) && !tag.IsPopulating)
                        {
                            tag.ListViewItem.ForeColor = Color.Black;
                        }
                        else if (tag.IsPopulating && (tag.ListViewItem.ForeColor != Color.Orange))
                        {
                            tag.ListViewItem.ForeColor = Color.Orange;
                        }
                        if (tag.DataSet != null)
                        {
                            this.marketHours_0.Market = tag.DataSet.Provider.GetMarketInfo(tag.Symbol);
                        }
                        if (this.marketHours_0.ConvertLocalTimeToNative(DateTime.Now) >= tag.NextRun)
                        {
                            if (tag.IsPopulating && !tag.RunOnce)
                            {
                                tag.NextRun = this.marketHours_0.GetNextTimeStamp(tag.NextRun, tag.DataScale);
                                tag.Refresh();
                            }
                            else
                            {
                                tag.AddedToExecutionList = true;
                                item2.ForeColor = Color.Red;
                                StrategyCenterExecutionItem item = new StrategyCenterExecutionItem(tag, tag.NextRun);
                                tag.SCEI = item;
                                lock (this.list_0)
                                {
                                    if (!this.method_4(tag))
                                    {
                                        this.list_0.Add(item);
                                    }
                                    if (!tag.RunOnce)
                                    {
                                        Thread thread = null;
                                        tag.UsingStreamingFilters = false;
                                        if ((tag.Scale == BarScale.Minute) && this.HaveStreamingFilters)
                                        {
                                            tag.NewTradeCount = 0;
                                            tag.UsingStreamingFilters = true;
                                            item.UpdateCount = item.SymbolsProcessing.Count;
                                            tag.SubscribeToStreamingBars();
                                            this.method_6(tag, "Launch Monitor Thread for Streaming Updates");
                                            tag.AlertsList.Clear();
                                            thread = new Thread(new ParameterizedThreadStart(this.method_9));
                                            tag.ThreadUpdate = thread;
                                            thread.Name = "SF Monitor";
                                        }
                                        else if (tag.DataScale.IsIntraday && ((tag.BarInterval < 0x3e8) || this.HaveStreamingFilters))
                                        {
                                            item.UpdateCount = item.SymbolsProcessing.Count;
                                            this.method_6(tag, "Launch Update(I)");
                                            thread = new Thread(new ParameterizedThreadStart(this.method_10));
                                            tag.ThreadUpdate = thread;
                                            thread.Name = "SM Intraday";
                                        }
                                        else
                                        {
                                            thread = new Thread(new ParameterizedThreadStart(this.method_18)) {
                                                Name = "SM Execute"
                                            };
                                        }
                                        if (thread != null)
                                        {
                                            thread.IsBackground = true;
                                            thread.Start(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.method_0();
        }

        public bool HaveStreamingFilters
        {
            get
            {
                if (!this.nullable_0.HasValue)
                {
                    StreamingDataProvider streamingProvider = MainModule.Instance.StreamingProvider;
                    if (streamingProvider != null)
                    {
                        this.nullable_0 = new bool?(streamingProvider.SupportsStreamingBars);
                    }
                    else
                    {
                        this.nullable_0 = false;
                    }
                }
                return (this.nullable_0 == true);
            }
        }

        public StrategyCenterItem ItemSelected
        {
            get
            {
                if (this.lvStrategies.SelectedItems.Count == 0)
                {
                    return null;
                }
                return (StrategyCenterItem) this.lvStrategies.SelectedItems[0].Tag;
            }
        }

        public MainForm MyMainForm
        {
            get
            {
                return (base.MdiParent as MainForm);
            }
        }

        public delegate bool ActivateItemCallback(StrategyCenterItem strategyCenterItem_0, bool activate);

        private delegate void Delegate30(StrategyCenterItem strategyCenterItem_0, string string_0);

        private delegate void Delegate31(StrategyCenterItem strategyCenterItem_0);

        private delegate void Delegate32(StrategyCenterItem strategyCenterItem_0);

        private delegate void Delegate33(TradingSystemExecutor tradingSystemExecutor_0);
    }
}

