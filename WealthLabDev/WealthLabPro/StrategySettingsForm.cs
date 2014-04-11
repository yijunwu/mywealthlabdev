namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class StrategySettingsForm : Form
    {
        private AccountTypeSelector accountTypeSelector1;
        private Button btnCancel;
        private Button btnOK;
        private CheckBox cbEmailOrders;
        private CheckBox cbOrders;
        private CheckBox cbPV;
        private ComboBox cmbAccounts;
        private ComboBox cmbHours;
        private ComboBox cmbMin;
        private BarDataRangeSelecter dataRange;
        private GroupBox groupBox1;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label5;
        private Label label6;
        private Label lblAccount;
        private Label lblData;
        private Label lblExecute;
        private Label lblExecuteLocal;
        private Label lblLocalTime;
        private Label lblMarketClose;
        private Label lblParameters;
        private Label lblPosSize;
        private Label lblScale;
        private Label lblSource;
        private Label lblTradeType;
        private ParameterSlidersContainer paramSliders;
        private PositionSizeSelecter posSize;
        private ScaleSelecter scale;
        private StrategyCenterItem strategyCenterItem;
        private DataSourceTreeView tree;

        public StrategySettingsForm()
        {
            this.InitializeComponent();
            new ToolTip().SetToolTip(this.accountTypeSelector1, "Trade Type for Buy and Sell Alerts");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
        }

        private void cbPV_CheckStateChanged(object sender, EventArgs e)
        {
            this.method_1();
        }

        private void cmbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnOK.Enabled = (this.cmbAccounts.SelectedItem != null) && (this.tree.SelectedNode != null);
            this.accountTypeSelector1.InitAccountTradeType(this.cmbAccounts.Text, "");
        }

        private void cmbHours_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.method_3();
        }

        private void cmbMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.method_3();
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
            this.lblData = new Label();
            this.lblPosSize = new Label();
            this.lblScale = new Label();
            this.lblSource = new Label();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.cbOrders = new CheckBox();
            this.lblParameters = new Label();
            this.paramSliders = new ParameterSlidersContainer();
            this.tree = new DataSourceTreeView();
            this.lblAccount = new Label();
            this.cmbAccounts = new ComboBox();
            this.cbPV = new CheckBox();
            this.cbEmailOrders = new CheckBox();
            this.lblTradeType = new Label();
            this.groupBox1 = new GroupBox();
            this.lblExecuteLocal = new Label();
            this.label5 = new Label();
            this.lblLocalTime = new Label();
            this.label6 = new Label();
            this.lblExecute = new Label();
            this.cmbMin = new ComboBox();
            this.label2 = new Label();
            this.cmbHours = new ComboBox();
            this.label3 = new Label();
            this.lblMarketClose = new Label();
            this.label1 = new Label();
            this.accountTypeSelector1 = new AccountTypeSelector(this.components);
            this.scale = new ScaleSelecter();
            this.posSize = new PositionSizeSelecter();
            this.dataRange = new BarDataRangeSelecter();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.lblData.AutoSize = true;
            this.lblData.Location = new Point(9, 0x29);
            this.lblData.Name = "lblData";
            this.lblData.Size = new Size(0x44, 13);
            this.lblData.TabIndex = 2;
            this.lblData.Text = "Data Range:";
            this.lblPosSize.AutoSize = true;
            this.lblPosSize.Location = new Point(9, 0x43);
            this.lblPosSize.Name = "lblPosSize";
            this.lblPosSize.Size = new Size(70, 13);
            this.lblPosSize.TabIndex = 4;
            this.lblPosSize.Text = "Position Size:";
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new Point(9, 0x5d);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new Size(0x25, 13);
            this.lblScale.TabIndex = 6;
            this.lblScale.Text = "Scale:";
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new Point(0x107, 9);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new Size(0xd5, 13);
            this.lblSource.TabIndex = 10;
            this.lblSource.Text = "Source Data (select a DataSet or a Symbol)";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x1c7, 0x1c8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new Point(0x176, 0x1c8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.cbOrders.AutoSize = true;
            this.cbOrders.Location = new Point(11, 0x1a9);
            this.cbOrders.Name = "cbOrders";
            this.cbOrders.Size = new Size(0xf7, 0x11);
            this.cbOrders.TabIndex = 12;
            this.cbOrders.Text = "Automatically Stage Orders from Strategy Alerts";
            this.cbOrders.UseVisualStyleBackColor = true;
            this.lblParameters.AutoSize = true;
            this.lblParameters.Location = new Point(8, 0xfc);
            this.lblParameters.Name = "lblParameters";
            this.lblParameters.Size = new Size(0x66, 13);
            this.lblParameters.TabIndex = 8;
            this.lblParameters.Text = "Strategy Parameters";
            this.paramSliders.Location = new Point(11, 270);
            this.paramSliders.Name = "paramSliders";
            this.paramSliders.Size = new Size(0xf9, 0x80);
            this.paramSliders.TabIndex = 9;
            this.paramSliders.WealthScript = null;
            this.tree.HideSelection = false;
            this.tree.ImageIndex = 0;
            this.tree.Location = new Point(0x10a, 0x1a);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = 0;
            this.tree.Size = new Size(0x123, 0x174);
            this.tree.TabIndex = 0;
            this.tree.AfterSelect += new TreeViewEventHandler(this.tree_AfterSelect);
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new Point(9, 14);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new Size(50, 13);
            this.lblAccount.TabIndex = 0;
            this.lblAccount.Text = "Account:";
            this.cmbAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAccounts.FormattingEnabled = true;
            this.cmbAccounts.Location = new Point(100, 10);
            this.cmbAccounts.Name = "cmbAccounts";
            this.cmbAccounts.Size = new Size(0x8a, 0x15);
            this.cmbAccounts.TabIndex = 1;
            this.cmbAccounts.SelectedIndexChanged += new EventHandler(this.cmbAccounts_SelectedIndexChanged);
            this.cbPV.AutoSize = true;
            this.cbPV.Location = new Point(11, 0x192);
            this.cbPV.Name = "cbPV";
            this.cbPV.Size = new Size(0x14f, 0x11);
            this.cbPV.TabIndex = 15;
            this.cbPV.Text = "Use Preferred Strategy Parameter Values assigned in Optimization";
            this.cbPV.UseVisualStyleBackColor = true;
            this.cbPV.CheckStateChanged += new EventHandler(this.cbPV_CheckStateChanged);
            this.cbEmailOrders.AutoSize = true;
            this.cbEmailOrders.Location = new Point(11, 0x1c0);
            this.cbEmailOrders.Name = "cbEmailOrders";
            this.cbEmailOrders.Size = new Size(270, 0x11);
            this.cbEmailOrders.TabIndex = 0x10;
            this.cbEmailOrders.Text = "Automatically Email Trade Alerts from Strategy Alerts";
            this.cbEmailOrders.UseVisualStyleBackColor = true;
            this.lblTradeType.AutoSize = true;
            this.lblTradeType.Location = new Point(8, 0x76);
            this.lblTradeType.Name = "lblTradeType";
            this.lblTradeType.Size = new Size(0x3e, 13);
            this.lblTradeType.TabIndex = 0x11;
            this.lblTradeType.Text = "Trade Type";
            this.groupBox1.Controls.Add(this.lblExecuteLocal);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblLocalTime);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblExecute);
            this.groupBox1.Controls.Add(this.cmbMin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbHours);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblMarketClose);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.groupBox1.Location = new Point(6, 0x8b);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x100, 0x6c);
            this.groupBox1.TabIndex = 0x13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Daily Strategy Scheduling";
            this.lblExecuteLocal.AutoSize = true;
            this.lblExecuteLocal.Location = new Point(0xaf, 0x52);
            this.lblExecuteLocal.Name = "lblExecuteLocal";
            this.lblExecuteLocal.Size = new Size(0x34, 13);
            this.lblExecuteLocal.TabIndex = 0x12;
            this.lblExecuteLocal.Text = "timeoffset";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(3, 0x25);
            this.label5.Name = "label5";
            this.label5.Size = new Size(250, 13);
            this.label5.TabIndex = 0x11;
            this.label5.Text = "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -";
            this.lblLocalTime.AutoSize = true;
            this.lblLocalTime.Location = new Point(0x84, 0x52);
            this.lblLocalTime.Name = "lblLocalTime";
            this.lblLocalTime.Size = new Size(0x2a, 13);
            this.lblLocalTime.TabIndex = 8;
            this.lblLocalTime.Text = "HH:mm";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(3, 0x51);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x4e, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Execute Local:";
            this.lblExecute.AutoSize = true;
            this.lblExecute.Location = new Point(0xaf, 0x38);
            this.lblExecute.Name = "lblExecute";
            this.lblExecute.Size = new Size(0x34, 13);
            this.lblExecute.TabIndex = 6;
            this.lblExecute.Text = "timeoffset";
            this.cmbMin.DisplayMember = "30";
            this.cmbMin.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMin.FormattingEnabled = true;
            this.cmbMin.Items.AddRange(new object[] { "0", "5", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55" });
            this.cmbMin.Location = new Point(0x86, 0x36);
            this.cmbMin.Name = "cmbMin";
            this.cmbMin.Size = new Size(40, 0x15);
            this.cmbMin.TabIndex = 5;
            this.cmbMin.ValueMember = "30";
            this.cmbMin.SelectedIndexChanged += new EventHandler(this.cmbMin_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x7e, 0x39);
            this.label2.Name = "label2";
            this.label2.Size = new Size(10, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = ":";
            this.cmbHours.DisplayMember = "16";
            this.cmbHours.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbHours.FormattingEnabled = true;
            this.cmbHours.Items.AddRange(new object[] { 
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", 
                "16", "17", "18", "19", "20", "21", "22", "23"
             });
            this.cmbHours.Location = new Point(0x52, 0x36);
            this.cmbHours.Name = "cmbHours";
            this.cmbHours.Size = new Size(40, 0x15);
            this.cmbHours.TabIndex = 3;
            this.cmbHours.ValueMember = "16";
            this.cmbHours.SelectedIndexChanged += new EventHandler(this.cmbHours_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(3, 0x38);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x31, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Execute:";
            this.lblMarketClose.AutoSize = true;
            this.lblMarketClose.Location = new Point(0x4f, 20);
            this.lblMarketClose.Name = "lblMarketClose";
            this.lblMarketClose.Size = new Size(0x5f, 13);
            this.lblMarketClose.TabIndex = 1;
            this.lblMarketClose.Text = "Market Close Time";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(3, 20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Market Close:";
            this.accountTypeSelector1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.accountTypeSelector1.FormattingEnabled = true;
            this.accountTypeSelector1.IgnoreCalls = false;
            this.accountTypeSelector1.Location = new Point(100, 0x73);
            this.accountTypeSelector1.Name = "accountTypeSelector1";
            this.accountTypeSelector1.Size = new Size(0x8a, 0x15);
            this.accountTypeSelector1.TabIndex = 0x12;
            this.scale.BackColor = Color.Cornsilk;
            this.scale.Location = new Point(0x65, 0x59);
            this.scale.Name = "scale";
            this.scale.Size = new Size(0x89, 20);
            this.scale.SM = false;
            this.scale.TabIndex = 7;
            this.scale.Load += new EventHandler(this.scale_Load);
            this.scale.ScaleChanged += new EventHandler<EventArgs>(this.method_2);
            this.posSize.BackColor = Color.Honeydew;
            this.posSize.Location = new Point(100, 0x3f);
            this.posSize.Name = "posSize";
            this.posSize.Size = new Size(0x8a, 20);
            this.posSize.TabIndex = 5;
            this.posSize.PositionSizeChanged += new EventHandler<EventArgs>(this.method_0);
            this.dataRange.BackColor = Color.AliceBlue;
            this.dataRange.IsStreaming = true;
            this.dataRange.Location = new Point(0x66, 0x25);
            this.dataRange.Name = "dataRange";
            this.dataRange.Size = new Size(0x88, 20);
            this.dataRange.TabIndex = 3;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x232, 0x1e5);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.accountTypeSelector1);
            base.Controls.Add(this.lblTradeType);
            base.Controls.Add(this.cbEmailOrders);
            base.Controls.Add(this.cbPV);
            base.Controls.Add(this.cmbAccounts);
            base.Controls.Add(this.lblAccount);
            base.Controls.Add(this.scale);
            base.Controls.Add(this.paramSliders);
            base.Controls.Add(this.lblParameters);
            base.Controls.Add(this.cbOrders);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.tree);
            base.Controls.Add(this.lblSource);
            base.Controls.Add(this.lblScale);
            base.Controls.Add(this.posSize);
            base.Controls.Add(this.lblPosSize);
            base.Controls.Add(this.dataRange);
            base.Controls.Add(this.lblData);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "StrategySettingsForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Strategy Activation Settings";
            base.Load += new EventHandler(this.StrategySettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(object sender, EventArgs e)
        {
            if (!this.posSize.PositionSize.RawProfitMode)
            {
                StrategyCenterForm.ShowPosSizeWarning();
            }
        }

        private void method_1()
        {
            if (this.cbPV.Checked)
            {
                this.paramSliders.Enabled = false;
                if (this.tree.Symbol == "")
                {
                    this.paramSliders.Visible = false;
                }
                else
                {
                    this.paramSliders.Visible = true;
                    this.Item.Strategy.LoadPreferredValues(this.tree.Symbol, this.Item.WealthScript);
                    this.paramSliders.WealthScript = this.Item.WealthScript;
                }
            }
            else
            {
                this.paramSliders.Visible = true;
                this.paramSliders.Enabled = true;
                this.paramSliders.WealthScript = this.Item.WealthScript;
            }
        }

        private void method_2(object sender, EventArgs e)
        {
            this.groupBox1.Enabled = !this.scale.DataScale.IsIntraday;
        }

        private void method_3()
        {
            if ((this.cmbHours.SelectedItem != null) && (this.cmbMin.SelectedItem != null))
            {
                try
                {
                    DateTime localTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, this.ExecuteHours, this.ExecuteMin, 0);
                    this.lblLocalTime.Text = TimeZoneInformation.ToLocalTime(this.Item.MarketInfo.TimeZoneName, localTime, TimeZoneInformation.CurrentTimeZone.Name).ToString("HH:mm");
                }
                catch
                {
                    this.lblLocalTime.Text = "";
                }
            }
        }

        private void scale_Load(object sender, EventArgs e)
        {
            this.scale.SM = true;
        }

        private void StrategySettingsForm_Load(object sender, EventArgs e)
        {
            if (this.tree.Nodes.Count == 0)
            {
                this.tree.Populate(MainModule.Instance.DataSources, false);
            }
            this.accountTypeSelector1.IgnoreCalls = true;
            if (MainModule.Instance.BrokerProvider != null)
            {
                foreach (string accountNumber in MainModule.Instance.AccountNumbers)
                {
                    this.cmbAccounts.Items.Add(accountNumber);
                }
            }
            this.cmbAccounts.SelectedIndex = this.cmbAccounts.Items.IndexOf(MainModule.Instance.DefaultAccountNumber);
            this.cmbAccounts.SelectedIndex = -1;
            IEnumerator enumerator = this.cmbAccounts.Items.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        string current = (string)enumerator.Current;
                        if (current == this.strategyCenterItem.AccountNumber)
                        {
                            this.cmbAccounts.SelectedItem = current;
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
            this.accountTypeSelector1.IgnoreCalls = false;
            this.accountTypeSelector1.InitAccountTradeType(this.cmbAccounts.Text, "");
            this.accountTypeSelector1.SelectAccountTradeType(this.strategyCenterItem.AccountTradeType, true);
            DateTime closeTimeNative = this.Item.MarketInfo.CloseTimeNative;
            this.lblMarketClose.Text = string.Concat(closeTimeNative.ToString("HH:mm"), " GMT ", TimeZoneInformation.GetTimeZone(this.Item.MarketInfo.TimeZoneName).StandardOffset);
            this.lblExecute.Text = string.Concat("GMT ", TimeZoneInformation.GetTimeZone(this.Item.MarketInfo.TimeZoneName).StandardOffset);
            this.lblExecuteLocal.Text = string.Concat("GMT ", TimeZoneInformation.CurrentTimeZone.StandardOffset);
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.btnOK.Enabled = (this.tree.SelectedNode != null) && (this.cmbAccounts.SelectedItem != null);
            this.method_1();
            string symbol = this.Symbol;
            if (((symbol == null) || (symbol == "")) && ((this.DataSet != null) && (this.DataSet.Symbols.Count > 0)))
            {
                symbol = this.DataSet.Symbols[0];
            }
            if (((symbol != null) && (symbol != "")) && (this.DataSet != null))
            {
                this.Item.MarketInfo = this.DataSet.Provider.GetMarketInfo(symbol);
            }
            this.lblMarketClose.Text = this.Item.MarketInfo.CloseTimeNative.ToShortTimeString() + " GMT " + TimeZoneInformation.GetTimeZone(this.Item.MarketInfo.TimeZoneName).StandardOffset;
            this.lblExecute.Text = "GMT " + TimeZoneInformation.GetTimeZone(this.Item.MarketInfo.TimeZoneName).StandardOffset;
            this.method_3();
        }

        public string AccountNumber
        {
            get
            {
                return (string) this.cmbAccounts.SelectedItem;
            }
        }

        public string AccountTradeType
        {
            get
            {
                return (string) this.accountTypeSelector1.SelectedItem;
            }
        }

        public bool AutoStage
        {
            get
            {
                return this.cbOrders.Checked;
            }
        }

        public BarDataRange DataRange
        {
            get
            {
                return this.dataRange.DataRange;
            }
        }

        public BarDataScale DataScale
        {
            get
            {
                return this.scale.DataScale;
            }
        }

        public DataSource DataSet
        {
            get
            {
                return this.tree.DataSource;
            }
        }

        public bool EmailAlerts
        {
            get
            {
                return this.cbEmailOrders.Checked;
            }
        }

        public int ExecuteHours
        {
            get
            {
                return int.Parse(this.cmbHours.SelectedItem.ToString());
            }
        }

        public int ExecuteMin
        {
            get
            {
                return int.Parse(this.cmbMin.SelectedItem.ToString());
            }
        }

        public StrategyCenterItem Item
        {
            get
            {
                return this.strategyCenterItem;
            }
            set
            {
                this.strategyCenterItem = value;
                if (this.tree.Nodes.Count == 0)
                {
                    this.tree.Populate(MainModule.Instance.DataSources, false);
                }
                this.dataRange.DataRange = this.strategyCenterItem.DataRange;
                this.posSize.PositionSize = this.strategyCenterItem.PositionSize;
                this.scale.DataScale = new BarDataScale(this.strategyCenterItem.Scale, this.strategyCenterItem.BarInterval);
                if (this.strategyCenterItem.Symbol == "")
                {
                    this.tree.SelectDataSource(this.strategyCenterItem.DataSet);
                }
                else
                {
                    this.tree.SelectSymbol(this.strategyCenterItem.DataSet, this.strategyCenterItem.Symbol);
                }
                this.cbOrders.Checked = this.strategyCenterItem.AutoStage;
                this.cbPV.Checked = this.strategyCenterItem.UsePreferredValues;
                this.cbEmailOrders.Checked = this.strategyCenterItem.EmailAlerts;
                this.paramSliders.WealthScript = this.strategyCenterItem.WealthScript;
                this.method_1();
                this.accountTypeSelector1.SelectAccountTradeType(this.strategyCenterItem.AccountTradeType, true);
                this.groupBox1.Enabled = !this.scale.DataScale.IsIntraday;
                this.cmbHours.SelectedItem = this.strategyCenterItem.ExecuteHours.ToString();
                this.cmbMin.SelectedItem = this.strategyCenterItem.ExecuteMin.ToString();
                this.method_3();
            }
        }

        public WealthLab.PositionSize PositionSize
        {
            get
            {
                return this.posSize.PositionSize;
            }
        }

        public string Symbol
        {
            get
            {
                return this.tree.Symbol;
            }
        }

        public bool UsePreferredValues
        {
            get
            {
                return this.cbPV.Checked;
            }
        }
    }
}

