namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class WLPException : Form
    {
        private Button btnDetails;
        private IContainer icontainer_0;
        private Label lblException;
        private Label lblExceptionDetails;
        private LinkLabel lnkDetails;
        private Panel pnlException;
        private RichTextBox txtExceptionDetails;

        public WLPException()
        {
            this.InitializeComponent();
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            base.Close();
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
            this.btnDetails = new Button();
            this.lblException = new Label();
            this.lnkDetails = new LinkLabel();
            this.pnlException = new Panel();
            this.txtExceptionDetails = new RichTextBox();
            this.lblExceptionDetails = new Label();
            this.pnlException.SuspendLayout();
            base.SuspendLayout();
            this.btnDetails.Location = new Point(0x91, 50);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new Size(0x4b, 0x17);
            this.btnDetails.TabIndex = 0;
            this.btnDetails.Text = "Ok";
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Click += new EventHandler(this.btnDetails_Click);
            this.lblException.Location = new Point(0, 0x10);
            this.lblException.Name = "lblException";
            this.lblException.Size = new Size(0x175, 30);
            this.lblException.TabIndex = 1;
            this.lblException.Text = "Unable to send message. Please click link below for more details.";
            this.lnkDetails.Location = new Point(0xfe, 0x40);
            this.lnkDetails.Name = "lnkDetails";
            this.lnkDetails.Size = new Size(0x68, 0x17);
            this.lnkDetails.TabIndex = 2;
            this.lnkDetails.TabStop = true;
            this.lnkDetails.Text = "View More Details";
            this.lnkDetails.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkDetails_LinkClicked);
            this.pnlException.Controls.Add(this.txtExceptionDetails);
            this.pnlException.Controls.Add(this.lblExceptionDetails);
            this.pnlException.Location = new Point(6, 80);
            this.pnlException.Name = "pnlException";
            this.pnlException.Size = new Size(0x160, 100);
            this.pnlException.TabIndex = 3;
            this.pnlException.Visible = false;
            this.txtExceptionDetails.Location = new Point(8, 20);
            this.txtExceptionDetails.Name = "txtExceptionDetails";
            this.txtExceptionDetails.ReadOnly = true;
            this.txtExceptionDetails.Size = new Size(0x152, 0x4d);
            this.txtExceptionDetails.TabIndex = 2;
            this.txtExceptionDetails.Text = "";
            this.lblExceptionDetails.AutoSize = true;
            this.lblExceptionDetails.Location = new Point(5, 4);
            this.lblExceptionDetails.Name = "lblExceptionDetails";
            this.lblExceptionDetails.Size = new Size(0x59, 13);
            this.lblExceptionDetails.TabIndex = 0;
            this.lblExceptionDetails.Text = "Exception Details";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x16c, 0x4e);
            base.Controls.Add(this.pnlException);
            base.Controls.Add(this.lnkDetails);
            base.Controls.Add(this.lblException);
            base.Controls.Add(this.btnDetails);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "WLPException";
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Wealth Lab Pro Error";
            base.Load += new EventHandler(this.WLPException_Load);
            this.pnlException.ResumeLayout(false);
            this.pnlException.PerformLayout();
            base.ResumeLayout(false);
        }

        private void lnkDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.pnlException.Visible = !this.pnlException.Visible;
            base.Height = this.pnlException.Visible ? 0xd0 : 0x69;
        }

        private void WLPException_Load(object sender, EventArgs e)
        {
        }

        public string ExceptionDetails
        {
            get
            {
                return this.txtExceptionDetails.Text;
            }
            set
            {
                this.txtExceptionDetails.Text = value;
            }
        }
    }
}

