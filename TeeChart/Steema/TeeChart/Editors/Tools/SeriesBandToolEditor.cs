namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class SeriesBandToolEditor : ToolSeriesEditor
    {
        private Button bBrush;
        private Button bPen;
        private CheckBox cbDrawBehindSeries;
        private ComboBox cbSeries2;
        private Label label2;
        private Label label3;
        private SeriesBandTool tool;
        private NumericUpDown udTransp;

        public SeriesBandToolEditor()
        {
            this.InitializeComponent();
        }

        public SeriesBandToolEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            base.setting = true;
            this.tool = (SeriesBandTool) t;
            base.SetTool(this.tool, null);
            if (this.tool != null)
            {
                base.FillSeriesCombo(this.cbSeries2, this.tool.Series2, this.tool.Chart);
                this.cbDrawBehindSeries.Checked = this.tool.DrawBehindSeries;
                this.udTransp.Value = this.tool.Transparency;
            }
            base.setting = false;
            EditorUtils.Translate(this);
        }

        private void bBrush_Click(object sender, EventArgs e)
        {
            if ((this.tool != null) && !base.setting)
            {
                BrushEditor.Edit(this.tool.Brush);
            }
        }

        private void bPen_Click(object sender, EventArgs e)
        {
            if ((this.tool != null) && !base.setting)
            {
                PenEditor.Edit(this.tool.Pen);
            }
        }

        private void cbDrawBehindSeries_Click(object sender, EventArgs e)
        {
            if ((this.tool != null) && !base.setting)
            {
                this.tool.DrawBehindSeries = this.cbDrawBehindSeries.Checked;
            }
        }

        private void cbSeries2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.tool != null) && !base.setting)
            {
                if ((this.cbSeries2.SelectedIndex == -1) || (this.cbSeries2.SelectedIndex == 0))
                {
                    this.tool.Series2 = null;
                }
                else
                {
                    this.tool.Series2 = this.tool.chart.series[this.cbSeries2.SelectedIndex - 1];
                }
            }
        }

        private void InitializeComponent()
        {
            this.cbSeries2 = new ComboBox();
            this.label2 = new Label();
            this.bPen = new Button();
            this.bBrush = new Button();
            this.cbDrawBehindSeries = new CheckBox();
            this.udTransp = new NumericUpDown();
            this.label3 = new Label();
            this.udTransp.BeginInit();
            base.SuspendLayout();
            this.cbSeries2.FormattingEnabled = true;
            this.cbSeries2.Location = new Point(0x52, 0x23);
            this.cbSeries2.Name = "cbSeries2";
            this.cbSeries2.Size = new Size(0x88, 0x15);
            this.cbSeries2.TabIndex = 2;
            this.cbSeries2.SelectedIndexChanged += new EventHandler(this.cbSeries2_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(30, 0x26);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2d, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "S&eries2:";
            this.bPen.FlatStyle = FlatStyle.Flat;
            this.bPen.Location = new Point(0x29, 0x3e);
            this.bPen.Name = "bPen";
            this.bPen.Size = new Size(0x4b, 0x17);
            this.bPen.TabIndex = 4;
            this.bPen.Text = "&Pen...";
            this.bPen.UseVisualStyleBackColor = true;
            this.bPen.Click += new EventHandler(this.bPen_Click);
            this.bBrush.FlatStyle = FlatStyle.Flat;
            this.bBrush.Location = new Point(0x7a, 0x3e);
            this.bBrush.Name = "bBrush";
            this.bBrush.Size = new Size(0x4b, 0x17);
            this.bBrush.TabIndex = 5;
            this.bBrush.Text = "&Brush...";
            this.bBrush.UseVisualStyleBackColor = true;
            this.bBrush.Click += new EventHandler(this.bBrush_Click);
            this.cbDrawBehindSeries.AutoSize = true;
            this.cbDrawBehindSeries.UseVisualStyleBackColor = true;
            this.cbDrawBehindSeries.Location = new Point(0x52, 0x5b);
            this.cbDrawBehindSeries.Name = "cbDrawBehindSeries";
            this.cbDrawBehindSeries.Size = new Size(0x77, 0x11);
            this.cbDrawBehindSeries.TabIndex = 6;
            this.cbDrawBehindSeries.Text = "&Draw Behind Series";
            this.cbDrawBehindSeries.Click += new EventHandler(this.cbDrawBehindSeries_Click);
            this.udTransp.Location = new Point(0x52, 0x7e);
            this.udTransp.Name = "udTransp";
            this.udTransp.Size = new Size(60, 20);
            this.udTransp.TabIndex = 7;
            this.udTransp.ValueChanged += new EventHandler(this.udTransp_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(1, 0x80);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4b, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "&Transparency:";
            base.ClientSize = new Size(0xe9, 0xac);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.udTransp);
            base.Controls.Add(this.cbDrawBehindSeries);
            base.Controls.Add(this.bBrush);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.cbSeries2);
            base.Controls.Add(this.bPen);
            base.Name = "SeriesBandToolEditor";
            base.Controls.SetChildIndex(this.bPen, 0);
            base.Controls.SetChildIndex(this.cbSeries2, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            base.Controls.SetChildIndex(this.bBrush, 0);
            base.Controls.SetChildIndex(this.cbDrawBehindSeries, 0);
            base.Controls.SetChildIndex(this.udTransp, 0);
            base.Controls.SetChildIndex(this.label3, 0);
            this.udTransp.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void udTransp_ValueChanged(object sender, EventArgs e)
        {
            if ((this.tool != null) && !base.setting)
            {
                this.tool.Transparency = (int) this.udTransp.Value;
            }
        }
    }
}

