namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class IsoSurfaceEditor : SurfaceSeries
    {
        private ButtonPen bpBandsPen;
        private IContainer components;
        private IsoSurface isoSeries;

        public IsoSurfaceEditor()
        {
            this.InitializeComponent();
        }

        public IsoSurfaceEditor(Series s) : this()
        {
            this.isoSeries = (IsoSurface) s;
            base.series = this.isoSeries;
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
            this.bpBandsPen = new ButtonPen();
            base.SuspendLayout();
            this.bpBandsPen.FlatStyle = FlatStyle.Flat;
            this.bpBandsPen.Location = new Point(0xb3, 130);
            this.bpBandsPen.Name = "bpBandsPen";
            this.bpBandsPen.Size = new Size(120, 0x17);
            this.bpBandsPen.TabIndex = 9;
            this.bpBandsPen.Text = "&Bands Pen...";
            this.bpBandsPen.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x16b, 0x9b);
            base.Controls.Add(this.bpBandsPen);
            base.Name = "IsoSurfaceEditor";
            this.Text = "IsoSurfaceEditor";
            base.Controls.SetChildIndex(this.bpBandsPen, 0);
            base.Controls.SetChildIndex(base.CBSmooth, 0);
            base.Controls.SetChildIndex(base.cbHideCells, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.isoSeries != null)
            {
                base.CBSmooth.Visible = false;
                base.cbHideCells.Visible = false;
                this.bpBandsPen.Pen = this.isoSeries.BandPen;
                base.SetParent(Parent);
            }
        }
    }
}

