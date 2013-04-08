using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.DataProviders.Helper;
using WealthLab.DataProviders.Yahoo;

internal class ProviderSettingsForm : Form
{
    private Button btnCancel;
    private Button btnOk;
    private CheckBox cbDividendAdj;
    private CheckBox cbNeverPerformOnDemand;
    private CheckBox cbPartialBar;
    private CheckBox cbSplitAdj;
    private GroupBox grpPremium;
    private IContainer icontainer_0;
    private Label lblAttemptCount;
    private Label lblDataRange;
    private Label lblInfo;
    private Label lblLogin;
    private Label lblPassword;
    private Label lblPremium;
    private Label lblThreadCount;
    private LinkLabel linkPremiumAccount;
    private NumericUpDown numAttemptCount;
    private NumericUpDown numThreadCount;
    private Panel pnlInfo;
    private RadioButton rbDataRangeConstant;
    private RadioButton rbDataRangeIgnore;
    private string string_0 = string.Empty;
    private string string_1;
    private string string_2;
    private TextBox txtLogin;
    private TextBox txtPassword;

    public ProviderSettingsForm()
    {
        this.string_0 = YahooStaticProvider.DataPath;
        this.InitializeComponent();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (this.string_0 == string.Empty)
        {
            throw new Exception("The settings file folder is not specified");
        }
        YahooClientSettings settings = new YahooClientSettings {
            ThreadCount = (int) this.numThreadCount.Value,
            AttemptCount = (int) this.numAttemptCount.Value,
            DividendAdj = this.cbDividendAdj.Checked,
            SplitAdj = this.cbSplitAdj.Checked,
            AlwaysPartialBar = this.cbPartialBar.Checked,
            Password = this.txtPassword.Text.Trim(),
            Login = this.txtLogin.Text.Trim()
        };
        if (this.rbDataRangeIgnore.Checked)
        {
            settings.AdjModeWhenDataRange = AdjustedModeWhenDataRange.Ignore;
        }
        settings.Serialize();
        if ((this.string_1 != this.txtLogin.Text.Trim()) || (this.string_2 != this.txtPassword.Text.Trim()))
        {
            Login.Reset();
        }
        YahooStaticProvider.ClientSettings = settings;
    }

    private void cbSplitAdj_CheckedChanged(object sender, EventArgs e)
    {
        this.method_1();
    }

