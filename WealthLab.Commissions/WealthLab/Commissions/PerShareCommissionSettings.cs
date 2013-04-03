namespace WealthLab.Commissions
{
    using CtrlLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PerShareCommissionSettings : UserControl
    {
        private IContainer icontainer_0;
        private Label lblPerShare;
        private NumEdit numPerShare;

        public PerShareCommissionSettings()
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
            this.lblPerShare = new Label();
            this.numPerShare = new NumEdit();
            base.SuspendLayout();
            this.lblPerShare.AutoSize = true;
            this.lblPerShare.Location = new Point(4, 4);
            this.lblPerShare.Name = "lblPerShare";
            this.lblPerShare.Size = new Size(0xf3, 13);
            this.lblPerShare.TabIndex = 0;
            this.lblPerShare.Text = "Amount of Commission to Apply per Share Traded:";
            this.numPerShare.InputType = NumEdit.NumEditType.Double;
            this.numPerShare.Location = new Point(170, 0x1d);
            this.numPerShare.Name = "numPerShare";
            this.numPerShare.Size = new Size(0x43, 20);
            this.numPerShare.TabIndex = 1;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.numPerShare);
            base.Controls.Add(this.lblPerShare);
            base.Name = "PerShareCommissionSettings";
            base.Size = new Size(0x105, 70);
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

