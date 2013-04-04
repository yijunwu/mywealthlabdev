namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SurfaceSeries : BaseSeriesForm
    {
        private Button Button1;
        private Button Button2;
        private Button Button3;
        private Button button4;
        protected CheckBox cbHideCells;
        protected CheckBox CBSmooth;
        private Container components;
        private Grid3DSeries grid3DEditor;
        private GroupBox groupBox1;
        private Label label1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        protected Surface series;
        private NumericUpDown udTransparency;

        public SurfaceSeries()
        {
            this.InitializeComponent();
        }

        public SurfaceSeries(Series s) : this()
        {
            this.series = (Surface) s;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.SideBrush);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.Pen);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.SideLines);
        }

        private void cbHideCells_Click(object sender, EventArgs e)
        {
            this.series.HideCells = this.cbHideCells.Checked;
        }

        private void CBSmooth_CheckedChanged(object sender, EventArgs e)
        {
            this.series.SmoothPalette = this.CBSmooth.Checked;
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
            this.Button2 = new Button();
            this.Button3 = new Button();
            this.CBSmooth = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.Button1 = new Button();
            this.udTransparency = new NumericUpDown();
            this.label1 = new Label();
            this.cbHideCells = new CheckBox();
            this.button4 = new Button();
            this.groupBox1.SuspendLayout();
            this.udTransparency.BeginInit();
            base.SuspendLayout();
            this.Button2.FlatStyle = FlatStyle.Flat;
            this.Button2.Location = new Point(8, 8);
            this.Button2.Name = "Button2";
            this.Button2.Size = new Size(0x4b, 0x17);
            this.Button2.TabIndex = 0;
            this.Button2.Text = "&Pen...";
            this.Button2.Click += new EventHandler(this.Button2_Click);
            this.Button3.FlatStyle = FlatStyle.Flat;
            this.Button3.Location = new Point(8, 40);
            this.Button3.Name = "Button3";
            this.Button3.Size = new Size(0x4b, 0x17);
            this.Button3.TabIndex = 1;
            this.Button3.Text = "&Brush...";
            this.Button3.Click += new EventHandler(this.Button3_Click);
            this.CBSmooth.FlatStyle = FlatStyle.Flat;
            this.CBSmooth.Location = new Point(8, 0x48);
            this.CBSmooth.Name = "CBSmooth";
            this.CBSmooth.Size = new Size(0x98, 0x10);
            this.CBSmooth.TabIndex = 2;
            this.CBSmooth.Text = "&Smooth palette";
            this.CBSmooth.CheckedChanged += new EventHandler(this.CBSmooth_CheckedChanged);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new Point(0x58, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x10f, 40);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "&Drawing Mode:";
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(0xbc, 0x10);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x49, 0x10);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Dot&Frame";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(0x63, 0x10);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x55, 0x10);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&WireFrame";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.Checked = true;
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(6, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x5f, 0x10);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "S&olid";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.Button1.FlatStyle = FlatStyle.Flat;
            this.Button1.Location = new Point(0xb3, 0x3d);
            this.Button1.Name = "Button1";
            this.Button1.Size = new Size(120, 0x17);
            this.Button1.TabIndex = 4;
            this.Button1.Text = "S&ide Brush...";
            this.Button1.Click += new EventHandler(this.Button1_Click);
            this.udTransparency.Location = new Point(0x56, 0x5f);
            this.udTransparency.Name = "udTransparency";
            this.udTransparency.Size = new Size(50, 20);
            this.udTransparency.TabIndex = 5;
            this.udTransparency.ValueChanged += new EventHandler(this.udTransparency_ValueChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(5, 0x61);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4b, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Transparency:";
            this.cbHideCells.FlatStyle = FlatStyle.Flat;
            this.cbHideCells.Location = new Point(8, 0x79);
            this.cbHideCells.Name = "cbHideCells";
            this.cbHideCells.Size = new Size(0x98, 0x10);
            this.cbHideCells.TabIndex = 7;
            this.cbHideCells.Text = "&Hide Cells";
            this.cbHideCells.Click += new EventHandler(this.cbHideCells_Click);
            this.button4.FlatStyle = FlatStyle.Flat;
            this.button4.Location = new Point(0xb3, 0x5f);
            this.button4.Name = "button4";
            this.button4.Size = new Size(120, 0x17);
            this.button4.TabIndex = 8;
            this.button4.Text = "Side &Line...";
            this.button4.Click += new EventHandler(this.button4_Click);
            base.ClientSize = new Size(0x16b, 0x8f);
            base.Controls.Add(this.button4);
            base.Controls.Add(this.cbHideCells);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.udTransparency);
            base.Controls.Add(this.Button1);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.CBSmooth);
            base.Controls.Add(this.Button3);
            base.Controls.Add(this.Button2);
            base.Name = "SurfaceSeries";
            this.groupBox1.ResumeLayout(false);
            this.udTransparency.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.series.WireFrame = false;
            this.series.DotFrame = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.series.WireFrame = true;
            this.series.DotFrame = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.series.WireFrame = false;
            this.series.DotFrame = true;
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.series != null)
            {
                this.CBSmooth.Checked = this.series.SmoothPalette;
                this.cbHideCells.Checked = this.series.HideCells;
                this.udTransparency.Value = this.series.Brush.Transparency;
                if (this.series.WireFrame)
                {
                    this.radioButton2.Checked = true;
                }
                else if (this.series.DotFrame)
                {
                    this.radioButton3.Checked = true;
                }
                else
                {
                    this.radioButton1.Checked = true;
                }
                if (this.grid3DEditor == null)
                {
                    this.grid3DEditor = new Grid3DSeries(this.series, Parent);
                }
            }
        }

        private void udTransparency_ValueChanged(object sender, EventArgs e)
        {
            if (this.series.Brush.Transparency != Convert.ToInt32(this.udTransparency.Value))
            {
                this.series.Brush.Transparency = Convert.ToInt32(this.udTransparency.Value);
            }
        }
    }
}

