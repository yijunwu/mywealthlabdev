using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WealthLab;
using WealthLab.DataProviders.Helper;
using WealthLab.DataProviders.Yahoo;

internal class ProviderSettingsControl : DataBehaviorUserControl
{
    private bool bool_0 = true;
    private CheckBox cbDividendAdj;
    private CheckBox cbNeverPerformOnDemand;
    private CheckBox cbPartialBar;
    private CheckBox cbSplitAdj;
    private GroupBox grpPremium;
    private IContainer icontainer_1;
    private Label lblAttemptCount;
    private Label lblDataRange;
    private Label lblLogin;
    private Label lblPassword;
    private Label lblPremium;
    private Label lblThreadCount;
    private LinkLabel linkPremiumAccount;
    private NumericUpDown numAttemptCount;
    private NumericUpDown numThreadCount;
    private RadioButton rbDataRangeConstant;
    private RadioButton rbDataRangeIgnore;
    private string string_0;
    private string string_1;
    private TextBox txtLogin;
    private TextBox txtPassword;

    public ProviderSettingsControl()
    {
        this.InitializeComponent_1();
        Application.ApplicationExit += new EventHandler(this.method_0);
    }

    private void cbDividendAdj_CheckedChanged(object sender, EventArgs e)
    {
        this.method_3();
        this.method_2();
    }

    private void cbNeverPerformOnDemand_CheckedChanged(object sender, EventArgs e)
    {
        this.method_2();
    }

