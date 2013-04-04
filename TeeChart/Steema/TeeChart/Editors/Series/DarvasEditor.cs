namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DarvasEditor : BaseSeriesForm
    {
        private ButtonColor buttonColor1;
        private ButtonPen buttonPen1;
        private CheckBox checkBox1;
        private IContainer components;
        private Darvas darvas;

        public DarvasEditor()
        {
            this.InitializeComponent();
        }

        public DarvasEditor(Series s) : this()
        {
            this.darvas = (Darvas) s;
        }

        private void buttonColor1_Click(object sender, EventArgs e)
        {
            this.darvas.Color = this.buttonColor1.Color;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.darvas.ColorEach = this.checkBox1.Checked;
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
            this.buttonColor1 = new ButtonColor();
            this.buttonPen1 = new ButtonPen();
            this.checkBox1 = new CheckBox();
            base.SuspendLayout();
            this.buttonColor1.Color = Color.Empty;
            this.buttonColor1.Location = new Point(12, 12);
            this.buttonColor1.Name = "buttonColor1";
            this.buttonColor1.Size = new Size(0x66, 0x17);
            this.buttonColor1.TabIndex = 0;
            this.buttonColor1.Text = "Color...";
            this.buttonColor1.UseVisualStyleBackColor = true;
            this.buttonColor1.Click += new EventHandler(this.buttonColor1_Click);
            this.buttonPen1.FlatStyle = FlatStyle.Flat;
            this.buttonPen1.Location = new Point(12, 0x29);
            this.buttonPen1.Name = "buttonPen1";
            this.buttonPen1.Size = new Size(0x66, 0x17);
            this.buttonPen1.TabIndex = 1;
            this.buttonPen1.Text = "Border...";
            this.buttonPen1.UseVisualStyleBackColor = true;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new Point(0x81, 0x10);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x4e, 0x11);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Color Each";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x121, 0x55);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.buttonPen1);
            base.Controls.Add(this.buttonColor1);
            base.Name = "DarvasEditor";
            this.Text = "DarvasEditor";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.darvas != null)
            {
                this.buttonColor1.Color = this.darvas.Color;
                this.buttonPen1.Pen = this.darvas.LinePen;
            }
        }
    }
}

