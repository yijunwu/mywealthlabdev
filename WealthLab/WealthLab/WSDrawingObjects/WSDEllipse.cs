namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class WSDEllipse : WSDrawingObject
    {
        private Color color_0;
        private Color color_1;
        private double double_0;
        private double double_1;
        private int int_0;
        private int int_1;
        private int int_2;
        private LineStyle lineStyle_0;

        public WSDEllipse(int bar1, double value1, int bar2, double value2, Color color, Color fillColor, LineStyle style, int width)
        {
            this.int_1 = bar1;
            this.double_0 = value1;
            this.int_2 = bar2;
            this.double_1 = value2;
            this.color_0 = color;
            this.color_1 = fillColor;
            this.lineStyle_0 = style;
            this.int_0 = width;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            int x = chartRenderer_0.ConvertBarToX(this.int_1);
            int y = pane.ConvertValueToY(this.double_0);
            int num3 = chartRenderer_0.ConvertBarToX(this.int_2);
            int num4 = pane.ConvertValueToY(this.double_1);
            if (this.color_1 != Color.Empty)
            {
                Brush brush = new SolidBrush(this.color_1);
                using (brush)
                {
                    graphics_0.FillEllipse(brush, x, y, num3 - x, num4 - y);
                }
            }
            Pen pen = new Pen(this.color_0, (float) this.int_0);
            using (pen)
            {
                ChartRenderer.SetPenStyle(pen, this.lineStyle_0);
                graphics_0.DrawEllipse(pen, x, y, num3 - x, num4 - y);
            }
        }
    }
}

