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

    public class SurfaceNearestToolEditor : ToolSeriesEditor
    {
        private ButtonColor buttonColor1;
        private ButtonColor buttonColor2;
        private ButtonColor buttonColor3;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        private IContainer components;
        private SurfaceNearestTool tool;

        public SurfaceNearestToolEditor()
        {
            this.InitializeComponent();
        }

        public SurfaceNearestToolEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.tool = (SurfaceNearestTool) t;
            base.SetTool(this.tool, typeof(Surface));
            this.buttonColor1.Color = this.tool.CellColor;
            this.buttonColor2.Color = this.tool.RowColor;
            this.buttonColor3.Color = this.tool.ColumnColor;
            this.checkBox1.Checked = this.tool.CellColor == Color.Red;
            this.checkBox2.Checked = this.tool.RowColor == Color.Blue;
            this.checkBox3.Checked = this.tool.ColumnColor == Color.Green;
            this.checkBox4.Checked = this.tool.CellColor == Color.Transparent;
            this.checkBox5.Checked = this.tool.RowColor == Color.Transparent;
            this.checkBox6.Checked = this.tool.ColumnColor == Color.Transparent;
            EditorUtils.Translate(this);
        }

        private void buttonColor1_Click(object sender, EventArgs e)
        {
            this.tool.CellColor = this.buttonColor1.Color;
            this.checkBox1.Checked = false;
        }

        private void buttonColor2_Click(object sender, EventArgs e)
        {
            this.tool.RowColor = this.buttonColor2.Color;
            this.checkBox2.Checked = false;
        }

        private void buttonColor3_Click(object sender, EventArgs e)
        {
            this.tool.ColumnColor = this.buttonColor3.Color;
            this.checkBox3.Checked = false;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            this.tool.CellColor = Color.Red;
            this.buttonColor1.Enabled = false;
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            this.tool.RowColor = Color.Blue;
            this.buttonColor2.Enabled = false;
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            this.tool.ColumnColor = Color.Green;
            this.buttonColor3.Enabled = false;
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            this.tool.CellColor = Color.Transparent;
        }

        private void checkBox5_Click(object sender, EventArgs e)
        {
            this.tool.RowColor = Color.Transparent;
        }

        private void checkBox6_Click(object sender, EventArgs e)
        {
            this.tool.ColumnColor = Color.Transparent;
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
            this.buttonColor1 = new ButtonColor();
            this.buttonColor2 = new ButtonColor();
            this.buttonColor3 = new ButtonColor();
            this.checkBox1 = new CheckBox();
            this.checkBox2 = new CheckBox();
            this.checkBox3 = new CheckBox();
            this.checkBox4 = new CheckBox();
            this.checkBox5 = new CheckBox();
            this.checkBox6 = new CheckBox();
            base.SuspendLayout();
            base.CBSeries.Name = "CBSeries";
            this.buttonColor1.Color = Color.Empty;
            this.buttonColor1.Location = new Point(9, 0x2d);
            this.buttonColor1.Name = "buttonColor1";
            this.buttonColor1.TabIndex = 2;
            this.buttonColor1.Text = "&Cell...";
            this.buttonColor1.Click += new EventHandler(this.buttonColor1_Click);
            this.buttonColor2.Color = Color.Empty;
            this.buttonColor2.Location = new Point(10, 0x4e);
            this.buttonColor2.Name = "buttonColor2";
            this.buttonColor2.TabIndex = 3;
            this.buttonColor2.Text = "&Row...";
            this.buttonColor2.Click += new EventHandler(this.buttonColor2_Click);
            this.buttonColor3.Color = Color.Empty;
            this.buttonColor3.Location = new Point(10, 0x6f);
            this.buttonColor3.Name = "buttonColor3";
            this.buttonColor3.TabIndex = 4;
            this.buttonColor3.Text = "C&olumn...";
            this.buttonColor3.Click += new EventHandler(this.buttonColor3_Click);
            this.checkBox1.Location = new Point(0x60, 0x31);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x40, 0x10);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Default";
            this.checkBox1.Click += new EventHandler(this.checkBox1_Click);
            this.checkBox2.Location = new Point(0x60, 0x51);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new Size(0x40, 0x10);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "Default";
            this.checkBox2.Click += new EventHandler(this.checkBox2_Click);
            this.checkBox3.Location = new Point(0x60, 0x73);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new Size(0x40, 0x10);
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Text = "Default";
            this.checkBox3.Click += new EventHandler(this.checkBox3_Click);
            this.checkBox4.Location = new Point(160, 0x31);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new Size(0x40, 0x10);
            this.checkBox4.TabIndex = 8;
            this.checkBox4.Text = "None";
            this.checkBox4.Click += new EventHandler(this.checkBox4_Click);
            this.checkBox5.Location = new Point(160, 0x51);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new Size(0x40, 0x10);
            this.checkBox5.TabIndex = 9;
            this.checkBox5.Text = "None";
            this.checkBox5.Click += new EventHandler(this.checkBox5_Click);
            this.checkBox6.Location = new Point(160, 0x73);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new Size(0x40, 0x10);
            this.checkBox6.TabIndex = 10;
            this.checkBox6.Text = "None";
            this.checkBox6.Click += new EventHandler(this.checkBox6_Click);
            base.ClientSize = new Size(0xe9, 0xac);
            base.Controls.Add(this.checkBox6);
            base.Controls.Add(this.checkBox5);
            base.Controls.Add(this.checkBox4);
            base.Controls.Add(this.checkBox3);
            base.Controls.Add(this.checkBox2);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.buttonColor3);
            base.Controls.Add(this.buttonColor2);
            base.Controls.Add(this.buttonColor1);
            base.Name = "SurfaceNearestToolEditor";
            base.Load += new EventHandler(this.SurfaceNearestToolEditor_Load);
            base.Controls.SetChildIndex(this.buttonColor1, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            base.Controls.SetChildIndex(this.buttonColor2, 0);
            base.Controls.SetChildIndex(this.buttonColor3, 0);
            base.Controls.SetChildIndex(this.checkBox1, 0);
            base.Controls.SetChildIndex(this.checkBox2, 0);
            base.Controls.SetChildIndex(this.checkBox3, 0);
            base.Controls.SetChildIndex(this.checkBox4, 0);
            base.Controls.SetChildIndex(this.checkBox5, 0);
            base.Controls.SetChildIndex(this.checkBox6, 0);
            base.ResumeLayout(false);
        }

        private void SurfaceNearestToolEditor_Load(object sender, EventArgs e)
        {
        }
    }
}

