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
    public class FibSettings : UserControl
    {
        private Button buttonAddValue;
        private Button buttonRemove;
        private CheckBox cbDisplayLevelValues;
        private CheckBox cbSnap;
        private ComboBox cmbStyle;
        private GroupBox grpOptions;
        private GroupBox grpRetracementLevels;
        private GroupBox grpStyle;
        private IContainer icontainer_0;
        private Label lblColor;
        private Label lblStyle;
        private Label lblWidth;
        private ListBox lbRetracementLevels;
        private NumericUpDown numWidth;
        private ColorPickerPanel pnlColor;
        private TextBox tbValue;

        public FibSettings()
        {
            this.InitializeComponent();
        }

        private void buttonAddValue_Click(object sender, EventArgs e)
        {
            try
            {
                Convert.ToSingle(this.tbValue.Text);
                this.lbRetracementLevels.Items.Add(this.tbValue.Text);
            }
            catch (FormatException)
            {
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.lbRetracementLevels.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.lbRetracementLevels.Items.RemoveAt(selectedIndex);
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
            this.cbSnap = new CheckBox();
            this.grpRetracementLevels = new GroupBox();
            this.buttonRemove = new Button();
            this.buttonAddValue = new Button();
            this.tbValue = new TextBox();
            this.cbDisplayLevelValues = new CheckBox();
            this.lbRetracementLevels = new ListBox();
            this.grpStyle.SuspendLayout();
            this.numWidth.BeginInit();
            this.grpOptions.SuspendLayout();
            this.grpRetracementLevels.SuspendLayout();
            base.SuspendLayout();
            this.grpStyle.Controls.Add(this.pnlColor);
            this.grpStyle.Controls.Add(this.cmbStyle);
            this.grpStyle.Controls.Add(this.lblStyle);
            this.grpStyle.Controls.Add(this.numWidth);
            this.grpStyle.Controls.Add(this.lblWidth);
            this.grpStyle.Controls.Add(this.lblColor);
            this.grpStyle.Location = new Point(5, 3);
            this.grpStyle.Name = "grpStyle";
            this.grpStyle.Size = new Size(0xd6, 100);
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
            this.grpOptions.Controls.Add(this.cbSnap);
            this.grpOptions.Location = new Point(5, 0x6d);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new Size(0xd6, 0x29);
            this.grpOptions.TabIndex = 2;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            this.cbSnap.AutoSize = true;
            this.cbSnap.Location = new Point(7, 0x12);
            this.cbSnap.Name = "cbSnap";
            this.cbSnap.Size = new Size(140, 0x11);
            this.cbSnap.TabIndex = 2;
            this.cbSnap.Text = "Snap Endpoints to Price";
            this.cbSnap.UseVisualStyleBackColor = true;
            this.grpRetracementLevels.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.grpRetracementLevels.Controls.Add(this.buttonRemove);
            this.grpRetracementLevels.Controls.Add(this.buttonAddValue);
            this.grpRetracementLevels.Controls.Add(this.tbValue);
            this.grpRetracementLevels.Controls.Add(this.cbDisplayLevelValues);
            this.grpRetracementLevels.Controls.Add(this.lbRetracementLevels);
            this.grpRetracementLevels.Location = new Point(5, 0x9d);
            this.grpRetracementLevels.Name = "grpRetracementLevels";
            this.grpRetracementLevels.Size = new Size(0xd6, 170);
            this.grpRetracementLevels.TabIndex = 3;
            this.grpRetracementLevels.TabStop = false;
            this.grpRetracementLevels.Text = "Retracement Levels";
            this.buttonRemove.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonRemove.Location = new Point(0x5d, 0x4d);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new Size(0x73, 0x17);
            this.buttonRemove.TabIndex = 4;
            this.buttonRemove.Text = "Remove Selected";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
            this.buttonAddValue.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonAddValue.Location = new Point(0x5d, 0x2f);
            this.buttonAddValue.Name = "buttonAddValue";
            this.buttonAddValue.Size = new Size(0x73, 0x17);
            this.buttonAddValue.TabIndex = 3;
            this.buttonAddValue.Text = "Add Value";
            this.buttonAddValue.UseVisualStyleBackColor = true;
            this.buttonAddValue.Click += new EventHandler(this.buttonAddValue_Click);
            this.tbValue.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.tbValue.Location = new Point(0x9a, 0x13);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new Size(0x36, 20);
            this.tbValue.TabIndex = 2;
            this.tbValue.WordWrap = false;
            this.tbValue.KeyPress += new KeyPressEventHandler(this.tbValue_KeyPress);
            this.cbDisplayLevelValues.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbDisplayLevelValues.AutoSize = true;
            this.cbDisplayLevelValues.Location = new Point(6, 0x93);
            this.cbDisplayLevelValues.Name = "cbDisplayLevelValues";
            this.cbDisplayLevelValues.Size = new Size(0xa7, 0x11);
            this.cbDisplayLevelValues.TabIndex = 1;
            this.cbDisplayLevelValues.Text = "Display Level Values on Chart";
            this.cbDisplayLevelValues.UseVisualStyleBackColor = true;
            this.lbRetracementLevels.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lbRetracementLevels.FormattingEnabled = true;
            this.lbRetracementLevels.Location = new Point(7, 20);
            this.lbRetracementLevels.Name = "lbRetracementLevels";
            this.lbRetracementLevels.Size = new Size(80, 0x79);
            this.lbRetracementLevels.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.grpRetracementLevels);
            base.Controls.Add(this.grpOptions);
            base.Controls.Add(this.grpStyle);
            base.Name = "FibSettings";
            base.Size = new Size(0xde, 0x14f);
            this.grpStyle.ResumeLayout(false);
            this.grpStyle.PerformLayout();
            this.numWidth.EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpRetracementLevels.ResumeLayout(false);
            this.grpRetracementLevels.PerformLayout();
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

        private void tbValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keyChar = e.KeyChar;
            if (keyChar != '\b')
            {
                string str = this.tbValue.Text + keyChar;
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

        public string RetracementLevels
        {
            get
            {
                int count = this.lbRetracementLevels.Items.Count;
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < count; i++)
                {
                    builder.Append(this.lbRetracementLevels.Items[i].ToString());
                    if (i < (count - 1))
                    {
                        builder.Append('|');
                    }
                }
                return builder.ToString();
            }
            set
            {
                this.lbRetracementLevels.Items.Clear();
                this.lbRetracementLevels.Items.AddRange(value.Split(new char[] { '|' }));
            }
        }

        public bool ShowLevels
        {
            get
            {
                return this.cbDisplayLevelValues.Checked;
            }
            set
            {
                this.cbDisplayLevelValues.Checked = value;
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

