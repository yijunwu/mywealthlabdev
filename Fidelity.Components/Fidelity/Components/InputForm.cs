namespace Fidelity.Components
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class InputForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private IContainer icontainer_0;
        private Label lblLabel;
        private TextBox txtInput;

        public InputForm(string caption, string label, string defaultValue, CharacterCasing casing)
        {
            this.InitializeComponent();
            this.Text = caption;
            this.lblLabel.Text = label;
            this.txtInput.Text = defaultValue;
            this.txtInput.CharacterCasing = casing;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtInput.Text = "";
            base.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
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
            this.lblLabel = new Label();
            this.txtInput = new TextBox();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            base.SuspendLayout();
            this.lblLabel.AutoSize = true;
            this.lblLabel.Location = new Point(13, 13);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new Size(0x3f, 13);
            this.lblLabel.TabIndex = 0;
            this.lblLabel.Text = "Input Label:";
            this.txtInput.Location = new Point(13, 30);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new Size(0x114, 20);
            this.txtInput.TabIndex = 1;
            this.txtInput.TextChanged += new EventHandler(this.txtInput_TextChanged);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xd5, 0x43);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Enabled = false;
            this.btnOK.Location = new Point(0x84, 0x43);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x128, 0x60);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.txtInput);
            base.Controls.Add(this.lblLabel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "InputForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "InputForm";
            base.TopMost = true;
            base.Load += new EventHandler(this.InputForm_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InputForm_Load(object sender, EventArgs e)
        {
            this.txtInput.SelectAll();
            this.txtInput.Focus();
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            this.btnOK.Enabled = this.txtInput.Text != "";
        }

        public string Input
        {
            get
            {
                return this.txtInput.Text;
            }
        }
    }
}

