namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;

    public class PolygonSeries : Series
    {
        protected internal Steema.TeeChart.Styles.Polygon iPolygon;
        private ChartPen pen;

        public PolygonSeries() : this(null)
        {
        }

        public PolygonSeries(Chart c) : base(c)
        {
        }

        protected override void AddSampleValues(int numValues)
        {
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            base.DrawLegendShape(g, valueIndex, rect);
        }

        public Steema.TeeChart.Styles.Polygon Polygon()
        {
            return this.iPolygon;
        }

        protected override void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref Color backColor, ref ChartBrush aBrush)
        {
            base.PrepareLegendCanvas(g, valueIndex, ref backColor, ref aBrush);
            this.Polygon().ParentSeries.DoBeforeDrawChart();
        }

        protected override void SetActive(bool value)
        {
            base.SetActive(value);
            this.Polygon().ParentSeries.Repaint();
        }

        protected override void SetSeriesColor(Color value)
        {
            base.SetSeriesColor(value);
        }

        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(Color.Black);
                }
                return this.pen;
            }
            set
            {
                this.pen = value;
            }
        }
    }
}

