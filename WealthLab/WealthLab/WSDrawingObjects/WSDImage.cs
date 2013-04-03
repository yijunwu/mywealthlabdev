namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class WSDImage : WSDrawingObject
    {
        private double double_0;
        private Image image_0;
        private int int_0;

        public WSDImage(Image image, int int_1, double value)
        {
            this.image_0 = image;
            this.int_0 = int_1;
            this.double_0 = value;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            int x = chartRenderer_0.ConvertBarToX(this.int_0) - (this.image_0.Width / 2);
            int y = pane.ConvertValueToY(this.double_0) - (this.image_0.Height / 2);
            graphics_0.DrawImage(this.image_0, x, y);
        }
    }
}

