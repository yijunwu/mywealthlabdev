namespace WealthLab.PosSizers
{
    using CtrlLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AvgDownPctEquityLimitSettings : PosSizerSettingsBase
    {
        private IContainer icontainer_1;
        private Label label2;
        private NumEdit numMaxPctEquity;

        public AvgDownPctEquityLimitSettings()
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
            this.label2 = new Label();
            this.numMaxPctEquity = new NumEdit();
            base.SuspendLayout();
            base.lblDesc.Size = new Size(0xe2, 0x34);
            base.lblDesc.Text = "This PosSizer allows a Strategy to average down without exceeding a certain percentage of the overall equity during backtesting.";
            this.label2.Location = new Point(4, 0xa1);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x5b, 0x2e);
            this.label2.TabIndex = 8;
            this.label2.Text = "Maximum % of Equity for all Positions:";
            this.numMaxPctEquity.InputType = NumEdit.NumEditType.Double;
            this.numMaxPctEquity.Location = new Point(0x62, 0xa1);
            this.numMaxPctEquity.Name = "numMaxPctEquity";
            this.numMaxPctEquity.Size = new Size(100, 20);
            this.numMaxPctEquity.TabIndex = 9;
            this.numMaxPctEquity.Text = "25";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            base.Controls.Add(this.numMaxPctEquity);
            base.Controls.Add(this.label2);
            base.Name = "AvgDownPctEquityLimitSettings";
            base.Size = new Size(0x10b, 0xed);
            base.Controls.SetChildIndex(base.rbFixedDollar, 0);
            base.Controls.SetChildIndex(base.rbPctEquity, 0);
            base.Controls.SetChildIndex(base.rbMaxRisk, 0);
            base.Controls.SetChildIndex(base.numFixedDollar, 0);
            base.Controls.SetChildIndex(base.numPctEquity, 0);
            base.Controls.SetChildIndex(base.numMaxRisk, 0);
            base.Controls.SetChildIndex(base.lblDesc, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.numMaxPctEquity, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public double MaxPositionSize
        {
            get
            {
                return (double) this.numMaxPctEquity.Value;
            }
            set
            {
                this.numMaxPctEquity.Text = value.ToString();
            }
        }
    }
}

