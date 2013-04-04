namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ContourSeries : BaseSeriesForm
    {
        private Button BLevelPen;
        private ButtonPen Button2;
        private ButtonColor buttonColorMarksFont;
        private ButtonPen buttonPenFrame;
        private CheckBox CBAntiOverlap;
        private CheckBox CBAtSegments;
        private CheckBox CBAutoLevels;
        private CheckBox CBColorEach;
        private CheckBox CBDefaultPen;
        private CheckBox CBEmpty;
        private CheckBox CBFilled;
        private CheckBox cboxMarksFontColor;
        private CheckBox cboxMarksVisible;
        private CheckBox CBYPosLevel;
        private List<ContourLevel> cLevels;
        private ComboBox comboBoxDrawing;
        private Container components;
        private TextBox EValue;
        private Grid3DSeries grid3DEditor;
        private GroupBox groupBox1;
        private HScrollBar hSBLevel;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Steema.TeeChart.Editors.SeriesPointer peditor;
        private Contour series;
        private bool setting;
        private System.Windows.Forms.Panel SHColor;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private NumericUpDown UDLevel;
        private NumericUpDown UDMarksDensity;
        private NumericUpDown UDMarksMargin;
        private NumericUpDown UDNum;
        private NumericUpDown UDYPos;

        public ContourSeries()
        {
            this.setting = true;
            this.InitializeComponent();
        }

        public ContourSeries(Series s) : this()
        {
            this.series = (Contour) s;
            this.setting = true;
            this.UDYPos.Value = Convert.ToDecimal(this.series.YPosition);
            this.UDLevel.Value = 0M;
            this.UDNum.Value = Convert.ToDecimal(this.series.NumLevels);
            this.UDLevel.Maximum = Convert.ToDecimal((int) (this.series.NumLevels - 1));
            this.hSBLevel.Maximum = (int) this.UDLevel.Maximum;
            this.CBYPosLevel.Checked = this.series.YPositionLevel;
            this.CBAutoLevels.Checked = this.series.AutomaticLevels;
            this.UDNum.Enabled = this.CBAutoLevels.Checked;
            this.CBColorEach.Checked = this.series.ColorEach;
            this.CBColorEach.Enabled = this.CBAutoLevels.Checked;
            this.CBFilled.Checked = this.series.FillLevels;
            this.Button2.Pen = this.series.Pen;
            this.buttonPenFrame.Pen = this.series.Frame;
            this.cboxMarksVisible.Checked = this.series.ContourMarks.Visible;
            this.cboxMarksFontColor.Checked = this.series.ContourMarks.ColorLevel;
            this.UDMarksDensity.Value = this.series.ContourMarks.Density;
            this.UDMarksMargin.Value = this.series.ContourMarks.Margin;
            this.CBAtSegments.Checked = this.series.ContourMarks.AtSegments;
            this.CBAntiOverlap.Checked = this.series.ContourMarks.AntiOverlap;
            this.buttonColorMarksFont.Color = this.series.Marks.Font.Color;
            this.comboBoxDrawing.SelectedIndex = (int) this.series.DrawingAlgorithm;
            if (this.peditor == null)
            {
                this.peditor = new Steema.TeeChart.Editors.SeriesPointer(this.series.Pointer, this.tabPage4);
            }
            this.SetLevel();
            this.setting = false;
        }

        private void BLevelPen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.Level.Pen);
            this.SHColor.BackColor = this.VisualColor;
        }

        private void buttonColorMarksFont_Click(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.Marks.Font.Color = this.buttonColorMarksFont.Color;
            }
        }

        private void CBAntiOverlap_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.ContourMarks.AntiOverlap = this.CBAntiOverlap.Checked;
            }
        }

        private void CBAtSegments_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.ContourMarks.AtSegments = this.CBAtSegments.Checked;
            }
        }

        private void CBAutoLevels_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.AutomaticLevels = this.CBAutoLevels.Checked;
                Utils.EnableControls(this.series.AutomaticLevels, new Control[] { this.UDLevel, this.UDNum, this.CBColorEach });
                this.SetLevel();
            }
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.ColorEach = this.CBColorEach.Checked;
                this.series.CreateAutoLevels();
                this.SetLevel();
            }
        }

        private void CBDefaultPen_Click(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.BLevelPen.Enabled = !this.CBDefaultPen.Checked;
                if (this.CBDefaultPen.Checked)
                {
                    this.Level.Pen = null;
                }
            }
        }

        private void CBEmpty_Click(object sender, EventArgs e)
        {
            if (this.CBEmpty.Checked)
            {
                this.Level.Color = Color.Transparent;
                this.Level.Pen.Color = this.Level.Color;
            }
            else
            {
                this.Level.Pen.Color = Utils.EmptyColor;
                this.cLevels = new List<ContourLevel>(this.series.Levels);
                this.series.AutomaticLevels = true;
                this.series.CreateAutoLevels();
                for (int i = 0; i < this.cLevels.Count; i++)
                {
                    if (this.cLevels[i].Pen.Color != Utils.EmptyColor)
                    {
                        this.series.Levels[i].Pen.Color = this.cLevels[i].Pen.Color;
                    }
                }
            }
            this.SHColor.BackColor = this.VisualColor;
        }

        private void CBFilled_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.FillLevels = this.CBFilled.Checked;
            }
        }

        private void cboxMarksFontColor_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.ContourMarks.ColorLevel = this.cboxMarksFontColor.Checked;
            }
        }

        private void cboxMarksVisible_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.ContourMarks.Visible = this.cboxMarksVisible.Checked;
            }
        }

        private void CBYPosLevel_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.YPositionLevel = this.CBYPosLevel.Checked;
            }
        }

        private void comboBoxDrawing_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.DrawingAlgorithm = (ContourConstruction) this.comboBoxDrawing.SelectedIndex;
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

        private void EValue_TextChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.Level.UpToValue = Utils.StringToDouble(this.EValue.Text, 0.0);
                this.CBAutoLevels.Checked = this.series.AutomaticLevels;
                this.series.Invalidate();
            }
        }

        private void hSBLevel_Scroll(object sender, ScrollEventArgs e)
        {
            this.UDLevel.Value = this.hSBLevel.Value;
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.comboBoxDrawing = new ComboBox();
            this.label6 = new Label();
            this.CBFilled = new CheckBox();
            this.buttonPenFrame = new ButtonPen();
            this.CBColorEach = new CheckBox();
            this.Button2 = new ButtonPen();
            this.tabPage2 = new TabPage();
            this.label2 = new Label();
            this.UDNum = new NumericUpDown();
            this.CBAutoLevels = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.CBDefaultPen = new CheckBox();
            this.BLevelPen = new Button();
            this.CBEmpty = new CheckBox();
            this.SHColor = new System.Windows.Forms.Panel();
            this.hSBLevel = new HScrollBar();
            this.label7 = new Label();
            this.EValue = new TextBox();
            this.label3 = new Label();
            this.UDLevel = new NumericUpDown();
            this.tabPage3 = new TabPage();
            this.buttonColorMarksFont = new ButtonColor();
            this.CBAntiOverlap = new CheckBox();
            this.CBAtSegments = new CheckBox();
            this.cboxMarksFontColor = new CheckBox();
            this.UDMarksMargin = new NumericUpDown();
            this.UDMarksDensity = new NumericUpDown();
            this.label5 = new Label();
            this.label4 = new Label();
            this.cboxMarksVisible = new CheckBox();
            this.tabPage4 = new TabPage();
            this.tabPage5 = new TabPage();
            this.UDYPos = new NumericUpDown();
            this.CBYPosLevel = new CheckBox();
            this.label1 = new Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.UDNum.BeginInit();
            this.groupBox1.SuspendLayout();
            this.UDLevel.BeginInit();
            this.tabPage3.SuspendLayout();
            this.UDMarksMargin.BeginInit();
            this.UDMarksDensity.BeginInit();
            this.tabPage5.SuspendLayout();
            this.UDYPos.BeginInit();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x160, 0xd8);
            this.tabControl1.TabIndex = 5;
            this.tabPage1.Controls.Add(this.comboBoxDrawing);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.CBFilled);
            this.tabPage1.Controls.Add(this.buttonPenFrame);
            this.tabPage1.Controls.Add(this.CBColorEach);
            this.tabPage1.Controls.Add(this.Button2);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x158, 190);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Options";
            this.comboBoxDrawing.Items.Add("Fast");
            this.comboBoxDrawing.Items.Add("Segments");
            this.comboBoxDrawing.Location = new Point(0x69, 90);
            this.comboBoxDrawing.Name = "comboBoxDrawing";
            this.comboBoxDrawing.Size = new Size(0x63, 0x15);
            this.comboBoxDrawing.TabIndex = 6;
            this.comboBoxDrawing.SelectedIndexChanged += new EventHandler(this.comboBoxDrawing_SelectedIndexChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(8, 0x5d);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x5b, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Drawing algorithm";
            this.CBFilled.FlatStyle = FlatStyle.Flat;
            this.CBFilled.Location = new Point(0x71, 0x34);
            this.CBFilled.Name = "CBFilled";
            this.CBFilled.Size = new Size(0x4a, 0x11);
            this.CBFilled.TabIndex = 4;
            this.CBFilled.Text = "Filled";
            this.CBFilled.CheckedChanged += new EventHandler(this.CBFilled_CheckedChanged);
            this.buttonPenFrame.FlatStyle = FlatStyle.Flat;
            this.buttonPenFrame.Location = new Point(6, 0x2e);
            this.buttonPenFrame.Name = "buttonPenFrame";
            this.buttonPenFrame.Size = new Size(0x4b, 0x17);
            this.buttonPenFrame.TabIndex = 3;
            this.buttonPenFrame.Text = "&Frame...";
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(0x71, 10);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x79, 0x13);
            this.CBColorEach.TabIndex = 2;
            this.CBColorEach.Text = "&Color Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.Button2.FlatStyle = FlatStyle.Flat;
            this.Button2.Location = new Point(6, 6);
            this.Button2.Name = "Button2";
            this.Button2.Size = new Size(0x4b, 0x17);
            this.Button2.TabIndex = 1;
            this.Button2.Text = "&Pen...";
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.UDNum);
            this.tabPage2.Controls.Add(this.CBAutoLevels);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0x158, 190);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Levels";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x6b, 7);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2f, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "&Number:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.UDNum.BorderStyle = BorderStyle.FixedSingle;
            this.UDNum.Location = new Point(160, 5);
            int[] bits = new int[4];
            bits[0] = 150;
            this.UDNum.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.UDNum.Minimum = new decimal(numArray2);
            this.UDNum.Name = "UDNum";
            this.UDNum.Size = new Size(0x38, 20);
            this.UDNum.TabIndex = 5;
            this.UDNum.TextAlign = HorizontalAlignment.Right;
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.UDNum.Value = new decimal(numArray3);
            this.UDNum.ValueChanged += new EventHandler(this.UDNum_ValueChanged);
            this.CBAutoLevels.FlatStyle = FlatStyle.Flat;
            this.CBAutoLevels.Location = new Point(6, 6);
            this.CBAutoLevels.Name = "CBAutoLevels";
            this.CBAutoLevels.Size = new Size(0x62, 0x11);
            this.CBAutoLevels.TabIndex = 4;
            this.CBAutoLevels.Text = "&Automatic";
            this.CBAutoLevels.CheckedChanged += new EventHandler(this.CBAutoLevels_CheckedChanged);
            this.groupBox1.Controls.Add(this.CBDefaultPen);
            this.groupBox1.Controls.Add(this.BLevelPen);
            this.groupBox1.Controls.Add(this.CBEmpty);
            this.groupBox1.Controls.Add(this.SHColor);
            this.groupBox1.Controls.Add(this.hSBLevel);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.EValue);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.UDLevel);
            this.groupBox1.Location = new Point(6, 0x2d);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(330, 0x89);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Level:";
            this.CBDefaultPen.AutoSize = true;
            this.CBDefaultPen.Location = new Point(0x15, 100);
            this.CBDefaultPen.Name = "CBDefaultPen";
            this.CBDefaultPen.Size = new Size(60, 0x11);
            this.CBDefaultPen.TabIndex = 12;
            this.CBDefaultPen.Text = "&Default";
            this.CBDefaultPen.UseVisualStyleBackColor = true;
            this.CBDefaultPen.Click += new EventHandler(this.CBDefaultPen_Click);
            this.BLevelPen.FlatStyle = FlatStyle.Flat;
            this.BLevelPen.Location = new Point(0x15, 0x47);
            this.BLevelPen.Name = "BLevelPen";
            this.BLevelPen.Size = new Size(0x4b, 0x17);
            this.BLevelPen.TabIndex = 11;
            this.BLevelPen.Text = "P&en...";
            this.BLevelPen.UseVisualStyleBackColor = true;
            this.BLevelPen.Click += new EventHandler(this.BLevelPen_Click);
            this.CBEmpty.AutoSize = true;
            this.CBEmpty.Location = new Point(0xa9, 0x47);
            this.CBEmpty.Name = "CBEmpty";
            this.CBEmpty.Size = new Size(0x37, 0x11);
            this.CBEmpty.TabIndex = 10;
            this.CBEmpty.Text = "&Empty";
            this.CBEmpty.UseVisualStyleBackColor = true;
            this.CBEmpty.Click += new EventHandler(this.CBEmpty_Click);
            this.SHColor.Location = new Point(0x72, 0x47);
            this.SHColor.Name = "SHColor";
            this.SHColor.Size = new Size(0x22, 30);
            this.SHColor.TabIndex = 9;
            this.hSBLevel.LargeChange = 1;
            this.hSBLevel.Location = new Point(0xa9, 0x15);
            this.hSBLevel.Name = "hSBLevel";
            this.hSBLevel.Size = new Size(0x83, 0x10);
            this.hSBLevel.TabIndex = 8;
            this.hSBLevel.Scroll += new ScrollEventHandler(this.hSBLevel_Scroll);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x12, 50);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x42, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "&Up to Value:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            this.EValue.BorderStyle = BorderStyle.FixedSingle;
            this.EValue.Location = new Point(90, 0x2d);
            this.EValue.Name = "EValue";
            this.EValue.Size = new Size(0x60, 20);
            this.EValue.TabIndex = 6;
            this.EValue.TextChanged += new EventHandler(this.EValue_TextChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x30, 0x15);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x24, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "&Level:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.UDLevel.BorderStyle = BorderStyle.FixedSingle;
            this.UDLevel.Location = new Point(0x5c, 0x13);
            int[] numArray4 = new int[4];
            numArray4[0] = 10;
            this.UDLevel.Maximum = new decimal(numArray4);
            this.UDLevel.Name = "UDLevel";
            this.UDLevel.Size = new Size(0x38, 20);
            this.UDLevel.TabIndex = 4;
            this.UDLevel.TextAlign = HorizontalAlignment.Right;
            this.UDLevel.ValueChanged += new EventHandler(this.UDLevel_ValueChanged);
            this.tabPage3.Controls.Add(this.buttonColorMarksFont);
            this.tabPage3.Controls.Add(this.CBAntiOverlap);
            this.tabPage3.Controls.Add(this.CBAtSegments);
            this.tabPage3.Controls.Add(this.cboxMarksFontColor);
            this.tabPage3.Controls.Add(this.UDMarksMargin);
            this.tabPage3.Controls.Add(this.UDMarksDensity);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.cboxMarksVisible);
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(0x158, 190);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Marks";
            this.buttonColorMarksFont.Color = Color.Empty;
            this.buttonColorMarksFont.Location = new Point(130, 0x68);
            this.buttonColorMarksFont.Name = "buttonColorMarksFont";
            this.buttonColorMarksFont.Size = new Size(0x4b, 0x17);
            this.buttonColorMarksFont.TabIndex = 8;
            this.buttonColorMarksFont.Text = "Color...";
            this.buttonColorMarksFont.Click += new EventHandler(this.buttonColorMarksFont_Click);
            this.CBAntiOverlap.FlatStyle = FlatStyle.Flat;
            this.CBAntiOverlap.Location = new Point(0x8f, 0x25);
            this.CBAntiOverlap.Name = "CBAntiOverlap";
            this.CBAntiOverlap.Size = new Size(0x51, 0x11);
            this.CBAntiOverlap.TabIndex = 7;
            this.CBAntiOverlap.Text = "Anti overlap";
            this.CBAntiOverlap.CheckedChanged += new EventHandler(this.CBAntiOverlap_CheckedChanged);
            this.CBAtSegments.FlatStyle = FlatStyle.Flat;
            this.CBAtSegments.Location = new Point(0x8f, 14);
            this.CBAtSegments.Name = "CBAtSegments";
            this.CBAtSegments.Size = new Size(0x51, 0x11);
            this.CBAtSegments.TabIndex = 6;
            this.CBAtSegments.Text = "At segments";
            this.CBAtSegments.CheckedChanged += new EventHandler(this.CBAtSegments_CheckedChanged);
            this.cboxMarksFontColor.FlatStyle = FlatStyle.Flat;
            this.cboxMarksFontColor.Location = new Point(11, 0x6b);
            this.cboxMarksFontColor.Name = "cboxMarksFontColor";
            this.cboxMarksFontColor.Size = new Size(0x6d, 0x11);
            this.cboxMarksFontColor.TabIndex = 5;
            this.cboxMarksFontColor.Text = "Font Color Level";
            this.cboxMarksFontColor.CheckedChanged += new EventHandler(this.cboxMarksFontColor_CheckedChanged);
            this.UDMarksMargin.Location = new Point(0x38, 0x48);
            this.UDMarksMargin.Name = "UDMarksMargin";
            this.UDMarksMargin.Size = new Size(0x36, 20);
            this.UDMarksMargin.TabIndex = 4;
            this.UDMarksMargin.ValueChanged += new EventHandler(this.UDMarksMargin_ValueChanged);
            this.UDMarksDensity.Location = new Point(0x38, 0x2c);
            int[] numArray5 = new int[4];
            numArray5[0] = 1;
            this.UDMarksDensity.Minimum = new decimal(numArray5);
            this.UDMarksDensity.Name = "UDMarksDensity";
            this.UDMarksDensity.Size = new Size(0x36, 20);
            this.UDMarksDensity.TabIndex = 3;
            int[] numArray6 = new int[4];
            numArray6[0] = 1;
            this.UDMarksDensity.Value = new decimal(numArray6);
            this.UDMarksDensity.ValueChanged += new EventHandler(this.UDMarksDensity_ValueChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(8, 0x4a);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x27, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Margin";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(8, 0x2e);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x2a, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Density";
            this.cboxMarksVisible.FlatStyle = FlatStyle.Flat;
            this.cboxMarksVisible.Location = new Point(8, 14);
            this.cboxMarksVisible.Name = "cboxMarksVisible";
            this.cboxMarksVisible.Size = new Size(0x48, 0x11);
            this.cboxMarksVisible.TabIndex = 0;
            this.cboxMarksVisible.Text = "Visible";
            this.cboxMarksVisible.CheckedChanged += new EventHandler(this.cboxMarksVisible_CheckedChanged);
            this.tabPage4.Location = new Point(4, 0x16);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new Size(0x158, 190);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Pointer";
            this.tabPage5.Controls.Add(this.UDYPos);
            this.tabPage5.Controls.Add(this.CBYPosLevel);
            this.tabPage5.Controls.Add(this.label1);
            this.tabPage5.Location = new Point(4, 0x16);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new Size(0x158, 190);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Position";
            this.UDYPos.BorderStyle = BorderStyle.FixedSingle;
            this.UDYPos.Location = new Point(100, 0x25);
            int[] numArray7 = new int[4];
            numArray7[0] = 0x7530;
            this.UDYPos.Maximum = new decimal(numArray7);
            int[] numArray8 = new int[4];
            numArray8[0] = 0x7530;
            numArray8[3] = -2147483648;
            this.UDYPos.Minimum = new decimal(numArray8);
            this.UDYPos.Name = "UDYPos";
            this.UDYPos.Size = new Size(0x48, 20);
            this.UDYPos.TabIndex = 7;
            this.UDYPos.TextAlign = HorizontalAlignment.Right;
            this.UDYPos.ValueChanged += new EventHandler(this.UDYPos_ValueChanged);
            this.CBYPosLevel.FlatStyle = FlatStyle.Flat;
            this.CBYPosLevel.Location = new Point(11, 11);
            this.CBYPosLevel.Name = "CBYPosLevel";
            this.CBYPosLevel.Size = new Size(0x7a, 20);
            this.CBYPosLevel.TabIndex = 6;
            this.CBYPosLevel.Text = "&Levels position";
            this.CBYPosLevel.CheckedChanged += new EventHandler(this.CBYPosLevel_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 0x27);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "&Vertical Position:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x160, 0xd8);
            base.Controls.Add(this.tabControl1);
            base.Name = "ContourSeries";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.UDNum.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.UDLevel.EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.UDMarksMargin.EndInit();
            this.UDMarksDensity.EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.UDYPos.EndInit();
            base.ResumeLayout(false);
        }

        private void SetDefaultLevelColor()
        {
        }

        private void SetLevel()
        {
            bool setting = this.setting;
            this.setting = true;
            bool enable = this.series.Levels.Count > ((int) this.UDLevel.Value);
            Utils.EnableControls(enable, new Control[] { this.UDLevel, this.EValue, this.BLevelPen, this.CBDefaultPen, this.SHColor, this.hSBLevel });
            if (enable)
            {
                Color visualColor = this.VisualColor;
                this.SHColor.BackColor = visualColor;
                this.CBEmpty.Checked = visualColor == Color.Empty;
                this.EValue.Text = this.Level.UpToValue.ToString("0.###");
                this.CBDefaultPen.Checked = this.Level.DefaultPen();
                this.BLevelPen.Enabled = !this.Level.DefaultPen();
                this.hSBLevel.Value = (int) this.UDLevel.Value;
            }
            this.setting = setting;
        }

        public override void SetParent(TabPage Parent)
        {
            if ((this.series != null) && (this.grid3DEditor == null))
            {
                this.grid3DEditor = new Grid3DSeries(this.series, Parent);
            }
        }

        private void UDLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting && (this.series.Levels.Count > 0))
            {
                this.SetLevel();
            }
        }

        private void UDMarksDensity_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.ContourMarks.Density = (int) this.UDMarksDensity.Value;
            }
        }

        private void UDMarksMargin_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.ContourMarks.Margin = (int) this.UDMarksMargin.Value;
            }
        }

        private void UDNum_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting && (this.series != null))
            {
                this.series.NumLevels = Convert.ToInt32(this.UDNum.Value);
                this.series.CreateAutoLevels();
                this.UDLevel.Maximum = this.series.NumLevels - 1;
                this.hSBLevel.Maximum = (int) this.UDLevel.Maximum;
                this.SetLevel();
            }
        }

        private void UDYPos_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.YPosition = Convert.ToDouble(this.UDYPos.Value);
            }
        }

        private ContourLevel Level
        {
            get
            {
                return this.series.Levels[(int) this.UDLevel.Value];
            }
        }

        private Color VisualColor
        {
            get
            {
                return this.Level.Color;
            }
        }
    }
}

