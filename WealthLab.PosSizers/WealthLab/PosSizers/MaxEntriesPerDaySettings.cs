namespace WealthLab.PosSizers
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MaxEntriesPerDaySettings : PosSizerSettingsBase
    {
        private IContainer icontainer_1;
        private Label label2;
        private NumericUpDown numEntries;

        public MaxEntriesPerDaySettings()
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
            this.numEntries = new NumericUpDown();
            this.label2 = new Label();
            this.numEntries.BeginInit();
            base.SuspendLayout();
            base.lblDesc.Size = new Size(0xe2, 0x2f);
            base.lblDesc.Text = "Specify the maximum number of entries per day (bar) the Strategy will be allowed to take below:";
            base.rbFixedDollar.Location = new Point(6, 0x7e);
            base.rbPctEquity.Location = new Point(6, 0x99);
            base.rbMaxRisk.Location = new Point(6, 0xb3);
            base.numFixedDollar.Location = new Point(0x61, 0x7e);
            base.numPctEquity.Location = new Point(0x61, 0x98);
            base.numMaxRisk.Location = new Point(0x61, 0xb2);
            this.numEntries.Location = new Point(7, 0x36);
            int[] bits = new int[4];
            bits[0] = 0x270f;
            this.numEntries.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numEntries.Minimum = new decimal(numArray2);
            this.numEntries.Name = "numEntries";
            this.numEntries.Size = new Size(0x37, 20);
            this.numEntries.TabIndex = 1;
            int[] numArray3 = new int[4];
            numArray3[0] = 2;
            this.numEntries.Value = new decimal(numArray3);
            this.label2.Location = new Point(4, 0x58);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0xb6, 0x1d);
            this.label2.TabIndex = 8;
            this.label2.Text = "Specifiy the Position size of the Positions that are allowed:";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            base.Controls.Add(this.label2);
            base.Controls.Add(this.numEntries);
            base.Name = "MaxEntriesPerDaySettings";
            base.Size = new Size(0xf1, 0xd7);
            base.Controls.SetChildIndex(base.numFixedDollar, 0);
            base.Controls.SetChildIndex(base.numPctEquity, 0);
            base.Controls.SetChildIndex(base.rbMaxRisk, 0);
            base.Controls.SetChildIndex(base.rbPctEquity, 0);
            base.Controls.SetChildIndex(base.numMaxRisk, 0);
            base.Controls.SetChildIndex(base.rbFixedDollar, 0);
            base.Controls.SetChildIndex(this.numEntries, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(base.lblDesc, 0);
            this.numEntries.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public int MaxEntries
        {
            get
            {
                return (int) this.numEntries.Value;
            }
            set
            {
                this.numEntries.Value = value;
            }
        }
    }
}

