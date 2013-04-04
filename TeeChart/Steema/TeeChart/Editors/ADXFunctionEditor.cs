namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Functions;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ADXFunctionEditor : Form
    {
        private ButtonPen BDown;
        private ButtonPen BUp;
        private Container components;
        internal Control controlToEnable;
        private ADXFunction function;
        private Label label1;
        private NumericUpDown UDPeriod;

        public ADXFunctionEditor()
        {
            this.InitializeComponent();
        }

        public ADXFunctionEditor(Function f) : this()
        {
            this.function = (ADXFunction) f;
            this.UDPeriod.Value = Convert.ToDecimal(this.function.Period);
            this.BUp.Pen = this.function.UpLinePen;
            this.BDown.Pen = this.function.DownLinePen;
        }

        private void BDown_Click(object sender, EventArgs e)
        {
            this.function.DownLinePen = this.BDown.Pen;
            this.Changed();
        }

        private void BUp_Click(object sender, EventArgs e)
        {
            this.function.UpLinePen = this.BUp.Pen;
            this.Changed();
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
            this.UDPeriod = new NumericUpDown();
            this.BUp = new ButtonPen();
            this.BDown = new ButtonPen();
            this.UDPeriod.BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x10, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Period:";
            this.UDPeriod.Location = new Point(0x44, 0x16);
            this.UDPeriod.Name = "UDPeriod";
            this.UDPeriod.Size = new Size(0x44, 20);
            this.UDPeriod.TabIndex = 1;
            this.UDPeriod.TextChanged += new EventHandler(this.UDPeriod_ValueChanged);
            this.UDPeriod.ValueChanged += new EventHandler(this.UDPeriod_ValueChanged);
            this.BUp.FlatStyle = FlatStyle.Flat;
            this.BUp.Location = new Point(0x45, 0x38);
            this.BUp.Name = "BUp";
            this.BUp.TabIndex = 2;
            this.BUp.Text = "&Up...";
            this.BUp.Click += new EventHandler(this.BUp_Click);
            this.BDown.FlatStyle = FlatStyle.Flat;
            this.BDown.Location = new Point(0x45, 0x57);
            this.BDown.Name = "BDown";
            this.BDown.TabIndex = 3;
            this.BDown.Text = "&Down...";
            this.BDown.Click += new EventHandler(this.BDown_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xf2, 0x7b);
            base.Controls.Add(this.BDown);
            base.Controls.Add(this.BUp);
            base.Controls.Add(this.UDPeriod);
            base.Controls.Add(this.label1);
            base.Name = "ADXFunctionEditor";
            this.Text = "ADXFunctionEditor";
            this.UDPeriod.EndInit();
            base.ResumeLayout(false);
        }

        private void UDPeriod_ValueChanged(object sender, EventArgs e)
        {
            this.function.Period = Convert.ToDouble(this.UDPeriod.Value);
            this.Changed();
        }
    }
}

