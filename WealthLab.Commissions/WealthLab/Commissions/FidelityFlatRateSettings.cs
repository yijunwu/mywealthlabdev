namespace WealthLab.Commissions
{
    using CtrlLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FidelityFlatRateSettings : UserControl
    {
        private IContainer icontainer_0;
        private Label lblPerShare;
        private NumEdit numPerShare;

        public FidelityFlatRateSettings()
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
            this.numPerShare = new NumEdit();
            this.lblPerShare = new Label();
            base.SuspendLayout();
            this.numPerShare.InputType = NumEdit.NumEditType.Double;
            this.numPerShare.Location = new Point(0x54, 0x1d);
            this.numPerShare.Name = "numPerShare";
            this.numPerShare.Size = new Size(0x43, 20);
            this.numPerShare.TabIndex = 3;
            this.lblPerShare.AutoSize = true;
            this.lblPerShare.Location = new Point(4, 4);
            this.lblPerShare.Name = "lblPerShare";
            this.lblPerShare.Size = new Size(0x9d, 13);
            this.lblPerShare.TabIndex = 2;
            this.lblPerShare.Text = "Amount of Commission to Apply:";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.numPerShare);
            base.Controls.Add(this.lblPerShare);
            base.Name = "FidelityFlatRateSettings";
            base.Size = new Size(0xaf, 70);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public double Value
        {
            get
            {
                return (double) this.numPerShare.Value;
            }
            set
            {
                this.numPerShare.Text = value.ToString();
            }
        }
    }
}

