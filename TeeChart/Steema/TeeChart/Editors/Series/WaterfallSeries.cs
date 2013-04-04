namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class WaterfallSeries : SurfaceSeries
    {
        private Button bLines;
        private Container components;
        private Waterfall waterfall;

        public WaterfallSeries()
        {
            this.InitializeComponent();
        }

        public WaterfallSeries(Series s) : this()
        {
            this.waterfall = (Waterfall) s;
            base.series = this.waterfall;
        }

        private void bLines_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(base.series.WaterLines);
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
            this.bLines = new Button();
            base.SuspendLayout();
            this.bLines.FlatStyle = FlatStyle.Flat;
            this.bLines.Location = new Point(0xb3, 0x7c);
            this.bLines.Name = "bLines";
            this.bLines.Size = new Size(0x4b, 0x17);
            this.bLines.TabIndex = 0;
            this.bLines.Text = "&Lines...";
            this.bLines.Click += new EventHandler(this.bLines_Click);
            base.ClientSize = new Size(0x177, 0x9a);
            base.Controls.Add(this.bLines);
            base.Name = "WaterfallSeries";
            base.Controls.SetChildIndex(this.bLines, 0);
            base.Controls.SetChildIndex(base.CBSmooth, 0);
            base.Controls.SetChildIndex(base.cbHideCells, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

