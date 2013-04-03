namespace WealthLabPro
{
    using CtrlLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class EditOrderForm : Form
    {
        private AccountTypeSelector accountTypeSelector;
        private Button btnCancel;
        private Button btnOK;
        private ComboBox cmbAccount;
        private ComboBox cmbAction;
        private ComboBox cmbOrderType;
        private ComboBox cmbRoute;
        private ComboBox cmbTIF;
        private IContainer icontainer_0;
        private int int_0;
        private Label lblAccount;
        private Label lblAction;
        private Label lblOrderType;
        private Label lblPrice;
        private Label lblQty;
        private Label lblRoute;
        private Label lblSymbol;
        private Label lblTif;
        private Label lblTradeType;
        private NumEdit numPrice;
        private NumEdit numQty;
        private Order order_0;
        private TextBox txtSymbol;

        public EditOrderForm(Order order)
        {
            this.InitializeComponent();
            this.order_0 = order;
            this.int_0 = DecimalsManager.Instance.GetPricingDecimalForSymbol(this.order_0.Symbol);
            this.accountTypeSelector.IgnoreCalls = true;
            foreach (string str in MainModule.Instance.AccountNumbers)
            {
                this.cmbAccount.Items.Add(str);
            }
            this.cmbAccount.SelectedIndex = this.cmbAccount.Items.IndexOf(order.Account);
            this.txtSymbol.Text = order.Symbol;
            TradeType[] values = (TradeType[]) Enum.GetValues(typeof(TradeType));
            foreach (TradeType type in values)
            {
                this.cmbAction.Items.Add(type);
            }
            this.cmbAction.SelectedItem = order.AlertType;
            this.numQty.Text = order.Shares.ToString();
            OrderType[] typeArray2 = (OrderType[]) Enum.GetValues(typeof(OrderType));
            foreach (OrderType type2 in typeArray2)
            {
                if (MainModule.Instance.BrokerProvider.AllowOrderTypeForRoute(order.Route, type2))
                {
                    this.cmbOrderType.Items.Add(type2.ToString());
                }
            }
            foreach (string str2 in MainModule.Instance.BrokerProvider.ExtendedOrderTypesAllowed(order.Account, order.Route, order.AlertType.ToString()))
            {
                this.cmbOrderType.Items.Add(str2);
            }
            this.cmbOrderType.SelectedItem = order.OrderTypeString;
            this.numPrice.Text = order.Price.ToString("N" + this.int_0);
            foreach (string str3 in MainModule.Instance.BrokerProvider.Routes)
            {
                this.cmbRoute.Items.Add(str3);
            }
            this.cmbRoute.SelectedIndex = this.cmbRoute.Items.IndexOf(order.Route);
            this.cmbTIF.SelectedIndex = this.cmbTIF.Items.IndexOf(order.TIF);
            this.accountTypeSelector.IgnoreCalls = false;
            this.accountTypeSelector.InitAccountTradeType(this.cmbAccount.Text, this.cmbAction.Text);
            this.accountTypeSelector.SelectAccountTradeType(order.AccountTradeType, true);
            this.method_0();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.order_0.Account = this.cmbAccount.Text;
            this.order_0.Symbol = this.txtSymbol.Text;
            this.order_0.AlertType = (TradeType) this.cmbAction.SelectedItem;
            this.order_0.Shares = (double) this.numQty.Value;
            this.order_0.ExtendedOrderType = "";
            if (this.cmbOrderType.Text == "Market")
            {
                this.order_0.OrderType = OrderType.Market;
            }
            else if (this.cmbOrderType.Text == "Limit")
            {
                this.order_0.OrderType = OrderType.Limit;
            }
            else if (this.cmbOrderType.Text == "Stop")
            {
                this.order_0.OrderType = OrderType.Stop;
            }
            else
            {
                this.order_0.OrderType = OrderType.Market;
                this.order_0.ExtendedOrderType = this.cmbOrderType.Text;
            }
            this.order_0.Price = (double) this.numPrice.Value;
            this.order_0.Route = this.cmbRoute.Text;
            this.order_0.TIF = this.cmbTIF.Text;
            this.order_0.AccountTradeType = this.accountTypeSelector.Text;
            base.DialogResult = DialogResult.OK;
        }

        private void cmbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.cmbOrderType.Text;
            this.cmbOrderType.Items.Clear();
            OrderType[] values = (OrderType[]) Enum.GetValues(typeof(OrderType));
            foreach (OrderType type in values)
            {
                if (MainModule.Instance.BrokerProvider.AllowOrderTypeForRoute(this.cmbRoute.Text, type))
                {
                    this.cmbOrderType.Items.Add(type.ToString());
                }
            }
            string action = this.cmbAction.Text;
            foreach (string str3 in MainModule.Instance.BrokerProvider.ExtendedOrderTypesAllowed(this.cmbAccount.Text, this.cmbRoute.Text, action))
            {
                this.cmbOrderType.Items.Add(str3);
            }
            this.cmbOrderType.SelectedIndex = this.cmbOrderType.Items.IndexOf(text);
            this.accountTypeSelector.InitAccountTradeType(this.cmbAccount.Text, this.cmbAction.Text);
            this.method_0();
        }

        private void cmbOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.cmbTIF.Text;
            this.cmbTIF.Items.Clear();
            foreach (string str2 in MainModule.Instance.BrokerProvider.TifsAllowed(this.cmbAccount.Text, this.cmbRoute.Text, this.cmbOrderType.Text))
            {
                this.cmbTIF.Items.Add(str2);
            }
            this.cmbTIF.SelectedIndex = this.cmbTIF.Items.IndexOf(text);
            if (MainModule.Instance.BrokerProvider.AllowDecimalsForExtendedOrderType(this.cmbOrderType.Text))
            {
                this.numPrice.InputType = NumEdit.NumEditType.Double;
            }
            else
            {
                this.numPrice.InputType = NumEdit.NumEditType.Integer;
                if (this.numPrice.Value != ((int) this.numPrice.Value))
                {
                    this.numPrice.Text = ((int) this.numPrice.Value).ToString("N" + this.int_0);
                }
            }
            this.method_0();
        }

        private void cmbRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.cmbTIF.Text;
            this.cmbTIF.Items.Clear();
            foreach (string str3 in MainModule.Instance.BrokerProvider.TifsAllowed(this.cmbAccount.Text, this.cmbRoute.Text, this.cmbOrderType.Text))
            {
                this.cmbTIF.Items.Add(str3);
            }
            this.cmbTIF.SelectedIndex = this.cmbTIF.Items.IndexOf(text);
            string str5 = this.cmbOrderType.Text;
            this.cmbOrderType.Items.Clear();
            OrderType[] values = (OrderType[]) Enum.GetValues(typeof(OrderType));
            foreach (OrderType type in values)
            {
                if (MainModule.Instance.BrokerProvider.AllowOrderTypeForRoute(this.cmbRoute.Text, type))
                {
                    this.cmbOrderType.Items.Add(type.ToString());
                }
            }
            string action = this.cmbAction.Text;
            foreach (string str2 in MainModule.Instance.BrokerProvider.ExtendedOrderTypesAllowed(this.cmbAccount.Text, this.cmbRoute.Text, action))
            {
                this.cmbOrderType.Items.Add(str2);
            }
            this.cmbOrderType.SelectedIndex = this.cmbOrderType.Items.IndexOf(str5);
            this.method_0();
        }

        private void cmbTIF_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.accountTypeSelector.InitAccountTradeType(this.cmbAccount.Text, this.cmbAction.Text);
            this.method_0();
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
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.lblPrice = new Label();
            this.numPrice = new NumEdit();
            this.lblOrderType = new Label();
            this.cmbOrderType = new ComboBox();
            this.lblQty = new Label();
            this.numQty = new NumEdit();
            this.cmbAction = new ComboBox();
            this.lblAction = new Label();
            this.txtSymbol = new TextBox();
            this.lblSymbol = new Label();
            this.cmbAccount = new ComboBox();
            this.lblAccount = new Label();
            this.lblRoute = new Label();
            this.cmbRoute = new ComboBox();
            this.lblTif = new Label();
            this.cmbTIF = new ComboBox();
            this.lblTradeType = new Label();
            this.accountTypeSelector = new AccountTypeSelector(this.icontainer_0);
            base.SuspendLayout();
            this.btnOK.Enabled = false;
            this.btnOK.Location = new Point(0x60, 280);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 0x1b;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xb1, 280);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0x1a;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new Point(12, 0x9b);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new Size(0x22, 13);
            this.lblPrice.TabIndex = 0x19;
            this.lblPrice.Text = "Price:";
            this.numPrice.InputType = NumEdit.NumEditType.Double;
            this.numPrice.Location = new Point(0x54, 150);
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new Size(100, 20);
            this.numPrice.TabIndex = 0x18;
            this.numPrice.TextChanged += new EventHandler(this.cmbTIF_SelectedIndexChanged);
            this.lblOrderType.AutoSize = true;
            this.lblOrderType.Location = new Point(12, 0x7f);
            this.lblOrderType.Name = "lblOrderType";
            this.lblOrderType.Size = new Size(0x3f, 13);
            this.lblOrderType.TabIndex = 0x17;
            this.lblOrderType.Text = "Order Type:";
            this.cmbOrderType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbOrderType.FormattingEnabled = true;
            this.cmbOrderType.Location = new Point(0x54, 0x7a);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.Size = new Size(100, 0x15);
            this.cmbOrderType.TabIndex = 0x16;
            this.cmbOrderType.SelectedIndexChanged += new EventHandler(this.cmbOrderType_SelectedIndexChanged);
            this.lblQty.AutoSize = true;
            this.lblQty.Location = new Point(12, 0x63);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new Size(0x31, 13);
            this.lblQty.TabIndex = 0x15;
            this.lblQty.Text = "Quantity:";
            this.numQty.InputType = NumEdit.NumEditType.Integer;
            this.numQty.Location = new Point(0x54, 0x5f);
            this.numQty.Name = "numQty";
            this.numQty.Size = new Size(100, 20);
            this.numQty.TabIndex = 20;
            this.numQty.TextChanged += new EventHandler(this.cmbTIF_SelectedIndexChanged);
            this.cmbAction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAction.FormattingEnabled = true;
            this.cmbAction.Location = new Point(0x54, 0x43);
            this.cmbAction.Name = "cmbAction";
            this.cmbAction.Size = new Size(100, 0x15);
            this.cmbAction.TabIndex = 0x13;
            this.cmbAction.SelectedIndexChanged += new EventHandler(this.cmbAction_SelectedIndexChanged);
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new Point(12, 0x47);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new Size(40, 13);
            this.lblAction.TabIndex = 0x12;
            this.lblAction.Text = "Action:";
            this.txtSymbol.CharacterCasing = CharacterCasing.Upper;
            this.txtSymbol.Location = new Point(0x54, 40);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new Size(100, 20);
            this.txtSymbol.TabIndex = 0x11;
            this.txtSymbol.TextChanged += new EventHandler(this.cmbTIF_SelectedIndexChanged);
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.Location = new Point(12, 0x2b);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new Size(0x2c, 13);
            this.lblSymbol.TabIndex = 0x10;
            this.lblSymbol.Text = "Symbol:";
            this.cmbAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAccount.FormattingEnabled = true;
            this.cmbAccount.Location = new Point(0x54, 12);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new Size(0xa8, 0x15);
            this.cmbAccount.TabIndex = 15;
            this.cmbAccount.SelectedIndexChanged += new EventHandler(this.cmbTIF_SelectedIndexChanged);
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new Point(12, 15);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new Size(50, 13);
            this.lblAccount.TabIndex = 14;
            this.lblAccount.Text = "Account:";
            this.lblRoute.AutoSize = true;
            this.lblRoute.Location = new Point(12, 0xb7);
            this.lblRoute.Name = "lblRoute";
            this.lblRoute.Size = new Size(0x27, 13);
            this.lblRoute.TabIndex = 0x1c;
            this.lblRoute.Text = "Route:";
            this.cmbRoute.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRoute.FormattingEnabled = true;
            this.cmbRoute.Location = new Point(0x54, 0xb1);
            this.cmbRoute.Name = "cmbRoute";
            this.cmbRoute.Size = new Size(100, 0x15);
            this.cmbRoute.TabIndex = 0x1d;
            this.cmbRoute.SelectedIndexChanged += new EventHandler(this.cmbRoute_SelectedIndexChanged);
            this.lblTif.AutoSize = true;
            this.lblTif.Location = new Point(12, 0xd3);
            this.lblTif.Name = "lblTif";
            this.lblTif.Size = new Size(0x1a, 13);
            this.lblTif.TabIndex = 30;
            this.lblTif.Text = "TIF:";
            this.cmbTIF.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTIF.FormattingEnabled = true;
            this.cmbTIF.Location = new Point(0x54, 0xcd);
            this.cmbTIF.Name = "cmbTIF";
            this.cmbTIF.Size = new Size(100, 0x15);
            this.cmbTIF.TabIndex = 0x1f;
            this.cmbTIF.SelectedIndexChanged += new EventHandler(this.cmbTIF_SelectedIndexChanged);
            this.lblTradeType.AutoSize = true;
            this.lblTradeType.Location = new Point(12, 0xef);
            this.lblTradeType.Name = "lblTradeType";
            this.lblTradeType.Size = new Size(0x3e, 13);
            this.lblTradeType.TabIndex = 0x20;
            this.lblTradeType.Text = "Trade Type";
            this.accountTypeSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            this.accountTypeSelector.FormattingEnabled = true;
            this.accountTypeSelector.IgnoreCalls = false;
            this.accountTypeSelector.Location = new Point(0x54, 0xec);
            this.accountTypeSelector.Name = "accountTypeSelector";
            this.accountTypeSelector.Size = new Size(100, 0x15);
            this.accountTypeSelector.TabIndex = 0x21;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x109, 0x13c);
            base.Controls.Add(this.accountTypeSelector);
            base.Controls.Add(this.lblTradeType);
            base.Controls.Add(this.cmbTIF);
            base.Controls.Add(this.lblTif);
            base.Controls.Add(this.cmbRoute);
            base.Controls.Add(this.lblRoute);
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
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "EditOrderForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit Order";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0()
        {
            this.btnOK.Enabled = ((((this.cmbAccount.SelectedIndex >= 0) && (this.txtSymbol.Text != "")) && ((this.numQty.Text != "") && (this.numPrice.Text != ""))) && ((this.cmbOrderType.SelectedIndex >= 0) && (this.cmbRoute.SelectedIndex >= 0))) && (this.cmbTIF.SelectedIndex >= 0);
            this.numPrice.Enabled = this.cmbOrderType.Text != "Market";
            if (this.CancelReplaceMode)
            {
                this.btnOK.Enabled = MainModule.Instance.BrokerProvider.AllowCancelReplace(this.order_0, this.cmbTIF.Text);
            }
        }

        public bool CancelReplaceMode
        {
            get
            {
                return !this.cmbAccount.Enabled;
            }
            set
            {
                this.cmbAccount.Enabled = !value;
                this.txtSymbol.Enabled = !value;
                this.cmbAction.Enabled = !value;
                this.cmbRoute.Enabled = !value;
                this.cmbOrderType.Items.Clear();
                foreach (string str in MainModule.Instance.BrokerProvider.CancelReplaceOrderTypesAllowed(this.order_0))
                {
                    this.cmbOrderType.Items.Add(str);
                }
                this.cmbOrderType.SelectedIndex = this.cmbOrderType.Items.IndexOf(this.order_0.OrderTypeString);
            }
        }
    }
}

