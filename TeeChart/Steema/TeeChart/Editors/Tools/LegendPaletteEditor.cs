namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class LegendPaletteEditor : ToolSeriesEditor
    {
        private AxesEditor axeseditor;
        private ButtonPen buttonPen;
        private ButtonPen buttonPenBorder;
        private CheckBox cboxInverted;
        private CheckBox cboxSmooth;
        private CheckBox cboxTransparent;
        private CheckBox cboxVertical;
        private ComboBox comboBoxAxis;
        private ComboBox comboBoxUnits;
        private IContainer components;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private NumericUpDown numericUpDownHeight;
        private NumericUpDown numericUpDownLeft;
        private NumericUpDown numericUpDownTop;
        private NumericUpDown numericUpDownWidth;
        private Steema.TeeChart.Editors.PanelEditor paneleditor;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private LegendPalette tool;

        public LegendPaletteEditor()
        {
            this.InitializeComponent();
        }

        public LegendPaletteEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.tool = t as LegendPalette;
            base.SetTool(this.tool, null);
            base.setting = true;
            if (this.tool != null)
            {
                this.cboxInverted.Checked = this.tool.Inverted;
                this.cboxSmooth.Checked = this.tool.Smooth;
                this.cboxTransparent.Checked = this.tool.Transparent;
                this.cboxVertical.Checked = this.tool.Vertical;
                this.numericUpDownHeight.Value = this.tool.Height;
                this.numericUpDownWidth.Value = this.tool.Width;
                this.numericUpDownTop.Value = this.tool.Top;
                this.numericUpDownLeft.Value = this.tool.Left;
                this.comboBoxAxis.SelectedIndex = (int) this.tool.Axis;
                this.comboBoxUnits.SelectedIndex = (int) this.tool.PositionUnits;
                this.buttonPen.Pen = this.tool.Pen;
                this.buttonPenBorder.Pen = this.tool.Border;
                if (this.paneleditor == null)
                {
                    this.paneleditor = new Steema.TeeChart.Editors.PanelEditor(this.tool.fChart.Panel, this.tabPage4);
                }
                if (this.axeseditor == null)
                {
                    this.axeseditor = new AxesEditor(this.tool.fChart.Chart, this.tabPage5);
                }
            }
            EditorUtils.Translate(this);
            base.setting = false;
        }

        private void cboxInverted_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Inverted = this.cboxInverted.Checked;
            }
        }

        private void cboxSmooth_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Smooth = this.cboxSmooth.Checked;
            }
        }

        private void cboxTransparent_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Transparent = this.cboxTransparent.Checked;
            }
        }

        private void cboxVertical_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Vertical = this.cboxVertical.Checked;
            }
        }

        private void comboBoxAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Axis = (LegendPaletteAxis) this.comboBoxAxis.SelectedIndex;
            }
        }

        private void comboBoxUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.PositionUnits = (PositionUnits) this.comboBoxUnits.SelectedIndex;
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
            this.tabPage1 = new TabPage();
            this.comboBoxAxis = new ComboBox();
            this.label2 = new Label();
            this.cboxSmooth = new CheckBox();
            this.cboxTransparent = new CheckBox();
            this.cboxInverted = new CheckBox();
            this.cboxVertical = new CheckBox();
            this.buttonPenBorder = new ButtonPen();
            this.buttonPen = new ButtonPen();
            this.tabPage2 = new TabPage();
            this.numericUpDownTop = new NumericUpDown();
            this.numericUpDownLeft = new NumericUpDown();
            this.comboBoxUnits = new ComboBox();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.tabPage3 = new TabPage();
            this.numericUpDownHeight = new NumericUpDown();
            this.label7 = new Label();
            this.numericUpDownWidth = new NumericUpDown();
            this.label6 = new Label();
            this.tabPage4 = new TabPage();
            this.tabPage5 = new TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.numericUpDownTop.BeginInit();
            this.numericUpDownLeft.BeginInit();
            this.tabPage3.SuspendLayout();
            this.numericUpDownHeight.BeginInit();
            this.numericUpDownWidth.BeginInit();
            base.SuspendLayout();
            this.tabControl1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new Point(12, 0x31);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0xce, 0xa4);
            this.tabControl1.TabIndex = 2;
            this.tabPage1.Controls.Add(this.comboBoxAxis);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.cboxSmooth);
            this.tabPage1.Controls.Add(this.cboxTransparent);
            this.tabPage1.Controls.Add(this.cboxInverted);
            this.tabPage1.Controls.Add(this.cboxVertical);
            this.tabPage1.Controls.Add(this.buttonPenBorder);
            this.tabPage1.Controls.Add(this.buttonPen);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(0xc6, 0x8a);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Options";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.comboBoxAxis.FormattingEnabled = true;
            this.comboBoxAxis.Items.AddRange(new object[] { "Default", "Other", "Both" });
            this.comboBoxAxis.Location = new Point(0x62, 0x30);
            this.comboBoxAxis.Name = "comboBoxAxis";
            this.comboBoxAxis.Size = new Size(0x5e, 0x15);
            this.comboBoxAxis.TabIndex = 7;
            this.comboBoxAxis.SelectedIndexChanged += new EventHandler(this.comboBoxAxis_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x5f, 0x1b);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1a, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Axis";
            this.cboxSmooth.AutoSize = true;
            this.cboxSmooth.FlatStyle = FlatStyle.Flat;
            this.cboxSmooth.Location = new Point(0x62, 0x6d);
            this.cboxSmooth.Name = "cboxSmooth";
            this.cboxSmooth.Size = new Size(0x3b, 0x11);
            this.cboxSmooth.TabIndex = 5;
            this.cboxSmooth.Text = "Smooth";
            this.cboxSmooth.UseVisualStyleBackColor = true;
            this.cboxSmooth.CheckedChanged += new EventHandler(this.cboxSmooth_CheckedChanged);
            this.cboxTransparent.AutoSize = true;
            this.cboxTransparent.FlatStyle = FlatStyle.Flat;
            this.cboxTransparent.Location = new Point(0x62, 0x56);
            this.cboxTransparent.Name = "cboxTransparent";
            this.cboxTransparent.Size = new Size(80, 0x11);
            this.cboxTransparent.TabIndex = 4;
            this.cboxTransparent.Text = "Transparent";
            this.cboxTransparent.UseVisualStyleBackColor = true;
            this.cboxTransparent.CheckedChanged += new EventHandler(this.cboxTransparent_CheckedChanged);
            this.cboxInverted.AutoSize = true;
            this.cboxInverted.FlatStyle = FlatStyle.Flat;
            this.cboxInverted.Location = new Point(6, 0x6d);
            this.cboxInverted.Name = "cboxInverted";
            this.cboxInverted.Size = new Size(0x3e, 0x11);
            this.cboxInverted.TabIndex = 3;
            this.cboxInverted.Text = "Inverted";
            this.cboxInverted.UseVisualStyleBackColor = true;
            this.cboxInverted.CheckedChanged += new EventHandler(this.cboxInverted_CheckedChanged);
            this.cboxVertical.AutoSize = true;
            this.cboxVertical.FlatStyle = FlatStyle.Flat;
            this.cboxVertical.Location = new Point(6, 0x56);
            this.cboxVertical.Name = "cboxVertical";
            this.cboxVertical.Size = new Size(0x3a, 0x11);
            this.cboxVertical.TabIndex = 2;
            this.cboxVertical.Text = "Vertical";
            this.cboxVertical.UseVisualStyleBackColor = true;
            this.cboxVertical.CheckedChanged += new EventHandler(this.cboxVertical_CheckedChanged);
            this.buttonPenBorder.FlatStyle = FlatStyle.Flat;
            this.buttonPenBorder.Location = new Point(6, 0x2e);
            this.buttonPenBorder.Name = "buttonPenBorder";
            this.buttonPenBorder.Size = new Size(0x4b, 0x17);
            this.buttonPenBorder.TabIndex = 1;
            this.buttonPenBorder.Text = "Border...";
            this.buttonPenBorder.UseVisualStyleBackColor = true;
            this.buttonPen.FlatStyle = FlatStyle.Flat;
            this.buttonPen.Location = new Point(6, 0x11);
            this.buttonPen.Name = "buttonPen";
            this.buttonPen.Size = new Size(0x4b, 0x17);
            this.buttonPen.TabIndex = 0;
            this.buttonPen.Text = "Pen...";
            this.buttonPen.UseVisualStyleBackColor = true;
            this.tabPage2.Controls.Add(this.numericUpDownTop);
            this.tabPage2.Controls.Add(this.numericUpDownLeft);
            this.tabPage2.Controls.Add(this.comboBoxUnits);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(0xc6, 0x8a);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Position";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.numericUpDownTop.Location = new Point(0x35, 0x4b);
            int[] bits = new int[4];
            bits[0] = 0x7d0;
            this.numericUpDownTop.Maximum = new decimal(bits);
            this.numericUpDownTop.Name = "numericUpDownTop";
            this.numericUpDownTop.Size = new Size(0x35, 20);
            this.numericUpDownTop.TabIndex = 5;
            this.numericUpDownTop.ValueChanged += new EventHandler(this.numericUpDownTop_ValueChanged);
            this.numericUpDownLeft.Location = new Point(0x35, 0x2c);
            int[] numArray2 = new int[4];
            numArray2[0] = 0x7d0;
            this.numericUpDownLeft.Maximum = new decimal(numArray2);
            this.numericUpDownLeft.Name = "numericUpDownLeft";
            this.numericUpDownLeft.Size = new Size(0x35, 20);
            this.numericUpDownLeft.TabIndex = 4;
            this.numericUpDownLeft.ValueChanged += new EventHandler(this.numericUpDownLeft_ValueChanged);
            this.comboBoxUnits.FormattingEnabled = true;
            this.comboBoxUnits.Items.AddRange(new object[] { "Percent", "Pixels" });
            this.comboBoxUnits.Location = new Point(0x35, 14);
            this.comboBoxUnits.Name = "comboBoxUnits";
            this.comboBoxUnits.Size = new Size(0x66, 0x15);
            this.comboBoxUnits.TabIndex = 3;
            this.comboBoxUnits.SelectedIndexChanged += new EventHandler(this.comboBoxUnits_SelectedIndexChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(6, 0x4b);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x1a, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Top";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(6, 0x2e);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x19, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Left";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(6, 0x11);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1f, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Units";
            this.tabPage3.Controls.Add(this.numericUpDownHeight);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.numericUpDownWidth);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(0xc6, 0x8a);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Size";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.numericUpDownHeight.Location = new Point(0x36, 0x2c);
            int[] numArray3 = new int[4];
            numArray3[0] = 0x7d0;
            this.numericUpDownHeight.Maximum = new decimal(numArray3);
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new Size(0x35, 20);
            this.numericUpDownHeight.TabIndex = 8;
            this.numericUpDownHeight.ValueChanged += new EventHandler(this.numericUpDownHeight_ValueChanged);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(7, 0x2e);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x26, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Height";
            this.numericUpDownWidth.Location = new Point(0x36, 0x12);
            int[] numArray4 = new int[4];
            numArray4[0] = 0x7d0;
            this.numericUpDownWidth.Maximum = new decimal(numArray4);
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new Size(0x35, 20);
            this.numericUpDownWidth.TabIndex = 6;
            this.numericUpDownWidth.ValueChanged += new EventHandler(this.numericUpDownWidth_ValueChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(7, 20);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x23, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Width";
            this.tabPage4.Location = new Point(4, 0x16);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new Size(0xc6, 0x8a);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Panel";
            this.tabPage4.UseVisualStyleBackColor = true;
            this.tabPage5.Location = new Point(4, 0x16);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new Size(0xc6, 0x8a);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Axes";
            this.tabPage5.UseVisualStyleBackColor = true;
            base.ClientSize = new Size(0xe9, 0xd7);
            base.Controls.Add(this.tabControl1);
            base.Name = "LegendPaletteEditor";
            base.Controls.SetChildIndex(this.tabControl1, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.numericUpDownTop.EndInit();
            this.numericUpDownLeft.EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.numericUpDownHeight.EndInit();
            this.numericUpDownWidth.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void numericUpDownHeight_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Height = (int) this.numericUpDownHeight.Value;
            }
        }

        private void numericUpDownLeft_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Left = (int) this.numericUpDownLeft.Value;
            }
        }

        private void numericUpDownTop_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Top = (int) this.numericUpDownTop.Value;
            }
        }

        private void numericUpDownWidth_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Width = (int) this.numericUpDownWidth.Value;
            }
        }
    }
}

