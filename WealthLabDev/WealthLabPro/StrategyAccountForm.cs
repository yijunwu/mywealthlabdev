namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Label = System.Windows.Forms.Label;

    public class StrategyAccountForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private ComboBox cmbAccount;
        private IContainer icontainer_0;
        private Label lblAccount;
        private string string_0 = "";

        public StrategyAccountForm()
        {
            this.InitializeComponent();
        }

        private void cmbAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.string_0 = this.cmbAccount.Text;
            this.btnOK.Enabled = this.cmbAccount.SelectedIndex >= 0;
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
            this.lblAccount = new Label();
            this.cmbAccount = new ComboBox();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            base.SuspendLayout();
            this.lblAccount.Location = new Point(13, 13);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new Size(0xf7, 0x22);
            this.lblAccount.TabIndex = 0;
            this.lblAccount.Text = "Select the Account to use for Orders generated from this Strategy:";
            this.cmbAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAccount.FormattingEnabled = true;
            this.cmbAccount.Location = new Point(0x10, 0x3d);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new Size(0xed, 0x15);
            this.cmbAccount.TabIndex = 1;
            this.cmbAccount.SelectedIndexChanged += new EventHandler(this.cmbAccount_SelectedIndexChanged);
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xb1, 0x6c);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new Point(0x60, 0x6c);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x109, 0x8d);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.cmbAccount);
            base.Controls.Add(this.lblAccount);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "StrategyAccountForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Strategy Account";
            base.Load += new EventHandler(this.StrategyAccountForm_Load);
            base.ResumeLayout(false);
        }

        private void StrategyAccountForm_Load(object sender, EventArgs e)
        {
            foreach (string str in MainModule.Instance.AccountNumbers)
            {
                this.cmbAccount.Items.Add(str);
            }
            this.cmbAccount.SelectedIndex = this.cmbAccount.Items.IndexOf(this.string_0);
            if (this.cmbAccount.SelectedIndex == -1)
            {
                this.cmbAccount.SelectedIndex = this.cmbAccount.Items.IndexOf(MainModule.Instance.DefaultAccountNumber);
            }
            if ((this.cmbAccount.SelectedIndex == -1) && (this.cmbAccount.Items.Count > 0))
            {
                this.cmbAccount.SelectedIndex = 0;
            }
        }

        public string AccountNumber
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }
    }
}

