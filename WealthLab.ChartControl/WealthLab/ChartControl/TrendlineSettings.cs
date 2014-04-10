namespace WealthLab.ChartControl
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TrendlineSettings : LineBasedObjectSettings
    {
        private CheckBox cbDisplayPercentageChange;
        private IContainer components;

        public TrendlineSettings()
        {
            this.method_0();
            this.cbDisplayPercentageChange = new CheckBox();
            base.grpOptions.SuspendLayout();
            base.SuspendLayout();
            base.Size = new Size(0xd1 + 60, 0x146);
            base.grpOptions.Controls.Add(this.cbDisplayPercentageChange);
            base.grpOptions.Size = new Size(0xc9 + 60, 0x77);
            base.grpName.Size = new Size(base.grpName.Size.Width + 60, base.grpName.Height);
            base.grpStyle.Size = new Size(base.grpStyle.Size.Width + 60, base.grpStyle.Height);
            this.cbDisplayPercentageChange.AutoSize = true;
            this.cbDisplayPercentageChange.Location = new Point(7, 0x5b);
            this.cbDisplayPercentageChange.Name = "cbDisplayPercentageChange";
            this.cbDisplayPercentageChange.Size = new Size(140, 0x11);
            this.cbDisplayPercentageChange.TabIndex = 3;
            this.cbDisplayPercentageChange.Text = "Display Percentage Change at end of Trendline";
            this.cbDisplayPercentageChange.UseVisualStyleBackColor = true;
            base.grpOptions.ResumeLayout(false);
            base.grpOptions.PerformLayout();
            base.ResumeLayout(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void method_0()
        {
            this.components = new Container();
        }

        public bool DisplayPercentageChange
        {
            get
            {
                return this.cbDisplayPercentageChange.Checked;
            }
            set
            {
                this.cbDisplayPercentageChange.Checked = value;
            }
        }
    }
}

