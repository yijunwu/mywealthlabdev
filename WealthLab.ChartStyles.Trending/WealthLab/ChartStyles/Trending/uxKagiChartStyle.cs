namespace WealthLab.ChartStyles.Trending
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class uxKagiChartStyle : UserControl
    {
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private double double_0 = 5.0;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private IContainer icontainer_0;
        private int int_0 = 5;
        private int int_1 = 4;
        private KagiReverseType kagiReverseType_0 = KagiReverseType.Percent;
        private Label label1;
        private Label lblATRPeriod;
        private NumericUpDown numericUpDown1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private TextBox tbPeriod;
        private TextBox textBox1;
        private CheckBox uxDrawOnHLC;
        private CheckBox uxOneColorOnly;
        private CheckBox uxReversalArrows;

        public uxKagiChartStyle()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.lblATRPeriod = new Label();
            this.tbPeriod = new TextBox();
            this.radioButton3 = new RadioButton();
            this.textBox1 = new TextBox();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.groupBox2 = new GroupBox();
            this.uxDrawOnHLC = new CheckBox();
            this.uxOneColorOnly = new CheckBox();
            this.label1 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.uxReversalArrows = new CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.numericUpDown1.BeginInit();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.lblATRPeriod);
            this.groupBox1.Controls.Add(this.tbPeriod);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x143, 0x48);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kagi Reversal Amount";
            this.lblATRPeriod.AutoSize = true;
            this.lblATRPeriod.Location = new Point(0xcb, 0x31);
            this.lblATRPeriod.Name = "lblATRPeriod";
            this.lblATRPeriod.Size = new Size(0x41, 13);
            this.lblATRPeriod.TabIndex = 6;
            this.lblATRPeriod.Text = "ATR Period:";
            this.lblATRPeriod.Visible = false;
            this.lblATRPeriod.Click += new EventHandler(this.lblATRPeriod_Click);
            this.tbPeriod.Location = new Point(0x112, 0x2e);
            this.tbPeriod.Name = "tbPeriod";
            this.tbPeriod.Size = new Size(0x22, 20);
            this.tbPeriod.TabIndex = 4;
            this.tbPeriod.Text = "5";
            this.tbPeriod.Visible = false;
            this.tbPeriod.TextChanged += new EventHandler(this.tbPeriod_TextChanged);
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new Point(0xce, 0x16);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x2f, 0x11);
            this.radioButton3.TabIndex = 3;
            this.radioButton3.Text = "ATR";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.textBox1.Location = new Point(6, 0x13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0x2f, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "1.0";
            this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new Point(60, 0x16);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(80, 0x11);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Percentage";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new Point(0x91, 0x16);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x36, 0x11);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.Text = "Points";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.groupBox2.Controls.Add(this.uxReversalArrows);
            this.groupBox2.Controls.Add(this.uxDrawOnHLC);
            this.groupBox2.Controls.Add(this.uxOneColorOnly);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Location = new Point(7, 0x55);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x143, 0x7a);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rendering Options";
            this.uxDrawOnHLC.AutoSize = true;
            this.uxDrawOnHLC.Location = new Point(6, 0x48);
            this.uxDrawOnHLC.Name = "uxDrawOnHLC";
            this.uxDrawOnHLC.Size = new Size(0x71, 0x11);
            this.uxDrawOnHLC.TabIndex = 7;
            this.uxDrawOnHLC.Text = "Overlay HLC chart";
            this.uxDrawOnHLC.UseVisualStyleBackColor = true;
            this.uxDrawOnHLC.CheckedChanged += new EventHandler(this.uxDrawOnHLC_CheckedChanged);
            this.uxOneColorOnly.AutoSize = true;
            this.uxOneColorOnly.Location = new Point(7, 0x31);
            this.uxOneColorOnly.Name = "uxOneColorOnly";
            this.uxOneColorOnly.Size = new Size(0xb3, 0x11);
            this.uxOneColorOnly.TabIndex = 6;
            this.uxOneColorOnly.Text = "Use one line color (Up Bar color)";
            this.uxOneColorOnly.UseVisualStyleBackColor = true;
            this.uxOneColorOnly.CheckedChanged += new EventHandler(this.uxOneColorOnly_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(60, 0x15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x6f, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Yin Line Width (pixels)";
            this.numericUpDown1.Location = new Point(6, 0x13);
            int[] bits = new int[4];
            bits[0] = 8;
            this.numericUpDown1.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numericUpDown1.Minimum = new decimal(numArray2);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x2f, 20);
            this.numericUpDown1.TabIndex = 4;
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.numericUpDown1.Value = new decimal(numArray3);
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.uxReversalArrows.AutoSize = true;
            this.uxReversalArrows.Location = new Point(0x1b, 0x5f);
            this.uxReversalArrows.Name = "uxReversalArrows";
            this.uxReversalArrows.Size = new Size(0x85, 0x11);
            this.uxReversalArrows.TabIndex = 8;
            this.uxReversalArrows.Text = "Show Reversal Arrows";
            this.uxReversalArrows.UseVisualStyleBackColor = true;
            this.uxReversalArrows.Visible = false;
            this.uxReversalArrows.CheckedChanged += new EventHandler(this.uxReversalArrows_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Name = "uxKagiChartStyle";
            base.Size = new Size(0x153, 0xd7);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.numericUpDown1.EndInit();
            base.ResumeLayout(false);
        }

        private void lblATRPeriod_Click(object sender, EventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.int_1 = (int) this.numericUpDown1.Value;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.kagiReverseType_0 = KagiReverseType.Points;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                this.kagiReverseType_0 = KagiReverseType.Percent;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.tbPeriod.Visible = this.radioButton3.Checked;
            this.lblATRPeriod.Visible = this.radioButton3.Checked;
            if (this.radioButton3.Checked)
            {
                this.kagiReverseType_0 = KagiReverseType.ATR;
            }
        }

        private void tbPeriod_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.KagiATRPeriod = Math.Abs(Convert.ToInt32(this.tbPeriod.Text));
            }
            catch
            {
                MessageBox.Show("Invalid ATR Period - Enter an integer");
                this.tbPeriod.Text = this.KagiATRPeriod.ToString("0");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.double_0 = Math.Abs(Convert.ToDouble(this.textBox1.Text));
            }
            catch
            {
                MessageBox.Show("Invalid reversal amount");
                this.textBox1.Text = this.double_0.ToString("0.0######");
            }
        }

        private void uxDrawOnHLC_CheckedChanged(object sender, EventArgs e)
        {
            this.bool_1 = this.uxDrawOnHLC.Checked;
            this.uxReversalArrows.Visible = this.bool_1;
        }

        private void uxOneColorOnly_CheckedChanged(object sender, EventArgs e)
        {
            this.bool_0 = this.uxOneColorOnly.Checked;
        }

        private void uxReversalArrows_CheckedChanged(object sender, EventArgs e)
        {
            this.bool_2 = this.uxReversalArrows.Checked;
        }

        public bool DrawHLC
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
                this.uxDrawOnHLC.Checked = value;
            }
        }

        public int KagiATRPeriod
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
                this.tbPeriod.Text = value.ToString("0");
                base.Invalidate();
            }
        }

        public int KagiLineWidth
        {
            get
            {
                return this.int_1;
            }
            set
            {
                if ((this.int_1 != value) & (value > 0))
                {
                    this.int_1 = value;
                    this.numericUpDown1.Value = this.int_1;
                    base.Invalidate();
                }
            }
        }

        public bool KagiOneColor
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                if (this.bool_0 != value)
                {
                    this.bool_0 = value;
                }
                this.uxOneColorOnly.Checked = value;
                base.Invalidate();
            }
        }

        public double KagiReversalAmount
        {
            get
            {
                return this.double_0;
            }
            set
            {
                if ((this.double_0 != value) & (value > 0.0))
                {
                    this.double_0 = value;
                }
                this.textBox1.Text = this.double_0.ToString("0.0######");
                base.Invalidate();
            }
        }

        public KagiReverseType KagiRevType
        {
            get
            {
                return this.kagiReverseType_0;
            }
            set
            {
                this.kagiReverseType_0 = value;
                switch (value)
                {
                    case KagiReverseType.Points:
                        this.radioButton1.Checked = true;
                        break;

                    case KagiReverseType.Percent:
                        this.radioButton2.Checked = true;
                        break;

                    case KagiReverseType.ATR:
                        this.radioButton3.Checked = true;
                        break;
                }
                base.Invalidate();
            }
        }

        public bool ShowArrows
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
                this.uxReversalArrows.Checked = value;
            }
        }
    }
}

