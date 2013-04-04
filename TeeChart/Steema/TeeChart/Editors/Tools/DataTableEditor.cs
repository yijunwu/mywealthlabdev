namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DataTableEditor : Form
    {
        private ButtonPen buttonPenColumn;
        private ButtonPen buttonPenRow;
        private CheckBox checkBoxFontColor;
        private CheckBox checkBoxInverted;
        private CheckBox checkBoxLegendOS;
        private CheckBox checkBoxLegendVisible;
        private CheckBox checkBoxPosAuto;
        private IContainer components;
        private GroupBox groupBoxPosition;
        private Label label1;
        private Label label2;
        private TextEditor legendfont;
        private NumericUpDown numericUpDownLeft;
        private NumericUpDown numericUpDownTop;
        private bool setting;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TextEditor tablefont;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPageFormat;
        private TabPage tabPageLegend;
        private TabPage tabPageText;
        private DataTableTool tool;

        public DataTableEditor()
        {
            this.setting = true;
            this.InitializeComponent();
        }

        public DataTableEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.setting = true;
            this.tool = (DataTableTool) t;
            if (this.tool != null)
            {
                this.checkBoxInverted.Checked = this.tool.Inverted;
                this.buttonPenRow.Pen = this.tool.RowPen;
                this.buttonPenColumn.Pen = this.tool.ColumnPen;
                this.checkBoxPosAuto.Checked = this.tool.AutoPosition;
                this.numericUpDownLeft.Value = this.tool.Left;
                this.numericUpDownTop.Value = this.tool.Top;
                this.checkBoxLegendVisible.Checked = this.tool.TableLegend.Visible;
                this.checkBoxFontColor.Checked = this.tool.TableLegend.FontSeriesColor;
                this.checkBoxLegendOS.Checked = this.tool.TableLegend.OtherSide;
                if (this.tablefont == null)
                {
                    this.tablefont = new TextEditor(this.tool.Font, this.tabPageText);
                }
                if (this.legendfont == null)
                {
                    this.legendfont = new TextEditor(this.tool.TableLegend.Font, this.tabPage3);
                }
            }
            this.setting = false;
            EditorUtils.Translate(this);
        }

        private void checkBoxFontColor_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.TableLegend.FontSeriesColor = this.checkBoxFontColor.Checked;
            }
        }

        private void checkBoxInverted_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.Inverted = this.checkBoxInverted.Checked;
            }
        }

        private void checkBoxLegendOS_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.TableLegend.OtherSide = this.checkBoxLegendOS.Checked;
            }
        }

        private void checkBoxLegendVisible_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.TableLegend.Visible = this.checkBoxLegendVisible.Checked;
            }
        }

        private void checkBoxPosAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.AutoPosition = this.checkBoxPosAuto.Checked;
            }
            this.numericUpDownLeft.Enabled = !this.checkBoxPosAuto.Checked;
            this.numericUpDownTop.Enabled = !this.checkBoxPosAuto.Checked;
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
            this.tabPageFormat = new TabPage();
            this.groupBoxPosition = new GroupBox();
            this.numericUpDownTop = new NumericUpDown();
            this.numericUpDownLeft = new NumericUpDown();
            this.label2 = new Label();
            this.label1 = new Label();
            this.checkBoxPosAuto = new CheckBox();
            this.checkBoxInverted = new CheckBox();
            this.buttonPenColumn = new ButtonPen();
            this.buttonPenRow = new ButtonPen();
            this.tabPageLegend = new TabPage();
            this.tabPageText = new TabPage();
            this.tabControl2 = new TabControl();
            this.tabPage1 = new TabPage();
            this.tabPage2 = new TabPage();
            this.tabPage3 = new TabPage();
            this.checkBoxLegendVisible = new CheckBox();
            this.checkBoxFontColor = new CheckBox();
            this.checkBoxLegendOS = new CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPageFormat.SuspendLayout();
            this.groupBoxPosition.SuspendLayout();
            this.numericUpDownTop.BeginInit();
            this.numericUpDownLeft.BeginInit();
            this.tabPageLegend.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabPageFormat);
            this.tabControl1.Controls.Add(this.tabPageLegend);
            this.tabControl1.Controls.Add(this.tabPageText);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x124, 0x10a);
            this.tabControl1.TabIndex = 0;
            this.tabPageFormat.Controls.Add(this.groupBoxPosition);
            this.tabPageFormat.Controls.Add(this.checkBoxInverted);
            this.tabPageFormat.Controls.Add(this.buttonPenColumn);
            this.tabPageFormat.Controls.Add(this.buttonPenRow);
            this.tabPageFormat.Location = new Point(4, 0x16);
            this.tabPageFormat.Name = "tabPageFormat";
            this.tabPageFormat.Padding = new Padding(3);
            this.tabPageFormat.Size = new Size(0x11c, 240);
            this.tabPageFormat.TabIndex = 0;
            this.tabPageFormat.Text = "Format";
            this.tabPageFormat.UseVisualStyleBackColor = true;
            this.groupBoxPosition.Controls.Add(this.numericUpDownTop);
            this.groupBoxPosition.Controls.Add(this.numericUpDownLeft);
            this.groupBoxPosition.Controls.Add(this.label2);
            this.groupBoxPosition.Controls.Add(this.label1);
            this.groupBoxPosition.Controls.Add(this.checkBoxPosAuto);
            this.groupBoxPosition.Location = new Point(8, 0x53);
            this.groupBoxPosition.Name = "groupBoxPosition";
            this.groupBoxPosition.Size = new Size(0xa5, 0x70);
            this.groupBoxPosition.TabIndex = 3;
            this.groupBoxPosition.TabStop = false;
            this.groupBoxPosition.Text = "Position";
            this.numericUpDownTop.Location = new Point(0x49, 0x51);
            int[] bits = new int[4];
            bits[0] = 0x7d0;
            this.numericUpDownTop.Maximum = new decimal(bits);
            this.numericUpDownTop.Name = "numericUpDownTop";
            this.numericUpDownTop.Size = new Size(0x42, 20);
            this.numericUpDownTop.TabIndex = 4;
            this.numericUpDownTop.ValueChanged += new EventHandler(this.numericUpDownTop_ValueChanged);
            this.numericUpDownLeft.Location = new Point(0x49, 0x3a);
            int[] numArray2 = new int[4];
            numArray2[0] = 0x7d0;
            this.numericUpDownLeft.Maximum = new decimal(numArray2);
            this.numericUpDownLeft.Name = "numericUpDownLeft";
            this.numericUpDownLeft.Size = new Size(0x42, 20);
            this.numericUpDownLeft.TabIndex = 3;
            this.numericUpDownLeft.ValueChanged += new EventHandler(this.numericUpDownLeft_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x27, 0x53);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1a, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Top";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(40, 60);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x19, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Left";
            this.checkBoxPosAuto.AutoSize = true;
            this.checkBoxPosAuto.FlatStyle = FlatStyle.Flat;
            this.checkBoxPosAuto.Location = new Point(6, 0x13);
            this.checkBoxPosAuto.Name = "checkBoxPosAuto";
            this.checkBoxPosAuto.Size = new Size(70, 0x11);
            this.checkBoxPosAuto.TabIndex = 0;
            this.checkBoxPosAuto.Text = "Automatic";
            this.checkBoxPosAuto.UseVisualStyleBackColor = true;
            this.checkBoxPosAuto.CheckedChanged += new EventHandler(this.checkBoxPosAuto_CheckedChanged);
            this.checkBoxInverted.AutoSize = true;
            this.checkBoxInverted.FlatStyle = FlatStyle.Flat;
            this.checkBoxInverted.Location = new Point(8, 60);
            this.checkBoxInverted.Name = "checkBoxInverted";
            this.checkBoxInverted.Size = new Size(0x3e, 0x11);
            this.checkBoxInverted.TabIndex = 2;
            this.checkBoxInverted.Text = "Inverted";
            this.checkBoxInverted.UseVisualStyleBackColor = true;
            this.checkBoxInverted.CheckedChanged += new EventHandler(this.checkBoxInverted_CheckedChanged);
            this.buttonPenColumn.FlatStyle = FlatStyle.Flat;
            this.buttonPenColumn.Location = new Point(0x62, 15);
            this.buttonPenColumn.Name = "buttonPenColumn";
            this.buttonPenColumn.Size = new Size(0x4b, 0x17);
            this.buttonPenColumn.TabIndex = 1;
            this.buttonPenColumn.Text = "Columns...";
            this.buttonPenColumn.UseVisualStyleBackColor = true;
            this.buttonPenRow.FlatStyle = FlatStyle.Flat;
            this.buttonPenRow.Location = new Point(8, 15);
            this.buttonPenRow.Name = "buttonPenRow";
            this.buttonPenRow.Size = new Size(0x4b, 0x17);
            this.buttonPenRow.TabIndex = 0;
            this.buttonPenRow.Text = "Rows...";
            this.buttonPenRow.UseVisualStyleBackColor = true;
            this.tabPageLegend.Controls.Add(this.tabControl2);
            this.tabPageLegend.Location = new Point(4, 0x16);
            this.tabPageLegend.Name = "tabPageLegend";
            this.tabPageLegend.Padding = new Padding(3);
            this.tabPageLegend.Size = new Size(0x11c, 240);
            this.tabPageLegend.TabIndex = 1;
            this.tabPageLegend.Text = "Legend";
            this.tabPageLegend.UseVisualStyleBackColor = true;
            this.tabPageText.Location = new Point(4, 0x16);
            this.tabPageText.Name = "tabPageText";
            this.tabPageText.Size = new Size(0x11c, 240);
            this.tabPageText.TabIndex = 2;
            this.tabPageText.Text = "Text";
            this.tabPageText.UseVisualStyleBackColor = true;
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = DockStyle.Fill;
            this.tabControl2.Location = new Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new Size(0x116, 0xea);
            this.tabControl2.TabIndex = 0;
            this.tabPage1.Controls.Add(this.checkBoxLegendOS);
            this.tabPage1.Controls.Add(this.checkBoxFontColor);
            this.tabPage1.Controls.Add(this.checkBoxLegendVisible);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(270, 0xd0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Format";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(270, 0xd0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Symbol";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(270, 0xd0);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Text";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.checkBoxLegendVisible.AutoSize = true;
            this.checkBoxLegendVisible.FlatStyle = FlatStyle.Flat;
            this.checkBoxLegendVisible.Location = new Point(6, 20);
            this.checkBoxLegendVisible.Name = "checkBoxLegendVisible";
            this.checkBoxLegendVisible.Size = new Size(0x35, 0x11);
            this.checkBoxLegendVisible.TabIndex = 0;
            this.checkBoxLegendVisible.Text = "Visible";
            this.checkBoxLegendVisible.UseVisualStyleBackColor = true;
            this.checkBoxLegendVisible.CheckedChanged += new EventHandler(this.checkBoxLegendVisible_CheckedChanged);
            this.checkBoxFontColor.AutoSize = true;
            this.checkBoxFontColor.FlatStyle = FlatStyle.Flat;
            this.checkBoxFontColor.Location = new Point(6, 0x2b);
            this.checkBoxFontColor.Name = "checkBoxFontColor";
            this.checkBoxFontColor.Size = new Size(0x63, 0x11);
            this.checkBoxFontColor.TabIndex = 1;
            this.checkBoxFontColor.Text = "Series font color";
            this.checkBoxFontColor.UseVisualStyleBackColor = true;
            this.checkBoxFontColor.CheckedChanged += new EventHandler(this.checkBoxFontColor_CheckedChanged);
            this.checkBoxLegendOS.AutoSize = true;
            this.checkBoxLegendOS.FlatStyle = FlatStyle.Flat;
            this.checkBoxLegendOS.Location = new Point(6, 0x42);
            this.checkBoxLegendOS.Name = "checkBoxLegendOS";
            this.checkBoxLegendOS.Size = new Size(0x47, 0x11);
            this.checkBoxLegendOS.TabIndex = 2;
            this.checkBoxLegendOS.Text = "Other side";
            this.checkBoxLegendOS.UseVisualStyleBackColor = true;
            this.checkBoxLegendOS.CheckedChanged += new EventHandler(this.checkBoxLegendOS_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x124, 0x10a);
            base.Controls.Add(this.tabControl1);
            base.Name = "DataTableEditor";
            this.Text = "DataTableEditor";
            this.tabControl1.ResumeLayout(false);
            this.tabPageFormat.ResumeLayout(false);
            this.tabPageFormat.PerformLayout();
            this.groupBoxPosition.ResumeLayout(false);
            this.groupBoxPosition.PerformLayout();
            this.numericUpDownTop.EndInit();
            this.numericUpDownLeft.EndInit();
            this.tabPageLegend.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void numericUpDownLeft_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.Left = (int) this.numericUpDownLeft.Value;
            }
        }

        private void numericUpDownTop_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.Top = (int) this.numericUpDownTop.Value;
            }
        }
    }
}

