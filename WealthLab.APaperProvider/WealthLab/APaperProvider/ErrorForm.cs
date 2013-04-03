namespace WealthLab.APaperProvider
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using WealthLab.APaperProvider.Properties;

    public class ErrorForm : Form
    {
        private Button btnOk;
        private IContainer icontainer_0;
        private LinkLabel lnkCopyToClipboard;
        private PictureBox picWarning;
        private string string_0;
        private TextBox txtErrorMessage;

        public ErrorForm()
        {
            this.InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
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

        private void ErrorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape))
            {
                base.Close();
            }
        }

        private void ErrorForm_Load(object sender, EventArgs e)
        {
        }

        public static Form GetMainForm()
        {
            Form form2;
            using (IEnumerator enumerator = Application.OpenForms.GetEnumerator())
            {
                Form current;
                while (enumerator.MoveNext())
                {
                    current = (Form) enumerator.Current;
                    if (current.Name == "MainForm")
                    {
                        goto Label_0036;
                    }
                }
                return null;
            Label_0036:
                form2 = current;
            }
            return form2;
        }

        private void InitializeComponent()
        {
            this.lnkCopyToClipboard = new LinkLabel();
            this.btnOk = new Button();
            this.picWarning = new PictureBox();
            this.txtErrorMessage = new TextBox();
            ((ISupportInitialize) this.picWarning).BeginInit();
            base.SuspendLayout();
            this.lnkCopyToClipboard.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lnkCopyToClipboard.AutoSize = true;
            this.lnkCopyToClipboard.LinkBehavior = LinkBehavior.AlwaysUnderline;
            this.lnkCopyToClipboard.Location = new Point(12, 0x7e);
            this.lnkCopyToClipboard.Name = "lnkCopyToClipboard";
            this.lnkCopyToClipboard.Size = new Size(90, 13);
            this.lnkCopyToClipboard.TabIndex = 1;
            this.lnkCopyToClipboard.TabStop = true;
            this.lnkCopyToClipboard.Text = "Copy to Clipboard";
            this.lnkCopyToClipboard.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkCopyToClipboard_LinkClicked);
            this.btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOk.Location = new Point(0x163, 120);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(80, 0x18);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.picWarning.Image = Resources.warning_3;
            this.picWarning.Location = new Point(11, 9);
            this.picWarning.Name = "picWarning";
            this.picWarning.Size = new Size(0x20, 0x20);
            this.picWarning.TabIndex = 3;
            this.picWarning.TabStop = false;
            this.txtErrorMessage.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.txtErrorMessage.Location = new Point(0x35, 9);
            this.txtErrorMessage.Multiline = true;
            this.txtErrorMessage.Name = "txtErrorMessage";
            this.txtErrorMessage.ReadOnly = true;
            this.txtErrorMessage.ScrollBars = ScrollBars.Vertical;
            this.txtErrorMessage.Size = new Size(0x17d, 100);
            this.txtErrorMessage.TabIndex = 4;
            this.txtErrorMessage.Text = "The selected Streaming provider does not support Paper accounts. Please try selecting a different Streaming provider in Preferences/Streaming Data. By default, Paper accounts will use Yahoo! data.";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1bf, 0x9c);
            base.Controls.Add(this.txtErrorMessage);
            base.Controls.Add(this.picWarning);
            base.Controls.Add(this.btnOk);
            base.Controls.Add(this.lnkCopyToClipboard);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ErrorForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Paper Provider Error";
            base.Load += new EventHandler(this.ErrorForm_Load);
            base.KeyDown += new KeyEventHandler(this.ErrorForm_KeyDown);
            ((ISupportInitialize) this.picWarning).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lnkCopyToClipboard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MethodInvoker method = null;
            if (GetMainForm() != null)
            {
                if (method == null)
                {
                    method = new MethodInvoker(this.method_0);
                }
                GetMainForm().Invoke(method);
            }
        }

        [CompilerGenerated]
        private void method_0()
        {
            try
            {
                Clipboard.SetText(this.string_0);
            }
            catch
            {
            }
        }

        public string ErrorText
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.txtErrorMessage.Text = this.string_0 = value;
            }
        }
    }
}

