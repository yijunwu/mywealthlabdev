namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class RenkoEditor : BaseSeriesForm
    {
        private Button buttonBrush;
        private ButtonColor buttonDownswingColor;
        private ButtonColor buttonUpSwingColor;
        private Container components;
        private Label label1;
        private Renko series;
        private TextBox textBoxBSize;

        public RenkoEditor()
        {
            this.InitializeComponent();
        }

        public RenkoEditor(Series s) : this()
        {
            this.series = (Renko) s;
        }

        private void buttonBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
        }

        private void buttonDownswingColor_Click(object sender, EventArgs e)
        {
            this.series.DownSwingColor = this.buttonDownswingColor.Color;
        }

        private void buttonUpSwingColor_Click(object sender, EventArgs e)
        {
            this.series.UpSwingColor = this.buttonUpSwingColor.Color;
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
            this.buttonUpSwingColor = new ButtonColor();
            this.buttonDownswingColor = new ButtonColor();
            this.label1 = new Label();
            this.textBoxBSize = new TextBox();
            this.buttonBrush = new Button();
            base.SuspendLayout();
            this.buttonUpSwingColor.Color = Color.Empty;
            this.buttonUpSwingColor.Location = new Point(12, 12);
            this.buttonUpSwingColor.Name = "buttonUpSwingColor";
            this.buttonUpSwingColor.Size = new Size(110, 0x17);
            this.buttonUpSwingColor.TabIndex = 0;
            this.buttonUpSwingColor.Text = "Upswing";
            this.buttonUpSwingColor.Click += new EventHandler(this.buttonUpSwingColor_Click);
            this.buttonDownswingColor.Color = Color.Empty;
            this.buttonDownswingColor.Location = new Point(12, 0x29);
            this.buttonDownswingColor.Name = "buttonDownswingColor";
            this.buttonDownswingColor.Size = new Size(110, 0x17);
            this.buttonDownswingColor.TabIndex = 1;
            this.buttonDownswingColor.Text = "Downswing";
            this.buttonDownswingColor.Click += new EventHandler(this.buttonDownswingColor_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0x54);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x31, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Box size:";
            this.textBoxBSize.Location = new Point(0x4b, 0x51);
            this.textBoxBSize.Name = "textBoxBSize";
            this.textBoxBSize.Size = new Size(0x37, 20);
            this.textBoxBSize.TabIndex = 3;
            this.textBoxBSize.TextChanged += new EventHandler(this.textBoxBSize_TextChanged);
            this.buttonBrush.FlatStyle = FlatStyle.Flat;
            this.buttonBrush.Location = new Point(0x8a, 12);
            this.buttonBrush.Name = "buttonBrush";
            this.buttonBrush.Size = new Size(0x4b, 0x17);
            this.buttonBrush.TabIndex = 4;
            this.buttonBrush.Text = "&Pattern";
            this.buttonBrush.Click += new EventHandler(this.buttonBrush_Click);
            base.ClientSize = new Size(0xf1, 0x83);
            base.Controls.Add(this.buttonBrush);
            base.Controls.Add(this.textBoxBSize);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.buttonDownswingColor);
            base.Controls.Add(this.buttonUpSwingColor);
            base.Name = "RenkoEditor";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.series != null)
            {
                this.buttonDownswingColor.Color = this.series.DownSwingColor;
                this.buttonUpSwingColor.Color = this.series.UpSwingColor;
                this.textBoxBSize.Text = this.series.BoxSize.ToString();
            }
        }

        private void textBoxBSize_TextChanged(object sender, EventArgs e)
        {
            this.series.BoxSize = Utils.StringToDouble(this.textBoxBSize.Text, 0.0);
        }
    }
}

