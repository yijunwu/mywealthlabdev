namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;
    using RadioButton = System.Windows.Forms.RadioButton;

    public class BarDataRangeSelecterForm : Form
    {
        private bool bool_0;
        private bool bool_1 = true;
        private Button btnCancel;
        private Button btnOK;
        private ComboBox cmbRecent;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private IContainer icontainer_0;
        private Label lblTo;
        private NumericUpDown numBars;
        private NumericUpDown numRecent;
        private RadioButton rbAllData;
        private RadioButton rbDateRange;
        private RadioButton rbFixed;
        private RadioButton rbRecent;

        public BarDataRangeSelecterForm(bool streaming)
        {
            this.InitializeComponent();
            this.bool_0 = streaming;
            MainModule.Instance.HelpProvider.SetHelpNavigator(this, HelpNavigator.Topic);
            MainModule.Instance.HelpProvider.SetHelpKeyword(this, "data_range_control.htm");
        }

        private void BarDataRangeSelecterForm_Load(object sender, EventArgs e)
        {
            this.dtpTo.Enabled = this.rbDateRange.Checked && !this.bool_0;
            if (this.cmbRecent.SelectedIndex == -1)
            {
                this.cmbRecent.SelectedIndex = 0;
            }
        }

        private void BarDataRangeSelecterForm_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Navy, 3f);
            using (pen)
            {
                e.Graphics.DrawRectangle(pen, 0, 0, base.Width, base.Height);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.rbDateRange.Checked)
            {
                if (this.dtpFrom.Value >= this.dtpTo.Value)
                {
                    MessageBox.Show("Start Date must be before End Date");
                }
                else
                {
                    base.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        private void cmbRecent_Enter(object sender, EventArgs e)
        {
            this.rbRecent.Checked = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dtpTo_Enter(object sender, EventArgs e)
        {
            this.rbDateRange.Checked = true;
        }

        private void InitializeComponent()
        {
            this.rbAllData = new RadioButton();
            this.rbFixed = new RadioButton();
            this.rbRecent = new RadioButton();
            this.numBars = new NumericUpDown();
            this.numRecent = new NumericUpDown();
            this.cmbRecent = new ComboBox();
            this.rbDateRange = new RadioButton();
            this.dtpFrom = new DateTimePicker();
            this.lblTo = new Label();
            this.dtpTo = new DateTimePicker();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.numBars.BeginInit();
            this.numRecent.BeginInit();
            base.SuspendLayout();
            this.rbAllData.AutoSize = true;
            this.rbAllData.Checked = true;
            this.rbAllData.Location = new Point(4, 4);
            this.rbAllData.Name = "rbAllData";
            this.rbAllData.Size = new Size(0x3e, 0x11);
            this.rbAllData.TabIndex = 0;
            this.rbAllData.TabStop = true;
            this.rbAllData.Text = "All Data";
            this.rbAllData.UseVisualStyleBackColor = true;
            this.rbAllData.CheckedChanged += new EventHandler(this.rbDateRange_CheckedChanged);
            this.rbFixed.AutoSize = true;
            this.rbFixed.ForeColor = SystemColors.ControlText;
            this.rbFixed.Location = new Point(4, 0x18);
            this.rbFixed.Name = "rbFixed";
            this.rbFixed.Size = new Size(0x62, 0x11);
            this.rbFixed.TabIndex = 1;
            this.rbFixed.Text = "Number of Bars";
            this.rbFixed.UseVisualStyleBackColor = true;
            this.rbFixed.CheckedChanged += new EventHandler(this.rbDateRange_CheckedChanged);
            this.rbRecent.AutoSize = true;
            this.rbRecent.ForeColor = SystemColors.ControlText;
            this.rbRecent.Location = new Point(4, 0x2c);
            this.rbRecent.Name = "rbRecent";
            this.rbRecent.Size = new Size(0x56, 0x11);
            this.rbRecent.TabIndex = 2;
            this.rbRecent.Text = "Most Recent";
            this.rbRecent.UseVisualStyleBackColor = true;
            this.rbRecent.CheckedChanged += new EventHandler(this.rbDateRange_CheckedChanged);
            this.numBars.ForeColor = SystemColors.GrayText;
            int[] bits = new int[4];
            bits[0] = 100;
            this.numBars.Increment = new decimal(bits);
            this.numBars.Location = new Point(0x66, 0x18);
            int[] numArray2 = new int[4];
            numArray2[0] = 0xf4240;
            this.numBars.Maximum = new decimal(numArray2);
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.numBars.Minimum = new decimal(numArray3);
            this.numBars.Name = "numBars";
            this.numBars.Size = new Size(0x3e, 20);
            this.numBars.TabIndex = 3;
            int[] numArray4 = new int[4];
            numArray4[0] = 500;
            this.numBars.Value = new decimal(numArray4);
            this.numBars.Enter += new EventHandler(this.numBars_Enter);
            this.numBars.ValueChanged += new EventHandler(this.numBars_ValueChanged);
            this.numRecent.ForeColor = SystemColors.GrayText;
            this.numRecent.Location = new Point(0x66, 0x2c);
            int[] numArray5 = new int[4];
            numArray5[0] = 0x186a0;
            this.numRecent.Maximum = new decimal(numArray5);
            int[] numArray6 = new int[4];
            numArray6[0] = 1;
            this.numRecent.Minimum = new decimal(numArray6);
            this.numRecent.Name = "numRecent";
            this.numRecent.Size = new Size(0x3e, 20);
            this.numRecent.TabIndex = 4;
            int[] numArray7 = new int[4];
            numArray7[0] = 10;
            this.numRecent.Value = new decimal(numArray7);
            this.numRecent.Enter += new EventHandler(this.cmbRecent_Enter);
            this.cmbRecent.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRecent.ForeColor = SystemColors.GrayText;
            this.cmbRecent.FormattingEnabled = true;
            this.cmbRecent.Items.AddRange(new object[] { "Years", "Months", "Weeks", "Days" });
            this.cmbRecent.Location = new Point(0xa8, 0x2c);
            this.cmbRecent.Name = "cmbRecent";
            this.cmbRecent.Size = new Size(0x3e, 0x15);
            this.cmbRecent.TabIndex = 5;
            this.cmbRecent.Enter += new EventHandler(this.cmbRecent_Enter);
            this.rbDateRange.AutoSize = true;
            this.rbDateRange.ForeColor = SystemColors.ControlText;
            this.rbDateRange.Location = new Point(4, 0x40);
            this.rbDateRange.Name = "rbDateRange";
            this.rbDateRange.Size = new Size(0x53, 0x11);
            this.rbDateRange.TabIndex = 6;
            this.rbDateRange.TabStop = true;
            this.rbDateRange.Text = "Date Range";
            this.rbDateRange.UseVisualStyleBackColor = true;
            this.rbDateRange.CheckedChanged += new EventHandler(this.rbDateRange_CheckedChanged);
            this.dtpFrom.Enabled = false;
            this.dtpFrom.Format = DateTimePickerFormat.Short;
            this.dtpFrom.Location = new Point(0x18, 0x57);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new Size(0x59, 20);
            this.dtpFrom.TabIndex = 7;
            this.dtpFrom.Enter += new EventHandler(this.dtpTo_Enter);
            this.lblTo.AutoSize = true;
            this.lblTo.ForeColor = SystemColors.GrayText;
            this.lblTo.Location = new Point(120, 0x5d);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new Size(0x10, 13);
            this.lblTo.TabIndex = 8;
            this.lblTo.Text = "to";
            this.dtpTo.Enabled = false;
            this.dtpTo.Format = DateTimePickerFormat.Short;
            this.dtpTo.Location = new Point(0x8f, 0x56);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new Size(0x59, 20);
            this.dtpTo.TabIndex = 9;
            this.dtpTo.Enter += new EventHandler(this.dtpTo_Enter);
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x9d, 0x7c);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Location = new Point(0x4c, 0x7c);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.AliceBlue;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(240, 0x9c);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.dtpTo);
            base.Controls.Add(this.lblTo);
            base.Controls.Add(this.dtpFrom);
            base.Controls.Add(this.rbDateRange);
            base.Controls.Add(this.cmbRecent);
            base.Controls.Add(this.numRecent);
            base.Controls.Add(this.numBars);
            base.Controls.Add(this.rbRecent);
            base.Controls.Add(this.rbFixed);
            base.Controls.Add(this.rbAllData);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "BarDataRangeSelecterForm";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "BarDataRangeSelecterForm";
            base.Paint += new PaintEventHandler(this.BarDataRangeSelecterForm_Paint);
            base.Load += new EventHandler(this.BarDataRangeSelecterForm_Load);
            this.numBars.EndInit();
            this.numRecent.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void numBars_Enter(object sender, EventArgs e)
        {
            if (this.bool_1)
            {
                this.bool_1 = false;
            }
            else
            {
                this.rbFixed.Checked = true;
            }
        }

        private void numBars_ValueChanged(object sender, EventArgs e)
        {
            if (!this.bool_1)
            {
                this.rbFixed.Checked = true;
            }
        }

        private void rbDateRange_CheckedChanged(object sender, EventArgs e)
        {
            Color color = Color.FromKnownColor(KnownColor.ControlText);
            Color color2 = Color.FromKnownColor(KnownColor.GrayText);
            this.numBars.ForeColor = this.rbFixed.Checked ? color : color2;
            this.numRecent.ForeColor = this.rbRecent.Checked ? color : color2;
            this.cmbRecent.ForeColor = this.numRecent.ForeColor;
            this.lblTo.ForeColor = this.rbDateRange.Checked ? color : color2;
            this.dtpFrom.Enabled = this.rbDateRange.Checked;
            this.dtpTo.Enabled = this.rbDateRange.Checked && !this.bool_0;
        }

        public DateTime EndDate
        {
            get
            {
                return this.dtpTo.Value;
            }
            set
            {
                this.dtpTo.Value = value;
            }
        }

        public int FixedBarsValue
        {
            get
            {
                return (int) this.numBars.Value;
            }
            set
            {
                this.numBars.Value = value;
            }
        }

        public BarRange Range
        {
            get
            {
                if (this.rbFixed.Checked)
                {
                    return BarRange.FixedBars;
                }
                if (this.rbRecent.Checked)
                {
                    switch (this.cmbRecent.SelectedIndex)
                    {
                        case 1:
                            return BarRange.RecentMonths;

                        case 2:
                            return BarRange.RecentWeeks;

                        case 3:
                            return BarRange.RecentDays;
                    }
                    return BarRange.RecentYears;
                }
                if (this.rbDateRange.Checked)
                {
                    return BarRange.DateRange;
                }
                return BarRange.AllData;
            }
            set
            {
                switch (value)
                {
                    case BarRange.AllData:
                        this.rbAllData.Checked = true;
                        return;

                    case BarRange.FixedBars:
                        this.rbFixed.Checked = true;
                        return;

                    case BarRange.RecentYears:
                        this.rbRecent.Checked = true;
                        this.cmbRecent.SelectedIndex = 0;
                        return;

                    case BarRange.RecentMonths:
                        this.rbRecent.Checked = true;
                        this.cmbRecent.SelectedIndex = 1;
                        return;

                    case BarRange.RecentWeeks:
                        this.rbRecent.Checked = true;
                        this.cmbRecent.SelectedIndex = 2;
                        return;

                    case BarRange.RecentDays:
                        this.rbRecent.Checked = true;
                        this.cmbRecent.SelectedIndex = 3;
                        return;

                    case BarRange.DateRange:
                        this.rbDateRange.Checked = true;
                        return;
                }
            }
        }

        public int RecentValue
        {
            get
            {
                return (int) this.numRecent.Value;
            }
            set
            {
                this.numRecent.Value = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.dtpFrom.Value;
            }
            set
            {
                this.dtpFrom.Value = value;
            }
        }
    }
}

