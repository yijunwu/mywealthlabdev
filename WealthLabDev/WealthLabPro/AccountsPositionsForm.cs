namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using WealthLab;

    public class AccountsPositionsForm : Form, IWorkspace, IConnectionStatus
    {
        private Account account_0;
        private ToolStripButton btnConfigure;
        private ToolStripButton btnHelp;
        private ToolStripButton btnLoadHistory;
        private ToolStripButton btnStreaming;
        private ToolStripComboBox cmbAccounts;
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
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private ColumnHeader columnHeader_5;
        private ColumnHeader columnHeader_6;
        private ColumnHeader columnHeader_7;
        private ColumnHeader columnHeader_8;
        private ColumnHeader columnHeader_9;
        private IContainer icontainer_0;
        private ImageList imageList_0;
        public static AccountsPositionsForm Instance = null;
        private static int int_0 = 0;
        private static int int_1 = 1;
        private static int int_2 = 0;
        private static int int_3 = 2;
        private static int int_4 = 1;
        private static int int_5 = 3;
        private static int int_6 = 500;
        private int int_7;
        private ToolStripLabel lblAccounts;
        private ToolStripLabel lblBalance;
        private ToolStripLabel lblBalances;
        private ToolStripLabel lblBuyingPower;
        private ToolStripLabel lblBuyingPowerValue;
        private ToolStripLabel lblCashValue;
        private ToolStripLabel lblHistoryLoaded;
        private ToolStripLabel lblPositions;
        private ToolStripLabel lblPositionsProfit;
        private ToolStripLabel lblPositionsValue;
        private ToolStripLabel lblProfit;
        private List<string> list_0 = new List<string>();
        private SortableListView lvPositions;
        private SortableListView lvTradeHistory;
        private ToolStripMenuItem mniCopy;
        private ToolStripMenuItem mniCopyHistory;
        private ToolStripMenuItem mniDeleteHistory;
        private ToolStripMenuItem mniLoadHistory;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrintHistory;
        private TabPage pageHistory;
        private TabPage pagePositions;
        private ContextMenuStrip popup;
        private ContextMenuStrip popupHistory;
        private ToolStripSeparator sepAccount;
        private ToolStripSeparator sepBalances;
        private StatusStrip status;
        private ToolStripStatusLabel statusAccountPositions;
        private ToolStripStatusLabel statusAccounts;
        private ToolStripStatusLabel statusAccountValue;
        private ToolStripStatusLabel statusPositions;
        private StreamingQuoteManager streamingQuoteManager_0;
        private static string string_0 = "All Live Accounts";
        private TabControl tabBalances;
        private Timer timer_0;
        private ToolStrip toolbar;
        private ToolStrip toolBarDetails;
        private ToolStripButton toolStripButton1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;

        public AccountsPositionsForm()
        {
            this.InitializeComponent();
        }

        private void AccountsPositionsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Instance = null;
        }

        private void AccountsPositionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer_0.Enabled = false;
            if (this.streamingQuoteManager_0.Provider != null)
            {
                this.streamingQuoteManager_0.Provider.ClearRequests(this.streamingQuoteManager_0);
            }
        }

        private void AccountsPositionsForm_Load(object sender, EventArgs e)
        {
            if (base.Width < 730)
            {
                base.Width = 730;
            }
            Instance = this;
            int num = 0;
            if (MainModule.Instance.BrokerProvider != null)
            {
                this.cmbAccounts.Items.Clear();
                foreach (string str in MainModule.Instance.AccountNumbers)
                {
                    Account account = MainModule.Instance.TradeManager.FindAccount(str);
                    if (account != null)
                    {
                        num += account.Positions.Count;
                    }
                    this.cmbAccounts.Items.Add(str);
                }
                this.btnConfigure.Visible = MainModule.Instance.BrokerProvider is ICustomSettings;
            }
            if (!MainModule.Instance.AuthProvider.AllowStreaming)
            {
                this.btnStreaming.Visible = false;
            }
            this.cmbAccounts.Items.Insert(0, string_0);
            this.cmbAccounts.Items.Insert(0, "Select an Account");
            if (MainModule.Instance.AuthProvider.LoggedIn)
            {
                this.cmbAccounts.SelectedIndex = this.cmbAccounts.Items.IndexOf(MainModule.Instance.DefaultAccountNumber);
                if ((this.cmbAccounts.SelectedIndex == -1) && (this.cmbAccounts.Items.Count > 0))
                {
                    this.cmbAccounts.SelectedIndex = 0;
                }
            }
            else
            {
                this.cmbAccounts.SelectedIndex = 0;
            }
            if (this.cmbAccounts.Text == "Select an Account")
            {
                this.statusAccounts.Text = "Select an Account";
            }
            else if (this.cmbAccounts.Items.Count == 1)
            {
                this.statusAccounts.Text = "1 Account";
            }
            else
            {
                this.statusAccounts.Text = (this.cmbAccounts.Items.Count - 2) + " Accounts";
            }
            if (num == 1)
            {
                this.statusPositions.Text = "1 Total Position";
            }
            else
            {
                this.statusPositions.Text = num + " Total Positions";
            }
        }

        public void AccountUpdated(Account account)
        {
            base.Invoke(new Delegate26(this.method_2), new object[] { account });
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            if (MainModule.Instance.Authenticate())
            {
                ICustomSettings brokerProvider = MainModule.Instance.BrokerProvider as ICustomSettings;
                if (brokerProvider != null)
                {
                    UserControl settingsUI = brokerProvider.GetSettingsUI();
                    ChartSettingsForm form = new ChartSettingsForm {
                        Text = "Account/Broker Settings"
                    };
                    form.AddSettingsUI(settingsUI);
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        brokerProvider.ChangeSettings(settingsUI);
                        brokerProvider.WriteSettings(MainModule.Instance.Settings);
                        string str = "";
                        if (this.cmbAccounts.SelectedIndex >= 1)
                        {
                            str = this.cmbAccounts.Items[this.cmbAccounts.SelectedIndex].ToString();
                        }
                        this.AccountsPositionsForm_Load(this, EventArgs.Empty);
                        for (int i = 0; i < this.cmbAccounts.Items.Count; i++)
                        {
                            if (this.cmbAccounts.Items[i].ToString() == str)
                            {
                                this.cmbAccounts.SelectedIndex = i;
                                break;
                            }
                        }
                        MainModule.Instance.LoginSuccessful();
                        this.AccountsPositionsForm_Load(this, EventArgs.Empty);
                        this.Cursor = Cursors.Arrow;
                    }
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MainModule.Instance.ContextSensitiveHelp("accountbalancespos.htm");
        }

        private void btnStreaming_CheckStateChanged(object sender, EventArgs e)
        {
            this.streamingQuoteManager_0.ConnectionStatus = MainModule.Instance;
            if (this.streamingQuoteManager_0.Provider == null)
            {
                this.streamingQuoteManager_0.Provider = MainModule.Instance.StreamingProvider;
            }
            if (this.streamingQuoteManager_0.Provider != null)
            {
                if (this.btnStreaming.Checked)
                {
                    foreach (ListViewItem item in this.lvPositions.Items)
                    {
                        string text = item.SubItems[this.columnHeader_1.DisplayIndex].Text;
                        if (!this.list_0.Contains(text))
                        {
                            this.list_0.Add(text);
                            this.streamingQuoteManager_0.Subscribe(text);
                        }
                    }
                    this.timer_0.Enabled = true;
                }
                else
                {
                    foreach (string str2 in this.list_0)
                    {
                        this.streamingQuoteManager_0.Unsubscribe(str2);
                    }
                    this.list_0.Clear();
                    this.timer_0.Enabled = false;
                }
            }
        }

        private void cmbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbAccounts.SelectedIndex != -1)
            {
                if ((this.cmbAccounts.Text == string_0) && !MainModule.Instance.Authenticate())
                {
                    this.cmbAccounts.SelectedIndex = 0;
                }
                else
                {
                    Account account = MainModule.Instance.TradeManager.FindAccount(this.cmbAccounts.Text);
                    if (account != null)
                    {
                        if (!account.IsPaperAccount && !MainModule.Instance.Authenticate())
                        {
                            this.cmbAccounts.SelectedIndex = 0;
                            return;
                        }
                    }
                    else if (((this.cmbAccounts.Text != string_0) && (this.cmbAccounts.Text != "Select an Account")) && !MainModule.Instance.Authenticate())
                    {
                        this.cmbAccounts.SelectedIndex = 0;
                        return;
                    }
                    this.method_7();
                    this.method_8();
                    this.statusAccountPositions.Visible = (this.cmbAccounts.Text != string_0) && (this.cmbAccounts.Text != "Select an Account");
                    this.statusAccountValue.Visible = this.statusAccountPositions.Visible;
                    this.lvTradeHistory.BeginUpdate();
                    this.lvTradeHistory.Items.Clear();
                    this.lvTradeHistory.EndUpdate();
                    this.method_9();
                }
            }
        }

        public void Connect()
        {
        }

        public void Connect(bool reconnect)
        {
            if (reconnect)
            {
                this.btnStreaming.Checked = false;
            }
        }

        public void CopyToClipboard()
        {
            if (this.tabBalances.SelectedTab.Name == "pagePositions")
            {
                MainModule.Instance.CopyListViewToClipboard(this.lvPositions);
            }
            else
            {
                MainModule.Instance.CopyListViewToClipboard(this.lvTradeHistory);
            }
        }

        public void Disconnect()
        {
            this.btnStreaming.Checked = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AccountsPositionsForm));
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.toolbar = new ToolStrip();
            this.lblBalances = new ToolStripLabel();
            this.lblAccounts = new ToolStripLabel();
            this.cmbAccounts = new ToolStripComboBox();
            this.sepAccount = new ToolStripSeparator();
            this.btnStreaming = new ToolStripButton();
            this.toolStripButton1 = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.lblHistoryLoaded = new ToolStripLabel();
            this.btnLoadHistory = new ToolStripButton();
            this.btnHelp = new ToolStripButton();
            this.toolBarDetails = new ToolStrip();
            this.lblBalance = new ToolStripLabel();
            this.lblCashValue = new ToolStripLabel();
            this.lblBuyingPower = new ToolStripLabel();
            this.lblBuyingPowerValue = new ToolStripLabel();
            this.sepBalances = new ToolStripSeparator();
            this.lblPositions = new ToolStripLabel();
            this.lblPositionsValue = new ToolStripLabel();
            this.lblProfit = new ToolStripLabel();
            this.lblPositionsProfit = new ToolStripLabel();
            this.btnConfigure = new ToolStripButton();
            this.status = new StatusStrip();
            this.statusAccounts = new ToolStripStatusLabel();
            this.statusPositions = new ToolStripStatusLabel();
            this.statusAccountPositions = new ToolStripStatusLabel();
            this.statusAccountValue = new ToolStripStatusLabel();
            this.tabBalances = new TabControl();
            this.popup = new ContextMenuStrip(this.icontainer_0);
            this.mniCopy = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.pagePositions = new TabPage();
            this.lvPositions = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_16 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.columnHeader_5 = new ColumnHeader();
            this.columnHeader_17 = new ColumnHeader();
            this.columnHeader_6 = new ColumnHeader();
            this.columnHeader_7 = new ColumnHeader();
            this.pageHistory = new TabPage();
            this.lvTradeHistory = new SortableListView();
            this.columnHeader_9 = new ColumnHeader();
            this.columnHeader_15 = new ColumnHeader();
            this.columnHeader_8 = new ColumnHeader();
            this.columnHeader_10 = new ColumnHeader();
            this.columnHeader_11 = new ColumnHeader();
            this.columnHeader_12 = new ColumnHeader();
            this.columnHeader_18 = new ColumnHeader();
            this.columnHeader_13 = new ColumnHeader();
            this.columnHeader_14 = new ColumnHeader();
            this.popupHistory = new ContextMenuStrip(this.icontainer_0);
            this.mniDeleteHistory = new ToolStripMenuItem();
            this.mniLoadHistory = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.mniCopyHistory = new ToolStripMenuItem();
            this.mniPrintHistory = new ToolStripMenuItem();
            this.timer_0 = new Timer(this.icontainer_0);
            this.streamingQuoteManager_0 = new StreamingQuoteManager(this.icontainer_0);
            this.toolbar.SuspendLayout();
            this.toolBarDetails.SuspendLayout();
            this.status.SuspendLayout();
            this.tabBalances.SuspendLayout();
            this.popup.SuspendLayout();
            this.pagePositions.SuspendLayout();
            this.pageHistory.SuspendLayout();
            this.popupHistory.SuspendLayout();
            base.SuspendLayout();
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("imgPositions.ImageStream");
            this.imageList_0.TransparentColor = Color.Silver;
            this.imageList_0.Images.SetKeyName(0, "");
            this.imageList_0.Images.SetKeyName(1, "");
            this.imageList_0.Images.SetKeyName(2, "sell.bmp");
            this.imageList_0.Images.SetKeyName(3, "cover.bmp");
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.lblBalances, this.lblAccounts, this.cmbAccounts, this.sepAccount, this.btnStreaming, this.toolStripButton1, this.toolStripSeparator1, this.lblHistoryLoaded, this.btnLoadHistory, this.btnHelp });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x2d2, 0x19);
            this.toolbar.TabIndex = 2;
            this.toolbar.Text = "toolStrip1";
            this.lblBalances.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.lblBalances.Name = "lblBalances";
            this.lblBalances.Size = new Size(0x39, 0x16);
            this.lblBalances.Text = "Balances";
            this.lblAccounts.Name = "lblAccounts";
            this.lblAccounts.Size = new Size(50, 0x16);
            this.lblAccounts.Text = "Account:";
            this.cmbAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAccounts.Name = "cmbAccounts";
            this.cmbAccounts.Size = new Size(0x79, 0x19);
            this.cmbAccounts.SelectedIndexChanged += new EventHandler(this.cmbAccounts_SelectedIndexChanged);
            this.sepAccount.Name = "sepAccount";
            this.sepAccount.Size = new Size(6, 0x19);
            this.btnStreaming.CheckOnClick = true;
            this.btnStreaming.Image = (Image) manager.GetObject("btnStreaming.Image");
            this.btnStreaming.ImageTransparentColor = Color.Magenta;
            this.btnStreaming.Name = "btnStreaming";
            this.btnStreaming.Size = new Size(0x76, 0x16);
            this.btnStreaming.Text = "Streaming Updates";
            this.btnStreaming.CheckStateChanged += new EventHandler(this.btnStreaming_CheckStateChanged);
            this.toolStripButton1.Image = (Image) manager.GetObject("toolStripButton1.Image");
            this.toolStripButton1.ImageTransparentColor = Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new Size(0x3e, 0x16);
            this.toolStripButton1.Text = "Update";
            this.toolStripButton1.Click += new EventHandler(this.toolStripButton1_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.lblHistoryLoaded.Name = "lblHistoryLoaded";
            this.lblHistoryLoaded.Size = new Size(0x94, 0x16);
            this.lblHistoryLoaded.Text = "0 History Items Loaded (of 0)";
            this.btnLoadHistory.Image = (Image) manager.GetObject("btnLoadHistory.Image");
            this.btnLoadHistory.ImageTransparentColor = Color.Magenta;
            this.btnLoadHistory.Name = "btnLoadHistory";
            this.btnLoadHistory.Size = new Size(0x61, 0x16);
            this.btnLoadHistory.Text = "Load Next 500";
            this.btnLoadHistory.Click += new EventHandler(this.mniLoadHistory_Click);
            this.btnHelp.Image = (Image) manager.GetObject("btnHelp.Image");
            this.btnHelp.ImageTransparentColor = Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x30, 0x16);
            this.btnHelp.Text = "Help";
            this.btnHelp.ToolTipText = "Help on Accounts and Positions";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.toolBarDetails.GripStyle = ToolStripGripStyle.Hidden;
            this.toolBarDetails.Items.AddRange(new ToolStripItem[] { this.lblBalance, this.lblCashValue, this.lblBuyingPower, this.lblBuyingPowerValue, this.sepBalances, this.lblPositions, this.lblPositionsValue, this.lblProfit, this.lblPositionsProfit, this.btnConfigure });
            this.toolBarDetails.LayoutStyle = ToolStripLayoutStyle.Flow;
            this.toolBarDetails.Location = new Point(0, 0x19);
            this.toolBarDetails.Name = "toolBarDetails";
            this.toolBarDetails.Size = new Size(0x2d2, 0x17);
            this.toolBarDetails.TabIndex = 3;
            this.toolBarDetails.Text = "toolStrip1";
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new Size(0x51, 13);
            this.lblBalance.Text = "Available Cash:";
            this.lblCashValue.ForeColor = Color.Green;
            this.lblCashValue.Name = "lblCashValue";
            this.lblCashValue.Size = new Size(0x23, 13);
            this.lblCashValue.Text = "$0.00";
            this.lblBuyingPower.Name = "lblBuyingPower";
            this.lblBuyingPower.Padding = new Padding(8, 0, 0, 0);
            this.lblBuyingPower.Size = new Size(0x54, 13);
            this.lblBuyingPower.Text = "Buying Power:";
            this.lblBuyingPowerValue.ForeColor = Color.Green;
            this.lblBuyingPowerValue.Name = "lblBuyingPowerValue";
            this.lblBuyingPowerValue.Size = new Size(0x23, 13);
            this.lblBuyingPowerValue.Text = "$0.00";
            this.sepBalances.Name = "sepBalances";
            this.sepBalances.Size = new Size(6, 0x17);
            this.lblPositions.Name = "lblPositions";
            this.lblPositions.Size = new Size(0x52, 13);
            this.lblPositions.Text = "Positions Value:";
            this.lblPositionsValue.ForeColor = Color.Green;
            this.lblPositionsValue.Name = "lblPositionsValue";
            this.lblPositionsValue.Size = new Size(0x23, 13);
            this.lblPositionsValue.Text = "$0.00";
            this.lblProfit.Name = "lblProfit";
            this.lblProfit.Padding = new Padding(8, 0, 0, 0);
            this.lblProfit.Size = new Size(90, 13);
            this.lblProfit.Text = "Positions Profit:";
            this.lblPositionsProfit.ForeColor = Color.Red;
            this.lblPositionsProfit.Name = "lblPositionsProfit";
            this.lblPositionsProfit.Size = new Size(0x23, 13);
            this.lblPositionsProfit.Text = "$0.00";
            this.btnConfigure.Image = (Image) manager.GetObject("btnConfigure.Image");
            this.btnConfigure.ImageTransparentColor = Color.Magenta;
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new Size(0x98, 20);
            this.btnConfigure.Text = "Configure Paper Accounts";
            this.btnConfigure.Click += new EventHandler(this.btnConfigure_Click);
            this.status.Items.AddRange(new ToolStripItem[] { this.statusAccounts, this.statusPositions, this.statusAccountPositions, this.statusAccountValue });
            this.status.Location = new Point(0, 0x193);
            this.status.Name = "status";
            this.status.Size = new Size(0x2d2, 0x16);
            this.status.TabIndex = 4;
            this.status.Text = "statusStrip1";
            this.statusAccounts.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusAccounts.Name = "statusAccounts";
            this.statusAccounts.Size = new Size(0x40, 0x11);
            this.statusAccounts.Text = "0 Accounts";
            this.statusPositions.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusPositions.Name = "statusPositions";
            this.statusPositions.Size = new Size(0x59, 0x11);
            this.statusPositions.Text = "0 Total Positions";
            this.statusAccountPositions.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusAccountPositions.Name = "statusAccountPositions";
            this.statusAccountPositions.Size = new Size(0x9f, 0x11);
            this.statusAccountPositions.Text = "0 Positions in Selected Account";
            this.statusAccountValue.Name = "statusAccountValue";
            this.statusAccountValue.Size = new Size(0xa9, 0x11);
            this.statusAccountValue.Text = "Account Value for Account: $0.00";
            this.tabBalances.ContextMenuStrip = this.popup;
            this.tabBalances.Controls.Add(this.pagePositions);
            this.tabBalances.Controls.Add(this.pageHistory);
            this.tabBalances.Dock = DockStyle.Fill;
            this.tabBalances.Location = new Point(0, 0x30);
            this.tabBalances.Name = "tabBalances";
            this.tabBalances.SelectedIndex = 0;
            this.tabBalances.Size = new Size(0x2d2, 0x163);
            this.tabBalances.TabIndex = 5;
            this.popup.Items.AddRange(new ToolStripItem[] { this.mniCopy, this.mniPrint });
            this.popup.Name = "popup";
            this.popup.Size = new Size(0x72, 0x30);
            this.mniCopy.Image = (Image) manager.GetObject("mniCopy.Image");
            this.mniCopy.ImageTransparentColor = Color.Fuchsia;
            this.mniCopy.Name = "mniCopy";
            this.mniCopy.Size = new Size(0x71, 0x16);
            this.mniCopy.Text = "Copy";
            this.mniCopy.ToolTipText = "Copy data to the clipboard";
            this.mniCopy.Click += new EventHandler(this.mniCopyHistory_Click);
            this.mniPrint.Image = (Image) manager.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0x71, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.Click += new EventHandler(this.mniPrintHistory_Click);
            this.pagePositions.Controls.Add(this.lvPositions);
            this.pagePositions.Location = new Point(4, 0x16);
            this.pagePositions.Name = "pagePositions";
            this.pagePositions.Padding = new Padding(3);
            this.pagePositions.Size = new Size(0x2ca, 0x149);
            this.pagePositions.TabIndex = 0;
            this.pagePositions.Text = "Positions";
            this.pagePositions.UseVisualStyleBackColor = true;
            this.lvPositions.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_16, this.columnHeader_1, this.columnHeader_2, this.columnHeader_3, this.columnHeader_4, this.columnHeader_5, this.columnHeader_17, this.columnHeader_6, this.columnHeader_7 });
            this.lvPositions.Dock = DockStyle.Fill;
            this.lvPositions.FullRowSelect = true;
            this.lvPositions.Location = new Point(3, 3);
            this.lvPositions.MultiSelect = false;
            this.lvPositions.Name = "lvPositions";
            this.lvPositions.Size = new Size(0x2c4, 0x143);
            this.lvPositions.SmallImageList = this.imageList_0;
            this.lvPositions.TabIndex = 0;
            this.lvPositions.UseCompatibleStateImageBehavior = false;
            this.lvPositions.View = View.Details;
            this.columnHeader_0.Tag = "S";
            this.columnHeader_0.Text = "Position";
            this.columnHeader_16.Text = "Account";
            this.columnHeader_16.Width = 80;
            this.columnHeader_1.Tag = "S";
            this.columnHeader_1.Text = "Symbol";
            this.columnHeader_2.Tag = "N";
            this.columnHeader_2.Text = "Quantity";
            this.columnHeader_2.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_3.Tag = "N";
            this.columnHeader_3.Text = "Cost Basis";
            this.columnHeader_3.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_3.Width = 70;
            this.columnHeader_4.Tag = "N";
            this.columnHeader_4.Text = "Last Price";
            this.columnHeader_4.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_4.Width = 70;
            this.columnHeader_5.Tag = "N";
            this.columnHeader_5.Text = "Market Value";
            this.columnHeader_5.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_5.Width = 80;
            this.columnHeader_17.Text = "Trade Type";
            this.columnHeader_6.Tag = "N";
            this.columnHeader_6.Text = "Profit";
            this.columnHeader_6.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_6.Width = 80;
            this.columnHeader_7.Tag = "N";
            this.columnHeader_7.Text = "% Profit";
            this.columnHeader_7.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_7.Width = 80;
            this.pageHistory.Controls.Add(this.lvTradeHistory);
            this.pageHistory.Location = new Point(4, 0x16);
            this.pageHistory.Name = "pageHistory";
            this.pageHistory.Padding = new Padding(3);
            this.pageHistory.Size = new Size(0x2ca, 0x147);
            this.pageHistory.TabIndex = 1;
            this.pageHistory.Text = "Local Trade History";
            this.pageHistory.UseVisualStyleBackColor = true;
            this.lvTradeHistory.Columns.AddRange(new ColumnHeader[] { this.columnHeader_9, this.columnHeader_15, this.columnHeader_8, this.columnHeader_10, this.columnHeader_11, this.columnHeader_12, this.columnHeader_18, this.columnHeader_13, this.columnHeader_14 });
            this.lvTradeHistory.ContextMenuStrip = this.popupHistory;
            this.lvTradeHistory.Dock = DockStyle.Fill;
            this.lvTradeHistory.FullRowSelect = true;
            this.lvTradeHistory.Location = new Point(3, 3);
            this.lvTradeHistory.Name = "lvTradeHistory";
            this.lvTradeHistory.Size = new Size(0x2c4, 0x141);
            this.lvTradeHistory.SmallImageList = this.imageList_0;
            this.lvTradeHistory.TabIndex = 1;
            this.lvTradeHistory.UseCompatibleStateImageBehavior = false;
            this.lvTradeHistory.View = View.Details;
            this.lvTradeHistory.SelectedIndexChanged += new EventHandler(this.lvTradeHistory_SelectedIndexChanged);
            this.lvTradeHistory.KeyDown += new KeyEventHandler(this.lvTradeHistory_KeyDown);
            this.columnHeader_9.Tag = "S";
            this.columnHeader_9.Text = "Action";
            this.columnHeader_9.Width = 80;
            this.columnHeader_15.Text = "Account";
            this.columnHeader_15.Width = 80;
            this.columnHeader_8.Tag = "DT";
            this.columnHeader_8.Text = "Date/Time";
            this.columnHeader_8.Width = 120;
            this.columnHeader_10.Tag = "N";
            this.columnHeader_10.Text = "Quantity";
            this.columnHeader_10.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_11.Tag = "S";
            this.columnHeader_11.Text = "Symbol";
            this.columnHeader_11.Width = 80;
            this.columnHeader_12.Tag = "N";
            this.columnHeader_12.Text = "Price";
            this.columnHeader_12.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_12.Width = 80;
            this.columnHeader_18.Text = "Trade Type";
            this.columnHeader_13.Text = "Strategy";
            this.columnHeader_13.Width = 100;
            this.columnHeader_14.Text = "Scale";
            this.columnHeader_14.Width = 80;
            this.popupHistory.Items.AddRange(new ToolStripItem[] { this.mniDeleteHistory, this.mniLoadHistory, this.toolStripSeparator2, this.mniCopyHistory, this.mniPrintHistory });
            this.popupHistory.Name = "popupHistory";
            this.popupHistory.Size = new Size(0xcf, 0x62);
            this.mniDeleteHistory.Image = (Image) manager.GetObject("mniDeleteHistory.Image");
            this.mniDeleteHistory.ImageTransparentColor = Color.Fuchsia;
            this.mniDeleteHistory.Name = "mniDeleteHistory";
            this.mniDeleteHistory.Size = new Size(0xce, 0x16);
            this.mniDeleteHistory.Text = "Delete Selected Item(s)";
            this.mniDeleteHistory.Click += new EventHandler(this.mniDeleteHistory_Click);
            this.mniLoadHistory.Image = (Image) manager.GetObject("mniLoadHistory.Image");
            this.mniLoadHistory.ImageTransparentColor = Color.Fuchsia;
            this.mniLoadHistory.Name = "mniLoadHistory";
            this.mniLoadHistory.Size = new Size(0xce, 0x16);
            this.mniLoadHistory.Text = "Load Next 500";
            this.mniLoadHistory.Click += new EventHandler(this.mniLoadHistory_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(0xcb, 6);
            this.mniCopyHistory.Image = (Image) manager.GetObject("mniCopyHistory.Image");
            this.mniCopyHistory.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyHistory.Name = "mniCopyHistory";
            this.mniCopyHistory.Size = new Size(0xce, 0x16);
            this.mniCopyHistory.Text = "Copy";
            this.mniCopyHistory.ToolTipText = "Copy data to the clipboard";
            this.mniCopyHistory.Click += new EventHandler(this.mniCopyHistory_Click);
            this.mniPrintHistory.Image = (Image) manager.GetObject("mniPrintHistory.Image");
            this.mniPrintHistory.Name = "mniPrintHistory";
            this.mniPrintHistory.Size = new Size(0xce, 0x16);
            this.mniPrintHistory.Text = "Print";
            this.mniPrintHistory.Click += new EventHandler(this.mniPrintHistory_Click);
            this.timer_0.Interval = 0x3e8;
            this.timer_0.Tick += new EventHandler(this.timer_0_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x2d2, 0x1a9);
            base.Controls.Add(this.tabBalances);
            base.Controls.Add(this.status);
            base.Controls.Add(this.toolBarDetails);
            base.Controls.Add(this.toolbar);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "AccountsPositionsForm";
            base.StartPosition = FormStartPosition.WindowsDefaultBounds;
            this.Text = "Account Balances and Positions";
            base.FormClosing += new FormClosingEventHandler(this.AccountsPositionsForm_FormClosing);
            base.FormClosed += new FormClosedEventHandler(this.AccountsPositionsForm_FormClosed);
            base.Load += new EventHandler(this.AccountsPositionsForm_Load);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.toolBarDetails.ResumeLayout(false);
            this.toolBarDetails.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.tabBalances.ResumeLayout(false);
            this.popup.ResumeLayout(false);
            this.pagePositions.ResumeLayout(false);
            this.pageHistory.ResumeLayout(false);
            this.popupHistory.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadWorkspaceItems(IList<string> items, int version)
        {
        }

        private void lvTradeHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if ((this.lvTradeHistory.SelectedItems.Count != 0) && ((e.KeyCode == Keys.Delete) && (MessageBox.Show("Delete the Selected History Items?", "Delete Trade History Items", MessageBoxButtons.YesNo) == DialogResult.Yes)))
            {
                this.mniDeleteHistory.PerformClick();
            }
        }

        private void lvTradeHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.mniDeleteHistory.Enabled = this.lvTradeHistory.SelectedItems.Count > 0;
        }


        private void method_0(HistoricalTrade historicalTrade_0)
        {
            if (this.cmbAccounts.Text == AccountsPositionsForm.string_0 || this.cmbAccounts.Text == historicalTrade_0.AccountNumber)
            {
                IEnumerator enumerator = this.lvTradeHistory.Items.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            ListViewItem current = (ListViewItem)enumerator.Current;
                            if (current.Tag == historicalTrade_0)
                            {
                                double quantity = historicalTrade_0.Quantity;
                                current.SubItems[this.columnHeader_10.DisplayIndex].Text = quantity.ToString();
                                double price = historicalTrade_0.Price;
                                current.SubItems[this.columnHeader_12.DisplayIndex].Text = price.ToString(string.Concat("N", DecimalsManager.Instance.GetPricingDecimalForSymbol(historicalTrade_0.Symbol)));
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
        }

        private void method_1(HistoricalTrade historicalTrade_0)
        {
            if ((this.cmbAccounts.Text == string_0) || (this.cmbAccounts.Text == historicalTrade_0.AccountNumber))
            {
                this.int_7++;
                this.method_12(historicalTrade_0);
                this.method_10();
            }
        }

        private void method_10()
        {
            this.lblHistoryLoaded.Text = string.Concat(new object[] { this.lvTradeHistory.Items.Count, " History Items Loaded (of ", this.int_7, ")" });
            this.btnLoadHistory.Enabled = this.lvTradeHistory.Items.Count < this.int_7;
            this.mniLoadHistory.Text = this.btnLoadHistory.Text;
            this.mniLoadHistory.Enabled = this.btnLoadHistory.Enabled;
        }

        private void method_11(Account account_1)
        {
            foreach (AccountPosition position in account_1.Positions)
            {
                if (position.Quantity != 0.0)
                {
                    this.method_15(position);
                }
            }
        }

        private void method_12(HistoricalTrade historicalTrade_0)
        {
            ListViewItem item = this.lvTradeHistory.Items.Add(historicalTrade_0.TradeType.ToString());
            item.Tag = historicalTrade_0;
            switch (historicalTrade_0.TradeType)
            {
                case TradeType.Sell:
                    item.ImageIndex = int_3;
                    break;

                case TradeType.Short:
                    item.ImageIndex = int_4;
                    break;

                case TradeType.Cover:
                    item.ImageIndex = int_5;
                    break;

                default:
                    item.ImageIndex = int_2;
                    break;
            }
            item.SubItems.Add(historicalTrade_0.AccountNumber);
            item.SubItems.Add(historicalTrade_0.TimeStamp.ToShortDateString() + " " + historicalTrade_0.TimeStamp.ToShortTimeString());
            item.SubItems.Add(historicalTrade_0.Quantity.ToString());
            item.SubItems.Add(historicalTrade_0.Symbol);
            item.SubItems.Add(historicalTrade_0.Price.ToString("N" + DecimalsManager.Instance.GetPricingDecimalForSymbol(historicalTrade_0.Symbol)));
            if (!string.IsNullOrEmpty(historicalTrade_0.AccountTradeType))
            {
                item.SubItems.Add(historicalTrade_0.AccountTradeType);
            }
            else
            {
                item.SubItems.Add("");
            }
            if (historicalTrade_0.StrategyID != "")
            {
                Strategy iD = MainModule.Instance.Strategies.LookupID(historicalTrade_0.StrategyID);
                if (iD != null)
                {
                    item.SubItems.Add(iD.Name);
                }
                else
                {
                    item.SubItems.Add("?");
                }
            }
            else
            {
                item.SubItems.Add("");
            }
            item.SubItems.Add(historicalTrade_0.Scale.ToString());
        }

        private void method_13()
        {
            double num = 0.0;
            double num2 = 0.0;
            foreach (Account account in MainModule.Instance.BrokerProvider.Accounts)
            {
                if ((account == this.account_0) || (this.cmbAccounts.Text == string_0))
                {
                    foreach (AccountPosition position in account.Positions)
                    {
                        if ((position.Quantity != 0.0) && (!account.IsPaperAccount || (this.cmbAccounts.Text != string_0)))
                        {
                            num += position.MarketValue;
                            num2 += position.ProfitDollars;
                        }
                    }
                }
            }
            this.lblPositionsValue.Text = num.ToString("C2");
            if (num > 0.0)
            {
                this.lblPositionsValue.ForeColor = Color.Green;
            }
            else
            {
                this.lblPositionsValue.ForeColor = Color.Red;
            }
            this.lblPositionsProfit.Text = num2.ToString("C2");
            if (num2 > 0.0)
            {
                this.lblPositionsProfit.ForeColor = Color.Green;
            }
            else
            {
                this.lblPositionsProfit.ForeColor = Color.Red;
            }
        }

        private void method_14(AccountPosition accountPosition_0, ListViewItem listViewItem_0)
        {
            if (accountPosition_0.EntryPrice == 0.0)
            {
                listViewItem_0.SubItems[this.columnHeader_5.DisplayIndex].Text = "N/A";
                listViewItem_0.SubItems[this.columnHeader_6.DisplayIndex].Text = "N/A";
                listViewItem_0.SubItems[this.columnHeader_7.DisplayIndex].Text = "N/A";
                listViewItem_0.SubItems[this.columnHeader_6.DisplayIndex].ForeColor = this.lvPositions.ForeColor;
                listViewItem_0.SubItems[this.columnHeader_7.DisplayIndex].ForeColor = this.lvPositions.ForeColor;
            }
            else
            {
                listViewItem_0.SubItems[this.columnHeader_5.DisplayIndex].Text = accountPosition_0.MarketValue.ToString("N2");
                listViewItem_0.SubItems[this.columnHeader_6.DisplayIndex].Text = accountPosition_0.ProfitDollars.ToString("N2");
                listViewItem_0.SubItems[this.columnHeader_7.DisplayIndex].Text = accountPosition_0.ProfitPct.ToString("N2");
                if (accountPosition_0.ProfitDollars > 0.0)
                {
                    listViewItem_0.SubItems[this.columnHeader_6.DisplayIndex].ForeColor = Color.Green;
                    listViewItem_0.SubItems[this.columnHeader_7.DisplayIndex].ForeColor = Color.Green;
                }
                else
                {
                    listViewItem_0.SubItems[this.columnHeader_6.DisplayIndex].ForeColor = Color.Red;
                    listViewItem_0.SubItems[this.columnHeader_7.DisplayIndex].ForeColor = Color.Red;
                }
            }
        }

        private void method_15(AccountPosition accountPosition_0)
        {
            ListViewItem item;
            if (accountPosition_0.PositionType == PositionType.Long)
            {
                item = this.lvPositions.Items.Add("Long", int_0);
            }
            else
            {
                item = this.lvPositions.Items.Add("Short", int_1);
            }
            item.UseItemStyleForSubItems = false;
            item.Tag = accountPosition_0;
            item.SubItems.Add(accountPosition_0.Account.AccountNumber);
            item.SubItems.Add(accountPosition_0.Symbol);
            item.SubItems.Add(accountPosition_0.Quantity.ToString());
            int pricingDecimalForSymbol = DecimalsManager.Instance.GetPricingDecimalForSymbol(accountPosition_0.Symbol);
            if (accountPosition_0.EntryPrice == 0.0)
            {
                item.SubItems.Add("N/A");
            }
            else
            {
                item.SubItems.Add(accountPosition_0.EntryPrice.ToString("N" + pricingDecimalForSymbol));
            }
            item.SubItems.Add(accountPosition_0.LastPrice.ToString("N" + pricingDecimalForSymbol));
            item.SubItems.Add((accountPosition_0.EntryPrice == 0.0) ? "N/A" : accountPosition_0.MarketValue.ToString("N2"));
            item.SubItems.Add(MainModule.Instance.GetPositionAccountTradeType(accountPosition_0));
            item.SubItems.Add((accountPosition_0.EntryPrice == 0.0) ? "N/A" : accountPosition_0.ProfitDollars.ToString("N2")).ForeColor = (accountPosition_0.ProfitDollars > 0.0) ? Color.Green : Color.Red;
            item.SubItems.Add((accountPosition_0.EntryPrice == 0.0) ? "N/A" : accountPosition_0.ProfitPct.ToString("N2")).ForeColor = (accountPosition_0.ProfitDollars > 0.0) ? Color.Green : Color.Red;
            this.lvPositions.SortByColumn(this.columnHeader_1, SortOrder.Ascending);
        }

        private void method_2(Account account_1)
        {
            if (((account_1 == null) || (account_1.AccountNumber == this.cmbAccounts.Text)) || (this.cmbAccounts.Text == string_0))
            {
                this.method_7();
            }
        }

        private void method_3(Account account_1)
        {
            if (((account_1 == null) || (account_1.AccountNumber == this.cmbAccounts.Text)) || (this.cmbAccounts.Text == string_0))
            {
                this.method_8();
                this.method_13();
            }
        }

        private void method_4(AccountPosition accountPosition_0)
        {
            if ((this.cmbAccounts.Text == string_0) || (this.cmbAccounts.Text == accountPosition_0.Account.AccountNumber))
            {
                foreach (ListViewItem item in this.lvPositions.Items)
                {
                    if (item.Tag == accountPosition_0)
                    {
                        return;
                    }
                }
                MainModule.Instance.DataSources.FindProvider("FidelityStaticProvider");
                this.method_15(accountPosition_0);
                this.method_7();
            }
        }

        private void method_5(AccountPosition accountPosition_0)
        {
            IEnumerator enumerator = this.lvPositions.Items.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ListViewItem current = (ListViewItem)enumerator.Current;
                        if (current.Tag == accountPosition_0)
                        {
                            double quantity = accountPosition_0.Quantity;
                            current.SubItems[this.columnHeader_2.DisplayIndex].Text = quantity.ToString();
                            int pricingDecimalForSymbol = DecimalsManager.Instance.GetPricingDecimalForSymbol(accountPosition_0.Symbol);
                            double entryPrice = accountPosition_0.EntryPrice;
                            current.SubItems[this.columnHeader_3.DisplayIndex].Text = entryPrice.ToString(string.Concat("N", pricingDecimalForSymbol));
                            this.method_14(accountPosition_0, current);
                            this.method_7();
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

        private void method_6(AccountPosition accountPosition_0)
        {
            for (int i = 0; i < this.lvPositions.Items.Count; i++)
            {
                if (this.lvPositions.Items[i].Tag == accountPosition_0)
                {
                    this.lvPositions.Items.RemoveAt(i);
                    this.method_7();
                    return;
                }
            }
        }

        private void method_7()
        {
            Account account = null;
            if ((this.cmbAccounts.Text != string_0) && (this.cmbAccounts.Text != "Select an Account"))
            {
                account = MainModule.Instance.BrokerProvider.FindAccount(this.cmbAccounts.Text);
            }
            if (account != null)
            {
                this.account_0 = account;
            }
            if (this.cmbAccounts.Text == "Select an Account")
            {
                this.account_0 = null;
            }
            if (this.cmbAccounts.Text == string_0)
            {
                double num = 0.0;
                double num2 = 0.0;
                foreach (Account account2 in MainModule.Instance.BrokerProvider.Accounts)
                {
                    if (!account2.IsPaperAccount)
                    {
                        num += account2.AvailableCash;
                        num2 += account2.BuyingPower;
                    }
                }
                this.lblCashValue.Text = num.ToString("C2");
                this.lblBuyingPowerValue.Text = num2.ToString("C2");
            }
            else if (account != null)
            {
                this.lblCashValue.Text = account.AvailableCash.ToString("C2");
                this.lblBuyingPowerValue.Text = account.BuyingPower.ToString("C2");
                this.statusAccountValue.Text = "Account Value for " + account.AccountNumber + ": " + account.AccountValue.ToString("C2") + " As of " + account.AccountValueTimeStamp.ToShortDateString() + " " + account.AccountValueTimeStamp.ToLongTimeString();
            }
            else
            {
                double num6 = 0.0;
                this.lblCashValue.Text = num6.ToString("C2");
                this.lblBuyingPowerValue.Text = num6.ToString("C2");
                this.statusAccountValue.Text = "";
            }
        }

        private void method_8()
        {
            Account account = null;
            if (this.cmbAccounts.Text != AccountsPositionsForm.string_0)
            {
                account = MainModule.Instance.BrokerProvider.FindAccount(this.cmbAccounts.Text);
            }
            if (account != null)
            {
                this.account_0 = account;
            }
            this.lvPositions.BeginUpdate();
            this.lvPositions.Items.Clear();
            if (account != null || !(this.cmbAccounts.Text != "Select an Account"))
            {
                if (account != null)
                {
                    this.method_11(account);
                }
            }
            else
            {
                foreach (Account account1 in MainModule.Instance.BrokerProvider.Accounts)
                {
                    if (account1.IsPaperAccount)
                    {
                        continue;
                    }
                    this.method_11(account1);
                }
            }
            this.lvPositions.EndUpdate();
            this.lvPositions.SortByColumn(this.columnHeader_1, SortOrder.Ascending);
            this.method_13();
            if (account != null && this.cmbAccounts.Text != "Select an Account")
            {
                if (account.Positions.Count != 1)
                {
                    this.statusAccountPositions.Text = string.Concat(account.Positions.Count, " Positions in Account ", account.AccountNumber);
                }
                else
                {
                    this.statusAccountPositions.Text = string.Concat("1 Position in Account ", account.AccountNumber);
                }
            }
            if (this.btnStreaming.Checked)
            {
                foreach (ListViewItem item in this.lvPositions.Items)
                {
                    string text = item.SubItems[this.columnHeader_1.DisplayIndex].Text;
                    if (this.streamingQuoteManager_0.Provider.IsSymbolStreaming(text, this.streamingQuoteManager_0))
                    {
                        continue;
                    }
                    this.streamingQuoteManager_0.Subscribe(text);
                }
                List<string> symbolsSubscribed = this.streamingQuoteManager_0.Provider.GetSymbolsSubscribed(this.streamingQuoteManager_0);
                foreach (string str in symbolsSubscribed)
                {
                    bool flag = true;
                    IEnumerator enumerator = this.lvPositions.Items.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            if (enumerator.MoveNext())
                            {
                                ListViewItem current = (ListViewItem)enumerator.Current;
                                if (current.SubItems[this.columnHeader_1.DisplayIndex].Text == str)
                                {
                                    flag = false;
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
                    if (!flag)
                    {
                        continue;
                    }
                    this.streamingQuoteManager_0.Unsubscribe(str);
                }
            }
        }

        private void method_9()
        {
            int num = this.lvTradeHistory.Items.Count + int_6;
            List<HistoricalTrade> list = new List<HistoricalTrade>();
            foreach (HistoricalTrade trade in MainModule.Instance.TradeManager.TradeHistory)
            {
                if (((this.cmbAccounts.Text == string_0) || (trade.AccountNumber == this.cmbAccounts.Text)) && ((this.cmbAccounts.Text != string_0) || !trade.AccountNumber.StartsWith("Paper")))
                {
                    list.Add(trade);
                }
            }
            this.lvTradeHistory.BeginUpdate();
            this.lvTradeHistory.Items.Clear();
            int num2 = list.Count - num;
            if (num2 < 0)
            {
                num2 = 0;
            }
            for (int i = num2; i < list.Count; i++)
            {
                this.method_12(list[i]);
            }
            this.lvTradeHistory.EndUpdate();
            this.int_7 = list.Count;
            this.method_10();
        }

        private void mniCopyHistory_Click(object sender, EventArgs e)
        {
            this.CopyToClipboard();
        }

        private void mniDeleteHistory_Click(object sender, EventArgs e)
        {
            for (int i = this.lvTradeHistory.Items.Count - 1; i >= 0; i--)
            {
                ListViewItem item = this.lvTradeHistory.Items[i];
                if (item.Selected)
                {
                    HistoricalTrade tag = (HistoricalTrade) item.Tag;
                    MainModule.Instance.TradeManager.TradeHistory.Remove(tag);
                    this.lvTradeHistory.Items.Remove(item);
                    this.int_7--;
                }
            }
            MainModule.Instance.TradeManager.SaveTradeHistory();
            this.method_10();
        }

        private void mniLoadHistory_Click(object sender, EventArgs e)
        {
            this.method_9();
        }

        private void mniPrintHistory_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        public void PositionAdded(AccountPosition accountPosition_0)
        {
            base.Invoke(new Delegate28(this.method_4), new object[] { accountPosition_0 });
        }

        public void PositionChanged(AccountPosition accountPosition_0)
        {
            base.Invoke(new Delegate28(this.method_5), new object[] { accountPosition_0 });
        }

        public void PositionRemoved(AccountPosition accountPosition_0)
        {
            base.Invoke(new Delegate28(this.method_6), new object[] { accountPosition_0 });
        }

        public void PositionsUpdated(Account account)
        {
            base.Invoke(new Delegate27(this.method_3), new object[] { account });
        }

        public void Print()
        {
            DataObject obj2 = new DataObject();
            if (this.tabBalances.SelectedTab.Name == "pagePositions")
            {
                obj2.SetData(PrintReport.fmtTitle.Name, "Account Positions");
                obj2.SetData(PrintReport.fmtListView.Name, this.lvPositions);
            }
            else
            {
                obj2.SetData(PrintReport.fmtTitle.Name, "Local Trade History");
                obj2.SetData(PrintReport.fmtListView.Name, this.lvTradeHistory);
            }
            obj2.SetData(PrintReport.fmtDisclosure.Name, "1. Only equity positions are displayed in this Positions report.  Information related to positions in other types of securities can be viewed on Fidelity.com.\n2. Fidelity-provided estimated cost basis (including cost basis and short sale proceeds information provided to Fidelity by customers) and change since purchase information may not reflect all adjustments necessary for tax reporting purposes. You should verify such information against your own records when calculating reportable gain or loss resulting from a sale.  Fidelity makes no warranties with respect to, and specifically disclaims any liability arising out of your use of, or any tax position taken in reliance upon, such information. Unless otherwise specified, Fidelity determines cost basis at the time of sale based on the first-in, first-out (FIFO) method for individual securities. Consult a tax advisor for further information.\n3. Market Value is calculated using your position share quantity and the corresponding last trade price from a real-time quote.\n4. In the case of a short sale, total cost ordinarily equals the cost of the asset (including commissions, if any) when purchased for delivery when the sale is closed or covered. However, as cost is unknown until the date the short sale is closed or covered, the value Fidelity reports in the Cost Basis column is equal to the price at which the short sale was transacted (total sales proceeds). When calculating Profit, Fidelity adds the negative value reported in the Market Value column to the positive value reported in the Cost Basis column.");
            PrintReport report = new PrintReport {
                BasePrintTitle = MainModule.Instance.AuthProvider.ApplicationName,
                printObject = obj2
            };
            PageSettings defaultPageSettings = this.MyMainForm.DefaultPageSettings;
            report.ShowPrintPreview = !MainModule.Instance.Settings.Get("HidePrintPreview", false);
            report.ShowPrintDialog = !MainModule.Instance.Settings.Get("HidePrintDialog", false);
            report.PrintGraphicReport(defaultPageSettings);
        }

        public int SaveWorkspaceItems(IList<string> items)
        {
            return 1;
        }

        public void ShowLoggedInState()
        {
            base.Invoke(new Delegate29(this.ShowLoggedInStateThreadSafe));
        }

        public void ShowLoggedInStateThreadSafe()
        {
            int num = 0;
            if (MainModule.Instance.BrokerProvider != null)
            {
                this.cmbAccounts.Items.Clear();
                foreach (string str in MainModule.Instance.AccountNumbers)
                {
                    Account account = MainModule.Instance.TradeManager.FindAccount(str);
                    if (account != null)
                    {
                        num += account.Positions.Count;
                    }
                    this.cmbAccounts.Items.Add(str);
                }
                this.btnConfigure.Visible = MainModule.Instance.BrokerProvider is ICustomSettings;
            }
            if (!MainModule.Instance.AuthProvider.AllowStreaming)
            {
                this.btnStreaming.Visible = false;
            }
            this.cmbAccounts.Items.Insert(0, string_0);
            this.cmbAccounts.Items.Insert(0, "Select an Account");
            if (MainModule.Instance.AuthProvider.LoggedIn)
            {
                this.cmbAccounts.SelectedIndex = this.cmbAccounts.Items.IndexOf(MainModule.Instance.DefaultAccountNumber);
                if ((this.cmbAccounts.SelectedIndex == -1) && (this.cmbAccounts.Items.Count > 0))
                {
                    this.cmbAccounts.SelectedIndex = 0;
                }
            }
            else
            {
                this.cmbAccounts.SelectedIndex = 0;
            }
            if (this.cmbAccounts.Text == "Select an Account")
            {
                this.statusAccounts.Text = "Select an Account";
            }
            else if (this.cmbAccounts.Items.Count == 1)
            {
                this.statusAccounts.Text = "1 Account";
            }
            else
            {
                this.statusAccounts.Text = (this.cmbAccounts.Items.Count - 2) + " Accounts";
            }
            if (num == 1)
            {
                this.statusPositions.Text = "1 Total Position";
            }
            else
            {
                this.statusPositions.Text = num + " Total Positions";
            }
        }

        public void StatusUpdate(ConnStatus status, int StatusCode, string Message)
        {
        }

        private void timer_0_Tick(object sender, EventArgs e)
        {
            if (this.streamingQuoteManager_0.FreshQuoteReady)
            {
                foreach (ListViewItem item in this.lvPositions.Items)
                {
                    AccountPosition tag = (AccountPosition) item.Tag;
                    Quote lastQuote = this.streamingQuoteManager_0.GetLastQuote(tag.Symbol);
                    if ((lastQuote != null) && (lastQuote.Price != tag.LastPrice))
                    {
                        tag.LastPrice = lastQuote.Price;
                        int pricingDecimalForSymbol = DecimalsManager.Instance.GetPricingDecimalForSymbol(tag.Symbol);
                        item.SubItems[this.columnHeader_4.DisplayIndex].Text = tag.LastPrice.ToString("N" + pricingDecimalForSymbol);
                        this.method_14(tag, item);
                    }
                }
                this.method_13();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MainModule.Instance.BrokerProvider.UpdateAccounts();
        }

        public void TradeHistoryItemAdded(HistoricalTrade historicalTrade_0)
        {
            base.Invoke(new Delegate25(this.method_1), new object[] { historicalTrade_0 });
        }

        public void TradeHistoryItemUpdated(HistoricalTrade historicalTrade_0)
        {
            base.Invoke(new Delegate24(this.method_0), new object[] { historicalTrade_0 });
        }

        public MainForm MyMainForm
        {
            get
            {
                return (base.MdiParent as MainForm);
            }
        }

        private delegate void Delegate24(HistoricalTrade historicalTrade_0);

        private delegate void Delegate25(HistoricalTrade historicalTrade_0);

        private delegate void Delegate26(Account account_0);

        private delegate void Delegate27(Account account_0);

        private delegate void Delegate28(AccountPosition accountPosition_0);

        private delegate void Delegate29();
    }
}

