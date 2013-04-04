namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Themes;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CircularGaugeSeries : BaseSeriesForm
    {
        private Button bCenter;
        private Button bEndPoint;
        private Button bFaceBrush;
        private Button BFont;
        private Button bGreenLine;
        private Button bHand;
        private Button bInnerBand;
        private Button bMiddleBand;
        private Button bMinorTicks;
        private Button bOuterBand;
        private Button bRedLine;
        private Button bTicks;
        private CheckBox CBInside;
        private CheckBox CBLabels;
        private ComboBox CBPalettes;
        private CheckBox CBRotateLabels;
        private CircledSeries circledEditor;
        private Container components;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        private NumericUpDown numericUpDown4;
        private CircularGauge series;
        private TabControl tabControl1;
        private TabPage tabFrame;
        private TabPage tabGreenLine;
        private TabPage tabLabels;
        private TabPage tabOptions;
        private TabPage tabRedLine;
        private TabPage tabTicks;
        private TextBox textBox1;
        private NumericUpDown UDDistance;
        private NumericUpDown UDGreenLineEnd;
        private NumericUpDown UDGreenLineStart;
        private NumericUpDown UDMax;
        private NumericUpDown UDMin;
        private NumericUpDown UDOffset;
        private NumericUpDown UDRedLineEnd;
        private NumericUpDown UDRedLineStart;
        private NumericUpDown UDTotalAngle;
        private NumericUpDown UDValue;

        public CircularGaugeSeries()
        {
            this.InitializeComponent();
        }

        public CircularGaugeSeries(Series s) : this()
        {
            this.series = (CircularGauge) s;
        }

        private void bCenter_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editors.GaugeSeriesPointer f = new Steema.TeeChart.Editors.GaugeSeriesPointer(this.series.Center);
            EditorUtils.ShowFormModal(f);
        }

        private void bEndPoint_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editors.SeriesPointer f = new Steema.TeeChart.Editors.SeriesPointer(this.series.EndPoint);
            EditorUtils.ShowFormModal(f);
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
            Steema.TeeChart.Editors.GaugeSeriesPointer f = new Steema.TeeChart.Editors.GaugeSeriesPointer(this.series.Hand);
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

        private void bOuterBand_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Frame.OuterBand);
        }

        private void BPMinor_Click(object sender, EventArgs e)
        {
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

        private void CBInside_Click(object sender, EventArgs e)
        {
            this.series.LabelsInside = this.CBInside.Checked;
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
                switch (this.CBPalettes.SelectedIndex)
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
                        break;

                    default:
                        return;
                }
            }
        }

        private void CBRotateLabels_Click(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.RotateLabels = this.CBRotateLabels.Checked;
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
            this.bEndPoint = new Button();
            this.bCenter = new Button();
            this.bHand = new Button();
            this.label7 = new Label();
            this.UDOffset = new NumericUpDown();
            this.UDDistance = new NumericUpDown();
            this.UDTotalAngle = new NumericUpDown();
            this.UDValue = new NumericUpDown();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.tabTicks = new TabPage();
            this.bMinorTicks = new Button();
            this.bTicks = new Button();
            this.numericUpDown4 = new NumericUpDown();
            this.numericUpDown3 = new NumericUpDown();
            this.numericUpDown2 = new NumericUpDown();
            this.numericUpDown1 = new NumericUpDown();
            this.UDMin = new NumericUpDown();
            this.UDMax = new NumericUpDown();
            this.label6 = new Label();
            this.label5 = new Label();
            this.tabLabels = new TabPage();
            this.CBRotateLabels = new CheckBox();
            this.textBox1 = new TextBox();
            this.label4 = new Label();
            this.BFont = new Button();
            this.CBInside = new CheckBox();
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
            this.tabControl1.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.UDOffset.BeginInit();
            this.UDDistance.BeginInit();
            this.UDTotalAngle.BeginInit();
            this.UDValue.BeginInit();
            this.tabTicks.SuspendLayout();
            this.numericUpDown4.BeginInit();
            this.numericUpDown3.BeginInit();
            this.numericUpDown2.BeginInit();
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
            this.tabOptions.Controls.Add(this.label10);
            this.tabOptions.Controls.Add(this.CBPalettes);
            this.tabOptions.Controls.Add(this.bFaceBrush);
            this.tabOptions.Controls.Add(this.bEndPoint);
            this.tabOptions.Controls.Add(this.bCenter);
            this.tabOptions.Controls.Add(this.bHand);
            this.tabOptions.Controls.Add(this.label7);
            this.tabOptions.Controls.Add(this.UDOffset);
            this.tabOptions.Controls.Add(this.UDDistance);
            this.tabOptions.Controls.Add(this.UDTotalAngle);
            this.tabOptions.Controls.Add(this.UDValue);
            this.tabOptions.Controls.Add(this.label3);
            this.tabOptions.Controls.Add(this.label2);
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
            this.bFaceBrush.Location = new Point(0xe5, 0x24);
            this.bFaceBrush.Name = "bFaceBrush";
            this.bFaceBrush.Size = new Size(0x56, 0x17);
            this.bFaceBrush.TabIndex = 20;
            this.bFaceBrush.Text = "FaceBrush...";
            this.bFaceBrush.UseVisualStyleBackColor = true;
            this.bFaceBrush.Click += new EventHandler(this.bFaceBrush_Click);
            this.bEndPoint.FlatStyle = FlatStyle.Flat;
            this.bEndPoint.Location = new Point(0x94, 0x24);
            this.bEndPoint.Name = "bEndPoint";
            this.bEndPoint.Size = new Size(0x4b, 0x17);
            this.bEndPoint.TabIndex = 0x12;
            this.bEndPoint.Text = "EndPoint...";
            this.bEndPoint.UseVisualStyleBackColor = true;
            this.bEndPoint.Click += new EventHandler(this.bEndPoint_Click);
            this.bCenter.FlatStyle = FlatStyle.Flat;
            this.bCenter.Location = new Point(0xe5, 7);
            this.bCenter.Name = "bCenter";
            this.bCenter.Size = new Size(0x4b, 0x17);
            this.bCenter.TabIndex = 0x11;
            this.bCenter.Text = "Center...";
            this.bCenter.UseVisualStyleBackColor = true;
            this.bCenter.Click += new EventHandler(this.bCenter_Click);
            this.bHand.FlatStyle = FlatStyle.Flat;
            this.bHand.Location = new Point(0x94, 7);
            this.bHand.Name = "bHand";
            this.bHand.Size = new Size(0x4b, 0x17);
            this.bHand.TabIndex = 0x10;
            this.bHand.Text = "Hand...";
            this.bHand.UseVisualStyleBackColor = true;
            this.bHand.Click += new EventHandler(this.bHand_Click);
            this.label7.Location = new Point(3, 0x57);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x4c, 0x11);
            this.label7.TabIndex = 15;
            this.label7.Text = "Offset:";
            this.UDOffset.Location = new Point(0x4e, 0x55);
            this.UDOffset.Name = "UDOffset";
            this.UDOffset.Size = new Size(0x40, 20);
            this.UDOffset.TabIndex = 14;
            this.UDOffset.ValueChanged += new EventHandler(this.UDOffset_ValueChanged);
            this.UDDistance.Location = new Point(0x4e, 0x3b);
            this.UDDistance.Name = "UDDistance";
            this.UDDistance.Size = new Size(0x40, 20);
            this.UDDistance.TabIndex = 9;
            this.UDDistance.ValueChanged += new EventHandler(this.UDDistance_ValueChanged);
            this.UDDistance.TextChanged += new EventHandler(this.UDDistance_ValueChanged);
            this.UDTotalAngle.Location = new Point(0x4e, 0x21);
            int[] bits = new int[4];
            bits[0] = 360;
            this.UDTotalAngle.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.UDTotalAngle.Minimum = new decimal(numArray2);
            this.UDTotalAngle.Name = "UDTotalAngle";
            this.UDTotalAngle.Size = new Size(0x40, 20);
            this.UDTotalAngle.TabIndex = 8;
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.UDTotalAngle.Value = new decimal(numArray3);
            this.UDTotalAngle.ValueChanged += new EventHandler(this.UDTotalAngle_ValueChanged);
            this.UDTotalAngle.TextChanged += new EventHandler(this.UDTotalAngle_ValueChanged);
            this.UDValue.Location = new Point(0x4e, 7);
            this.UDValue.Name = "UDValue";
            this.UDValue.Size = new Size(0x40, 20);
            this.UDValue.TabIndex = 7;
            this.UDValue.ValueChanged += new EventHandler(this.UDValue_ValueChanged);
            this.UDValue.TextChanged += new EventHandler(this.UDValue_ValueChanged);
            this.label3.Location = new Point(3, 0x3d);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x40, 0x10);
            this.label3.TabIndex = 6;
            this.label3.Text = "Distance:";
            this.label2.Location = new Point(3, 0x23);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4c, 0x11);
            this.label2.TabIndex = 5;
            this.label2.Text = "Total Angle:";
            this.label1.Location = new Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x38, 0x10);
            this.label1.TabIndex = 4;
            this.label1.Text = "Value:";
            this.tabTicks.Controls.Add(this.bMinorTicks);
            this.tabTicks.Controls.Add(this.bTicks);
            this.tabTicks.Controls.Add(this.numericUpDown4);
            this.tabTicks.Controls.Add(this.numericUpDown3);
            this.tabTicks.Controls.Add(this.numericUpDown2);
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
            this.numericUpDown2.Location = new Point(0xd0, 0x12);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x40, 20);
            this.numericUpDown2.TabIndex = 7;
            this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown2.TextChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown1.Location = new Point(0x80, 0x12);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x40, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.UDMin.Location = new Point(0x4f, 0x6f);
            int[] numArray4 = new int[4];
            numArray4[0] = 0x3e8;
            this.UDMin.Maximum = new decimal(numArray4);
            this.UDMin.Name = "UDMin";
            this.UDMin.Size = new Size(0x40, 20);
            this.UDMin.TabIndex = 5;
            this.UDMin.ValueChanged += new EventHandler(this.UDMin_ValueChanged);
            this.UDMin.TextChanged += new EventHandler(this.UDMin_ValueChanged);
            this.UDMax.Location = new Point(0x4f, 0x57);
            int[] numArray5 = new int[4];
            numArray5[0] = 0x3e8;
            this.UDMax.Maximum = new decimal(numArray5);
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
            this.tabLabels.Controls.Add(this.CBRotateLabels);
            this.tabLabels.Controls.Add(this.textBox1);
            this.tabLabels.Controls.Add(this.label4);
            this.tabLabels.Controls.Add(this.BFont);
            this.tabLabels.Controls.Add(this.CBInside);
            this.tabLabels.Controls.Add(this.CBLabels);
            this.tabLabels.Location = new Point(4, 0x16);
            this.tabLabels.Name = "tabLabels";
            this.tabLabels.Size = new Size(0x150, 0x9c);
            this.tabLabels.TabIndex = 1;
            this.tabLabels.Text = "Labels";
            this.tabLabels.UseVisualStyleBackColor = true;
            this.CBRotateLabels.FlatStyle = FlatStyle.Flat;
            this.CBRotateLabels.Location = new Point(0x18, 0x35);
            this.CBRotateLabels.Name = "CBRotateLabels";
            this.CBRotateLabels.Size = new Size(0x68, 0x18);
            this.CBRotateLabels.TabIndex = 10;
            this.CBRotateLabels.Text = "Rotate Labels";
            this.CBRotateLabels.Click += new EventHandler(this.CBRotateLabels_Click);
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
            this.CBInside.FlatStyle = FlatStyle.Flat;
            this.CBInside.Location = new Point(0x18, 0x21);
            this.CBInside.Name = "CBInside";
            this.CBInside.Size = new Size(0x68, 0x18);
            this.CBInside.TabIndex = 6;
            this.CBInside.Text = "Inside";
            this.CBInside.Click += new EventHandler(this.CBInside_Click);
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
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x158, 0xb6);
            base.Controls.Add(this.tabControl1);
            base.Name = "CircularGaugeSeries";
            this.Text = "Circular Gauge Editor";
            this.tabControl1.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.tabOptions.PerformLayout();
            this.UDOffset.EndInit();
            this.UDDistance.EndInit();
            this.UDTotalAngle.EndInit();
            this.UDValue.EndInit();
            this.tabTicks.ResumeLayout(false);
            this.numericUpDown4.EndInit();
            this.numericUpDown3.EndInit();
            this.numericUpDown2.EndInit();
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
                this.series.GetVertAxis.MinorTickCount = (int) this.numericUpDown3.Value;
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

        public override void SetParent(TabPage Parent)
        {
            if (this.series != null)
            {
                if (this.circledEditor == null)
                {
                    this.circledEditor = CircledSeries.InsertForm(Parent, this.series);
                    this.circledEditor.BBack.Visible = false;
                    this.circledEditor.BBGrad.Visible = false;
                }
                this.UDValue.Value = (decimal) this.series.Value;
                this.UDTotalAngle.Value = (decimal) this.series.TotalAngle;
                this.UDDistance.Value = this.series.HandDistance;
                this.UDOffset.Value = this.series.HandOffset;
                this.UDRedLineStart.Value = (decimal) this.series.RedLineStartValue;
                this.UDRedLineEnd.Value = (decimal) this.series.RedLineEndValue;
                this.UDGreenLineStart.Value = (decimal) this.series.GreenLineStartValue;
                this.UDGreenLineEnd.Value = (decimal) this.series.GreenLineEndValue;
                this.CBLabels.Checked = this.series.GetVertAxis.Labels.Visible;
                this.CBInside.Checked = this.series.LabelsInside;
                this.CBRotateLabels.Checked = this.series.RotateLabels;
                this.textBox1.Text = this.series.GetVertAxis.Labels.ValueFormat;
                this.numericUpDown1.Value = this.series.Ticks.VertSize;
                this.numericUpDown2.Value = this.series.MinorTickDistance;
                this.numericUpDown3.Value = this.series.Axis.MinorTickCount;
                this.numericUpDown4.Value = this.series.MinorTicks.VertSize;
                this.UDMax.Value = (decimal) this.series.Maximum;
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.series.GetVertAxis.Labels.ValueFormat = this.textBox1.Text;
        }

        private void UDDistance_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.HandDistance = (int) this.UDDistance.Value;
            }
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

        private void UDOffset_ValueChanged(object sender, EventArgs e)
        {
            this.series.HandOffset = (int) this.UDOffset.Value;
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

        private void UDTotalAngle_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.TotalAngle = (double) this.UDTotalAngle.Value;
            }
        }

        private void UDValue_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.Value = (double) this.UDValue.Value;
            }
        }
    }
}

