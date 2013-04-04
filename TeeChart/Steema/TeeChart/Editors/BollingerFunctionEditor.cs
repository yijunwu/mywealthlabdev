namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Functions;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BollingerFunctionEditor : Form
    {
        private ButtonPen buttonPen1;
        private ButtonPen buttonPen2;
        private CheckBox checkBox1;
        private Container components;
        internal Control controlToEnable;
        private Bollinger function;
        private Label label1;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private TextBox textBox1;

        public BollingerFunctionEditor()
        {
            this.InitializeComponent();
        }

        public BollingerFunctionEditor(Function f) : this()
        {
            this.function = (Bollinger) f;
            this.buttonPen1.Pen = this.function.LowBandPen;
            this.buttonPen2.Pen = this.function.UpperBandPen;
            this.numericUpDown1.Value = Convert.ToDecimal(this.function.Period);
            this.checkBox1.Checked = this.function.Exponential;
            this.textBox1.Text = this.function.Deviation.ToString();
        }

        private void buttonPen1_Click(object sender, EventArgs e)
        {
            this.function.LowBandPen = this.buttonPen1.Pen;
            this.Changed();
        }

        private void buttonPen2_Click(object sender, EventArgs e)
        {
            this.function.UpperBandPen = this.buttonPen2.Pen;
            this.Changed();
        }

        private void Changed()
        {
            if (this.controlToEnable != null)
            {
                this.controlToEnable.Enabled = true;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            this.function.Exponential = this.checkBox1.Checked;
            this.Changed();
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
            this.numericUpDown1 = new NumericUpDown();
            this.checkBox1 = new CheckBox();
            this.textBox1 = new TextBox();
            this.label2 = new Label();
            this.buttonPen1 = new ButtonPen();
            this.buttonPen2 = new ButtonPen();
            this.numericUpDown1.BeginInit();
            base.SuspendLayout();
            this.label1.Location = new Point(0x24, 0x1b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 0x17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Period:";
            this.numericUpDown1.Location = new Point(80, 0x18);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x40, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.checkBox1.Location = new Point(80, 0x30);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x60, 0x18);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Exponential";
            this.checkBox1.Click += new EventHandler(this.checkBox1_Click);
            this.textBox1.Location = new Point(80, 80);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0x40, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "";
            this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.label2.Location = new Point(20, 0x52);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x38, 0x17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Deviation:";
            this.buttonPen1.FlatStyle = FlatStyle.Flat;
            this.buttonPen1.Location = new Point(160, 0x4f);
            this.buttonPen1.Name = "buttonPen1";
            this.buttonPen1.TabIndex = 5;
            this.buttonPen1.Text = "Lower...";
            this.buttonPen1.Click += new EventHandler(this.buttonPen1_Click);
            this.buttonPen2.FlatStyle = FlatStyle.Flat;
            this.buttonPen2.Location = new Point(160, 0x18);
            this.buttonPen2.Name = "buttonPen2";
            this.buttonPen2.TabIndex = 6;
            this.buttonPen2.Text = "Upper...";
            this.buttonPen2.Click += new EventHandler(this.buttonPen2_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xf2, 0x7b);
            base.Controls.Add(this.buttonPen2);
            base.Controls.Add(this.buttonPen1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.label1);
            base.Name = "BollingerFunctionEditor";
            this.Text = "BollingerFunctionEditor";
            this.numericUpDown1.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.function.Period = Convert.ToDouble(this.numericUpDown1.Value);
            this.Changed();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            double num = 0.0;
            try
            {
                num = Utils.StringToDouble(this.textBox1.Text, 0.0);
            }
            finally
            {
                this.function.Deviation = num;
                this.Changed();
            }
        }
    }
}

