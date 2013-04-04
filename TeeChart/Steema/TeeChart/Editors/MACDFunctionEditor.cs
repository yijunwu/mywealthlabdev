namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Functions;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MACDFunctionEditor : Form
    {
        private ButtonPen bHistogram;
        private ButtonPen bMACD;
        private ButtonPen bMACDExp;
        private Container components;
        internal Control controlToEnable;
        private MACDFunction function;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;

        public MACDFunctionEditor()
        {
            this.InitializeComponent();
        }

        public MACDFunctionEditor(Function f) : this()
        {
            this.function = (MACDFunction) f;
            this.numericUpDown1.Value = Convert.ToDecimal(this.function.Period);
            this.numericUpDown2.Value = Convert.ToDecimal(this.function.Period2);
            this.numericUpDown3.Value = Convert.ToDecimal(this.function.Period3);
            this.bHistogram.Pen = this.function.HistogramPen;
            this.bMACD.Pen = this.function.MACDPen;
            this.bMACDExp.Pen = this.function.MACDExpPen;
        }

        private void bHistogram_Click(object sender, EventArgs e)
        {
            this.function.HistogramPen = this.bHistogram.Pen;
        }

        private void bMACD_Click(object sender, EventArgs e)
        {
            this.function.MACDPen = this.bMACD.Pen;
        }

        private void bMACDExp_Click(object sender, EventArgs e)
        {
            this.function.MACDExpPen = this.bMACDExp.Pen;
        }

        private void Changed()
        {
            if (this.controlToEnable != null)
            {
                this.controlToEnable.Enabled = true;
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
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.numericUpDown2 = new NumericUpDown();
            this.numericUpDown3 = new NumericUpDown();
            this.bHistogram = new ButtonPen();
            this.bMACD = new ButtonPen();
            this.bMACDExp = new ButtonPen();
            this.numericUpDown1.BeginInit();
            this.numericUpDown2.BeginInit();
            this.numericUpDown3.BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x10, 0x20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x31, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Period &1:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x10, 0x38);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x31, 0x10);
            this.label2.TabIndex = 1;
            this.label2.Text = "Period &2:";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x10, 80);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x31, 0x10);
            this.label3.TabIndex = 2;
            this.label3.Text = "Period 3:";
            this.numericUpDown1.Location = new Point(0x58, 30);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x3f, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown2.Location = new Point(0x58, 0x36);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x3f, 20);
            this.numericUpDown2.TabIndex = 4;
            this.numericUpDown2.TextChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown3.Location = new Point(0x58, 0x4d);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new Size(0x3f, 20);
            this.numericUpDown3.TabIndex = 5;
            this.numericUpDown3.TextChanged += new EventHandler(this.numericUpDown3_ValueChanged);
            this.numericUpDown3.ValueChanged += new EventHandler(this.numericUpDown3_ValueChanged);
            this.bHistogram.FlatStyle = FlatStyle.Flat;
            this.bHistogram.Location = new Point(0xa8, 0x1c);
            this.bHistogram.Name = "bHistogram";
            this.bHistogram.Size = new Size(0x57, 0x17);
            this.bHistogram.TabIndex = 6;
            this.bHistogram.Text = "&Histogram...";
            this.bHistogram.Click += new EventHandler(this.bHistogram_Click);
            this.bMACD.FlatStyle = FlatStyle.Flat;
            this.bMACD.Location = new Point(0xa8, 0x34);
            this.bMACD.Name = "bMACD";
            this.bMACD.Size = new Size(0x57, 0x17);
            this.bMACD.TabIndex = 7;
            this.bMACD.Text = "&MACD...";
            this.bMACD.Click += new EventHandler(this.bMACD_Click);
            this.bMACDExp.FlatStyle = FlatStyle.Flat;
            this.bMACDExp.Location = new Point(0xa8, 0x4c);
            this.bMACDExp.Name = "bMACDExp";
            this.bMACDExp.Size = new Size(0x57, 0x17);
            this.bMACDExp.TabIndex = 8;
            this.bMACDExp.Text = "&Exp Line...";
            this.bMACDExp.Click += new EventHandler(this.bMACDExp_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x108, 0x74);
            base.Controls.Add(this.bMACDExp);
            base.Controls.Add(this.bMACD);
            base.Controls.Add(this.bHistogram);
            base.Controls.Add(this.numericUpDown3);
            base.Controls.Add(this.numericUpDown2);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Name = "MACDFunctionEditor";
            this.Text = "MACDFunctionEditor";
            this.numericUpDown1.EndInit();
            this.numericUpDown2.EndInit();
            this.numericUpDown3.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.function.Period = Convert.ToDouble(this.numericUpDown1.Value);
            this.Changed();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.function.Period2 = Convert.ToDouble(this.numericUpDown2.Value);
            this.Changed();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.function.Period3 = Convert.ToDouble(this.numericUpDown3.Value);
            this.Changed();
        }
    }
}

