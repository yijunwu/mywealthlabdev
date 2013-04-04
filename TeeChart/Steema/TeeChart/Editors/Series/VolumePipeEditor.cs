namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class VolumePipeEditor : BaseSeriesForm
    {
        private Button bPattern;
        private ButtonPen bpLines;
        private CheckBox checkBox1;
        private IContainer components;
        private Label label1;
        private Label label2;
        private VolumePipe series;
        private NumericUpDown udConePercent;
        private NumericUpDown udSeparation;

        public VolumePipeEditor()
        {
            this.InitializeComponent();
        }

        public VolumePipeEditor(Series s) : this()
        {
            this.series = (VolumePipe) s;
        }

        private void bPattern_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
        }

        private void bpLines_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.series.Chart != null)
            {
                this.series.Chart.Invalidate();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.series.Chart != null)
            {
                this.series.LineAsPointColor = this.checkBox1.Checked;
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
            this.bpLines = new ButtonPen();
            this.udConePercent = new NumericUpDown();
            this.label1 = new Label();
            this.bPattern = new Button();
            this.checkBox1 = new CheckBox();
            this.label2 = new Label();
            this.udSeparation = new NumericUpDown();
            this.udConePercent.BeginInit();
            this.udSeparation.BeginInit();
            base.SuspendLayout();
            this.bpLines.FlatStyle = FlatStyle.Flat;
            this.bpLines.Location = new Point(12, 12);
            this.bpLines.Name = "bpLines";
            this.bpLines.Size = new Size(0x4b, 0x17);
            this.bpLines.TabIndex = 0;
            this.bpLines.Text = "Lines...";
            this.bpLines.UseVisualStyleBackColor = true;
            this.bpLines.MouseUp += new MouseEventHandler(this.bpLines_MouseUp);
            this.udConePercent.Location = new Point(0xad, 50);
            this.udConePercent.Name = "udConePercent";
            this.udConePercent.Size = new Size(0x39, 20);
            this.udConePercent.TabIndex = 1;
            this.udConePercent.ValueChanged += new EventHandler(this.udConePercent_ValueChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x5c, 0x34);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4b, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cone Percent:";
            this.bPattern.FlatStyle = FlatStyle.Flat;
            this.bPattern.Location = new Point(12, 0x2f);
            this.bPattern.Name = "bPattern";
            this.bPattern.Size = new Size(0x4b, 0x17);
            this.bPattern.TabIndex = 3;
            this.bPattern.Text = "Pattern...";
            this.bPattern.UseVisualStyleBackColor = true;
            this.bPattern.Click += new EventHandler(this.bPattern_Click);
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new Point(0x5f, 0x10);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x72, 0x11);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Line as Point Color";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x3d, 0x59);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x6a, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Segment Separation:";
            this.udSeparation.Location = new Point(0xad, 0x57);
            int[] bits = new int[4];
            bits[0] = 30;
            this.udSeparation.Maximum = new decimal(bits);
            this.udSeparation.Name = "udSeparation";
            this.udSeparation.Size = new Size(0x39, 20);
            this.udSeparation.TabIndex = 5;
            this.udSeparation.ValueChanged += new EventHandler(this.udSeparation_ValueChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x124, 0x7a);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.udSeparation);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.bPattern);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.udConePercent);
            base.Controls.Add(this.bpLines);
            base.Name = "VolumePipeEditor";
            this.Text = "VolumePipeEditor";
            this.udConePercent.EndInit();
            this.udSeparation.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.series != null)
            {
                this.udConePercent.Value = this.series.ConePercent;
                this.udSeparation.Value = this.series.Separation;
                this.bpLines.Pen = this.series.LinesPen;
                this.checkBox1.Checked = this.series.LineAsPointColor;
            }
        }

        private void udConePercent_ValueChanged(object sender, EventArgs e)
        {
            this.series.ConePercent = Convert.ToInt32(this.udConePercent.Value);
            this.series.Invalidate();
        }

        private void udSeparation_ValueChanged(object sender, EventArgs e)
        {
            this.series.Separation = Convert.ToInt32(this.udSeparation.Value);
            this.series.Invalidate();
        }
    }
}

