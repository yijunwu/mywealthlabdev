namespace WealthLab.ChartDrawingObjects
{
    using Fidelity.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class GannFanSettings : UserControl
    {
        private Button btnAdd;
        private Button btnRemove;
        private CheckBox cbFanDown;
        private CheckBox cbFanUp;
        private CheckBox cbSnap;
        private ComboBox cmbStyle;
        private GroupBox groupBox1;
        private GroupBox grpOptions;
        private GroupBox grpStyle;
        private IContainer icontainer_0;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label lblColor;
        private Label lblStyle;
        private Label lblWidth;
        private ListBox lbRatios;
        private NumericUpDown numWidth;
        private ColorPickerPanel pnlColor;
        private TextBox Price;
        private TextBox Time;
        private TextBox tPriceUnit;

        public GannFanSettings()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(this.Time.Text);
                int num2 = Convert.ToInt32(this.Price.Text);
                if ((num > 0) && (num2 > 0))
                {
                    string item = num + "x" + num2;
                    this.lbRatios.Items.Add(item);
                }
            }
            catch (FormatException)
            {
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.lbRatios.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.lbRatios.Items.RemoveAt(selectedIndex);
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
            this.grpStyle = new GroupBox();
            this.pnlColor = new ColorPickerPanel();
            this.cmbStyle = new ComboBox();
            this.lblStyle = new Label();
            this.numWidth = new NumericUpDown();
            this.lblWidth = new Label();
            this.lblColor = new Label();
            this.grpOptions = new GroupBox();
            this.cbFanDown = new CheckBox();
            this.cbFanUp = new CheckBox();
            this.cbSnap = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.label4 = new Label();
            this.tPriceUnit = new TextBox();
            this.btnRemove = new Button();
            this.btnAdd = new Button();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.Price = new TextBox();
            this.Time = new TextBox();
            this.lbRatios = new ListBox();
            this.grpStyle.SuspendLayout();
            this.numWidth.BeginInit();
            this.grpOptions.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.grpStyle.Controls.Add(this.pnlColor);
            this.grpStyle.Controls.Add(this.cmbStyle);
            this.grpStyle.Controls.Add(this.lblStyle);
            this.grpStyle.Controls.Add(this.numWidth);
            this.grpStyle.Controls.Add(this.lblWidth);
            this.grpStyle.Controls.Add(this.lblColor);
            this.grpStyle.Location = new Point(5, 3);
            this.grpStyle.Name = "grpStyle";
            this.grpStyle.Size = new Size(0xf6, 100);
            this.grpStyle.TabIndex = 1;
            this.grpStyle.TabStop = false;
            this.grpStyle.Text = "Drawing Style";
            this.pnlColor.BackColor = System.Drawing.Color.Black;
            this.pnlColor.Cursor = Cursors.Hand;
            this.pnlColor.DrawOutline = false;
            this.pnlColor.Location = new Point(0x53, 13);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.OutlineColor = System.Drawing.Color.Black;
            this.pnlColor.Size = new Size(0x6a, 0x17);
            this.pnlColor.TabIndex = 6;
            this.pnlColor.Text = "colorPickerPanel1";
            this.cmbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStyle.FormattingEnabled = true;
            this.cmbStyle.Items.AddRange(new object[] { "Solid", "Dotted", "Dashed" });
            this.cmbStyle.Location = new Point(0x53, 0x44);
            this.cmbStyle.Name = "cmbStyle";
            this.cmbStyle.Size = new Size(0x6a, 0x15);
            this.cmbStyle.TabIndex = 5;
            this.lblStyle.AutoSize = true;
            this.lblStyle.Location = new Point(7, 0x47);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new Size(0x38, 13);
            this.lblStyle.TabIndex = 4;
            this.lblStyle.Text = "Line Style:";
            this.numWidth.Location = new Point(0x53, 0x2a);
            int[] bits = new int[4];
            bits[0] = 1;
            this.numWidth.Minimum = new decimal(bits);
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new Size(0x6a, 20);
            this.numWidth.TabIndex = 3;
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numWidth.Value = new decimal(numArray2);
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new Point(7, 0x2c);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new Size(0x3d, 13);
            this.lblWidth.TabIndex = 1;
            this.lblWidth.Text = "Line Width:";
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new Point(7, 20);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new Size(0x22, 13);
            this.lblColor.TabIndex = 0;
            this.lblColor.Text = "Color:";
            this.grpOptions.Controls.Add(this.cbFanDown);
            this.grpOptions.Controls.Add(this.cbFanUp);
            this.grpOptions.Controls.Add(this.cbSnap);
            this.grpOptions.Location = new Point(5, 0x6d);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new Size(0xf6, 0x4d);
            this.grpOptions.TabIndex = 2;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            this.cbFanDown.AutoSize = true;
            this.cbFanDown.Checked = true;
            this.cbFanDown.CheckState = CheckState.Checked;
            this.cbFanDown.Location = new Point(7, 0x39);
            this.cbFanDown.Name = "cbFanDown";
            this.cbFanDown.Size = new Size(0x4b, 0x11);
            this.cbFanDown.TabIndex = 4;
            this.cbFanDown.Text = "Fan Down";
            this.cbFanDown.UseVisualStyleBackColor = true;
            this.cbFanUp.AutoSize = true;
            this.cbFanUp.Checked = true;
            this.cbFanUp.CheckState = CheckState.Checked;
            this.cbFanUp.Location = new Point(7, 0x25);
            this.cbFanUp.Name = "cbFanUp";
            this.cbFanUp.Size = new Size(0x3d, 0x11);
            this.cbFanUp.TabIndex = 3;
            this.cbFanUp.Text = "Fan Up";
            this.cbFanUp.UseVisualStyleBackColor = true;
            this.cbSnap.AutoSize = true;
            this.cbSnap.Checked = true;
            this.cbSnap.CheckState = CheckState.Checked;
            this.cbSnap.Location = new Point(7, 0x12);
            this.cbSnap.Name = "cbSnap";
            this.cbSnap.Size = new Size(140, 0x11);
            this.cbSnap.TabIndex = 2;
            this.cbSnap.Text = "Snap Endpoints to Price";
            this.cbSnap.UseVisualStyleBackColor = true;
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tPriceUnit);
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Price);
            this.groupBox1.Controls.Add(this.Time);
            this.groupBox1.Controls.Add(this.lbRatios);
            this.groupBox1.Location = new Point(5, 0xc0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xf6, 0xb2);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time by Price Ratios";
            this.label4.Location = new Point(7, 0x8f);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0xb6, 0x1d);
            this.label4.TabIndex = 11;
            this.label4.Text = "Each Time Unit (Bar) is equal to how many Price Units?";
            this.tPriceUnit.Location = new Point(0xc3, 0x93);
            this.tPriceUnit.Name = "tPriceUnit";
            this.tPriceUnit.Size = new Size(0x2d, 20);
            this.tPriceUnit.TabIndex = 10;
            this.tPriceUnit.KeyPress += new KeyPressEventHandler(this.tPriceUnit_KeyPress);
            this.btnRemove.Location = new Point(0x73, 90);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new Size(0x7d, 0x17);
            this.btnRemove.TabIndex = 7;
            this.btnRemove.Text = "Remove Selected";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
            this.btnAdd.Location = new Point(0x73, 0x3d);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(0x7d, 0x17);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add Value";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xa6, 0x13);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1f, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Price";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x70, 0x13);
            this.label2.Name = "label2";
            this.label2.Size = new Size(30, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Time";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x95, 0x26);
            this.label1.Name = "label1";
            this.label1.Size = new Size(14, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "X";
            this.Price.Location = new Point(0xa8, 0x23);
            this.Price.Name = "Price";
            this.Price.Size = new Size(0x1d, 20);
            this.Price.TabIndex = 2;
            this.Price.Text = "1";
            this.Price.TextAlign = HorizontalAlignment.Center;
            this.Price.KeyPress += new KeyPressEventHandler(this.Time_KeyPress);
            this.Time.Location = new Point(0x73, 0x23);
            this.Time.Name = "Time";
            this.Time.Size = new Size(0x1d, 20);
            this.Time.TabIndex = 1;
            this.Time.Text = "2";
            this.Time.TextAlign = HorizontalAlignment.Center;
            this.Time.KeyPress += new KeyPressEventHandler(this.Time_KeyPress);
            this.lbRatios.ColumnWidth = 0x2d;
            this.lbRatios.FormattingEnabled = true;
            this.lbRatios.Location = new Point(10, 0x13);
            this.lbRatios.MultiColumn = true;
            this.lbRatios.Name = "lbRatios";
            this.lbRatios.Size = new Size(0x60, 0x79);
            this.lbRatios.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.grpOptions);
            base.Controls.Add(this.grpStyle);
            base.Name = "GannFanSettings";
            base.Size = new Size(0xfe, 0x179);
            this.grpStyle.ResumeLayout(false);
            this.grpStyle.PerformLayout();
            this.numWidth.EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void method_0(object sender, KeyEventArgs e)
        {
            char keyValue = (char) e.KeyValue;
            if (keyValue == '|')
            {
                e.SuppressKeyPress = true;
            }
        }

        private void Time_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keyChar = e.KeyChar;
            if (keyChar != '\b')
            {
                string str = keyChar.ToString();
                try
                {
                    Convert.ToInt32(str);
                }
                catch (FormatException)
                {
                    e.Handled = true;
                }
            }
        }

        private void tPriceUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keyChar = e.KeyChar;
            if (keyChar != '\b')
            {
                string str = this.tPriceUnit.Text + keyChar;
                try
                {
                    Convert.ToSingle(str);
                }
                catch (FormatException)
                {
                    e.Handled = true;
                }
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this.pnlColor.BackColor;
            }
            set
            {
                this.pnlColor.BackColor = value;
            }
        }

        public int DrawingObjectWidth
        {
            get
            {
                return (int) this.numWidth.Value;
            }
            set
            {
                this.numWidth.Value = value;
            }
        }

        public bool FanDown
        {
            get
            {
                return this.cbFanDown.Checked;
            }
            set
            {
                this.cbFanDown.Checked = value;
            }
        }

        public bool FanUp
        {
            get
            {
                return this.cbFanUp.Checked;
            }
            set
            {
                this.cbFanUp.Checked = value;
            }
        }

        public string PriceUnit
        {
            get
            {
                return this.tPriceUnit.Text;
            }
            set
            {
                try
                {
                    this.tPriceUnit.Text = value;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public string Ratios
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < this.lbRatios.Items.Count; i++)
                {
                    builder.Append(this.lbRatios.Items[i].ToString());
                    if (i < (this.lbRatios.Items.Count - 1))
                    {
                        builder.Append('|');
                    }
                }
                return builder.ToString();
            }
            set
            {
                this.lbRatios.Items.Clear();
                this.lbRatios.Items.AddRange(value.Split(new char[] { '|' }));
            }
        }

        public bool SnapToValue
        {
            get
            {
                return this.cbSnap.Checked;
            }
            set
            {
                this.cbSnap.Checked = value;
            }
        }

        public LineStyle Style
        {
            get
            {
                return (LineStyle) Enum.Parse(typeof(LineStyle), this.cmbStyle.Text);
            }
            set
            {
                this.cmbStyle.Text = value.ToString();
            }
        }
    }
}

