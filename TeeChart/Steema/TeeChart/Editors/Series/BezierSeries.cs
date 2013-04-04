namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BezierSeries : BaseSeriesForm
    {
        private ButtonPen button1;
        private Container components;
        private Steema.TeeChart.Editors.SeriesPointer pointEditor;
        private Bezier series;

        public BezierSeries()
        {
            this.InitializeComponent();
        }

        public BezierSeries(Series s) : this()
        {
            this.series = (Bezier) s;
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
            this.button1 = new ButtonPen();
            base.SuspendLayout();
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(8, 0x10);
            this.button1.Name = "button1";
            this.button1.TabIndex = 0;
            this.button1.Text = "&Border...";
            base.ClientSize = new Size(0xd8, 0x6d);
            base.Controls.Add(this.button1);
            base.Name = "BezierSeries";
            base.ResumeLayout(false);
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.series != null)
            {
                if (this.pointEditor == null)
                {
                    this.pointEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(Parent, this.series.Pointer);
                }
                this.button1.Pen = this.series.LinePen;
            }
        }
    }
}

