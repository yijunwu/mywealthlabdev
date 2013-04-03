namespace WealthLab.PosSizers
{
    using CtrlLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class PosSizerSettingsBase : UserControl
    {
        private IContainer icontainer_0;
        protected Label lblDesc;
        protected NumEdit numFixedDollar;
        protected NumEdit numMaxRisk;
        protected NumEdit numPctEquity;
        protected RadioButton rbFixedDollar;
        protected RadioButton rbMaxRisk;
        protected RadioButton rbPctEquity;

        public PosSizerSettingsBase()
        {
            this.InitializeComponent();
        }

        public PosSizerSettingsBase(string description)
        {
            this.InitializeComponent();
            if (!string.IsNullOrEmpty(description))
            {
                this.lblDesc.Text = description;
            }
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
            this.rbFixedDollar = new RadioButton();
            this.rbPctEquity = new RadioButton();
            this.rbMaxRisk = new RadioButton();
            this.numFixedDollar = new NumEdit();
            this.numPctEquity = new NumEdit();
            this.numMaxRisk = new NumEdit();
            base.SuspendLayout();
            this.lblDesc.Location = new Point(4, 4);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new Size(0xdb, 0x3f);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "Select the basic Position Sizing method to use below:";
            this.rbFixedDollar.AutoSize = true;
            this.rbFixedDollar.Checked = true;
            this.rbFixedDollar.Location = new Point(7, 70);
            this.rbFixedDollar.Name = "rbFixedDollar";
            this.rbFixedDollar.Size = new Size(80, 0x11);
            this.rbFixedDollar.TabIndex = 1;
            this.rbFixedDollar.TabStop = true;
            this.rbFixedDollar.Text = "Fixed Dollar";
            this.rbFixedDollar.UseVisualStyleBackColor = true;
            this.rbPctEquity.AutoSize = true;
            this.rbPctEquity.Location = new Point(7, 0x61);
            this.rbPctEquity.Name = "rbPctEquity";
            this.rbPctEquity.Size = new Size(0x55, 0x11);
            this.rbPctEquity.TabIndex = 2;
            this.rbPctEquity.Text = "Pct of Equity";
            this.rbPctEquity.UseVisualStyleBackColor = true;
            this.rbMaxRisk.AutoSize = true;
            this.rbMaxRisk.Location = new Point(7, 0x7b);
            this.rbMaxRisk.Name = "rbMaxRisk";
            this.rbMaxRisk.Size = new Size(0x58, 0x11);
            this.rbMaxRisk.TabIndex = 3;
            this.rbMaxRisk.Text = "Max Risk Pct";
            this.rbMaxRisk.UseVisualStyleBackColor = true;
            this.numFixedDollar.InputType = NumEdit.NumEditType.Double;
            this.numFixedDollar.Location = new Point(0x62, 70);
            this.numFixedDollar.Name = "numFixedDollar";
            this.numFixedDollar.Size = new Size(100, 20);
            this.numFixedDollar.TabIndex = 4;
            this.numFixedDollar.Text = "5000.00";
            this.numFixedDollar.Enter += new EventHandler(this.numFixedDollar_Enter);
            this.numPctEquity.InputType = NumEdit.NumEditType.Double;
            this.numPctEquity.Location = new Point(0x62, 0x60);
            this.numPctEquity.Name = "numPctEquity";
            this.numPctEquity.Size = new Size(100, 20);
            this.numPctEquity.TabIndex = 5;
            this.numPctEquity.Text = "5";
            this.numPctEquity.Enter += new EventHandler(this.numPctEquity_Enter);
            this.numMaxRisk.InputType = NumEdit.NumEditType.Double;
            this.numMaxRisk.Location = new Point(0x62, 0x7a);
            this.numMaxRisk.Name = "numMaxRisk";
            this.numMaxRisk.Size = new Size(100, 20);
            this.numMaxRisk.TabIndex = 6;
            this.numMaxRisk.Text = "2";
            this.numMaxRisk.Enter += new EventHandler(this.numMaxRisk_Enter);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            base.Controls.Add(this.numMaxRisk);
            base.Controls.Add(this.numPctEquity);
            base.Controls.Add(this.numFixedDollar);
            base.Controls.Add(this.rbMaxRisk);
            base.Controls.Add(this.rbPctEquity);
            base.Controls.Add(this.rbFixedDollar);
            base.Controls.Add(this.lblDesc);
            base.Name = "PosSizerSettingsBase";
            base.Size = new Size(0xe9, 0x9a);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void numFixedDollar_Enter(object sender, EventArgs e)
        {
            this.rbFixedDollar.Checked = true;
        }

        private void numMaxRisk_Enter(object sender, EventArgs e)
        {
            this.rbMaxRisk.Checked = true;
        }

        private void numPctEquity_Enter(object sender, EventArgs e)
        {
            this.rbPctEquity.Checked = true;
        }

        public double FixedDollarSize
        {
            get
            {
                return (double) this.numFixedDollar.Value;
            }
            set
            {
                this.numFixedDollar.Text = value.ToString();
            }
        }

        public double MaxRiskSize
        {
            get
            {
                return (double) this.numMaxRisk.Value;
            }
            set
            {
                this.numMaxRisk.Text = value.ToString();
            }
        }

        public double PctEquitySize
        {
            get
            {
                return (double) this.numPctEquity.Value;
            }
            set
            {
                this.numPctEquity.Text = value.ToString();
            }
        }

        public double PositionSizeAmount
        {
            get
            {
                switch (this.PosSizeMode)
                {
                    case WealthLab.PosSizeMode.Dollar:
                        return this.FixedDollarSize;

                    case WealthLab.PosSizeMode.PctEquity:
                        return this.PctEquitySize;

                    case WealthLab.PosSizeMode.MaxRisk:
                        return this.MaxRiskSize;
                }
                throw new ArgumentException("Invalid PosSizeMode: " + this.PosSizeMode);
            }
        }

        public WealthLab.PosSizeMode PosSizeMode
        {
            get
            {
                if (this.rbFixedDollar.Checked)
                {
                    return WealthLab.PosSizeMode.Dollar;
                }
                if (this.rbPctEquity.Checked)
                {
                    return WealthLab.PosSizeMode.PctEquity;
                }
                return WealthLab.PosSizeMode.MaxRisk;
            }
            set
            {
                switch (value)
                {
                    case WealthLab.PosSizeMode.Dollar:
                        this.rbFixedDollar.Checked = true;
                        return;

                    case WealthLab.PosSizeMode.Share:
                        break;

                    case WealthLab.PosSizeMode.PctEquity:
                        this.rbPctEquity.Checked = true;
                        return;

                    case WealthLab.PosSizeMode.MaxRisk:
                        this.rbMaxRisk.Checked = true;
                        break;

                    default:
                        return;
                }
            }
        }
    }
}

