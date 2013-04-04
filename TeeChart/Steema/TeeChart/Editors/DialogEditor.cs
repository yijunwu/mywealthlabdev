namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Editors.Series;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DialogEditor : Form
    {
        private Button bOk;
        private Container components;
        private Panel panel1;

        public DialogEditor()
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
            this.panel1 = new Panel();
            this.bOk = new Button();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.AddRange(new Control[] { this.bOk });
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0xe5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x120, 40);
            this.panel1.TabIndex = 0;
            this.bOk.DialogResult = DialogResult.OK;
            this.bOk.FlatStyle = FlatStyle.Flat;
            this.bOk.Location = new Point(200, 9);
            this.bOk.Name = "bOk";
            this.bOk.TabIndex = 0;
            this.bOk.Text = "Ok";
            base.AcceptButton = this.bOk;
            base.ClientSize = new Size(0x120, 0x10d);
            base.Controls.AddRange(new Control[] { this.panel1 });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.Name = "DialogEditor";
            base.StartPosition = FormStartPosition.CenterParent;
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        internal void InsertForm(Form f)
        {
            if (f is BaseSeriesForm)
            {
                TabControl control = new TabControl {
                    Dock = DockStyle.Fill
                };
                TabPage page = new TabPage(f.Text);
                control.TabPages.Add(page);
                base.Controls.Add(control);
                base.Width = Math.Max(f.Width, control.Width);
                base.Height = Math.Max(f.Height, control.Height) + this.panel1.Height;
                this.bOk.Left = base.Width - 0x5e;
                this.Text = f.Text;
                (f as BaseSeriesForm).SetParent(page);
                EditorUtils.InsertForm(f, page);
            }
            else
            {
                base.Width = f.Width;
                base.Height = f.Height + this.panel1.Height;
                this.bOk.Left = base.Width - 0x5e;
                this.Text = f.Text;
                EditorUtils.InsertForm(f, this);
            }
        }
    }
}