    private void InitializeComponent()
    {
        this.btnCancel = new Button();
        this.btnOk = new Button();
        this.lblInfo = new Label();
        this.pnlInfo = new Panel();
        this.numThreadCount = new NumericUpDown();
        this.lblThreadCount = new Label();
        this.numAttemptCount = new NumericUpDown();
        this.lblAttemptCount = new Label();
        this.cbDividendAdj = new CheckBox();
        this.cbSplitAdj = new CheckBox();
        this.grpPremium = new GroupBox();
        this.linkPremiumAccount = new LinkLabel();
        this.txtPassword = new TextBox();
        this.lblPassword = new Label();
        this.txtLogin = new TextBox();
        this.lblLogin = new Label();
        this.lblPremium = new Label();
        this.cbPartialBar = new CheckBox();
        this.rbDataRangeConstant = new RadioButton();
        this.rbDataRangeIgnore = new RadioButton();
        this.lblDataRange = new Label();
        this.cbNeverPerformOnDemand = new CheckBox();
        this.pnlInfo.SuspendLayout();
        this.numThreadCount.BeginInit();
        this.numAttemptCount.BeginInit();
        this.grpPremium.SuspendLayout();
        base.SuspendLayout();
        this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.btnCancel.DialogResult = DialogResult.Cancel;
        this.btnCancel.Location = new Point(0x1bb, 0x191);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(80, 0x18);
        this.btnCancel.TabIndex = 9;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.btnOk.DialogResult = DialogResult.OK;
        this.btnOk.Location = new Point(0x165, 0x191);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new Size(80, 0x18);
        this.btnOk.TabIndex = 8;
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = true;
        this.btnOk.Click += new EventHandler(this.btnOk_Click);
        this.lblInfo.AutoSize = true;
        this.lblInfo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblInfo.Location = new Point(80, 14);
        this.lblInfo.Name = "lblInfo";
        this.lblInfo.Size = new Size(0x16d, 13);
        this.lblInfo.TabIndex = 2;
        this.lblInfo.Text = "These settings will apply to all DataSets of the Yahoo! provider";
        this.pnlInfo.BackColor = Color.White;
        this.pnlInfo.Controls.Add(this.lblInfo);
        this.pnlInfo.Location = new Point(0, 0);
        this.pnlInfo.Name = "pnlInfo";
        this.pnlInfo.Size = new Size(0x218, 0x29);
        this.pnlInfo.TabIndex = 3;
        this.numThreadCount.Location = new Point(12, 0x38);
        int[] bits = new int[4];
        bits[0] = 10;
        this.numThreadCount.Maximum = new decimal(bits);
        int[] numArray2 = new int[4];
        numArray2[0] = 1;
        this.numThreadCount.Minimum = new decimal(numArray2);
        this.numThreadCount.Name = "numThreadCount";
        this.numThreadCount.Size = new Size(0x2c, 20);
        this.numThreadCount.TabIndex = 0;
        int[] numArray3 = new int[4];
        numArray3[0] = 1;
        this.numThreadCount.Value = new decimal(numArray3);
        this.lblThreadCount.AutoSize = true;
        this.lblThreadCount.Location = new Point(0x3e, 0x3a);
        this.lblThreadCount.Name = "lblThreadCount";
        this.lblThreadCount.Size = new Size(0x164, 13);
        this.lblThreadCount.TabIndex = 5;
        this.lblThreadCount.Text = "Number of threads when loading historical data (recommended setting: 10)";
        this.numAttemptCount.Location = new Point(12, 0x52);
        int[] numArray4 = new int[4];
        numArray4[0] = 5;
        this.numAttemptCount.Maximum = new decimal(numArray4);
        int[] numArray5 = new int[4];
        numArray5[0] = 1;
        this.numAttemptCount.Minimum = new decimal(numArray5);
        this.numAttemptCount.Name = "numAttemptCount";
        this.numAttemptCount.Size = new Size(0x2b, 20);
        this.numAttemptCount.TabIndex = 1;
        int[] numArray6 = new int[4];
        numArray6[0] = 1;
        this.numAttemptCount.Value = new decimal(numArray6);
        this.lblAttemptCount.AutoSize = true;
        this.lblAttemptCount.Location = new Point(0x3e, 0x56);
        this.lblAttemptCount.Name = "lblAttemptCount";
        this.lblAttemptCount.Size = new Size(340, 13);
        this.lblAttemptCount.TabIndex = 7;
        this.lblAttemptCount.Text = "Retry attempts when server didn't return data (recommended setting: 5)";
        this.cbDividendAdj.AutoSize = true;
        this.cbDividendAdj.Location = new Point(12, 0x98);
        this.cbDividendAdj.Name = "cbDividendAdj";
        this.cbDividendAdj.Size = new Size(0xa2, 0x11);
        this.cbDividendAdj.TabIndex = 4;
        this.cbDividendAdj.Text = "Perform Dividend Adjustment";
        this.cbDividendAdj.UseVisualStyleBackColor = true;
        this.cbDividendAdj.CheckedChanged += new EventHandler(this.cbSplitAdj_CheckedChanged);
        this.cbSplitAdj.AutoSize = true;
        this.cbSplitAdj.Location = new Point(12, 0xaf);
        this.cbSplitAdj.Name = "cbSplitAdj";
        this.cbSplitAdj.Size = new Size(140, 0x11);
        this.cbSplitAdj.TabIndex = 5;
        this.cbSplitAdj.Text = "Perform Split Adjustment";
        this.cbSplitAdj.UseVisualStyleBackColor = true;
        this.cbSplitAdj.CheckedChanged += new EventHandler(this.cbSplitAdj_CheckedChanged);
        this.grpPremium.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.grpPremium.Controls.Add(this.linkPremiumAccount);
        this.grpPremium.Controls.Add(this.txtPassword);
        this.grpPremium.Controls.Add(this.lblPassword);
        this.grpPremium.Controls.Add(this.txtLogin);
        this.grpPremium.Controls.Add(this.lblLogin);
        this.grpPremium.Controls.Add(this.lblPremium);
        this.grpPremium.Location = new Point(12, 0x10b);
        this.grpPremium.Name = "grpPremium";
        this.grpPremium.Size = new Size(0x1ff, 0x7d);
        this.grpPremium.TabIndex = 11;
        this.grpPremium.TabStop = false;
        this.grpPremium.Text = "Premium account";
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
        this.lblLogin.AutoSize = true;
        this.lblLogin.Location = new Point(7, 0x49);
        this.lblLogin.Name = "lblLogin";
        this.lblLogin.Size = new Size(0x24, 13);
        this.lblLogin.TabIndex = 1;
        this.lblLogin.Text = "Login:";
        this.lblPremium.Location = new Point(6, 0x10);
        this.lblPremium.Name = "lblPremium";
        this.lblPremium.Size = new Size(0x1f3, 0x2c);
        this.lblPremium.TabIndex = 0;
        this.lblPremium.Text = "If you have a Premium account at Yahoo!, specify its credentials below to receive data without \r\nthe 20 minute delay in streaming and static mode for AMEX, NYSE and NASDAQ.";
        this.lblPremium.TextAlign = ContentAlignment.MiddleCenter;
        this.cbPartialBar.AutoSize = true;
        this.cbPartialBar.Location = new Point(12, 0x6c);
        this.cbPartialBar.Name = "cbPartialBar";
        this.cbPartialBar.Size = new Size(0xb8, 0x11);
        this.cbPartialBar.TabIndex = 3;
        this.cbPartialBar.Text = "Always return data with partial bar";
        this.cbPartialBar.UseVisualStyleBackColor = true;
        this.rbDataRangeConstant.AutoSize = true;
        this.rbDataRangeConstant.Checked = true;
        this.rbDataRangeConstant.Location = new Point(0x1f, 0xda);
        this.rbDataRangeConstant.Name = "rbDataRangeConstant";
        this.rbDataRangeConstant.Size = new Size(0x117, 0x11);
        this.rbDataRangeConstant.TabIndex = 13;
        this.rbDataRangeConstant.TabStop = true;
        this.rbDataRangeConstant.Text = "Keep the adjustments constant based on future prices";
        this.rbDataRangeConstant.UseVisualStyleBackColor = true;
        this.rbDataRangeIgnore.AutoSize = true;
        this.rbDataRangeIgnore.Location = new Point(0x1f, 0xf1);
        this.rbDataRangeIgnore.Name = "rbDataRangeIgnore";
        this.rbDataRangeIgnore.Size = new Size(0x113, 0x11);
        this.rbDataRangeIgnore.TabIndex = 14;
        this.rbDataRangeIgnore.TabStop = true;
        this.rbDataRangeIgnore.Text = "Ignore splits and dividends which fall out of the range";
        this.rbDataRangeIgnore.UseVisualStyleBackColor = true;
        this.lblDataRange.AutoSize = true;
        this.lblDataRange.Location = new Point(9, 0xc6);
        this.lblDataRange.Name = "lblDataRange";
        this.lblDataRange.Size = new Size(0xe2, 13);
        this.lblDataRange.TabIndex = 15;
        this.lblDataRange.Text = "Adjustment mode when Data Range specified:";
        this.cbNeverPerformOnDemand.AutoSize = true;
        this.cbNeverPerformOnDemand.Location = new Point(12, 0x81);
        this.cbNeverPerformOnDemand.Name = "cbNeverPerformOnDemand";
        this.cbNeverPerformOnDemand.Size = new Size(0xdd, 0x11);
        this.cbNeverPerformOnDemand.TabIndex = 0x10;
        this.cbNeverPerformOnDemand.Text = "Never perform On Demand data updates.";
        this.cbNeverPerformOnDemand.UseVisualStyleBackColor = true;
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.ClientSize = new Size(0x217, 430);
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
        base.Controls.Add(this.pnlInfo);
        base.Controls.Add(this.btnOk);
        base.Controls.Add(this.btnCancel);
        base.FormBorderStyle = FormBorderStyle.FixedSingle;
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        base.Name = "ProviderSettingsForm";
        base.ShowIcon = false;
        base.ShowInTaskbar = false;
        base.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Provider Settings";
        base.Shown += new EventHandler(this.ProviderSettingsForm_Shown);
        this.pnlInfo.ResumeLayout(false);
        this.pnlInfo.PerformLayout();
        this.numThreadCount.EndInit();
        this.numAttemptCount.EndInit();
        this.grpPremium.ResumeLayout(false);
        this.grpPremium.PerformLayout();
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    private void linkPremiumAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start("http://billing.finance.yahoo.com/realtime_quotes/signup?.src=quote&.refer=qb");
    }

