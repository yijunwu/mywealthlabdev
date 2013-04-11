namespace QWhale.Editor.Dialogs
{
    using QWhale.Common;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgGoto : Form, IGotoLineDialog
    {
        public Button btCancel;
        public Button btOK;
        private IContainer components;
        public Label laEnterNewLine;
        public Panel panel1;
        public TextBox tbNewLineNumber;

        public DlgGoto()
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

        private void DlgGoto_Load(object sender, EventArgs e)
        {
            this.LoadFromResource();
        }

        public virtual DialogResult Execute(object sender, int lines, ref int line)
        {
            return this.Execute(sender, lines, ref line, null);
        }

        public virtual DialogResult Execute(object sender, int lines, ref int line, IWin32Window owner)
        {
            this.Lines = lines;
            this.Line = line;
            DialogResult result = (owner != null) ? base.ShowDialog(owner) : base.ShowDialog();
            if (result == DialogResult.OK)
            {
                line = this.Line;
            }
            return result;
        }

        private void InitializeComponent()
        {
            this.btCancel = new Button();
            this.laEnterNewLine = new Label();
            this.tbNewLineNumber = new TextBox();
            this.btOK = new Button();
            this.panel1 = new Panel();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btCancel.FlatStyle = FlatStyle.System;
            this.btCancel.Location = new Point(0x98, 8);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new Size(0x4b, 0x17);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.laEnterNewLine.AutoSize = true;
            this.laEnterNewLine.FlatStyle = FlatStyle.System;
            this.laEnterNewLine.Location = new Point(7, 8);
            this.laEnterNewLine.Name = "laEnterNewLine";
            this.laEnterNewLine.Size = new Size(0x44, 13);
            this.laEnterNewLine.TabIndex = 3;
            this.laEnterNewLine.Text = "Line number:";
            this.tbNewLineNumber.Location = new Point(7, 0x1a);
            this.tbNewLineNumber.Name = "tbNewLineNumber";
            this.tbNewLineNumber.Size = new Size(0xd8, 20);
            this.tbNewLineNumber.TabIndex = 4;
            this.tbNewLineNumber.KeyPress += new KeyPressEventHandler(this.tbNewLineNumber_KeyPress);
            this.btOK.DialogResult = DialogResult.OK;
            this.btOK.FlatStyle = FlatStyle.System;
            this.btOK.Location = new Point(0x48, 8);
            this.btOK.Name = "btOK";
            this.btOK.Size = new Size(0x4b, 0x17);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.panel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.panel1.Controls.Add(this.btCancel);
            this.panel1.Controls.Add(this.btOK);
            this.panel1.Location = new Point(-1, 0x34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0xf2, 40);
            this.panel1.TabIndex = 5;
            base.AcceptButton = this.btOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btCancel;
            base.ClientSize = new Size(240, 0x5e);
            base.Controls.Add(this.laEnterNewLine);
            base.Controls.Add(this.tbNewLineNumber);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DlgGoto";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Go To Line";
            base.Load += new EventHandler(this.DlgGoto_Load);
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadFromResource()
        {
            this.Text = StringConsts.DlgGotoCaption;
            this.btOK.Text = StringConsts.OKCaption_GotoDlg;
            this.btCancel.Text = StringConsts.CancelCaption_GotoDlg;
        }

        event HelpEventHandler IGotoLineDialog.HelpRequested
        {
            add
            {
                base.HelpRequested += value;
            }
            remove
            {
                base.HelpRequested -= value;
            }
        }

        private void tbNewLineNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > ' ')
            {
                e.Handled = (e.KeyChar < '0') || (e.KeyChar > '9');
                if (!e.Handled)
                {
                    try
                    {
                        string text = this.tbNewLineNumber.Text;
                        int.Parse(text.Insert(Math.Min(this.tbNewLineNumber.SelectionStart, text.Length), e.KeyChar.ToString()));
                    }
                    catch
                    {
                        e.Handled = true;
                        OSUtils.MessageBeep();
                    }
                }
                else
                {
                    OSUtils.MessageBeep();
                }
            }
        }

        public int Line
        {
            get
            {
                return (int.Parse(this.tbNewLineNumber.Text) - 1);
            }
            set
            {
                this.tbNewLineNumber.Text = (value + 1).ToString();
            }
        }

        public int Lines
        {
            set
            {
                this.laEnterNewLine.Text = string.Format(StringConsts.LineNumberCaption, 1, value);
            }
        }
    }
}

