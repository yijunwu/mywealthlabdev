namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class WSDPolygon : WSDrawingObject
    {
        private Color color_0;
        private Color color_1;
        private double[] double_0;
        private int int_0;
        private LineStyle lineStyle_0;

        public WSDPolygon(Color color, Color fillColor, LineStyle style, int width, double[] coords)
        {
            this.color_0 = color;
            this.color_1 = fillColor;
            this.lineStyle_0 = style;
            this.int_0 = width;
            this.double_0 = coords;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            Point[] points = new Point[this.double_0.Length / 2];
            int index = 0;
            for (int i = 0; i < this.double_0.Length; i += 2)
            {
                int num3 = (int) this.double_0[i];
                double num4 = this.double_0[i + 1];
                int x = chartRenderer_0.ConvertBarToX(num3);
                int y = pane.ConvertValueToY(num4);
                points[index] = new Point(x, y);
                index++;
            }
            Brush brush = new SolidBrush(this.color_1);
            using (brush)
            {
                graphics_0.FillPolygon(brush, points);
            }
            Pen pen = new Pen(this.color_0, (float) this.int_0);
            ChartRenderer.SetPenStyle(pen, this.lineStyle_0);
            using (pen)
            {
                graphics_0.DrawPolygon(pen, points);
            }
        }
    }
}

