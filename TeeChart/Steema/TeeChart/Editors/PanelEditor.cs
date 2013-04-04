namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PanelEditor : Form
    {
        private ButtonColor bColor;
        private BevelEditor bevelEditor;
        private BevelImageEditor bevelImageEditor;
        private ButtonPen button10;
        private Button button7;
        private Button button9;
        private ComboBox CBBevels;
        private CheckBox CBImageTrans;
        private CheckBox checkBox1;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private Container components;
        private GroupBox GBMargins;
        private GradientEditor gradientEditor;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label6;
        private Steema.TeeChart.Panel panel;
        private GroupBox panel1;
        private GroupBox panel2;
        private TabControl PanelPages;
        private PictureBox pictureBox1;
        private ShadowEditor shadowEditor;
        private TabPage tabPage6;
        private TabPage tabPage7;
        private TabPage tabPagePanelGradient;
        private TabPage tabShadow;
        private TabPage tpMargins;
        private NumericUpDown UDBotMa;
        private NumericUpDown UDLeftMa;
        private NumericUpDown UDRightMa;
        private NumericUpDown UDTopMa;
        private NumericUpDown upBorderRound;

        public PanelEditor()
        {
            this.InitializeComponent();
            this.comboBox1.Items.Add("Stretch");
            this.comboBox1.Items.Add("Tile");
            this.comboBox1.Items.Add("Center");
            this.comboBox1.Items.Add("Normal");
        }

        public PanelEditor(Steema.TeeChart.Panel p, Control parent) : this()
        {
            this.panel = p;
            this.bColor.Color = this.panel.Color;
            this.button10.Pen = this.panel.Pen;
            this.checkBox1.Checked = this.panel.Color == Color.Transparent;
            this.comboBox1.SelectedIndex = EditorUtils.ImageModeToIndex(this.panel.ImageMode);
            this.bevelEditor = new BevelEditor(this.panel.Bevel, this.panel1);
            if (this.panel.MarginUnits == PanelMarginUnits.Percent)
            {
                this.comboBox2.SelectedIndex = 0;
            }
            else
            {
                this.comboBox2.SelectedIndex = 1;
            }
            this.UDBotMa.Value = (int) this.panel.MarginBottom;
            this.UDRightMa.Value = (int) this.panel.MarginRight;
            this.UDLeftMa.Value = (int) this.panel.MarginLeft;
            this.UDTopMa.Value = (int) this.panel.MarginTop;
            this.upBorderRound.Value = this.panel.BorderRound;
            this.bevelImageEditor = new BevelImageEditor(this.panel.ImageBevel, this.panel2);
            if (this.panel.ImageBevel.Visible)
            {
                this.CBBevels.SelectedIndex = 1;
            }
            else
            {
                this.CBBevels.SelectedIndex = 0;
            }
            EditorUtils.InsertForm(this, parent);
            this.CBBevels_Click(null, null);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.panel.Brush);
            this.bColor.Color = this.panel.Color;
            this.checkBox1.Checked = this.panel.Color == Color.Transparent;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.panel.Color = this.bColor.Color;
            this.checkBox1.Checked = this.panel.Color == Color.Transparent;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.button9.Text == Texts.ClearImage)
            {
                this.panel.Image = null;
                this.pictureBox1.Image = null;
                this.button9.Text = Texts.BrowseImage;
                this.CBImageTrans.Checked = false;
                this.CBImageTrans.Enabled = false;
            }
            else
            {
                string filename = PictureDialog.FileName(this);
                if (filename.Length != 0)
                {
                    this.panel.Image = Image.FromFile(filename);
                    this.pictureBox1.Image = this.panel.Image;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.button9.Text = Texts.ClearImage;
                    this.CBImageTrans.Enabled = true;
                }
            }
        }

        private void CBBevels_Click(object sender, EventArgs e)
        {
            switch (this.CBBevels.SelectedIndex)
            {
                case 0:
                    this.panel.ImageBevel.Visible = false;
                    this.bevelEditor.EnableControls();
                    this.bevelImageEditor.DisableControls();
                    return;

                case 1:
                    this.panel.ImageBevel.Visible = true;
                    this.bevelEditor.DisableControls();
                    this.bevelImageEditor.EnableControls();
                    return;
            }
        }

        private void CBBevels_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CBBevels_Click(sender, e);
        }

        private void CBImageTrans_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CBImageTrans.Checked)
            {
                this.panel.ImageTransparent = true;
            }
            else
            {
                this.panel.ImageTransparent = false;
            }
        }

        private void CBImageTrans_Click(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.panel.Color = Color.Transparent;
                if (this.panel.chart.parent != null)
                {
                    this.panel.chart.parent.DoSetControlStyle();
                }
            }
            else if ((this.panel.Color == Color.FromArgb(0, 0xff, 0xff, 0xff)) || (this.panel.Color == Color.Transparent))
            {
                this.panel.Color = Color.Empty;
                if ((this.panel.Color == Color.FromArgb(0, 0, 0, 0)) || (this.panel.Color == Color.Empty))
                {
                    this.panel.Color = Color.FromArgb(0xff, SystemColors.Control.R, SystemColors.Control.G, SystemColors.Control.B);
                }
            }
            this.bColor.Color = this.panel.Color;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panel.ImageMode = EditorUtils.IndexToImageMode(this.comboBox1.SelectedIndex);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedIndex == 0)
            {
                this.panel.MarginUnits = PanelMarginUnits.Percent;
                this.SetUpDownLimits(0, 100);
            }
            else
            {
                this.panel.MarginUnits = PanelMarginUnits.Pixels;
                this.SetUpDownLimits(0, 0x7d0);
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
            this.PanelPages = new TabControl();
            this.tabPage7 = new TabPage();
            this.panel2 = new GroupBox();
            this.panel1 = new GroupBox();
            this.CBBevels = new ComboBox();
            this.GBMargins = new GroupBox();
            this.comboBox2 = new ComboBox();
            this.label2 = new Label();
            this.button10 = new ButtonPen();
            this.tabPage6 = new TabPage();
            this.checkBox1 = new CheckBox();
            this.button7 = new Button();
            this.groupBox1 = new GroupBox();
            this.comboBox1 = new ComboBox();
            this.label1 = new Label();
            this.pictureBox1 = new PictureBox();
            this.button9 = new Button();
            this.CBImageTrans = new CheckBox();
            this.bColor = new ButtonColor();
            this.tpMargins = new TabPage();
            this.label6 = new Label();
            this.UDBotMa = new NumericUpDown();
            this.UDLeftMa = new NumericUpDown();
            this.UDTopMa = new NumericUpDown();
            this.UDRightMa = new NumericUpDown();
            this.upBorderRound = new NumericUpDown();
            this.tabPagePanelGradient = new TabPage();
            this.tabShadow = new TabPage();
            this.PanelPages.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            this.GBMargins.SuspendLayout();
            this.tpMargins.SuspendLayout();
            this.UDBotMa.BeginInit();
            this.UDLeftMa.BeginInit();
            this.UDTopMa.BeginInit();
            this.UDRightMa.BeginInit();
            this.upBorderRound.BeginInit();
            base.SuspendLayout();
            this.PanelPages.Controls.Add(this.tabPage7);
            this.PanelPages.Controls.Add(this.tabPage6);
            this.PanelPages.Controls.Add(this.tpMargins);
            this.PanelPages.Controls.Add(this.tabPagePanelGradient);
            this.PanelPages.Controls.Add(this.tabShadow);
            this.PanelPages.Dock = DockStyle.Fill;
            this.PanelPages.HotTrack = true;
            this.PanelPages.Location = new Point(0, 0);
            this.PanelPages.Name = "PanelPages";
            this.PanelPages.SelectedIndex = 0;
            this.PanelPages.Size = new Size(0x188, 0xed);
            this.PanelPages.TabIndex = 1;
            this.PanelPages.SelectedIndexChanged += new EventHandler(this.PanelPages_SelectedIndexChanged);
            this.tabPage7.Controls.Add(this.panel2);
            this.tabPage7.Controls.Add(this.panel1);
            this.tabPage7.Controls.Add(this.upBorderRound);
            this.tabPage7.Controls.Add(this.label2);
            this.tabPage7.Controls.Add(this.CBBevels);
            this.tabPage7.Controls.Add(this.button10);
            this.tabPage7.Location = new Point(4, 0x16);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new Size(0x180, 0xd3);
            this.tabPage7.TabIndex = 1;
            this.tabPage7.Text = "Borders";
            this.tabPage7.UseVisualStyleBackColor = true;
            this.panel2.Location = new Point(200, 0x30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0xb6, 0x88);
            this.panel2.TabIndex = 6;
            this.panel2.TabStop = false;
            this.panel2.Text = "Image Bevel";
            this.panel1.Location = new Point(0, 0x30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(200, 0x88);
            this.panel1.TabIndex = 5;
            this.panel1.TabStop = false;
            this.panel1.Text = "Bevel";
            this.CBBevels.Items.AddRange(new object[] { "Bevel", "Image Bevel" });
            this.CBBevels.Location = new Point(110, 0x12);
            this.CBBevels.Name = "CBBevels";
            this.CBBevels.Size = new Size(0x6f, 0x15);
            this.CBBevels.TabIndex = 3;
            this.CBBevels.Text = "Bevel";
            this.CBBevels.SelectedIndexChanged += new EventHandler(this.CBBevels_SelectedIndexChanged);
            this.CBBevels.Click += new EventHandler(this.CBBevels_Click);
            this.button10.FlatStyle = FlatStyle.Flat;
            this.button10.Location = new Point(0x10, 0x10);
            this.button10.Name = "button10";
            this.button10.Size = new Size(0x4b, 0x17);
            this.button10.TabIndex = 1;
            this.button10.Text = "&Border...";
            this.tabPage6.Controls.Add(this.checkBox1);
            this.tabPage6.Controls.Add(this.button7);
            this.tabPage6.Controls.Add(this.groupBox1);
            this.tabPage6.Controls.Add(this.bColor);
            this.tabPage6.Location = new Point(4, 0x16);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new Size(0x180, 0xd3);
            this.tabPage6.TabIndex = 0;
            this.tabPage6.Text = "Background";
            this.tabPage6.UseVisualStyleBackColor = true;
            this.tabPage6.Visible = false;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(0x10, 0x26);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x88, 0x18);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "&Transparent";
            this.checkBox1.Click += new EventHandler(this.checkBox1_Click);
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.button7.FlatStyle = FlatStyle.Flat;
            this.button7.Location = new Point(0x80, 8);
            this.button7.Name = "button7";
            this.button7.Size = new Size(0x4b, 0x17);
            this.button7.TabIndex = 2;
            this.button7.Text = "&Pattern...";
            this.button7.Click += new EventHandler(this.button7_Click);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.CBImageTrans);
            this.groupBox1.Location = new Point(0x10, 0x40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(200, 0x7f);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Background Image:";
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new Point(0x10, 0x4c);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x58, 0x15);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x10, 0x3b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x21, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Style:";
            this.pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            this.pictureBox1.Location = new Point(0x77, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x43, 0x4c);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.button9.FlatStyle = FlatStyle.Flat;
            this.button9.Location = new Point(14, 0x18);
            this.button9.Name = "button9";
            this.button9.Size = new Size(0x4b, 0x17);
            this.button9.TabIndex = 0;
            this.button9.Text = "&Browse...";
            this.button9.Click += new EventHandler(this.button9_Click);
            this.CBImageTrans.FlatStyle = FlatStyle.Flat;
            this.CBImageTrans.Location = new Point(0x11, 0x62);
            this.CBImageTrans.Name = "CBImageTrans";
            this.CBImageTrans.Size = new Size(0x88, 0x18);
            this.CBImageTrans.TabIndex = 4;
            this.CBImageTrans.Text = "&Transparent";
            this.CBImageTrans.Click += new EventHandler(this.CBImageTrans_Click);
            this.CBImageTrans.CheckedChanged += new EventHandler(this.CBImageTrans_CheckedChanged);
            this.bColor.Color = Color.Empty;
            this.bColor.Location = new Point(0x10, 8);
            this.bColor.Name = "bColor";
            this.bColor.Size = new Size(0x4b, 0x17);
            this.bColor.TabIndex = 0;
            this.bColor.Text = "&Color...";
            this.bColor.Click += new EventHandler(this.button8_Click);
            this.tpMargins.Controls.Add(this.GBMargins);
            this.tpMargins.Location = new Point(4, 0x16);
            this.tpMargins.Name = "tpMargins";
            this.tpMargins.Size = new Size(0x180, 0xd3);
            this.tpMargins.TabIndex = 4;
            this.tpMargins.Text = "Margins";
            this.tpMargins.UseVisualStyleBackColor = true;
            this.GBMargins.Controls.Add(this.comboBox2);
            this.GBMargins.Controls.Add(this.label6);
            this.GBMargins.Controls.Add(this.UDBotMa);
            this.GBMargins.Controls.Add(this.UDLeftMa);
            this.GBMargins.Controls.Add(this.UDTopMa);
            this.GBMargins.Controls.Add(this.UDRightMa);
            this.GBMargins.Location = new Point(8, 0x10);
            this.GBMargins.Name = "GBMargins";
            this.GBMargins.Size = new Size(0x88, 0x88);
            this.GBMargins.TabIndex = 2;
            this.GBMargins.TabStop = false;
            this.GBMargins.Text = "Margins (%)";
            this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox2.Items.AddRange(new object[] { "Percent", "Pixels" });
            this.comboBox2.Location = new Point(14, 0x6b);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new Size(0x68, 0x15);
            this.comboBox2.TabIndex = 5;
            this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(12, 0x58);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x22, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "&Units:";
            this.UDBotMa.BorderStyle = BorderStyle.FixedSingle;
            this.UDBotMa.Location = new Point(0x30, 0x3e);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.UDBotMa.Maximum = new decimal(bits);
            this.UDBotMa.Name = "UDBotMa";
            this.UDBotMa.Size = new Size(40, 20);
            this.UDBotMa.TabIndex = 3;
            this.UDBotMa.TextAlign = HorizontalAlignment.Right;
            int[] numArray2 = new int[4];
            numArray2[0] = 3;
            this.UDBotMa.Value = new decimal(numArray2);
            this.UDBotMa.ValueChanged += new EventHandler(this.UDBotMa_ValueChanged);
            this.UDLeftMa.BorderStyle = BorderStyle.FixedSingle;
            this.UDLeftMa.Location = new Point(8, 40);
            int[] numArray3 = new int[4];
            numArray3[0] = 0x3e8;
            this.UDLeftMa.Maximum = new decimal(numArray3);
            this.UDLeftMa.Name = "UDLeftMa";
            this.UDLeftMa.Size = new Size(40, 20);
            this.UDLeftMa.TabIndex = 1;
            this.UDLeftMa.TextAlign = HorizontalAlignment.Right;
            int[] numArray4 = new int[4];
            numArray4[0] = 3;
            this.UDLeftMa.Value = new decimal(numArray4);
            this.UDLeftMa.ValueChanged += new EventHandler(this.UDLeftMa_ValueChanged);
            this.UDTopMa.BorderStyle = BorderStyle.FixedSingle;
            this.UDTopMa.Location = new Point(0x30, 0x10);
            int[] numArray5 = new int[4];
            numArray5[0] = 0x3e8;
            this.UDTopMa.Maximum = new decimal(numArray5);
            this.UDTopMa.Name = "UDTopMa";
            this.UDTopMa.Size = new Size(40, 20);
            this.UDTopMa.TabIndex = 0;
            this.UDTopMa.TextAlign = HorizontalAlignment.Right;
            int[] numArray6 = new int[4];
            numArray6[0] = 3;
            this.UDTopMa.Value = new decimal(numArray6);
            this.UDTopMa.ValueChanged += new EventHandler(this.UDTopMa_ValueChanged);
            this.UDRightMa.BorderStyle = BorderStyle.FixedSingle;
            this.UDRightMa.Location = new Point(0x58, 40);
            int[] numArray7 = new int[4];
            numArray7[0] = 0x3e8;
            this.UDRightMa.Maximum = new decimal(numArray7);
            this.UDRightMa.Name = "UDRightMa";
            this.UDRightMa.Size = new Size(40, 20);
            this.UDRightMa.TabIndex = 2;
            this.UDRightMa.TextAlign = HorizontalAlignment.Right;
            int[] numArray8 = new int[4];
            numArray8[0] = 3;
            this.UDRightMa.Value = new decimal(numArray8);
            this.UDRightMa.ValueChanged += new EventHandler(this.UDRightMa_ValueChanged);
            this.tabPagePanelGradient.Location = new Point(4, 0x16);
            this.tabPagePanelGradient.Name = "tabPagePanelGradient";
            this.tabPagePanelGradient.Size = new Size(0x180, 0xd3);
            this.tabPagePanelGradient.TabIndex = 2;
            this.tabPagePanelGradient.Text = "Gradient";
            this.tabPagePanelGradient.UseVisualStyleBackColor = true;
            this.tabPagePanelGradient.Visible = false;
            this.tabShadow.Location = new Point(4, 0x16);
            this.tabShadow.Name = "tabShadow";
            this.tabShadow.Size = new Size(0x180, 0xd3);
            this.tabShadow.TabIndex = 3;
            this.tabShadow.Text = "Shadow";
            this.tabShadow.UseVisualStyleBackColor = true;
            this.upBorderRound.Location = new Point(0x146, 0x13);
            this.upBorderRound.Name = "upBorderRound";
            this.upBorderRound.Size = new Size(50, 20);
            this.upBorderRound.TabIndex = 7;
            this.upBorderRound.Click += new EventHandler(this.upBorderRound_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xf4, 0x15);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4c, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "&Round Border:";
            base.ClientSize = new Size(0x188, 0xed);
            base.Controls.Add(this.PanelPages);
            base.Name = "PanelEditor";
            this.Text = "Panel Editor";
            this.PanelPages.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((ISupportInitialize) this.pictureBox1).EndInit();
            this.tpMargins.ResumeLayout(false);
            this.GBMargins.ResumeLayout(false);
            this.GBMargins.PerformLayout();
            this.UDBotMa.EndInit();
            this.UDLeftMa.EndInit();
            this.UDTopMa.EndInit();
            this.UDRightMa.EndInit();
            this.upBorderRound.EndInit();
            base.ResumeLayout(false);
        }

        private void PanelPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.PanelPages.SelectedTab == this.tabPagePanelGradient) & (this.gradientEditor == null))
            {
                this.gradientEditor = new GradientEditor(this.panel.Gradient, this.tabPagePanelGradient);
                EditorUtils.Translate(this.gradientEditor);
            }
            else if ((this.PanelPages.SelectedTab == this.tabShadow) & (this.shadowEditor == null))
            {
                this.shadowEditor = new ShadowEditor(this.panel.Shadow, this.tabShadow);
                EditorUtils.Translate(this.shadowEditor);
            }
            else if (this.PanelPages.SelectedTab == this.tabPage6)
            {
                this.checkBox1.Checked = (this.panel.Color == Color.Transparent) || (this.panel.Color == Color.FromArgb(0, 0xff, 0xff, 0xff));
                if (this.panel.Image != null)
                {
                    this.pictureBox1.Image = this.panel.Image;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.button9.Text = Texts.ClearImage;
                    this.CBImageTrans.Enabled = true;
                    this.CBImageTrans.Checked = this.panel.ImageTransparent;
                }
                else
                {
                    this.CBImageTrans.Enabled = false;
                }
            }
        }

        private void SetUpDownLimits(int min, int max)
        {
            this.UDBotMa.Minimum = min;
            this.UDBotMa.Maximum = max;
            this.UDTopMa.Minimum = min;
            this.UDTopMa.Maximum = max;
            this.UDLeftMa.Minimum = min;
            this.UDLeftMa.Maximum = max;
            this.UDRightMa.Minimum = min;
            this.UDRightMa.Maximum = max;
        }

        private void UDBotMa_ValueChanged(object sender, EventArgs e)
        {
            if (this.panel != null)
            {
                this.panel.MarginBottom = Convert.ToDouble(this.UDBotMa.Value);
            }
        }

        private void UDLeftMa_ValueChanged(object sender, EventArgs e)
        {
            if (this.panel != null)
            {
                this.panel.MarginLeft = Convert.ToDouble(this.UDLeftMa.Value);
            }
        }

        private void UDRightMa_ValueChanged(object sender, EventArgs e)
        {
            if (this.panel != null)
            {
                this.panel.MarginRight = Convert.ToDouble(this.UDRightMa.Value);
            }
        }

        private void UDTopMa_ValueChanged(object sender, EventArgs e)
        {
            if (this.panel != null)
            {
                this.panel.MarginTop = Convert.ToDouble(this.UDTopMa.Value);
            }
        }

        private void upBorderRound_Click(object sender, EventArgs e)
        {
            this.panel.BorderRound = Convert.ToInt32(this.upBorderRound.Value);
            this.panel.Chart.Parent.DoInvalidate();
        }
    }
}

