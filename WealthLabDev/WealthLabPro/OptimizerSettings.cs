namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class OptimizerSettings : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private IContainer icontainer_0;
        private Panel pnlUI;

        public OptimizerSettings()
        {
            this.InitializeComponent();
        }

        public void AddUserControl(UserControl userControl_0)
        {
            this.pnlUI.Controls.Clear();
            this.pnlUI.Controls.Add(userControl_0);
            userControl_0.Dock = DockStyle.Top;
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
            this.pnlUI = new Panel();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            base.SuspendLayout();
            this.pnlUI.AutoScroll = true;
            this.pnlUI.Dock = DockStyle.Top;
            this.pnlUI.Location = new Point(0, 0);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new Size(0x183, 0x126);
            this.pnlUI.TabIndex = 0;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(300, 0x132);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0xdb, 0x132);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x183, 0x155);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.pnlUI);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "OptimizerSettings";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Optimizer Settings";
            base.ResumeLayout(false);
        }
    }
}

