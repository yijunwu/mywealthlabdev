namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AnnotationEditor : Form, IStopComboBoxTranslate
    {
        private Annotation annotation;
        private ButtonPen BCalloutPen;
        private Button BCalloutPointer;
        private CheckBox cbAllowEdit;
        private ComboBox cbCursor;
        private CheckBox CBCustomSize;
        private CheckBox CBCustPos;
        private ComboBox CBHead;
        private ComboBox CBPos;
        private ComboBox CBPositionUnits;
        private ComboBox cbTextAlign;
        private CheckBox checkBoxClipText;
        private Container components;
        private NumericUpDown EX;
        private NumericUpDown EY;
        private NumericUpDown EZ;
        private GroupBox groupBox1;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label15;
        private Label label16;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox MemoText;
        private CustomShapeEditor shapeForm;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private Steema.TeeChart.Tools.Tool tool;
        private NumericUpDown UDArrowDist;
        private NumericUpDown UDHeadSize;
        private NumericUpDown UDHeight;
        private NumericUpDown UDLeft;
        private NumericUpDown UDTop;
        private NumericUpDown UDWidth;

        public AnnotationEditor()
        {
            this.InitializeComponent();
            this.cbTextAlign.Items.Add("Left");
            this.cbTextAlign.Items.Add("Center");
            this.cbTextAlign.Items.Add("Right");
            this.CBPos.Items.Add("Left top");
            this.CBPos.Items.Add("Left bottom");
            this.CBPos.Items.Add("Right top");
            this.CBPos.Items.Add("Right bottom");
            this.CBPositionUnits.Items.Add("Pixels");
            this.CBPositionUnits.Items.Add("Percent");
            this.CBHead.Items.Add("None");
            this.CBHead.Items.Add("Line");
            this.CBHead.Items.Add("Solid");
        }

        public AnnotationEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            this.tool = s;
            this.annotation = (Annotation) s;
            if (s is Magnify)
            {
                TabControl control = new TabControl();
                this.tabPage1.Parent = control;
                this.tabPage4.Parent = control;
            }
            else
            {
                this.shapeForm = CustomShapeEditor.Add(this.tabControl1, this.annotation.Shape);
            }
            if (s is PageNumber)
            {
                this.MemoText.Text = ((PageNumber) s).Format;
            }
            else
            {
                this.MemoText.Text = this.annotation.Text;
            }
            switch (this.annotation.TextAlign)
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
            this.CBPos.SelectedIndex = (int) this.annotation.Position;
            this.checkBoxClipText.Checked = this.annotation.ClipText;
            this.cbAllowEdit.Checked = this.annotation.AllowEdit;
            this.BCalloutPen.Pen = this.annotation.Callout.Arrow;
            this.EX.Value = this.annotation.Callout.XPosition;
            this.EY.Value = this.annotation.Callout.YPosition;
            this.EZ.Value = this.annotation.Callout.ZPosition;
            this.UDArrowDist.Value = this.annotation.Callout.Distance;
            this.CBHead.SelectedIndex = (int) this.annotation.Callout.ArrowHead;
            this.UDHeadSize.Value = this.annotation.Callout.ArrowHeadSize;
            this.CBCustPos.Checked = this.annotation.Shape.CustomPosition;
            this.CBCustomSize.Checked = this.annotation.AutoSize;
            this.CBPos.Enabled = !this.CBCustPos.Checked;
            this.UDLeft.Value = this.annotation.Left;
            this.UDTop.Value = this.annotation.Top;
            this.UDLeft.Enabled = this.CBCustPos.Checked;
            this.UDTop.Enabled = this.CBCustPos.Checked;
            if (this.annotation.Height >= 0)
            {
                this.UDHeight.Value = this.annotation.Height;
            }
            if (this.annotation.Width >= 0)
            {
                this.UDWidth.Value = this.annotation.Width;
            }
            this.UDHeight.Enabled = !this.CBCustomSize.Checked;
            this.UDWidth.Enabled = !this.CBCustomSize.Checked;
            if (this.annotation.Shape.CustomPosition)
            {
                if (this.annotation.PositionUnits == PositionUnits.Pixels)
                {
                    this.CBPositionUnits.SelectedIndex = 0;
                }
                else
                {
                    this.CBPositionUnits.SelectedIndex = 1;
                }
            }
            EditorUtils.FillCursors(this.cbCursor, this.annotation.Cursor);
        }

        private void BCalloutPointer_Click(object sender, EventArgs e)
        {
            EditorUtils.ShowFormModal(new Steema.TeeChart.Editors.SeriesPointer(this.annotation.Callout));
        }

        private void cbAllowEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.AllowEdit = this.cbAllowEdit.Checked;
            }
        }

        private void cbCursor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Cursor = EditorUtils.StringToCursor(this.cbCursor.SelectedItem.ToString());
            }
        }

        private void CBCustomSize_Click(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.AutoSize = this.CBCustomSize.Checked;
                this.UDHeight.Enabled = !this.CBCustomSize.Checked;
                this.UDWidth.Enabled = !this.CBCustomSize.Checked;
            }
        }

        private void CBCustPos_CheckedChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Shape.CustomPosition = this.CBCustPos.Checked;
                this.UDLeft.Enabled = this.CBCustPos.Checked;
                this.UDTop.Enabled = this.CBCustPos.Checked;
                this.CBPos.Enabled = !this.CBCustPos.Checked;
            }
        }

        private void CBHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Callout.ArrowHead = (ArrowHeadStyles) this.CBHead.SelectedIndex;
            }
        }

        private void CBPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Position = (AnnotationPositions) this.CBPos.SelectedIndex;
            }
        }

        private void CBPositionUnits_Click(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                if (this.CBPositionUnits.SelectedIndex == 0)
                {
                    this.annotation.PositionUnits = PositionUnits.Pixels;
                }
                else
                {
                    this.annotation.PositionUnits = PositionUnits.Percent;
                }
            }
        }

        private void cbTextAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                switch (this.cbTextAlign.SelectedIndex)
                {
                    case 0:
                        this.annotation.TextAlign = StringAlignment.Near;
                        return;

                    case 1:
                        this.annotation.TextAlign = StringAlignment.Center;
                        return;
                }
                this.annotation.TextAlign = StringAlignment.Far;
            }
        }

        private void checkBoxClipText_CheckedChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.ClipText = this.checkBoxClipText.Checked;
                this.annotation.Invalidate();
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

        private void EX_ValueChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Callout.XPosition = (int) this.EX.Value;
            }
        }

        private void EY_ValueChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Callout.YPosition = (int) this.EY.Value;
            }
        }

        private void EZ_ValueChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Callout.ZPosition = (int) this.EZ.Value;
            }
        }

        public ComboBox[] GetComboBoxes()
        {
            return new ComboBox[] { this.cbCursor };
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.checkBoxClipText = new CheckBox();
            this.cbCursor = new ComboBox();
            this.cbTextAlign = new ComboBox();
            this.label6 = new Label();
            this.label5 = new Label();
            this.MemoText = new TextBox();
            this.label1 = new Label();
            this.tabPage2 = new TabPage();
            this.CBPos = new ComboBox();
            this.label2 = new Label();
            this.CBPositionUnits = new ComboBox();
            this.label13 = new Label();
            this.UDLeft = new NumericUpDown();
            this.UDTop = new NumericUpDown();
            this.CBCustPos = new CheckBox();
            this.label3 = new Label();
            this.label4 = new Label();
            this.tabPage4 = new TabPage();
            this.UDHeight = new NumericUpDown();
            this.UDWidth = new NumericUpDown();
            this.label16 = new Label();
            this.label15 = new Label();
            this.CBCustomSize = new CheckBox();
            this.tabPage3 = new TabPage();
            this.UDHeadSize = new NumericUpDown();
            this.label12 = new Label();
            this.CBHead = new ComboBox();
            this.label11 = new Label();
            this.UDArrowDist = new NumericUpDown();
            this.label10 = new Label();
            this.groupBox1 = new GroupBox();
            this.EZ = new NumericUpDown();
            this.EY = new NumericUpDown();
            this.label9 = new Label();
            this.label8 = new Label();
            this.label7 = new Label();
            this.EX = new NumericUpDown();
            this.BCalloutPointer = new Button();
            this.BCalloutPen = new ButtonPen();
            this.cbAllowEdit = new CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.UDLeft.BeginInit();
            this.UDTop.BeginInit();
            this.tabPage4.SuspendLayout();
            this.UDHeight.BeginInit();
            this.UDWidth.BeginInit();
            this.tabPage3.SuspendLayout();
            this.UDHeadSize.BeginInit();
            this.UDArrowDist.BeginInit();
            this.groupBox1.SuspendLayout();
            this.EZ.BeginInit();
            this.EY.BeginInit();
            this.EX.BeginInit();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x110, 0xea);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabPage1.Controls.Add(this.cbAllowEdit);
            this.tabPage1.Controls.Add(this.checkBoxClipText);
            this.tabPage1.Controls.Add(this.cbCursor);
            this.tabPage1.Controls.Add(this.cbTextAlign);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.MemoText);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x108, 0xd0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Options";
            this.checkBoxClipText.AutoSize = true;
            this.checkBoxClipText.FlatStyle = FlatStyle.Flat;
            this.checkBoxClipText.Location = new Point(0x10, 0xab);
            this.checkBoxClipText.Name = "checkBoxClipText";
            this.checkBoxClipText.Size = new Size(0x40, 0x11);
            this.checkBoxClipText.TabIndex = 6;
            this.checkBoxClipText.Text = "Clip Text";
            this.checkBoxClipText.UseVisualStyleBackColor = true;
            this.checkBoxClipText.CheckedChanged += new EventHandler(this.checkBoxClipText_CheckedChanged);
            this.cbCursor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCursor.Location = new Point(0x99, 0x90);
            this.cbCursor.Name = "cbCursor";
            this.cbCursor.Size = new Size(0x60, 0x15);
            this.cbCursor.TabIndex = 5;
            this.cbCursor.SelectedIndexChanged += new EventHandler(this.cbCursor_SelectedIndexChanged);
            this.cbTextAlign.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbTextAlign.Location = new Point(0x10, 0x90);
            this.cbTextAlign.Name = "cbTextAlign";
            this.cbTextAlign.Size = new Size(0x79, 0x15);
            this.cbTextAlign.TabIndex = 3;
            this.cbTextAlign.SelectedIndexChanged += new EventHandler(this.cbTextAlign_SelectedIndexChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x98, 0x80);
            this.label6.Name = "label6";
            this.label6.Size = new Size(40, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "&Cursor:";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x10, 0x80);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x4f, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Text &alignment:";
            this.MemoText.BorderStyle = BorderStyle.FixedSingle;
            this.MemoText.Location = new Point(0x10, 0x20);
            this.MemoText.Multiline = true;
            this.MemoText.Name = "MemoText";
            this.MemoText.Size = new Size(0xe8, 0x58);
            this.MemoText.TabIndex = 1;
            this.MemoText.TextChanged += new EventHandler(this.MemoText_TextChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1f, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Text:";
            this.tabPage2.Controls.Add(this.CBPos);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.CBPositionUnits);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.UDLeft);
            this.tabPage2.Controls.Add(this.UDTop);
            this.tabPage2.Controls.Add(this.CBCustPos);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0x108, 0xd0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Position";
            this.CBPos.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBPos.ItemHeight = 13;
            this.CBPos.Location = new Point(0x48, 0x16);
            this.CBPos.Name = "CBPos";
            this.CBPos.Size = new Size(0x88, 0x15);
            this.CBPos.TabIndex = 1;
            this.CBPos.SelectedIndexChanged += new EventHandler(this.CBPos_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x22, 0x18);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x20, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Auto:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.CBPositionUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBPositionUnits.ItemHeight = 13;
            this.CBPositionUnits.Location = new Point(0x48, 0x88);
            this.CBPositionUnits.Name = "CBPositionUnits";
            this.CBPositionUnits.Size = new Size(0x88, 0x15);
            this.CBPositionUnits.TabIndex = 7;
            this.CBPositionUnits.SelectedIndexChanged += new EventHandler(this.CBPositionUnits_Click);
            this.CBPositionUnits.Click += new EventHandler(this.CBPositionUnits_Click);
            this.label13.AutoSize = true;
            this.label13.Location = new Point(0x20, 0x8b);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x22, 13);
            this.label13.TabIndex = 8;
            this.label13.Text = "&Units:";
            this.label13.TextAlign = ContentAlignment.TopRight;
            this.UDLeft.BorderStyle = BorderStyle.FixedSingle;
            this.UDLeft.Enabled = false;
            int[] bits = new int[4];
            bits[0] = 5;
            this.UDLeft.Increment = new decimal(bits);
            this.UDLeft.Location = new Point(0x48, 80);
            int[] numArray2 = new int[4];
            numArray2[0] = 0x3e8;
            this.UDLeft.Maximum = new decimal(numArray2);
            int[] numArray3 = new int[4];
            numArray3[0] = 100;
            numArray3[3] = -2147483648;
            this.UDLeft.Minimum = new decimal(numArray3);
            this.UDLeft.Name = "UDLeft";
            this.UDLeft.Size = new Size(0x38, 20);
            this.UDLeft.TabIndex = 4;
            this.UDLeft.TextAlign = HorizontalAlignment.Right;
            int[] numArray4 = new int[4];
            numArray4[0] = 10;
            this.UDLeft.Value = new decimal(numArray4);
            this.UDLeft.Click += new EventHandler(this.UDLeft_Click);
            this.UDTop.BorderStyle = BorderStyle.FixedSingle;
            this.UDTop.Enabled = false;
            int[] numArray5 = new int[4];
            numArray5[0] = 5;
            this.UDTop.Increment = new decimal(numArray5);
            this.UDTop.Location = new Point(0x48, 0x68);
            int[] numArray6 = new int[4];
            numArray6[0] = 0x3e8;
            this.UDTop.Maximum = new decimal(numArray6);
            int[] numArray7 = new int[4];
            numArray7[0] = 100;
            numArray7[3] = -2147483648;
            this.UDTop.Minimum = new decimal(numArray7);
            this.UDTop.Name = "UDTop";
            this.UDTop.Size = new Size(0x38, 20);
            this.UDTop.TabIndex = 6;
            this.UDTop.TextAlign = HorizontalAlignment.Right;
            this.UDTop.Click += new EventHandler(this.UDTop_Click);
            this.CBCustPos.FlatStyle = FlatStyle.Flat;
            this.CBCustPos.Location = new Point(0x48, 0x38);
            this.CBCustPos.Name = "CBCustPos";
            this.CBCustPos.Size = new Size(0x51, 0x10);
            this.CBCustPos.TabIndex = 2;
            this.CBCustPos.Text = "&Custom";
            this.CBCustPos.CheckedChanged += new EventHandler(this.CBCustPos_CheckedChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(40, 0x53);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1c, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "L&eft:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x26, 0x6b);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1d, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "T&op:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.tabPage4.Controls.Add(this.UDHeight);
            this.tabPage4.Controls.Add(this.UDWidth);
            this.tabPage4.Controls.Add(this.label16);
            this.tabPage4.Controls.Add(this.label15);
            this.tabPage4.Controls.Add(this.CBCustomSize);
            this.tabPage4.Location = new Point(4, 0x16);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new Size(0x108, 0xd0);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Size";
            this.UDHeight.BorderStyle = BorderStyle.FixedSingle;
            this.UDHeight.Location = new Point(0x58, 0x40);
            int[] numArray8 = new int[4];
            numArray8[0] = 0x3e8;
            this.UDHeight.Maximum = new decimal(numArray8);
            int[] numArray9 = new int[4];
            numArray9[0] = 100;
            numArray9[3] = -2147483648;
            this.UDHeight.Minimum = new decimal(numArray9);
            this.UDHeight.Name = "UDHeight";
            this.UDHeight.Size = new Size(0x38, 20);
            this.UDHeight.TabIndex = 1;
            this.UDHeight.TextAlign = HorizontalAlignment.Right;
            this.UDHeight.Click += new EventHandler(this.UDHeight_Click);
            this.UDWidth.BorderStyle = BorderStyle.FixedSingle;
            this.UDWidth.Location = new Point(0x58, 0x58);
            int[] numArray10 = new int[4];
            numArray10[0] = 0x3e8;
            this.UDWidth.Maximum = new decimal(numArray10);
            int[] numArray11 = new int[4];
            numArray11[0] = 100;
            numArray11[3] = -2147483648;
            this.UDWidth.Minimum = new decimal(numArray11);
            this.UDWidth.Name = "UDWidth";
            this.UDWidth.Size = new Size(0x38, 20);
            this.UDWidth.TabIndex = 2;
            this.UDWidth.TextAlign = HorizontalAlignment.Right;
            this.UDWidth.Click += new EventHandler(this.UDWidth_Click);
            this.label16.AutoSize = true;
            this.label16.Location = new Point(0x2e, 90);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x26, 13);
            this.label16.TabIndex = 7;
            this.label16.Text = "Width:";
            this.label16.TextAlign = ContentAlignment.TopRight;
            this.label15.AutoSize = true;
            this.label15.Location = new Point(0x29, 0x42);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x29, 13);
            this.label15.TabIndex = 7;
            this.label15.Text = "Height:";
            this.label15.TextAlign = ContentAlignment.TopRight;
            this.CBCustomSize.FlatStyle = FlatStyle.Flat;
            this.CBCustomSize.Location = new Point(0x58, 0x20);
            this.CBCustomSize.Name = "CBCustomSize";
            this.CBCustomSize.Size = new Size(80, 0x10);
            this.CBCustomSize.TabIndex = 0;
            this.CBCustomSize.Text = "Automatic";
            this.CBCustomSize.Click += new EventHandler(this.CBCustomSize_Click);
            this.tabPage3.Controls.Add(this.UDHeadSize);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.CBHead);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.UDArrowDist);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.BCalloutPointer);
            this.tabPage3.Controls.Add(this.BCalloutPen);
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(0x108, 0xd0);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Callout";
            this.UDHeadSize.BorderStyle = BorderStyle.FixedSingle;
            this.UDHeadSize.Location = new Point(200, 0x88);
            int[] numArray12 = new int[4];
            numArray12[0] = 0x3e8;
            this.UDHeadSize.Maximum = new decimal(numArray12);
            int[] numArray13 = new int[4];
            numArray13[0] = 0x3e8;
            numArray13[3] = -2147483648;
            this.UDHeadSize.Minimum = new decimal(numArray13);
            this.UDHeadSize.Name = "UDHeadSize";
            this.UDHeadSize.Size = new Size(0x30, 20);
            this.UDHeadSize.TabIndex = 8;
            this.UDHeadSize.TextAlign = HorizontalAlignment.Right;
            this.UDHeadSize.ValueChanged += new EventHandler(this.UDHeadSize_ValueChanged);
            this.UDHeadSize.TextChanged += new EventHandler(this.UDHeadSize_ValueChanged);
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0xac, 0x8a);
            this.label12.Name = "label12";
            this.label12.Size = new Size(30, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "&Size:";
            this.CBHead.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBHead.Location = new Point(0x90, 0x68);
            this.CBHead.Name = "CBHead";
            this.CBHead.Size = new Size(0x68, 0x15);
            this.CBHead.TabIndex = 6;
            this.CBHead.SelectedIndexChanged += new EventHandler(this.CBHead_SelectedIndexChanged);
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x90, 0x58);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x40, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "&Arrow head:";
            this.UDArrowDist.BorderStyle = BorderStyle.FixedSingle;
            this.UDArrowDist.Location = new Point(0xc2, 0x36);
            int[] numArray14 = new int[4];
            numArray14[0] = 0x3e8;
            this.UDArrowDist.Maximum = new decimal(numArray14);
            int[] numArray15 = new int[4];
            numArray15[0] = 0x3e8;
            numArray15[3] = -2147483648;
            this.UDArrowDist.Minimum = new decimal(numArray15);
            this.UDArrowDist.Name = "UDArrowDist";
            this.UDArrowDist.Size = new Size(0x36, 20);
            this.UDArrowDist.TabIndex = 4;
            this.UDArrowDist.TextAlign = HorizontalAlignment.Right;
            this.UDArrowDist.ValueChanged += new EventHandler(this.UDArrowDist_ValueChanged);
            this.UDArrowDist.TextChanged += new EventHandler(this.UDArrowDist_ValueChanged);
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x90, 0x38);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x34, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "&Distance:";
            this.label10.TextAlign = ContentAlignment.TopRight;
            this.groupBox1.Controls.Add(this.EZ);
            this.groupBox1.Controls.Add(this.EY);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.EX);
            this.groupBox1.Location = new Point(0x10, 0x38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x70, 0x68);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "P&osition";
            this.EZ.BorderStyle = BorderStyle.FixedSingle;
            this.EZ.Location = new Point(40, 70);
            int[] numArray16 = new int[4];
            numArray16[0] = 0x3e8;
            this.EZ.Maximum = new decimal(numArray16);
            int[] numArray17 = new int[4];
            numArray17[0] = 0x3e8;
            numArray17[3] = -2147483648;
            this.EZ.Minimum = new decimal(numArray17);
            this.EZ.Name = "EZ";
            this.EZ.Size = new Size(0x40, 20);
            this.EZ.TabIndex = 11;
            this.EZ.TextAlign = HorizontalAlignment.Right;
            this.EZ.ValueChanged += new EventHandler(this.EZ_ValueChanged);
            this.EZ.TextChanged += new EventHandler(this.EZ_ValueChanged);
            this.EY.BorderStyle = BorderStyle.FixedSingle;
            this.EY.Location = new Point(40, 0x2e);
            int[] numArray18 = new int[4];
            numArray18[0] = 0x3e8;
            this.EY.Maximum = new decimal(numArray18);
            int[] numArray19 = new int[4];
            numArray19[0] = 0x3e8;
            numArray19[3] = -2147483648;
            this.EY.Minimum = new decimal(numArray19);
            this.EY.Name = "EY";
            this.EY.Size = new Size(0x40, 20);
            this.EY.TabIndex = 10;
            this.EY.TextAlign = HorizontalAlignment.Right;
            this.EY.ValueChanged += new EventHandler(this.EY_ValueChanged);
            this.EY.TextChanged += new EventHandler(this.EY_ValueChanged);
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x18, 0x48);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x11, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "&Z:";
            this.label9.TextAlign = ContentAlignment.TopRight;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x18, 0x30);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x11, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "&Y:";
            this.label8.TextAlign = ContentAlignment.TopRight;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x18, 0x18);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x11, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "&X:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            this.EX.BorderStyle = BorderStyle.FixedSingle;
            this.EX.Location = new Point(40, 0x16);
            int[] numArray20 = new int[4];
            numArray20[0] = 0x3e8;
            this.EX.Maximum = new decimal(numArray20);
            int[] numArray21 = new int[4];
            numArray21[0] = 0x3e8;
            numArray21[3] = -2147483648;
            this.EX.Minimum = new decimal(numArray21);
            this.EX.Name = "EX";
            this.EX.Size = new Size(0x40, 20);
            this.EX.TabIndex = 9;
            this.EX.TextAlign = HorizontalAlignment.Right;
            this.EX.ValueChanged += new EventHandler(this.EX_ValueChanged);
            this.EX.TextChanged += new EventHandler(this.EX_ValueChanged);
            this.BCalloutPointer.FlatStyle = FlatStyle.Flat;
            this.BCalloutPointer.Location = new Point(0x60, 0x10);
            this.BCalloutPointer.Name = "BCalloutPointer";
            this.BCalloutPointer.Size = new Size(0x48, 0x18);
            this.BCalloutPointer.TabIndex = 1;
            this.BCalloutPointer.Text = "&Pointer...";
            this.BCalloutPointer.Click += new EventHandler(this.BCalloutPointer_Click);
            this.BCalloutPen.FlatStyle = FlatStyle.Flat;
            this.BCalloutPen.Location = new Point(0x10, 0x10);
            this.BCalloutPen.Name = "BCalloutPen";
            this.BCalloutPen.Size = new Size(0x48, 0x18);
            this.BCalloutPen.TabIndex = 0;
            this.BCalloutPen.Text = "&Border...";
            this.cbAllowEdit.AutoSize = true;
            this.cbAllowEdit.FlatStyle = FlatStyle.Flat;
            this.cbAllowEdit.Location = new Point(0x80, 0xab);
            this.cbAllowEdit.Name = "cbAllowEdit";
            this.cbAllowEdit.Size = new Size(0x45, 0x11);
            this.cbAllowEdit.TabIndex = 7;
            this.cbAllowEdit.Text = "Allow Edit";
            this.cbAllowEdit.UseVisualStyleBackColor = true;
            this.cbAllowEdit.CheckedChanged += new EventHandler(this.cbAllowEdit_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x110, 0xea);
            base.Controls.Add(this.tabControl1);
            base.Name = "AnnotationEditor";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.UDLeft.EndInit();
            this.UDTop.EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.UDHeight.EndInit();
            this.UDWidth.EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.UDHeadSize.EndInit();
            this.UDArrowDist.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.EZ.EndInit();
            this.EY.EndInit();
            this.EX.EndInit();
            base.ResumeLayout(false);
        }

        private void MemoText_TextChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                if (this.tool is PageNumber)
                {
                    string text = this.MemoText.Text;
                    while (text.IndexOf("{") != -1)
                    {
                        int index = text.IndexOf("{");
                        text = text.Substring(index + 1);
                        if ((text.IndexOf("}") == -1) || ((text.IndexOf("{") != -1) && (text.IndexOf("{") < text.IndexOf("}"))))
                        {
                            return;
                        }
                    }
                    text = this.MemoText.Text;
                    if (((text.IndexOf("}") == -1) || (text.IndexOf("{") != -1)) && ((text.IndexOf("}") == -1) || (text.IndexOf("}") >= text.IndexOf("{"))))
                    {
                        this.annotation.Text = this.MemoText.Text;
                    }
                }
                else
                {
                    this.annotation.Text = this.MemoText.Text;
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((this.tabControl1.SelectedIndex == 1) || (this.tabControl1.SelectedIndex == 2)) && (this.annotation.Text != ""))
            {
                this.CBCustPos.Checked = this.annotation.Shape.CustomPosition;
                this.CBCustomSize.Checked = this.annotation.AutoSize;
                this.CBPos.Enabled = !this.CBCustPos.Checked;
                this.UDLeft.Value = this.annotation.Left;
                this.UDTop.Value = this.annotation.Top;
                this.UDLeft.Enabled = this.CBCustPos.Checked;
                this.UDTop.Enabled = this.CBCustPos.Checked;
                if (this.annotation.Height >= 0)
                {
                    if (this.annotation.Height <= this.UDHeight.Maximum)
                    {
                        this.UDHeight.Value = this.annotation.Height;
                    }
                    else
                    {
                        this.UDHeight.Value = this.UDHeight.Maximum;
                    }
                }
                if (this.annotation.Width >= 0)
                {
                    if (this.annotation.Width <= this.UDWidth.Maximum)
                    {
                        this.UDWidth.Value = this.annotation.Width;
                    }
                    else
                    {
                        this.UDWidth.Value = this.UDWidth.Maximum;
                    }
                }
                this.UDHeight.Enabled = !this.CBCustomSize.Checked;
                this.UDWidth.Enabled = !this.CBCustomSize.Checked;
                if (this.annotation.PositionUnits == PositionUnits.Pixels)
                {
                    this.CBPositionUnits.SelectedIndex = 0;
                }
                else
                {
                    this.CBPositionUnits.SelectedIndex = 1;
                }
            }
        }

        private void UDArrowDist_ValueChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Callout.Distance = (int) this.UDArrowDist.Value;
            }
        }

        private void UDHeadSize_ValueChanged(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Callout.ArrowHeadSize = (int) this.UDHeadSize.Value;
            }
        }

        private void UDHeight_Click(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Height = (int) this.UDHeight.Value;
                this.annotation.Invalidate();
            }
        }

        private void UDLeft_Click(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Left = (int) this.UDLeft.Value;
            }
        }

        private void UDTop_Click(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Top = (int) this.UDTop.Value;
            }
        }

        private void UDWidth_Click(object sender, EventArgs e)
        {
            if (this.annotation != null)
            {
                this.annotation.Width = (int) this.UDWidth.Value;
                this.annotation.Invalidate();
            }
        }
    }
}

