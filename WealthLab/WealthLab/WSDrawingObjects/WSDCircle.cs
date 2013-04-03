namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class WSDCircle : WSDrawingObject
    {
        private Color color_0;
        private Color color_1;
        private double double_0;
        private int int_0;
        private int int_1;
        private int int_2;
        private LineStyle lineStyle_0;

        public WSDCircle(int radius, int int_3, double value, Color color, Color fillColor, LineStyle style, int width)
        {
            this.int_0 = radius;
            this.int_1 = int_3;
            this.double_0 = value;
            this.int_2 = width;
            this.lineStyle_0 = style;
            this.color_0 = color;
            this.color_1 = fillColor;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            if ((this.int_1 >= 0) && (this.int_1 < chartRenderer_0.Bars.Count))
            {
                int x = chartRenderer_0.ConvertBarToX(this.int_1) - (this.int_0 / 2);
                int y = pane.ConvertValueToY(this.double_0) - (this.int_0 / 2);
                Brush brush = new SolidBrush(this.color_1);
                using (brush)
                {
                    graphics_0.FillEllipse(brush, x, y, this.int_0, this.int_0);
                }
                Pen pen = new Pen(this.color_0, (float) this.int_2);
                using (pen)
                {
                    ChartRenderer.SetPenStyle(pen, this.lineStyle_0);
                    graphics_0.DrawEllipse(pen, x, y, this.int_0, this.int_0);
                }
            }
        }
    }
}

