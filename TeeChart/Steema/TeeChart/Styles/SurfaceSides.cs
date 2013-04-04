namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;

    public class SurfaceSides
    {
        private bool levels = true;
        private ChartPen pen;
        private Surface series;

        public SurfaceSides(Surface Series)
        {
            this.series = Series;
            this.pen = new ChartPen(this.series.Chart, false);
        }

        private void CanvasChanged()
        {
            this.series.Repaint();
        }

        public ChartBrush Brush
        {
            get
            {
                return this.series.SideBrush;
            }
            set
            {
                this.series.SideBrush = value;
            }
        }

        [DefaultValue(true)]
        public bool Levels
        {
            get
            {
                return this.levels;
            }
            set
            {
                if (this.levels != value)
                {
                    this.levels = value;
                    this.series.Repaint();
                }
            }
        }

        public ChartPen Pen
        {
            get
            {
                return this.pen;
            }
            set
            {
                this.pen = value;
            }
        }
    }
}

