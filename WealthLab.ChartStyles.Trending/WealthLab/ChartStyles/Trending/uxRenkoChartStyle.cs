namespace WealthLab.ChartStyles.Trending
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class uxRenkoChartStyle : UserControl
    {
        private bool bool_0;
        private double double_0 = 1.0;
        private GroupBox groupBox1;
        private IContainer icontainer_0;
        private TextBox textBox1;
        private CheckBox uxDrawBoxes;

        public uxRenkoChartStyle()
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
            this.uxDrawBoxes = new CheckBox();
            this.textBox1 = new TextBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.uxDrawBoxes);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xa5, 0x47);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Renko Price Units";
            this.uxDrawBoxes.AutoSize = true;
            this.uxDrawBoxes.Location = new Point(6, 0x2c);
            this.uxDrawBoxes.Name = "uxDrawBoxes";
            this.uxDrawBoxes.Size = new Size(0x71, 0x11);
            this.uxDrawBoxes.TabIndex = 2;
            this.uxDrawBoxes.Text = "Overlay HLC chart";
            this.uxDrawBoxes.UseVisualStyleBackColor = true;
            this.uxDrawBoxes.CheckedChanged += new EventHandler(this.uxDrawBoxes_CheckedChanged);
            this.textBox1.Location = new Point(6, 0x12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0x2b, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "1.0";
            this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBox1);
            base.Name = "uxRenkoChartStyle";
            base.Size = new Size(0xc1, 0x54);
            base.Load += new EventHandler(this.uxRenkoChartStyle_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.double_0 = Math.Abs(Convert.ToDouble(this.textBox1.Text));
            }
            catch
            {
                MessageBox.Show("Invalid value for price units", "Renko Settings");
                this.textBox1.Text = this.double_0.ToString("0.0######");
            }
        }

        private void uxDrawBoxes_CheckedChanged(object sender, EventArgs e)
        {
            this.bool_0 = this.uxDrawBoxes.Checked;
        }

        private void uxRenkoChartStyle_Load(object sender, EventArgs e)
        {
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
                this.uxDrawBoxes.Checked = value;
            }
        }

        public double RenkoPriceUnits
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
    }
}

