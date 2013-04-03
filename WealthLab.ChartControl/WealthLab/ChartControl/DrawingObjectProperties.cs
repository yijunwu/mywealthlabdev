namespace WealthLab.ChartControl
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DrawingObjectProperties : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private IContainer icontainer_0;

        public DrawingObjectProperties()
        {
            this.InitializeComponent();
        }

        public void AddUserControl(UserControl userControl_0)
        {
            base.Controls.Add(userControl_0);
            userControl_0.Left = 4;
            base.Width = userControl_0.Width + 10;
            base.Height = userControl_0.Height + 0x40;
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
            base.SuspendLayout();
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x62, 0x47);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xb3, 0x47);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x101, 0x61);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "DrawingObjectProperties";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Drawing Object Properties";
            base.ResumeLayout(false);
        }
    }
}