    private void InitializeComponent_1()
    {
        this.lblDataRange = new Label();
        this.rbDataRangeIgnore = new RadioButton();
        this.rbDataRangeConstant = new RadioButton();
        this.cbPartialBar = new CheckBox();
        this.grpPremium = new GroupBox();
        this.linkPremiumAccount = new LinkLabel();
        this.txtPassword = new TextBox();
        this.lblPassword = new Label();
        this.txtLogin = new TextBox();
        this.lblLogin = new Label();
        this.lblPremium = new Label();
        this.cbSplitAdj = new CheckBox();
        this.cbDividendAdj = new CheckBox();
        this.lblAttemptCount = new Label();
        this.numAttemptCount = new NumericUpDown();
        this.lblThreadCount = new Label();
        this.numThreadCount = new NumericUpDown();
        this.cbNeverPerformOnDemand = new CheckBox();
        this.grpPremium.SuspendLayout();
        this.numAttemptCount.BeginInit();
        this.numThreadCount.BeginInit();
        base.SuspendLayout();
        this.lblDataRange.AutoSize = true;
        this.lblDataRange.Location = new Point(4, 0x97);
        this.lblDataRange.Name = "lblDataRange";
        this.lblDataRange.Size = new Size(0xe2, 13);
        this.lblDataRange.TabIndex = 0x1a;
        this.lblDataRange.Text = "Adjustment mode when Data Range specified:";
        this.rbDataRangeIgnore.AutoSize = true;
        this.rbDataRangeIgnore.Location = new Point(0x1a, 0xc2);
        this.rbDataRangeIgnore.Name = "rbDataRangeIgnore";
        this.rbDataRangeIgnore.Size = new Size(0x113, 0x11);
        this.rbDataRangeIgnore.TabIndex = 0x19;
        this.rbDataRangeIgnore.TabStop = true;
        this.rbDataRangeIgnore.Text = "Ignore splits and dividends which fall out of the range";
        this.rbDataRangeIgnore.UseVisualStyleBackColor = true;
        this.rbDataRangeIgnore.CheckedChanged += new EventHandler(this.cbNeverPerformOnDemand_CheckedChanged);
        this.rbDataRangeConstant.AutoSize = true;
        this.rbDataRangeConstant.Checked = true;
        this.rbDataRangeConstant.Location = new Point(0x1a, 0xab);
        this.rbDataRangeConstant.Name = "rbDataRangeConstant";
        this.rbDataRangeConstant.Size = new Size(0x117, 0x11);
        this.rbDataRangeConstant.TabIndex = 0x18;
        this.rbDataRangeConstant.TabStop = true;
        this.rbDataRangeConstant.Text = "Keep the adjustments constant based on future prices";
        this.rbDataRangeConstant.UseVisualStyleBackColor = true;
        this.rbDataRangeConstant.CheckedChanged += new EventHandler(this.cbNeverPerformOnDemand_CheckedChanged);
        this.cbPartialBar.AutoSize = true;
        this.cbPartialBar.Location = new Point(7, 0x3b);
        this.cbPartialBar.Name = "cbPartialBar";
        this.cbPartialBar.Size = new Size(0xb8, 0x11);
        this.cbPartialBar.TabIndex = 0x12;
        this.cbPartialBar.Text = "Always return data with partial bar";
        this.cbPartialBar.UseVisualStyleBackColor = true;
        this.cbPartialBar.CheckedChanged += new EventHandler(this.cbNeverPerformOnDemand_CheckedChanged);
        this.grpPremium.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
        this.grpPremium.Controls.Add(this.linkPremiumAccount);
        this.grpPremium.Controls.Add(this.txtPassword);
        this.grpPremium.Controls.Add(this.lblPassword);
        this.grpPremium.Controls.Add(this.txtLogin);
        this.grpPremium.Controls.Add(this.lblLogin);
        this.grpPremium.Controls.Add(this.lblPremium);
        this.grpPremium.Location = new Point(7, 0xe0);
        this.grpPremium.Name = "grpPremium";
        this.grpPremium.Size = new Size(0x1ff, 0x7d);
        this.grpPremium.TabIndex = 0x17;
        this.grpPremium.TabStop = false;
        this.grpPremium.Text = "Premium account";
        this.linkPremiumAccount.Anchor = AnchorStyles.Right | AnchorStyles.Top;
        this.linkPremiumAccount.AutoSize = true;
        this.linkPremiumAccount.Location = new Point(320, 0x6a);
        this.linkPremiumAccount.Name = "linkPremiumAccount";
        this.linkPremiumAccount.Size = new Size(0xb9, 13);
        this.linkPremiumAccount.TabIndex = 8;
        this.linkPremiumAccount.TabStop = true;
        this.linkPremiumAccount.Text = "More information on Premium account";
        this.linkPremiumAccount.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkPremiumAccount_LinkClicked);
        this.txtPassword.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.txtPassword.Location = new Point(0x42, 0x63);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.PasswordChar = '*';
        this.txtPassword.Size = new Size(0xb8, 20);
        this.txtPassword.TabIndex = 7;
        this.txtPassword.TextChanged += new EventHandler(this.cbNeverPerformOnDemand_CheckedChanged);
        this.lblPassword.AutoSize = true;
        this.lblPassword.Location = new Point(7, 0x66);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new Size(0x38, 13);
        this.lblPassword.TabIndex = 3;
        this.lblPassword.Text = "Password:";
        this.txtLogin.Location = new Point(0x42, 70);
        this.txtLogin.Name = "txtLogin";
        this.txtLogin.Size = new Size(0xb8, 20);
        this.txtLogin.TabIndex = 6;
        this.txtLogin.TextChanged += new EventHandler(this.cbNeverPerformOnDemand_CheckedChanged);
        this.lblLogin.AutoSize = true;
        this.lblLogin.Location = new Point(7, 0x49);
        this.lblLogin.Name = "lblLogin";
        this.lblLogin.Size = new Size(0x24, 13);
        this.lblLogin.TabIndex = 1;
        this.lblLogin.Text = "Login:";
        this.lblPremium.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
        this.lblPremium.Location = new Point(6, 0x10);
        this.lblPremium.Name = "lblPremium";
        this.lblPremium.Size = new Size(0x1f3, 0x2c);
        this.lblPremium.TabIndex = 0;
        this.lblPremium.Text = "If you have a Premium account at Yahoo!, specify its credentials below to receive data without the 20 minute delay in streaming and static mode for AMEX, NYSE and NASDAQ.";
        this.lblPremium.TextAlign = ContentAlignment.MiddleCenter;
        this.cbSplitAdj.AutoSize = true;
        this.cbSplitAdj.Location = new Point(7, 0x80);
        this.cbSplitAdj.Name = "cbSplitAdj";
        this.cbSplitAdj.Size = new Size(140, 0x11);
        this.cbSplitAdj.TabIndex = 0x15;
        this.cbSplitAdj.Text = "Perform Split Adjustment";
        this.cbSplitAdj.UseVisualStyleBackColor = true;
        this.cbSplitAdj.CheckedChanged += new EventHandler(this.cbDividendAdj_CheckedChanged);
        this.cbDividendAdj.AutoSize = true;
        this.cbDividendAdj.Location = new Point(7, 0x69);
        this.cbDividendAdj.Name = "cbDividendAdj";
        this.cbDividendAdj.Size = new Size(0xa2, 0x11);
        this.cbDividendAdj.TabIndex = 0x13;
        this.cbDividendAdj.Text = "Perform Dividend Adjustment";
        this.cbDividendAdj.UseVisualStyleBackColor = true;
        this.cbDividendAdj.CheckedChanged += new EventHandler(this.cbDividendAdj_CheckedChanged);
        this.lblAttemptCount.AutoSize = true;
        this.lblAttemptCount.Location = new Point(0x39, 0x25);
        this.lblAttemptCount.Name = "lblAttemptCount";
        this.lblAttemptCount.Size = new Size(340, 13);
        this.lblAttemptCount.TabIndex = 0x16;
        this.lblAttemptCount.Text = "Retry attempts when server didn't return data (recommended setting: 5)";
        this.numAttemptCount.Location = new Point(7, 0x21);
        int[] bits = new int[4];
        bits[0] = 5;
        this.numAttemptCount.Maximum = new decimal(bits);
        int[] numArray2 = new int[4];
        numArray2[0] = 1;
        this.numAttemptCount.Minimum = new decimal(numArray2);
        this.numAttemptCount.Name = "numAttemptCount";
        this.numAttemptCount.Size = new Size(0x2b, 20);
        this.numAttemptCount.TabIndex = 0x11;
        int[] numArray3 = new int[4];
        numArray3[0] = 1;
        this.numAttemptCount.Value = new decimal(numArray3);
        this.numAttemptCount.ValueChanged += new EventHandler(this.cbNeverPerformOnDemand_CheckedChanged);
        this.lblThreadCount.AutoSize = true;
        this.lblThreadCount.Location = new Point(0x39, 9);
        this.lblThreadCount.Name = "lblThreadCount";
        this.lblThreadCount.Size = new Size(0x164, 13);
        this.lblThreadCount.TabIndex = 20;
        this.lblThreadCount.Text = "Number of threads when loading historical data (recommended setting: 10)";
        this.numThreadCount.Location = new Point(7, 7);
        int[] numArray4 = new int[4];
        numArray4[0] = 10;
        this.numThreadCount.Maximum = new decimal(numArray4);
        int[] numArray5 = new int[4];
        numArray5[0] = 1;
        this.numThreadCount.Minimum = new decimal(numArray5);
        this.numThreadCount.Name = "numThreadCount";
        this.numThreadCount.Size = new Size(0x2c, 20);
        this.numThreadCount.TabIndex = 0x10;
        int[] numArray6 = new int[4];
        numArray6[0] = 1;
        this.numThreadCount.Value = new decimal(numArray6);
        this.numThreadCount.ValueChanged += new EventHandler(this.cbNeverPerformOnDemand_CheckedChanged);
        this.cbNeverPerformOnDemand.AutoSize = true;
        this.cbNeverPerformOnDemand.Location = new Point(7, 0x52);
        this.cbNeverPerformOnDemand.Name = "cbNeverPerformOnDemand";
        this.cbNeverPerformOnDemand.Size = new Size(0xda, 0x11);
        this.cbNeverPerformOnDemand.TabIndex = 0x1b;
        this.cbNeverPerformOnDemand.Text = "Never perform On Demand data updates";
        this.cbNeverPerformOnDemand.UseVisualStyleBackColor = true;
        this.cbNeverPerformOnDemand.CheckedChanged += new EventHandler(this.cbNeverPerformOnDemand_CheckedChanged);
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.cbNeverPerformOnDemand);
        base.Controls.Add(this.lblDataRange);
        base.Controls.Add(this.rbDataRangeIgnore);
        base.Controls.Add(this.rbDataRangeConstant);
        base.Controls.Add(this.cbPartialBar);
        base.Controls.Add(this.grpPremium);
        base.Controls.Add(this.cbSplitAdj);
        base.Controls.Add(this.cbDividendAdj);
        base.Controls.Add(this.lblAttemptCount);
        base.Controls.Add(this.numAttemptCount);
        base.Controls.Add(this.lblThreadCount);
        base.Controls.Add(this.numThreadCount);
        base.Name = "ProviderSettingsControl";
        base.Size = new Size(0x20e, 360);
        base.Tag = "Yahoo! Data";
        this.grpPremium.ResumeLayout(false);
        this.grpPremium.PerformLayout();
        this.numAttemptCount.EndInit();
        this.numThreadCount.EndInit();
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    private void linkPremiumAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start("http://billing.finance.yahoo.com/realtime_quotes/signup?.src=quote&.refer=qb");
    }

    private void method_0(object sender, EventArgs e)
    {
        YahooStaticProvider.ClientSettings.Serialize();
    }

    public void method_1()
    {
        this.bool_0 = false;
        this.numThreadCount.Value = YahooStaticProvider.ClientSettings.ThreadCount;
        this.numAttemptCount.Value = YahooStaticProvider.ClientSettings.AttemptCount;
        this.cbDividendAdj.Checked = YahooStaticProvider.ClientSettings.DividendAdj;
        this.cbSplitAdj.Checked = YahooStaticProvider.ClientSettings.SplitAdj;
        this.cbPartialBar.Checked = YahooStaticProvider.ClientSettings.AlwaysPartialBar;
        this.string_0 = this.txtPassword.Text = YahooStaticProvider.ClientSettings.Password;
        this.string_1 = this.txtLogin.Text = YahooStaticProvider.ClientSettings.Login;
        this.cbNeverPerformOnDemand.Checked = YahooStaticProvider.ClientSettings.NeverPerformOnDemandUpdates;
        if (YahooStaticProvider.ClientSettings.AdjModeWhenDataRange == AdjustedModeWhenDataRange.Constant)
        {
            this.rbDataRangeConstant.Checked = true;
        }
        else
        {
            this.rbDataRangeIgnore.Checked = true;
        }
        this.method_3();
        this.bool_0 = true;
    }

    private void method_2()
    {
        if (this.bool_0)
        {
            YahooStaticProvider.ClientSettings.ThreadCount = (int) this.numThreadCount.Value;
            YahooStaticProvider.ClientSettings.AttemptCount = (int) this.numAttemptCount.Value;
            YahooStaticProvider.ClientSettings.DividendAdj = this.cbDividendAdj.Checked;
            YahooStaticProvider.ClientSettings.SplitAdj = this.cbSplitAdj.Checked;
            YahooStaticProvider.ClientSettings.AlwaysPartialBar = this.cbPartialBar.Checked;
            YahooStaticProvider.ClientSettings.Password = this.txtPassword.Text.Trim();
            YahooStaticProvider.ClientSettings.Login = this.txtLogin.Text.Trim();
            YahooStaticProvider.ClientSettings.AdjModeWhenDataRange = this.rbDataRangeIgnore.Checked ? AdjustedModeWhenDataRange.Ignore : AdjustedModeWhenDataRange.Constant;
            YahooStaticProvider.ClientSettings.NeverPerformOnDemandUpdates = this.cbNeverPerformOnDemand.Checked;
            if ((this.string_0 != this.txtLogin.Text.Trim()) || (this.string_1 != this.txtPassword.Text.Trim()))
            {
                Class18.smethod_4();
            }
        }
    }

    private void method_3()
    {
        this.lblDataRange.Enabled = this.rbDataRangeConstant.Enabled = this.rbDataRangeIgnore.Enabled = this.cbDividendAdj.Checked || this.cbSplitAdj.Checked;
    }

    void DataBehaviorUserControl.Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_1 != null))
        {
            this.icontainer_1.Dispose();
        }
        base.Dispose(disposing);
    }
}

