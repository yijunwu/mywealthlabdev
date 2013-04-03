namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab;

    public abstract class WSDrawingObject
    {
        protected WSDrawingObject()
        {
        }

        internal abstract void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0);
    }
}

