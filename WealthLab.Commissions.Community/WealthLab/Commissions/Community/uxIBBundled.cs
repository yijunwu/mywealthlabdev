namespace WealthLab.Commissions.Community
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class uxIBBundled : UserControl
    {
        private double _maxPct = 0.5;
        private double _minCommish = 1.0;
        private double _perContract = 2.4;
        private double _perShare = 0.005;
        private IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtMax;
        private TextBox txtMin;
        private TextBox txtPerContract;
        private TextBox txtPerShare;

        public uxIBBundled()
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
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.txtPerShare = new TextBox();
            this.txtMin = new TextBox();
            this.txtMax = new TextBox();
            this.txtPerContract = new TextBox();
            this.groupBox1 = new GroupBox();
            this.groupBox2 = new GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(4, 0x33);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Min Per Order ($)";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(4, 0x17);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Per Share ($)";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(4, 0x4f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x75, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Max (% of Trade Value)";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(5, 0x11);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x51, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Per Contract ($)";
            this.txtPerShare.Location = new Point(0x7f, 0x17);
            this.txtPerShare.Name = "txtPerShare";
            this.txtPerShare.Size = new Size(0x36, 20);
            this.txtPerShare.TabIndex = 4;
            this.txtPerShare.Text = "0.123";
            this.txtPerShare.TextChanged += new EventHandler(this.txtPerShare_TextChanged);
            this.txtMin.Location = new Point(0x7f, 0x33);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new Size(0x36, 20);
            this.txtMin.TabIndex = 5;
            this.txtMin.Text = "0.123";
            this.txtMin.TextChanged += new EventHandler(this.txtMin_TextChanged);
            this.txtMax.Location = new Point(0x7f, 0x4f);
            this.txtMax.Name = "txtMax";
            this.txtMax.Size = new Size(0x36, 20);
            this.txtMax.TabIndex = 6;
            this.txtMax.Text = "0.123";
            this.txtMax.TextChanged += new EventHandler(this.txtMax_TextChanged);
            this.txtPerContract.Location = new Point(0x80, 0x11);
            this.txtPerContract.Name = "txtPerContract";
            this.txtPerContract.Size = new Size(0x36, 20);
            this.txtPerContract.TabIndex = 7;
            this.txtPerContract.Text = "1.23";
            this.txtPerContract.TextChanged += new EventHandler(this.txtPerContract_TextChanged);
            this.groupBox1.Controls.Add(this.txtPerContract);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new Point(0x12, 0x7c);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xbf, 0x30);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Futures";
            this.groupBox2.Controls.Add(this.txtMax);
            this.groupBox2.Controls.Add(this.txtMin);
            this.groupBox2.Controls.Add(this.txtPerShare);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new Point(0x12, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xbd, 0x73);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stocks";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Name = "uxIBBundled";
            base.Size = new Size(0xe2, 0xb9);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
        }

        private void txtMax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this._maxPct = Math.Abs(Convert.ToDouble(this.txtMax.Text));
            }
            catch
            {
                MessageBox.Show("Invalid value for commissions", "Maximum % of Trade Value");
                this.txtMax.Text = this._maxPct.ToString("0.0#######");
            }
        }

        private void txtMin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this._minCommish = Math.Abs(Convert.ToDouble(this.txtMin.Text));
            }
            catch
            {
                MessageBox.Show("Invalid value for commissions", "Minimum Per Order");
                this.txtMin.Text = this._minCommish.ToString("0.00######");
            }
        }

        private void txtPerContract_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this._perContract = Math.Abs(Convert.ToDouble(this.txtPerContract.Text));
            }
            catch
            {
                MessageBox.Show("Invalid value for commissions", "Per Contract Commissions");
                this.txtPerContract.Text = this._perContract.ToString("0.00######");
            }
        }

        private void txtPerShare_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this._perShare = Math.Abs(Convert.ToDouble(this.txtPerShare.Text));
            }
            catch
            {
                MessageBox.Show("Invalid value for commissions", "Per Share Commissions");
                this.txtPerShare.Text = this._perShare.ToString("0.00######");
            }
        }

        public double MaxPct
        {
            get
            {
                return this._maxPct;
            }
            set
            {
                if ((this._maxPct != value) && (value >= 0.0))
                {
                    this._maxPct = value;
                }
                this.txtMax.Text = this._maxPct.ToString("0.0#######");
                base.Invalidate();
            }
        }

        public double MinCommish
        {
            get
            {
                return this._minCommish;
            }
            set
            {
                if ((this._minCommish != value) && (value >= 0.0))
                {
                    this._minCommish = value;
                }
                this.txtMin.Text = this._minCommish.ToString("0.00######");
                base.Invalidate();
            }
        }

        public double PerContract
        {
            get
            {
                return this._perContract;
            }
            set
            {
                if ((this._perContract != value) && (value >= 0.0))
                {
                    this._perContract = value;
                }
                this.txtPerContract.Text = this._perContract.ToString("0.00######");
                base.Invalidate();
            }
        }

        public double PerShare
        {
            get
            {
                return this._perShare;
            }
            set
            {
                if ((this._perShare != value) && (value >= 0.0))
                {
                    this._perShare = value;
                }
                this.txtPerShare.Text = this._perShare.ToString("0.00######");
                base.Invalidate();
            }
        }
    }
}

