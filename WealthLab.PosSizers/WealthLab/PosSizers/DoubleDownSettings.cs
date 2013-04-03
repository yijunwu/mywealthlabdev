namespace WealthLab.PosSizers
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DoubleDownSettings : PosSizerSettingsBase
    {
        private IContainer icontainer_1;

        public DoubleDownSettings()
        {
            this.InitializeComponent_1();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_1 != null))
            {
                this.icontainer_1.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent_1()
        {
            base.SuspendLayout();
            base.lblDesc.Size = new Size(0xd5, 0x3f);
            base.lblDesc.Text = "This PosSizer doubles the Position Size as additional Positions are established.  Select the Position Size mode and initial Position Size below.";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            base.Name = "DoubleDownSettings";
            base.Size = new Size(0xe8, 0xb6);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

