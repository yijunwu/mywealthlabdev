namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class WSDHorzLine : WSDrawingObject
    {
        private Color color_0;
        private double double_0;
        private int int_0;
        private LineStyle lineStyle_0;

        public WSDHorzLine(double value, Color color, LineStyle style, int width)
        {
            this.double_0 = value;
            this.color_0 = color;
            this.lineStyle_0 = style;
            this.int_0 = width;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            Pen pen = new Pen(this.color_0, (float) this.int_0);
            using (pen)
            {
                ChartRenderer.SetPenStyle(pen, this.lineStyle_0);
                int num = pane.ConvertValueToY(this.double_0);
                graphics_0.DrawLine(pen, 0, num, chartRenderer_0.Width - chartRenderer_0.MarginRightWidth, num);
                chartRenderer_0.ClipToPane(graphics_0, pane, true);
                pane.drawLastBarValue(graphics_0, this.double_0, this.color_0);
                chartRenderer_0.ClipToPane(graphics_0, pane);
            }
        }
    }
}

