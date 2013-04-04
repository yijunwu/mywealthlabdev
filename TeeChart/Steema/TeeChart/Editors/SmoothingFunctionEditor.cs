namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Functions;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SmoothingFunctionEditor : Form
    {
        private CheckBox checkBox1;
        private Container components;
        internal Control controlToEnable;
        private Smoothing function;
        private Label label1;
        private NumericUpDown numericUpDown1;

        public SmoothingFunctionEditor()
        {
            this.InitializeComponent();
        }

        public SmoothingFunctionEditor(Function f) : this()
        {
            this.function = (Smoothing) f;
            this.numericUpDown1.Value = Convert.ToDecimal(this.function.Factor);
            this.checkBox1.Checked = this.function.Interpolate;
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
            this.function.Interpolate = this.checkBox1.Checked;
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
            this.numericUpDown1.BeginInit();
            base.SuspendLayout();
            this.label1.Location = new Point(40, 0x30);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 0x17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Factor:";
            this.numericUpDown1.Location = new Point(0x58, 0x30);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.numericUpDown1.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numericUpDown1.Minimum = new decimal(numArray2);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x40, 20);
            this.numericUpDown1.TabIndex = 1;
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.numericUpDown1.Value = new decimal(numArray3);
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = CheckState.Checked;
            this.checkBox1.Location = new Point(0x58, 0x10);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x60, 0x18);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Interpolate";
            this.checkBox1.Click += new EventHandler(this.checkBox1_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xf2, 0x7b);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.label1);
            base.Name = "SmoothingFunctionEditor";
            this.Text = "SmoothingFunctionEditor";
            this.numericUpDown1.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (this.function != null)
            {
                this.function.Factor = Convert.ToInt16(this.numericUpDown1.Value);
                this.Changed();
            }
        }
    }
}

