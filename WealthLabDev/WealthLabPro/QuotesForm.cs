namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using WealthLab;

    public class QuotesForm : Form, IWorkspace, IConnectionStatus
    {
        [CompilerGenerated]
        private WealthLab.AutoTradingMode autoTradingMode_0;
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private ToolStripButton btnAddPriceTrigger;
        private ToolStripButton btnAddQuotes;
        private ToolStripButton btnAutoRemove;
        private ToolStripButton btnAutoSort;
        private ToolStripButton btnAutoStage;
        private ToolStripButton btnClear;
        private ToolStripButton btnConvertMarket;
        private ToolStripButton btnEmailAlerts;
        private ToolStripButton btnHelp;
        private ToolStripButton btnHelpAlerts;
        private ToolStripButton btnPlace;
        private ToolStripButton btnRemove;
        private ToolStripButton btnSelectAll;
        private ToolStripButton btnStage;
        private byte byte_0;
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
        private DateTime dateTime_0;
        private double double_0 = 100.0;
        private double double_1;
        private Font font_0;
        private IContainer components;
        private ImageList imageList_0;
        private ImageList imageList_1;
        private const int int_0 = 0;
        private const int int_1 = 1;
        private const int int_10 = 10;
        private const int int_11 = 11;
        private const int int_12 = 12;
        private int int_13 = 12;
        private int int_14 = 13;
        private const int int_2 = 2;
        private const int int_3 = 3;
        private const int int_4 = 4;
        private const int int_5 = 5;
        private const int int_6 = 6;
        private const int int_7 = 7;
        private const int int_8 = 8;
        private const int int_9 = 9;
        private ToolStripLabel lblAlerts;
        private ToolStripLabel lblGapFilter;
        private ToolStripLabel lblGapFilterpct;
        private ToolStripLabel lblPct;
        private ToolStripLabel lblQuotes;
        private ToolStripLabel lblThreshold;
        private List<Alert> list_0 = new List<Alert>();
        private ListView listView_0;
        private SortableListView lvAlerts;
        private SortableListView lvQuotes;
        private MarketHours marketHours_0;
        private ToolStripMenuItem mniAddPriceTrigger;
        private ToolStripMenuItem mniAddQuotes;
        private ToolStripMenuItem mniAutoStage;
        private ToolStripMenuItem mniClear;
        private ToolStripMenuItem mniClearAll;
        private ToolStripMenuItem mniClearTriggered;
        private ToolStripMenuItem mniCopyAlerts;
        private ToolStripMenuItem mniCopyQuotes;
        private ToolStripMenuItem mniEditAlert;
        private ToolStripMenuItem mniEditPriceTrigger;
        private ToolStripMenuItem mniEmailAlerts;
        private ToolStripMenuItem mniPlace;
        private ToolStripMenuItem mniPrintAlerts;
        private ToolStripMenuItem mniPrintQuotes;
        private ToolStripMenuItem mniRemoveAlerts;
        private ToolStripMenuItem mniRename;
        private ToolStripMenuItem mniRename2;
        private ToolStripMenuItem mniSaveTriggered;
        private ToolStripMenuItem mniSelectAll;
        private ToolStripMenuItem mniStage;
        private ContextMenuStrip popupAlerts;
        private ContextMenuStrip popupQuotes;
        private PrintPreview printPreview_0;
        private PrintReport printReport_0;
        private ToolStripMenuItem removeSelectedQuotesToolStripMenuItem;
        private SaveFileDialog saveFileDialog_0;
        private ToolStripSeparator sepAutoStage;
        private ToolStripSeparator sepClear;
        private ToolStripSeparator sepGenerate;
        private ToolStripSeparator sepHelp;
        private ToolStripSeparator sepPrint;
        private ToolStripSeparator sepRename;
        private ToolStripSeparator sepRename2;
        private ToolStripSeparator sepSelectAll;
        private ToolStripSeparator sepThreshold;
        private ToolStripSeparator sepTrigger;
        private SplitContainer split;
        private StatusStrip status;
        private ToolStripStatusLabel statusAlerts;
        private ToolStripStatusLabel statusQuotes;
        private StreamingQuoteManager streamingQuoteManager_0;
        private string string_0 = "";
        private SymbolParser symbolParser_0;
        private Timer timer_0;
        private ToolStrip toolbarAlerts;
        private ToolStrip toolbarQuotes;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem triggerSelectedQuotesNowToolStripMenuItem;
        private ToolStripTextBox txtGapFilter;
        private ToolStripTextBox txtThreshold;

        public QuotesForm()
        {
            this.InitializeComponent();
        }

        public void AddAlert(Alert alert)
        {
            if (alert.Account == "")
            {
                if (alert.Strategy != null)
                {
                    alert.Account = alert.Strategy.AccountNumber;
                }
                if (alert.Account == "")
                {
                    alert.Account = MainModule.Instance.DefaultAccountNumber;
                }
            }
            ListViewItem item = this.method_1(alert.Symbol);
            item.Tag = new Alert(alert);
            item.ImageIndex = (int) alert.AlertType;
            item.SubItems[6].Text = alert.Account;
            item.SubItems[7].Text = alert.AlertType.ToString();
            item.SubItems[8].Text = alert.Shares.ToString();
            item.SubItems[9].Text = alert.OrderType.ToString();
            string format = this.method_0(alert.Symbol);
            if ((alert.OrderType == OrderType.Limit) || (alert.OrderType == OrderType.Stop))
            {
                item.SubItems[10].Text = alert.Price.ToString(format);
            }
            double num = 0.0;
            item.SubItems[11].Text = num.ToString("N2");
            item.SubItems[11].Tag = num;
            item.SubItems[this.int_13].Text = alert.AccountTradeType;
            this.method_4(alert.Symbol);
            if (alert.OrderType == OrderType.Market)
            {
                this.bool_1 = false;
            }
            else if (alert.OrderType == OrderType.AtClose)
            {
                this.bool_2 = false;
            }
        }

        private void btnAddPriceTrigger_Click(object sender, EventArgs e)
        {
            AddPriceTriggerForm form = new AddPriceTriggerForm(this.IsPaper);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Alert alert = form.Alert;
                this.AddAlert(alert);
            }
        }

        private void btnAddQuotes_Click(object sender, EventArgs e)
        {
            string str = InputBox.Show("Add Symbols to Quote", "Symbols (separate by comma or space):", "", CharacterCasing.Upper);
            this.symbolParser_0.Text = str;
            foreach (string str2 in this.symbolParser_0.Symbols)
            {
                this.method_1(str2);
            }
            this.method_2(this.symbolParser_0.Symbols);
        }

        private void btnAutoRemove_Click(object sender, EventArgs e)
        {
            if (this.btnAutoRemove.Checked)
            {
                for (int i = this.lvQuotes.Items.Count - 1; i >= 0; i--)
                {
                    ListViewItem item = this.lvQuotes.Items[i];
                    Alert tag = item.Tag as Alert;
                    if ((tag != null) && tag.Triggered)
                    {
                        this.method_9(item);
                    }
                }
            }
            MainModule.Instance.Settings.Set("AutoRemove", this.btnAutoRemove.Checked);
        }

        private void btnAutoSort_Click(object sender, EventArgs e)
        {
            MainModule.Instance.Settings.Set("AutoSort", this.btnAutoSort.Checked);
        }

        private void btnAutoStage_Click(object sender, EventArgs e)
        {
            this.btnAutoStage.Checked = !this.btnAutoStage.Checked;
            this.mniAutoStage.Checked = this.btnAutoStage.Checked;
            MainModule.Instance.Settings.Set("Orders", this.btnAutoStage.Checked);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (this.streamingQuoteManager_0.Provider != null)
            {
                this.streamingQuoteManager_0.Provider.ClearRequests(this.streamingQuoteManager_0);
            }
            this.lvQuotes.Items.Clear();
            this.method_11();
        }

        private void btnConvertMarket_Click(object sender, EventArgs e)
        {
            MainModule.Instance.Settings.Set("ConvertToMarketOrder", this.btnConvertMarket.Checked);
        }

        private void btnEmailAlerts_Click(object sender, EventArgs e)
        {
            this.btnEmailAlerts.Checked = !this.btnEmailAlerts.Checked;
            this.mniEmailAlerts.Checked = this.btnEmailAlerts.Checked;
            MainModule.Instance.Settings.Set("EmailAlerts", this.btnEmailAlerts.Checked);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MainModule.Instance.ContextSensitiveHelp("quotespane.htm");
        }

        private void btnHelpAlerts_Click(object sender, EventArgs e)
        {
            MainModule.Instance.ContextSensitiveHelp("alertspane.htm");
        }

        private void btnPlace_Click(object sender, EventArgs e)
        {
            this.method_10(true);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            for (int i = this.lvAlerts.Items.Count - 1; i >= 0; i--)
            {
                if (this.lvAlerts.Items[i].Selected)
                {
                    this.lvAlerts.Items.RemoveAt(i);
                }
            }
            this.method_11();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvAlerts.Items)
            {
                item.Selected = true;
            }
        }

        private void btnStage_Click(object sender, EventArgs e)
        {
            this.method_10(false);
        }

        public void Connect()
        {
        }

        public void Connect(bool reconnect)
        {
            if (reconnect)
            {
                List<string> list = new List<string>();
                foreach (ListViewItem item in this.lvQuotes.Items)
                {
                    list.Add(item.SubItems[1].Text);
                }
                if (list.Count > 0)
                {
                    this.method_2(list);
                }
            }
        }

        public void CopyToClipboard()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Quotes");
            builder.Append(Environment.NewLine);
            foreach (ColumnHeader header2 in this.lvQuotes.Columns)
            {
                builder.Append(header2.Text);
                builder.Append('\t');
            }
            builder.Append(Environment.NewLine);
            foreach (ListViewItem item2 in this.lvQuotes.Items)
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

        public void Disconnect()
        {
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(QuotesForm));
            this.status = new StatusStrip();
            this.statusQuotes = new ToolStripStatusLabel();
            this.statusAlerts = new ToolStripStatusLabel();
            this.split = new SplitContainer();
            this.lvQuotes = new SortableListView();
            this.columnHeader_22 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_26 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_5 = new ColumnHeader();
            this.columnHeader_6 = new ColumnHeader();
            this.columnHeader_7 = new ColumnHeader();
            this.columnHeader_8 = new ColumnHeader();
            this.columnHeader_9 = new ColumnHeader();
            this.columnHeader_24 = new ColumnHeader();
            this.popupQuotes = new ContextMenuStrip(this.components);
            this.mniEditPriceTrigger = new ToolStripMenuItem();
            this.removeSelectedQuotesToolStripMenuItem = new ToolStripMenuItem();
            this.triggerSelectedQuotesNowToolStripMenuItem = new ToolStripMenuItem();
            this.mniClearTriggered = new ToolStripMenuItem();
            this.mniClear = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.mniAddQuotes = new ToolStripMenuItem();
            this.mniAddPriceTrigger = new ToolStripMenuItem();
            this.sepRename = new ToolStripSeparator();
            this.mniRename = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.mniCopyQuotes = new ToolStripMenuItem();
            this.mniPrintQuotes = new ToolStripMenuItem();
            this.imageList_0 = new ImageList(this.components);
            this.imageList_1 = new ImageList(this.components);
            this.toolbarQuotes = new ToolStrip();
            this.lblQuotes = new ToolStripLabel();
            this.btnAddQuotes = new ToolStripButton();
            this.btnAddPriceTrigger = new ToolStripButton();
            this.btnClear = new ToolStripButton();
            this.sepTrigger = new ToolStripSeparator();
            this.lblThreshold = new ToolStripLabel();
            this.txtThreshold = new ToolStripTextBox();
            this.lblPct = new ToolStripLabel();
            this.lblGapFilter = new ToolStripLabel();
            this.txtGapFilter = new ToolStripTextBox();
            this.lblGapFilterpct = new ToolStripLabel();
            this.sepThreshold = new ToolStripSeparator();
            this.btnAutoSort = new ToolStripButton();
            this.btnAutoRemove = new ToolStripButton();
            this.sepHelp = new ToolStripSeparator();
            this.btnHelp = new ToolStripButton();
            this.lvAlerts = new SortableListView();
            this.columnHeader_10 = new ColumnHeader();
            this.columnHeader_12 = new ColumnHeader();
            this.columnHeader_11 = new ColumnHeader();
            this.columnHeader_13 = new ColumnHeader();
            this.columnHeader_14 = new ColumnHeader();
            this.columnHeader_15 = new ColumnHeader();
            this.columnHeader_16 = new ColumnHeader();
            this.columnHeader_17 = new ColumnHeader();
            this.columnHeader_18 = new ColumnHeader();
            this.columnHeader_19 = new ColumnHeader();
            this.columnHeader_20 = new ColumnHeader();
            this.columnHeader_21 = new ColumnHeader();
            this.columnHeader_23 = new ColumnHeader();
            this.columnHeader_25 = new ColumnHeader();
            this.popupAlerts = new ContextMenuStrip(this.components);
            this.mniSelectAll = new ToolStripMenuItem();
            this.mniEditAlert = new ToolStripMenuItem();
            this.sepSelectAll = new ToolStripSeparator();
            this.mniPlace = new ToolStripMenuItem();
            this.mniStage = new ToolStripMenuItem();
            this.mniRemoveAlerts = new ToolStripMenuItem();
            this.mniClearAll = new ToolStripMenuItem();
            this.sepAutoStage = new ToolStripSeparator();
            this.mniAutoStage = new ToolStripMenuItem();
            this.sepClear = new ToolStripSeparator();
            this.mniEmailAlerts = new ToolStripMenuItem();
            this.mniSaveTriggered = new ToolStripMenuItem();
            this.sepRename2 = new ToolStripSeparator();
            this.mniRename2 = new ToolStripMenuItem();
            this.sepPrint = new ToolStripSeparator();
            this.mniCopyAlerts = new ToolStripMenuItem();
            this.mniPrintAlerts = new ToolStripMenuItem();
            this.toolbarAlerts = new ToolStrip();
            this.lblAlerts = new ToolStripLabel();
            this.btnSelectAll = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.btnPlace = new ToolStripButton();
            this.btnStage = new ToolStripButton();
            this.btnRemove = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.btnAutoStage = new ToolStripButton();
            this.btnEmailAlerts = new ToolStripButton();
            this.btnConvertMarket = new ToolStripButton();
            this.sepGenerate = new ToolStripSeparator();
            this.btnHelpAlerts = new ToolStripButton();
            this.timer_0 = new Timer(this.components);
            this.saveFileDialog_0 = new SaveFileDialog();
            this.symbolParser_0 = new SymbolParser(this.components);
            this.streamingQuoteManager_0 = new StreamingQuoteManager(this.components);
            this.marketHours_0 = new MarketHours(this.components);
            this.status.SuspendLayout();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.popupQuotes.SuspendLayout();
            this.toolbarQuotes.SuspendLayout();
            this.popupAlerts.SuspendLayout();
            this.toolbarAlerts.SuspendLayout();
            base.SuspendLayout();
            this.status.Items.AddRange(new ToolStripItem[] { this.statusQuotes, this.statusAlerts });
            this.status.Location = new Point(0, 0x1b7);
            this.status.Name = "status";
            this.status.Size = new Size(0x34d, 0x16);
            this.status.TabIndex = 2;
            this.status.Text = "statusStrip2";
            this.statusQuotes.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusQuotes.Name = "statusQuotes";
            this.statusQuotes.Size = new Size(0x37, 0x11);
            this.statusQuotes.Text = "0 Quotes";
            this.statusAlerts.Name = "statusAlerts";
            this.statusAlerts.Size = new Size(0x2c, 0x11);
            this.statusAlerts.Text = "0 Alerts";
            this.split.BackColor = SystemColors.ControlDark;
            this.split.Dock = DockStyle.Fill;
            this.split.Location = new Point(0, 0);
            this.split.Name = "split";
            this.split.Orientation = Orientation.Horizontal;
            this.split.Panel1.BackColor = SystemColors.Control;
            this.split.Panel1.Controls.Add(this.lvQuotes);
            this.split.Panel1.Controls.Add(this.toolbarQuotes);
            this.split.Panel2.BackColor = SystemColors.Control;
            this.split.Panel2.Controls.Add(this.lvAlerts);
            this.split.Panel2.Controls.Add(this.toolbarAlerts);
            this.split.Size = new Size(0x34d, 0x1b7);
            this.split.SplitterDistance = 0xfd;
            this.split.TabIndex = 3;
            this.lvQuotes.AllowDrop = true;
            this.lvQuotes.Columns.AddRange(new ColumnHeader[] { this.columnHeader_22, this.columnHeader_1, this.columnHeader_26, this.columnHeader_2, this.columnHeader_3, this.columnHeader_4, this.columnHeader_0, this.columnHeader_5, this.columnHeader_6, this.columnHeader_7, this.columnHeader_8, this.columnHeader_9, this.columnHeader_24 });
            this.lvQuotes.ContextMenuStrip = this.popupQuotes;
            this.lvQuotes.Dock = DockStyle.Fill;
            this.lvQuotes.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lvQuotes.FullRowSelect = true;
            this.lvQuotes.HideSelection = false;
            this.lvQuotes.Location = new Point(0, 0x19);
            this.lvQuotes.Name = "lvQuotes";
            this.lvQuotes.OwnerDraw = true;
            this.lvQuotes.Size = new Size(0x34d, 0xe4);
            this.lvQuotes.SmallImageList = this.imageList_0;
            this.lvQuotes.StateImageList = this.imageList_1;
            this.lvQuotes.TabIndex = 3;
            this.lvQuotes.UseCompatibleStateImageBehavior = false;
            this.lvQuotes.View = View.Details;
            this.lvQuotes.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.lvQuotes_DrawColumnHeader);
            this.lvQuotes.DrawItem += new DrawListViewItemEventHandler(this.lvQuotes_DrawItem);
            this.lvQuotes.SelectedIndexChanged += new EventHandler(this.lvQuotes_SelectedIndexChanged);
            this.lvQuotes.DoubleClick += new EventHandler(this.mniEditPriceTrigger_Click);
            this.lvQuotes.DragDrop += new DragEventHandler(this.lvQuotes_DragDrop);
            this.lvQuotes.DragEnter += new DragEventHandler(this.lvQuotes_DragEnter);
            this.lvQuotes.DragOver += new DragEventHandler(this.lvQuotes_DragOver);
            this.lvQuotes.DrawSubItem += new DrawListViewSubItemEventHandler(this.lvQuotes_DrawSubItem);
            this.columnHeader_22.Text = "Time";
            this.columnHeader_22.Width = 0x69;
            this.columnHeader_1.Tag = "S";
            this.columnHeader_1.Text = "Symbol";
            this.columnHeader_26.Text = "Open";
            this.columnHeader_2.Tag = "N";
            this.columnHeader_2.Text = "Last Price";
            this.columnHeader_2.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_3.Tag = "N";
            this.columnHeader_3.Text = "Change";
            this.columnHeader_3.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_4.Tag = "N";
            this.columnHeader_4.Text = "% Change";
            this.columnHeader_4.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_0.Tag = "S";
            this.columnHeader_0.Text = "Account";
            this.columnHeader_0.Width = 70;
            this.columnHeader_5.Tag = "S";
            this.columnHeader_5.Text = "Action";
            this.columnHeader_6.Tag = "N";
            this.columnHeader_6.Text = "Quantity";
            this.columnHeader_6.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_7.Tag = "S";
            this.columnHeader_7.Text = "Order Type";
            this.columnHeader_7.Width = 70;
            this.columnHeader_8.Tag = "N";
            this.columnHeader_8.Text = "Price";
            this.columnHeader_8.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_9.Tag = "double";
            this.columnHeader_9.Text = "Threshold";
            this.columnHeader_9.Width = 100;
            this.columnHeader_24.Text = "Trade Type";
            this.popupQuotes.Items.AddRange(new ToolStripItem[] { this.mniEditPriceTrigger, this.removeSelectedQuotesToolStripMenuItem, this.triggerSelectedQuotesNowToolStripMenuItem, this.mniClearTriggered, this.mniClear, this.toolStripSeparator2, this.mniAddQuotes, this.mniAddPriceTrigger, this.sepRename, this.mniRename, this.toolStripSeparator4, this.mniCopyQuotes, this.mniPrintQuotes });
            this.popupQuotes.Name = "popupQuotes";
            this.popupQuotes.Size = new Size(0xef, 0xf2);
            this.mniEditPriceTrigger.Enabled = false;
            this.mniEditPriceTrigger.Image = (Image) resources.GetObject("mniEditPriceTrigger.Image");
            this.mniEditPriceTrigger.ImageTransparentColor = Color.Fuchsia;
            this.mniEditPriceTrigger.Name = "mniEditPriceTrigger";
            this.mniEditPriceTrigger.Size = new Size(0xee, 0x16);
            this.mniEditPriceTrigger.Text = "Edit Price Trigger ...";
            this.mniEditPriceTrigger.Click += new EventHandler(this.mniEditPriceTrigger_Click);
            this.removeSelectedQuotesToolStripMenuItem.Enabled = false;
            this.removeSelectedQuotesToolStripMenuItem.Image = (Image) resources.GetObject("removeSelectedQuotesToolStripMenuItem.Image");
            this.removeSelectedQuotesToolStripMenuItem.ImageTransparentColor = Color.Fuchsia;
            this.removeSelectedQuotesToolStripMenuItem.Name = "removeSelectedQuotesToolStripMenuItem";
            this.removeSelectedQuotesToolStripMenuItem.Size = new Size(0xee, 0x16);
            this.removeSelectedQuotesToolStripMenuItem.Text = "Remove selected items";
            this.removeSelectedQuotesToolStripMenuItem.Click += new EventHandler(this.removeSelectedQuotesToolStripMenuItem_Click);
            this.triggerSelectedQuotesNowToolStripMenuItem.Enabled = false;
            this.triggerSelectedQuotesNowToolStripMenuItem.Image = (Image) resources.GetObject("triggerSelectedQuotesNowToolStripMenuItem.Image");
            this.triggerSelectedQuotesNowToolStripMenuItem.ImageTransparentColor = Color.Silver;
            this.triggerSelectedQuotesNowToolStripMenuItem.Name = "triggerSelectedQuotesNowToolStripMenuItem";
            this.triggerSelectedQuotesNowToolStripMenuItem.Size = new Size(0xee, 0x16);
            this.triggerSelectedQuotesNowToolStripMenuItem.Text = "Trigger selected items now";
            this.triggerSelectedQuotesNowToolStripMenuItem.Click += new EventHandler(this.triggerSelectedQuotesNowToolStripMenuItem_Click);
            this.mniClearTriggered.Image = (Image) resources.GetObject("mniClearTriggered.Image");
            this.mniClearTriggered.ImageTransparentColor = Color.Silver;
            this.mniClearTriggered.Name = "mniClearTriggered";
            this.mniClearTriggered.Size = new Size(0xee, 0x16);
            this.mniClearTriggered.Text = "Clear Triggered items";
            this.mniClearTriggered.Click += new EventHandler(this.mniClearTriggered_Click);
            this.mniClear.Image = (Image) resources.GetObject("mniClear.Image");
            this.mniClear.ImageTransparentColor = Color.Fuchsia;
            this.mniClear.Name = "mniClear";
            this.mniClear.Size = new Size(0xee, 0x16);
            this.mniClear.Text = "Clear all items";
            this.mniClear.Click += new EventHandler(this.btnClear_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(0xeb, 6);
            this.mniAddQuotes.Image = (Image) resources.GetObject("mniAddQuotes.Image");
            this.mniAddQuotes.ImageTransparentColor = Color.Fuchsia;
            this.mniAddQuotes.Name = "mniAddQuotes";
            this.mniAddQuotes.Size = new Size(0xee, 0x16);
            this.mniAddQuotes.Text = "Add Quote(s) ...";
            this.mniAddQuotes.Click += new EventHandler(this.btnAddQuotes_Click);
            this.mniAddPriceTrigger.Image = (Image) resources.GetObject("mniAddPriceTrigger.Image");
            this.mniAddPriceTrigger.ImageTransparentColor = Color.Fuchsia;
            this.mniAddPriceTrigger.Name = "mniAddPriceTrigger";
            this.mniAddPriceTrigger.Size = new Size(0xee, 0x16);
            this.mniAddPriceTrigger.Text = "Add a Price Trigger";
            this.mniAddPriceTrigger.Click += new EventHandler(this.btnAddPriceTrigger_Click);
            this.sepRename.Name = "sepRename";
            this.sepRename.Size = new Size(0xeb, 6);
            this.mniRename.Image = (Image) resources.GetObject("mniRename.Image");
            this.mniRename.ImageTransparentColor = Color.Fuchsia;
            this.mniRename.Name = "mniRename";
            this.mniRename.Size = new Size(0xee, 0x16);
            this.mniRename.Text = "Rename this Quotes Window ...";
            this.mniRename.Click += new EventHandler(this.mniRename2_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(0xeb, 6);
            this.mniCopyQuotes.Image = (Image) resources.GetObject("mniCopyQuotes.Image");
            this.mniCopyQuotes.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyQuotes.Name = "mniCopyQuotes";
            this.mniCopyQuotes.Size = new Size(0xee, 0x16);
            this.mniCopyQuotes.Text = "Copy Quotes to Clipboard";
            this.mniCopyQuotes.Click += new EventHandler(this.mniCopyQuotes_Click);
            this.mniPrintQuotes.Image = (Image) resources.GetObject("mniPrintQuotes.Image");
            this.mniPrintQuotes.Name = "mniPrintQuotes";
            this.mniPrintQuotes.Size = new Size(0xee, 0x16);
            this.mniPrintQuotes.Text = "Print Quotes";
            this.mniPrintQuotes.Click += new EventHandler(this.mniPrintQuotes_Click);
            this.imageList_0.ImageStream = (ImageListStreamer) resources.GetObject("imagesAlerts.ImageStream");
            this.imageList_0.TransparentColor = Color.Silver;
            this.imageList_0.Images.SetKeyName(0, "buy.bmp");
            this.imageList_0.Images.SetKeyName(1, "sell.bmp");
            this.imageList_0.Images.SetKeyName(2, "short.bmp");
            this.imageList_0.Images.SetKeyName(3, "cover.bmp");
            this.imageList_0.Images.SetKeyName(4, "GapFilter.bmp");
            this.imageList_1.ImageStream = (ImageListStreamer) resources.GetObject("imagesState.ImageStream");
            this.imageList_1.TransparentColor = Color.Silver;
            this.imageList_1.Images.SetKeyName(0, "alertsent.bmp");
            this.imageList_1.Images.SetKeyName(1, "GapFilter.bmp");
            this.toolbarQuotes.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbarQuotes.Items.AddRange(new ToolStripItem[] { this.lblQuotes, this.btnAddQuotes, this.btnAddPriceTrigger, this.btnClear, this.sepTrigger, this.lblThreshold, this.txtThreshold, this.lblPct, this.lblGapFilter, this.txtGapFilter, this.lblGapFilterpct, this.sepThreshold, this.btnAutoSort, this.btnAutoRemove, this.sepHelp, this.btnHelp });
            this.toolbarQuotes.Location = new Point(0, 0);
            this.toolbarQuotes.Name = "toolbarQuotes";
            this.toolbarQuotes.Size = new Size(0x34d, 0x19);
            this.toolbarQuotes.TabIndex = 0;
            this.toolbarQuotes.Text = "toolStrip1";
            this.lblQuotes.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.lblQuotes.Name = "lblQuotes";
            this.lblQuotes.Size = new Size(0x2f, 0x16);
            this.lblQuotes.Text = "Quotes";
            this.btnAddQuotes.Image = (Image) resources.GetObject("btnAddQuotes.Image");
            this.btnAddQuotes.ImageTransparentColor = Color.Magenta;
            this.btnAddQuotes.Name = "btnAddQuotes";
            this.btnAddQuotes.Size = new Size(0x5c, 0x16);
            this.btnAddQuotes.Text = "Add Quote(s)";
            this.btnAddQuotes.ToolTipText = "Add one or more Symbols to Quote";
            this.btnAddQuotes.Click += new EventHandler(this.btnAddQuotes_Click);
            this.btnAddPriceTrigger.Image = (Image) resources.GetObject("btnAddPriceTrigger.Image");
            this.btnAddPriceTrigger.ImageTransparentColor = Color.Magenta;
            this.btnAddPriceTrigger.Name = "btnAddPriceTrigger";
            this.btnAddPriceTrigger.Size = new Size(0x6d, 0x16);
            this.btnAddPriceTrigger.Text = "Add Price Trigger";
            this.btnAddPriceTrigger.ToolTipText = "Add a Price Trigger";
            this.btnAddPriceTrigger.Click += new EventHandler(this.btnAddPriceTrigger_Click);
            this.btnClear.Image = (Image) resources.GetObject("btnClear.Image");
            this.btnClear.ImageTransparentColor = Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(0x42, 0x16);
            this.btnClear.Text = "Clear All";
            this.btnClear.ToolTipText = "Clear all Quotes now";
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            this.sepTrigger.Name = "sepTrigger";
            this.sepTrigger.Size = new Size(6, 0x19);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new Size(0x5f, 0x16);
            this.lblThreshold.Text = "Trigger Threshold:";
            this.txtThreshold.Name = "txtThreshold";
            this.txtThreshold.Size = new Size(30, 0x19);
            this.txtThreshold.Text = "100";
            this.txtThreshold.ToolTipText = "How close should Quotes get to Target before triggering Alerts";
            this.txtThreshold.Leave += new EventHandler(this.txtThreshold_Leave);
            this.lblPct.Name = "lblPct";
            this.lblPct.Size = new Size(0x12, 0x16);
            this.lblPct.Text = "%";
            this.lblGapFilter.Name = "lblGapFilter";
            this.lblGapFilter.Size = new Size(0x39, 0x16);
            this.lblGapFilter.Text = "Gap Filter:";
            this.txtGapFilter.Name = "txtGapFilter";
            this.txtGapFilter.Size = new Size(30, 0x19);
            this.txtGapFilter.Text = "100";
            this.txtGapFilter.Leave += new EventHandler(this.txtGapFilter_Leave);
            this.lblGapFilterpct.Name = "lblGapFilterpct";
            this.lblGapFilterpct.Size = new Size(0x12, 0x16);
            this.lblGapFilterpct.Text = "%";
            this.sepThreshold.Name = "sepThreshold";
            this.sepThreshold.Size = new Size(6, 0x19);
            this.btnAutoSort.CheckOnClick = true;
            this.btnAutoSort.Image = (Image) resources.GetObject("btnAutoSort.Image");
            this.btnAutoSort.ImageTransparentColor = Color.Magenta;
            this.btnAutoSort.Name = "btnAutoSort";
            this.btnAutoSort.Size = new Size(0x4a, 0x16);
            this.btnAutoSort.Text = "Auto-Sort";
            this.btnAutoSort.ToolTipText = "Automatically Sort Price Triggers by Threshold";
            this.btnAutoSort.Click += new EventHandler(this.btnAutoSort_Click);
            this.btnAutoRemove.Checked = true;
            this.btnAutoRemove.CheckOnClick = true;
            this.btnAutoRemove.CheckState = CheckState.Checked;
            this.btnAutoRemove.Image = (Image) resources.GetObject("btnAutoRemove.Image");
            this.btnAutoRemove.ImageTransparentColor = Color.Magenta;
            this.btnAutoRemove.Name = "btnAutoRemove";
            this.btnAutoRemove.Size = new Size(0x5d, 0x16);
            this.btnAutoRemove.Text = "Auto-Remove";
            this.btnAutoRemove.ToolTipText = "Remove Quotes when they Trigger Alerts";
            this.btnAutoRemove.Click += new EventHandler(this.btnAutoRemove_Click);
            this.sepHelp.Name = "sepHelp";
            this.sepHelp.Size = new Size(6, 0x19);
            this.btnHelp.Image = (Image) resources.GetObject("btnHelp.Image");
            this.btnHelp.ImageTransparentColor = Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x30, 0x16);
            this.btnHelp.Text = "Help";
            this.btnHelp.ToolTipText = "Help on Quotes";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.lvAlerts.Columns.AddRange(new ColumnHeader[] { this.columnHeader_10, this.columnHeader_12, this.columnHeader_11, this.columnHeader_13, this.columnHeader_14, this.columnHeader_15, this.columnHeader_16, this.columnHeader_17, this.columnHeader_18, this.columnHeader_19, this.columnHeader_20, this.columnHeader_21, this.columnHeader_23, this.columnHeader_25 });
            this.lvAlerts.ContextMenuStrip = this.popupAlerts;
            this.lvAlerts.Dock = DockStyle.Fill;
            this.lvAlerts.FullRowSelect = true;
            this.lvAlerts.HideSelection = false;
            this.lvAlerts.Location = new Point(0, 0x19);
            this.lvAlerts.Name = "lvAlerts";
            this.lvAlerts.Size = new Size(0x34d, 0x9d);
            this.lvAlerts.TabIndex = 15;
            this.lvAlerts.UseCompatibleStateImageBehavior = false;
            this.lvAlerts.View = View.Details;
            this.lvAlerts.SelectedIndexChanged += new EventHandler(this.lvAlerts_SelectedIndexChanged);
            this.lvAlerts.DoubleClick += new EventHandler(this.mniEditAlert_Click);
            this.columnHeader_10.Tag = "DT";
            this.columnHeader_10.Text = "Alert Time";
            this.columnHeader_10.Width = 120;
            this.columnHeader_12.Tag = "S";
            this.columnHeader_12.Text = "Symbol";
            this.columnHeader_11.Tag = "S";
            this.columnHeader_11.Text = "Account";
            this.columnHeader_13.Tag = "S";
            this.columnHeader_13.Text = "Action";
            this.columnHeader_14.Tag = "N";
            this.columnHeader_14.Text = "Qty";
            this.columnHeader_15.Tag = "S";
            this.columnHeader_15.Text = "Order Type";
            this.columnHeader_15.Width = 70;
            this.columnHeader_16.Tag = "N";
            this.columnHeader_16.Text = "Price";
            this.columnHeader_16.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_17.Tag = "S";
            this.columnHeader_17.Text = "Strategy";
            this.columnHeader_17.Width = 120;
            this.columnHeader_18.Tag = "S";
            this.columnHeader_18.Text = "DataSet";
            this.columnHeader_19.Tag = "S";
            this.columnHeader_19.Text = "Range";
            this.columnHeader_20.Tag = "S";
            this.columnHeader_20.Text = "Scale";
            this.columnHeader_21.Text = "Position Size";
            this.columnHeader_23.Text = "Signal Name";
            this.columnHeader_23.Width = 80;
            this.columnHeader_25.Text = "Trade Type";
            this.popupAlerts.Items.AddRange(new ToolStripItem[] { 
                this.mniSelectAll, this.mniEditAlert, this.sepSelectAll, this.mniPlace, this.mniStage, this.mniRemoveAlerts, this.mniClearAll, this.sepAutoStage, this.mniAutoStage, this.sepClear, this.mniEmailAlerts, this.mniSaveTriggered, this.sepRename2, this.mniRename2, this.sepPrint, this.mniCopyAlerts, 
                this.mniPrintAlerts
             });
            this.popupAlerts.Name = "popupAlerts";
            this.popupAlerts.Size = new Size(0x10b, 0x12a);
            this.mniSelectAll.Image = (Image) resources.GetObject("mniSelectAll.Image");
            this.mniSelectAll.ImageTransparentColor = Color.Fuchsia;
            this.mniSelectAll.Name = "mniSelectAll";
            this.mniSelectAll.Size = new Size(0x10a, 0x16);
            this.mniSelectAll.Text = "Select All";
            this.mniSelectAll.Click += new EventHandler(this.btnSelectAll_Click);
            this.mniEditAlert.Enabled = false;
            this.mniEditAlert.Image = (Image) resources.GetObject("mniEditAlert.Image");
            this.mniEditAlert.ImageTransparentColor = Color.Fuchsia;
            this.mniEditAlert.Name = "mniEditAlert";
            this.mniEditAlert.Size = new Size(0x10a, 0x16);
            this.mniEditAlert.Text = "Edit Alert ...";
            this.mniEditAlert.Click += new EventHandler(this.mniEditAlert_Click);
            this.sepSelectAll.Name = "sepSelectAll";
            this.sepSelectAll.Size = new Size(0x107, 6);
            this.mniPlace.Enabled = false;
            this.mniPlace.Image = (Image) resources.GetObject("mniPlace.Image");
            this.mniPlace.ImageTransparentColor = Color.Fuchsia;
            this.mniPlace.Name = "mniPlace";
            this.mniPlace.Size = new Size(0x10a, 0x16);
            this.mniPlace.Text = "Place Selected Orders";
            this.mniPlace.Click += new EventHandler(this.btnPlace_Click);
            this.mniStage.Enabled = false;
            this.mniStage.Image = (Image) resources.GetObject("mniStage.Image");
            this.mniStage.ImageTransparentColor = Color.Fuchsia;
            this.mniStage.Name = "mniStage";
            this.mniStage.Size = new Size(0x10a, 0x16);
            this.mniStage.Text = "Stage Selected Orders";
            this.mniStage.Click += new EventHandler(this.btnStage_Click);
            this.mniRemoveAlerts.Enabled = false;
            this.mniRemoveAlerts.Image = (Image) resources.GetObject("mniRemoveAlerts.Image");
            this.mniRemoveAlerts.ImageTransparentColor = Color.Fuchsia;
            this.mniRemoveAlerts.Name = "mniRemoveAlerts";
            this.mniRemoveAlerts.Size = new Size(0x10a, 0x16);
            this.mniRemoveAlerts.Text = "Remove Selected Alerts";
            this.mniRemoveAlerts.Click += new EventHandler(this.btnRemove_Click);
            this.mniClearAll.Image = (Image) resources.GetObject("mniClearAll.Image");
            this.mniClearAll.ImageTransparentColor = Color.Fuchsia;
            this.mniClearAll.Name = "mniClearAll";
            this.mniClearAll.Size = new Size(0x10a, 0x16);
            this.mniClearAll.Text = "Clear all Alerts";
            this.mniClearAll.Click += new EventHandler(this.mniClearAll_Click);
            this.sepAutoStage.Name = "sepAutoStage";
            this.sepAutoStage.Size = new Size(0x107, 6);
            this.mniAutoStage.Image = (Image) resources.GetObject("mniAutoStage.Image");
            this.mniAutoStage.ImageTransparentColor = Color.Fuchsia;
            this.mniAutoStage.Name = "mniAutoStage";
            this.mniAutoStage.Size = new Size(0x10a, 0x16);
            this.mniAutoStage.Text = "Auto-Stage Alerts";
            this.mniAutoStage.Click += new EventHandler(this.btnAutoStage_Click);
            this.sepClear.Name = "sepClear";
            this.sepClear.Size = new Size(0x107, 6);
            this.mniEmailAlerts.Image = (Image) resources.GetObject("mniEmailAlerts.Image");
            this.mniEmailAlerts.ImageScaling = ToolStripItemImageScaling.None;
            this.mniEmailAlerts.Name = "mniEmailAlerts";
            this.mniEmailAlerts.Size = new Size(0x10a, 0x16);
            this.mniEmailAlerts.Text = "Auto-Email";
            this.mniEmailAlerts.Click += new EventHandler(this.btnEmailAlerts_Click);
            this.mniSaveTriggered.Image = (Image) resources.GetObject("mniSaveTriggered.Image");
            this.mniSaveTriggered.ImageTransparentColor = Color.Fuchsia;
            this.mniSaveTriggered.Name = "mniSaveTriggered";
            this.mniSaveTriggered.Size = new Size(0x10a, 0x16);
            this.mniSaveTriggered.Text = "Save Alerts to File when Triggered ...";
            this.mniSaveTriggered.Click += new EventHandler(this.mniSaveTriggered_Click);
            this.sepRename2.Name = "sepRename2";
            this.sepRename2.Size = new Size(0x107, 6);
            this.mniRename2.Image = (Image) resources.GetObject("mniRename2.Image");
            this.mniRename2.ImageTransparentColor = Color.Fuchsia;
            this.mniRename2.Name = "mniRename2";
            this.mniRename2.Size = new Size(0x10a, 0x16);
            this.mniRename2.Text = "Rename this Quotes Window ...";
            this.mniRename2.Click += new EventHandler(this.mniRename2_Click);
            this.sepPrint.Name = "sepPrint";
            this.sepPrint.Size = new Size(0x107, 6);
            this.mniCopyAlerts.Image = (Image) resources.GetObject("mniCopyAlerts.Image");
            this.mniCopyAlerts.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyAlerts.Name = "mniCopyAlerts";
            this.mniCopyAlerts.Size = new Size(0x10a, 0x16);
            this.mniCopyAlerts.Text = "Copy Alerts to Clipboard";
            this.mniCopyAlerts.Click += new EventHandler(this.mniCopyAlerts_Click);
            this.mniPrintAlerts.Image = (Image) resources.GetObject("mniPrintAlerts.Image");
            this.mniPrintAlerts.Name = "mniPrintAlerts";
            this.mniPrintAlerts.Size = new Size(0x10a, 0x16);
            this.mniPrintAlerts.Text = "Print Alerts";
            this.mniPrintAlerts.Click += new EventHandler(this.mniPrintAlerts_Click);
            this.toolbarAlerts.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbarAlerts.Items.AddRange(new ToolStripItem[] { this.lblAlerts, this.btnSelectAll, this.toolStripSeparator3, this.btnPlace, this.btnStage, this.btnRemove, this.toolStripSeparator1, this.btnAutoStage, this.btnEmailAlerts, this.btnConvertMarket, this.sepGenerate, this.btnHelpAlerts });
            this.toolbarAlerts.Location = new Point(0, 0);
            this.toolbarAlerts.Name = "toolbarAlerts";
            this.toolbarAlerts.Size = new Size(0x34d, 0x19);
            this.toolbarAlerts.TabIndex = 14;
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
            this.btnRemove.Enabled = false;
            this.btnRemove.Image = (Image) resources.GetObject("btnRemove.Image");
            this.btnRemove.ImageTransparentColor = Color.Magenta;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new Size(0x42, 0x16);
            this.btnRemove.Text = "Remove";
            this.btnRemove.ToolTipText = "Remove Selected Alerts";
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.btnAutoStage.Image = (Image) resources.GetObject("btnAutoStage.Image");
            this.btnAutoStage.ImageTransparentColor = Color.Magenta;
            this.btnAutoStage.Name = "btnAutoStage";
            this.btnAutoStage.Size = new Size(0x52, 0x16);
            this.btnAutoStage.Text = "Auto-Stage";
            this.btnAutoStage.ToolTipText = "Automatically Stage Orders from Price Trigger Alerts";
            this.btnAutoStage.Click += new EventHandler(this.btnAutoStage_Click);
            this.btnEmailAlerts.Image = (Image) resources.GetObject("btnEmailAlerts.Image");
            this.btnEmailAlerts.ImageScaling = ToolStripItemImageScaling.None;
            this.btnEmailAlerts.ImageTransparentColor = Color.Magenta;
            this.btnEmailAlerts.Name = "btnEmailAlerts";
            this.btnEmailAlerts.Size = new Size(0x4f, 0x16);
            this.btnEmailAlerts.Text = "Auto-Email";
            this.btnEmailAlerts.ToolTipText = "Automatically Send Emails for Price Trigger Trade Alerts";
            this.btnEmailAlerts.Click += new EventHandler(this.btnEmailAlerts_Click);
            this.btnConvertMarket.CheckOnClick = true;
            this.btnConvertMarket.Image = (Image) resources.GetObject("btnConvertMarket.Image");
            this.btnConvertMarket.ImageTransparentColor = Color.Silver;
            this.btnConvertMarket.Name = "btnConvertMarket";
            this.btnConvertMarket.Size = new Size(0x76, 0x16);
            this.btnConvertMarket.Text = "Convert to Market ";
            this.btnConvertMarket.Click += new EventHandler(this.btnConvertMarket_Click);
            this.sepGenerate.Name = "sepGenerate";
            this.sepGenerate.Size = new Size(6, 0x19);
            this.btnHelpAlerts.Image = (Image) resources.GetObject("btnHelpAlerts.Image");
            this.btnHelpAlerts.ImageTransparentColor = Color.Magenta;
            this.btnHelpAlerts.Name = "btnHelpAlerts";
            this.btnHelpAlerts.Size = new Size(0x30, 0x16);
            this.btnHelpAlerts.Text = "Help";
            this.btnHelpAlerts.ToolTipText = "Help on Quotes";
            this.btnHelpAlerts.Click += new EventHandler(this.btnHelpAlerts_Click);
            this.timer_0.Enabled = true;
            this.timer_0.Interval = 300;
            this.timer_0.Tick += new EventHandler(this.timer_0_Tick);
            this.saveFileDialog_0.DefaultExt = "txt";
            this.saveFileDialog_0.Filter = "Text Files|*.txt";
            this.saveFileDialog_0.Title = "Save Triggered Alerts";
            this.symbolParser_0.Text = null;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x34d, 0x1cd);
            base.Controls.Add(this.split);
            base.Controls.Add(this.status);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "QuotesForm";
            base.StartPosition = FormStartPosition.WindowsDefaultBounds;
            this.Text = "Quotes";
            base.Load += new EventHandler(this.QuotesForm_Load);
            base.FormClosed += new FormClosedEventHandler(this.QuotesForm_FormClosed);
            base.FormClosing += new FormClosingEventHandler(this.QuotesForm_FormClosing);
            base.TextChanged += new EventHandler(this.QuotesForm_TextChanged);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel1.PerformLayout();
            this.split.Panel2.ResumeLayout(false);
            this.split.Panel2.PerformLayout();
            this.split.ResumeLayout(false);
            this.popupQuotes.ResumeLayout(false);
            this.toolbarQuotes.ResumeLayout(false);
            this.toolbarQuotes.PerformLayout();
            this.popupAlerts.ResumeLayout(false);
            this.toolbarAlerts.ResumeLayout(false);
            this.toolbarAlerts.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadWorkspaceItems(IList<string> items, int version)
        {
            this.txtThreshold.Text = items[0];
            this.double_0 = double.Parse(this.txtThreshold.Text);
            this.btnAutoSort.Checked = bool.Parse(items[1]);
            this.btnAutoRemove.Checked = bool.Parse(items[2]);
            this.btnAutoStage.Checked = bool.Parse(items[3]);
            this.mniAutoStage.Checked = this.btnAutoStage.Checked;
            string[] strArray = items[4].Split(new char[] { ',' });
            foreach (string str in strArray)
            {
                this.method_1(str);
            }
            this.method_2(strArray);
            if (version >= 2)
            {
                this.Text = items[5];
            }
            if (version >= 3)
            {
                this.btnConvertMarket.Checked = bool.Parse(items[6]);
            }
            if (version >= 4)
            {
                if (MainModule.Instance.EmailSettingsAvailable)
                {
                    if (items.Count > 7)
                    {
                        this.btnEmailAlerts.Checked = bool.Parse(items[7]);
                        this.mniEmailAlerts.Checked = this.btnEmailAlerts.Checked;
                    }
                }
                else
                {
                    this.btnEmailAlerts.ToolTipText = "Please add Email Settings in Preferences to enable this feature.";
                }
            }
        }

        private void lvAlerts_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag = this.method_12();
            this.mniEditAlert.Enabled = this.lvAlerts.SelectedItems.Count == 1;
            if (this.lvAlerts.SelectedItems.Count > 0)
            {
                this.btnPlace.Enabled = flag;
                this.mniPlace.Enabled = flag;
                this.mniStage.Enabled = true;
                this.mniRemoveAlerts.Enabled = true;
                this.btnRemove.Enabled = true;
                this.btnStage.Enabled = true;
            }
            else
            {
                this.btnPlace.Enabled = false;
                this.mniPlace.Enabled = false;
                this.mniStage.Enabled = false;
                this.mniRemoveAlerts.Enabled = false;
                this.btnRemove.Enabled = false;
                this.btnStage.Enabled = false;
            }
        }

        private void lvQuotes_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(TreeNode)) is TreeNode)
            {
                this.method_5(e);
            }
            else
            {
                this.method_6(e);
            }
        }

        private void lvQuotes_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(ListViewItem)) is ListViewItem)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lvQuotes_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(TreeNode)) is TreeNode)
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void lvQuotes_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lvQuotes_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lvQuotes_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if ((e.ColumnIndex != 4) && (e.ColumnIndex != 5))
            {
                if (e.ColumnIndex == 11)
                {
                    Color gray;
                    e.DrawDefault = false;
                    e.DrawBackground();
                    if (e.SubItem.Text == "")
                    {
                        return;
                    }
                    double tag = (double) e.SubItem.Tag;
                    if (tag < 0.0)
                    {
                        gray = Color.Gray;
                    }
                    else
                    {
                        int num3 = (int) (100.0 - tag);
                        int num4 = 100 - num3;
                        gray = Color.FromArgb((int) (num4 * 2.55), (int) (num3 * 1.28), 0);
                    }
                    SizeF ef2 = e.Graphics.MeasureString(e.SubItem.Text, this.lvQuotes.Font);
                    SizeF ef = e.Graphics.MeasureString("100.00", this.lvQuotes.Font);
                    Brush brush = new SolidBrush(gray);
                    using (brush)
                    {
                        e.Graphics.DrawString(e.SubItem.Text, this.lvQuotes.Font, brush, ((e.Bounds.Left + 2) + ef.Width) - ef2.Width, (float) e.Bounds.Top);
                        double num = (e.Bounds.Width - ef.Width) - 2f;
                        num *= tag / 100.0;
                        e.Graphics.FillRectangle(brush, (e.Bounds.Left + ef.Width) + 2f, (float) (e.Bounds.Top + 2), (float) ((int) num), (float) (e.Bounds.Height - 4));
                        return;
                    }
                }
                e.DrawDefault = true;
            }
            else
            {
                Color red;
                e.DrawDefault = false;
                e.DrawBackground();
                if (e.SubItem.Text.Contains("-"))
                {
                    red = Color.Red;
                }
                else
                {
                    red = Color.Blue;
                }
                SizeF ef3 = e.Graphics.MeasureString(e.SubItem.Text, this.lvQuotes.Font);
                Brush brush3 = new SolidBrush(red);
                using (brush3)
                {
                    e.Graphics.DrawString(e.SubItem.Text, this.lvQuotes.Font, brush3, (e.Bounds.Right - ef3.Width) - 2f, (float) e.Bounds.Top);
                }
            }
        }

        private void lvQuotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lvQuotes.SelectedItems.Count > 0)
            {
                if (this.lvQuotes.SelectedItems.Count == 1)
                {
                    this.mniEditPriceTrigger.Enabled = this.lvQuotes.SelectedItems[0].Tag != null;
                }
                else
                {
                    this.mniEditPriceTrigger.Enabled = false;
                }
                this.removeSelectedQuotesToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.mniEditPriceTrigger.Enabled = false;
                this.removeSelectedQuotesToolStripMenuItem.Enabled = false;
                this.triggerSelectedQuotesNowToolStripMenuItem.Enabled = false;
            }
            IEnumerator enumerator = this.lvQuotes.SelectedItems.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ListViewItem current = (ListViewItem)enumerator.Current;
                        if (current.Tag != null)
                        {
                            this.triggerSelectedQuotesNowToolStripMenuItem.Enabled = true;
                            break;
                        }
                        else
                        {
                            this.triggerSelectedQuotesNowToolStripMenuItem.Enabled = false;
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

        private string method_0(string string_1)
        {
            int pricing = DecimalsManager.Instance.Pricing;
            using (IEnumerator<SymbolInfo> enumerator = BarsLoader.SymbolInfo.GetEnumerator())
            {
                SymbolInfo current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Symbol.ToUpper() == string_1.ToUpper())
                    {
                        ///goto  Label_005F;  ///WYJ fix, simplify the flow
                        pricing = current.Decimals;
                        break;
                    }
                    if (Regex.IsMatch(string_1, "^" + current.Symbol + "$"))
                    {
                        ///goto  Label_0068;
                        pricing = current.Decimals;
                        break;
                    }
                }
            }
            return ("N" + pricing.ToString());
        }

        private ListViewItem method_1(string string_1)
        {
            ListViewItem item = this.lvQuotes.Items.Add("");
            for (int i = 1; i <= this.lvQuotes.Columns.Count; i++)
            {
                item.SubItems.Add("");
            }
            item.UseItemStyleForSubItems = false;
            item.SubItems[1].Text = string_1;
            item.SubItems[1].Font = this.font_0;
            item.SubItems[11].Tag = -1.7976931348623157E+308;
            this.method_11();
            return item;
        }

        private void method_10(bool bool_6)
        {
            this.list_0.Clear();
            foreach (ListViewItem item in this.lvAlerts.SelectedItems)
            {
                Alert tag = (Alert) item.Tag;
                this.list_0.Add(tag);
            }
            MainModule.Instance.TradeManager.AddAlerts(this.list_0, bool_6, false);
        }

        private void method_11()
        {
            this.statusQuotes.Text = this.lvQuotes.Items.Count + " Quotes";
            this.statusAlerts.Text = this.lvAlerts.Items.Count + " Alerts";
        }

        private bool method_12()
        {
            bool flag3;
            if (flag3 = MainModule.Instance.AuthProvider.LoggedIn && (MainModule.Instance.BrokerProvider != null))
            {
                return flag3;
            }
            bool flag = false;
            bool flag2 = false;
            if (this.lvAlerts.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in this.lvAlerts.SelectedItems)
                {
                    Alert tag = (Alert) item.Tag;
                    if (tag.Account != "")
                    {
                        if (tag.Account.StartsWith("Paper"))
                        {
                            flag2 = true;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    if (flag2 && flag)
                    {
                        break;
                    }
                }
            }
            return (!flag && flag2);
        }

        private void method_2(IList<string> ilist_0)
        {
            if (!this.bool_0 && (ilist_0.Count > 0))
            {
                this.method_3();
                foreach (string str in ilist_0)
                {
                    if (this.bool_0)
                    {
                        break;
                    }
                    this.streamingQuoteManager_0.Subscribe(str);
                    if (this.bool_0)
                    {
                        this.streamingQuoteManager_0.Unsubscribe(str);
                    }
                }
            }
        }

        private bool method_3()
        {
            this.streamingQuoteManager_0.ConnectionStatus = MainModule.Instance;
            this.streamingQuoteManager_0.Provider = MainModule.Instance.StreamingProvider;
            return this.streamingQuoteManager_0.Provider.IsConnected;
        }

        private void method_4(string string_1)
        {
            if (!this.bool_0)
            {
                this.method_3();
                this.streamingQuoteManager_0.Subscribe(string_1);
                if (this.bool_0)
                {
                    this.streamingQuoteManager_0.Unsubscribe(string_1);
                }
            }
        }

        private void method_5(DragEventArgs dragEventArgs_0)
        {
            TreeNode data = dragEventArgs_0.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (data != null)
            {
                List<string> list = new List<string>();
                if (data.Level == 0)
                {
                    DataSource tag = data.Tag as DataSource;
                    if (tag != null)
                    {
                        foreach (string str in tag.Symbols)
                        {
                            this.method_1(str);
                            list.Add(str);
                        }
                    }
                }
                else
                {
                    this.method_1(data.Text);
                    list.Add(data.Text);
                }
                this.method_2(list);
            }
        }

        private void method_6(DragEventArgs dragEventArgs_0)
        {
            ListViewItem data = dragEventArgs_0.Data.GetData(typeof(ListViewItem)) as ListViewItem;
            if (data != null)
            {
                List<string> list = new List<string>();
                foreach (ListViewItem item2 in data.ListView.SelectedItems)
                {
                    Alert tag = item2.Tag as Alert;
                    this.AddAlert(tag);
                    list.Add(tag.Symbol);
                }
                this.method_2(list);
            }
        }

        private void method_7()
        {
            this.lvQuotes.SortByColumn(this.columnHeader_9, SortOrder.Descending);
        }

        private void method_8(ListViewItem listViewItem_0, Alert alert_0)
        {
            if (!alert_0.Triggered)
            {
                if (this.btnConvertMarket.Checked)
                {
                    alert_0.OrderType = OrderType.Market;
                }
                listViewItem_0.StateImageIndex = 0;
                alert_0.Triggered = true;
                ListViewItem item = this.lvAlerts.Items.Add(DateTime.Now.ToShortTimeString());
                item.UseItemStyleForSubItems = false;
                item.Tag = new Alert(alert_0);
                item.SubItems.Add(alert_0.Symbol);
                item.SubItems[1].Font = this.font_0;
                item.SubItems.Add(alert_0.Account);
                item.SubItems.Add(alert_0.AlertType.ToString());
                item.SubItems.Add(alert_0.Shares.ToString());
                item.SubItems.Add(alert_0.OrderType.ToString());
                if ((alert_0.OrderType != OrderType.Limit) && (alert_0.OrderType != OrderType.Stop))
                {
                    item.SubItems.Add("");
                }
                else
                {
                    item.SubItems.Add(alert_0.Price.ToString(this.method_0(alert_0.Symbol)));
                }
                if (alert_0.Strategy != null)
                {
                    item.SubItems.Add(alert_0.Strategy.Name);
                    if (alert_0.DataSet != null)
                    {
                        item.SubItems.Add(alert_0.DataSet.Name);
                    }
                    else
                    {
                        item.SubItems.Add("");
                    }
                    if (alert_0.DataRange != null)
                    {
                        item.SubItems.Add(alert_0.DataRange.Text);
                    }
                    else
                    {
                        item.SubItems.Add("");
                    }
                    item.SubItems.Add(alert_0.DataScale.ToString());
                    if (alert_0.PosSize != null)
                    {
                        item.SubItems.Add(alert_0.PosSize.Text);
                    }
                    else
                    {
                        item.SubItems.Add("");
                    }
                }
                else
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        item.SubItems.Add("");
                    }
                }
                item.SubItems.Add(alert_0.SignalName);
                item.SubItems.Add(alert_0.AccountTradeType);
                if (MainModule.Instance.Settings.Get("SoundsQuotes", true))
                {
                    string defaultValue = MainModule.Instance.AppPath + @"\Data\Sounds\alert1.wav";
                    string path = MainModule.Instance.Settings.Get("SoundsQuotes_File", defaultValue);
                    if (!File.Exists(path))
                    {
                        path = defaultValue;
                        MainModule.Instance.Settings.Set("SoundsQuotes_File", path);
                    }
                    MainModule.Instance.PlaySound(path);
                }
                if (this.btnAutoStage.Checked)
                {
                    MainModule.Instance.TradeManager.AddAlert(alert_0, MainModule.Instance.ShouldOrderBePlaced(alert_0), true);
                }
                if (this.btnEmailAlerts.Checked)
                {
                    MainModule.Instance.method_25(alert_0, MainModule.Instance.ShouldOrderBePlaced(alert_0));
                }
                if (this.btnAutoRemove.Checked)
                {
                    this.method_9(listViewItem_0);
                }
                this.method_11();
                if (this.mniSaveTriggered.Checked)
                {
                    lock (this.string_0)
                    {
                        string[] contents = new string[this.lvAlerts.Items.Count];
                        int index = 0;
                        foreach (ListViewItem item2 in this.lvAlerts.Items)
                        {
                            contents[index] = item2.SubItems[1].Text + ";" + item2.Text + ";" + item2.SubItems[3].Text + ";" + item2.SubItems[4].Text + ";" + item2.SubItems[5].Text + ";" + item2.SubItems[6].Text + ";" + item2.SubItems[7].Text + ";" + item2.SubItems[8].Text + ";" + item2.SubItems[9].Text;
                            index++;
                        }
                        try
                        {
                            FileNameValidator.ValidateFileName(this.string_0);
                            File.WriteAllLines(this.string_0, contents);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                    }
                }
            }
        }

        private void method_9(ListViewItem listViewItem_0)
        {
            string text = listViewItem_0.SubItems[1].Text;
            this.streamingQuoteManager_0.Unsubscribe(text);
            listViewItem_0.Remove();
            this.method_11();
        }

        private void mniClearAll_Click(object sender, EventArgs e)
        {
            this.lvAlerts.Items.Clear();
            this.method_11();
        }

        private void mniClearTriggered_Click(object sender, EventArgs e)
        {
            for (int i = this.lvQuotes.Items.Count - 1; i >= 0; i--)
            {
                ListViewItem item = this.lvQuotes.Items[i];
                Alert tag = item.Tag as Alert;
                if ((tag != null) && tag.Triggered)
                {
                    this.method_9(item);
                }
            }
        }

        private void mniCopyAlerts_Click(object sender, EventArgs e)
        {
            MainModule.Instance.CopyListViewToClipboard(this.lvAlerts);
        }

        private void mniCopyQuotes_Click(object sender, EventArgs e)
        {
            MainModule.Instance.CopyListViewToClipboard(this.lvQuotes);
        }

        private void mniEditAlert_Click(object sender, EventArgs e)
        {
            if (this.lvAlerts.SelectedItems.Count == 1)
            {
                ListViewItem item = this.lvAlerts.SelectedItems[0];
                Alert tag = (Alert) item.Tag;
                EditAlertForm form = new EditAlertForm(this.IsPaper) {
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
                        item.SubItems[6].Text = tag.Price.ToString(this.method_0(tag.Symbol));
                    }
                    item.SubItems[this.int_14].Text = tag.AccountTradeType;
                }
            }
        }

        private void mniEditPriceTrigger_Click(object sender, EventArgs e)
        {
            if (this.lvQuotes.SelectedItems.Count == 1)
            {
                ListViewItem item = this.lvQuotes.SelectedItems[0];
                Alert tag = (Alert) item.Tag;
                if (tag != null)
                {
                    EditAlertForm form = new EditAlertForm(this.IsPaper) {
                        Text = "Edit Price Trigger",
                        Alert = new Alert(tag)
                    };
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        tag = form.Alert;
                        tag.Triggered = false;
                        item.Tag = tag;
                        item.StateImageIndex = -1;
                        if (item.SubItems[1].Text != tag.Symbol)
                        {
                            this.streamingQuoteManager_0.Unsubscribe(item.SubItems[1].Text);
                            this.method_4(tag.Symbol);
                        }
                        item.SubItems[1].Text = tag.Symbol;
                        item.SubItems[6].Text = tag.Account;
                        item.SubItems[7].Text = tag.AlertType.ToString();
                        item.SubItems[8].Text = tag.Shares.ToString();
                        item.SubItems[9].Text = tag.OrderType.ToString();
                        if ((tag.OrderType != OrderType.Limit) && (tag.OrderType != OrderType.Stop))
                        {
                            item.SubItems[10].Text = "";
                        }
                        else
                        {
                            item.SubItems[10].Text = tag.Price.ToString(this.method_0(tag.Symbol));
                        }
                        item.SubItems[this.int_13].Text = tag.AccountTradeType;
                    }
                }
            }
        }

        private void mniPrintAlerts_Click(object sender, EventArgs e)
        {
            this.listView_0 = this.lvAlerts;
            this.Print();
            this.listView_0 = null;
        }

        private void mniPrintQuotes_Click(object sender, EventArgs e)
        {
            this.listView_0 = this.lvQuotes;
            this.Print();
            this.listView_0 = null;
        }

        private void mniRename2_Click(object sender, EventArgs e)
        {
            this.Text = InputBox.Show("Rename Quotes Window", "Quotes Window Name:", this.Text);
        }

        private void mniSaveTriggered_Click(object sender, EventArgs e)
        {
            this.mniSaveTriggered.Checked = !this.mniSaveTriggered.Checked;
            this.saveFileDialog_0.InitialDirectory = MainModule.Instance.DataPath;
            if (this.mniSaveTriggered.Checked)
            {
                if (this.saveFileDialog_0.ShowDialog() == DialogResult.OK)
                {
                    this.string_0 = this.saveFileDialog_0.FileName;
                }
                else
                {
                    this.mniSaveTriggered.Checked = false;
                }
            }
        }

        public void Print()
        {
            PrintDocument prtdoc = new PrintDocument();
            prtdoc.BeginPrint += new PrintEventHandler(this.QuotesReport_BeginPrint);
            prtdoc.PrintPage += new PrintPageEventHandler(this.QuotesReport_PrintPage);
            prtdoc.EndPrint += new PrintEventHandler(this.QuotesReport_EndPrint);
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

        private void QuotesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.MyMainForm.ItemRemoved(this);
        }

        private void QuotesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.bool_0 = true;
            if (this.streamingQuoteManager_0.Provider != null)
            {
                this.streamingQuoteManager_0.Provider.ClearRequests(this.streamingQuoteManager_0);
            }
        }

        private void QuotesForm_Load(object sender, EventArgs e)
        {
            this.font_0 = new Font(this.lvQuotes.Font, FontStyle.Bold);
            this.MyMainForm.ItemAdded(this);
            this.dateTime_0 = this.marketHours_0.MarketCloseTimeLocal.AddMinutes(-2.0);
            this.double_0 = MainModule.Instance.Settings.Get("TriggerThreshold", (double) 100.0);
            this.txtThreshold.Text = this.double_0.ToString();
            if ((MainModule.Instance.StreamingProvider != null) && !MainModule.Instance.StreamingProvider.ProvidesOpen)
            {
                this.txtGapFilter.Enabled = false;
            }
            this.double_1 = MainModule.Instance.Settings.Get("GapFilter", (double) 0.0);
            this.txtGapFilter.Text = this.double_1.ToString();
            this.btnConvertMarket.Checked = MainModule.Instance.Settings.Get("ConvertToMarketOrder", false);
            this.btnAutoRemove.Checked = MainModule.Instance.Settings.Get("AutoRemove", false);
            this.btnAutoSort.Checked = MainModule.Instance.Settings.Get("AutoSort", false);
            this.btnAutoStage.Checked = MainModule.Instance.Settings.Get("Orders", false);
            if (MainModule.Instance.EmailSettingsAvailable)
            {
                this.btnEmailAlerts.Checked = MainModule.Instance.Settings.Get("EmailAlerts", false);
                this.mniEmailAlerts.Checked = this.btnEmailAlerts.Checked;
            }
            else
            {
                this.btnEmailAlerts.Enabled = false;
                this.btnEmailAlerts.ToolTipText = "Please add Email Settings in Preferences to enable this feature.";
                this.mniEmailAlerts.Enabled = false;
                this.mniEmailAlerts.ToolTipText = "Please add Email Settings in Preferences to enable this feature.";
            }
            this.Text = this.FormTitle;
            this.ShowAutoTradingState(MainModule.Instance.AutoTradingEnabled);
        }

        private void QuotesForm_TextChanged(object sender, EventArgs e)
        {
            this.MyMainForm.ItemChanged(this);
        }

        public void QuotesReport_BeginPrint(object sender, PrintEventArgs e)
        {
            this.printReport_0 = new PrintReport();
            this.printReport_0.printTitle = "Quotes";
            this.printReport_0.BasePrintTitle = MainModule.Instance.AuthProvider.ApplicationName;
            this.printReport_0.rptDisclosure = "1. Change and % Change represent the change and % change from the previous trading day's closing price.\n2. Threshold % is a measure of the proximity of the current price to the target stop or limit price with respect to a reference price, where 100% means that price has attained the target. Set the threshold in percent at which you want Alerts to be triggered. When the Threshold value matches or exceeds the Trigger Threshold %, the Alert is triggered. The default setting is 100%, but any positive percentage can be used. A setting below 100% will trigger stop and limit orders before the market actually reaches the alert price, which may help placing an order in time to catch a fast-moving market, or to place a short limit order early while a stock is still in an up tick.";
            if (this.listView_0 == null)
            {
                this.printReport_0.printListView = this.lvQuotes;
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

        public void QuotesReport_EndPrint(object sender, PrintEventArgs e)
        {
            this.printReport_0.intPageCounter--;
            this.printReport_0.endPageCount = this.printReport_0.intPageCounter;
            if (this.printPreview_0 != null)
            {
                this.printPreview_0.FromPage = 1;
                this.printPreview_0.ToPage = this.printReport_0.intPageCounter;
            }
        }

        public void QuotesReport_PrintPage(object sender, PrintPageEventArgs e)
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

        private void removeSelectedQuotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = this.lvQuotes.Items.Count - 1; i >= 0; i--)
            {
                ListViewItem item = this.lvQuotes.Items[i];
                if (item.Selected)
                {
                    this.method_9(item);
                }
            }
        }

        public int SaveWorkspaceItems(IList<string> items)
        {
            items.Add(this.txtThreshold.Text);
            items.Add(this.btnAutoSort.Checked.ToString());
            items.Add(this.btnAutoRemove.Checked.ToString());
            items.Add(this.btnAutoStage.Checked.ToString());
            StringBuilder builder = new StringBuilder();
            foreach (ListViewItem item in this.lvQuotes.Items)
            {
                if (item.Tag == null)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(",");
                    }
                    builder.Append(item.SubItems[1].Text);
                }
            }
            items.Add(builder.ToString());
            items.Add(this.Text);
            items.Add(this.btnConvertMarket.Checked.ToString());
            items.Add(this.btnEmailAlerts.Checked.ToString());
            return 4;
        }

        public void ShowAutoTradingState(WealthLab.AutoTradingMode mode)
        {
            string str;
            if (mode == this.AutoTradingMode)
            {
                str = "Place";
            }
            else
            {
                str = "Stage";
            }
            this.btnAutoStage.Text = "Auto-" + str;
            this.mniAutoStage.Text = this.btnAutoStage.Text;
            this.btnAutoStage.ToolTipText = "Automatically " + str + " Orders from Price Trigger Alerts";
        }

        public void StatusUpdate(ConnStatus status, int StatusCode, string Message)
        {
        }

        private void timer_0_Tick(object sender, EventArgs e)
        {
            if (!this.bool_1 && this.marketHours_0.IsMarketOpenNow)
            {
                this.bool_1 = true;
                for (int i = this.lvQuotes.Items.Count - 1; i >= 0; i--)
                {
                    ListViewItem item2 = this.lvQuotes.Items[i];
                    Alert tag = item2.Tag as Alert;
                    if (((tag != null) && (tag.OrderType == OrderType.Market)) && !tag.Triggered)
                    {
                        this.method_8(item2, tag);
                    }
                }
            }
            if (!this.bool_2 && (DateTime.Now >= this.dateTime_0))
            {
                this.bool_2 = true;
                for (int j = this.lvQuotes.Items.Count - 1; j >= 0; j--)
                {
                    ListViewItem item3 = this.lvQuotes.Items[j];
                    Alert alert3 = item3.Tag as Alert;
                    if (((alert3 != null) && (alert3.OrderType == OrderType.AtClose)) && !alert3.Triggered)
                    {
                        this.method_8(item3, alert3);
                    }
                }
            }
            if (this.streamingQuoteManager_0.FreshQuoteReady)
            {
                for (int k = this.lvQuotes.Items.Count - 1; k >= 0; k--)
                {
                    ListViewItem item = this.lvQuotes.Items[k];
                    double num10 = 0.0;
                    if (item.SubItems[3].Tag != null)
                    {
                        num10 = (double) item.SubItems[3].Tag;
                    }
                    Quote lastQuote = this.streamingQuoteManager_0.GetLastQuote(item.SubItems[1].Text);
                    if ((lastQuote != null) && (lastQuote.Price != num10))
                    {
                        string format = this.method_0(lastQuote.Symbol);
                        item.Text = lastQuote.TimeStamp.ToLongTimeString();
                        item.SubItems[3].Tag = lastQuote.Price;
                        item.SubItems[3].Text = lastQuote.Price.ToString(format);
                        double num11 = lastQuote.Price - lastQuote.PreviousClose;
                        item.SubItems[4].Text = num11.ToString(format);
                        if (lastQuote.PreviousClose > 0.0)
                        {
                            item.SubItems[5].Text = ((num11 * 100.0) / lastQuote.PreviousClose).ToString("N2");
                        }
                        item.SubItems[2].Text = lastQuote.Open.ToString(format);
                        if (item.Tag != null)
                        {
                            Alert alert = (Alert) item.Tag;
                            if (alert.FirstTick == -1.0)
                            {
                                alert.FirstTick = lastQuote.Price;
                            }
                            double previousClose = lastQuote.PreviousClose;
                            if ((alert.AlertType != TradeType.Buy) && (alert.AlertType != TradeType.Cover))
                            {
                                if (alert.OrderType == OrderType.Limit)
                                {
                                    if (alert.Price < lastQuote.PreviousClose)
                                    {
                                        previousClose = alert.FirstTick;
                                    }
                                }
                                else if ((alert.OrderType == OrderType.Stop) && (alert.Price > lastQuote.PreviousClose))
                                {
                                    previousClose = alert.FirstTick;
                                }
                            }
                            else if (alert.OrderType == OrderType.Limit)
                            {
                                if (alert.Price > lastQuote.PreviousClose)
                                {
                                    previousClose = alert.FirstTick;
                                }
                            }
                            else if ((alert.OrderType == OrderType.Stop) && (alert.Price < lastQuote.PreviousClose))
                            {
                                previousClose = alert.FirstTick;
                            }
                            double num5 = alert.Price - previousClose;
                            double num4 = lastQuote.Price - previousClose;
                            if ((alert.AlertType == TradeType.Sell) || (alert.AlertType == TradeType.Short))
                            {
                                num4 = -num4;
                                num5 = -num5;
                            }
                            if (alert.OrderType == OrderType.Stop)
                            {
                                num5 = -num5;
                                num4 = -num4;
                            }
                            double num7 = 0.0;
                            if (num5 != 0.0)
                            {
                                num7 = 100.0 - (((num5 - num4) * 100.0) / num5);
                            }
                            bool flag2 = false;
                            switch (alert.AlertType)
                            {
                                case TradeType.Buy:
                                case TradeType.Cover:
                                    flag2 = (alert.OrderType == OrderType.Limit) ? (lastQuote.Price <= alert.Price) : (lastQuote.Price >= alert.Price);
                                    break;

                                case TradeType.Sell:
                                case TradeType.Short:
                                    flag2 = (alert.OrderType == OrderType.Limit) ? (lastQuote.Price >= alert.Price) : (lastQuote.Price <= alert.Price);
                                    break;
                            }
                            if (flag2)
                            {
                                num7 = 100.0;
                            }
                            bool flag = true;
                            if (this.streamingQuoteManager_0.Provider.ProvidesOpen)
                            {
                                if ((alert.AlertType == TradeType.Buy) && (alert.OrderType == OrderType.Limit))
                                {
                                    if ((this.double_1 != 0.0) && (lastQuote.Open < (((this.double_1 * alert.Price) / 100.0) + alert.Price)))
                                    {
                                        flag = false;
                                        item.StateImageIndex = 1;
                                        item.ToolTipText = "The criteria of the Gap Filter is such that the opening price is not being met, thus filtering out the Alert so the order will never trigger";
                                    }
                                    else
                                    {
                                        if (item.StateImageIndex == 1)
                                        {
                                            item.StateImageIndex = -1;
                                        }
                                        item.ToolTipText = "";
                                    }
                                }
                                if ((alert.AlertType == TradeType.Short) && (alert.OrderType == OrderType.Limit))
                                {
                                    if (((this.double_1 != 0.0) && (lastQuote.Open > (alert.Price - ((this.double_1 * alert.Price) / 100.0)))) || ((this.double_1 != 0.0) && (lastQuote.Open == 0.0)))
                                    {
                                        flag = false;
                                        item.StateImageIndex = 1;
                                        item.ToolTipText = "The criteria of the Gap Filter is such that the opening price is not being met, thus filtering out the Alert so the order will never trigger";
                                    }
                                    else
                                    {
                                        if (item.StateImageIndex == 1)
                                        {
                                            item.StateImageIndex = -1;
                                        }
                                        item.ToolTipText = "";
                                    }
                                }
                            }
                            if (((num7 >= this.double_0) && !alert.Triggered) && flag)
                            {
                                this.method_8(item, alert);
                            }
                            item.SubItems[11].Text = num7.ToString("N2");
                            item.SubItems[11].Tag = num7;
                        }
                    }
                }
                if (this.btnAutoSort.Checked)
                {
                    byte num8;
                    this.byte_0 = (byte) ((num8 = this.byte_0) + 1);
                    if ((num8 % 4) == 0)
                    {
                        bool flag3 = false;
                        for (int m = 1; m < this.lvQuotes.Items.Count; m++)
                        {
                            if (this.lvQuotes.Items[m - 1].Tag == null)
                            {
                                if (this.lvQuotes.Items[m].Tag == null)
                                {
                                    continue;
                                }
                                flag3 = true;
                                break;
                            }
                            if (this.lvQuotes.Items[m].Tag != null)
                            {
                                double num15 = (double) this.lvQuotes.Items[m - 1].SubItems[11].Tag;
                                double num16 = (double) this.lvQuotes.Items[m].SubItems[11].Tag;
                                if (num15 < num16)
                                {
                                    flag3 = true;
                                    break;
                                }
                            }
                        }
                        if (flag3)
                        {
                            this.method_7();
                        }
                    }
                }
            }
        }

        private void triggerSelectedQuotesNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = this.lvQuotes.Items.Count - 1; i >= 0; i--)
            {
                ListViewItem item = this.lvQuotes.Items[i];
                if (item.Selected)
                {
                    Alert tag = item.Tag as Alert;
                    if ((tag != null) && !tag.Triggered)
                    {
                        this.method_8(item, tag);
                    }
                }
            }
        }

        private void txtGapFilter_Leave(object sender, EventArgs e)
        {
            double result = 0.0;
            if (double.TryParse(this.txtGapFilter.Text, out result))
            {
                result = double.Parse(result.ToString("0.0#"));
                if (result > 100.0)
                {
                    result = 100.0;
                }
                if (result < 0.0)
                {
                    result = 0.0;
                }
                this.txtGapFilter.Text = result.ToString();
                this.double_1 = result;
                MainModule.Instance.Settings.Set("GapFilter", this.double_1);
            }
            else
            {
                this.txtGapFilter.Text = this.double_1.ToString();
            }
        }

        private void txtThreshold_Leave(object sender, EventArgs e)
        {
            double num = 0.0;
            try
            {
                num = double.Parse(this.txtThreshold.Text);
                if (num > 100.0)
                {
                    num = 100.0;
                    this.txtThreshold.Text = num.ToString();
                }
            }
            catch
            {
            }
            if (num > 0.0)
            {
                this.double_0 = num;
                MainModule.Instance.Settings.Set("TriggerThreshold", this.double_0);
            }
            else
            {
                this.txtThreshold.Text = this.double_0.ToString();
            }
        }

        public WealthLab.AutoTradingMode AutoTradingMode
        {
            [CompilerGenerated]
            get
            {
                return this.autoTradingMode_0;
            }
            [CompilerGenerated]
            set
            {
                this.autoTradingMode_0 = value;
            }
        }

        public string FormTitle
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(this.IsPaper ? "Quotes (Paper Trading)" : "Quotes");
                if (MainModule.Instance.WindowWithTitleExists(builder.ToString(), this))
                {
                    builder.Append(" ");
                    int length = builder.Length;
                    int num2 = 2;
                    do
                    {
                        if (builder.Length > length)
                        {
                            builder.Remove(length, builder.Length - length);
                        }
                        builder.Append(num2++);
                    }
                    while (MainModule.Instance.WindowWithTitleExists(builder.ToString(), this));
                }
                return builder.ToString();
            }
        }

        private bool IsPaper
        {
            get
            {
                return (this.AutoTradingMode == WealthLab.AutoTradingMode.Paper);
            }
        }

        public MainForm MyMainForm
        {
            get
            {
                return (base.MdiParent as MainForm);
            }
        }
    }
}

