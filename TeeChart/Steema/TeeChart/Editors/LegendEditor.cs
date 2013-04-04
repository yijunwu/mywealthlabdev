namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class LegendEditor : Form
    {
        private ButtonPen BDivLines;
        private Button bShadow;
        private ButtonPen bSymPen;
        private CheckBox CBBoxes;
        private ComboBox CBColWUnits;
        private CheckBox CBContinuous;
        private CheckBox CBCustPos;
        private CheckBox CBFontColor;
        private CheckBox CBInverted;
        private ComboBox CBLegendStyle;
        private ComboBox CBLegStyle;
        private CheckBox CBResizeChart;
        private CheckBox CBShow;
        private CheckBox cbSquared;
        private ComboBox CBSymbolPos;
        private CheckBox cbSymPen;
        private ComboBox cbTextAlign;
        private CheckBox cbTitleVisible;
        private CheckBox cbVisible;
        private Chart chart;
        private Container components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label10;
        private Label label12;
        private Label label13;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Legend legend;
        private LegendTitle legendTitle;
        private TextBox MemoText;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private bool setting;
        private CustomShapeEditor shapeEditor;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabPage tabPage1;
        private TabPage tabPosition;
        private TabPage tabStyle;
        private TabPage tabSymbols;
        private TabPage tabTitle;
        private CustomShapeEditor titleEditor;
        private NumericUpDown UDColWi;
        private NumericUpDown UDLeft;
        private NumericUpDown UDMargin;
        private NumericUpDown UDTop;
        private NumericUpDown UDTopPos;
        private NumericUpDown UDVertSpacing;

        public LegendEditor()
        {
            this.InitializeComponent();
            this.CBLegStyle.Items.Add("Plain");
            this.CBLegStyle.Items.Add("Left Value");
            this.CBLegStyle.Items.Add("Right Value");
            this.CBLegStyle.Items.Add("Left Percent");
            this.CBLegStyle.Items.Add("Right Percent");
            this.CBLegStyle.Items.Add("X Value");
            this.CBLegStyle.Items.Add("Value");
            this.CBLegStyle.Items.Add("Percent");
            this.CBLegStyle.Items.Add("X and Value");
            this.CBLegStyle.Items.Add("X and Percent");
            this.CBLegendStyle.Items.Add("Automatic");
            this.CBLegendStyle.Items.Add("Series Names");
            this.CBLegendStyle.Items.Add("Series Values");
            this.CBLegendStyle.Items.Add("Last Values");
            this.CBLegendStyle.Items.Add("Palette");
            this.CBSymbolPos.Items.Add("Left");
            this.CBSymbolPos.Items.Add("Right");
            this.CBColWUnits.Items.Add("Percent");
            this.CBColWUnits.Items.Add("Pixels");
            this.cbTextAlign.Items.Add("Left");
            this.cbTextAlign.Items.Add("Center");
            this.cbTextAlign.Items.Add("Right");
        }

        public LegendEditor(Chart c, Control parent) : this(c, c.Legend, parent)
        {
        }

        public LegendEditor(Chart c, Legend l, Control parent) : this()
        {
            this.setting = true;
            this.chart = c;
            this.legend = l;
            this.legendTitle = l.Title;
            this.shapeEditor = CustomShapeEditor.Add(this.tabControl1, this.legend);
            this.titleEditor = CustomShapeEditor.Add(this.tabControl2, this.legendTitle);
            EditorUtils.InsertForm(this, parent);
            this.CBBoxes.Checked = this.legend.CheckBoxes;
            this.CBColWUnits.SelectedIndex = (int) this.legend.Symbol.WidthUnits;
            this.CBContinuous.Checked = this.legend.Symbol.Continous;
            this.CBCustPos.Checked = this.legend.CustomPosition;
            this.CBFontColor.Checked = this.legend.FontSeriesColor;
            this.CBInverted.Checked = this.legend.Inverted;
            this.CBLegendStyle.SelectedIndex = (int) this.legend.LegendStyle;
            this.CBLegStyle.SelectedIndex = (int) this.legend.TextStyle;
            this.CBResizeChart.Checked = this.legend.ResizeChart;
            this.CBShow.Checked = this.legend.Visible;
            this.cbVisible.Checked = this.legend.Symbol.Visible;
            this.CBSymbolPos.SelectedIndex = (int) this.legend.Symbol.Position;
            this.bSymPen.Pen = this.legend.Symbol.Pen;
            this.BDivLines.Pen = this.legend.DividingLines;
            this.UDColWi.Value = this.legend.Symbol.Width;
            this.UDLeft.Value = this.legend.Left;
            this.UDTop.Value = this.legend.Top;
            this.UDTopPos.Value = this.legend.TopLeftPos;
            this.UDVertSpacing.Value = this.legend.VertSpacing;
            this.EnableMarginControls();
            this.EnableCustomPosition();
            this.EnableWidthControls();
            this.MemoText.Text = this.legendTitle.Text;
            switch (this.legendTitle.TextAlign)
            {
                case StringAlignment.Near:
                    this.cbTextAlign.SelectedIndex = 0;
                    break;

                case StringAlignment.Center:
                    this.cbTextAlign.SelectedIndex = 1;
                    break;

                default:
                    this.cbTextAlign.SelectedIndex = 2;
                    break;
            }
            this.cbTitleVisible.Checked = this.legendTitle.Visible;
            if (this.legend.Alignment == LegendAlignments.Left)
            {
                this.radioButton1.Checked = true;
            }
            else if (this.legend.Alignment == LegendAlignments.Top)
            {
                this.radioButton2.Checked = true;
            }
            else if (this.legend.Alignment == LegendAlignments.Right)
            {
                this.radioButton3.Checked = true;
            }
            else
            {
                this.radioButton4.Checked = true;
            }
            this.setting = false;
        }

        private void bShadow_Click(object sender, EventArgs e)
        {
            ShadowEditor c = new ShadowEditor(this.legend.Symbol.Shadow);
            EditorUtils.Translate(c);
            EditorUtils.ShowFormModal(c, this);
        }

        private void CBBoxes_CheckedChanged(object sender, EventArgs e)
        {
            this.legend.CheckBoxes = this.CBBoxes.Checked;
        }

        private void CBColWUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CBColWUnits.SelectedIndex == 0)
            {
                this.legend.Symbol.WidthUnits = LegendSymbolSize.Percent;
            }
            else
            {
                this.legend.Symbol.WidthUnits = LegendSymbolSize.Pixels;
            }
        }

        private void CBContinuous_CheckedChanged(object sender, EventArgs e)
        {
            this.legend.Symbol.Continous = this.CBContinuous.Checked;
        }

        private void CBCustPos_CheckedChanged(object sender, EventArgs e)
        {
            this.legend.CustomPosition = this.CBCustPos.Checked;
            this.EnableCustomPosition();
        }

        private void CBFontColor_CheckedChanged(object sender, EventArgs e)
        {
            this.legend.FontSeriesColor = this.CBFontColor.Checked;
        }

        private void CBInverted_CheckedChanged(object sender, EventArgs e)
        {
            this.legend.Inverted = this.CBInverted.Checked;
        }

        private void CBLegendStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.CBLegendStyle.SelectedIndex)
            {
                case 0:
                    this.legend.LegendStyle = LegendStyles.Auto;
                    return;

                case 1:
                    this.legend.LegendStyle = LegendStyles.Series;
                    return;

                case 2:
                    this.legend.LegendStyle = LegendStyles.Values;
                    return;

                case 3:
                    this.legend.LegendStyle = LegendStyles.LastValues;
                    return;
            }
            this.legend.LegendStyle = LegendStyles.Palette;
        }

        private void CBLegStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.CBLegStyle.SelectedIndex)
            {
                case 0:
                    this.legend.TextStyle = LegendTextStyles.Plain;
                    return;

                case 1:
                    this.legend.TextStyle = LegendTextStyles.LeftValue;
                    return;

                case 2:
                    this.legend.TextStyle = LegendTextStyles.RightValue;
                    return;

                case 3:
                    this.legend.TextStyle = LegendTextStyles.LeftPercent;
                    return;

                case 4:
                    this.legend.TextStyle = LegendTextStyles.RightPercent;
                    return;

                case 5:
                    this.legend.TextStyle = LegendTextStyles.XValue;
                    return;

                case 6:
                    this.legend.TextStyle = LegendTextStyles.Value;
                    return;

                case 7:
                    this.legend.TextStyle = LegendTextStyles.Percent;
                    return;

                case 8:
                    this.legend.TextStyle = LegendTextStyles.XAndValue;
                    return;

                case 9:
                    this.legend.TextStyle = LegendTextStyles.XAndPercent;
                    return;
            }
            this.legend.TextStyle = LegendTextStyles.Value;
        }

        private void CBResizeChart_CheckedChanged(object sender, EventArgs e)
        {
            this.legend.ResizeChart = this.CBResizeChart.Checked;
        }

        private void CBShow_CheckedChanged(object sender, EventArgs e)
        {
            this.legend.Visible = this.CBShow.Checked;
        }

        private void cbSquared_CheckedChanged(object sender, EventArgs e)
        {
            this.legend.Symbol.Squared = this.cbSquared.Checked;
            this.EnableWidthControls();
        }

        private void CBSymbolPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.legend.Symbol.Position = (LegendSymbolPosition) this.CBSymbolPos.SelectedIndex;
        }

        private void cbSymPen_CheckedChanged(object sender, EventArgs e)
        {
            this.legend.Symbol.DefaultPen = this.cbSymPen.Checked;
            this.bSymPen.Enabled = !this.cbSymPen.Checked;
        }

        private void cbTextAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cbTextAlign.SelectedIndex)
            {
                case 0:
                    this.legendTitle.TextAlign = StringAlignment.Near;
                    return;

                case 1:
                    this.legendTitle.TextAlign = StringAlignment.Center;
                    return;
            }
            this.legendTitle.TextAlign = StringAlignment.Far;
        }

        private void cbTitleVisible_Click(object sender, EventArgs e)
        {
            this.legendTitle.Visible = this.cbTitleVisible.Checked;
        }

        private void cbVisible_CheckedChanged(object sender, EventArgs e)
        {
            this.legend.Symbol.Visible = this.cbVisible.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableCustomPosition()
        {
            bool customPosition = this.legend.CustomPosition;
            this.UDLeft.Enabled = customPosition;
            this.UDTop.Enabled = customPosition;
            if (customPosition)
            {
                this.UDLeft.Value = this.legend.Left;
                this.UDTop.Value = this.legend.Top;
            }
            this.CBResizeChart.Enabled = !customPosition;
            this.UDMargin.Enabled = !customPosition;
            this.UDTopPos.Enabled = !customPosition;
        }

        private void EnableMarginControls()
        {
            this.UDMargin.Value = this.legend.Vertical ? this.legend.HorizMargin : this.legend.VertMargin;
        }

        private void EnableWidthControls()
        {
            this.UDColWi.Enabled = !this.legend.Symbol.Squared;
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabStyle = new TabPage();
            this.BDivLines = new ButtonPen();
            this.UDVertSpacing = new NumericUpDown();
            this.CBLegStyle = new ComboBox();
            this.CBLegendStyle = new ComboBox();
            this.label7 = new Label();
            this.label6 = new Label();
            this.label1 = new Label();
            this.CBFontColor = new CheckBox();
            this.CBBoxes = new CheckBox();
            this.CBInverted = new CheckBox();
            this.CBShow = new CheckBox();
            this.tabPosition = new TabPage();
            this.groupBox2 = new GroupBox();
            this.UDTop = new NumericUpDown();
            this.UDLeft = new NumericUpDown();
            this.label4 = new Label();
            this.label3 = new Label();
            this.CBCustPos = new CheckBox();
            this.UDTopPos = new NumericUpDown();
            this.UDMargin = new NumericUpDown();
            this.label2 = new Label();
            this.label5 = new Label();
            this.CBResizeChart = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.radioButton4 = new RadioButton();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.tabSymbols = new TabPage();
            this.bSymPen = new ButtonPen();
            this.cbSquared = new CheckBox();
            this.cbSymPen = new CheckBox();
            this.cbVisible = new CheckBox();
            this.CBContinuous = new CheckBox();
            this.UDColWi = new NumericUpDown();
            this.CBSymbolPos = new ComboBox();
            this.CBColWUnits = new ComboBox();
            this.label10 = new Label();
            this.label9 = new Label();
            this.label8 = new Label();
            this.tabTitle = new TabPage();
            this.tabControl2 = new TabControl();
            this.tabPage1 = new TabPage();
            this.cbTitleVisible = new CheckBox();
            this.cbTextAlign = new ComboBox();
            this.label12 = new Label();
            this.MemoText = new TextBox();
            this.label13 = new Label();
            this.bShadow = new Button();
            this.tabControl1.SuspendLayout();
            this.tabStyle.SuspendLayout();
            this.tabPosition.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabSymbols.SuspendLayout();
            this.UDVertSpacing.BeginInit();
            this.UDTop.BeginInit();
            this.UDLeft.BeginInit();
            this.UDTopPos.BeginInit();
            this.UDMargin.BeginInit();
            this.UDColWi.BeginInit();
            this.tabTitle.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabStyle);
            this.tabControl1.Controls.Add(this.tabPosition);
            this.tabControl1.Controls.Add(this.tabSymbols);
            this.tabControl1.Controls.Add(this.tabTitle);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x176, 0xe0);
            this.tabControl1.TabIndex = 0;
            this.tabStyle.Controls.Add(this.BDivLines);
            this.tabStyle.Controls.Add(this.UDVertSpacing);
            this.tabStyle.Controls.Add(this.CBLegStyle);
            this.tabStyle.Controls.Add(this.CBLegendStyle);
            this.tabStyle.Controls.Add(this.label7);
            this.tabStyle.Controls.Add(this.label6);
            this.tabStyle.Controls.Add(this.label1);
            this.tabStyle.Controls.Add(this.CBFontColor);
            this.tabStyle.Controls.Add(this.CBBoxes);
            this.tabStyle.Controls.Add(this.CBInverted);
            this.tabStyle.Controls.Add(this.CBShow);
            this.tabStyle.Location = new Point(4, 0x16);
            this.tabStyle.Name = "tabStyle";
            this.tabStyle.Size = new Size(0x16e, 0xc6);
            this.tabStyle.TabIndex = 0;
            this.tabStyle.Text = "Style";
            this.BDivLines.FlatStyle = FlatStyle.Flat;
            this.BDivLines.Location = new Point(0xb0, 0x74);
            this.BDivLines.Name = "BDivLines";
            this.BDivLines.Size = new Size(0x90, 0x17);
            this.BDivLines.TabIndex = 10;
            this.BDivLines.Text = "&Dividing Lines...";
            this.UDVertSpacing.BorderStyle = BorderStyle.FixedSingle;
            this.UDVertSpacing.Location = new Point(240, 0x49);
            int[] bits = new int[4];
            bits[0] = 10;
            bits[3] = -2147483648;
            this.UDVertSpacing.Minimum = new decimal(bits);
            this.UDVertSpacing.Name = "UDVertSpacing";
            this.UDVertSpacing.Size = new Size(0x38, 20);
            this.UDVertSpacing.TabIndex = 8;
            this.UDVertSpacing.TextAlign = HorizontalAlignment.Right;
            this.UDVertSpacing.ValueChanged += new EventHandler(this.UDVertSpacing_ValueChanged);
            this.UDVertSpacing.TextChanged += new EventHandler(this.UDVertSpacing_ValueChanged);
            this.CBLegStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBLegStyle.Location = new Point(0xc4, 40);
            this.CBLegStyle.Name = "CBLegStyle";
            this.CBLegStyle.Size = new Size(140, 0x15);
            this.CBLegStyle.TabIndex = 5;
            this.CBLegStyle.SelectedIndexChanged += new EventHandler(this.CBLegStyle_SelectedIndexChanged);
            this.CBLegendStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBLegendStyle.Location = new Point(0xc4, 8);
            this.CBLegendStyle.Name = "CBLegendStyle";
            this.CBLegendStyle.Size = new Size(140, 0x15);
            this.CBLegendStyle.TabIndex = 2;
            this.CBLegendStyle.SelectedIndexChanged += new EventHandler(this.CBLegendStyle_SelectedIndexChanged);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(160, 0x4b);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x4a, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "V&ert. Spacing:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x87, 0x2a);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x39, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Text St&yle:";
            this.label6.TextAlign = ContentAlignment.TopRight;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(120, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Legend &Style:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.CBFontColor.FlatStyle = FlatStyle.Flat;
            this.CBFontColor.Location = new Point(0x18, 0x6c);
            this.CBFontColor.Name = "CBFontColor";
            this.CBFontColor.Size = new Size(0x98, 0x10);
            this.CBFontColor.TabIndex = 9;
            this.CBFontColor.Text = "&Font Series Color";
            this.CBFontColor.CheckedChanged += new EventHandler(this.CBFontColor_CheckedChanged);
            this.CBBoxes.FlatStyle = FlatStyle.Flat;
            this.CBBoxes.Location = new Point(0x18, 0x4c);
            this.CBBoxes.Name = "CBBoxes";
            this.CBBoxes.Size = new Size(120, 0x10);
            this.CBBoxes.TabIndex = 6;
            this.CBBoxes.Text = "C&heck boxes";
            this.CBBoxes.CheckedChanged += new EventHandler(this.CBBoxes_CheckedChanged);
            this.CBInverted.FlatStyle = FlatStyle.Flat;
            this.CBInverted.Location = new Point(0x18, 0x2c);
            this.CBInverted.Name = "CBInverted";
            this.CBInverted.Size = new Size(0x48, 0x10);
            this.CBInverted.TabIndex = 3;
            this.CBInverted.Text = "&Inverted";
            this.CBInverted.CheckedChanged += new EventHandler(this.CBInverted_CheckedChanged);
            this.CBShow.Checked = true;
            this.CBShow.CheckState = CheckState.Checked;
            this.CBShow.FlatStyle = FlatStyle.Flat;
            this.CBShow.Location = new Point(0x18, 12);
            this.CBShow.Name = "CBShow";
            this.CBShow.Size = new Size(0x48, 0x10);
            this.CBShow.TabIndex = 0;
            this.CBShow.Text = "&Visible";
            this.CBShow.CheckedChanged += new EventHandler(this.CBShow_CheckedChanged);
            this.tabPosition.Controls.Add(this.groupBox2);
            this.tabPosition.Controls.Add(this.UDTopPos);
            this.tabPosition.Controls.Add(this.UDMargin);
            this.tabPosition.Controls.Add(this.label2);
            this.tabPosition.Controls.Add(this.label5);
            this.tabPosition.Controls.Add(this.CBResizeChart);
            this.tabPosition.Controls.Add(this.groupBox1);
            this.tabPosition.Location = new Point(4, 0x16);
            this.tabPosition.Name = "tabPosition";
            this.tabPosition.Size = new Size(0x16e, 0xc6);
            this.tabPosition.TabIndex = 1;
            this.tabPosition.Text = "Position";
            this.groupBox2.Controls.Add(this.UDTop);
            this.groupBox2.Controls.Add(this.UDLeft);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.CBCustPos);
            this.groupBox2.Location = new Point(0x13, 0x88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(300, 0x30);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.UDTop.BorderStyle = BorderStyle.FixedSingle;
            int[] numArray2 = new int[4];
            numArray2[0] = 5;
            this.UDTop.Increment = new decimal(numArray2);
            this.UDTop.Location = new Point(0xe8, 0x10);
            int[] numArray3 = new int[4];
            numArray3[0] = 0x7d0;
            this.UDTop.Maximum = new decimal(numArray3);
            int[] numArray4 = new int[4];
            numArray4[0] = 100;
            numArray4[3] = -2147483648;
            this.UDTop.Minimum = new decimal(numArray4);
            this.UDTop.Name = "UDTop";
            this.UDTop.Size = new Size(0x34, 20);
            this.UDTop.TabIndex = 4;
            this.UDTop.TextAlign = HorizontalAlignment.Right;
            this.UDTop.ValueChanged += new EventHandler(this.UDTop_ValueChanged);
            this.UDTop.TextChanged += new EventHandler(this.UDTop_ValueChanged);
            this.UDLeft.BorderStyle = BorderStyle.FixedSingle;
            int[] numArray5 = new int[4];
            numArray5[0] = 5;
            this.UDLeft.Increment = new decimal(numArray5);
            this.UDLeft.Location = new Point(0x88, 0x10);
            int[] numArray6 = new int[4];
            numArray6[0] = 0x7d0;
            this.UDLeft.Maximum = new decimal(numArray6);
            int[] numArray7 = new int[4];
            numArray7[0] = 100;
            numArray7[3] = -2147483648;
            this.UDLeft.Minimum = new decimal(numArray7);
            this.UDLeft.Name = "UDLeft";
            this.UDLeft.Size = new Size(0x34, 20);
            this.UDLeft.TabIndex = 2;
            this.UDLeft.TextAlign = HorizontalAlignment.Right;
            this.UDLeft.ValueChanged += new EventHandler(this.UDLeft_ValueChanged);
            this.UDLeft.TextChanged += new EventHandler(this.UDLeft_ValueChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(200, 0x12);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1d, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "T&op:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x6a, 0x12);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1c, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "L&eft:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.CBCustPos.FlatStyle = FlatStyle.Flat;
            this.CBCustPos.Location = new Point(0x10, 0x13);
            this.CBCustPos.Name = "CBCustPos";
            this.CBCustPos.Size = new Size(0x58, 0x10);
            this.CBCustPos.TabIndex = 0;
            this.CBCustPos.Text = "&Custom:";
            this.CBCustPos.CheckedChanged += new EventHandler(this.CBCustPos_CheckedChanged);
            this.UDTopPos.BorderStyle = BorderStyle.FixedSingle;
            this.UDTopPos.Location = new Point(0xdf, 0x60);
            int[] numArray8 = new int[4];
            numArray8[0] = 100;
            numArray8[3] = -2147483648;
            this.UDTopPos.Minimum = new decimal(numArray8);
            this.UDTopPos.Name = "UDTopPos";
            this.UDTopPos.Size = new Size(0x40, 20);
            this.UDTopPos.TabIndex = 5;
            this.UDTopPos.TextAlign = HorizontalAlignment.Right;
            this.UDTopPos.ValueChanged += new EventHandler(this.UDTopPos_ValueChanged);
            this.UDTopPos.TextChanged += new EventHandler(this.UDTopPos_ValueChanged);
            this.UDMargin.BorderStyle = BorderStyle.FixedSingle;
            this.UDMargin.Location = new Point(0xdf, 0x40);
            int[] numArray9 = new int[4];
            numArray9[0] = 0x3e8;
            this.UDMargin.Maximum = new decimal(numArray9);
            int[] numArray10 = new int[4];
            numArray10[0] = 0x3e8;
            numArray10[3] = -2147483648;
            this.UDMargin.Minimum = new decimal(numArray10);
            this.UDMargin.Name = "UDMargin";
            this.UDMargin.Size = new Size(0x40, 20);
            this.UDMargin.TabIndex = 3;
            this.UDMargin.TextAlign = HorizontalAlignment.Right;
            this.UDMargin.ValueChanged += new EventHandler(this.UDMargin_ValueChanged);
            this.UDMargin.TextChanged += new EventHandler(this.UDMargin_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x83, 0x62);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x59, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "&Position Offset %:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0xb1, 0x42);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x2a, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "&Margin:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.CBResizeChart.Checked = true;
            this.CBResizeChart.CheckState = CheckState.Checked;
            this.CBResizeChart.FlatStyle = FlatStyle.Flat;
            this.CBResizeChart.Location = new Point(0xdf, 40);
            this.CBResizeChart.Name = "CBResizeChart";
            this.CBResizeChart.Size = new Size(0x70, 0x10);
            this.CBResizeChart.TabIndex = 1;
            this.CBResizeChart.Text = "Resi&ze Chart";
            this.CBResizeChart.CheckedChanged += new EventHandler(this.CBResizeChart_CheckedChanged);
            this.groupBox1.Controls.Add(this.radioButton4);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new Point(0x12, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x5d, 0x70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position";
            this.radioButton4.FlatStyle = FlatStyle.Flat;
            this.radioButton4.Location = new Point(0x10, 0x58);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new Size(0x48, 0x10);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "&Bottom";
            this.radioButton4.CheckedChanged += new EventHandler(this.radioButton4_CheckedChanged);
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(0x10, 0x40);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x48, 0x10);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "&Right";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(0x10, 40);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x48, 0x10);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&Top";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(0x10, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x48, 0x10);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "&Left";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.tabSymbols.Controls.Add(this.bShadow);
            this.tabSymbols.Controls.Add(this.bSymPen);
            this.tabSymbols.Controls.Add(this.cbSquared);
            this.tabSymbols.Controls.Add(this.cbSymPen);
            this.tabSymbols.Controls.Add(this.cbVisible);
            this.tabSymbols.Controls.Add(this.CBContinuous);
            this.tabSymbols.Controls.Add(this.UDColWi);
            this.tabSymbols.Controls.Add(this.CBSymbolPos);
            this.tabSymbols.Controls.Add(this.CBColWUnits);
            this.tabSymbols.Controls.Add(this.label10);
            this.tabSymbols.Controls.Add(this.label9);
            this.tabSymbols.Controls.Add(this.label8);
            this.tabSymbols.Location = new Point(4, 0x16);
            this.tabSymbols.Name = "tabSymbols";
            this.tabSymbols.Size = new Size(0x16e, 0xc6);
            this.tabSymbols.TabIndex = 2;
            this.tabSymbols.Text = "Symbols";
            this.bSymPen.FlatStyle = FlatStyle.Flat;
            this.bSymPen.Location = new Point(0xe0, 0x40);
            this.bSymPen.Name = "bSymPen";
            this.bSymPen.Size = new Size(0x4b, 0x17);
            this.bSymPen.TabIndex = 10;
            this.bSymPen.Text = "&Border...";
            this.cbSquared.FlatStyle = FlatStyle.Flat;
            this.cbSquared.Location = new Point(0x60, 0x88);
            this.cbSquared.Name = "cbSquared";
            this.cbSquared.Size = new Size(0x68, 0x18);
            this.cbSquared.TabIndex = 9;
            this.cbSquared.Text = "&Squared";
            this.cbSquared.CheckedChanged += new EventHandler(this.cbSquared_CheckedChanged);
            this.cbSymPen.FlatStyle = FlatStyle.Flat;
            this.cbSymPen.Location = new Point(200, 0x20);
            this.cbSymPen.Name = "cbSymPen";
            this.cbSymPen.Size = new Size(0x68, 0x18);
            this.cbSymPen.TabIndex = 3;
            this.cbSymPen.Text = "D&efault border";
            this.cbSymPen.CheckedChanged += new EventHandler(this.cbSymPen_CheckedChanged);
            this.cbVisible.FlatStyle = FlatStyle.Flat;
            this.cbVisible.Location = new Point(0x60, 7);
            this.cbVisible.Name = "cbVisible";
            this.cbVisible.Size = new Size(0x68, 0x18);
            this.cbVisible.TabIndex = 0;
            this.cbVisible.Text = "&Visible";
            this.cbVisible.CheckedChanged += new EventHandler(this.cbVisible_CheckedChanged);
            this.CBContinuous.FlatStyle = FlatStyle.Flat;
            this.CBContinuous.Location = new Point(0x60, 120);
            this.CBContinuous.Name = "CBContinuous";
            this.CBContinuous.Size = new Size(0x60, 0x10);
            this.CBContinuous.TabIndex = 8;
            this.CBContinuous.Text = "&Continuous";
            this.CBContinuous.CheckedChanged += new EventHandler(this.CBContinuous_CheckedChanged);
            this.UDColWi.BorderStyle = BorderStyle.FixedSingle;
            int[] numArray11 = new int[4];
            numArray11[0] = 5;
            this.UDColWi.Increment = new decimal(numArray11);
            this.UDColWi.Location = new Point(0x60, 0x20);
            this.UDColWi.Name = "UDColWi";
            this.UDColWi.Size = new Size(0x38, 20);
            this.UDColWi.TabIndex = 2;
            this.UDColWi.TextAlign = HorizontalAlignment.Right;
            this.UDColWi.ValueChanged += new EventHandler(this.UDColWi_ValueChanged);
            this.UDColWi.TextChanged += new EventHandler(this.UDColWi_ValueChanged);
            this.CBSymbolPos.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBSymbolPos.Location = new Point(0x60, 0x58);
            this.CBSymbolPos.Name = "CBSymbolPos";
            this.CBSymbolPos.Size = new Size(0x60, 0x15);
            this.CBSymbolPos.TabIndex = 7;
            this.CBSymbolPos.SelectedIndexChanged += new EventHandler(this.CBSymbolPos_SelectedIndexChanged);
            this.CBColWUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBColWUnits.Location = new Point(0x60, 60);
            this.CBColWUnits.Name = "CBColWUnits";
            this.CBColWUnits.Size = new Size(0x60, 0x15);
            this.CBColWUnits.TabIndex = 5;
            this.CBColWUnits.SelectedIndexChanged += new EventHandler(this.CBColWUnits_SelectedIndexChanged);
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x31, 90);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x2f, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "&Position:";
            this.label10.TextAlign = ContentAlignment.TopRight;
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x20, 0x3e);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x41, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Width &Units:";
            this.label9.TextAlign = ContentAlignment.TopRight;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x3d, 0x22);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x26, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "&Width:";
            this.label8.TextAlign = ContentAlignment.TopRight;
            this.tabTitle.Controls.Add(this.tabControl2);
            this.tabTitle.Location = new Point(4, 0x16);
            this.tabTitle.Name = "tabTitle";
            this.tabTitle.Size = new Size(0x16e, 0xc6);
            this.tabTitle.TabIndex = 3;
            this.tabTitle.Text = "Title";
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Dock = DockStyle.Fill;
            this.tabControl2.Location = new Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new Size(0x16e, 0xc6);
            this.tabControl2.TabIndex = 0;
            this.tabPage1.Controls.Add(this.cbTitleVisible);
            this.tabPage1.Controls.Add(this.cbTextAlign);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.MemoText);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x166, 0xac);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Options";
            this.cbTitleVisible.Location = new Point(0x99, 0x93);
            this.cbTitleVisible.Name = "cbTitleVisible";
            this.cbTitleVisible.Size = new Size(0x6a, 0x11);
            this.cbTitleVisible.TabIndex = 4;
            this.cbTitleVisible.Text = "Visible";
            this.cbTitleVisible.Click += new EventHandler(this.cbTitleVisible_Click);
            this.cbTextAlign.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbTextAlign.Location = new Point(0x10, 0x90);
            this.cbTextAlign.Name = "cbTextAlign";
            this.cbTextAlign.Size = new Size(0x79, 0x15);
            this.cbTextAlign.TabIndex = 3;
            this.cbTextAlign.SelectedIndexChanged += new EventHandler(this.cbTextAlign_SelectedIndexChanged);
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0x10, 0x80);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x4f, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Text &alignment:";
            this.MemoText.BorderStyle = BorderStyle.FixedSingle;
            this.MemoText.Location = new Point(0x10, 0x20);
            this.MemoText.Multiline = true;
            this.MemoText.Name = "MemoText";
            this.MemoText.Size = new Size(0xe8, 0x58);
            this.MemoText.TabIndex = 1;
            this.MemoText.TextChanged += new EventHandler(this.MemoText_TextChanged);
            this.label13.AutoSize = true;
            this.label13.Location = new Point(0x10, 10);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x1f, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "&Text:";
            this.bShadow.FlatStyle = FlatStyle.Flat;
            this.bShadow.Location = new Point(0xe0, 90);
            this.bShadow.Name = "bShadow";
            this.bShadow.Size = new Size(0x4b, 0x17);
            this.bShadow.TabIndex = 11;
            this.bShadow.Text = "S&hadow...";
            this.bShadow.UseVisualStyleBackColor = true;
            this.bShadow.Click += new EventHandler(this.bShadow_Click);
            base.ClientSize = new Size(0x176, 0xe0);
            base.Controls.Add(this.tabControl1);
            base.Name = "LegendEditor";
            this.tabControl1.ResumeLayout(false);
            this.tabStyle.ResumeLayout(false);
            this.tabStyle.PerformLayout();
            this.tabPosition.ResumeLayout(false);
            this.tabPosition.PerformLayout();
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabSymbols.ResumeLayout(false);
            this.tabSymbols.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.UDVertSpacing.EndInit();
            this.UDTop.EndInit();
            this.UDLeft.EndInit();
            this.UDTopPos.EndInit();
            this.UDMargin.EndInit();
            this.UDColWi.EndInit();
            this.tabTitle.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void MemoText_TextChanged(object sender, EventArgs e)
        {
            this.legendTitle.Text = this.MemoText.Text;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.legend.Alignment = LegendAlignments.Left;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.legend.Alignment = LegendAlignments.Top;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.legend.Alignment = LegendAlignments.Right;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.legend.Alignment = LegendAlignments.Bottom;
            }
        }

        private void UDColWi_ValueChanged(object sender, EventArgs e)
        {
            this.legend.Symbol.Width = (int) this.UDColWi.Value;
        }

        private void UDLeft_ValueChanged(object sender, EventArgs e)
        {
            this.legend.Left = Convert.ToInt32(this.UDLeft.Value);
        }

        private void UDMargin_ValueChanged(object sender, EventArgs e)
        {
            if (this.legend.Vertical)
            {
                this.legend.HorizMargin = (int) this.UDMargin.Value;
            }
            else
            {
                this.legend.VertMargin = (int) this.UDMargin.Value;
            }
        }

        private void UDTop_ValueChanged(object sender, EventArgs e)
        {
            this.legend.Top = Convert.ToInt32(this.UDTop.Value);
        }

        private void UDTopPos_ValueChanged(object sender, EventArgs e)
        {
            this.legend.TopLeftPos = (int) this.UDTopPos.Value;
        }

        private void UDVertSpacing_ValueChanged(object sender, EventArgs e)
        {
            this.legend.VertSpacing = (int) this.UDVertSpacing.Value;
        }
    }
}

