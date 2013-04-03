namespace WealthLabPro
{
    using CtrlLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class AddPriceTriggerForm : Form
    {
        private AccountTypeSelector accountTypeSelector1;
        private bool bool_0;
        private Button btnCancel;
        private Button btnOK;
        private ComboBox cmbAccount;
        private ComboBox cmbTradeAction;
        private ComboBox cmbTradeOrder;
        private IContainer icontainer_0;
        private Label lblAccount;
        private Label lblAction;
        private Label lblOrder;
        private Label lblPriceTrigger;
        private Label lblQuantity;
        private Label lblSymbol;
        private Label lblTradeType;
        private Label lblTrigger;
        private NumEdit numQuantity;
        private NumEdit numTradePrice;
        private TextBox txtSymbol;

        public AddPriceTriggerForm()
        {
            this.InitializeComponent();
        }

        public AddPriceTriggerForm(bool paperAcct)
        {
            this.InitializeComponent();
            this.bool_0 = paperAcct;
        }

        private void AddPriceTriggerForm_Activated(object sender, EventArgs e)
        {
            this.txtSymbol.Focus();
        }

        private void AddPriceTriggerForm_Load(object sender, EventArgs e)
        {
            this.accountTypeSelector1.IgnoreCalls = true;
            if (MainModule.Instance.BrokerProvider != null)
            {
                foreach (string str in MainModule.Instance.AccountNumbers)
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
            }
            if (MainModule.Instance.DefaultAccountNumber != "")
            {
                this.cmbAccount.SelectedIndex = this.cmbAccount.Items.IndexOf(MainModule.Instance.DefaultAccountNumber);
            }
            else if (this.cmbAccount.Items.Count > 0)
            {
                this.cmbAccount.SelectedIndex = 0;
            }
            this.cmbTradeAction.SelectedIndex = 0;
            this.cmbTradeOrder.SelectedIndex = 0;
            this.accountTypeSelector1.IgnoreCalls = false;
            this.accountTypeSelector1.InitAccountTradeType(this.cmbAccount.Text, this.cmbTradeAction.Text);
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
            this.lblPriceTrigger = new Label();
            this.cmbTradeOrder = new ComboBox();
            this.cmbTradeAction = new ComboBox();
            this.cmbAccount = new ComboBox();
            this.lblAccount = new Label();
            this.lblSymbol = new Label();
            this.txtSymbol = new TextBox();
            this.lblAction = new Label();
            this.lblQuantity = new Label();
            this.lblOrder = new Label();
            this.lblTrigger = new Label();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.numTradePrice = new NumEdit();
            this.numQuantity = new NumEdit();
            this.lblTradeType = new Label();
            this.accountTypeSelector1 = new AccountTypeSelector(this.icontainer_0);
            base.SuspendLayout();
            this.lblPriceTrigger.Location = new Point(13, 13);
            this.lblPriceTrigger.Name = "lblPriceTrigger";
            this.lblPriceTrigger.Size = new Size(0x1d9, 0x13);
            this.lblPriceTrigger.TabIndex = 0;
            this.lblPriceTrigger.Text = "The Quotes Window will Trigger an Alert when the price of the Symbol reaches your Price Trigger.";
            this.cmbTradeOrder.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTradeOrder.FormattingEnabled = true;
            this.cmbTradeOrder.Items.AddRange(new object[] { "Limit", "Stop" });
            this.cmbTradeOrder.Location = new Point(340, 0x3d);
            this.cmbTradeOrder.Name = "cmbTradeOrder";
            this.cmbTradeOrder.Size = new Size(0x44, 0x15);
            this.cmbTradeOrder.TabIndex = 11;
            this.cmbTradeOrder.SelectedIndexChanged += new EventHandler(this.numQuantity_TextChanged);
            this.cmbTradeAction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTradeAction.FormattingEnabled = true;
            this.cmbTradeAction.Items.AddRange(new object[] { "Buy", "Sell", "Short", "Cover" });
            this.cmbTradeAction.Location = new Point(200, 0x3d);
            this.cmbTradeAction.Name = "cmbTradeAction";
            this.cmbTradeAction.Size = new Size(0x44, 0x15);
            this.cmbTradeAction.TabIndex = 9;
            this.cmbTradeAction.SelectedIndexChanged += new EventHandler(this.numQuantity_TextChanged);
            this.cmbAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAccount.FormattingEnabled = true;
            this.cmbAccount.Location = new Point(13, 0x3d);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new Size(0x68, 0x15);
            this.cmbAccount.TabIndex = 7;
            this.cmbAccount.SelectedIndexChanged += new EventHandler(this.numQuantity_TextChanged);
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new Point(13, 0x29);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new Size(0x2f, 13);
            this.lblAccount.TabIndex = 1;
            this.lblAccount.Text = "Account";
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.Location = new Point(120, 0x2a);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new Size(0x29, 13);
            this.lblSymbol.TabIndex = 2;
            this.lblSymbol.Text = "Symbol";
            this.txtSymbol.CharacterCasing = CharacterCasing.Upper;
            this.txtSymbol.Location = new Point(0x7b, 0x3e);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new Size(0x47, 20);
            this.txtSymbol.TabIndex = 8;
            this.txtSymbol.TextChanged += new EventHandler(this.numQuantity_TextChanged);
            this.txtSymbol.Enter += new EventHandler(this.txtSymbol_Enter);
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new Point(200, 0x29);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new Size(0x25, 13);
            this.lblAction.TabIndex = 3;
            this.lblAction.Text = "Action";
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new Point(0x113, 0x2a);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new Size(0x2e, 13);
            this.lblQuantity.TabIndex = 4;
            this.lblQuantity.Text = "Quantity";
            this.lblOrder.AutoSize = true;
            this.lblOrder.Location = new Point(0x155, 0x2a);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new Size(60, 13);
            this.lblOrder.TabIndex = 5;
            this.lblOrder.Text = "Order Type";
            this.lblTrigger.AutoSize = true;
            this.lblTrigger.Location = new Point(0x19b, 0x2a);
            this.lblTrigger.Name = "lblTrigger";
            this.lblTrigger.Size = new Size(0x43, 13);
            this.lblTrigger.TabIndex = 6;
            this.lblTrigger.Text = "Trigger Price";
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x1ef, 0x68);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new Point(0x19e, 0x68);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.numTradePrice.InputType = NumEdit.NumEditType.Double;
            this.numTradePrice.Location = new Point(0x19e, 0x3d);
            this.numTradePrice.Name = "numTradePrice";
            this.numTradePrice.Size = new Size(0x4b, 20);
            this.numTradePrice.TabIndex = 12;
            this.numTradePrice.Text = "10.0";
            this.numTradePrice.TextChanged += new EventHandler(this.numQuantity_TextChanged);
            this.numTradePrice.Enter += new EventHandler(this.numQuantity_Enter);
            this.numQuantity.InputType = NumEdit.NumEditType.Integer;
            this.numQuantity.Location = new Point(0x113, 0x3e);
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new Size(0x3b, 20);
            this.numQuantity.TabIndex = 10;
            this.numQuantity.Text = "100";
            this.numQuantity.TextChanged += new EventHandler(this.numQuantity_TextChanged);
            this.numQuantity.Enter += new EventHandler(this.numQuantity_Enter);
            this.lblTradeType.AutoSize = true;
            this.lblTradeType.Location = new Point(0x1f0, 0x29);
            this.lblTradeType.Name = "lblTradeType";
            this.lblTradeType.Size = new Size(0x3e, 13);
            this.lblTradeType.TabIndex = 0x10;
            this.lblTradeType.Text = "Trade Type";
            this.accountTypeSelector1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.accountTypeSelector1.FormattingEnabled = true;
            this.accountTypeSelector1.IgnoreCalls = false;
            this.accountTypeSelector1.Location = new Point(0x1f3, 0x3d);
            this.accountTypeSelector1.Name = "accountTypeSelector1";
            this.accountTypeSelector1.Size = new Size(0x47, 0x15);
            this.accountTypeSelector1.TabIndex = 0x11;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x246, 0x8b);
            base.Controls.Add(this.accountTypeSelector1);
            base.Controls.Add(this.lblTradeType);
            base.Controls.Add(this.numQuantity);
            base.Controls.Add(this.numTradePrice);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.lblTrigger);
            base.Controls.Add(this.lblOrder);
            base.Controls.Add(this.lblQuantity);
            base.Controls.Add(this.lblAction);
            base.Controls.Add(this.txtSymbol);
            base.Controls.Add(this.lblSymbol);
            base.Controls.Add(this.lblAccount);
            base.Controls.Add(this.cmbTradeOrder);
            base.Controls.Add(this.cmbTradeAction);
            base.Controls.Add(this.cmbAccount);
            base.Controls.Add(this.lblPriceTrigger);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AddPriceTriggerForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Add a Price Trigger";
            base.Load += new EventHandler(this.AddPriceTriggerForm_Load);
            base.Activated += new EventHandler(this.AddPriceTriggerForm_Activated);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(object sender, KeyEventArgs e)
        {
            this.method_1();
        }

        private void method_1()
        {
            this.btnOK.Enabled = ((((this.txtSymbol.Text != "") && (this.numQuantity.Value > 0M)) && ((this.numTradePrice.Value > 0M) && (this.cmbAccount.SelectedIndex >= 0))) && (this.cmbTradeAction.SelectedIndex >= 0)) && (this.cmbTradeOrder.SelectedIndex >= 0);
        }

        private void numQuantity_Enter(object sender, EventArgs e)
        {
            ((NumEdit) sender).SelectAll();
        }

        private void numQuantity_TextChanged(object sender, EventArgs e)
        {
            if ((sender == this.cmbAccount) || (sender == this.cmbTradeAction))
            {
                this.accountTypeSelector1.InitAccountTradeType(this.cmbAccount.Text, this.cmbTradeAction.Text);
            }
            this.method_1();
        }

        private void txtSymbol_Enter(object sender, EventArgs e)
        {
            this.txtSymbol.SelectAll();
        }

        public WealthLab.Alert Alert
        {
            get
            {
                WealthLab.Alert alert = new WealthLab.Alert {
                    AlertDate = DateTime.Now,
                    Account = this.cmbAccount.Text,
                    AlertType = (TradeType) Enum.Parse(typeof(TradeType), this.cmbTradeAction.Text),
                    OrderType = (OrderType) Enum.Parse(typeof(OrderType), this.cmbTradeOrder.Text),
                    Price = double.Parse(this.numTradePrice.Text),
                    Shares = double.Parse(this.numQuantity.Text),
                    Symbol = this.txtSymbol.Text
                };
                if (MainModule.Instance.BrokerProvider != null)
                {
                    alert.AlertDate = MainModule.Instance.BrokerProvider.ConvertToMarketTimeZone(alert.Symbol, alert.AlertDate);
                }
                alert.AccountTradeType = this.accountTypeSelector1.Text;
                return alert;
            }
        }
    }
}

