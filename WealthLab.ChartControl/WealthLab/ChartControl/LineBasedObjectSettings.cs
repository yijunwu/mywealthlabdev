namespace WealthLab.ChartControl
{
    using Fidelity.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class LineBasedObjectSettings : UserControl
    {
        private CheckBox cbExtendLeft;
        private CheckBox cbExtendRight;
        private CheckBox cbSnap;
        private ComboBox cmbStyle;
        protected GroupBox grpName;
        protected GroupBox grpOptions;
        protected GroupBox grpStyle;
        private IContainer icontainer_0;
        private Label lblColor;
        private Label lblName;
        private Label lblStyle;
        private Label lblWidth;
        private NumericUpDown numWidth;
        private ColorPickerPanel pnlColor;
        private TextBox txtName;

        public LineBasedObjectSettings()
        {
            this.InitializeComponent();
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
            this.grpName = new GroupBox();
            this.txtName = new TextBox();
            this.lblName = new Label();
            this.grpStyle = new GroupBox();
            this.pnlColor = new ColorPickerPanel();
            this.cmbStyle = new ComboBox();
            this.lblStyle = new Label();
            this.numWidth = new NumericUpDown();
            this.lblWidth = new Label();
            this.lblColor = new Label();
            this.grpOptions = new GroupBox();
            this.cbSnap = new CheckBox();
            this.cbExtendRight = new CheckBox();
            this.cbExtendLeft = new CheckBox();
            this.grpName.SuspendLayout();
            this.grpStyle.SuspendLayout();
            this.numWidth.BeginInit();
            this.grpOptions.SuspendLayout();
            base.SuspendLayout();
            this.grpName.Controls.Add(this.txtName);
            this.grpName.Controls.Add(this.lblName);
            this.grpName.Location = new Point(4, 4);
            this.grpName.Name = "grpName";
            this.grpName.Size = new Size(0xc9, 0x54);
            this.grpName.TabIndex = 0;
            this.grpName.TabStop = false;
            this.grpName.Text = "Name of Line";
            this.txtName.Location = new Point(10, 0x36);
            this.txtName.MaxLength = 40;
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(0xb3, 20);
            this.txtName.TabIndex = 1;
            this.txtName.KeyDown += new KeyEventHandler(this.txtName_KeyDown);
            this.lblName.Location = new Point(7, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(0xb6, 30);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Used to identify particular Drawing Objects in Trading Systems";
            this.grpStyle.Controls.Add(this.pnlColor);
            this.grpStyle.Controls.Add(this.cmbStyle);
            this.grpStyle.Controls.Add(this.lblStyle);
            this.grpStyle.Controls.Add(this.numWidth);
            this.grpStyle.Controls.Add(this.lblWidth);
            this.grpStyle.Controls.Add(this.lblColor);
            this.grpStyle.Location = new Point(4, 0x5f);
            this.grpStyle.Name = "grpStyle";
            this.grpStyle.Size = new Size(0xc9, 100);
            this.grpStyle.TabIndex = 1;
            this.grpStyle.TabStop = false;
            this.grpStyle.Text = "Drawing Style";
            this.pnlColor.BackColor = System.Drawing.Color.Black;
            this.pnlColor.Cursor = Cursors.Hand;
            this.pnlColor.Location = new Point(0x53, 13);
            this.pnlColor.Name = "pnlColor";
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
            this.grpOptions.Controls.Add(this.cbExtendRight);
            this.grpOptions.Controls.Add(this.cbExtendLeft);
            this.grpOptions.Location = new Point(4, 0xc9);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new Size(0xc9, 0x5d);
            this.grpOptions.TabIndex = 2;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            this.cbSnap.AutoSize = true;
            this.cbSnap.Location = new Point(7, 0x43);
            this.cbSnap.Name = "cbSnap";
            this.cbSnap.Size = new Size(140, 0x11);
            this.cbSnap.TabIndex = 2;
            this.cbSnap.Text = "Snap Endpoints to Price";
            this.cbSnap.UseVisualStyleBackColor = true;
            this.cbExtendRight.AutoSize = true;
            this.cbExtendRight.Location = new Point(7, 0x2b);
            this.cbExtendRight.Name = "cbExtendRight";
            this.cbExtendRight.Size = new Size(110, 0x11);
            this.cbExtendRight.TabIndex = 1;
            this.cbExtendRight.Text = "Extend Line Right";
            this.cbExtendRight.UseVisualStyleBackColor = true;
            this.cbExtendLeft.AutoSize = true;
            this.cbExtendLeft.Location = new Point(7, 20);
            this.cbExtendLeft.Name = "cbExtendLeft";
            this.cbExtendLeft.Size = new Size(0x67, 0x11);
            this.cbExtendLeft.TabIndex = 0;
            this.cbExtendLeft.Text = "Extend Line Left";
            this.cbExtendLeft.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.grpOptions);
            base.Controls.Add(this.grpStyle);
            base.Controls.Add(this.grpName);
            base.Name = "LineBasedObjectSettings";
            base.Size = new Size(0xd1, 0x12e);
            this.grpName.ResumeLayout(false);
            this.grpName.PerformLayout();
            this.grpStyle.ResumeLayout(false);
            this.grpStyle.PerformLayout();
            this.numWidth.EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            base.ResumeLayout(false);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            char keyValue = (char) e.KeyValue;
            if (keyValue == '|')
            {
                e.SuppressKeyPress = true;
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

        public string DrawingObjectName
        {
            get
            {
                return this.txtName.Text;
            }
            set
            {
                this.txtName.Text = value;
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

        public bool ExtendLeft
        {
            get
            {
                return this.cbExtendLeft.Checked;
            }
            set
            {
                this.cbExtendLeft.Checked = value;
            }
        }

        public bool ExtendRight
        {
            get
            {
                return this.cbExtendRight.Checked;
            }
            set
            {
                this.cbExtendRight.Checked = value;
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

