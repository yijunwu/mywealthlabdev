namespace WealthLab.Optimizers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MonteCarloSettings : UserControl
    {
        private ComboBox cmbMetric;
        private IContainer components;
        private Label lblMetric;
        private Label lblRuns;
        private Label lblTests;
        private NumericUpDown numRuns;
        private NumericUpDown numTests;
        private RadioButton rbHighest;
        private RadioButton rbLowest;

        public MonteCarloSettings()
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
            this.lblMetric = new Label();
            this.cmbMetric = new ComboBox();
            this.rbHighest = new RadioButton();
            this.rbLowest = new RadioButton();
            this.lblRuns = new Label();
            this.numRuns = new NumericUpDown();
            this.lblTests = new Label();
            this.numTests = new NumericUpDown();
            this.numRuns.BeginInit();
            this.numTests.BeginInit();
            base.SuspendLayout();
            this.lblMetric.AutoSize = true;
            this.lblMetric.Location = new Point(4, 7);
            this.lblMetric.Name = "lblMetric";
            this.lblMetric.Size = new Size(0x5e, 13);
            this.lblMetric.TabIndex = 0;
            this.lblMetric.Text = "Metric to Optimize:";
            this.cmbMetric.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMetric.FormattingEnabled = true;
            this.cmbMetric.Location = new Point(0x68, 4);
            this.cmbMetric.Name = "cmbMetric";
            this.cmbMetric.Size = new Size(0x85, 0x15);
            this.cmbMetric.TabIndex = 1;
            this.rbHighest.AutoSize = true;
            this.rbHighest.Checked = true;
            this.rbHighest.Location = new Point(7, 0x1f);
            this.rbHighest.Name = "rbHighest";
            this.rbHighest.Size = new Size(0x5b, 0x11);
            this.rbHighest.TabIndex = 2;
            this.rbHighest.TabStop = true;
            this.rbHighest.Text = "Highest Value";
            this.rbHighest.UseVisualStyleBackColor = true;
            this.rbHighest.CheckedChanged += new EventHandler(this.rbHighest_CheckedChanged);
            this.rbLowest.AutoSize = true;
            this.rbLowest.Location = new Point(0x68, 0x1f);
            this.rbLowest.Name = "rbLowest";
            this.rbLowest.Size = new Size(0x59, 0x11);
            this.rbLowest.TabIndex = 3;
            this.rbLowest.Text = "Lowest Value";
            this.rbLowest.UseVisualStyleBackColor = true;
            this.rbLowest.CheckedChanged += new EventHandler(this.rbLowest_CheckedChanged);
            this.lblRuns.AutoSize = true;
            this.lblRuns.Location = new Point(4, 0x4e);
            this.lblRuns.Name = "lblRuns";
            this.lblRuns.Size = new Size(0x57, 13);
            this.lblRuns.TabIndex = 4;
            this.lblRuns.Text = "Number of Runs:";
            this.numRuns.Location = new Point(0xb3, 0x4c);
            int[] bits = new int[4];
            bits[0] = 0x2710;
            this.numRuns.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numRuns.Minimum = new decimal(numArray2);
            this.numRuns.Name = "numRuns";
            this.numRuns.Size = new Size(0x3a, 20);
            this.numRuns.TabIndex = 5;
            int[] numArray3 = new int[4];
            numArray3[0] = 20;
            this.numRuns.Value = new decimal(numArray3);
            this.lblTests.AutoSize = true;
            this.lblTests.Location = new Point(4, 0x68);
            this.lblTests.Name = "lblTests";
            this.lblTests.Size = new Size(0x95, 13);
            this.lblTests.TabIndex = 6;
            this.lblTests.Text = "Number of Tests in each Run:";
            this.numTests.Location = new Point(0xb3, 0x66);
            int[] numArray4 = new int[4];
            numArray4[0] = 0x2710;
            this.numTests.Maximum = new decimal(numArray4);
            int[] numArray5 = new int[4];
            numArray5[0] = 1;
            this.numTests.Minimum = new decimal(numArray5);
            this.numTests.Name = "numTests";
            this.numTests.Size = new Size(0x3a, 20);
            this.numTests.TabIndex = 7;
            int[] numArray6 = new int[4];
            numArray6[0] = 10;
            this.numTests.Value = new decimal(numArray6);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.numTests);
            base.Controls.Add(this.lblTests);
            base.Controls.Add(this.numRuns);
            base.Controls.Add(this.lblRuns);
            base.Controls.Add(this.rbLowest);
            base.Controls.Add(this.rbHighest);
            base.Controls.Add(this.cmbMetric);
            base.Controls.Add(this.lblMetric);
            base.Name = "MonteCarloSettings";
            base.Size = new Size(250, 0x85);
            this.numRuns.EndInit();
            this.numTests.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void PopulateMetric(IList<string> metrics)
        {
            foreach (string str in metrics)
            {
                this.cmbMetric.Items.Add(str);
            }
            if (this.cmbMetric.Items.Count > 0)
            {
                this.cmbMetric.SelectedIndex = 0;
            }
        }

        private void rbHighest_CheckedChanged(object sender, EventArgs e)
        {
            this.rbLowest.Checked = !this.rbHighest.Checked;
        }

        private void rbLowest_CheckedChanged(object sender, EventArgs e)
        {
            this.rbHighest.Checked = !this.rbLowest.Checked;
        }

        public bool HighestValue
        {
            get
            {
                return this.rbHighest.Checked;
            }
            set
            {
                this.rbHighest.Checked = value;
                this.rbLowest.Checked = !value;
            }
        }

        public string Metric
        {
            get
            {
                return this.cmbMetric.Text;
            }
            set
            {
                if (this.cmbMetric.Items.Contains(value))
                {
                    this.cmbMetric.SelectedIndex = this.cmbMetric.Items.IndexOf(value);
                }
                else
                {
                    this.cmbMetric.SelectedIndex = -1;
                }
            }
        }

        public int RunCount
        {
            get
            {
                return (int) this.numRuns.Value;
            }
            set
            {
                this.numRuns.Value = value;
            }
        }

        public int TestCount
        {
            get
            {
                return (int) this.numTests.Value;
            }
            set
            {
                this.numTests.Value = value;
            }
        }
    }
}

