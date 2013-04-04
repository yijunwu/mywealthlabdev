namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SeriesRegionToolEditor : ToolSeriesEditor
    {
        private Button buttonBrush;
        private Button buttonGradient;
        private ButtonPen buttonPen;
        private CheckBox checkBox1;
        private CheckBox checkBoxAutoBounds;
        private CheckBox checkBoxUseOrigin;
        private IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private NumericUpDown numericUpDownTransparency;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox textBoxLB;
        private TextBox textBoxOrigin;
        private TextBox textBoxUB;
        private SeriesRegionTool tool;

        public SeriesRegionToolEditor()
        {
            this.InitializeComponent();
        }

        public SeriesRegionToolEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            base.setting = true;
            this.tool = (SeriesRegionTool) t;
            base.SetTool(this.tool, null);
            if (this.tool != null)
            {
                this.checkBoxUseOrigin.Checked = this.tool.UseOrigin;
                this.textBoxOrigin.Text = this.tool.Origin.ToString();
                this.checkBoxAutoBounds.Checked = this.tool.AutoBound;
                this.textBoxLB.Text = this.tool.LowerBound.ToString();
                this.textBoxUB.Text = this.tool.UpperBound.ToString();
                this.checkBox1.Checked = this.tool.DrawBehindSeries;
                this.buttonPen.Pen = this.tool.Pen;
                this.numericUpDownTransparency.Value = this.tool.Transparency;
            }
            base.setting = false;
            EditorUtils.Translate(this);
        }

        private void buttonBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.tool.Brush, true);
        }

        private void buttonGradient_Click(object sender, EventArgs e)
        {
            GradientEditor.Edit(this.tool.Gradient);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.DrawBehindSeries = this.checkBox1.Checked;
            }
        }

        private void checkBoxAutoBounds_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.AutoBound = this.checkBoxAutoBounds.Checked;
            }
            this.textBoxLB.Enabled = !this.checkBoxAutoBounds.Checked;
            this.textBoxUB.Enabled = !this.checkBoxAutoBounds.Checked;
        }

        private void checkBoxUseOrigin_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.UseOrigin = this.checkBoxUseOrigin.Checked;
            }
            this.textBoxOrigin.Enabled = this.checkBoxUseOrigin.Checked;
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
            this.groupBox2 = new GroupBox();
            this.textBoxUB = new TextBox();
            this.label4 = new Label();
            this.textBoxLB = new TextBox();
            this.label3 = new Label();
            this.checkBoxAutoBounds = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.textBoxOrigin = new TextBox();
            this.label2 = new Label();
            this.checkBoxUseOrigin = new CheckBox();
            this.tabPage2 = new TabPage();
            this.numericUpDownTransparency = new NumericUpDown();
            this.label5 = new Label();
            this.buttonBrush = new Button();
            this.buttonGradient = new Button();
            this.buttonPen = new ButtonPen();
            this.checkBox1 = new CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.numericUpDownTransparency.BeginInit();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new Point(3, 0x36);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0xe0, 0xa4);
            this.tabControl1.TabIndex = 5;
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(0xd8, 0x8a);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Options";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.groupBox2.Controls.Add(this.textBoxUB);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxLB);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.checkBoxAutoBounds);
            this.groupBox2.Location = new Point(5, 0x3a);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xce, 0x45);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bounds";
            this.textBoxUB.Location = new Point(0x91, 0x2e);
            this.textBoxUB.Name = "textBoxUB";
            this.textBoxUB.Size = new Size(0x30, 20);
            this.textBoxUB.TabIndex = 5;
            this.textBoxUB.Text = "0";
            this.textBoxUB.TextChanged += new EventHandler(this.textBoxUB_TextChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x5c, 0x2e);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x24, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Upper";
            this.textBoxLB.Location = new Point(0x91, 20);
            this.textBoxLB.Name = "textBoxLB";
            this.textBoxLB.Size = new Size(0x30, 20);
            this.textBoxLB.TabIndex = 3;
            this.textBoxLB.Text = "0";
            this.textBoxLB.TextChanged += new EventHandler(this.textBoxLB_TextChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x5c, 0x17);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x24, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Lower";
            this.checkBoxAutoBounds.AutoSize = true;
            this.checkBoxAutoBounds.Location = new Point(6, 0x13);
            this.checkBoxAutoBounds.Name = "checkBoxAutoBounds";
            this.checkBoxAutoBounds.Size = new Size(0x49, 0x11);
            this.checkBoxAutoBounds.TabIndex = 0;
            this.checkBoxAutoBounds.Text = "Automatic";
            this.checkBoxAutoBounds.UseVisualStyleBackColor = true;
            this.checkBoxAutoBounds.CheckedChanged += new EventHandler(this.checkBoxAutoBounds_CheckedChanged);
            this.groupBox1.Controls.Add(this.textBoxOrigin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkBoxUseOrigin);
            this.groupBox1.Location = new Point(4, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xce, 0x2e);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Origin";
            this.textBoxOrigin.Location = new Point(0x91, 0x12);
            this.textBoxOrigin.Name = "textBoxOrigin";
            this.textBoxOrigin.Size = new Size(0x37, 20);
            this.textBoxOrigin.TabIndex = 2;
            this.textBoxOrigin.Text = "0";
            this.textBoxOrigin.TextChanged += new EventHandler(this.textBoxOrigin_TextChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x5e, 0x15);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x22, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Origin";
            this.checkBoxUseOrigin.AutoSize = true;
            this.checkBoxUseOrigin.FlatStyle = FlatStyle.Flat;
            this.checkBoxUseOrigin.Location = new Point(6, 0x13);
            this.checkBoxUseOrigin.Name = "checkBoxUseOrigin";
            this.checkBoxUseOrigin.Size = new Size(70, 0x11);
            this.checkBoxUseOrigin.TabIndex = 0;
            this.checkBoxUseOrigin.Text = "Use origin";
            this.checkBoxUseOrigin.UseVisualStyleBackColor = true;
            this.checkBoxUseOrigin.CheckedChanged += new EventHandler(this.checkBoxUseOrigin_CheckedChanged);
            this.tabPage2.Controls.Add(this.numericUpDownTransparency);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.buttonBrush);
            this.tabPage2.Controls.Add(this.buttonGradient);
            this.tabPage2.Controls.Add(this.buttonPen);
            this.tabPage2.Controls.Add(this.checkBox1);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(0xd8, 0x8a);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Format";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.numericUpDownTransparency.Location = new Point(0x57, 0x6d);
            this.numericUpDownTransparency.Name = "numericUpDownTransparency";
            this.numericUpDownTransparency.Size = new Size(0x4b, 20);
            this.numericUpDownTransparency.TabIndex = 11;
            this.numericUpDownTransparency.ValueChanged += new EventHandler(this.numericUpDownTransparency_ValueChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(6, 0x6f);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x48, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Transparency";
            this.buttonBrush.FlatStyle = FlatStyle.Flat;
            this.buttonBrush.Location = new Point(0x57, 0x2a);
            this.buttonBrush.Name = "buttonBrush";
            this.buttonBrush.Size = new Size(0x4b, 0x17);
            this.buttonBrush.TabIndex = 9;
            this.buttonBrush.Text = "Pattern";
            this.buttonBrush.UseVisualStyleBackColor = true;
            this.buttonBrush.Click += new EventHandler(this.buttonBrush_Click);
            this.buttonGradient.FlatStyle = FlatStyle.Flat;
            this.buttonGradient.Location = new Point(6, 0x47);
            this.buttonGradient.Name = "buttonGradient";
            this.buttonGradient.Size = new Size(0x4b, 0x17);
            this.buttonGradient.TabIndex = 8;
            this.buttonGradient.Text = "Gradient";
            this.buttonGradient.UseVisualStyleBackColor = true;
            this.buttonGradient.Click += new EventHandler(this.buttonGradient_Click);
            this.buttonPen.FlatStyle = FlatStyle.Flat;
            this.buttonPen.Location = new Point(6, 0x2a);
            this.buttonPen.Name = "buttonPen";
            this.buttonPen.Size = new Size(0x48, 0x17);
            this.buttonPen.TabIndex = 7;
            this.buttonPen.Text = "Border";
            this.buttonPen.UseVisualStyleBackColor = true;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new Point(6, 0x13);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x56, 0x11);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Draw behind";
            this.checkBox1.UseVisualStyleBackColor = true;
            base.ClientSize = new Size(0xef, 0xdb);
            base.Controls.Add(this.tabControl1);
            base.Name = "SeriesRegionToolEditor";
            base.Controls.SetChildIndex(base.CBSeries, 0);
            base.Controls.SetChildIndex(this.tabControl1, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.numericUpDownTransparency.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void numericUpDownTransparency_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Transparency = (int) this.numericUpDownTransparency.Value;
            }
        }

        private void textBoxLB_TextChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.LowerBound = Utils.StringToDouble(this.textBoxLB.Text, 0.0);
            }
        }

        private void textBoxOrigin_TextChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Origin = Utils.StringToDouble(this.textBoxOrigin.Text, 0.0);
            }
        }

        private void textBoxUB_TextChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.UpperBound = Utils.StringToDouble(this.textBoxUB.Text, 0.0);
            }
        }
    }
}

