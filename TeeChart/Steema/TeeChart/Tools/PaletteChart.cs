namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [DesignTimeVisible(false)]
    internal class PaletteChart : TChart
    {
        public PaletteChart()
        {
            base.Panel.MarginLeft = 8.0;
            base.Panel.MarginRight = 8.0;
            base.Panel.MarginBottom = 8.0;
            base.Panel.MarginTop = 8.0;
            base.Panel.Bevel.Outer = BevelStyles.None;
            base.Panel.Pen.Visible = true;
            base.Panel.Color = Color.White;
            base.Panel.Transparent = false;
            base.Legend.Visible = false;
            base.Header.Visible = false;
            base.Zoom.Allow = false;
            base.Panning.Allow = ScrollModes.None;
            base.Aspect.View3D = false;
            base.Aspect.ClipPoints = false;
            base.Graphics3D.UseBuffer = false;
            base.Walls.Visible = false;
            this.PrepareAxis(base.Axes.Left);
            this.PrepareAxis(base.Axes.Right);
            this.PrepareAxis(base.Axes.Top);
            this.PrepareAxis(base.Axes.Bottom);
        }

        private void PrepareAxis(Axis axis)
        {
            axis.AxisPen.Width = 1;
            axis.AxisPen.EndCap = LineCap.Square;
            axis.Grid.Visible = false;
        }
    }
}

