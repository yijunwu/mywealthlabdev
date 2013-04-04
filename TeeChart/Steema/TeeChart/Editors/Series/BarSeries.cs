namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BarSeries : BaseSeriesForm
    {
        internal bool addStackEditor;
        private Button BBarBrush;
        private ButtonColor BBarColor;
        private ButtonPen BBarPen;
        private Button BGradient;
        private ButtonPen BTickLines;
        private CheckBox CBBarSideMargins;
        private ComboBox CBBarStyle;
        private CheckBox CBColorEach;
        private CheckBox CBDarkBar;
        private CheckBox CBMarksAutoPosition;
        private CheckBox cbRelative;
        private Container components;
        private GroupBox groupBox1;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label LStyle;
        private CustomBar series;
        private StackBarSeries stackEditor;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private NumericUpDown UDBarDepth;
        private NumericUpDown UDBarOffset;
        private NumericUpDown UDBarWidth;

        public BarSeries()
        {
            this.addStackEditor = true;
            this.InitializeComponent();
            this.CBBarStyle.Items.Add("Arrow");
            this.CBBarStyle.Items.Add("Cone");
            this.CBBarStyle.Items.Add("Cylinder");
            this.CBBarStyle.Items.Add("Ellipse");
            this.CBBarStyle.Items.Add("Pyramid");
            this.CBBarStyle.Items.Add("Inv. Pyramid");
            this.CBBarStyle.Items.Add("Rectangle");
            this.CBBarStyle.Items.Add("Rect. Gradient");
            this.CBBarStyle.Items.Add("Inv. Arrow");
            this.CBBarStyle.Items.Add("Inv. Cone");
        }

        public BarSeries(Series s) : this()
        {
            this.series = (CustomBar) s;
        }

        private void BBarBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
        }

        private void BBarColor_Click(object sender, EventArgs e)
        {
            this.series.Color = this.BBarColor.Color;
            if (this.CBColorEach.Checked)
            {
                this.CBColorEach.Checked = false;
            }
        }

        private void BGradient_Click(object sender, EventArgs e)
        {
            GradientEditor.Edit(this.series.Gradient);
        }

        private void CBBarSideMargins_CheckedChanged(object sender, EventArgs e)
        {
            this.series.SideMargins = this.CBBarSideMargins.Checked;
        }

        private void CBBarStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.CBBarStyle.SelectedIndex)
            {
                case 0:
                    this.series.BarStyle = BarStyles.Arrow;
                    break;

                case 1:
                    this.series.BarStyle = BarStyles.Cone;
                    break;

                case 2:
                    this.series.BarStyle = BarStyles.Cylinder;
                    break;

                case 3:
                    this.series.BarStyle = BarStyles.Ellipse;
                    break;

                case 4:
                    this.series.BarStyle = BarStyles.Pyramid;
                    break;

                case 5:
                    this.series.BarStyle = BarStyles.InvPyramid;
                    break;

                case 6:
                    this.series.BarStyle = BarStyles.Rectangle;
                    break;

                case 7:
                    this.series.BarStyle = BarStyles.RectGradient;
                    break;

                case 8:
                    this.series.BarStyle = BarStyles.InvArrow;
                    break;

                case 9:
                    this.series.BarStyle = BarStyles.InvCone;
                    break;
            }
            this.cbRelative.Enabled = (this.series.BarStyle == BarStyles.RectGradient) || (this.series.BarStyle == BarStyles.Rectangle);
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEach = this.CBColorEach.Checked;
        }

        private void CBDarkBar_CheckedChanged(object sender, EventArgs e)
        {
            this.series.Dark3D = this.CBDarkBar.Checked;
        }

        private void CBMarksAutoPosition_CheckedChanged(object sender, EventArgs e)
        {
            this.series.AutoMarkPosition = this.CBMarksAutoPosition.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.series.GradientRelative = this.cbRelative.Checked;
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
            this.CBBarStyle = new ComboBox();
            this.BBarPen = new ButtonPen();
            this.BBarBrush = new Button();
            this.BBarColor = new ButtonColor();
            this.groupBox1 = new GroupBox();
            this.CBColorEach = new CheckBox();
            this.CBMarksAutoPosition = new CheckBox();
            this.CBBarSideMargins = new CheckBox();
            this.CBDarkBar = new CheckBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.UDBarWidth = new NumericUpDown();
            this.UDBarOffset = new NumericUpDown();
            this.groupBox3 = new GroupBox();
            this.cbRelative = new CheckBox();
            this.BGradient = new Button();
            this.UDBarDepth = new NumericUpDown();
            this.label3 = new Label();
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.LStyle = new Label();
            this.BTickLines = new ButtonPen();
            this.tabPage2 = new TabPage();
            this.groupBox1.SuspendLayout();
            this.UDBarWidth.BeginInit();
            this.UDBarOffset.BeginInit();
            this.groupBox3.SuspendLayout();
            this.UDBarDepth.BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            base.SuspendLayout();
            this.CBBarStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBBarStyle.Location = new Point(8, 0x18);
            this.CBBarStyle.Name = "CBBarStyle";
            this.CBBarStyle.Size = new Size(0x6a, 0x15);
            this.CBBarStyle.TabIndex = 0;
            this.CBBarStyle.SelectedIndexChanged += new EventHandler(this.CBBarStyle_SelectedIndexChanged);
            this.BBarPen.FlatStyle = FlatStyle.Flat;
            this.BBarPen.Location = new Point(0x10, 0x38);
            this.BBarPen.Name = "BBarPen";
            this.BBarPen.Size = new Size(0x58, 0x17);
            this.BBarPen.TabIndex = 3;
            this.BBarPen.Text = "&Border...";
            this.BBarBrush.FlatStyle = FlatStyle.Flat;
            this.BBarBrush.Location = new Point(0x88, 0x18);
            this.BBarBrush.Name = "BBarBrush";
            this.BBarBrush.Size = new Size(80, 0x17);
            this.BBarBrush.TabIndex = 1;
            this.BBarBrush.Text = "&Pattern...";
            this.BBarBrush.Click += new EventHandler(this.BBarBrush_Click);
            this.BBarColor.Color = Color.Empty;
            this.BBarColor.Location = new Point(8, 0x20);
            this.BBarColor.Name = "BBarColor";
            this.BBarColor.Size = new Size(0x58, 0x17);
            this.BBarColor.TabIndex = 1;
            this.BBarColor.Text = "&Color...";
            this.BBarColor.Click += new EventHandler(this.BBarColor_Click);
            this.groupBox1.Controls.Add(this.BBarColor);
            this.groupBox1.Controls.Add(this.CBColorEach);
            this.groupBox1.FlatStyle = FlatStyle.Flat;
            this.groupBox1.Location = new Point(8, 0x58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x70, 0x3f);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(8, 10);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x60, 0x10);
            this.CBColorEach.TabIndex = 0;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.CBMarksAutoPosition.FlatStyle = FlatStyle.Flat;
            this.CBMarksAutoPosition.Location = new Point(0x38, 0x80);
            this.CBMarksAutoPosition.Name = "CBMarksAutoPosition";
            this.CBMarksAutoPosition.Size = new Size(0xc3, 0x10);
            this.CBMarksAutoPosition.TabIndex = 4;
            this.CBMarksAutoPosition.Text = "&Auto Mark Position";
            this.CBMarksAutoPosition.CheckedChanged += new EventHandler(this.CBMarksAutoPosition_CheckedChanged);
            this.CBBarSideMargins.FlatStyle = FlatStyle.Flat;
            this.CBBarSideMargins.Location = new Point(0x38, 0x68);
            this.CBBarSideMargins.Name = "CBBarSideMargins";
            this.CBBarSideMargins.Size = new Size(0x90, 0x10);
            this.CBBarSideMargins.TabIndex = 3;
            this.CBBarSideMargins.Text = "Bar S&ide Margins";
            this.CBBarSideMargins.CheckedChanged += new EventHandler(this.CBBarSideMargins_CheckedChanged);
            this.CBDarkBar.FlatStyle = FlatStyle.Flat;
            this.CBDarkBar.Location = new Point(0x88, 0x88);
            this.CBDarkBar.Name = "CBDarkBar";
            this.CBDarkBar.Size = new Size(0x90, 0x10);
            this.CBDarkBar.TabIndex = 6;
            this.CBDarkBar.Text = "&Dark Bar 3D Sides";
            this.CBDarkBar.CheckedChanged += new EventHandler(this.CBDarkBar_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(40, 0x2a);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x44, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "% Bar &Width:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(40, 0x43);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x44, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "% Bar O&ffset:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.UDBarWidth.BorderStyle = BorderStyle.FixedSingle;
            this.UDBarWidth.Location = new Point(0x70, 40);
            this.UDBarWidth.Name = "UDBarWidth";
            this.UDBarWidth.Size = new Size(0x33, 20);
            this.UDBarWidth.TabIndex = 1;
            this.UDBarWidth.TextAlign = HorizontalAlignment.Right;
            this.UDBarWidth.TextChanged += new EventHandler(this.UDBarWidth_ValueChanged);
            this.UDBarWidth.ValueChanged += new EventHandler(this.UDBarWidth_ValueChanged);
            this.UDBarOffset.BorderStyle = BorderStyle.FixedSingle;
            this.UDBarOffset.Location = new Point(0x70, 0x40);
            int[] bits = new int[4];
            bits[0] = 100;
            bits[3] = -2147483648;
            this.UDBarOffset.Minimum = new decimal(bits);
            this.UDBarOffset.Name = "UDBarOffset";
            this.UDBarOffset.Size = new Size(0x33, 20);
            this.UDBarOffset.TabIndex = 2;
            this.UDBarOffset.TextAlign = HorizontalAlignment.Right;
            this.UDBarOffset.TextChanged += new EventHandler(this.UDBarOffset_ValueChanged);
            this.UDBarOffset.ValueChanged += new EventHandler(this.UDBarOffset_ValueChanged);
            this.groupBox3.Controls.Add(this.cbRelative);
            this.groupBox3.Controls.Add(this.BGradient);
            this.groupBox3.FlatStyle = FlatStyle.Flat;
            this.groupBox3.Location = new Point(0x88, 0x38);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0xa8, 0x48);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Gradient:";
            this.cbRelative.Enabled = false;
            this.cbRelative.FlatStyle = FlatStyle.Flat;
            this.cbRelative.Location = new Point(8, 0x30);
            this.cbRelative.Name = "cbRelative";
            this.cbRelative.Size = new Size(120, 0x10);
            this.cbRelative.TabIndex = 1;
            this.cbRelative.Text = "&Relative";
            this.cbRelative.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.BGradient.FlatStyle = FlatStyle.Flat;
            this.BGradient.Location = new Point(8, 0x12);
            this.BGradient.Name = "BGradient";
            this.BGradient.Size = new Size(0x60, 0x17);
            this.BGradient.TabIndex = 0;
            this.BGradient.Text = "&Gradient...";
            this.BGradient.Click += new EventHandler(this.BGradient_Click);
            this.UDBarDepth.BorderStyle = BorderStyle.FixedSingle;
            this.UDBarDepth.Location = new Point(0x70, 0x10);
            this.UDBarDepth.Name = "UDBarDepth";
            this.UDBarDepth.Size = new Size(0x33, 20);
            this.UDBarDepth.TabIndex = 0;
            this.UDBarDepth.TextAlign = HorizontalAlignment.Right;
            this.UDBarDepth.TextChanged += new EventHandler(this.UDBarDepth_ValueChanged);
            this.UDBarDepth.ValueChanged += new EventHandler(this.UDBarDepth_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(40, 0x11);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x45, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "% Bar De&pth:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(320, 0xb6);
            this.tabControl1.TabIndex = 0;
            this.tabPage1.Controls.Add(this.LStyle);
            this.tabPage1.Controls.Add(this.BTickLines);
            this.tabPage1.Controls.Add(this.CBBarStyle);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.BBarPen);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.CBDarkBar);
            this.tabPage1.Controls.Add(this.BBarBrush);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x138, 0x9c);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Format";
            this.LStyle.Location = new Point(9, 8);
            this.LStyle.Name = "LStyle";
            this.LStyle.Size = new Size(0x52, 0x10);
            this.LStyle.TabIndex = 7;
            this.LStyle.Text = "St&yle";
            this.BTickLines.FlatStyle = FlatStyle.Flat;
            this.BTickLines.Location = new Point(0xe0, 0x18);
            this.BTickLines.Name = "BTickLines";
            this.BTickLines.Size = new Size(80, 0x17);
            this.BTickLines.TabIndex = 2;
            this.BTickLines.Text = "&Tick lines...";
            this.tabPage2.Controls.Add(this.UDBarDepth);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.UDBarWidth);
            this.tabPage2.Controls.Add(this.UDBarOffset);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.CBMarksAutoPosition);
            this.tabPage2.Controls.Add(this.CBBarSideMargins);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0x138, 0x9c);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Size";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(320, 0xb6);
            base.Controls.Add(this.tabControl1);
            base.Name = "BarSeries";
            this.groupBox1.ResumeLayout(false);
            this.UDBarWidth.EndInit();
            this.UDBarOffset.EndInit();
            this.groupBox3.ResumeLayout(false);
            this.UDBarDepth.EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            base.ResumeLayout(false);
        }

        public static BarSeries InsertEditor(Control parent, Bar s)
        {
            TabControl control = (TabControl) ((TabPage) parent).Parent;
            TabPage page = new TabPage("Bar");
            control.TabPages.Add(page);
            BarSeries f = new BarSeries(s) {
                addStackEditor = false
            };
            f.SetParent(page);
            EditorUtils.InsertForm(f, page);
            TabPage page2 = control.TabPages[1];
            control.TabPages[1] = page;
            control.TabPages[control.TabCount - 1] = page2;
            return f;
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.series != null)
            {
                this.CBColorEach.Checked = this.series.ColorEach;
                this.CBDarkBar.Checked = this.series.Dark3D;
                this.CBBarSideMargins.Checked = this.series.SideMargins;
                this.UDBarDepth.Value = this.series.DepthPercent;
                this.UDBarOffset.Value = this.series.OffsetPercent;
                this.UDBarWidth.Value = this.series.barSizePercent;
                this.CBMarksAutoPosition.Checked = this.series.AutoMarkPosition;
                this.cbRelative.Checked = this.series.GradientRelative;
                this.cbRelative.Enabled = (this.series.BarStyle == BarStyles.RectGradient) || (this.series.BarStyle == BarStyles.Rectangle);
                this.BBarPen.Pen = this.series.Pen;
                this.BBarColor.Color = this.series.Brush.Color;
                this.BTickLines.Pen = this.series.TickLines;
                switch (this.series.BarStyle)
                {
                    case BarStyles.Rectangle:
                        this.CBBarStyle.SelectedIndex = 6;
                        break;

                    case BarStyles.Pyramid:
                        this.CBBarStyle.SelectedIndex = 4;
                        break;

                    case BarStyles.InvPyramid:
                        this.CBBarStyle.SelectedIndex = 5;
                        break;

                    case BarStyles.Cylinder:
                        this.CBBarStyle.SelectedIndex = 2;
                        break;

                    case BarStyles.Ellipse:
                        this.CBBarStyle.SelectedIndex = 3;
                        break;

                    case BarStyles.Arrow:
                        this.CBBarStyle.SelectedIndex = 0;
                        break;

                    case BarStyles.RectGradient:
                        this.CBBarStyle.SelectedIndex = 7;
                        break;

                    case BarStyles.Cone:
                        this.CBBarStyle.SelectedIndex = 1;
                        break;

                    case BarStyles.InvArrow:
                        this.CBBarStyle.SelectedIndex = 8;
                        break;

                    case BarStyles.InvCone:
                        this.CBBarStyle.SelectedIndex = 9;
                        break;
                }
                if (this.addStackEditor && (this.stackEditor == null))
                {
                    TabControl parent = (TabControl) Parent.Parent;
                    TabPage page = new TabPage(Texts.Stack);
                    parent.TabPages.Add(page);
                    this.stackEditor = new StackBarSeries(this.series, page);
                    EditorUtils.Translate(this.stackEditor);
                    TabPage page2 = parent.TabPages[1];
                    parent.TabPages[1] = page;
                    parent.TabPages[parent.TabCount - 1] = page2;
                }
            }
        }

        private void UDBarDepth_ValueChanged(object sender, EventArgs e)
        {
            this.series.DepthPercent = (int) this.UDBarDepth.Value;
        }

        private void UDBarOffset_ValueChanged(object sender, EventArgs e)
        {
            this.series.OffsetPercent = (int) this.UDBarOffset.Value;
        }

        private void UDBarWidth_ValueChanged(object sender, EventArgs e)
        {
            this.series.SetBarSizePercent((int) this.UDBarWidth.Value);
        }
    }
}

