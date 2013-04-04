namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AreaSeries : BaseSeriesForm
    {
        private Area area;
        private ButtonColor BAreaColor;
        private ButtonPen BAreaLinePen;
        private ButtonPen BAreaLinesPen;
        private Button button1;
        private Button button2;
        private CheckBox CBColorEach;
        private CheckBox CBInvStairs;
        private CheckBox CBStairs;
        private CheckBox CBUseOrigin;
        private ComboBox comboBoxTreatNulls;
        private Container components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private Label label2;
        private Steema.TeeChart.Editors.SeriesPointer pointerEditor;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private bool setting;
        private TabControl tabControl1;
        private TabPage tabPageOptions;
        private TabPage tabPageStack;
        private NumericUpDown UDOrigin;
        private NumericUpDown UDStackGroup;

        public AreaSeries()
        {
            this.InitializeComponent();
        }

        public AreaSeries(Series s) : this()
        {
            this.area = (Area) s;
            this.setting = true;
            this.CBStairs.Checked = this.area.Stairs;
            this.CBInvStairs.Checked = this.area.InvertedStairs;
            this.CBInvStairs.Enabled = this.CBStairs.Checked;
            this.CBColorEach.Checked = this.area.ColorEach;
            this.CBUseOrigin.Checked = this.area.UseOrigin;
            this.UDOrigin.Value = Convert.ToDecimal(this.area.Origin);
            this.UDOrigin.Enabled = this.area.UseOrigin;
            this.BAreaColor.Color = this.area.Color;
            this.BAreaColor.Click += new EventHandler(this.BAreaColor_Click);
            this.BAreaLinePen.Pen = this.area.LinePen;
            this.BAreaLinesPen.Pen = this.area.AreaLines;
            this.groupBox2.Enabled = this.area.MultiArea != MultiAreas.None;
            this.comboBoxTreatNulls.SelectedIndex = (int) this.area.TreatNulls;
            if (this.area.MultiArea == MultiAreas.None)
            {
                this.radioButton1.Checked = true;
            }
            else if (this.area.MultiArea == MultiAreas.Stacked)
            {
                this.radioButton2.Checked = true;
            }
            else if (this.area.MultiArea == MultiAreas.Stacked100)
            {
                this.radioButton3.Checked = true;
            }
            this.setting = false;
        }

        private void BAreaColor_Click(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.area.Color = this.BAreaColor.Color;
            }
            this.CBColorEach.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.area.AreaBrush);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.area.Brush);
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.area.ColorEach = this.CBColorEach.Checked;
            }
        }

        private void CBInvStairs_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.area.InvertedStairs = this.CBInvStairs.Checked;
            }
        }

        private void CBStairs_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.area.Stairs = this.CBStairs.Checked;
            }
            this.CBInvStairs.Enabled = this.CBStairs.Checked;
        }

        private void CBUseOrigin_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.area.UseOrigin = this.CBUseOrigin.Checked;
            }
            this.UDOrigin.Enabled = this.area.UseOrigin;
        }

        private void comboBoxTreatNulls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.area.TreatNulls = (TreatNullsStyle) this.comboBoxTreatNulls.SelectedIndex;
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
            this.tabPageOptions = new TabPage();
            this.groupBox4 = new GroupBox();
            this.BAreaColor = new ButtonColor();
            this.CBColorEach = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.button2 = new Button();
            this.button1 = new Button();
            this.CBInvStairs = new CheckBox();
            this.CBStairs = new CheckBox();
            this.BAreaLinesPen = new ButtonPen();
            this.BAreaLinePen = new ButtonPen();
            this.tabPageStack = new TabPage();
            this.groupBox2 = new GroupBox();
            this.UDStackGroup = new NumericUpDown();
            this.groupBox3 = new GroupBox();
            this.radioButton3 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.groupBox5 = new GroupBox();
            this.UDOrigin = new NumericUpDown();
            this.label2 = new Label();
            this.CBUseOrigin = new CheckBox();
            this.groupBox6 = new GroupBox();
            this.comboBoxTreatNulls = new ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPageOptions.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageStack.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.UDStackGroup.BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.UDOrigin.BeginInit();
            this.groupBox6.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabPageOptions);
            this.tabControl1.Controls.Add(this.tabPageStack);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x145, 0xd1);
            this.tabControl1.TabIndex = 4;
            this.tabPageOptions.Controls.Add(this.groupBox6);
            this.tabPageOptions.Controls.Add(this.groupBox4);
            this.tabPageOptions.Controls.Add(this.groupBox1);
            this.tabPageOptions.Location = new Point(4, 0x16);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Padding = new Padding(3);
            this.tabPageOptions.Size = new Size(0x13d, 0xb7);
            this.tabPageOptions.TabIndex = 0;
            this.tabPageOptions.Text = "Options";
            this.tabPageOptions.UseVisualStyleBackColor = true;
            this.groupBox4.Controls.Add(this.BAreaColor);
            this.groupBox4.Controls.Add(this.CBColorEach);
            this.groupBox4.Location = new Point(6, 0x5f);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(0x79, 0x49);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Color:";
            this.BAreaColor.Color = Color.Empty;
            this.BAreaColor.Location = new Point(14, 40);
            this.BAreaColor.Name = "BAreaColor";
            this.BAreaColor.Size = new Size(0x4b, 0x17);
            this.BAreaColor.TabIndex = 1;
            this.BAreaColor.Text = "&Color...";
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(14, 0x10);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x62, 0x10);
            this.CBColorEach.TabIndex = 0;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.CBInvStairs);
            this.groupBox1.Controls.Add(this.CBStairs);
            this.groupBox1.Controls.Add(this.BAreaLinesPen);
            this.groupBox1.Controls.Add(this.BAreaLinePen);
            this.groupBox1.Location = new Point(6, 0x10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x129, 0x49);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "0";
            this.groupBox1.Text = "Area:";
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x70, 12);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x4b, 0x17);
            this.button2.TabIndex = 2;
            this.button2.Text = "&Pattern...";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x70, 40);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 4;
            this.button1.Text = "P&attern...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.CBInvStairs.FlatStyle = FlatStyle.Flat;
            this.CBInvStairs.Location = new Point(12, 0x2c);
            this.CBInvStairs.Name = "CBInvStairs";
            this.CBInvStairs.Size = new Size(0x5c, 0x10);
            this.CBInvStairs.TabIndex = 1;
            this.CBInvStairs.Text = "&Inverted";
            this.CBInvStairs.CheckedChanged += new EventHandler(this.CBInvStairs_CheckedChanged);
            this.CBStairs.FlatStyle = FlatStyle.Flat;
            this.CBStairs.Location = new Point(12, 0x15);
            this.CBStairs.Name = "CBStairs";
            this.CBStairs.Size = new Size(0x5c, 0x10);
            this.CBStairs.TabIndex = 0;
            this.CBStairs.Text = "&Stairs";
            this.CBStairs.CheckedChanged += new EventHandler(this.CBStairs_CheckedChanged);
            this.BAreaLinesPen.FlatStyle = FlatStyle.Flat;
            this.BAreaLinesPen.Location = new Point(0xc4, 40);
            this.BAreaLinesPen.Name = "BAreaLinesPen";
            this.BAreaLinesPen.Size = new Size(0x59, 0x17);
            this.BAreaLinesPen.TabIndex = 5;
            this.BAreaLinesPen.Text = "Area &Lines...";
            this.BAreaLinePen.FlatStyle = FlatStyle.Flat;
            this.BAreaLinePen.Location = new Point(0xc4, 12);
            this.BAreaLinePen.Name = "BAreaLinePen";
            this.BAreaLinePen.Size = new Size(0x59, 0x17);
            this.BAreaLinePen.TabIndex = 3;
            this.BAreaLinePen.Text = "&Border...";
            this.tabPageStack.Controls.Add(this.groupBox2);
            this.tabPageStack.Controls.Add(this.groupBox3);
            this.tabPageStack.Controls.Add(this.groupBox5);
            this.tabPageStack.Location = new Point(4, 0x16);
            this.tabPageStack.Name = "tabPageStack";
            this.tabPageStack.Padding = new Padding(3);
            this.tabPageStack.Size = new Size(0x150, 0xcc);
            this.tabPageStack.TabIndex = 1;
            this.tabPageStack.Text = "Stack";
            this.tabPageStack.UseVisualStyleBackColor = true;
            this.groupBox2.Controls.Add(this.UDStackGroup);
            this.groupBox2.Location = new Point(0x9b, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x73, 0x49);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stack group";
            this.UDStackGroup.Location = new Point(6, 0x13);
            this.UDStackGroup.Name = "UDStackGroup";
            this.UDStackGroup.Size = new Size(0x41, 20);
            this.UDStackGroup.TabIndex = 0;
            this.UDStackGroup.ValueChanged += new EventHandler(this.USStackGroup_ValueChanged);
            this.groupBox3.Controls.Add(this.radioButton3);
            this.groupBox3.Controls.Add(this.radioButton1);
            this.groupBox3.Controls.Add(this.radioButton2);
            this.groupBox3.Location = new Point(8, 11);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x8d, 0x49);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "&Multiple Areas:";
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(0x18, 0x30);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x60, 0x10);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Stac&k 100%";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(0x18, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x60, 0x10);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "&None";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(0x18, 0x20);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x60, 0x10);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "S&tack";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.groupBox5.Controls.Add(this.UDOrigin);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.CBUseOrigin);
            this.groupBox5.Location = new Point(8, 0x60);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(0x106, 0x27);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.UDOrigin.BorderStyle = BorderStyle.FixedSingle;
            this.UDOrigin.Location = new Point(0xa1, 15);
            int[] bits = new int[4];
            bits[0] = 0x540be400;
            bits[1] = 2;
            this.UDOrigin.Maximum = new decimal(bits);
            this.UDOrigin.Minimum = new decimal(new int[] { 0x540be400, 2, 0, -2147483648 });
            this.UDOrigin.Name = "UDOrigin";
            this.UDOrigin.Size = new Size(80, 20);
            this.UDOrigin.TabIndex = 2;
            this.UDOrigin.TextAlign = HorizontalAlignment.Right;
            this.UDOrigin.ValueChanged += new EventHandler(this.UDOrigin_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x76, 0x12);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x25, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Origin:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.CBUseOrigin.FlatStyle = FlatStyle.Flat;
            this.CBUseOrigin.Location = new Point(8, 15);
            this.CBUseOrigin.Name = "CBUseOrigin";
            this.CBUseOrigin.Size = new Size(0x68, 0x10);
            this.CBUseOrigin.TabIndex = 0;
            this.CBUseOrigin.Text = "&Use Origin";
            this.CBUseOrigin.CheckedChanged += new EventHandler(this.CBUseOrigin_CheckedChanged);
            this.groupBox6.Controls.Add(this.comboBoxTreatNulls);
            this.groupBox6.Location = new Point(0x88, 0x60);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new Size(0xa7, 0x31);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Treat Nulls";
            this.comboBoxTreatNulls.FormattingEnabled = true;
            this.comboBoxTreatNulls.Items.AddRange(new object[] { "Don't paint", "Skip", "Ignore" });
            this.comboBoxTreatNulls.Location = new Point(6, 0x13);
            this.comboBoxTreatNulls.Name = "comboBoxTreatNulls";
            this.comboBoxTreatNulls.Size = new Size(0x79, 0x15);
            this.comboBoxTreatNulls.TabIndex = 0;
            this.comboBoxTreatNulls.SelectedIndexChanged += new EventHandler(this.comboBoxTreatNulls_SelectedIndexChanged);
            base.ClientSize = new Size(0x145, 0xd1);
            base.Controls.Add(this.tabControl1);
            base.Name = "AreaSeries";
            this.tabControl1.ResumeLayout(false);
            this.tabPageOptions.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabPageStack.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.UDStackGroup.EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.UDOrigin.EndInit();
            this.groupBox6.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.area.MultiArea = MultiAreas.None;
            }
            this.groupBox2.Enabled = this.area.MultiArea != MultiAreas.None;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.area.MultiArea = MultiAreas.Stacked;
            }
            this.groupBox2.Enabled = this.area.MultiArea != MultiAreas.None;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.area.MultiArea = MultiAreas.Stacked100;
            }
            this.groupBox2.Enabled = this.area.MultiArea != MultiAreas.None;
        }

        public override void SetParent(TabPage Parent)
        {
            if ((this.area != null) && (this.pointerEditor == null))
            {
                this.pointerEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(Parent, this.area.Pointer);
            }
        }

        private void UDOrigin_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.area.Origin = Convert.ToDouble(this.UDOrigin.Value);
            }
        }

        private void USStackGroup_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.area.StackGroup = (int) this.UDStackGroup.Value;
            }
        }
    }
}

