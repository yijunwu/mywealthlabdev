namespace WealthLab.ChartStyles.Trending
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class uxLineBreakChartStyle : UserControl
    {
        private bool bool_0;
        private GroupBox groupBox1;
        private IContainer icontainer_0;
        private int int_0 = 3;
        private CheckBox uxDrawOnHLC;
        private NumericUpDown uxLinesToBreak;

        public uxLineBreakChartStyle()
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
            this.uxDrawOnHLC = new CheckBox();
            this.uxLinesToBreak = new NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.uxLinesToBreak.BeginInit();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.uxDrawOnHLC);
            this.groupBox1.Controls.Add(this.uxLinesToBreak);
            this.groupBox1.Location = new Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xc6, 0x4b);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Number of Lines to Break";
            this.uxDrawOnHLC.AutoSize = true;
            this.uxDrawOnHLC.Location = new Point(6, 0x2d);
            this.uxDrawOnHLC.Name = "uxDrawOnHLC";
            this.uxDrawOnHLC.Size = new Size(0x71, 0x11);
            this.uxDrawOnHLC.TabIndex = 1;
            this.uxDrawOnHLC.Text = "Overlay HLC chart";
            this.uxDrawOnHLC.UseVisualStyleBackColor = true;
            this.uxDrawOnHLC.CheckedChanged += new EventHandler(this.uxDrawOnHLC_CheckedChanged);
            this.uxLinesToBreak.Location = new Point(6, 0x13);
            int[] bits = new int[4];
            bits[0] = 8;
            this.uxLinesToBreak.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 2;
            this.uxLinesToBreak.Minimum = new decimal(numArray2);
            this.uxLinesToBreak.Name = "uxLinesToBreak";
            this.uxLinesToBreak.Size = new Size(0x26, 20);
            this.uxLinesToBreak.TabIndex = 0;
            int[] numArray3 = new int[4];
            numArray3[0] = 3;
            this.uxLinesToBreak.Value = new decimal(numArray3);
            this.uxLinesToBreak.ValueChanged += new EventHandler(this.uxLinesToBreak_ValueChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBox1);
            base.Name = "uxLineBreakChartStyle";
            base.Size = new Size(0xd7, 0x59);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.uxLinesToBreak.EndInit();
            base.ResumeLayout(false);
        }

        private void uxDrawOnHLC_CheckedChanged(object sender, EventArgs e)
        {
            this.bool_0 = this.uxDrawOnHLC.Checked;
        }

        private void uxLinesToBreak_ValueChanged(object sender, EventArgs e)
        {
            this.int_0 = (int) this.uxLinesToBreak.Value;
        }

        public bool DrawHLC
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
                this.uxDrawOnHLC.Checked = value;
            }
        }

        public int LinesToBreak
        {
            get
            {
                return this.int_0;
            }
            set
            {
                if (this.int_0 != value)
                {
                    this.int_0 = value;
                }
                this.uxLinesToBreak.Value = this.int_0;
                base.Invalidate();
            }
        }
    }
}

