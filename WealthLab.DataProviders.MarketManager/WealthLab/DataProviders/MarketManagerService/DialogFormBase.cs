namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class DialogFormBase : Form
    {
        protected MarketInfo _currentMarketInfo;
        public Button btnOk;
        protected Button button1;
        public ErrorProvider errProvider;
        private IContainer icontainer_0;

        public DialogFormBase()
        {
            this.InitializeComponent();
        }

        private void DialogFormBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.DialogResult = DialogResult.Cancel;
            }
            if (e.KeyCode == Keys.Enter)
            {
                this.btnOk.PerformClick();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public virtual void Initialize(MarketInfo currentMarketInfo)
        {
            this._currentMarketInfo = currentMarketInfo;
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DialogFormBase));
            this.btnOk = new Button();
            this.button1 = new Button();
            this.errProvider = new ErrorProvider(this.icontainer_0);
            ((ISupportInitialize) this.errProvider).BeginInit();
            base.SuspendLayout();
            this.btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOk.Location = new Point(0xa1, 0x2d);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x48, 0x18);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.button1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.button1.DialogResult = DialogResult.Cancel;
            this.button1.Location = new Point(0xef, 0x2d);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x48, 0x18);
            this.button1.TabIndex = 6;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.errProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            this.errProvider.ContainerControl = this;
            this.errProvider.Icon = (Icon) manager.GetObject("errProvider.Icon");
            this.errProvider.RightToLeft = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x143, 0x51);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.btnOk);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DialogFormBase";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "DialogBoxBase";
            base.KeyDown += new KeyEventHandler(this.DialogFormBase_KeyDown);
            ((ISupportInitialize) this.errProvider).EndInit();
            base.ResumeLayout(false);
        }

        protected MarketInfo CurrentMarketInfo
        {
            get
            {
                return this._currentMarketInfo;
            }
        }
    }
}

