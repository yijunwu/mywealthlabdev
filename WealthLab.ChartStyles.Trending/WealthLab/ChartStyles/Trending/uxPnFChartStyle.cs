namespace WealthLab.ChartStyles.Trending
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class uxPnFChartStyle : UserControl
    {
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private bool bool_6;
        private CheckBox cbGrid;
        private CheckBox cbLogMethod;
        private ComboBox cboPriceField;
        private CheckBox cbSettings;
        private CheckBox cbTargets;
        private CheckBox cbTrendLines;
        private double double_0 = 1.0;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private IContainer icontainer_0;
        private int int_0 = 3;
        private Label lblAuto;
        private Label lblBoxSize;
        private Label lblPriceField;
        private Label lblRevBoxes;
        private RadioButton rbTraditional;
        private RadioButton rbUserSpecified;
        private const string string_0 = "Box Size (%)";
        private const string string_1 = "Box Size (Pts)";
        private string string_2 = Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
        private string string_3 = Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberGroupSeparator;
        private string string_4 = "Close";
        private TextBox txtBoxSize;
        private NumericUpDown uxReversalBoxes;

        public uxPnFChartStyle()
        {
            this.InitializeComponent();
        }

        private void cbGrid_CheckedChanged(object sender, EventArgs e)
        {
            this.bool_2 = this.cbGrid.Checked;
        }

        private void cbLogMethod_CheckedChanged(object sender, EventArgs e)
        {
            this.bool_0 = this.cbLogMethod.Checked;
            if (this.bool_0)
            {
                this.lblBoxSize.Text = "Box Size (%)";
            }
            else
            {
                this.lblBoxSize.Text = "Box Size (Pts)";
            }
        }

        private void cboPriceField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.string_4 = this.cboPriceField.Text;
        }

        private void cbSettings_CheckedChanged(object sender, EventArgs e)
        {
            this.bool_4 = this.cbSettings.Checked;
        }

        private void cbTargets_CheckedChanged(object sender, EventArgs e)
        {
            this.bool_3 = this.cbTargets.Checked;
        }

        private void cbTrendLines_CheckedChanged(object sender, EventArgs e)
        {
            this.bool_1 = this.cbTrendLines.Checked;
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
            this.groupBox1 = new GroupBox();
            this.lblAuto = new Label();
            this.cboPriceField = new ComboBox();
            this.cbLogMethod = new CheckBox();
            this.lblPriceField = new Label();
            this.lblRevBoxes = new Label();
            this.lblBoxSize = new Label();
            this.txtBoxSize = new TextBox();
            this.uxReversalBoxes = new NumericUpDown();
            this.rbUserSpecified = new RadioButton();
            this.rbTraditional = new RadioButton();
            this.cbSettings = new CheckBox();
            this.cbGrid = new CheckBox();
            this.cbTrendLines = new CheckBox();
            this.groupBox2 = new GroupBox();
            this.cbTargets = new CheckBox();
            this.groupBox1.SuspendLayout();
            this.uxReversalBoxes.BeginInit();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.lblAuto);
            this.groupBox1.Controls.Add(this.cboPriceField);
            this.groupBox1.Controls.Add(this.cbLogMethod);
            this.groupBox1.Controls.Add(this.lblPriceField);
            this.groupBox1.Controls.Add(this.lblRevBoxes);
            this.groupBox1.Controls.Add(this.lblBoxSize);
            this.groupBox1.Controls.Add(this.txtBoxSize);
            this.groupBox1.Controls.Add(this.uxReversalBoxes);
            this.groupBox1.Controls.Add(this.rbUserSpecified);
            this.groupBox1.Controls.Add(this.rbTraditional);
            this.groupBox1.Location = new Point(5, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x166, 0x70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Point and Figure Control";
            this.lblAuto.AutoSize = true;
            this.lblAuto.Location = new Point(0x120, 0x2e);
            this.lblAuto.Name = "lblAuto";
            this.lblAuto.Size = new Size(0x1d, 13);
            this.lblAuto.TabIndex = 4;
            this.lblAuto.Text = "Auto";
            this.cboPriceField.FormattingEnabled = true;
            this.cboPriceField.Items.AddRange(new object[] { "Close", "High/Low" });
            this.cboPriceField.Location = new Point(0x4c, 0x11);
            this.cboPriceField.Name = "cboPriceField";
            this.cboPriceField.Size = new Size(0x4a, 0x15);
            this.cboPriceField.TabIndex = 0;
            this.cboPriceField.Text = "n.i.";
            this.cboPriceField.SelectedIndexChanged += new EventHandler(this.cboPriceField_SelectedIndexChanged);
            this.cbLogMethod.AutoSize = true;
            this.cbLogMethod.ImageAlign = ContentAlignment.MiddleRight;
            this.cbLogMethod.Location = new Point(0xc1, 0x13);
            this.cbLogMethod.Name = "cbLogMethod";
            this.cbLogMethod.Size = new Size(0x7a, 0x11);
            this.cbLogMethod.TabIndex = 3;
            this.cbLogMethod.Text = "Use Log (%) Method";
            this.cbLogMethod.UseVisualStyleBackColor = true;
            this.cbLogMethod.CheckedChanged += new EventHandler(this.cbLogMethod_CheckedChanged);
            this.lblPriceField.AutoSize = true;
            this.lblPriceField.Location = new Point(6, 20);
            this.lblPriceField.Name = "lblPriceField";
            this.lblPriceField.Size = new Size(0x38, 13);
            this.lblPriceField.TabIndex = 7;
            this.lblPriceField.Text = "Price Field";
            this.lblRevBoxes.AutoSize = true;
            this.lblRevBoxes.Location = new Point(190, 0x55);
            this.lblRevBoxes.Name = "lblRevBoxes";
            this.lblRevBoxes.Size = new Size(0x51, 13);
            this.lblRevBoxes.TabIndex = 7;
            this.lblRevBoxes.Tag = "99";
            this.lblRevBoxes.Text = "Reversal Boxes";
            this.lblBoxSize.AutoSize = true;
            this.lblBoxSize.Location = new Point(190, 0x41);
            this.lblBoxSize.Name = "lblBoxSize";
            this.lblBoxSize.Size = new Size(0x39, 13);
            this.lblBoxSize.TabIndex = 5;
            this.lblBoxSize.Tag = "99";
            this.lblBoxSize.Text = "Box Size ()";
            this.txtBoxSize.Location = new Point(0x123, 0x3e);
            this.txtBoxSize.MaxLength = 10;
            this.txtBoxSize.Name = "txtBoxSize";
            this.txtBoxSize.Size = new Size(0x2e, 20);
            this.txtBoxSize.TabIndex = 6;
            this.txtBoxSize.Tag = "99";
            this.txtBoxSize.Text = "1.0";
            this.txtBoxSize.TextChanged += new EventHandler(this.txtBoxSize_TextChanged);
            this.txtBoxSize.KeyDown += new KeyEventHandler(this.txtBoxSize_KeyDown);
            this.txtBoxSize.KeyPress += new KeyPressEventHandler(this.txtBoxSize_KeyPress);
            this.txtBoxSize.Enter += new EventHandler(this.txtBoxSize_Enter);
            this.txtBoxSize.Validating += new CancelEventHandler(this.txtBoxSize_Validating);
            this.uxReversalBoxes.AutoSize = true;
            this.uxReversalBoxes.Location = new Point(0x123, 0x53);
            int[] bits = new int[4];
            bits[0] = 13;
            this.uxReversalBoxes.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.uxReversalBoxes.Minimum = new decimal(numArray2);
            this.uxReversalBoxes.Name = "uxReversalBoxes";
            this.uxReversalBoxes.Size = new Size(0x2e, 20);
            this.uxReversalBoxes.TabIndex = 8;
            this.uxReversalBoxes.Tag = "99";
            int[] numArray3 = new int[4];
            numArray3[0] = 4;
            this.uxReversalBoxes.Value = new decimal(numArray3);
            this.uxReversalBoxes.ValueChanged += new EventHandler(this.uxReversalBoxes_ValueChanged);
            this.rbUserSpecified.AutoSize = true;
            this.rbUserSpecified.Location = new Point(9, 0x41);
            this.rbUserSpecified.Name = "rbUserSpecified";
            this.rbUserSpecified.Size = new Size(0x8a, 0x11);
            this.rbUserSpecified.TabIndex = 2;
            this.rbUserSpecified.Text = "User-Specified Settings:";
            this.rbUserSpecified.UseVisualStyleBackColor = true;
            this.rbTraditional.AutoSize = true;
            this.rbTraditional.Checked = true;
            this.rbTraditional.Location = new Point(9, 0x2a);
            this.rbTraditional.Name = "rbTraditional";
            this.rbTraditional.Size = new Size(0xaf, 0x11);
            this.rbTraditional.TabIndex = 1;
            this.rbTraditional.TabStop = true;
            this.rbTraditional.Text = "Automated (Traditional) Settings";
            this.rbTraditional.UseVisualStyleBackColor = true;
            this.rbTraditional.CheckedChanged += new EventHandler(this.rbTraditional_CheckedChanged);
            this.cbSettings.AutoSize = true;
            this.cbSettings.Location = new Point(9, 0x13);
            this.cbSettings.Name = "cbSettings";
            this.cbSettings.Size = new Size(0x56, 0x11);
            this.cbSettings.TabIndex = 10;
            this.cbSettings.Text = "PnF Settings";
            this.cbSettings.UseVisualStyleBackColor = true;
            this.cbSettings.CheckedChanged += new EventHandler(this.cbSettings_CheckedChanged);
            this.cbGrid.AutoSize = true;
            this.cbGrid.Location = new Point(0x66, 0x13);
            this.cbGrid.Name = "cbGrid";
            this.cbGrid.Size = new Size(0x2d, 0x11);
            this.cbGrid.TabIndex = 11;
            this.cbGrid.Text = "Grid";
            this.cbGrid.UseVisualStyleBackColor = true;
            this.cbGrid.CheckedChanged += new EventHandler(this.cbGrid_CheckedChanged);
            this.cbTrendLines.AutoSize = true;
            this.cbTrendLines.Location = new Point(0xa5, 0x13);
            this.cbTrendLines.Name = "cbTrendLines";
            this.cbTrendLines.Size = new Size(0x5e, 0x11);
            this.cbTrendLines.TabIndex = 12;
            this.cbTrendLines.Text = "45\x00ba Trendlines";
            this.cbTrendLines.UseVisualStyleBackColor = true;
            this.cbTrendLines.CheckedChanged += new EventHandler(this.cbTrendLines_CheckedChanged);
            this.groupBox2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.cbTargets);
            this.groupBox2.Controls.Add(this.cbSettings);
            this.groupBox2.Controls.Add(this.cbTrendLines);
            this.groupBox2.Controls.Add(this.cbGrid);
            this.groupBox2.Location = new Point(5, 0x7c);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x166, 0x30);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Show:";
            this.cbTargets.AutoSize = true;
            this.cbTargets.Location = new Point(0x109, 0x13);
            this.cbTargets.Name = "cbTargets";
            this.cbTargets.Size = new Size(0x3e, 0x11);
            this.cbTargets.TabIndex = 13;
            this.cbTargets.Text = "Targets";
            this.cbTargets.UseVisualStyleBackColor = true;
            this.cbTargets.Visible = false;
            this.cbTargets.CheckedChanged += new EventHandler(this.cbTargets_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Name = "uxPnFChartStyle";
            base.Size = new Size(0x16e, 0xaf);
            base.Load += new EventHandler(this.uxPnFChartStyle_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.uxReversalBoxes.EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
        }

        private void rbTraditional_CheckedChanged(object sender, EventArgs e)
        {
            bool flag = !this.rbTraditional.Checked;
            this.txtBoxSize.Enabled = flag;
            this.cbLogMethod.Enabled = flag;
            this.uxReversalBoxes.Enabled = flag;
            if (this.rbTraditional.Checked)
            {
                this.lblAuto.Text = "(Auto)";
            }
            else
            {
                this.lblAuto.Text = " ";
            }
            this.bool_5 = this.rbTraditional.Checked;
        }

        private void txtBoxSize_Enter(object sender, EventArgs e)
        {
            this.txtBoxSize.SelectAll();
        }

        private void txtBoxSize_KeyDown(object sender, KeyEventArgs e)
        {
            this.bool_6 = false;
            if ((((e.KeyCode < Keys.D0) || (e.KeyCode > Keys.D9)) && ((e.KeyCode < Keys.NumPad0) || (e.KeyCode > Keys.NumPad9))) && (e.KeyCode != Keys.Back))
            {
                this.bool_6 = true;
            }
        }

        private void txtBoxSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.bool_6)
            {
                string str = e.KeyChar.ToString();
                if (!str.Equals(this.string_2) && !str.Equals(this.string_3))
                {
                    e.Handled = true;
                }
            }
        }

        private void txtBoxSize_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.double_0 = Math.Abs(double.Parse(this.txtBoxSize.Text));
            }
            catch
            {
                MessageBox.Show("Box size is a positive, non-zero number.", "Box Size");
                this.txtBoxSize.Text = this.double_0.ToString("0.0#####");
            }
        }

        private void txtBoxSize_Validating(object sender, CancelEventArgs e)
        {
            if (double.Parse(this.txtBoxSize.Text) < 1E-06)
            {
                e.Cancel = true;
                MessageBox.Show("Box size must be a positive, non-zero number.", "Box Size");
            }
        }

        private void uxPnFChartStyle_Load(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.SetToolTip(this.cbTrendLines, "Draws 45\x00ba trendlines, including inner trendlines. Not valid for 1-box charts.");
            tip.SetToolTip(this.cbSettings, "Shows the Box Size x Reversal Boxes at the top center of the PricePane.");
            tip.SetToolTip(this.cbLogMethod, "Makes box size percentage-based.");
            tip.SetToolTip(this.cbGrid, "Draws a grid of boxes.");
            tip.SetToolTip(this.cbTargets, "Shows targets based on horzontal and vertical box counts.");
            tip.SetToolTip(this.txtBoxSize, "Enter a positive, non-zero number.");
        }

        private void uxReversalBoxes_ValueChanged(object sender, EventArgs e)
        {
            this.int_0 = (int) this.uxReversalBoxes.Value;
            this.cbTrendLines.Enabled = this.int_0 > 1;
            this.cbTargets.Enabled = this.int_0 > 1;
            this.bool_1 = this.cbTrendLines.Enabled && this.cbTrendLines.Checked;
            this.bool_3 = this.cbTargets.Enabled && this.cbTargets.Checked;
        }

        public double BoxSize
        {
            get
            {
                return this.double_0;
            }
            set
            {
                if ((this.double_0 != value) & (value > 0.0))
                {
                    this.double_0 = Math.Abs(value);
                }
                this.txtBoxSize.Text = this.double_0.ToString("0.0#######");
                base.Invalidate();
            }
        }

        public bool DrawGrid
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
                this.cbGrid.Checked = value;
            }
        }

        public bool DrawSettings
        {
            get
            {
                return this.bool_4;
            }
            set
            {
                this.bool_4 = value;
                this.cbSettings.Checked = value;
            }
        }

        public bool DrawTargets
        {
            get
            {
                return this.bool_3;
            }
            set
            {
                this.bool_3 = value;
                this.cbTargets.Checked = value;
            }
        }

        public bool DrawTrendLines
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
                this.cbTrendLines.Checked = value;
            }
        }

        public bool LogMethod
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
                this.cbLogMethod.Checked = value;
                if (this.bool_0)
                {
                    this.lblBoxSize.Text = "Box Size (%)";
                }
                else
                {
                    this.lblBoxSize.Text = "Box Size (Pts)";
                }
            }
        }

        public string PriceField
        {
            get
            {
                return this.string_4;
            }
            set
            {
                this.string_4 = value;
                if (this.string_4 == "Close")
                {
                    this.cboPriceField.Text = (string) this.cboPriceField.Items[0];
                }
                else
                {
                    this.cboPriceField.Text = (string) this.cboPriceField.Items[1];
                }
            }
        }

        public int ReversalBoxes
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
                this.uxReversalBoxes.Value = value;
            }
        }

        public bool TraditionalSettings
        {
            get
            {
                return this.bool_5;
            }
            set
            {
                this.bool_5 = value;
                this.rbTraditional.Checked = value;
                this.rbUserSpecified.Checked = !value;
            }
        }
    }
}

