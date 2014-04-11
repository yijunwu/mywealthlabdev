namespace WealthLab.ChartControl
{
    using CtrlLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Label = System.Windows.Forms.Label;

    public class DataEditor : Form
    {
        private NumEdit _close;
        private Label _closeLabel;
        private Label _dateLabel;
        private DateTimePicker _datePicker;
        private NumEdit _high;
        private Label _highLabel;
        private Label _labelTime;
        private NumEdit _low;
        private Label _lowLabel;
        private NumEdit _open;
        private Label _openLabel;
        private Label _timeLabel;
        private DateTimePicker _timePicker;
        private NumEdit _volume;
        private Label _volumeLabel;
        private Button cancelBtn;
        private GroupBox groupBox1;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label9;
        private Button okBtn;
        private Operation operation = Operation.Cancel;
        private Button removeBtn;

        public DataEditor(DateTime dateAndTime, double open, double high, double double_0, double close, double volume, bool bIntraday)
        {
            this.InitializeComponent();
            this._datePicker.Value = dateAndTime;
            this._timePicker.Value = dateAndTime;
            this._open.Text = open.ToString();
            this._high.Text = high.ToString();
            this._low.Text = double_0.ToString();
            this._close.Text = close.ToString();
            this._volume.Text = volume.ToString();
            this._dateLabel.Text = dateAndTime.ToShortDateString();
            this._timeLabel.Text = dateAndTime.ToShortTimeString();
            this._openLabel.Text = open.ToString();
            this._highLabel.Text = high.ToString();
            this._lowLabel.Text = double_0.ToString();
            this._closeLabel.Text = close.ToString();
            this._volumeLabel.Text = volume.ToString();
            if (bIntraday)
            {
                this._timePicker.Visible = true;
                this._timeLabel.Visible = true;
                this._labelTime.Visible = true;
            }
            else
            {
                this._timePicker.Visible = false;
                this._timeLabel.Visible = false;
                this._labelTime.Visible = false;
            }
        }

        private void _timePicker_ValueChanged(object sender, EventArgs e)
        {
            if (sender is NumEdit)
            {
                NumEdit edit = (NumEdit) sender;
                if (edit.Text.StartsWith("-"))
                {
                    edit.Text = edit.Text.Substring(1);
                }
            }
            if (base.Created)
            {
                this.removeBtn.Enabled = !this.method_0();
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.operation = Operation.Cancel;
        }

        private void DataEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.operation = Operation.Cancel;
            }
            if (this.operation == Operation.Ok)
            {
                if (this._open.Text.Length == 0)
                {
                    MessageBox.Show("You must supply a value for open.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this._open.Focus();
                    e.Cancel = true;
                    this.operation = Operation.Cancel;
                }
                else if (this._high.Text.Length == 0)
                {
                    MessageBox.Show("You must supply a value for High.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this._high.Focus();
                    e.Cancel = true;
                    this.operation = Operation.Cancel;
                }
                else if (this._low.Text.Length == 0)
                {
                    MessageBox.Show("You must supply a value for Low.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this._low.Focus();
                    e.Cancel = true;
                    this.operation = Operation.Cancel;
                }
                else if (this._close.Text.Length == 0)
                {
                    MessageBox.Show("You must supply a value for Close.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this._close.Focus();
                    e.Cancel = true;
                    this.operation = Operation.Cancel;
                }
                else if (this._volume.Text.Length == 0)
                {
                    MessageBox.Show("You must supply a value for Volume.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this._volume.Focus();
                    e.Cancel = true;
                    this.operation = Operation.Cancel;
                }
            }
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
            this.label5 = new Label();
            this.okBtn = new Button();
            this.removeBtn = new Button();
            this.cancelBtn = new Button();
            this._dateLabel = new Label();
            this._open = new NumEdit();
            this._high = new NumEdit();
            this._low = new NumEdit();
            this._close = new NumEdit();
            this._volume = new NumEdit();
            this._datePicker = new DateTimePicker();
            this.label7 = new Label();
            this._timePicker = new DateTimePicker();
            this._labelTime = new Label();
            this.label6 = new Label();
            this.label9 = new Label();
            this._timeLabel = new Label();
            this._openLabel = new Label();
            this._highLabel = new Label();
            this._lowLabel = new Label();
            this._closeLabel = new Label();
            this._volumeLabel = new Label();
            this.groupBox1 = new GroupBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x12, 0x1d);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x24, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Open:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x16, 0x38);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x20, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "High:";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x18, 0x53);
            this.label3.Name = "label3";
            this.label3.Size = new Size(30, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Low:";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x12, 110);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x24, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Close:";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(9, 0x89);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x2d, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Volume:";
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new Point(12, 0x101);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new Size(0x4b, 0x17);
            this.okBtn.TabIndex = 6;
            this.okBtn.Text = "Ok";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new EventHandler(this.okBtn_Click);
            this.removeBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.removeBtn.Location = new Point(100, 0x101);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new Size(0x4b, 0x17);
            this.removeBtn.TabIndex = 7;
            this.removeBtn.Text = "Remove";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new Point(0xbc, 0x101);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new Size(0x4b, 0x17);
            this.cancelBtn.TabIndex = 8;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
            this._dateLabel.BorderStyle = BorderStyle.Fixed3D;
            this._dateLabel.Location = new Point(0x2b, 20);
            this._dateLabel.Name = "_dateLabel";
            this._dateLabel.Size = new Size(0x60, 0x13);
            this._dateLabel.TabIndex = 15;
            this._dateLabel.Text = "09/02/2008";
            this._dateLabel.TextAlign = ContentAlignment.MiddleLeft;
            this._open.InputType = NumEdit.NumEditType.Double;
            this._open.Location = new Point(0x9b, 0x19);
            this._open.Name = "_open";
            this._open.Size = new Size(0x60, 20);
            this._open.TabIndex = 0;
            this._open.WordWrap = false;
            this._open.TextChanged += new EventHandler(this._timePicker_ValueChanged);
            this._high.InputType = NumEdit.NumEditType.Double;
            this._high.Location = new Point(0x9b, 0x34);
            this._high.Name = "_high";
            this._high.Size = new Size(0x60, 20);
            this._high.TabIndex = 1;
            this._high.TextChanged += new EventHandler(this._timePicker_ValueChanged);
            this._low.InputType = NumEdit.NumEditType.Double;
            this._low.Location = new Point(0x9b, 0x4f);
            this._low.Name = "_low";
            this._low.Size = new Size(0x60, 20);
            this._low.TabIndex = 2;
            this._low.TextChanged += new EventHandler(this._timePicker_ValueChanged);
            this._close.InputType = NumEdit.NumEditType.Double;
            this._close.Location = new Point(0x9b, 0x6a);
            this._close.Name = "_close";
            this._close.Size = new Size(0x60, 20);
            this._close.TabIndex = 3;
            this._close.WordWrap = false;
            this._close.TextChanged += new EventHandler(this._timePicker_ValueChanged);
            this._volume.InputType = NumEdit.NumEditType.Integer;
            this._volume.Location = new Point(0x9b, 0x85);
            this._volume.Name = "_volume";
            this._volume.Size = new Size(0x60, 20);
            this._volume.TabIndex = 4;
            this._volume.WordWrap = false;
            this._volume.TextChanged += new EventHandler(this._timePicker_ValueChanged);
            this._datePicker.Format = DateTimePickerFormat.Short;
            this._datePicker.Location = new Point(0x8f, 0x13);
            this._datePicker.Name = "_datePicker";
            this._datePicker.Size = new Size(0x60, 20);
            this._datePicker.TabIndex = 0;
            this._datePicker.ValueChanged += new EventHandler(this._timePicker_ValueChanged);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(9, 0x17);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x21, 13);
            this.label7.TabIndex = 0x12;
            this.label7.Text = "Date:";
            this._timePicker.CustomFormat = "hh:mm tt";
            this._timePicker.Format = DateTimePickerFormat.Custom;
            this._timePicker.Location = new Point(0x8f, 0x29);
            this._timePicker.Name = "_timePicker";
            this._timePicker.ShowUpDown = true;
            this._timePicker.Size = new Size(0x60, 20);
            this._timePicker.TabIndex = 1;
            this._timePicker.ValueChanged += new EventHandler(this._timePicker_ValueChanged);
            this._labelTime.AutoSize = true;
            this._labelTime.Location = new Point(9, 0x2d);
            this._labelTime.Name = "_labelTime";
            this._labelTime.Size = new Size(0x21, 13);
            this._labelTime.TabIndex = 20;
            this._labelTime.Text = "Time:";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(80, 9);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x29, 13);
            this.label6.TabIndex = 0x15;
            this.label6.Text = "Current";
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0xb8, 9);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x1d, 13);
            this.label9.TabIndex = 0x16;
            this.label9.Text = "New";
            this._timeLabel.BorderStyle = BorderStyle.Fixed3D;
            this._timeLabel.Location = new Point(0x2b, 0x2a);
            this._timeLabel.Name = "_timeLabel";
            this._timeLabel.Size = new Size(0x60, 0x13);
            this._timeLabel.TabIndex = 0x17;
            this._timeLabel.Text = "12:04:46 PM";
            this._timeLabel.TextAlign = ContentAlignment.MiddleLeft;
            this._openLabel.BorderStyle = BorderStyle.Fixed3D;
            this._openLabel.Location = new Point(0x37, 0x1a);
            this._openLabel.Name = "_openLabel";
            this._openLabel.Size = new Size(0x60, 0x13);
            this._openLabel.TabIndex = 0x18;
            this._openLabel.Text = "0";
            this._openLabel.TextAlign = ContentAlignment.MiddleLeft;
            this._highLabel.BorderStyle = BorderStyle.Fixed3D;
            this._highLabel.Location = new Point(0x37, 0x35);
            this._highLabel.Name = "_highLabel";
            this._highLabel.Size = new Size(0x60, 0x13);
            this._highLabel.TabIndex = 0x19;
            this._highLabel.Text = "0";
            this._highLabel.TextAlign = ContentAlignment.MiddleLeft;
            this._lowLabel.BorderStyle = BorderStyle.Fixed3D;
            this._lowLabel.Location = new Point(0x37, 80);
            this._lowLabel.Name = "_lowLabel";
            this._lowLabel.Size = new Size(0x60, 0x13);
            this._lowLabel.TabIndex = 0x1a;
            this._lowLabel.Text = "0";
            this._lowLabel.TextAlign = ContentAlignment.MiddleLeft;
            this._closeLabel.BorderStyle = BorderStyle.Fixed3D;
            this._closeLabel.Location = new Point(0x37, 0x6b);
            this._closeLabel.Name = "_closeLabel";
            this._closeLabel.Size = new Size(0x60, 0x13);
            this._closeLabel.TabIndex = 0x1b;
            this._closeLabel.Text = "0";
            this._closeLabel.TextAlign = ContentAlignment.MiddleLeft;
            this._volumeLabel.BorderStyle = BorderStyle.Fixed3D;
            this._volumeLabel.Location = new Point(0x37, 0x86);
            this._volumeLabel.Name = "_volumeLabel";
            this._volumeLabel.Size = new Size(0x60, 0x13);
            this._volumeLabel.TabIndex = 0x1c;
            this._volumeLabel.Text = "0";
            this._volumeLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.groupBox1.Controls.Add(this._datePicker);
            this.groupBox1.Controls.Add(this._dateLabel);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this._timePicker);
            this.groupBox1.Controls.Add(this._labelTime);
            this.groupBox1.Controls.Add(this._timeLabel);
            this.groupBox1.Location = new Point(12, 0xa8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(250, 0x4b);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create New Bar";
            base.AcceptButton = this.okBtn;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.cancelBtn;
            base.ClientSize = new Size(0x116, 0x125);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this._volumeLabel);
            base.Controls.Add(this._closeLabel);
            base.Controls.Add(this._lowLabel);
            base.Controls.Add(this._highLabel);
            base.Controls.Add(this._openLabel);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.label6);
            base.Controls.Add(this._volume);
            base.Controls.Add(this._close);
            base.Controls.Add(this._low);
            base.Controls.Add(this._high);
            base.Controls.Add(this._open);
            base.Controls.Add(this.cancelBtn);
            base.Controls.Add(this.removeBtn);
            base.Controls.Add(this.okBtn);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DataEditor";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bar Data Editor";
            base.FormClosing += new FormClosingEventHandler(this.DataEditor_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private bool method_0()
        {
            return ((this._open.Text != this._openLabel.Text) || ((this._high.Text != this._highLabel.Text) || ((this._low.Text != this._lowLabel.Text) || ((this._close.Text != this._closeLabel.Text) || ((this._volume.Text != this._volumeLabel.Text) || ((this._datePicker.Value.ToShortDateString() != this._dateLabel.Text) || (this._timePicker.Value.ToShortTimeString() != this._timeLabel.Text)))))));
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.operation = Operation.Ok;
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            this.operation = Operation.Remove;
        }

        public Operation Action
        {
            get
            {
                return this.operation;
            }
        }

        public double CLOSE
        {
            get
            {
                return Convert.ToDouble(this._close.Text);
            }
        }

        public DateTime Date
        {
            get
            {
                return this._datePicker.Value;
            }
        }

        public DateTime DateAndTime
        {
            get
            {
                return new DateTime(this._datePicker.Value.Year, this._datePicker.Value.Month, this._datePicker.Value.Day, this._timePicker.Value.Hour, this._timePicker.Value.Minute, this._timePicker.Value.Second);
            }
        }

        public double HIGH
        {
            get
            {
                return Convert.ToDouble(this._high.Text);
            }
        }

        public double LOW
        {
            get
            {
                return Convert.ToDouble(this._low.Text);
            }
        }

        public double OPEN
        {
            get
            {
                return Convert.ToDouble(this._open.Text);
            }
        }

        public DateTime Time
        {
            get
            {
                return this._timePicker.Value;
            }
        }

        public double VOLUME
        {
            get
            {
                return Convert.ToDouble(this._volume.Text);
            }
        }

        public enum Operation
        {
            Remove,
            Ok,
            Cancel
        }
    }
}

