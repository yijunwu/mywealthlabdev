namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.DataProviders.MarketManagerService.Properties;

    public class MarketManagerControl : DataBehaviorUserControl
    {
        private bool bool_0 = true;
        private Button btnApplySymbolsChange;
        private CheckBox cbDefaultMarket;
        private ContextMenuStrip cmnuMain;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private DateTimePicker dtCloseTime;
        private DateTimePicker dtOpenTime;
        private IContainer icontainer_1;
        private Label lblCloseTime;
        private Label lblEnterSymbols;
        private Label lblInfo;
        private Label lblMarketInfo;
        private Label lblOpenTime;
        private Label lblTimeZone;
        private ListViewItem listViewItem_0;
        private LinkLabel lnkCopyFromDataSet;
        private ListView lsvHolidays;
        private ListView lsvMarkets;
        private ListView lsvSpecialHours;
        private MarketInfo marketInfo_0;
        private ToolStripMenuItem mniEdit;
        private ToolStripMenuItem mniNew;
        private ToolStripMenuItem mniRemove;
        private TabPage pgHolidays;
        private TabPage pgProperties;
        private TabPage pgSpecialHours;
        private TabPage pgSymbols;
        private PictureBox pictureBox1;
        private SplitContainer pnlBottom;
        private Class8 pnlInfo;
        private Class8 pnlMarketInfo;
        private Panel pnlTop;
        private TabControl tabProperties;
        private TextBox txtSymbols;

        public MarketManagerControl()
        {
            this.InitializeComponent_1();
            this.pnlMarketInfo.Visible = false;
            this.method_15();
            this.method_5();
            this.method_31();
            if (this.lsvMarkets.Items.Count > 0)
            {
                this.lsvMarkets.Items[0].Selected = true;
            }
        }

        private void btnApplySymbolsChange_Click(object sender, EventArgs e)
        {
            Symbols symbols = new Symbols(this.txtSymbols.Text.ToUpper());
            Symbols symbols2 = MarketManager.smethod_23(symbols, this.marketInfo_0);
            MarketInfoSettings settings = MarketManager.smethod_17(this.marketInfo_0.Name);
            if (symbols2.Items.Count > 0)
            {
                DuplicateSymbolsDialogForm form = new DuplicateSymbolsDialogForm {
                    DuplicateSymbols = symbols2
                };
                if (form.ShowDialog() == DialogResult.OK)
                {
                    switch (form.DeleteDuplicateOption)
                    {
                        case DeleteDuplicateOption.FromThisMarket:
                            MarketManager.smethod_24(symbols, symbols2, this.marketInfo_0);
                            this.txtSymbols.Text = symbols.Text;
                            break;

                        case DeleteDuplicateOption.FromOtherMarkets:
                            MarketManager.smethod_25(symbols2, this.marketInfo_0);
                            settings.SymbolList.Text = this.txtSymbols.Text;
                            this.txtSymbols.Text = settings.SymbolList.Text;
                            break;
                    }
                }
                else
                {
                    this.txtSymbols.Text = settings.SymbolList.Text;
                }
            }
            else
            {
                settings.SymbolList.Text = this.txtSymbols.Text.ToUpper();
                this.txtSymbols.Text = settings.SymbolList.Text;
            }
            MarketManager.smethod_16();
            this.btnApplySymbolsChange.Enabled = false;
        }

        private void cbDefaultMarket_CheckedChanged(object sender, EventArgs e)
        {
            if (this.bool_0)
            {
                if (this.cbDefaultMarket.Checked)
                {
                    MarketManager.smethod_22(this.marketInfo_0);
                }
                else
                {
                    MarketManager.smethod_22(null);
                }
            }
            this.method_31();
        }

        private void cmnuMain_Opening(object sender, CancelEventArgs e)
        {
            if ((sender as ContextMenuStrip).SourceControl is ListView)
            {
                string tag = (string) ((sender as ContextMenuStrip).SourceControl as ListView).Tag;
                string str2 = tag;
                if (str2 != null)
                {
                    if (str2 == "Markets")
                    {
                        MarketInfo info = this.method_0();
                        this.mniEdit.Enabled = this.mniRemove.Enabled = info != null;
                        if ((info != null) && MarketManager.smethod_3(info))
                        {
                            this.mniRemove.Enabled = false;
                            this.mniEdit.Enabled = false;
                        }
                    }
                    else if ((str2 == "Holidays") || (str2 == "SpecialHours"))
                    {
                        this.mniEdit.Enabled = this.mniRemove.Enabled = this.method_14(tag);
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_1 != null))
            {
                this.icontainer_1.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dtOpenTime_ValueChanged(object sender, EventArgs e)
        {
            if ((this.marketInfo_0 != null) && this.bool_0)
            {
                this.marketInfo_0.OpenTimeNative = new DateTime(0x7d7, 9, 0x13);
                this.marketInfo_0.OpenTimeNative = this.marketInfo_0.OpenTimeNative.Add(this.dtOpenTime.Value.TimeOfDay);
                this.marketInfo_0.CloseTimeNative = new DateTime(0x7d7, 9, 0x13);
                this.marketInfo_0.CloseTimeNative = this.marketInfo_0.CloseTimeNative.Add(this.dtCloseTime.Value.TimeOfDay);
                if (this.marketInfo_0.CloseTimeNative.TimeOfDay == TimeSpan.Zero)
                {
                    this.marketInfo_0.CloseTimeNative = new DateTime(0x7d7, 9, 0x13, 0x17, 0x3b, 0x3b);
                    this.dtCloseTime.Value = new DateTime(0x7d7, 9, 0x13, 0x17, 0x3b, 0x3b);
                }
                MarketManager.smethod_15();
            }
        }

        private void InitializeComponent_1()
        {
            this.icontainer_1 = new Container();
            this.pnlTop = new Panel();
            this.pnlBottom = new SplitContainer();
            this.lsvMarkets = new ListView();
            this.columnHeader_0 = new ColumnHeader();
            this.cmnuMain = new ContextMenuStrip(this.icontainer_1);
            this.mniNew = new ToolStripMenuItem();
            this.mniEdit = new ToolStripMenuItem();
            this.mniRemove = new ToolStripMenuItem();
            this.tabProperties = new TabControl();
            this.pgProperties = new TabPage();
            this.dtCloseTime = new DateTimePicker();
            this.lblCloseTime = new Label();
            this.dtOpenTime = new DateTimePicker();
            this.lblOpenTime = new Label();
            this.lblTimeZone = new Label();
            this.cbDefaultMarket = new CheckBox();
            this.pgHolidays = new TabPage();
            this.lsvHolidays = new ListView();
            this.columnHeader_1 = new ColumnHeader();
            this.pgSpecialHours = new TabPage();
            this.lsvSpecialHours = new ListView();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.pgSymbols = new TabPage();
            this.lblEnterSymbols = new Label();
            this.lnkCopyFromDataSet = new LinkLabel();
            this.btnApplySymbolsChange = new Button();
            this.txtSymbols = new TextBox();
            this.pnlMarketInfo = new Class8();
            this.lblMarketInfo = new Label();
            this.pictureBox1 = new PictureBox();
            this.pnlInfo = new Class8();
            this.lblInfo = new Label();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.Panel1.SuspendLayout();
            this.pnlBottom.Panel2.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.cmnuMain.SuspendLayout();
            this.tabProperties.SuspendLayout();
            this.pgProperties.SuspendLayout();
            this.pgHolidays.SuspendLayout();
            this.pgSpecialHours.SuspendLayout();
            this.pgSymbols.SuspendLayout();
            this.pnlMarketInfo.SuspendLayout();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            this.pnlInfo.SuspendLayout();
            base.SuspendLayout();
            this.pnlTop.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pnlTop.Controls.Add(this.pnlInfo);
            this.pnlTop.Location = new Point(3, 3);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new Size(590, 0x2e);
            this.pnlTop.TabIndex = 0;
            this.pnlBottom.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnlBottom.Location = new Point(3, 0x37);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Panel1.Controls.Add(this.lsvMarkets);
            this.pnlBottom.Panel2.Controls.Add(this.tabProperties);
            this.pnlBottom.Size = new Size(590, 0x132);
            this.pnlBottom.SplitterDistance = 0xc4;
            this.pnlBottom.TabIndex = 1;
            this.lsvMarkets.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0 });
            this.lsvMarkets.ContextMenuStrip = this.cmnuMain;
            this.lsvMarkets.Dock = DockStyle.Fill;
            this.lsvMarkets.FullRowSelect = true;
            this.lsvMarkets.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lsvMarkets.HideSelection = false;
            this.lsvMarkets.Location = new Point(0, 0);
            this.lsvMarkets.MultiSelect = false;
            this.lsvMarkets.Name = "lsvMarkets";
            this.lsvMarkets.Size = new Size(0xc4, 0x132);
            this.lsvMarkets.TabIndex = 0;
            this.lsvMarkets.Tag = "Markets";
            this.lsvMarkets.UseCompatibleStateImageBehavior = false;
            this.lsvMarkets.View = View.Details;
            this.lsvMarkets.SelectedIndexChanged += new EventHandler(this.lsvMarkets_SelectedIndexChanged);
            this.lsvMarkets.DoubleClick += new EventHandler(this.lsvSpecialHours_DoubleClick);
            this.lsvMarkets.KeyDown += new KeyEventHandler(this.lsvSpecialHours_KeyDown);
            this.columnHeader_0.Text = "Markets";
            this.columnHeader_0.Width = 150;
            this.cmnuMain.Items.AddRange(new ToolStripItem[] { this.mniNew, this.mniEdit, this.mniRemove });
            this.cmnuMain.Name = "cmnuMain";
            this.cmnuMain.Size = new Size(0x7d, 70);
            this.cmnuMain.Opening += new CancelEventHandler(this.cmnuMain_Opening);
            this.mniNew.Name = "mniNew";
            this.mniNew.Size = new Size(0x7c, 0x16);
            this.mniNew.Tag = "New";
            this.mniNew.Text = "New";
            this.mniNew.Click += new EventHandler(this.mniRemove_Click);
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.Size = new Size(0x7c, 0x16);
            this.mniEdit.Tag = "Edit";
            this.mniEdit.Text = "Edit";
            this.mniEdit.Click += new EventHandler(this.mniRemove_Click);
            this.mniRemove.Name = "mniRemove";
            this.mniRemove.Size = new Size(0x7c, 0x16);
            this.mniRemove.Tag = "Remove";
            this.mniRemove.Text = "Remove";
            this.mniRemove.Click += new EventHandler(this.mniRemove_Click);
            this.tabProperties.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabProperties.Controls.Add(this.pgProperties);
            this.tabProperties.Controls.Add(this.pgHolidays);
            this.tabProperties.Controls.Add(this.pgSpecialHours);
            this.tabProperties.Controls.Add(this.pgSymbols);
            this.tabProperties.Location = new Point(3, 0);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.SelectedIndex = 0;
            this.tabProperties.Size = new Size(0x185, 0x134);
            this.tabProperties.TabIndex = 0;
            this.pgProperties.Controls.Add(this.pnlMarketInfo);
            this.pgProperties.Controls.Add(this.dtCloseTime);
            this.pgProperties.Controls.Add(this.lblCloseTime);
            this.pgProperties.Controls.Add(this.dtOpenTime);
            this.pgProperties.Controls.Add(this.lblOpenTime);
            this.pgProperties.Controls.Add(this.lblTimeZone);
            this.pgProperties.Controls.Add(this.cbDefaultMarket);
            this.pgProperties.Location = new Point(4, 0x16);
            this.pgProperties.Name = "pgProperties";
            this.pgProperties.Padding = new Padding(3);
            this.pgProperties.Size = new Size(0x17d, 0x11a);
            this.pgProperties.TabIndex = 0;
            this.pgProperties.Text = "Properties";
            this.pgProperties.UseVisualStyleBackColor = true;
            this.dtCloseTime.Format = DateTimePickerFormat.Time;
            this.dtCloseTime.Location = new Point(70, 0x24);
            this.dtCloseTime.Name = "dtCloseTime";
            this.dtCloseTime.ShowUpDown = true;
            this.dtCloseTime.Size = new Size(0x55, 20);
            this.dtCloseTime.TabIndex = 6;
            this.dtCloseTime.Value = new DateTime(0x7d9, 2, 3, 0x10, 0, 0, 0);
            this.dtCloseTime.ValueChanged += new EventHandler(this.dtOpenTime_ValueChanged);
            this.lblCloseTime.AutoSize = true;
            this.lblCloseTime.Location = new Point(6, 40);
            this.lblCloseTime.Name = "lblCloseTime";
            this.lblCloseTime.Size = new Size(0x3e, 13);
            this.lblCloseTime.TabIndex = 5;
            this.lblCloseTime.Text = "Close Time:";
            this.dtOpenTime.Format = DateTimePickerFormat.Time;
            this.dtOpenTime.Location = new Point(70, 10);
            this.dtOpenTime.Name = "dtOpenTime";
            this.dtOpenTime.ShowUpDown = true;
            this.dtOpenTime.Size = new Size(0x55, 20);
            this.dtOpenTime.TabIndex = 4;
            this.dtOpenTime.Value = new DateTime(0x7d9, 2, 3, 9, 30, 0, 0);
            this.dtOpenTime.ValueChanged += new EventHandler(this.dtOpenTime_ValueChanged);
            this.lblOpenTime.AutoSize = true;
            this.lblOpenTime.Location = new Point(6, 14);
            this.lblOpenTime.Name = "lblOpenTime";
            this.lblOpenTime.Size = new Size(0x3e, 13);
            this.lblOpenTime.TabIndex = 3;
            this.lblOpenTime.Text = "Open Time:";
            this.lblTimeZone.AutoSize = true;
            this.lblTimeZone.Location = new Point(6, 0xaf);
            this.lblTimeZone.Name = "lblTimeZone";
            this.lblTimeZone.Size = new Size(0x3d, 13);
            this.lblTimeZone.TabIndex = 1;
            this.lblTimeZone.Text = "Time Zone:";
            this.lblTimeZone.Visible = false;
            this.cbDefaultMarket.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.cbDefaultMarket.AutoEllipsis = true;
            this.cbDefaultMarket.Location = new Point(9, 0x3e);
            this.cbDefaultMarket.Name = "cbDefaultMarket";
            this.cbDefaultMarket.Size = new Size(0x169, 0x13);
            this.cbDefaultMarket.TabIndex = 0;
            this.cbDefaultMarket.Text = "Use this Market by default for symbols not specified in other markets.";
            this.cbDefaultMarket.UseVisualStyleBackColor = true;
            this.cbDefaultMarket.CheckedChanged += new EventHandler(this.cbDefaultMarket_CheckedChanged);
            this.pgHolidays.Controls.Add(this.lsvHolidays);
            this.pgHolidays.Location = new Point(4, 0x16);
            this.pgHolidays.Name = "pgHolidays";
            this.pgHolidays.Size = new Size(0x17d, 0x11a);
            this.pgHolidays.TabIndex = 2;
            this.pgHolidays.Text = "Holidays";
            this.pgHolidays.UseVisualStyleBackColor = true;
            this.lsvHolidays.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lsvHolidays.Columns.AddRange(new ColumnHeader[] { this.columnHeader_1 });
            this.lsvHolidays.ContextMenuStrip = this.cmnuMain;
            this.lsvHolidays.FullRowSelect = true;
            this.lsvHolidays.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lsvHolidays.HideSelection = false;
            this.lsvHolidays.Location = new Point(3, 3);
            this.lsvHolidays.MultiSelect = false;
            this.lsvHolidays.Name = "lsvHolidays";
            this.lsvHolidays.Size = new Size(370, 0x10c);
            this.lsvHolidays.TabIndex = 1;
            this.lsvHolidays.Tag = "Holidays";
            this.lsvHolidays.UseCompatibleStateImageBehavior = false;
            this.lsvHolidays.View = View.Details;
            this.lsvHolidays.DoubleClick += new EventHandler(this.lsvSpecialHours_DoubleClick);
            this.lsvHolidays.KeyDown += new KeyEventHandler(this.lsvSpecialHours_KeyDown);
            this.columnHeader_1.Text = "Date";
            this.columnHeader_1.Width = 120;
            this.pgSpecialHours.Controls.Add(this.lsvSpecialHours);
            this.pgSpecialHours.Location = new Point(4, 0x16);
            this.pgSpecialHours.Name = "pgSpecialHours";
            this.pgSpecialHours.Size = new Size(0x17d, 0x11a);
            this.pgSpecialHours.TabIndex = 3;
            this.pgSpecialHours.Text = "Special Hours";
            this.pgSpecialHours.UseVisualStyleBackColor = true;
            this.lsvSpecialHours.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lsvSpecialHours.Columns.AddRange(new ColumnHeader[] { this.columnHeader_2, this.columnHeader_3, this.columnHeader_4 });
            this.lsvSpecialHours.ContextMenuStrip = this.cmnuMain;
            this.lsvSpecialHours.FullRowSelect = true;
            this.lsvSpecialHours.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lsvSpecialHours.HideSelection = false;
            this.lsvSpecialHours.Location = new Point(3, 3);
            this.lsvSpecialHours.MultiSelect = false;
            this.lsvSpecialHours.Name = "lsvSpecialHours";
            this.lsvSpecialHours.Size = new Size(370, 0x10c);
            this.lsvSpecialHours.TabIndex = 1;
            this.lsvSpecialHours.Tag = "SpecialHours";
            this.lsvSpecialHours.UseCompatibleStateImageBehavior = false;
            this.lsvSpecialHours.View = View.Details;
            this.lsvSpecialHours.DoubleClick += new EventHandler(this.lsvSpecialHours_DoubleClick);
            this.lsvSpecialHours.KeyDown += new KeyEventHandler(this.lsvSpecialHours_KeyDown);
            this.columnHeader_2.Text = "Date";
            this.columnHeader_2.Width = 120;
            this.columnHeader_3.Text = "Open Time";
            this.columnHeader_3.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_3.Width = 90;
            this.columnHeader_4.Text = "Close Time";
            this.columnHeader_4.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_4.Width = 90;
            this.pgSymbols.Controls.Add(this.lblEnterSymbols);
            this.pgSymbols.Controls.Add(this.lnkCopyFromDataSet);
            this.pgSymbols.Controls.Add(this.btnApplySymbolsChange);
            this.pgSymbols.Controls.Add(this.txtSymbols);
            this.pgSymbols.Location = new Point(4, 0x16);
            this.pgSymbols.Name = "pgSymbols";
            this.pgSymbols.Padding = new Padding(3);
            this.pgSymbols.Size = new Size(0x17d, 0x11a);
            this.pgSymbols.TabIndex = 1;
            this.pgSymbols.Text = "Symbols";
            this.pgSymbols.UseVisualStyleBackColor = true;
            this.lblEnterSymbols.AutoEllipsis = true;
            this.lblEnterSymbols.AutoSize = true;
            this.lblEnterSymbols.Location = new Point(3, 6);
            this.lblEnterSymbols.Name = "lblEnterSymbols";
            this.lblEnterSymbols.Size = new Size(0x10a, 13);
            this.lblEnterSymbols.TabIndex = 3;
            this.lblEnterSymbols.Text = "Enter Symbols below, separated by spaces or commas:";
            this.lnkCopyFromDataSet.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lnkCopyFromDataSet.AutoSize = true;
            this.lnkCopyFromDataSet.LinkBehavior = LinkBehavior.AlwaysUnderline;
            this.lnkCopyFromDataSet.Location = new Point(2, 250);
            this.lnkCopyFromDataSet.Name = "lnkCopyFromDataSet";
            this.lnkCopyFromDataSet.Size = new Size(0x69, 13);
            this.lnkCopyFromDataSet.TabIndex = 2;
            this.lnkCopyFromDataSet.TabStop = true;
            this.lnkCopyFromDataSet.Text = "Copy from a DataSet";
            this.lnkCopyFromDataSet.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkCopyFromDataSet_LinkClicked);
            this.btnApplySymbolsChange.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnApplySymbolsChange.Location = new Point(240, 0xf7);
            this.btnApplySymbolsChange.Name = "btnApplySymbolsChange";
            this.btnApplySymbolsChange.Size = new Size(0x85, 0x18);
            this.btnApplySymbolsChange.TabIndex = 1;
            this.btnApplySymbolsChange.Text = "Apply Symbols Change";
            this.btnApplySymbolsChange.UseVisualStyleBackColor = true;
            this.btnApplySymbolsChange.Click += new EventHandler(this.btnApplySymbolsChange_Click);
            this.txtSymbols.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.txtSymbols.Location = new Point(3, 0x16);
            this.txtSymbols.Multiline = true;
            this.txtSymbols.Name = "txtSymbols";
            this.txtSymbols.ScrollBars = ScrollBars.Both;
            this.txtSymbols.Size = new Size(370, 0xd8);
            this.txtSymbols.TabIndex = 0;
            this.txtSymbols.TextChanged += new EventHandler(this.txtSymbols_TextChanged);
            this.pnlMarketInfo.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pnlMarketInfo.BackColor = Color.FromArgb(0xed, 0xef, 0xfc);
            this.pnlMarketInfo.method_1(Color.FromArgb(220, 0xe0, 250));
            this.pnlMarketInfo.method_3(1);
            this.pnlMarketInfo.Controls.Add(this.lblMarketInfo);
            this.pnlMarketInfo.Controls.Add(this.pictureBox1);
            this.pnlMarketInfo.Location = new Point(9, 0x59);
            this.pnlMarketInfo.Name = "pnlMarketInfo";
            this.pnlMarketInfo.Size = new Size(0x169, 0x2e);
            this.pnlMarketInfo.TabIndex = 7;
            this.lblMarketInfo.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lblMarketInfo.Location = new Point(0x15, 5);
            this.lblMarketInfo.Name = "lblMarketInfo";
            this.lblMarketInfo.Size = new Size(0x14b, 0x24);
            this.lblMarketInfo.TabIndex = 1;
            this.lblMarketInfo.Text = "lblMarketInfo";
            this.lblMarketInfo.TextAlign = ContentAlignment.MiddleCenter;
            this.pictureBox1.Image = Resources.information;
            this.pictureBox1.Location = new Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x10, 0x10);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pnlInfo.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pnlInfo.BackColor = Color.FromArgb(0xff, 0xff, 0xe1);
            this.pnlInfo.method_1(Color.FromArgb(0xfc, 0xf2, 0xad));
            this.pnlInfo.method_3(1);
            this.pnlInfo.Controls.Add(this.lblInfo);
            this.pnlInfo.Location = new Point(0, 3);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new Size(590, 40);
            this.pnlInfo.TabIndex = 0;
            this.lblInfo.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.lblInfo.AutoEllipsis = true;
            this.lblInfo.Location = new Point(3, 2);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new Size(0x247, 0x25);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Use this tool to configure properties for the exchanges where your symbols are traded at.  \r\nClick here to view and manage data providers supported by this tool.";
            this.lblInfo.TextAlign = ContentAlignment.MiddleCenter;
            this.lblInfo.Click += new EventHandler(this.lblInfo_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.pnlBottom);
            base.Controls.Add(this.pnlTop);
            base.Name = "MarketManagerControl";
            base.Size = new Size(0x254, 0x16c);
            base.Tag = "Market Manager";
            this.pnlTop.ResumeLayout(false);
            this.pnlBottom.Panel1.ResumeLayout(false);
            this.pnlBottom.Panel2.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.cmnuMain.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            this.pgProperties.ResumeLayout(false);
            this.pgProperties.PerformLayout();
            this.pgHolidays.ResumeLayout(false);
            this.pgSpecialHours.ResumeLayout(false);
            this.pgSymbols.ResumeLayout(false);
            this.pgSymbols.PerformLayout();
            this.pnlMarketInfo.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBox1).EndInit();
            this.pnlInfo.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void lblInfo_Click(object sender, EventArgs e)
        {
            ProvidersForm form = new ProvidersForm();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                MarketManager.smethod_6();
            }
        }

        private void lnkCopyFromDataSet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DataSetsForm form = new DataSetsForm();
            form.Initialize();
            if ((form.ShowDialog(this) == DialogResult.OK) && (form.SelectedSymbols != null))
            {
                StringBuilder builder = new StringBuilder();
                foreach (string str in form.SelectedSymbols)
                {
                    builder.Append(str);
                    builder.Append(" ");
                }
                this.txtSymbols.Text = this.txtSymbols.Text + " " + builder.ToString();
            }
        }

        private void lsvMarkets_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.bool_0 = false;
            this.btnApplySymbolsChange.Enabled = false;
            try
            {
                if (this.lsvMarkets.SelectedItems.Count != 0)
                {
                    this.marketInfo_0 = (MarketInfo) this.lsvMarkets.SelectedItems[0].Tag;
                    this.listViewItem_0 = this.lsvMarkets.SelectedItems[0];
                    this.lsvHolidays.Items.Clear();
                    this.lsvSpecialHours.Items.Clear();
                    this.method_7(this.marketInfo_0);
                    this.method_9(this.marketInfo_0);
                    this.method_13(this.marketInfo_0);
                    this.dtCloseTime.Value = this.marketInfo_0.CloseTimeNative;
                    this.dtOpenTime.Value = this.marketInfo_0.OpenTimeNative;
                    this.cbDefaultMarket.Checked = MarketManager.smethod_17(this.marketInfo_0.Name).UseMarketByDefault;
                    this.txtSymbols.Text = MarketManager.smethod_17(this.marketInfo_0.Name).SymbolList.Text;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.bool_0 = true;
            }
        }

        private void lsvSpecialHours_DoubleClick(object sender, EventArgs e)
        {
            string tag = (string) (sender as ListView).Tag;
            if (this.method_14(tag))
            {
                this.method_26(tag, "Edit");
            }
        }

        private void lsvSpecialHours_KeyDown(object sender, KeyEventArgs e)
        {
            string tag = (string) (sender as ListView).Tag;
            if (e.KeyCode == Keys.Insert)
            {
                if (((tag == "Holidays") && (this.marketInfo_0 != null)) && MarketManager.smethod_17(this.marketInfo_0.Name).HolidaysReadOnly)
                {
                    return;
                }
                if (((tag == "SpecialHours") && (this.marketInfo_0 != null)) && MarketManager.smethod_17(this.marketInfo_0.Name).SpecialHoursReadOnly)
                {
                    return;
                }
                this.method_26(tag, "New");
            }
            if ((e.KeyCode == Keys.Delete) && this.method_14(tag))
            {
                this.method_26(tag, "Remove");
            }
            if ((e.KeyCode == Keys.Enter) && this.method_14(tag))
            {
                this.method_26(tag, "Edit");
            }
        }

        private MarketInfo method_0()
        {
            return (MarketInfo) this.method_3(this.lsvMarkets);
        }

        private DateTime? method_1()
        {
            object obj2 = this.method_3(this.lsvHolidays);
            if (obj2 == null)
            {
                return null;
            }
            return new DateTime?((DateTime) obj2);
        }

        private void method_10()
        {
        }

        private void method_11(ListView listView_0)
        {
            listView_0.BackColor = SystemColors.Control;
            listView_0.ContextMenuStrip = null;
        }

        private void method_12(ListView listView_0)
        {
            listView_0.BackColor = SystemColors.Window;
            listView_0.ContextMenuStrip = this.cmnuMain;
        }

        private void method_13(MarketInfo marketInfo_1)
        {
            this.pnlMarketInfo.Visible = false;
            MarketInfoSettings settings = MarketManager.smethod_17(marketInfo_1.Name);
            this.cbDefaultMarket.Checked = settings.UseMarketByDefault;
            if (MarketManager.smethod_3(marketInfo_1))
            {
                this.method_11(this.lsvHolidays);
                this.method_11(this.lsvSpecialHours);
                this.lblMarketInfo.Text = "Editing is disabled for this market: Name, Properties, Holidays, Special Hours";
                this.pnlMarketInfo.Visible = true;
            }
            else
            {
                this.method_12(this.lsvHolidays);
                this.method_12(this.lsvSpecialHours);
            }
        }

        private bool method_14(string string_0)
        {
            switch (string_0)
            {
                case "Markets":
                {
                    MarketInfo info = this.method_0();
                    if (info == null)
                    {
                        return false;
                    }
                    return !MarketManager.smethod_17(info.Name).NameReadOnly;
                }
                case "Holidays":
                    return ((this.method_1().HasValue && (this.marketInfo_0 != null)) && !MarketManager.smethod_17(this.marketInfo_0.Name).HolidaysReadOnly);

                case "SpecialHours":
                    return (((this.method_2() != null) && (this.marketInfo_0 != null)) && !MarketManager.smethod_17(this.marketInfo_0.Name).SpecialHoursReadOnly);
            }
            return false;
        }

        private void method_15()
        {
        }

        private MarketInfo method_16(string string_0)
        {
            return new MarketInfo { Name = string_0, OpenTimeNative = new DateTime(0x7d7, 9, 0x13, 0, 0, 0), CloseTimeNative = new DateTime(0x7d7, 9, 0x13, 0x17, 0x3b, 0x3b), TimeZoneName = "Eastern Standard Time" };
        }

        private void method_17()
        {
            MarketNameDialogForm form = new MarketNameDialogForm();
            form.Initialize(this.marketInfo_0);
            if (form.ShowDialog() == DialogResult.OK)
            {
                MarketInfo item = this.method_16(form.MarketName);
                MarketManager.ArrayOfMarketInfo.Add(item);
                this.method_4(item);
                MarketManager.smethod_15();
                this.lsvMarkets.Items[this.lsvMarkets.Items.Count - 1].Selected = true;
            }
        }

        private void method_18()
        {
            MarketNameDialogForm form = new MarketNameDialogForm();
            form.Initialize(this.marketInfo_0);
            form.MarketName = this.marketInfo_0.Name;
            if (form.ShowDialog() == DialogResult.OK)
            {
                MarketInfoSettings settings = MarketManager.smethod_17(this.marketInfo_0.Name);
                this.marketInfo_0.Name = this.listViewItem_0.Text = form.MarketName;
                settings.Name = this.marketInfo_0.Name;
                MarketManager.smethod_15();
                MarketManager.smethod_16();
            }
        }

        private void method_19()
        {
            if (this.method_28() == DialogResult.OK)
            {
                MarketManager.ArrayOfMarketInfo.Remove(this.marketInfo_0);
                MarketManager.ArrayOfMarketInfoSettings.Remove(MarketManager.smethod_17(this.marketInfo_0.Name));
                this.marketInfo_0 = null;
                this.method_27(this.lsvMarkets);
                MarketManager.smethod_15();
                MarketManager.smethod_16();
            }
        }

        private MarketSpecialHours method_2()
        {
            return (MarketSpecialHours) this.method_3(this.lsvSpecialHours);
        }

        private void method_20()
        {
            HolidayDateDialogForm form = new HolidayDateDialogForm();
            form.Initialize(this.marketInfo_0);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.marketInfo_0.Holidays.Add(form.HolidayDate);
                MarketManager.smethod_19(this.marketInfo_0);
                this.method_6(form.HolidayDate);
                MarketManager.smethod_15();
                this.lsvHolidays.Items[this.lsvHolidays.Items.Count - 1].Selected = true;
            }
        }

        private void method_21()
        {
            HolidayDateDialogForm form = new HolidayDateDialogForm();
            form.Initialize(this.marketInfo_0);
            form.HolidayDate = this.method_1().Value;
            if (form.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < this.marketInfo_0.Holidays.Count; i++)
                {
                    DateTime time2 = this.marketInfo_0.Holidays[i];
                    if (time2.Date.Equals(this.method_1()))
                    {
                        this.marketInfo_0.Holidays[i] = form.HolidayDate;
                    }
                }
                this.lsvHolidays.SelectedItems[0].Tag = form.HolidayDate;
                this.lsvHolidays.SelectedItems[0].Text = form.HolidayDate.ToShortDateString();
                MarketManager.smethod_19(this.marketInfo_0);
                MarketManager.smethod_15();
            }
        }

        private void method_22()
        {
            this.marketInfo_0.Holidays.Remove(this.method_1().Value);
            this.method_27(this.lsvHolidays);
            MarketManager.smethod_15();
        }

        private void method_23()
        {
            SpecialHoursDialogForm form = new SpecialHoursDialogForm();
            form.Initialize(this.marketInfo_0);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.marketInfo_0.SpecialHours.Add(form.MarketSpecialHours);
                MarketManager.smethod_21(this.marketInfo_0);
                this.method_8(form.MarketSpecialHours);
                MarketManager.smethod_15();
                this.lsvSpecialHours.Items[this.lsvSpecialHours.Items.Count - 1].Selected = true;
            }
        }

        private void method_24()
        {
            SpecialHoursDialogForm form = new SpecialHoursDialogForm();
            form.Initialize(this.marketInfo_0);
            form.MarketSpecialHours = this.method_2();
            if (form.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < this.marketInfo_0.SpecialHours.Count; i++)
                {
                    if (this.marketInfo_0.SpecialHours[i].Date.Equals(this.method_2().Date))
                    {
                        this.marketInfo_0.SpecialHours[i] = form.MarketSpecialHours;
                    }
                }
                this.lsvSpecialHours.SelectedItems[0].SubItems.Clear();
                this.lsvSpecialHours.SelectedItems[0].Tag = form.MarketSpecialHours;
                this.lsvSpecialHours.SelectedItems[0].Text = form.MarketSpecialHours.Date.ToShortDateString();
                this.lsvSpecialHours.SelectedItems[0].SubItems.Add(form.MarketSpecialHours.OpenTimeNative.ToShortTimeString());
                this.lsvSpecialHours.SelectedItems[0].SubItems.Add(form.MarketSpecialHours.CloseTimeNative.ToShortTimeString());
                MarketManager.smethod_21(this.marketInfo_0);
                MarketManager.smethod_15();
            }
        }

        private void method_25()
        {
            DateTime date = this.method_2().Date;
            for (int i = this.marketInfo_0.SpecialHours.Count - 1; i >= 0; i--)
            {
                if (this.marketInfo_0.SpecialHours[i].Date.Date == date.Date.Date)
                {
                    this.marketInfo_0.SpecialHours.RemoveAt(i);
                    this.method_27(this.lsvSpecialHours);
                }
            }
            MarketManager.smethod_15();
        }

        private void method_26(string string_0, string string_1)
        {
            string str = string_0;
            if (str != null)
            {
                if (str == "Markets")
                {
                    switch (string_1)
                    {
                        case "New":
                            this.method_17();
                            return;

                        case "Edit":
                            this.method_18();
                            break;

                        case "Remove":
                            this.method_19();
                            break;
                    }
                }
                else if (str == "Holidays")
                {
                    string str3 = string_1;
                    if (str3 != null)
                    {
                        switch (str3)
                        {
                            case "New":
                                this.method_20();
                                return;

                            case "Edit":
                                this.method_21();
                                return;

                            case "Remove":
                                this.method_22();
                                break;
                        }
                    }
                }
                else
                {
                    string str4;
                    if ((str == "SpecialHours") && ((str4 = string_1) != null))
                    {
                        if (str4 == "New")
                        {
                            this.method_23();
                        }
                        else if (str4 == "Edit")
                        {
                            this.method_24();
                        }
                        else if (str4 == "Remove")
                        {
                            this.method_25();
                        }
                    }
                }
            }
        }

        private void method_27(ListView listView_0)
        {
            if (listView_0.SelectedItems.Count > 0)
            {
                int index = listView_0.SelectedItems[0].Index;
                listView_0.Items.RemoveAt(index);
                if (index < listView_0.Items.Count)
                {
                    listView_0.Items[index].Selected = true;
                }
                else if (listView_0.Items.Count > 0)
                {
                    listView_0.Items[listView_0.Items.Count - 1].Selected = true;
                }
            }
        }

        private DialogResult method_28()
        {
            return MessageBox.Show("Are you sure you want to delete this market?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        private void method_29(object sender, DrawItemEventArgs e)
        {
        }

        private object method_3(ListView listView_0)
        {
            if (listView_0.SelectedItems.Count > 0)
            {
                return listView_0.SelectedItems[0].Tag;
            }
            return null;
        }

        private void method_30()
        {
            MessageBox.Show("Not Implemented");
        }

        private void method_31()
        {
            foreach (ListViewItem item in this.lsvMarkets.Items)
            {
                if (MarketManager.smethod_17((item.Tag as MarketInfo).Name).UseMarketByDefault)
                {
                    item.Font = new Font(this.lsvMarkets.Font, FontStyle.Bold);
                }
                else if (item.Font.Bold)
                {
                    item.Font = this.lsvMarkets.Font;
                }
            }
        }

        private void method_4(MarketInfo marketInfo_1)
        {
            ListViewItem item = new ListViewItem(marketInfo_1.Name) {
                Tag = marketInfo_1
            };
            this.lsvMarkets.Items.Add(item);
        }

        private void method_5()
        {
            this.lsvMarkets.BeginUpdate();
            try
            {
                foreach (MarketInfo info in MarketManager.ArrayOfMarketInfo)
                {
                    this.method_4(info);
                }
            }
            finally
            {
                this.lsvMarkets.EndUpdate();
            }
        }

        private void method_6(DateTime dateTime_0)
        {
            ListViewItem item = new ListViewItem(dateTime_0.ToShortDateString()) {
                Tag = dateTime_0
            };
            this.lsvHolidays.Items.Add(item);
        }

        private void method_7(MarketInfo marketInfo_1)
        {
            this.lsvHolidays.BeginUpdate();
            try
            {
                foreach (DateTime time in marketInfo_1.Holidays)
                {
                    this.method_6(time);
                }
            }
            finally
            {
                this.lsvHolidays.EndUpdate();
            }
        }

        private void method_8(MarketSpecialHours marketSpecialHours_0)
        {
            ListViewItem item = new ListViewItem(marketSpecialHours_0.Date.ToShortDateString());
            item.SubItems.Add(marketSpecialHours_0.OpenTimeNative.ToShortTimeString());
            item.SubItems.Add(marketSpecialHours_0.CloseTimeNative.ToShortTimeString());
            item.Tag = marketSpecialHours_0;
            this.lsvSpecialHours.Items.Add(item);
        }

        private void method_9(MarketInfo marketInfo_1)
        {
            this.lsvSpecialHours.BeginUpdate();
            try
            {
                foreach (MarketSpecialHours hours in marketInfo_1.SpecialHours)
                {
                    this.method_8(hours);
                }
            }
            finally
            {
                this.lsvSpecialHours.EndUpdate();
            }
        }

        private void mniRemove_Click(object sender, EventArgs e)
        {
            string tag = (string) (((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as ListView).Tag;
            string str2 = (string) (sender as ToolStripMenuItem).Tag;
            this.method_26(tag, str2);
        }

        private void txtSymbols_TextChanged(object sender, EventArgs e)
        {
            if (this.bool_0)
            {
                this.btnApplySymbolsChange.Enabled = true;
            }
        }
    }
}

