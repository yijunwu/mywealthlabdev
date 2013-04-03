namespace WealthLabPro
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class DebugForm : Form
    {
        private ToolStripButton btnClear;
        private ToolStripButton btnTop;
        private IContainer icontainer_0;
        public static DebugForm Instance;
        private StatusStrip status;
        private ToolStrip toolbar;
        private TextBox txtDebug;

        public DebugForm()
        {
            this.InitializeComponent();
        }

        public void AddLine(string text)
        {
            this.txtDebug.Text = this.txtDebug.Text + text + Environment.NewLine;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtDebug.Clear();
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            this.btnTop.Checked = !this.btnTop.Checked;
            base.TopMost = this.btnTop.Checked;
        }

        public void Clear()
        {
            this.txtDebug.Text = "";
        }

        private void DebugForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainModule.Instance.Settings.Set(this, "DebugForm");
            Instance = null;
        }

        private void DebugForm_Load(object sender, EventArgs e)
        {
            Instance = this;
            MainModule.Instance.Settings.Get(this, "DebugForm");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public void Flush(IList<string> messages)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str in messages)
            {
                builder.Append(str);
                builder.Append(Environment.NewLine);
            }
            this.txtDebug.Text = this.txtDebug.Text + builder.ToString();
            messages.Clear();
            this.txtDebug.SelectionStart = this.txtDebug.Text.Length;
            this.txtDebug.ScrollToCaret();
            this.txtDebug.Refresh();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DebugForm));
            this.toolbar = new ToolStrip();
            this.btnClear = new ToolStripButton();
            this.btnTop = new ToolStripButton();
            this.status = new StatusStrip();
            this.txtDebug = new TextBox();
            this.toolbar.SuspendLayout();
            base.SuspendLayout();
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.btnClear, this.btnTop });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x159, 0x19);
            this.toolbar.TabIndex = 0;
            this.toolbar.Text = "toolStrip1";
            this.btnClear.Image = (Image) manager.GetObject("btnClear.Image");
            this.btnClear.ImageTransparentColor = Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(0x34, 0x16);
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            this.btnTop.Alignment = ToolStripItemAlignment.Right;
            this.btnTop.Image = (Image) manager.GetObject("btnTop.Image");
            this.btnTop.ImageTransparentColor = Color.Magenta;
            this.btnTop.Name = "btnTop";
            this.btnTop.Size = new Size(0x44, 0x16);
            this.btnTop.Text = "Topmost";
            this.btnTop.ToolTipText = "Keep this Window on top";
            this.btnTop.Click += new EventHandler(this.btnTop_Click);
            this.status.Location = new Point(0, 0x131);
            this.status.Name = "status";
            this.status.Size = new Size(0x159, 0x16);
            this.status.TabIndex = 1;
            this.status.Text = "statusStrip1";
            this.txtDebug.Dock = DockStyle.Fill;
            this.txtDebug.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtDebug.Location = new Point(0, 0x19);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.ScrollBars = ScrollBars.Vertical;
            this.txtDebug.Size = new Size(0x159, 280);
            this.txtDebug.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x159, 0x147);
            base.Controls.Add(this.txtDebug);
            base.Controls.Add(this.status);
            base.Controls.Add(this.toolbar);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            base.Name = "DebugForm";
            this.Text = "Debug and Error Messages";
            base.FormClosed += new FormClosedEventHandler(this.DebugForm_FormClosed);
            base.Load += new EventHandler(this.DebugForm_Load);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

