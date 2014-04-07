namespace WealthLabPro
{
    using CtrlLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class EditAlertForm : Form
    {
        private AccountTypeSelector accountTypeSelector1;
        private WealthLab.Alert alert_0;
        private bool bool_0;
        private bool bool_1;
        private Button btnCancel;
        private Button btnOK;
        private ComboBox cmbAccount;
        private ComboBox cmbAction;
        private ComboBox cmbOrderType;
        private IContainer components;
        private Label lblAccount;
        private Label lblAction;
        private Label lblOrderType;
        private Label lblPrice;
        private Label lblQty;
        private Label lblSymbol;
        private Label lblTradeType;
        private NumEdit numPrice;
        private NumEdit numQty;
        private TextBox txtSymbol;

        public EditAlertForm()
        {
            this.InitializeComponent();
        }

        public EditAlertForm(bool paperAcct)
        {
            this.InitializeComponent();
            this.bool_0 = paperAcct;
            this.bool_1 = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.alert_0.Account = this.cmbAccount.Text;
            this.alert_0.Symbol = this.txtSymbol.Text;
            this.alert_0.AlertType = (TradeType) this.cmbAction.SelectedIndex;
            this.alert_0.Shares = (double) this.numQty.Value;
            this.alert_0.OrderType = (OrderType) this.cmbOrderType.SelectedIndex;
            this.alert_0.Price = (double) this.numPrice.Value;
            this.alert_0.AccountTradeType = (string) this.accountTypeSelector1.SelectedItem;
            base.DialogResult = DialogResult.OK;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EditAlertForm_Load(object sender, EventArgs e)
        {
            this.accountTypeSelector1.IgnoreCalls = true;
            foreach (string str in MainModule.Instance.AccountNumbers)
            {
                if (this.bool_1)
                {
                    if (str.StartsWith("Paper"))
                    {
                        if (this.bool_0)
                        {
                            this.cmbAccount.Items.Add(str);
                        }
                    }
                    else if (!this.bool_0)
                    {
                        this.cmbAccount.Items.Add(str);
                    }
                }
                else
                {
                    this.cmbAccount.Items.Add(str);
                }
            }
            OrderType[] values = (OrderType[]) Enum.GetValues(typeof(OrderType));
            foreach (OrderType type in values)
            {
                if (MainModule.Instance.BrokerProvider.AllowOrderTypeForRoute("Default", type))
                {
                    this.cmbOrderType.Items.Add(type.ToString());
                }
            }
            if (this.alert_0 != null)
            {
                this.cmbAccount.Text = this.alert_0.Account;
                this.txtSymbol.Text = this.alert_0.Symbol;
                this.cmbAction.Text = this.alert_0.AlertType.ToString();
                this.numQty.Text = this.alert_0.Shares.ToString();
                this.cmbOrderType.Text = this.alert_0.OrderType.ToString();
                this.numPrice.Text = this.alert_0.Price.ToString("N" + DecimalsManager.Instance.GetPricingDecimalForSymbol(this.alert_0.Symbol));
            }
            this.accountTypeSelector1.IgnoreCalls = false;
            this.accountTypeSelector1.InitAccountTradeType(this.cmbAccount.Text, this.cmbAction.Text);
            this.accountTypeSelector1.SelectAccountTradeType(this.alert_0.AccountTradeType, true);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.lblAccount = new Label();
            this.cmbAccount = new ComboBox();
            this.lblSymbol = new Label();
            this.txtSymbol = new TextBox();
            this.lblAction = new Label();
            this.cmbAction = new ComboBox();
            this.numQty = new NumEdit();
            this.lblQty = new Label();
            this.cmbOrderType = new ComboBox();
            this.lblOrderType = new Label();
            this.numPrice = new NumEdit();
            this.lblPrice = new Label();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.lblTradeType = new Label();
            this.accountTypeSelector1 = new AccountTypeSelector(this.components);
            base.SuspendLayout();
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new Point(13, 13);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new Size(50, 13);
            this.lblAccount.TabIndex = 0;
            this.lblAccount.Text = "Account:";
            this.cmbAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAccount.FormattingEnabled = true;
            this.cmbAccount.Location = new Point(0x57, 12);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new Size(0xa8, 0x15);
            this.cmbAccount.TabIndex = 1;
            this.cmbAccount.SelectedIndexChanged += new EventHandler(this.numPrice_TextChanged);
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.Location = new Point(13, 0x2c);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new Size(0x2c, 13);
            this.lblSymbol.TabIndex = 2;
            this.lblSymbol.Text = "Symbol:";
            this.txtSymbol.CharacterCasing = CharacterCasing.Upper;
            this.txtSymbol.Location = new Point(0x57, 0x29);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new Size(100, 20);
            this.txtSymbol.TabIndex = 3;
            this.txtSymbol.TextChanged += new EventHandler(this.numPrice_TextChanged);
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new Point(13, 0x4b);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new Size(40, 13);
            this.lblAction.TabIndex = 4;
            this.lblAction.Text = "Action:";
            this.cmbAction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAction.FormattingEnabled = true;
            this.cmbAction.Items.AddRange(new object[] { "Buy", "Sell", "Short", "Cover" });
            this.cmbAction.Location = new Point(0x57, 0x48);
            this.cmbAction.Name = "cmbAction";
            this.cmbAction.Size = new Size(100, 0x15);
            this.cmbAction.TabIndex = 5;
            this.cmbAction.SelectedIndexChanged += new EventHandler(this.numPrice_TextChanged);
            this.numQty.InputType = NumEdit.NumEditType.Integer;
            this.numQty.Location = new Point(0x57, 0x63);
            this.numQty.Name = "numQty";
            this.numQty.Size = new Size(100, 20);
            this.numQty.TabIndex = 6;
            this.numQty.TextChanged += new EventHandler(this.numPrice_TextChanged);
            this.lblQty.AutoSize = true;
            this.lblQty.Location = new Point(13, 0x67);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new Size(0x31, 13);
            this.lblQty.TabIndex = 7;
            this.lblQty.Text = "Quantity:";
            this.cmbOrderType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbOrderType.FormattingEnabled = true;
            this.cmbOrderType.Location = new Point(0x57, 0x7f);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.Size = new Size(100, 0x15);
            this.cmbOrderType.TabIndex = 8;
            this.cmbOrderType.SelectedIndexChanged += new EventHandler(this.numPrice_TextChanged);
            this.lblOrderType.AutoSize = true;
            this.lblOrderType.Location = new Point(13, 130);
            this.lblOrderType.Name = "lblOrderType";
            this.lblOrderType.Size = new Size(0x3f, 13);
            this.lblOrderType.TabIndex = 9;
            this.lblOrderType.Text = "Order Type:";
            this.numPrice.InputType = NumEdit.NumEditType.Double;
            this.numPrice.Location = new Point(0x57, 0x9b);
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new Size(100, 20);
            this.numPrice.TabIndex = 10;
            this.numPrice.TextChanged += new EventHandler(this.numPrice_TextChanged);
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new Point(13, 0x9e);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new Size(0x22, 13);
            this.lblPrice.TabIndex = 11;
            this.lblPrice.Text = "Price:";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xb9, 210);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Location = new Point(0x68, 210);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.lblTradeType.AutoSize = true;
            this.lblTradeType.Location = new Point(13, 0xb9);
            this.lblTradeType.Name = "lblTradeType";
            this.lblTradeType.Size = new Size(0x3e, 13);
            this.lblTradeType.TabIndex = 15;
            this.lblTradeType.Text = "Trade Type";
            this.accountTypeSelector1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.accountTypeSelector1.FormattingEnabled = true;
            this.accountTypeSelector1.IgnoreCalls = false;
            this.accountTypeSelector1.Location = new Point(0x57, 0xb6);
            this.accountTypeSelector1.Name = "accountTypeSelector1";
            this.accountTypeSelector1.Size = new Size(100, 0x15);
            this.accountTypeSelector1.TabIndex = 0x10;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x110, 0xf6);
            base.Controls.Add(this.accountTypeSelector1);
            base.Controls.Add(this.lblTradeType);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.lblPrice);
            base.Controls.Add(this.numPrice);
            base.Controls.Add(this.lblOrderType);
            base.Controls.Add(this.cmbOrderType);
            base.Controls.Add(this.lblQty);
            base.Controls.Add(this.numQty);
            base.Controls.Add(this.cmbAction);
            base.Controls.Add(this.lblAction);
            base.Controls.Add(this.txtSymbol);
            base.Controls.Add(this.lblSymbol);
            base.Controls.Add(this.cmbAccount);
            base.Controls.Add(this.lblAccount);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "EditAlertForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit Alert";
            base.Load += new EventHandler(this.EditAlertForm_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void numPrice_TextChanged(object sender, EventArgs e)
        {
            this.btnOK.Enabled = ((this.txtSymbol.Text != "") && (this.numPrice.Text != "")) && (this.numQty.Text != "");
            this.numPrice.Enabled = this.cmbOrderType.Text != "Market";
            if ((sender == this.cmbAccount) || (sender == this.cmbAction))
            {
                this.accountTypeSelector1.InitAccountTradeType(this.cmbAccount.Text, this.cmbAction.Text);
            }
        }

        public WealthLab.Alert Alert
        {
            get
            {
                return this.alert_0;
            }
            set
            {
                this.alert_0 = value;
            }
        }
    }
}

