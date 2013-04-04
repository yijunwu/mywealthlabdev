namespace Fidelity.Components
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DontShowAgainForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private CheckBox cbDontShow;
        private IContainer icontainer_0;
        private Label lblMessage;

        public DontShowAgainForm(string caption, string label)
        {
            this.InitializeComponent();
            this.Text = caption;
            this.lblMessage.Text = label;
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
            this.lblMessage = new Label();
            this.cbDontShow = new CheckBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            base.SuspendLayout();
            this.lblMessage.Location = new Point(13, 13);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new Size(0xfe, 0x53);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Message";
            this.cbDontShow.AutoSize = true;
            this.cbDontShow.Location = new Point(0x10, 0x63);
            this.cbDontShow.Name = "cbDontShow";
            this.cbDontShow.Size = new Size(0xac, 0x11);
            this.cbDontShow.TabIndex = 1;
            this.cbDontShow.Text = "Don't show this message again";
            this.cbDontShow.UseVisualStyleBackColor = true;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0xbf, 0x8a);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xbf, 0x8a);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x117, 0xad);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.cbDontShow);
            base.Controls.Add(this.lblMessage);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DontShowAgainForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "DontShowAgainForm";
            base.TopMost = true;
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void MakeYesNo()
        {
            this.btnOK.Text = "Yes";
            this.btnCancel.Text = "No";
        }

        public bool CancelButtonVisible
        {
            get
            {
                return this.btnCancel.Visible;
            }
            set
            {
                this.btnCancel.Visible = value;
                if (value)
                {
                    this.btnOK.Left = 0x71;
                }
                else
                {
                    this.btnOK.Left = 0xbf;
                }
            }
        }

        public bool DontShowAgain
        {
            get
            {
                return this.cbDontShow.Checked;
            }
        }
    }
}

