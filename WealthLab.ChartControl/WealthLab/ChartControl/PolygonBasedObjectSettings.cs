namespace WealthLab.ChartControl
{
    using Fidelity.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class PolygonBasedObjectSettings : UserControl
    {
        private CheckBox cbSnap;
        private ComboBox cmbStyle;
        private GroupBox grpOptions;
        private GroupBox grpStyle;
        private IContainer icontainer_0;
        private Label label1;
        private Label lblColor;
        private Label lblStyle;
        private Label lblWidth;
        private NumericUpDown numTransparency;
        private NumericUpDown numWidth;
        private ColorPickerPanel pnlColor;

        public PolygonBasedObjectSettings()
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
            this.grpStyle = new GroupBox();
            this.numTransparency = new NumericUpDown();
            this.label1 = new Label();
            this.pnlColor = new ColorPickerPanel();
            this.cmbStyle = new ComboBox();
            this.lblStyle = new Label();
            this.numWidth = new NumericUpDown();
            this.lblWidth = new Label();
            this.lblColor = new Label();
            this.grpOptions = new GroupBox();
            this.cbSnap = new CheckBox();
            this.grpStyle.SuspendLayout();
            this.numTransparency.BeginInit();
            this.numWidth.BeginInit();
            this.grpOptions.SuspendLayout();
            base.SuspendLayout();
            this.grpStyle.Controls.Add(this.numTransparency);
            this.grpStyle.Controls.Add(this.label1);
            this.grpStyle.Controls.Add(this.pnlColor);
            this.grpStyle.Controls.Add(this.cmbStyle);
            this.grpStyle.Controls.Add(this.lblStyle);
            this.grpStyle.Controls.Add(this.numWidth);
            this.grpStyle.Controls.Add(this.lblWidth);
            this.grpStyle.Controls.Add(this.lblColor);
            this.grpStyle.Location = new Point(5, 3);
            this.grpStyle.Name = "grpStyle";
            this.grpStyle.Size = new Size(0xd1, 0x80);
            this.grpStyle.TabIndex = 1;
            this.grpStyle.TabStop = false;
            this.grpStyle.Text = "Drawing Style";
            int[] bits = new int[4];
            bits[0] = 5;
            this.numTransparency.Increment = new decimal(bits);
            this.numTransparency.Location = new Point(0x61, 0x5f);
            this.numTransparency.Name = "numTransparency";
            this.numTransparency.Size = new Size(0x6a, 20);
            this.numTransparency.TabIndex = 8;
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numTransparency.Value = new decimal(numArray2);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(4, 0x66);
            this.label1.Name = "label1";
            this.label1.Size = new Size(90, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Fill Transparency:";
            this.pnlColor.BackColor = System.Drawing.Color.Black;
            this.pnlColor.Cursor = Cursors.Hand;
            this.pnlColor.DrawOutline = false;
            this.pnlColor.Location = new Point(0x61, 13);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.OutlineColor = System.Drawing.Color.Black;
            this.pnlColor.Size = new Size(0x6a, 0x17);
            this.pnlColor.TabIndex = 6;
            this.pnlColor.Text = "colorPickerPanel1";
            this.cmbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStyle.FormattingEnabled = true;
            this.cmbStyle.Items.AddRange(new object[] { "Solid", "Dotted", "Dashed" });
            this.cmbStyle.Location = new Point(0x61, 0x44);
            this.cmbStyle.Name = "cmbStyle";
            this.cmbStyle.Size = new Size(0x6a, 0x15);
            this.cmbStyle.TabIndex = 5;
            this.lblStyle.AutoSize = true;
            this.lblStyle.Location = new Point(4, 0x4c);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new Size(0x38, 13);
            this.lblStyle.TabIndex = 4;
            this.lblStyle.Text = "Line Style:";
            this.numWidth.Location = new Point(0x61, 0x2a);
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.numWidth.Minimum = new decimal(numArray3);
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new Size(0x6a, 20);
            this.numWidth.TabIndex = 3;
            int[] numArray4 = new int[4];
            numArray4[0] = 1;
            this.numWidth.Value = new decimal(numArray4);
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new Point(4, 0x31);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new Size(0x3d, 13);
            this.lblWidth.TabIndex = 1;
            this.lblWidth.Text = "Line Width:";
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new Point(4, 0x17);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new Size(0x22, 13);
            this.lblColor.TabIndex = 0;
            this.lblColor.Text = "Color:";
            this.grpOptions.Controls.Add(this.cbSnap);
            this.grpOptions.Location = new Point(5, 0x89);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new Size(0xd1, 0x29);
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
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.grpOptions);
            base.Controls.Add(this.grpStyle);
            base.Name = "PolygonBasedObjectSettings";
            base.Size = new Size(0xde, 0xc3);
            this.grpStyle.ResumeLayout(false);
            this.grpStyle.PerformLayout();
            this.numTransparency.EndInit();
            this.numWidth.EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
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

        public int FillTransparency
        {
            get
            {
                return (int) this.numTransparency.Value;
            }
            set
            {
                this.numTransparency.Value = value;
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

