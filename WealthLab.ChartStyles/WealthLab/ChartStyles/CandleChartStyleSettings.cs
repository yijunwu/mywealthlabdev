namespace WealthLab.ChartStyles
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class CandleChartStyleSettings : UserControl
    {
        internal CheckBox cbDrawOutline;
        internal CheckBox cbFillCandleSticks;
        private IContainer icontainer_0;

        public CandleChartStyleSettings()
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
            this.cbFillCandleSticks = new CheckBox();
            this.cbDrawOutline = new CheckBox();
            base.SuspendLayout();
            this.cbFillCandleSticks.AutoSize = true;
            this.cbFillCandleSticks.Location = new Point(3, 3);
            this.cbFillCandleSticks.Name = "cbFillCandleSticks";
            this.cbFillCandleSticks.Size = new Size(0x67, 0x11);
            this.cbFillCandleSticks.TabIndex = 0;
            this.cbFillCandleSticks.Text = "Fill CandleSticks";
            this.cbFillCandleSticks.UseVisualStyleBackColor = true;
            this.cbDrawOutline.AutoSize = true;
            this.cbDrawOutline.Location = new Point(4, 0x1b);
            this.cbDrawOutline.Name = "cbDrawOutline";
            this.cbDrawOutline.Size = new Size(0x5c, 0x11);
            this.cbDrawOutline.TabIndex = 1;
            this.cbDrawOutline.Text = "Draw Outlines";
            this.cbDrawOutline.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.cbDrawOutline);
            base.Controls.Add(this.cbFillCandleSticks);
            base.Name = "CandleChartStyleSettings";
            base.Size = new Size(0x77, 0x33);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

