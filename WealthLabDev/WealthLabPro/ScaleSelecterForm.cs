namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class ScaleSelecterForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private ComboBox cmbScale;
        private IContainer icontainer_0;
        private Label lblCustom;
        private ListBox lbScale;
        private NumericUpDown numScale;

        public ScaleSelecterForm()
        {
            this.InitializeComponent();
            MainModule.Instance.HelpProvider.SetHelpNavigator(this, HelpNavigator.Topic);
            MainModule.Instance.HelpProvider.SetHelpKeyword(this, "scale_control.htm");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.lbScale.SelectedIndex = -1;
            base.DialogResult = DialogResult.OK;
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
            this.lbScale = new ListBox();
            this.lblCustom = new Label();
            this.numScale = new NumericUpDown();
            this.cmbScale = new ComboBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.numScale.BeginInit();
            base.SuspendLayout();
            this.lbScale.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.lbScale.FormattingEnabled = true;
            this.lbScale.Items.AddRange(new object[] { "Daily", "Weekly", "Monthly", "Quarterly", "Yearly", "--------", "1 minute", "3 minute", "5 minute", "10 minute", "15 minute", "30 minute", "60 minute" });
            this.lbScale.Location = new Point(4, 4);
            this.lbScale.Name = "lbScale";
            this.lbScale.Size = new Size(170, 160);
            this.lbScale.TabIndex = 0;
            this.lbScale.Click += new EventHandler(this.lbScale_Click);
            this.lblCustom.AutoSize = true;
            this.lblCustom.Location = new Point(4, 0xa7);
            this.lblCustom.Name = "lblCustom";
            this.lblCustom.Size = new Size(0x71, 13);
            this.lblCustom.TabIndex = 1;
            this.lblCustom.Text = "Custom Intraday Scale";
            this.numScale.Location = new Point(7, 0xb8);
            int[] bits = new int[4];
            bits[0] = 0xf4240;
            this.numScale.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numScale.Minimum = new decimal(numArray2);
            this.numScale.Name = "numScale";
            this.numScale.Size = new Size(0x27, 20);
            this.numScale.TabIndex = 2;
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.numScale.Value = new decimal(numArray3);
            this.cmbScale.FormattingEnabled = true;
            this.cmbScale.Items.AddRange(new object[] { "minute", "second", "tick" });
            this.cmbScale.Location = new Point(0x35, 0xb8);
            this.cmbScale.Name = "cmbScale";
            this.cmbScale.Size = new Size(0x3a, 0x15);
            this.cmbScale.TabIndex = 3;
            this.btnOK.Location = new Point(0x75, 0xb7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x23, 0x18);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x75, 0xd3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x39, 0x17);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Cornsilk;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0xb2, 240);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.cmbScale);
            base.Controls.Add(this.numScale);
            base.Controls.Add(this.lblCustom);
            base.Controls.Add(this.lbScale);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "ScaleSelecterForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "ScaleSelecterForm";
            base.Load += new EventHandler(this.ScaleSelecterForm_Load);
            this.numScale.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lbScale_Click(object sender, EventArgs e)
        {
            if ((this.lbScale.SelectedIndex > -1) && (this.lbScale.SelectedIndex != 5))
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        private void ScaleSelecterForm_Load(object sender, EventArgs e)
        {
            this.cmbScale.SelectedIndex = 0;
        }

        public ComboBox AvailableScales
        {
            get
            {
                return this.cmbScale;
            }
            set
            {
                this.cmbScale = value;
            }
        }

        public BarDataScale DataScale
        {
            get
            {
                BarDataScale scale = new BarDataScale();
                if (this.lbScale.SelectedIndex != -1)
                {
                    switch (this.lbScale.SelectedIndex)
                    {
                        case 0:
                            scale.Scale = BarScale.Daily;
                            return scale;

                        case 1:
                            scale.Scale = BarScale.Weekly;
                            return scale;

                        case 2:
                            scale.Scale = BarScale.Monthly;
                            return scale;

                        case 3:
                            scale.Scale = BarScale.Quarterly;
                            return scale;

                        case 4:
                            scale.Scale = BarScale.Yearly;
                            return scale;

                        case 5:
                            return scale;

                        case 6:
                            scale.Scale = BarScale.Minute;
                            scale.BarInterval = 1;
                            return scale;

                        case 7:
                            scale.Scale = BarScale.Minute;
                            scale.BarInterval = 3;
                            return scale;

                        case 8:
                            scale.Scale = BarScale.Minute;
                            scale.BarInterval = 5;
                            return scale;

                        case 9:
                            scale.Scale = BarScale.Minute;
                            scale.BarInterval = 10;
                            return scale;

                        case 10:
                            scale.Scale = BarScale.Minute;
                            scale.BarInterval = 15;
                            return scale;

                        case 11:
                            scale.Scale = BarScale.Minute;
                            scale.BarInterval = 30;
                            return scale;

                        case 12:
                            scale.Scale = BarScale.Minute;
                            scale.BarInterval = 60;
                            return scale;
                    }
                    return scale;
                }
                switch (this.cmbScale.SelectedIndex)
                {
                    case 0:
                        scale.Scale = BarScale.Minute;
                        break;

                    case 1:
                        scale.Scale = BarScale.Second;
                        break;

                    case 2:
                        scale.Scale = BarScale.Tick;
                        break;
                }
                scale.BarInterval = (int) this.numScale.Value;
                return scale;
            }
            set
            {
                switch (value.Scale)
                {
                    case BarScale.Daily:
                        this.lbScale.SelectedIndex = 0;
                        return;

                    case BarScale.Weekly:
                        this.lbScale.SelectedIndex = 1;
                        return;

                    case BarScale.Monthly:
                        this.lbScale.SelectedIndex = 2;
                        return;

                    case BarScale.Quarterly:
                        this.lbScale.SelectedIndex = 3;
                        return;

                    case BarScale.Yearly:
                        this.lbScale.SelectedIndex = 4;
                        return;
                }
                int num = -1;
                if (value.Scale == BarScale.Minute)
                {
                    if (value.BarInterval == 1)
                    {
                        num = 6;
                    }
                    else if (value.BarInterval == 3)
                    {
                        num = 7;
                    }
                    else if (value.BarInterval == 5)
                    {
                        num = 8;
                    }
                    else if (value.BarInterval == 10)
                    {
                        num = 9;
                    }
                    else if (value.BarInterval == 15)
                    {
                        num = 10;
                    }
                    else if (value.BarInterval == 30)
                    {
                        num = 11;
                    }
                    else if (value.BarInterval == 60)
                    {
                        num = 12;
                    }
                }
                if (num > -1)
                {
                    this.lbScale.SelectedIndex = num;
                }
                else
                {
                    this.lbScale.SelectedIndex = -1;
                    switch (value.Scale)
                    {
                        case BarScale.Minute:
                            this.cmbScale.SelectedIndex = 0;
                            break;

                        case BarScale.Second:
                            this.cmbScale.SelectedIndex = 1;
                            break;

                        case BarScale.Tick:
                            this.cmbScale.SelectedIndex = 2;
                            break;
                    }
                    this.numScale.Value = value.BarInterval;
                }
            }
        }
    }
}

