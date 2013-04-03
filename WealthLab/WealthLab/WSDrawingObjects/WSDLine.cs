namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class WSDLine : WSDrawingObject
    {
        private Color color_0;
        private double double_0;
        private double double_1;
        private int int_0;
        private int int_1;
        private int int_2;
        private LineStyle lineStyle_0;

        public WSDLine(int bar1, double value1, int bar2, double value2, Color color, LineStyle style, int width)
        {
            this.int_0 = bar1;
            this.double_0 = value1;
            this.int_1 = bar2;
            this.double_1 = value2;
            this.color_0 = color;
            this.lineStyle_0 = style;
            this.int_2 = width;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            int num = chartRenderer_0.ConvertBarToX(this.int_0);
            int num2 = pane.ConvertValueToY(this.double_0);
            int num3 = chartRenderer_0.ConvertBarToX(this.int_1);
            int num4 = pane.ConvertValueToY(this.double_1);
            Pen pen = new Pen(this.color_0, (float) this.int_2);
            using (pen)
            {
                ChartRenderer.SetPenStyle(pen, this.lineStyle_0);
                graphics_0.DrawLine(pen, num, num2, num3, num4);
            }
        }
    }
}

