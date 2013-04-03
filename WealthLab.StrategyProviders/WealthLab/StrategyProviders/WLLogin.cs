namespace WealthLab.StrategyProviders
{
    using MS123.Web.Strategies;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class WLLogin : Form
    {
        private bool _bFormAction;
        private static string _encryptedPassword = "";
        private static string _encryptedUsername = "";
        private static bool _rememberMe = false;
        private static RSACryptography _rsaCrypt = new RSACryptography();
        private Button btnCancel;
        private Button btnOK;
        private CheckBox cbRememberMe;
        private IContainer components;
        private Label lblPassword;
        private Label lblUserName;
        private TextBox tbPassword;
        private TextBox tbUsername;

        internal WLLogin()
        {
            this.InitializeComponent();
        }

        private void cbRememberMe_CheckedChanged(object sender, EventArgs e)
        {
            if (!this._bFormAction)
            {
                _rememberMe = this.cbRememberMe.Checked;
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

        private void EnableOK()
        {
            this.btnOK.Enabled = this._FormComplete;
        }

        private void EnableRememberMe()
        {
            this.cbRememberMe.Enabled = this._FormComplete;
            if (!this.cbRememberMe.Enabled)
            {
                this.cbRememberMe.Checked = false;
            }
            else
            {
                this.cbRememberMe.Checked = _rememberMe;
            }
        }

        private void InitializeComponent()
        {
            this.cbRememberMe = new CheckBox();
            this.lblPassword = new Label();
            this.lblUserName = new Label();
            this.tbUsername = new TextBox();
            this.tbPassword = new TextBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            base.SuspendLayout();
            this.cbRememberMe.AutoSize = true;
            this.cbRememberMe.Enabled = false;
            this.cbRememberMe.Location = new Point(13, 0x63);
            this.cbRememberMe.Name = "cbRememberMe";
            this.cbRememberMe.Size = new Size(0x97, 0x11);
            this.cbRememberMe.TabIndex = 15;
            this.cbRememberMe.Text = "Remember me this session";
            this.cbRememberMe.UseVisualStyleBackColor = true;
            this.cbRememberMe.CheckedChanged += new EventHandler(this.cbRememberMe_CheckedChanged);
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblPassword.Location = new Point(13, 0x34);
            this.lblPassword.Margin = new Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new Size(0x40, 15);
            this.lblPassword.TabIndex = 12;
            this.lblPassword.Text = "Password:";
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblUserName.Location = new Point(13, 9);
            this.lblUserName.Margin = new Padding(4, 0, 4, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new Size(0x49, 15);
            this.lblUserName.TabIndex = 0x10;
            this.lblUserName.Text = "User Name:";
            this.tbUsername.Location = new Point(13, 0x1c);
            this.tbUsername.Margin = new Padding(4);
            this.tbUsername.MaxLength = 20;
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new Size(0xce, 20);
            this.tbUsername.TabIndex = 13;
            this.tbUsername.TextChanged += new EventHandler(this.tbUsername_TextChanged);
            this.tbPassword.Location = new Point(12, 0x47);
            this.tbPassword.Margin = new Padding(4);
            this.tbPassword.MaxLength = 20;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new Size(0xce, 20);
            this.tbPassword.TabIndex = 14;
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbPassword.TextChanged += new EventHandler(this.tbPassword_TextChanged);
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new Point(0xe2, 0x1c);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4a, 0x17);
            this.btnOK.TabIndex = 0x11;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xe1, 0x44);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0x12;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x135, 0x7e);
            base.ControlBox = false;
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.cbRememberMe);
            base.Controls.Add(this.lblPassword);
            base.Controls.Add(this.lblUserName);
            base.Controls.Add(this.tbUsername);
            base.Controls.Add(this.tbPassword);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "WLLogin";
            base.SizeGripStyle = SizeGripStyle.Hide;
            this.Text = "Wealth-Lab.com Login";
            base.Load += new EventHandler(this.WLLogin_Load);
            base.FormClosing += new FormClosingEventHandler(this.WLLogin_FormClosing);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            if (!this._bFormAction)
            {
                this.EnableOK();
                this.EnableRememberMe();
            }
        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {
            if (!this._bFormAction)
            {
                this.EnableOK();
                this.EnableRememberMe();
            }
        }

        private void WLLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._bFormAction = true;
            _encryptedUsername = _rsaCrypt.EncryptString(this.tbUsername.Text);
            _encryptedPassword = _rsaCrypt.EncryptString(this.tbPassword.Text);
            this.tbUsername.Text = "";
            this.tbPassword.Text = "";
            this._bFormAction = false;
        }

        private void WLLogin_Load(object sender, EventArgs e)
        {
            this._bFormAction = true;
            if (_rememberMe)
            {
                try
                {
                    this.tbUsername.Text = (_encryptedUsername.Length > 0) ? _rsaCrypt.DecryptString(_encryptedUsername) : "";
                    this.tbPassword.Text = (_encryptedPassword.Length > 0) ? _rsaCrypt.DecryptString(_encryptedPassword) : "";
                }
                catch
                {
                    _rememberMe = false;
                }
            }
            this.EnableRememberMe();
            this.EnableOK();
            this.tbUsername.Focus();
            this._bFormAction = false;
        }

        private bool _FormComplete
        {
            get
            {
                return ((this.tbUsername.Text.Length > 0) && (this.tbPassword.Text.Length > 0));
            }
        }

        internal byte[] Password
        {
            get
            {
                return RSACryptography.GetMD5Hash((_encryptedPassword.Length > 0) ? _rsaCrypt.DecryptString(_encryptedPassword) : this.tbPassword.Text);
            }
        }

        internal string Username
        {
            get
            {
                if (_encryptedUsername.Length <= 0)
                {
                    return this.tbUsername.Text;
                }
                return _rsaCrypt.DecryptString(_encryptedUsername);
            }
        }
    }
}

