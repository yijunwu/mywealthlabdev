namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class WSDChartAnnotation : WSDrawingObject
    {
        private Color color_0;
        private Color color_1;
        private double double_0;
        private Font font_0;
        private HorizontalAlignment horizontalAlignment_0 = HorizontalAlignment.Center;
        private int int_0;
        private string string_0;

        public WSDChartAnnotation(string text, int int_1, double value, Color color, Color backgroundColor, Font font, HorizontalAlignment alignment)
        {
            this.string_0 = text;
            this.int_0 = int_1;
            this.double_0 = value;
            this.color_0 = color;
            this.color_1 = backgroundColor;
            this.font_0 = font;
            this.horizontalAlignment_0 = alignment;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            int num;
            SizeF ef = graphics_0.MeasureString(this.string_0, chartRenderer_0.AxisFont);
            switch (this.horizontalAlignment_0)
            {
                case HorizontalAlignment.Left:
                    num = chartRenderer_0.ConvertBarToX(this.int_0);
                    break;

                case HorizontalAlignment.Right:
                    num = chartRenderer_0.ConvertBarToX(this.int_0) - ((int) ef.Width);
                    break;

                default:
                    num = chartRenderer_0.ConvertBarToX(this.int_0) - ((int) (ef.Width / 2f));
                    break;
            }
            int y = pane.ConvertValueToY(this.double_0);
            Rectangle rect = new Rectangle(num, y, (int) ef.Width, (int) ef.Height);
            Brush brush3 = new SolidBrush(this.color_1);
            using (brush3)
            {
                graphics_0.FillRectangle(brush3, rect);
            }
            Brush brush = new SolidBrush(this.color_0);
            Font font = (this.font_0 == null) ? chartRenderer_0.AxisFont : this.font_0;
            using (brush)
            {
                graphics_0.DrawString(this.string_0, font, brush, (float) num, (float) y);
            }
        }
    }
}

