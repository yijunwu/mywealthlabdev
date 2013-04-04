namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PolarGridEditor : PolarSeries
    {
        private CheckBox checkBox1;
        private IContainer components;
        private Grid3DSeries grid3DEditor;
        private PolarGrid series;

        public PolarGridEditor()
        {
            this.InitializeComponent();
        }

        public PolarGridEditor(Series s) : this()
        {
            this.series = (PolarGrid) s;
            base.polar = this.series;
            base.SetPolar(base.polar);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.Centered = this.checkBox1.Checked;
            }
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
            this.checkBox1 = new CheckBox();
            base.UDAngleInc.BeginInit();
            base.UDRadiusInc.BeginInit();
            base.tabControl1.SuspendLayout();
            base.tabPage1.SuspendLayout();
            base.tabPage2.SuspendLayout();
            base.SuspendLayout();
            base.tabPage1.Controls.Add(this.checkBox1);
            base.tabPage1.UseVisualStyleBackColor = true;
            base.tabPage1.Controls.SetChildIndex(this.checkBox1, 0);
            base.tabPage1.Controls.SetChildIndex(base.label2, 0);
            base.tabPage1.Controls.SetChildIndex(base.UDRadiusInc, 0);
            base.tabPage1.Controls.SetChildIndex(base.UDAngleInc, 0);
            base.tabPage1.Controls.SetChildIndex(base.label1, 0);
            base.tabPage1.Controls.SetChildIndex(base.CBClose, 0);
            base.tabPage1.Controls.SetChildIndex(base.CBColorEach, 0);
            base.tabPage1.Controls.SetChildIndex(base.BPen, 0);
            base.tabPage2.UseVisualStyleBackColor = true;
            this.checkBox1.AutoSize = true;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(240, 0x8f);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x42, 0x11);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Centered";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            base.ClientSize = new Size(0x188, 0xd6);
            base.Name = "PolarGridEditor";
            base.Load += new EventHandler(this.PolarGridEditor_Load);
            base.UDAngleInc.EndInit();
            base.UDRadiusInc.EndInit();
            base.tabControl1.ResumeLayout(false);
            base.tabPage1.ResumeLayout(false);
            base.tabPage1.PerformLayout();
            base.tabPage2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void PolarGridEditor_Load(object sender, EventArgs e)
        {
            this.checkBox1.Top = base.CBClose.Top;
            base.CBClose.Visible = false;
        }

        public override void SetParent(TabPage Parent)
        {
            base.SetParent(Parent);
            if (this.series != null)
            {
                this.checkBox1.Checked = this.series.Centered;
                if (this.grid3DEditor == null)
                {
                    this.grid3DEditor = new Grid3DSeries(this.series.i3D, Parent);
                }
            }
        }
    }
}

