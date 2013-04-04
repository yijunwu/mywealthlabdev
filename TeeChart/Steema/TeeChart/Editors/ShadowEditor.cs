namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ShadowEditor : Form
    {
        private ButtonColor button1;
        private Button button2;
        private CheckBox cbSmooth;
        private CheckBox checkBox1;
        private Container components;
        private GroupBox groupBox1;
        private float H;
        private float L;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private float S;
        private Shadow shadow;
        private TrackBar tbBlur;
        private TrackBar tbColor;
        private NumericUpDown udTransp;

        public ShadowEditor()
        {
            this.InitializeComponent();
        }

        public ShadowEditor(Shadow s) : this()
        {
            this.UpdateDialog(s);
        }

        public ShadowEditor(Shadow s, Control parent) : this()
        {
            this.UpdateDialog(s);
            EditorUtils.InsertForm(this, parent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.shadow.Color = this.button1.Color;
            Steema.TeeChart.Drawing.Filter.ColorToHLS(this.shadow.Color, out this.H, out this.L, out this.S);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.shadow.Brush);
            this.udTransp.Value = this.shadow.Brush.Transparency;
        }

        private void cbSmooth_Click(object sender, EventArgs e)
        {
            this.shadow.Smooth = this.cbSmooth.Checked;
            this.tbBlur.Enabled = this.shadow.Smooth;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.shadow.Visible = this.checkBox1.Checked;
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
            this.numericUpDown2 = new NumericUpDown();
            this.label1 = new Label();
            this.label2 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.button1 = new ButtonColor();
            this.groupBox1 = new GroupBox();
            this.checkBox1 = new CheckBox();
            this.button2 = new Button();
            this.label3 = new Label();
            this.udTransp = new NumericUpDown();
            this.tbColor = new TrackBar();
            this.cbSmooth = new CheckBox();
            this.tbBlur = new TrackBar();
            this.label4 = new Label();
            this.numericUpDown2.BeginInit();
            this.numericUpDown1.BeginInit();
            this.groupBox1.SuspendLayout();
            this.udTransp.BeginInit();
            this.tbColor.BeginInit();
            this.tbBlur.BeginInit();
            base.SuspendLayout();
            this.numericUpDown2.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown2.Location = new Point(0x43, 0x2b);
            int[] bits = new int[4];
            bits[0] = 100;
            bits[3] = -2147483648;
            this.numericUpDown2.Minimum = new decimal(bits);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x23, 20);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown2.TextChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 0x13);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Horizontal:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x15, 0x2d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2d, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Vertical:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(0x43, 0x11);
            int[] numArray2 = new int[4];
            numArray2[0] = 100;
            numArray2[3] = -2147483648;
            this.numericUpDown1.Minimum = new decimal(numArray2);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x23, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.button1.Color = Color.Empty;
            this.button1.Location = new Point(3, 0x17);
            this.button1.Name = "button1";
            this.button1.Size = new Size(80, 0x17);
            this.button1.TabIndex = 2;
            this.button1.Text = "&Color...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(0x72, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(120, 70);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Size:";
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(3, 1);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x40, 0x10);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "&Visible";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(3, 0x67);
            this.button2.Name = "button2";
            this.button2.Size = new Size(80, 0x17);
            this.button2.TabIndex = 3;
            this.button2.Text = "&Pattern...";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x6f, 0x58);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4b, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Transparency:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.udTransp.BorderStyle = BorderStyle.FixedSingle;
            this.udTransp.Location = new Point(0xc2, 0x56);
            this.udTransp.Name = "udTransp";
            this.udTransp.Size = new Size(40, 20);
            this.udTransp.TabIndex = 5;
            this.udTransp.TextAlign = HorizontalAlignment.Right;
            this.udTransp.ValueChanged += new EventHandler(this.ndTransp_ValueChanged);
            this.udTransp.TextChanged += new EventHandler(this.ndTransp_ValueChanged);
            this.tbColor.LargeChange = 10;
            this.tbColor.Location = new Point(3, 0x34);
            this.tbColor.Maximum = 100;
            this.tbColor.Name = "tbColor";
            this.tbColor.Size = new Size(0x69, 0x2d);
            this.tbColor.TabIndex = 6;
            this.tbColor.TickFrequency = 10;
            this.tbColor.Scroll += new EventHandler(this.tbColor_Scroll);
            this.cbSmooth.Location = new Point(3, 0x8f);
            this.cbSmooth.Name = "cbSmooth";
            this.cbSmooth.Size = new Size(0x3e, 0x11);
            this.cbSmooth.TabIndex = 7;
            this.cbSmooth.Text = "&Smooth";
            this.cbSmooth.AutoSize = true;
            this.cbSmooth.UseVisualStyleBackColor = true;
            this.cbSmooth.Click += new EventHandler(this.cbSmooth_Click);
            this.tbBlur.Location = new Point(100, 0x86);
            this.tbBlur.Maximum = 0x4b;
            this.tbBlur.Minimum = -75;
            this.tbBlur.Name = "tbBlur";
            this.tbBlur.Size = new Size(0x86, 0x2d);
            this.tbBlur.TabIndex = 8;
            this.tbBlur.TickFrequency = 5;
            this.tbBlur.Scroll += new EventHandler(this.tbBlur_Scroll);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x6b, 0x76);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1c, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "&Blur:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            base.ClientSize = new Size(0xf6, 0xb5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.tbBlur);
            base.Controls.Add(this.cbSmooth);
            base.Controls.Add(this.tbColor);
            base.Controls.Add(this.udTransp);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.groupBox1);
            base.Name = "ShadowEditor";
            this.Text = "Shadow Editor";
            this.numericUpDown2.EndInit();
            this.numericUpDown1.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.udTransp.EndInit();
            this.tbColor.EndInit();
            this.tbBlur.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ndTransp_ValueChanged(object sender, EventArgs e)
        {
            this.shadow.Transparency = (int) this.udTransp.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.shadow.Width = (int) this.numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.shadow.Height = (int) this.numericUpDown2.Value;
        }

        internal void RefreshControls(Shadow s)
        {
            this.UpdateDialog(s);
        }

        private void tbBlur_Scroll(object sender, EventArgs e)
        {
            this.shadow.SmoothBlur = this.tbBlur.Value;
        }

        private void tbColor_Scroll(object sender, EventArgs e)
        {
            this.L = (float) (((double) this.tbColor.Value) / 100.0);
            this.shadow.Color = Steema.TeeChart.Drawing.Filter.HLSToColor(this.H, this.L, this.S);
            this.button1.Color = this.shadow.Color;
        }

        private void UpdateDialog(Shadow s)
        {
            this.shadow = s;
            this.numericUpDown1.Value = s.Width;
            this.numericUpDown2.Value = s.Height;
            this.checkBox1.Checked = s.Visible;
            this.udTransp.Value = s.Transparency;
            this.button1.Color = this.shadow.Color;
            this.cbSmooth.Checked = this.shadow.Smooth;
            this.tbBlur.Value = this.shadow.SmoothBlur;
            this.tbBlur.Enabled = this.shadow.Smooth;
            Steema.TeeChart.Drawing.Filter.ColorToHLS(this.shadow.Color, out this.H, out this.L, out this.S);
            this.tbColor.Value = Utils.Round((float) (this.L * 100f));
        }
    }
}

