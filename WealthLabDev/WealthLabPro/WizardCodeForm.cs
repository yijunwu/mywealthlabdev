namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class WizardCodeForm : Form
    {
        private Button btnCopy;
        private Button btnOK;
        private IContainer icontainer_0;
        private TextBox txtCode;

        public WizardCodeForm()
        {
            this.InitializeComponent();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            this.txtCode.SelectAll();
            this.txtCode.Copy();
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
            this.txtCode = new TextBox();
            this.btnOK = new Button();
            this.btnCopy = new Button();
            base.SuspendLayout();
            this.txtCode.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.txtCode.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtCode.Location = new Point(13, 13);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = ScrollBars.Both;
            this.txtCode.Size = new Size(0x1f2, 420);
            this.txtCode.TabIndex = 0;
            this.txtCode.WordWrap = false;
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x1b3, 440);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnCopy.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCopy.Location = new Point(0x13f, 440);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new Size(110, 0x17);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "Copy to Clipboard";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new EventHandler(this.btnCopy_Click);
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x20b, 0x1d9);
            base.Controls.Add(this.btnCopy);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.txtCode);
            base.Name = "WizardCodeForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Builder-Generated Strategy Code";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public string Code
        {
            get
            {
                return this.txtCode.Text;
            }
            set
            {
                this.txtCode.Text = value;
            }
        }
    }
}

