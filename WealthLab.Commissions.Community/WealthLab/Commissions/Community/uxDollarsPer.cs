namespace WealthLab.Commissions.Community
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class uxDollarsPer : UserControl
    {
        private double _commission = 0.01;
        private string _label = "Amount of commission to apply (per share)";
        private IContainer components;
        private System.Windows.Forms.Label lblCommission;
        private TextBox txtCommission;

        public uxDollarsPer()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblCommission = new System.Windows.Forms.Label();
            this.txtCommission = new TextBox();
            base.SuspendLayout();
            this.lblCommission.AutoSize = true;
            this.lblCommission.Location = new Point(4, 12);
            this.lblCommission.Name = "lblCommission";
            this.lblCommission.Size = new Size(11, 13);
            this.lblCommission.TabIndex = 0;
            this.lblCommission.Text = "*";
            this.txtCommission.Location = new Point(0x8d, 0x22);
            this.txtCommission.Name = "txtCommission";
            this.txtCommission.Size = new Size(0x40, 20);
            this.txtCommission.TabIndex = 1;
            this.txtCommission.Text = "0.01";
            this.txtCommission.TextChanged += new EventHandler(this.txtCommission_TextChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.txtCommission);
            base.Controls.Add(this.lblCommission);
            base.Name = "uxDollarsPer";
            base.Size = new Size(0x12a, 150);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void txtCommission_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this._commission = Math.Abs(Convert.ToDouble(this.txtCommission.Text));
            }
            catch
            {
                MessageBox.Show("Invalid value for commissions", "Commission Settings");
                this.txtCommission.Text = this._commission.ToString("0.0#######");
            }
        }

        public double Commission
        {
            get
            {
                return this._commission;
            }
            set
            {
                if ((this._commission != value) & (value > 0.0))
                {
                    this._commission = value;
                }
                this.txtCommission.Text = this._commission.ToString("0.0#######");
                base.Invalidate();
            }
        }

        public string Label
        {
            get
            {
                return this._label;
            }
            set
            {
                this._label = value;
                this.lblCommission.Text = this._label;
                base.Invalidate();
            }
        }
    }
}

