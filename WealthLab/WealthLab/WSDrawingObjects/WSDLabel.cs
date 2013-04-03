namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class WSDLabel : WSDrawingObject
    {
        private Color color_0;
        private string string_0;

        public WSDLabel(string text, Color color)
        {
            this.color_0 = color;
            this.string_0 = text;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            pane.method_7(graphics_0, this.string_0, this.color_0);
        }
    }
}

