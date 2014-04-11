namespace WealthLabPro
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
    public class Alerts : UserControl
    {
        private bool isStreaming;
        private bool bool_1;
        private ToolStripButton btnAutoStage;
        private ToolStripButton btnEmailAlerts;
        private ToolStripButton btnHelp;
        private ToolStripButton btnPlaceOrders;
        private ToolStripButton btnQuote;
        private ToolStripButton btnSelectAll;
        private ToolStripButton btnStageOrders;
        private ChartForm myChartForm;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_10;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private ColumnHeader columnHeader_5;
        private ColumnHeader columnHeader_6;
        private ColumnHeader columnHeader_7;
        private ColumnHeader columnHeader_8;
        private ColumnHeader columnHeader_9;
        private IContainer components;
        private ImageList imageList_0;
        private int int_0;
        private List<Alert> list_0;
        private SortableListView lvAlerts;
        private ToolStripMenuItem mniAutoStage;
        private ToolStripMenuItem mniCopy;
        private ToolStripMenuItem mniEditAlert;
        private ToolStripMenuItem mniEmailAlerts;
        private ToolStripMenuItem mniPlace;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrintAll;
        private ToolStripMenuItem mniQuote;
        private ToolStripMenuItem mniSelectAll;
        private ToolStripMenuItem mniStage;
        private ContextMenuStrip popupAlerts;
        private ToolStripSeparator sepHelp;
        private ToolStripSeparator sepHelpMenu;
        private ToolStripSeparator sepSelectAll;
        private ToolStripSeparator sepStage;
        private StatusStrip status;
        private ToolStripStatusLabel statusAlerts;
        private string string_0;
        private TabPage tabPage;
        private ToolStrip toolbar;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripComboBox tscmbAccountTradeType;

        public Alerts()
        {
            this.list_0 = new List<Alert>();
            this.int_0 = 9;
            this.string_0 = "";
            this.InitializeComponent();
            this.EnableTradeButtons();
        }

        public Alerts(ChartForm chartForm_1)
        {
            this.list_0 = new List<Alert>();
            this.int_0 = 9;
            this.string_0 = "";
            this.InitializeComponent();
            this.myChartForm = chartForm_1;
            this.EnableTradeButtons();
        }

        public void AddAlert(Alert alert)
        {
            string text = alert.AlertDate.ToShortDateString();
            if (alert.BarInterval > 0)
            {
                text = text + " " + alert.AlertDate.ToShortTimeString();
            }
            ListViewItem item = this.lvAlerts.Items.Add(text);
            item.Tag = alert;
            item.ImageIndex = (int) alert.AlertType;
            item.SubItems.Add(alert.Symbol);
            item.SubItems.Add(alert.Account);
            item.SubItems.Add(alert.AlertType.ToString());
            item.SubItems.Add(alert.Shares.ToString());
            item.SubItems.Add(alert.OrderType.ToString());
            if ((alert.OrderType != OrderType.Market) && (alert.OrderType != OrderType.AtClose))
            {
                item.SubItems.Add(alert.Price.ToString("N" + DecimalsManager.Instance.GetPricingDecimalForSymbol(alert.Symbol)));
            }
            else
            {
                item.SubItems.Add("");
            }
            BarDataScale scale = new BarDataScale(alert.Scale, alert.BarInterval);
            item.SubItems.Add(scale.ToString());
            item.SubItems.Add(alert.SignalName);
            if (!string.IsNullOrEmpty(this.tscmbAccountTradeType.Text) && ((alert.AlertType == TradeType.Buy) || (alert.AlertType == TradeType.Sell)))
            {
                alert.AccountTradeType = this.tscmbAccountTradeType.Text;
            }
            else if (string.IsNullOrEmpty(alert.AccountTradeType))
            {
                alert.AccountTradeType = MainModule.Instance.DefaultAccountTradeType(alert.Account, alert.AlertType.ToString());
            }
            item.SubItems.Add(alert.AccountTradeType);
            string name = "";
            if (MainModule.Instance.Strategies.LookupID(alert.StrategyID.ToString()) != null)
            {
                name = MainModule.Instance.Strategies.LookupID(alert.StrategyID.ToString()).Name;
            }
            item.SubItems.Add(name);
        }

        private void Alerts_Load(object sender, EventArgs e)
        {
            this.ShowAutoTradingState(MainModule.Instance.AutoTradingEnabled);
        }

        private void btnAutoStage_Click(object sender, EventArgs e)
        {
            ChartForm parentForm = (ChartForm) base.ParentForm;
            parentForm.AutoStage = this.btnAutoStage.Checked;
            this.mniAutoStage.Checked = this.btnAutoStage.Checked;
        }

        private void btnEmailAlerts_Click(object sender, EventArgs e)
        {
            ChartForm parentForm = (ChartForm) base.ParentForm;
            parentForm.EmailAlerts = this.btnEmailAlerts.Checked;
            this.mniEmailAlerts.Checked = this.btnEmailAlerts.Checked;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MainModule.Instance.ContextSensitiveHelp("alertview.htm");
        }

        public void Clear()
        {
            this.lvAlerts.Items.Clear();
        }

        public void CopyToClipboard()
        {
            ((ChartForm) base.ParentForm).CopyListViewToClipboard(this.lvAlerts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void EnableTradeButtons()
        {
            bool flag;
            if (!(flag = MainModule.Instance.AuthProvider.LoggedIn && (MainModule.Instance.BrokerProvider != null)))
            {
                bool flag2 = false;
                bool flag3 = false;
                if (this.lvAlerts.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in this.lvAlerts.SelectedItems)
                    {
                        Alert tag = (Alert) item.Tag;
                        if (tag.Account != "")
                        {
                            if (tag.Account.StartsWith("Paper"))
                            {
                                flag3 = true;
                            }
                            else
                            {
                                flag2 = true;
                            }
                        }
                        if (flag3 && flag2)
                        {
                            break;
                        }
                    }
                }
                if (!flag2)
                {
                    if (flag3)
                    {
                        flag = true;
                    }
                    else if (this.MyChartForm != null)
                    {
                        Strategy strategy = this.MyChartForm.Strategy;
                        if (strategy != null)
                        {
                            string accountNumber = strategy.AccountNumber;
                            if (accountNumber == "")
                            {
                                accountNumber = MainModule.Instance.DefaultAccountNumber;
                            }
                            if (accountNumber.StartsWith("Paper"))
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
            this.btnPlaceOrders.Enabled = (this.lvAlerts.SelectedItems.Count > 0) && flag;
            this.btnStageOrders.Enabled = this.lvAlerts.SelectedItems.Count > 0;
            this.btnAutoStage.Visible = this.IsStreaming;
            this.mniAutoStage.Visible = this.btnAutoStage.Visible;
            this.btnEmailAlerts.Visible = this.IsStreaming;
            this.mniEmailAlerts.Visible = this.btnEmailAlerts.Visible;
            if (!MainModule.Instance.EmailSettingsAvailable)
            {
                this.btnEmailAlerts.Enabled = false;
                this.mniEmailAlerts.Visible = this.btnEmailAlerts.Visible;
                this.btnEmailAlerts.ToolTipText = "Please add Email Settings in Preferences to enable this feature.";
                this.mniEmailAlerts.ToolTipText = "Please add Email Settings in Preferences to enable this feature.";
            }
            this.btnQuote.Visible = true;
            this.mniQuote.Visible = this.btnQuote.Visible;
            this.sepHelp.Visible = this.btnQuote.Visible;
            this.sepHelpMenu.Visible = this.sepHelp.Visible;
            this.method_1();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Alerts));
            this.toolbar = new ToolStrip();
            this.btnSelectAll = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.btnPlaceOrders = new ToolStripButton();
            this.btnStageOrders = new ToolStripButton();
            this.tscmbAccountTradeType = new ToolStripComboBox();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.btnAutoStage = new ToolStripButton();
            this.btnEmailAlerts = new ToolStripButton();
            this.btnQuote = new ToolStripButton();
            this.sepHelp = new ToolStripSeparator();
            this.btnHelp = new ToolStripButton();
            this.imageList_0 = new ImageList(this.components);
            this.status = new StatusStrip();
            this.statusAlerts = new ToolStripStatusLabel();
            this.popupAlerts = new ContextMenuStrip(this.components);
            this.mniSelectAll = new ToolStripMenuItem();
            this.mniEditAlert = new ToolStripMenuItem();
            this.sepSelectAll = new ToolStripSeparator();
            this.mniPlace = new ToolStripMenuItem();
            this.mniStage = new ToolStripMenuItem();
            this.sepStage = new ToolStripSeparator();
            this.mniAutoStage = new ToolStripMenuItem();
            this.mniEmailAlerts = new ToolStripMenuItem();
            this.mniQuote = new ToolStripMenuItem();
            this.sepHelpMenu = new ToolStripSeparator();
            this.mniCopy = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.mniPrintAll = new ToolStripMenuItem();
            this.lvAlerts = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_8 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.columnHeader_5 = new ColumnHeader();
            this.columnHeader_6 = new ColumnHeader();
            this.columnHeader_7 = new ColumnHeader();
            this.columnHeader_9 = new ColumnHeader();
            this.columnHeader_10 = new ColumnHeader();
            this.toolbar.SuspendLayout();
            this.status.SuspendLayout();
            this.popupAlerts.SuspendLayout();
            base.SuspendLayout();
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.btnSelectAll, this.toolStripSeparator3, this.btnPlaceOrders, this.btnStageOrders, this.tscmbAccountTradeType, this.toolStripSeparator4, this.btnAutoStage, this.btnEmailAlerts, this.btnQuote, this.sepHelp, this.btnHelp });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x2c0, 0x19);
            this.toolbar.TabIndex = 5;
            this.toolbar.Text = "toolStrip1";
            this.btnSelectAll.Image = (Image) resources.GetObject("btnSelectAll.Image");
            this.btnSelectAll.ImageTransparentColor = Color.Magenta;
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new Size(70, 0x16);
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.Click += new EventHandler(this.mniSelectAll_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 0x19);
            this.btnPlaceOrders.Enabled = false;
            this.btnPlaceOrders.Image = (Image) resources.GetObject("btnPlaceOrders.Image");
            this.btnPlaceOrders.ImageTransparentColor = Color.Magenta;
            this.btnPlaceOrders.Name = "btnPlaceOrders";
            this.btnPlaceOrders.Size = new Size(0x58, 0x16);
            this.btnPlaceOrders.Text = "Place Orders";
            this.btnPlaceOrders.ToolTipText = "Place Orders for selected Alerts";
            this.btnPlaceOrders.Click += new EventHandler(this.mniPlace_Click);
            this.btnStageOrders.Enabled = false;
            this.btnStageOrders.Image = (Image) resources.GetObject("btnStageOrders.Image");
            this.btnStageOrders.ImageTransparentColor = Color.Magenta;
            this.btnStageOrders.Name = "btnStageOrders";
            this.btnStageOrders.Size = new Size(0x5b, 0x16);
            this.btnStageOrders.Text = "Stage Orders";
            this.btnStageOrders.ToolTipText = "Stage Orders for selected Alerts";
            this.btnStageOrders.Click += new EventHandler(this.mniStage_Click);
            this.tscmbAccountTradeType.AutoToolTip = true;
            this.tscmbAccountTradeType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tscmbAccountTradeType.Name = "tscmbAccountTradeType";
            this.tscmbAccountTradeType.Size = new Size(80, 0x19);
            this.tscmbAccountTradeType.ToolTipText = "Trade Type for Buy and Sell Alerts";
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(6, 0x19);
            this.btnAutoStage.CheckOnClick = true;
            this.btnAutoStage.Image = (Image) resources.GetObject("btnAutoStage.Image");
            this.btnAutoStage.ImageTransparentColor = Color.Magenta;
            this.btnAutoStage.Name = "btnAutoStage";
            this.btnAutoStage.Size = new Size(0x52, 0x16);
            this.btnAutoStage.Text = "Auto-Stage";
            this.btnAutoStage.ToolTipText = "Automatically Stage Orders from Streaming Strategy Alerts";
            this.btnAutoStage.Visible = false;
            this.btnAutoStage.Click += new EventHandler(this.btnAutoStage_Click);
            this.btnEmailAlerts.CheckOnClick = true;
            this.btnEmailAlerts.Image = (Image) resources.GetObject("btnEmailAlerts.Image");
            this.btnEmailAlerts.ImageScaling = ToolStripItemImageScaling.None;
            this.btnEmailAlerts.ImageTransparentColor = Color.Magenta;
            this.btnEmailAlerts.Name = "btnEmailAlerts";
            this.btnEmailAlerts.Size = new Size(0x4f, 0x16);
            this.btnEmailAlerts.Text = "Auto-Email";
            this.btnEmailAlerts.ToolTipText = "Automatically Send Emails for Strategy Trade Alerts ";
            this.btnEmailAlerts.Visible = false;
            this.btnEmailAlerts.Click += new EventHandler(this.btnEmailAlerts_Click);
            this.btnQuote.Enabled = false;
            this.btnQuote.Image = (Image) resources.GetObject("btnQuote.Image");
            this.btnQuote.ImageTransparentColor = Color.Magenta;
            this.btnQuote.Name = "btnQuote";
            this.btnQuote.Size = new Size(0x99, 0x16);
            this.btnQuote.Text = "Monitor in Quotes Window";
            this.btnQuote.ToolTipText = "Monitor selected Alerts in Quotes Window";
            this.btnQuote.Click += new EventHandler(this.mniQuote_Click);
            this.sepHelp.Name = "sepHelp";
            this.sepHelp.Size = new Size(6, 0x19);
            this.btnHelp.Image = (Image) resources.GetObject("btnHelp.Image");
            this.btnHelp.ImageTransparentColor = Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x30, 0x16);
            this.btnHelp.Text = "Help";
            this.btnHelp.ToolTipText = "Help on Alerts";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.imageList_0.ImageStream = (ImageListStreamer) resources.GetObject("imgOrders.ImageStream");
            this.imageList_0.TransparentColor = Color.Silver;
            this.imageList_0.Images.SetKeyName(0, "buy.bmp");
            this.imageList_0.Images.SetKeyName(1, "sell.bmp");
            this.imageList_0.Images.SetKeyName(2, "short.bmp");
            this.imageList_0.Images.SetKeyName(3, "cover.bmp");
            this.status.Items.AddRange(new ToolStripItem[] { this.statusAlerts });
            this.status.Location = new Point(0, 0x1c9);
            this.status.Name = "status";
            this.status.Size = new Size(0x2c0, 0x16);
            this.status.TabIndex = 6;
            this.status.Text = "statusStrip1";
            this.statusAlerts.Name = "statusAlerts";
            this.statusAlerts.Size = new Size(0x2c, 0x11);
            this.statusAlerts.Text = "0 Alerts";
            this.popupAlerts.Items.AddRange(new ToolStripItem[] { this.mniSelectAll, this.mniEditAlert, this.sepSelectAll, this.mniPlace, this.mniStage, this.sepStage, this.mniAutoStage, this.mniEmailAlerts, this.mniQuote, this.sepHelpMenu, this.mniCopy, this.mniPrint, this.mniPrintAll });
            this.popupAlerts.Name = "popupAlerts";
            this.popupAlerts.Size = new Size(0x11b, 0xf2);
            this.mniSelectAll.Image = (Image) resources.GetObject("mniSelectAll.Image");
            this.mniSelectAll.ImageTransparentColor = Color.Fuchsia;
            this.mniSelectAll.Name = "mniSelectAll";
            this.mniSelectAll.Size = new Size(0x11a, 0x16);
            this.mniSelectAll.Text = "Select All";
            this.mniSelectAll.Click += new EventHandler(this.mniSelectAll_Click);
            this.mniEditAlert.Enabled = false;
            this.mniEditAlert.Image = (Image) resources.GetObject("mniEditAlert.Image");
            this.mniEditAlert.ImageTransparentColor = Color.Fuchsia;
            this.mniEditAlert.Name = "mniEditAlert";
            this.mniEditAlert.Size = new Size(0x11a, 0x16);
            this.mniEditAlert.Text = "Edit Alert ...";
            this.mniEditAlert.Click += new EventHandler(this.mniEditAlert_Click);
            this.sepSelectAll.Name = "sepSelectAll";
            this.sepSelectAll.Size = new Size(0x117, 6);
            this.mniPlace.Enabled = false;
            this.mniPlace.Image = (Image) resources.GetObject("mniPlace.Image");
            this.mniPlace.ImageTransparentColor = Color.Fuchsia;
            this.mniPlace.Name = "mniPlace";
            this.mniPlace.Size = new Size(0x11a, 0x16);
            this.mniPlace.Text = "Place Selected Orders";
            this.mniPlace.Click += new EventHandler(this.mniPlace_Click);
            this.mniStage.Enabled = false;
            this.mniStage.Image = (Image) resources.GetObject("mniStage.Image");
            this.mniStage.ImageTransparentColor = Color.Fuchsia;
            this.mniStage.Name = "mniStage";
            this.mniStage.Size = new Size(0x11a, 0x16);
            this.mniStage.Text = "Stage Selected Orders";
            this.mniStage.Click += new EventHandler(this.mniStage_Click);
            this.sepStage.Name = "sepStage";
            this.sepStage.Size = new Size(0x117, 6);
            this.mniAutoStage.Image = (Image) resources.GetObject("mniAutoStage.Image");
            this.mniAutoStage.ImageTransparentColor = Color.Fuchsia;
            this.mniAutoStage.Name = "mniAutoStage";
            this.mniAutoStage.Size = new Size(0x11a, 0x16);
            this.mniAutoStage.Text = "Auto-Stage";
            this.mniAutoStage.Visible = false;
            this.mniAutoStage.Click += new EventHandler(this.mniAutoStage_Click);
            this.mniEmailAlerts.Image = (Image) resources.GetObject("mniEmailAlerts.Image");
            this.mniEmailAlerts.ImageScaling = ToolStripItemImageScaling.None;
            this.mniEmailAlerts.Name = "mniEmailAlerts";
            this.mniEmailAlerts.Size = new Size(0x11a, 0x16);
            this.mniEmailAlerts.Text = "Auto-Email";
            this.mniEmailAlerts.Click += new EventHandler(this.mniEmailAlerts_Click);
            this.mniQuote.Enabled = false;
            this.mniQuote.Image = (Image) resources.GetObject("mniQuote.Image");
            this.mniQuote.ImageTransparentColor = Color.Fuchsia;
            this.mniQuote.Name = "mniQuote";
            this.mniQuote.Size = new Size(0x11a, 0x16);
            this.mniQuote.Text = "Monitor Selected Alerts in Quote Window";
            this.mniQuote.Click += new EventHandler(this.mniQuote_Click);
            this.sepHelpMenu.Name = "sepHelpMenu";
            this.sepHelpMenu.Size = new Size(0x117, 6);
            this.mniCopy.Image = (Image) resources.GetObject("mniCopy.Image");
            this.mniCopy.ImageTransparentColor = Color.Fuchsia;
            this.mniCopy.Name = "mniCopy";
            this.mniCopy.Size = new Size(0x11a, 0x16);
            this.mniCopy.Text = "Copy";
            this.mniCopy.Click += new EventHandler(this.mniCopy_Click);
            this.mniPrint.Image = (Image) resources.GetObject("mniPrint.Image");
            this.mniPrint.ImageTransparentColor = Color.Fuchsia;
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0x11a, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.mniPrintAll.Name = "mniPrintAll";
            this.mniPrintAll.Size = new Size(0x11a, 0x16);
            this.mniPrintAll.Text = "Print All";
            this.mniPrintAll.ToolTipText = "Print content from all tabs";
            this.mniPrintAll.Click += new EventHandler(this.mniPrintAll_Click);
            this.lvAlerts.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_1, this.columnHeader_8, this.columnHeader_2, this.columnHeader_3, this.columnHeader_4, this.columnHeader_5, this.columnHeader_6, this.columnHeader_7, this.columnHeader_9, this.columnHeader_10 });
            this.lvAlerts.ContextMenuStrip = this.popupAlerts;
            this.lvAlerts.Dock = DockStyle.Fill;
            this.lvAlerts.FullRowSelect = true;
            this.lvAlerts.HideSelection = false;
            this.lvAlerts.Location = new Point(0, 0x19);
            this.lvAlerts.Name = "lvAlerts";
            this.lvAlerts.Size = new Size(0x2c0, 0x1b0);
            this.lvAlerts.SmallImageList = this.imageList_0;
            this.lvAlerts.TabIndex = 7;
            this.lvAlerts.UseCompatibleStateImageBehavior = false;
            this.lvAlerts.View = View.Details;
            this.lvAlerts.SelectedIndexChanged += new EventHandler(this.lvAlerts_SelectedIndexChanged);
            this.lvAlerts.DoubleClick += new EventHandler(this.lvAlerts_DoubleClick);
            this.columnHeader_0.Tag = "DT";
            this.columnHeader_0.Text = "Alert Time";
            this.columnHeader_0.Width = 120;
            this.columnHeader_1.Tag = "S";
            this.columnHeader_1.Text = "Symbol";
            this.columnHeader_8.Text = "Account";
            this.columnHeader_8.Width = 80;
            this.columnHeader_2.Tag = "S";
            this.columnHeader_2.Text = "Action";
            this.columnHeader_3.Tag = "N";
            this.columnHeader_3.Text = "Qty";
            this.columnHeader_4.Tag = "S";
            this.columnHeader_4.Text = "Order Type";
            this.columnHeader_4.Width = 70;
            this.columnHeader_5.Tag = "N";
            this.columnHeader_5.Text = "Price";
            this.columnHeader_5.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_6.Tag = "S";
            this.columnHeader_6.Text = "Scale";
            this.columnHeader_7.Text = "Signal Name";
            this.columnHeader_7.Width = 80;
            this.columnHeader_9.Text = "Trade Type";
            this.columnHeader_10.Text = "Strategy Name";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(this.lvAlerts);
            base.Controls.Add(this.status);
            base.Controls.Add(this.toolbar);
            base.Name = "Alerts";
            base.Size = new Size(0x2c0, 0x1df);
            base.Load += new EventHandler(this.Alerts_Load);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.popupAlerts.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lvAlerts_DoubleClick(object sender, EventArgs e)
        {
            if (this.lvAlerts.SelectedItems.Count == 1)
            {
                Alert tag = (Alert) this.lvAlerts.SelectedItems[0].Tag;
                (base.ParentForm as ChartForm).SelectSymbol(tag.Symbol);
            }
        }

        private void lvAlerts_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableTradeButtons();
            bool flag = this.lvAlerts.SelectedItems.Count > 0;
            this.btnQuote.Enabled = flag;
            this.mniQuote.Enabled = flag;
            this.mniPlace.Enabled = this.btnPlaceOrders.Enabled;
            this.mniStage.Enabled = this.btnStageOrders.Enabled;
            this.mniEditAlert.Enabled = this.lvAlerts.SelectedItems.Count == 1;
            this.btnQuote.Visible = true;
            this.mniQuote.Visible = this.btnQuote.Visible;
            this.sepHelp.Visible = this.btnQuote.Visible;
            this.sepHelpMenu.Visible = this.sepHelp.Visible;
        }

        private void method_0(bool bool_2)
        {
            this.list_0.Clear();
            Alert tag = null;
            foreach (ListViewItem item in this.lvAlerts.SelectedItems)
            {
                tag = (Alert) item.Tag;
                if (tag != null)
                {
                    this.list_0.Add(tag);
                    string text = this.tscmbAccountTradeType.Text;
                    if (text != "")
                    {
                        tag.AccountTradeType = text;
                    }
                }
            }
            if (this.list_0.Count > 0)
            {
                MainModule.Instance.TradeManager.AddAlerts(this.list_0, bool_2, false);
                MainModule.Instance.FirstMainForm.OpenOrderManager();
                string account = tag.Account;
                if (account == "")
                {
                    account = MainModule.Instance.DefaultAccountNumber;
                }
                OrdersAlertsForm.Instance.SwitchToAccount(account);
            }
        }

        private void method_1()
        {
            if ((this.string_0 != MainModule.Instance.DefaultAccountNumber) || (this.bool_1 != MainModule.Instance.AuthProvider.LoggedIn))
            {
                string selectedItem = "";
                if (this.string_0 == MainModule.Instance.DefaultAccountNumber)
                {
                    if (this.tscmbAccountTradeType.SelectedIndex > -1)
                    {
                        selectedItem = (string) this.tscmbAccountTradeType.SelectedItem;
                    }
                }
                else
                {
                    this.string_0 = MainModule.Instance.DefaultAccountNumber;
                }
                this.bool_1 = MainModule.Instance.AuthProvider.LoggedIn;
                IList<string> list = MainModule.Instance.AccountTradeTypes(this.string_0);
                this.tscmbAccountTradeType.BeginUpdate();
                this.tscmbAccountTradeType.Items.Clear();
                foreach (string str2 in list)
                {
                    if (!string.IsNullOrEmpty(str2))
                    {
                        this.tscmbAccountTradeType.Items.Add(str2);
                    }
                }
                this.tscmbAccountTradeType.EndUpdate();
                if (this.tscmbAccountTradeType.Items.Count > 0)
                {
                    if (!string.IsNullOrEmpty(selectedItem))
                    {
                        this.tscmbAccountTradeType.SelectedIndex = this.tscmbAccountTradeType.Items.IndexOf(selectedItem);
                    }
                    if (this.tscmbAccountTradeType.SelectedIndex == -1)
                    {
                        this.tscmbAccountTradeType.SelectedIndex = 0;
                    }
                }
            }
        }

        private void mniAutoStage_Click(object sender, EventArgs e)
        {
            ChartForm parentForm = (ChartForm) base.ParentForm;
            parentForm.AutoStage = this.btnAutoStage.Checked;
            this.mniAutoStage.Checked = !this.btnAutoStage.Checked;
            this.btnAutoStage.Checked = this.mniAutoStage.Checked;
        }

        private void mniCopy_Click(object sender, EventArgs e)
        {
            this.CopyToClipboard();
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
                    item.SubItems[this.int_0].Text = tag.AccountTradeType;
                    this.EnableTradeButtons();
                }
            }
        }

        private void mniEmailAlerts_Click(object sender, EventArgs e)
        {
            ChartForm parentForm = (ChartForm) base.ParentForm;
            parentForm.EmailAlerts = !this.btnEmailAlerts.Checked;
            this.mniEmailAlerts.Checked = !this.btnEmailAlerts.Checked;
            this.btnEmailAlerts.Checked = this.mniEmailAlerts.Checked;
        }

        private void mniPlace_Click(object sender, EventArgs e)
        {
            this.method_0(true);
        }

        private void mniPrint_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private void mniPrintAll_Click(object sender, EventArgs e)
        {
            ((ChartForm) base.ParentForm).PrintAll();
        }

        private void mniQuote_Click(object sender, EventArgs e)
        {
            ChartForm parentForm = (ChartForm) base.ParentForm;
            MainForm myMainForm = parentForm.MyMainForm;
            if (MainModule.Instance.Authenticate())
            {
                foreach (ListViewItem item in this.lvAlerts.SelectedItems)
                {
                    Alert tag = (Alert) item.Tag;
                    if (tag.Account == "")
                    {
                        if (tag.Strategy != null)
                        {
                            tag.Account = tag.Strategy.AccountNumber;
                        }
                        if (tag.Account == "")
                        {
                            tag.Account = MainModule.Instance.DefaultAccountNumber;
                        }
                    }
                    string acctNum = tag.Account;
                    Account account = MainModule.Instance.TradeManager.FindAccount(acctNum);
                    QuotesForm quoteForm = myMainForm.GetQuoteForm((account == null) ? AutoTradingMode.Live : account.AutoTradingMode);
                    quoteForm.BringToFront();
                    quoteForm.WindowState = FormWindowState.Normal;
                    quoteForm.AddAlert(tag);
                }
            }
        }

        private void mniSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvAlerts.Items)
            {
                item.Selected = true;
            }
        }

        private void mniStage_Click(object sender, EventArgs e)
        {
            this.method_0(false);
        }

        public void Populate(SystemResults results)
        {
            this.method_1();
            this.lvAlerts.Items.Clear();
            foreach (Alert alert in results.Alerts)
            {
                this.AddAlert(alert);
            }
            this.EnableTradeButtons();
        }

        public void Print()
        {
            DataObject printObject = new DataObject();
            printObject.SetData(PrintReport.fmtTitle.Name, "Alerts");
            printObject.SetData(PrintReport.fmtListView.Name, this.lvAlerts);
            printObject.SetData(PrintReport.fmtBaseTitle.Name, MainModule.Instance.AuthProvider.ApplicationName);
            ChartForm parentForm = (ChartForm) base.ParentForm;
            parentForm.StrategySummary(ref printObject);
            PrintReport report = new PrintReport(printObject, true);
            PageSettings pageSettings = new PageSettings();
            parentForm.GetPageSettings(ref pageSettings);
            report.ShowPrintPreview = !MainModule.Instance.Settings.Get("HidePrintPreview", false);
            report.ShowPrintDialog = !MainModule.Instance.Settings.Get("HidePrintDialog", false);
            report.PrintGraphicReport(pageSettings);
        }

        public void ShowAutoTradingState(AutoTradingMode mode)
        {
            string str2;
            ChartForm parentForm = base.ParentForm as ChartForm;
            bool flag = false;
            string accountNumber = "";
            if (parentForm.Strategy != null)
            {
                accountNumber = parentForm.Strategy.AccountNumber;
            }
            if (accountNumber == "")
            {
                accountNumber = MainModule.Instance.DefaultAccountNumber;
            }
            if ((parentForm.Strategy != null) && accountNumber.StartsWith("PaperAccount"))
            {
                flag = MainModule.Instance.AutoTradingEnabled == AutoTradingMode.Paper;
            }
            else
            {
                flag = MainModule.Instance.AutoTradingEnabled == AutoTradingMode.Live;
            }
            if (flag)
            {
                str2 = "Place";
            }
            else
            {
                str2 = "Stage";
            }
            this.btnAutoStage.Text = "Auto-" + str2;
            this.mniAutoStage.Text = this.btnAutoStage.Text;
            this.btnAutoStage.ToolTipText = "Automatically " + str2 + " Orders from Strategy Alerts";
        }

        public void ShowLoggedInState(bool loggedIn)
        {
            this.EnableTradeButtons();
        }

        public void UpdateStatus()
        {
            if (this.lvAlerts.Items.Count == 1)
            {
                this.statusAlerts.Text = "1 Alert";
            }
            else
            {
                this.statusAlerts.Text = this.lvAlerts.Items.Count + " Alerts";
            }
            if (this.tabPage != null)
            {
                this.tabPage.Text = this.statusAlerts.Text;
            }
        }

        public bool AutoStage
        {
            get
            {
                return (this.btnAutoStage.Checked && this.IsStreaming);
            }
            set
            {
                this.btnAutoStage.Checked = value;
            }
        }

        public bool EmailAlerts
        {
            get
            {
                return (this.btnEmailAlerts.Checked && this.IsStreaming);
            }
            set
            {
                this.btnEmailAlerts.Checked = value;
            }
        }

        public bool IsStreaming
        {
            get
            {
                return this.isStreaming;
            }
            set
            {
                this.isStreaming = value;
                if (!this.isStreaming)
                {
                    this.btnEmailAlerts.Checked = false;
                }
                this.EnableTradeButtons();
            }
        }

        public ChartForm MyChartForm
        {
            get
            {
                return this.myChartForm;
            }
        }

        public TabPage Page
        {
            get
            {
                return this.tabPage;
            }
            set
            {
                this.tabPage = value;
            }
        }
    }
}

