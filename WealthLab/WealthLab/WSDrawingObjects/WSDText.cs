namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class WSDText : WSDrawingObject
    {
        private Color color_0;
        private Color color_1;
        private Font font_0;
        private int int_0;
        private int int_1;
        private string string_0;

        public WSDText(string text, int int_2, int int_3, Color color, Color backgroundColor, Font font)
        {
            this.string_0 = text;
            this.int_0 = int_2;
            this.int_1 = int_3;
            this.color_0 = color;
            this.color_1 = backgroundColor;
            this.font_0 = font;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            Font font = (this.font_0 == null) ? chartRenderer_0.AxisFont : this.font_0;
            if (this.color_1 != Color.Empty)
            {
                SizeF ef = graphics_0.MeasureString(this.string_0, font);
                Rectangle rect = new Rectangle(this.int_0, this.int_1, (int) ef.Width, (int) ef.Height);
                Brush brush3 = new SolidBrush(this.color_1);
                using (brush3)
                {
                    graphics_0.FillRectangle(brush3, rect);
                }
            }
            Brush brush = new SolidBrush(this.color_0);
            using (brush)
            {
                graphics_0.DrawString(this.string_0, font, brush, (float) this.int_0, (float) this.int_1);
            }
        }
    }
}

