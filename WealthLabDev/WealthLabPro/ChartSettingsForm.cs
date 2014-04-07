namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using DialogResult = System.Windows.Forms.DialogResult;

    public class ChartSettingsForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private IContainer icontainer_0;
        private Panel pnlSettings;

        public ChartSettingsForm()
        {
            this.InitializeComponent();
        }

        public void AddSettingsUI(UserControl userControl_0)
        {
            if (userControl_0.Height != this.pnlSettings.Height)
            {
                this.pnlSettings.Height = userControl_0.Height;
                base.Height = this.pnlSettings.Height + 0x44;
            }
            if (userControl_0.Width != this.pnlSettings.Width)
            {
                int num = userControl_0.Width - this.pnlSettings.Width;
                this.pnlSettings.Width = userControl_0.Width;
                if ((this.btnOK.Left + num) < 0x12)
                {
                    num += (0x12 - this.btnOK.Left) - num;
                }
                this.btnOK.Left += num;
                this.btnCancel.Left += num;
                base.Width = this.btnCancel.Right + 0x12;
            }
            this.pnlSettings.Controls.Add(userControl_0);
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
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.pnlSettings = new Panel();
            base.SuspendLayout();
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new Point(0xd4, 0x88);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnCancel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x125, 0x88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.pnlSettings.AutoScroll = true;
            this.pnlSettings.Dock = DockStyle.Top;
            this.pnlSettings.Location = new Point(0, 0);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new Size(380, 130);
            this.pnlSettings.TabIndex = 2;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(380, 0xa6);
            base.Controls.Add(this.pnlSettings);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "ChartSettingsForm";
            base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chart Settings";
            base.ResumeLayout(false);
        }
    }
}

