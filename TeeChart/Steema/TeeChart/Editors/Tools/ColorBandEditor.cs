namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ColorBandEditor : AxisToolEdit
    {
        private ButtonPen BEnd;
        private ButtonPen BStart;
        private Button button1;
        private Button button2;
        private ButtonColor button3;
        private CheckBox cbBehind;
        private CheckBox CBEnd;
        private CheckBox CBStart;
        private IContainer components;
        private TextBox eEnd;
        private TextBox eStart;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown ndTransp;
        private bool setting;
        private TabControl tabControl1;
        private TabPage tabOptions;
        private TabPage tabValues;
        private ColorBand tool;

        public ColorBandEditor()
        {
            this.InitializeComponent();
        }

        public ColorBandEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.setting = true;
            this.tool = (ColorBand) t;
            base.SetTool(this.tool);
            this.ndTransp.Value = this.tool.Transparency;
            this.eStart.Text = this.tool.Start.ToString();
            this.eEnd.Text = this.tool.End.ToString();
            this.cbBehind.Checked = this.tool.DrawBehind;
            this.button3.Color = this.tool.Color;
            this.BStart.Pen = this.tool.StartLinePen;
            this.BEnd.Pen = this.tool.EndLinePen;
            this.CBStart.Checked = this.tool.ResizeStart;
            this.CBEnd.Checked = this.tool.ResizeEnd;
            this.setting = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.tool.Brush, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GradientEditor.Edit(this.tool.Gradient);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.tool.Color = this.button3.Color;
        }

        private void cbBehind_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.DrawBehind = this.cbBehind.Checked;
            }
        }

        private void CBEnd_Click(object sender, EventArgs e)
        {
            this.tool.ResizeEnd = this.CBEnd.Checked;
        }

        private void CBStart_Click(object sender, EventArgs e)
        {
            this.tool.ResizeStart = this.CBStart.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void eEnd_TextChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.End = Utils.StringToDouble(this.eEnd.Text, 0.0);
            }
        }

        private void eStart_TextChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.Start = Utils.StringToDouble(this.eStart.Text, 0.0);
            }
        }

        private void InitializeComponent()
        {
            this.button1 = new Button();
            this.tabControl1 = new TabControl();
            this.tabValues = new TabPage();
            this.CBEnd = new CheckBox();
            this.CBStart = new CheckBox();
            this.BEnd = new ButtonPen();
            this.BStart = new ButtonPen();
            this.label3 = new Label();
            this.label2 = new Label();
            this.eEnd = new TextBox();
            this.eStart = new TextBox();
            this.tabOptions = new TabPage();
            this.label4 = new Label();
            this.button3 = new ButtonColor();
            this.button2 = new Button();
            this.cbBehind = new CheckBox();
            this.ndTransp = new NumericUpDown();
            this.tabControl1.SuspendLayout();
            this.tabValues.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.ndTransp.BeginInit();
            base.SuspendLayout();
            base.BPen.Name = "BPen";
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x9b, 40);
            this.button1.Name = "button1";
            this.button1.Size = new Size(80, 0x17);
            this.button1.TabIndex = 10;
            this.button1.Text = "&Pattern...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.tabControl1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabControl1.Controls.Add(this.tabValues);
            this.tabControl1.Controls.Add(this.tabOptions);
            this.tabControl1.Location = new Point(0, 0x40);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0xf4, 0x98);
            this.tabControl1.TabIndex = 11;
            this.tabValues.Controls.Add(this.CBEnd);
            this.tabValues.Controls.Add(this.CBStart);
            this.tabValues.Controls.Add(this.BEnd);
            this.tabValues.Controls.Add(this.BStart);
            this.tabValues.Controls.Add(this.label3);
            this.tabValues.Controls.Add(this.label2);
            this.tabValues.Controls.Add(this.eEnd);
            this.tabValues.Controls.Add(this.eStart);
            this.tabValues.Location = new Point(4, 0x16);
            this.tabValues.Name = "tabValues";
            this.tabValues.Size = new Size(0xe8, 0x7e);
            this.tabValues.TabIndex = 0;
            this.tabValues.Text = "Values";
            this.CBEnd.FlatStyle = FlatStyle.Flat;
            this.CBEnd.Location = new Point(0x43, 0x5e);
            this.CBEnd.Name = "CBEnd";
            this.CBEnd.Size = new Size(0x4b, 0x10);
            this.CBEnd.TabIndex = 0x18;
            this.CBEnd.Text = "Allow Drag";
            this.CBEnd.Click += new EventHandler(this.CBEnd_Click);
            this.CBStart.FlatStyle = FlatStyle.Flat;
            this.CBStart.Location = new Point(0x43, 0x2c);
            this.CBStart.Name = "CBStart";
            this.CBStart.Size = new Size(0x4b, 0x10);
            this.CBStart.TabIndex = 0x17;
            this.CBStart.Text = "Allow Drag";
            this.CBStart.Click += new EventHandler(this.CBStart_Click);
            this.BEnd.FlatStyle = FlatStyle.Flat;
            this.BEnd.Location = new Point(0xa3, 70);
            this.BEnd.Name = "BEnd";
            this.BEnd.Size = new Size(0x42, 0x17);
            this.BEnd.TabIndex = 0x16;
            this.BEnd.Text = "Border...";
            this.BStart.FlatStyle = FlatStyle.Flat;
            this.BStart.Location = new Point(160, 0x16);
            this.BStart.Name = "BStart";
            this.BStart.Size = new Size(0x42, 0x17);
            this.BStart.TabIndex = 0x15;
            this.BStart.Text = "Border...";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(1, 0x4a);
            this.label3.Name = "label3";
            this.label3.Size = new Size(60, 0x10);
            this.label3.TabIndex = 0x13;
            this.label3.Text = "&End Value:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(1, 0x18);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x3f, 0x10);
            this.label2.TabIndex = 0x11;
            this.label2.Text = "&Start Value:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.eEnd.BorderStyle = BorderStyle.FixedSingle;
            this.eEnd.Location = new Point(0x43, 0x48);
            this.eEnd.Name = "eEnd";
            this.eEnd.Size = new Size(0x59, 20);
            this.eEnd.TabIndex = 20;
            this.eEnd.Text = "";
            this.eEnd.TextChanged += new EventHandler(this.eEnd_TextChanged);
            this.eStart.BorderStyle = BorderStyle.FixedSingle;
            this.eStart.Location = new Point(0x44, 0x17);
            this.eStart.Name = "eStart";
            this.eStart.Size = new Size(0x58, 20);
            this.eStart.TabIndex = 0x12;
            this.eStart.Text = "";
            this.eStart.TextChanged += new EventHandler(this.eStart_TextChanged);
            this.tabOptions.Controls.Add(this.label4);
            this.tabOptions.Controls.Add(this.button3);
            this.tabOptions.Controls.Add(this.button2);
            this.tabOptions.Controls.Add(this.cbBehind);
            this.tabOptions.Controls.Add(this.ndTransp);
            this.tabOptions.Location = new Point(4, 0x16);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new Size(0xe8, 0x7e);
            this.tabOptions.TabIndex = 1;
            this.tabOptions.Text = "Options";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x10, 80);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x4d, 0x10);
            this.label4.TabIndex = 0x11;
            this.label4.Text = "&Transparency:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.button3.Color = Color.Empty;
            this.button3.Location = new Point(0x10, 0x30);
            this.button3.Name = "button3";
            this.button3.Size = new Size(80, 0x17);
            this.button3.TabIndex = 0x15;
            this.button3.Text = "&Color...";
            this.button3.Click += new EventHandler(this.button3_Click);
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x10, 0x10);
            this.button2.Name = "button2";
            this.button2.Size = new Size(80, 0x17);
            this.button2.TabIndex = 20;
            this.button2.Text = "&Gradient...";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.cbBehind.FlatStyle = FlatStyle.Flat;
            this.cbBehind.Location = new Point(0x70, 0x10);
            this.cbBehind.Name = "cbBehind";
            this.cbBehind.TabIndex = 0x13;
            this.cbBehind.Text = "Draw &Behind";
            this.cbBehind.CheckedChanged += new EventHandler(this.cbBehind_CheckedChanged);
            this.ndTransp.BorderStyle = BorderStyle.FixedSingle;
            this.ndTransp.Location = new Point(0x61, 0x4e);
            this.ndTransp.Name = "ndTransp";
            this.ndTransp.Size = new Size(0x38, 20);
            this.ndTransp.TabIndex = 0x12;
            this.ndTransp.TextAlign = HorizontalAlignment.Right;
            this.ndTransp.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.ndTransp.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            base.ClientSize = new Size(0xf4, 0xd5);
            base.Controls.Add(this.tabControl1);
            base.Controls.Add(this.button1);
            base.Name = "ColorBandEditor";
            base.Controls.SetChildIndex(base.BPen, 0);
            base.Controls.SetChildIndex(this.button1, 0);
            base.Controls.SetChildIndex(this.tabControl1, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabValues.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.ndTransp.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.Transparency = (int) this.ndTransp.Value;
            }
        }
    }
}

