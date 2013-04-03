namespace WealthLab.PosSizers
{
    using CtrlLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PctWinnersPosSizingSettings : UserControl
    {
        private IContainer icontainer_0;
        private Label lblDesc;
        private Label lblMax;
        private Label lblMin;
        private Label lblPct;
        private NumEdit numMax;
        private NumEdit numMin;
        private NumEdit numPct;

        public PctWinnersPosSizingSettings()
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
            this.lblDesc = new Label();
            this.lblMin = new Label();
            this.numMin = new NumEdit();
            this.lblMax = new Label();
            this.numMax = new NumEdit();
            this.lblPct = new Label();
            this.numPct = new NumEdit();
            base.SuspendLayout();
            this.lblDesc.Location = new Point(4, 4);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new Size(0xdd, 0x3a);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "Fluctuates the Position Size between a minimum and maximum fixed dollar size based on the percentage of trades that were profitable to date.";
            this.lblMin.AutoSize = true;
            this.lblMin.Location = new Point(7, 0x4c);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new Size(0x5c, 13);
            this.lblMin.TabIndex = 1;
            this.lblMin.Text = "Minimum Pos Size";
            this.numMin.InputType = NumEdit.NumEditType.Double;
            this.numMin.Location = new Point(0x69, 0x49);
            this.numMin.Name = "numMin";
            this.numMin.Size = new Size(100, 20);
            this.numMin.TabIndex = 2;
            this.numMin.Text = "5000";
            this.lblMax.AutoSize = true;
            this.lblMax.Location = new Point(7, 0x66);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new Size(0x5f, 13);
            this.lblMax.TabIndex = 3;
            this.lblMax.Text = "Maximum Pos Size";
            this.numMax.InputType = NumEdit.NumEditType.Double;
            this.numMax.Location = new Point(0x69, 0x63);
            this.numMax.Name = "numMax";
            this.numMax.Size = new Size(100, 20);
            this.numMax.TabIndex = 4;
            this.numMax.Text = "20000";
            this.lblPct.Location = new Point(7, 0x85);
            this.lblPct.Name = "lblPct";
            this.lblPct.Size = new Size(0xc6, 0x21);
            this.lblPct.TabIndex = 5;
            this.lblPct.Text = "Maximum size will be used when winning percent greater than or equal to:";
            this.numPct.InputType = NumEdit.NumEditType.Double;
            this.numPct.Location = new Point(0x69, 0xa9);
            this.numPct.Name = "numPct";
            this.numPct.Size = new Size(100, 20);
            this.numPct.TabIndex = 6;
            this.numPct.Text = "75";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.numPct);
            base.Controls.Add(this.lblPct);
            base.Controls.Add(this.numMax);
            base.Controls.Add(this.lblMax);
            base.Controls.Add(this.numMin);
            base.Controls.Add(this.lblMin);
            base.Controls.Add(this.lblDesc);
            base.Name = "PctWinnersPosSizingSettings";
            base.Size = new Size(0xed, 0xd4);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public double MaxSize
        {
            get
            {
                return (double) this.numMax.Value;
            }
            set
            {
                this.numMax.Text = value.ToString();
            }
        }

        public double MaxTrigger
        {
            get
            {
                return (double) this.numPct.Value;
            }
            set
            {
                this.numPct.Text = value.ToString();
            }
        }

        public double MinSize
        {
            get
            {
                return (double) this.numMin.Value;
            }
            set
            {
                this.numMin.Text = value.ToString();
            }
        }
    }
}