    private void method_0(YahooClientSettings yahooClientSettings_0)
    {
        this.numThreadCount.Value = yahooClientSettings_0.ThreadCount;
        this.numAttemptCount.Value = yahooClientSettings_0.AttemptCount;
        this.cbDividendAdj.Checked = yahooClientSettings_0.DividendAdj;
        this.cbSplitAdj.Checked = yahooClientSettings_0.SplitAdj;
        this.cbPartialBar.Checked = yahooClientSettings_0.AlwaysPartialBar;
        this.string_1 = this.txtPassword.Text = yahooClientSettings_0.Password;
        this.string_2 = this.txtLogin.Text = yahooClientSettings_0.Login;
        if (yahooClientSettings_0.AdjModeWhenDataRange == AdjustedModeWhenDataRange.Constant)
        {
            this.rbDataRangeConstant.Checked = true;
        }
        else
        {
            this.rbDataRangeIgnore.Checked = true;
        }
        this.method_1();
    }

    private void method_1()
    {
        this.lblDataRange.Enabled = this.rbDataRangeConstant.Enabled = this.rbDataRangeIgnore.Enabled = this.cbDividendAdj.Checked || this.cbSplitAdj.Checked;
    }

    private void ProviderSettingsForm_Shown(object sender, EventArgs e)
    {
        YahooClientSettings settings = YahooClientSettings.Deserialize(this.string_0);
        settings = (settings == null) ? new YahooClientSettings() : settings;
        this.method_0(settings);
    }

    // ///WYJ fix, original signature void Form.Dispose(bool disposing)
    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_0 != null))
        {
            this.icontainer_0.Dispose();
        }
        base.Dispose(disposing);
    }
}

