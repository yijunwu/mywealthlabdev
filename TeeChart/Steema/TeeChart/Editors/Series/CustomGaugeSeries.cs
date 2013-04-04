namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Themes;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CustomGaugeSeries : BaseSeriesForm
    {
        private Button bFaceBrush;
        private Button BFont;
        private Button bGreenLine;
        protected Button bHand;
        private Button bInnerBand;
        private Button bMiddleBand;
        private Button bMinorTicks;
        private Button bMinus;
        private Button bOuterBand;
        private Button bPlus;
        private Button bRedLine;
        private Button bTicks;
        private CheckBox CBLabels;
        protected ComboBox CBPalettes;
        private Container components;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label8;
        private Label label9;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        private NumericUpDown numericUpDown4;
        protected CustomGauge series;
        protected TabControl tabControl1;
        protected TabPage tabFrame;
        protected TabPage tabGreenLine;
        protected TabPage tabLabels;
        protected TabPage tabOptions;
        protected TabPage tabRedLine;
        protected TabPage tabTicks;
        private TextBox tbValue;
        private TextBox textBox1;
        private NumericUpDown UDGreenLineEnd;
        private NumericUpDown UDGreenLineStart;
        private NumericUpDown UDMax;
        private NumericUpDown UDMin;
        private NumericUpDown UDRedLineEnd;
        private NumericUpDown UDRedLineStart;

        public CustomGaugeSeries()
        {
            this.InitializeComponent();
        }

        public CustomGaugeSeries(Series s) : this()
        {
            this.series = (CustomGauge) s;
        }

        private void bFaceBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.FaceBrush);
        }

        private void BFont_Click(object sender, EventArgs e)
        {
            EditorUtils.EditFont(this.series.Axis.Labels.Font);
        }

        private void bGreenLine_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editors.GaugeSeriesPointer f = new Steema.TeeChart.Editors.GaugeSeriesPointer(this.series.GreenLine, false);
            EditorUtils.ShowFormModal(f);
        }

        private void bHand_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editors.GaugeSeriesPointer f = new Steema.TeeChart.Editors.GaugeSeriesPointer(this.series.Hand, false);
            EditorUtils.ShowFormModal(f);
        }

        private void bInnerBand_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Frame.InnerBand);
        }

        private void bMiddleBand_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Frame.MiddleBand);
        }

        private void bMinorTicks_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editors.GaugeSeriesPointer f = new Steema.TeeChart.Editors.GaugeSeriesPointer(this.series.MinorTicks);
            EditorUtils.ShowFormModal(f);
        }

        private void bMinus_Click(object sender, EventArgs e)
        {
            this.InValue(-1);
        }

        private void bOuterBand_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Frame.OuterBand);
        }

        private void bPlus_Click(object sender, EventArgs e)
        {
            this.InValue(1);
        }

        private void bRedLine_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editors.GaugeSeriesPointer f = new Steema.TeeChart.Editors.GaugeSeriesPointer(this.series.RedLine, false);
            EditorUtils.ShowFormModal(f);
        }

        private void bTicks_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editors.GaugeSeriesPointer f = new Steema.TeeChart.Editors.GaugeSeriesPointer(this.series.Ticks);
            EditorUtils.ShowFormModal(f);
        }

        private void CBLabels_Click(object sender, EventArgs e)
        {
            this.series.Axis.Labels.Visible = this.CBLabels.Checked;
            this.series.Invalidate();
        }

        private void CBPalettes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.SetGaugeColorPalette(this.CBPalettes.SelectedIndex);
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
            this.tabControl1 = new TabControl();
            this.tabOptions = new TabPage();
            this.label10 = new Label();
            this.CBPalettes = new ComboBox();
            this.bFaceBrush = new Button();
            this.bHand = new Button();
            this.label1 = new Label();
            this.tabTicks = new TabPage();
            this.numericUpDown2 = new NumericUpDown();
            this.bMinorTicks = new Button();
            this.bTicks = new Button();
            this.numericUpDown4 = new NumericUpDown();
            this.numericUpDown3 = new NumericUpDown();
            this.numericUpDown1 = new NumericUpDown();
            this.UDMin = new NumericUpDown();
            this.UDMax = new NumericUpDown();
            this.label6 = new Label();
            this.label5 = new Label();
            this.tabLabels = new TabPage();
            this.textBox1 = new TextBox();
            this.label4 = new Label();
            this.BFont = new Button();
            this.CBLabels = new CheckBox();
            this.tabRedLine = new TabPage();
            this.UDRedLineEnd = new NumericUpDown();
            this.UDRedLineStart = new NumericUpDown();
            this.label9 = new Label();
            this.label8 = new Label();
            this.bRedLine = new Button();
            this.tabGreenLine = new TabPage();
            this.UDGreenLineEnd = new NumericUpDown();
            this.UDGreenLineStart = new NumericUpDown();
            this.label11 = new Label();
            this.label12 = new Label();
            this.bGreenLine = new Button();
            this.tabFrame = new TabPage();
            this.bInnerBand = new Button();
            this.bMiddleBand = new Button();
            this.bOuterBand = new Button();
            this.tbValue = new TextBox();
            this.bPlus = new Button();
            this.bMinus = new Button();
            this.tabControl1.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.tabTicks.SuspendLayout();
            this.numericUpDown2.BeginInit();
            this.numericUpDown4.BeginInit();
            this.numericUpDown3.BeginInit();
            this.numericUpDown1.BeginInit();
            this.UDMin.BeginInit();
            this.UDMax.BeginInit();
            this.tabLabels.SuspendLayout();
            this.tabRedLine.SuspendLayout();
            this.UDRedLineEnd.BeginInit();
            this.UDRedLineStart.BeginInit();
            this.tabGreenLine.SuspendLayout();
            this.UDGreenLineEnd.BeginInit();
            this.UDGreenLineStart.BeginInit();
            this.tabFrame.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabOptions);
            this.tabControl1.Controls.Add(this.tabTicks);
            this.tabControl1.Controls.Add(this.tabLabels);
            this.tabControl1.Controls.Add(this.tabRedLine);
            this.tabControl1.Controls.Add(this.tabGreenLine);
            this.tabControl1.Controls.Add(this.tabFrame);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x158, 0xb6);
            this.tabControl1.TabIndex = 0;
            this.tabOptions.Controls.Add(this.bMinus);
            this.tabOptions.Controls.Add(this.bPlus);
            this.tabOptions.Controls.Add(this.tbValue);
            this.tabOptions.Controls.Add(this.label10);
            this.tabOptions.Controls.Add(this.CBPalettes);
            this.tabOptions.Controls.Add(this.bFaceBrush);
            this.tabOptions.Controls.Add(this.bHand);
            this.tabOptions.Controls.Add(this.label1);
            this.tabOptions.Location = new Point(4, 0x16);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new Size(0x150, 0x9c);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.Text = "Options";
            this.tabOptions.UseVisualStyleBackColor = true;
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x94, 0x57);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x30, 13);
            this.label10.TabIndex = 0x16;
            this.label10.Text = "Palettes:";
            this.CBPalettes.FormattingEnabled = true;
            this.CBPalettes.Location = new Point(0xca, 0x53);
            this.CBPalettes.Name = "CBPalettes";
            this.CBPalettes.Size = new Size(0x79, 0x15);
            this.CBPalettes.TabIndex = 0x15;
            this.CBPalettes.Text = "BlackPalette";
            this.CBPalettes.SelectedIndexChanged += new EventHandler(this.CBPalettes_SelectedIndexChanged);
            this.bFaceBrush.FlatStyle = FlatStyle.Flat;
            this.bFaceBrush.Location = new Point(8, 0x41);
            this.bFaceBrush.Name = "bFaceBrush";
            this.bFaceBrush.Size = new Size(0x56, 0x17);
            this.bFaceBrush.TabIndex = 20;
            this.bFaceBrush.Text = "FaceBrush...";
            this.bFaceBrush.UseVisualStyleBackColor = true;
            this.bFaceBrush.Click += new EventHandler(this.bFaceBrush_Click);
            this.bHand.FlatStyle = FlatStyle.Flat;
            this.bHand.Location = new Point(8, 0x24);
            this.bHand.Name = "bHand";
            this.bHand.Size = new Size(0x4b, 0x17);
            this.bHand.TabIndex = 0x10;
            this.bHand.Text = "Hand...";
            this.bHand.UseVisualStyleBackColor = true;
            this.bHand.Click += new EventHandler(this.bHand_Click);
            this.label1.Location = new Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x38, 0x10);
            this.label1.TabIndex = 4;
            this.label1.Text = "Value:";
            this.tabTicks.Controls.Add(this.numericUpDown2);
            this.tabTicks.Controls.Add(this.bMinorTicks);
            this.tabTicks.Controls.Add(this.bTicks);
            this.tabTicks.Controls.Add(this.numericUpDown4);
            this.tabTicks.Controls.Add(this.numericUpDown3);
            this.tabTicks.Controls.Add(this.numericUpDown1);
            this.tabTicks.Controls.Add(this.UDMin);
            this.tabTicks.Controls.Add(this.UDMax);
            this.tabTicks.Controls.Add(this.label6);
            this.tabTicks.Controls.Add(this.label5);
            this.tabTicks.Location = new Point(4, 0x16);
            this.tabTicks.Name = "tabTicks";
            this.tabTicks.Size = new Size(0x150, 0x9c);
            this.tabTicks.TabIndex = 2;
            this.tabTicks.Text = "Ticks";
            this.tabTicks.UseVisualStyleBackColor = true;
            this.numericUpDown2.Location = new Point(0xd0, 0x12);
            int[] bits = new int[4];
            bits[0] = 100;
            bits[3] = -2147483648;
            this.numericUpDown2.Minimum = new decimal(bits);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x40, 20);
            this.numericUpDown2.TabIndex = 12;
            this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.bMinorTicks.FlatStyle = FlatStyle.Flat;
            this.bMinorTicks.Location = new Point(0x11, 0x2e);
            this.bMinorTicks.Name = "bMinorTicks";
            this.bMinorTicks.Size = new Size(0x54, 0x17);
            this.bMinorTicks.TabIndex = 11;
            this.bMinorTicks.Text = "Minor Ticks...";
            this.bMinorTicks.UseVisualStyleBackColor = true;
            this.bMinorTicks.Click += new EventHandler(this.bMinorTicks_Click);
            this.bTicks.FlatStyle = FlatStyle.Flat;
            this.bTicks.Location = new Point(0x11, 15);
            this.bTicks.Name = "bTicks";
            this.bTicks.Size = new Size(0x54, 0x17);
            this.bTicks.TabIndex = 10;
            this.bTicks.Text = "Ticks...";
            this.bTicks.UseVisualStyleBackColor = true;
            this.bTicks.Click += new EventHandler(this.bTicks_Click);
            this.numericUpDown4.Location = new Point(0xd0, 50);
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new Size(0x40, 20);
            this.numericUpDown4.TabIndex = 9;
            this.numericUpDown4.ValueChanged += new EventHandler(this.numericUpDown4_ValueChanged);
            this.numericUpDown4.TextChanged += new EventHandler(this.numericUpDown4_ValueChanged);
            this.numericUpDown3.Location = new Point(0x80, 0x31);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new Size(0x40, 20);
            this.numericUpDown3.TabIndex = 8;
            this.numericUpDown3.ValueChanged += new EventHandler(this.numericUpDown3_ValueChanged);
            this.numericUpDown3.TextChanged += new EventHandler(this.numericUpDown3_ValueChanged);
            this.numericUpDown1.Location = new Point(0x80, 0x12);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x40, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.UDMin.Location = new Point(0x4f, 0x6f);
            int[] numArray2 = new int[4];
            numArray2[0] = 0x3e8;
            this.UDMin.Maximum = new decimal(numArray2);
            this.UDMin.Name = "UDMin";
            this.UDMin.Size = new Size(0x40, 20);
            this.UDMin.TabIndex = 5;
            this.UDMin.ValueChanged += new EventHandler(this.UDMin_ValueChanged);
            this.UDMin.TextChanged += new EventHandler(this.UDMin_ValueChanged);
            this.UDMax.Location = new Point(0x4f, 0x57);
            int[] numArray3 = new int[4];
            numArray3[0] = 0x3e8;
            this.UDMax.Maximum = new decimal(numArray3);
            this.UDMax.Name = "UDMax";
            this.UDMax.Size = new Size(0x40, 20);
            this.UDMax.TabIndex = 4;
            this.UDMax.ValueChanged += new EventHandler(this.UDMax_ValueChanged);
            this.UDMax.TextChanged += new EventHandler(this.UDMax_ValueChanged);
            this.label6.Location = new Point(0x18, 0x70);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x38, 0x18);
            this.label6.TabIndex = 3;
            this.label6.Text = "Minimum:";
            this.label5.Location = new Point(0x18, 0x58);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x38, 0x17);
            this.label5.TabIndex = 2;
            this.label5.Text = "Maximum:";
            this.tabLabels.Controls.Add(this.textBox1);
            this.tabLabels.Controls.Add(this.label4);
            this.tabLabels.Controls.Add(this.BFont);
            this.tabLabels.Controls.Add(this.CBLabels);
            this.tabLabels.Location = new Point(4, 0x16);
            this.tabLabels.Name = "tabLabels";
            this.tabLabels.Size = new Size(0x150, 0x9c);
            this.tabLabels.TabIndex = 1;
            this.tabLabels.Text = "Labels";
            this.tabLabels.UseVisualStyleBackColor = true;
            this.textBox1.Location = new Point(0x20, 0x72);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(100, 20);
            this.textBox1.TabIndex = 9;
            this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.label4.Location = new Point(0x18, 0x62);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x68, 0x10);
            this.label4.TabIndex = 8;
            this.label4.Text = "Format:";
            this.BFont.FlatStyle = FlatStyle.Flat;
            this.BFont.Location = new Point(0x97, 14);
            this.BFont.Name = "BFont";
            this.BFont.Size = new Size(0x4b, 0x17);
            this.BFont.TabIndex = 7;
            this.BFont.Text = "Font...";
            this.BFont.Click += new EventHandler(this.BFont_Click);
            this.CBLabels.FlatStyle = FlatStyle.Flat;
            this.CBLabels.Location = new Point(0x18, 13);
            this.CBLabels.Name = "CBLabels";
            this.CBLabels.Size = new Size(0x68, 0x18);
            this.CBLabels.TabIndex = 5;
            this.CBLabels.Text = "Show Labels";
            this.CBLabels.Click += new EventHandler(this.CBLabels_Click);
            this.tabRedLine.Controls.Add(this.UDRedLineEnd);
            this.tabRedLine.Controls.Add(this.UDRedLineStart);
            this.tabRedLine.Controls.Add(this.label9);
            this.tabRedLine.Controls.Add(this.label8);
            this.tabRedLine.Controls.Add(this.bRedLine);
            this.tabRedLine.Location = new Point(4, 0x16);
            this.tabRedLine.Name = "tabRedLine";
            this.tabRedLine.Size = new Size(0x150, 0x9c);
            this.tabRedLine.TabIndex = 3;
            this.tabRedLine.Text = "Red Line";
            this.tabRedLine.UseVisualStyleBackColor = true;
            this.UDRedLineEnd.Location = new Point(0x4b, 0x4f);
            this.UDRedLineEnd.Name = "UDRedLineEnd";
            this.UDRedLineEnd.Size = new Size(0x37, 20);
            this.UDRedLineEnd.TabIndex = 0x18;
            this.UDRedLineEnd.ValueChanged += new EventHandler(this.UDRedLineEnd_ValueChanged);
            this.UDRedLineStart.Location = new Point(0x4b, 0x36);
            this.UDRedLineStart.Name = "UDRedLineStart";
            this.UDRedLineStart.Size = new Size(0x37, 20);
            this.UDRedLineStart.TabIndex = 0x17;
            this.UDRedLineStart.ValueChanged += new EventHandler(this.UDRedLineStart_ValueChanged);
            this.label9.AutoSize = true;
            this.label9.Location = new Point(8, 0x51);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x3a, 13);
            this.label9.TabIndex = 0x16;
            this.label9.Text = "End value:";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(8, 0x38);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x3d, 13);
            this.label8.TabIndex = 0x15;
            this.label8.Text = "Start value:";
            this.bRedLine.FlatStyle = FlatStyle.Flat;
            this.bRedLine.Location = new Point(8, 15);
            this.bRedLine.Name = "bRedLine";
            this.bRedLine.Size = new Size(0x4b, 0x17);
            this.bRedLine.TabIndex = 20;
            this.bRedLine.Text = "Style...";
            this.bRedLine.UseVisualStyleBackColor = true;
            this.bRedLine.Click += new EventHandler(this.bRedLine_Click);
            this.tabGreenLine.Controls.Add(this.UDGreenLineEnd);
            this.tabGreenLine.Controls.Add(this.UDGreenLineStart);
            this.tabGreenLine.Controls.Add(this.label11);
            this.tabGreenLine.Controls.Add(this.label12);
            this.tabGreenLine.Controls.Add(this.bGreenLine);
            this.tabGreenLine.Location = new Point(4, 0x16);
            this.tabGreenLine.Name = "tabGreenLine";
            this.tabGreenLine.Size = new Size(0x150, 0x9c);
            this.tabGreenLine.TabIndex = 5;
            this.tabGreenLine.Text = "Green Line";
            this.tabGreenLine.UseVisualStyleBackColor = true;
            this.UDGreenLineEnd.Location = new Point(0x4b, 0x4f);
            this.UDGreenLineEnd.Name = "UDGreenLineEnd";
            this.UDGreenLineEnd.Size = new Size(0x37, 20);
            this.UDGreenLineEnd.TabIndex = 0x1d;
            this.UDGreenLineEnd.ValueChanged += new EventHandler(this.UDGreenLineEnd_ValueChanged);
            this.UDGreenLineStart.Location = new Point(0x4b, 0x36);
            this.UDGreenLineStart.Name = "UDGreenLineStart";
            this.UDGreenLineStart.Size = new Size(0x37, 20);
            this.UDGreenLineStart.TabIndex = 0x1c;
            this.UDGreenLineStart.ValueChanged += new EventHandler(this.UDGreenLineStart_ValueChanged);
            this.label11.AutoSize = true;
            this.label11.Location = new Point(8, 0x51);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x3a, 13);
            this.label11.TabIndex = 0x1b;
            this.label11.Text = "End value:";
            this.label12.AutoSize = true;
            this.label12.Location = new Point(8, 0x38);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x3d, 13);
            this.label12.TabIndex = 0x1a;
            this.label12.Text = "Start value:";
            this.bGreenLine.FlatStyle = FlatStyle.Flat;
            this.bGreenLine.Location = new Point(8, 15);
            this.bGreenLine.Name = "bGreenLine";
            this.bGreenLine.Size = new Size(0x4b, 0x17);
            this.bGreenLine.TabIndex = 0x19;
            this.bGreenLine.Text = "Style...";
            this.bGreenLine.UseVisualStyleBackColor = true;
            this.bGreenLine.Click += new EventHandler(this.bGreenLine_Click);
            this.tabFrame.Controls.Add(this.bInnerBand);
            this.tabFrame.Controls.Add(this.bMiddleBand);
            this.tabFrame.Controls.Add(this.bOuterBand);
            this.tabFrame.Location = new Point(4, 0x16);
            this.tabFrame.Name = "tabFrame";
            this.tabFrame.Size = new Size(0x150, 0x9c);
            this.tabFrame.TabIndex = 4;
            this.tabFrame.Text = "Frame";
            this.tabFrame.UseVisualStyleBackColor = true;
            this.bInnerBand.FlatStyle = FlatStyle.Flat;
            this.bInnerBand.Location = new Point(0x12, 0x4c);
            this.bInnerBand.Name = "bInnerBand";
            this.bInnerBand.Size = new Size(0x5c, 0x17);
            this.bInnerBand.TabIndex = 2;
            this.bInnerBand.Text = "Inner Band...";
            this.bInnerBand.UseVisualStyleBackColor = true;
            this.bInnerBand.Click += new EventHandler(this.bInnerBand_Click);
            this.bMiddleBand.FlatStyle = FlatStyle.Flat;
            this.bMiddleBand.Location = new Point(0x12, 0x2f);
            this.bMiddleBand.Name = "bMiddleBand";
            this.bMiddleBand.Size = new Size(0x5c, 0x17);
            this.bMiddleBand.TabIndex = 1;
            this.bMiddleBand.Text = "Middle Band...";
            this.bMiddleBand.UseVisualStyleBackColor = true;
            this.bMiddleBand.Click += new EventHandler(this.bMiddleBand_Click);
            this.bOuterBand.FlatStyle = FlatStyle.Flat;
            this.bOuterBand.Location = new Point(0x12, 0x12);
            this.bOuterBand.Name = "bOuterBand";
            this.bOuterBand.Size = new Size(0x5c, 0x17);
            this.bOuterBand.TabIndex = 0;
            this.bOuterBand.Text = "Outer Band...";
            this.bOuterBand.UseVisualStyleBackColor = true;
            this.bOuterBand.Click += new EventHandler(this.bOuterBand_Click);
            this.tbValue.Location = new Point(0x41, 6);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new Size(0x45, 20);
            this.tbValue.TabIndex = 0x17;
            this.tbValue.TextChanged += new EventHandler(this.tbValue_TextChanged);
            this.bPlus.FlatStyle = FlatStyle.Flat;
            this.bPlus.Location = new Point(140, 4);
            this.bPlus.Name = "bPlus";
            this.bPlus.Size = new Size(0x18, 0x18);
            this.bPlus.TabIndex = 0x18;
            this.bPlus.Text = "+";
            this.bPlus.UseVisualStyleBackColor = true;
            this.bPlus.Click += new EventHandler(this.bPlus_Click);
            this.bMinus.FlatStyle = FlatStyle.Flat;
            this.bMinus.Location = new Point(170, 4);
            this.bMinus.Name = "bMinus";
            this.bMinus.Size = new Size(0x18, 0x18);
            this.bMinus.TabIndex = 0x19;
            this.bMinus.Text = "-";
            this.bMinus.UseVisualStyleBackColor = true;
            this.bMinus.Click += new EventHandler(this.bMinus_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x158, 0xb6);
            base.Controls.Add(this.tabControl1);
            base.Name = "CustomGaugeSeries";
            this.Text = "Custom Gauge Editor";
            this.tabControl1.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.tabOptions.PerformLayout();
            this.tabTicks.ResumeLayout(false);
            this.numericUpDown2.EndInit();
            this.numericUpDown4.EndInit();
            this.numericUpDown3.EndInit();
            this.numericUpDown1.EndInit();
            this.UDMin.EndInit();
            this.UDMax.EndInit();
            this.tabLabels.ResumeLayout(false);
            this.tabLabels.PerformLayout();
            this.tabRedLine.ResumeLayout(false);
            this.tabRedLine.PerformLayout();
            this.UDRedLineEnd.EndInit();
            this.UDRedLineStart.EndInit();
            this.tabGreenLine.ResumeLayout(false);
            this.tabGreenLine.PerformLayout();
            this.UDGreenLineEnd.EndInit();
            this.UDGreenLineStart.EndInit();
            this.tabFrame.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void InValue(int p)
        {
            this.series.Value += p;
            this.tbValue.Text = this.series.Value.ToString();
            this.series.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.Ticks.VertSize = (int) this.numericUpDown1.Value;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.MinorTickDistance = (int) this.numericUpDown2.Value;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.Axis.MinorTickCount = (int) this.numericUpDown3.Value;
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.MinorTicks.VertSize = (int) this.numericUpDown4.Value;
                this.series.MinorTicks.HorizSize = (int) this.numericUpDown4.Value;
            }
        }

        protected virtual void SetGaugeColorPalette(int index)
        {
            switch (index)
            {
                case 1:
                    this.series.GaugeColorPalette = CustomGauge.BlackPalette;
                    return;

                case 2:
                    this.series.GaugeColorPalette = CustomGauge.BluesPalette;
                    return;

                case 3:
                    this.series.GaugeColorPalette = Theme.TeeChartPalette;
                    return;

                case 4:
                    this.series.GaugeColorPalette = Theme.ExcelPalette;
                    return;

                case 5:
                    this.series.GaugeColorPalette = Theme.ClassicPalette;
                    return;

                case 6:
                    this.series.GaugeColorPalette = Theme.WindowsXPPalette;
                    return;

                case 7:
                    this.series.GaugeColorPalette = Theme.WebPalette;
                    return;

                case 8:
                    this.series.GaugeColorPalette = Theme.VictorianPalette;
                    return;

                case 9:
                    this.series.GaugeColorPalette = Theme.PastelsPalette;
                    return;

                case 10:
                    this.series.GaugeColorPalette = Theme.GrayscalePalette;
                    return;

                case 11:
                    this.series.GaugeColorPalette = Theme.SolidPalette;
                    return;

                case 12:
                    this.series.GaugeColorPalette = Theme.ModernPalette;
                    return;

                case 13:
                    this.series.GaugeColorPalette = Theme.RainbowPalette;
                    return;
            }
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.series != null)
            {
                this.tbValue.Text = this.series.Value.ToString();
                this.UDRedLineStart.Value = (decimal) this.series.RedLineStartValue;
                this.UDRedLineEnd.Value = (decimal) this.series.RedLineEndValue;
                this.UDGreenLineStart.Value = (decimal) this.series.GreenLineStartValue;
                this.UDGreenLineEnd.Value = (decimal) this.series.GreenLineEndValue;
                this.CBLabels.Checked = this.series.GetVertAxis.Labels.Visible;
                this.textBox1.Text = this.series.GetVertAxis.Labels.ValueFormat;
                this.numericUpDown1.Value = this.series.Ticks.VertSize;
                this.numericUpDown3.Value = this.series.Axis.MinorTickCount;
                this.numericUpDown4.Value = this.series.MinorTicks.VertSize;
                if (double.IsPositiveInfinity(this.series.Maximum))
                {
                    this.UDMax.Enabled = false;
                }
                else
                {
                    this.UDMax.Value = (decimal) this.series.Maximum;
                }
                this.UDMin.Value = (decimal) this.series.Minimum;
                int count = Theme.ColorPalettes.Count;
                this.CBPalettes.Items.Add("Current");
                this.CBPalettes.Items.Add("BlackPalette");
                this.CBPalettes.Items.Add("BluesPalette");
                for (int i = 2; i < (count + 2); i++)
                {
                    this.CBPalettes.Items.Add(Theme.ColorPalettes[i - 2]);
                }
                this.CBPalettes.SelectedIndex = 0;
            }
        }

        private void tbValue_TextChanged(object sender, EventArgs e)
        {
            this.series.Value = Convert.ToDouble(this.tbValue.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.series.GetVertAxis.Labels.ValueFormat = this.textBox1.Text;
        }

        private void UDGreenLineEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.GreenLineEndValue = (int) this.UDGreenLineEnd.Value;
            }
        }

        private void UDGreenLineStart_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.GreenLineStartValue = (int) this.UDGreenLineStart.Value;
            }
        }

        private void UDMax_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.Maximum = (double) this.UDMax.Value;
            }
        }

        private void UDMin_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.Minimum = (double) this.UDMin.Value;
            }
        }

        private void UDRedLineEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.RedLineEndValue = (int) this.UDRedLineEnd.Value;
            }
        }

        private void UDRedLineStart_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.RedLineStartValue = (int) this.UDRedLineStart.Value;
            }
        }
    }
}

