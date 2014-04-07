namespace WealthLabPro
{
    using QWhale.Editor;
    using QWhale.Syntax;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class EditDescriptionForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private HtmlParser htmlParser_0;
        private IContainer components;
        private Label lblEdit;
        private SyntaxEdit syntaxEdit;

        public EditDescriptionForm()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(EditDescriptionForm));
            this.lblEdit = new Label();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.syntaxEdit = new SyntaxEdit(this.components);
            this.htmlParser_0 = new HtmlParser();
            base.SuspendLayout();
            this.lblEdit.AutoSize = true;
            this.lblEdit.Location = new Point(13, 13);
            this.lblEdit.Name = "lblEdit";
            this.lblEdit.Size = new Size(0xcd, 13);
            this.lblEdit.TabIndex = 0;
            this.lblEdit.Text = "Edit HTML for Strategy Description below:";
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(610, 0x1aa);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new Point(0x211, 0x1aa);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.syntaxEdit.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.syntaxEdit.BackColor = SystemColors.Window;
            this.syntaxEdit.Cursor = Cursors.IBeam;
            this.syntaxEdit.Font = new Font("Courier New", 10f);
            this.syntaxEdit.Lexer = this.htmlParser_0;
            this.syntaxEdit.Location = new Point(12, 0x1d);
            this.syntaxEdit.Name = "syntaxEdit";
            this.syntaxEdit.Size = new Size(0x2a1, 0x187);
            this.syntaxEdit.TabIndex = 4;
            this.syntaxEdit.Text = "";
            this.syntaxEdit.WordWrap = true;
            this.htmlParser_0.DefaultState = 0;
            this.htmlParser_0.Options = SyntaxOptions.SmartIndent | SyntaxOptions.Outline;
            this.htmlParser_0.XmlScheme = resources.GetString("htmlParser.XmlScheme");
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x2b9, 0x1cd);
            base.Controls.Add(this.syntaxEdit);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.lblEdit);
            base.Name = "EditDescriptionForm";
            this.Text = "Edit Strategy Description";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public string Description
        {
            get
            {
                return this.syntaxEdit.Text;
            }
            set
            {
                this.syntaxEdit.Text = value;
            }
        }
    }
}

