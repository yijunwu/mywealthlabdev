namespace WealthLab.ChartStyles
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class LineChartStyleSettings : UserControl
    {
        private IContainer icontainer_0;
        private Label lblWidth;
        internal NumericUpDown numWidth;

        public LineChartStyleSettings()
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
            this.lblWidth = new Label();
            this.numWidth = new NumericUpDown();
            this.numWidth.BeginInit();
            base.SuspendLayout();
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new Point(4, 4);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new Size(0x3d, 13);
            this.lblWidth.TabIndex = 0;
            this.lblWidth.Text = "Line Width:";
            this.numWidth.Location = new Point(0x48, 4);
            int[] bits = new int[4];
            bits[0] = 10;
            this.numWidth.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numWidth.Minimum = new decimal(numArray2);
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new Size(0x2c, 20);
            this.numWidth.TabIndex = 1;
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.numWidth.Value = new decimal(numArray3);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.numWidth);
            base.Controls.Add(this.lblWidth);
            base.Name = "LineChartStyleSettings";
            base.Size = new Size(0x84, 0x20);
            this.numWidth.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

