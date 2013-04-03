namespace WealthLab
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ParameterForm : Form
    {
        private bool bool_0;
        private bool bool_1;
        private Button btnCancel;
        private Button btnOK;
        private double double_0;
        private double double_1;
        private double double_2;
        private double double_3;
        private GroupBox grpGuideline;
        private IContainer icontainer_0;
        private int int_0 = 0xd7;
        private int int_1 = 270;
        private Label lblIncrement;
        private Label lblMax;
        private Label lblMin;
        private TextBox tbIncrement;
        private TextBox tbMax;
        private TextBox tbMin;
        private TextBox tbParameter;
        private TextBox tbValue;
        private TextBox tbWarning;

        public ParameterForm(string parameter, double value, double double_4, double double_5, double double_6)
        {
            this.InitializeComponent();
            this.bool_0 = true;
            this.tbParameter.Text = parameter;
            this.tbValue.Text = value.ToString();
            this.tbMin.Text = double_4.ToString();
            this.tbMax.Text = double_5.ToString();
            this.tbIncrement.Text = double_6.ToString();
            this.double_0 = value;
            this.double_1 = value;
            this.bool_0 = false;
            this.double_2 = double_4;
            this.double_3 = double_5;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.double_1 = this.double_0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            double num;
            if (!double.TryParse(this.tbValue.Text, out num))
            {
                this.btnOK.Enabled = false;
            }
            else if (!this.bool_1 && !this.method_0(num))
            {
                base.Height = this.int_1;
                this.tbWarning.Visible = true;
                this.tbValue.Focus();
                this.bool_1 = true;
            }
            else
            {
                base.DialogResult = DialogResult.OK;
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
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.lblMin = new Label();
            this.lblMax = new Label();
            this.lblIncrement = new Label();
            this.grpGuideline = new GroupBox();
            this.tbIncrement = new TextBox();
            this.tbMax = new TextBox();
            this.tbMin = new TextBox();
            this.tbValue = new TextBox();
            this.tbParameter = new TextBox();
            this.tbWarning = new TextBox();
            this.grpGuideline.SuspendLayout();
            base.SuspendLayout();
            this.btnOK.Location = new Point(0x3d, 0x9d);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x8e, 0x9d);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.lblMin.AutoSize = true;
            this.lblMin.Location = new Point(0x11, 0x27);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new Size(0x18, 13);
            this.lblMin.TabIndex = 4;
            this.lblMin.Text = "Min";
            this.lblMax.AutoSize = true;
            this.lblMax.Location = new Point(0x11, 0x3d);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new Size(0x1b, 13);
            this.lblMax.TabIndex = 6;
            this.lblMax.Text = "Max";
            this.lblIncrement.AutoSize = true;
            this.lblIncrement.Location = new Point(0x11, 0x54);
            this.lblIncrement.Name = "lblIncrement";
            this.lblIncrement.Size = new Size(0x36, 13);
            this.lblIncrement.TabIndex = 8;
            this.lblIncrement.Text = "Increment";
            this.grpGuideline.Controls.Add(this.tbIncrement);
            this.grpGuideline.Controls.Add(this.tbMax);
            this.grpGuideline.Controls.Add(this.tbMin);
            this.grpGuideline.Controls.Add(this.lblMax);
            this.grpGuideline.Controls.Add(this.lblIncrement);
            this.grpGuideline.Controls.Add(this.lblMin);
            this.grpGuideline.Location = new Point(12, 0x25);
            this.grpGuideline.Name = "grpGuideline";
            this.grpGuideline.Size = new Size(0xcd, 0x72);
            this.grpGuideline.TabIndex = 9;
            this.grpGuideline.TabStop = false;
            this.grpGuideline.Text = "The following Min, Max, and Increment are guidelines.";
            this.tbIncrement.BackColor = SystemColors.Window;
            this.tbIncrement.Enabled = false;
            this.tbIncrement.Location = new Point(0x4d, 0x51);
            this.tbIncrement.Name = "tbIncrement";
            this.tbIncrement.ReadOnly = true;
            this.tbIncrement.Size = new Size(100, 20);
            this.tbIncrement.TabIndex = 12;
            this.tbIncrement.TabStop = false;
            this.tbMax.BackColor = SystemColors.Window;
            this.tbMax.Enabled = false;
            this.tbMax.Location = new Point(0x4d, 0x3a);
            this.tbMax.Name = "tbMax";
            this.tbMax.ReadOnly = true;
            this.tbMax.Size = new Size(100, 20);
            this.tbMax.TabIndex = 11;
            this.tbMax.TabStop = false;
            this.tbMin.BackColor = SystemColors.Window;
            this.tbMin.Enabled = false;
            this.tbMin.Location = new Point(0x4d, 0x24);
            this.tbMin.Name = "tbMin";
            this.tbMin.ReadOnly = true;
            this.tbMin.Size = new Size(100, 20);
            this.tbMin.TabIndex = 10;
            this.tbMin.TabStop = false;
            this.tbValue.Location = new Point(0x94, 11);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new Size(0x45, 20);
            this.tbValue.TabIndex = 0;
            this.tbValue.WordWrap = false;
            this.tbValue.TextChanged += new EventHandler(this.tbValue_TextChanged);
            this.tbParameter.BackColor = SystemColors.Control;
            this.tbParameter.BorderStyle = BorderStyle.None;
            this.tbParameter.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.tbParameter.Location = new Point(12, 11);
            this.tbParameter.Name = "tbParameter";
            this.tbParameter.ReadOnly = true;
            this.tbParameter.Size = new Size(100, 13);
            this.tbParameter.TabIndex = 11;
            this.tbParameter.TabStop = false;
            this.tbParameter.Text = "parameter";
            this.tbWarning.BackColor = SystemColors.Control;
            this.tbWarning.BorderStyle = BorderStyle.None;
            this.tbWarning.CausesValidation = false;
            this.tbWarning.Cursor = Cursors.Default;
            this.tbWarning.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.tbWarning.ForeColor = Color.FromArgb(0xc0, 0, 0);
            this.tbWarning.Location = new Point(12, 0xba);
            this.tbWarning.Multiline = true;
            this.tbWarning.Name = "tbWarning";
            this.tbWarning.ReadOnly = true;
            this.tbWarning.Size = new Size(0xcd, 0x34);
            this.tbWarning.TabIndex = 12;
            this.tbWarning.TabStop = false;
            this.tbWarning.Text = "Value entered is outside the range specified by the Min and Max. This may cause an error in the strategy. Press OK to accept the value.";
            this.tbWarning.Visible = false;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0xe5, 0xf5);
            base.Controls.Add(this.tbWarning);
            base.Controls.Add(this.tbParameter);
            base.Controls.Add(this.tbValue);
            base.Controls.Add(this.grpGuideline);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ParameterForm";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            this.Text = "Change Parameter Value";
            base.Shown += new EventHandler(this.ParameterForm_Shown);
            this.grpGuideline.ResumeLayout(false);
            this.grpGuideline.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private bool method_0(double double_4)
        {
            return ((double_4 >= this.double_2) && (double_4 <= this.double_3));
        }

        private void ParameterForm_Shown(object sender, EventArgs e)
        {
            base.Height = this.int_0;
            base.Left = Control.MousePosition.X + 3;
            base.Top = (Control.MousePosition.Y - base.Height) - 10;
            this.tbValue.Focus();
        }

        private void tbValue_TextChanged(object sender, EventArgs e)
        {
            if (!this.bool_0)
            {
                if (((this.tbValue.Text == "") || (this.tbValue.Text == "-")) || (this.tbValue.Text == "."))
                {
                    this.btnOK.Enabled = false;
                }
                else
                {
                    double num;
                    if (!double.TryParse(this.tbValue.Text, out num))
                    {
                        this.bool_0 = true;
                        this.tbValue.Text = this.double_1.ToString();
                        this.bool_0 = false;
                    }
                    else
                    {
                        this.double_1 = num;
                    }
                    if (this.bool_1)
                    {
                        if (this.method_0(num))
                        {
                            this.tbWarning.Enabled = false;
                        }
                        else
                        {
                            this.tbWarning.Enabled = true;
                        }
                    }
                    this.btnOK.Enabled = true;
                }
            }
        }

        public double NewValue
        {
            get
            {
                return this.double_1;
            }
        }
    }
}

