namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FibonacciToolEditor : ToolSeriesEditor
    {
        private ButtonPen bPen;
        private Button buttonAddLevel;
        private Button buttonDefault;
        private ButtonPen buttonPen1;
        private Button buttonRemoveLevel;
        private ComboBox cbDrawingStyle;
        private CheckBox checkBox1;
        private IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private FibonacciTool.FibonacciItem level;
        private NumericUpDown numericUpDownLevels;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox textBox1;
        private TextBox textBoxEndX;
        private TextBox textBoxEndY;
        private TextBox textBoxStartX;
        private TextBox textBoxStartY;
        private FibonacciTool tool;

        public FibonacciToolEditor()
        {
            this.InitializeComponent();
            this.cbDrawingStyle.Items.Add("Fibonacci arcs");
            this.cbDrawingStyle.Items.Add("Fibonacci fans");
        }

        public FibonacciToolEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            base.setting = true;
            this.tool = (FibonacciTool) t;
            base.SetTool(this.tool, null);
            this.cbDrawingStyle.SelectedIndex = (int) this.tool.DrawStyle;
            this.checkBox1.Checked = this.tool.ShowLabels;
            this.bPen.Pen = this.tool.TrendPen;
            this.textBoxStartX.Text = this.tool.StartX.ToString();
            this.textBoxStartY.Text = this.tool.StartY.ToString();
            this.textBoxEndX.Text = this.tool.EndX.ToString();
            this.textBoxEndY.Text = this.tool.EndY.ToString();
            if (this.tool.Levels.Count > 0)
            {
                this.level = this.tool.Levels[0];
            }
            this.UpdateLevelsGUI();
            base.setting = false;
        }

        private void buttonAddLevel_Click(object sender, EventArgs e)
        {
            this.level = new FibonacciTool.FibonacciItem(this.tool, 50.0);
            this.tool.Levels.Add(this.level);
            this.numericUpDownLevels_ValueChanged(null, EventArgs.Empty);
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            this.tool.CreateDefaultLevels();
            this.numericUpDownLevels_ValueChanged(null, EventArgs.Empty);
        }

        private void buttonRemoveLevel_Click(object sender, EventArgs e)
        {
            if (this.level != null)
            {
                this.tool.Levels.Remove(this.level);
            }
            this.numericUpDownLevels_ValueChanged(null, EventArgs.Empty);
        }

        private void cbButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.DrawStyle = (FibonacciStyle) this.cbDrawingStyle.SelectedIndex;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.tool.ShowLabels = this.checkBox1.Checked;
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
            this.label2 = new Label();
            this.cbDrawingStyle = new ComboBox();
            this.checkBox1 = new CheckBox();
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.groupBox2 = new GroupBox();
            this.textBoxEndY = new TextBox();
            this.textBoxEndX = new TextBox();
            this.label5 = new Label();
            this.label6 = new Label();
            this.groupBox1 = new GroupBox();
            this.textBoxStartY = new TextBox();
            this.textBoxStartX = new TextBox();
            this.label4 = new Label();
            this.label3 = new Label();
            this.bPen = new ButtonPen();
            this.tabPage2 = new TabPage();
            this.groupBox3 = new GroupBox();
            this.textBox1 = new TextBox();
            this.buttonPen1 = new ButtonPen();
            this.numericUpDownLevels = new NumericUpDown();
            this.buttonDefault = new Button();
            this.buttonRemoveLevel = new Button();
            this.buttonAddLevel = new Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.numericUpDownLevels.BeginInit();
            base.SuspendLayout();
            base.CBSeries.Location = new Point(90, 8);
            base.CBSeries.ItemHeight = 13;
            base.label1.Location = new Point(0x34, 12);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(3, 0x29);
            this.label2.Name = "label2";
            this.label2.Size = new Size(80, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Fibonacci style:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.cbDrawingStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbDrawingStyle.Location = new Point(90, 0x26);
            this.cbDrawingStyle.Name = "cbDrawingStyle";
            this.cbDrawingStyle.Size = new Size(0x79, 0x15);
            this.cbDrawingStyle.TabIndex = 4;
            this.cbDrawingStyle.SelectedIndexChanged += new EventHandler(this.cbButton_SelectedIndexChanged);
            this.checkBox1.AutoSize = true;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(0x5b, 0x41);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x4e, 0x11);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Draw labels";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.tabControl1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new Point(3, 0x58);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0xef, 0x87);
            this.tabControl1.TabIndex = 6;
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.bPen);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(0xe7, 0x6d);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Trendline";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.groupBox2.Controls.Add(this.textBoxEndY);
            this.groupBox2.Controls.Add(this.textBoxEndX);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new Point(0x6c, 0x23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x60, 0x41);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "End";
            this.textBoxEndY.Location = new Point(0x22, 0x23);
            this.textBoxEndY.Name = "textBoxEndY";
            this.textBoxEndY.Size = new Size(0x34, 20);
            this.textBoxEndY.TabIndex = 3;
            this.textBoxEndY.TextChanged += new EventHandler(this.textBoxEndY_TextChanged);
            this.textBoxEndX.Location = new Point(0x22, 13);
            this.textBoxEndX.Name = "textBoxEndX";
            this.textBoxEndX.Size = new Size(0x34, 20);
            this.textBoxEndX.TabIndex = 2;
            this.textBoxEndX.TextChanged += new EventHandler(this.textBoxEndX_TextChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(6, 0x26);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x11, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Y:";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(6, 0x10);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x11, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "X:";
            this.groupBox1.Controls.Add(this.textBoxStartY);
            this.groupBox1.Controls.Add(this.textBoxStartX);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new Point(6, 0x23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x60, 0x41);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Start";
            this.textBoxStartY.Location = new Point(0x22, 0x23);
            this.textBoxStartY.Name = "textBoxStartY";
            this.textBoxStartY.Size = new Size(0x34, 20);
            this.textBoxStartY.TabIndex = 3;
            this.textBoxStartY.TextChanged += new EventHandler(this.textBoxStartY_TextChanged);
            this.textBoxStartX.Location = new Point(0x22, 13);
            this.textBoxStartX.Name = "textBoxStartX";
            this.textBoxStartX.Size = new Size(0x34, 20);
            this.textBoxStartX.TabIndex = 2;
            this.textBoxStartX.TextChanged += new EventHandler(this.textBoxStartX_TextChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(6, 0x26);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x11, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Y:";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(6, 0x10);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x11, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "X:";
            this.bPen.FlatStyle = FlatStyle.Flat;
            this.bPen.Location = new Point(6, 6);
            this.bPen.Name = "bPen";
            this.bPen.Size = new Size(0x57, 0x17);
            this.bPen.TabIndex = 3;
            this.bPen.Text = "Trend &Pen...";
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.buttonDefault);
            this.tabPage2.Controls.Add(this.buttonRemoveLevel);
            this.tabPage2.Controls.Add(this.buttonAddLevel);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0xe7, 0x6d);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Levels";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.buttonPen1);
            this.groupBox3.Controls.Add(this.numericUpDownLevels);
            this.groupBox3.Location = new Point(5, 0x29);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0xdf, 50);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Level";
            this.textBox1.Location = new Point(0x38, 0x11);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0x3a, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.buttonPen1.FlatStyle = FlatStyle.Flat;
            this.buttonPen1.Location = new Point(120, 15);
            this.buttonPen1.Name = "buttonPen1";
            this.buttonPen1.Size = new Size(0x4b, 0x17);
            this.buttonPen1.TabIndex = 6;
            this.buttonPen1.Text = "Pen";
            this.buttonPen1.UseVisualStyleBackColor = true;
            this.numericUpDownLevels.Location = new Point(6, 0x12);
            this.numericUpDownLevels.Name = "numericUpDownLevels";
            this.numericUpDownLevels.Size = new Size(0x26, 20);
            this.numericUpDownLevels.TabIndex = 5;
            this.numericUpDownLevels.ValueChanged += new EventHandler(this.numericUpDownLevels_ValueChanged);
            this.buttonDefault.FlatStyle = FlatStyle.Flat;
            this.buttonDefault.Location = new Point(0x3d, 3);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new Size(0x47, 0x17);
            this.buttonDefault.TabIndex = 2;
            this.buttonDefault.Text = "Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new EventHandler(this.buttonDefault_Click);
            this.buttonRemoveLevel.FlatStyle = FlatStyle.Flat;
            this.buttonRemoveLevel.Location = new Point(0x21, 3);
            this.buttonRemoveLevel.Name = "buttonRemoveLevel";
            this.buttonRemoveLevel.Size = new Size(0x16, 0x17);
            this.buttonRemoveLevel.TabIndex = 1;
            this.buttonRemoveLevel.Text = "-";
            this.buttonRemoveLevel.UseVisualStyleBackColor = true;
            this.buttonRemoveLevel.Click += new EventHandler(this.buttonRemoveLevel_Click);
            this.buttonAddLevel.FlatStyle = FlatStyle.Flat;
            this.buttonAddLevel.Location = new Point(5, 3);
            this.buttonAddLevel.Name = "buttonAddLevel";
            this.buttonAddLevel.Size = new Size(0x16, 0x17);
            this.buttonAddLevel.TabIndex = 0;
            this.buttonAddLevel.Text = "+";
            this.buttonAddLevel.UseVisualStyleBackColor = true;
            this.buttonAddLevel.Click += new EventHandler(this.buttonAddLevel_Click);
            base.ClientSize = new Size(0xf3, 0xdf);
            base.Controls.Add(this.tabControl1);
            base.Controls.Add(this.cbDrawingStyle);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.checkBox1);
            base.Name = "FibonacciToolEditor";
            base.Controls.SetChildIndex(this.checkBox1, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.cbDrawingStyle, 0);
            base.Controls.SetChildIndex(this.tabControl1, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.numericUpDownLevels.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void numericUpDownLevels_ValueChanged(object sender, EventArgs e)
        {
            int num = (int) this.numericUpDownLevels.Value;
            if (num < this.tool.Levels.Count)
            {
                this.level = this.tool.Levels[num];
            }
            else
            {
                this.level = null;
            }
            this.UpdateLevelsGUI();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.level != null)
            {
                this.level.Value = Utils.StringToDouble(this.textBox1.Text, 100.0);
            }
        }

        private void textBoxEndX_TextChanged(object sender, EventArgs e)
        {
            this.tool.EndX = Utils.StringToDouble(this.textBoxEndX.Text, 0.0);
        }

        private void textBoxEndY_TextChanged(object sender, EventArgs e)
        {
            this.tool.EndY = Utils.StringToDouble(this.textBoxEndY.Text, 0.0);
        }

        private void textBoxStartX_TextChanged(object sender, EventArgs e)
        {
            this.tool.StartX = Utils.StringToDouble(this.textBoxStartX.Text, 0.0);
        }

        private void textBoxStartY_TextChanged(object sender, EventArgs e)
        {
            this.tool.StartY = Utils.StringToDouble(this.textBoxStartY.Text, 0.0);
        }

        private void UpdateLevelsGUI()
        {
            this.buttonRemoveLevel.Enabled = this.tool.Levels.Count > 1;
            this.numericUpDownLevels.Maximum = this.tool.Levels.Count - 1;
            this.textBox1.Enabled = this.level != null;
            this.buttonPen1.Enabled = this.level != null;
            if (this.level != null)
            {
                this.textBox1.Text = this.level.Value.ToString();
                this.buttonPen1.Pen = this.level.Pen;
            }
            else
            {
                this.textBox1.Text = "";
            }
        }
    }
}

