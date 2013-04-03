namespace WealthLab.PaperProvider
{
    using CtrlLib;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class PaperBrokerProviderSettings : UserControl
    {
        private List<WealthLab.Account> _accounts = new List<WealthLab.Account>();
        private Dictionary<Account, double> _accountValues = new Dictionary<Account, double>();
        private Button btnAddAccount;
        private Button btnDeleteAccount;
        private Button btnUpdateBalance;
        private IContainer components;
        private ListBox lbAccounts;
        private Label lblAccount;
        private Label lblAccounts;
        private Label lblCash;
        private NumEdit numCash;

        public PaperBrokerProviderSettings()
        {
            this.InitializeComponent();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string str;
            int num = 1;
            do
            {
                str = "PaperAccount" + num++;
            }
            while (this.HasAccount(str));
            Account item = new Account {
                AccountNumber = str,
                AccountValue = 100000.0,
                AvailableCash = 100000.0,
                BuyingPower = 100000.0,
                IsPaperAccount = true
            };
            this._accounts.Add(item);
            this.lbAccounts.Items.Add(item);
            this._accountValues[item] = 100000.0;
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            Account selectedItem = (Account) this.lbAccounts.SelectedItem;
            if (selectedItem != null)
            {
                if (this.lbAccounts.Items.Count == 1)
                {
                    MessageBox.Show("Cannot delete the last Account.");
                }
                else
                {
                    this.lbAccounts.Items.Remove(selectedItem);
                    this._accounts.Remove(selectedItem);
                }
            }
        }

        private void btnUpdateBalance_Click(object sender, EventArgs e)
        {
            Account selectedItem = (Account) this.lbAccounts.SelectedItem;
            if (selectedItem != null)
            {
                this._accountValues[selectedItem] = (double) this.numCash.Value;
                this.numCash.Text = this.numCash.Value.ToString("F2");
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

        private bool HasAccount(string s)
        {
            foreach (Account account in this._accounts)
            {
                if (account.AccountNumber == s)
                {
                    return true;
                }
            }
            return false;
        }

        private void InitializeComponent()
        {
            this.lblAccount = new Label();
            this.lblAccounts = new Label();
            this.lbAccounts = new ListBox();
            this.lblCash = new Label();
            this.numCash = new NumEdit();
            this.btnUpdateBalance = new Button();
            this.btnAddAccount = new Button();
            this.btnDeleteAccount = new Button();
            base.SuspendLayout();
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new Point(4, 4);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new Size(0x97, 13);
            this.lblAccount.TabIndex = 0;
            this.lblAccount.Text = "Paper Provider Account Setup";
            this.lblAccounts.AutoSize = true;
            this.lblAccounts.Location = new Point(7, 0x21);
            this.lblAccounts.Name = "lblAccounts";
            this.lblAccounts.Size = new Size(0x34, 13);
            this.lblAccounts.TabIndex = 1;
            this.lblAccounts.Text = "Accounts";
            this.lbAccounts.FormattingEnabled = true;
            this.lbAccounts.Location = new Point(10, 50);
            this.lbAccounts.Name = "lbAccounts";
            this.lbAccounts.Size = new Size(0x91, 160);
            this.lbAccounts.Sorted = true;
            this.lbAccounts.TabIndex = 2;
            this.lbAccounts.SelectedIndexChanged += new EventHandler(this.lbAccounts_SelectedIndexChanged);
            this.lblCash.AutoSize = true;
            this.lblCash.Location = new Point(0xa2, 50);
            this.lblCash.Name = "lblCash";
            this.lblCash.Size = new Size(0x77, 13);
            this.lblCash.TabIndex = 3;
            this.lblCash.Text = "Account Cash Balance:";
            this.numCash.InputType = NumEdit.NumEditType.Double;
            this.numCash.Location = new Point(0xa5, 0x43);
            this.numCash.Name = "numCash";
            this.numCash.Size = new Size(0x74, 20);
            this.numCash.TabIndex = 4;
            this.numCash.TextChanged += new EventHandler(this.numCash_TextChanged);
            this.btnUpdateBalance.Enabled = false;
            this.btnUpdateBalance.Location = new Point(0xa5, 0x5d);
            this.btnUpdateBalance.Name = "btnUpdateBalance";
            this.btnUpdateBalance.Size = new Size(0x74, 0x17);
            this.btnUpdateBalance.TabIndex = 5;
            this.btnUpdateBalance.Text = "Update Balance";
            this.btnUpdateBalance.UseVisualStyleBackColor = true;
            this.btnUpdateBalance.Click += new EventHandler(this.btnUpdateBalance_Click);
            this.btnAddAccount.Location = new Point(10, 0xd9);
            this.btnAddAccount.Name = "btnAddAccount";
            this.btnAddAccount.Size = new Size(0x91, 0x17);
            this.btnAddAccount.TabIndex = 6;
            this.btnAddAccount.Text = "Add Account";
            this.btnAddAccount.UseVisualStyleBackColor = true;
            this.btnAddAccount.Click += new EventHandler(this.btnAddAccount_Click);
            this.btnDeleteAccount.Enabled = false;
            this.btnDeleteAccount.Location = new Point(10, 0xf6);
            this.btnDeleteAccount.Name = "btnDeleteAccount";
            this.btnDeleteAccount.Size = new Size(0x91, 0x17);
            this.btnDeleteAccount.TabIndex = 7;
            this.btnDeleteAccount.Text = "Delete Account";
            this.btnDeleteAccount.UseVisualStyleBackColor = true;
            this.btnDeleteAccount.Click += new EventHandler(this.btnDeleteAccount_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.btnDeleteAccount);
            base.Controls.Add(this.btnAddAccount);
            base.Controls.Add(this.btnUpdateBalance);
            base.Controls.Add(this.numCash);
            base.Controls.Add(this.lblCash);
            base.Controls.Add(this.lbAccounts);
            base.Controls.Add(this.lblAccounts);
            base.Controls.Add(this.lblAccount);
            base.Name = "PaperBrokerProviderSettings";
            base.Size = new Size(300, 0x130);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            Account selectedItem = (Account) this.lbAccounts.SelectedItem;
            if (selectedItem == null)
            {
                this.btnDeleteAccount.Enabled = false;
            }
            else
            {
                this.btnDeleteAccount.Enabled = true;
                this.numCash.Text = this._accountValues[selectedItem].ToString("F2");
                this.btnUpdateBalance.Enabled = false;
            }
        }

        private void numCash_TextChanged(object sender, EventArgs e)
        {
            this.btnUpdateBalance.Enabled = true;
        }

        public List<Account> Accounts
        {
            get
            {
                foreach (Account account in this._accounts)
                {
                    account.AvailableCash = this._accountValues[account];
                    account.BuyingPower = this._accountValues[account];
                }
                return this._accounts;
            }
            set
            {
                this._accounts = value;
                this.lbAccounts.Items.Clear();
                foreach (Account account in this._accounts)
                {
                    this._accountValues[account] = account.BuyingPower;
                    this.lbAccounts.Items.Add(account);
                }
            }
        }
    }
}

