namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class WSDBarAnnotation : WSDrawingObject
    {
        private bool bool_0;
        private Color color_0;
        private Color color_1;
        private Font font_0;
        private int int_0;
        private string string_0;

        public WSDBarAnnotation(string text, int int_1, bool aboveBar, Color color, Color backgroundColor, Font font)
        {
            this.string_0 = text;
            this.int_0 = int_1;
            this.bool_0 = aboveBar;
            this.color_0 = color;
            this.color_1 = backgroundColor;
            this.font_0 = font;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            int num3;
            SizeF ef = graphics_0.MeasureString(this.string_0, chartRenderer_0.AxisFont);
            int x = chartRenderer_0.ConvertBarToX(this.int_0) - ((int) (ef.Width / 2f));
            if (this.bool_0)
            {
                int barAnnotationCount = chartRenderer_0.GetBarAnnotationCount(this.int_0, true);
                num3 = (int) ((pane.ConvertValueToY(chartRenderer_0.Bars.High[this.int_0]) - (ef.Height * (barAnnotationCount + 1))) - 2f);
                barAnnotationCount++;
                chartRenderer_0.SetBarAnnotationCount(this.int_0, true, barAnnotationCount);
            }
            else
            {
                int num4 = chartRenderer_0.GetBarAnnotationCount(this.int_0, false);
                num3 = (pane.ConvertValueToY(chartRenderer_0.Bars.Low[this.int_0]) + 2) + ((int) (ef.Height * num4));
                num4++;
                chartRenderer_0.SetBarAnnotationCount(this.int_0, false, num4);
            }
            Rectangle rect = new Rectangle(x, num3, (int) ef.Width, (int) ef.Height);
            Brush brush3 = new SolidBrush(this.color_1);
            using (brush3)
            {
                graphics_0.FillRectangle(brush3, rect);
            }
            Font font = (this.font_0 == null) ? chartRenderer_0.AxisFont : this.font_0;
            Brush brush = new SolidBrush(this.color_0);
            using (brush)
            {
                graphics_0.DrawString(this.string_0, font, brush, (float) x, (float) num3);
            }
        }
    }
}

